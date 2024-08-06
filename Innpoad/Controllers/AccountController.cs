using Innpoad.Models.Common;
using Innpoad.Models.Login;
using Innpoad.Models.Registration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Win32;
using NuGet.Protocol.Plugins;
using System.Data;
using System.Security.Claims;

namespace Innpoad.Controllers
{
    public class AccountController : Controller
    {
        private readonly Common _comman;

        public AccountController(Common common)
        {
            _comman = common;
        }
        public IActionResult Registration()
        {
            ViewData["Country"] = new List<SelectListItem>
  {
      new SelectListItem { Value = "India", Text = "India" },
      new SelectListItem { Value = "Chaina", Text = "Chaina" },
      new SelectListItem { Value = "USA", Text = "USA" }
  };
            ViewData["State"] = new List<SelectListItem>
  {
      new SelectListItem { Value = "Delhi", Text = "Delhi" },
      new SelectListItem { Value = "Gurugram", Text = "Gurugram" },
      new SelectListItem { Value = "Assam", Text = "Assam" }
  };
            ViewData["City"] = new List<SelectListItem>
  {
      new SelectListItem { Value = "Lucknow", Text = "Lucknow" },
      new SelectListItem { Value = "Ayodhya", Text = "Ayodhya" },
      new SelectListItem { Value = "Faizabad", Text = "Faizabad" }
  };
            ViewData["Type"] = new List<SelectListItem>
  {
      new SelectListItem { Value = "Admin", Text = "Admin" },
      new SelectListItem { Value = "User", Text = "User" },
  };
            return View();
        }
        [HttpPost]
        public IActionResult Registration(Registration registration)
        {
            ViewData["Country"] = new List<SelectListItem>
  {
      new SelectListItem { Value = "India", Text = "India" },
      new SelectListItem { Value = "Chaina", Text = "Chaina" },
      new SelectListItem { Value = "USA", Text = "USA" }
  };
            ViewData["State"] = new List<SelectListItem>
  {
      new SelectListItem { Value = "Delhi", Text = "Delhi" },
      new SelectListItem { Value = "Gurugram", Text = "Gurugram" },
      new SelectListItem { Value = "Assam", Text = "Assam" }
  };
            ViewData["City"] = new List<SelectListItem>
  {
      new SelectListItem { Value = "Lucknow", Text = "Lucknow" },
      new SelectListItem { Value = "Ayodhya", Text = "Ayodhya" },
      new SelectListItem { Value = "Faizabad", Text = "Faizabad" }
  };
            ViewData["Type"] = new List<SelectListItem>
  {
      new SelectListItem { Value = "Admin", Text = "Admin" },
      new SelectListItem { Value = "User", Text = "User" },
  };

            var (dataSet1, success1, message1) = _comman.ExecuteStoreProcedure(registration, "sp_Registeration", 41);
            if (dataSet1.Tables.Count > 0 && dataSet1.Tables[0].Rows.Count > 0)
            {
                
                ViewData["chkEmailExist"] = "Email Already Exist";
                return View();
            }
            else
            {
                var (dataSet, success, message) = _comman.ExecuteStoreProcedure(registration, "sp_Registeration", 11);
                if (!success)
                {
                    ViewBag.ErrorMessage = message;
                    return View("Error");
                }
                return RedirectToAction("Login", "Account");
            }

        }
        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;

            if (claimUser.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            return View();
        }
         [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {

            var (dataSet, success, message) = _comman.ExecuteStoreProcedure(login, "sp_Registeration", 42);
            if (!success)
            {
                ViewBag.ErrorMessage = message;
                return View("Error");
            }
            Registration register1 = new Registration();
            if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                DataRow row = dataSet.Tables[0].Rows[0];
                register1.Email = row["Email"].ToString();
                register1.Password = row["Password"].ToString();
                register1.FirstName = row["FirstName"].ToString();
                register1.Type = row["Type"].ToString();
                
           
            if(login.Email !=null || login.Password != null)
            {
                if (login.Email == register1.Email &&
                                login.Password == register1.Password)
                {
                    List<Claim> claims = new List<Claim>() {
            new Claim(ClaimTypes.NameIdentifier, login.Email),
            new Claim(ClaimTypes.Name, register1.FirstName ),
            new Claim(ClaimTypes.Role, register1.Type )

        };

                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                        CookieAuthenticationDefaults.AuthenticationScheme);

                    AuthenticationProperties properties = new AuthenticationProperties()
                    {

                        AllowRefresh = true,
                      IsPersistent = login.KeepLoggedIn
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity), properties);


                    return RedirectToAction("Index", "Home");
                }
            }
                ViewData["ValidateMessage"] = "Email or Password Worng";
                return View();
            }



            ViewData["ValidateMessage"] = "Email or Password Worng";
            return View();
        }

        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");

        }
    }
}
