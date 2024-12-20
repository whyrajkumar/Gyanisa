using Gyanisa.Data.Interface;
using Gyanisa.Models;
using Gyanisa.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gyanisa.Data.Repository
{
    public class SubjectRepository : RepositoryBase<Subject>, ISubjectRepository
    {

        public SubjectRepository(ApplicationDbContext applicationDbContext)
            : base(applicationDbContext)
        {
        }

        public IEnumerable<Subject> GellAllSubject()
        {
            //var sublist = (from s in _applicationDbContext.Subjects.Take(8)
            //              select new 
            //              {
            //                  s.Id,
            //                  s.SubjectName,
            //                  s.SubjectCode,
            //                  s.SubjectCategoryId,
            //                  s.SubjectCategory.SubjectCategoryName,
            //                  s.SubjectCategory.SubjectCategoryCode,
            //                  s.SubjectCategory.SubjectCategoryDescription
            //              }).ToAsyncEnumerable();
            return _applicationDbContext.Subjects.Include(s => s.SubjectCategory).ThenInclude(r => r.SubjectCategoryName);
                                        
        }


        public IEnumerable<SubjectCategory> GetAllWithSubjectCategory()
        {
            return _applicationDbContext.SubjectCategories.ToList();
        }

        public IEnumerable<Subject> GetAllWithSubjects()
        {
            return _applicationDbContext.Subjects.ToList();
        }

        public SubjectCategory GetWithSubjectCategory(int id)
        {
            return _applicationDbContext.SubjectCategories.Where(a => a.Id == id).FirstOrDefault(); 
        }

        public Subject GetWithSubjects(int id)
        {
            return _applicationDbContext.Subjects.Where(a => a.Id == id).FirstOrDefault(); 
        }
    }
}