using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Recrutment_system.Models.ViewModel;
using Recrutment_system.Models.DB;
using Recrutment_system.Models.EntityManager;
using System.Web.Security;
using Recrutment_system.Security;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;


namespace Recrutment_system.Controllers
{
 
    public class AccountController : Controller
    {
        //
        // GET: /Account/
        public ActionResult companySignup()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult companySignup(RecrutmentSignupView rsv)
        {
            if (ModelState.IsValid)
            {
                UserManager UM = new UserManager();
                if (!UM.IsLoginEmailExits(rsv.CompanyEmail))
                {
                    UM.AddCompanyaccount(rsv);
                    if(rsv.ComapnyrefId != null)
                    {
                        using (MailMessage mm = new MailMessage())
                        {
                            //string code = Guid.NewGuid().ToString();
                            mm.To.Add(new MailAddress(rsv.CompanyEmail));
                            mm.From = new MailAddress("Cofuse@gmail.com");
                            mm.Subject = "Email confirmation !";
                            // mm.Body = "This email create is company use of term and condition <br/> Your Email:- " + rsv.CompanyEmail + " <br/> you Password:-" + password + " ";
                            mm.Body = string.Format("Dear {0}<BR/>Thank you for your registration, please click on the below link to complete your registration: <a href=\"{1}\" title=\"User Email Confirm\">{1}</a>", rsv.ComapanyName, Url.Action("ConfirmEmail", "Account", new { Token = rsv.ComapnyrefId, Email = rsv.CompanyEmail }, Request.Url.Scheme));
                            mm.IsBodyHtml = true;

                            using (SmtpClient smtp = new SmtpClient())
                            {
                                smtp.Host = "smtp.gmail.com";
                                smtp.EnableSsl = true;
                                NetworkCredential networkcred = new NetworkCredential("connectistsindia@gmail.com", "connectists@57confuse");
                                smtp.UseDefaultCredentials = true;
                                smtp.Credentials = networkcred;
                                smtp.Port = 587;
                                smtp.Send(mm);
                                ViewBag.message = "Check inbox on your mail !";
                                return RedirectToAction("Confirm", "Account");

                            }
                        }
                    }
                    //FormsAuthentication.SetAuthCookie(rsv.ComapanyName, false);
                    //TempData["msg"] = "Account Created ! Check Mail get Password.";
                    //return RedirectToAction("Login", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "Email is already taken");
                }
            }
            return View();
        }
        [AllowAnonymous]
        public ActionResult Confirm(string Email)
        {
            ViewBag.Email = Email;
            return View();
        }

        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string Token, string Email)
        {
            //ApplicationUser user = this.UserManager.FindById(Token);
            using (RecrutmentsysEntities db = new RecrutmentsysEntities())
            {

                //var user = from i in db.COMPANY_REGISTER_REFRENCES
                //           where i.ComapanyRefId == Convert.ToInt32(Token)
                //           select i.CompanyEmailRef;

                //var user=db.COMPANY_REGISTER_REFRENCES.Find(Token);
                COMPANY_REGISTER_REFRENCES CRF = db.COMPANY_REGISTER_REFRENCES.Find(Convert.ToInt32(Token));

                int CRFID = CRF.ComapanyRefId;

                if (CRFID != null)
                {
                    string mail=CRF.CompanyEmailRef;
                    if (mail == Email)
                    {
                        COMPANY_REGISTER_REFRENCES crr = db.COMPANY_REGISTER_REFRENCES.Find(Convert.ToInt32(Token));
                        crr.ConfirmedEmail = true;
                        db.SaveChanges();

                        var istrue = db.COMPANY_REGISTER_REFRENCES.Where(o => o.CompanyEmailRef.Equals(mail));
                        bool check =Convert.ToBoolean( istrue.FirstOrDefault().ConfirmedEmail);
                        if(check == true)
                        {
                            TempData["msg"] = "Account Created ! Please Login Here";
                            return RedirectToAction("Login", "Account");
                        }
                        else
                        {
                            return RedirectToAction("Confirm", "Account", new { Email = mail });
                        }
                        
                        //return RedirectToAction("Index", "Home", new { ConfirmedEmail = mail });
                    }
                    else
                    {
                        return RedirectToAction("Confirm", "Account", new { Email = mail });
                    }
                }
                else
                {
                    return RedirectToAction("Confirm", "Account", new { Email = "" });
                }
            }

        }

        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ forgot password here start -!!!!!!!!!!!!
        public ActionResult ResetPassword()
        {
            return View();
        }
        
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ResetPassword(ResetPWD rsp)
        {
            if(ModelState.IsValid)
            {
                using(RecrutmentsysEntities db = new RecrutmentsysEntities())
                {
                    var users = db.COMPANY_REGISTER_REFRENCES.Where(o=>o.CompanyEmailRef.ToLower().Equals(rsp.Email));

                    if (users.Any())
                    {
                        int refid = users.FirstOrDefault().ComapanyRefId;
                        string mail=users.FirstOrDefault().CompanyEmailRef;
                        using(MailMessage mm = new MailMessage())
                        {
                            mm.To.Add(new MailAddress(mail));
                            mm.From = new MailAddress("Cofuse@gmail.com");
                            mm.Subject = "Forgot Password !";
                            // mm.Body = "This email create is company use of term and condition <br/> Your Email:- " + rsv.CompanyEmail + " <br/> you Password:-" + password + " ";
                            mm.Body = string.Format("Dear {0}<BR/>Forgotpassword for your Email, please click on the below link to complete your process: <a href=\"{1}\" title=\"User Email Confirm\">{1}</a>", mail, Url.Action("Forgotpassword", "Account", new { Token = refid, Email = mail }, Request.Url.Scheme));
                            mm.IsBodyHtml = true;

                            using (SmtpClient smtp = new SmtpClient())
                            {
                                smtp.Host = "smtp.gmail.com";
                                smtp.EnableSsl = true;
                                NetworkCredential networkcred = new NetworkCredential("connectistsindia@gmail.com", "connectists@57confuse");
                                smtp.UseDefaultCredentials = true;
                                smtp.Credentials = networkcred;
                                smtp.Port = 587;
                                smtp.Send(mm);
                                ModelState.Clear();
                                return RedirectToAction("ResetPassword", "Account", new {message="Check You inbox get forgot password link" });

                            }
                        }

                    }
                    else
                    {
                        ModelState.AddModelError("","Your Email is not valid");
                    }
                }
            }
            return View();
        }
        [AllowAnonymous]
        public async Task<ActionResult> Forgotpassword(int Token, string Email)
        {
            Session["mail"] = Email;
            Session["token"] = Token;
            return View();
        }

        [HttpPost]
        public ActionResult Forgotpassword(forgetpassword fp)
        {
            string m= Session["mail"].ToString();
            string t = Session["token"].ToString();
           

            if(ModelState.IsValid)
            {
                UserManager UM = new UserManager();
                UM.Fogot_to_new(fp, m, t);
                TempData["msg"] = "Password Changed Succssefully";
                return RedirectToAction("Login", "Account");

            }
            return View();

        }

        
        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ end reset password here ~!!!!!!!!!!!!
        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogIn(UserLoginView ULV,string returnUrl)
        {
            if (ModelState.IsValid)
            {
                UserManager UM = new UserManager();

                string password = UM.GetUserPassword(ULV.LoginEmail);
                if (string.IsNullOrEmpty(password))
                {
                    ModelState.AddModelError("", "The UserLogin or Password is in correct");
                }
                else
                {
                    if (ULV.Password.Equals(password))
                    {
                        using(RecrutmentsysEntities db = new RecrutmentsysEntities())
                        {
                            var istrue = db.COMPANY_REGISTER_REFRENCES.Where(o => o.CompanyEmailRef.Equals(ULV.LoginEmail));
                            bool check =Convert.ToBoolean( istrue.FirstOrDefault().ConfirmedEmail);

                            if(check==true)
                            {
                                FormsAuthentication.SetAuthCookie(ULV.LoginEmail, false);
                                return RedirectToAction("AdminOnly", "Home");
                            }
                            else
                            {
                                ModelState.AddModelError("", "Please Confirm Your Account");
                                return View(ULV);
                            }
                            
                        }
                        
                        
                    }
                    else
                    {
                        ModelState.AddModelError("", "The password provide is incorrect");
                    }
                }
            }
            // If we got this far, something failed, redisplay form

            return View(ULV);
        }


        [AuthorizeRole("Admin")]
        public ActionResult Createuser()
        {
            return View();
        }
        

        [HttpPost]
        public ActionResult Createuser(UserSignup US)
        {
            if (ModelState.IsValid)
            {
                UserManager UM = new UserManager();
                if (!UM.IsLoginUsercheck(US.HrEmail))
                {
                    string loginname = HttpContext.User.Identity.Name;
                    if (loginname != null)
                    {
                        UM.AddUseraccount(US, loginname);
                        ModelState.Clear();
                        TempData["msg"] = "You have Create Your Comany Hr Account! Thank you.";
                        TempData["msg1"] = "Password Sent to Hr Mail .";
                        //FormsAuthentication.SetAuthCookie(US.HrEmail, false);
                        //return RedirectToAction("AdminOnly", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Admin login not approve");
                    }
                }
                else
                {
                    ModelState.AddModelError("","Login Email Alredy tekan");
                }
            }
            return View();
        }

        //here user is authorize then hr or other persone will add job manully
        [IsUserAuthorize("Other")]
        public ActionResult NewJobOpen()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewJobOpen(NewJobView NJV)
        {
            if (ModelState.IsValid)
            {
                string loginame = HttpContext.User.Identity.Name;
                if (loginame != null)
                {
                    UserManager UM = new UserManager();
                    UM.AddnewjobIsopen(NJV, loginame);
                    //NewJobView Njview = new NewJobView() { DepartmentName = string.Empty, Name_of_Vacancy = string.Empty, Number_of_Position = default(int), Required_Technical_Skill = string.Empty, Special_Technical_Skill = string.Empty, Min_Yearof_Experience = default(int), Min_Qualification = string.Empty, Deadline_Date=default(DateTime) };
                    ModelState.Clear();
                    ViewBag.clear = "Save Successfully";
                    return View();
                }
                else
                {
                    ModelState.AddModelError("", "User login is not Authnticated !");
                }
            }
            return View();
        }

        //get department code with data..
        [IsUserAuthorize("Other")]
        public ActionResult addcandidate()
        {
            string loginname = HttpContext.User.Identity.Name;
          
            RecrutmentsysEntities db = new RecrutmentsysEntities();

            var id = db.COMPANY_HR_REGISTER.Where(o => o.HrEmail == loginname);
            int hrid = id.FirstOrDefault().HrId;
            List<COMPANY_JOB_OPENING> objdeptcode = (from data in db.COMPANY_JOB_OPENING
                                                     where data.HrId==hrid && data.Approval==true
                                                     select data).ToList();
            COMPANY_JOB_OPENING cjo = new COMPANY_JOB_OPENING();
            cjo.DepartmentCode = "Select";
            objdeptcode.Insert(0, cjo);
            SelectList objdpt = new SelectList(objdeptcode, "DepartmentCode", "DepartmentCode", 0);
            candidatepostion obj = new candidatepostion();
            obj.Deptcode = objdpt;
            
            return View(obj);
        }
        [HttpPost]
        public ActionResult addcandidate(candidatepostion cp)
        {


            candidate();
            candidatepostion obj = new candidatepostion();
            obj.Deptcode = ViewBag.obj1;
                if (ModelState.IsValid)
                {
                    if (cp.departmentcode == "Select")
                    {
                        ViewBag.foo = "you have not select please selct";
                        return View(obj);
                    }
                    else
                    {
                        
                        UserManager UM = new UserManager();
                        if (!UM.IspostionEmailExits(cp.email))
                        {
                            //ViewBag.foo = cp.departmentcode;
                            UM.addcandidateposition(cp, cp.departmentcode);
                            ModelState.Clear();
                            candidate();
                            obj.Deptcode = ViewBag.obj1;
                            TempData["msg"] = "Insert save successfully";

                            
                        }
                        else
                        {
                            ModelState.AddModelError("","Email Is Already in use");
                        }
                    }
                    
                }
                if (cp.departmentcode == "Select")
                {
                    ViewBag.foo = "please select first?";
                    return View(obj);
                }

                return View(obj);
           
        }
        public void candidate()
        {

            string loginname = HttpContext.User.Identity.Name;

            RecrutmentsysEntities db = new RecrutmentsysEntities();

            var id = db.COMPANY_HR_REGISTER.Where(o => o.HrEmail == loginname);
            int hrid = id.FirstOrDefault().HrId;
            List<COMPANY_JOB_OPENING> objdeptcode = (from data in db.COMPANY_JOB_OPENING
                                                     where data.HrId == hrid && data.Approval == true
                                                     select data).ToList();
            COMPANY_JOB_OPENING cjo = new COMPANY_JOB_OPENING();
            cjo.DepartmentCode = "Select";
            objdeptcode.Insert(0, cjo);
            ViewBag.obj1 = new SelectList(objdeptcode, "DepartmentCode", "DepartmentCode", 0);
        }
      
        //public void populatedepartmentcode(Object selectedDepartmentcode=null)
        //{
        //    using (RecrutmentsysEntities db = new RecrutmentsysEntities())
        //    {
        //        var deptquery = from d in db.COMPANY_JOB_OPENING
        //                        orderby d.DepartmentCode
        //                        select d;
        //        ViewBag.Departmentcode = new SelectList(deptquery, "DepartmentCode", "DepartmentCode", selectedDepartmentcode);
        //    }
                          
        //}

        [IsUserAuthorize("Other")]
        public ActionResult AddCandidateFeedback()
        {
            string loginname = HttpContext.User.Identity.Name;
            RecrutmentsysEntities db = new RecrutmentsysEntities();
            var id = db.COMPANY_HR_REGISTER.Where(o => o.HrEmail == loginname);
            int hrid = id.FirstOrDefault().HrId;

            List<CANDIDATE_POSITION> CNP = (from data in db.CANDIDATE_POSITION                                            
                                            where data.HrId==hrid && data.ISActive==true                                           
                                            select data).ToList();
            CANDIDATE_POSITION CANP = new CANDIDATE_POSITION();
            CANP.CandidateCode = "Select";
            CNP.Insert(0, CANP);
            SelectList Obj = new SelectList(CNP, "CandidateCode", "CandidateCode", 0);
            candidatefeedback addcanfeedback = new candidatefeedback();
            addcanfeedback.candicode = Obj;

            return View(addcanfeedback);
        }

        [HttpPost]
        public ActionResult AddCandidateFeedback(candidatefeedback cfb)
        {

            //string loginname = HttpContext.User.Identity.Name;
            //RecrutmentsysEntities db = new RecrutmentsysEntities();
            //var id = db.COMPANY_HR_REGISTER.Where(o => o.HrEmail == loginname);
            //int hrid = id.FirstOrDefault().HrId;

            //List<CANDIDATE_POSITION> CNP = (from data in db.CANDIDATE_POSITION
            //                                where data.HrId == hrid && data.ISActive == true 
            //                                select data).ToList();
            //CANDIDATE_POSITION CANP = new CANDIDATE_POSITION();
            //CANP.CandidateCode = "Select";
            //CNP.Insert(0, CANP);
            //SelectList Obj = new SelectList(CNP, "CandidateCode", "CandidateCode", 0);
            feedbackselect();
            candidatefeedback addcanfeedback = new candidatefeedback();
            addcanfeedback.candicode = ViewBag.obj;
            if (ModelState.IsValid)
            {
                if (cfb.candidatecode == "Select")
                {

                    ModelState.AddModelError("", "Please select First");
                    return View(addcanfeedback);
                }
                else
                {
                    UserManager UM = new UserManager();
                    UM.addcandidatefeedbackposition(cfb);
                    ModelState.Clear();
                    TempData["msg"] = "Insert successfully";
                        feedbackselect();
                    addcanfeedback.candicode = ViewBag.obj;

                }
            }

            return View(addcanfeedback);
        }

        public void feedbackselect()
        {
            string loginname = HttpContext.User.Identity.Name;
            RecrutmentsysEntities db = new RecrutmentsysEntities();
            var id = db.COMPANY_HR_REGISTER.Where(o => o.HrEmail == loginname);
            int hrid = id.FirstOrDefault().HrId;

            List<CANDIDATE_POSITION> CNP = (from data in db.CANDIDATE_POSITION
                                            where data.HrId == hrid && data.ISActive == true
                                            select data).ToList();
            CANDIDATE_POSITION CANP = new CANDIDATE_POSITION();
            CANP.CandidateCode = "Select";
            CNP.Insert(0, CANP);
            ViewBag.obj = new SelectList(CNP, "CandidateCode", "CandidateCode", 0);
            
        }
        [Authorize]
        public ActionResult SignOut()
        {
            System.Web.Security.FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [AuthorizeRole("Admin")]
        public ActionResult GetData()
        {
            if(User.Identity.IsAuthenticated)
            {
                //string Loginname = HttpContext.User.Identity.Name;
                //var id = db.COMPANY_REGISTER_REFRENCES.Where(o => o.CompanyEmailRef == Loginname);
                //int compid =Convert.ToInt32( id.FirstOrDefault().CompanyId);
                //var listusers = (from cjo in db.CANDIDATE_FEEDBACK_POSITION
                //                 join rdf in db.Round_Detail_feedback on cjo.CandidateCode equals rdf.CandidateCode
                //                 where cjo.CompanyId == compid
                //                 select new { cjo.CandidateCode,cjo.DepartmentCode, rdf.totalround_per }).ToList();
               // return Json(new { data = listusers }, JsonRequestBehavior.AllowGet);

                string Loginname = HttpContext.User.Identity.Name;
                UserManager UM = new UserManager();
                Listofoffer LO = UM.getlistoffer(Loginname);
                return PartialView(LO);

                
            }
            return View();
        }

        [AuthorizeRole("Admin")]
        public ActionResult ListofUsers()
        {
            return View();
        }
	}
}