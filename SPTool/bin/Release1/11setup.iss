; �ű��� Inno Setup �ű��� ���ɣ�
; �йش��� Inno Setup �ű��ļ�����ϸ��������İ����ĵ���

#define MyAppName "SPTool"
#define MyAppVersion "2.0"
#define MyAppPublisher "SPTool"
#define MyAppExeName "SPTool.exe"


[Setup]
; ע: AppId��ֵΪ������ʶ��Ӧ�ó���
; ��ҪΪ������װ����ʹ����ͬ��AppIdֵ��
; (�����µ�GUID����� ����|��IDE������GUID��)
AppId={{51950893-2D6B-4F26-8C07-A34A624C0106}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName=C:\{#MyAppName}
DefaultGroupName={#MyAppName}
OutputDir=C:\
OutputBaseFilename=SPToolSetup
SetupIconFile=D:\��������\����\ɽ���ӱ�\���հ汾\DataPost2019123001\SPTool\bin\Release1\a.ico
Compression=lzma
SolidCompression=yes

[Languages]
Name: "chinesesimp"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked; OnlyBelowVersion: 0,6.1

[Files]
Source: "D:\��������\����\ɽ���ӱ�\���հ汾\DataPost2019123001\SPTool\bin\Release1\SPTool.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\��������\����\ɽ���ӱ�\���հ汾\DataPost2019123001\SPTool\bin\Release1\db.sql"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\��������\����\ɽ���ӱ�\���հ汾\DataPost2019123001\SPTool\bin\Release1\db.bat"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\��������\����\ɽ���ӱ�\���հ汾\DataPost2019123001\SPTool\bin\Release1\sqlite3.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\��������\����\ɽ���ӱ�\���հ汾\DataPost2019123001\SPTool\bin\Release1\Macrosage.RabbitMQ.Server.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\��������\����\ɽ���ӱ�\���հ汾\DataPost2019123001\SPTool\bin\Release1\RabbitMQ.Client.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\��������\����\ɽ���ӱ�\���հ汾\DataPost2019123001\SPTool\bin\Release1\RabbitMQ.Core.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\��������\����\ɽ���ӱ�\���հ汾\DataPost2019123001\SPTool\bin\Release1\sqlite3.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\��������\����\ɽ���ӱ�\���հ汾\DataPost2019123001\SPTool\bin\Release1\C5.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\��������\����\ɽ���ӱ�\���հ汾\DataPost2019123001\SPTool\bin\Release1\ICSharpCode.SharpZipLib.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\��������\����\ɽ���ӱ�\���հ汾\DataPost2019123001\SPTool\bin\Release1\Common.Logging.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\��������\����\ɽ���ӱ�\���հ汾\DataPost2019123001\SPTool\bin\Release1\Newtonsoft.Json.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\��������\����\ɽ���ӱ�\���հ汾\DataPost2019123001\SPTool\bin\Release1\Quartz.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\��������\����\ɽ���ӱ�\���հ汾\DataPost2019123001\SPTool\bin\Release1\DBUtility.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\��������\����\ɽ���ӱ�\���հ汾\DataPost2019123001\SPTool\bin\Release1\DataBase.db"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\��������\����\ɽ���ӱ�\���հ汾\DataPost2019123001\SPTool\bin\Release1\System.Core.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\��������\����\ɽ���ӱ�\���հ汾\DataPost2019123001\SPTool\bin\Release1\SOTool.BLL.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\��������\����\ɽ���ӱ�\���հ汾\DataPost2019123001\SPTool\bin\Release1\SPTool.DAL.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\��������\����\ɽ���ӱ�\���հ汾\DataPost2019123001\SPTool\bin\Release1\SPTool.Model.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "D:\��������\����\ɽ���ӱ�\���հ汾\DataPost2019123001\SPTool\bin\Release1\System.Data.SQLite.DLL"; DestDir: "{app}"; Flags: ignoreversion
; ע��: ��Ҫ���κι���ϵͳ�ļ���ʹ�á�Flags: ignoreversion��

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[code]
[Code]
//�ؼ����뾲Ĭ��װ
procedure InitializeWizard();
begin
  //����ʾ�߿��������ܴﵽ������������
  WizardForm.BorderStyle:=bsNone;
end; 
procedure CurPageChanged(CurPageID: Integer);
begin
 //��Ϊ��װ���̽������ز��ˣ��������ô��ڿ��Ϊ0
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

