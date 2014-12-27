nuget pack ../SQLGeneration/SQLGeneration.nuspec
nuget push *.nupkg
del *.nupkg

