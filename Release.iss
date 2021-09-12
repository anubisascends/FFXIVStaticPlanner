#define AppVer GetVersionNumbersString("FFXIVStaticPlanner\bin\Debug\net5.0-windows\FFXIVStaticPlanner.exe")

[Files]
Source: "FFXIVStaticPlanner\bin\Debug\net5.0-windows\FFXIVStaticPlanner.Core.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "FFXIVStaticPlanner\bin\Debug\net5.0-windows\FFXIVStaticPlanner.Data.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "FFXIVStaticPlanner\bin\Debug\net5.0-windows\FFXIVStaticPlanner.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "FFXIVStaticPlanner\bin\Debug\net5.0-windows\FFXIVStaticPlanner.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "FFXIVStaticPlanner\bin\Debug\net5.0-windows\FFXIVStaticPlanner.Views.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "FFXIVStaticPlanner\bin\Debug\net5.0-windows\Metadata.xml"; DestDir: "{app}"; Flags: ignoreversion
Source: "FFXIVStaticPlanner\bin\Debug\net5.0-windows\SharpVectors.Converters.Wpf.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "FFXIVStaticPlanner\bin\Debug\net5.0-windows\SharpVectors.Core.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "FFXIVStaticPlanner\bin\Debug\net5.0-windows\SharpVectors.Css.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "FFXIVStaticPlanner\bin\Debug\net5.0-windows\SharpVectors.Dom.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "FFXIVStaticPlanner\bin\Debug\net5.0-windows\SharpVectors.Model.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "FFXIVStaticPlanner\bin\Debug\net5.0-windows\SharpVectors.Rendering.Gdi.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "FFXIVStaticPlanner\bin\Debug\net5.0-windows\SharpVectors.Rendering.Wpf.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "FFXIVStaticPlanner\bin\Debug\net5.0-windows\SharpVectors.Runtime.Wpf.dll"; DestDir: "{app}"; Flags: ignoreversion

[Dirs]
Name: "{app}\Images"

[Setup]
DefaultDirName=FFXIVStaticPlanner
AppName=FFXIV Static Planner
AppVersion={#AppVer}
RestartIfNeededByRun=False
AppPublisher=Andrew Riebe
DisableDirPage=no
OutputBaseFilename=FFXIVStaticPlanner_v{#AppVer}