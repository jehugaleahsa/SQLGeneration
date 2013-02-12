nuget pack ../SQLGeneration/SQLGeneration.csproj -Build -Properties Configuration=Release
nuget push *.nupkg
del *.nupkg