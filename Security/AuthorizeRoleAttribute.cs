using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Recrutment_system.Models.DB;
using Recrutment_system.Models.EntityManager;
namespace Recrutment_system.Security
{
    public class AuthorizeRoleAttribute:AuthorizeAttribute
    {
        private readonly string[] userAssignRoles;

        public AuthorizeRoleAttribute(params string[] roles)
        {
            this.userAssignRoles = roles;
        }

        protected override bool AuthorizeCore(HttpContextBase HttpContext)
        {
            bool authorize = false;
            using(RecrutmentsysEntities db = new RecrutmentsysEntities())
            {
                UserManager UM = new UserManager();
                foreach (var roles in userAssignRoles)
                {
                    authorize = UM.IsUserInRole(HttpContext.User.Identity.Name, roles);
                    if (authorize)
                        return authorize;
                }
                
            }
            return authorize;
            
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filtercontext)

        {
            filtercontext.Result = new RedirectResult("~/Home/Index");
        }

          
    }
}