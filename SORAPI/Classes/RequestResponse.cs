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
        public string TxnAmount { get; set; }
        public string DailyLimit { get; set; }
        public string MonthlyLimit { get; set; }
        public string YearlyLimit { get; set; }
        public string TransactionReferenceNumber { get; set; }
        public string TransactionType { get; set; }
        public string AccountNumber { get; set; }
        public string CardNumber { get; set; }
        public string PanNumber { get; set; }
        public string ReserveFeild1 { get; set; }
        public string ReserveFeild2 { get; set; }
        public string ReserveFeild3 { get; set; }
        public string ReserveFeild4 { get; set; }
        public string ReserveFeild5 { get; set; }
        public string ReserveFeild6 { get; set; }
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

    public enum enumTransactionType
    {
        CustomerOnboard = 11
    }
}

