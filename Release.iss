#define AppVer GetVersionNumbersString("FFXIVStaticPlanner\bin\Release\net5.0-windows\FFXIVStaticPlanner.exe")

[Files]
Source: "FFXIVStaticPlanner\bin\Debug\net5.0-windows\FFXIVStaticPlanner.Core.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "FFXIVStaticPlanner\bin\Debug\net5.0-windows\FFXIVStaticPlanner.Data.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "FFXIVStaticPlanner\bin\Debug\net5.0-windows\FFXIVStaticPlanner.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "FFXIVStaticPlanner\bin\Debug\net5.0-windows\FFXIVStaticPlanner.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "FFXIVStaticPlanner\bin\Debug\net5.0-windows\FFXIVStaticPlanner.Views.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "FFXIVStaticPlanner.Data\Metadata - Copy.xml"; DestDir: "{app}"; DestName: "Metadata.xml"; Flags: ignoreversion
Source: "FFXIVStaticPlanner\bin\Debug\net5.0-windows\SharpVectors.Converters.Wpf.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "FFXIVStaticPlanner\bin\Debug\net5.0-windows\SharpVectors.Core.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "FFXIVStaticPlanner\bin\Debug\net5.0-windows\SharpVectors.Css.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "FFXIVStaticPlanner\bin\Debug\net5.0-windows\SharpVectors.Dom.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "FFXIVStaticPlanner\bin\Debug\net5.0-windows\SharpVectors.Model.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "FFXIVStaticPlanner\bin\Debug\net5.0-windows\SharpVectors.Rendering.Gdi.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "FFXIVStaticPlanner\bin\Debug\net5.0-windows\SharpVectors.Rendering.Wpf.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "FFXIVStaticPlanner\bin\Debug\net5.0-windows\SharpVectors.Runtime.Wpf.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "FFXIVStaticPlanner\bin\Release\net5.0-windows\Xceed.Wpf.AvalonDock.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "FFXIVStaticPlanner\bin\Release\net5.0-windows\Xceed.Wpf.AvalonDock.Themes.Aero.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "FFXIVStaticPlanner\bin\Release\net5.0-windows\Xceed.Wpf.AvalonDock.Themes.Metro.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "FFXIVStaticPlanner\bin\Release\net5.0-windows\Xceed.Wpf.AvalonDock.Themes.VS2010.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "FFXIVStaticPlanner\bin\Release\net5.0-windows\Xceed.Wpf.Toolkit.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "FFXIVStaticPlanner\bin\Release\net5.0-windows\cs-CZ\Xceed.Wpf.AvalonDock.resources.dll"; DestDir: "{app}\cs-CZ"
Source: "FFXIVStaticPlanner\bin\Release\net5.0-windows\de\Xceed.Wpf.AvalonDock.resources.dll"; DestDir: "{app}\de"
Source: "FFXIVStaticPlanner\bin\Release\net5.0-windows\es\Xceed.Wpf.AvalonDock.resources.dll"; DestDir: "{app}\es"
Source: "FFXIVStaticPlanner\bin\Release\net5.0-windows\fr\Xceed.Wpf.AvalonDock.resources.dll"; DestDir: "{app}\fr"
Source: "FFXIVStaticPlanner\bin\Release\net5.0-windows\hu\Xceed.Wpf.AvalonDock.resources.dll"; DestDir: "{app}\hu"
Source: "FFXIVStaticPlanner\bin\Release\net5.0-windows\it\Xceed.Wpf.AvalonDock.resources.dll"; DestDir: "{app}\it"
Source: "FFXIVStaticPlanner\bin\Release\net5.0-windows\ja-JP\Xceed.Wpf.AvalonDock.resources.dll"; DestDir: "{app}\ja-JP"
Source: "FFXIVStaticPlanner\bin\Release\net5.0-windows\pt-BR\Xceed.Wpf.AvalonDock.resources.dll"; DestDir: "{app}\pt-BR"
Source: "FFXIVStaticPlanner\bin\Release\net5.0-windows\ro\Xceed.Wpf.AvalonDock.resources.dll"; DestDir: "{app}\ro"
Source: "FFXIVStaticPlanner\bin\Release\net5.0-windows\ru\Xceed.Wpf.AvalonDock.resources.dll"; DestDir: "{app}\ru"
Source: "FFXIVStaticPlanner\bin\Release\net5.0-windows\sv\Xceed.Wpf.AvalonDock.resources.dll"; DestDir: "{app}\sv"
Source: "FFXIVStaticPlanner\bin\Release\net5.0-windows\zh-Hans\Xceed.Wpf.AvalonDock.resources.dll"; DestDir: "{app}\zh-Hans"
Source: "FFXIVStaticPlanner\bin\Release\net5.0-windows\FFXIVStaticPlanner.runtimeconfig.json"; DestDir: "{app}"; Flags: ignoreversion

[Dirs]
Name: "{app}\Images"
Name: "{app}\cs-CZ"
Name: "{app}\de"
Name: "{app}\es"
Name: "{app}\fr"
Name: "{app}\hu"
Name: "{app}\it"
Name: "{app}\ja-JP"
Name: "{app}\ro"
Name: "{app}\ru"
Name: "{app}\sv"
Name: "{app}\zh-Hans"
Name: "{app}\pt-BR"

[Setup]
DefaultDirName={autopf}\FFXIVStaticPlanner
AppName=FFXIV Static Planner
AppVersion={#AppVer}
RestartIfNeededByRun=False
AppPublisher=Andrew Riebe
DisableDirPage=no
OutputBaseFilename=FFXIVStaticPlanner_v{#AppVer}