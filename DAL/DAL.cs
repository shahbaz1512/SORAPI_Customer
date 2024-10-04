using DbNetLink.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;


namespace DAL
{

    public class ConfigDb
    {
        public static string Constring { get; set; }
    }
    public  class DAL
    {
        public static IConfiguration _configuration;

        public  DataSet GetRRN(string TerminalId, string ReferenceNumber)
        {
            DataSet dttxnCount = new DataSet();
            dttxnCount = null;
            NpgsqlParameter[] Parameters = new NpgsqlParameter[]
                 {
                        new NpgsqlParameter("@ReferenceNumber", ReferenceNumber),
                        new NpgsqlParameter("@TerminalId", TerminalId),
                 };
            dttxnCount = SqlDBHelper.ExecuteParamerizedSelectCommand("SelectUpdateTxnCount", CommandType.StoredProcedure, Parameters);
            return dttxnCount;
        } 
        public  string GetRouterUrl(string txnid)
        {
            string RouterUrl = string.Empty;
            DataSet DtRouterAddress = new DataSet();
            try
            {
                NpgsqlParameter[] Parameters = new NpgsqlParameter[]
                {
                         new NpgsqlParameter("p_txn_id",  NpgsqlTypes.NpgsqlDbType.Varchar) { Value = txnid },
                };

                DtRouterAddress = SqlDBHelper.ExecuteParamerizedSelectCommand("SELECT * FROM public.spGetRouterAddress(@p_txn_id)", CommandType.Text, Parameters);

                if (DtRouterAddress.Tables[0].Rows.Count > 0)
                {
                    RouterUrl = DtRouterAddress.Tables[0].Rows[0]["RouterAddress"].ToString();
                }
                else
                {
                    RouterUrl = "RouterAddress Not Available";
                }
            }
            catch (Exception ex)
            {
                RouterUrl = "";
            }
            return RouterUrl;
        }
        public  DataSet GetRecords(string functionName, Dictionary<string, object> parameters) 
        {
            DataSet ds = new DataSet();
            try
            {
                using (var connection = new NpgsqlConnection(ConfigDb.Constring))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand(functionName, connection))
                    {
                        command.CommandType = CommandType.Text;
                        var parameterArray = parameters
                            .Select(param => new NpgsqlParameter(param.Key, param.Value ?? DBNull.Value))
                            .ToArray();

                        command.Parameters.AddRange(parameterArray);
                        using (var reader = command.ExecuteReader())
                        {
                            var dataTable = new DataTable();
                            dataTable.Load(reader);
                            ds.Tables.Add(dataTable);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return ds;
        }
    }
}
