@ECHO OFF
%SYSTEMROOT%\Microsoft.NET\Framework\v3.5\msbuild.exe master.build /t:CleanDebug %*
%SYSTEMROOT%\Microsoft.NET\Framework\v3.5\msbuild.exe master.build /t:CleanRelease %*