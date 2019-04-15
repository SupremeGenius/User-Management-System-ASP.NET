
using BSEF15M025.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace BSEF15M025.Controllers
{
    public class UserController : Controller
    {

        
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult NewUser(String login)
        {
            BSEF15M025.Models.BAL b = new BSEF15M025.Models.BAL();
            return View(b.getUserByLogin(login));
        
        }
        public ActionResult NewUserPage()
        {
            BSEF15M025.Models.UserDTO u =new  BSEF15M025.Models.UserDTO();           
            return View("NewUser",u);

        }
        [HttpPost]
        public ActionResult CreateUser(BSEF15M025.Models.UserDTO user)
        {
                
                var g = Request["gender"];
                var r = Request["dob"].GetType();
                user.dob = DateTime.Parse(Request["dob"]);

                if(Request["hockey"]=="on")
                {
                    user.hockey = 1;
                }
                if (Request["chess"]=="on")
                {
                    user.chess = 1;
                }
                if (Request["cricket"]=="on")
                {
                    user.cricket = 1;
                }
                var file = Request.Files["img"];
                var name = "";
                if(file!=null)
                {
                    if (file.FileName != "")
                    {
                        var ext = System.IO.Path.GetExtension(file.FileName);
                        name=Guid.NewGuid().ToString()+ext;
                        var rootPath = Server.MapPath("~/images");
                        var fileSavePath = System.IO.Path.Combine(rootPath, name);
                        file.SaveAs(fileSavePath);
                        user.img = name;
                    }
                }
                if (g=="Female")
                    user.gender = 'F';
                else if (g == "Male")
                    user.gender = 'M';

                ViewBag.dto = user;
                BSEF15M025.Models.BAL b=new BSEF15M025.Models.BAL();
                if (!b.isEmptyUser(user))
                {
                    Response.Write("<script>alert('please fill all the fields!')</script>");
                    return View("NewUser",user);
                }
                BSEF15M025.Models.DAL dal = new BSEF15M025.Models.DAL();
                var edit = false;
                if (Session["edit"] != null)
                {
                    edit = (bool)Session["edit"];
                }
                var editAdmin=false;
                if (Session["editAdmin"] != null)
                {
                    editAdmin = (bool)Session["editAdmin"];
                }
                if (edit==false && editAdmin!=true)
                {
                    if (b.isUserDuplicate(user.login, user.nic, user.email))
                    {
                        Response.Write("<script>alert('user already exist!')</script>");
                        return View("NewUser", user);
                    }

                    else if (dal.saveUser(user))
                    {
                        Response.Write("<script>alert('user is saved!')</script>");
                        return View("Home",user);
                    }
                }
                if (edit == true)
                {
                    if (dal.UpdateUser(user)) {
                        Response.Write("<script>alert('profile updated!')</script>");
                        
                        
                        return View("Home",user);
                    }
                }
                if (editAdmin == true)
                {
                    if (dal.UpdateUser(user))
                    {
                        Response.Write("<script>alert('profile updated!')</script>");
                       
                        return View("AdminHome");
                    }
                }
                
                return View("NewUser",user);
            

        }
     
        public ActionResult ExistingUser()
        {
            return View();
        }
        public ActionResult Admin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string login, string pswd)
        {
            BSEF15M025.Models.BAL b = new BSEF15M025.Models.BAL();
            if (!b.isEmptyLogin(login,pswd))
            {
                Response.Write("<script>alert('please fill all the fields!')</script>");
                return View("ExistingUser");
            }

            BSEF15M025.Models.UserDTO u = b.getUserByLogin(login);
            if (b.isUserExist(login, pswd))
            {
                Session["user"] = u;
                return View("Home",u);
            }
            else
                return View("ExistingUser");
        }
        [HttpPost]
       public ActionResult ForgetPassword(String email)
        {
            if (email == "")
            {
                Response.Write("<script>alert('please the field!')</script>");
                return View("ExistingUser");
            }
            BSEF15M025.Models.BAL b = new BSEF15M025.Models.BAL();
            string code=b.sendEmail(email);
            if(code!=null)
            {
                Session["code"] = code;
                Session["updateEmail"] = email;
                return View("ResetCode");
            }
            else
            {
                Response.Write("<script>alert('email does not exist!')</script>");
                return View("ExistingUser");
            }
        }
        [HttpPost]
        public ActionResult UpdatePassword(String code)
        {
            var c = Session["code"];
            if (c.Equals(code))
            {               
                return View("updatePassword");
            }
            else
            {
                Response.Write("<script>alert('code does not match!')</script>");
                return View("ResetCode");
            }
        }
        public ActionResult Update(String pswd)
        {
            BSEF15M025.Models.BAL bal = new BSEF15M025.Models.BAL();
            var e =(String) Session["updateEmail"];
            BSEF15M025.Models.UserDTO u = bal.getUserByEmail(e);
            u.pswd = pswd;
            
            if (bal.UpdateUser(u))
            {
                Session["user"] = u;
                return View("Home",u);
            }
            else
            {
                Response.Write("<script>alert('password is not reset!')</script>");
                return View("UpdatePassword");
            }
        }
        
       
        public ActionResult AdminHome(BSEF15M025.Models.AdminDTO admin)
        {
            BSEF15M025.Models.BAL bal = new BSEF15M025.Models.BAL();
            if (!bal.isEmptyLogin(admin.login,admin.pswd))
            {
                Response.Write("<script>alert('please fill all the fields!')</script>");
                return View("Admin");
            }
            if (bal.isAdminExist(admin))
            {
                return View("AdminHome");

            }
            else
            {
                Response.Write("<script>alert('admin does not exist!')</script>");
                return View("Admin");
            }
        }
      public ActionResult Edit(int UserId)
        {
            BSEF15M025.Models.UserDTO u = new BSEF15M025.Models.UserDTO();
            BSEF15M025.Models.BAL b = new BSEF15M025.Models.BAL();
            u = b.getUserById(UserId);
            Session["editAdmin"] = true;
            return View("NewUser",u);
        }
	}
}