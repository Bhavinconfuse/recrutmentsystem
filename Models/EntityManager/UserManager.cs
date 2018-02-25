using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Recrutment_system.Models.ViewModel;
using Recrutment_system.Models.DB;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;
using Microsoft.Owin.Security;
using System.Web.Mvc;


namespace Recrutment_system.Models.EntityManager
{
    public class UserManager
    {
        public void AddCompanyaccount(RecrutmentSignupView rsv)
        {
            
            using (RecrutmentsysEntities db = new RecrutmentsysEntities())
            {
                COMPANY_REGISTER CR = new COMPANY_REGISTER();
                CR.ComapanyName = rsv.ComapanyName;
                CR.CompanyEmail = rsv.CompanyEmail;
                CR.CompanyContect = rsv.CompanyContect;
                CR.CompanyAddress = rsv.CompanyAddress;
                db.COMPANY_REGISTER.Add(CR);
                db.SaveChanges();
                

                //string password = RandomString(8);
                
                COMPANY_REGISTER_REFRENCES CRF = new COMPANY_REGISTER_REFRENCES();
                CRF.CompanyId = CR.CompanyId;
                CRF.CompanyEmailRef = rsv.CompanyEmail;
                CRF.CompanyPassword = rsv.CompanyPassword;
                CRF.CompanyType = "Admin";
                CRF.ConfirmedEmail = false;
                db.COMPANY_REGISTER_REFRENCES.Add(CRF);
                db.SaveChanges();
                rsv.ComapnyrefId = CRF.ComapanyRefId;

                //using (MailMessage mm = new MailMessage())
                //{

                //    mm.To.Add(new MailAddress(rsv.CompanyEmail));
                //    mm.From = new MailAddress("Cofuse@gmail.com");
                //    mm.Subject = "Email confirmation !";
                //   // mm.Body = "This email create is company use of term and condition <br/> Your Email:- " + rsv.CompanyEmail + " <br/> you Password:-" + password + " ";
                //    mm.Body = string.Format("Dear {0}<BR/>Thank you for your registration, please click on the below link to complete your registration: <a href=\"{1}\" title=\"User Email Confirm\">{1}</a>", CR.ComapanyName, Url.Action("ConfirmEmail", "Account", new { Token = CRF.ComapanyRefId, Email = CRF.CompanyEmailRef }, Request.Url.Scheme));
                //    mm.IsBodyHtml = true;

                //    using (SmtpClient smtp = new SmtpClient())
                //    {
                //        smtp.Host = "smtp.gmail.com";
                //        smtp.EnableSsl = true;
                //        NetworkCredential networkcred = new NetworkCredential("connectistsindia@gmail.com", "connectists@57confuse");
                //        smtp.UseDefaultCredentials = true;
                //        smtp.Credentials = networkcred;
                //        smtp.Port = 587;
                //        smtp.Send(mm);


                //    }
                //}
            }
            

        }

        public string RandomString(int noOfCharacter)
        {
            string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random r = new Random();
            string randonpassword = new string(
                Enumerable.Repeat(characters, noOfCharacter)
                          .Select(s => s[r.Next(s.Length)])
                          .ToArray());
            return randonpassword;
        }

        public bool IsLoginEmailExits(string Loginemail)
        {
            using (RecrutmentsysEntities db = new RecrutmentsysEntities())
            {
                return db.COMPANY_REGISTER.Where(o => o.CompanyEmail.Equals(Loginemail)).Any();
                
            }
        }

        //other user exits check...
        public bool IsLoginUsercheck(string Loginemail)
        {
            using (RecrutmentsysEntities db = new RecrutmentsysEntities())
            {
                return db.COMPANY_REGISTER_REFRENCES.Where(o => o.CompanyEmailRef.Equals(Loginemail)).Any();
            }
        }

        public string GetUserPassword(string Loginemail)
        {
            using (RecrutmentsysEntities db = new RecrutmentsysEntities())
            {
                var user = db.COMPANY_REGISTER_REFRENCES.Where(o => o.CompanyEmailRef.ToLower().Equals(Loginemail));
                if (user.Any())
                {
                    return user.FirstOrDefault().CompanyPassword;
                }
                else
                {
                    return string.Empty;
                }

            }
        }

        public bool IsUserInRole(string loginname, string rolename)
        {
            using (RecrutmentsysEntities db = new RecrutmentsysEntities())
            {

                COMPANY_REGISTER_REFRENCES CRF = db.COMPANY_REGISTER_REFRENCES.Where(o => o.CompanyEmailRef.ToLower().Equals(loginname)).FirstOrDefault();
                if (CRF != null)
                {
                    var roles = from cr in db.COMPANY_REGISTER
                                join crf in db.COMPANY_REGISTER_REFRENCES on cr.CompanyId equals crf.CompanyId
                                where crf.CompanyType.Equals(rolename) && crf.CompanyEmailRef.Equals(loginname)
                                select cr.CompanyEmail;

                    if (roles != null)
                    {
                        return roles.Any();
                    }
                }

                return false;
            }
        }

        // here is other user in role..check out
        public bool ISAuthorizeUserInRole(string Loginname, string rolename)
        {
            using (RecrutmentsysEntities db = new RecrutmentsysEntities())
            {
                COMPANY_REGISTER_REFRENCES CRF = db.COMPANY_REGISTER_REFRENCES.Where(o => o.CompanyEmailRef.ToLower().Equals(Loginname)).FirstOrDefault();
                if (CRF != null)
                {
                    var roles = from chr in db.COMPANY_HR_REGISTER
                                join crr in db.COMPANY_REGISTER_REFRENCES on chr.CompanyId equals crr.CompanyId
                                where crr.CompanyType.Equals(rolename) && chr.HrEmail.Equals(Loginname)
                                select chr.HrEmail;

                    if (roles != null)
                    {
                        return roles.Any();
                    }
                }
            }

            return false;
        }


        /////if the hr or particulaer person register.

        public void AddUseraccount(UserSignup US,string LoginName)
        {
            using (RecrutmentsysEntities db = new RecrutmentsysEntities())
            {

                COMPANY_REGISTER_REFRENCES CRF = db.COMPANY_REGISTER_REFRENCES.Where(o => o.CompanyEmailRef.Equals(LoginName)).FirstOrDefault();
                    COMPANY_HR_REGISTER CHR = new COMPANY_HR_REGISTER();
                    CHR.CompanyId = CRF.CompanyId;
                    CHR.HrName = US.HrName;
                    CHR.HrContact = US.HrContact;
                    CHR.HrEmail = US.HrEmail;

                    db.COMPANY_HR_REGISTER.Add(CHR);
                    db.SaveChanges();

                    string password=RandomString(8);

                    COMPANY_REGISTER_REFRENCES CRR = new COMPANY_REGISTER_REFRENCES();
                    CRR.CompanyId = CHR.CompanyId;
                    CRR.CompanyEmailRef = US.HrEmail;
                    CRR.CompanyPassword = password;
                    CRR.CompanyType = "Other";
                    db.COMPANY_REGISTER_REFRENCES.Add(CRR);
                    db.SaveChanges();

                    using (MailMessage mm = new MailMessage())
                    {

                        mm.To.Add(new MailAddress(US.HrEmail));
                        mm.From = new MailAddress("Cofuse@gmail.com");
                        mm.Subject = "Your Password Genrated for by Admin!";
                        mm.Body = "This email create is company use of term and condition <br/> Your Email:- " + US.HrEmail + " <br/> you Password:-" + password + " ";
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


                        }
                    }
            }
        }


        // far to the new job opening 

        public void AddnewjobIsopen(NewJobView NJV, string Loginname)
        {
            using (RecrutmentsysEntities db = new RecrutmentsysEntities())
            {
                COMPANY_HR_REGISTER CHR = db.COMPANY_HR_REGISTER.Where(o => o.HrEmail.Equals(Loginname)).FirstOrDefault();
                COMPANY_JOB_OPENING CJO = new COMPANY_JOB_OPENING();
                CJO.CompanyId =Convert.ToInt32(CHR.CompanyId);
                CJO.HrId = CHR.HrId;
                CJO.DepartmentName = NJV.DepartmentName;
                CJO.RequestedBy = NJV.RequestedBy;
                CJO.HrEmail = Loginname;
                CJO.Name_of_Vacancy = NJV.Name_of_Vacancy;
                CJO.Number_of_Position = NJV.Number_of_Position;
                CJO.Required_Technical_Skill = NJV.Required_Technical_Skill;
                CJO.Special_Technical_Skill = NJV.Special_Technical_Skill;
                CJO.Min_Yearof_Experience = NJV.Min_Yearof_Experience;
                CJO.Min_Qualification = NJV.Min_Qualification;
                CJO.Deadline_Date = NJV.Deadline_Date;
                CJO.Requested_Date = DateTime.Now;
                CJO.Approval = false;

                db.COMPANY_JOB_OPENING.Add(CJO);
                db.SaveChanges();

                COMPANY_JOB_OPENING_REFRENCES CJOR = new COMPANY_JOB_OPENING_REFRENCES();
                CJOR.CompanyId = CJO.CompanyId;
                CJOR.HrId = CJO.HrId;
                CJOR.DepartmentCode =Convert.ToString(CJO.DepartmentCode);
                CJOR.Number_of_Position = CJO.Number_of_Position;
                CJOR.RowcreatedDate = DateTime.Now;
                CJOR.RowModifiddate = DateTime.Now;

                db.COMPANY_JOB_OPENING_REFRENCES.Add(CJOR);
                db.SaveChanges();


            }
        }

        //get the record
        public List<UsersJob> getuserjobs(string LoginName)
        {
            List<UsersJob> userjobs = new List<UsersJob>();
            using (RecrutmentsysEntities db = new RecrutmentsysEntities())
            {
                var type = db.COMPANY_REGISTER_REFRENCES.Where(o => o.CompanyEmailRef.Equals(LoginName));
                string typefor = type.FirstOrDefault().CompanyType;
                if (typefor == "Other")
                {
                    UsersJob UJV;
                    var hr_id = db.COMPANY_HR_REGISTER.Where(o => o.HrEmail == LoginName);
                    int hrid = hr_id.FirstOrDefault().HrId;
                    var users = db.COMPANY_JOB_OPENING.ToList();
                    foreach (COMPANY_JOB_OPENING u in db.COMPANY_JOB_OPENING.Where(o => o.HrId.Equals(hrid)))
                    {
                        UJV = new UsersJob();
                        UJV.CompanyId = u.CompanyId;
                        UJV.DepatmentID = u.DepatmentID;
                        UJV.DepartmentCode = u.DepartmentCode;
                        UJV.DepartmentName = u.DepartmentName;
                        UJV.RequestedBy = u.RequestedBy;
                        UJV.HrEmail = u.HrEmail;
                        UJV.Name_of_Vacancy = u.Name_of_Vacancy;
                        UJV.Number_of_Position = Convert.ToInt32(u.Number_of_Position);
                        UJV.Required_Technical_Skill = u.Required_Technical_Skill;
                        UJV.Special_Technical_Skill = u.Special_Technical_Skill;
                        UJV.Min_Yearof_Experience = u.Min_Yearof_Experience;
                        UJV.Min_Qualification = u.Min_Qualification;
                        UJV.Deadline_Date = Convert.ToDateTime(u.Deadline_Date);
                        UJV.Requested_Date = Convert.ToDateTime(u.Requested_Date);
                        UJV.Approval = Convert.ToBoolean(u.Approval);

                        userjobs.Add(UJV);
                    }
                }
                else
                {
                    UsersJob UJV;
                    var compid = db.COMPANY_REGISTER_REFRENCES.Where(o => o.CompanyEmailRef == LoginName);
                    int companyid =Convert.ToInt32(compid.FirstOrDefault().CompanyId);
                    var users = db.COMPANY_JOB_OPENING.ToList();
                    foreach (COMPANY_JOB_OPENING u in db.COMPANY_JOB_OPENING.Where(o => o.CompanyId.Equals(companyid)))
                    {
                        UJV = new UsersJob();
                        UJV.CompanyId = u.CompanyId;
                        UJV.DepatmentID = u.DepatmentID;
                        UJV.DepartmentCode = u.DepartmentCode;
                        UJV.DepartmentName = u.DepartmentName;
                        UJV.RequestedBy = u.RequestedBy;
                        UJV.HrEmail = u.HrEmail;
                        UJV.Name_of_Vacancy = u.Name_of_Vacancy;
                        UJV.Number_of_Position = Convert.ToInt32(u.Number_of_Position);
                        UJV.Required_Technical_Skill = u.Required_Technical_Skill;
                        UJV.Special_Technical_Skill = u.Special_Technical_Skill;
                        UJV.Min_Yearof_Experience = u.Min_Yearof_Experience;
                        UJV.Min_Qualification = u.Min_Qualification;
                        UJV.Deadline_Date = Convert.ToDateTime(u.Deadline_Date);
                        UJV.Requested_Date = Convert.ToDateTime(u.Requested_Date);
                        UJV.Approval = Convert.ToBoolean(u.Approval);
                        UJV.Approval_admin = Convert.ToBoolean(u.Approval);
                        userjobs.Add(UJV);
                    }
                }
                
            }
            return userjobs;
        }
        public listJobView getlistofJob(string LoginName)
        {
            listJobView LJV = new listJobView();
            List<UsersJob> userjobs = getuserjobs(LoginName);

            List<requested_by> requestedby = new List<requested_by>();
            requestedby.Add(new requested_by { Text = "Admin", Value = "Admin" });
            requestedby.Add(new requested_by { Text = "HR", Value = "HR" });
            
            LJV.userjobs = userjobs;
            LJV.hr_requested_by = new hrRequested_by { requestd = requestedby };
            return LJV;
        }

        public void Updatejobsdata(UsersJob UJ)
        {
            using (RecrutmentsysEntities db = new RecrutmentsysEntities())
            {
                //db.Database.Connection.Open();
                //using (var dbcontexttransection = db.Database.Connection.BeginTransaction())
                //{
                //    try
                //    {
                        COMPANY_JOB_OPENING CJO = db.COMPANY_JOB_OPENING.Find(UJ.DepatmentID);
                        CJO.DepartmentName = UJ.DepartmentName;
                        CJO.RequestedBy = UJ.RequestedBy;
                        CJO.Name_of_Vacancy = UJ.Name_of_Vacancy;
                        CJO.Number_of_Position = UJ.Number_of_Position;
                        CJO.Required_Technical_Skill = UJ.Required_Technical_Skill;
                        CJO.Special_Technical_Skill = UJ.Special_Technical_Skill;
                        CJO.Min_Yearof_Experience = UJ.Min_Yearof_Experience;
                        CJO.Min_Qualification = UJ.Min_Qualification;
                        CJO.Deadline_Date = UJ.Deadline_Date;
                        db.SaveChanges();


                    //    dbcontexttransection.Commit();
                    //}
                    //catch
                    //{
                    //    dbcontexttransection.Rollback();
                    //}

                //}
            }
        }

        public void DeleteJob(int depetid)
        {
            using (RecrutmentsysEntities db = new RecrutmentsysEntities())
            {
                var CJ = db.COMPANY_JOB_OPENING.Where(o => o.DepatmentID == depetid);
                
                    if(CJ.Any())
                    {
                        db.COMPANY_JOB_OPENING.Remove(CJ.FirstOrDefault());
                        db.SaveChanges();
                    }
                
                var CJR = db.COMPANY_JOB_OPENING_REFRENCES.Where(o => o.Job_refrenceId == depetid);

                if(CJR.Any())
                {
                    db.COMPANY_JOB_OPENING_REFRENCES.Remove(CJR.FirstOrDefault());
                    db.SaveChanges();
                }
            }

        }
        //@@@@@@@********************* approval jobs
        public void approvaljob(UsersJob UJ)
        {
            using (RecrutmentsysEntities db = new RecrutmentsysEntities())
            {
                COMPANY_JOB_OPENING CJO = db.COMPANY_JOB_OPENING.Find(UJ.DepatmentID);
                CJO.Approval = UJ.Approval_admin;
                db.SaveChanges();
            }
        }
        //************@@@@@@@@@@@

        // here list of depet code here
        public bool IspostionEmailExits(string Loginname)
        {
            using (RecrutmentsysEntities db = new RecrutmentsysEntities())
            {
                return db.CANDIDATE_POSITION.Where(o => o.CandidateEmail.Equals(Loginname) && o.ISActive==false).Any();
            }
        }
        public void addcandidateposition(candidatepostion CP,string code)
        {
            using(RecrutmentsysEntities db = new RecrutmentsysEntities())
            {
                COMPANY_JOB_OPENING data=db.COMPANY_JOB_OPENING.Where(o=>o.DepartmentCode==code).FirstOrDefault();
                if(data != null)
                {
                    CANDIDATE_POSITION CANP = new CANDIDATE_POSITION();
                    CANP.CompanyId=data.CompanyId;
                    CANP.DepartmentCode=data.DepartmentCode;
                    CANP.DepartmentName=data.DepartmentName;
                    CANP.HrId=data.HrId;
                    CANP.CandidateEmail=CP.email;
                    CANP.Candidate_Yof_exp=CP.year_of_experience;
                    CANP.ISActive = false;
                    db.CANDIDATE_POSITION .Add(CANP);
                    db.SaveChanges();
                }
            }
        }
        //Add candidate feedback for position
        public void addcandidatefeedbackposition(candidatefeedback CF)
        {
            using (RecrutmentsysEntities db = new RecrutmentsysEntities())
            {
                CANDIDATE_POSITION cp = db.CANDIDATE_POSITION.Where(o => o.CandidateCode == CF.candidatecode).FirstOrDefault();
                CANDIDATE_FEEDBACK_POSITION cfp = new CANDIDATE_FEEDBACK_POSITION();
                cfp.CandidateCode = cp.CandidateCode;
                cfp.CandidateEmail = cp.CandidateEmail;
                cfp.Candidate_Yof_exp = cp.Candidate_Yof_exp;
                cfp.CompanyId = cp.CompanyId;
                cfp.DepartmentCode = cp.DepartmentCode;

                COMPANY_JOB_OPENING cjo = db.COMPANY_JOB_OPENING.Where(o => o.DepartmentCode == cp.DepartmentCode).FirstOrDefault();
                cfp.DepartmentName = cjo.DepartmentName;
                cfp.RequestedBy = cjo.RequestedBy;
                cfp.HrId = cjo.HrId;
                cfp.HrEmail = cjo.HrEmail;
                cfp.Name_of_Vacancy = cjo.Name_of_Vacancy;
                cfp.Number_of_Position = cjo.Number_of_Position;
                cfp.Required_Technical_Skill = cjo.Required_Technical_Skill;
                cfp.Special_Technical_Skill = cjo.Special_Technical_Skill;
                cfp.Min_Yearof_Experience = cjo.Min_Yearof_Experience;
                cfp.Min_Qualification = cjo.Min_Qualification;
                cfp.Deadline_Date = cjo.Deadline_Date;
                cfp.Requested_Date = cjo.Requested_Date;
                cfp.RoundingPercentage = CF.rounding_percent;
                cfp.OfferLatter = "No";
                cfp.Feedbackprocess = false;

                db.CANDIDATE_FEEDBACK_POSITION.Add(cfp);
                db.SaveChanges();

                Round_Detail_feedback rdf = new Round_Detail_feedback();
                rdf.CompanyId = cfp.CompanyId;
                rdf.CandidateCode = cfp.CandidateCode;
                rdf.DepartmentCode = cfp.DepartmentCode;
                rdf.HrId = cfp.HrId;
                rdf.RoundOne = CF.round1;
                rdf.RoundOne_per = CF.round1P;
                rdf.RoundTwo = CF.round2;
                rdf.RoundTwo_per = CF.round2P;
                rdf.RoundThree = CF.round3;
                rdf.RoundThree_per = CF.round3P;
                rdf.totalround_per =CF.rounding_percent;

                db.Round_Detail_feedback.Add(rdf);
                db.SaveChanges();

                CANDIDATE_POSITION canp = db.CANDIDATE_POSITION.Find(cp.CandidateId);
                canp.ISActive = false;
                db.SaveChanges();

            }
        }
        //Here User Candiaate isactive process
        public ListofIsactive Isactiveornot(string Loginname)
        {
            ListofIsactive LIA = new ListofIsactive();
            List<candidatepostion> cndiposi = getuserisactive(Loginname);
            LIA.candidateposi= cndiposi;
            return LIA;
        }

        public List<candidatepostion> getuserisactive(string Loginname)
        {
            List<candidatepostion> candiposi = new List<candidatepostion>();
            using(RecrutmentsysEntities db= new RecrutmentsysEntities())
            {
                var id = db.COMPANY_HR_REGISTER.Where(o => o.HrEmail == Loginname);
                int hrid = id.FirstOrDefault().HrId;
                candidatepostion can;
                var users = db.CANDIDATE_POSITION.ToList();
                foreach(CANDIDATE_POSITION u in db.CANDIDATE_POSITION.Where(o=>o.HrId==hrid))
                {
                    can= new candidatepostion();
                    can.canid = u.CandidateId;
                    can.candidatecode=u.CandidateCode;
                    can.departmentcode=u.DepartmentCode;
                    can.Departmentname=u.DepartmentName;
                    can.email=u.CandidateEmail;
                    can.candi_yer_of_exper=Convert.ToInt32(u.Candidate_Yof_exp);
                    can.Isactive=Convert.ToBoolean(u.ISActive);
                    candiposi.Add(can);

                }

            }
            return candiposi;
        }

        public void approvalisactiveuser(candidatepostion can)
        {
            using(RecrutmentsysEntities db = new RecrutmentsysEntities())
            {
                CANDIDATE_POSITION cp = db.CANDIDATE_POSITION.Find(can.canid);
                cp.ISActive = can.Isactive;
                db.SaveChanges();
            }
        }

        public Listofoffer getlistoffer(string Loginame)
        {
            Listofoffer LO = new Listofoffer();
            List<candidatefeedback> lisoffer =getoffer(Loginame);
            LO.listoffer = lisoffer;
            return LO;
        }

        public List<candidatefeedback> getoffer(string Loginname)
        {
            List<candidatefeedback> offerforcandi = new List<candidatefeedback>();
            using(RecrutmentsysEntities db = new RecrutmentsysEntities())
            {
                var id = db.COMPANY_REGISTER_REFRENCES.Where(o => o.CompanyEmailRef == Loginname);
                int compid = Convert.ToInt32(id.FirstOrDefault().CompanyId);
                var listusers = (from cjo in db.CANDIDATE_FEEDBACK_POSITION
                                 join rdf in db.Round_Detail_feedback on cjo.CandidateCode equals rdf.CandidateCode
                                 where cjo.CompanyId == compid
                                 select new
                                 {
                                     cjo.CandidateCode,
                                     cjo.DepartmentCode,
                                     cjo.CandidateEmail,
                                     cjo.Candidate_Yof_exp,
                                     cjo.DepartmentName,
                                     cjo.Name_of_Vacancy,
                                     cjo.Number_of_Position,
                                     cjo.Required_Technical_Skill,
                                     cjo.Special_Technical_Skill,
                                     cjo.Min_Qualification,
                                     cjo.Min_Yearof_Experience,
                                     cjo.Deadline_Date,
                                     rdf.RoundOne,
                                     rdf.RoundOne_per,
                                     rdf.RoundTwo,
                                     rdf.RoundTwo_per,
                                     rdf.RoundThree,
                                     rdf.RoundThree_per,
                                     rdf.totalround_per
                                 }).ToList();
                if (listusers != null)
                {

                    candidatefeedback cf;
                    foreach (var i in listusers)
                    {
                        cf = new candidatefeedback();
                        cf.candidatecode = i.CandidateCode;
                        cf.candidateemail = i.CandidateEmail;
                        cf.yerofexperience = Convert.ToString(i.Candidate_Yof_exp);
                        cf.Depetcode = i.DepartmentCode;
                        cf.Departmentname = i.DepartmentName;
                        cf.nameofvacancy = i.Name_of_Vacancy;
                        cf.numberofpostion = i.Number_of_Position;
                        cf.reqtechnicalskill = i.Required_Technical_Skill;
                        cf.specialskill = i.Special_Technical_Skill;
                        cf.minyerofexp = Convert.ToString(i.Min_Yearof_Experience);
                        cf.minqualifi = i.Min_Qualification;
                        cf.deadlinedate = Convert.ToString(i.Deadline_Date);
                        cf.round1 = i.RoundOne;
                        cf.round1P = Convert.ToInt32(i.RoundOne_per);
                        cf.round2 = i.RoundTwo;
                        cf.round2P = Convert.ToInt32(i.RoundTwo_per);
                        cf.round3 = i.RoundThree;
                        cf.round3P = Convert.ToInt32(i.RoundThree_per);
                        cf.rounding_percent = Convert.ToInt32(i.totalround_per);
                        offerforcandi.Add(cf);
                    }

                }
            }
            return offerforcandi;
        }

        public void Fogot_to_new(forgetpassword FP,string mail,string token)
        {
            using(RecrutmentsysEntities db = new RecrutmentsysEntities())
            {
                
                COMPANY_REGISTER_REFRENCES crr = db.COMPANY_REGISTER_REFRENCES.Where(o => o.CompanyEmailRef.Equals(mail)).FirstOrDefault();
                crr.CompanyPassword = FP.newpassword;
                db.SaveChanges();
                
            }
        }
    }
}