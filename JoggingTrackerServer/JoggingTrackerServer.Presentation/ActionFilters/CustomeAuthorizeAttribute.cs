using JoggingTrackerServer.Domain.Enums;
using JoggingTrackerServer.Domain.Interfaces;
using JoggingTrackerServer.Presentation.App_Start;
using Ninject;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace JoggingTrackerServer.Presentation.ActionFilters
{
    public class CustomeAuthorizeAttribute : ActionFilterAttribute
    {
        IUnitOfWork _unitOfWork;
        public CustomeAuthorizeAttribute(UserPermission permission = UserPermission.Regular)
        {
            Permission = permission;
        }
        public UserPermission Permission { get; set; } = 0;
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.Request.Headers.Contains("access-token"))
            {
                UnAuthorizedUser(actionContext);
                return;
            }
            _unitOfWork = NinjectWebCommon.bootstrapper.Kernel.Get<IUnitOfWork>();

            var token = actionContext.Request.Headers.GetValues("access-token").First();
            var user = _unitOfWork.UserRepository.AuthorizateUser(token);

            if (user == null
                || (Permission == UserPermission.Manager && user.Permission == UserPermission.Regular)
                || Permission == UserPermission.Admin && user.Permission != UserPermission.Admin)
            {
                UnAuthorizedUser(actionContext);
                return;
            }
            if (!_unitOfWork.UserRepository.RefreshToken(token))
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.InternalServerError);
                return;
            }
            base.OnActionExecuting(actionContext);

        }
        private void UnAuthorizedUser(HttpActionContext actionContext)
        {
            var unAuthorizedUser = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            actionContext.Response = unAuthorizedUser;
        }

    }
}