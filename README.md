# Project Manager API

API REST desenvolvida em .NET 8 para gerenciamento de projetos e tarefas.

## 🚀 Tecnologias

- .NET 8
- SqlServer
- Docker
- Entity Framework Core

## 📋 Pré-requisitos

- [Docker](https://www.docker.com/products/docker-desktop/)
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (opcional, apenas para desenvolvimento)

## 🏃 Como Executar

1. Clone o repositório

2. Execute os containers com Docker Compose
  2.1 Em um terminal ou powershell execute o comando abaixo dentro do diretório raiz da solução
  
  cd {Diretorio raiz onde o repositório foi clonado}\Project.Manager
  
  docker-compose up -d

3. Rodando as migrações
  3.1 Abra a solução (Project.Manager.sln) no Visual Studio (Preferencialmente 2022)
  3.2 Coloque o projeto presentation/Project.Manager.WebApi como Startup Project e abra o Package Manager Console
  3.3 No terminal execute os comandos abaixo
  
  $env:SQL_SERVER_CONNECTION_STRING='Server=(local);Database=ProjectManager;User Id=SA;Password=docker@sql01;TrustServerCertificate=True;Encrypt=False;'
  
  Update-Database -Context SqlServerDbContext


4. Acesse os serviços:
- API: http://localhost:8080/swagger

## 🛑 Para Parar a Aplicação

cd {Diretorio raiz onde o repositório foi clonado}\Project.Manager

docker-compose down

## 📝 Licença

Este projeto está sob a licença MIT.