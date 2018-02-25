using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Recrutment_system.Security;
using Recrutment_system.Models.EntityManager;
using Recrutment_system.Models.ViewModel;

namespace Recrutment_system.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [AuthorizeRole("Admin")]
        public ActionResult AdminOnly()
        {
            return View();
        }

        public ActionResult UnAuthorized()
        {

            return View();
        }
        // @@@@@@@@********** Admin
        [AuthorizeRole("Admin")]
        public ActionResult ListOfApprovalJobs()
        {
            return View();
        }
        [AuthorizeRole("Admin")]
        public ActionResult ManageAprroval(string status="")
        {
            if (User.Identity.IsAuthenticated)
            {
                string loginName = User.Identity.Name;
                UserManager UM = new UserManager();
                listJobView LJV = UM.getlistofJob(loginName);

                string message = string.Empty;
                if (status.Equals("Approval"))
                {
                    message = "approvad";
                }
                else if (status.Equals("NotApproval"))
                {
                    message = "not approvad";
                }

                ViewBag.Message = message;


                return PartialView(LJV);
            }
            //return View();
            return RedirectToAction("Index", "Home");
        }

        [AuthorizeRole("Admin")]
        public ActionResult Approvaljob(int approvalid,bool apr)
        {
            UsersJob UJ = new UsersJob();
            UJ.DepatmentID = approvalid;
            UJ.Approval_admin = apr;
            UserManager UM =new UserManager();
            UM.approvaljob(UJ);
            return Json(new { success = true });
            //return RedirectToAction("Index", "Home");
        }
        // **************@@@@@@@@@@@
        //================================================================Users isactive starts

        [IsUserAuthorize("Other")]
        public ActionResult ListOfIsActive()
        {
            return View();
        }
          [IsUserAuthorize("Other")]
        public ActionResult ISActive(string status="")
        {
            if (User.Identity.IsAuthenticated)
            {
                string Loginname = HttpContext.User.Identity.Name;
                UserManager UM = new UserManager();
               ListofIsactive LIA= UM.Isactiveornot(Loginname);
               string message = string.Empty;
               if (status.Equals("Active"))
               {
                   message = "approvad";
               }
               else if (status.Equals("NotActive"))
               {
                   message = "not approvad";
               }

               ViewBag.Message = message;
               return PartialView(LIA);
            }
            return View();
        }
      
       [IsUserAuthorize("Other")]
        public ActionResult approveisactive(int id,bool boolli)
          {
              candidatepostion can = new candidatepostion();
              can.canid = id;
              can.Isactive = boolli;
              UserManager UM = new UserManager();
              UM.approvalisactiveuser(can);
              return Json(new { success = true });
          }

        //===============================================================User active  close
        public ActionResult ListOfJobs()
        {
            return View();
        }
        [IsUserAuthorize("Other")]
        public ActionResult ManageJobIsOpen(string status = "")
        {
            if (User.Identity.IsAuthenticated)
            {
                string loginName = User.Identity.Name;
                UserManager UM = new UserManager();
                listJobView LJV = UM.getlistofJob(loginName);
                string message = string.Empty;
                if (status.Equals("update"))
                    message = "Update Successful";
                else if (status.Equals("delete"))
                    message = "Delete Successful";

                ViewBag.Message = message;


                return PartialView(LJV);
            }
            //return View();
            return RedirectToAction("Index", "Home");
        }


        [IsUserAuthorize("Other")]
        public ActionResult Updatejobdata(int deptId,string deptcode, string deptname, string reqsetedby, string nameofvacansy, int numberofposition, string requiredskill, string specialskill, double minyofexp, string minqualifi, DateTime deadlinedt)
        {
            UsersJob UJ = new UsersJob();
            UJ.DepatmentID = deptId;
            UJ.DepartmentCode = deptcode;
            UJ.DepartmentName = deptname;
            UJ.RequestedBy = reqsetedby;
            UJ.Name_of_Vacancy = nameofvacansy;
            UJ.Number_of_Position = numberofposition;
            UJ.Required_Technical_Skill = requiredskill;
            UJ.Special_Technical_Skill = specialskill;
            UJ.Min_Yearof_Experience = minyofexp;
            UJ.Min_Qualification = minqualifi;
            UJ.Deadline_Date = deadlinedt;

            UserManager UM = new UserManager();
            UM.Updatejobsdata(UJ);
            return Json(new { success = true });
        }

        [IsUserAuthorize("Other")]
        public ActionResult DeleteJob(int depetid)
        {
            UserManager UM = new UserManager();
            UM.DeleteJob(depetid);
            return Json(new { success = true });

        }
        

        
    }

    
}