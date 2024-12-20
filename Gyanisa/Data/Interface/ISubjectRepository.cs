using Gyanisa.Models;
using Gyanisa.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gyanisa.Data.Interface
{
    public interface ISubjectRepository : IRepositoryBase<Subject>
    {
        IEnumerable<Subject> GetAllWithSubjects();
        Subject GetWithSubjects(int id);

        IEnumerable<SubjectCategory> GetAllWithSubjectCategory();
        SubjectCategory GetWithSubjectCategory(int id);
        IEnumerable<Subject> GellAllSubject();
    }
}
