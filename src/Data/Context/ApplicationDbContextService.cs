using Core.Interfaces;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Context
{
    public class ApplicationDbContextService : IDbContextService
    {
        private string _sqlConnectionString;

        private IDbConnection DbConnection
        {
            get => new SqlConnection(_sqlConnectionString);
            set { }
        }

        public ApplicationDbContextService(string sqlConnection)
            => _sqlConnectionString = sqlConnection;

        public async Task<T> MakeFirstOrDefaultAsync<T>(string query, object? param = null)
        {
            try
            {
                DbConnection.Open();

                T res = await DbConnection.QueryFirstOrDefaultAsync<T>(query, param);

                DbConnection.Close();

                return res;
            }
            catch (Exception exp)
            {
                return default(T);
            }
        }

        public async Task ExecuteAsync(string query, object? param = null)
        {
            try
            {
                DbConnection.Open();

                await DbConnection.ExecuteAsync(query, param);

                DbConnection.Close();
            }
            catch (Exception exp)
            { 
                //logger
            }
        }

        public async Task<IEnumerable<T>> MakeQueryAsync<T>(string query, object? param = null)
        {
            try
            {
                DbConnection.Open();

                IEnumerable<T> res = await DbConnection.QueryAsync<T>(query, param);

                DbConnection.Close();

                return res;
            }
            catch (Exception exp)
            {
                //logger
                return Enumerable.Empty<T>();
            }
        }        
    }
}
