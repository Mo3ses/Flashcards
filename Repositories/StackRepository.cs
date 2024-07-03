using System.Data.SqlClient;
using Dapper;
using Flashcards.Models;
using Microsoft.Extensions.Configuration;

namespace Flashcards.Repositories
{
    public class StackRepository
    {
        private readonly string _connectionString;

        public StackRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("FCConnection");
        }

        public IEnumerable<Stack> GetAllStacks()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<Stack>("SELECT * FROM Stacks");
            }
        }

        public Stack GetStackById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QueryFirstOrDefault<Stack>("SELECT * FROM Stacks WHERE StackId = @Id", new { Id = id });
            }
        }

        public void AddStack(Stack stack)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute("INSERT INTO Stacks (Name) VALUES (@Name)", stack);
            }
        }

        public void DeleteStack(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute("DELETE FROM Stacks WHERE StackId = @Id", new { Id = id });
            }
        }
    }

}