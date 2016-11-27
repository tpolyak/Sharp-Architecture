call RestorePackages.cmd
"%ProgramFiles(x86)%\MSBuild\14.0\Bin\msbuild.exe" Build.proj /t:Build /p:IsDesktopBuild=true /p:PreReleaseLabel=alpha
pause