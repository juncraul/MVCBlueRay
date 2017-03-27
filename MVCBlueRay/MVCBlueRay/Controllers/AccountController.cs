using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCBlueRay.Models;
using System.Net;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace MVCBlueRay.Controllers
{
    public class AccountController : Controller
    {
        public UserManager<ApplicationUser> UserManager { get; private set; }

        // GET: Account
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User user)
        {
            if(ModelState.IsValid)
            {
                using (MyDbContext db = new MyDbContext())
                {
                    if(db.Users.FirstOrDefault(a=>a.Username == user.Username) != null)
                    {
                        ViewBag.ErrorMessage = "Username is taken." + Environment.NewLine;
                        return View();
                    }
                    if (db.Users.FirstOrDefault(a => a.Email == user.Email) != null)
                    {
                        ViewBag.ErrorMessage = "Email is taken." + Environment.NewLine;
                        return View();
                    }
                    db.Users.Add(user);
                    db.SaveChanges();
                }
                ModelState.Clear();
                ViewBag.Message = user.Username + " successfully registred."; 
            }
            return View();
        }

        //Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            using (MyDbContext db = new MyDbContext())
            {
                User userFound = db.Users.FirstOrDefault(u => u.Username == user.Username);
                if(userFound != null)
                {
                    if(userFound.Password == user.Password)
                    {
                        Session["UserID"] = userFound.Id.ToString();
                        Session["Username"] = userFound.Username.ToString();
                        return RedirectToAction("LoggedIn");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Password is wrong.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Username does not exist.");
                }
            }
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            using (MyDbContext db = new MyDbContext())
            {
                User user = db.Users.FirstOrDefault(u => u.Email == loginInfo.Email);
                if (user != null)
                {
                    //await SignInAsync(user, isPersistent: false);
                    //return RedirectToLocal(returnUrl);
                    return RedirectToAction("Login", user);
                }
                else
                {
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
                }
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                User user = new User {
                    FirstName = "firstname",
                    LastName = "lastName",
                    Email = model.Email,
                    Username = model.Email,
                    Password = "NoPassword",
                    ConfirmPassword = "NoPassword"
                };
                using (MyDbContext db = new MyDbContext())
                {
                    if (db.Users.FirstOrDefault(a => a.Username == user.Username) != null)
                    {
                        ViewBag.ErrorMessage = "Username is taken." + Environment.NewLine;
                        return View();
                    }
                    if (db.Users.FirstOrDefault(a => a.Email == user.Email) != null)
                    {
                        ViewBag.ErrorMessage = "Email is taken." + Environment.NewLine;
                        return View();
                    }
                    db.Users.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("Login", user);
                }
            }
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        public ActionResult Logout()
        {
            Session["UserID"] = null;
            Session["Username"] = null;
            return RedirectToAction("Index", "Home");
        }

        public ActionResult LoggedIn()
        {

            if (Session["UserID"] == null)
                return RedirectToAction("Login");

            int id = int.Parse(Session["UserID"].ToString());
            using (MyDbContext db = new MyDbContext())
            {
                User userFound = db.Users.FirstOrDefault(u => u.Id == id);
                List<BluRay> userBlueRay = db.UserBlueRays.Where(u => u.UserId == id).Select(a=>a.BluRay).ToList();
                return View(userBlueRay);
            }
        }

        //Edit
        public ActionResult ChangePassword(int id)
        {
            using (MyDbContext db = new MyDbContext())
            {
                User user = db.Users.FirstOrDefault(u => u.Id == id);
                UserChangePasswordViewModel userChangePasswordViewModel = new UserChangePasswordViewModel()
                {
                    Id = user.Id
                };
                if (user != null)
                {
                    return View(userChangePasswordViewModel);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
        public ActionResult ChangePassword(UserChangePasswordViewModel model)
        {
            if(ModelState.IsValid)
            {
                using (MyDbContext db = new MyDbContext())
                {
                    User user = db.Users.FirstOrDefault(u => u.Id == model.Id);
                    if (user.Password != model.OldPassword)
                    {
                        ViewBag.Message = "Old Password is incorrect.";
                        return View(model);
                    }

                    user.Password = model.NewPassword;
                    user.ConfirmPassword = model.ConfirmPassword;
                    db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        public ActionResult AddBluRay()
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login");
            
            int id = int.Parse(Session["UserID"].ToString());
            using (MyDbContext db = new MyDbContext())
            {
                User user = db.Users.FirstOrDefault(u => u.Id == id);
                List<CheckBloxViewModel> bluRay = db.BluRays
                    .Select(a => new CheckBloxViewModel
                    {
                        Id = a.Id,
                        Name = a.Title,
                        Checked = db.UserBlueRays.Count(u=>u.BluRayId == a.Id && u.UserId == id) > 0
                    }).ToList();

                UserViewModel userViewModel = new UserViewModel()
                {
                    BluRays = bluRay,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Id = user.Id
                };

                return View(userViewModel);
            }
        }

        [HttpPost]
        public ActionResult AddBluRay(UserViewModel userView)
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login");
            
            int id = int.Parse(Session["UserID"].ToString());
            using (MyDbContext db = new MyDbContext())
            {
                User user = db.Users.FirstOrDefault(u => u.Id == id);

                //Delete any uncheked bluerays
                foreach(CheckBloxViewModel c in userView.BluRays.Where(b=>!b.Checked))
                {
                    //check if blueray exists in user's collection
                    UserBlueRay userBlueRayToDelete = user.UserBlueRays.FirstOrDefault(u => u.BluRayId == c.Id && u.UserId == user.Id);
                    if (userBlueRayToDelete == null) continue;

                    //delete the use-bluray connection
                    db.Entry(userBlueRayToDelete).State = System.Data.Entity.EntityState.Deleted;
                }

                //Loop through checked bluerays and add them to the user
                foreach (CheckBloxViewModel c in userView.BluRays.Where(b=>b.Checked))
                {
                    //Blueray already exists in user's collection
                    if (db.UserBlueRays.FirstOrDefault(a => a.BluRayId == c.Id && a.UserId == user.Id) != null) continue;

                    //Create new user-bluray connection and add it 
                    UserBlueRay entity = db.UserBlueRays.Create();
                    entity.BluRayId = c.Id;
                    entity.UserId = id;
                    db.UserBlueRays.Add(entity);
                }
                db.SaveChanges();
                return RedirectToAction("LoggedIn");
            }
        }

        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri) : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
    }
}