using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    public class Database
    {
        private static String Fields(Dictionary<String,Object> fields,int num)
        {
            StringBuilder builder = new StringBuilder();
            if (num == 0)
            {
                foreach (var key in fields)
                {
                    builder.Append(key.Key + ",");
                }
            }
            else if(num == 2){

                foreach (var key in fields)
                {
                    builder.Append(key.Key +"="+"?"+ ",");
                }
            }
            else
            {
                foreach (var key in fields)
                {
                    builder.Append("?"+ ",");
                }
            }
            return builder.ToString().TrimEnd(',');
        }
        private static void Parameter(IDbCommand command,Dictionary<String,Object> param)
        {

                foreach (var p in param)
                {
                    var pa = command.CreateParameter();
                    pa.ParameterName = p.Key;
                    pa.Value = p.Value;

                    command.Parameters.Add(pa);
                }
                command.ExecuteNonQuery();


        }
        IDbConnection GLOBAL_CONNECTION;
        IDbCommand command;
        IDbDataAdapter adapter;
        public Database(IDbConnection connection, IDbCommand Command)
        {
            GLOBAL_CONNECTION = connection;
            GLOBAL_CONNECTION.ConnectionString = connection.ConnectionString;
            command = Command;

        }
        void Init()
        {
            GLOBAL_CONNECTION.Open();  
            command.Connection = GLOBAL_CONNECTION;
        }
    public void setAdapter(IDbDataAdapter Adapter)
    {
        adapter = Adapter;
    }


       /// <summary>
       /// Insert Item to Database 
       /// </summary>
       /// <param name="Table_Name"></param>
       /// <param name="Data"></param>
        public void Insert(String Table_Name, Dictionary<String,Object>  Data)
        {
            try {
                Init();
                command.CommandText = $@"INSERT INTO {Table_Name} ({Fields(Data, 0)}) VALUES ({Fields(Data, 1)})";
                Parameter(command, Data);
             
            }
            catch (Exception ex) { throw ex; }
            finally { GLOBAL_CONNECTION.Close(); }


        }

    /// <summary>
    /// Bulk Insert Into Database : Not Done Yet
    /// </summary>
    /// <param name="Table_Name"></param>
    /// <param name="Data"></param>
    public void Bulk_Insert(String Table_Name,Dictionary<String,Object[]> Data)
    {
        try
        {
            Init();
            int i = 0;
            foreach (var Key in Data.Keys)
            {
                foreach (var dataSet in Data.Values)
                {
                   
                    command.CommandText = $@"INSERT INTO {Table_Name} ({Key}) VALUES (""{dataSet[i]}"")";
                    command.ExecuteNonQuery();
                    i++;
                }
            }
         


        }
        catch (Exception ex) { throw ex; }
        finally { GLOBAL_CONNECTION.Close(); }
    }

    /// <summary>
    /// Delete something to Database
    /// </summary>
    /// <param name="Table_Name"></param>
    /// <param name="Data"></param>
        public void Delete(String Table_Name, Dictionary<String, Object> Data)
        {
            try
            {


                Init();
                command.Connection = GLOBAL_CONNECTION;
                command.CommandText = $@"DELETE FROM {Table_Name} WHERE {Fields(Data, 0)}={Fields(Data, 2)}";
                Parameter(command, Data);
               
            }
            catch (Exception ex) { throw ex; }
            finally { GLOBAL_CONNECTION.Close(); }

        }

    /// <summary>
    /// Delete something to Database
    /// </summary>
    /// <param name="Table_Name"></param>
    /// <param name="Condition"></param>
        public void Delete(String Table_Name, String Condition)
        {
            try {
                Init();
                StringBuilder builder = new StringBuilder();
                if (Condition != "")
                {
                    builder.Append($@"DELETE FROM {Table_Name} ");
                    builder.Append($@"WHERE {Condition}");
                }
                else
                {
                    builder.Append($@"DELETE {Table_Name} ");
                }
                command.CommandText = builder.ToString();

                command.ExecuteNonQuery();
                //  Parameter(command, Data);
            
            }
            catch (Exception ex) { throw ex; }
            finally { GLOBAL_CONNECTION.Close(); }
        }

    /// <summary>
    /// Update data from Database
    /// </summary>
    /// <param name="Table_Name"></param>
    /// <param name="Data"></param>
    /// <param name="Condition"></param>
        public void Update(String Table_Name,Dictionary<String,Object> Data,String Condition)
        {
            try {
                Init();
                StringBuilder builder = new StringBuilder();
                builder.Append($@"UPDATE {Table_Name} SET {Fields(Data, 2)} ");
                if (Condition != "")
                {
                    builder.Append($@"WHERE {Condition}");
                }
                command.CommandText = builder.ToString();
                Parameter(command, Data);
                
            }
            catch (Exception ex) { throw ex; }
            finally { GLOBAL_CONNECTION.Close(); }
        }


    /// <summary>
    /// Returns DataTable for source of Data, from Database
    /// </summary>
    /// <param name="Table_Name"></param>
    /// <param name="Data"></param>
    /// <param name="Condition"></param>
    /// <returns></returns>
    public DataTable Select(String Table_Name,Dictionary<String,Object> Data,String Condition)
    {
        DataSet ds = new DataSet();
        try
        {
            Init();
            StringBuilder builder = new StringBuilder();
            if (Data == null)
            {
                builder.Append($@"SELECT * from {Table_Name} ");
                if (Condition != "")
                {
                    builder.Append($@"WHERE {Condition}");
                }
                command.CommandText = builder.ToString();
                
            }
            else
            {

                builder.Append($@"SELECT {Fields(Data, 0)} from {Table_Name} ");
                if (Condition != "")
                {
                    builder.Append($@"WHERE {Condition}");
                }
                command.CommandText = builder.ToString();
              
              
            }
            adapter.SelectCommand = command;
            adapter.Fill(ds);

        }
        catch (Exception ex) { throw ex; }
        finally { GLOBAL_CONNECTION.Close(); }
        
      
        return ds.Tables[0];
    }

    }

