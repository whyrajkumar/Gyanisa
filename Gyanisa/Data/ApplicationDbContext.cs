using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Gyanisa.Models;
using Microsoft.AspNetCore.Identity;
using Gyanisa.Models.ManageViewModels;
using Gyanisa.Models.Admin;
using Gyanisa.Models.Blog;

namespace Gyanisa.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
           
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            //tutor course list
            builder.Entity<TutorCourse>()
                .HasKey(c => new { c.UserID, c.CourseID });
            builder.Entity<TutorCourse>()
                .HasOne(tc => tc.UserInformation)
                .WithMany(c => c.TutorCourses)
                .HasForeignKey(tc => tc.UserID);
            builder.Entity<TutorCourse>()
                .HasOne(tc => tc.Course)
                .WithMany(c => c.TutorCourses)
                .HasForeignKey(tc => tc.CourseID);

            // Tutor Subject Category List
            builder.Entity<TutorSubjectCategory>()
                .HasKey(c => new { c.UserID, c.SubjectCategoryId });
            builder.Entity<TutorSubjectCategory>()
                .HasOne(cs => cs.UserInformation)
                .WithMany(ct => ct.TutorSubjectCategories)
                .HasForeignKey(c => c.UserID);
            builder.Entity<TutorSubjectCategory>()
                .HasOne(c => c.SubjectCategory)
                .WithMany(c => c.TutorSubjectCategories)
                .HasForeignKey(c => c.SubjectCategoryId);

            //tutor subject list
            builder.Entity<TutorSubject>()
                .HasKey(s => new { s.UserID, s.SubjectId });
            builder.Entity<TutorSubject>()
                .HasOne(ts => ts.UserInformation)
                .WithMany(s => s.TutorSubjects)
                .HasForeignKey(ts => ts.UserID);
            builder.Entity<TutorSubject>()
                .HasOne(ts => ts.Subject)
                .WithMany(s => s.TutorSubjects)
                .HasForeignKey(ts => ts.SubjectId);

            //Laungages List
           
            builder.Entity<TaughtLanguage>()
                .HasKey(c => new { c.UserID, c.LanguageID });
            builder.Entity<TaughtLanguage>()
                .HasOne(tc => tc.UserInformation)
                .WithMany(c => c.TaughtLanguages)
                .HasForeignKey(tc => tc.UserID);
            builder.Entity<TaughtLanguage>()
                .HasOne(tc => tc.Language)
                .WithMany(c => c.TaughtLanguage)
                .HasForeignKey(tc => tc.LanguageID);
        }

        public DbSet<UserInformation> UserInformations { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Subject>Subjects { get; set; }
        public DbSet<SubjectCategory> SubjectCategories { get; set; }
        public DbSet<TutorCourse> TutorCourses { get; set; }
        public DbSet<TutorSubject> TutorSubjects { get; set; }
        public DbSet<TutorSubjectCategory>  TutorSubjectCategories { get; set; }
        public DbSet<RatingLogModel> RatingLogModels { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet <TaughtLanguage> TaughtLanguages { get; set; }


        public DbSet<TermAndService> TermAndServices { get; set; }
        public DbSet<PostRequire> PostRequires { get; set; }
        public DbSet<TeacherInquiry> TeacherInquiries { get; set; }
        public DbSet<Gyanisa.Models.Blog.PostGroup> PostGroup { get; set; }




        //public DbSet<Country> Countries { get; set; }
        //public DbSet<Region> Regions { get; set; }
        //public DbSet<District> Districts { get; set; }
        //public DbSet<City> Cities { get; set; }

    }
}
