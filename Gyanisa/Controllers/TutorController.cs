using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gyanisa.Data;
using Gyanisa.Models;
using Gyanisa.Models.ManageViewModels;
using Gyanisa.Extensions;
using Microsoft.AspNetCore.Authorization;

using Gyanisa.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Gyanisa.Services;
using System.Text;
using Gyanisa.Middleware;
using System.Net;
using PagedList.Core;
using Gyanisa.Data.FileManager;

namespace Gyanisa.Controllers
{
    
    [Route("[controller]/[action]")]
    //[Route("tutor")]
    public class TutorController : Controller
    {
        public static string CurrentUrlPath;
        protected readonly ApplicationDbContext _context;
        protected readonly IEmailSender _emailSender;
        protected readonly IFileManager _fileManager;
        public TutorController(ApplicationDbContext context,IEmailSender emailSender, IFileManager fileManager)
        {
            _context = context;
            _emailSender = emailSender;
            _fileManager = fileManager;
        }

        [Route("")]
        public IActionResult Index()
        {  
            return View();
        }

        
        [Produces("application/json")]
        [HttpGet]
        public async Task<IActionResult> SubjectSearch()
        {
            try
            {
                string term = HttpContext.Request.Query["term"].ToString();
                var names = await _context.Subjects.Where(p => p.SubjectName.Contains(term)).Select(p => p.SubjectName).ToListAsync();
                return Ok(names);
            }
            catch
            {
                return BadRequest();
            }
        }

        [Produces("application/json")]
        [HttpGet]
        public IActionResult ClassSearch()
        {
            try
            {
                string term = HttpContext.Request.Query["term"].ToString();
                var names = _context.SubjectCategories.Where(p => p.SubjectCategoryName.Contains(term) || p.SubjectCategoryCode.Contains(term)).Select(p =>  p.SubjectCategoryName ).ToList()
                    .Concat(_context.Courses.Where(p => p.CourseName.Contains(term) || p.CourseCode.Contains(term)).Select(p =>  p.CourseName ).ToList());

                return Ok(names);
            }
            catch
            {
                return BadRequest();
            }
        }


        [Produces("application/json")]
        [HttpGet]
        public async Task<IActionResult> Locationsearch()
        {
            try
            {
                string term = HttpContext.Request.Query["term"].ToString();
                var names = await _context.UserInformations.Where(p => p.Add1.Contains(term) || p.Add2.Contains(term) || p.ZipCode.Contains(term)).Select(p => p.Add2).ToListAsync();
                //var names = _context.Cities.Where(p => p.CityName.Contains(term)).Select(p => p.CityName).ToList();

                //var names = (from c in _context.Countries
                //             join r in _context.Regions on c.CountryID equals r.CountryID into RC from r in RC.DefaultIfEmpty()
                //             join d in _context.Districts on r.RegionID equals d.RegionID into DR from d in DR.DefaultIfEmpty()
                //             join cy in _context.Cities on d.DistrictID equals cy.DistrictID into CYD from cy in CYD.DefaultIfEmpty()
                //             where (cy.CityName.Contains(term) || d.DistrictName.Contains(term) || r.RegionName.Contains(term) || c.CountryName.Contains(term))
                //             orderby cy.CityName, d.DistrictName, r.RegionName, c.CountryName ascending
                //             select (string.Join(" ",cy.CityName, d.DistrictName, r.RegionName, c.CountryName))
                //            ).ToList();
                //             //{
                //             //    cy.CityName
                //             //    //string.Concat(",", cy.CityName, d.DistrictName,r.RegionName,c.CountryName).ToArray()
                //             //};

                return Ok(names);
            }
            catch
            {
                return BadRequest();
            }
        }
        
        [HttpGet]
        [Route("search")]
        public IActionResult Search(int page=1,int pageSize=20)
        {

            var subject = HttpContext.Request.Query["subject"].ToString();// == "" ? sub: HttpContext.Request.Query["subject"].ToString();
            var city = HttpContext.Request.Query["city"].ToString();// == "" ? loc : HttpContext.Request.Query["city"].ToString();
            var classess = HttpContext.Request.Query["classess"].ToString();

            var result = _context.UserInformations
                .Include(p=>p.TutorSubjects)
                .Where(p => p.Add1.Contains(city) || 
                            p.Add2.Contains(city) || 
                            p.ZipCode.Contains(city) ||
                            p.TutorSubjects.Any(t=>t.Subject.SubjectName.Contains(subject)));
            PagedList<UserInformation> model = new PagedList<UserInformation>(result.AsQueryable().AsNoTracking(), page, pageSize);
            ViewBag.subject = subject;
            ViewBag.city = city;
            ViewBag.classess = classess;
            return View("Index2", model);
        }

        [Route("local-classes")]
        [HttpGet("/local-classes/{classes}", Name = "local-classes")]
        public IActionResult localclasses(string classes,int page= 1, int pageSize = 20)
        {
            classes = classes.Replace("-", " ");
          
            var result = _context.UserInformations
                .Include(s => s.TutorSubjectCategories)
                .Include(c => c.TutorCourses)
                .Where(p => p.TutorCourses.Any(cc => cc.Course.CourseName.Contains(classes)) ||
                            p.TutorCourses.Any(cc => cc.Course.CourseCode.Contains(classes)) ||
                            p.TutorSubjectCategories.Any(ss => ss.SubjectCategory.SubjectCategoryName.Contains(classes))||
                            p.TutorSubjectCategories.Any(ss => ss.SubjectCategory.SubjectCategoryCode.Contains(classes)));

            PagedList<UserInformation> model = new PagedList<UserInformation>(result.AsQueryable().AsNoTracking(), page, pageSize);
            ViewBag.subject = "";
            ViewBag.city = "";
            ViewBag.classes = classes;
            return View("Index2", model);

            //return View(model);

        }

        //[HttpGet("/city")]
        [HttpGet("/city/{city}/{subject}", Name ="City")]
        public IActionResult Citytuition(string city,string subject)
        {
            if(subject.Trim()== "all-tutors")
                subject =string.Empty;

            var result = _context.UserInformations
                .Include(p => p.TutorSubjects)
                .Where(p => p.Add1.Contains(city) ||
                            p.Add2.Contains(city) ||
                            p.ZipCode.Contains(city) ||
                            p.TutorSubjects.Any(t => t.Subject.SubjectName.Contains(subject)));
          
            ViewBag.subject = subject;
            ViewBag.city = city;
            return View(result);
        }


        // GET: Tutor/Details/5

        [HttpGet("product/{id}/{SeoFriendlyTitle}", Name = "GetProduct")]
        public IActionResult GetProduct(int id, string title)
        {
            // Get the product as indicated by the ID from a database or some repository.
            var product = _context.UserInformations.SingleOrDefault(cfp => cfp.UserID == id);

            // If a product with the specified ID was not found, return a 404 Not Found response.
            if (product == null)
            {
                return this.NotFound();
            }

            // Get the actual friendly version of the title.
            string friendlyTitle = FriendlyUrlHelper.GetFriendlyTitle(product.Slug);

            // Compare the title with the friendly title.
            if (!string.Equals(friendlyTitle, title, StringComparison.Ordinal))
            {
                // If the title is null, empty or does not match the friendly title, return a 301 Permanent
                // Redirect to the correct friendly URL.
                return this.RedirectToRoutePermanent("GetProduct", new { id = id, title = friendlyTitle });
            }

            // The URL the client has browsed to is correct, show them the view containing the product.
            return this.View(product);
        }


        public class NotFoundViewResult : ViewResult
        {
            public NotFoundViewResult(string viewName)
            {
                ViewName = viewName;
                StatusCode = (int)HttpStatusCode.NotFound;
            }
        }

        [Route("/tutor/{id}/{title}.htm")]
        [HttpGet("/tutor/{id}/{title}", Name = "Tutor")]
        public async Task<IActionResult> Details(int? id,string title)
        {
            if (id == null)
            {
                throw new Exception("Details Not Found");
            }
            
            UsersInformationViewModel model = new UsersInformationViewModel();
            var TutorDetail = await _context.UserInformations.Include(t => t.TutorCourses).ThenInclude(e => e.Course).AsNoTracking()
                .SingleOrDefaultAsync(u => u.UserID == Convert.ToInt32(id));
            if(TutorDetail== null)
            {
                throw new Exception("Details Not Found");
            }
            var tcourse = _context.Courses.Select(cm => new CheckBoxItem()
            {
                Id = cm.Id,
                Title = cm.CourseName,
                IsChecked = cm.TutorCourses.Any(x => x.UserID == id) ? true : false
            }).ToList();

            var tsubject = _context.Subjects.Select(cm => new CheckBoxItem()
            {
                Id = cm.Id,
                Title = cm.SubjectName,
                IsChecked = cm.TutorSubjects.Any(x => x.UserID == id) ? true : false
            }).ToList();


            var tsubjectcategory = _context.SubjectCategories.Select(cm => new CheckBoxItem()
            {
                Id = cm.Id,
                Title = cm.SubjectCategoryName,
                IsChecked = cm.TutorSubjectCategories.Any(x => x.UserID == id) ? true : false
            }).ToList();

            var tLanguage = _context.Languages.Select(cm => new CheckBoxItem()
            {
                Id = cm.Id,
                Title = cm.LanguageName,
                IsChecked = cm.TaughtLanguage.Any(x => x.UserID == id) ? true : false
            }).ToList();


            if (TutorDetail != null)
            {
                model.Id = TutorDetail.UserID;
                model.FirstName = TutorDetail.FirstName;
                model.LastName = TutorDetail.LastName;
                model.Email = TutorDetail.Email;
                model.Heading = TutorDetail.Heading;
                model.HourlyFee = TutorDetail.HourlyFee;
                model.Add1 = TutorDetail.Add1;
                model.Add2 = TutorDetail.Add2;
                model.ZipCode = TutorDetail.ZipCode;
                model.Active = TutorDetail.Active;
                model.HisHome = TutorDetail.IsHisHome;
                model.YourHome = TutorDetail.IsYourHome;
                model.WebCam = TutorDetail.IsWebCam;
                model.MobilePhone = TutorDetail.MobilePhone;
                model.DOB = TutorDetail.DOB;

                model.Gender = (TutorDetail.Gender == null ? "" : TutorDetail.Gender.ToString());
                model.Description = TutorDetail.CvDescription;
                // model.UserPhoto = tutordetel.UserPhoto;
                 ViewBag.UserPhoto = TutorDetail.UserPhoto;
                model.AvailableCourses = tcourse;
                model.AvailableSubjects = tsubject;
                model.AvailableSubjectCategories = tsubjectcategory;
                model.AvailableLanguages = tLanguage;


                model.Votes = TutorDetail.Votes;

                model.Experience = TutorDetail.Experience;
                model.GEducation = TutorDetail.GEducation;
                model.PEducation = TutorDetail.PEducation;
                model.Role = TutorDetail.Roles;
                /* Seo Detail */
                model.MetaTitle = TutorDetail.MetaTitle;
                model.MetaKeywords = TutorDetail.MetaKeywords;
                model.MetaDescription = TutorDetail.MetaDescription;
                model.Metaabstract = TutorDetail.Metaabstract;

            }

            // Get the actual friendly version of the title.
            string friendlyTitle = FriendlyUrlHelper.GetFriendlyTitle(TutorDetail.Slug);
            // Compare the title with the friendly title.
            CurrentUrlPath = MyHttpContext.GetAbsoluteUri().ToString();
            if (!string.Equals(friendlyTitle, title, StringComparison.Ordinal))
            {
                // If the title is null, empty or does not match the friendly title, return a 301 Permanent
                // Redirect to the correct friendly URL.

                return this.RedirectToRoutePermanent("Details", new { id = id, title = friendlyTitle });
         
            }
      
            // The URL the client has browsed to is correct, show them the view containing the product.
            return this.View(model);
        }

        // GET: Tutor/Create
        public IActionResult Create()
        {
            ViewData["UserID"] = new SelectList(_context.ApplicationUser, "Id", "Id");
            return View();
        }

        // POST: Tutor/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,Email,Heading,HourlyFee,Addressline1,Addressline2,City,ZipCode,UserPhoto,Active,UserID,Id,AddedDate,ModifiedDate,IPAddress")] UserInformation userDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserID"] = new SelectList(_context.ApplicationUser, "Id", "Id", userDetail.UserID);
            return View(userDetail);
        }

        // GET: Tutor/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userDetail = await _context.UserInformations.SingleOrDefaultAsync(m => m.UserID == id);
            if (userDetail == null)
            {
                return NotFound();
            }
            ViewData["UserID"] = new SelectList(_context.ApplicationUser, "Id", "Id", userDetail.UserID);
            return View(userDetail);
        }

        // POST: Tutor/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("FirstName,LastName,Email,Heading,HourlyFee,Addressline1,Addressline2,City,ZipCode,UserPhoto,Active,UserID,Id,AddedDate,ModifiedDate,IPAddress")] UserInformation userDetail)
        {
            if (id != userDetail.UserID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserDetailExists(userDetail.UserID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserID"] = new SelectList(_context.ApplicationUser, "Id", "Id", userDetail.UserID);
            return View(userDetail);
        }

        // GET: Tutor/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userDetail = await _context.UserInformations
                .Include(u => u.User)
                .SingleOrDefaultAsync(m => m.UserID == id);
            if (userDetail == null)
            {
                return NotFound();
            }

            return View(userDetail);
        }

        // POST: Tutor/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var userDetail = await _context.UserInformations.SingleOrDefaultAsync(m => m.UserID == id);
            _context.UserInformations.Remove(userDetail);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserDetailExists(long id)
        {
            return _context.UserInformations.Any(e => e.UserID == id);
        }
        
        public JsonResult SendRating(string r, string s, string id, string url)
        {
            int autoId = 0;
            Int16 thisVote = 0;
            Int16 sectionId = 0;
            Int16.TryParse(s, out sectionId);
            Int16.TryParse(r, out thisVote);
            int.TryParse(id, out autoId);
            
            if (!User.Identity.IsAuthenticated)
            {
                return Json("Not authenticated!");
            }

            if (autoId.Equals(0))
            {
                return Json("Sorry, record to vote doesn't exists");
            }

            switch (s)
            {
                case "5" : // school voting
                    // check if he has already voted
                    var isIt = _context.RatingLogModels.Where(v => v.SectionId == sectionId &&
                        v.UserName.Equals(User.Identity.Name, StringComparison.CurrentCultureIgnoreCase) && v.VoteForID == autoId).FirstOrDefault();
                    if (isIt != null)
                    {
                        // keep the school voting flag to stop voting by this member
                        //HttpCookie cookie = new HttpCookie(url, "true");
                        //Response.Cookies.Add(cookie);
                        return Json("<br />You have already rated this post, thanks !");
                    }

                    var sch = _context.UserInformations.Where(sc => sc.UserID == autoId).FirstOrDefault();
                    if (sch != null)
                    {
                        object obj = sch.Votes;

                        string updatedVotes = string.Empty;
                        string[] votes = null;
                        if (obj != null && obj.ToString().Length > 0)
                        {
                            string currentVotes = obj.ToString(); // votes pattern will be 0,0,0,0,0
                            votes = currentVotes.Split(',');
                            // if proper vote data is there in the database
                            if (votes.Length.Equals(5))
                            {
                                // get the current number of vote count of the selected vote, always say -1 than the current vote in the array 
                                int currentNumberOfVote = int.Parse(votes[thisVote - 1]);
                                // increase 1 for this vote
                                currentNumberOfVote++;
                                // set the updated value into the selected votes
                                votes[thisVote - 1] = currentNumberOfVote.ToString();
                            }
                            else
                            {
                                votes = new string[] { "0", "0", "0", "0", "0" };
                                votes[thisVote - 1] = "1";
                            }
                        }
                        else
                        {
                            votes = new string[] { "0", "0", "0", "0", "0" };
                            votes[thisVote - 1] = "1";
                        }

                        // concatenate all arrays now
                        foreach (string ss in votes)
                        {
                            updatedVotes += ss + ",";
                        }
                        updatedVotes = updatedVotes.Substring(0, updatedVotes.Length - 1);

                        _context.Entry(sch).State = EntityState.Modified;
                        sch.Votes = updatedVotes;
                        _context.SaveChanges();

                        RatingLogModel vm = new RatingLogModel()
                        {
                            Active = true,
                            SectionId = Int16.Parse(s),
                            UserName = User.Identity.Name,
                            UserID = thisVote,
                            VoteForID = autoId
                        };
                        
                        _context.RatingLogModels.Add(vm);
                        
                        _context.SaveChanges();

                        // keep the school voting flag to stop voting by this member
                        //HttpCookie cookie = new HttpCookie(url, "true");
                        //Response.Cookies.Add(cookie);
                    }
                    break;
                default:
                    break;
            }
            return Json("<br />You rated " + r + " star(s), thanks !");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TeacherInquiry(TeacherInquiryViewModel model)
        {
           
            var enquiry = new TeacherInquiry
            {
                FullName = model.FullName,
                Email = model.Email,
                Mobile = model.Mobile,
                Subject = model.Subject,
                Zip = model.Zip,
                UserID = model.UserID,
                date = DateTime.Now,
                Archive=false,
                Complete=false,
                Message=model.Message,
                IP = Convert.ToString(_emailSender.GetClientIP(Request.HttpContext.Connection.RemoteIpAddress.ToString()).SingleOrDefault())
            };
            if (ModelState.IsValid)
            {
               var user= _context.UserInformations.Where(x => x.UserID == model.UserID).SingleOrDefault();
                if (user != null)
                {
                    StringBuilder builder = new StringBuilder();
                    var subject = enquiry.Subject + " Enquiry";
                    builder.Append("Tutor Name : " + Convert.ToString(user.FirstName +" " + user.LastName+ ""));
                    builder.AppendLine();
                    builder.Append("Enquiry Person : " + Convert.ToString(enquiry.FullName));
                    builder.AppendLine();
                    builder.Append("Subject : " + Convert.ToString(enquiry.Subject));
                    builder.AppendLine();
                    builder.Append("Location : " + Convert.ToString(enquiry.Zip));
                    if (enquiry.Mobile != null)
                    {
                        builder.AppendLine();
                        builder.Append("Contact No. : " + Convert.ToString(enquiry.Mobile));
                        
                    }
                    if (enquiry.Email != null)
                    {
                        builder.AppendLine();
                        builder.Append("Email : " + Convert.ToString(enquiry.Email));
                        
                    }
                    builder.AppendLine();
                    builder.Append("Message : " + Convert.ToString(enquiry.Message));

                    builder.AppendLine();
                    builder.AppendLine();
                    builder.Append("Thank You, ");
                    builder.AppendLine();
                    builder.Append(Convert.ToString(enquiry.FullName)+".");

                    // var messagebody = _emailSender.ConfirmEmailRegistrationAsync(user, callbackUrl, subject, model.Email);
                    await _emailSender.SendCustomEmailAsync(user.Email, subject, builder.ToString());

                    ViewBag.Message = string.Format("Your Message Submited");
                    //StatusMessage = "Verification email sent. Please check your email.";

                }
                _context.Add(enquiry);
                await _context.SaveChangesAsync();
                return Content("Your Message Submited");
            }
            // return Redirect("/");
            return Content(string.Empty);
        }

        [HttpGet("/UserImage/{image}")]
        public IActionResult UserImage(string image)
        {
            var mime = image.Substring(image.LastIndexOf('.') + 1);
            return new FileStreamResult(_fileManager.ImageStream(image, "user"), $"image/{mime}");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostRequire(PostRequireModel model)
        {
            var req = new PostRequire
            {
                FullName = model.FullName,
                Email = model.Email,
                Mobile = model.Mobile,
                Subject = model.Subject,
                Zip = model.Zip,
                date = DateTime.Now,
                IP = Convert.ToString(_emailSender.GetClientIP(Request.HttpContext.Connection.RemoteIpAddress.ToString()).SingleOrDefault())
            };
            if (ModelState.IsValid)
            {
                _context.Add(req);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // return Redirect("/");
            return View(model);
        }


    }
}
