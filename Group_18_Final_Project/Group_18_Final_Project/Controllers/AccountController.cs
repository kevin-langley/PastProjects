using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Group_18_Final_Project.DAL;
using Group_18_Final_Project.Models;

//TODO: Change this using statement to match your project
using Group_18_Final_Project.DAL;
using Group_18_Final_Project.Models;
using Microsoft.EntityFrameworkCore;



//TODO: Change this namespace to match your project
namespace Group_18_Final_Project.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private SignInManager<User> _signInManager;
        private UserManager<User> _userManager;
        private PasswordValidator<User> _passwordValidator;
        private AppDbContext _db;

        public AccountController(AppDbContext context, UserManager<User> userManager, SignInManager<User> signIn)
        {
            _db = context;
            _userManager = userManager;
            _signInManager = signIn;
           
            //user manager only has one password validator
            _passwordValidator = (PasswordValidator<User>)userManager.PasswordValidators.FirstOrDefault();
        }

        
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated) //user has been redirected here from a page they're not authorized to see
            {
                return View("Error", new string[] { "Access Denied" });
            }
            _signInManager.SignOutAsync(); //this removes any old cookies hanging around
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return Redirect(returnUrl ?? "/");
            }
            else
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }
        }

       
        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        //: This is the method where you create a new user
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    UserName = model.Email,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Password = model.Password,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Address = model.Address,
                    City = model.City,
                    State = model.State,
                    ZipCode = model.ZipCode,
                    ActiveUser = true,
                    UserType = "Customer"


                    //: You will need to add all of the properties for your User model here
                    //Make sure that you have included ALL of the properties and that they match
                    //the model class EXACTLY!!


                };
                IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //TODO: Add user to desired role
                    //This will not work until you have seeded Identity OR added the "Customer" role 
                    //by navigating to the RoleAdmin controller and manually added the "Customer" role
                
                    await _userManager.AddToRoleAsync(user, "Customer");
                    //another example
                    //await _userManager.AddToRoleAsync(user, "Manager");
                    //await _userManager.AddToRoleAsync(user, "Employee");

                    // send confirmation email 
                    String emailSubject = "Thank you " + user.FirstName + " for registering with Bevo's Bookstore!";
                    String emailBody = "Welcome to the Bevo's Bookstore family! " +
                        "Don't hesitate to reach out to us with any issues.";
                    Utilities.EmailMessaging.SendEmail(user.Email, emailSubject, emailBody);



                    Microsoft.AspNetCore.Identity.SignInResult result2 = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, lockoutOnFailure: false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }

        //GET: Account/Index
        public ActionResult Index()
        {
            IndexViewModel ivm = new IndexViewModel();

            //get user info
            String id = User.Identity.Name;
            User user = _db.Users.Include(m => m.CreditCards).FirstOrDefault(u => u.UserName == id);

            //populate the view model
            ivm.Email = user.Email;
            ivm.HasPassword = true;
            ivm.UserID = user.Id;
            ivm.UserName = user.UserName;
            ivm.FirstName = user.FirstName;
            ivm.LastName = user.LastName;
            ivm.Address = user.Address;
            ivm.City = user.City;
            ivm.State = user.State;
            ivm.ZipCode = user.ZipCode;
            ivm.PhoneNumber = user.PhoneNumber;
            ivm.creditCards = user.CreditCards;
            //TODO: activeuser?


            return View(ivm);
        }



        //Logic for change password
        // GET: /Account/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            User userLoggedIn = await _userManager.FindByNameAsync(User.Identity.Name);
            var result = await _userManager.ChangePasswordAsync(userLoggedIn, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(userLoggedIn, isPersistent: false);

                // send confirmation email 
                String emailSubject = "Attention " + userLoggedIn.FirstName + "! Your password has been changed.";
                String emailBody = "If this was not you, you may have a serious security concern on your hands. " +
                    "Please reach out to our cusotomer service and let us help you maintain control of your account.";
                Utilities.EmailMessaging.SendEmail(userLoggedIn.Email, emailSubject, emailBody);

                return RedirectToAction("Index", "Home");
            }
            AddErrors(result);
            return View(model);
        }


        //Logic for Editing account info
        // GET: /Account/EditInfo
        public ActionResult EditInfo(int? id)
        {
            if (id == null)
            {
                string username = User.Identity.Name;

                // Fetch the userprofile
                User user = _db.Users.FirstOrDefault(u => u.UserName.Equals(username));

                // Construct the viewmodel
                EditInfoViewModel model = new EditInfoViewModel();
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
                model.Email = user.Email;
                model.PhoneNumber = user.PhoneNumber;
                model.Address = user.Address;
                model.City = user.City;
                model.State = user.State;
                model.ZipCode = user.ZipCode;

                return View(model);
            }
            else
            {
                // Fetch the userprofile
                User user = _db.Users.FirstOrDefault(u => u.Id.Equals(id));

                // Construct the viewmodel
                EditInfoViewModel model = new EditInfoViewModel();
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
                model.PhoneNumber = user.PhoneNumber;
                model.Address = user.Address;
                model.City = user.City;
                model.State = user.State;
                model.ZipCode = user.ZipCode;

                return View(model);
            }
        }

        //
        // POST: /Account/EditInfo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditInfo(EditInfoViewModel userprofile)
        {
            if (ModelState.IsValid)
            {
                string username = User.Identity.Name;
                // Get the userprofile
                User user = _db.Users.FirstOrDefault(u => u.UserName.Equals(username));

                // Update fields
                user.FirstName = userprofile.FirstName;
                user.LastName = userprofile.LastName;
                user.Email = userprofile.Email;
                user.PhoneNumber = userprofile.PhoneNumber;
                user.Address = userprofile.Address;
                user.City = userprofile.City;
                user.State = userprofile.State;
                user.ZipCode = userprofile.ZipCode;

                //save changes
                _db.SaveChanges();

                return RedirectToAction("Index", "Home"); // or whatever
            }

            return View(userprofile);
        }


        //Logic for adding credit card info
        // GET: /Account/AddCard
        public ActionResult AddCard()
        {
            string username = User.Identity.Name;
            User user = _db.Users.Include(m => m.CreditCards).FirstOrDefault(u => u.UserName.Equals(username));
            if(user.CreditCards.Count() > 3)
            {
                return RedirectToAction("Index","Account");
            }


            return RedirectToAction("Create","CreditCards");
        }

        //Logic for editing credit card info
        // GET: /Account/EditCards
        public ActionResult EditCards()
        {
            string username = User.Identity.Name;
            User user = _db.Users.Include(m => m.CreditCards).FirstOrDefault(u => u.UserName.Equals(username));
            return RedirectToAction("Index", "CreditCards");
        }



        //GET:/Account/AccessDenied
        public ActionResult AccessDenied(String ReturnURL)
        {
            return View("Error", new string[] { "Access is denied" });
        }

        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
           

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
    }
}