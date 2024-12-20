using Gyanisa.Data.Interface;
using Gyanisa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gyanisa.Data.Interface
{
   public interface ICourseRepository: IRepositoryBase<Course>
    {
        //IEnumerable<Subject> GetAllWithSubjects();
        //Subject GetWithSubjects(int id);
    }
}
