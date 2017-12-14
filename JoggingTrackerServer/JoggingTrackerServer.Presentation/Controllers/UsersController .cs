using AutoMapper;
using JoggingTrackerServer.Domain.Entities;
using JoggingTrackerServer.Domain.Enums;
using JoggingTrackerServer.Domain.Interfaces;
using JoggingTrackerServer.Presentation.ActionFilters;
using JoggingTrackerServer.Presentation.Models;
using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;

namespace JoggingTrackerServer.Presentation.Controllers
{
    [CustomeAuthorize(UserPermission.Manager)]
    public class UsersController : ApiController
    {
        IUnitOfWork _unitOfWork;
        public UsersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IHttpActionResult Get(int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var users = _unitOfWork.UserRepository.GetAll().Select(Mapper.Map<UserViewModel>);
                int totalCount = users.Count();
                users = users.Skip((pageIndex - 1) * pageSize).Take(pageSize);

                return Ok(new { users, totalCount });
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }
        public IHttpActionResult Get(int id)
        {
            try
            {
                var user = _unitOfWork.UserRepository.GetByID(id);
                if (user == null)
                    return NotFound();
                return Ok<UserViewModel>(Mapper.Map<UserViewModel>(user));
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }
        public IHttpActionResult Post([FromBody]UserViewModel user)
        {
            if (!ModelState.IsValid) return BadRequest();
            try
            {
                var token = Request.Headers.GetValues("access-token").First();
                var creator = _unitOfWork.UserRepository.AuthorizateUser(token);

                if (user.Permission == UserPermission.Admin && creator.Permission == UserPermission.Manager)
                    return Unauthorized();
                var u = _unitOfWork.UserRepository.Create(Mapper.Map<User>(user));
                _unitOfWork.Complete();
                if (u != null)
                    return Created<UserViewModel>(Request.RequestUri + "/" + u.ID, Mapper.Map<UserViewModel>(u));
                return BadRequest();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }
        public IHttpActionResult Put(int id, [FromBody]UserViewModel user)
        {
            if (!ModelState.IsValid) return BadRequest();
            try
            {
                var token = Request.Headers.GetValues("access-token").First();
                var creator = _unitOfWork.UserRepository.AuthorizateUser(token);

                if (user.Permission == UserPermission.Admin && creator.Permission == UserPermission.Manager)
                    return Unauthorized();

                var u = _unitOfWork.UserRepository.Update(id, Mapper.Map<User>(user));
                _unitOfWork.Complete();
                if (user != null)
                    return Ok<UserViewModel>(Mapper.Map<UserViewModel>(u));
                return NotFound();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }
        public IHttpActionResult Delete(int id)
        {
            try
            {
                var token = Request.Headers.GetValues("access-token").First();
                var creator = _unitOfWork.UserRepository.AuthorizateUser(token);
                var user = _unitOfWork.UserRepository.GetByID(id);

                if (user == null)
                    return NotFound();
                if (user.Permission == UserPermission.Admin && creator.Permission == UserPermission.Manager)
                    return Unauthorized();

                var deleted = _unitOfWork.UserRepository.Delete(id);
                _unitOfWork.Complete();

                if (deleted != null)
                    return StatusCode(HttpStatusCode.NoContent);
                return NotFound();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }
    }
}
