## Getting started

To get this project up and running as is, feel free to follow these steps:

### Prerequisites

- Install the .NET 8 
- Install Docker Desktop

### Setup

1. Clone this repository
2. Make sure Docker Desktop is running
3. At the root directory, restore required packages by running:

```
dotnet restore
```

4. Still at the root directory, start Docker Containers from YAML file by running:

```
docker-compose
```

5. Navigate into the `\App.Infrastructure` directory and run the following command:

```
dotnet ef database update -c ApplicationDataContext -s App.Api -p App.Infrastructure 
dotnet ef database update -c ApplicationIdentityContext -s App.Api -p App.Infrastructure 
```

6. Next, build the solution by running:

```
dotnet build
```

7. Get "your-secret-token-key" - \App.Api\App.Api.csproj in section \<UserSecretsId> and run:

```
dotnet user-secrets set "TokenKey" "your-secret-token-key"  --project App.Api

//to get secrets list use
dotnet user-secrets list --project App.Api
```

8. Once done, launch the application by running:

```
dotnet run --project App.Api
```

9. Launch http://localhost:5217/swagger/index.html in your browser to view the Swagger documentation of your API.

10. Use Swagger, Postman or any other application to send a request to http://localhost:5217/users/login to login. See default users in App.Infrastructure\Persistence\Seed\IdentitySeed.cs 
