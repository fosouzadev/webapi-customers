## Domínio
Essa aplicação gerencia um cadastro simples de clientes.

## Arquitetura
Utiliza a [Onion Architeture](https://jeffreypalermo.com/2008/07/the-onion-architecture-part-1/).

## Tecnologias utilizadas
* C# .Net 8
* Docker para o banco de dados MongoDB

## Testes
Possui três níveis de testes:
* Unitários
* Integração
* Funcionais

Utiliza a biblioteca [Testcontainers](https://dotnet.testcontainers.org/) nos testes de integração e funcionais.