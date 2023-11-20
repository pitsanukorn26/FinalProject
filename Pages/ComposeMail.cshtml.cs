using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System;

namespace FinalProject.Pages.Compose_New_Email
{
    public class ComposeMailModel : PageModel
    {
        [BindProperty]
        public string To { get; set; }
        [BindProperty]
        public string Subject { get; set; }
        [BindProperty]
        public string Message { get; set; }

        public string ErrorMessage { get; private set; }
        public string SuccessMessage { get; private set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrWhiteSpace(To) || string.IsNullOrWhiteSpace(Subject) || string.IsNullOrWhiteSpace(Message))
            {
                ErrorMessage = "All fields are required";
                return Page();
            }

            try
            {
                // ?????????????????????????????????????????????????????????????
                string connectionString = "Server=tcp:cs436final.database.windows.net,1433;Initial Catalog=FinalProject;Persist Security Info=False;User ID=final_admin;Password=Cs436227F;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO emails (emailsubject, emailmessage, emaildate, emailisread, emailsender, emailreceiver) VALUES (@subject, @message, @date, @isread, @sender, @reciever)";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@subject", Subject);
                        command.Parameters.AddWithValue("@message", Message);
                        command.Parameters.AddWithValue("@date", DateTime.Now);
                        command.Parameters.AddWithValue("@isread", 0);
                        command.Parameters.AddWithValue("@sender", User.Identity.Name); // Replace with actual sender name logic
                        command.Parameters.AddWithValue("@reciever", To);
                        command.ExecuteNonQuery();
                    }
                }
                SuccessMessage = "Email sent successfully";
                return RedirectToPage("/Success");
            }
            catch (SqlException sqlEx)
            {
                ErrorMessage = "Database error: " + sqlEx.Message;
                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = "An error occurred: " + ex.Message;
                return Page();
            }
        }
    }
}
