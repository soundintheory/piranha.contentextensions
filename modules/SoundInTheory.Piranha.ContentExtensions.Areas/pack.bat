:: Clean and build in release
dotnet restore
dotnet clean
dotnet build -c Release

:: Create NuGet package
dotnet pack SoundInTheory.Piranha.ContentExtensions.Areas.csproj --no-build -c Release -o ./.nuget