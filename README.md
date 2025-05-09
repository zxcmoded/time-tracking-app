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

<img width="530" alt="image" src="https://github.com/user-attachments/assets/d121f030-1b98-40c9-85c1-2e271f655708" />

<img width="1022" alt="image" src="https://github.com/user-attachments/assets/9985ca21-58b9-4808-8c01-f5f24d6d04fa" />

<img width="660" alt="image" src="https://github.com/user-attachments/assets/a568292b-7443-4253-9096-af44dddf64fd" />
