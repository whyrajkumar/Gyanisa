using Gyanisa.Models.Admin;
using Gyanisa.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gyanisa.Models.ManageViewModels
{
    public class UsersInformationViewModel : SeoEntity
    {
        public int Id { get; set; }
        [Display(Name = "Title")]
        [Required]
        //[StringLength(50, ErrorMessage = "Title Should be 50 Charactor or 5-6 Words")]
        public string Heading { get; set; }
        [Display(Name = "Hourly Fee")]

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        
        //[StringLength(50, ErrorMessage = "Use 5-6 Words")]
        public string Tag { get; set; }

        public string Email { get; set; }

        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }
        
        [DataType(DataType.PhoneNumber)]
        public string MobilePhone { get; set; }


        public decimal HourlyFee { get; set; }
        
        public string Add1 { get; set; }
        [Required]
        public string Add2 { get; set; }


        [Required]
        public string ZipCode { get; set; }
        public IFormFile UserPhoto { get; set; }
        public bool Active { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]        
        [Display(Name = "Resume Description")]
        public string Description { get; set; }

        [Display(Name = "Tutor's Home")]
        public bool HisHome { get; set; }
        [Display(Name = "Student's Home")]
        public bool YourHome { get; set; }
        [Display(Name = "Online(Skype , Google Hangout)")]
        public bool WebCam { get; set; }


        public string faceb= "https://www.facebook.com/";
        public string FacebookUrl
        {
            get { return faceb; }
            set { faceb =value; }
        }

        public string TwitterUrl { get; set; }
        public string InstragramUrl { get; set; }
        public string LinkedInUrl { get; set; }
        public string WhatsAppUrl { get; set; }


        //Review  & Rating
        public int ViewCount { get; set; }
        public string Votes { get; set; }


        //Subject Category List
        public List<CheckBoxItem> AvailableSubjectCategories { get; set; }

        //Course List
        public List<CheckBoxItem> AvailableCourses { get; set; }
        //Subject List
        public List<CheckBoxItem> AvailableSubjects { get; set; }

        public List<CheckBoxItem> AvailableLanguages { get; set; }

        public string GEducation { get; set; }
        public string PEducation { get; set; }
        public string Experience { get; set; }

        [Display(Name ="Mobile")]
        public bool IsMobile { get; set; }
        [Display(Name = "Email")]
        public bool IsEmail { get; set; }
        [Display(Name = "Address")]
        public bool IsAddress { get; set; }
        [Display(Name = "Education")]
        public bool IsEducation { get; set; }

        [Display(Name = "Girl Only")]
        public bool IsGirl { get; set; }
        [Display(Name = "Boy Only")]
        public bool IsBoy { get; set; }
        [Display(Name = "Both(Girl and Boy)")]
        public bool IsCoed { get; set; }


        public string Role { get; set; }
        public string StatusMessage { get; set; }

        //public int CityID { get; set; }
        //public int DistrictID { get; set; }
        //public int RegionID { get; set; }
        //public int CountryID { get; set; }

        //public int UserID { get; set; }
        //public ApplicationUser User { get; set; }
    }
}
