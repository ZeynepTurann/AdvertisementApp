using AdvertisementApp.Business.Interfaces;
using AdvertisementApp.Common.Enums;
using AdvertisementApp.Dtos;
using AdvertisementApp.UI.Extensions;
using AdvertisementApp.UI.Models;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AdvertisementApp.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IGenderService _genderService;
        private readonly IValidator<UserCreateModel> _userCreateModelValidator;
        private readonly IAppUserService _appUserService;
        private readonly IMapper _mapper;

        public AccountController(IGenderService genderService, IValidator<UserCreateModel> userCreateModelValidator, IAppUserService appUserService, IMapper mapper, IValidator<AppUserLoginDto> appUserLoginDtoValidator)
        {
            _genderService = genderService;
            _userCreateModelValidator = userCreateModelValidator;
            _appUserService = appUserService;
            _mapper = mapper;
        }

        public async Task<IActionResult> SignUp()
        {
            var response= await _genderService.GetAllAsync();
            var model = new UserCreateModel
            {
                Genders = new SelectList(response.Data, "Id", "Definition")
            };
            return View(model); 
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(UserCreateModel model)
        {
           var result= _userCreateModelValidator.Validate(model);
            if (result.IsValid)
            {
               var createResponse = await _appUserService.CreateWithRoleAsync(_mapper.Map<AppUserCreateDto>(model),(int)RoleType.Member);
                return this.ResponseRedirectAction(createResponse, "LogIn");
          
            }
            foreach (var item in result.Errors)
            {
                ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
            }
            var response = await _genderService.GetAllAsync();
            model.Genders = new SelectList(response.Data, "Id", "Definition",model.GenderId);
            return View(model);
        }

        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(AppUserLoginDto model)
        {
           var result = await _appUserService.CheckUserAsync(model);
            if (result.ResponseType == ResponseType.Success)
            {
              var roleResult = await _appUserService.GetRolesByUserIdAsync(result.Data.Id);
                //we need to get roles of related user
                var claims = new List<Claim>();
                if (roleResult.ResponseType==ResponseType.Success)
                {
                    foreach (var role in roleResult.Data)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role.Definition));
                       
                       
                    }
                  
                }
                //if related user has no role
                claims.Add(new Claim(ClaimTypes.NameIdentifier, result.Data.Id.ToString()));
               

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {

                    IsPersistent = model.RememberMe
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", result.Message);
            return View();

        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(
             CookieAuthenticationDefaults.AuthenticationScheme);    //custom cookie based microsoft website
            return RedirectToAction("Index", "Home");
        }

    }
}
