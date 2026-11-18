# ClimaFixApi — Sistema de Gestão de Climatização

- **Aluno:** Vinicius Augusto Anjos Basilio
- **Curso / Turma:** Análise e Desenvolvimento de Sistemas (ADS) — Faculdade Newton Paiva (2026.2)
- **Repositório:**https://github.com/Vinigol/RefrigeraClimaBh

## Descrição do Projeto
A **RefrigeraClimaBh** é uma aplicação web completa desenvolvida para a disciplina de Arquitetura de Aplicações Web. O sistema gerencia equipamentos de climatização e ordens de manutenção preventiva/corretiva, oferecendo uma API RESTful documentada, persistência em banco de dados NoSQL (MongoDB), autenticação via JWT, autorização baseada em perfis (RBAC) e uma interface web assíncrona.

## Pré-requisitos
- **.NET SDK 8.0** (ou superior)
- **MongoDB** (instância local ou MongoDB Atlas)
- **Git**

## Variáveis de Ambiente
Antes de executar a aplicação, configure a string de conexão do banco de dados e a chave JWT através de variáveis de ambiente ou no ficheiro `appsettings.Development.json` (nunca versione senhas reais no repositório):

```env
MONGODB_URI=mongodb://localhost:27017
JwtSettings__SecretKey=SuaChaveSecretaSuperSeguraComPeloMenos32Caracteres