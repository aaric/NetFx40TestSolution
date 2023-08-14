# NetFx40TestSolution

[![C#](https://img.shields.io/badge/C%23-4.0-brightgreen.svg?style=flat&logo=csharp)](https://learn.microsoft.com/zh-cn/dotnet/csharp/programming-guide)
[![.NET Framework](https://img.shields.io/badge/.NET_Framework-4.0-brightgreen.svg?style=flat&logo=.net)](https://dotnet.microsoft.com/zh-cn/download/dotnet-framework)
[![Release](https://img.shields.io/badge/Release-1.0.0-blue.svg)](https://github.com/aaric/NetFx40TestSolution/releases)

> .NET Framework 4.0 Sample.

## [AutoUpdater.NET](https://www.nuget.org/packages/AutoUpdater.NET.CredentialsFix)

1. AutoUpdate.xml

```xml
<?xml version="1.0" encoding="UTF-8"?>
<item>
    <version>1.0.0.2</version><!-- 升级版本 -->
    <url>http://127.0.0.1:8080/vs2013/test/AutoUpdaterTest.zip</url><!-- 升级包下载路径 -->
    <changelog>http://127.0.0.1:8080/vs2013/test/AutoUpdateChangeLog.html</changelog><!-- 升级日志 -->
    <mandatory mode="2">false</mandatory><!-- 是否强制升级：false-否，true-是 -->
    <!-- certutil.exe -hashfile AutoUpdaterTest.zip MD5 --><!-- 升级包校验 -->
    <!--<checksum algorithm="MD5">566e34284c5d49b1b377db8de0ee3ce9</checksum>-->
    <!-- certutil.exe -hashfile AutoUpdaterTest.zip SHA256 --><!-- 升级包校验 -->
    <checksum algorithm="SHA256">01f7d512f62b5f654e9796ac952f8912cd4f78c589d42089f51373193d9f1f09</checksum>
</item>
```

2. AutoUpdateChangeLog.html

```html
<!doctype html>
<html lang="zh">
<head>
    <meta charset="UTF-8">
    <title>Auto Update Change Log</title>
</head>
<body>
    <h1>这是一个重要的更新，从1.0.0.1到1.0.0.2</h1>
</body>
</html>
```

3. AutoUpdaterTest.zip

```powershell
# md5
certutil.exe -hashfile AutoUpdaterTest.zip MD5

# sha256
certutil.exe -hashfile AutoUpdaterTest.zip SHA256
```


