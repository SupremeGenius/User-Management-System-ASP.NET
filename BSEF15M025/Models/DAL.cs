using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BSEF15M025.Models
{
    public class DAL
    {

        public bool isUserExist(string login, string nic, string email)
        {
            string connString = @"Data Source=.\SQLEXPRESS; Initial Catalog=Assignment4; User Id=sa; Password=12345;";
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string query = "select * from dbo.Users where Login='" + login + "' OR NIC='" + nic + "' OR Email='" + email + "' ";
                SqlCommand command = new SqlCommand(query, conn);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                    return true;
                return false;
            }

        }

        public bool isAdminExist(string login, string pass)
        {
            string connString = @"Data Source=.\SQLEXPRESS; Initial Catalog=Assignment4; User Id=sa; Password=12345;";
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string query = "select * from dbo.Admin where Login='" + login + "' AND Password='" + pass + "'";
                SqlCommand command = new SqlCommand(query, conn);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                    return true;
                return false;
            }

        }
        public bool isExist(string login, string pass)
        {
            string connString = @"Data Source=.\SQLEXPRESS; Initial Catalog=Assignment4; User Id=sa; Password=12345;";
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string query = "select * from dbo.Users where Login='" + login + "' AND Password='" + pass + "'";
                SqlCommand command = new SqlCommand(query, conn);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                    return true;
                return false;
            }

        }
        public bool saveUser(UserDTO u)
        {
            string connstring = @"Data Source=.\SQLEXPRESS; Initial Catalog=Assignment4; User Id=sa; Password=12345";
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                conn.Open();

               var d = DateTime.Now;
               u.createdOn = DateTime.Parse(d.ToString("yyyy-MM-dd'T'HH:mm:ssZ"));
               var s = u.dob;
               u.dob = DateTime.Parse(s.ToString("yyyy-MM-dd'T'HH:mm:ssZ"));
                string query = String.Format(@"insert into dbo.Users Values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}')", u.name, u.login, u.pswd, u.gender, u.address, u.age, u.nic, u.dob, u.cricket, u.hockey, u.chess, u.img, u.createdOn, u.email);
                SqlCommand command = new SqlCommand(query, conn);

                int rec = command.ExecuteNonQuery();
                if (rec > 0)
                    return true;
            }
            return false;
        }


        public UserDTO getUser(string login)
        {
            UserDTO u = new UserDTO();
            string connstring = @"Data Source=.\SQLEXPRESS; Initial Catalog=Assignment4; User Id=sa; Password=12345";
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                conn.Open();
                string query = "select * from dbo.Users where Login='" + login + "'";
                SqlCommand command = new SqlCommand(query, conn);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    u.login = login;
                    u.UserID = reader.GetInt32(reader.GetOrdinal("UserID"));
                    u.name = reader.GetString(reader.GetOrdinal("Name"));
                    u.pswd = reader.GetString(reader.GetOrdinal("Password"));
                    u.gender = Convert.ToChar(reader.GetString(reader.GetOrdinal("Gender")));
                    u.address = reader.GetString(reader.GetOrdinal("Address"));
                    u.age = reader.GetInt32(reader.GetOrdinal("Age"));
                    
                    u.createdOn =reader.GetDateTime(reader.GetOrdinal("CreatedOn"));
                   
                    u.nic = reader.GetString(reader.GetOrdinal("NIC"));
                    u.dob = reader.GetDateTime(reader.GetOrdinal("DOB"));
                   
                    u.email = reader.GetString(reader.GetOrdinal("Email"));
                    u.cricket = Convert.ToInt16(reader.GetValue(9));
                    u.hockey = Convert.ToInt16(reader.GetValue(10));
                    u.chess = Convert.ToInt16(reader.GetValue(11));
                    u.img = reader.GetString(reader.GetOrdinal("ImageName"));
                    
                }
            }
            return u;
        }

        public UserDTO getUserById(int id)
        {
            UserDTO u = new UserDTO();
            string connstring = @"Data Source=.\SQLEXPRESS; Initial Catalog=Assignment4; User Id=sa; Password=12345";
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                conn.Open();
                string query = "select * from dbo.Users where UserId='" + id + "'";
                SqlCommand command = new SqlCommand(query, conn);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    u.login = reader.GetString(reader.GetOrdinal("Login"));
                    u.UserID = id;
                    u.name = reader.GetString(reader.GetOrdinal("Name"));
                    u.pswd = reader.GetString(reader.GetOrdinal("Password"));
                    char g = Convert.ToChar(reader.GetString(reader.GetOrdinal("Gender")));
                    u.gender = g;
                    u.address = reader.GetString(reader.GetOrdinal("Address"));
                    u.age = reader.GetInt32(reader.GetOrdinal("Age"));
                    u.createdOn = reader.GetDateTime(reader.GetOrdinal("CreatedOn"));                   
                    u.nic = reader.GetString(reader.GetOrdinal("NIC"));
                    u.dob = reader.GetDateTime(reader.GetOrdinal("DOB"));
                    u.email = reader.GetString(reader.GetOrdinal("Email"));
                    u.cricket = Convert.ToInt16(reader.GetValue(9));
                    u.hockey = Convert.ToInt16(reader.GetValue(10));
                    u.chess = Convert.ToInt16(reader.GetValue(11));
                    u.img = reader.GetString(reader.GetOrdinal("ImageName"));

                }
            }
            return u;
        }

        public bool UpdateUser(UserDTO u)
        {
            string connstring = @"Data Source=.\SQLEXPRESS; Initial Catalog=Assignment4; User Id=sa; Password=12345";
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                conn.Open();

                string query = String.Format(@"Update dbo.Users set Name='{0}',Password='{1}',Gender='{2}',Address='{3}',Age='{4}',NIC='{5}',DOB='{6}',IsCricket='{7}',Hockey='{8}',Chess='{9}',ImageName='{10}',CreatedOn='{11}',Email='{12}',Login='{13}' where UserID='" + u.UserID + "'", u.name, u.pswd, u.gender, u.address, u.age, u.nic, u.dob, u.cricket, u.hockey, u.chess, u.img, u.createdOn, u.email,u.login);
                SqlCommand command = new SqlCommand(query, conn);

                int rec = command.ExecuteNonQuery();
                if (rec > 0)
                    return true;
            }
            return false;
        }

        public UserDTO getUserByEmail(string email)
        {
            UserDTO u = new UserDTO();
            string connstring = @"Data Source=.\SQLEXPRESS; Initial Catalog=Assignment4; User Id=sa; Password=12345";
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                conn.Open();
                string query = "select * from dbo.Users where Email='" + email + "'";
                SqlCommand command = new SqlCommand(query, conn);
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                u.login = reader.GetString(reader.GetOrdinal("Login"));
                u.UserID = reader.GetInt32(reader.GetOrdinal("UserID"));
                u.name = reader.GetString(reader.GetOrdinal("Name"));
                u.pswd = reader.GetString(reader.GetOrdinal("Password"));
                char g = Convert.ToChar(reader.GetString(reader.GetOrdinal("Gender")));
                u.gender = g;
                u.address = reader.GetString(reader.GetOrdinal("Address"));
                u.age = reader.GetInt32(reader.GetOrdinal("Age"));
                u.nic = reader.GetString(reader.GetOrdinal("NIC"));
                u.dob = reader.GetDateTime(reader.GetOrdinal("DOB"));
                u.email = email;
                u.cricket = Convert.ToInt16(reader.GetValue(9));
                u.hockey = Convert.ToInt16(reader.GetValue(10));
                u.chess = Convert.ToInt16(reader.GetValue(11));
                u.img = reader.GetString(reader.GetOrdinal("ImageName"));
                u.createdOn = reader.GetDateTime(reader.GetOrdinal("CreatedOn"));
            }
            return u;
        }


        public bool sendEmail(String toEmailAddress, String subject, String body)
        {
            try
            {
               
                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();

                MailAddress to = new MailAddress(toEmailAddress);
                mail.To.Add(to);

                MailAddress from = new MailAddress("tehreemkamran93@gmail.com", "Admin");
                mail.From = from;

                mail.Subject = subject;
                mail.Body = body;

                var sc = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new System.Net.NetworkCredential("tehreemkamran93@gmail.com", "090078601"),
                EnableSsl = true
            };

                sc.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
            


        public DataTable GetUsersGrid()
        {
            DataTable dt = new DataTable("Users");

            DataColumn dc = new DataColumn("UserID", typeof(System.Int32));
            dt.Columns.Add(dc);

            dc = new DataColumn("Name", typeof(System.String));
            dt.Columns.Add(dc);

            dc = new DataColumn("Login", typeof(System.String));
            dt.Columns.Add(dc);

            dc = new DataColumn("Address", typeof(System.String));
            dt.Columns.Add(dc);

            dc = new DataColumn("Age", typeof(System.Int16));
            dt.Columns.Add(dc);

            DataRow dr;

            string connString = @"Data Source=.\SQLEXPRESS; Initial Catalog=Assignment4; User Id=sa; Password=12345;";
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string query = "select * from dbo.Users";
                SqlCommand command = new SqlCommand(query, conn);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    dr = dt.NewRow();
                    dr["UserID"] = reader.GetInt32(reader.GetOrdinal("UserID"));
                    dr["Name"] = reader.GetString(reader.GetOrdinal("Name"));
                    dr["Login"] = reader.GetString(reader.GetOrdinal("Login"));
                    dr["Address"] = reader.GetString(reader.GetOrdinal("Address"));
                    dr["Age"] = reader.GetInt32(reader.GetOrdinal("Age"));
                    dt.Rows.Add(dr);
                }

            }

            return dt;
        }
    }
}