@ECHO OFF

SET msbuild="%windir%\Microsoft.NET\Framework\v4.0.30319\msbuild.exe"

%msbuild% master.build /t:Cleanup /p:Configuration=Debug /v:M /fl /flp:LogFile=msbuild.log;Verbosity=Normal /nr:false
%msbuild% master.build /t:Cleanup /p:Configuration=Release /v:M /fl /flp:LogFile=msbuild.log;Append;Verbosity=Normal /nr:false