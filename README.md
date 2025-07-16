# BillingService
[![Build Status](https://github.com/JoaoSimino/BillingService/actions/workflows/ci-cd.yml/badge.svg)](https://github.com/JoaoSimino/BillingService/actions/workflows/ci-cd.yml)
[![Tests](https://img.shields.io/badge/tests-passing-brightgreen.svg)]()


## Sumário

- [Descrição](#descrição)
- [Arquitetura e Tecnologias](#arquitetura-e-tecnologias)
- [Funcionalidades](#funcionalidades)
- [Pré-requisitos](#pré-requisitos)
- [Como rodar](#como-rodar)
- [Testes](#testes)
- [Documentação da API](#documentação-da-api)
- [Contribuindo](#contribuindo)
- [Pipeline CI/CD](#pipeline-cicd)
- [Licença](#licença)

## Descrição

BillingService é um microserviço responsável por receber eventos de propostas de crédito aprovadas, emitidos pelo serviço de análise de crédito (CreditScoringEngine), por meio de uma fila RabbitMQ. Ao receber uma proposta, ela entra no sistema com o status FaturaPendente, aguardando a escolha do cliente por uma das opções de pagamento sugeridas.

Essas opções de pagamento são calculadas automaticamente com base no valor da proposta aprovada, considerando regras definidas no próprio serviço. Após a seleção da forma de pagamento pelo cliente, o BillingService gera automaticamente a fatura correspondente e as respectivas parcelas, que ficam prontas para integração com o módulo de pagamentos.

Este microserviço foi desenvolvido com foco em mensageria assíncrona, event-driven architecture, e resiliência em sistemas distribuídos, utilizando práticas modernas com .NET 8, RabbitMQ, EF Core, testes automatizados e CI/CD via GitHub Actions.

## Arquitetura e Tecnologias
Este projeto aplica boas práticas de arquitetura limpa no .NET 8, com foco em testes automatizados, containerização e CI/CD, proporcionando robustez e qualidade para ambientes financeiros.
- .NET 8 Minimal APIs
- Clean Architecture com separação em camadas:
  - **Domain**: entidades e regras de negócio
  - **Application**: serviços e regras de aplicação
  - **Infrastructure**: persistência (EF Core)
  - **API**: endpoints, configuração e bootstrap
- Entity Framework Core com InMemory para testes
- xUnit para testes unitários e de integração
- GitHub Actions para CI/CD

## Funcionalidades

- Gerenciamento de Faturas
    - Cadastro, consulta, atualização e exclusão de faturas (CRUD)
    - Atualização do status da fatura: de FaturaPendente para FaturaPaga
- Gerenciamento de Propostas
    - Cadastro, consulta, atualização e exclusão de propostas aprovadas (CRUD)
    - Persistência dos eventos PropostaAprovadaEvent recebidos via RabbitMQ
- Parcelamento Inteligente
    - Consulta de parcelas vinculadas a uma fatura, utilizando IdFatura ou IdCliente
    - Sugestão automática de opções de pagamento com e sem juros, com base no valor da proposta
- Escolha de Forma de Pagamento
    - Endpoint para buscar as opções de pagamento disponíveis para uma fatura específica
    - Endpoint para informar a escolha do cliente (número de parcelas, com ou sem juros)
- Mensageria Assíncrona
    - Consumo de eventos de propostas aprovadas publicados pelo CreditScoringEngine via RabbitMQ
    - Geração automática de fatura e parcelas ao receber proposta aprovada

- Outras funcionalidades
    - Testes automatizados cobrindo unidades, serviços e endpoints críticos

## Pré-requisitos

- .NET SDK 8.0 instalado (Download)
- SQL Server local ou via container Docker (exemplo da imagem oficial: mcr.microsoft.com/mssql/server:2022-latest)
- RabbitMQ local ou via container Docker
- Visual Studio 2022 ou VS Code recomendado
- Docker instalado (opcional, para rodar container isolado do SQL Server ou da aplicação)

## Como rodar

Clone o repositório:

```bash
git clone https://github.com/JoaoSimino/BillingService.git
cd BillingService
```

Restaurar dependências e rodar a aplicação:

```bash
dotnet restore BillingService.sln
dotnet run --project BillingService.API
```

A API estará disponível em `http://localhost:5000` (ou porta configurada).

## Testes

Para rodar todos os testes:

```bash
dotnet test BillingService.sln --configuration Release --verbosity normal
```

Os testes utilizam banco InMemory para isolamento total.

## Documentação da API

A API está configurada com Swagger UI para facilitar testes e visualização da documentação.  
Acesse `http://localhost:5000/swagger` após rodar a aplicação.

Exemplo de requisição curl para listar Clientes:

```bash
curl -X GET http://localhost:5000/api/Cliente
```

## Contribuindo

Contribuições são bem-vindas! Para contribuir:

1. Fork este repositório
2. Crie uma branch feature com o padrão `feature/nome-da-feature`
3. Faça commits claros e descritivos
4. Abra um Pull Request detalhando as alterações

Por favor, siga o padrão de commits [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/).

## Pipeline CI/CD

O projeto utiliza GitHub Actions para:

- Validar o código a cada push/PR na branch `main`
- Executar testes automaticamente
- Buildar e preparar o pacote para release, e subir ja um container atualizar para o Docker hub. 

O workflow está disponível em `.github/workflows/ci-cd.yml`.

## Licença

Este projeto está licenciado sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

---

Obrigado por usar o BillingService!  
Para dúvidas ou sugestões, abra uma issue ou entre em contato comigo.