using Gyanisa.Data.Interface;
using Gyanisa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gyanisa.Data.Repository
{
    public class SubjectCategoryRepository : RepositoryBase<SubjectCategory>, ISubjectCategoryRepository
    {

        public SubjectCategoryRepository(ApplicationDbContext applicationDbContext)
            : base(applicationDbContext)
        {
        }

        //public IEnumerable<SubjectCategory> GetAllWithSubjects()
        //{
        //    throw new NotImplementedException();
        //}

        //public SubjectCategory GetWithSubjects(int id)
        //{
        //    throw new NotImplementedException();
        //}
        public IEnumerable<SubjectCategory> GetAllWithSubjects()
        {
            return _applicationDbContext.SubjectCategories;
        }
        public SubjectCategory GetWithSubjects(int id)
        {
            return _applicationDbContext.SubjectCategories.Where(a => a.Id == id).FirstOrDefault(); ;
        }
    }
}