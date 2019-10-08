rmdir .pack /s /q
cd src
dotnet pack -c Release -o ../.pack
cd ..