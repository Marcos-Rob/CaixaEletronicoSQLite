namespace CaixaEletronicoSQLite
{
    /// <summary>
    /// Classe que representa uma transação bancária.
    /// Registra depósitos, saques e transferências entre contas.
    /// </summary>
    public class Transacao
    {
        public int Id { get; set; }
        public string Tipo { get; set; } // "DEPOSITO", "SAQUE", "TRANSFERENCIA"
        public decimal Valor { get; set; }
        public DateTime DataHora { get; set; }
        public int? ContaOrigem { get; set; }
        public int? ContaDestino { get; set; }
    }
}