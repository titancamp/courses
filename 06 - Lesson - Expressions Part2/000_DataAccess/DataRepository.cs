using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace _000_DataAccess
{
    public class DataRepository
    {
        public DataRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private readonly string _connectionString;

        public IEnumerable<TModel> Execute<TModel>(string query, Func<IDataRecord, TModel> convert)
        {
            foreach (var reader in AsEnumerable(query))
            {
                yield return convert(reader);
            }
        }

        public IEnumerable<IDataRecord> AsEnumerable(string query)
        {
            using var conn = new SqlConnection(_connectionString);
            if (conn.State != ConnectionState.Open)
                conn.Open();

            using var comand = conn.CreateCommand();
            comand.CommandType = CommandType.Text;
            comand.CommandText = query;
            var reader = comand.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                    yield return reader;
            }
        }
    }
}
