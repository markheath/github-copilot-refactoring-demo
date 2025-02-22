dotnet new nunit -n GloboticketWeb.Tests
dotnet sln GloboticketWeb.sln add GloboticketWeb.Tests/GloboticketWeb.Tests.csproj
dotnet add GloboticketWeb.Tests/GloboticketWeb.Tests.csproj reference GloboticketWeb/GloboticketWeb.csproj
