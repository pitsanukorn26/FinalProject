using FinalProject.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Identity;

namespace FinalProject.Pages
{
    public class IndexModel : PageModel
    {
        public List<EmailInfo> listEmails = new List<EmailInfo>();
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            try
            {
                string connectionString = "Server=tcp:cs436final.database.windows.net,1433;Initial Catalog=FinalProject;Persist Security Info=False;User ID=final_admin;Password=Cs436227F;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string username = User.Identity.Name ?? "";

                    string sql = "SELECT * FROM emails WHERE emailreceiver=@username";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                EmailInfo emailInfo = new EmailInfo
                                {
                                    EmailID = reader.GetInt32(0), // Assuming EmailID is an int in your database
                                    EmailSubject = reader.GetString(1),
                                    EmailMessage = reader.GetString(2),
                                    EmailDate = reader.GetDateTime(3).ToString(),
                                    EmailIsRead = reader.GetString(4),
                                    EmailSender = reader.GetString(5),
                                    EmailReceiver = reader.GetString(6)
                                };

                                listEmails.Add(emailInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching emails.");
            }
        }

        public IActionResult OnPostDelete(int emailId)
        {
            try
            {
                string connectionString = "Server = tcp:cs436final.database.windows.net,1433; Initial Catalog = FinalProject; Persist Security Info = False; User ID = final_admin; Password = Cs436227F; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30; ";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "DELETE FROM emails WHERE emailid=@emailId";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@emailId", emailId);
                        command.ExecuteNonQuery();
                    }
                }

                // Optionally add a success message or redirect
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting email.");
                // Handle the error, maybe display a message
                return Page();
            }
        }

    }

    public class EmailInfo
    {
        public int EmailID; // Assuming EmailID is an int
        public string EmailSubject;
        public string EmailMessage;
        public string EmailDate;
        public string EmailIsRead;
        public string EmailSender;
        public string EmailReceiver;
    }
}
