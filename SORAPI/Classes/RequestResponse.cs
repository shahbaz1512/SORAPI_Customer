using Microsoft.VisualBasic;
using System.Buffers.Text;
using System.Data.SqlTypes;

namespace SORAPI.Classes
{

    public class Request
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public string Salutation { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateOnly Dob { get; set; }
        public string MaritalStatus { get; set; }
        public string MobileNumber { get; set; }
        public string EmailId { get; set; }
        public string OperationMode { get; set; }
        public string CustomerCategory { get; set; }
        public string Occupation { get; set; }
        public string CompanyName { get; set; }
        public string ProfilePicture { get; set; }
        public string KycStatus { get; set; }
        public string AccountType { get; set; }
        public string ChannelCode { get; set; }
        public string ProgramId { get; set; }
        public string PartnerId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string PinCode { get; set; }
        public string IdProofType { get; set; }
        public string IdInformation { get; set; }
        public DateOnly IssueDate { get; set; }
        public DateOnly ExpiryDate { get; set; }
        public string AccessType { get; set; }
        public string Remarks { get; set; }
        public string IsAuthorization { get; set; }
        public string AuthorizedBy { get; set; }
        public string AuthorizedOn { get; set; }
        public string MailingType { get; set; }
        public string Reason { get; set; }
        public string Createdby { get; set; }
        public string Modifiedby { get; set; }
        public int Isremoved { get; set; }
        public string TopupAmount { get; set; }
        public int TxnAmount { get; set; }
        public int DailyLimit { get; set; }
        public int MonthlyLimit { get; set; }
        public int YearlyLimit { get; set; }
        public string TransactionReferenceNumber { get; set; }
        public string TransactionType { get; set; }
        public string AccountNumber { get; set; }
        public string CardNumber { get; set; }
        public string PanNumber { get; set; }
        public int AmountLimit { get; set; }
        public string AvailableBalance { get; set; }
        public string ProgramCode { get; set; }
        public string SchemeType { get; set; }
        public string AccountStatus { get; set; }
        public string ExpiryMonth { get; set; }
        public string ExpiryYear { get; set; }
        public string ExpiryMMYY { get; set; }
        public string ProductCode { get; set; }
        public string PartnerCode { get; set; }
        public string Balance { get; set; }
        public string MaskedCard { get; set; }
    }

    public class Response
    {
        public string ResponseCode { get; set; }
        public string RequestDescription { get; set; }
        public string CustomerId { get; set; }
        public string CustomerDetails { get; set; }
        public string CustomerAvailableLimit { get; set; }
        public string CustomerTopupDetails { get; set; }
        public string CustomerDebitDetails { get; set; }
        public string CustomerCreditDetails { get; set; }

    }


    public class CustomerRequest
    {
        public int CustomerId { get; set; }
        public string Salutation { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Lastname { get; set; }
        public string Gender { get; set; }
        public DateTime Dob { get; set; } // Date of Birth
        public string MaritalStatus { get; set; }
        public string MobileNumber { get; set; }
        public string EmailId { get; set; }
        public string OperationMode { get; set; }
        public string CustomerCategory { get; set; }
        public string Occupation { get; set; }
        public string CompanyName { get; set; }
        public string ProfilePicture { get; set; }
        public string KycStatus { get; set; }
        public string AccountType { get; set; }
        public string ProgramId { get; set; }
        public string Partner { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }
        public string IdProofType { get; set; }
        public string IdInformation { get; set; }
        public string Remarks { get; set; }
        public bool IsAuthorization { get; set; }
        public string AuthorizedBy { get; set; }
        public string MailingType { get; set; }
        public string Reason { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string Reserve1 { get; set; }
        public string Reserve2 { get; set; }
        public string Reserve3 { get; set; }
        public string Reserve4 { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string AccountNumber { get; set; }
        public string SchemeType { get; set; }
        public string AccountStatus { get; set; }
        public double LastCbsBalance { get; set; }
        public string Remark { get; set; }
        public string CardNumber { get; set; }
        public double CbsBalance { get; set; }
        public string ProgramCode { get; set; }
        public string PartnerId { get; set; }
        public string TransId { get; set; }
        public string TxnFlag { get; set; }
        public double OpeningBalance { get; set; }
        public double ClosingBalance { get; set; }
        public string Type { get; set; }
        public string ChannelId { get; set; }
        public string Narration { get; set; }
        public string ReferenceNumber { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public string ProductCode { get; set; }
        public string PartnerCode { get; set; }
        public string CommandType { get; set; }
        public string TransMode { get; set; }
        public string TransSource { get; set; }
        public string TransactionType { get; set; }
        public string ProcessingCode { get; set; }
        public double TransactionAmount { get; set; }
        public string SystemTraceAuditNumber { get; set; }
        public DateTime TransmissionDateTime { get; set; }
        public DateTime LocalTransactionDateTime { get; set; }
        public string MerchantCategory { get; set; }
        public string AcquirerInstitutionCode { get; set; }
        public string ReceivingInstitutionCode { get; set; }
        public string AuthorizationNumber { get; set; }
        public string TerminalId { get; set; }
        public string TerminalLocation { get; set; }
        public string GeoTag { get; set; }
        public string CardAccepterName { get; set; }
        public double AdditionalAmounts { get; set; }
        public string TransCurrencyCode { get; set; }
        public string AdditionalIssuerData { get; set; }
        public string OriginalDataElements { get; set; }
        public string FromAccountNumber { get; set; }
        public string ToAccountNumber { get; set; }
        public string PanNumber { get; set; }
        public string AadharNumber { get; set; }
        public string Channel { get; set; }
        public string PanEntryMode { get; set; }
        public string Bin { get; set; }
        public string CountryCode { get; set; }
        public string StateCode { get; set; }
        public string CityCode { get; set; }

    }


    public enum enumTransactionType
    {
        CustomerOnboard = 11
    }
}

