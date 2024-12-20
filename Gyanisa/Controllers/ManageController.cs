using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Gyanisa.Models;
using Gyanisa.Models.ManageViewModels;
using Gyanisa.Services;
using Gyanisa.Data;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Gyanisa.Models.Admin;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gyanisa.Comman;
using Gyanisa.Extensions;
using Gyanisa.Models.ViewModel;
using Newtonsoft.Json;
using MimeKit;
using Gyanisa.Data.FileManager;

namespace Gyanisa.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class ManageController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly UrlEncoder _urlEncoder;
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _appEnvironment;
        private readonly IFileManager _fileManager;
        private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";
        private const string RecoveryCodesKey = nameof(RecoveryCodesKey);

        public ManageController(
          UserManager<ApplicationUser> userManager,
          SignInManager<ApplicationUser> signInManager,
          IEmailSender emailSender,
          ILogger<ManageController> logger,
          UrlEncoder urlEncoder,IHostingEnvironment hostingEnvironment,
          IFileManager fileManager,
          ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _urlEncoder = urlEncoder;
            _appEnvironment = hostingEnvironment;
            this._context = context;
            _fileManager = fileManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            var model = new IndexViewModel
            {

                Username = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IsEmailConfirmed = user.EmailConfirmed,
                IsPhoneConfirmed=user.PhoneNumberConfirmed,
                StatusMessage = StatusMessage
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(IndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            var email = user.Email;
            if (model.Email != email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, model.Email);
                if (!setEmailResult.Succeeded)
                {
                    throw new ApplicationException($"Unexpected error occurred setting email for user with ID '{user.Id}'.");
                }
            }
         

            var phoneNumber = user.PhoneNumber;
            if (model.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, model.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    throw new ApplicationException($"Unexpected error occurred setting phone number for user with ID '{user.Id}'.");
                }
            }

            StatusMessage = "Your profile has been updated";
            return RedirectToAction(nameof(Index));
        }



        private int GetData()
        {
            var dr = _context.UserInformations.Where(x => x.UserID == Convert.ToInt32(_userManager.GetUserId(User)))
                                                                    .Select(x => new UserInformation {
                                                                        UserID = x.UserID
                                                                    }).SingleOrDefault();
            return Convert.ToInt32(dr.UserID);
        }
     
        //public JsonResult GetDistrictID(int id)
        //{
        //    List<District> list = new List<District>();
        //    list = _context.Districts.Where(x => x.RegionID == id).ToList();
        //    list.Insert(0, new District { DistrictID = 0, DistrictName = "Please Select Dist" });
        //    return Json(new SelectList(list, "DistrictID", "DistrictName"));
        //}
        //public JsonResult GetCityID(int id)
        //{
        //    List<City> list = new List<City>();
        //    list = _context.Cities.Where(x => x.DistrictID == id).ToList();
        //    list.Insert(0, new City { CityID = 0, CityName = "Please Select City" });
        //    return Json(new SelectList(list, "CityID", "CityName"));
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendVerificationEmail(IndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.EmailConfirmationLink(user.Id.ToString(), code, Request.Scheme);

            var subject = "Confirm Account Registration";
            var messagebody = "Please confirm your account by clicking    "  + callbackUrl ; // _emailSender.ConfirmEmailRegistrationAsync(user, callbackUrl, subject, model.Email);
            await _emailSender.SendCustomEmailAsync(user.Email, subject, messagebody);

            StatusMessage = "Verification email sent. Please check your email.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendVerificationPhoneNumber(IndexViewModel model)
        {
            //Hint  IndexViewModel updaet IsPhonenumberConfirmed
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //var callbackUrl = Url.EmailConfirmationLink(user.Id.ToString(), code, Request.Scheme);
            //var email = user.Email;
            //await _emailSender.SendEmailConfirmationAsync(email, callbackUrl);

            StatusMessage = "Verification OTP sent. Please check your phone.";
            return RedirectToAction(nameof(Index));
        }

        
        [HttpGet]
        public async Task<IActionResult> UpdateProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if(user==null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            UsersInformationViewModel model = new UsersInformationViewModel();
            //var userinfo = await _context.UserInformations.Include(t=>t.TutorCourses).ThenInclude(e=>e.Course).AsNoTracking()
            //    .SingleOrDefaultAsync(u=>u.UserID == Convert.ToInt32(_userManager.GetUserId(User)));

            var userinfo = await _context.UserInformations
                .SingleOrDefaultAsync(u => u.UserID == Convert.ToInt32(_userManager.GetUserId(User)));

            if (userinfo==null)
            {
                return NotFound();
            }
 

            var allSubjectcategories = _context.SubjectCategories.Select(cm => new CheckBoxItem()
            {
                Id = cm.Id,
                Title = cm.SubjectCategoryName,
                IsChecked = cm.TutorSubjectCategories.Any(x => x.UserID == userinfo.UserID) ? true : false
            }).ToList();

            var allCourses = _context.Courses.Select(cm => new CheckBoxItem()
            {
                Id = cm.Id,
                Title = cm.CourseName,
                IsChecked = cm.TutorCourses.Any(x => x.UserID == userinfo.UserID) ? true : false
            }).ToList();

            var allSubjects = _context.Subjects.Select(cm => new CheckBoxItem()
            {
                Id = cm.Id,
                Title = cm.SubjectName,
                IsChecked = cm.TutorSubjects.Any(x => x.UserID == userinfo.UserID) ? true : false
            }).ToList();

            var allLanguage = _context.Languages.Select(lg => new CheckBoxItem()
            {
                Id = lg.Id,
                Title = lg.LanguageName,
                IsChecked = lg.TaughtLanguage.Any(l => l.UserID == userinfo.UserID) ? true : false
            }).ToList();



            if (userinfo != null)
            {
                model.Id = userinfo.UserID;
                model.Heading = userinfo.Heading;
                model.FirstName = userinfo.FirstName;
                model.LastName = userinfo.LastName;
                
                model.HourlyFee = userinfo.HourlyFee <100 ? 100: userinfo.HourlyFee;
                model.Add1 = userinfo.Add1;
                model.Add2 = userinfo.Add2;
                model.ZipCode = userinfo.ZipCode;
                model.Active = userinfo.Active;
                model.HisHome = userinfo.IsHisHome;
                model.YourHome = userinfo.IsYourHome;
                model.WebCam = userinfo.IsWebCam;
                model.Gender = (userinfo.Gender == null ? "": userinfo.Gender.ToString());
                model.Description = userinfo.CvDescription;
              
                model.DOB = userinfo.DOB == DateTime.MinValue ? DateTime.Now : userinfo.DOB;
                model.GEducation = userinfo.GEducation;
                model.PEducation = userinfo.PEducation;
                model.Experience = userinfo.Experience;

                //setting
                model.IsAddress = userinfo.IsAddress;
                model.IsMobile = userinfo.IsMobile;
                model.IsEmail = userinfo.IsEmail;
                model.IsEducation = userinfo.IsEducation;

                model.IsGirl = userinfo.IsGirl;
                model.IsBoy = userinfo.IsBoy;
                model.IsCoed = userinfo.IsCoed;

                model.MetaKeywords = userinfo.MetaKeywords;
                model.MetaDescription = userinfo.MetaDescription;
                model.MetaTitle = userinfo.Heading;
                model.Slug = userinfo.Slug;
                model.Metaabstract = userinfo.Metaabstract;


                model.Email = user.Email;//From to UserManager
                model.MobilePhone = user.PhoneNumber;//From to UserManager

                model.AvailableCourses = allCourses.OrderBy(X=>X.Title).ToList();
                model.AvailableSubjects = allSubjects.OrderBy(X => X.Title).ToList();
                model.AvailableSubjectCategories = allSubjectcategories.OrderBy(x => x.Title).ToList();
                model.AvailableLanguages = allLanguage.OrderBy(l => l.Title).ToList();

                ViewBag.UserPhoto = userinfo.UserPhoto == null ? "" : userinfo.UserPhoto.ToString();
                //model.UserPhoto = userinfo.UserPhoto == null ? "DemoProfile.png" : userinfo.UserPhoto.ToString();

               
            }
            return View(model);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[RequestSizeLimit(262144)]
        public async Task<IActionResult> UpdateProfile(int? id, UsersInformationViewModel model, UserInformation um, TutorCourse Tc, TutorSubject ts, TutorSubjectCategory Tsc, IFormFile formFile)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            if (id != model.Id)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                try
                {
                    bool isNew = id.HasValue;
                    //var ud = _context.UserDetails.Where(x => x.Id == id).SingleOrDefault();
                    UserInformation userDetail = _context.UserInformations.Where(x => x.UserID == model.Id).SingleOrDefault();
                    userDetail.FirstName = model.FirstName;
                    userDetail.LastName = model.LastName;
                    userDetail.Heading = model.Heading;
                    userDetail.HourlyFee = model.HourlyFee;
                    userDetail.Add1 = model.Add1;
                    userDetail.Add2 = model.Add2;
                    userDetail.ZipCode = model.ZipCode;
                    userDetail.Active = model.Active;
                    userDetail.IsHisHome = model.HisHome;
                    userDetail.IsYourHome = model.YourHome;
                    userDetail.IsWebCam = model.WebCam;
                    userDetail.Gender = model.Gender.ToString();
                    userDetail.CvDescription = model.Description;
                    userDetail.IPAddress = Convert.ToString(_emailSender.GetClientIP(Request.HttpContext.Connection.RemoteIpAddress.ToString()).SingleOrDefault()); 

                    userDetail.DOB = model.DOB;

                    userDetail.MetaKeywords = model.MetaKeywords;
                    userDetail.Slug = FriendlyUrlHelper.GetFriendlyTitle(model.Heading);
                    userDetail.MetaDescription = model.MetaDescription;
                    userDetail.MetaTitle = model.Heading;
                    userDetail.Metaabstract = model.Metaabstract;

                    userDetail.GEducation = model.GEducation;
                    userDetail.PEducation = model.PEducation;
                    userDetail.Experience = model.Experience;
                    //setting
                    userDetail.IsAddress = model.IsAddress;
                    userDetail.IsMobile = model.IsMobile;
                    userDetail.IsEmail = model.IsEmail;
                    userDetail.IsEducation = model.IsEducation;

                    userDetail.IsGirl = model.IsGirl;
                    userDetail.IsBoy = model.IsBoy;
                    userDetail.IsCoed = model.IsCoed;

                    userDetail.Email = user.Email;//User Mangager
                    userDetail.MobilePhone = user.PhoneNumber;//User Mangager
                    if (userDetail.Votes == null)
                        userDetail.Votes = "0,0,0,0,1";

                    //Delete Grabase Image
                        #region Profile Image
                    ViewBag.Image = userDetail.UserPhoto;

                    if (model.UserPhoto == null)
                        userDetail.UserPhoto = ViewBag.Image;
                    else
                    {
                        if (!string.IsNullOrEmpty(ViewBag.Image))
                            _fileManager.RemoveImage(ViewBag.Image,"user");
                        userDetail.UserPhoto = await _fileManager.SaveImage(model.UserPhoto, "user", userDetail.Slug);
                    }

                    #endregion


                    userDetail.ModifiedDate = DateTime.UtcNow;
                    if (isNew)
                    {
                        _context.Update(userDetail);
                    }
                    _context.SaveChanges();




                    List<TutorCourse> ctc = new List<TutorCourse>();
                    List<TutorSubject> stc = new List<TutorSubject>();
                    List<TutorSubjectCategory> sctc = new List<TutorSubjectCategory>();
                    List<TaughtLanguage> tl = new List<TaughtLanguage>();

                    #region Courselist
                    foreach (var item in model.AvailableCourses)
                    {
                        if (item.IsChecked == true)
                        {
                            ctc.Add(new TutorCourse() { UserID = Convert.ToInt32(id), CourseID = item.Id });
                        }
                    }
                    var Tutordatabasetable = _context.TutorCourses.Where(a => a.UserID == Convert.ToInt32(id)).ToList();
                    var Tutorresultlist = Tutordatabasetable.Except(ctc).ToList();
                    foreach (var item in Tutorresultlist)
                    {
                        _context.TutorCourses.Remove(item);
                        _context.SaveChanges();
                    }
                    var getcourseid = _context.TutorCourses.Where(a => a.UserID == Convert.ToInt32(id)).ToList();
                    foreach (var item in ctc)
                    {
                        if (!getcourseid.Contains(item))
                        {
                            _context.TutorCourses.Add(item);
                            _context.SaveChanges();
                        }

                    }
                    #endregion

                    #region Subjectlist
                    foreach (var item in model.AvailableSubjects)
                    {
                        if (item.IsChecked == true)
                        {
                            stc.Add(new TutorSubject() { UserID = Convert.ToInt32(id), SubjectId = item.Id });
                        }
                    }
                    var Subjectdatabasetable = _context.TutorSubjects.Where(a => a.UserID == Convert.ToInt32(id)).ToList();
                    var Subjectresultlist = Subjectdatabasetable.Except(stc).ToList();
                    foreach (var item in Subjectresultlist)
                    {
                        _context.TutorSubjects.Remove(item);
                        _context.SaveChanges();
                    }
                    var getsubjectid = _context.TutorSubjects.Where(a => a.UserID == Convert.ToInt32(id)).ToList();
                    foreach (var item in stc)
                    {
                        if (!getsubjectid.Contains(item))
                        {
                            _context.TutorSubjects.Add(item);
                            _context.SaveChanges();
                        }

                    }
                    #endregion

                    #region SubjectCategorylist
                    foreach (var item in model.AvailableSubjectCategories)
                    {
                        if (item.IsChecked == true)
                        {
                            sctc.Add(new TutorSubjectCategory() { UserID = Convert.ToInt32(id), SubjectCategoryId = item.Id });
                        }
                    }
                    var sCateogryTbl = _context.TutorSubjectCategories.Where(a => a.UserID == Convert.ToInt32(id)).ToList();
                    var SCategryList = sCateogryTbl.Except(sctc).ToList();
                    foreach (var item in SCategryList)
                    {
                        _context.TutorSubjectCategories.Remove(item);
                        _context.SaveChanges();
                    }
                    var getscategoryid = _context.TutorSubjectCategories.Where(a => a.UserID == Convert.ToInt32(id)).ToList();
                    foreach (var item in sctc)
                    {
                        if (!getscategoryid.Contains(item))
                        {
                            _context.TutorSubjectCategories.Add(item);
                            _context.SaveChanges();
                        }
                    }
                    #endregion

                    #region Languserlist
                    foreach (var item in model.AvailableLanguages)
                    {
                        if (item.IsChecked == true)
                        {
                            tl.Add(new TaughtLanguage() { UserID = Convert.ToInt32(id), LanguageID = item.Id });
                        }
                    }
                    var languagedatabasetable = _context.TaughtLanguages.Where(a => a.UserID == Convert.ToInt32(id)).ToList();
                    var languageresultlist = languagedatabasetable.Except(tl).ToList();
                    foreach (var item in languageresultlist)
                    {
                        _context.TaughtLanguages.Remove(item);
                        _context.SaveChanges();
                    }
                    var getlanguageid = _context.TaughtLanguages.Where(a => a.UserID == Convert.ToInt32(id)).ToList();
                    foreach (var item in tl)
                    {
                        if (!getlanguageid.Contains(item))
                        {
                            _context.TaughtLanguages.Add(item);
                            _context.SaveChanges();
                        }

                    }
                    #endregion

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserInformationExists(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                StatusMessage = "Your profile has been updated";
                return RedirectToAction(nameof(Index));
                //   return View(model);
            }
            return View(model);
        }
        private bool UserInformationExists(int id)
        {
            return _context.UserInformations.Any(e => e.UserID == id);
        }


        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var hasPassword = await _userManager.HasPasswordAsync(user);
            if (!hasPassword)
            {
                return RedirectToAction(nameof(SetPassword));
            }

            var model = new ChangePasswordViewModel { StatusMessage = StatusMessage };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                AddErrors(changePasswordResult);
                return View(model);
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            _logger.LogInformation("User changed their password successfully.");
            StatusMessage = "Your password has been changed.";

            return RedirectToAction(nameof(ChangePassword));
        }

        [HttpGet]
        public async Task<IActionResult> SetPassword()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var hasPassword = await _userManager.HasPasswordAsync(user);

            if (hasPassword)
            {
                return RedirectToAction(nameof(ChangePassword));
            }

            var model = new SetPasswordViewModel { StatusMessage = StatusMessage };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var addPasswordResult = await _userManager.AddPasswordAsync(user, model.NewPassword);
            if (!addPasswordResult.Succeeded)
            {
                AddErrors(addPasswordResult);
                return View(model);
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            StatusMessage = "Your password has been set.";

            return RedirectToAction(nameof(SetPassword));
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLogins()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var model = new ExternalLoginsViewModel { CurrentLogins = await _userManager.GetLoginsAsync(user) };
            model.OtherLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync())
                .Where(auth => model.CurrentLogins.All(ul => auth.Name != ul.LoginProvider))
                .ToList();
            model.ShowRemoveButton = await _userManager.HasPasswordAsync(user) || model.CurrentLogins.Count > 1;
            model.StatusMessage = StatusMessage;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LinkLogin(string provider)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            // Request a redirect to the external login provider to link a login for the current user
            var redirectUrl = Url.Action(nameof(LinkLoginCallback));
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, _userManager.GetUserId(User));
            return new ChallengeResult(provider, properties);
        }

        [HttpGet]
        public async Task<IActionResult> LinkLoginCallback()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var info = await _signInManager.GetExternalLoginInfoAsync(user.Id.ToString());
            if (info == null)
            {
                throw new ApplicationException($"Unexpected error occurred loading external login info for user with ID '{user.Id}'.");
            }

            var result = await _userManager.AddLoginAsync(user, info);
            if (!result.Succeeded)
            {
                throw new ApplicationException($"Unexpected error occurred adding external login for user with ID '{user.Id}'.");
            }

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            StatusMessage = "The external login was added.";
            return RedirectToAction(nameof(ExternalLogins));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveLogin(RemoveLoginViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var result = await _userManager.RemoveLoginAsync(user, model.LoginProvider, model.ProviderKey);
            if (!result.Succeeded)
            {
                throw new ApplicationException($"Unexpected error occurred removing external login for user with ID '{user.Id}'.");
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            StatusMessage = "The external login was removed.";
            return RedirectToAction(nameof(ExternalLogins));
        }

        [HttpGet]
        public async Task<IActionResult> TwoFactorAuthentication()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var model = new TwoFactorAuthenticationViewModel
            {
                HasAuthenticator = await _userManager.GetAuthenticatorKeyAsync(user) != null,
                Is2faEnabled = user.TwoFactorEnabled,
                RecoveryCodesLeft = await _userManager.CountRecoveryCodesAsync(user),
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Disable2faWarning()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!user.TwoFactorEnabled)
            {
                throw new ApplicationException($"Unexpected error occured disabling 2FA for user with ID '{user.Id}'.");
            }

            return View(nameof(Disable2fa));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Disable2fa()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var disable2faResult = await _userManager.SetTwoFactorEnabledAsync(user, false);
            if (!disable2faResult.Succeeded)
            {
                throw new ApplicationException($"Unexpected error occured disabling 2FA for user with ID '{user.Id}'.");
            }

            _logger.LogInformation("User with ID {UserId} has disabled 2fa.", user.Id);
            return RedirectToAction(nameof(TwoFactorAuthentication));
        }

        [HttpGet]
        public async Task<IActionResult> EnableAuthenticator()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var model = new EnableAuthenticatorViewModel();
            await LoadSharedKeyAndQrCodeUriAsync(user, model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnableAuthenticator(EnableAuthenticatorViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadSharedKeyAndQrCodeUriAsync(user, model);
                return View(model);
            }

            // Strip spaces and hypens
            var verificationCode = model.Code.Replace(" ", string.Empty).Replace("-", string.Empty);

            var is2faTokenValid = await _userManager.VerifyTwoFactorTokenAsync(
                user, _userManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);

            if (!is2faTokenValid)
            {
                ModelState.AddModelError("Code", "Verification code is invalid.");
                await LoadSharedKeyAndQrCodeUriAsync(user, model);
                return View(model);
            }

            await _userManager.SetTwoFactorEnabledAsync(user, true);
            _logger.LogInformation("User with ID {UserId} has enabled 2FA with an authenticator app.", user.Id);
            var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
            TempData[RecoveryCodesKey] = recoveryCodes.ToArray();

            return RedirectToAction(nameof(ShowRecoveryCodes));
        }

        [HttpGet]
        public IActionResult ShowRecoveryCodes()
        {
            var recoveryCodes = (string[])TempData[RecoveryCodesKey];
            if (recoveryCodes == null)
            {
                return RedirectToAction(nameof(TwoFactorAuthentication));
            }

            var model = new ShowRecoveryCodesViewModel { RecoveryCodes = recoveryCodes };
            return View(model);
        }

        [HttpGet]
        public IActionResult ResetAuthenticatorWarning()
        {
            return View(nameof(ResetAuthenticator));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetAuthenticator()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await _userManager.SetTwoFactorEnabledAsync(user, false);
            await _userManager.ResetAuthenticatorKeyAsync(user);
            _logger.LogInformation("User with id '{UserId}' has reset their authentication app key.", user.Id);

            return RedirectToAction(nameof(EnableAuthenticator));
        }

        [HttpGet]
        public async Task<IActionResult> GenerateRecoveryCodesWarning()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!user.TwoFactorEnabled)
            {
                throw new ApplicationException($"Cannot generate recovery codes for user with ID '{user.Id}' because they do not have 2FA enabled.");
            }

            return View(nameof(GenerateRecoveryCodes));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateRecoveryCodes()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!user.TwoFactorEnabled)
            {
                throw new ApplicationException($"Cannot generate recovery codes for user with ID '{user.Id}' as they do not have 2FA enabled.");
            }

            var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
            _logger.LogInformation("User with ID {UserId} has generated new 2FA recovery codes.", user.Id);

            var model = new ShowRecoveryCodesViewModel { RecoveryCodes = recoveryCodes.ToArray() };

            return View(nameof(ShowRecoveryCodes), model);
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private string FormatKey(string unformattedKey)
        {
            var result = new StringBuilder();
            int currentPosition = 0;
            while (currentPosition + 4 < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition, 4)).Append(" ");
                currentPosition += 4;
            }
            if (currentPosition < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition));
            }

            return result.ToString().ToLowerInvariant();
        }

        private string GenerateQrCodeUri(string email, string unformattedKey)
        {
            return string.Format(
                AuthenticatorUriFormat,
                _urlEncoder.Encode("Gyanisa"),
                _urlEncoder.Encode(email),
                unformattedKey);
        }

        private async Task LoadSharedKeyAndQrCodeUriAsync(ApplicationUser user, EnableAuthenticatorViewModel model)
        {
            var unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
            if (string.IsNullOrEmpty(unformattedKey))
            {
                await _userManager.ResetAuthenticatorKeyAsync(user);
                unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
            }

            model.SharedKey = FormatKey(unformattedKey);
            model.AuthenticatorUri = GenerateQrCodeUri(user.Email, unformattedKey);
        }

      


        #endregion
    }
}
