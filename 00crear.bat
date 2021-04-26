
dotnet new web 		-n Inventario.DataAccess
dotnet new web 		-n Inventario.Core
dotnet new blazorserver	-n Inventario.Web
dotnet new xunit	-n Inventario.DataAccess.Tests
dotnet new xunit	-n Inventario.Core.Tests
dotnet new xunit	-n Inventario.Web.Tests

dotnet new sln 		-n Inventario

dotnet sln add Inventario.DataAccess
dotnet sln add Inventario.Core
dotnet sln add Inventario.Web
dotnet sln add Inventario.DataAccess.Tests
dotnet sln add Inventario.Core.Tests
dotnet sln add Inventario.Web.Tests
