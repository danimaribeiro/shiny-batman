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

        public void Save(BaseModel model, long id)
        {
            string consulta = string.Empty;
            if (id > 0)
                consulta = GenerateUpdate(model, id);
            else
                consulta = GenerateInsert(model);

            Npgsql.NpgsqlConnection conexao = new Npgsql.NpgsqlConnection("Server=127.0.0.1;Port=5432;Database=padrao;User ID=desenvolvimento;Password=12345;");            
            Npgsql.NpgsqlCommand comando = new Npgsql.NpgsqlCommand(consulta, conexao);
            try
            {
                foreach (var item in model.Properties)
                {
                    if (item.ActualValue != null)
                    {
                        if (id > 0 && item.CanUpdate && !item.IsPrimaryKey)
                        {
                            //TODO Adicionar os parametros conforme o tipo
                        }
                        else if (id <= 0 && item.CanInsert && !item.IsPrimaryKey)
                        {
                            //TODO Adicionar os parametros conforme o tipo
                        }
                    }
                }
                conexao.Open();
                comando.ExecuteNonQuery();                
            }
            finally
            {
                conexao.Close();
            }
        }

        public void Delete(BaseModel model, long id)
        {
            throw new NotImplementedException();
        }


        private string GenerateInsert(BaseModel model)
        {
            string parameters = "";
            string values = "";

            foreach (var item in model.Properties)
            {
                item.ActualValue = model[item.Name];
                if (item.CanInsert && !item.IsPrimaryKey)
                {
                    parameters += item.Name + ",";
                    values += "@" + item.Name + ",";
                }
            }
            return  string.Format( "insert into {0} ({1}) values ({2}) returning {3};", model.TableName, parameters, values, model.Properties[0].Name);
        }

        private string GenerateUpdate(BaseModel model, long id)
        {
            string parameters = "";
            
            foreach (var item in model.Properties)
            {
                item.ActualValue = model[item.Name];
                if (item.CanUpdate && !item.IsPrimaryKey)
                {
                    parameters += item.Name + "=" + item.Name + ",";
                }
            }
            return string.Format("update {0} set {1} where {2}", model.TableName, parameters, model.Properties[0].Name);
        }
    }
}