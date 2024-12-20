using Gyanisa.Extensions;
using Gyanisa.Models.Admin;
using Gyanisa.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Gyanisa.Models
{
    public class UserInformation : SeoEntity
    {
        [Key, ForeignKey("User")]
        public int UserID { get; set; }
        public ApplicationUser User { get; set; }

        public DateTime AddedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string IPAddress { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }

        //[DataType(DataType.PhoneNumber)]
        [Phone]
        public string MobilePhone { get; set; }

        public string Heading { get; set; }
        public decimal HourlyFee { get; set; }
        public string Add1 { get; set; }
        public string Add2 { get; set; }

        public string ZipCode { get; set; }
        public string UserPhoto { get; set; }
        public bool Active { get; set; }
        public string Gender { get; set; }
        public string Roles { get; set; }
        public string CvDescription { get; set; }
        public bool IsHisHome { get; set; }
        public bool IsYourHome { get; set; }
        public bool IsWebCam { get; set; }
        public string GEducation { get; set; }
        public string PEducation { get; set; }
        public string Experience { get; set; }

        [NotMapped]
        private string _title;
        [NotMapped]
        public string Title
        {
            get
            {
                return FriendlyUrlHelper.GetFriendlyTitle(Heading);
            }
            set
            {
                _title = Heading;
            }
        }

        public int ViewCount { get; set; }
        public string Votes { get; set; }

        public bool IsMobile { get; set; }
        public bool IsEmail { get; set; }
        public bool IsAddress { get; set; }
        public bool IsEducation { get; set; }

        public bool IsGirl { get; set; }
        public bool IsBoy { get; set; }
        public bool IsCoed { get; set; }


        public List<TutorSubjectCategory> TutorSubjectCategories { get; set; }
        public List<TutorCourse> TutorCourses { get; set; }
        public List<TutorSubject> TutorSubjects { get; set; }
        public List<TaughtLanguage> TaughtLanguages { get; set; }


        //public string FacebookUrl { get; set; }
        //public string TwitterUrl { get; set; }
        //public string InstragramUrl { get; set; }
        //public string LinkedInUrl { get; set; }
        //public string WhatsAppUrl { get; set; }

        //public int CityID { get; set; }
        //public int DistrictID { get; set; }
        //public int RegionID { get; set; }
        //public int CountryID { get; set; }


        //public class UserDetailMap
        //{
        //    public UserDetailMap(EntityTypeBuilder<UserDetail> entityBuilder)
        //    {
        //        entityBuilder.HasKey(t => t.Id);
        //        entityBuilder.Property(t => t.Heading).IsRequired();
        //        entityBuilder.Property(t => t.HourlyCharge).IsRequired();
        //        entityBuilder.Property(t => t.Email).IsRequired();
        //        entityBuilder.Property(t => t.MobileNo).IsRequired();
        //    }
        //}

    }
}
