# Gaspra.Logging

Designed and created to easily allow you more
control over your logging. Providing the ability
to use the dotnet core logging framework and
control how logs and their relevant information
are serialized.

---

### Config

In order to use the Fluentd provider with default
settings you'll require configuration:

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
    "Properties": {
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
