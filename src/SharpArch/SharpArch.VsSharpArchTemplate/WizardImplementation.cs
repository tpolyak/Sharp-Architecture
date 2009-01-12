using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TemplateWizard;
using VSLangProj;
using EnvDTE;
using EnvDTE80;
using System.IO;
using System.Windows.Forms;
using System.Threading;

namespace SharpArchApplicationWizard
{
    // Class that implements the IWizard() interface
    internal class WizardImplementation : IWizard
    {
        /// <summary>
        /// Provide a means for sub-projects to have access to the solution name
        /// </summary>
        private static string solutionName;

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

            // Make the solution root path available for all the child projects
            if (runKind == WizardRunKind.AsNewProject) {
                replacementsDictionary.Add("$solutionrootpath$", GetSolutionRootPath() + solutionName + "\\");
            }
        }

        /// <summary>
        /// Runs custom wizard logic when a project has finished generating
        /// </summary>
        void IWizard.ProjectFinishedGenerating(EnvDTE.Project project) {
            if (project != null) {
                if (project.Name == "SolutionItemsContainer") {
                    CreateSolutionDirectoryStructure();
                    MoveSolutionItemsToLib(project);
                }
                else if (project.Name == "ToolsSolutionItemsContainer") {
                    MoveSolutionItemsToToolsLib(project);
                }
                else if (project.Name == "CrudScaffolding") {
                    Project movedProject = MoveProjectTo("\\tools\\", project, "Code Generation");
                    ExcludeProjectFromBuildProcess(movedProject);
                }
                else if (project.Name == GetSolutionName() + ".Tests") {
                    MoveProjectTo("\\tests\\", project);
                }
                else if (project.Name == GetSolutionName() + ".Web.Controllers" || 
                    project.Name == GetSolutionName() + ".Core" || 
                    project.Name == GetSolutionName() + ".Data" || 
                    project.Name == GetSolutionName() + ".Web") {
                    MoveProjectTo("\\app\\", project);
                }
            }
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
            SolutionBuild2 solutionBuild = (SolutionBuild2) solution.SolutionBuild;

            foreach (SolutionConfiguration solutionConfiguration in solutionBuild.SolutionConfigurations) {
                foreach (SolutionContext solutionContext in solutionConfiguration.SolutionContexts) {
                    if (solutionContext.ProjectName.IndexOf(project.Name) > -1) {
                        Log("Setting build to false for project " + solutionContext.ProjectName + 
                            " within the " + solutionConfiguration.Name + " configuration");
                        solutionContext.ShouldBuild = false;
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

                Log("Removing " + projectName + " from solution");
                solution.Remove(project);
                // Give the solution time to release the lock on the project file
                System.Threading.Thread.Sleep(500);

                string targetLocation = GetSolutionRootPath() + GetSolutionName() + targetSubFolder + projectName;

                Log("Moving " + projectName + " from " + originalLocation + " to target location at " + targetLocation);
                Directory.Move(originalLocation, targetLocation);

                if (!string.IsNullOrEmpty(solutionFolderName)) {
                    SolutionFolder solutionFolder = (SolutionFolder)solution.AddSolutionFolder(solutionFolderName).Object;
                    Log("Adding " + projectName + " to solution folder " + targetLocation);
                    return solutionFolder.AddFromFile(targetLocation + "\\" + projectName + ".csproj");
                }
                else {
                    Log("Adding " + projectName + " to solution");
                    return solution.AddFromFile(targetLocation + "\\" + projectName + ".csproj", false);
                }
            }
            else {
                throw new ApplicationException("Couldn't find " + originalLocation + " to move");
            }
        }

        private void MoveSolutionItemsToLib(EnvDTE.Project solutionItemsContainerProject) {
            string originalLocation = GetSolutionRootPath() + GetSolutionName() + "\\SolutionItemsContainer\\Solution Items";

            if (Directory.Exists(originalLocation)) {
                string targetLibFolder = GetSolutionRootPath() + GetSolutionName() + "\\lib";

                Log("Moving solution items from " + originalLocation + " to lib at " + targetLibFolder);
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

                Log("Moving tools solution items from " + originalLocation + " to tools lib at " + targetToolsLibFolder);
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
            Directory.CreateDirectory(GetSolutionRootPath() + GetSolutionName() + "\\db");
            Directory.CreateDirectory(GetSolutionRootPath() + GetSolutionName() + "\\docs");
            Directory.CreateDirectory(GetSolutionRootPath() + GetSolutionName() + "\\logs");
            Directory.CreateDirectory(GetSolutionRootPath() + GetSolutionName() + "\\tests");
            Directory.CreateDirectory(GetSolutionRootPath() + GetSolutionName() + "\\tools");
            Directory.CreateDirectory(GetSolutionRootPath() + GetSolutionName() + "\\tools\\build");
        }

        private void MoveSolutionFileToProjectsDirectory() {
            dte.Solution.SaveAs(
                GetSolutionRootPath() + GetSolutionName() + "\\" + GetSolutionFileName());
        }

        private void DeleteSuoFile() {
            string suoFile = GetSolutionRootPath() + GetSolutionName() + ".suo";

            if (File.Exists(suoFile)) {
                Log("Deleting " + suoFile);
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

