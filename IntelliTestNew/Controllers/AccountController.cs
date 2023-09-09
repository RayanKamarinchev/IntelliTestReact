using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using IdentityTemplate.Identity.Models;
using IntelliTest.Data.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;


namespace IdentityTemplate.Controllers
{
    [Authorize]
    [Route("Account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IEmailSender _emailSender;
        private readonly UrlEncoder _urlEncoder;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly Regex re = new Regex("^([a-zA-Z0-9_\\-\\.]+)@([a-zA-Z0-9_\\-]+)(\\.[a-zA-Z]{2,5}){1,2}$");

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<AccountController> logger,
            IEmailSender emailSender,
            UrlEncoder urlEncoder,
            IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _urlEncoder = urlEncoder;
            this.webHostEnvironment = webHostEnvironment;

        }

        private bool IsModelStateAndEmailValid(string email)
        {
            if (!ModelState.IsValid)
            {
                return false;
            }

            if (email is null || !re.IsMatch(email))
            {
                ModelState.AddModelError(string.Empty, "Невалиден имейл.");
                return false;
            }
            return true;
        }
        private bool IsModelStateAndObjectValid(object obj)
        {
            if (!ModelState.IsValid)
            {
                return false;
            }
            var results = new List<ValidationResult>();

            bool objectIsValid = Validator.TryValidateObject(obj, new ValidationContext(obj), results, true);

            if (!objectIsValid)
            {
                foreach (var res in results)
                {
                    ModelState.AddModelError(string.Empty, res.ErrorMessage);
                }
                return false;
            }
            return true;
        }

        private string GetEmailTemplate(string link)
        {
            string path = webHostEnvironment.WebRootPath + "/html/email.html";
            string emailHtml = System.IO.File.ReadAllText(path);
            emailHtml = emailHtml.Replace("$$$", link);
            return emailHtml;
        }

        [AllowAnonymous]
        [HttpGet("Demo")]
        public async Task<IActionResult> Demo()
        {
            return Ok("evala");
        }

        [AllowAnonymous]
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            var statusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";

            return Redirect("/Account/ConfirmEmail");                        
        }

        [AllowAnonymous]
        [HttpGet("ConfirmEmailChange")]
        public async Task<IActionResult> ConfirmEmailChange(string userId, string email, string code)
        {
            if (userId == null || email == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ChangeEmailAsync(user, email, code);
            if (!result.Succeeded)
            {
                var msg = "Error changing email.";
                return Redirect("/Account/ConfirmEmailChange");
            }

            // In our UI email and user name are one and the same, so when we update the email
            // we need to update the user name.
            var setUserNameResult = await _userManager.SetUserNameAsync(user, email);
            if (!setUserNameResult.Succeeded)
            {
                var msg = "Error changing user name.";
                return Redirect("/Account/ConfirmEmailChange");
            }

            await _signInManager.RefreshSignInAsync(user);
            var message = "Thank you for confirming your email change.";
            return Redirect("/Account/ConfirmEmailChange");
        }

                
        [HttpGet("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string email, string password, bool rememberMe, string returnUrl)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            var loginInput = new LoginInput()
            {
                Email = email,
                Password = password,
                RememberMe = rememberMe,
                ReturnUrl = returnUrl
            };
            if (IsModelStateAndObjectValid(loginInput))
            {
                var result = await _signInManager.PasswordSignInAsync(loginInput.Email, loginInput.Password, loginInput.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    
                    return Ok(new RedirectResponse()
                    {
                        Pathname = returnUrl
                    });
                }
                else
                {
                    ModelState.AddModelError("ModelErrors", "Invalid login attempt.");

                    return Ok(ModelState);
                }
            }
            
            return Ok(ModelState);
        }

        [AllowAnonymous]
        [HttpGet("Registers")]
        public async Task<IActionResult> Register(string email, string password, string confirmPassword, string returnUrl)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            var registerInput = new RegisterInput()
            {
                Email = email,
                ConfirmPassword = confirmPassword,
                Password = password,
                ReturnUrl = returnUrl
            };
            var ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (IsModelStateAndObjectValid(registerInput))
            {
                var user = new User { UserName = registerInput.Email, Email = registerInput.Email };
                var result = await _userManager.CreateAsync(user, registerInput.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    var callbackUrl = $"/Account/ConfirmEmail?userId={user.Id}&code={code}&returnUrl={returnUrl}";

                    await _emailSender.SendEmailAsync(registerInput.Email, "Confirm your email",
                        GetEmailTemplate(callbackUrl));
                    
                    
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return Ok(new RedirectResponse()
                    {
                        Pathname = returnUrl
                    });
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("ModelErrors", error.Description);
                }
            }

            return Ok(ModelState);
        }

        [AllowAnonymous]
        [HttpGet("ExternalLogins")]
        public async Task<IList<AuthenticationScheme>> GetExternalLogins()
        {
            return (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        
        [AllowAnonymous]
        [HttpGet("ResendEmailConfirmation")]
        public async Task<IActionResult> ResendEmailConfirmation(string email)
        {
            if (!IsModelStateAndEmailValid(email))
            {
                return Ok(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Ok(new StatusResponse()
                {
                    Status = "Verification email sent. Please check your email."
                });
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var callbackUrl = $"/Account/ConfirmEmail?userId={userId}&code={code}";

            await _emailSender.SendEmailAsync(
                email,
                "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            return Ok(new StatusResponse()
            {
                Status = "Verification email sent. Please check your email."
            });
        }

        [AllowAnonymous]
        [HttpGet("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (IsModelStateAndEmailValid(email))
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return Ok(new RedirectResponse()
                    {
                        Pathname = "/Account/ForgotPasswordConfirmation"
                    });
                }

                // For more information on how to enable account confirmation and password reset please 
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = $"/Account/ResetPassword?code={code}";

                await _emailSender.SendEmailAsync(
                    email,
                    "Reset Password",
                    GetEmailTemplate(callbackUrl));

                return Ok(new RedirectResponse()
                {
                    Pathname = "/Account/ForgotPasswordConfirmation"
                });
            }

            return Ok(ModelState);
        }

        [AllowAnonymous]
        [HttpPost("SignedInUser")]
        public async Task<IActionResult> GetSignedInUser()
        {
            if (_signInManager.IsSignedIn(User))
            {
                var appUser = await _userManager.GetUserAsync(User);
                return Ok(appUser);
            }

            return Ok(false);
        }


        [HttpGet("ChangeEmail")]
        public async Task<IActionResult> ChangeEmail(string newEmail)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            if (!IsModelStateAndEmailValid(newEmail))
            {
                return Ok(ModelState);
            }

            var email = await _userManager.GetEmailAsync(user);
            if (newEmail != email)
            { 
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateChangeEmailTokenAsync(user, newEmail);

                var callbackUrl = $"/Account/ConfirmEmailChange?userId={userId}&email={newEmail}&code={code}";
                await _emailSender.SendEmailAsync(
                    newEmail,
                    "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                return Ok(new StatusResponse()
                {
                    Status = "Confirmation link to change email sent. Please check your email."
                });
            }

            return Ok(new StatusResponse()
            {
                Status = "Your email is unchanged."
            });
        }

        [HttpGet("Logout")]
        public async Task<IActionResult> Logout(string returnUrl)
        {
            await _signInManager.SignOutAsync();

            _logger.LogInformation("User logged out.");
            
            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                return Ok(new RedirectResponse()
                {
                    Pathname = returnUrl
                });
            }
            else
            {
                return Ok(new RedirectResponse()
                {
                    Pathname = ""
                });
            }
        }

        [HttpGet("ManageProfile")]
        public async Task<IActionResult> ManageProfile(string phoneNumberInput)
        {
            var input = new ManageProfileInput() { PhoneNumber = phoneNumberInput };
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!IsModelStateAndObjectValid(input))
            {
                return Ok(ModelState);
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    return Ok(new StatusResponse()
                    {
                        Status = "Unexpected error when trying to set phone number.",
                        AlertType = AlertType.Error
                    });
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            return Ok(new StatusResponse()
            {
                Status = "Your profile has been updated"
            });
        }
       
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody]ChangePasswordInput input)
        {
            if (!ModelState.IsValid)
            {
                return Ok(ModelState);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, input.OldPassword, input.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError("ModelErrors", error.Description);
                }
                return Ok(ModelState);
            }

            await _signInManager.RefreshSignInAsync(user);
            _logger.LogInformation("User changed their password successfully.");

            return Ok(new StatusResponse()
            {
                Status = "Your password has been changed."
            });
        }
                
        [HttpPost("SetPassword")]
        public async Task<IActionResult> SetPassword([FromBody]SetPasswordInput input)
        {
            if (!ModelState.IsValid)
            {
                return Ok(ModelState);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var addPasswordResult = await _userManager.AddPasswordAsync(user, input.NewPassword);
            if (!addPasswordResult.Succeeded)
            {
                foreach (var error in addPasswordResult.Errors)
                {
                    ModelState.AddModelError("ModelErrors", error.Description);
                }
                return Ok(ModelState);
            }

            await _signInManager.RefreshSignInAsync(user);
            var statusMessage = "Your password has been set.";

            return Ok(new StatusResponse()
            {
                Status = statusMessage
            });
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPasswordPost([FromBody]ResetPasswordInput input)
        {
            if (!ModelState.IsValid)
            {
                return Ok(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(input.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return Ok(new RedirectResponse()
                {
                    Pathname = "/Account/ResetPasswordConfirmation"
                });
            }

            var result = await _userManager.ResetPasswordAsync(user, input.Code, input.Password);
            if (result.Succeeded)
            {
                return Ok(new RedirectResponse()
                {
                    Pathname = "/Account/ResetPasswordConfirmation"
                });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("ModelErrors", error.Description);
            }
            return Ok(ModelState);

        }

        [HttpPost("HasPassword")]
        public async Task<IActionResult> HasPassword()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            return Ok(await _userManager.HasPasswordAsync(user));
        }

        [HttpGet("IsMachineRemembered")]
        public async Task<IActionResult> IsMachineRemembered()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Ok(false);
            }

            var isRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user);

            return Ok(isRemembered);
        }
    }
}
