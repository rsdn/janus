@ECHO OFF

SET msbuild="%windir%\Microsoft.NET\Framework\v4.0.30319\msbuild.exe"

%msbuild% master.build /t:Build /p:Configuration=Debug /v:M /fl /flp:LogFile=msbuild.log;Verbosity=Normal /nr:false