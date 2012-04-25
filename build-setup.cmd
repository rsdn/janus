@ECHO OFF

SET msbuild="%windir%\Microsoft.NET\Framework\v4.0.30319\msbuild.exe"
%msbuild% master.build /t:BuildSetup %*