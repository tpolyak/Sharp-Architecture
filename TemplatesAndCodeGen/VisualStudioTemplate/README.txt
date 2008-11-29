This contains everything you need to get a S#arp Architecture project up and running quickly.  To use:

1) Close all instances of Visual Studio 2008

2) Copy SharpArchApplicationTemplate.zip to "C:\Documents and Settings\<YOUR USERNAME>\My Documents\Visual Studio 2008\Templates\ProjectTemplates\Visual C#\"  (Other locations are available; see http://msdn.microsoft.com/en-us/library/y3kkate1.aspx for details.)

3) Copy SharpArchApplicationWizard.dll to "C:\Program Files\Microsoft Visual Studio 9.0\Common7\IDE\"

4) Create your S#arp Architecture project:

	* Open Visual Studio 2008
	* Select File / New / Project
	* Select the project type "Visual C#"
	* Select "S#arp Architecture Application" under "My Templates"
	* Enter a name for your project; e.g., Northwind, WickedCool
	* Enter the location that will serve as the parent folder of the project; e.g., C:\MyProjects, E:\Dev\AllProjects
	* Keep "Create directory for solution" unchecked (it'll work if this is selected but will add another parent folder)
	* Click OK

5) Right click the <YOUR PROJECT>.Web project and select "Set as StartUp Project"

6) Click F5 to build and run it!