Project Setup

- Dependencies
  .net core 7
  node js 16.20.0
  Angular CLI: 14.1.3
  SQL Server

- Run the application
  WEB
  - open the web folder
  - type "npm-install --legacy-peer-deps"
  - ng serve
 
  API
  - open the tracking-api/api folder
  - type "dotnet restore"
  - in appsettings.json change the connectionstring based on your DB constring
  - type "dotnet ef database update"
  - type "dotnet watch run"

  browse the application
  http://localhost:4200
  - username: admin
  - password: 123456
