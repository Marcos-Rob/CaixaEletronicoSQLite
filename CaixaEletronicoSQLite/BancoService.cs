using System.Data.SQLite;

namespace CaixaEletronicoSQLite
{
    /// <summary>
    /// Classe de serviço que contém as regras de negócio do sistema bancário
    /// </summary>
    public class BancoService
    {
        private readonly ContaRepository _contaRepository;
        private readonly TransacaoRepository _transacaoRepository;

        public BancoService(string connectionString = "Data Source=banco.db;Version=3;")
        {
            _contaRepository = new ContaRepository(connectionString);
            _transacaoRepository = new TransacaoRepository(connectionString);
        }

        /// <summary>
        /// Cria uma nova conta bancária
        /// </summary>
        public void CriarConta(string titular)
        {
            if (string.IsNullOrWhiteSpace(titular))
                throw new ArgumentException("O nome do titular não pode estar vazio.");

            var novaConta = new Conta(titular);
            _contaRepository.CriarConta(novaConta);
        }

        /// <summary>
        /// Busca a última conta criada por um titular
        /// </summary>
        public Conta BuscarUltimaContaPorTitular(string titular)
        {
            return _contaRepository.BuscarUltimaContaPorTitular(titular)
                ?? throw new Exception("Conta não encontrada");
        }

        /// <summary>
        /// Busca uma conta completa pelo número
        /// </summary>
        public Conta BuscarConta(int numeroConta)
        {
            var conta = _contaRepository.BuscarContaPorNumero(numeroConta);
            return conta ?? throw new Exception("Conta não encontrada");
        }

        /// <summary>
        /// Realiza um depósito em uma conta
        /// </summary>
        public void Depositar(int numeroConta, decimal valor)
        {
            ValidarValorPositivo(valor);
            var conta = BuscarContaValida(numeroConta);

            decimal novoSaldo = conta.SaldoDaConta + valor;
            _contaRepository.AtualizarSaldo(numeroConta, novoSaldo);
            _transacaoRepository.RegistrarTransacao("DEPOSITO", valor, null, numeroConta);
        }

        /// <summary>
        /// Realiza um saque em uma conta
        /// </summary>
        public void Sacar(int numeroConta, decimal valor)
        {
            ValidarValorPositivo(valor);
            var conta = BuscarContaValida(numeroConta);
            ValidarSaldoSuficiente(conta, valor);

            decimal novoSaldo = conta.SaldoDaConta - valor;
            _contaRepository.AtualizarSaldo(numeroConta, novoSaldo);
            _transacaoRepository.RegistrarTransacao("SAQUE", valor, numeroConta, null);
        }

        /// <summary>
        /// Realiza uma transferência entre contas
        /// </summary>
        public void Transferir(int contaOrigem, int contaDestino, decimal valor)
        {
            ValidarValorPositivo(valor);

            var origem = BuscarContaValida(contaOrigem);
            var destino = BuscarContaValida(contaDestino);

            ValidarSaldoSuficiente(origem, valor);

            // Debita da conta origem
            decimal novoSaldoOrigem = origem.SaldoDaConta - valor;
            _contaRepository.AtualizarSaldo(contaOrigem, novoSaldoOrigem);

            // Credita na conta destino
            decimal novoSaldoDestino = destino.SaldoDaConta + valor;
            _contaRepository.AtualizarSaldo(contaDestino, novoSaldoDestino);

            // Registra a transação
            _transacaoRepository.RegistrarTransacao("TRANSFERENCIA", valor, contaOrigem, contaDestino);
        }

        /// <summary>
        /// Consulta o saldo de uma conta
        /// </summary>
        public decimal ConsultarSaldo(int numeroConta)
        {
            var conta = BuscarContaValida(numeroConta);
            return conta.SaldoDaConta;
        }

        /// <summary>
        /// Busca o histórico de transações de uma conta
        /// </summary>
        public List<Transacao> ConsultarHistorico(int numeroConta)
        {
            if (!_contaRepository.ContaExiste(numeroConta))
                throw new Exception("Conta não encontrada");

            return _transacaoRepository.BuscarTransacoesPorConta(numeroConta);
        }

        /// <summary>
        /// Lista todas as contas do sistema
        /// </summary>
        public List<Conta> ListarTodasContas()
        {
            return _contaRepository.BuscarTodasContas();
        }

        /// <summary>
        /// Busca uma conta válida pelo número
        /// </summary>
        private Conta BuscarContaValida(int numeroConta)
        {
            var conta = _contaRepository.BuscarContaPorNumero(numeroConta);
            return conta ?? throw new Exception("Conta não encontrada");
        }

        /// <summary>
        /// Valida se o valor é positivo
        /// </summary>
        private void ValidarValorPositivo(decimal valor)
        {
            if (valor <= 0)
                throw new ArgumentException("O valor deve ser maior que zero.");
        }

        /// <summary>
        /// Valida se há saldo suficiente para a operação
        /// </summary>
        private void ValidarSaldoSuficiente(Conta conta, decimal valor)
        {
            if (valor > conta.SaldoDaConta)
                throw new InvalidOperationException("Saldo insuficiente para realizar a operação.");
        }
    }
}