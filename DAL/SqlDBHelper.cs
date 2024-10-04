using Dapper;
using MaxiSwitch.EncryptionDecryption;

using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;
using System.Xml.Linq;

namespace DAL
{
    public class SqlDBHelper
    {
        private readonly IConfiguration _configuration;
        static string CONNECTION_STRING = string.Empty;
        static string DabType = string.Empty;
        public SqlDBHelper(IConfiguration configuration)
        {
            _configuration = configuration;

            ConnectionStringEncryptDecrypt connectionStringEncryptDecrypt = new ConnectionStringEncryptDecrypt();
            //CONNECTION_STRING = "Host=172.25.54.100;Username=postgres;Password=P@ss1234;Database=AgencyBank"; //ConnectionStringEncryptDecrypt.DecryptConnectionString(_configuration["appsetting:ConnectionString"]);
            //DabType = "MySQL";
            DabType = "PostgreSQL";
        }
        public static DataTable ExecuteFunctionAndGetDataTable(string functionName)
        {
            DataTable dataTable = new DataTable();

            using (NpgsqlConnection con = new NpgsqlConnection(ConfigDb.Constring))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand(functionName, con))
                {
                    cmd.CommandType = CommandType.Text; // or CommandType.StoredProcedure if using a stored procedure

                    try
                    {
                        if (con.State != ConnectionState.Open)
                        {
                            con.Open();
                        }

                        using (NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd))
                        {
                            da.Fill(dataTable);  // Fill the DataTable with data from the function call
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions (e.g., log the exception or throw)
                        throw new Exception("An error occurred while executing the function.", ex);
                    }
                }
            }

            return dataTable;
        }

        public static DataSet ExecuteParamerizedSelectCommand(string CommandName, CommandType cmdType, NpgsqlParameter[] param)
        {
            DataSet ds = new DataSet();
            using (var connection = new NpgsqlConnection(ConfigDb.Constring))
            {
                connection.Open();

                using (var command = new NpgsqlCommand(CommandName, connection))
                {
                    command.CommandType = cmdType;
                    //command.CommandText = CommandName;

                    // Add parameters to the command
                    if (param != null)
                    {
                        command.Parameters.AddRange(param);
                    }

                    // Create a data adapter
                    using (var adapter = new NpgsqlDataAdapter(command))
                    {
                        // Fill the DataTable
                        adapter.Fill(ds);
                    }
                }
            }

            //if (DabType.Equals("PostgreSQL"))
            //{
            //using (var con = new NpgsqlConnection(ConfigDb.Constring))
            //{

            //    try
            //    {
            //        if (con.State != ConnectionState.Open)
            //        {
            //            con.Open();

            //            var comand = con.CreateCommand();
            //            comand.CommandType = CommandType.Text;
            //            comand.CommandText = CommandName;
            //            comand.Parameters.AddRange(param);
            //            using (NpgsqlDataAdapter da = new NpgsqlDataAdapter(comand))
            //            {
            //                da.Fill(ds);
            //            }

            //            comand.Dispose();
            //            con.Close();







            //        }

                  
            //    }

            //    catch
            //    {
            //        //throw;
            //    }

            //    //}
            //}

            return ds;
        }


        public async Task<IEnumerable<dynamic>> GetEntitiesAsync()
        {
            using (IDbConnection dbConnection = new NpgsqlConnection(ConfigDb.Constring))
            {
                dbConnection.Open();

                // Call the stored procedure and specify that the result should be mapped to an anonymous type
                var result = await dbConnection.QueryAsync<dynamic>(
                    "GetEntities",
                    commandType: CommandType.StoredProcedure
                );

                return result;
            }
        }



        public static bool ExecuteNonQuery(string CommandName, CommandType cmdType, NpgsqlParameter[] pars)
        {
            int result = 0;
            //using (var connection = new NpgsqlConnection(ConfigDb.Constring))
            //{
            //    using (var command = new NpgsqlCommand("spaepstransactions", connection))
            //    {
            //        command.CommandType = CommandType.StoredProcedure;

            //        string compositeValue = $"(_cycleno => {2})";

            //        // Create and set the composite type parameter
            //        var compositeParameter = new NpgsqlParameter("params", NpgsqlTypes.NpgsqlDbType.Jsonb)
            //        {
            //            Value = compositeValue
            //        };
            //        command.Parameters.Add(compositeParameter);

            //        try
            //        {
            //            connection.Open();
            //            command.ExecuteNonQuery();
            //        }
            //        catch (Exception ex)
            //        {
            //            Console.WriteLine($"Error executing function: {ex.Message}");
            //            throw;
            //        }
            //    }
            //}

            //if (DabType.Equals("PostgreSQL"))
            //{

                using (NpgsqlConnection con = new NpgsqlConnection(ConfigDb.Constring))
                {
                    using (NpgsqlCommand cmd = con.CreateCommand())
                    {
                        cmd.CommandType = cmdType;
                        cmd.CommandText = CommandName;
                        cmd.Parameters.AddRange(pars);

                        try
                        {
                            if (con.State != ConnectionState.Open)
                            {
                                con.Open();
                            }

                         result=    cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            
                        }
                    }
                }
            //}


            return (result > 0);
        }

        public static bool ExecuteQuery_Test(string CommandName, CommandType cmdType, NpgsqlParameter[] pars)
        {
            bool result = false;
            using (NpgsqlConnection con = new NpgsqlConnection(ConfigDb.Constring))
            {
                using (NpgsqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = cmdType;
                    cmd.CommandText = CommandName;
                    cmd.Parameters.AddRange(pars);

                    try
                    {
                        if (con.State != ConnectionState.Open)
                        {
                            con.Open();
                        }

                        using (var reader = cmd.ExecuteReader())
                        {
                            // Process results if needed
                            result = reader.HasRows;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error executing function: {ex.Message}");
                        throw;
                    }
                }
            }

            return result;
        }




        public static DataTable ExecuteFunctionToDataTable(string functionName, params NpgsqlParameter[] parameters)
        {
            DataTable dataTable = new DataTable();

            using (var connection = new NpgsqlConnection(ConfigDb.Constring))
            {
                connection.Open();

                using (var command = new NpgsqlCommand(functionName, connection))
                {
                    command.CommandType = CommandType.Text;

                    // Add parameters to the command
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.Add(param);
                        }
                    }

                    // Create a data adapter
                    using (var adapter = new NpgsqlDataAdapter(command))
                    {
                        // Fill the DataTable
                        adapter.Fill(dataTable);
                    }
                }
            }

            return dataTable;
        }


       
    }
}
