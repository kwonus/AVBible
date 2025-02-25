; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "AV-Bible for Windows"
#define MyAppVersion "9.25.2.24"
#define MyAppPublisher "Digital-AV.org"
#define MyAppURL "https://github.com/kwonus/AVBible"
#define MyRawExeName "AVBible.exe"
#define MyAppExeName "AV-Bible.exe"
#define RootSRC "C:\src"
;#define DotnetInstaller "windowsdesktop-runtime-8.0.11-win-x64.exe"
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
OutputBaseFilename=AV-Bible-For-Windows-Setup(MSA-Test)
SetupIconFile={#RootSRC}\AV-Bible\gutenburg-press-256.ico
Compression=lzma
SolidCompression=yes
WizardStyle=modern
OutputDir=Setup\{#MyAppVersion}

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "{#RootSRC}\AV-Bible\bin\x64\Release\net8.0-windows10.0.17763.0\{#MyRawExeName}"; DestDir: "{app}"; DestName: {#MyAppExeName}; Flags: ignoreversion
Source: "{#RootSRC}\AV-Bible\gutenburg-press-256.ico"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#RootSRC}\AV-Bible\bin\x64\Release\net8.0-windows10.0.17763.0\AVBible.deps.json"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#RootSRC}\AV-Bible\bin\x64\Release\net8.0-windows10.0.17763.0\AVBible.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#RootSRC}\AV-Bible\bin\x64\Release\net8.0-windows10.0.17763.0\AVBible.runtimeconfig.json"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#RootSRC}\AV-Bible\bin\x64\Release\net8.0-windows10.0.17763.0\AV-Engine.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#RootSRC}\AV-Bible\bin\x64\Release\net8.0-windows10.0.17763.0\AV-Search.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#RootSRC}\AV-Bible\bin\x64\Release\net8.0-windows10.0.17763.0\AVXLib.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#RootSRC}\AV-Bible\bin\x64\Release\net8.0-windows10.0.17763.0\Blacklight.Controls.Wpf.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#RootSRC}\AV-Bible\bin\x64\Release\net8.0-windows10.0.17763.0\Blueprint-Blue-Lib.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#RootSRC}\AV-Bible\bin\x64\Release\net8.0-windows10.0.17763.0\Microsoft.Windows.SDK.NET.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#RootSRC}\AV-Bible\bin\x64\Release\net8.0-windows10.0.17763.0\Neo.Markdig.Xaml.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#RootSRC}\AV-Bible\bin\x64\Release\net8.0-windows10.0.17763.0\Markdig.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#RootSRC}\AV-Bible\bin\x64\Release\net8.0-windows10.0.17763.0\PhonemeEmbeddings.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "{#RootSRC}\NUPhone\PhonemeEmbeddings\en_US.txt"; DestDir: "{app}\NUPhone"; Flags: ignoreversion
Source: "{#RootSRC}\pinshot-blue\target\x86_64-pc-windows-gnu\release\pinshot_blue.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#RootSRC}\AV-Bible\bin\x64\Release\net8.0-windows10.0.17763.0\YamlDotNet.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#RootSRC}\AV-Bible\bin\x64\Release\net8.0-windows10.0.17763.0\WinRT.Runtime.dll"; DestDir: "{app}"; Flags: ignoreversion
;
Source: "{#RootSRC}\Digital-AV\omega\data\target\x86_64-pc-windows-gnu\release\omega_data.dll"; DestDir: "{app}"; Flags: ignoreversion
;Source: "{#RootSRC}\AV-Bible\Digital-AV\AVX-Omega.data"; DestDir: "{app}\Digital-AV"; Flags: ignoreversion
Source: "{#RootSRC}\Digital-AV\omega\AVX-Omega.md5"; DestDir: "{app}\Digital-AV"; Flags: ignoreversion
Source: "{#RootSRC}\Digital-AV\omega\AVX-Omega.txt"; DestDir: "{app}\Digital-AV"; Flags: ignoreversion
;
Source: "{#RootSRC}\AV-Help\target\x86_64-pc-windows-gnu\release\av_help.dll"; DestDir: "{app}"; Flags: ignoreversion
;
;Source: "{#RootSRC}\AVBible\Installation\Prerequisites\{#DotnetInstaller}"; DestDir: {tmp}; Flags: deleteafterinstall; AfterInstall: InstallFramework

; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

