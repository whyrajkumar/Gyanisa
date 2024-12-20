using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gyanisa.Data;
using Gyanisa.Data.Interface;
using Gyanisa.Models;
using Gyanisa.Models.Blog;
using Microsoft.EntityFrameworkCore;

namespace Gyanisa.Data.Repository
{
    public class PostRepository : RepositoryBase<Post>, IPostRepository
    {   
        public PostRepository(ApplicationDbContext applicationDbContext)
            :base(applicationDbContext)
        {
            
        }
        public void Edit(int? id)
        {
           
        }
        public new void Update(Post entity)
        {
            this._applicationDbContext.Set<Post>().Update(entity);
        }
    }
}
