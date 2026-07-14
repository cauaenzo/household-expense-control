# Controle de Gastos Residenciais

API REST com interface web para registrar receitas e despesas por pessoa em um contexto residencial.

## Tecnologias

**Backend**
- .NET 10, ASP.NET Core Web API com Controllers
- Entity Framework Core 10 + SQLite
- Swashbuckle (Swagger UI em `/swagger`)
- DotNetEnv (carregamento de variáveis de `.env`)

**Frontend**
- React 18 + TypeScript 5 + Vite 5
- React Router DOM
- Axios
- CSS Modules

## Como executar

### Pré-requisitos

- .NET 10 SDK
- Node.js 18+ e npm

### Backend

```bash
cd backend/GastosResidenciais.Api

# Copiar variáveis de ambiente (valores já funcionais para desenvolvimento local)
cp .env.example .env

# Restaurar dependências e aplicar migrations (o banco é criado automaticamente ao rodar)
dotnet run
```

A API sobe em `httxp://localhost:5112`.  
O Swagger UI fica em `http://localhost:5112/swagger`.

Caso prefira aplicar a migration manualmente antes de rodar:

```bash
dotnet tool install --global dotnet-ef
dotnet ef database update
```

### Frontend

```bash
cd frontend

# Copiar variáveis de ambiente
cp .env.example .env

npm install
npm run dev
```

O frontend sobe em `http://localhost:5173`.

## Estrutura do projeto

```
household-expense-control/
├── PROGRESS.md
├── README.md
├── GastosResidenciais.sln
├── backend/
│   └── GastosResidenciais.Api/
│       ├── .env / .env.example
│       ├── Controllers/
│       ├── Services/
│       ├── Repositories/
│       ├── Models/
│       ├── DTOs/
│       ├── Data/
│       │   └── Migrations/
│       ├── Middleware/
│       └── Program.cs
└── frontend/
    ├── .env / .env.example
    └── src/
        ├── components/
        ├── pages/
        ├── services/
        ├── types/
        └── hooks/
```

## Regras de negócio

- Pessoas têm nome (obrigatório) e idade (inteiro >= 0).
- Ao excluir uma pessoa, todas as suas transações são removidas em cascata.
- Transações têm descrição, valor (> 0), tipo (Receita ou Despesa) e vínculo obrigatório com uma pessoa.
- Edição e exclusão de transações não estão implementadas por estar fora do escopo definido.
- **Regra crítica:** pessoas com idade menor que 18 anos não podem registrar transações do tipo `Receita`. A tentativa retorna HTTP 400 com mensagem explicativa. Essa validação ocorre no backend e o frontend também bloqueia a operação visualmente.
- O valor da transação representa sempre o montante absoluto. O campo `tipo` define se é entrada ou saída.


- [Diagramação do Backend e Relacionamentos](EXTENSIONR.MD)


## Endpoints da API

| Método | Rota | Body | Sucesso | Erros possíveis |
|--------|------|------|---------|-----------------|
| POST | `/api/pessoas` | `{ nome, idade }` | 201 + pessoa criada | 400 (validação) |
| GET | `/api/pessoas` | — | 200 + lista | — |
| DELETE | `/api/pessoas/{id}` | — | 204 | 404 (não existe) |
| POST | `/api/transacoes` | `{ descricao, valor, tipo, pessoaId }` | 201 + transação criada | 400 (menor/valor inválido), 404 (pessoa não existe) |
| GET | `/api/transacoes` | — (query opcional `?pessoaId=`) | 200 + lista | — |
| GET | `/api/relatorios/totais` | — | 200 + `{ pessoas, totalGeral }` | — |

Formato de erro padrão em toda a API:

```json
{ "mensagem": "Descrição do erro para o usuário." }
```

## Melhorias futuras

- Testes automatizados (unitários e de integração)
- Autenticação e controle de acesso por usuário
- Edição e exclusão de transações
- Paginação nas listagens
- Filtros por período nas transações
- Exportação de relatório em CSV ou PDF
