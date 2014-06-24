using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.Configuration;


namespace MyORM
{
    public class MyAccessor<T>
    {
        private string connectionString;
        private string tableName;
        private string idField;
        private string[] fieldNames;
        private string[] fieldTypes;
        private PropertyInfo[] objProps;
        int idFieldIndex;

        public MyAccessor()
        {
            connectionString = ConfigurationManager.ConnectionStrings["MyORMConnectionString"].ConnectionString;
            GetTableInfo();
        }

        private void GetTableInfo()
        {

            Type entityType = typeof(T);

            tableName = ((TableAttribute)entityType.GetCustomAttribute(typeof(TableAttribute), false)).Name;

            PropertyInfo[] props = entityType.GetProperties();

            List<string> fieldNamesList = new List<string>();
            List<string> fieldTypesList = new List<string>();
            List<PropertyInfo> objPropsList = new List<PropertyInfo>();

            foreach (var p in props)
            {
                FieldAttribute fieldAttr = (FieldAttribute)p.GetCustomAttribute(typeof(FieldAttribute), false);

                if (fieldAttr != null)
                {
                    fieldNamesList.Add(fieldAttr.Name);
                    fieldTypesList.Add(fieldAttr.Type);
                    objPropsList.Add(p);
                }
            }

            fieldNames = fieldNamesList.ToArray();
            fieldTypes = fieldTypesList.ToArray();
            objProps = objPropsList.ToArray();



            //idField = ((TableAttribute)entityType.GetCustomAttribute(typeof(TableAttribute), false)).IdField;

            for (int i = 0; i < fieldNames.Length; i++)
            {
                if (objProps[i].Name.ToLower().Contains("id"))
                {
                    idFieldIndex = i;
                    break;
                }
            }

        }


        public List<T> GetAll()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = string.Format("SELECT {0} FROM {1}", string.Join(", ", fieldNames), tableName);
                SqlDataReader reader = cmd.ExecuteReader();

                List<T> result = new List<T>();

                while (reader.Read())
                {
                    var item = Activator.CreateInstance(typeof(T));

                    for (int i = 0; i < fieldNames.Length; i++)
                    {
                        objProps[i].SetValue(item, reader[fieldNames[i]]);
                    }

                    result.Add((T)item);
                }

                connection.Close();
                reader.Close();

                return result;
            }

        }

        public void DeleteById(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string query = string.Format("DELETE FROM {0} WHERE {1} = @Id", tableName, idField);

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void Insert(T item)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string[] fldNames = fieldNames.ToArray();
                fldNames = fldNames.Where(w => w != fldNames[idFieldIndex]).ToArray();
                string query = string.Format("INSERT INTO {0} ({1}) VALUES (@{2})", tableName, string.Join(", ", fldNames), string.Join(", @", fldNames));
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    for (int i = 0; i < fieldNames.Length; i++)
                    {
                        if (i != idFieldIndex)
                        {
                            cmd.Parameters.AddWithValue("@" + fieldNames[i], objProps[i].GetValue(item));
                        }
                    }
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }




        public T GetById(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = string.Format("SELECT {0} FROM {1} WHERE {2} = @Id", string.Join(", ", fieldNames), tableName, idField);

                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = query;

                cmd.Parameters.AddWithValue("@Id", id);

                SqlDataReader reader = cmd.ExecuteReader();

                var result = Activator.CreateInstance(typeof(T));

                if (reader.Read())
                {
                    for (int i = 0; i < fieldNames.Length; i++)
                    {
                        objProps[i].SetValue(result, reader[fieldNames[i]]);
                    }

                }

                cmd.ExecuteNonQuery();
                connection.Close();
                reader.Close();

                return (T)result;
            }
        }

    }
}
