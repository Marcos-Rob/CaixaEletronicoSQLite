namespace CaixaEletronicoSQLite
{
    /// <summary>
    /// Classe principal do programa que simula um caixa eletrônico com 
    /// funcionalidades de criação de conta, listagem das contas, consulta de saldo,
    /// consulta de histórico de transações, saque, depósito e transferência.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Repositórios estáticos e somente leitura para acesso aos dados 
        /// de contas e transações no banco de dados.
        /// </summary>
        private static readonly ContaRepository _contaRepository = new();
        private static readonly TransacaoRepository _transacaoRepository = new();

        /// <summary>
        /// Método principal que inicia o programa.
        /// </summary>
        static void Main()
        {
            // Inicializa o banco de dados e cria as tabelas se não existirem.
            var databaseContext = new DatabaseContext();
            Console.WriteLine("Banco de dados inicializado com sucesso!");

            bool sendoUsado = true;

            //Loop principal do menu do caixa eletrônico.
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

                // Processa a escolha do usuário e chama o método correspondente.
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
        /// Cria uma nova conta solicitando o nome do titular ao usuário.
        /// </summary>
        static void CriarConta()
        {
            Console.Clear();
            Console.WriteLine("--- Criação de Nova Conta ---");
            Console.Write("Digite o nome completo do titular: ");
            string titular = Console.ReadLine();

            try
            {
                // Cria a conta e salva no banco de dados.
                var novaConta = new Conta(titular);
                _contaRepository.CriarConta(novaConta);
                Console.WriteLine("Conta criada com sucesso!");

                // Busca a conta recém-criada para obter o número gerado.
                var contaCriada = _contaRepository.BuscarUltimaContaPorTitular(titular);

                Console.WriteLine($"\n Conta criada com sucesso!");
                Console.WriteLine($"Número da conta: {contaCriada.NumeroDaConta}");
                Console.WriteLine($"Titular: {contaCriada.TitularDaConta}");
                Console.WriteLine($"Saldo inicial: R$ {contaCriada.SaldoDaConta:0.00}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao criar conta: {ex.Message}");
            }

            Console.WriteLine("Pressione qualquer tecla para voltar ao menu inicial.");
            Console.ReadKey();
        }

        /// <summary>
        /// Consulta o saldo de uma conta existente solicitando o número da conta ao usuário.
        /// </summary>
        static void ConsultarSaldo()
        {
            Console.Clear();
            Console.WriteLine("--- Consulta de Saldo ---");
            Console.Write("Digite o número da conta: ");

            //Valida e converte o número da conta.
            if (int.TryParse(Console.ReadLine(), out int numeroConta))
            {
                var conta = _contaRepository.BuscarContaPorNumero(numeroConta);

                if (conta != null)
                {
                    // Exibe as informações da conta.
                    Console.WriteLine($"\nConta: {conta.NumeroDaConta}");
                    Console.WriteLine($"Titular: {conta.TitularDaConta}");
                    Console.WriteLine($"Saldo: R$ {conta.SaldoDaConta:0.00}");
                }
                else
                {
                    Console.WriteLine("Conta não encontrada!");
                }
            }
            else
            {
                Console.WriteLine("Número de conta inválido!");
            }

            Console.WriteLine("Pressione qualquer tecla para voltar ao menu inicial.");
            Console.ReadKey();
        }

        /// <summary>
        /// Realiza um depósito em uma conta existente solicitando o número da conta e o valor ao usuário.
        /// </summary>
        static void Depositar()
        {
            Console.Clear();
            Console.WriteLine("--- Depósito ---");
            Console.Write("Digite o número da conta: ");

            if (int.TryParse(Console.ReadLine(), out int numeroConta))
            {
                var conta = _contaRepository.BuscarContaPorNumero(numeroConta);
                if (conta != null)
                {
                    Console.Write("Digite o valor do depósito: ");
                    // Valida e converte o valor do depósito.
                    if (decimal.TryParse(Console.ReadLine(), out decimal valor) && valor > 0)
                    {
                        // Atualiza o saldo da conta e registra a transação para o histórico.
                        decimal novoSaldo = conta.SaldoDaConta + valor;
                        _contaRepository.AtualizarSaldo(numeroConta, novoSaldo);
                        _transacaoRepository.RegistrarTransacao("DEPOSITO", valor, contaOrigem: null, contaDestino: numeroConta);

                        Console.WriteLine($"Depósito de R$ {valor:0.00} realizado com sucesso!");
                        Console.WriteLine($"Novo saldo: R$ {novoSaldo:0.00}");
                    }
                    else
                    {
                        Console.WriteLine("Valor inválido!");
                    }
                }
                else
                {
                    Console.WriteLine("Conta não encontrada!");
                }
            }
            else
            {
                Console.WriteLine("Número de conta inválido!");
            }

            Console.WriteLine("Pressione qualquer tecla para voltar ao menu inicial.");
            Console.ReadKey();
        }

        /// <summary>
        /// Realiza um saque em uma conta existente solicitando o número da conta e o valor ao usuário.
        /// </summary>
        static void Sacar()
        {
            Console.Clear();
            Console.WriteLine("--- Saque ---");
            Console.Write("Digite o número da conta: ");

            if (int.TryParse(Console.ReadLine(), out int numeroConta))
            {
                var conta = _contaRepository.BuscarContaPorNumero(numeroConta);
                if (conta != null)
                {
                    Console.Write("Digite o valor do saque: ");
                    if (decimal.TryParse(Console.ReadLine(), out decimal valor) && valor > 0)
                    {
                        // Verifica se há saldo suficiente para o saque.
                        if (valor <= conta.SaldoDaConta)
                        {
                            decimal novoSaldo = conta.SaldoDaConta - valor;
                            _contaRepository.AtualizarSaldo(numeroConta, novoSaldo);
                            _transacaoRepository.RegistrarTransacao("SAQUE", valor, contaOrigem: numeroConta, contaDestino: null);

                            Console.WriteLine($"Saque de R$ {valor:0.00} realizado com sucesso!");
                            Console.WriteLine($"Novo saldo: R$ {novoSaldo:0.00}");
                        }
                        else
                        {
                            Console.WriteLine("Saldo insuficiente!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Valor inválido!");
                    }
                }
                else
                {
                    Console.WriteLine("Conta não encontrada!");
                }
            }
            else
            {
                Console.WriteLine("Número de conta inválido!");
            }

            Console.WriteLine("Pressione qualquer tecla para voltar ao menu inicial.");
            Console.ReadKey();
        }

        /// <summary>
        /// Realiza transferência entre duas contas existentes solicitando os números das contas e o valor ao usuário.
        /// </summary>
        static void Transferir()
        {
            Console.Clear();
            Console.WriteLine("--- Transferência ---");
            Console.Write("Digite o número da conta de origem: ");

            if (int.TryParse(Console.ReadLine(), out int contaOrigem))
            {
                var contaOrigemObj = _contaRepository.BuscarContaPorNumero(contaOrigem);
                if (contaOrigemObj != null)
                {
                    Console.Write("Digite o número da conta de destino: ");
                    if (int.TryParse(Console.ReadLine(), out int contaDestino))
                    {
                        // Verifica se a conta destino existe.
                        if (_contaRepository.ContaExiste(contaDestino))
                        {
                            Console.Write("Digite o valor da transferência: ");
                            if (decimal.TryParse(Console.ReadLine(), out decimal valor) && valor > 0)
                            {
                                if (valor <= contaOrigemObj.SaldoDaConta)
                                {
                                    // Debita da conta origem.
                                    decimal novoSaldoOrigem = contaOrigemObj.SaldoDaConta - valor;
                                    _contaRepository.AtualizarSaldo(contaOrigem, novoSaldoOrigem);

                                    // Credita na conta destino.
                                    var contaDestinoObj = _contaRepository.BuscarContaPorNumero(contaDestino);
                                    decimal novoSaldoDestino = contaDestinoObj.SaldoDaConta + valor;
                                    _contaRepository.AtualizarSaldo(contaDestino, novoSaldoDestino);

                                    // Registra a transação para o histórico.
                                    _transacaoRepository.RegistrarTransacao("TRANSFERENCIA", valor, contaOrigem, contaDestino);

                                    Console.WriteLine($"Transferência de R$ {valor:0.00} realizada com sucesso!");
                                    Console.WriteLine($"Novo saldo da conta {contaOrigem}: R$ {novoSaldoOrigem:0.00}");
                                }
                                else
                                {
                                    Console.WriteLine("Saldo insuficiente!");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Valor inválido!");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Conta destino não encontrada!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Número da conta destino inválido!");
                    }
                }
                else
                {
                    Console.WriteLine("Conta origem não encontrada!");
                }
            }
            else
            {
                Console.WriteLine("Número da conta origem inválido!");
            }

            Console.WriteLine("Pressione qualquer tecla para voltar ao menu inicial.");
            Console.ReadKey();
        }

        /// <summary>
        /// Consulta o histórico de transações de uma conta existente solicitando o número da conta ao usuário.
        /// </summary>
        static void ConsultarHistorico()
        {
            Console.Clear();
            Console.WriteLine("--- Histórico de Transações ---");
            Console.Write("Digite o número da conta: ");

            if (int.TryParse(Console.ReadLine(), out int numeroConta))
            {
                if (_contaRepository.ContaExiste(numeroConta))
                {
                    var transacoes = _transacaoRepository.BuscarTransacoesPorConta(numeroConta);

                    Console.WriteLine($"\nHistórico da Conta {numeroConta}:");
                    Console.WriteLine("Data/Hora | Tipo | Valor | Conta Relacionada");
                    Console.WriteLine(new string('-', 50));

                    // Exibe cada transação com detalhes.
                    foreach (var transacao in transacoes)
                    {
                        string contaRelacionada = transacao.Tipo switch
                        {
                            "DEPOSITO" => "Entrada",
                            "SAQUE" => "Saída",
                            "TRANSFERENCIA" => transacao.ContaOrigem == numeroConta ?
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
                else
                {
                    Console.WriteLine("Conta não encontrada!");
                }
            }
            else
            {
                Console.WriteLine("Número de conta inválido!");
            }

            Console.WriteLine("Pressione qualquer tecla para voltar ao menu inicial.");
            Console.ReadKey();
        }

        /// <summary>
        /// Lista todas as contas cadastradas no sistema e seus respectivos números.
        /// </summary>
        static void ListarContas()
        {
            Console.Clear();
            Console.WriteLine("--- Lista de Todas as Contas ---");

            var contas = _contaRepository.BuscarTodasContas();

            if (contas.Count > 0)
            {
                // Exibe os números e contas em formato de tabela.
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

            Console.WriteLine("\nPressione qualquer tecla para continuar...");
            Console.ReadKey();
        }
    }
}