[![.NET](https://github.com/fosouzadev/fosouzadev-customers/actions/workflows/dotnet.yml/badge.svg)](https://github.com/fosouzadev/fosouzadev-customers/actions/workflows/dotnet.yml)

## Domínio
Essa aplicação gerencia um cadastro simples de clientes.

## Arquitetura
Utiliza a [Onion Architeture](https://jeffreypalermo.com/2008/07/the-onion-architecture-part-1/).

## Tecnologias e ferramentas utilizadas
* [C# .Net 8](https://dotnet.microsoft.com/pt-br/download/dotnet/8.0)
* [Visual Studio 2022](https://visualstudio.microsoft.com/pt-br/vs/)
* [NoSQLBooster for MongoDB](https://nosqlbooster.com/downloads)
* [MongoDB](https://www.mongodb.com/)
* [Docker](https://www.docker.com/) para o banco de dados
	* Utilize o seguinte comando para criar um banco de dados de teste:
`docker run --name mongodb -d -p 27017:27017 -e MONGO_INITDB_ROOT_USERNAME=test -e MONGO_INITDB_ROOT_PASSWORD="Abc1234" -e MONGO_INITDB_DATABASE="WebApiCustomers" mongo:latest`
	* A string de conexão seria a seguinte: `mongodb://test:Abc1234@localhost:27017`

## Testes
Possui três níveis de testes:
* Unitários
* Integração
* Funcionais

Utiliza a biblioteca [Testcontainers](https://dotnet.testcontainers.org/) nos testes de integração e funcionais.