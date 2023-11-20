

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace FinalProject.Pages
{
    public class ReadMailModel : PageModel
    {
        public string EmailSender { get; set; }
        public string EmailSubject { get; set; }
        public string EmailMessage { get; set; }

        public void OnGet(int emailid)
        {
            string connectionString = "Server=tcp:cs436final.database.windows.net,1433;Initial Catalog=FinalProject;Persist Security Info=False;User ID=final_admin;Password=Cs436227F;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT emailsender, emailsubject, emailmessage FROM emails WHERE emailid = @emailid";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@emailid", emailid);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            EmailSender = reader.GetString(0);
                            EmailSubject = reader.GetString(1);
                            EmailMessage = reader.GetString(2);
                        }
                    }
                }
            }
        }
    }
}
