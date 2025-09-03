using System.Data.SQLite;

namespace CaixaEletronicoSQLite
{
    /// <summary>
    /// Classe responsável pela configuração e inicialização do banco de dados SQLite.
    /// </summary>
    public class DatabaseContext
    {
        // String de conexão com o banco de dados.
        private const string ConnectionString = "Data Source=banco.db;Version=3;";

        // Construtor que inicializa o banco de dados ao criar uma instância da classe.
        public DatabaseContext()
        {
            InitializeDatabase();
        }

        /// <summary>
        /// Fornece uma nova conexão com o banco de dados SQLite.
        /// </summary>
        public static SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(ConnectionString);
        }

        /// <summary>
        /// Inicializa o banco de dados criando as tabelas necessárias se elas não existirem.
        /// Este método é chamado no construtor da classe.
        /// </summary>
        private static void InitializeDatabase()
        {
            using var conn = GetConnection();
            conn.Open();

            // 1. Cria tabela de Contas se não existir.
            string createContasTable = @"
                    CREATE TABLE IF NOT EXISTS Contas (
                        NumeroDaConta INTEGER PRIMARY KEY AUTOINCREMENT,
                        TitularDaConta TEXT NOT NULL,
                        SaldoDaConta DECIMAL NOT NULL DEFAULT 0
                    )";

            // 2. Cria tabela de Transações se não existir.
            string createTransacoesTable = @"
                    CREATE TABLE IF NOT EXISTS Transacoes (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Tipo TEXT NOT NULL,
                        Valor DECIMAL NOT NULL,
                        DataHora TEXT NOT NULL,
                        ContaOrigem INTEGER,
                        ContaDestino INTEGER,
                        FOREIGN KEY (ContaOrigem) REFERENCES Contas(NumeroDaConta),
                        FOREIGN KEY (ContaDestino) REFERENCES Contas(NumeroDaConta)
                    )";

            // Executa os comandos SQL para criar as tabelas.
            using (var command = new SQLiteCommand(createContasTable, conn))
            {
                command.ExecuteNonQuery();
            }

            using (var command = new SQLiteCommand(createTransacoesTable, conn))
            {
                command.ExecuteNonQuery();
            }
        }
    }
}