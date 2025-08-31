namespace CaixaEletronicoSQLite
{
    public class Conta
    {
        private static int contadorDeContas = 0;

        public int NumeroDaConta { get; private set; }
        public string TitularDaConta { get; private set; }
        public decimal SaldoDaConta { get; private set; }

        public Conta(string titulardaconta)
        {
            if (string.IsNullOrWhiteSpace(titulardaconta))
                throw new ArgumentException("O nome do titular não pode estar vazio.");

            NumeroDaConta = ++contadorDeContas;
            TitularDaConta = titulardaconta;
            SaldoDaConta = 0;
        }

        public void Depositar(decimal valordaconta)
        {
            if (valordaconta <= 0)
                throw new ArgumentException("O valor do depósito deve ser maior que zero.");

            SaldoDaConta += valordaconta;
            Console.WriteLine($"Depósito de R${valordaconta:0.00} realizado com sucesso!");
        }

        public void Sacar(decimal valordaconta)
        {
            if (valordaconta <= 0)
                throw new ArgumentException("O valor do saque deve ser maior que zero.");

            if (valordaconta > SaldoDaConta)
                throw new ArgumentException("Saldo insuficiente.");

            SaldoDaConta -= valordaconta;
            Console.WriteLine($"Saque de R${valordaconta:0.00} realizado com sucesso.");
        }

        public void ConsultarSaldo()
        {
            Console.WriteLine($"Conta {NumeroDaConta} - Titular: {TitularDaConta} - Saldo atual: R${SaldoDaConta: 0.00}");
        }

        public override string ToString()
        {
            return $"Conta: {NumeroDaConta} - Titular: {TitularDaConta} - Saldo: R$:{SaldoDaConta}";
        }
    }
}
