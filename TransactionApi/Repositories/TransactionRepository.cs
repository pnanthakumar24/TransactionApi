using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;
using TransactionApi.Data;
using TransactionApi.Models;

namespace TransactionApi.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _db;
        public TransactionRepository(AppDbContext db) => _db = db;

        public async Task<Transaction> CreateAsync(Transaction transaction)
        {
            var conn = _db.Database.GetDbConnection();
            try
            {
                if (conn.State != ConnectionState.Open)
                    await conn.OpenAsync();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = "sp_CreateTransaction";
                cmd.CommandType = CommandType.StoredProcedure;

                var p = cmd.CreateParameter(); p.ParameterName = "@UserId"; p.Value = transaction.UserId; cmd.Parameters.Add(p);
                p = cmd.CreateParameter(); p.ParameterName = "@CardMasked"; p.Value = transaction.CardMasked; cmd.Parameters.Add(p);
                p = cmd.CreateParameter(); p.ParameterName = "@CardHolder"; p.Value = transaction.CardHolder; cmd.Parameters.Add(p);
                p = cmd.CreateParameter(); p.ParameterName = "@Amount"; p.Value = transaction.Amount; cmd.Parameters.Add(p);
                p = cmd.CreateParameter(); p.ParameterName = "@Currency"; p.Value = transaction.Currency; cmd.Parameters.Add(p);
                p = cmd.CreateParameter(); p.ParameterName = "@Status"; p.Value = transaction.Status; cmd.Parameters.Add(p);

                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    var created = new Transaction
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                        CardMasked = reader.GetString(reader.GetOrdinal("CardMasked")),
                        CardHolder = reader.GetString(reader.GetOrdinal("CardHolder")),
                        Amount = reader.GetDecimal(reader.GetOrdinal("Amount")),
                        Currency = reader.GetString(reader.GetOrdinal("Currency")),
                        Status = reader.GetString(reader.GetOrdinal("Status")),
                        CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                    };

                    return created;
                }

                throw new InvalidOperationException("Stored procedure did not return the created transaction.");
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    await conn.CloseAsync();
            }
        }

        public async Task<IEnumerable<Transaction>> GetByUserAsync(int userId)
        {
            var results = new List<Transaction>();
            var conn = _db.Database.GetDbConnection();
            try
            {
                if (conn.State != ConnectionState.Open)
                    await conn.OpenAsync();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = "sp_GetTransactionsByUser";
                cmd.CommandType = CommandType.StoredProcedure;

                var p = cmd.CreateParameter(); p.ParameterName = "@UserId"; p.Value = userId; cmd.Parameters.Add(p);

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var t = new Transaction
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                        CardMasked = reader.GetString(reader.GetOrdinal("CardMasked")),
                        CardHolder = reader.GetString(reader.GetOrdinal("CardHolder")),
                        Amount = reader.GetDecimal(reader.GetOrdinal("Amount")),
                        Currency = reader.GetString(reader.GetOrdinal("Currency")),
                        Status = reader.GetString(reader.GetOrdinal("Status")),
                        CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                    };
                    results.Add(t);
                }
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    await conn.CloseAsync();
            }

            return results;
        }
    }
}