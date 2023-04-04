; 脚本由 Inno Setup 脚本向导 生成！
; 有关创建 Inno Setup 脚本文件的详细资料请查阅帮助文档！

#define MyAppName "SPTool"
#define MyAppVersion "2.0"
#define MyAppPublisher "SPTool"
#define MyAppExeName "SPTool.exe"


[Setup]
; 注: AppId的值为单独标识该应用程序。
; 不要为其他安装程序使用相同的AppId值。
; (生成新的GUID，点击 工具|在IDE中生成GUID。)
AppId={{51950893-2D6B-4F26-8C07-A34A624C0106}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName=C:\{#MyAppName}
DefaultGroupName={#MyAppName}
OutputDir=C:\
OutputBaseFilename=SPToolSetup
SetupIconFile=D:\工作资料\壳牌\山东河北\最终版本\DataPost2019123001\SPTool\bin\Release1\a.ico
Compression=lzma
SolidCompression=yes

[Languages]
Name: "chinesesimp"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked; OnlyBelowVersion: 0,6.1

[Files]
Source: "D:\工作资料\壳牌\山东河北\最终版本\DataPost2019123001\SPTool\bin\Release1\SPTool.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\工作资料\壳牌\山东河北\最终版本\DataPost2019123001\SPTool\bin\Release1\db.sql"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\工作资料\壳牌\山东河北\最终版本\DataPost2019123001\SPTool\bin\Release1\db.bat"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\工作资料\壳牌\山东河北\最终版本\DataPost2019123001\SPTool\bin\Release1\sqlite3.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\工作资料\壳牌\山东河北\最终版本\DataPost2019123001\SPTool\bin\Release1\Macrosage.RabbitMQ.Server.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\工作资料\壳牌\山东河北\最终版本\DataPost2019123001\SPTool\bin\Release1\RabbitMQ.Client.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\工作资料\壳牌\山东河北\最终版本\DataPost2019123001\SPTool\bin\Release1\RabbitMQ.Core.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\工作资料\壳牌\山东河北\最终版本\DataPost2019123001\SPTool\bin\Release1\sqlite3.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\工作资料\壳牌\山东河北\最终版本\DataPost2019123001\SPTool\bin\Release1\C5.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\工作资料\壳牌\山东河北\最终版本\DataPost2019123001\SPTool\bin\Release1\ICSharpCode.SharpZipLib.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\工作资料\壳牌\山东河北\最终版本\DataPost2019123001\SPTool\bin\Release1\Common.Logging.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\工作资料\壳牌\山东河北\最终版本\DataPost2019123001\SPTool\bin\Release1\Newtonsoft.Json.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\工作资料\壳牌\山东河北\最终版本\DataPost2019123001\SPTool\bin\Release1\Quartz.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\工作资料\壳牌\山东河北\最终版本\DataPost2019123001\SPTool\bin\Release1\DBUtility.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\工作资料\壳牌\山东河北\最终版本\DataPost2019123001\SPTool\bin\Release1\DataBase.db"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\工作资料\壳牌\山东河北\最终版本\DataPost2019123001\SPTool\bin\Release1\System.Core.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\工作资料\壳牌\山东河北\最终版本\DataPost2019123001\SPTool\bin\Release1\SOTool.BLL.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\工作资料\壳牌\山东河北\最终版本\DataPost2019123001\SPTool\bin\Release1\SPTool.DAL.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\工作资料\壳牌\山东河北\最终版本\DataPost2019123001\SPTool\bin\Release1\SPTool.Model.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\工作资料\壳牌\山东河北\最终版本\DataPost2019123001\SPTool\bin\Release1\System.Data.SQLite.DLL"; DestDir: "{app}"; Flags: ignoreversion
; 注意: 不要在任何共享系统文件上使用“Flags: ignoreversion”

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[code]
[Code]
//关键代码静默安装
procedure InitializeWizard();
begin
  //不显示边框，这样就能达到不会闪两下了
  WizardForm.BorderStyle:=bsNone;
end; 
procedure CurPageChanged(CurPageID: Integer);
begin
 //因为安装过程界面隐藏不了，所以设置窗口宽高为0
  WizardForm.ClientWidth := ScaleX(0)
  WizardForm.ClientHeight := ScaleY(0)
if CurPageID = wpWelcome then
WizardForm.NextButton.OnClick(WizardForm);
if CurPageID >= wpInstalling then
    WizardForm.Visible := False
  else
    WizardForm.Visible := True;
 // WizardForm.NextButton.OnClick(WizardForm);
end;

function ShouldSkipPage(PageID: Integer): Boolean;
begin
result := true;
end;
[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: shellexec postinstall skipifsilent

