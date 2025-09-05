namespace CaixaEletronicoSQLite
{
    /// <summary>
    /// Classe principal do programa que simula um caixa eletrônico
    /// </summary>
    class Program
    {
        /// <summary>
        /// Serviço bancário com as regras de negócio
        /// </summary>
        private static readonly BancoService _bancoService = new();

        /// <summary>
        /// Método auxiliar para buscar uma conta pelo número com validação
        /// </summary>
        private static Conta? BuscarContaComValidacao(string mensagem = "Digite o número da conta: ")
        {
            Console.Write(mensagem);

            if (int.TryParse(Console.ReadLine(), out int numeroConta))
            {
                try
                {
                    return _bancoService.BuscarConta(numeroConta);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Número de conta inválido!");
            }

            Console.WriteLine("Pressione qualquer tecla para continuar...");
            Console.ReadKey();
            return null;
        }

        /// <summary>
        /// Método principal que inicia o programa
        /// </summary>
        static void Main()
        {
            // Inicializa o banco de dados e cria as tabelas se não existirem
            var databaseContext = new DatabaseContext();
            Console.WriteLine("Banco de dados inicializado com sucesso!");

            bool sendoUsado = true;

            // Loop principal do menu do caixa eletrônico
            while (sendoUsado)
            {
                Console.Clear();
                Console.WriteLine(
    @"----- Banco Marcos -----

    1 - Criar Conta
    2 - Listar Todas as Contas
    3 - Consultar Saldo de uma conta
    4 - Consultar Histórico de uma conta
    5 - Sacar de uma conta
    6 - Depositar em uma conta
    7 - Transferir de uma conta para outra
    0 - Sair

    Escolha uma opção: ");

                string escolha = Console.ReadLine();

                // Processa a escolha do usuário
                switch (escolha)
                {
                    case "1":
                        CriarConta();
                        break;
                    case "2":
                        ListarContas();
                        break;
                    case "3":
                        ConsultarSaldo();
                        break;
                    case "4":
                        ConsultarHistorico();
                        break;
                    case "5":
                        Sacar();
                        break;
                    case "6":
                        Depositar();
                        break;
                    case "7":
                        Transferir();
                        break;
                    case "0":
                        Console.WriteLine("Obrigado por usar o Banco Marcos. Até logo!");
                        sendoUsado = false;
                        break;
                    default:
                        Console.WriteLine("Opção inválida. Tente novamente.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        /// <summary>
        /// Cria uma nova conta solicitando o nome do titular ao usuário
        /// </summary>
        static void CriarConta()
        {
            Console.Clear();
            Console.WriteLine("--- Criação de Nova Conta ---");
            Console.Write("Digite o nome completo do titular: ");
            string titular = Console.ReadLine();

            try
            {
                _bancoService.CriarConta(titular);
                var contaCriada = _bancoService.BuscarUltimaContaPorTitular(titular);

                Console.WriteLine($"\nConta criada com sucesso!");
                Console.WriteLine($"Número da conta: {contaCriada.NumeroDaConta}");
                Console.WriteLine($"Titular: {contaCriada.TitularDaConta}");
                Console.WriteLine($"Saldo inicial: R$ {contaCriada.SaldoDaConta:0.00}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }

            Console.WriteLine("Pressione qualquer tecla para voltar ao menu inicial.");
            Console.ReadKey();
        }

        /// <summary>
        /// Consulta o saldo de uma conta existente
        /// </summary>
        static void ConsultarSaldo()
        {
            Console.Clear();
            Console.WriteLine("--- Consulta de Saldo ---");

            var conta = BuscarContaComValidacao();
            if (conta == null) return;

            Console.WriteLine($"\nConta: {conta.NumeroDaConta}");
            Console.WriteLine($"Titular: {conta.TitularDaConta}");
            Console.WriteLine($"Saldo: R$ {conta.SaldoDaConta:0.00}");

            Console.WriteLine("Pressione qualquer tecla para voltar ao menu inicial.");
            Console.ReadKey();
        }

        /// <summary>
        /// Realiza um depósito em uma conta existente
        /// </summary>
        static void Depositar()
        {
            Console.Clear();
            Console.WriteLine("--- Depósito ---");

            var conta = BuscarContaComValidacao();
            if (conta == null) return;

            Console.Write("Digite o valor do depósito: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal valor))
            {
                try
                {
                    _bancoService.Depositar(conta.NumeroDaConta, valor);
                    var contaAtualizada = _bancoService.BuscarConta(conta.NumeroDaConta);

                    Console.WriteLine($"Depósito de R$ {valor:0.00} realizado com sucesso!");
                    Console.WriteLine($"Novo saldo: R$ {contaAtualizada.SaldoDaConta:0.00}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Valor inválido!");
            }

            Console.WriteLine("Pressione qualquer tecla para voltar ao menu inicial.");
            Console.ReadKey();
        }

        /// <summary>
        /// Realiza um saque em uma conta existente
        /// </summary>
        static void Sacar()
        {
            Console.Clear();
            Console.WriteLine("--- Saque ---");

            var conta = BuscarContaComValidacao();
            if (conta == null) return;

            Console.Write("Digite o valor do saque: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal valor))
            {
                try
                {
                    _bancoService.Sacar(conta.NumeroDaConta, valor);
                    var contaAtualizada = _bancoService.BuscarConta(conta.NumeroDaConta);

                    Console.WriteLine($"Saque de R$ {valor:0.00} realizado com sucesso!");
                    Console.WriteLine($"Novo saldo: R$ {contaAtualizada.SaldoDaConta:0.00}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Valor inválido!");
            }

            Console.WriteLine("Pressione qualquer tecla para voltar ao menu inicial.");
            Console.ReadKey();
        }

        /// <summary>
        /// Realiza transferência entre duas contas existentes
        /// </summary>
        static void Transferir()
        {
            Console.Clear();
            Console.WriteLine("--- Transferência ---");

            // Busca conta origem
            var contaOrigem = BuscarContaComValidacao("Digite o número da conta de origem: ");
            if (contaOrigem == null) return;

            // Busca conta destino
            var contaDestino = BuscarContaComValidacao("Digite o número da conta de destino: ");
            if (contaDestino == null) return;

            Console.Write("Digite o valor da transferência: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal valor))
            {
                try
                {
                    _bancoService.Transferir(contaOrigem.NumeroDaConta, contaDestino.NumeroDaConta, valor);
                    var contaOrigemAtualizada = _bancoService.BuscarConta(contaOrigem.NumeroDaConta);

                    Console.WriteLine($"Transferência de R$ {valor:0.00} realizada com sucesso!");
                    Console.WriteLine($"Novo saldo da conta {contaOrigem.NumeroDaConta}: R$ {contaOrigemAtualizada.SaldoDaConta:0.00}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Valor inválido!");
            }

            Console.WriteLine("Pressione qualquer tecla para voltar ao menu inicial.");
            Console.ReadKey();
        }

        /// <summary>
        /// Consulta o histórico de transações de uma conta existente
        /// </summary>
        static void ConsultarHistorico()
        {
            Console.Clear();
            Console.WriteLine("--- Histórico de Transações ---");

            var conta = BuscarContaComValidacao();
            if (conta == null) return;

            try
            {
                var transacoes = _bancoService.ConsultarHistorico(conta.NumeroDaConta);

                Console.WriteLine($"\nHistórico da Conta {conta.NumeroDaConta}:");
                Console.WriteLine("Data/Hora | Tipo | Valor | Conta Relacionada");
                Console.WriteLine(new string('-', 50));

                foreach (var transacao in transacoes)
                {
                    string contaRelacionada = transacao.Tipo switch
                    {
                        "DEPOSITO" => "Entrada",
                        "SAQUE" => "Saída",
                        "TRANSFERENCIA" => transacao.ContaOrigem == conta.NumeroDaConta ?
                                          $"Para conta {transacao.ContaDestino}" :
                                          $"Da conta {transacao.ContaOrigem}",
                        _ => ""
                    };

                    Console.WriteLine($"{transacao.DataHora:dd/MM/yy HH:mm} | {transacao.Tipo} | R$ {transacao.Valor:0.00} | {contaRelacionada}");
                }

                if (transacoes.Count == 0)
                {
                    Console.WriteLine("Nenhuma transação encontrada.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }

            Console.WriteLine("Pressione qualquer tecla para voltar ao menu inicial.");
            Console.ReadKey();
        }

        /// <summary>
        /// Lista todas as contas cadastradas no sistema
        /// </summary>
        static void ListarContas()
        {
            Console.Clear();
            Console.WriteLine("--- Lista de Todas as Contas ---");

            try
            {
                var contas = _bancoService.ListarTodasContas();

                if (contas.Count > 0)
                {
                    Console.WriteLine("┌──────────┬──────────────────────────────┐");
                    Console.WriteLine("│ Número   │ Titular                      │");
                    Console.WriteLine("├──────────┼──────────────────────────────┤");

                    foreach (var conta in contas)
                    {
                        Console.WriteLine($"│ {conta.NumeroDaConta,-8} │ {conta.TitularDaConta,-28} │");
                    }

                    Console.WriteLine("└──────────┴──────────────────────────────┘");
                    Console.WriteLine($"Total de contas cadastradas: {contas.Count}");
                }
                else
                {
                    Console.WriteLine("Nenhuma conta cadastrada no sistema.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }

            Console.WriteLine("\nPressione qualquer tecla para continuar...");
            Console.ReadKey();
        }
    }
}