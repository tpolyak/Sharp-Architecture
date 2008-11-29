using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TemplateWizard;
using VSLangProj;
using EnvDTE;
using EnvDTE80;
using System.Windows.Forms;
using System.IO;

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
        }

        /// <summary>
        /// Runs custom wizard logic when a project has finished generating
        /// </summary>
        void IWizard.ProjectFinishedGenerating(EnvDTE.Project project) {
            if (project != null && project.Name == "SolutionItemsContainer") {
                MoveSolutionItemsToRoot(project);
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

        private void MoveSolutionItemsToRoot(EnvDTE.Project solutionItemsContainerProject) {
            string originalLocation = GetSolutionRootPath() + GetSolutionName() + "\\SolutionItemsContainer\\Solution Items";

            if (Directory.Exists(originalLocation)) {
                Directory.Move(originalLocation, GetSolutionRootPath() + GetSolutionName() + "\\Solution Items");

                Solution2 solution = dte.Solution as Solution2;
                solution.Remove(solutionItemsContainerProject);

                Directory.Delete(GetSolutionRootPath() + GetSolutionName() + "\\SolutionItemsContainer", true);
            }
            else {
                throw new ApplicationException("Couldn't find " + originalLocation + " to move");
            }
        }

        private void MoveSolutionFileToProjectsDirectory() {
            dte.Solution.SaveAs(
                GetSolutionRootPath() + GetSolutionName() + "\\" + GetSolutionFileName());
        }

        private void DeleteSuoFile() {
            if (File.Exists(GetSolutionRootPath() + GetSolutionName() + ".suo")) {
                File.Delete(GetSolutionRootPath() + GetSolutionName() + ".suo");
            }
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
    }
}

