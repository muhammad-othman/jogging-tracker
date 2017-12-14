using AutoMapper;
using JoggingTrackerServer.Domain.Entities;
using JoggingTrackerServer.Domain.Enums;
using JoggingTrackerServer.Domain.Interfaces;
using JoggingTrackerServer.Presentation.ActionFilters;
using JoggingTrackerServer.Presentation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace JoggingTrackerServer.Presentation.Controllers
{
    public class AccountController : ApiController
    {
        IUnitOfWork _unitOfWork;
        public AccountController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpPost]
        [Route("api/login")]
        public IHttpActionResult Login([FromBody]LoginViewModel loginInfo)
        {
            if (!ModelState.IsValid) return BadRequest("Check Your Info");
            try
            {
                string AccessToken;
                User userinfo;
                var result = _unitOfWork.UserRepository.Login(loginInfo.UserName, loginInfo.Password, loginInfo.Remmber, out AccessToken, out userinfo);
                if (result == LoginResult.UserNotFound)
                    return BadRequest("Check Your Info");
                else if (result == LoginResult.WrongPassword)
                    return BadRequest("Check Your Info");

                var user = Mapper.Map<UserInfoViewModel>(userinfo);
                return Ok(new { AccessToken, user });


            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }
        [HttpPost]
        [Route("api/logout")]
        [CustomeAuthorize]
        public IHttpActionResult Logout()
        {
            try
            {
                var token = Request.Headers.GetValues("access-token").First();
                if (_unitOfWork.UserRepository.Logout(token))
                    return NotFound();
                return Ok();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }
        [HttpGet]
        [Route("api/userinfo")]
        public IHttpActionResult UserInfo()
        {
            try
            {
                var token = Request.Headers.GetValues("access-token").FirstOrDefault();
                if (String.IsNullOrEmpty(token)) return Unauthorized();
                var user = _unitOfWork.UserRepository.AuthorizateUser(token);
                if (user == null)
                    return Unauthorized();
                return Ok(Mapper.Map<UserInfoViewModel>(user));
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
        [HttpPost]
        [Route("api/register")]
        public IHttpActionResult Register([FromBody]UserViewModel userInfo)
        {
            if (!ModelState.IsValid) return BadRequest();
            try
            {
                userInfo.Permission = UserPermission.Regular;


                var u = _unitOfWork.UserRepository.Create(Mapper.Map<User>(userInfo));
                _unitOfWork.Complete();
                if (u == null)
                    return BadRequest();

                return Login(new LoginViewModel { UserName = u.UserName, Password = u.Password, Remmber = false });
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }
        [Route("api/permissions")]
        [CustomeAuthorize(UserPermission.Manager)]
        public IHttpActionResult GetUsersPermissions()
        {
            try
            {
                var token = Request.Headers.GetValues("access-token").FirstOrDefault();
                var user = _unitOfWork.UserRepository.AuthorizateUser(token);
                List<object> list = new List<object>();

                foreach (UserPermission val in Enum.GetValues(typeof(UserPermission)))
                {
                    list.Add(new { permission = (int)val, permissionName = Enum.GetName(typeof(UserPermission), val) });
                }
                    
                return Ok(list);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }
    }
}
