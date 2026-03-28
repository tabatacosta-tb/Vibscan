# VendasLux

`VendasLux` é uma API REST em `C#` com `.NET 8` voltada para o domínio de vendas, com suporte ao gerenciamento de `Clientes`, `Produtos` e `Pedidos`.

A solução foi construída com uma arquitetura em camadas para manter as regras de negócio isoladas da interface de apresentação e dos detalhes de infraestrutura, facilitando manutenção, testes e evolução futura.

## Tecnologias e Padrões Arquiteturais

### Tecnologias utilizadas
- `.NET 8`
- `ASP.NET Core Web API`
- `Swagger` para documentação e testes da API
- Injeção de Dependência

### Padrões e princípios aplicados
- Arquitetura Hexagonal
- `SOLID`
- `Clean Code`
- `MVC` com controllers finos
- Padrão `Repository`
- `Domain-Driven Design` básico
- `Agregado` principal em `Pedido`, contendo `ItensPedido`

### Estratégia atual de persistência
Os repositórios utilizam listas estáticas em memória.  
Essa abordagem simplifica a execução local e o desenvolvimento inicial, e pode ser substituída futuramente por banco de dados sem impacto direto na camada de domínio.

## Desenho Arquitetural

### Diagrama de contexto
Visão macro da interação com a API.

### Diagrama de componentes e contêineres
graph TD
    subgraph API_Layer[🌐 Camada API]
        C[Controllers]
        SW[Swagger]
        DI[Injeção de Dependência]
    end

    subgraph APP[⚙️ Camada Application]
        SV[Serviços]
        SI[Interfaces de Serviços]
    end

    subgraph DOM[🎯 Camada Domain]
        EN[Entidades]
        RI[Interfaces de Repositório]
    end

    subgraph INF[💾 Camada Infrastructure]
        RP[Repositórios Concretos]
        ME[Listas Estáticas em Memória]
    end

    C --> SV
    DI --> SV
    DI --> RP

    SV --> RI
    SV --> EN

    RP --> RI
    RP --> EN
    RP --> ME

### Fluxo de dependência
- `API` depende de `Application` e `Infrastructure`
- `Application` depende de `Domain`
- `Infrastructure` depende de `Domain`
- `Domain` não depende das demais camadas

Esse desenho mantém o núcleo do sistema concentrado no domínio, reduzindo acoplamento e melhorando a manutenção.

## Estrutura de Pastas

### `VendasLux.API`
Responsável por:
- Expor os endpoints HTTP
- Receber as requisições por meio dos controllers
- Configurar o `Swagger`
- Inicializar a aplicação
- Registrar as dependências com `builder.Services.ResolveDependencies()`

### `VendasLux.Application`
Responsável por:
- Serviços da aplicação
- Orquestração das regras de negócio
- Validações de uso
- Coordenação entre a camada de apresentação e o domínio

### `VendasLux.Domain`
Responsável por:
- Entidades do sistema
- Interfaces dos repositórios
- Regras centrais do negócio
- Conceitos de domínio, como `Pedido` e seus `ItensPedido`

### `VendasLux.Infrastructure`
Responsável por:
- Implementações concretas dos repositórios
- Armazenamento em memória com listas estáticas
- Isolar detalhes técnicos do restante da aplicação

## Como executar

### Pré-requisitos
- `.NET 8 SDK`
- Visual Studio 2022 ou superior, ou VS Code com suporte a C#

### 1. Restaurar as dependências
dotnet restore

### 2. Compilar a solução
dotnet build

### 3. Executar a API
dotnet run --project VendasLux.API

### 4. Acessar o Swagger
Com a aplicação em execução no ambiente de desenvolvimento, acesse:
https://localhost:<porta>/swagger

### 5. Testar as rotas
No `Swagger`, é possível:
- Visualizar os endpoints disponíveis
- Enviar requisições
- Validar respostas
- Testar operações de `Clientes`, `Produtos` e `Pedidos`

## Documentação da API

A documentação da API é disponibilizada por meio do `Swagger`, permitindo visualizar e testar os endpoints diretamente no navegador.

### Como acessar
Após executar a aplicação em ambiente de desenvolvimento, abra:
https://localhost:<porta>/swagger

No `Swagger`, você encontrará:

- A descrição dos endpoints disponíveis
- Informações sobre os parâmetros e tipos de dados
- Exemplos de requisições e respostas
- Funcionalidade para testar as chamadas à API diretamente pela interface

Explore as rotas para entender as funcionalidades de `Clientes`, `Produtos` e `Pedidos`.

### O que o Swagger oferece
- Lista de endpoints disponíveis
- Descrição dos contratos de entrada e saída
- Execução de requisições diretamente pela interface
- Validação rápida dos retornos da API

### Observação
A documentação fica habilitada apenas em ambiente de desenvolvimento, conforme a configuração definida no `Program.cs`.

### Personalização atual
O projeto utiliza um `Schema Filter` para ajustar a representação de `PedidoRequest` na documentação gerada pelo Swagger.

## Detalhamento da Implementação da API

A camada `API` é responsável por receber as requisições HTTP, configurar a aplicação e expor a documentação interativa.

### Inicialização da aplicação
O arquivo `Program.cs` realiza a configuração inicial do projeto:

- registra os controllers com `builder.Services.AddControllers()`
- configura o Swagger com `builder.Services.AddSwaggerGen()`
- adiciona o `Schema Filter` para personalizar a documentação de `PedidoRequest`
- executa a injeção de dependência com `builder.Services.ResolveDependencies()`
- habilita o Swagger apenas em ambiente de desenvolvimento

### Configuração de Swagger
O Swagger foi configurado para facilitar:
- visualização dos endpoints
- entendimento dos contratos de entrada e saída
- execução de testes diretamente pelo navegador

Além disso, o projeto utiliza:

- `PedidoRequestSchemaFilter`

Esse filtro ajusta a forma como o objeto `PedidoRequest` aparece na documentação gerada.

### Controllers
Os controllers da aplicação devem permanecer finos e com responsabilidade limitada a:

- receber requisições
- validar dados de entrada básicos
- chamar serviços da camada `Application`
- devolver as respostas HTTP

### Injeção de dependência
A camada `API` não conhece diretamente as implementações concretas dos serviços e repositórios.  
Ela apenas solicita as dependências por meio das interfaces registradas no contêiner.

### Configuração de ambiente
A documentação do Swagger é habilitada somente quando a aplicação está em modo de desenvolvimento, garantindo que o acesso público em produção possa ser controlado conforme a necessidade.

### Exemplo do fluxo de execução
1. O cliente envia uma requisição HTTP
2. O controller recebe a chamada
3. O controller encaminha a operação para um serviço da `Application`
4. O serviço executa a regra de negócio
5. O serviço acessa a interface de repositório no `Domain`
6. A implementação concreta na `Infrastructure` responde com os dados
7. A API retorna a resposta ao cliente

## Observações
- Os controllers devem permanecer finos
- As regras de negócio devem ficar fora da camada `API`
- As interfaces dos repositórios devem permanecer no `Domain`
- As implementações concretas devem permanecer no `Infrastructure`
- A solução já está preparada para futura integração com banco de dados

## Visão geral da solução
VendasLux ├── VendasLux.API ├── VendasLux.Application ├── VendasLux.Domain └── VendasLux.Infrastructure

## Modelo de Domínio

O domínio do sistema é formado por quatro conceitos principais:

- `Cliente`
- `Produto`
- `Pedido`
- `ItemPedido`

A entidade `Pedido` funciona como **Raiz de Agregação**, sendo responsável por concentrar os itens do pedido e representar a operação comercial completa.

### Relacionamentos principais
- Um `Cliente` pode realizar vários `Pedidos`
- Um `Pedido` pertence a um único `Cliente`
- Um `Pedido` possui um ou mais `ItensPedido`
- Cada `ItemPedido` referencia um `Produto`

### Diagrama do modelo de domínio

## Licença
Projeto interno.

## Responsabilidades das Camadas

A solução `VendasLux` foi dividida em camadas para manter o sistema organizado, desacoplado e fácil de evoluir.

### `API`
Responsável pela entrada da aplicação.

**Funções principais:**
- Receber requisições HTTP
- Expor endpoints por meio de controllers
- Configurar o Swagger
- Realizar a inicialização da aplicação
- Registrar as dependências no contêiner de DI

### `Application`
Responsável pela orquestração dos casos de uso.

**Funções principais:**
- Implementar serviços da aplicação
- Coordenar as regras de negócio
- Validar fluxos de execução
- Fazer a ponte entre `API` e `Domain`

### `Domain`
Responsável pelo núcleo do negócio.

**Funções principais:**
- Definir as entidades do sistema
- Declarar os contratos dos repositórios
- Concentrar os conceitos centrais do domínio
- Manter o modelo de negócio independente de detalhes técnicos

### `Infrastructure`
Responsável pelos detalhes técnicos e de persistência.

**Funções principais:**
- Implementar os repositórios concretos
- Armazenar os dados em memória
- Isolar a lógica de acesso a dados
- Permitir futura troca de persistência sem afetar o domínio

### Regra de dependência entre as camadas
- `API` depende de `Application` e `Infrastructure`
- `Application` depende de `Domain`
- `Infrastructure` depende de `Domain`
- `Domain` não depende de nenhuma outra camada

Essa separação garante que o domínio permaneça estável, enquanto as demais camadas podem evoluir com menor risco de impacto.

## Diagramas – Modelo C4 (Mermaid)

### Diagrama de Contexto

Person(usuario, "Usuário", "Consome a API para cadastrar, consultar e gerenciar clientes, produtos e pedidos.")
System(api, "VendasLux API", "API REST em .NET 8 para o domínio de vendas.")

Rel(usuario, api, "Faz requisições HTTP")

### Diagrama de Contêineres
Person(usuario, "Usuário", "Consome a API")

System_Boundary(sistema, "VendasLux") {
    Container(api, "VendasLux.API", ".NET 8 / ASP.NET Core", "Exposição dos endpoints, Swagger e configuração da aplicação.")
    Container(app, "VendasLux.Application", ".NET 8", "Serviços e orquestração das regras de negócio.")
    Container(domain, "VendasLux.Domain", ".NET 8", "Entidades, agregados e contratos do domínio.")
    Container(infra, "VendasLux.Infrastructure", ".NET 8", "Implementações dos repositórios e persistência em memória.")
}

Rel(usuario, api, "Faz requisições HTTP")
Rel(api, app, "Chama serviços")
Rel(api, infra, "Resolve dependências")
Rel(app, domain, "Usa entidades e interfaces")
Rel(infra, domain, "Implementa contratos")

### Diagrama de Componentes
Container(api, "VendasLux.API", ".NET 8 / ASP.NET Core", "Camada de apresentação")
Container(app, "VendasLux.Application", ".NET 8", "Camada de aplicação")
Container(domain, "VendasLux.Domain", ".NET 8", "Núcleo do domínio")
Container(infra, "VendasLux.Infrastructure", ".NET 8", "Camada de infraestrutura")

Component(controller, "Controllers", "ASP.NET Core", "Recebem requisições HTTP e encaminham para os serviços.")
Component(service, "Serviços", "C#", "Executam os casos de uso e coordenam as regras de negócio.")
Component(interfacesDomain, "Interfaces de Repositório", "C#", "Definem os contratos usados pela aplicação.")
Component(entities, "Entidades", "C#", "Representam os objetos centrais do domínio.")
Component(repositories, "Repositórios", "C#", "Persistem e recuperam dados em memória.")

Rel(api, controller, "Contém")
Rel(controller, service, "Usa")
Rel(service, interfacesDomain, "Depende de")
Rel(service, entities, "Manipula")
Rel(infra, repositories, "Contém")
Rel(repositories, interfacesDomain, "Implementa")
Rel(repositories, entities, "Usa")

### Direção das dependências

- `API` depende de `Application` e `Infrastructure`
- `Application` depende de `Domain`
- `Infrastructure` depende de `Domain`
- `Domain` não depende de nenhuma outra camada