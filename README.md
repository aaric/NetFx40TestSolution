# NetFx40TestSolution

[![C#](https://img.shields.io/badge/C%23-4.0-brightgreen.svg?style=flat&logo=csharp)](https://learn.microsoft.com/zh-cn/dotnet/csharp/programming-guide)
[![.NET Framework](https://img.shields.io/badge/.NET_Framework-4.0-brightgreen.svg?style=flat&logo=.net)](https://dotnet.microsoft.com/zh-cn/download/dotnet-framework)
[![Release](https://img.shields.io/badge/Release-1.4.0-blue.svg)](https://github.com/aaric/NetFx40TestSolution/releases)

> .NET Framework 4.0 Sample.

## 1 [Autoupdater.NET.Official](https://github.com/ravibpatel/AutoUpdater.NET/tree/v1.6.4)

### 1.1 AutoUpdate.xml | AutoUpdate.json

```xml
<?xml version="1.0" encoding="UTF-8"?>
<item>
    <!-- 升级版本 -->
    <version>1.2.2.0</version>
    <!-- 升级包下载路径 -->
    <url>http://10.0.11.25:8021/vs2013/test/AutoUpdaterTest.zip</url>
    <!-- 升级日志 -->
    <changelog>http://10.0.11.25:8021/vs2013/test/AutoUpdaterChangeLog.html</changelog>
    <!-- 是否强制升级：false-否，true-是 -->
    <!-- 模式属性：1-隐藏更新对话框上的“关闭”按钮，2-跳过更新对话框并自动开始下载和更新应用程序 -->
    <mandatory mode="2" minVersion="1.0.0.0">false</mandatory>
    <!-- 升级包校验：certutil.exe -hashfile AutoUpdaterTest.zip MD5 -->
    <!--<checksum algorithm="MD5">51603a7ee91eb8e5410a2502481a4dda</checksum>-->
    <!-- 升级包校验：certutil.exe -hashfile AutoUpdaterTest.zip SHA256 -->
    <checksum algorithm="SHA256">2998416ccd3a090db274fc9b32ee5157849636638a56342cce9d6fa1006e75a3</checksum>
</item>
```

&emsp;&emsp;*由于json方式实现了`ParseUpdateInfoEvent`，可以引入`BaseUri`，使用相对路径。*

```json
{
  "version": "1.2.2.0",
  "url": "/vs2013/test/AutoUpdaterTest.zip",
  "changelog": "/vs2013/test/AutoUpdaterChangeLog.html",
  "mandatory": {
    "mode": 1,
    "minVersion": "1.0.0.0",
    "value": false
  },
  "checksum": {
    "hashingAlgorithm": "SHA256",
    "value": "2998416ccd3a090db274fc9b32ee5157849636638a56342cce9d6fa1006e75a3"
  }
}
```

### 1.2 AutoUpdateChangeLog.html

```html
<!DOCTYPE html>
<html lang="zh">
<head>
    <meta charset="UTF-8">
    <title>Auto Updater Change Log</title>
</head>
<body>
<h3>这是一个重要的更新！！！</h3>
</body>
</html>
```

### 1.3 AutoUpdaterTest.zip

```powershell
# md5
certutil.exe -hashfile AutoUpdaterTest.zip MD5

# sha256
certutil.exe -hashfile AutoUpdaterTest.zip SHA256
```

## 2 [Microsoft Visual Studio 2013 Installer Projects](https://marketplace.visualstudio.com/items?itemName=UnniRavindranathan-MSFT.MicrosoftVisualStudio2013InstallerProjects)

&emsp;&emsp;*略。*
