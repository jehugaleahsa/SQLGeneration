nuget pack ../SQLGeneration/SQLGeneration.csproj -Prop Configuration=Release -Build
nuget push *.nupkg
del *.nupkg