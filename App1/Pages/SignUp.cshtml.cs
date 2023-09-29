using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace App1.Pages
{

    public class SignUpModel : PageModel
    {
        private readonly string connectionString;

        public SignUpModel(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public UserInfo userInfo = new UserInfo();

        public void OnGet()
        {
        }
        public void OnPost()
        {
            userInfo.FirstName = Request.Form["FirstName"];
            userInfo.LastName = Request.Form["LastName"];
            userInfo.Email = Request.Form["Email"];
            userInfo.Password = Request.Form["Password"];

            try
            {
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userInfo.Password);
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    String sql = "INSERT INTO Users" +
                        "(Firstname, Lastname, Email, Password) VALUES " +
                        "(@Firstname, @Lastname, @Email, @Password)";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", userInfo.FirstName);
                        command.Parameters.AddWithValue("@LastName", userInfo.LastName);
                        command.Parameters.AddWithValue("@Email", userInfo.Email);
                        command.Parameters.AddWithValue("@Password", hashedPassword);
                        command.ExecuteNonQuery();
                    }
                }   
                    
            }
            catch(Exception ex)
            {
                Console.WriteLine( ex.ToString());
            }
            userInfo.FirstName = "";
            userInfo.LastName = "";
            userInfo.Email = "";
            userInfo.Password = "";
            //Response.Redirect("/Login");
        }
     

    }
   
}
