using Gyanisa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gyanisa.Data.Interface
{
    public interface ISubjectCategoryRepository : IRepositoryBase<SubjectCategory>
    {
        IEnumerable<SubjectCategory> GetAllWithSubjects();
        SubjectCategory GetWithSubjects(int id);

    }
}
