using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace shiny_batman.OrmBatman.Postgres
{
    public class ModelQuery : IModelQuery
    {
        public long ModelCount(BaseModel model)
        {
            Npgsql.NpgsqlConnection conexao = new Npgsql.NpgsqlConnection("Server=127.0.0.1;Port=5432;Database=padrao;User ID=desenvolvimento;Password=12345;");

            string query = string.Format("select count(*) from {0};", model.TableName);
            Npgsql.NpgsqlCommand comando = new Npgsql.NpgsqlCommand(query, conexao);

            try
            {
                long total = 0;
                conexao.Open();
                total = (long)comando.ExecuteScalar();
                return total;
            }
            finally
            {
                conexao.Close();
            }
        }

        public object GetModel(BaseModel model, long id)
        {
            Npgsql.NpgsqlConnection conexao = new Npgsql.NpgsqlConnection("Server=127.0.0.1;Port=5432;Database=padrao;User ID=desenvolvimento;Password=12345;");

            string query = string.Format("select * from {0} where {1} = {2};", model.TableName, model.Properties.Find(x => x.IsPrimaryKey).Name, id);
            Npgsql.NpgsqlCommand comando = new Npgsql.NpgsqlCommand(query, conexao);

            try
            {
                var dict = new Dictionary<string, object>();
                conexao.Open();
                var reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    foreach (var item in model.Properties)
                    {
                        dict.Add(item.Name, reader[item.Name.ToLower()]);
                    }
                }
                reader.Close();
                return dict;
            }
            finally
            {
                conexao.Close();
            }
        }

        public List<object> QueryModel(BaseModel model, int maxResults, int page)
        {
            Npgsql.NpgsqlConnection conexao = new Npgsql.NpgsqlConnection("Server=127.0.0.1;Port=5432;Database=padrao;User ID=desenvolvimento;Password=12345;");

            string query = string.Format("select * from {0} limit {1} offset {2};", model.TableName, maxResults, (page * maxResults));
            Npgsql.NpgsqlCommand comando = new Npgsql.NpgsqlCommand(query, conexao);

            try
            {
                List<object> retorno = new List<object>();
                conexao.Open();
                var reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    var dict = new Dictionary<string, object>();
                    foreach (var item in model.Properties)
                    {
                        dict.Add(item.Name, reader[item.Name.ToLower()]);
                    }
                    retorno.Add(dict);
                }
                reader.Close();
                return retorno;
            }
            finally
            {
                conexao.Close();
            }
        }

    }
}