;NSIS Modern User Interface
;Basic Example Script
;Written by Joost Verburg

;--------------------------------
;Include Modern UI

  !include "MUI2.nsh"

;--------------------------------
;General

  ;Name and file
  Name "iutNotify"
  OutFile "iutNotify Setup.exe"

  ;Default installation folder
  InstallDir "$PROGRAMFILES32\iutNotify"
  
  ;Get installation folder from registry if available
  InstallDirRegKey HKLM "Software\iutNotify" "InstallLocation"

  ;Request application privileges for Windows Vista
  RequestExecutionLevel admin

;--------------------------------
;Interface Settings

  !define MUI_ABORTWARNING

;--------------------------------
;Pages

  !insertmacro MUI_PAGE_DIRECTORY
  !insertmacro MUI_PAGE_INSTFILES
  
  !insertmacro MUI_UNPAGE_CONFIRM
  !insertmacro MUI_UNPAGE_INSTFILES
  
;--------------------------------
;Languages
 
  !insertmacro MUI_LANGUAGE "French"

;--------------------------------
;Installer Sections

Section

  SetOutPath "$INSTDIR"
  
  DetailPrint "Installation des fichiers"
  ;ADD YOUR OWN FILES HERE...
  File dotNetClientAgent\bin\x86\Release\iutNotify.exe
  
  ;Create uninstaller
  WriteUninstaller "$INSTDIR\uninstall.exe"

  DetailPrint "Ajout d'iutNotify.exe au démarage"
  ;Run at windows start
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Run" "iutNotify" "$INSTDIR\iutNotify.exe"  

  DetailPrint "Création des clés de registre"
  ;Store installation folder
  WriteRegStr HKLM "Software\iutNotify" "InstallLocation" $INSTDIR
  
  ;Add uninstaller into system
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\iutNotify" "DisplayName" "iutNotify"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\iutNotify" "Publisher" "IUTSB.Jeff"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\iutNotify" "UninstallString" "$\"$INSTDIR\uninstall.exe$\""
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\iutNotify" "QuietUninstallString" "$\"$INSTDIR\uninstall.exe$\" /S"
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\iutNotify" "VersionMajor" "0"
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\iutNotify" "VersionMinor" "1"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\iutNotify" "DisplayVersion" "0.1"

  DetailPrint "Lancement d'iutNotify.exe"
  Exec '"$WINDIR\explorer.exe" "$INSTDIR\iutNotify.exe"'
SectionEnd

;--------------------------------
;Uninstaller Section

Section "Uninstall"
  DetailPrint "Arrêt de iutNotify.exe"
  StrCpy $0 "iutNotify.exe"
  KillProcWMI::KillProc "iutNotify.exe"

  DetailPrint "Attente de l'arrêt d'iutNotify.exe"
  Sleep 1000
  
  DetailPrint "Suppression des fichiers"
  Delete "$INSTDIR\iutNotify.exe"
  Delete "$INSTDIR\uninstall.exe"

  RMDir "$INSTDIR"

  DetailPrint "Suppression des clés de registre"
  DeleteRegKey /ifempty HKLM "Software\iutNotify"
  DeleteRegValue HKLM "Software\Microsoft\Windows\CurrentVersion\Run" "iutNotify"
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\iutNotify"

SectionEnd