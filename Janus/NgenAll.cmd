@echo off

SET NETFW=%SystemRoot%\Microsoft.Net\Framework\v2.0.50727

SET NGEN=%NETFW%\ngen.exe /nologo 
%NGEN% BLToolkit.2.dll
%NGEN% BLToolkit.3.dll
%NGEN% FirebirdSql.Data.Firebird.dll
%NGEN% FirebirdSql.Data.FirebirdClient.dll
%NGEN% GoJanusNet.dll
%NGEN% Janus-Common.dll
%NGEN% Janus-Model.dll
%NGEN% Janus.Framework.dll
%NGEN% JanusProtocol.dll
%NGEN% Lucene.Net.dll
%NGEN% PropertyGridCustomizer.dll
%NGEN% R.SAT-Common.dll
%NGEN% R.SAT-Model.dll
%NGEN% Rsdn.Framework.Common.dll
%NGEN% Rsdn.Framework.Formatting.dll
%NGEN% ScintillaEditor.dll
%NGEN% Shortcuts.dll
%NGEN% System.Core.Emulation.dll
%NGEN% System.Data.SQLite.DLL
%NGEN% TreeGrid3.dll
