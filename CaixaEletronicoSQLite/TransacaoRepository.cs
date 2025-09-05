using System.Data.SQLite;

namespace CaixaEletronicoSQLite
{
    /// <summary>
    /// Classe responsável por operações de banco de dados relacionadas a tabela transações.
    /// </summary>
    /// <remarks>
    /// Construtor que inicializa o repositório com a string de conexão.
    /// </remarks>
    public class TransacaoRepository(string connectionString = "Data Source=banco.db;Version=3;")
    {
        private readonly string _connectionString = connectionString;

        /// <summary>
        /// Registra uma nova transação no banco de dados.
        /// </summary>
        public void RegistrarTransacao(string tipo, decimal valor, int? contaOrigem = null, int? contaDestino = null)
        {
            using var conn = new SQLiteConnection(_connectionString);
            conn.Open();
            string sql = @"INSERT INTO Transacoes (Tipo, Valor, DataHora, ContaOrigem, ContaDestino) 
                              VALUES (@tipo, @valor, @dataHora, @contaOrigem, @contaDestino)";

            using var cmd = new SQLiteCommand(sql, conn);
            // Parametriza a query para evitar SQL Injection.
            cmd.Parameters.AddWithValue("@tipo", tipo);
            cmd.Parameters.AddWithValue("@valor", valor);
            cmd.Parameters.AddWithValue("@dataHora", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            // Trata valores nulos para contas de origem e destino convertendo para DBNull.
            cmd.Parameters.AddWithValue("@contaOrigem", contaOrigem ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@contaDestino", contaDestino ?? (object)DBNull.Value);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Busca todas as transações relacionadas a uma conta específica.
        /// </summary>
        public List<Transacao> BuscarTransacoesPorConta(int numeroConta)
        {
            var transacoes = new List<Transacao>();

            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = @"SELECT * FROM Transacoes 
                              WHERE ContaOrigem = @conta OR ContaDestino = @conta 
                              ORDER BY DataHora DESC";

                using var cmd = new SQLiteCommand(sql, conn);
                cmd.Parameters.AddWithValue("@conta", numeroConta);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    // Mapeia os dados do banco para o objeto Transacao.
                    // Trata valores NULL do banco para null no objeto.
                    transacoes.Add(new Transacao
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Tipo = reader["Tipo"].ToString(),
                        Valor = Convert.ToDecimal(reader["Valor"]),
                        DataHora = DateTime.Parse(reader["DataHora"].ToString()),
                        ContaOrigem = reader["ContaOrigem"] is DBNull ? null : (int?)Convert.ToInt32(reader["ContaOrigem"]),
                        ContaDestino = reader["ContaDestino"] is DBNull ? null : (int?)Convert.ToInt32(reader["ContaDestino"])
                    });
                }
            }
            return transacoes;
        }
    }
}