# Project Manager API

API REST desenvolvida em .NET 8 para gerenciamento de projetos e tarefas.

## 🚀 Tecnologias

- .NET 8
- SQL Server
- Docker
- Entity Framework Core

## 📋 Pré-requisitos

- [Docker](https://www.docker.com/products/docker-desktop/)
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (opcional, apenas para desenvolvimento)

## 🏃 Como Executar

1. Clone o repositório

2. Execute os containers com Docker Compose  
   2.1. Em um terminal ou PowerShell, execute os comandos abaixo dentro do diretório raiz da solução:

   ```bash
   cd {Diretorio raiz onde o repositório foi clonado}\Project.Manager
   docker-compose up -d
   ```

3. Rodando as migrações  
   3.1. Abra a solução `Project.Manager.sln` no Visual Studio (preferencialmente 2022)  
   3.2. Coloque o projeto `presentation/Project.Manager.WebApi` como *Startup Project*  
   3.3. No Package Manager Console, execute os comandos abaixo:

   ```powershell
   $env:SQL_SERVER_CONNECTION_STRING='Server=(local);Database=ProjectManager;User Id=SA;Password=docker@sql01;TrustServerCertificate=True;Encrypt=False;'

   Update-Database -Context SqlServerDbContext
   ```

4. Acesse os serviços:
   - API: [http://localhost:8080/swagger](http://localhost:8080/swagger)

## 🛑 Para Parar a Aplicação

```bash
cd {Diretorio raiz onde o repositório foi clonado}\Project.Manager
docker-compose down
```

## 📝 Licença

Este projeto está sob a licença MIT.
```
