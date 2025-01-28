; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "AV-Bible"
#define MyAppVersion "9.25.1.27"
#define MyAppPublisher "Digital-AV.org"
#define MyAppURL "https://github.com/kwonus/AVBible"
#define MyRawExeName "AVBible.exe"
#define MyAppExeName "AV-Bible.exe"
#define MyMgrExeName "AV-Data-Manager.exe"
#define RootSRC "C:\src"
#define DotnetInstaller "windowsdesktop-runtime-8.0.11-win-x64.exe"
[Setup]
; NOTE: The value of AppId uniquely identifies this application. Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{426CF911-39B3-4B06-AD22-9EEB7BE28AA2}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\{#MyAppName}
DisableDirPage=yes
; "ArchitecturesAllowed=x64compatible" specifies that Setup cannot run
; on anything but x64 and Windows 11 on Arm.
ArchitecturesAllowed=x64compatible
; "ArchitecturesInstallIn64BitMode=x64compatible" requests that the
; install be done in "64-bit mode" on x64 or Windows 11 on Arm,
; meaning it should use the native 64-bit Program Files directory and
; the 64-bit view of the registry.
ArchitecturesInstallIn64BitMode=x64compatible
DisableProgramGroupPage=yes
LicenseFile={#RootSRC}\AV-Bible\LICENSE.md
; Uncomment the following line to run in non administrative install mode (install for current user only.)
PrivilegesRequired=lowest
OutputBaseFilename=AV-Bible-2025-Setup
SetupIconFile={#RootSRC}\AV-Bible\gutenburg-press-256.ico
Compression=lzma
SolidCompression=yes
WizardStyle=modern
OutputDir=Release\9.25.1.27

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Components]
Name: "avapp"; Description: "Install AV-Bible Desktop Application (requires Microsoft .NET 8)"; Types: full compact custom
Name: "manager"; Description: "Install AV-Data-Manager"; Types: full custom
Name: "manager/addin"; Description: "Install AV-Bible Addin for Microsoft Word (requires Microsoft Office)"; Types: full custom

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked; Components: avapp

[Files]
Source: "{#RootSRC}\AV-Bible\bin\x64\Release\net8.0-windows10.0.17763.0\{#MyRawExeName}"; DestDir: "{app}"; DestName: {#MyAppExeName}; Flags: ignoreversion; Components: avapp
Source: "{#RootSRC}\AV-Bible\gutenburg-press-256.ico"; DestDir: "{app}"; Flags: ignoreversion; Components: avapp
Source: "{#RootSRC}\AV-Bible\bin\x64\Release\net8.0-windows10.0.17763.0\AVBible.deps.json"; DestDir: "{app}"; Flags: ignoreversion; Components: avapp
Source: "{#RootSRC}\AV-Bible\bin\x64\Release\net8.0-windows10.0.17763.0\AVBible.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: avapp
Source: "{#RootSRC}\AV-Bible\bin\x64\Release\net8.0-windows10.0.17763.0\AVBible.runtimeconfig.json"; DestDir: "{app}"; Flags: ignoreversion; Components: avapp
Source: "{#RootSRC}\AV-Bible\bin\x64\Release\net8.0-windows10.0.17763.0\AV-Engine.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: avapp
Source: "{#RootSRC}\AV-Bible\bin\x64\Release\net8.0-windows10.0.17763.0\AV-Search.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: avapp
Source: "{#RootSRC}\AV-Bible\bin\x64\Release\net8.0-windows10.0.17763.0\AVXLib.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: avapp
Source: "{#RootSRC}\AV-Bible\bin\x64\Release\net8.0-windows10.0.17763.0\Blacklight.Controls.Wpf.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: avapp
Source: "{#RootSRC}\AV-Bible\bin\x64\Release\net8.0-windows10.0.17763.0\Blueprint-Blue-Lib.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: avapp
Source: "{#RootSRC}\AV-Bible\bin\x64\Release\net8.0-windows10.0.17763.0\Microsoft.Windows.SDK.NET.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: avapp
Source: "{#RootSRC}\AV-Bible\bin\x64\Release\net8.0-windows10.0.17763.0\Neo.Markdig.Xaml.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: avapp
Source: "{#RootSRC}\AV-Bible\bin\x64\Release\net8.0-windows10.0.17763.0\Markdig.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: avapp
Source: "{#RootSRC}\AV-Bible\bin\x64\Release\net8.0-windows10.0.17763.0\PhonemeEmbeddings.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: avapp
Source: "{#RootSRC}\AV-Bible\bin\x64\Release\net8.0-windows10.0.17763.0\pinshot_blue.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: avapp
Source: "{#RootSRC}\AV-Bible\bin\x64\Release\net8.0-windows10.0.17763.0\YamlDotNet.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: avapp
Source: "{#RootSRC}\AV-Bible\bin\x64\Release\net8.0-windows10.0.17763.0\WinRT.Runtime.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: avapp
;
Source: "{#RootSRC}\AV-Bible\Digital-AV\AVX-Omega.data"; DestDir: "{app}\Digital-AV"; Flags: ignoreversion; Components: avapp or manager
Source: "{#RootSRC}\AV-Bible\Digital-AV\AVX-Omega.md5"; DestDir: "{app}\Digital-AV"; Flags: ignoreversion; Components: avapp or manager
Source: "{#RootSRC}\AV-Bible\Digital-AV\AVX-Omega.txt"; DestDir: "{app}\Digital-AV"; Flags: ignoreversion; Components: avapp or manager
Source: "{#RootSRC}\AV-Bible\Digital-AV\en_US.txt"; DestDir: "{app}\Digital-AV"; Flags: ignoreversion; Components: avapp or manager
;
Source: "{#RootSRC}\AV-Bible\Help\application.html"; DestDir: "{app}\Help"; Flags: ignoreversion; Components: avapp
Source: "{#RootSRC}\AV-Bible\Help\application.md"; DestDir: "{app}\Help"; Flags: ignoreversion; Components: avapp
Source: "{#RootSRC}\AV-Bible\Help\AV-Bible-S4T.html"; DestDir: "{app}\Help"; Flags: ignoreversion; Components: avapp
Source: "{#RootSRC}\AV-Bible\Help\AV-Bible-S4T.md"; DestDir: "{app}\Help"; Flags: ignoreversion; Components: avapp
Source: "{#RootSRC}\AV-Bible\Help\export.html"; DestDir: "{app}\Help"; Flags: ignoreversion; Components: avapp
Source: "{#RootSRC}\AV-Bible\Help\export.md"; DestDir: "{app}\Help"; Flags: ignoreversion; Components: avapp
Source: "{#RootSRC}\AV-Bible\Help\hashtags.html"; DestDir: "{app}\Help"; Flags: ignoreversion; Components: avapp
Source: "{#RootSRC}\AV-Bible\Help\hashtags.md"; DestDir: "{app}\Help"; Flags: ignoreversion; Components: avapp
Source: "{#RootSRC}\AV-Bible\Help\Index.html"; DestDir: "{app}\Help"; Flags: ignoreversion; Components: avapp
Source: "{#RootSRC}\AV-Bible\Help\Index-application.html"; DestDir: "{app}\Help"; Flags: ignoreversion; Components: avapp
Source: "{#RootSRC}\AV-Bible\Help\Index-export.html"; DestDir: "{app}\Help"; Flags: ignoreversion; Components: avapp
Source: "{#RootSRC}\AV-Bible\Help\Index-hashtags.html"; DestDir: "{app}\Help"; Flags: ignoreversion; Components: avapp
Source: "{#RootSRC}\AV-Bible\Help\Index-language.html"; DestDir: "{app}\Help"; Flags: ignoreversion; Components: avapp
Source: "{#RootSRC}\AV-Bible\Help\Index-selection.html"; DestDir: "{app}\Help"; Flags: ignoreversion; Components: avapp
Source: "{#RootSRC}\AV-Bible\Help\Index-settings.html"; DestDir: "{app}\Help"; Flags: ignoreversion; Components: avapp
Source: "{#RootSRC}\AV-Bible\Help\Index-system.html"; DestDir: "{app}\Help"; Flags: ignoreversion; Components: avapp
Source: "{#RootSRC}\AV-Bible\Help\language.html"; DestDir: "{app}\Help"; Flags: ignoreversion; Components: avapp
Source: "{#RootSRC}\AV-Bible\Help\language.md"; DestDir: "{app}\Help"; Flags: ignoreversion; Components: avapp
Source: "{#RootSRC}\AV-Bible\Help\selection.html"; DestDir: "{app}\Help"; Flags: ignoreversion; Components: avapp
Source: "{#RootSRC}\AV-Bible\Help\selection.md"; DestDir: "{app}\Help"; Flags: ignoreversion; Components: avapp
Source: "{#RootSRC}\AV-Bible\Help\settings.html"; DestDir: "{app}\Help"; Flags: ignoreversion; Components: avapp
Source: "{#RootSRC}\AV-Bible\Help\settings.md"; DestDir: "{app}\Help"; Flags: ignoreversion; Components: avapp
Source: "{#RootSRC}\AV-Bible\Help\system.html"; DestDir: "{app}\Help"; Flags: ignoreversion; Components: avapp
Source: "{#RootSRC}\AV-Bible\Help\system.md"; DestDir: "{app}\Help"; Flags: ignoreversion; Components: avapp
Source: "{#RootSRC}\AV-Bible\Help\topics.html"; DestDir: "{app}\Help"; Flags: ignoreversion; Components: avapp
Source: "{#RootSRC}\AV-Bible\Help\html-generator\md-page.js"; DestDir: "{app}\Help\html-generator"; Flags: ignoreversion; Components: avapp
Source: "{#RootSRC}\AV-Bible\Help\css\style.css"; DestDir: "{app}\Help\css"; Flags: ignoreversion; Components: avapp
Source: "{#RootSRC}\AV-Bible\Help\diagrams\QCommand.png"; DestDir: "{app}\Help\diagrams"; Flags: ignoreversion; Components: avapp
Source: "{#RootSRC}\AV-Bible\Help\diagrams\QFind.png"; DestDir: "{app}\Help\diagrams"; Flags: ignoreversion; Components: avapp
;
Source: "{#RootSRC}\AV-Bible-Addin\bin\Release\AV-Bible-Addin.dll"; DestDir: "{app}\Addin"; Flags: ignoreversion; Components: manager/addin
Source: "{#RootSRC}\AV-Bible-Addin\bin\Release\AV-Bible-Addin.dll.manifest"; DestDir: "{app}\Addin"; Flags: ignoreversion; Components: manager/addin
Source: "{#RootSRC}\AV-Bible-Addin\bin\Release\AV-Bible-Addin.vsto"; DestDir: "{app}\Addin"; Flags: ignoreversion; Components: manager/addin
Source: "{#RootSRC}\AV-Bible-Addin\bin\Release\Microsoft.Office.Tools.Common.v4.0.Utilities.dll"; DestDir: "{app}\Addin"; Flags: ignoreversion; Components: manager/addin
Source: "{#RootSRC}\AV-Bible-Addin\bin\Release\YamlDotNet.dll"; DestDir: "{app}\Addin"; Flags: ignoreversion; Components: manager/addin
;
Source: "{#RootSRC}\AV-Data-Manager\bin\Release\net8.0-windows7.0\{#MyMgrExeName}"; DestDir: "{app}\Manager"; Flags: ignoreversion; Components: manager
Source: "{#RootSRC}\AV-Data-Manager\bin\Release\net8.0-windows7.0\AV-Data-Manager.dll"; DestDir: "{app}\Manager"; Flags: ignoreversion; Components: manager
Source: "{#RootSRC}\AV-Data-Manager\bin\Release\net8.0-windows7.0\AV-API.dll"; DestDir: "{app}\Manager"; Flags: ignoreversion; Components: manager
Source: "{#RootSRC}\AV-Data-Manager\bin\Release\net8.0-windows7.0\AV-Engine.dll"; DestDir: "{app}\Manager"; Flags: ignoreversion; Components: manager
Source: "{#RootSRC}\AV-Data-Manager\bin\Release\net8.0-windows7.0\AV-Search.dll"; DestDir: "{app}\Manager"; Flags: ignoreversion; Components: manager
Source: "{#RootSRC}\AV-Data-Manager\bin\Release\net8.0-windows7.0\AVXLib.dll"; DestDir: "{app}\Manager"; Flags: ignoreversion; Components: manager
Source: "{#RootSRC}\AV-Data-Manager\bin\Release\net8.0-windows7.0\Blueprint-Blue-Lib.dll"; DestDir: "{app}\Manager"; Flags: ignoreversion; Components: manager
Source: "{#RootSRC}\AV-Data-Manager\bin\Release\net8.0-windows7.0\PhonemeEmbeddings.dll"; DestDir: "{app}\Manager"; Flags: ignoreversion; Components: manager
Source: "{#RootSRC}\AV-Data-Manager\bin\Release\net8.0-windows7.0\YamlDotNet.dll"; DestDir: "{app}\Manager"; Flags: ignoreversion; Components: manager
Source: "{#RootSRC}\AV-Data-Manager\bin\Release\net8.0-windows7.0\pinshot_blue.dll"; DestDir: "{app}\Manager"; Flags: ignoreversion; Components: manager
Source: "{#RootSRC}\AV-Data-Manager\bin\Release\net8.0-windows7.0\AV-Data-Manager.runtimeconfig.json"; DestDir: "{app}\Manager"; Flags: ignoreversion; Components: manager
Source: "{#RootSRC}\AV-Data-Manager\bin\Release\net8.0-windows7.0\AV-Data-Manager.deps.json"; DestDir: "{app}\Manager"; Flags: ignoreversion; Components: manager
Source: "{#RootSRC}\AV-Data-Manager\bin\Release\net8.0-windows7.0\appsettings.json"; DestDir: "{app}\Manager"; Flags: ignoreversion; Components: manager

;
;Source: "{#RootSRC}\AVBible\Installation\Prerequisites\{#DotnetInstaller}"; DestDir: {tmp}; Flags: deleteafterinstall; AfterInstall: InstallFramework; Components: avapp

; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Code]
procedure CurStepChanged(CurStep: TSetupStep);
var
  AppPath: string;
  i: Integer;
begin
  if (CurStep = ssPostInstall) and IsComponentSelected('manager/addin')
  then begin
    AppPath := ExpandConstant('{app}');
    for i := 1 to Length(AppPath)
    do begin
      if AppPath[i] = '\'
        then AppPath[i] := '/';
    end;
    
    // Write the converted path to the registry if the task is selected
    RegWriteStringValue(HKCU, 'Software\Microsoft\Office\Word\Addins\AV-Bible-Addin', 'Manifest', 'file:///' + AppPath + '/Addin/AV-Bible-Addin.vsto|vstolocal');
  end;
end;

procedure CurUninstallStepChanged(CurUninstallStep: TUninstallStep);
begin
  RegDeleteValue(HKCU, 'Software\Microsoft\Office\Word\Addins\AV-Bible-Addin', 'Manifest');
end;

//procedure InstallFramework;
//var
//  ResultCode: Integer;
//begin
//  if not Exec(ExpandConstant('{tmp}\{#DotnetInstaller}'), '/q /norestart', '', SW_SHOW, ewWaitUntilTerminated, ResultCode) then
//  begin
//    { you can interact with the user that the installation failed }
//    MsgBox('.NET installation failed with code: ' + IntToStr(ResultCode) + '.', mbError, MB_OK);
//  end;
//end;

[Registry]
Root: HKCU; Subkey: Software\Microsoft\Windows\CurrentVersion\Run; ValueType: string; \
            ValueName: AV-Bible-Addin-Manager; ValueData: {app}\Manager\{#MyMgrExeName}; Components: manager/addin
Root: HKCU; Subkey: Software\Microsoft\Office\Word\Addins\AV-Bible-Addin; ValueType: DWORD; \
            ValueName: LoadBehavior; ValueData: 3; Components: manager/addin
Root: HKCU; Subkey: Software\Microsoft\Office\Word\Addins\AV-Bible-Addin; ValueType: string; \
            ValueName: Description; ValueData: AV-Bible-Addin; Components: manager/addin
Root: HKCU; Subkey: Software\Microsoft\Office\Word\Addins\AV-Bible-Addin; ValueType: string; \
            ValueName: FriendlyName; ValueData: AV-Bible Addin for Microsoft Word; Components: manager/addin
;Root: HKCU; Subkey: Software\Microsoft\Office\Word\Addins\AV-Bible-Addin; ValueType: string; \
;            ValueName: Manifest; ValueData: file:///{app}/Addin/AV-Bible-Addin.vsto|vstolocal; Components: manager/addin

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Components: avapp
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon; Components: avapp

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent; Components: avapp
Filename: "{app}\Manager\{#MyMgrExeName}"; Description: "AV-Data-Manager"; Flags: nowait postinstall; Components: manager

