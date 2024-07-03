using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Flashcards
{
    public class DatabaseInitializer
    {
        private readonly string _connectionString;
        private readonly string _databaseName;

        public DatabaseInitializer(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("FCConnection");
            _databaseName = config["DatabaseSettings:DatabaseName"];
        }

        public void InitializeDatabase()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var checkDbQuery = $"SELECT db_id('{_databaseName}')";
                var databaseExists = connection.ExecuteScalar<int?>(checkDbQuery) != null;

                if (!databaseExists)
                {
                    var createDbQuery = $"CREATE DATABASE [{_databaseName}]";
                    connection.Execute(createDbQuery);

                    var createTablesQuery = $@"
                    USE [{_databaseName}];

                    CREATE TABLE Stacks (
                        StackId INT PRIMARY KEY IDENTITY(1,1),
                        Name NVARCHAR(100) UNIQUE NOT NULL
                    );

                    CREATE TABLE Flashcards (
                        FlashcardId INT PRIMARY KEY IDENTITY(1,1),
                        StackId INT NOT NULL,
                        Question NVARCHAR(255) NOT NULL,
                        Answer NVARCHAR(255) NOT NULL,
                        FOREIGN KEY (StackId) REFERENCES Stacks(StackId) ON DELETE CASCADE
                    );

                    CREATE TABLE StudySessions (
                        SessionId INT PRIMARY KEY IDENTITY(1,1),
                        StackId INT NOT NULL,
                        Date DATETIME NOT NULL,
                        Score INT NOT NULL,
                        FOREIGN KEY (StackId) REFERENCES Stacks(StackId) ON DELETE CASCADE
                    );
                ";

                    connection.Execute(createTablesQuery);
                }
            }
        }
    }

}