# Example of a Console App That Watches an Azure Service Bus Queue

This is a simple example of an application that watches and reacts to an Azure Service Bus queue.  

You will need to either add an appsettings.config that looks like this:
```json
{
    "ServiceBus": {
        "ConnectionString": "Endpoint=sb://yoursb.servicebus.windows.net/;SharedAccessKeyName=yourlistener;SharedAccessKey=xyz",
        "Queue": "yourqueue"
    }
}
```

Or, you can (and probably should) use [dotnet user-secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-8.0&tabs=windows) to ensure you don't accidentally share your connection string details.

I'm using Visual Studio Code for development, so I have the following files in my .vscode folder next to my .sln file to simplify debugging locally:

launch.json
```json
{
    "version": "0.2.0",
    "configurations": [
        {
            "name": ".NET Core Launch (console)",
            "type": "coreclr",
            "request": "launch",
            "preLaunchTask": "build",
            "program": "${workspaceFolder}/src/ASBDemo.Processor/bin/Debug/net8.0/ASBDemo.Processor.dll",
            "args": [],
            "cwd": "${workspaceFolder}",
            "stopAtEntry": false,
            "console": "internalConsole",
            "logging": {
                "diagnosticsLog": {
                    "path": "${workspaceFolder}/bin/Debug/net8.0/diagnostic.log",
                    "level": "verbose"
                },
            },
            "internalConsoleOptions": "openOnSessionStart",
            "env": {
                "DOTNET_USE_POLLING_FILE_WATCHER": "1",
                "DOTNET_ENVIRONMENT": "Development"
            }
        }
    ]
}
```

and 

tasks.json

```json
{
    "version": "2.0.0",
    "tasks": [
        {
            "label": "build",
            "command": "dotnet",
            "type": "process",
            "args": [
                "build",
                "${workspaceFolder}/src/ASBDemo.Processor/ASBDemo.Processor.csproj"
            ],
            "problemMatcher": "$msCompile"
        }
    ]
}
```