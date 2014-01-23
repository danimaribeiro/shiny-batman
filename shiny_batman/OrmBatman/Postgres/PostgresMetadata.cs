using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace shiny_batman.OrmBatman.Postgres
{
    public class PostgresMetadata : IMetadata
    {
        public List<BaseModel> InspectDatabase()
        {
            Npgsql.NpgsqlConnection conexao = new Npgsql.NpgsqlConnection("Server=127.0.0.1;Port=5432;Database=padrao;User ID=desenvolvimento;Password=12345;");
            Npgsql.NpgsqlCommand comando = new Npgsql.NpgsqlCommand("SELECT tablename, description FROM pg_catalog.pg_tables inner join pg_class on tablename = relname " +
                        "left join pg_description on pg_description.objoid = pg_class.oid where schemaname='public' order by tablename", conexao);

            try
            {
                List<BaseModel> databaseTables = new List<BaseModel>();
                conexao.Open();
                var reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    var model = new BaseModel();
                    model.TableName = reader.GetString(0);
                    model.Name = model.TableName;
                    model.Description = reader.IsDBNull(1) ? "" : reader.GetString(1);
                    databaseTables.Add(model);
                }
                reader.Close();
                foreach (var item in databaseTables)
                {
                    item.Properties = InspectModel(item.TableName).Properties;
                }
                return databaseTables;
            }
            finally
            {
                conexao.Close();
            }
        }


        public BaseModel InspectModel(string model)
        {
            var item = new BaseModel();
            item.TableName = model;
            item.Name = model;

            Npgsql.NpgsqlConnection conexao = new Npgsql.NpgsqlConnection("Server=127.0.0.1;Port=5432;Database=padrao;User ID=desenvolvimento;Password=12345;");

            string query = "SELECT ordinal_position, column_name,data_type,column_default,is_nullable,character_maximum_length, " +
                    "numeric_precision FROM information_schema.columns WHERE table_name = '" + model + "' ORDER BY ordinal_position;";

            Npgsql.NpgsqlCommand comandoColunas = new Npgsql.NpgsqlCommand(query, conexao);
            try
            {
                conexao.Open();
                var readerColunas = comandoColunas.ExecuteReader();
                List<Property> properties = new List<Property>();
                while (readerColunas.Read())
                {
                    var prop = new Property();
                    prop.IsPrimaryKey = readerColunas.GetInt32(0) == 1 ? true : false;
                    prop.LongDescription = readerColunas.GetString(1);
                    prop.ShortDescription = prop.LongDescription;
                    prop.Name = prop.LongDescription;
                    prop.Size = readerColunas.IsDBNull(5) ? 0 : readerColunas.GetInt32(5);
                    prop.Required = readerColunas.GetString(4) == "YES" ? false : true ;
                    prop.Type = readerColunas.GetString(1);
                    prop.RelatedModel = "";
                    prop.RelatedColumn = "";
                    properties.Add(prop);
                }
                readerColunas.Close();
                item.Properties = properties;
            }
            finally
            {
                conexao.Close();
            }
            return item;
        }
    }
}