using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gyanisa.Data;
using Gyanisa.Data.Interface;
using Gyanisa.Models;
using Microsoft.EntityFrameworkCore;

namespace Gyanisa.Data.Repository
{
    public class CourseRepository : RepositoryBase<Course>, ICourseRepository
    {   
        public CourseRepository(ApplicationDbContext applicationDbContext)
            :base(applicationDbContext)
        {
            
        }
 
        //We can override this accourding to system
        public IEnumerable<Course> GetAllCourse()
        {
            return _applicationDbContext.Courses;
        }
        public Course GetWithId(int id)
        {
            return _applicationDbContext.Courses.Where(a => a.Id == id).FirstOrDefault(); ;
        }



    }
}
