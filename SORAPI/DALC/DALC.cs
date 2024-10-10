using DAL;
using Dapper;
using Newtonsoft.Json.Linq;
using Npgsql;
using Serilog;
using SORAPI.Classes;
using SORAPI.Interface;
using System.Data;

namespace SORAPI.DALC
{
    public class DALC : ProcessAsyncTransactionInterface
    {

        Response _response = new Response();
        public async Task<DataSet> GetRecords(string functionName, Dictionary<string, object> parameters)
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
                            connection.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error: {ex.Message}");
            }
            return ds;
        }

        public async Task<DataSet> GetReportDataProc(string mode, DateTime? fromDate, DateTime? toDate, int? isClosed)
        {
            DataSet ds = new DataSet();
            try
            {
                using (var connection = new NpgsqlConnection(ConfigDb.Constring))
                {
                    connection.Open();
                    DateTime? sqlFromDate = fromDate.HasValue ? fromDate.Value.Date : (DateTime?)null;
                    DateTime? sqlToDate = toDate.HasValue ? toDate.Value.Date : (DateTime?)null;
                    var parameters = new
                    {
                        mode = mode,
                        fromdate = sqlFromDate,
                        todate = sqlToDate,
                        isclosed = isClosed
                    };
                    var result = connection.Query("CALL public.proc_mastercibreportv1(@mode, @fromdate, @todate, @isclosed)", parameters, commandType: CommandType.Text);
                    using (var fetchCmd = new NpgsqlCommand("SELECT * FROM CNCFTempResult", connection))
                    using (var adapter = new NpgsqlDataAdapter(fetchCmd))
                    {
                        adapter.Fill(ds);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error: {ex.Message}");
            }
            return ds;
        }

        //public async Task<Response> InsertCustomerInformation(Request request)
        //{
        //    DataSet ds = new DataSet();
        //    string response = string.Empty;
        //    try
        //    {
        //        using (var con = new NpgsqlConnection(ConfigDb.Constring))
        //        {
        //            await con.OpenAsync();

        //            // Define parameters with correct types
        //            NpgsqlParameter[] parameters = new NpgsqlParameter[]
        //            {
        //                new NpgsqlParameter("p_salutation", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.Salutaion },
        //                new NpgsqlParameter("p_firstname", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.FName },
        //                new NpgsqlParameter("p_middlename", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.MName },
        //                new NpgsqlParameter("p_lastname", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.LName },
        //                new NpgsqlParameter("p_dob", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.DOB },
        //                new NpgsqlParameter("p_mobileno", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.MobileNo },
        //                new NpgsqlParameter("p_emailid", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.Email },
        //                new NpgsqlParameter("p_operartionmode", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = "Manual" },
        //                new NpgsqlParameter("p_customercategory", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = "Regular" },
        //                new NpgsqlParameter("p_address1", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.Address1 },
        //                new NpgsqlParameter("p_address2", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.Address2 },
        //                new NpgsqlParameter("p_address3", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.Address3 },
        //                new NpgsqlParameter("p_country", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.Country },
        //                new NpgsqlParameter("p_state", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.State },
        //                new NpgsqlParameter("p_city", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.City },
        //                new NpgsqlParameter("p_pincode", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.Pincode },
        //                new NpgsqlParameter("p_idprooftype", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.IDProofType },
        //                new NpgsqlParameter("p_idinformation", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = "A1234567" },
        //                new NpgsqlParameter("p_issuedate", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = DateTime.Now.ToString("yyyy-MM-dd") },
        //                new NpgsqlParameter("p_expirydate", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = DateTime.Now.ToString("yyyy-MM-dd") },
        //                new NpgsqlParameter("p_accesstype", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = "Full" },
        //                new NpgsqlParameter("p_remarks", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = "Testing" },
        //                new NpgsqlParameter("p_authorizationflag", NpgsqlTypes.NpgsqlDbType.Boolean) { Value = true },
        //                new NpgsqlParameter("p_authorizedby", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = "PostGre" },
        //                new NpgsqlParameter("p_mailtype", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = "Mail News letter" },
        //                new NpgsqlParameter("p_reason", NpgsqlTypes.NpgsqlDbType.Text) { Value = "Reason" },
        //                new NpgsqlParameter("p_createdby", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = "portal" },
        //                new NpgsqlParameter("p_modifiedby", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = "Mi" },
        //                new NpgsqlParameter("p_modifiedon", NpgsqlTypes.NpgsqlDbType.Timestamp) { Value = DateTime.Now },
        //                new NpgsqlParameter("p_reserve1", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = "revers1" },
        //                new NpgsqlParameter("p_reserve2", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = "revers2" },
        //                new NpgsqlParameter("p_reserve3", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = "revers3" },
        //                new NpgsqlParameter("p_reserve4", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = "revers4" },
        //                new NpgsqlParameter("p_bitisremoved", NpgsqlTypes.NpgsqlDbType.Boolean) { Value = false }
        //            };

        //            // SQL query to call the function and retrieve results
        //            string sql = "SELECT * FROM public.fn_verifyinsertcustomer(@p_salutation, @p_firstname, @p_middlename, @p_lastname, @p_dob, @p_mobileno, @p_emailid, @p_operartionmode, @p_customercategory, @p_address1, @p_address2, @p_address3, @p_country, @p_state, @p_city, @p_pincode, @p_idprooftype, @p_idinformation, @p_issuedate, @p_expirydate, @p_accesstype, @p_remarks, @p_authorizationflag, @p_authorizedby, @p_mailtype, @p_reason, @p_createdby, @p_modifiedby, @p_modifiedon, @p_reserve1, @p_reserve2, @p_reserve3, @p_reserve4, @p_bitisremoved)";

        //            // Execute the query and read results
        //            using (var cmd = new NpgsqlCommand(sql, con))
        //            {
        //                cmd.Parameters.AddRange(parameters);
        //                using (var reader = await cmd.ExecuteReaderAsync())
        //                {
        //                    while (await reader.ReadAsync())
        //                    {
        //                        // Process each row
        //                        var customerId = reader["customerid"].ToString();
        //                        _response.Customerid = reader["customerid"].ToString();
        //                        var salutation = reader["salutation"].ToString();
        //                        Console.WriteLine($"Customer ID: {customerId}");
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error($"Error: {ex.Message}");
        //    }
        //    return _response;
        //}//old

        public async Task<Response> InsertCustomerInformation(Request request)
        {
            DataSet ds = new DataSet();
            string response = string.Empty;
            try
            {
                using (var con = new NpgsqlConnection(ConfigDb.Constring))
                {
                    await con.OpenAsync();
                    //var Dob = new DateOnly(request.Dob.Year,request.Dob.Month,request.Dob.Day);
                    // Define parameters with correct types
                    NpgsqlParameter[] parameters = new NpgsqlParameter[]
                      {
                          new NpgsqlParameter("p_Salutation", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.Salutation ?? (object)DBNull.Value },
                          new NpgsqlParameter("p_FirstName", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.FirstName ?? (object)DBNull.Value },
                          new NpgsqlParameter("p_MiddleName", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.MiddleName ?? (object)DBNull.Value },
                          new NpgsqlParameter("p_LastName", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.LastName ?? (object)DBNull.Value },
                          new NpgsqlParameter("p_Gender", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.Gender ?? (object)DBNull.Value },
                          new NpgsqlParameter("p_DOB", NpgsqlTypes.NpgsqlDbType.Date) { Value = request.Dob != default ? (object)request.Dob : DBNull.Value },
                          new NpgsqlParameter("p_MaritalStatus", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.MaritalStatus ?? (object)DBNull.Value },
                          new NpgsqlParameter("p_MobileNumber", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.MobileNumber ?? (object)DBNull.Value },
                          new NpgsqlParameter("p_EmailID", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.EmailId ?? (object)DBNull.Value },
                          new NpgsqlParameter("p_OperationMode", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.OperationMode ?? (object)DBNull.Value },
                          new NpgsqlParameter("p_CustomerCategory", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.CustomerCategory ?? (object)DBNull.Value },
                          new NpgsqlParameter("p_Occupation", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.Occupation ?? (object)DBNull.Value },
                          new NpgsqlParameter("p_CompanyName", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.CompanyName ?? (object)DBNull.Value },
                          new NpgsqlParameter("p_ProfilePicture", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.ProfilePicture ?? (object)DBNull.Value },
                          new NpgsqlParameter("p_KycStatus", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.KycStatus ?? (object)DBNull.Value },
                          new NpgsqlParameter("p_AccountType", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.AccountType ?? (object)DBNull.Value },
                          new NpgsqlParameter("p_ChannelCode", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.ChannelCode ?? (object)DBNull.Value },
                          new NpgsqlParameter("p_ProgramID", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.ProgramId ?? (object)DBNull.Value },
                          new NpgsqlParameter("p_ProductCode", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.ProductCode ?? (object)DBNull.Value },
                          new NpgsqlParameter("p_PartnerCode", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.PartnerCode ?? (object)DBNull.Value },
                          new NpgsqlParameter("p_Partner", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.PartnerId ?? (object)DBNull.Value },
                          new NpgsqlParameter("p_Address1", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.Address1 ?? (object)DBNull.Value },
                          new NpgsqlParameter("p_Address2", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.Address2 ?? (object)DBNull.Value },
                          new NpgsqlParameter("p_Address3", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.Address3 ?? (object)DBNull.Value },
                          new NpgsqlParameter("p_Country", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.Country ?? (object)DBNull.Value },
                          new NpgsqlParameter("p_State", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.State ?? (object)DBNull.Value },
                          new NpgsqlParameter("p_City", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.City ?? (object)DBNull.Value },
                          new NpgsqlParameter("p_Pincode", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.PinCode ?? (object)DBNull.Value },
                          new NpgsqlParameter("p_IDProofType", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.IdProofType ?? (object)DBNull.Value },
                          new NpgsqlParameter("p_IDInformation", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.IdInformation ?? (object)DBNull.Value },
                          new NpgsqlParameter("p_IssueDate", NpgsqlTypes.NpgsqlDbType.Date) { Value = request.IssueDate != default ? (object)request.IssueDate : DBNull.Value },
                          new NpgsqlParameter("p_ExpiryDate", NpgsqlTypes.NpgsqlDbType.Date) { Value = request.ExpiryDate != default ? (object)request.ExpiryDate : DBNull.Value },
                          new NpgsqlParameter("p_AccessType", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.AccessType ?? (object)DBNull.Value },
                          new NpgsqlParameter("p_Remarks", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.Remarks ?? (object)DBNull.Value },
                          new NpgsqlParameter("p_IsAuthorization", NpgsqlTypes.NpgsqlDbType.Bit) { Value = DBNull.Value}, // Assuming it's a string
                          new NpgsqlParameter("p_AuthorizedBy", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.AuthorizedBy ?? (object)DBNull.Value },
                          new NpgsqlParameter("p_MailingType", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.MailingType ?? (object)DBNull.Value },
                          new NpgsqlParameter("p_Reason", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.Reason ?? (object)DBNull.Value },
                          new NpgsqlParameter("p_CreatedBy", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.Createdby ?? (object)DBNull.Value },
                          new NpgsqlParameter("p_AccountNumber", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.AccountNumber ?? (object)DBNull.Value },
                          new NpgsqlParameter("p_CardNumber", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.CardNumber ?? (object)DBNull.Value },
                          new NpgsqlParameter("p_AmountLimit", NpgsqlTypes.NpgsqlDbType.Numeric) { Value = request.AmountLimit }, // Ensure this is a decimal or double
                          new NpgsqlParameter("p_DailyLimit", NpgsqlTypes.NpgsqlDbType.Numeric) { Value = request.DailyLimit },
                          new NpgsqlParameter("p_MonthlyLimit", NpgsqlTypes.NpgsqlDbType.Numeric) { Value = request.MonthlyLimit },
                          new NpgsqlParameter("p_Balance", NpgsqlTypes.NpgsqlDbType.Numeric) { Value = request.Balance ?? (object)DBNull.Value },
                          new NpgsqlParameter("p_ProgramCode", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.ProgramCode ?? (object)DBNull.Value },
                          new NpgsqlParameter("p_SchemeType", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.SchemeType ?? (object)DBNull.Value },
                          new NpgsqlParameter("p_AccountStatus", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.AccountStatus ?? (object)DBNull.Value },
                          new NpgsqlParameter("p_PartnerID", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.PartnerId ?? (object)DBNull.Value },
                          new NpgsqlParameter("p_ExpiryMonth", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.ExpiryMonth ?? (object)DBNull.Value },
                          new NpgsqlParameter("p_ExpiryYear", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.ExpiryYear ?? (object)DBNull.Value },
                          new NpgsqlParameter("p_ExpiryMMYY", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.ExpiryMMYY ?? (object)DBNull.Value },
                          new NpgsqlParameter("p_MaskedCard", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.MaskedCard ?? (object)DBNull.Value },
                          new NpgsqlParameter("o_customerid", NpgsqlTypes.NpgsqlDbType.Varchar) { Value ="" } // Output parameter
                      };

                    // SQL query to call the function and retrieve results
                    //old// string sql = "call proc_insertcustomerinformation_test( @p_Salutation ,  @p_FirstName,   @p_MiddleName ,@p_LastName ,   @p_Gender ,  @p_DOB ,  @p_MaritalStatus ,   @p_MobileNumber ,  @p_EmailID ,  @p_OperationMode ,@p_CustomerCategory ,  @p_Occupation ,  @p_CompanyName ,   @p_profilePicture ,   @p_KycStatus,   @p_AccountType ,@p_ChannelCode ,  @p_ProgramID ,   @p_Partner ,  @p_Address1 ,  @p_Address2 ,   @p_Address3 ,  @p_Country ,   @p_State ,@p_City ,   @p_Pincode ,  @p_IDProofType ,  @p_IDInformation ,  @p_IssueDate ,  @p_ExpiryDate ,   @p_AccessType ,   @p_Remarks  ,@p_IsAuthorization,  @p_AuthorizedBy  ,   @p_MailingType ,  @p_Reason ,   @p_CreatedBy ,@o_customerid)";
                    string sql = "CALL proc_CustomerRegistration( " +"@p_Salutation, " +"@p_FirstName, " + "@p_MiddleName, " +"@p_LastName, " + "@p_Gender, " +"@p_DOB, " + "@p_MaritalStatus, " + "@p_MobileNumber, " + "@p_EmailID, " + "@p_OperationMode, " + "@p_CustomerCategory, " +  "@p_Occupation, " +  "@p_CompanyName, " +"@p_ProfilePicture, " + "@p_KycStatus, " + "@p_AccountType, " +   "@p_ChannelCode, " + "@p_ProgramID, " +  "@p_ProductCode, " +   "@p_PartnerCode, " + "@p_Partner, " +  "@p_Address1, " +   "@p_Address2, " +   "@p_Address3, " +  "@p_Country, " + "@p_State, " + "@p_City, " + "@p_Pincode, " + "@p_IDProofType, " +  "@p_IDInformation, " + "@p_IssueDate, " +  "@p_ExpiryDate, " + "@p_AccessType, " + "@p_Remarks, " + "@p_IsAuthorization, " + "@p_AuthorizedBy, " + "@p_MailingType, " +   "@p_Reason, " +  "@p_CreatedBy, " +  "@p_AccountNumber, " + "@p_CardNumber, " + "@p_AmountLimit, " + "@p_DailyLimit, " + "@p_MonthlyLimit, " + "@p_Balance, " + "@p_ProgramCode, " +  "@p_SchemeType, " + "@p_AccountStatus, " + "@p_PartnerID, " + "@p_ExpiryMonth, " + "@p_ExpiryYear, " + "@p_ExpiryMMYY, " +  "@p_MaskedCard, " + "@o_customerid)";

                    //"call proc_CustomerRegistration( @p_Salutation ,  @p_FirstName,   @p_MiddleName ,@p_LastName ,   @p_Gender ,  @p_DOB ,  @p_MaritalStatus ,   @p_MobileNumber ,  @p_EmailID ,  @p_OperationMode ,@p_CustomerCategory ,  @p_Occupation ,  @p_CompanyName ,   @p_profilePicture ,   @p_KycStatus,   @p_AccountType ,@p_ChannelCode ,  @p_ProgramID ,   @p_Partner ,  @p_Address1 ,  @p_Address2 ,   @p_Address3 ,  @p_Country ,   @p_State ,@p_City ,   @p_Pincode ,  @p_IDProofType ,  @p_IDInformation ,  @p_IssueDate ,  @p_ExpiryDate ,   @p_AccessType ,   @p_Remarks  ,@p_IsAuthorization,  @p_AuthorizedBy  ,   @p_MailingType ,  @p_Reason ,   @p_CreatedBy ,@o_customerid)";

                    // Execute the query and read results
                    using (var cmd = new NpgsqlCommand(sql, con))
                    {
                        cmd.Parameters.AddRange(parameters);
                        //await cmd.ExecuteReaderAsync();
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                // Process each row
                                var customerId = reader["o_customerid"].ToString();
                                _response.CustomerId = customerId;//reader["CustomerID"].ToString();
                                //var salutation = reader["salutation"].ToString();
                                //Console.WriteLine($"Customer ID: {CustomerID}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error: {ex.Message}");
            }
            return _response;
        }

        public async Task<Response> InsertCustomerwallethistory(Request request)
        {
            DataSet ds = new DataSet();
            string response = string.Empty;
            try
            {
                using (var con = new NpgsqlConnection(ConfigDb.Constring))
                {
                    await con.OpenAsync();

                    // Define parameters with correct types
                    NpgsqlParameter[] parameters = new NpgsqlParameter[]
                    {
                        //new NpgsqlParameter("pid", NpgsqlTypes.NpgsqlDbType.Integer) { Value = request.Id },
                        //new NpgsqlParameter("pcustomerid", NpgsqlTypes.NpgsqlDbType.Integer) { Value = request.CustomerId },
                        new NpgsqlParameter("p_Salutation", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.Salutation },
                        new NpgsqlParameter("p_FirstName", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.FirstName },
                        new NpgsqlParameter("p_MiddleName", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.MiddleName },
                        new NpgsqlParameter("p_LastName", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.LastName },
                        new NpgsqlParameter("p_Gender", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.Gender },
                        new NpgsqlParameter("p_DOB", NpgsqlTypes.NpgsqlDbType.Date) { Value = request.Dob },
                        new NpgsqlParameter("p_MaritalStatus", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.MaritalStatus },
                        new NpgsqlParameter("p_MobileNumber", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.MobileNumber },
                        new NpgsqlParameter("p_EmailID", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.EmailId },
                        new NpgsqlParameter("p_OperationMode", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.OperationMode },
                        new NpgsqlParameter("p_CustomerCategory", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.CustomerCategory },
                        new NpgsqlParameter("p_Occupation", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.Occupation },
                        new NpgsqlParameter("p_CompanyName", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.CompanyName },
                        new NpgsqlParameter("p_ProfilePicture", NpgsqlTypes.NpgsqlDbType.Bytea) { Value = DBNull.Value},
                        new NpgsqlParameter("p_KycStatus", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.KycStatus },
                        new NpgsqlParameter("p_AccountType", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.AccountType },
                        new NpgsqlParameter("p_ChannelCode", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.ChannelCode },
                        new NpgsqlParameter("p_ProgramID", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.ProgramId },
                        new NpgsqlParameter("p_Partner", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.PartnerId },
                        new NpgsqlParameter("p_Address1", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.Address1 },
                        new NpgsqlParameter("p_Address2", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.Address2 },
                        new NpgsqlParameter("p_Address3", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.Address3 },
                        new NpgsqlParameter("p_Country", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.Country },
                        new NpgsqlParameter("p_State", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.State },
                        new NpgsqlParameter("p_City", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.City },
                        new NpgsqlParameter("p_Pincode", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.PinCode },
                        new NpgsqlParameter("p_IDProofType", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.IdProofType },
                        new NpgsqlParameter("p_IDInformation", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.IdInformation },
                        new NpgsqlParameter("p_IssueDate", NpgsqlTypes.NpgsqlDbType.Timestamp) { Value = DBNull.Value },
                        new NpgsqlParameter("p_ExpiryDate", NpgsqlTypes.NpgsqlDbType.Timestamp) { Value = DBNull.Value},
                        new NpgsqlParameter("p_AccessType", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.AccessType },
                        new NpgsqlParameter("p_Remarks", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.Remarks },
                        new NpgsqlParameter("p_IsAuthorization", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = "1" },
                        new NpgsqlParameter("p_AuthorizedBy", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.AuthorizedBy },
                        new NpgsqlParameter("p_AuthorizedOn", NpgsqlTypes.NpgsqlDbType.Timestamp) { Value = DBNull.Value },
                        new NpgsqlParameter("p_MailingType", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.MailingType },
                        new NpgsqlParameter("p_Reason", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.Reason },
                        new NpgsqlParameter("p_CreatedBy", NpgsqlTypes.NpgsqlDbType.Timestamp) { Value = DateTime.Now },
                        new NpgsqlParameter("p_createdby", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.Createdby },
                        //new NpgsqlParameter("p_ModifiedBy", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = request.Modifiedby },
                        //new NpgsqlParameter("p_ModifiedOn", NpgsqlTypes.NpgsqlDbType.Timestamp) { Value = DateTime.Now },
                        //new NpgsqlParameter("p_Reserve1", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = "revers1" },
                        //new NpgsqlParameter("p_Reserve2", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = "revers2" },
                        //new NpgsqlParameter("p_Reserve3", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = "revers3" },
                        //new NpgsqlParameter("p_Reserve4", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = "revers4" },
                        //new NpgsqlParameter("p_IsRemoved", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = "0" },
                    };

                    // SQL query to call the function and retrieve results
                    string sql = "SELECT * FROM public.proc_insertcustomerinformation( @p_Salutation ,  @p_FirstName ,   @p_MiddleName ," +
                                 "@p_LastName ,   @p_Gender ,  @p_DOB ,  @p_MaritalStatus ,   @p_MobileNumber ,  @p_EmailID ,  @p_OperationMode ," +
                                 "@p_CustomerCategory ,  @p_Occupation ,  @p_CompanyName ,   @p_ProfilePicture ,   @p_KycStatus ,   @p_AccountType ," +
                                 "@p_ChannelCode ,  @p_ProgramID ,   @p_Partner ,  @p_Address1 ,  @p_Address2 ,   @p_Address3 ,  @p_Country ,   @p_State ," +
                                 "@p_City ,   @p_Pincode ,  @p_IDProofType ,  @p_IDInformation ,  @p_IssueDate ,  @p_ExpiryDate ,   @p_AccessType ,   @p_Remarks ," +
                                 "@p_IsAuthorization ,  @p_AuthorizedBy ,   @p_AuthorizedOn ,   @p_MailingType ,  @p_Reason ,   @p_CreatedBy)";
                    //"  @p_modifiedby ,@p_ModifiedOn ,  @p_Reserve1 ,  @p_Reserve2 ,  @p_Reserve3 ,   @p_Reserve4 ,   @p_IsRemoved)";

                    // Execute the query and read results
                    using (var cmd = new NpgsqlCommand(sql, con))
                    {
                        cmd.Parameters.AddRange(parameters);
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                // Process each row
                                var customerId = reader["customerid"].ToString();
                                _response.CustomerId = reader["customerid"].ToString();
                                var salutation = reader["salutation"].ToString();
                                Console.WriteLine($"Customer ID: {customerId}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error: {ex.Message}");
            }
            return _response;
        }

        //public static async Task<string> RequestDBdump()
        //{
        //    var resultMessage = "Stored procedure executed successfully.";

        //    await using (var connection = new NpgsqlConnection(CONFIGURATIONCONFIGDATA.ConnPostgreSQL))
        //    {
        //        connection.Notice += (o, e) =>
        //        {
        //            Console.WriteLine($"NOTICE: {e.Notice.MessageText}");
        //        };
        //        await connection.OpenAsync();

        //        try
        //        {
        //            string sql = "CALL public.Proc_InsRequestTranslog(@p_TransRequestType, @p_Mobile, @p_AccountNumber, @p_CustID, @p_ReferenceNumber, @p_TxnAmount, @p_Remark, @p_DeviceID, @p_Channel, @p_Cycle, @p_ResponseCode, @p_ModifiedOn, @p_IsRemoved)";

        //            using (var cmd = new NpgsqlCommand(sql, connection))
        //            {
        //                NpgsqlParameter[] parameters = new NpgsqlParameter[]
        //                {
        //     new NpgsqlParameter("p_TransRequestType", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = (object)"Request OnBoard" ?? DBNull.Value },
        //     new NpgsqlParameter("p_Mobile", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = (object)"9876543215" ?? DBNull.Value },
        //     new NpgsqlParameter("p_AccountNumber", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = (object)"12345678" ?? DBNull.Value },
        //     new NpgsqlParameter("p_CustID", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = (object)"Cust456" ?? DBNull.Value },
        //     new NpgsqlParameter("p_ReferenceNumber", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = (object)"20240814008" ?? DBNull.Value },
        //     new NpgsqlParameter("p_TxnAmount", NpgsqlTypes.NpgsqlDbType.Numeric) { Value = 1500 },
        //     new NpgsqlParameter("p_Remark", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = (object)"Remark text" ?? DBNull.Value },
        //     new NpgsqlParameter("p_DeviceID", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = (object)"Device456" ?? DBNull.Value },
        //     new NpgsqlParameter("p_Channel", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = (object)"INT" ?? DBNull.Value },
        //     new NpgsqlParameter("p_Cycle", NpgsqlTypes.NpgsqlDbType.Integer) { Value = 2 },
        //     new NpgsqlParameter("p_ResponseCode", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = (object)"00" ?? DBNull.Value },
        //     new NpgsqlParameter("p_ModifiedOn", NpgsqlTypes.NpgsqlDbType.Timestamp) { Value = DateTime.Now },
        //     new NpgsqlParameter("p_IsRemoved", NpgsqlTypes.NpgsqlDbType.Boolean) { Value = false }
        //                };
        //                cmd.Parameters.AddRange(parameters);

        //                using (var reader = await cmd.ExecuteReaderAsync())
        //                {
        //                    while (await reader.ReadAsync())
        //                    {
        //                        var insertedId = reader["id"]?.ToString();
        //                        Console.WriteLine($"Inserted ID: {insertedId}");
        //                    }
        //                }
        //            }
        //        }
        //        catch (PostgresException ex)
        //        {
        //            resultMessage = $"Postgres error: {ex.MessageText}";
        //            Console.WriteLine(resultMessage);
        //        }
        //        catch (Exception ex)
        //        {
        //            resultMessage = $"Error: {ex.Message}";
        //            Console.WriteLine(resultMessage);
        //        }
        //    }
        //    return resultMessage;
        //}

        //public async Task<Response> InsertCustomerInformation(Request request)
        //{
        //    DataSet ds = new DataSet();
        //    string response = string.Empty;
        //    try
        //    {
        //        using (var con = new NpgsqlConnection(ConfigDb.Constring))
        //        {
        //            await con.OpenAsync();
        //            NpgsqlTransaction tran = con.BeginTransaction();

        //            // Define a command to call stored procedure show_cities_multiple
        //            NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM public.show_cities_multiple();", con);
        //            command.CommandType = CommandType.Text;

        //            // Execute the stored procedure and obtain the first result set
        //            NpgsqlDataReader dr = await command.ExecuteReaderAsync();

        //            // Output the rows of the first result set
        //            while (dr.Read())
        //                Console.Write("{0}\t{1} \n", dr[0], dr[1]);

        //            // Switch to the second result set
        //            dr.NextResult();

        //            // Output the rows of the second result set
        //            while (dr.Read())
        //                Console.Write("{0}\t{1} \n", dr[0], dr[1]);

        //            tran.Commit();
        //            con.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error($"Error: {ex.Message}");
        //    }
        //    return _response;
        //}


    }
}