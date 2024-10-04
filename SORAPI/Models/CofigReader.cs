namespace SORAPI.Models
{

    public class AppSettings
    {
        public string RouterAddress { get; set; }
        public string ReqAuth { get; set; }
        public string BANKCODE { get; set; }
        public string TerminalID { get; set; }
        public string DEVICE_TYPE { get; set; }
        public int ResponseTimer { get; set; }
        public int SleepCount { get; set; }
        public bool AEPSValidation { get; set; }
    }
    public static class ConfigReader
    {
        public static string Constring { get; set; }
        public static AppSettings AppSettings { get; set; }
    }
}
