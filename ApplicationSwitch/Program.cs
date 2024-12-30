using ApplicationSwitch;
using ApplicationSwitch.Lib;
using ApplicationSwitch.Lib.Rules;
using ApplicationSwitch.Lib.Yml;
using ApplicationSwitch.Test;
using YamlDotNet.Serialization;


var path3 = @"..\..\..\Test\AppConfig03.yml";
//var app = AppRoot.Load(path2);
//app.Show();

//sample_AppConfig002.Test01();
//DataSerializer.Save(path3, app);


//var app = DataSerializer.Load<AppRoot>(path2);
//DataSerializer.Show(app);



var app = DataSerializer.Load<AppRoot>(path3);
DataSerializer.Show(app);

app.ProcessFromRule();


//sample_AppConfig003.Test01();







Console.ReadLine();
