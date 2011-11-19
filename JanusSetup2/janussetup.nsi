;NSIS Modern User Interface
;Basic Example Script
;Written by Joost Verburg

!define FILES_ROOT_FOLDER "..\Build\Release"

;--------------------------------
;Include Modern UI

  !include "MUI.nsh"

;--------------------------------
;General

  ;Name and file
  Name "RSDN@Home 1.1.4"
  OutFile "RAHSetup.exe"

  ;Default installation folder
  InstallDir "$PROGRAMFILES\RSDN@Home"
  
  ;Get installation folder from registry if available
  InstallDirRegKey HKCU "Software\Janus" ""

  Icon "..\Janus\Janus.ico"
  UninstallIcon "..\Janus\Janus.ico"

  XPStyle on

  Var MUI_TEMP
  Var STARTMENU_FOLDER

;--------------------------------
;Interface Settings

  !define MUI_ABORTWARNING

 ;--------------------------------
;Language Selection Dialog Settings
  ;Remember the installer language
  !define MUI_LANGDLL_REGISTRY_ROOT "HKCU" 
  !define MUI_LANGDLL_REGISTRY_KEY "Software\Modern UI Test" 
  !define MUI_LANGDLL_REGISTRY_VALUENAME "Installer Language"

;--------------------------------
;Pages

  !insertmacro MUI_PAGE_WELCOME
;  !insertmacro MUI_PAGE_LICENSE "${NSISDIR}\Contrib\Modern UI\License.txt"
  !insertmacro MUI_PAGE_COMPONENTS
  !insertmacro MUI_PAGE_DIRECTORY

  ;Start Menu Folder Page Configuration
  !define MUI_STARTMENUPAGE_REGISTRY_ROOT "HKCU" 
  !define MUI_STARTMENUPAGE_REGISTRY_KEY "Software\Janus" 
  !define MUI_STARTMENUPAGE_REGISTRY_VALUENAME "Start Menu Folder"
  !define MUI_STARTMENUPAGE_DEFAULTFOLDER "RSDN@Home"
  
  !insertmacro MUI_PAGE_STARTMENU Application $STARTMENU_FOLDER

  !insertmacro MUI_PAGE_INSTFILES
  !insertmacro MUI_PAGE_FINISH
  
  !insertmacro MUI_UNPAGE_WELCOME
  !insertmacro MUI_UNPAGE_CONFIRM
  !insertmacro MUI_UNPAGE_INSTFILES
  !insertmacro MUI_UNPAGE_FINISH
  
;--------------------------------
;Languages
 
  !insertmacro MUI_LANGUAGE "English"
  !insertmacro MUI_LANGUAGE "Russian"

;--------------------------------
;Installer Functions

Function .onInit

  !insertmacro MUI_LANGDLL_DISPLAY

FunctionEnd

;--------------------------------
;Installer Sections

LangString UninstallLnkName ${LANG_ENGLISH} "Uninstall"
LangString UninstallLnkName ${LANG_RUSSIAN} "Удалить"

;=== Common ===
Section "" SecCommon

  SetOutPath "$INSTDIR"

  File "${FILES_ROOT_FOLDER}\Janus.exe"
  File "${FILES_ROOT_FOLDER}\Janus.exe.config"
  File "${FILES_ROOT_FOLDER}\Janus.exe.manifest"

  File "${FILES_ROOT_FOLDER}\Interop.ADOX.dll"
  File "${FILES_ROOT_FOLDER}\Interop.JRO.dll"
  File "${FILES_ROOT_FOLDER}\Lucene.Net.dll"
  File "${FILES_ROOT_FOLDER}\JanusProtocol.dll"
  File "${FILES_ROOT_FOLDER}\NamedPipesSupport.dll"
  File "${FILES_ROOT_FOLDER}\PropertyGridCustomizer.dll"
  File "${FILES_ROOT_FOLDER}\Rsdn.Framework.Common.dll"
  File "${FILES_ROOT_FOLDER}\Rsdn.Framework.Formatting.dll"
  File "${FILES_ROOT_FOLDER}\Rsdn.Framework.Data.2.dll"
  File "${FILES_ROOT_FOLDER}\ScintillaEditor.dll"
  File "${FILES_ROOT_FOLDER}\Shortcuts.dll"
  File "${FILES_ROOT_FOLDER}\TreeGrid3.dll"

  File "${FILES_ROOT_FOLDER}\JanusDoc.chm"

  File "..\Dependencies\ado-interop\adodb.dll"

  SetOutPath "$INSTDIR\ru"
  File /r /x ".svn" "${FILES_ROOT_FOLDER}\ru\*.dll"

  SetOutPath "$INSTDIR\buttons\_default"
  File /r /x ".svn" "${FILES_ROOT_FOLDER}\buttons\_default\*.png"
  SetOutPath "$INSTDIR\buttons\classic"
  File /r /x ".svn" "${FILES_ROOT_FOLDER}\buttons\classic\*.png"

  SetOutPath "$INSTDIR\images"
  File /r /x ".svn" "${FILES_ROOT_FOLDER}\images\*.gif"

  SetOutPath "$INSTDIR\sound"
  File /r "${FILES_ROOT_FOLDER}\sound\*.wav"

  SetOutPath "$INSTDIR\styles"
  File /r "${FILES_ROOT_FOLDER}\styles\*.css"

  File "${FILES_ROOT_FOLDER}\firebird.conf"
  File "${FILES_ROOT_FOLDER}\fbembed.dll"
  File "${FILES_ROOT_FOLDER}\FirebirdSql.Data.Firebird.dll"
  File "${FILES_ROOT_FOLDER}\ib_util.dll"
  File "${FILES_ROOT_FOLDER}\firebird.log"
  File "${FILES_ROOT_FOLDER}\firebird.msg"

  SetOutPath "$INSTDIR\udf"
  File /r "${FILES_ROOT_FOLDER}\udf\*.*"


  !insertmacro MUI_STARTMENU_WRITE_BEGIN Application
    
    ;Create shortcuts
    CreateDirectory "$SMPROGRAMS\$STARTMENU_FOLDER"
    CreateShortCut "$SMPROGRAMS\$STARTMENU_FOLDER\$(UninstallLnkName).lnk" "$INSTDIR\Uninstall.exe"
    CreateShortCut "$SMPROGRAMS\$STARTMENU_FOLDER\RSDN@Home.lnk" "$INSTDIR\Janus.exe"
  
  !insertmacro MUI_STARTMENU_WRITE_END
  
  ;Store installation folder
  WriteRegStr HKCU "Software\Janus" "" $INSTDIR
  
  ;Create uninstaller
  WriteUninstaller "$INSTDIR\Uninstall.exe"

SectionEnd

;=== GoJanus ===
LangString SecGoJanusName ${LANG_ENGLISH} "IE plugin"
LangString SecGoJanusName ${LANG_Russian} "Плагин для IE"

Section "$(SecGoJanusName)" SecGoJanus
	SetOutPath "$INSTDIR\GoJanus"
	File "..\GoJanus\ReleaseUMinDependency\GoJanus.dll"
	File "..\GoJanus\ReleaseUMinDependency\GoJanus.dll.html"

	RegDLL "$INSTDIR\GoJanus\GoJanus.dll"
SectionEnd

LangString SecGoJanusDesc ${LANG_ENGLISH} "Context menu plugin for Internet Explorer browser"
LangString SecGoJanusDesc ${LANG_RUSSIAN} "Плагин, расширяющий контекстное меню браузера Internet Explorer"


;=== Outlook ===
LangString SecStyleOutlookName ${LANG_ENGLISH} "Outlook icon set"
LangString SecStyleOutlookName ${LANG_Russian} "Набор иконок Outlook"

Section "$(SecStyleOutlookName)" SecStyleOutlook
	SetOutPath "$INSTDIR\buttons\outlook"
  File /r /x ".svn" "${FILES_ROOT_FOLDER}\buttons\outlook\*.png"
SectionEnd

LangString SecStyleOutlookDesc ${LANG_ENGLISH} "Outlook-style icon set for application"
LangString SecStyleOutlookDesc ${LANG_RUSSIAN} "Набор иконок для приложения в стиле Outlook"


;=== Modern ===
LangString SecStyleModernName ${LANG_ENGLISH} "Modern icon set"
LangString SecStyleModernName ${LANG_Russian} "Набор иконок Modern"

Section "$(SecStyleModernName)" SecStyleModern
	SetOutPath "$INSTDIR\buttons\modern"
  File /r /x ".svn" "${FILES_ROOT_FOLDER}\buttons\modern\*.png"
SectionEnd

LangString SecStyleModernDesc ${LANG_ENGLISH} "Modern-style icon set for application"
LangString SecStyleModernDesc ${LANG_RUSSIAN} "Набор иконок для приложения в стиле Modern"

;sections description
!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
	!insertmacro MUI_DESCRIPTION_TEXT ${SecGoJanus} $(SecGoJanusDesc)
	!insertmacro MUI_DESCRIPTION_TEXT ${SecStyleOutlook} $(SecStyleOutlookDesc)
  !insertmacro MUI_DESCRIPTION_TEXT ${SecStyleModern} $(SecStyleModernDesc)
!insertmacro MUI_FUNCTION_DESCRIPTION_END


;--------------------------------
;Uninstaller Section

Section "Uninstall"

  ;ADD YOUR OWN FILES HERE...

  Delete "$INSTDIR\Uninstall.exe"

  Delete "$INSTDIR\Janus.exe"
  Delete "$INSTDIR\Janus.exe.config"
  Delete "$INSTDIR\Janus.exe.manifest"

  Delete "$INSTDIR\Interop.ADOX.dll"
  Delete "$INSTDIR\Interop.JRO.dll"
  Delete "$INSTDIR\JanusProtocol.dll"
  Delete "$INSTDIR\NamedPipesSupport.dll"
  Delete "$INSTDIR\PropertyGridCustomizer.dll"
  Delete "$INSTDIR\Rsdn.Framework.Common.dll"
  Delete "$INSTDIR\Rsdn.Framework.Formatting.dll"
  Delete "$INSTDIR\Rsdn.Framework.Data.2.dll"
  Delete "$INSTDIR\ScintillaEditor.dll"
  Delete "$INSTDIR\Shortcuts.dll"
  Delete "$INSTDIR\TreeGrid3.dll"
  
  Delete "$INSTDIR\Lucene.Net.dll"

  Delete "$INSTDIR\SciLexer.dll"

  Delete "$INSTDIR\JanusDoc.chm"
  
  Delete "$INSTDIR\firebird.conf"
  Delete "$INSTDIR\fbembed.dll"
  Delete "$INSTDIR\FirebirdSql.Data.Firebird.dll"
  Delete "$INSTDIR\ib_util.dll"
  Delete "$INSTDIR\firebird.log"
  Delete "$INSTDIR\firebird.msg"
  
  Delete "$INSTDIR\adodb.dll"

  RMDir /r "$INSTDIR\ru"

  UnregDLL "$INSTDIR\GoJanus\GoJanus.dll"
  RMDir /r "$INSTDIR\GoJanus"

  RMDir /r "$INSTDIR\buttons"

  RMDir /r "$INSTDIR\images"

  RMDir /r "$INSTDIR\sound"

  RMDir /r "$INSTDIR\styles"
  RMDir /r "$INSTDIR\udf"

  !insertmacro MUI_STARTMENU_GETFOLDER Application $MUI_TEMP

  Delete "$SMPROGRAMS\$MUI_TEMP\$(UninstallLnkName).lnk"
  Delete "$SMPROGRAMS\$MUI_TEMP\RSDN@Home.lnk"

  StrCpy $MUI_TEMP "$SMPROGRAMS\$MUI_TEMP"
 
  startMenuDeleteLoop:
	ClearErrors
    RMDir $MUI_TEMP
    GetFullPathName $MUI_TEMP "$MUI_TEMP\.."
    
    IfErrors startMenuDeleteLoopDone
  
    StrCmp $MUI_TEMP $SMPROGRAMS startMenuDeleteLoopDone startMenuDeleteLoop
  startMenuDeleteLoopDone:

  DeleteRegKey /ifempty HKCU "Software\Janus"

SectionEnd