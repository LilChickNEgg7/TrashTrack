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
    public partial class Dispatcher_AccountSettings : System.Web.UI.Page
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
            }
        }

        private void LoadProfile()
        {
            try
            {
                // Check if the session is active
                if (Session["od_id"] == null)
                {
                    Response.Redirect("LoginPage.aspx");
                    return;
                }

                int adminId = (int)Session["od_id"];  // Retrieve admin ID from session
                string roleName = (string)Session["od_rolename"];
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
            int samId = (int)Session["od_id"];

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

        protected void Button1_Click(object sender, EventArgs e)
        {
            int empId = (int)Session["od_id"];
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