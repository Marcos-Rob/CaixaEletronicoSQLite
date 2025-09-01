using System.Data.SQLite;

namespace CaixaEletronicoSQLite
{
    /// <summary>
    /// Classe responsável por operações de banco de dados relacionadas a tabela contas.
    /// </summary>
    public class ContaRepository
    {
        private readonly string _connectionString;

        /// <summary>
        /// Construtor que inicializa o repositório com a string de conexão.
        /// </summary>
        public ContaRepository(string connectionString = "Data Source=banco.db;Version=3;")
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Insere uma nova conta no banco de dados.
        /// </summary>
        public void CriarConta(Conta conta)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = "INSERT INTO Contas (TitularDaConta, SaldoDaConta) VALUES (@titular, @saldo)";

                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    // Parametriza a query para evitar SQL Injection
                    cmd.Parameters.AddWithValue("@titular", conta.TitularDaConta);
                    cmd.Parameters.AddWithValue("@saldo", conta.SaldoDaConta);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Busca uma conta pelo número único.
        /// </summary>
        public Conta BuscarContaPorNumero(int numeroConta)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM Contas WHERE NumeroDaConta = @numero";

                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@numero", numeroConta);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Conta
                            {
                                NumeroDaConta = Convert.ToInt32(reader["NumeroDaConta"]),
                                TitularDaConta = reader["TitularDaConta"].ToString(),
                                SaldoDaConta = Convert.ToDecimal(reader["SaldoDaConta"])
                            };
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Atualiza o saldo de uma conta existente.
        /// </summary>
        public void AtualizarSaldo(int numeroConta, decimal novoSaldo)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = "UPDATE Contas SET SaldoDaConta = @saldo WHERE NumeroDaConta = @numero";

                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@saldo", novoSaldo);
                    cmd.Parameters.AddWithValue("@numero", numeroConta);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Verifica se uma conta existe pelo número.
        /// </summary>
        public bool ContaExiste(int numeroConta)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT COUNT(1) FROM Contas WHERE NumeroDaConta = @numero";

                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@numero", numeroConta);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        /// <summary>
        /// Busca a última conta criada para um titular específico.
        /// </summary>
        public Conta BuscarUltimaContaPorTitular(string titular)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM Contas WHERE TitularDaConta = @titular ORDER BY NumeroDaConta DESC LIMIT 1";

                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@titular", titular);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Conta
                            {
                                NumeroDaConta = Convert.ToInt32(reader["NumeroDaConta"]),
                                TitularDaConta = reader["TitularDaConta"].ToString(),
                                SaldoDaConta = Convert.ToDecimal(reader["SaldoDaConta"])
                            };
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Retorna todas as contas do banco de dados.
        /// </summary>
        public List<Conta> BuscarTodasContas()
        {
            var contas = new List<Conta>();

            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM Contas ORDER BY NumeroDaConta";

                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            contas.Add(new Conta
                            {
                                NumeroDaConta = Convert.ToInt32(reader["NumeroDaConta"]),
                                TitularDaConta = reader["TitularDaConta"].ToString(),
                                SaldoDaConta = Convert.ToDecimal(reader["SaldoDaConta"])
                            });
                        }
                    }
                }
            }
            return contas;
        }
    }
}