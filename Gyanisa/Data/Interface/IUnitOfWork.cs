using Gyanisa.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gyanisa.Data.Interface
{
    public interface IUnitOfWork
    {
        ICourseRepository CourseRepository { get; }
        void Save();
    }
}
