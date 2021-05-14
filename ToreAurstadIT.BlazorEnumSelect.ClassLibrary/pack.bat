echo Building the library and packing it. Remember to increase the version attribute value 
echo of the csproj before pushing to nuget

dotnet build  --configuration Release


dotnet pack --configuration Release