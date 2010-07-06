This template only works with Visual Studio 2010. To install the template you will need to do the following:

Close all instances of Visual Studio 2010


If you haven’t already done so, install the T4Toolbox from http://www.codeplex.com/t4toolbox. This enables Visual Studio with templating capabilities. (This is needed for the CV resume CRUD scaffolding generator.) Please note any incompatibilities noted within the section entitled "Installing and Configuring Prerequisites."


Optionally install the T4 Editor Community Edition from http://www.t4editor.net/downloads.html which will add basic coloring to the templates from within Visual Studio.


Copy "<unzip location>\SharpArchApplicationTemplate_VS2010.zip" to "C:\Documents and Settings\<YOUR USERNAME>\My Documents\Visual Studio 2010\Templates\ProjectTemplates\Visual C#\Web\". (Other locations are also available for organizing your templates; see http://msdn.microsoft.com/en-us/library/y3kkate1.aspx for details.)


From the same source folder, copy SharpArchApplicationWizard.dll to "C:\Program Files\Microsoft Visual Studio 10.0\Common7\IDE\"
NOTE: use "C:\Program Files (x86)\Microsoft Visual Studio 10.0\Common7\IDE" on a 64bit system


You’re now ready to create your S#arp Architecture project:
Open Visual Studio 2010
Select File / New / Project
Select the project type Visual C# / Web
Select "S#arp Architecture Application" under "My Templates"
Enter a project name with no spaces; e.g., Northwind, WickedCool, or OtherProjectName
Enter the location that will serve as the parent folder of the project; e.g., C:\MyProjects, E:\Dev\AllProjects
Keep "Create directory for solution" unchecked (it'll work if this is selected but will add an extraneous parent folder)
Click OK


The generated project does not depend on ASP.NET MVC being installed on the development machine. While this is good for compatibility with multiple versions of MVC, the drawback is that ASP.NET MVC item templates are not available within Visual Studio by default. If you have ASP.NET MVC installed and you’d like to have the item templates available, do the following:
Using Windows Explorer, browse to the generated project and open <YOUR PROJECT>.Web.csproj with Notepad.
Insert the following immediately after the opening <ProjectTypeGuids> tag, and before the other GUIDs: "{F85E285D-A4E0-4152-9332-AB1D724D3325};"
Save and close the csproj file; you’ll be prompted to reload it in VS.


Additionally, ASP.NET MVC allows views to be compiled with the remainder of your solution. While this is a terrific benefit, it does add an appreciable amount of time to the compile time. The VS generated project is already preconfigured to support this capability but requires you to turn it on if you’d like it, by doing the following:
Using Windows Explorer, browse to the generated project and open <YOUR PROJECT>.Web.csproj with Notepad.
Change “false” to “true” within the MvcBuildViews tag.
Save and close the csproj file; you’ll be prompted to reload it in VS.


Create your empty project database, if it doesn’t already exist, and set the connection string within <YOUR PROJECT>.Web/NHibernate.config


Right click the <YOUR PROJECT>.Web project and select "Set as StartUp Project"


Click F5 to build and run it!


##########################################################################
## 
## Using the CrudScaffolding and CrudScaffoldingForEnterpriseApps
##
##########################################################################

Using either of these will require you to update the EntityScaffoldingDetails.tt and BaseTemplate.tt in each of these projects.
The paths to SharpArch.Core.dll and Inflector.Net.dll will need to be set to the full path of the assembly located in the /lib folder.