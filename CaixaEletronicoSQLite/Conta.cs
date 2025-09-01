namespace CaixaEletronicoSQLite
{
    /// <summary>
    /// Classe que representa uma conta bancária.
    /// </summary>
    public class Conta
    {
        public int NumeroDaConta { get; set; }
        public string TitularDaConta { get; set; }
        public decimal SaldoDaConta { get; set; }

        // Construtor vazio necessário para o SQLite.
        public Conta() { }

        //Construtor para criar uma nova conta com titular.
        public Conta(string titularDaConta)
        {
            if (string.IsNullOrWhiteSpace(titularDaConta))
                throw new ArgumentException("O nome do titular não pode estar vazio.");

            TitularDaConta = titularDaConta;
            SaldoDaConta = 0;
        }

        //Retorna uma representação em string formatada da conta.
        public override string ToString()
        {
            return $"Conta: {NumeroDaConta} - Titular: {TitularDaConta} - Saldo: R${SaldoDaConta:0.00}";
        }
    }
}