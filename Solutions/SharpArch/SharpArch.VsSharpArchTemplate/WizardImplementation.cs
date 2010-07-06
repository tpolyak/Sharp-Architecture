using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TemplateWizard;
using VSLangProj;
using EnvDTE;
using EnvDTE80;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Xml;

namespace SharpArchApplicationWizard
{
    // Class that implements the IWizard() interface
    internal class WizardImplementation : IWizard
    {
        /// <summary>
        /// Provide a means for sub-projects to have access to the solution name
        /// </summary>
        private static string solutionName;
        private static string guidAssignedToCore = "{00000000-0000-0000-0000-000000000000}";
        private static string guidAssignedToData = "{00000000-0000-0000-0000-000000000000}";
        private static string guidAssignedToApplicationServices = "{00000000-0000-0000-0000-000000000000}";
        private static string guidAssignedToControllers = "{00000000-0000-0000-0000-000000000000}";

        private const int MIN_TIME_FOR_PROJECT_TO_RELEASE_FILE_LOCK = 700;
        private EnvDTE._DTE dte;
        private WizardRunKind runKind;
        private Dictionary<string, string> replacementsDictionary;

        // RunStarted() method gets called before the template wizard creates the project.
        void IWizard.RunStarted(object application, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams) {
            this.dte = application as EnvDTE._DTE;
            this.runKind = runKind;
            this.replacementsDictionary = replacementsDictionary;

            // Store the solution name locally while processing the solution template
            if (runKind == WizardRunKind.AsMultiProject) {
                solutionName = replacementsDictionary["$safeprojectname$"];
            }

            replacementsDictionary.Add("$solutionname$", solutionName);

            if (runKind == WizardRunKind.AsNewProject) {
                // Make the solution root path available for all the projects
                replacementsDictionary.Add("$solutionrootpath$", GetSolutionRootPath() + solutionName + "\\");

                AddProjectGuidsTo(replacementsDictionary);
            }
        }

        /// <summary>
        /// Makes the project GUIDs, which are collected during the project creation process, 
        /// available to subsequent projects
        /// </summary>
        private static void AddProjectGuidsTo(Dictionary<string, string> replacementsDictionary) {
            replacementsDictionary.Add("$guidAssignedToCore$", guidAssignedToCore);
            replacementsDictionary.Add("$guidAssignedToData$", guidAssignedToData);
            replacementsDictionary.Add("$guidAssignedToApplicationServices$", guidAssignedToApplicationServices);
            replacementsDictionary.Add("$guidAssignedToControllers$", guidAssignedToControllers);
        }

        /// <summary>
        /// Runs custom wizard logic when a project has finished generating
        /// </summary>
        void IWizard.ProjectFinishedGenerating(EnvDTE.Project project) {
            if (project != null) {
                if (project.Name == "SolutionItemsContainer") {
                    PerformSolutionInitialization(project);
                    MoveSolutionItemsToLib(project);
                }
                else if (project.Name == "ToolsSolutionItemsContainer") {
                    MoveSolutionItemsToToolsLib(project);
                }
                else if (project.Name == "CrudScaffolding") {
                    Project movedProject = MoveProjectTo("\\tools\\", project, "Code Generation");
                    ExcludeProjectFromBuildProcess(movedProject);
                }
                else if (project.Name == "CrudScaffoldingForEnterpriseApp") {
                    Project movedProject = MoveProjectTo("\\tools\\", project, "Code Generation");
                    ExcludeProjectFromBuildProcess(movedProject);
                }
                else if (project.Name == GetSolutionName() + ".Tests") {
                    MoveProjectTo("\\tests\\", project);
                }
                else if (project.Name == GetSolutionName() + ".Web.Controllers" ||
                    project.Name == GetSolutionName() + ".ApplicationServices" ||
                    project.Name == GetSolutionName() + ".Core" ||
                    project.Name == GetSolutionName() + ".Data" ||
                    project.Name == GetSolutionName() + ".Web") {
                    Project movedProject = MoveProjectTo("\\app\\", project);

                    // Give the solution time to release the lock on the project file
                    System.Threading.Thread.Sleep(MIN_TIME_FOR_PROJECT_TO_RELEASE_FILE_LOCK);
                    
                    CaptureProjectGuidOf(movedProject);
                }
            }
        }

        private void CaptureProjectGuidOf(Project project) {
            if (IsProjectReferredByOtherProjects(project)) {
                string projectPath = GetSolutionRootPath() + GetSolutionName() + "\\app\\" + project.Name + "\\" + project.Name + ".csproj";

                Log("CaptureProjectGuidOf: Does " + projectPath + " exist? " + File.Exists(projectPath).ToString());
                Log("CaptureProjectGuidOf: About to open " + projectPath);

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(projectPath);
                XmlNodeList projectGuidNodes = xmlDocument.GetElementsByTagName("ProjectGuid");

                if (projectGuidNodes == null || projectGuidNodes.Count == 0)
                    throw new ApplicationException("Couldn't find a matching node in the project file for ProjectGuid");

                StoreCapturedGuidForLaterUse(project, projectGuidNodes);

                Log("CaptureProjectGuidOf: Captured the GUID " + projectGuidNodes[0].InnerText + " for " + project.Name);
            }
        }

        private void StoreCapturedGuidForLaterUse(Project project, XmlNodeList projectGuidNodes) {
            if (project.Name == GetSolutionName() + ".ApplicationServices") {
                guidAssignedToApplicationServices = projectGuidNodes[0].InnerText;
            }
            else if (project.Name == GetSolutionName() + ".Core") {
                guidAssignedToCore = projectGuidNodes[0].InnerText;
            }
            else if (project.Name == GetSolutionName() + ".Web.Controllers") {
                guidAssignedToControllers = projectGuidNodes[0].InnerText;
            }
            else if (project.Name == GetSolutionName() + ".Data") {
                guidAssignedToData = projectGuidNodes[0].InnerText;
            }
        }

        private bool IsProjectReferredByOtherProjects(Project project) {
            return project.Name == GetSolutionName() + ".ApplicationServices" ||
                project.Name == GetSolutionName() + ".Core" ||
                project.Name == GetSolutionName() + ".Web.Controllers" ||
                project.Name == GetSolutionName() + ".Data";
        }

        /// <summary>
        /// Sets up the solution structure and performs a number of related initialization steps
        /// </summary>
        private void PerformSolutionInitialization(EnvDTE.Project project) {
            CreateSolutionDirectoryStructure();
            MoveCommonAssemblyInfoToRoot(project);
        }

        /// <summary>
        /// Runs custom wizard logic when the wizard has completed all tasks
        /// </summary>
        void IWizard.RunFinished() {
            // Only copy the solution items once, right after processing the solution template
            if (runKind == WizardRunKind.AsMultiProject) {
                DeleteSuoFile();

                // Operations after this must take into account that the solution path has changed
                MoveSolutionFileToProjectsDirectory();
            }
        }

        private void ExcludeProjectFromBuildProcess(EnvDTE.Project project) {
            Solution2 solution = dte.Solution as Solution2;
            SolutionBuild2 solutionBuild = (SolutionBuild2)solution.SolutionBuild;

            foreach (SolutionConfiguration solutionConfiguration in solutionBuild.SolutionConfigurations) {
                foreach (SolutionContext solutionContext in solutionConfiguration.SolutionContexts) {
                    if (solutionContext.ProjectName.IndexOf(project.Name) > -1) {
                        Log("ExcludeProjectFromBuildProcess: Setting build to false for project " + solutionContext.ProjectName +
                            " within the " + solutionConfiguration.Name + " configuration");
                        solutionContext.ShouldBuild = false;
                        solutionContext.ShouldDeploy = false;
                    }
                }
            }
        }

        private Project MoveProjectTo(string targetSubFolder, EnvDTE.Project project) {
            return MoveProjectTo(targetSubFolder, project, null);
        }

        private Project MoveProjectTo(string targetSubFolder, EnvDTE.Project project, string solutionFolderName) {
            string projectName = project.Name;
            string originalLocation = GetSolutionRootPath() + GetSolutionName() + "\\" + projectName;

            if (Directory.Exists(originalLocation)) {
                Solution2 solution = dte.Solution as Solution2;

                Log("MoveProjectTo: Removing " + projectName + " from solution");
                solution.Remove(project);

                // Give the solution time to release the lock on the project file
                System.Threading.Thread.Sleep(MIN_TIME_FOR_PROJECT_TO_RELEASE_FILE_LOCK);

                PerformManualProjectReplacementsTo(originalLocation + "\\" + projectName + ".csproj");

                string targetLocation = GetSolutionRootPath() + GetSolutionName() + targetSubFolder + projectName;

                Log("MoveProjectTo: Moving " + projectName + " from " + originalLocation + " to target location at " + targetLocation);
                Directory.Move(originalLocation, targetLocation);

                if (!string.IsNullOrEmpty(solutionFolderName)) {
                    SolutionFolder solutionFolder;
                    Project solutionFolderAsProject = FindProjectByName(solution, solutionFolderName);

                    if (solutionFolderAsProject != null) {
                        solutionFolder = solutionFolderAsProject.Object as SolutionFolder;
                    }
                    else {
                        solutionFolder = (SolutionFolder) solution.AddSolutionFolder(solutionFolderName).Object;
                    }

                    Log("MoveProjectTo: Adding " + projectName + " to solution folder " + targetLocation);
                    return solutionFolder.AddFromFile(targetLocation + "\\" + projectName + ".csproj");
                }
                else {
                    Log("MoveProjectTo: Adding " + projectName + " to solution");
                    return solution.AddFromFile(targetLocation + "\\" + projectName + ".csproj", false);
                }
            }
            else {
                throw new ApplicationException("Couldn't find " + originalLocation + " to move");
            }
        }

        public static Project FindProjectByName(Solution2 solution, string name) {
            foreach (Project project in solution.Projects) {
                if (project.Name == name)
                    return project;
            }

            return null;
        } 

        /// <summary>
        /// This does any manual value replacement on project files when it can't be handled 
        /// (or is being handled incorrectly by the VS templating process.
        /// </summary>
        private void PerformManualProjectReplacementsTo(string projectFilePath) {
            if (File.Exists(projectFilePath)) {
                Log("PerformManualProjectReplacementsTo: Going to PerformManualProjectReplacementsTo on " + projectFilePath);

                // Open a file for reading
                StreamReader streamReader;
                streamReader = File.OpenText(projectFilePath);

                // Now, read the entire file into a string
                string contents = streamReader.ReadToEnd();
                streamReader.Close();

                // Write the modification into the same fil
                StreamWriter streamWriter = File.CreateText(projectFilePath);
                streamWriter.Write(contents.Replace("PLACE_HOLDER_COMMON_ASSEMLY_INFO_LOCATION", "..\\..\\CommonAssemblyInfo.cs"));
                streamWriter.Close();
            }
            else {
                throw new ApplicationException("Couldn't find " + projectFilePath + " to PerformManualProjectReplacementsTo");
            }
        }

        private void MoveCommonAssemblyInfoToRoot(EnvDTE.Project solutionItemsContainerProject) {
            string originalFileLocation = GetSolutionRootPath() + GetSolutionName() + "\\SolutionItemsContainer\\CommonAssemblyInfo.cs";

            if (File.Exists(originalFileLocation)) {
                string targetFileLocation = GetSolutionRootPath() + GetSolutionName() + "\\CommonAssemblyInfo.cs";

                Log("MoveCommonAssemblyInfoToRoot: Moving CommonAssemblyInfo.cs from " + originalFileLocation + " to root at " + targetFileLocation);
                File.Move(originalFileLocation, targetFileLocation);
            }
            else {
                throw new ApplicationException("Couldn't find CommonAssemblyInfo.cs to move");
            }
        }

        private void MoveSolutionItemsToLib(EnvDTE.Project solutionItemsContainerProject) {
            string originalLocation = GetSolutionRootPath() + GetSolutionName() + "\\SolutionItemsContainer\\Solution Items";

            if (Directory.Exists(originalLocation)) {
                string targetLibFolder = GetSolutionRootPath() + GetSolutionName() + "\\lib";

                Log("MoveSolutionItemsToLib: Moving solution items from " + originalLocation + " to lib at " + targetLibFolder);
                Directory.Move(originalLocation, targetLibFolder);

                Solution2 solution = dte.Solution as Solution2;
                solution.Remove(solutionItemsContainerProject);
                // Give the solution time to release the lock on the project file
                System.Threading.Thread.Sleep(500);

                Directory.Delete(GetSolutionRootPath() + GetSolutionName() + "\\SolutionItemsContainer", true);
            }
            else {
                throw new ApplicationException("Couldn't find " + originalLocation + " to move");
            }
        }

        private void MoveSolutionItemsToToolsLib(EnvDTE.Project toolsSolutionItemsContainerProject) {
            string originalLocation = GetSolutionRootPath() + GetSolutionName() + "\\ToolsSolutionItemsContainer\\Solution Items";

            if (Directory.Exists(originalLocation)) {
                string targetToolsLibFolder = GetSolutionRootPath() + GetSolutionName() + "\\tools\\lib";

                Log("MoveSolutionItemsToToolsLib: Moving tools solution items from " + originalLocation + " to tools lib at " + targetToolsLibFolder);
                Directory.Move(originalLocation, targetToolsLibFolder);

                Solution2 solution = dte.Solution as Solution2;
                solution.Remove(toolsSolutionItemsContainerProject);
                // Give the solution time to release the lock on the project file
                System.Threading.Thread.Sleep(500);

                Directory.Delete(GetSolutionRootPath() + GetSolutionName() + "\\ToolsSolutionItemsContainer", true);
            }
            else {
                throw new ApplicationException("Couldn't find " + originalLocation + " to move");
            }
        }

        /// <summary>
        /// Note that this is called BEFORE the SLN is moved to the solution folder; therefore, we have 
        /// to add the solution name after the root path.
        /// </summary>
        private void CreateSolutionDirectoryStructure() {
            Directory.CreateDirectory(GetSolutionRootPath() + GetSolutionName() + "\\app");
            Directory.CreateDirectory(GetSolutionRootPath() + GetSolutionName() + "\\build");
            Directory.CreateDirectory(GetSolutionRootPath() + GetSolutionName() + "\\db");
            Directory.CreateDirectory(GetSolutionRootPath() + GetSolutionName() + "\\db\\schema");
            Directory.CreateDirectory(GetSolutionRootPath() + GetSolutionName() + "\\docs");
            Directory.CreateDirectory(GetSolutionRootPath() + GetSolutionName() + "\\logs");
            Directory.CreateDirectory(GetSolutionRootPath() + GetSolutionName() + "\\tests");
            Directory.CreateDirectory(GetSolutionRootPath() + GetSolutionName() + "\\tools");
        }

        private void MoveSolutionFileToProjectsDirectory() {
            dte.Solution.SaveAs(
                GetSolutionRootPath() + GetSolutionName() + "\\" + GetSolutionFileName());
        }

        private void DeleteSuoFile() {
            string suoFile = GetSolutionRootPath() + GetSolutionName() + ".suo";

            if (File.Exists(suoFile)) {
                Log("DeleteSuoFile: Deleting " + suoFile);
                File.Delete(suoFile);
            }
        }

        private void Log(string message) {
            StreamWriter streamWriter = File.AppendText(GetSolutionRootPath() + GetSolutionName() + "\\logs\\" + LOG_FILE_NAME);
            streamWriter.WriteLine(DateTime.Now.ToLongTimeString() + "\t" + message);
            streamWriter.Close();
        }

        private string GetSolutionName() {
            return replacementsDictionary["$solutionname$"];
        }

        private string GetSolutionFileName() {
            return GetSolutionName() + ".sln";
        }

        private string GetSolutionFileFullName() {
            return dte.Solution.Properties.Item("Path").Value.ToString();
        }

        private string GetSolutionRootPath() {
            return GetSolutionFileFullName().Replace(GetSolutionFileName(), "");
        }

        // This method is called before opening any item which is marked for opening in the editor in the 
        // .vstemplate file using the "OpenInEditor" attribute.
        void IWizard.BeforeOpeningFile(EnvDTE.ProjectItem projectItem) {

        }

        // This method is only applicable for item templates and does not get called for project templates.
        void IWizard.ProjectItemFinishedGenerating(EnvDTE.ProjectItem projectItem) {
        }

        // This method is only applicable for item templates and does not get called for project templates.
        bool IWizard.ShouldAddProjectItem(string filePath) {
            return true;
        }

        private const string LOG_FILE_NAME = "SharpArch.VSharpArchTemplate.log";
    }
}

