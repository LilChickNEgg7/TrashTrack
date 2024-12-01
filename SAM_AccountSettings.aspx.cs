using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Text.RegularExpressions;
using Npgsql;

using System.IO;
using static Capstone.PaymentController;


namespace Capstone
{
    public partial class SAM_AccountSettings : System.Web.UI.Page
    {
        // Database Connection String
        private readonly string con = "Server=localhost;Port=5432;User Id=postgres;Password=123456;Database=trashtrack";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindNotifications();
                GetUnreadNotificationCount();
                //AccountManList();
                //profile_image.DataBind();
                LoadProfile();
                //HiddenActiveTab.Value = "profile-overview"; // Default tab
                // Set the active tab based on the hidden field value during initial load.
                string activeTab = hfActiveTab.Value;
                //hfActiveTab.Value = "#profile-overview";
                // If the hidden field is empty, default to "profile-overview"
                if (string.IsNullOrEmpty(activeTab))
                {
                    hfActiveTab.Value = "#profile-overview";
                }
            }
        }

        public int GetUnreadNotificationCount()
        {
            int unreadCount = 0;

            // Replace with your actual PostgreSQL connection string
            using (var connection = new NpgsqlConnection(con))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM notification WHERE notif_read = false AND notif_type = 'request verification'";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    unreadCount = Convert.ToInt32(command.ExecuteScalar());
                }
            }

            return unreadCount;
        }

        protected void NotificationTimer_Tick(object sender, EventArgs e)
        {
            // Fetch updated notifications
            var notifications = GetNotificationsFromDb();

            // Bind to the Repeater
            NotificationRepeater.DataSource = notifications;
            NotificationRepeater.DataBind();

            // Update the notification count
            int unreadCount = notifications.Count(n => !n.NotifRead);
            notificationCount.InnerText = unreadCount.ToString();
            notificationCount.Style["display"] = unreadCount > 0 ? "block" : "none";

            // Update the header count
            notificationHeader.InnerText = unreadCount.ToString();

        }
        private void BindNotifications()
        {
            var notifications = GetNotificationsFromDb();  // This gets a List<Notification>
            NotificationRepeater.DataSource = notifications;
            NotificationRepeater.DataBind();
            // Update notification count (if applicable)
            // Optionally, update the notification count and header
            notificationCount.InnerText = notifications.Count.ToString();
            notificationCount.Visible = notifications.Count > 0;  // Hide if there are no notifications
            notificationHeader.InnerText = notifications.Count.ToString() + " new notifications";
        }
        private List<Notification> GetNotificationsFromDb()
        {
            string query = "SELECT notif_id, notif_message, notif_created_at, notif_read, notif_type, cus_id, notif_status FROM notification WHERE notif_status != 'Deleted' AND notif_type = 'request verification' ORDER BY notif_created_at DESC;";
            var notifications = new List<Notification>();

            using (var connection = new NpgsqlConnection(con))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        notifications.Add(new Notification
                        {
                            NotifId = reader.GetInt32(0),
                            NotifMessage = reader.GetString(1),
                            NotifCreatedAt = reader.GetDateTime(2),
                            NotifRead = reader.GetBoolean(3),
                            NotifType = reader.GetString(4),
                            CusId = reader.GetInt32(5),
                            NotifStatus = reader.GetString(6)
                        });
                    }
                }
            }

            return notifications;
        }

        protected void NotificationBell_Click(object sender, EventArgs e)
        {
            // Call the method to retrieve notifications (replace with your actual logic)
            BindNotifications();

            ScriptManager.RegisterStartupScript(this, GetType(), "OpenDropdown",
                   "$('#LinkButton3').dropdown('show');", true);
            //Response.Redirect($"SAM_AccountManCustomers.aspx");

            UpdatePanelNotifications.Update();
            Response.Redirect($"SAM_AccountMan.aspx");
            //this.ModalPopupExtender12.Show();

        }

        protected void Notification_Click(object sender, EventArgs e)
        {
            LinkButton btn = sender as LinkButton;
            if (btn != null)
            {
                int notifId = Convert.ToInt32(btn.CommandArgument);
                MarkNotificationAsRead(notifId);
                Response.Redirect($"SAM_AccountManCustomers.aspx");

                // Rebind notifications to reflect the change
                BindNotifications();
            }
        }


        protected void ViewAllNotifications_Click(object sender, EventArgs e)
        {
            string query = "UPDATE notification SET notif_read = true WHERE notif_type = 'request verification';";
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
        protected void DeleteAllNotifications_Click(object sender, EventArgs e)
        {

            string query = "UPDATE notification SET notif_status = 'Deleted', notif_read = true WHERE notif_type = 'request verification' AND notif_status != 'Deleted';";
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
        private void MarkNotificationAsRead(int notifId)
        {
            string query = "UPDATE notification SET notif_read = true WHERE notif_id = @notifId;";
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


        private void DeleteNotificationFromDatabase(string notifId)
        {
            string query = "UPDATE notification SET notif_status = 'Deleted', notif_read = true WHERE notif_id = @notifId AND notif_type = 'request verification';";
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

        private void LoadProfile()
        {
            try
            {
                // Check if the session is active
                if (Session["sam_id"] == null)
                {
                    Response.Redirect("LoginPage.aspx");
                    return;
                }

                int adminId = (int)Session["sam_id"];  // Retrieve admin ID from session
                string roleName = (string)Session["sam_rolename"];
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
            int samId = (int)Session["sam_id"];

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
                        cmdSelect.Parameters.AddWithValue("@adminId", samId);

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
                                cmdUpdate.Parameters.AddWithValue("@adminId", samId);

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


        //protected void changepassword_Click(object sender, EventArgs e)
        //{
        //    // Get admin ID from session
        //    int samId = (int)Session["sam_id"];

        //    string currentPass = currentpassword.Text.Trim();
        //    string newPass = changetxt.Text.Trim();

        //    // Hash the entered current password
        //    string hashedCurrentPassword = HashPassword(currentPass);

        //    using (var db = new NpgsqlConnection(con))
        //    {
        //        db.Open();

        //        try
        //        {
        //            // Query to get the current password for the user
        //            string selectQuery = "SELECT emp_password FROM employee WHERE emp_id = @adminId";
        //            using (var cmdSelect = new NpgsqlCommand(selectQuery, db))
        //            {
        //                cmdSelect.Parameters.AddWithValue("@adminId", samId);

        //                var storedPassword = cmdSelect.ExecuteScalar()?.ToString();

        //                // Check if the current password matches the stored password
        //                if (storedPassword == hashedCurrentPassword)
        //                {
        //                    // Ensure the new password is not the same as the old password
        //                    if (currentPass == newPass)
        //                    {
        //                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('New password cannot be the same as the current password.');", true);
        //                        return;
        //                    }

        //                    // Validate the new password pattern (minimum requirements)
        //                    Regex passwordPattern = new Regex(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,}$");
        //                    if (!passwordPattern.IsMatch(newPass))
        //                    {
        //                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Password must contain at least one uppercase letter, one lowercase letter, and one digit, and must be at least 6 characters long.');", true);
        //                        return;
        //                    }

        //                    // Hash the new password
        //                    string hashedNewPassword = HashPassword(newPass);

        //                    // Query to update the password
        //                    string updateQuery = "UPDATE employee SET emp_password = @newPassword WHERE emp_id = @adminId";
        //                    using (var cmdUpdate = new NpgsqlCommand(updateQuery, db))
        //                    {
        //                        cmdUpdate.Parameters.AddWithValue("@newPassword", hashedNewPassword);
        //                        cmdUpdate.Parameters.AddWithValue("@adminId", samId);

        //                        int rowsAffected = cmdUpdate.ExecuteNonQuery();

        //                        if (rowsAffected > 0)
        //                        {
        //                            // Password updated successfully
        //                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Password changed successfully!');", true);
        //                        }
        //                        else
        //                        {
        //                            // Error updating the password
        //                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error: Unable to update password!');", true);
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    // Current password does not match the stored password
        //                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error: Incorrect current password!');", true);
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            // Handle exception (logging or showing a user-friendly message)
        //            ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('An error occurred: {ex.Message}');", true);
        //        }
        //    }
        //}

        //protected void changepassword_Click(object sender, EventArgs e)
        //{
        //    // Get admin ID from session
        //    int samId = (int)Session["sam_id"];

        //    string currentPass = currentpassword.Text.Trim();
        //    string newPass = changetxt.Text.Trim();

        //    // Hash the entered current password
        //    string hashedCurrentPassword = HashPassword(currentPass);

        //    using (var db = new NpgsqlConnection(con))
        //    {
        //        db.Open();

        //        try
        //        {
        //            // Query to get the current password for the user
        //            string selectQuery = "SELECT emp_password FROM employee WHERE emp_id = @adminId";
        //            using (var cmdSelect = new NpgsqlCommand(selectQuery, db))
        //            {
        //                cmdSelect.Parameters.AddWithValue("@adminId", samId);

        //                var storedPassword = cmdSelect.ExecuteScalar()?.ToString();

        //                // Check if the current password matches the stored password
        //                if (storedPassword == hashedCurrentPassword)
        //                {
        //                    // Ensure the new password is not the same as the old password
        //                    if (currentPass == newPass)
        //                    {
        //                        ScriptManager.RegisterStartupScript(this, GetType(), "showPasswordError",
        //                            "Swal.fire({ icon: 'error', title: 'Error', text: 'New password cannot be the same as the current password.', confirmButtonColor: '#dc3545' });", true);
        //                        return;
        //                    }

        //                    // Validate the new password pattern (minimum requirements)
        //                    Regex passwordPattern = new Regex(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,}$");
        //                    if (!passwordPattern.IsMatch(newPass))
        //                    {
        //                        ScriptManager.RegisterStartupScript(this, GetType(), "showPasswordPatternError",
        //                            "Swal.fire({ icon: 'error', title: 'Invalid Password', text: 'Password must contain at least one uppercase letter, one lowercase letter, and one digit, and must be at least 6 characters long.', confirmButtonColor: '#dc3545' });", true);
        //                        return;
        //                    }

        //                    // Hash the new password
        //                    string hashedNewPassword = HashPassword(newPass);

        //                    // Query to update the password
        //                    string updateQuery = "UPDATE employee SET emp_password = @newPassword WHERE emp_id = @adminId";
        //                    using (var cmdUpdate = new NpgsqlCommand(updateQuery, db))
        //                    {
        //                        cmdUpdate.Parameters.AddWithValue("@newPassword", hashedNewPassword);
        //                        cmdUpdate.Parameters.AddWithValue("@adminId", samId);

        //                        int rowsAffected = cmdUpdate.ExecuteNonQuery();

        //                        if (rowsAffected > 0)
        //                        {
        //                            // Password updated successfully
        //                            ScriptManager.RegisterStartupScript(this, GetType(), "showPasswordSuccess",
        //                                "Swal.fire({ icon: 'success', title: 'Success', text: 'Password changed successfully!', confirmButtonColor: '#28a745' });", true);
        //                        }
        //                        else
        //                        {
        //                            // Error updating the password
        //                            ScriptManager.RegisterStartupScript(this, GetType(), "showUpdateError",
        //                                "Swal.fire({ icon: 'error', title: 'Error', text: 'Unable to update password.', confirmButtonColor: '#dc3545' });", true);
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    // Current password does not match the stored password
        //                    ScriptManager.RegisterStartupScript(this, GetType(), "showIncorrectPasswordError",
        //                        "Swal.fire({ icon: 'error', title: 'Incorrect Password', text: 'The current password you entered is incorrect.', confirmButtonColor: '#dc3545' });", true);
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            // Handle exception (logging or showing a user-friendly message)
        //            ScriptManager.RegisterStartupScript(this, GetType(), "showError",
        //                $"Swal.fire({{ icon: 'error', title: 'Error', text: 'An error occurred: {ex.Message}', confirmButtonColor: '#dc3545' }});", true);
        //        }
        //    }
        //}


        ////without profile update
        //protected void Button1_Click(object sender, EventArgs e)
        //{

        //    // Get employee ID from session
        //    int empId = (int)Session["sam_id"];

        //    // Extracting user input from the front-end form fields
        //    string firstName = firstname.Text;  // First Name TextBox
        //    string middleInitial = m_initial.Text;  // Middle Initial TextBox                    
        //    string lastName = lastname.Text;  // Last Name TextBox
        //    string addr = address.Text;  // Address TextBox
        //    string contact = phone.Text;  // Phone TextBox
        //    string email = this.email.Text;  // Email TextBox

        //    // Ensure all required fields are filled
        //    if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName) &&
        //        !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(contact) &&
        //        !string.IsNullOrEmpty(addr))
        //    {
        //        using (var db = new NpgsqlConnection(con))
        //        {
        //            db.Open();

        //            // SQL to get the current profile information based on session ID
        //            string currentProfileQuery = "SELECT emp_fname, emp_mname, emp_lname, emp_contact, emp_address, emp_email FROM employee WHERE emp_id = @emp_id";
        //            DataRow currentValues;

        //            using (var command = new NpgsqlCommand(currentProfileQuery, db))
        //            {
        //                command.Parameters.AddWithValue("@emp_id", empId);
        //                using (var adapter = new NpgsqlDataAdapter(command))
        //                {
        //                    DataTable dt = new DataTable();
        //                    adapter.Fill(dt);
        //                    currentValues = dt.Rows.Count > 0 ? dt.Rows[0] : null;
        //                }
        //            }

        //            // If employee is found
        //            if (currentValues != null)
        //            {
        //                // Build the update query based on changed values
        //                string updateQuery = "UPDATE employee SET ";
        //                bool hasChanges = false;  // Track if any updates are necessary

        //                if (!string.Equals(currentValues["emp_fname"].ToString(), firstName, StringComparison.OrdinalIgnoreCase))
        //                {
        //                    updateQuery += "emp_fname = @firstName, ";
        //                    hasChanges = true;
        //                }
        //                if (!string.Equals(currentValues["emp_mname"].ToString(), middleInitial, StringComparison.OrdinalIgnoreCase))
        //                {
        //                    updateQuery += "emp_mname = @middleInitial, ";
        //                    hasChanges = true;
        //                }
        //                if (!string.Equals(currentValues["emp_lname"].ToString(), lastName, StringComparison.OrdinalIgnoreCase))
        //                {
        //                    updateQuery += "emp_lname = @lastName, ";
        //                    hasChanges = true;
        //                }
        //                if (!string.Equals(currentValues["emp_contact"].ToString(), contact, StringComparison.OrdinalIgnoreCase))
        //                {
        //                    updateQuery += "emp_contact = @contact, ";
        //                    hasChanges = true;
        //                }
        //                if (!string.Equals(currentValues["emp_address"].ToString(), addr, StringComparison.OrdinalIgnoreCase))
        //                {
        //                    updateQuery += "emp_address = @address, ";
        //                    hasChanges = true;
        //                }
        //                if (!string.Equals(currentValues["emp_email"].ToString(), email, StringComparison.OrdinalIgnoreCase))
        //                {
        //                    updateQuery += "emp_email = @email, ";
        //                    hasChanges = true;
        //                }
        //                LoadProfile();
        //                // Finalize the query
        //                if (hasChanges)
        //                {
        //                    updateQuery += "emp_updated_at = CURRENT_TIMESTAMP WHERE emp_id = @emp_id"; // Update only if necessary

        //                    using (var updateCommand = new NpgsqlCommand(updateQuery, db))
        //                    {
        //                        updateCommand.Parameters.AddWithValue("@emp_id", empId);

        //                        if (updateQuery.Contains("emp_fname"))
        //                            updateCommand.Parameters.Add("@firstName", NpgsqlTypes.NpgsqlDbType.Text).Value = firstName;
        //                        if (updateQuery.Contains("emp_mname"))
        //                            updateCommand.Parameters.Add("@middleInitial", NpgsqlTypes.NpgsqlDbType.Text).Value = middleInitial;
        //                        if (updateQuery.Contains("emp_lname"))
        //                            updateCommand.Parameters.Add("@lastName", NpgsqlTypes.NpgsqlDbType.Text).Value = lastName;
        //                        if (updateQuery.Contains("emp_contact"))
        //                            updateCommand.Parameters.Add("@contact", NpgsqlTypes.NpgsqlDbType.Text).Value = contact;
        //                        if (updateQuery.Contains("emp_address"))
        //                            updateCommand.Parameters.Add("@address", NpgsqlTypes.NpgsqlDbType.Text).Value = addr;
        //                        if (updateQuery.Contains("emp_email"))
        //                            updateCommand.Parameters.Add("@email", NpgsqlTypes.NpgsqlDbType.Text).Value = email;

        //                        // Execute the update command
        //                        int affectedRows = updateCommand.ExecuteNonQuery();
        //                        if (affectedRows >= 1)
        //                        {
        //                            Response.Write("<script>alert('Profile updated successfully!')</script>");
        //                        }
        //                        else
        //                        {
        //                            Response.Write("<script>alert('Profile update failed!')</script>");
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    Response.Write("<script>alert('No changes detected in your profile.')</script>");
        //                }
        //            }
        //            else
        //            {
        //                Response.Write("<script>alert('Employee not found.')</script>");
        //            }
        //            LoadProfile();
        //            db.Close();
        //        }
        //    }
        //    else
        //    {
        //        // Validation error: Required fields are not filled
        //        Response.Write("<script>alert('Please fill up all the required fields!')</script>");
        //        LoadProfile();
        //    }
        //}






        //protected void Button1_Click(object sender, EventArgs e)
        //{
        //    // Get employee ID from session
        //    int empId = (int)Session["sam_id"];

        //    // Extracting user input from the front-end form fields
        //    string firstName = firstname.Text;
        //    string middleInitial = m_initial.Text;
        //    string lastName = lastname.Text;
        //    string addr = address.Text;
        //    string contact = phone.Text;
        //    string email = this.email.Text;

        //    // Check if the required fields are filled
        //    if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName) &&
        //        !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(contact) &&
        //        !string.IsNullOrEmpty(addr))
        //    {
        //        using (var db = new NpgsqlConnection(con))
        //        {
        //            db.Open();

        //            // Query to get the current profile information
        //            string currentProfileQuery = "SELECT emp_fname, emp_mname, emp_lname, emp_contact, emp_address, emp_email, emp_profile FROM employee WHERE emp_id = @emp_id";
        //            DataRow currentValues;

        //            using (var command = new NpgsqlCommand(currentProfileQuery, db))
        //            {
        //                command.Parameters.AddWithValue("@emp_id", empId);
        //                using (var adapter = new NpgsqlDataAdapter(command))
        //                {
        //                    DataTable dt = new DataTable();
        //                    adapter.Fill(dt);
        //                    currentValues = dt.Rows.Count > 0 ? dt.Rows[0] : null;
        //                }
        //            }

        //            if (currentValues != null)
        //            {
        //                // Build update query based on changes
        //                string updateQuery = "UPDATE employee SET ";
        //                bool hasChanges = false;

        //                if (!string.Equals(currentValues["emp_fname"].ToString(), firstName, StringComparison.OrdinalIgnoreCase))
        //                {
        //                    updateQuery += "emp_fname = @firstName, ";
        //                    hasChanges = true;
        //                }
        //                if (!string.Equals(currentValues["emp_mname"].ToString(), middleInitial, StringComparison.OrdinalIgnoreCase))
        //                {
        //                    updateQuery += "emp_mname = @middleInitial, ";
        //                    hasChanges = true;
        //                }
        //                if (!string.Equals(currentValues["emp_lname"].ToString(), lastName, StringComparison.OrdinalIgnoreCase))
        //                {
        //                    updateQuery += "emp_lname = @lastName, ";
        //                    hasChanges = true;
        //                }
        //                if (!string.Equals(currentValues["emp_contact"].ToString(), contact, StringComparison.OrdinalIgnoreCase))
        //                {
        //                    updateQuery += "emp_contact = @contact, ";
        //                    hasChanges = true;
        //                }
        //                if (!string.Equals(currentValues["emp_address"].ToString(), addr, StringComparison.OrdinalIgnoreCase))
        //                {
        //                    updateQuery += "emp_address = @address, ";
        //                    hasChanges = true;
        //                }
        //                if (!string.Equals(currentValues["emp_email"].ToString(), email, StringComparison.OrdinalIgnoreCase))
        //                {
        //                    updateQuery += "emp_email = @email, ";
        //                    hasChanges = true;
        //                }

        //                // Handle profile image updates/removals
        //                if (formFile.HasFile)
        //                {
        //                    // Update with new profile image
        //                    byte[] profileImage = formFile.FileBytes;
        //                    updateQuery += "emp_profile = @profileImage, ";
        //                    hasChanges = true;
        //                }
        //                else if (Request.Form["btnHiddenRemoveImage"] != null)
        //                {
        //                    // Set to default profile image if no new image is uploaded and remove is not clicked
        //                    updateQuery += "emp_profile = @defaultImage, ";
        //                    hasChanges = true;
        //                }
        //                //{
        //                //    // Remove profile image (set to blank)
        //                //    updateQuery += "emp_profile = NULL, ";
        //                //    hasChanges = true;
        //                //}

        //                // Finalize and execute the update if changes exist
        //                if (hasChanges)
        //                {
        //                    updateQuery += "emp_updated_at = CURRENT_TIMESTAMP WHERE emp_id = @emp_id";

        //                    using (var updateCommand = new NpgsqlCommand(updateQuery, db))
        //                    {
        //                        updateCommand.Parameters.AddWithValue("@emp_id", empId);

        //                        if (updateQuery.Contains("emp_fname"))
        //                            updateCommand.Parameters.Add("@firstName", NpgsqlTypes.NpgsqlDbType.Text).Value = firstName;
        //                        if (updateQuery.Contains("emp_mname"))
        //                            updateCommand.Parameters.Add("@middleInitial", NpgsqlTypes.NpgsqlDbType.Text).Value = middleInitial;
        //                        if (updateQuery.Contains("emp_lname"))
        //                            updateCommand.Parameters.Add("@lastName", NpgsqlTypes.NpgsqlDbType.Text).Value = lastName;
        //                        if (updateQuery.Contains("emp_contact"))
        //                            updateCommand.Parameters.Add("@contact", NpgsqlTypes.NpgsqlDbType.Text).Value = contact;
        //                        if (updateQuery.Contains("emp_address"))
        //                            updateCommand.Parameters.Add("@address", NpgsqlTypes.NpgsqlDbType.Text).Value = addr;
        //                        if (updateQuery.Contains("emp_email"))
        //                            updateCommand.Parameters.Add("@email", NpgsqlTypes.NpgsqlDbType.Text).Value = email;
        //                        if (updateQuery.Contains("emp_profile"))
        //                            updateCommand.Parameters.Add("@profileImage", NpgsqlTypes.NpgsqlDbType.Bytea).Value = formFile.FileBytes;

        //                        // Execute the update command
        //                        int affectedRows = updateCommand.ExecuteNonQuery();
        //                        if (affectedRows >= 1)
        //                        {
        //                            Response.Write("<script>alert('Profile updated successfully!')</script>");
        //                            LoadProfile();
        //                        }
        //                        else
        //                        {
        //                            Response.Write("<script>alert('Profile update failed!')</script>");
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    Response.Write("<script>alert('No changes detected.')</script>");
        //                }
        //            }
        //            else
        //            {
        //                Response.Write("<script>alert('Employee not found.')</script>");
        //            }

        //            db.Close();
        //        }
        //    }
        //    else
        //    {
        //        // Validation error: required fields are not filled
        //        Response.Write("<script>alert('Please fill all required fields!')</script>");
        //    }
        //}


        //protected void Button1_Click(object sender, EventArgs e)
        //{
        //    // Get employee ID from session
        //    int empId = (int)Session["sam_id"];

        //    // Extracting user input from the front-end form fields
        //    string firstName = firstname.Text;
        //    string middleInitial = m_initial.Text;
        //    string lastName = lastname.Text;
        //    string addr = address.Text;
        //    string contact = phone.Text;
        //    string email = this.email.Text;

        //    // Check if the required fields are filled
        //    if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName) &&
        //        !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(contact) &&
        //        !string.IsNullOrEmpty(addr))
        //    {
        //        using (var db = new NpgsqlConnection(con))
        //        {
        //            db.Open();

        //            // Query to get the current profile information
        //            string currentProfileQuery = "SELECT emp_fname, emp_mname, emp_lname, emp_contact, emp_address, emp_email, emp_profile FROM employee WHERE emp_id = @emp_id";
        //            DataRow currentValues;

        //            using (var command = new NpgsqlCommand(currentProfileQuery, db))
        //            {
        //                command.Parameters.AddWithValue("@emp_id", empId);
        //                using (var adapter = new NpgsqlDataAdapter(command))
        //                {
        //                    DataTable dt = new DataTable();
        //                    adapter.Fill(dt);
        //                    currentValues = dt.Rows.Count > 0 ? dt.Rows[0] : null;
        //                }
        //            }

        //            if (currentValues != null)
        //            {
        //                // Build update query based on changes
        //                string updateQuery = "UPDATE employee SET ";
        //                bool hasChanges = false;

        //                // Check for changes in other fields
        //                if (!string.Equals(currentValues["emp_fname"].ToString(), firstName, StringComparison.OrdinalIgnoreCase))
        //                {
        //                    updateQuery += "emp_fname = @firstName, ";
        //                    hasChanges = true;
        //                }
        //                if (!string.Equals(currentValues["emp_mname"].ToString(), middleInitial, StringComparison.OrdinalIgnoreCase))
        //                {
        //                    updateQuery += "emp_mname = @middleInitial, ";
        //                    hasChanges = true;
        //                }
        //                if (!string.Equals(currentValues["emp_lname"].ToString(), lastName, StringComparison.OrdinalIgnoreCase))
        //                {
        //                    updateQuery += "emp_lname = @lastName, ";
        //                    hasChanges = true;
        //                }
        //                if (!string.Equals(currentValues["emp_contact"].ToString(), contact, StringComparison.OrdinalIgnoreCase))
        //                {
        //                    updateQuery += "emp_contact = @contact, ";
        //                    hasChanges = true;
        //                }
        //                if (!string.Equals(currentValues["emp_address"].ToString(), addr, StringComparison.OrdinalIgnoreCase))
        //                {
        //                    updateQuery += "emp_address = @address, ";
        //                    hasChanges = true;
        //                }
        //                if (!string.Equals(currentValues["emp_email"].ToString(), email, StringComparison.OrdinalIgnoreCase))
        //                {
        //                    updateQuery += "emp_email = @email, ";
        //                    hasChanges = true;
        //                }

        //                // Handle profile image updates/removals
        //                byte[] profileImage = null;

        //                // Check if a new image is uploaded
        //                if (formFile.HasFile)
        //                {
        //                    // Save the uploaded profile image
        //                    profileImage = formFile.FileBytes;
        //                    updateQuery += "emp_profile = @profileImage, ";
        //                    hasChanges = true;
        //                }
        //                // Check if the remove button was clicked
        //                else if (Request.Form["btnHiddenRemoveImage"] != null)
        //                {

        //                    // Set to default profile image if remove button was clicked
        //                    //profileImage = GetDefaultImage();
        //                    profileImage = File.ReadAllBytes(Server.MapPath("Pictures\\blank_prof.png"));
        //                    updateQuery += "emp_profile = @profileImage, ";
        //                    hasChanges = true;
        //                }

        //                // Finalize and execute the update if changes exist
        //                if (hasChanges)
        //                {
        //                    updateQuery += "emp_updated_at = CURRENT_TIMESTAMP WHERE emp_id = @emp_id";

        //                    using (var updateCommand = new NpgsqlCommand(updateQuery, db))
        //                    {
        //                        updateCommand.Parameters.AddWithValue("@emp_id", empId);

        //                        // Add parameters for updated fields
        //                        if (updateQuery.Contains("emp_fname"))
        //                            updateCommand.Parameters.Add("@firstName", NpgsqlTypes.NpgsqlDbType.Text).Value = firstName;
        //                        if (updateQuery.Contains("emp_mname"))
        //                            updateCommand.Parameters.Add("@middleInitial", NpgsqlTypes.NpgsqlDbType.Text).Value = middleInitial;
        //                        if (updateQuery.Contains("emp_lname"))
        //                            updateCommand.Parameters.Add("@lastName", NpgsqlTypes.NpgsqlDbType.Text).Value = lastName;
        //                        if (updateQuery.Contains("emp_contact"))
        //                            updateCommand.Parameters.Add("@contact", NpgsqlTypes.NpgsqlDbType.Text).Value = contact;
        //                        if (updateQuery.Contains("emp_address"))
        //                            updateCommand.Parameters.Add("@address", NpgsqlTypes.NpgsqlDbType.Text).Value = addr;
        //                        if (updateQuery.Contains("emp_email"))
        //                            updateCommand.Parameters.Add("@email", NpgsqlTypes.NpgsqlDbType.Text).Value = email;

        //                        // Set the profile image parameter (either the uploaded image or the default image)
        //                        if (profileImage != null)
        //                        {
        //                            updateCommand.Parameters.Add("@profileImage", NpgsqlTypes.NpgsqlDbType.Bytea).Value = profileImage;
        //                        }

        //                        // Execute the update command
        //                        int affectedRows = updateCommand.ExecuteNonQuery();
        //                        if (affectedRows >= 1)
        //                        {
        //                            Response.Write("<script>alert('Profile updated successfully!')</script>");
        //                            LoadProfile();
        //                        }
        //                        else
        //                        {
        //                            Response.Write("<script>alert('Profile update failed!')</script>");
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    Response.Write("<script>alert('No changes detected.')</script>");
        //                }
        //            }
        //            else
        //            {
        //                Response.Write("<script>alert('Employee not found.')</script>");
        //            }

        //            db.Close();
        //        }
        //    }
        //    else
        //    {
        //        // Validation error: required fields are not filled
        //        Response.Write("<script>alert('Please fill all required fields!')</script>");
        //    }
        //}




        ////latest na gumagana without crop
        //protected void Button1_Click(object sender, EventArgs e)
        //{
        //    // Get employee ID from session
        //    int empId = (int)Session["sam_id"];

        //    // Extracting user input from the front-end form fields
        //    string firstName = firstname.Text;
        //    string middleInitial = m_initial.Text;
        //    string lastName = lastname.Text;
        //    string addr = address.Text;
        //    string contact = phone.Text;
        //    string email = this.email.Text;

        //    // Check if the required fields are filled
        //    if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName) &&
        //        !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(contact) &&
        //        !string.IsNullOrEmpty(addr))
        //    {
        //        using (var db = new NpgsqlConnection(con))
        //        {
        //            db.Open();

        //            // Query to get the current profile information
        //            string currentProfileQuery = "SELECT emp_fname, emp_mname, emp_lname, emp_contact, emp_address, emp_email, emp_profile FROM employee WHERE emp_id = @emp_id";
        //            DataRow currentValues;

        //            using (var command = new NpgsqlCommand(currentProfileQuery, db))
        //            {
        //                command.Parameters.AddWithValue("@emp_id", empId);
        //                using (var adapter = new NpgsqlDataAdapter(command))
        //                {
        //                    DataTable dt = new DataTable();
        //                    adapter.Fill(dt);
        //                    currentValues = dt.Rows.Count > 0 ? dt.Rows[0] : null;
        //                }
        //            }

        //            if (currentValues != null)
        //            {
        //                // Build update query based on changes
        //                string updateQuery = "UPDATE employee SET ";
        //                bool hasChanges = false;

        //                // Check for changes in other fields
        //                if (!string.Equals(currentValues["emp_fname"].ToString(), firstName, StringComparison.OrdinalIgnoreCase))
        //                {
        //                    updateQuery += "emp_fname = @firstName, ";
        //                    hasChanges = true;
        //                }
        //                if (!string.Equals(currentValues["emp_mname"].ToString(), middleInitial, StringComparison.OrdinalIgnoreCase))
        //                {
        //                    updateQuery += "emp_mname = @middleInitial, ";
        //                    hasChanges = true;
        //                }
        //                if (!string.Equals(currentValues["emp_lname"].ToString(), lastName, StringComparison.OrdinalIgnoreCase))
        //                {
        //                    updateQuery += "emp_lname = @lastName, ";
        //                    hasChanges = true;
        //                }
        //                if (!string.Equals(currentValues["emp_contact"].ToString(), contact, StringComparison.OrdinalIgnoreCase))
        //                {
        //                    updateQuery += "emp_contact = @contact, ";
        //                    hasChanges = true;
        //                }
        //                if (!string.Equals(currentValues["emp_address"].ToString(), addr, StringComparison.OrdinalIgnoreCase))
        //                {
        //                    updateQuery += "emp_address = @address, ";
        //                    hasChanges = true;
        //                }
        //                if (!string.Equals(currentValues["emp_email"].ToString(), email, StringComparison.OrdinalIgnoreCase))
        //                {
        //                    updateQuery += "emp_email = @email, ";
        //                    hasChanges = true;
        //                }

        //                // Handle profile image updates/removals
        //                byte[] profileImage = null;

        //                // Check if a new image is uploaded
        //                if (formFile.HasFile)
        //                {
        //                    // Save the uploaded profile image
        //                    profileImage = formFile.FileBytes;
        //                    updateQuery += "emp_profile = @profileImage, ";
        //                    hasChanges = true;
        //                }
        //                // Check if the remove button was clicked via the hidden field
        //                else if (hfImageRemoved.Value == "true")
        //                {
        //                    // Read the default profile image directly from the server path
        //                    profileImage = File.ReadAllBytes(Server.MapPath("~/Pictures/blank_prof.png"));
        //                    updateQuery += "emp_profile = @profileImage, ";
        //                    hasChanges = true;
        //                }

        //                // Always add emp_profile to the update query if there's an image to set
        //                if (profileImage != null)
        //                {
        //                    // Finalize and execute the update if changes exist
        //                    if (hasChanges)
        //                    {
        //                        updateQuery += "emp_updated_at = CURRENT_TIMESTAMP WHERE emp_id = @emp_id";

        //                        using (var updateCommand = new NpgsqlCommand(updateQuery, db))
        //                        {
        //                            updateCommand.Parameters.AddWithValue("@emp_id", empId);

        //                            // Add parameters for updated fields
        //                            if (updateQuery.Contains("emp_fname"))
        //                                updateCommand.Parameters.Add("@firstName", NpgsqlTypes.NpgsqlDbType.Text).Value = firstName;
        //                            if (updateQuery.Contains("emp_mname"))
        //                                updateCommand.Parameters.Add("@middleInitial", NpgsqlTypes.NpgsqlDbType.Text).Value = middleInitial;
        //                            if (updateQuery.Contains("emp_lname"))
        //                                updateCommand.Parameters.Add("@lastName", NpgsqlTypes.NpgsqlDbType.Text).Value = lastName;
        //                            if (updateQuery.Contains("emp_contact"))
        //                                updateCommand.Parameters.Add("@contact", NpgsqlTypes.NpgsqlDbType.Text).Value = contact;
        //                            if (updateQuery.Contains("emp_address"))
        //                                updateCommand.Parameters.Add("@address", NpgsqlTypes.NpgsqlDbType.Text).Value = addr;
        //                            if (updateQuery.Contains("emp_email"))
        //                                updateCommand.Parameters.Add("@email", NpgsqlTypes.NpgsqlDbType.Text).Value = email;

        //                            // Set the profile image parameter (either the uploaded image or the default image)
        //                            updateCommand.Parameters.Add("@profileImage", NpgsqlTypes.NpgsqlDbType.Bytea).Value = profileImage;

        //                            // Execute the update command
        //                            int affectedRows = updateCommand.ExecuteNonQuery();
        //                            if (affectedRows >= 1)
        //                            {
        //                                Response.Write("<script>alert('Successfully Changed!')</script>");
        //                                LoadProfile();
        //                            }
        //                            else
        //                            {
        //                                Response.Write("<script>alert('Profile update failed!')</script>");
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        Response.Write("<script>alert('No changes detected.')</script>");
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                Response.Write("<script>alert('Employee not found.')</script>");
        //            }

        //            db.Close();
        //        }
        //    }
        //    else
        //    {
        //        // Validation error: required fields are not filled
        //        Response.Write("<script>alert('Please fill all required fields!')</script>");
        //    }
        //}



        ////WITH CROP
        //protected void Button1_Click(object sender, EventArgs e)
        //{
        //    // Get employee ID from session
        //    int empId = (int)Session["sam_id"];

        //    // Extracting user input from the front-end form fields
        //    string firstName = firstname.Text;
        //    string middleInitial = m_initial.Text;
        //    string lastName = lastname.Text;
        //    string addr = address.Text;
        //    string contact = phone.Text;
        //    string email = this.email.Text;

        //    // Check if the required fields are filled
        //    if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName) &&
        //        !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(contact) &&
        //        !string.IsNullOrEmpty(addr))
        //    {
        //        using (var db = new NpgsqlConnection(con))
        //        {
        //            db.Open();

        //            // Query to get the current profile information
        //            string currentProfileQuery = "SELECT emp_fname, emp_mname, emp_lname, emp_contact, emp_address, emp_email, emp_profile FROM employee WHERE emp_id = @emp_id";
        //            DataRow currentValues;

        //            using (var command = new NpgsqlCommand(currentProfileQuery, db))
        //            {
        //                command.Parameters.AddWithValue("@emp_id", empId);
        //                using (var adapter = new NpgsqlDataAdapter(command))
        //                {
        //                    DataTable dt = new DataTable();
        //                    adapter.Fill(dt);
        //                    currentValues = dt.Rows.Count > 0 ? dt.Rows[0] : null;
        //                }
        //            }

        //            if (currentValues != null)
        //            {
        //                // Build update query based on changes
        //                string updateQuery = "UPDATE employee SET ";
        //                bool hasChanges = false;

        //                // Check for changes in other fields
        //                if (!string.Equals(currentValues["emp_fname"].ToString(), firstName, StringComparison.OrdinalIgnoreCase))
        //                {
        //                    updateQuery += "emp_fname = @firstName, ";
        //                    hasChanges = true;
        //                }
        //                if (!string.Equals(currentValues["emp_mname"].ToString(), middleInitial, StringComparison.OrdinalIgnoreCase))
        //                {
        //                    updateQuery += "emp_mname = @middleInitial, ";
        //                    hasChanges = true;
        //                }
        //                if (!string.Equals(currentValues["emp_lname"].ToString(), lastName, StringComparison.OrdinalIgnoreCase))
        //                {
        //                    updateQuery += "emp_lname = @lastName, ";
        //                    hasChanges = true;
        //                }
        //                if (!string.Equals(currentValues["emp_contact"].ToString(), contact, StringComparison.OrdinalIgnoreCase))
        //                {
        //                    updateQuery += "emp_contact = @contact, ";
        //                    hasChanges = true;
        //                }
        //                if (!string.Equals(currentValues["emp_address"].ToString(), addr, StringComparison.OrdinalIgnoreCase))
        //                {
        //                    updateQuery += "emp_address = @address, ";
        //                    hasChanges = true;
        //                }
        //                if (!string.Equals(currentValues["emp_email"].ToString(), email, StringComparison.OrdinalIgnoreCase))
        //                {
        //                    updateQuery += "emp_email = @email, ";
        //                    hasChanges = true;
        //                }

        //                // Handle profile image updates/removals
        //                byte[] profileImage = null;

        //                // Check if a new image is uploaded or cropped image exists
        //                if (!string.IsNullOrEmpty(hfCroppedImage.Value))
        //                {
        //                    // Handle cropped image from Base64 (from the hidden field)
        //                    string base64ImageData = hfCroppedImage.Value.Replace("data:image/png;base64,", string.Empty);
        //                    profileImage = Convert.FromBase64String(base64ImageData);
        //                    updateQuery += "emp_profile = @profileImage, ";
        //                    hasChanges = true;
        //                }
        //                else if (formFile.HasFile)
        //                {
        //                    // Save the uploaded profile image
        //                    profileImage = formFile.FileBytes;
        //                    updateQuery += "emp_profile = @profileImage, ";
        //                    hasChanges = true;
        //                }
        //                // Check if the remove button was clicked via the hidden field
        //                else if (hfImageRemoved.Value == "true")
        //                {
        //                    // Read the default profile image directly from the server path
        //                    profileImage = File.ReadAllBytes(Server.MapPath("~/Pictures/blank_prof.png"));
        //                    updateQuery += "emp_profile = @profileImage, ";
        //                    hasChanges = true;
        //                }

        //                // Finalize and execute the update if changes exist
        //                if (hasChanges)
        //                {
        //                    updateQuery += "emp_updated_at = CURRENT_TIMESTAMP WHERE emp_id = @emp_id";

        //                    using (var updateCommand = new NpgsqlCommand(updateQuery, db))
        //                    {
        //                        updateCommand.Parameters.AddWithValue("@emp_id", empId);

        //                        // Add parameters for updated fields
        //                        if (updateQuery.Contains("emp_fname"))
        //                            updateCommand.Parameters.Add("@firstName", NpgsqlTypes.NpgsqlDbType.Text).Value = firstName;
        //                        if (updateQuery.Contains("emp_mname"))
        //                            updateCommand.Parameters.Add("@middleInitial", NpgsqlTypes.NpgsqlDbType.Text).Value = middleInitial;
        //                        if (updateQuery.Contains("emp_lname"))
        //                            updateCommand.Parameters.Add("@lastName", NpgsqlTypes.NpgsqlDbType.Text).Value = lastName;
        //                        if (updateQuery.Contains("emp_contact"))
        //                            updateCommand.Parameters.Add("@contact", NpgsqlTypes.NpgsqlDbType.Text).Value = contact;
        //                        if (updateQuery.Contains("emp_address"))
        //                            updateCommand.Parameters.Add("@address", NpgsqlTypes.NpgsqlDbType.Text).Value = addr;
        //                        if (updateQuery.Contains("emp_email"))
        //                            updateCommand.Parameters.Add("@email", NpgsqlTypes.NpgsqlDbType.Text).Value = email;

        //                        // Set the profile image parameter (either the uploaded or the default image)
        //                        if (profileImage != null)
        //                        {
        //                            updateCommand.Parameters.Add("@profileImage", NpgsqlTypes.NpgsqlDbType.Bytea).Value = profileImage;
        //                        }

        //                        // Execute the update command
        //                        int affectedRows = updateCommand.ExecuteNonQuery();
        //                        if (affectedRows >= 1)
        //                        {
        //                            Response.Write("<script>alert('Successfully Changed!')</script>");
        //                            LoadProfile();
        //                        }
        //                        else
        //                        {
        //                            Response.Write("<script>alert('Profile update failed!')</script>");
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    Response.Write("<script>alert('No changes detected.')</script>");
        //                }
        //            }
        //            else
        //            {
        //                Response.Write("<script>alert('Employee not found.')</script>");
        //            }

        //            db.Close();
        //        }
        //    }
        //    else
        //    {
        //        // Validation error: required fields are not filled
        //        Response.Write("<script>alert('Please fill all required fields!')</script>");
        //    }
        //}



        ////new version
        //protected void Button1_Click(object sender, EventArgs e)
        //{
        //    // Get employee ID from session
        //    int empId = (int)Session["sam_id"];

        //    // Extract user input from form fields
        //    string firstName = firstname.Text;
        //    string middleInitial = m_initial.Text;
        //    string lastName = lastname.Text;
        //    string addr = address.Text;
        //    string contact = phone.Text;
        //    string email = this.email.Text;

        //    // Ensure all required fields are filled
        //    if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName) &&
        //        !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(contact) &&
        //        !string.IsNullOrEmpty(addr))
        //    {
        //        using (var db = new NpgsqlConnection(con))
        //        {
        //            db.Open();

        //            // SQL to get the current profile information based on session ID
        //            string currentProfileQuery = "SELECT emp_profile, emp_fname, emp_mname, emp_lname, emp_contact, emp_address, emp_email FROM employee WHERE emp_id = @emp_id";
        //            DataRow currentValues;

        //            using (var command = new NpgsqlCommand(currentProfileQuery, db))
        //            {
        //                command.Parameters.AddWithValue("@emp_id", empId);
        //                using (var adapter = new NpgsqlDataAdapter(command))
        //                {
        //                    DataTable dt = new DataTable();
        //                    adapter.Fill(dt);
        //                    currentValues = dt.Rows.Count > 0 ? dt.Rows[0] : null;
        //                }
        //            }

        //            // If employee is found
        //            if (currentValues != null)
        //            {
        //                // Build the update query based on changed values
        //                string updateQuery = "UPDATE employee SET ";
        //                bool hasChanges = false;

        //                // Update personal information if changed
        //                if (!string.Equals(currentValues["emp_fname"].ToString(), firstName, StringComparison.OrdinalIgnoreCase))
        //                {
        //                    updateQuery += "emp_fname = @firstName, ";
        //                    hasChanges = true;
        //                }
        //                if (!string.Equals(currentValues["emp_mname"].ToString(), middleInitial, StringComparison.OrdinalIgnoreCase))
        //                {
        //                    updateQuery += "emp_mname = @middleInitial, ";
        //                    hasChanges = true;
        //                }
        //                if (!string.Equals(currentValues["emp_lname"].ToString(), lastName, StringComparison.OrdinalIgnoreCase))
        //                {
        //                    updateQuery += "emp_lname = @lastName, ";
        //                    hasChanges = true;
        //                }
        //                if (!string.Equals(currentValues["emp_contact"].ToString(), contact, StringComparison.OrdinalIgnoreCase))
        //                {
        //                    updateQuery += "emp_contact = @contact, ";
        //                    hasChanges = true;
        //                }
        //                if (!string.Equals(currentValues["emp_address"].ToString(), addr, StringComparison.OrdinalIgnoreCase))
        //                {
        //                    updateQuery += "emp_address = @address, ";
        //                    hasChanges = true;
        //                }
        //                if (!string.Equals(currentValues["emp_email"].ToString(), email, StringComparison.OrdinalIgnoreCase))
        //                {
        //                    updateQuery += "emp_email = @email, ";
        //                    hasChanges = true;
        //                }

        //                // Handle profile image upload or removal
        //                if (btnHiddenRemoveImage.Visible) // Check if "Remove" button was clicked
        //                {
        //                    updateQuery += "emp_profile = NULL, ";  // Clear profile image
        //                    hasChanges = true;
        //                }
        //                else if (formFile.HasFile)  // If a new file is uploaded
        //                {
        //                    byte[] imageBytes = formFile.FileBytes; // Get uploaded file data
        //                    updateQuery += "emp_profile = @profileImage, ";
        //                    hasChanges = true;

        //                    using (var updateCommand = new NpgsqlCommand(updateQuery, db))
        //                    {
        //                        updateCommand.Parameters.AddWithValue("@profileImage", imageBytes); // Save image
        //                        updateCommand.Parameters.AddWithValue("@emp_id", empId);
        //                        if (updateQuery.Contains("emp_fname"))
        //                            updateCommand.Parameters.Add("@firstName", NpgsqlTypes.NpgsqlDbType.Text).Value = firstName;
        //                        if (updateQuery.Contains("emp_mname"))
        //                            updateCommand.Parameters.Add("@middleInitial", NpgsqlTypes.NpgsqlDbType.Text).Value = middleInitial;
        //                        if (updateQuery.Contains("emp_lname"))
        //                            updateCommand.Parameters.Add("@lastName", NpgsqlTypes.NpgsqlDbType.Text).Value = lastName;
        //                        if (updateQuery.Contains("emp_contact"))
        //                            updateCommand.Parameters.Add("@contact", NpgsqlTypes.NpgsqlDbType.Text).Value = contact;
        //                        if (updateQuery.Contains("emp_address"))
        //                            updateCommand.Parameters.Add("@address", NpgsqlTypes.NpgsqlDbType.Text).Value = addr;
        //                        if (updateQuery.Contains("emp_email"))
        //                            updateCommand.Parameters.Add("@email", NpgsqlTypes.NpgsqlDbType.Text).Value = email;

        //                        int affectedRows = updateCommand.ExecuteNonQuery();
        //                        if (affectedRows >= 1)
        //                        {
        //                            Response.Write("<script>alert('Profile updated successfully!')</script>");
        //                        }
        //                        else
        //                        {
        //                            Response.Write("<script>alert('Profile update failed!')</script>");
        //                        }
        //                    }
        //                }
        //                else if (hasChanges) // If there were changes but no image updates
        //                {
        //                    updateQuery += "emp_updated_at = CURRENT_TIMESTAMP WHERE emp_id = @emp_id";
        //                    using (var updateCommand = new NpgsqlCommand(updateQuery, db))
        //                    {
        //                        updateCommand.Parameters.AddWithValue("@emp_id", empId);
        //                        updateCommand.ExecuteNonQuery();
        //                    }
        //                }
        //                else
        //                {
        //                    Response.Write("<script>alert('No changes detected in your profile.')</script>");
        //                }

        //                LoadProfile();
        //            }
        //            else
        //            {
        //                Response.Write("<script>alert('Employee not found.')</script>");
        //            }
        //            db.Close();
        //        }
        //    }
        //    else
        //    {
        //        // Validation error: Required fields are not filled
        //        Response.Write("<script>alert('Please fill up all the required fields!')</script>");
        //    }
        //}


        //protected void Button1_Click(object sender, EventArgs e)
        //{
        //    // Get employee ID from session
        //    int empId = (int)Session["sam_id"];

        //    // Extracting user input from the front-end form fields
        //    string firstName = firstname.Text;
        //    string middleInitial = m_initial.Text;
        //    string lastName = lastname.Text;
        //    string addr = address.Text;
        //    string contact = phone.Text;
        //    string email = this.email.Text;

        //    // Check if the required fields are filled
        //    if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName) &&
        //        !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(contact) &&
        //        !string.IsNullOrEmpty(addr))
        //    {
        //        using (var db = new NpgsqlConnection(con))
        //        {
        //            db.Open();

        //            // Query to get the current profile information
        //            string currentProfileQuery = "SELECT emp_fname, emp_mname, emp_lname, emp_contact, emp_address, emp_email, emp_profile FROM employee WHERE emp_id = @emp_id";
        //            DataRow currentValues;

        //            using (var command = new NpgsqlCommand(currentProfileQuery, db))
        //            {
        //                command.Parameters.AddWithValue("@emp_id", empId);
        //                using (var adapter = new NpgsqlDataAdapter(command))
        //                {
        //                    DataTable dt = new DataTable();
        //                    adapter.Fill(dt);
        //                    currentValues = dt.Rows.Count > 0 ? dt.Rows[0] : null;
        //                }
        //            }

        //            if (currentValues != null)
        //            {
        //                // Build update query based on changes
        //                string updateQuery = "UPDATE employee SET ";
        //                bool hasChanges = false;

        //                // Check for changes in other fields
        //                if (!string.Equals(currentValues["emp_fname"].ToString(), firstName, StringComparison.OrdinalIgnoreCase))
        //                {
        //                    updateQuery += "emp_fname = @firstName, ";
        //                    hasChanges = true;
        //                }
        //                if (!string.Equals(currentValues["emp_mname"].ToString(), middleInitial, StringComparison.OrdinalIgnoreCase))
        //                {
        //                    updateQuery += "emp_mname = @middleInitial, ";
        //                    hasChanges = true;
        //                }
        //                if (!string.Equals(currentValues["emp_lname"].ToString(), lastName, StringComparison.OrdinalIgnoreCase))
        //                {
        //                    updateQuery += "emp_lname = @lastName, ";
        //                    hasChanges = true;
        //                }
        //                if (!string.Equals(currentValues["emp_contact"].ToString(), contact, StringComparison.OrdinalIgnoreCase))
        //                {
        //                    updateQuery += "emp_contact = @contact, ";
        //                    hasChanges = true;
        //                }
        //                if (!string.Equals(currentValues["emp_address"].ToString(), addr, StringComparison.OrdinalIgnoreCase))
        //                {
        //                    updateQuery += "emp_address = @address, ";
        //                    hasChanges = true;
        //                }
        //                if (!string.Equals(currentValues["emp_email"].ToString(), email, StringComparison.OrdinalIgnoreCase))
        //                {
        //                    updateQuery += "emp_email = @email, ";
        //                    hasChanges = true;
        //                }

        //                // Handle profile image updates/removals
        //                byte[] profileImage = null;

        //                // Check if a new image is uploaded
        //                if (formFile.HasFile)
        //                {
        //                    // Save the uploaded profile image
        //                    profileImage = formFile.FileBytes;
        //                    updateQuery += "emp_profile = @profileImage, ";
        //                    hasChanges = true;
        //                }
        //                // Check if the remove button was clicked via the hidden field
        //                else if (hfImageRemoved.Value == "true")
        //                {
        //                    // Read the default profile image directly from the server path
        //                    profileImage = File.ReadAllBytes(Server.MapPath("~/Pictures/blank_prof.png"));
        //                    updateQuery += "emp_profile = @profileImage, ";
        //                    hasChanges = true;
        //                }

        //                // Always add emp_profile to the update query if there's an image to set
        //                if (profileImage != null)
        //                {
        //                    // Finalize and execute the update if changes exist
        //                    if (hasChanges)
        //                    {
        //                        updateQuery += "emp_updated_at = CURRENT_TIMESTAMP WHERE emp_id = @emp_id";

        //                        using (var updateCommand = new NpgsqlCommand(updateQuery, db))
        //                        {
        //                            updateCommand.Parameters.AddWithValue("@emp_id", empId);

        //                            // Add parameters for updated fields
        //                            if (updateQuery.Contains("emp_fname"))
        //                                updateCommand.Parameters.Add("@firstName", NpgsqlTypes.NpgsqlDbType.Text).Value = firstName;
        //                            if (updateQuery.Contains("emp_mname"))
        //                                updateCommand.Parameters.Add("@middleInitial", NpgsqlTypes.NpgsqlDbType.Text).Value = middleInitial;
        //                            if (updateQuery.Contains("emp_lname"))
        //                                updateCommand.Parameters.Add("@lastName", NpgsqlTypes.NpgsqlDbType.Text).Value = lastName;
        //                            if (updateQuery.Contains("emp_contact"))
        //                                updateCommand.Parameters.Add("@contact", NpgsqlTypes.NpgsqlDbType.Text).Value = contact;
        //                            if (updateQuery.Contains("emp_address"))
        //                                updateCommand.Parameters.Add("@address", NpgsqlTypes.NpgsqlDbType.Text).Value = addr;
        //                            if (updateQuery.Contains("emp_email"))
        //                                updateCommand.Parameters.Add("@email", NpgsqlTypes.NpgsqlDbType.Text).Value = email;

        //                            // Set the profile image parameter (either the uploaded image or the default image)
        //                            updateCommand.Parameters.Add("@profileImage", NpgsqlTypes.NpgsqlDbType.Bytea).Value = profileImage;

        //                            // Execute the update command
        //                            int affectedRows = updateCommand.ExecuteNonQuery();
        //                            if (affectedRows >= 1)
        //                            {
        //                                Response.Write("<script>alert('Successfully Changed!')</script>");
        //                                LoadProfile();
        //                            }
        //                            else
        //                            {
        //                                Response.Write("<script>alert('Profile update failed!')</script>");
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        Response.Write("<script>alert('No changes detected.')</script>");
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                Response.Write("<script>alert('Employee not found.')</script>");
        //            }

        //            db.Close();
        //        }
        //    }
        //    else
        //    {
        //        // Validation error: required fields are not filled
        //        Response.Write("<script>alert('Please fill all required fields!')</script>");
        //    }
        //}


        ////FINAL WITHOUT SCRIPTMANAGER DIALOG
        //protected void Button1_Click(object sender, EventArgs e)
        //{
        //    // Get employee ID from session
        //    int empId = (int)Session["sam_id"];

        //    // Extract user input from the form fields
        //    string firstName = firstname.Text;
        //    string middleInitial = m_initial.Text;
        //    string lastName = lastname.Text;
        //    string addr = address.Text;
        //    string contact = phone.Text;
        //    string email = this.email.Text;

        //    // Check if the required fields are filled
        //    if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName) &&
        //        !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(contact) &&
        //        !string.IsNullOrEmpty(addr))
        //    {
        //        using (var db = new NpgsqlConnection(con))
        //        {
        //            db.Open();

        //            // Prepare update query
        //            string updateQuery = "UPDATE employee SET emp_fname = @firstName, emp_mname = @middleInitial, emp_lname = @lastName, emp_contact = @contact, emp_address = @address, emp_email = @email";
        //            bool hasChanges = true;
        //            byte[] profileImage = null;

        //            // Check if the remove button was clicked
        //            if (hfImageRemoved.Value == "true")
        //            {
        //                // Load the default profile image
        //                profileImage = File.ReadAllBytes(Server.MapPath("~/Pictures/blank_prof.png"));
        //                updateQuery += ", emp_profile = @profileImage";
        //                hasChanges = true;
        //            }
        //            else if (formFile.HasFile)
        //            {
        //                // Save the uploaded profile image
        //                profileImage = formFile.FileBytes;
        //                updateQuery += ", emp_profile = @profileImage";
        //                hasChanges = true;
        //            }

        //            // Finalize the update query
        //            updateQuery += ", emp_updated_at = CURRENT_TIMESTAMP WHERE emp_id = @emp_id";

        //            if (hasChanges)
        //            {
        //                using (var updateCommand = new NpgsqlCommand(updateQuery, db))
        //                {
        //                    // Add parameters for updated fields
        //                    updateCommand.Parameters.AddWithValue("@emp_id", empId);
        //                    updateCommand.Parameters.AddWithValue("@firstName", firstName);
        //                    updateCommand.Parameters.AddWithValue("@middleInitial", middleInitial);
        //                    updateCommand.Parameters.AddWithValue("@lastName", lastName);
        //                    updateCommand.Parameters.AddWithValue("@contact", contact);
        //                    updateCommand.Parameters.AddWithValue("@address", addr);
        //                    updateCommand.Parameters.AddWithValue("@email", email);

        //                    // Add profile image parameter if required
        //                    if (profileImage != null)
        //                    {
        //                        updateCommand.Parameters.Add("@profileImage", NpgsqlTypes.NpgsqlDbType.Bytea).Value = profileImage;
        //                    }

        //                    // Execute the update command
        //                    int affectedRows = updateCommand.ExecuteNonQuery();
        //                    if (affectedRows >= 1)
        //                    {
        //                        //Response.Write("<script>alert('Successfully Changed!')</script>");
        //                        ScriptManager.RegisterStartupScript(this, GetType(), "showEmailAlert",
        //                        "Swal.fire({ icon: 'error', title: 'Email Exists', text: 'The email address is already associated with another account.', confirmButtonColor: '#dc3545' });", true);
        //                        LoadProfile();
        //                    }
        //                    else
        //                    {
        //                        Response.Write("<script>alert('Profile update failed!')</script>");
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                Response.Write("<script>alert('No changes detected.')</script>");
        //            }
        //        }
        //    }
        //    else
        //    {
        //        Response.Write("<script>alert('Please fill all required fields!')</script>");
        //    }

        //    // Clear the image removed hidden field after saving
        //    hfImageRemoved.Value = "false";
        //}



        //protected void Button1_Click(object sender, EventArgs e)
        //{
        //    // Get employee ID from session
        //    int empId = (int)Session["sam_id"];

        //    // Extract user input from the form fields
        //    string firstName = firstname.Text;
        //    string middleInitial = m_initial.Text;
        //    string lastName = lastname.Text;
        //    string addr = address.Text;
        //    string contact = phone.Text;
        //    string email = this.email.Text;

        //    // Check if the required fields are filled
        //    if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName) &&
        //        !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(contact) &&
        //        !string.IsNullOrEmpty(addr))
        //    {
        //        using (var db = new NpgsqlConnection(con))
        //        {
        //            db.Open();

        //            // Check if the email already exists for a different employee
        //            string emailCheckQuery = "SELECT COUNT(*) FROM employee WHERE emp_email = @Email AND emp_id != @emp_id";
        //            using (var emailCheckCommand = new NpgsqlCommand(emailCheckQuery, db))
        //            {
        //                emailCheckCommand.Parameters.AddWithValue("@Email", email);
        //                emailCheckCommand.Parameters.AddWithValue("@emp_id", empId);
        //                int emailExists = Convert.ToInt32(emailCheckCommand.ExecuteScalar());

        //                if (emailExists > 0)
        //                {
        //                    ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
        //                                        "Swal.fire({ icon: 'error', title: 'Invalid ID Format', text: 'Please enter a valid ID format.', background: '#f8d7da', confirmButtonColor: '#dc3545' });", true);
        //                    // Show alert if email already exists
        //                    ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
        //                        "Swal.fire({ icon: 'error', title: 'Email Exists', text: 'The email address is already associated with another account.', confirmButtonColor: '#dc3545' });", true);
        //                    return;
        //                }
        //            }

        //            // Prepare update query
        //            string updateQuery = "UPDATE employee SET emp_fname = @firstName, emp_mname = @middleInitial, emp_lname = @lastName, emp_contact = @contact, emp_address = @address, emp_email = @Email";
        //            bool hasChanges = true;
        //            byte[] profileImage = null;

        //            // Check if the remove button was clicked
        //            if (hfImageRemoved.Value == "true")
        //            {
        //                // Load the default profile image
        //                profileImage = File.ReadAllBytes(Server.MapPath("~/Pictures/blank_prof.png"));
        //                updateQuery += ", emp_profile = @profileImage";
        //                hasChanges = true;
        //            }
        //            else if (formFile.HasFile)
        //            {
        //                // Save the uploaded profile image
        //                profileImage = formFile.FileBytes;
        //                updateQuery += ", emp_profile = @profileImage";
        //                hasChanges = true;
        //            }

        //            // Finalize the update query
        //            updateQuery += ", emp_updated_at = CURRENT_TIMESTAMP WHERE emp_id = @emp_id";

        //            if (hasChanges)
        //            {
        //                using (var updateCommand = new NpgsqlCommand(updateQuery, db))
        //                {
        //                    // Add parameters for updated fields
        //                    updateCommand.Parameters.AddWithValue("@emp_id", empId);
        //                    updateCommand.Parameters.AddWithValue("@firstName", firstName);
        //                    updateCommand.Parameters.AddWithValue("@middleInitial", middleInitial);
        //                    updateCommand.Parameters.AddWithValue("@lastName", lastName);
        //                    updateCommand.Parameters.AddWithValue("@contact", contact);
        //                    updateCommand.Parameters.AddWithValue("@address", addr);
        //                    updateCommand.Parameters.AddWithValue("@Email", email);

        //                    // Add profile image parameter if required
        //                    if (profileImage != null)
        //                    {
        //                        updateCommand.Parameters.Add("@profileImage", NpgsqlTypes.NpgsqlDbType.Bytea).Value = profileImage;
        //                    }

        //                    // Execute the update command
        //                    int affectedRows = updateCommand.ExecuteNonQuery();
        //                    if (affectedRows >= 1)
        //                    {
        //                        ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
        //                            "Swal.fire({ icon: 'success', title: 'Profile Updated', text: 'Your profile was successfully updated!', confirmButtonColor: '#28a745' });", true);
        //                        LoadProfile();
        //                    }
        //                    else
        //                    {
        //                        ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
        //                            "Swal.fire({ icon: 'error', title: 'Update Failed', text: 'Profile update failed. Please try again.', confirmButtonColor: '#dc3545' });", true);
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
        //                    "Swal.fire({ icon: 'info', title: 'No Changes', text: 'No changes detected in your profile.', confirmButtonColor: '#17a2b8' });", true);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
        //            "Swal.fire({ icon: 'warning', title: 'Incomplete Fields', text: 'Please fill all required fields!', confirmButtonColor: '#ffc107' });", true);
        //    }

        //    // Clear the image removed hidden field after saving
        //    hfImageRemoved.Value = "false";
        //}

        protected void Button1_Click(object sender, EventArgs e)
        {
            int empId = (int)Session["sam_id"];
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