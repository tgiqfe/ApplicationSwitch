using ApplicationSwitch;
using ApplicationSwitch.Lib;
using ApplicationSwitch.Lib.Rules;
using ApplicationSwitch.Lib.Yml;
using ApplicationSwitch.Test;
using YamlDotNet.Serialization;


var path2 = @"..\..\..\Test\AppConfig02.yml";
var path3 = @"..\..\..\Test\AppConfig03.yml";
//var app = AppRoot.Load(path2);
//app.Show();

//sample_AppConfig002.Test01();

var app = DataSerializer.Load<AppRoot>(path2);
DataSerializer.Show(app);
//DataSerializer.Save(path3, app);





Console.ReadLine();
