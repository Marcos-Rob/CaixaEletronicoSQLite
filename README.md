# 🏦 Sistema de Caixa Eletrônico - Banco Marcos

## 📋 Descrição do Projeto

Sistema de caixa eletrônico desenvolvido em C# (.NET 8.0) com persistência em SQLite. Permite realizar operações bancárias completas com armazenamento permanente dos dados. Desafio backend que demonstra conceitos de programação orientada a objetos, acesso a banco de dados e arquitetura em camadas.

## 🛠️ Funcionalidades

- ✅ **Criar Conta** - Cadastra novas contas bancárias com número automático
- ✅ **Listar Contas** - Exibe todas as contas do sistema (número e titular)
- ✅ **Consultar Saldo** - Mostra saldo detalhado de uma conta específica
- ✅ **Realizar Depósito** - Adiciona fundos a uma conta
- ✅ **Realizar Saque** - Remove fundos de uma conta (com validação de saldo)
- ✅ **Transferência entre Contas** - Transfere valores entre contas
- ✅ **Consultar Histórico** - Visualiza extrato completo de transações

## ✨ Tecnologias Utilizadas

- **.NET 8.0** - Framework principal
- **SQLite** - Banco de dados embutido
- **System.Data.SQLite** - Driver para acesso ao SQLite
- **C#** - Linguagem de programação
- **Visual Studio 2022** - IDE de desenvolvimento

## 🚀 Como Executar o Projeto

### Pré-requisitos
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Git (para clonar o repositório)

### Passos para Executar

1. Clone o repositório:
   ```bash
   git clone https://github.com/Marcos-Rob/CaixaEletronicoSQLite.git
   cd CaixaEletronicoSQLite
   dotnet restore
   dotnet run
   ```
## 🎮 Como Usar o Sistema

### Primeira Execução:
1. O sistema criará automaticamente:

	Arquivo banco.db na pasta bin/Debug/net8.0/

	Tabelas Contas e Transacoes

2. Crie sua primeira conta:

	Selecione a opção 1 no menu

	Digite o nome do titular

	Anote o número da conta gerado

	Utilize as outras opções do menu para operar

## 🤝 Contribuindo

1. Faça um fork do projeto
2. Crie uma branch para sua feature ```git checkout -b feature/nova-funcionalidade```
3. Faça commit das suas alterações ```git commit -m 'Adiciona nova funcionalidade'```
4. Faça push para a branch ```git push origin feature/nova-funcionalidade```
5. Abra um Pull Request

## 📄 Licença

Este projeto está licenciado sob a Licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.