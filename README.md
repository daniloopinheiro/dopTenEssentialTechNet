<div align="center">
   <h1>Dez tecnologias essenciais em .NET</h1>

  [![CI - Build & Test](https://github.com/dop-s/dopTenEssentialTechNet/actions/workflows/ci.yml/badge.svg)](https://github.com/dop-s/dopTenEssentialTechNet/actions/workflows/ci.yml)
  [![Dependencies Check & Update](https://github.com/dop-s/dopTenEssentialTechNet/actions/workflows/dependencies.yml/badge.svg)](https://github.com/dop-s/dopTenEssentialTechNet/actions/workflows/dependencies.yml)
  [![Monitoring & Performance](https://github.com/dop-s/dopTenEssentialTechNet/actions/workflows/monitoring.yml/badge.svg)](https://github.com/dop-s/dopTenEssentialTechNet/actions/workflows/monitoring.yml)
  [![CD - Deploy](https://github.com/dop-s/dopTenEssentialTechNet/actions/workflows/cd.yml/badge.svg)](https://github.com/dop-s/dopTenEssentialTechNet/actions/workflows/cd.yml)
  [![Auto Tag & Version](https://github.com/dop-s/dopTenEssentialTechNet/actions/workflows/auto-tag.yml/badge.svg)](https://github.com/dop-s/dopTenEssentialTechNet/actions/workflows/auto-tag.yml)

  ![Licença](https://img.shields.io/github/license/dop-s/dopTenEssentialTechNet)

   Repositório com <strong>projetos .NET 10</strong> (console, APIs web e testes) que exploram técnicas e bibliotecas recorrentes no ecossistema .NET moderno.
</div>

<br>

## Índice

1. [Visão geral](#visão-geral)
2. [Projetos na solução](#projetos-na-solução)
3. [Requisitos](#requisitos)
4. [Instalação e build](#instalação-e-build)
5. [Como executar um projeto](#como-executar-um-projeto)
6. [CI/CD e workflows](#cicd-e-workflows)
7. [Documentação adicional](#documentação-adicional)
8. [Contribuições](#contribuições)
9. [Artigos e conteúdos](#artigos-e-conteúdos)
10. [Licença](#licença)
11. [Contato](#contato)

## Visão geral

O **dopTenEssentialTechNet** agrupa vários projetos independentes, todos direcionados ao **.NET 10** (`net10.0`). A maior parte são **aplicativos de console**; há também **APIs minimais** (por exemplo, FastEndpoints ou EF Core com HTTP) quando o tema pede hospedagem web. Cada pasta corresponde a um tema (`Span`, configuração, resiliência HTTP, testes com containers, pooling de `DbContext`, logging gerado em compile time, entre outros), para estudo, artigos ou referência rápida.

A solução principal está em `dopTenEssentialTechNet.sln` na raiz do repositório.

## Projetos na solução

| Projeto | Tema (foco) |
|---------|-------------|
| **SubStringSpan** | Strings e `Span`/`Memory` para código eficiente e com menos alocações |
| **OptionsConfig** | Padrão Options e `IConfiguration` |
| **BackgroundServer** | Serviços em segundo plano com `Host` / hospedagem genérica |
| **WorkflowCore** | Orquestração de fluxos com WorkflowCore |
| **VerticalSliceFastEndpoints** | Organização vertical slice com FastEndpoints |
| **HTTPolly** | Chamadas HTTP com políticas de resiliência (Polly) |
| **Flurl** | Cliente HTTP fluente com Flurl |
| **TddContainers** | Testes de integração com Testcontainers |
| **DynamicDapper** | Acesso a dados com Dapper e cenários dinâmicos |
| **ContextPool** | EF Core com `AddDbContextPool`, SQL Server e Minimal API |
| **SourceGeneratedLogging** | Logging com `[LoggerMessage]` (source generator de alto desempenho) |

## Requisitos

- [.NET SDK 10](https://dotnet.microsoft.com/download) (TFM `net10.0`)

Verifique a versão instalada:

```bash
dotnet --version
```

## Instalação e build

```bash
git clone https://github.com/daniloopinheiro/dopTenEssentialTechNet
cd dopTenEssentialTechNet
dotnet restore
dotnet build
```

Para compilar apenas a solução:

```bash
dotnet build dopTenEssentialTechNet.sln
```

## Como executar um projeto

Substitua `NomeDoProjeto` por uma das pastas listadas acima (por exemplo, `SubStringSpan`):

```bash
dotnet run --project NomeDoProjeto/NomeDoProjeto.csproj
```

Na pasta **WorkflowCore** o arquivo de projeto é `WorkflowCore.App.csproj` (nome escolhido para não conflitar com o pacote NuGet **WorkflowCore**).

Na pasta **Flurl** o arquivo é `FlurlDemo.App.csproj` (o nome **Flurl** conflita com o pacote NuGet **Flurl** usado pelo **Flurl.Http**).

O projeto **TddContainers** é de **testes** (xUnit + Testcontainers). Execute com `dotnet test` — é necessário **Docker** em execução para subir o PostgreSQL em container.

**APIs web (Minimal API / ASP.NET Core):**

| Projeto | URL predefinida (desenvolvimento) |
|---------|-----------------------------------|
| **VerticalSliceFastEndpoints** | `http://localhost:5088` |
| **ContextPool** | `http://localhost:5089` |

**SQL Server (LocalDB ou instância própria):**

- **DynamicDapper** — connection string em `ConnectionStrings:Default` (`appsettings.json`).
- **ContextPool** — `ConnectionStrings:DefaultConnection`; o arranque cria a base e insere dados de exemplo. Ajuste a string se não usar LocalDB.

**Logging com source generator:**

- **SourceGeneratedLogging** — `dotnet run` executa uma chamada gerada em compile time (`[LoggerMessage]`) e imprime na consola (nível Debug).

Exemplo:

```bash
dotnet run --project SubStringSpan/SubStringSpan.csproj
```

## CI/CD e workflows

O repositório inclui workflows em [`.github/workflows/`](.github/workflows/) para integração contínua, dependências, monitoramento, deploy e versionamento. Resumo:

| Workflow | Arquivo | Função principal |
|----------|---------|------------------|
| CI - Build & Test | [`ci.yml`](.github/workflows/ci.yml) | Build, testes e verificações de qualidade |
| CD - Deploy | [`cd.yml`](.github/workflows/cd.yml) | Entrega e artefatos de deploy |
| Dependencies Check & Update | [`dependencies.yml`](.github/workflows/dependencies.yml) | Dependências e atualizações |
| Monitoring & Performance | [`monitoring.yml`](.github/workflows/monitoring.yml) | Monitoramento e performance |
| PR Automation | [`pr-automation.yml`](.github/workflows/pr-automation.yml) | Automação de pull requests |
| Auto Tag & Version | [`auto-tag.yml`](.github/workflows/auto-tag.yml) | Tags e versionamento |
| Auto Release | [`auto-release.yml`](.github/workflows/auto-release.yml) | Releases automáticas |
| Create Version Tags | [`create-tags.yml`](.github/workflows/create-tags.yml) | Criação de tags de versão |
| Create Release | [`release.yml`](.github/workflows/release.yml) | Criação de releases |

Documentação complementar:

- [README dos workflows](.github/README_WORKFLOWS.md)
- [Início rápido](.github/QUICKSTART.md)
- [Versionamento automático](.github/AUTO_VERSIONING.md)
- [Guia de release](.github/RELEASE_GUIDE.md)
- [Badges](.github/BADGES.md)

## Documentação adicional

- [Artigo](ARTICLE.md) — contexto editorial sobre o repositório
- [CHANGELOG](CHANGELOG.md)
- [Segurança](SECURITY.md)
- [Contribuição](CONTRIBUTING.md)

## Contribuições

Contribuições são bem-vindas. Consulte [CONTRIBUTING.md](CONTRIBUTING.md) e o [código de conduta](CODE_OF_CONDUCT.md). Para bugs e ideias, use [Issues](https://github.com/dop-s/dopTenEssentialTechNet/issues).

## Artigos e conteúdos

* [LinkedIn](https://www.linkedin.com/in/daniloopinheiro)
* [Medium](https://medium.com/@daniloopinheiro)
* [Dev.to](https://dev.to/daniloopinheiro)
* [Substack](https://substack.com/@daniloopinheiro)

## Licença

MIT — veja [LICENSE.md](LICENSE.md).

## Contato

### Suporte técnico

- **Issues**: [GitHub Issues](https://github.com/daniloopinheiro/dopTenEssentialTechNet/issues)

### Autor

**Danilo O. Pinheiro**

- **Email pessoal**: [daniloopro@gmail.com](mailto:daniloopro@gmail.com)
- **Email empresarial**: [devsfree@devsfree.com.br](mailto:devsfree@devsfree.com.br)
- **Consultoria**: [contato@dopme.io](mailto:contato@dopme.io)
- **LinkedIn**: [Danilo O. Pinheiro](https://www.linkedin.com/in/daniloopinheiro/)

### Empresas

- **[DevsFree](https://devsfree.com.br)**: comunidade de desenvolvedores
- **[dopme.io](https://dopme.io)**: consultoria e soluções tecnológicas

<div align="center">

<p>
Feito com cuidado por <strong>Danilo O. Pinheiro</strong><br/>
<a href="https://devsfree.com.br" target="_blank">DevsFree</a> • <a href="https://dopme.io" target="_blank">dopme.io</a>
</p>

</div>
