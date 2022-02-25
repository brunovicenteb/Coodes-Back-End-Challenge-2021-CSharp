# Belchior - Back-end Challenge 🏅 2021 - Space Flight News

Essa é uma REST API criada por **Bruno Belchior**, cujo propósito é demonstrar a equipe da [Coodesh](https://coodesh.com/) uma visão das habilidades e competências do candidato a vaga de Back-end Developer. Conforme exigido pelo desafio, foram utilizados os dados da API pública [Space Flight News](https://api.spaceflightnewsapi.net/v3/documentation) para a implementação do projeto.

>  This is a challenge by [Coodesh](https://coodesh.com/)

Uma versão desse mesmo em execução está [disponível](https://coodesh-backend-challenge.herokuapp.com), na hospedagem gratuita fornecida pela Heroku. Para ver a documentação disponível no formato Open API 3.0 basta acessar [esse link](https://coodesh-backend-challenge.herokuapp.com/swagger/index.html).

>  ![GitHub Workflow Status](https://img.shields.io/github/workflow/status/brunovicenteb/Coodes-Back-End-Challenge-2021-CSharp/Build-Tests-Coverlet-DeployHeroku) ![GitHub last commit](https://img.shields.io/github/last-commit/brunovicenteb/Coodes-Back-End-Challenge-2021-CSharp) ![GitHub top language](https://img.shields.io/github/languages/top/brunovicenteb/Coodes-Back-End-Challenge-2021-CSharp) ![GitHub language count](https://img.shields.io/github/languages/count/brunovicenteb/Coodes-Back-End-Challenge-2021-CSharp) ![GitHub repo size](https://img.shields.io/github/repo-size/brunovicenteb/Coodes-Back-End-Challenge-2021-CSharp) [![License](https://img.shields.io/badge/license-MIT-green)](./LICENSE) 

### Tecnologias utilizadas no desenvolvimento do projeto

<table>
  <tr>
    <th></th>
    <th>Tecnologia</th>
    <th>Versão</th>
    <th>Ferramentas</th>    
  </tr>
  <tr>
    <td><img align="center" alt="Rafa-Csharp" height="30" width="40" src="https://icongr.am/devicon/dot-net-original.svg?size=40"></td>
    <td>.Net Core</td>
    <td>5.0</td>
    <td><a href="https://serilog.net">Serilog</a>, <a href="https://nunit.org">NUnit</a>, <a href="https://fluentvalidation.net">FluentValidation</a></td>
  </tr>
  <tr>
    <td><img align="center" alt="Rafa-Csharp" height="30" width="40" src="https://icongr.am/devicon/csharp-original.svg?size=40"></td>
    <td>C#</td>
    <td>8.0</td>
    <td></td>
  </tr>    
  <tr>
    <td><img align="center" alt="Rafa-Csharp" height="30" width="40" src="https://icongr.am/devicon/visualstudio-plain.svg?size=40"></td>
    <td>Visual Studio</td>
    <td>2022 Community</td>
    <td></td>
  </tr>    
  <tr>
    <td><img align="center" alt="Rafa-Csharp" height="30" width="40" src="https://icongr.am/devicon/mongodb-original.svg?size=40"></td>
    <td>MongoDB</td>
    <td>5.0.6 Community</td>
    <td><a href="https://docs.mongodb.com/compass/current">MongoDB Compass</a>, <a href="https://docs.mongodb.com/drivers/csharp">MongoDB.Driver</a></td>    
  </tr>
  <tr>
    <td><img align="center" alt="Rafa-Csharp" height="30" width="40" src="https://icongr.am/devicon/docker-original.svg?size=40"></td>
    <td>Docker</td>
    <td>lasted</td>
    <td><a href="https://hub.docker.com/editions/community/docker-ce-desktop-windows">Docker Desktop for Windows</a></td>    
  </tr>
  <tr>
    <td><img align="center" alt="Rafa-Csharp" height="30" width="40" src="https://icongr.am/devicon/redis-original-wordmark.svg?size=40"></td>
    <td>Redis</td>
    <td>lasted</td>
    <td></td>    
  </tr>
  <tr>
    <td><img align="center" alt="Rafa-Csharp" height="30" width="40" src="https://icongr.am/devicon/git-original.svg?size=40"></td>
    <td>Git</td>
    <td>lasted</td>
    <td><a href="https://github.com/nvie/gitflow">Git Flow</a></td>    
  </tr>  
</table>

## Armazenamento dos dados

Conforme informado na tabela acima, o banco de dados utilizado foi o **MongoDB** e está armazenado de forma gratuita no [Atlas](https://www.mongodb.com/cloud/atlas).

## Pré-requisitos para a instalação do projeto:

+ Git
+ Docker
+ Docker-compose

## Instalação do projeto e configuração do ambiente:

1. Clonar o repositório:

   `
   git clone https://github.com/brunovicenteb/Coodes-Back-End-Challenge-2021-CSharp.git
   `

2. Entrar no diretório criado:

   `
   cd Coodes-Back-End-Challenge-2021-CSharp
   `

4. Subir o container:

   `
   docker-compose -p challenge up -d
   `
  
Nessa etapa **já é possível acessar** a [API](http://localhost:8000/) e a [documentação](http://localhost:8000/swagger/index.html) no formato Open API 3.0; pelo navegador.

## Alimentação dos dados via script

1. Conectar no terminal do container:

   `
   docker exec -ti coodesh.api /bin/bash
   `
   
2. Executar o script de alimentação de dados:

   `
   ./script.sh
   `

Após a execução do script o banco já estará povoado com todos os dados de voos espaciai. **Não é necessário qualquer configuração do crontab**, o container já é iniciado com toda a configuração necessária.

# License

[The MIT License (MIT)](LICENSE)