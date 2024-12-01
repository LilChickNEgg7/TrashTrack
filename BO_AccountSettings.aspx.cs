using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using Npgsql;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Text.Json;
using System.IO;
using static Capstone.PaymentController;


namespace Capstone
{
    public partial class BO_AccountSettings : System.Web.UI.Page
    {
        // Database Connection String
        private readonly string con = "Server=localhost;Port=5432;User Id=postgres;Password=123456;Database=trashtrack";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadProfile();

                string activeTab = hfActiveTab.Value;

                if (string.IsNullOrEmpty(activeTab))
                {
                    hfActiveTab.Value = "#profile-overview";
                }
                BindNotifications();
                GetUnreadNotificationCount();
            }
        }

        public int GetUnreadNotificationCount()
        {
            int unreadCount1 = 0;

            // Replace with your actual PostgreSQL connection string
            using (var connection = new NpgsqlConnection(con))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM notification WHERE notif_read = false AND notif_type IN ('slip', 'payment');";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    unreadCount1 = Convert.ToInt32(command.ExecuteScalar());
                }
            }

            return unreadCount1;
        }


        protected void NotificationTimer_Tick(object sender, EventArgs e)
        {
            // Fetch updated notifications
            var notifications = GetNotificationsFromDb1();

            // Bind to the Repeater
            NotificationRepeater1.DataSource = notifications;
            NotificationRepeater1.DataBind();

            // Update the notification count
            int unreadCount = notifications.Count(n => !n.NotifRead1);
            notificationCount1.InnerText = unreadCount.ToString();
            notificationCount1.Style["display"] = unreadCount > 0 ? "block" : "none";

            // Update the header count
            notificationHeader1.InnerText = unreadCount.ToString();

        }


        private void BindNotifications()
        {
            var notifications1 = GetNotificationsFromDb1();  // This gets a List<Notification>
            NotificationRepeater1.DataSource = notifications1;
            NotificationRepeater1.DataBind();
            // Update notification count (if applicable)
            // Optionally, update the notification count and header
            notificationCount1.InnerText = notifications1.Count.ToString();
            notificationCount1.Visible = notifications1.Count > 0;  // Hide if there are no notifications
            notificationHeader1.InnerText = notifications1.Count.ToString() + " new notifications";
        }
        private List<Notification1> GetNotificationsFromDb1()
        {
            string query = "SELECT notif_id, notif_message, notif_created_at, notif_read, notif_type, cus_id, notif_status " +
                           "FROM notification WHERE notif_status != 'Deleted' AND notif_type IN ('slip', 'payment') " +
                           "ORDER BY notif_created_at DESC;";
            var notifications1 = new List<Notification1>();

            using (var connection = new NpgsqlConnection(con))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        notifications1.Add(new Notification1
                        {
                            NotifId1 = reader.GetInt32(0),
                            NotifMessage1 = reader.GetString(1),
                            NotifCreatedAt1 = reader.GetDateTime(2),
                            NotifRead1 = reader.GetBoolean(3),
                            NotifType1 = reader.GetString(4),
                            CusId1 = reader.GetInt32(5),
                            NotifStatus1 = reader.GetString(6)
                        });
                    }
                }
            }

            return notifications1;
        }

        protected void NotificationBell_Click(object sender, EventArgs e)
        {
            // Call the method to retrieve notifications (replace with your actual logic)
            BindNotifications();

            ScriptManager.RegisterStartupScript(this, GetType(), "OpenDropdown",
                   "$('#LinkButton7').dropdown('show');", true);
            //Response.Redirect($"SAM_AccountManCustomers.aspx");

            UpdatePanelNotifications1.Update();
            Response.Redirect($"BO_Billing.aspx");
            //this.ModalPopupExtender12.Show();

        }

        protected void Notification_Click(object sender, EventArgs e)
        {
            LinkButton btn = sender as LinkButton;
            if (btn != null)
            {
                int notifId = Convert.ToInt32(btn.CommandArgument);
                MarkNotificationAsRead(notifId);
                Response.Redirect($"BO_Billing.aspx");

                // Rebind notifications to reflect the change
                BindNotifications();
            }
        }


        protected void ViewAllNotifications_Click(object sender, EventArgs e)
        {
            string query = "UPDATE notification SET notif_read = true WHERE notif_type IN ('slip', 'payment') AND notif_read = false;";
            using (var connection = new NpgsqlConnection(con))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    BindNotifications();
                }
            }
        }
        private void MarkNotificationAsRead(int notifId)
        {
            string query = "UPDATE notification SET notif_read = true WHERE notif_id = @notifId AND notif_type IN ('slip', 'payment');";
            using (var connection = new NpgsqlConnection(con))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@notifId", notifId);
                    command.ExecuteNonQuery();
                    BindNotifications();
                    GetUnreadNotificationCount();
                }
            }
        }

        //Helper function to get the corresponding icon based on the status
        protected string GetNotificationIcon(string status)
        {
            switch (status)
            {
                case "Pending":
                    return "bi bi-exclamation-circle text-warning";
                case "Declined":
                    return "bi bi-x-circle text-danger";
                case "Approved":
                    return "bi bi-check-circle text-success";
                default:
                    return "bi bi-info-circle text-primary";
            }
        }



        protected void DeleteAllNotifications_Click(object sender, EventArgs e)
        {



            string query = "UPDATE notification SET notif_status = 'Deleted', notif_read = true WHERE notif_type IN ('slip', 'payment');";
            using (var connection = new NpgsqlConnection(con))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    BindNotifications();
                    GetUnreadNotificationCount();

                }
            }
            //BindNotifications();
            //DeleteAllNotificationsFromDb();
            //GetUnreadNotificationCount();

        }


        protected void DeleteNotification_Click(object sender, EventArgs e)
        {
            // Get the ID of the notification to be deleted from the CommandArgument
            LinkButton btnDelete = (LinkButton)sender;
            string notifId = btnDelete.CommandArgument;

            using (var conn = new NpgsqlConnection(con)) // Replace 'con' with your connection string variable
            {
                conn.Open();

                // Update the notification status to 'Deleted' and notif_read to true
                string updateQuery = @"
            UPDATE notification
            SET notif_status = 'Deleted',
                notif_read = true
            WHERE notif_id = @notifId;
        ";

                using (var cmd = new NpgsqlCommand(updateQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@notifId", int.Parse(notifId));

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        // Optionally show a success message
                        ScriptManager.RegisterStartupScript(this, GetType(), "updateSuccess",
                            "Swal.fire({ icon: 'success', title: 'Notification Deleted', text: 'The notification has been successfully deleted.', confirmButtonColor: '#28a745' });", true);
                        // Refresh the notifications list
                        BindNotifications();
                    }
                    else
                    {
                        // Optionally show an error message
                        ScriptManager.RegisterStartupScript(this, GetType(), "updateError",
                            "Swal.fire({ icon: 'error', title: 'Error', text: 'Unable to delete the notification.', confirmButtonColor: '#dc3545' });", true);
                    }
                }
            }
            GetUnreadNotificationCount();
            // Refresh the notifications list
            BindNotifications();
        }


        //public class Notification1
        //{
        //    public int NotifId1 { get; set; }
        //    public string NotifMessage1 { get; set; }
        //    public DateTime NotifCreatedAt1 { get; set; }
        //    public bool NotifRead1 { get; set; }
        //    public string NotifType1 { get; set; }
        //    public int CusId1 { get; set; }
        //    public string NotifStatus1 { get; set; }
        //    public string NotifIcon1 { get; set; }  // Ensure this property is included
        //}

        private void LoadProfile()
        {
            try
            {
                // Check if the session is active
                if (Session["bo_id"] == null)
                {
                    Response.Redirect("LoginPage.aspx");
                    return;
                }

                int adminId = (int)Session["bo_id"];  // Retrieve admin ID from session
                string roleName = (string)Session["bo_rolename"];
                role_name.Text = roleName;

                // Variables to hold user details
                byte[] imageData = null;
                string originalFirstname = null;
                string originalMi = null;
                string originalLastname = null;
                string originalAddress = null;
                string originalPhone = null;
                string originalEmail = null;

                // Define the PostgreSQL connection
                using (var db = new NpgsqlConnection(con))
                {
                    db.Open();

                    // PostgreSQL query to get employee details including profile image
                    string query = "SELECT emp_fname, emp_mname, emp_lname, emp_contact, emp_email, emp_profile, emp_address FROM employee WHERE emp_id = @id";
                    using (var cmd = new NpgsqlCommand(query, db))
                    {
                        cmd.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Integer, adminId);

                        // Execute the query and retrieve employee details
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                originalFirstname = reader["emp_fname"].ToString();
                                originalMi = reader["emp_mname"].ToString();
                                originalLastname = reader["emp_lname"].ToString();
                                originalAddress = reader["emp_address"].ToString();
                                originalPhone = reader["emp_contact"].ToString();
                                originalEmail = reader["emp_email"].ToString();
                                imageData = reader["emp_profile"] as byte[];  // Image data might be null
                            }
                            else
                            {
                                Response.Write("<script>alert('No data found for the specified ID.')</script>");
                                return;
                            }
                        }
                    }
                }

                // Update textboxes with the retrieved data
                firstname.Text = originalFirstname;
                m_initial.Text = originalMi;
                lastname.Text = originalLastname;
                address.Text = originalAddress;
                phone.Text = originalPhone;
                email.Text = originalEmail;

                // Update labels and profile image
                Label6.Text = originalAddress;
                Label7.Text = originalPhone;
                Label8.Text = originalEmail;
                Label10.Text = $"{originalFirstname} {originalMi}. {originalLastname}";

                // Set profile image or default image if none exists
                if (image_edit != null)
                {
                    if (imageData != null && imageData.Length > 0)
                    {
                        string base64String = Convert.ToBase64String(imageData);
                        image_edit.ImageUrl = "data:image/jpeg;base64," + base64String;  // Set image as base64 string
                    }
                    else
                    {
                        image_edit.ImageUrl = "~/Pictures/blank_prof.png";  // Default image if no profile picture is found
                    }
                }
                else
                {
                    Response.Write("<script>alert('Profile image control is not found');</script>");
                }



                //Check if the profile_image control exists and is not null
                if (profile_image != null)
                {
                    if (imageData != null && imageData.Length > 0)
                    {
                        string base64String = Convert.ToBase64String(imageData);
                        profile_image.ImageUrl = "data:image/jpeg;base64," + base64String;  // Set image as base64 string
                        prof.Src = "data:image/jpeg;base64," + base64String;  // Set image as base64 string
                    }
                    else
                    {
                        profile_image.ImageUrl = "~/Pictures/blank_prof.png";  // Default image if no profile picture found
                    }
                }
                else
                {
                    Response.Write("<script>alert('Profile image control is not found');</script>");
                }


                // If originalFirstname and originalLastname are not null, set the label text
                if (!string.IsNullOrEmpty(originalFirstname) && !string.IsNullOrEmpty(originalLastname))
                {
                    Label2.Text = $"{originalFirstname[0]}. {originalLastname}";
                    Label3.Text = $"{roleName}";
                    profile_name.Text = originalFirstname;
                    Label10.Text = $"{originalFirstname} {originalMi}. {originalLastname}";
                }
                else
                {
                    Label2.Text = "Welcome!";
                }
            }
            catch (Exception ex)
            {
                // Handle exception and fallback
                Response.Write("<script>alert('Error loading profile: " + ex.Message + "');</script>");
                image_edit.ImageUrl = "~/Pictures/blank_prof.png";  // Fallback image in case of an error
            }
        }


        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert the hashed bytes to a hexadecimal string
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < hashedBytes.Length; i++)
                {
                    stringBuilder.Append(hashedBytes[i].ToString("x2"));
                }

                return stringBuilder.ToString();
            }
        }

        protected void changepassword_Click(object sender, EventArgs e)
        {
            // Get admin ID from session
            int boId = (int)Session["bo_id"];

            string currentPass = currentpassword.Text.Trim();
            string newPass = changetxt.Text.Trim();

            // Hash the entered current password
            string hashedCurrentPassword = HashPassword(currentPass);

            using (var db = new NpgsqlConnection(con))
            {
                db.Open();

                try
                {
                    // Query to get the current password for the user from the database
                    string selectQuery = "SELECT emp_password FROM employee WHERE emp_id = @adminId";
                    using (var cmdSelect = new NpgsqlCommand(selectQuery, db))
                    {
                        cmdSelect.Parameters.AddWithValue("@adminId", boId);

                        // Fetch the stored password (it is already hashed in the database)
                        var storedPassword = cmdSelect.ExecuteScalar()?.ToString();

                        // Check if the hashed current password entered by the user matches the stored hashed password
                        if (storedPassword != null && storedPassword == hashedCurrentPassword)
                        {
                            // Ensure the new password is not the same as the old password
                            if (currentPass == newPass)
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "showPasswordError",
                                    "Swal.fire({ icon: 'error', title: 'Error', text: 'New password cannot be the same as the current password.', confirmButtonColor: '#dc3545' });", true);
                                return;
                            }

                            // Validate the new password pattern (minimum requirements)
                            Regex passwordPattern = new Regex(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,}$");
                            if (!passwordPattern.IsMatch(newPass))
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "showPasswordPatternError",
                                    "Swal.fire({ icon: 'error', title: 'Invalid Password', text: 'Password must contain at least one uppercase letter, one lowercase letter, and one digit, and must be at least 6 characters long.', confirmButtonColor: '#dc3545' });", true);
                                return;
                            }

                            // Hash the new password
                            string hashedNewPassword = HashPassword(newPass);

                            // Query to update the password in the database
                            string updateQuery = "UPDATE employee SET emp_password = @newPassword WHERE emp_id = @adminId";
                            using (var cmdUpdate = new NpgsqlCommand(updateQuery, db))
                            {
                                cmdUpdate.Parameters.AddWithValue("@newPassword", hashedNewPassword);
                                cmdUpdate.Parameters.AddWithValue("@adminId", boId);

                                int rowsAffected = cmdUpdate.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    // Password updated successfully
                                    ScriptManager.RegisterStartupScript(this, GetType(), "showPasswordSuccess",
                                        "Swal.fire({ icon: 'success', title: 'Success', text: 'Password changed successfully!', confirmButtonColor: '#28a745' });", true);
                                }
                                else
                                {
                                    // Error updating the password
                                    ScriptManager.RegisterStartupScript(this, GetType(), "showUpdateError",
                                        "Swal.fire({ icon: 'error', title: 'Error', text: 'Unable to update password.', confirmButtonColor: '#dc3545' });", true);
                                }
                            }
                        }
                        else
                        {
                            // The current password entered does not match the stored password
                            ScriptManager.RegisterStartupScript(this, GetType(), "showIncorrectPasswordError",
                                "Swal.fire({ icon: 'error', title: 'Incorrect Password', text: 'The current password you entered is incorrect.', confirmButtonColor: '#dc3545' });", true);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle exception (logging or showing a user-friendly message)
                    ScriptManager.RegisterStartupScript(this, GetType(), "showError",
                        $"Swal.fire({{ icon: 'error', title: 'Error', text: 'An error occurred: {ex.Message}', confirmButtonColor: '#dc3545' }});", true);
                }
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            int empId = (int)Session["bo_id"];
            string firstName = firstname.Text;
            string middleInitial = m_initial.Text;
            string lastName = lastname.Text;
            string addr = address.Text;
            string contact = phone.Text;
            string email = this.email.Text;

            if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName) &&
                !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(contact) &&
                !string.IsNullOrEmpty(addr))
            {
                using (var db = new NpgsqlConnection(con))
                {
                    db.Open();

                    // Check if the email already exists for a different employee
                    string emailCheckQuery = "SELECT COUNT(*) FROM employee WHERE emp_email = @Email AND emp_id != @emp_id";
                    using (var emailCheckCommand = new NpgsqlCommand(emailCheckQuery, db))
                    {
                        emailCheckCommand.Parameters.AddWithValue("@Email", email);
                        emailCheckCommand.Parameters.AddWithValue("@emp_id", empId);
                        int emailExists = Convert.ToInt32(emailCheckCommand.ExecuteScalar());

                        if (emailExists > 0)
                        {

                            // Show alert if email already exists
                            ScriptManager.RegisterStartupScript(this, GetType(), "showEmailAlert",
                                "Swal.fire({ icon: 'error', title: 'Email Exists', text: 'The email address is already associated with another account.', confirmButtonColor: '#dc3545' });", true);
                            hfActiveTab.Value = "#profile-edit";

                            return;
                        }
                    }

                    // Prepare update query
                    string updateQuery = "UPDATE employee SET emp_fname = @firstName, emp_mname = @middleInitial, emp_lname = @lastName, emp_contact = @contact, emp_address = @address, emp_email = @Email";
                    bool hasChanges = true;
                    byte[] profileImage = null;

                    if (hfImageRemoved.Value == "true")
                    {
                        profileImage = File.ReadAllBytes(Server.MapPath("~/Pictures/blank_prof.png"));
                        updateQuery += ", emp_profile = @profileImage";
                        hasChanges = true;
                    }
                    else if (formFile.HasFile)
                    {
                        profileImage = formFile.FileBytes;
                        updateQuery += ", emp_profile = @profileImage";
                        hasChanges = true;
                    }

                    updateQuery += ", emp_updated_at = CURRENT_TIMESTAMP WHERE emp_id = @emp_id";

                    if (hasChanges)
                    {
                        using (var updateCommand = new NpgsqlCommand(updateQuery, db))
                        {
                            updateCommand.Parameters.AddWithValue("@emp_id", empId);
                            updateCommand.Parameters.AddWithValue("@firstName", firstName);
                            updateCommand.Parameters.AddWithValue("@middleInitial", middleInitial);
                            updateCommand.Parameters.AddWithValue("@lastName", lastName);
                            updateCommand.Parameters.AddWithValue("@contact", contact);
                            updateCommand.Parameters.AddWithValue("@address", addr);
                            updateCommand.Parameters.AddWithValue("@Email", email);

                            if (profileImage != null)
                            {
                                updateCommand.Parameters.Add("@profileImage", NpgsqlTypes.NpgsqlDbType.Bytea).Value = profileImage;
                                LoadProfile();
                            }

                            int affectedRows = updateCommand.ExecuteNonQuery();
                            if (affectedRows >= 1)
                            {
                                //Response.Write("<script>alert('Successfully Changed!')</script>");
                                ScriptManager.RegisterStartupScript(this, GetType(), "showSuccessAlert",
                                    "Swal.fire({ icon: 'success', title: 'Profile Updated', text: 'Your profile was successfully updated!', confirmButtonColor: '#28a745' });", true);
                                hfActiveTab.Value = "#profile-edit";
                                LoadProfile();
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "showUpdateFailedAlert",
                                    "Swal.fire({ icon: 'error', title: 'Update Failed', text: 'Profile update failed. Please try again.', confirmButtonColor: '#dc3545' });", true);
                            }
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNoChangesAlert",
                            "Swal.fire({ icon: 'info', title: 'No Changes', text: 'No changes detected in your profile.', confirmButtonColor: '#17a2b8' });", true);
                    }
                }
                LoadProfile();

            }
            else
            {
                hfActiveTab.Value = "#profile-edit";
                ScriptManager.RegisterStartupScript(this, GetType(), "showIncompleteAlert",
                    "Swal.fire({ icon: 'warning', title: 'Incomplete Fields', text: 'Please fill all required fields!', confirmButtonColor: '#ffc107' });", true);
            }

            hfImageRemoved.Value = "false";


        }
    }
}