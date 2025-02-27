using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using TechBack_Identity.Models.Dto;
using TechBack_Identity.Models.Entitys;
using TechBack_Identity.Models.Register;
using TechBack_Identity.Services;

namespace TechBack_Identity.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly EmailService _emailService;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailService = new EmailService();
        }
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
                MyAccountInfoDto accountInfoDto = new MyAccountInfoDto()
                {
                    Email = user.Email,
                    EmailConfrimed = user.EmailConfirmed,
                    PhoneNumber = user.PhoneNumber,
                    PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                    FullName = $"{user.Name},{user.LastName}",
                    Id = user.Id,
                    TwoFactorEnabled = user.TwoFactorEnabled,
                    User = user.UserName
                };
                return View(accountInfoDto);
            }
            else
            {
                // کاربر لاگین نکرده است، او را به صفحه لاگین هدایت کنید
                return RedirectToAction("Login", "Account");
            }
        }
        public IActionResult TwoFactorEnabled()
        {
            var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            var result = _userManager.SetTwoFactorEnabledAsync(user, !user.TwoFactorEnabled).Result;
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(Register register)
        {
            if (ModelState.IsValid == false)
            {
                return View(register);
            }
            User NewUser = new User()
            {
                UserName = register.Email,
                Name = register.FerstName,
                Email = register.Email,
                LastName = register.LastName,

            };
            var resulte = _userManager.CreateAsync(NewUser, register.Password).Result;
            if (resulte.Succeeded)
            {
                var token = _userManager.GenerateEmailConfirmationTokenAsync(NewUser).Result;
                string callbackUrl = Url.Action("confirmEmail", "Account", new
                {
                    UserId = NewUser.Id,
                    token = token
                }, protocol: Request.Scheme);
                string body = $"برای فعال سازی حساب خود لطفا برو روی لینک زیر کلیک کنید <br/> <a href={callbackUrl}";
                _emailService.Execute(NewUser.Email, body, "فعال سازی حساب کاربری تک بک");
                return RedirectToAction("DisplayEmail");
            }
            string Massege = "";
            foreach (var item in resulte.Errors.ToList())
            {
                Massege += item.Description + Environment.NewLine;
            }
            TempData["Massege"] = Massege;
            return View(register);
        }

        public IActionResult confirmEmail(string UserId, string Token)
        {
            if (UserId == null || Token == null)
            {
                return BadRequest();
            }
            var user = _userManager.FindByIdAsync(UserId).Result;
            if (User == null)
            {
                return View("Error");
            }
            var resulte = _userManager.ConfirmEmailAsync(user, Token).Result;
            if (resulte.Succeeded == true)
            {
                //اگر تایید شد
            }
            else
            {
                //اگر تایید نشد
            }
            return View();
        }
        public IActionResult DisplayEmail()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login(string returnurl = "/")
        {

            return View(new LoginDto
            {
                ReturnUrl = returnurl,
            });
        }
        [HttpPost]
        public IActionResult Login(LoginDto login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }
            var user = _userManager.FindByNameAsync(login.UserName).Result;
            _signInManager.SignOutAsync();
            var resulte = _signInManager.PasswordSignInAsync(user, login.Password, login.IsPersistent, true).Result;
            if (resulte.Succeeded == true)
            {
                return Redirect(login.ReturnUrl);
            }
            if (resulte.RequiresTwoFactor == true)
            {
                return RedirectToAction("TwoFactorLogin", new { login.UserName, login.IsPersistent });
            }
            if (resulte.IsLockedOut)
            { 
            
            }
                ModelState.AddModelError(string.Empty, "");
            return View();
        }
        public IActionResult TwoFactorLogin(string UserName, bool IsPersistent)
        {
            TwoFactorLoginDto Model = new TwoFactorLoginDto();
            var user = _userManager.FindByNameAsync(UserName).Result;
            if (user == null)
            {
                return BadRequest();
            }
            var provider = _userManager.GetValidTwoFactorProvidersAsync(user).Result;

            if (provider.Contains("Phone"))
            {
                string smacode=_userManager.GenerateTwoFactorTokenAsync(user, "Phone").Result;
                SMSService  sMSService=new SMSService();
                sMSService.Send(user.PhoneNumber,smacode);
                Model.IsPersistent = IsPersistent;
                Model.Provider = "Phone";
            }
            else if (provider.Contains("Email"))
            {
                string Emailcode = _userManager.GenerateTwoFactorTokenAsync(user, "Email").Result;
                EmailService EmailService=new EmailService();
                EmailService.Execute(user.Email,$"Two Factor{Emailcode}","Two Factor Loig");
                Model.IsPersistent = IsPersistent;
                Model.Provider = "Email";
            }
            return View(Model);
        }
        [HttpPost]
        public IActionResult TwoFactorLogin(TwoFactorLoginDto twoFactor)
        {
            var user=_signInManager.GetTwoFactorAuthenticationUserAsync().Result;
            if (user == null)
            {
                return BadRequest();
            }
            var reulte = _signInManager.TwoFactorSignInAsync(twoFactor.Provider,twoFactor.Code,  twoFactor.IsPersistent, false).Result;
            if (reulte.Succeeded)
            {
                return Redirect("index");
            }else if (reulte.IsLockedOut)
            {
                ModelState.AddModelError("حساب کاربری قفل شده است","");
                return View();
            }
            else 
            {
                ModelState.AddModelError("", "کد وارد شده صحیح نیست ");
                return View();
            }
        }

        public IActionResult SingOut(LoginDto login)
        {
            _signInManager.SignOutAsync();
            return Redirect("index");
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordConfirmationDto forgot)
        {
            if (!ModelState.IsValid)
            {
                return View(forgot);
            }
            var user = _userManager.FindByEmailAsync(forgot.Email).Result;
            if (user == null || _userManager.IsEmailConfirmedAsync(user).Result == false)
            {
                ViewBag.meesage = "ممکن است ایمیل وارد شده معتبر نباشد! و یا اینکه ایمیل خود را تایید نکرده باشید";
                return View();
            }
            var token = _userManager.GeneratePasswordResetTokenAsync(user).Result;
            var callbakurl = Url.Action("RestPassword", "Account", new
            {
                UserId = user.Id,
                token = token
            }, protocol: Request.Scheme);
            string body = $"ارسال ایمیل برای فراموشی رمز عبور<a href={callbakurl}> link Rest Password</a>";
            _emailService.Execute(user.Email, body, "فراموشی رمز عبور");
            ViewBag.meesage = "لینک تنطیم مجدد رمز عبور برای ایمیل شما ارسال شد";
            return View();
        }
        public IActionResult Restpassword(string UserId, string Token)
        {
            return View(new RestPasswordDto
            {
                UserId = UserId,
                Token = Token
            });
        }
        [HttpPost]
        public IActionResult Restpassword(RestPasswordDto rest)
        {
            if (!ModelState.IsValid)
            {
                return View(rest);
            }
            if (rest.Password != rest.ConfirmPassword)
            {
                return BadRequest();
            }
            var user = _userManager.FindByIdAsync(rest.UserId).Result;
            if (user == null)
            {
                return BadRequest();
            }
            var Resulte = _userManager.ResetPasswordAsync(user, rest.Token, rest.Password).Result;
            if (Resulte.Succeeded)
            {
                return RedirectToAction(nameof(Restpasswordconfirm));
            }
            else
            {
                ViewBag.Errors = Resulte.Errors;
                return View(rest);
            }

        }
        public IActionResult Restpasswordconfirm()
        {
            return View();
        }



        public IActionResult SetPhoneNumber()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SetPhoneNumber(SetphoneNumberDto setphoneNumber)
        {
            var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            var setresulte = _userManager.SetPhoneNumberAsync(user, setphoneNumber.PhoneNumber).Result;
            var code = _userManager.GenerateChangePhoneNumberTokenAsync(user, setphoneNumber.PhoneNumber).Result;
            SMSService sMSService = new SMSService();
            sMSService.Send(setphoneNumber.PhoneNumber, code);
            TempData["phoneNumber"] = setphoneNumber.PhoneNumber;
            return RedirectToAction(nameof(VreifyphoneNumber));
        }
        public IActionResult VreifyphoneNumber()
        {
            return View(new VreifyPhoneNumber
            {
                phoneNumber = TempData["phoneNumber"].ToString()
            });
        }
        [HttpPost]
        public IActionResult VreifyphoneNumber(VreifyPhoneNumber verify)
        {
            var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            bool resultVerify = _userManager.VerifyChangePhoneNumberTokenAsync(user, verify.Code, verify.phoneNumber).Result;
            if (resultVerify == false)
            {
                ViewData["Message"] = $"کد وارد شده اشتباه است{verify.phoneNumber}";
                return View(verify);
            }
            else
            {
                user.PhoneNumberConfirmed = true;
                _userManager.UpdateAsync(user);
            }
            return RedirectToAction(nameof(issucss));
        }
        public IActionResult issucss()
        {
            return View();
        }
    }
}
//20-09