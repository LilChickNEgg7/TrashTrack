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
using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using System.Xml.Linq;


namespace Capstone
{
    public partial class AM_AccountMan : System.Web.UI.Page
    {
        // Database Connection String
        // Database Connection String
        private readonly string con = "Server=localhost;Port=5432;User Id=postgres;Password=123456;Database=trashtrack";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //BindNotifications();
                LoadRoles();
                AccountManList();
                BillinOfficerList();
                OperationalDispList();
                HaulerList();
                LoadProfile();
                LoadChangeRoles();
                ClearFormFields();
                hfActiveTab.Value = "#am"; // Set Tab 1 as the default

            }

        }

        //protected void BindNotifications()
        //{
        //    using (var db = new NpgsqlConnection(con))
        //    {
        //        db.Open();
        //        using (var cmd = db.CreateCommand())
        //        {
        //            // Query to fetch recent contractual applications that are not deleted
        //            cmd.CommandType = CommandType.Text;
        //            cmd.CommandText = "SELECT cont_id, cont_rep_name, cont_comp_name, cont_status, cont_created_at, read_status FROM contractual WHERE cont_status != 'Deleted' ORDER BY cont_created_at DESC";

        //            DataTable dtNotifications = new DataTable();
        //            NpgsqlDataAdapter sda = new NpgsqlDataAdapter(cmd);
        //            sda.Fill(dtNotifications);

        //            // Bind the data to the Repeater
        //            NotificationRepeater.DataSource = dtNotifications;
        //            NotificationRepeater.DataBind();

        //            // Update the notification count and header dynamically
        //            int notificationCountValue = 0; // Initialize count variable

        //            // Query to count unread notifications
        //            cmd.CommandText = "SELECT COUNT(*) FROM contractual WHERE read_status = 'Unread' AND cont_status != 'Deleted'";
        //            notificationCountValue = Convert.ToInt32(cmd.ExecuteScalar()); // Get the count

        //            // Access the server control 'notificationCount' (ensure this is the actual control name in .aspx)
        //            notificationCount.InnerText = notificationCountValue.ToString(); // Update the count
        //            notificationHeader.InnerText = notificationCountValue.ToString(); // Assuming 'notificationHeader' is an HtmlGenericControl
        //        }
        //        db.Close();
        //    }
        //}


        // Helper function to get the corresponding icon based on the status
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

        protected void NotificationRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "MarkAsRead")
            {
                int notificationId = Convert.ToInt32(e.CommandArgument);

                // Call a method to mark this notification as read in the database
                MarkNotificationAsRead(notificationId);

                // Update the notification count
                int currentCount = Convert.ToInt32(notificationCount.InnerText); // Get current count
                notificationCount.InnerText = (currentCount - 1).ToString(); // Decrement the count

                // Update the header count as well
                notificationHeader.InnerText = notificationCount.InnerText;

                // Re-fetch and re-bind the notifications to reflect the updated status
                //BindNotifications();
            }
        }

        private void MarkNotificationAsRead(int notificationId)
        {
            using (var db = new NpgsqlConnection(con))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    // Update the notification status in the database
                    cmd.CommandText = "UPDATE contractual SET read_status = 'Read' WHERE cont_id = @notificationId";
                    cmd.Parameters.AddWithValue("@notificationId", notificationId);
                    cmd.ExecuteNonQuery();
                }
                db.Close();
            }
        }


        protected void Notification_Click(object sender, EventArgs e)
        {
            // Get the clicked notification's ID
            LinkButton lnkButton = (LinkButton)sender;
            int notificationId = Convert.ToInt32(lnkButton.CommandArgument);

            // Logic to handle the notification (mark as read)
            MarkNotificationAsRead(notificationId);

            // Update notification count
            int currentCount = Convert.ToInt32(notificationCount.InnerText);
            notificationCount.InnerText = (currentCount - 1).ToString();

            // Optionally: Update the notification header count
            int headerCount = Convert.ToInt32(notificationHeader.InnerText);
            notificationHeader.InnerText = (headerCount - 1).ToString();

            // Re-bind notifications to update the view
            //BindNotifications();
        }





        private void LoadRoles()
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(con))
            {
                string query = "SELECT role_id, role_name FROM roles ORDER BY role_id";
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

                try
                {
                    conn.Open();
                    NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    emp_role.DataSource = dt;
                    emp_role.DataTextField = "role_name";
                    emp_role.DataValueField = "role_id";
                    emp_role.DataBind();

                    // Clear existing items and add the default "Select Role" option at the top
                    emp_role.Items.Clear();
                    ListItem selectRoleItem = new ListItem("--Select Role--", "0");
                    selectRoleItem.Attributes.Add("disabled", "true"); // Disable the item
                    selectRoleItem.Attributes.Add("selected", "true"); // Set as selected
                    emp_role.Items.Add(selectRoleItem);
                    emp_role.Items.AddRange(dt.AsEnumerable().Select(row => new ListItem(row["role_name"].ToString(), row["role_id"].ToString())).ToArray());
                }
                catch (Exception ex)
                {
                    // Handle exception (logging, showing a message, etc.)
                }
            }
        }


        private void LoadProfile()
        {
            try
            {
                if (Session["am_id"] == null)
                {
                    // Session expired or not set, redirect to login
                    Response.Redirect("LoginPage.aspx");
                    return;
                }

                int adminId = (int)Session["am_id"];  // Retrieve admin ID from session
                string roleName = (string)Session["am_rolename"];


                byte[] imageData = null;  // Initialize imageData
                string originalFirstname = null;
                string originalMi = null;
                string originalLastname = null;

                // Define the PostgreSQL connection
                using (var db = new NpgsqlConnection(con))
                {
                    db.Open();

                    // PostgreSQL query to get employee details including profile image
                    string query = "SELECT emp_fname, emp_mname, emp_lname, emp_contact, emp_email, emp_password, emp_profile FROM employee WHERE emp_id = @id";
                    using (var cmd = new NpgsqlCommand(query, db))
                    {
                        // Set the parameter for admin ID
                        cmd.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Integer, adminId);

                        // Execute the query and retrieve employee details
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                originalFirstname = reader["emp_fname"].ToString();
                                originalMi = reader["emp_mname"].ToString();
                                originalLastname = reader["emp_lname"].ToString();
                                imageData = reader["emp_profile"] as byte[];
                            }
                            else
                            {
                                // If no data found for the specified ID, return an alert
                                Response.Write("<script>alert('No data found for the specified ID.')</script>");
                                return;
                            }
                        }
                    }
                }



                // Check if the profile_image control exists and is not null
                if (profile_image != null)
                {
                    if (imageData != null && imageData.Length > 0)
                    {
                        string base64String = Convert.ToBase64String(imageData);
                        profile_image.ImageUrl = "data:image/jpeg;base64," + base64String;  // Set image as base64 string
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

                // Check if originalFirstname and originalLastname are not null or empty before setting the label text
                if (!string.IsNullOrEmpty(originalFirstname) && !string.IsNullOrEmpty(originalLastname))
                {
                    Label2.Text = $"{originalFirstname[0]}. {originalLastname}";
                    Label3.Text = $"{roleName}";
                }
                else
                {
                    Label2.Text = "Welcome!";
                }
            }
            catch (Exception ex)
            {
                // Handle the exception
                Response.Write("<script>alert('Error loading profile: " + ex.Message + "');</script>");
                profile_image.ImageUrl = "~/Pictures/blank_prof.png";  // Fallback in case of an error
            }
        }




        protected void AccountManList()
        {

            using (var db = new NpgsqlConnection(con))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    // Modified the query to match the column names in the account_manager table
                    cmd.CommandText = "SELECT * FROM employee WHERE emp_status != 'Deleted' AND role_id = 1 AND emp_id != @id ORDER BY emp_id, emp_status";
                    cmd.Parameters.AddWithValue("@id", Convert.ToInt32(Session["am_id"]));

                    DataTable admin_datatable = new DataTable();
                    NpgsqlDataAdapter admin_sda = new NpgsqlDataAdapter(cmd);
                    admin_sda.Fill(admin_datatable);

                    gridView2.DataSource = admin_datatable; ;
                    gridView2.DataBind();
                }
                db.Close();
            }
        }


        protected void BillinOfficerList()
        {
            using (var db = new NpgsqlConnection(con))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    // Modified the query to match the column names in the account_manager table
                    cmd.CommandText = "SELECT * FROM employee WHERE emp_status != 'Deleted' AND role_id = 3 AND emp_id != @id ORDER BY emp_id, emp_status";
                    cmd.Parameters.AddWithValue("@id", Convert.ToInt32(Session["am_id"]));

                    DataTable admin_datatable = new DataTable();
                    NpgsqlDataAdapter admin_sda = new NpgsqlDataAdapter(cmd);
                    admin_sda.Fill(admin_datatable);

                    gridView3.DataSource = admin_datatable; ;
                    gridView3.DataBind();
                }
                db.Close();
            }
        }


        protected void HaulerList()
        {
            using (var db = new NpgsqlConnection(con))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    // Modified the query to match the column names in the account_manager table
                    cmd.CommandText = "SELECT * FROM employee WHERE emp_status != 'Deleted' AND role_id = 4 AND emp_id != @id ORDER BY emp_id, emp_status";
                    cmd.Parameters.AddWithValue("@id", Convert.ToInt32(Session["id"]));

                    DataTable admin_datatable = new DataTable();
                    NpgsqlDataAdapter admin_sda = new NpgsqlDataAdapter(cmd);
                    admin_sda.Fill(admin_datatable);

                    gridView5.DataSource = admin_datatable; ;
                    gridView5.DataBind();
                }
                db.Close();
            }
        }


        protected void OperationalDispList()
        {
            using (var db = new NpgsqlConnection(con))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    // Modified the query to match the column names in the account_manager table
                    cmd.CommandText = "SELECT * FROM employee WHERE emp_status != 'Deleted' AND role_id = 5 AND emp_id != @id ORDER BY emp_id, emp_status";
                    cmd.Parameters.AddWithValue("@id", Convert.ToInt32(Session["id"]));

                    DataTable admin_datatable = new DataTable();
                    NpgsqlDataAdapter admin_sda = new NpgsqlDataAdapter(cmd);
                    admin_sda.Fill(admin_datatable);

                    gridView4.DataSource = admin_datatable; ;
                    gridView4.DataBind();
                }
                db.Close();
            }
        }


        protected void UpdateAdminInfo(object sender, EventArgs e)
        {
            int id;
            if (!int.TryParse(txtbxID.Text, out id))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
                    "Swal.fire({ icon: 'error', title: 'Invalid ID Format', text: 'Please enter a valid ID format.', background: '#f8d7da', confirmButtonColor: '#dc3545' });", true);
                return;
            }

            string firstname = txtbfirstname.Text;
            string mi = txtmi.Text;
            string lastname = txtLastname.Text;
            string contact = txtContact.Text;
            string email = txtEmail.Text;

            bool emailExists = false;
            bool isEmailSuspendedOrInactive = false;

            // Get selected role from the dropdown list
            int roleId = Convert.ToInt32(promoteddl.SelectedValue);
            string rolee = "Employee";

            byte[] uploadedImageData = null;
            if (FileUpload1.HasFile)
            {
                try
                {
                    string fileExtension = Path.GetExtension(FileUpload1.PostedFile.FileName).ToLower();
                    string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
                    if (allowedExtensions.Contains(fileExtension))
                    {
                        uploadedImageData = FileUpload1.FileBytes;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
                        "Swal.fire({ icon: 'error', title: 'Invalid File Type', text: 'Only image files are allowed (jpg, jpeg, png, gif).', background: '#f8d7da', confirmButtonColor: '#dc3545' });", true);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('Error uploading image: " + ex.Message + "')</script>");
                    return;
                }
            }

            // Connect to PostgreSQL
            using (var db = new NpgsqlConnection(con))
            {
                db.Open();

                // SQL query to check if the email exists in any relevant table and retrieve status
                string emailCheckQuery = @"
    SELECT cus_email AS email, cus_status AS status FROM customer WHERE cus_email = @emp_email
    UNION ALL
    SELECT emp_email AS email, emp_status AS status FROM employee WHERE emp_email = @emp_email";

                using (var cmd = new NpgsqlCommand(emailCheckQuery, db))
                {
                    cmd.Parameters.AddWithValue("@emp_email", email);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            emailExists = true;  // Email exists
                            string status = reader["status"].ToString().ToLower();

                            // Email is inactive or suspended
                            if (status == "inactive" || status == "suspend")
                            {
                                isEmailSuspendedOrInactive = true;
                                break;
                            }
                        }
                    }
                }

                if (emailExists && isEmailSuspendedOrInactive)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
                    "Swal.fire({ icon: 'info', title: 'Inactive or Suspended Account', text: 'The email is associated with an inactive or suspended account. Please use a different email.', background: '#e9ecef', confirmButtonColor: '#6c757d' });", true);
                    return;
                }

                string selectQuery = "SELECT * FROM employee WHERE emp_id = @id";
                using (var cmdSelect = new NpgsqlCommand(selectQuery, db))
                {
                    cmdSelect.Parameters.AddWithValue("@id", id);

                    string originalFirstname = null;
                    string originalMi = null;
                    string originalLastname = null;
                    string originalContact = null;
                    string originalEmail = null;
                    string originalPassword = null;
                    byte[] originalProfileImage = null;
                    int originalRoleId = 0;

                    using (var reader = cmdSelect.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            originalFirstname = reader["emp_fname"].ToString();
                            originalMi = reader["emp_mname"].ToString();
                            originalLastname = reader["emp_lname"].ToString();
                            originalContact = reader["emp_contact"].ToString();
                            originalEmail = reader["emp_email"].ToString();
                            originalPassword = reader["emp_password"].ToString();
                            originalProfileImage = reader["emp_profile"] as byte[];
                            originalRoleId = Convert.ToInt32(reader["role_id"]);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
                                    "Swal.fire({ icon: 'error', title: 'No Data Found', text: 'No data found for the specified ID.', background: '#f8d7da', confirmButtonColor: '#dc3545' });", true);
                            return;
                        }
                    }

                    var updateFields = new List<string>();
                    var updateParams = new List<NpgsqlParameter>();
                    var changes = new List<string>();

                    // Check and update each field (excluding password)
                    if (!string.IsNullOrEmpty(firstname) && firstname != originalFirstname)
                    {
                        updateFields.Add("emp_fname = @firstname");
                        updateParams.Add(new NpgsqlParameter("@firstname", firstname));
                        changes.Add($"First Name: {originalFirstname} → {firstname}");
                    }
                    if (!string.IsNullOrEmpty(mi) && mi != originalMi)
                    {
                        updateFields.Add("emp_mname = @mi");
                        updateParams.Add(new NpgsqlParameter("@mi", mi));
                        changes.Add($"Middle Initial: {originalMi} → {mi}");
                    }
                    if (!string.IsNullOrEmpty(lastname) && lastname != originalLastname)
                    {
                        updateFields.Add("emp_lname = @lastname");
                        updateParams.Add(new NpgsqlParameter("@lastname", lastname));
                        changes.Add($"Last Name: {originalLastname} → {lastname}");
                    }
                    if (!string.IsNullOrEmpty(contact) && contact != originalContact)
                    {
                        updateFields.Add("emp_contact = @contact");
                        updateParams.Add(new NpgsqlParameter("@contact", contact));
                        changes.Add($"Contact: {originalContact} → {contact}");
                    }
                    if (!string.IsNullOrEmpty(email) && email != originalEmail)
                    {
                        updateFields.Add("emp_email = @email");
                        updateParams.Add(new NpgsqlParameter("@email", email));
                        changes.Add($"Email: {originalEmail} → {email}");
                    }
                    if (uploadedImageData != null)
                    {
                        updateFields.Add("emp_profile = @profile");
                        updateParams.Add(new NpgsqlParameter("@profile", uploadedImageData));
                        changes.Add("Profile Picture: Updated");
                    }

                    // Update role if it has changed
                    if (roleId != originalRoleId)
                    {
                        updateFields.Add("role_id = @roleId");
                        updateParams.Add(new NpgsqlParameter("@roleId", roleId));
                        changes.Add($"Role: {originalRoleId} → {roleId}");
                    }

                    if (updateFields.Count > 0)
                    {
                        string updateQuery = $"UPDATE employee SET {string.Join(", ", updateFields)} WHERE emp_id = @id";
                        using (var cmdUpdate = new NpgsqlCommand(updateQuery, db))
                        {
                            cmdUpdate.Parameters.AddWithValue("@id", id);
                            cmdUpdate.Parameters.AddRange(updateParams.ToArray());

                            int updatedRows = cmdUpdate.ExecuteNonQuery();
                            if (updatedRows > 0)
                            {
                                string changeDetails = string.Join("\n", changes);
                                string subject = "Account Information Update Notification";
                                string body = $"Dear Staff,\n\nYour account information has been updated. Below are the details of the changes:\n\n{changeDetails}\n\nIf you did not request these changes, please contact support immediately.\n\nBest regards,\nThe Account Manager Team";

                                if (!string.IsNullOrEmpty(email) && email != originalEmail)
                                {
                                    Send_Email(originalEmail, subject, body);
                                    Send_Email(email, subject, body);
                                    AccountManList();
                                    BillinOfficerList();
                                    OperationalDispList();
                                    HaulerList();
                                }
                                else
                                {
                                    Send_Email(originalEmail, subject, body);
                                }
                                AccountManList();
                                BillinOfficerList();
                                OperationalDispList();
                                HaulerList();
                                ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
                                $"Swal.fire({{ icon: 'success', title: 'Update Successful', text: '{rolee} information updated successfully!', background: '#e9f7ef', confirmButtonColor: '#28a745' }});", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
                                $"Swal.fire({{ icon: 'error', title: 'Update Failed', text: '{rolee} information.', background: '#f8d7da', confirmButtonColor: '#dc3545' }});", true);
                            }
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
                        $"Swal.fire({{ icon: 'info', title: 'No Changes Detected', text: 'No changes detected in the {rolee} information.', background: '#e9ecef', confirmButtonColor: '#6c757d' }});", true);
                    }
                }
                db.Close();
            }
        }

        protected void btnResetPass_Click(object sender, EventArgs e)
        {
            string userEmail = txtEmail.Text; // Get the user's email
            string newPassword = GenerateRandomPassword(12); // Generate a new random password (or you can take input from the user)
            string hashedPassword = HashPassword(newPassword); // Hash the new password

            // Update the password in the database
            using (var db = new NpgsqlConnection(con))
            {
                db.Open();

                // SQL query to update the password of the user by their email
                string updatePasswordQuery = "UPDATE employee SET emp_password = @newPassword WHERE emp_email = @userEmail";

                using (var cmd = new NpgsqlCommand(updatePasswordQuery, db))
                {
                    cmd.Parameters.AddWithValue("@newPassword", hashedPassword); // Add hashed password to query
                    cmd.Parameters.AddWithValue("@userEmail", userEmail); // Add email parameter to identify the user

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        // Successfully updated password
                        ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
                            "Swal.fire({ icon: 'success', title: 'Password Reset', text: 'Password has been successfully reset.', background: '#e9f7ef', confirmButtonColor: '#28a745' });", true);
                    }
                    else
                    {
                        // Failure: Email not found or update failed
                        ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
                            "Swal.fire({ icon: 'error', title: 'Password Reset Failed', text: 'The email address was not found or there was an error.', background: '#f8d7da', confirmButtonColor: '#dc3545' });", true);
                    }
                }

                db.Close();
            }

            // Optionally, you can also send an email to the user with the new password
            string subject = "Your Password Has Been Reset";
            string body = $"Dear {txtbfirstname.Text},\n\n" +
                          $"Your password has been successfully reset. Your new login details are as follows:\n" +
                          $"Email: {txtEmail.Text}\n" +
                          $"New Password: {newPassword}\n\n" +
                          "Please log in and change your password after your first login.\n\n" +
                          "Best regards,\n" +
                          "The Support Team";

            Send_Email(txtEmail.Text, subject, body); // Function to send email with new password
        }



        private string GenerateRandomPassword(int length)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()";
            StringBuilder password = new StringBuilder();
            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                password.Append(validChars[random.Next(validChars.Length)]);
            }

            return password.ToString();
        }
        protected void submitBtn_Click(object sender, EventArgs e)
        {
            int adminId = (int)Session["am_id"];

            // Extracting user input
            string roleIdString = emp_role.SelectedValue; // Get selected role_id
            //string hashedPassword = HashPassword(emp_pass.Text);  // Hashing the password
            byte[] defaultImageData = File.ReadAllBytes(Server.MapPath("Pictures\\blank_prof.png"));  // Default profile image
            byte[] imageData = formFile.HasFile ? formFile.FileBytes : defaultImageData;  // Use uploaded image or default image
            string email = emp_email.Text;

            bool emailExists = false;
            bool isEmailSuspendedOrInactive = false;

            // Generate a random password if needed
            string randomPassword = GenerateRandomPassword(12); // You can specify the length here
            string randomPasswordHashed = HashPassword(randomPassword);  // Hashing the generated password

            // Email Message
            string toAddress = email;
            string subject = "Important: Your Login Credentials for Completing Registration";
            string body = $"Dear Staff and Good Day!,\n\n" +
                $"As a part of our onboarding process, we have generated your initial login credentials. Please use the following information to access the designated registration website and complete your profile:\n\n" +
                $"Email: {email}\n" +
                $"Password: {randomPassword}\n\n" +
                $"Visit the registration page on our main login page.\n\n" +
                $"Once you log in, kindly fill out the remaining information required to complete your registration. After completing this step, these credentials will serve as your permanent login information for daily use in our system.\n\n" +
                $"If you encounter any issues or have any questions, please do not hesitate to contact our support team.\n\n" +
                $"Best regards,\n" +
                $"The Account Manager Team\n" +
                $"[Company Name]";

            // Validation: Ensure all required fields are filled
            if (!string.IsNullOrEmpty(emp_firstname.Text) &&
                !string.IsNullOrEmpty(emp_lastname.Text) &&
                !string.IsNullOrEmpty(emp_email.Text) &&
                //!string.IsNullOrEmpty(emp_pass.Text) &&
                //!string.IsNullOrEmpty(emp_address.Text) &&
                !string.IsNullOrEmpty(emp_contact.Text) &&
                !string.IsNullOrEmpty(roleIdString) && // Validate that a role is selected
                roleIdString != "0") // Check if a valid role is selected
            {
                // Connect to PostgreSQL
                using (var db = new NpgsqlConnection(con))
                {
                    db.Open();

                    // SQL query to check if the email exists in any relevant table and retrieve status
                    string emailCheckQuery = @"
SELECT cus_email AS email, cus_status AS status FROM customer WHERE cus_email = @emp_email
UNION ALL
SELECT emp_email AS email, emp_status AS status FROM employee WHERE emp_email = @emp_email";

                    using (var cmd = new NpgsqlCommand(emailCheckQuery, db))
                    {
                        cmd.Parameters.AddWithValue("@emp_email", email);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                emailExists = true;  // Email exists
                                string status = reader["status"].ToString().ToLower();

                                if (status == "inactive" || status == "suspend")
                                {
                                    isEmailSuspendedOrInactive = true;
                                    break;
                                }
                            }
                        }
                    }

                    // If email exists and is suspended/inactive, prevent the addition of a new account manager
                    if (emailExists)
                    {
                        if (isEmailSuspendedOrInactive)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
                            "Swal.fire({ icon: 'info', title: 'Inactive or Suspended Account', text: 'The email is associated with an inactive or suspended account. Please use a different email.', background: '#e9ecef', confirmButtonColor: '#6c757d' });", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
                            "Swal.fire({ icon: 'info', title: 'Email Already Exists', text: 'The email already exists. Please use a different email.', background: '#e9ecef', confirmButtonColor: '#6c757d' });", true);
                        }
                        return;  // Exit the function if the email is invalid or already exists
                    }

                    // Validate roleId and proceed
                    int roleId = int.Parse(roleIdString); // Convert the selected value to an integer

                    // Proceed to insert the new Account Manager
                    using (var cmd = new NpgsqlCommand(
                        @"INSERT INTO employee 
                (emp_fname, emp_mname, emp_lname, emp_contact,  emp_email, emp_password, emp_profile, role_id, acc_id, emp_created_at, emp_updated_at, emp_otp) 
                VALUES (@emp_fname, @emp_mname, @emp_lname, @emp_contact, @emp_email, @emp_password, @emp_profile, @role_id, @acc_id, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, @emp_otp)", db))
                    {
                        // Adding parameters to prevent SQL injection
                        cmd.Parameters.AddWithValue("@emp_fname", emp_firstname.Text);
                        cmd.Parameters.AddWithValue("@emp_mname", emp_mi.Text);
                        cmd.Parameters.AddWithValue("@emp_lname", emp_lastname.Text);
                        cmd.Parameters.AddWithValue("@emp_contact", emp_contact.Text);
                        //cmd.Parameters.AddWithValue("@emp_address", emp_address.Text);  // Include employee address
                        cmd.Parameters.AddWithValue("@emp_email", email);
                        cmd.Parameters.AddWithValue("@emp_password", randomPasswordHashed);
                        cmd.Parameters.AddWithValue("@emp_profile", imageData);  // Profile image as byte array
                        cmd.Parameters.AddWithValue("@role_id", roleId);  // Insert role_id
                        cmd.Parameters.AddWithValue("@acc_id", adminId);  // Handle nullable acc_id
                        cmd.Parameters.AddWithValue("@emp_otp", (object)null ?? DBNull.Value);  // Handle nullable emp_otp

                        // Execute the query and check how many rows were affected
                        int ctr = cmd.ExecuteNonQuery();
                        if (ctr >= 1)
                        {
                            // Success: Account Manager added
                            ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
                            $"Swal.fire({{ icon: 'success', title: 'Adding Successful', text: 'Employee added successfully!', background: '#e9f7ef', confirmButtonColor: '#28a745' }});", true);
                            AccountManList();
                            BillinOfficerList();
                            OperationalDispList();
                            HaulerList();
                            Send_Email(toAddress, subject, body);  // Send the welcome email after successful insertion
                            ClearFormFields();
                        }
                        else
                        {
                            // Failure: Account Manager registration failed
                            ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
                            $"Swal.fire({{ icon: 'error', title: 'Failed Registration', text: 'Employee failed to Register!',  background: '#f8d7da', confirmButtonColor: '#dc3545'}});", true);
                        }
                    }

                    db.Close();
                }
            }
            else
            {
                // Validation error: Required fields are not filled
                ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
                $"Swal.fire({{ icon: 'info', title: 'Fill All Fields', text: 'Please fill up all the required fields!', background: '#e9ecef', confirmButtonColor: '#6c757d' }});", true);
            }
        }

        private void ClearFormFields()
        {
            emp_firstname.Text = "";
            emp_lastname.Text = "";
            emp_email.Text = "";
            //emp_pass.Text = "";
            //emp_address.Text = "";
            emp_contact.Text = "";
            emp_role.SelectedValue = "0"; // Reset the role dropdown to default or empty
            //formFile.Attributes.Clear(); // Clear file input
        }


        public static void Send_Email(string toAddress, string subject, string body)
        {

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential("official.trashtrack@gmail.com", "oojn qqna nkxb gmby");
            smtpClient.EnableSsl = true;
            smtpClient.Port = 587;

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("official.trashtrack@gmail.com");
            mailMessage.To.Add(toAddress);
            mailMessage.Subject = subject;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = false;

            smtpClient.Send(mailMessage);
        }




        protected void ClearForm_Click(object sender, EventArgs e)
        {
            emp_firstname.Text = "";
            emp_lastname.Text = "";
            emp_email.Text = "";
            //emp_pass.Text = "";
            //emp_address.Text = "";
            emp_contact.Text = "";
            emp_role.SelectedIndex = 0;
            //lblErrorMessage.Text = ""; // Clear any error messages
        }


        // Generate Salt for password security
        protected string GenerateSalt()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var saltChars = new char[16];
            for (int i = 0; i < saltChars.Length; i++)
            {
                saltChars[i] = chars[random.Next(chars.Length)];
            }
            return new string(saltChars);
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                var saltedPassword = password;
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));

                // Convert the hashed bytes to a hexadecimal string
                StringBuilder stringBuilder = new StringBuilder();
                foreach (byte b in hashedBytes)
                {
                    stringBuilder.Append(b.ToString("x2"));
                }

                return stringBuilder.ToString();
            }
        }

        private void LoadChangeRoles()
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(con))
            {
                string query = "SELECT role_id, role_name FROM roles ORDER BY role_id";
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

                try
                {
                    conn.Open();
                    NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Clear existing items and add the default "Select Role" option at the top
                    promoteddl.Items.Clear();
                    ListItem selectRoleItem = new ListItem("--Select Role--", "0");
                    selectRoleItem.Attributes.Add("disabled", "true"); // Disable the item
                    selectRoleItem.Attributes.Add("selected", "true"); // Set as selected
                    promoteddl.Items.Add(selectRoleItem);
                    promoteddl.Items.AddRange(dt.AsEnumerable()
                        .Select(row => new ListItem(row["role_name"].ToString(), row["role_id"].ToString()))
                        .ToArray());
                }
                catch (Exception ex)
                {
                    // Handle exception (logging, showing a message, etc.)
                }
            }
        }

        protected void Update_Click(object sender, EventArgs e)
        {
            LinkButton btn = sender as LinkButton;
            int id = Convert.ToInt32(btn.CommandArgument); // Get the employee ID from the button's CommandArgument

            try
            {
                using (var db = new NpgsqlConnection(con))
                {
                    db.Open();

                    // Query to get employee details along with the role_id
                    string query = @"
                SELECT emp_fname, emp_mname, emp_lname, emp_contact, emp_email, emp_profile, emp_address, role_id 
                FROM employee 
                WHERE emp_id = @acc_id";

                    using (var cmd = new NpgsqlCommand(query, db))
                    {
                        cmd.Parameters.AddWithValue("@acc_id", id);

                        // Execute the query
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read()) // Check if data is available for the given employee ID
                            {
                                // Assign the data to the respective textboxes
                                txtbfirstname.Text = reader["emp_fname"].ToString();
                                txtmi.Text = reader["emp_mname"].ToString();
                                txtLastname.Text = reader["emp_lname"].ToString();
                                txtContact.Text = reader["emp_contact"].ToString();
                                txtEmail.Text = reader["emp_email"].ToString();
                                txtAddress.Text = reader["emp_address"] == DBNull.Value ? "" : reader["emp_address"].ToString();

                                byte[] imageData = reader["emp_profile"] as byte[]; // Retrieve profile image data (byte array)

                                // Display profile image in the preview control
                                if (imagePreviewUpdate != null)
                                {
                                    if (imageData != null && imageData.Length > 0)
                                    {
                                        string base64String = Convert.ToBase64String(imageData);
                                        imagePreviewUpdate.ImageUrl = "data:image/jpeg;base64," + base64String; // Set image as base64 string
                                    }
                                    else
                                    {
                                        imagePreviewUpdate.ImageUrl = "~/Pictures/blank_prof.png"; // Default image if no profile picture found
                                    }
                                }

                                // Load roles into the dropdown
                                LoadChangeRoles();
                                // Set the selected role in the dropdown based on the role_id
                                int roleId = Convert.ToInt32(reader["role_id"]);
                                if (roleId == 1)
                                {
                                    hfActiveTab.Value = "#am"; // Set tab am as active
                                }
                                else if (roleId == 3)
                                {
                                    hfActiveTab.Value = "#bo"; // Set tab am as active
                                }
                                else if (roleId == 4)
                                {
                                    hfActiveTab.Value = "#hauler"; // Set tab am as active
                                }
                                else if (roleId == 5)
                                {
                                    hfActiveTab.Value = "#od"; // Set tab am as active
                                }
                                if (roleId > 0)
                                {
                                    promoteddl.SelectedValue = roleId.ToString(); // Set the selected value to the user's role
                                }
                            }
                            else
                            {
                                // Handle case when no data is found for the given employee ID
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
                                    "swal('Unsuccessful!', 'Employee not found.', 'error')", true);
                                return; // Exit if no data is found
                            }
                        }
                    }

                    db.Close();
                }
            }
            catch (Exception ex)
            {
                // Handle any errors
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
                    "swal('Unsuccessful!', '" + ex.Message + "', 'error')", true);
                return; // Exit if there was an error
            }

            // Set the ID textbox and show the modal popup
            txtbxID.Text = id.ToString();
            this.ModalPopupExtender2.Show(); // Show the modal popup

            // Optionally refresh the account manager list after the modal popup
            AccountManList();
        }



        public void GetAdminInfo(int admID)
        {
            try
            {
                // Connect to PostgreSQL
                using (var db = new NpgsqlConnection(con))
                {
                    db.Open();

                    // Define the SQL query to get the admin details based on the admin ID (acc_id)
                    string query = "SELECT emp_fname, emp_mname, emp_lname, emp_username, emp_contact, emp_email FROM employee WHERE emp_id = @acc_id";

                    using (var cmd = new NpgsqlCommand(query, db))
                    {
                        cmd.Parameters.AddWithValue("@acc_id", admID);

                        // Execute the query
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Assign the data to the respective textboxes
                                txtbfirstname.Text = reader["emp_fname"].ToString();
                                txtmi.Text = reader["emp_mname"].ToString();
                                txtLastname.Text = reader["emp_lname"].ToString();
                                //txtUsername.Text = reader["acc_username"].ToString();
                                txtContact.Text = reader["emp_contact"].ToString();
                                txtEmail.Text = reader["emp_email"].ToString();

                                AccountManList();
                            }
                            else
                            {
                                // Handle case when no data is found for the given admin ID
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
                                    "swal('Unsuccessful!', 'Admin not found.', 'error')", true);
                            }
                        }
                    }

                    db.Close();
                }
                AccountManList();
            }
            catch (Exception ex)
            {
                // Handle any errors
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
                    "swal('Unsuccessful!', '" + ex.Message + "', 'error')", true);
            }
        }


        protected void Suspend_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int managerId = Convert.ToInt32(btn.CommandArgument);

            try
            {
                using (var db = new NpgsqlConnection(con))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;

                        cmd.CommandText = "UPDATE employee SET emp_status = 'Suspend' WHERE emp_id = @id";
                        cmd.Parameters.AddWithValue("@id", managerId);
                        var ctr = cmd.ExecuteNonQuery();

                        if (ctr >= 1)
                        {
                            cmd.CommandText = "SELECT role_id FROM employee WHERE emp_id = @id";
                            int roleId = (int)cmd.ExecuteScalar();

                            cmd.CommandText = "SELECT role_name FROM roles WHERE role_id = @roleId";
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@roleId", roleId);

                            string roleName = (string)cmd.ExecuteScalar();
                            if (roleId == 1)
                            {
                                hfActiveTab.Value = "#am"; // Set tab am as active
                            }
                            else if (roleId == 3)
                            {
                                hfActiveTab.Value = "#bo"; // Set tab am as active
                            }
                            else if (roleId == 4)
                            {
                                hfActiveTab.Value = "#hauler"; // Set tab am as active
                            }
                            else if (roleId == 5)
                            {
                                hfActiveTab.Value = "#od"; // Set tab am as active
                            }
                            ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
                                $"Swal.fire({{ icon: 'success', title: 'Suspended!', text: '{roleName} Suspended Successfully!', background: '#e9f7ef', confirmButtonColor: '#28a745' }});",
                                true);

                            AccountManList();
                        }
                    }

                    // Refresh lists
                    AccountManList();
                    BillinOfficerList();
                    OperationalDispList();
                    HaulerList();
                    LoadProfile();
                    db.Close();
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
                    "swal('Unsuccessful!', '" + ex.Message + "', 'error')", true);
            }
        }




        // Unsuspension of the admin Action
        protected void Unsuspend_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int managerId = Convert.ToInt32(btn.CommandArgument);

            try
            {
                using (var db = new NpgsqlConnection(con))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;

                        cmd.CommandText = "UPDATE employee SET emp_status = 'Active' WHERE emp_id = @id";
                        cmd.Parameters.AddWithValue("@id", managerId);
                        var ctr = cmd.ExecuteNonQuery();

                        if (ctr >= 1)
                        {
                            cmd.CommandText = "SELECT role_id FROM employee WHERE emp_id = @id";
                            int roleId = (int)cmd.ExecuteScalar();
                            if (roleId == 1)
                            {
                                hfActiveTab.Value = "#am"; // Set tab am as active
                            }
                            else if (roleId == 3)
                            {
                                hfActiveTab.Value = "#bo"; // Set tab am as active
                            }
                            else if (roleId == 4)
                            {
                                hfActiveTab.Value = "#hauler"; // Set tab am as active
                            }
                            else if (roleId == 5)
                            {
                                hfActiveTab.Value = "#od"; // Set tab am as active
                            }
                            cmd.CommandText = "SELECT role_name FROM roles WHERE role_id = @roleId";
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@roleId", roleId);

                            string roleName = (string)cmd.ExecuteScalar();

                            ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
                                $"Swal.fire({{ icon: 'success', title: 'Activated!', text: '{roleName} Activated Successfully!', background: '#e9f7ef', confirmButtonColor: '#28a745' }});",
                                true);

                            AccountManList();
                        }
                    }

                    AccountManList();
                    BillinOfficerList();
                    OperationalDispList();
                    HaulerList();
                    LoadProfile();
                    db.Close();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
                    "swal('Unsuccessful!', '" + ex.Message + "', 'error')", true);
            }
        }


        // validate if the admin status is Suspend
        protected Boolean IsSuspended(string status)
        {
            return status == "Suspend";
        }

        // validate if the admin status is Unsuspend
        protected Boolean IsActive(string status)
        {
            return status == "Active";
        }

        // Deletion of the admin or update the status to Inactive if the admin is inactive anymore
        protected void Remove_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;

            int adminId = Convert.ToInt32(btn.CommandArgument);

            try
            {
                using (var db = new NpgsqlConnection(con))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;

                        cmd.CommandText = "UPDATE employee SET emp_status = 'Deleted' WHERE emp_id = @id";
                        cmd.Parameters.AddWithValue("@id", adminId);
                        var ctr = cmd.ExecuteNonQuery();

                        if (ctr >= 1)
                        {
                            cmd.CommandText = "SELECT role_id FROM employee WHERE emp_id = @id";
                            int roleId = (int)cmd.ExecuteScalar();
                            if (roleId == 1)
                            {
                                hfActiveTab.Value = "#am"; // Set tab am as active
                            }
                            else if (roleId == 3)
                            {
                                hfActiveTab.Value = "#bo"; // Set tab am as active
                            }
                            else if (roleId == 4)
                            {
                                hfActiveTab.Value = "#hauler"; // Set tab am as active
                            }
                            else if (roleId == 5)
                            {
                                hfActiveTab.Value = "#od"; // Set tab am as active
                            }
                            cmd.CommandText = "SELECT role_name FROM roles WHERE role_id = @roleId";
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@roleId", roleId);

                            string roleName = (string)cmd.ExecuteScalar();

                            ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
                                $"Swal.fire({{ icon: 'success', title: 'Account Removed!', text: '{roleName} Removed Successfully!', background: '#e9f7ef', confirmButtonColor: '#28a745' }});",
                                true);

                            AccountManList();
                        }
                    }

                    AccountManList();
                    BillinOfficerList();
                    OperationalDispList();
                    HaulerList();
                    LoadProfile();
                    db.Close();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
                    "swal('Unsuccessful!', '" + ex.Message + "', 'error')", true);
            }

        }


    }
}