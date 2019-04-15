using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BSEF15M025.Models
{
    public class BAL
    {
        public UserDTO getUserByLogin(string login)
        {
            DAL d = new DAL();
            UserDTO u = d.getUser(login);
            return u;
        }
        public UserDTO getUserById(int id)
        {
            DAL d = new DAL();
            UserDTO u = d.getUserById(id);
            return u;
        }
        public UserDTO getUserByEmail(string email)
        {
            DAL d = new DAL();
            BSEF15M025.Models.UserDTO u = d.getUserByEmail(email);
            return u;
        }
        public bool isEmptyUser(UserDTO user)
        {
            if (user.name == null || user.login == null || user.img == null || user.nic == null || user.pswd == null || user.email == null || user.address == null || user.age < 1 || user.nic == null)
            {
                return false;
            }
            else
                return true;
        }
        public bool isEmptyLogin(string login,string pswd)
        {
            if (login == "" || pswd == "")
                return false;
            else
                return true;
        }
        public bool isAdminExist(AdminDTO admin)
        {
            DAL d = new DAL();
            if (d.isAdminExist(admin.login, admin.pswd))
                return true;
            else
                return false;
        }

        public bool isUserExist(string login, string pswd)
        {
            DAL d = new DAL();
            if(d.isExist(login, pswd))
                return true;
            else
                return false;
        }
        public bool isUserDuplicate(string login, string email,string nic)
        {
            DAL d = new DAL();
            if (d.isUserExist(login,nic, email))
                return true;
            else
                return false;
        }
        public bool UpdateUser(UserDTO u)
        {
            DAL d = new DAL();
            if (d.UpdateUser(u))
                return true;
            else
                return false;
                    
        }
        public string sendEmail(string email)
        {
            DAL dal = new DAL();
            Random r = new Random();
            int c = r.Next(100, 1000);
            String code = c.ToString();
            if (dal.sendEmail(email, "Recovery Code", "Your recovery code is " + code))
                return code;
            else
                return null;
        }




    }
}