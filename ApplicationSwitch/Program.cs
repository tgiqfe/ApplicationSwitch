using ApplicationSwitch;
using ApplicationSwitch.Lib;
using ApplicationSwitch.Test;
using System.Diagnostics;

//  Start process
Environment.CurrentDirectory = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
var setting = DataSerializer.Load<Setting>("setting.yml");
if (!Directory.Exists(setting.LogDirectory))
{
    Directory.CreateDirectory(setting.LogDirectory);
}
Logger.LogFile =
    Path.Combine(setting.LogDirectory, "AppSwitch_" + Path.Combine(DateTime.Now.ToString("yyyyMMdd")) + ".log");
Logger.WriteLine("Start Application switch.", 0);


//  Main process
/*
var path = @"..\..\..\Test\AppConfig04.yml";
var app = DataSerializer.Load<AppRoot>(path);
app.ProcessRules(setting.EvacuateDirectory);
*/

sample_AppConfig003.Test02("ping localhost -n 5");
sample_AppConfig003.Test02(@"net use \\localhost\Share$");
sample_AppConfig003.Test02(@"powershell -File ""C:\Temp\Sample\App\setup.ps1""");
sample_AppConfig003.Test02(@"""C:\Temp\aa bb\cc\dd e\test.exe"" arg1 arg2 arg3");


//  End process
Logger.WriteLine("End Application switch.", 0);

#if DEBUG
Console.ReadLine();
#endif