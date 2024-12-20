using Gyanisa.Data.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gyanisa.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private ICourseRepository _courseRepository;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

       // ICourseRepository IUnitOfWork.CourseRepository => throw new NotImplementedException();

        public ICourseRepository CourseRepository
        {
            get
            {
                return _courseRepository = _courseRepository ?? new CourseRepository(_context);
            }
        }

        void IUnitOfWork.Save()
        {
            _context.SaveChanges();
        }
    }
}
