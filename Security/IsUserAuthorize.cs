using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Recrutment_system.Models.DB;
using Recrutment_system.Models.EntityManager;
namespace Recrutment_system.Security

{
    public class IsUserAuthorize:AuthorizeAttribute
    {
        private readonly string[] userAssignRoles;
        public IsUserAuthorize(params string[] roles)
        {
            this.userAssignRoles = roles;
        }

        protected override bool AuthorizeCore(HttpContextBase HttpContext)
        {
            bool isauthorize = false;
            using (RecrutmentsysEntities db = new RecrutmentsysEntities())
            {
                UserManager UM = new UserManager();
                foreach (var roles in userAssignRoles)
                {
                    isauthorize = UM.ISAuthorizeUserInRole(HttpContext.User.Identity.Name, roles);
                    if (isauthorize)
                        return isauthorize;
                }

                
            }

            return isauthorize;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filtercontext)
        {
            filtercontext.Result = new RedirectResult("~/Home/Index");
        }

    }

}