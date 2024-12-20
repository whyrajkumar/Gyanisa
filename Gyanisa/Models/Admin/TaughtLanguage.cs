namespace Gyanisa.Models.Admin
{
    public class TaughtLanguage
    {
        public int UserID { get; set; }
        public UserInformation UserInformation { get; set; }

        public int LanguageID { get; set; }
        public Language Language { get; set; }
    }
}