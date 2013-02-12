msbuild ../SQLGeneration.sln /p:Configuration=Release
nuget pack ../SQLGeneration/SQLGeneration.csproj -Properties Configuration=Release
nuget push *.nupkg
del *.nupkg