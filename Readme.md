# Belchior - Back-end Challenge 🏅 2021 - Space Flight News

Essa é uma REST API criada por **Bruno Belchior**, cujo propósito é demonstrar a equipe da [Coodesh](https://coodesh.com/) uma visão das habilidades e competências do candidato a vaga de Back-end Developer. Conforme exigido pelo desafio, foram utilizados os dados da API pública [Space Flight News](https://api.spaceflightnewsapi.net/v3/documentation) para a implementação do projeto.

>  This is a challenge by [Coodesh](https://coodesh.com/)

### Tecnologias utilizadas no desenvolvimento do projeto

<table>
  <tr>
    <th></th>
    <th>Tecnologia</th>
    <th>Versão</th>
  </tr>
  <tr>
    <td><img align="center" alt="Rafa-Csharp" height="30" width="40" src="https://icongr.am/devicon/dot-net-original.svg?size=40"></td>
    <td>.Net Core</td>
    <td>5.0</td>
  </tr>
  <tr>
    <td><img align="center" alt="Rafa-Csharp" height="30" width="40" src="https://icongr.am/devicon/csharp-original.svg?size=40"></td>
    <td>C#</td>
    <td>8.0</td>
  </tr>    
  <tr>
    <td><img align="center" alt="Rafa-Csharp" height="30" width="40" src="https://icongr.am/devicon/visualstudio-plain.svg?size=40"></td>
    <td>Visual Studio</td>
    <td>2019 Community</td>
  </tr>    
  <tr>
    <td><img align="center" alt="Rafa-Csharp" height="30" width="40" src="https://icongr.am/devicon/mongodb-original.svg?size=40"></td>
    <td>MongoDB</td>
    <td>5.0.6 Community</td>
  </tr>
  <tr>
    <td><img align="center" alt="Rafa-Csharp" height="30" width="40" src="https://icongr.am/devicon/docker-original.svg?size=40"></td>
    <td>Docker</td>
    <td>20.10.12</td>
  </tr>    
</table>

### Back-End Challenge:

Nessa etapa você deverá construir uma API Restful com as melhores práticas de desenvolvimento, baseada na API [Space Flight News](https://api.spaceflightnewsapi.net/v3/documentation). Para isso você deve executar os passos a seguir:

**Obrigatório 1** - Você deverá desenvolver as seguintes rotas. **IMPLEMENTADO**

- `[GET]/: ` Retornar um Status: 200 e uma Mensagem "Back-end Challenge 2021 🏅 - Space Flight News"
- `[GET]/articles/:`   Listar todos os artigos da base de dados, utilizar o sistema de paginação para não sobrecarregar a REQUEST
- `[GET]/articles/{id}:` Obter a informação somente de um artigo
- `[POST]/articles/:` Adicionar um novo artigo
- `[PUT]/articles/{id}:` Atualizar um artigo baseado no `id`
- `[DELETE]/articles/{id}:` Remover um artigo baseado no `id`

**Obrigatório 2** - Para alimentar o seu banco de dados você deve criar um script para armazenar os dados de todos os artigos na Space Flight News API.  **IMPLEMENTADO**

**Obrigatório 3** - Além disso você precisa desenvolver um CRON para ser executado diariamente às 9h e armazenar em seu os novos artigos ao seu banco de dados. (Para essa tarefa você poderá alterar o seu modelo de dados).  **IMPLEMENTADO**

**Diferencial 1** Configurar Docker no Projeto para facilitar o Deploy da equipe de DevOps; **IMPLEMENTADO**

**Diferencial 2** Configurar um sistema de alerta se houver algum falha durante a sincronização dos artigos; **IMPLEMENTADO**

**Diferencial 3** Descrever a documentação da API utilizando o conceito de Open API 3.0; **IMPLEMENTADO**

- `[GET]/swagger/index.html ` URL para acesso

**Diferencial 4** Escrever Unit Tests para os endpoints da API;

## Readme do Repositório

- Deve conter o título do projeto
- Uma descrição sobre o projeto em frase
- Deve conter uma lista com linguagem, framework e/ou tecnologias usadas
- Como instalar e usar o projeto (instruções)
- Não esqueça o [.gitignore](https://www.toptal.com/developers/gitignore)
- Se está usando github pessoal, referencie que é um challenge by coodesh:  

>  This is a challenge by [Coodesh](https://coodesh.com/)

>  docker build --no-cache -t coodesh-api:dev .

>  docker run -d -p 17133:80 --name bb-codesh coodesh-api:dev

## Finalização e Instruções para a Apresentação

Avisar sobre a finalização e enviar para correção.

1. Confira se você respondeu o Scorecard da Vaga que chegou no seu email; **FEITO**
2. Confira se você respondeu o Mapeamento Comportamental que chegou no seu email; **FEITO**
3. Acesse: [https://coodesh.com/challenges/review](https://coodesh.com/challenges/review);
4. Adicione o repositório com a sua solução;
5. Grave um vídeo, com no máximo 5 minutos, com a apresentação do seu projeto. Foque em pontos obrigatórios e diferenciais quando for apresentar. **NÃO FEITO**
6. Adicione o link da apresentação do seu projeto no README.md. **NÃO FEITO**
7. Verifique se o Readme está bom e faça o commit final em seu repositório; **NÃO FEITO**
8. Confira a vaga desejada;
9. Envie e aguarde as instruções para seguir no processo. Sucesso e boa sorte. =)

<a target="_blank" href="https://www.linkedin.com/in/brunovicenteb/"> 
  <img align="left" alt="LinkdeIN" width="22px" src="https://cdn.jsdelivr.net/npm/simple-icons@v3/icons/linkedin.svg" />
</a>
