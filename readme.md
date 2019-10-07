# Gaspra.Logging

Designed and created to easily allow you more control over your logging. Providing
the ability to use the dotnet core logging framework and control how logs and their relevant information are serialized.

---

### Config

```
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug"
    },
    "Providers": {
      "Fluentd": {
        "Host": ,
        "Port": ,
        "DisconnectTime": 5
      }
    },
    "ApplicationInformation": {
      "system": "Internal",
      "instance": "Gaspra.Default.Sample",
      "team": {
        "developers": [
          "Gaspra"
        ],
        "name": "Logging"
      }
    }
  }
```
