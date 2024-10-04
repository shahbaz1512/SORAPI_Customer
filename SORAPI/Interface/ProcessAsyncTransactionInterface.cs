using DAL;
using Dapper;
using Npgsql;
using SORAPI.Classes;
using System.Data;

namespace SORAPI.Interface
{
    public interface ProcessAsyncTransactionInterface
    {
        Task<DataSet> GetRecords(string functionName, Dictionary<string, object> parameters);
        Task<DataSet> GetReportDataProc(string mode, DateTime? fromDate, DateTime? toDate, int? isClosed);
        Task<Response> InsertCustomerInformation(Request request);
        Task<Response> InsertCustomerwallethistory(Request request);
    }

    public interface Datavalidator
    {
        Task<bool> DataValidators(enumTransactionType _transtype, Request request);

        Task<string> SSLDecrypt(string encryptedMessage);

        Task<string> SSLEncrypt(string PlainMessage);

    }
}
