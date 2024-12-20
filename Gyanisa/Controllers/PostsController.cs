using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gyanisa.Data;
using Gyanisa.Models.Blog;
using Gyanisa.Data.Repository;
using Gyanisa.Models.ViewModel;
using Gyanisa.Extensions;
using Gyanisa.Data.FileManager;
using Gyanisa.Data.Interface;
using Microsoft.AspNetCore.Authorization;
using PagedList.Core;

namespace Gyanisa.Controllers
{
    [Authorize(Roles = "Admin,Manager")]    
    [Route("[controller]/[action]")]
    public class PostsController : Controller
    { 
        protected readonly IPostRepository _postRepository;
        protected readonly IFileManager _fileManager;
        protected readonly ApplicationDbContext _context;
        public PostsController(IPostRepository postRepository,IFileManager fileManager,ApplicationDbContext context)
        {   
            _postRepository = postRepository;
            _fileManager = fileManager;
            _context = context;
        }

        // GET: Posts
     
           
        public async Task<IActionResult> Index()
        {
            //var result = await _postRepository.GetAllAsync();
            var result =  await _context.Posts.Include(p => p.postGroup).ToListAsync();
            List<Post> model = new List<Post>(result.AsQueryable().OrderByDescending(s => s.Created));
            return View(model);
        }

        [AllowAnonymous]
        [Route("/blog")]
        public async Task<IActionResult> Blog(int page = 1, int pageSize=10)
        {
            var result1 =await _context.Posts.Include(p => p.postGroup).ToListAsync();
            
            //var result1 = await _postRepository.GetAllAsync();
            //var item = _context.Posts.AsNoTracking().OrderBy(s => s.Created);   
            PagedList<Post> model = new PagedList<Post>(result1.AsQueryable().AsNoTracking().OrderByDescending(s=>s.Created) , page, pageSize);
            return View("Blog",model);  
        }
        [AllowAnonymous]
        public async Task<IActionResult> BlogSearch(int page = 1, int pageSize = 10)
        {
            var result1 = await _context.Posts.Include(p => p.postGroup).ToListAsync();
            //var result1 = await _postRepository.GetAllAsync();
            //var item = _context.Posts.AsNoTracking().OrderBy(s => s.Created);   
            PagedList<Post> model = new PagedList<Post>(result1.AsQueryable().AsNoTracking().OrderByDescending(s => s.Created), page, pageSize);
            return View("Blog", model);
        }

        //[AllowAnonymous]
        //public async Task<IActionResult> Blog()
        //{   return View(await _postRepository.GetAllAsync());
        //}
         
        [AllowAnonymous]
        [Route("/posts/{id}/{title}.htm")]
        [HttpGet("/posts/{id}/{title}",Name ="Posts")]
        public async Task<IActionResult> Details(int? id, string title)
        {

            if (id == null)
            {
                throw new Exception("Details Not Found");
            }
            else
            {
                //ViewBag.Popular = await _postRepository.GetAllAsync();
                //Post post = await _postRepository.GetByIdAsync(Convert.ToInt32(id));

                var post = await _context.Posts
                                        .Include(p => p.postGroup)
                                        .SingleOrDefaultAsync(m => m.Id == id);

              

                //var result = await  _context.Posts.Include(p => p.postGroup).ToListAsync();
                //List<Post> model = new List<Post>( result.AsQueryable().OrderByDescending(s => s.Created));
               

               

                if (post == null)
                {
                    throw new Exception("Details Not Found");
                }

                //IQueryable<Post> Result = _context.Posts.Select(aa => new Post
                //{
                //    Id = aa.Id,
                //    Title = aa.Title,
                //    Body = aa.Body,
                //    Created = aa.Created,
                //    Image = aa.Image,
                //    Metaabstract = aa.Metaabstract,
                //    MetaDescription = aa.MetaDescription,
                //    MetaKeywords = aa.MetaKeywords,
                //    MetaTitle = aa.MetaTitle,
                //    Slug = aa.Slug,
                //    postGroup = new PostGroup
                //    {
                //        PostGroupName = aa.postGroup.PostGroupName
                //    }
                //}).AsQueryable().Where(f => f.PostGroupID == post.PostGroupID);
                
                ViewBag.Popular = await _context.Posts.Where(s=>s.PostGroupID==post.PostGroupID && s.Id!=post.Id).Take(10).ToListAsync();
                return View(post);
            }
        }

         
        // GET: Posts/Create
        public IActionResult Create()
        {
            ViewData["PostGroupID"] = new SelectList(_context.PostGroup, "PostGroupID", "PostGroupName");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Body,Image,Created,Postgroupid,Slug,MetaTitle,MetaKeywords,MetaDescription,Metaabstract")] PostViewModel model)
        {
            var post = new Post
            { 
                Title = model.Title,
                Body = model.Body,
                Image = "",//handle image
                MetaTitle = model.Title,
                MetaKeywords = model.MetaKeywords,
                MetaDescription = model.MetaDescription,
                Metaabstract = model.Metaabstract,
                Slug = FriendlyUrlHelper.GetFriendlyTitle(model.Slug),
                PostGroupID= model.Postgroupid,
            };

            if (ModelState.IsValid)
            {
                _postRepository.Create(post);
                await _postRepository.SaveAsync();
                RedirectToAction("PostSitemap", "Sitemap");
                return RedirectToAction(nameof(Index));
            }
            ViewData["PostGroupID"] = new SelectList(_context.PostGroup, "PostGroupID", "PostGroupName", post.PostGroupID);
            return View(model);
        }

        [AllowAnonymous]
        [HttpGet("/BlogImage/{image}")]
        public IActionResult Image(string image)
        {   
            var mime = image.Substring(image.LastIndexOf('.') + 1);
           //FileStreamResult streamResult =new FileStreamResult(_fileManager.ImageStream(image, "blog"), $"image/{mime}");
           // if (streamResult.EntityTag != null)
           //     return streamResult;
           // else
                return new FileStreamResult(_fileManager.ImageStream(image,"blog"),$"image/{mime}");
        }
        
        // GET: Posts/Edit/5
        [HttpGet("/posts/Edit/{id}", Name = "Edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View(new PostViewModel());
            }
            else
            {
               
                var post = await _postRepository.GetByIdAsync(Convert.ToInt32(id));
                ViewData["PostGroupID"] = new SelectList(_context.PostGroup, "PostGroupID", "PostGroupName", post.PostGroupID);
                return View(new PostViewModel
                {
                    Id=post.Id,
                    Title=post.Title,
                    Body=post.Body,
                    Slug=post.Slug,
                    MetaTitle= post.Title, 
                    MetaKeywords=post.MetaKeywords,
                    MetaDescription=post.MetaDescription,
                    Metaabstract=post.Metaabstract,
                    CurrentImage= post.Image
                });
            }
            
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HttpPost("/posts/Edit/{id}", Name = "Edit")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Body,Image,Created,Postgroupid,Slug,MetaTitle,MetaKeywords,MetaDescription,Metaabstract,CurrentImage")] PostViewModel vm)
        {

            var post = new Post
            {
                Id = vm.Id,
                Title = vm.Title,
                Body = vm.Body,
                MetaTitle = vm.Title,
                MetaKeywords = vm.MetaKeywords,
                MetaDescription = vm.MetaDescription,
                Metaabstract = vm.Metaabstract,
                Slug = FriendlyUrlHelper.GetFriendlyTitle(vm.Slug),
                //                Image = await _fileManager.SaveImage(vm.Image,"blog")
                PostGroupID = vm.Postgroupid,
            };

            if (vm.Image == null)
                post.Image = vm.CurrentImage;
            else
            {
                if (!string.IsNullOrEmpty(vm.CurrentImage))
                    _fileManager.RemoveImage(vm.CurrentImage, "blog");
                post.Image = await _fileManager.SaveImage(vm.Image, "blog",post.Slug);
            }



            if (id != post.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _postRepository.Update(post);
                    await _postRepository.SaveAsync();
                    RedirectToAction("PostSitemap", "Sitemap");
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                    //if (!PostExists(post.Id))
                    //{
                    //    return NotFound();
                    //}
                    //else
                    //{
                    //    throw;
                    //}
                }
                
                //return RedirectToAction("Sitemap", "Sitemap");
                return RedirectToAction(nameof(Index));
            }
            ViewData["PostGroupID"] = new SelectList(_context.PostGroup, "PostGroupID", "PostGroupID", post.PostGroupID);
            return View(post);
        }

        // GET: Posts/Delete/5
        [HttpGet("/posts/Delete/{id}", Name = "Delete")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var post = await _postRepository.GetByIdAsync(Convert.ToInt32(id));
            _postRepository.Delete(post);
            await _postRepository.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
        //private bool PostExists(long id)
        //{
        //    return _context.Posts.Any(e => e.Id == id);
        //}
    }
}
