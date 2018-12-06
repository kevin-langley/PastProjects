//TODO: Change this using statement to match your project
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

//TODO: Update these using statements to match your project
using Group_18_Final_Project.DAL;
using Group_18_Final_Project.Models;
using System;

//TODO: Change this namespace to match your project
namespace Group_18_Final_Project.Controllers
{
    //: Uncomment this line once you have roles working correctly
    [Authorize(Roles = "Manager, Employee")]
    public class RoleAdminController : Controller
    {
        private AppDbContext _db;
        private UserManager<User> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public RoleAdminController(AppDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }


        // GET: /RoleAdmin/
        public async Task<ActionResult> Index()
        {
            List<RoleEditModel> roles = new List<RoleEditModel>();

            //Show everyone for manager
            if (User.IsInRole("Manager"))
            {
                foreach (IdentityRole role in _roleManager.Roles)
                {
                    List<User> members = new List<User>();
                    List<User> nonMembers = new List<User>();
                    foreach (User user in _userManager.Users)
                    {
                        var list = await _userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;
                        list.Add(user);
                    }
                    RoleEditModel re = new RoleEditModel();
                    re.Role = role;
                    re.Members = members;
                    re.NonMembers = nonMembers;
                    roles.Add(re);
                }
            }

            //Only show customers for employee
            if (User.IsInRole("Employee"))
            {
                foreach (IdentityRole role in _roleManager.Roles)
                {
                    if (role.Name == "Customer")
                    {
                        List<User> members = new List<User>();
                        List<User> nonMembers = new List<User>();
                        foreach (User user in _userManager.Users)
                        {
                            var list = await _userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;
                            list.Add(user);
                        }
                        RoleEditModel re = new RoleEditModel();
                        re.Role = role;
                        re.Members = members;
                        re.NonMembers = nonMembers;
                        roles.Add(re);
                    }
                }
            }
            return View(roles);
        }

        [Authorize(Roles = "Manager")]
        public ActionResult HireNewEmployee()
        {
            return View();
        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<ActionResult> HireNewEmployee(RegisterViewModel model)
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
                    UserType = "Employee"

                };

                IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //TODO: Add user to desired role
                    //This will not work until you have seeded Identity OR added the "Customer" role 
                    //by navigating to the RoleAdmin controller and manually added the "Customer" role

                    await _userManager.AddToRoleAsync(user, "Employee");
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }

            //if code gets this far, we need to show an error
            return View(model);
        }

        public async Task<ActionResult> Edit(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            List<User> members = new List<User>();
            List<User> nonMembers = new List<User>();
            foreach (User user in _userManager.Users)
            {
                var list = await _userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;
                list.Add(user);
            }

            if (role.Name == "Employee")
            {
                ViewBag.Disable = "Fire Employee";
            }
            else if (role.Name == "Customer")
            {
                ViewBag.Disable = "Disable Customer Account";
            }

            return View(new RoleEditModel { Role = role, Members = members, NonMembers = nonMembers });
        }

        [HttpPost]
        public async Task<ActionResult> Edit(RoleModificationModel model)
        {
            IdentityResult result;
            if (ModelState.IsValid)
            {

                foreach (string userId in model.IdsToAdd ?? new string[] { })
                {
                    User user = await _userManager.FindByIdAsync(userId);
                    if (user.SecurityStamp == null)
                    {
                        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                        var stringChars = new char[8];
                        var random = new Random();

                        for (int i = 0; i < stringChars.Length; i++)
                        {
                            stringChars[i] = chars[random.Next(chars.Length)];
                        }

                        var finalString = new String(stringChars);

                        user.SecurityStamp = finalString;
                    }
                    if (user.UserName == null)
                    {
                        user.UserName = user.Email;
                    }
                    //user.UserName = user.Email;
                    

                    //if(!result.Succeeded && user.UserName == null)
                    //{
                    //    try
                    //    {
                    //        user.UserName = user.Email;
                    //        result = await _userManager.AddToRoleAsync(user, model.RoleName);
                    //    }
                    //    catch
                    //    {
                    //        return View("Error", result.Errors);
                    //    }
                    //}
                    if (user.UserType == "Employee")
                    {
                        //Promote to manager
                        if (model.RoleName == "Manager")
                        {
                            result = await _userManager.AddToRoleAsync(user, "Manager");
                            _db.Update(user);
                            _db.SaveChanges();

                            if (!result.Succeeded)
                            {
                                return View("Error", result.Errors);
                            }
                        }

                        //Rehire an employee
                        if (model.RoleName == "Employee")
                        {
                            user.ActiveUser = true;
                            _db.Update(user);
                            _db.SaveChanges();
                        }
                    }

                    if (user.UserType == "Customer")
                    {
                        user.ActiveUser = true;
                        _db.Update(user);
                        _db.SaveChanges();
                    }
                }

                foreach (string userId in model.IdsToDelete ?? new string[] { })
                {
                    User user = await _userManager.FindByIdAsync(userId);
                    user.ActiveUser = false;
                    _db.Update(user);
                    _db.SaveChanges();
                    //result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);
                }
                return RedirectToAction("Index");
            }
            return View("Error", new string[] { "Role Not Found" });
        }




        private void AddErrorsFromResult(IdentityResult result)
        { 
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        

   }
}