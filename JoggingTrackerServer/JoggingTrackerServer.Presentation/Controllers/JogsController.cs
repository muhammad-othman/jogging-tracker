using AutoMapper;
using JoggingTrackerServer.Domain.Entities;
using JoggingTrackerServer.Domain.Enums;
using JoggingTrackerServer.Domain.Interfaces;
using JoggingTrackerServer.Presentation.ActionFilters;
using JoggingTrackerServer.Presentation.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;

namespace JoggingTrackerServer.Presentation.Controllers
{
    [EnableCors("*", "*", "*")]
    [CustomeAuthorize]
    public class JogsController : ApiController
    {
        IUnitOfWork _unitOfWork;
        public JogsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        private static DateTime StartOfWeek(DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }
            return dt.AddDays(-1 * diff).Date;
        }
        [Route("api/jogs/report/{userID?}")]
        public IHttpActionResult GetReports(int? userID = null)
        {
            try
            {
                var token = Request.Headers.GetValues("access-token").First();
                var user = _unitOfWork.UserRepository.AuthorizateUser(token);

                IEnumerable<JogViewModel> jogs = null;
                if (user.Permission != UserPermission.Admin)
                    jogs = _unitOfWork.JogRepository.GetUserJogging(user.ID, null, null).Select(Mapper.Map<JogViewModel>);
                else if (userID != null)
                    jogs = _unitOfWork.JogRepository.GetUserJogging(userID.Value, null, null).Select(Mapper.Map<JogViewModel>);
                else
                    jogs = _unitOfWork.JogRepository.GetJogging(null, null).Select(Mapper.Map<JogViewModel>);

                var report = jogs.GroupBy(i => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(i.Date, CalendarWeekRule.FirstDay, DayOfWeek.Monday))
                    .Select(e => new { totalDistance= e.Sum(j => j.Distance/1000), average = e.Average(j => (j.Distance/1000) / (j.Duration/60.0)), weekstarts = StartOfWeek(e.First().Date, DayOfWeek.Monday) }).ToList();
                return Ok(report);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        public IHttpActionResult Get(int pageIndex = 1, int pageSize = 10, int? userID = null, DateTime? from = null, DateTime? to = null)
        {
            try
            {
                var token = Request.Headers.GetValues("access-token").First();
                var user = _unitOfWork.UserRepository.AuthorizateUser(token);

                IEnumerable<JogViewModel> jogs = null;
                int totalCount = -1;
                if (user.Permission != UserPermission.Admin)
                {
                    jogs = _unitOfWork.JogRepository.GetUserJogging(user.ID, from, to).Select(Mapper.Map<JogViewModel>).OrderByDescending(e => e.Date);
                    totalCount = jogs.Count();
                    jogs = jogs.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                    return Ok(new { jogs, totalCount });
                }
                else if (userID != null)
                {
                    jogs = _unitOfWork.JogRepository.GetUserJogging(userID.Value, from, to).Select(Mapper.Map<JogViewModel>).OrderByDescending(e => e.Date);
                    totalCount = jogs.Count();
                    jogs = jogs.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                    return Ok(new { jogs, totalCount });
                }
                jogs = _unitOfWork.JogRepository.GetJogging(from, to).Select(Mapper.Map<JogViewModel>).OrderByDescending(e=>e.Date);
                totalCount = jogs.Count();
                jogs = jogs.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                return Ok(new { jogs, totalCount });
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
                var token = Request.Headers.GetValues("access-token").First();
                var user = _unitOfWork.UserRepository.AuthorizateUser(token);
                var jog = _unitOfWork.JogRepository.GetByID(id);

                if (jog == null)
                    return NotFound();

                if (user.Permission != UserPermission.Admin && jog.UserID != user.ID)
                    return Unauthorized();

                return Ok<JogViewModel>(Mapper.Map<JogViewModel>(jog));
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        public IHttpActionResult Post([FromBody]JogViewModel jog)
        {
            if (!ModelState.IsValid) return BadRequest();
            try
            {
                var token = Request.Headers.GetValues("access-token").First();
                var user = _unitOfWork.UserRepository.AuthorizateUser(token);
                jog.UserID = user.ID;
                var g = _unitOfWork.JogRepository.Create(Mapper.Map<Jog>(jog));
                _unitOfWork.Complete();
                if (g != null)
                    return Created<JogViewModel>(Request.RequestUri + "/" + g.ID, Mapper.Map<JogViewModel>(g));
                return BadRequest();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        public IHttpActionResult Put(int id, [FromBody]JogViewModel jog)
        {
            if (!ModelState.IsValid) return BadRequest();
            try
            {


                var token = Request.Headers.GetValues("access-token").First();
                var user = _unitOfWork.UserRepository.AuthorizateUser(token);
                var oldJog = _unitOfWork.JogRepository.GetByID(id);
                if (oldJog == null)
                    return NotFound();
                if (user.Permission != UserPermission.Admin && oldJog.UserID != user.ID)
                    return Unauthorized();

                var g = _unitOfWork.JogRepository.Update(id, Mapper.Map<Jog>(jog));
                _unitOfWork.Complete();
                if (jog != null)
                    return Ok<JogViewModel>(Mapper.Map<JogViewModel>(g));
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
                var user = _unitOfWork.UserRepository.AuthorizateUser(token);
                var oldJog = _unitOfWork.JogRepository.GetByID(id);
                if (oldJog == null)
                    return NotFound();
                if (user.Permission != UserPermission.Admin && oldJog.UserID != user.ID)
                    return Unauthorized();

                var deleted = _unitOfWork.JogRepository.Delete(id);

                if (deleted == null)
                    return NotFound();

                _unitOfWork.Complete();

                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }
    }
}
