# Project Name to build and pack
$Projects = @(
    "Gaspra.Logging.Serializer",
    "Gaspra.Logging.Builder",
    "Gaspra.Logging.Provider",
    "Gaspra.Logging.Provider.Fluentd",
    "Gaspra.Logging.Provider.Console",
    "Gaspra.Logging.Provider.File"
    )
$Version = "1.0.0-local"

#Restore paket
dotnet tool restore
dotnet paket restore

# Pack each project in the Projects list
ForEach ($Project in $Projects)
{
    $ProjectPath = Get-ChildItem -Path . -Filter "$Project.csproj" -Recurse | %{$_.FullName}
    Write-Host $ProjectPath
    dotnet pack $ProjectPath -c Release -o ./.pack /p:Version=$Version
}