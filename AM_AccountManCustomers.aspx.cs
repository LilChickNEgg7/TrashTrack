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
    public partial class AM_AccountManCustomers : System.Web.UI.Page
    {
        // Database Connection String
        private readonly string con = "Server=localhost;Port=5432;User Id=postgres;Password=123456;Database=trashtrack";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //emp_role.Items.FindByValue(string.Empty).Attributes.Add("disabled", "disabled");
                //LoadRoles();
                ContractList();
                //NonContractList();
                LoadProfile();
                //RequestsContractual();
            }
            if (IsPostBack && Request["__EVENTTARGET"] == "btnDecline")
            {
                string[] args = Request["__EVENTARGUMENT"].Split('|');
                if (args.Length == 2)
                {
                    int contId = Convert.ToInt32(args[0]);
                    string declineReason = args[1];

                    DeclineContract(contId, declineReason);
                }
            }
        }

        private void DeclineContract(int contId, string declineReason)
        {
            try
            {
                using (var db = new NpgsqlConnection(con))
                {
                    db.Open();

                    using (var cmd = db.CreateCommand())
                    {
                        // Update the contractual status to 'Declined' and insert the decline reason
                        cmd.CommandText = "UPDATE contractual SET cont_status = 'Declined', cont_faileddesc = @declineReason WHERE cont_id = @id";
                        cmd.Parameters.AddWithValue("@declineReason", declineReason);
                        cmd.Parameters.AddWithValue("@id", contId);
                        cmd.ExecuteNonQuery();
                    }
                    db.Close();
                }

                // Re-bind lists if necessary
                //RequestsContractual();

                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
                    "swal('Success!', 'Contract declined!', 'success')", true);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
                    "swal('Unsuccessful!', '" + ex.Message + "', 'error')", true);
            }
        }



        protected void gridView3_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Use the cont_id from the first cell (adjust as necessary)
                string contId = e.Row.Cells[0].Text;
                e.Row.Attributes["onclick"] = $"openModal('{contId}');"; // Set onclick to open the modal
                e.Row.Attributes["style"] = "cursor: pointer;"; // Change cursor to pointer for better UX
            }
        }

        //private void LoadRoles()
        //{
        //    using (NpgsqlConnection conn = new NpgsqlConnection(con))
        //    {
        //        string query = "SELECT role_id, role_name FROM roles ORDER BY role_id";
        //        NpgsqlCommand cmd = new NpgsqlCommand(query, conn);

        //        try
        //        {
        //            conn.Open();
        //            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
        //            DataTable dt = new DataTable();
        //            da.Fill(dt);

        //            emp_role.DataSource = dt;
        //            emp_role.DataTextField = "role_name";
        //            emp_role.DataValueField = "role_id";
        //            emp_role.DataBind();

        //            // Clear existing items and add the default "Select Role" option at the top
        //            emp_role.Items.Clear();
        //            ListItem selectRoleItem = new ListItem("--Select Role--", "0");
        //            selectRoleItem.Attributes.Add("disabled", "true"); // Disable the item
        //            selectRoleItem.Attributes.Add("selected", "true"); // Set as selected
        //            emp_role.Items.Add(selectRoleItem);
        //            emp_role.Items.AddRange(dt.AsEnumerable().Select(row => new ListItem(row["role_name"].ToString(), row["role_id"].ToString())).ToArray());
        //        }
        //        catch (Exception ex)
        //        {
        //            // Handle exception (logging, showing a message, etc.)
        //        }
        //    }
        //}



        // Function to retrieve data from PostgreSQL
        private DataTable GetAccountManagers()
        {
            DataTable dt = new DataTable();
            using (NpgsqlConnection conn = new NpgsqlConnection(con))
            {
                conn.Open();
                string query = "SELECT emp_id, emp_fname, emp_mname, emp_lname, emp_contact, emp_email, emp_created_at, emp_updated_at, emp_status FROM employee";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                {
                    using (NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
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


        protected void Accept_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            int contId = Convert.ToInt32(btn.CommandArgument);

            try
            {
                using (var db = new NpgsqlConnection(con))
                {
                    db.Open();

                    // Start a transaction
                    using (var transaction = db.BeginTransaction())
                    {
                        using (var cmd = db.CreateCommand())
                        {
                            // Update the contractual status to 'Accepted'
                            cmd.CommandText = "UPDATE contractual SET cont_status = 'Accepted' WHERE cont_id = @id";
                            cmd.Parameters.AddWithValue("@id", contId);
                            int ctr = cmd.ExecuteNonQuery();

                            if (ctr >= 1)
                            {
                                // Get the cus_id associated with the cont_id
                                cmd.CommandText = "SELECT cus_id FROM contractual WHERE cont_id = @id";
                                cmd.Parameters.Clear(); // Clear previous parameters
                                cmd.Parameters.AddWithValue("@id", contId); // Use the same parameter name for SELECT
                                var cusId = cmd.ExecuteScalar();

                                if (cusId != null)
                                {
                                    // Update the customer type to 'Contractual'
                                    cmd.CommandText = "UPDATE customer SET cus_type = 'Contractual' WHERE cus_id = @cusId";
                                    cmd.Parameters.Clear(); // Clear previous parameters again
                                    cmd.Parameters.AddWithValue("@cusId", (int)cusId);
                                    cmd.ExecuteNonQuery();
                                }

                                // Commit the transaction
                                transaction.Commit();

                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
                                    "swal('Success!', 'Contract accepted and customer type updated!', 'success')", true);

                                // Re-bind lists if necessary
                                ContractList();
                                //NonContractList();
                                //RequestsContractual();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // In case of error, rollback transaction
                //try
                //{
                //    transaction.Rollback();
                //}
                //catch
                //{
                //    // Handle rollback error
                //}

                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
                    "swal('Unsuccessful!', '" + ex.Message + "', 'error')", true);
            }
        }



        protected void Decline_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            int contId = Convert.ToInt32(btn.CommandArgument);

            try
            {
                using (var db = new NpgsqlConnection(con))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "UPDATE contractual SET cont_status = 'Declined' WHERE cont_id = @id";
                        cmd.Parameters.AddWithValue("@id", contId);

                        var ctr = cmd.ExecuteNonQuery();
                        if (ctr >= 1)
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
                                "swal('Suspended!', 'Account Manager Suspended Successfully!', 'success')", true);
                            ContractList();
                            //NonContractList();
                            //RequestsContractual();
                        }
                    }
                    db.Close();
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
                    "swal('Unsuccessful!', '" + ex.Message + "', 'error')", true);
            }
        }


        protected void ContractList()
        {
            using (var db = new NpgsqlConnection(con))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    // Modified the query to match the column names in the account_manager table
                    cmd.CommandText = "SELECT * FROM customer WHERE cus_status != 'Deleted' ORDER BY cus_id, cus_status";

                    // Ensure the parameter type is correct (assuming emp_id is an integer)
                    //cmd.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Integer, Convert.ToInt32(Session["id"]));

                    // Execute the query and bind to the GridView
                    DataTable admin_datatable = new DataTable();
                    NpgsqlDataAdapter admin_sda = new NpgsqlDataAdapter(cmd);
                    admin_sda.Fill(admin_datatable);

                    gridView1.DataSource = admin_datatable;
                    gridView1.DataBind();
                }
            }
        }




        //protected void NonContractList()
        //{
        //    using (var db = new NpgsqlConnection(con))
        //    {
        //        db.Open();
        //        using (var cmd = db.CreateCommand())
        //        {
        //            cmd.CommandType = CommandType.Text;
        //            // Modified the query to match the column names in the account_manager table
        //            cmd.CommandText = "SELECT * FROM customer WHERE cus_status != 'Deleted' AND cus_type = 'Non-Contractual' ORDER BY cus_id, cus_status";
        //            //cmd.Parameters.AddWithValue("@id", Convert.ToInt32(Session["id"]));

        //            DataTable admin_datatable = new DataTable();
        //            NpgsqlDataAdapter admin_sda = new NpgsqlDataAdapter(cmd);
        //            admin_sda.Fill(admin_datatable);

        //            gridView2.DataSource = admin_datatable; ;
        //            gridView2.DataBind();
        //        }
        //        db.Close();
        //    }
        //}


        //protected void RequestsContractual()
        //{
        //    using (var db = new NpgsqlConnection(con))
        //    {
        //        db.Open();
        //        using (var cmd = db.CreateCommand())
        //        {
        //            cmd.CommandType = CommandType.Text;
        //            // Modified the query to match the column names in the account_manager table
        //            cmd.CommandText = "SELECT * FROM contractual WHERE cont_status != 'Deleted' AND cont_status != 'Accepted' AND cont_status != 'Declined' ORDER BY cont_created_at, cont_status";
        //            //cmd.Parameters.AddWithValue("@id", Convert.ToInt32(Session["id"]));

        //            DataTable admin_datatable = new DataTable();
        //            NpgsqlDataAdapter admin_sda = new NpgsqlDataAdapter(cmd);
        //            admin_sda.Fill(admin_datatable);

        //            gridView3.DataSource = admin_datatable; ;
        //            gridView3.DataBind();
        //        }
        //        db.Close();
        //    }
        //}

        //protected void UpdateAdminInfo(object sender, EventArgs e)
        //{
        //    int id;
        //    if (!int.TryParse(txtbxID.Text, out id))
        //    {
        //        ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
        //            "Swal.fire({ icon: 'error', title: 'Invalid ID Format', text: 'Please enter a valid ID format.', background: '#f8d7da', confirmButtonColor: '#dc3545' });", true);
        //        return;
        //    }

        //    string firstname = txtbfirstname.Text;
        //    string mi = txtmi.Text;
        //    string lastname = txtLastname.Text;
        //    string contact = txtContact.Text;
        //    string email = txtEmail.Text;
        //    string pass = TextBox1.Text;

        //    // Get selected role from the dropdown list
        //    int roleId = Convert.ToInt32(promoteddl.SelectedValue);
        //    string rolee = "Employee";

        //    byte[] uploadedImageData = null;
        //    if (FileUpload1.HasFile)
        //    {
        //        try
        //        {
        //            string fileExtension = Path.GetExtension(FileUpload1.PostedFile.FileName).ToLower();
        //            string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
        //            if (allowedExtensions.Contains(fileExtension))
        //            {
        //                uploadedImageData = FileUpload1.FileBytes;
        //            }
        //            else
        //            {
        //                ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
        //                "Swal.fire({ icon: 'error', title: 'Invalid File Type', text: 'Only image files are allowed (jpg, jpeg, png, gif).', background: '#f8d7da', confirmButtonColor: '#dc3545' });", true);
        //                return;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Response.Write("<script>alert('Error uploading image: " + ex.Message + "')</script>");
        //            return;
        //        }
        //    }

        //    using (var db = new NpgsqlConnection(con))
        //    {
        //        db.Open();

        //        // Check if the email already exists (excluding the current admin's email)
        //        string emailCheckQuery = "SELECT COUNT(*) FROM employee WHERE emp_email = @newEmail AND emp_id <> @id";
        //        using (var cmdCheckEmail = new NpgsqlCommand(emailCheckQuery, db))
        //        {
        //            cmdCheckEmail.Parameters.AddWithValue("@newEmail", email);
        //            cmdCheckEmail.Parameters.AddWithValue("@id", id);

        //            int emailExists = Convert.ToInt32(cmdCheckEmail.ExecuteScalar());
        //            if (emailExists > 0)
        //            {
        //                ScriptManager.RegisterStartupScript(this, GetType(), "showAlert", "Swal.fire({ icon: 'error', title: 'Email Taken', text: 'Email is already taken. Please use a different email.', background: '#f8d7da', confirmButtonColor: '#dc3545' });", true);
        //                return;
        //            }
        //        }

        //        // Get current data for the admin based on the ID
        //        string selectQuery = "SELECT * FROM employee WHERE emp_id = @id";
        //        using (var cmdSelect = new NpgsqlCommand(selectQuery, db))
        //        {
        //            cmdSelect.Parameters.AddWithValue("@id", id);

        //            string originalFirstname = null;
        //            string originalMi = null;
        //            string originalLastname = null;
        //            string originalContact = null;
        //            string originalEmail = null;
        //            string originalPassword = null;
        //            byte[] originalProfileImage = null;
        //            int originalRoleId = 0;

        //            using (var reader = cmdSelect.ExecuteReader())
        //            {
        //                if (reader.Read())
        //                {
        //                    originalFirstname = reader["emp_fname"].ToString();
        //                    originalMi = reader["emp_mname"].ToString();
        //                    originalLastname = reader["emp_lname"].ToString();
        //                    originalContact = reader["emp_contact"].ToString();
        //                    originalEmail = reader["emp_email"].ToString();
        //                    originalPassword = reader["emp_password"].ToString();
        //                    originalProfileImage = reader["emp_profile"] as byte[];
        //                    originalRoleId = Convert.ToInt32(reader["role_id"]); // Get the current role ID
        //                }
        //                else
        //                {
        //                    ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
        //                            "Swal.fire({ icon: 'error', title: 'No Data Found', text: 'No data found for the specified ID.', background: '#f8d7da', confirmButtonColor: '#dc3545' });", true);
        //                    return;
        //                }
        //            }

        //            var updateFields = new List<string>();
        //            var updateParams = new List<NpgsqlParameter>();
        //            var changes = new List<string>();

        //            // Check and update each field
        //            if (!string.IsNullOrEmpty(firstname) && firstname != originalFirstname)
        //            {
        //                updateFields.Add("emp_fname = @firstname");
        //                updateParams.Add(new NpgsqlParameter("@firstname", firstname));
        //                changes.Add($"First Name: {originalFirstname} → {firstname}");
        //            }
        //            if (!string.IsNullOrEmpty(mi) && mi != originalMi)
        //            {
        //                updateFields.Add("emp_mname = @mi");
        //                updateParams.Add(new NpgsqlParameter("@mi", mi));
        //                changes.Add($"Middle Initial: {originalMi} → {mi}");
        //            }
        //            if (!string.IsNullOrEmpty(lastname) && lastname != originalLastname)
        //            {
        //                updateFields.Add("emp_lname = @lastname");
        //                updateParams.Add(new NpgsqlParameter("@lastname", lastname));
        //                changes.Add($"Last Name: {originalLastname} → {lastname}");
        //            }
        //            if (!string.IsNullOrEmpty(contact) && contact != originalContact)
        //            {
        //                updateFields.Add("emp_contact = @contact");
        //                updateParams.Add(new NpgsqlParameter("@contact", contact));
        //                changes.Add($"Contact: {originalContact} → {contact}");
        //            }
        //            if (!string.IsNullOrEmpty(email) && email != originalEmail)
        //            {
        //                updateFields.Add("emp_email = @email");
        //                updateParams.Add(new NpgsqlParameter("@email", email));
        //                changes.Add($"Email: {originalEmail} → {email}");
        //            }
        //            if (!string.IsNullOrEmpty(pass) && pass != originalPassword)
        //            {
        //                string hashedPassword = HashPassword(pass);
        //                updateFields.Add("emp_password = @password");
        //                updateParams.Add(new NpgsqlParameter("@password", hashedPassword));
        //                changes.Add($"Password: (Updated)"); // Change details might be better to mention as 'Updated'
        //            }
        //            if (uploadedImageData != null)
        //            {
        //                updateFields.Add("emp_profile = @profile");
        //                updateParams.Add(new NpgsqlParameter("@profile", uploadedImageData));
        //                changes.Add("Profile Picture: Updated");
        //            }

        //            // Update role if it has changed
        //            if (roleId != originalRoleId)
        //            {
        //                updateFields.Add("role_id = @roleId");
        //                updateParams.Add(new NpgsqlParameter("@roleId", roleId));
        //                changes.Add($"Role: {originalRoleId} → {roleId}"); // Adding change to changes list
        //            }
        //            //Response.Write("<script>alert('" + updateFields + "');</script>");
        //            //Response.Write("<script>alert('" + updateFields.Count + "');</script>");

        //            if (updateFields.Count > 0)
        //            {


        //                string updateQuery = $"UPDATE employee SET {string.Join(", ", updateFields)} WHERE emp_id = @id";
        //                using (var cmdUpdate = new NpgsqlCommand(updateQuery, db))
        //                {
        //                    cmdUpdate.Parameters.AddWithValue("@id", id);
        //                    cmdUpdate.Parameters.AddRange(updateParams.ToArray());

        //                    int updatedRows = cmdUpdate.ExecuteNonQuery();
        //                    if (updatedRows > 0)
        //                    {
        //                        // Notify changes via email
        //                        string changeDetails = string.Join("\n", changes);
        //                        string subject = "Account Information Update Notification";
        //                        string body = $"Dear Staff,\n\nYour account information has been updated. Below are the details of the changes:\n\n{changeDetails}\n\nIf you did not request these changes, please contact support immediately.\n\nBest regards,\nThe Account Manager Team";

        //                        if (!string.IsNullOrEmpty(email) && email != originalEmail)
        //                        {
        //                            Send_Email(originalEmail, subject, body);
        //                            Send_Email(email, subject, body);
        //                        }
        //                        else
        //                        {
        //                            Send_Email(originalEmail, subject, body);
        //                        }
        //                        ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
        //                        $"Swal.fire({{ icon: 'success', title: 'Update Successful', text: '{rolee} information updated successfully!', background: '#e9f7ef', confirmButtonColor: '#28a745' }});", true);

        //                        AccountManList();
        //                        SupAccountManList();
        //                        BillinOfficerList();
        //                        OperationalDispList();
        //                        HaulerList();
        //                    }
        //                    else
        //                    {
        //                        ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
        //                        $"Swal.fire({{ icon: 'error', title: 'Update Failed', text: '{rolee} information.', background: '#f8d7da', confirmButtonColor: '#dc3545' }});", true);
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
        //                $"Swal.fire({{ icon: 'info', title: 'No Changes Detected', text: 'No changes detected in the {rolee} information.', background: '#e9ecef', confirmButtonColor: '#6c757d' }});", true);
        //            }

        //        }
        //        db.Close();
        //    }
        //}

        //without scriptmanager and confusing but latest
        //protected void UpdateCustomerInfo(object sender, EventArgs e)
        //{
        //    int id;
        //    if (!int.TryParse(txtbxID.Text, out id))
        //    {
        //        ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
        //            "Swal.fire({ icon: 'error', title: 'Invalid ID Format', text: 'Please enter a valid ID format.', background: '#f8d7da', confirmButtonColor: '#dc3545' });", true);
        //        return;
        //    }

        //    string firstname = txtbfirstname.Text;
        //    string mi = txtmi.Text;
        //    string lastname = txtLastname.Text;
        //    string contact = txtContact.Text;
        //    string email = txtEmail.Text;
        //    string pass = TextBox1.Text;

        //    byte[] uploadedImageData = null;
        //    if (FileUpload1.HasFile)
        //    {
        //        try
        //        {
        //            string fileExtension = Path.GetExtension(FileUpload1.PostedFile.FileName).ToLower();
        //            string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
        //            if (allowedExtensions.Contains(fileExtension))
        //            {
        //                uploadedImageData = FileUpload1.FileBytes;
        //            }
        //            else
        //            {
        //                ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
        //                $"Swal.fire({{ icon: 'info', title: 'Invalid File Type', text: 'Only image files are allowed (jpg, jpeg, png, gif).', background: '#e9ecef', confirmButtonColor: '#6c757d' }});", true);

        //                return;

        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
        //                        $"Swal.fire({{ icon: 'error', title: 'Upload Failed', text: 'Error uploading image.', background: '#f8d7da', confirmButtonColor: '#dc3545' }});", true);
        //            //Response.Write("<script>alert('Error uploading image: " + ex.Message + "')</script>");
        //            return;
        //        }
        //    }

        //    using (var db = new NpgsqlConnection(con))
        //    {
        //        db.Open();

        //        // Check if the email already exists (excluding the current admin's email)
        //        string emailCheckQuery = "SELECT COUNT(*) FROM customer WHERE cus_email = @newEmail AND cus_id <> @id";
        //        using (var cmdCheckEmail = new NpgsqlCommand(emailCheckQuery, db))
        //        {
        //            cmdCheckEmail.Parameters.AddWithValue("@newEmail", email);
        //            cmdCheckEmail.Parameters.AddWithValue("@id", id);

        //            int emailExists = Convert.ToInt32(cmdCheckEmail.ExecuteScalar());
        //            if (emailExists > 0)
        //            {
        //                ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
        //                $"Swal.fire({{ icon: 'info', title: 'Email Taken', text: 'Email is already taken. Please use a different email.', background: '#e9ecef', confirmButtonColor: '#6c757d' }});", true);
        //                //Response.Write("<script>alert('Email is already taken. Please use a different email.')</script>");
        //                return;
        //            }
        //        }

        //        // Get current data for the admin based on the ID
        //        string selectQuery = "SELECT * FROM customer WHERE cus_id = @id";
        //        using (var cmdSelect = new NpgsqlCommand(selectQuery, db))
        //        {
        //            cmdSelect.Parameters.AddWithValue("@id", id);

        //            string originalFirstname = null;
        //            string originalMi = null;
        //            string originalLastname = null;
        //            string originalContact = null;
        //            string originalEmail = null;
        //            string originalPassword = null;
        //            byte[] originalProfileImage = null;

        //            using (var reader = cmdSelect.ExecuteReader())
        //            {
        //                if (reader.Read())
        //                {
        //                    originalFirstname = reader["cus_fname"].ToString();
        //                    originalMi = reader["cus_mname"].ToString();
        //                    originalLastname = reader["cus_lname"].ToString();
        //                    originalContact = reader["cus_contact"].ToString();
        //                    originalEmail = reader["cus_email"].ToString();
        //                    originalPassword = reader["cus_password"].ToString();
        //                    originalProfileImage = reader["cus_profile"] as byte[];
        //                }
        //                else
        //                {
        //                    ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
        //                        $"Swal.fire({{ icon: 'error', title: 'Info Not Found', text: 'No data found for the specified ID.', background: '#f8d7da', confirmButtonColor: '#dc3545' }});", true);
        //                    //Response.Write("<script>alert('No data found for the specified ID.')</script>");
        //                    return;
        //                }
        //            }

        //            var updateFields = new List<string>();
        //            var updateParams = new List<NpgsqlParameter>();
        //            var changes = new List<string>();

        //            // Check and update each field
        //            if (!string.IsNullOrEmpty(firstname) && firstname != originalFirstname)
        //            {
        //                updateFields.Add("cus_fname = @firstname");
        //                updateParams.Add(new NpgsqlParameter("@firstname", firstname));
        //                changes.Add($"First Name: {originalFirstname} → {firstname}");
        //            }
        //            if (!string.IsNullOrEmpty(mi) && mi != originalMi)
        //            {
        //                updateFields.Add("cus_mname = @mi");
        //                updateParams.Add(new NpgsqlParameter("@mi", mi));
        //                changes.Add($"Middle Initial: {originalMi} → {mi}");
        //            }
        //            if (!string.IsNullOrEmpty(lastname) && lastname != originalLastname)
        //            {
        //                updateFields.Add("cus_lname = @lastname");
        //                updateParams.Add(new NpgsqlParameter("@lastname", lastname));
        //                changes.Add($"Last Name: {originalLastname} → {lastname}");
        //            }
        //            if (!string.IsNullOrEmpty(contact) && contact != originalContact)
        //            {
        //                updateFields.Add("cus_contact = @contact");
        //                updateParams.Add(new NpgsqlParameter("@contact", contact));
        //                changes.Add($"Contact: {originalContact} → {contact}");
        //            }
        //            if (!string.IsNullOrEmpty(email) && email != originalEmail)
        //            {
        //                updateFields.Add("cus_email = @email");
        //                updateParams.Add(new NpgsqlParameter("@email", email));
        //                changes.Add($"Email: {originalEmail} → {email}");
        //            }
        //            if (!string.IsNullOrEmpty(pass) && pass != originalPassword)
        //            {
        //                string hashedPassword = HashPassword(pass);
        //                updateFields.Add("cus_password = @password");
        //                updateParams.Add(new NpgsqlParameter("@password", hashedPassword));
        //                changes.Add("Password: (Updated)");
        //            }

        //            if (uploadedImageData != null)
        //            {
        //                updateFields.Add("cus_profile = @profile");
        //                updateParams.Add(new NpgsqlParameter("@profile", uploadedImageData));
        //                changes.Add("Profile Picture: Updated");
        //            }

        //            if (updateFields.Count > 0)
        //            {
        //                string updateQuery = $"UPDATE customer SET {string.Join(", ", updateFields)} WHERE cus_id = @id";
        //                using (var cmdUpdate = new NpgsqlCommand(updateQuery, db))
        //                {
        //                    cmdUpdate.Parameters.AddWithValue("@id", id);
        //                    cmdUpdate.Parameters.AddRange(updateParams.ToArray());

        //                    int updatedRows = cmdUpdate.ExecuteNonQuery();
        //                    if (updatedRows > 0)
        //                    {
        //                        // Notify changes via email
        //                        string changeDetails = string.Join("\n", changes);
        //                        string subject = "Account Information Update Notification";
        //                        string body = $"Dear Admin,\n\nYour account information has been updated. Below are the details of the changes:\n\n{changeDetails}\n\nIf you did not request these changes, please contact support immediately.\n\nBest regards,\nThe Account Manager Team";

        //                        if (!string.IsNullOrEmpty(email) && email != originalEmail)
        //                        {
        //                            Send_Email(originalEmail, subject, body);
        //                            Send_Email(email, subject, body);
        //                        }
        //                        else
        //                        {
        //                            Send_Email(originalEmail, subject, body);
        //                        }
        //                        ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
        //                        $"Swal.fire({{ icon: 'success', title: 'Customer Update Success', text: 'Customer information updated successfully!', background: '#e9f7ef', confirmButtonColor: '#28a745' }});", true);

        //                        //Response.Write("<script>alert('Customer information updated successfully!')</script>");
        //                    }
        //                    else
        //                    {
        //                        ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
        //                        $"Swal.fire({{ icon: 'error', title: 'Customer Update Failed: 'Failed to update customer information.', background: '#f8d7da', confirmButtonColor: '#dc3545' }});", true);
        //                        //Response.Write("<script>alert('Failed to update customer information.')</script>");
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
        //                $"Swal.fire({{ icon: 'info', title: 'No Changes Detected', text: 'No changes detected in the customer information.', background: '#e9ecef', confirmButtonColor: '#6c757d' }});", true);

        //            }
        //        }
        //    }
        //}

        //protected void UpdateCustomerInfo(object sender, EventArgs e)
        //{
        //    int id;
        //    if (!int.TryParse(txtbxID.Text, out id))
        //    {
        //        ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
        //            "Swal.fire({ icon: 'error', title: 'Invalid ID Format', text: 'Please enter a valid ID format.', background: '#f8d7da', confirmButtonColor: '#dc3545' });", true);
        //        return;
        //    }

        //    string firstname = txtbfirstname.Text;
        //    string mi = txtmi.Text;
        //    string lastname = txtLastname.Text;
        //    string contact = txtContact.Text;
        //    string email = txtEmail.Text;
        //    string pass = TextBox1.Text;

        //    byte[] uploadedImageData = null;
        //    if (FileUpload1.HasFile)
        //    {
        //        try
        //        {
        //            string fileExtension = Path.GetExtension(FileUpload1.PostedFile.FileName).ToLower();
        //            string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
        //            if (allowedExtensions.Contains(fileExtension))
        //            {
        //                uploadedImageData = FileUpload1.FileBytes;
        //            }
        //            else
        //            {
        //                ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
        //                $"Swal.fire({{ icon: 'info', title: 'Invalid File Type', text: 'Only image files are allowed (jpg, jpeg, png, gif).', background: '#e9ecef', confirmButtonColor: '#6c757d' }});", true);
        //                return;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
        //                        $"Swal.fire({{ icon: 'error', title: 'Upload Failed', text: 'Error uploading image.', background: '#f8d7da', confirmButtonColor: '#dc3545' }});", true);
        //            return;
        //        }
        //    }

        //    using (var db = new NpgsqlConnection(con))
        //    {
        //        db.Open();

        //        // Check if the email already exists (excluding the current customer's email)
        //        string emailCheckQuery = "SELECT COUNT(*) FROM customer WHERE cus_email = @newEmail AND cus_id <> @id";
        //        using (var cmdCheckEmail = new NpgsqlCommand(emailCheckQuery, db))
        //        {
        //            cmdCheckEmail.Parameters.AddWithValue("@newEmail", email);
        //            cmdCheckEmail.Parameters.AddWithValue("@id", id);

        //            int emailExists = Convert.ToInt32(cmdCheckEmail.ExecuteScalar());
        //            if (emailExists > 0)
        //            {
        //                ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
        //                $"Swal.fire({{ icon: 'info', title: 'Email Taken', text: 'Email is already taken. Please use a different email.', background: '#e9ecef', confirmButtonColor: '#6c757d' }});", true);
        //                return;
        //            }
        //        }

        //        // Get current data for the customer based on the ID
        //        string selectQuery = "SELECT * FROM customer WHERE cus_id = @id";
        //        using (var cmdSelect = new NpgsqlCommand(selectQuery, db))
        //        {
        //            cmdSelect.Parameters.AddWithValue("@id", id);

        //            string originalFirstname = null;
        //            string originalMi = null;
        //            string originalLastname = null;
        //            string originalContact = null;
        //            string originalEmail = null;
        //            string originalPassword = null;
        //            byte[] originalProfileImage = null;

        //            using (var reader = cmdSelect.ExecuteReader())
        //            {
        //                if (reader.Read())
        //                {
        //                    originalFirstname = reader["cus_fname"].ToString();
        //                    originalMi = reader["cus_mname"].ToString();
        //                    originalLastname = reader["cus_lname"].ToString();
        //                    originalContact = reader["cus_contact"].ToString();
        //                    originalEmail = reader["cus_email"].ToString();
        //                    originalPassword = reader["cus_password"].ToString();
        //                    originalProfileImage = reader["cus_profile"] as byte[];
        //                }
        //                else
        //                {
        //                    ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
        //                        $"Swal.fire({{ icon: 'error', title: 'Info Not Found', text: 'No data found for the specified ID.', background: '#f8d7da', confirmButtonColor: '#dc3545' }});", true);
        //                    return;
        //                }
        //            }

        //            var updateFields = new List<string>();
        //            var updateParams = new List<NpgsqlParameter>();
        //            var changes = new List<string>();

        //            // Check and update each field
        //            if (!string.IsNullOrEmpty(firstname) && firstname != originalFirstname)
        //            {
        //                updateFields.Add("cus_fname = @firstname");
        //                updateParams.Add(new NpgsqlParameter("@firstname", firstname));
        //                changes.Add($"First Name: {originalFirstname} → {firstname}");
        //            }
        //            if (!string.IsNullOrEmpty(mi) && mi != originalMi)
        //            {
        //                updateFields.Add("cus_mname = @mi");
        //                updateParams.Add(new NpgsqlParameter("@mi", mi));
        //                changes.Add($"Middle Initial: {originalMi} → {mi}");
        //            }
        //            if (!string.IsNullOrEmpty(lastname) && lastname != originalLastname)
        //            {
        //                updateFields.Add("cus_lname = @lastname");
        //                updateParams.Add(new NpgsqlParameter("@lastname", lastname));
        //                changes.Add($"Last Name: {originalLastname} → {lastname}");
        //            }
        //            if (!string.IsNullOrEmpty(contact) && contact != originalContact)
        //            {
        //                updateFields.Add("cus_contact = @contact");
        //                updateParams.Add(new NpgsqlParameter("@contact", contact));
        //                changes.Add($"Contact: {originalContact} → {contact}");
        //            }
        //            if (!string.IsNullOrEmpty(email) && email != originalEmail)
        //            {
        //                updateFields.Add("cus_email = @email");
        //                updateParams.Add(new NpgsqlParameter("@email", email));
        //                changes.Add($"Email: {originalEmail} → {email}");
        //            }
        //            if (!string.IsNullOrEmpty(pass) && pass != originalPassword)
        //            {
        //                string hashedPassword = HashPassword(pass);
        //                updateFields.Add("cus_password = @password");
        //                updateParams.Add(new NpgsqlParameter("@password", hashedPassword));
        //                changes.Add("Password: (Updated)");
        //            }

        //            if (uploadedImageData != null)
        //            {
        //                updateFields.Add("cus_profile = @profile");
        //                updateParams.Add(new NpgsqlParameter("@profile", uploadedImageData));
        //                changes.Add("Profile Picture: Updated");
        //            }

        //            if (updateFields.Count > 0)
        //            {
        //                string updateQuery = $"UPDATE customer SET {string.Join(", ", updateFields)} WHERE cus_id = @id";
        //                using (var cmdUpdate = new NpgsqlCommand(updateQuery, db))
        //                {
        //                    cmdUpdate.Parameters.AddWithValue("@id", id);
        //                    cmdUpdate.Parameters.AddRange(updateParams.ToArray());

        //                    int updatedRows = cmdUpdate.ExecuteNonQuery();
        //                    if (updatedRows > 0)
        //                    {
        //                        // Notify changes via email
        //                        string changeDetails = string.Join("\n", changes);
        //                        string subject = "Account Information Update Notification";
        //                        string body = $"Dear Customer,\n\nYour account information has been updated. Below are the details of the changes:\n\n{changeDetails}\n\nIf you did not request these changes, please contact support immediately.\n\nBest regards,\nThe Customer Support Team";

        //                        if (!string.IsNullOrEmpty(email) && email != originalEmail)
        //                        {
        //                            Send_Email(originalEmail, subject, body);
        //                            Send_Email(email, subject, body);
        //                        }
        //                        else
        //                        {
        //                            Send_Email(originalEmail, subject, body);
        //                        }
        //                        ContractList();
        //                        ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
        //                        $"Swal.fire({{ icon: 'success', title: 'Customer Update Success', text: 'Customer information updated successfully!', background: '#e9f7ef', confirmButtonColor: '#28a745' }});", true);
        //                    }
        //                    else
        //                    {
        //                        ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
        //                        $"Swal.fire({{ icon: 'error', title: 'Customer Update Failed', text: 'Failed to update customer information.', background: '#f8d7da', confirmButtonColor: '#dc3545' }});", true);
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                ContractList();
        //                ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
        //                $"Swal.fire({{ icon: 'info', title: 'No Changes Detected', text: 'No changes detected in the customer information.', background: '#e9ecef', confirmButtonColor: '#6c757d' }});", true);
        //            }
        //        }
        //    }
        //}


        protected void UpdateCustomerInfo(object sender, EventArgs e)
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
                        "Swal.fire({ icon: 'info', title: 'Invalid File Type', text: 'Only image files are allowed (jpg, jpeg, png, gif).', background: '#e9ecef', confirmButtonColor: '#6c757d' });", true);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
                                "Swal.fire({ icon: 'error', title: 'Upload Failed', text: 'Error uploading image.', background: '#f8d7da', confirmButtonColor: '#dc3545' });", true);
                    return;
                }
            }

            using (var db = new NpgsqlConnection(con))
            {
                db.Open();

                // Check if the email already exists (excluding the current customer's email)
                string emailCheckQuery = "SELECT COUNT(*) FROM customer WHERE cus_email = @newEmail AND cus_id <> @id";
                using (var cmdCheckEmail = new NpgsqlCommand(emailCheckQuery, db))
                {
                    cmdCheckEmail.Parameters.AddWithValue("@newEmail", email);
                    cmdCheckEmail.Parameters.AddWithValue("@id", id);

                    int emailExists = Convert.ToInt32(cmdCheckEmail.ExecuteScalar());
                    if (emailExists > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
                        "Swal.fire({ icon: 'info', title: 'Email Taken', text: 'Email is already taken. Please use a different email.', background: '#e9ecef', confirmButtonColor: '#6c757d' });", true);
                        return;
                    }
                }

                // Get current data for the customer based on the ID
                string selectQuery = "SELECT * FROM customer WHERE cus_id = @id";
                using (var cmdSelect = new NpgsqlCommand(selectQuery, db))
                {
                    cmdSelect.Parameters.AddWithValue("@id", id);

                    string originalFirstname = null;
                    string originalMi = null;
                    string originalLastname = null;
                    string originalContact = null;
                    string originalEmail = null;
                    byte[] originalProfileImage = null;

                    using (var reader = cmdSelect.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            originalFirstname = reader["cus_fname"].ToString();
                            originalMi = reader["cus_mname"].ToString();
                            originalLastname = reader["cus_lname"].ToString();
                            originalContact = reader["cus_contact"].ToString();
                            originalEmail = reader["cus_email"].ToString();
                            originalProfileImage = reader["cus_profile"] as byte[];
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
                                "Swal.fire({ icon: 'error', title: 'Info Not Found', text: 'No data found for the specified ID.', background: '#f8d7da', confirmButtonColor: '#dc3545' });", true);
                            return;
                        }
                    }

                    var updateFields = new List<string>();
                    var updateParams = new List<NpgsqlParameter>();
                    var changes = new List<string>();

                    // Check and update each field
                    if (!string.IsNullOrEmpty(firstname) && firstname != originalFirstname)
                    {
                        updateFields.Add("cus_fname = @firstname");
                        updateParams.Add(new NpgsqlParameter("@firstname", firstname));
                        changes.Add($"First Name: {originalFirstname} → {firstname}");
                    }
                    if (!string.IsNullOrEmpty(mi) && mi != originalMi)
                    {
                        updateFields.Add("cus_mname = @mi");
                        updateParams.Add(new NpgsqlParameter("@mi", mi));
                        changes.Add($"Middle Initial: {originalMi} → {mi}");
                    }
                    if (!string.IsNullOrEmpty(lastname) && lastname != originalLastname)
                    {
                        updateFields.Add("cus_lname = @lastname");
                        updateParams.Add(new NpgsqlParameter("@lastname", lastname));
                        changes.Add($"Last Name: {originalLastname} → {lastname}");
                    }
                    if (!string.IsNullOrEmpty(contact) && contact != originalContact)
                    {
                        updateFields.Add("cus_contact = @contact");
                        updateParams.Add(new NpgsqlParameter("@contact", contact));
                        changes.Add($"Contact: {originalContact} → {contact}");
                    }
                    if (!string.IsNullOrEmpty(email) && email != originalEmail)
                    {
                        updateFields.Add("cus_email = @email");
                        updateParams.Add(new NpgsqlParameter("@email", email));
                        changes.Add($"Email: {originalEmail} → {email}");
                    }

                    if (uploadedImageData != null)
                    {
                        updateFields.Add("cus_profile = @profile");
                        updateParams.Add(new NpgsqlParameter("@profile", uploadedImageData));
                        changes.Add("Profile Picture: Updated");
                    }

                    if (updateFields.Count > 0)
                    {
                        string updateQuery = $"UPDATE customer SET {string.Join(", ", updateFields)} WHERE cus_id = @id";
                        using (var cmdUpdate = new NpgsqlCommand(updateQuery, db))
                        {
                            cmdUpdate.Parameters.AddWithValue("@id", id);
                            cmdUpdate.Parameters.AddRange(updateParams.ToArray());

                            int updatedRows = cmdUpdate.ExecuteNonQuery();
                            if (updatedRows > 0)
                            {
                                // Notify changes via email
                                string changeDetails = string.Join("\n", changes);
                                string subject = "Account Information Update Notification";
                                string body = $"Dear Customer,\n\nYour account information has been updated. Below are the details of the changes:\n\n{changeDetails}\n\nIf you did not request these changes, please contact support immediately.\n\nBest regards,\nThe Customer Support Team";

                                if (!string.IsNullOrEmpty(email) && email != originalEmail)
                                {
                                    Send_Email(originalEmail, subject, body);
                                    Send_Email(email, subject, body);
                                }
                                else
                                {
                                    Send_Email(originalEmail, subject, body);
                                }
                                ContractList();
                                ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
                                "Swal.fire({ icon: 'success', title: 'Customer Update Success', text: 'Customer information updated successfully!', background: '#e9f7ef', confirmButtonColor: '#28a745' });", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
                                "Swal.fire({ icon: 'error', title: 'Customer Update Failed', text: 'Failed to update customer information.', background: '#f8d7da', confirmButtonColor: '#dc3545' });", true);
                            }
                        }
                    }
                    else
                    {
                        ContractList();
                        ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
                        "Swal.fire({ icon: 'info', title: 'No Changes Detected', text: 'No changes detected in the customer information.', background: '#e9ecef', confirmButtonColor: '#6c757d' });", true);
                    }
                }
            }
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
            byte[] defaultImageData = File.ReadAllBytes(Server.MapPath("Pictures\\blank_prof.png"));  // Default profile image
            byte[] imageData = formFile.HasFile ? formFile.FileBytes : defaultImageData;  // Use uploaded image or default image
            string email = emp_email.Text;

            bool emailExists = false;
            bool isEmailSuspendedOrInactive = false;

            // Generate a random password for the customer (no input from the user)
            string randomPassword = GenerateRandomPassword(12);  // You can specify the length here
            string hashedPassword = HashPassword(randomPassword);  // Hashing the generated password

            // Email Message
            string toAddress = email;
            string subject = "Important: Your Login Credentials for Completing Registration";
            string body = $"Dear Customer,\n\n" +
                $"As part of our onboarding process, we have generated your initial login credentials. Please use the following information to access the designated registration website and complete your profile:\n\n" +
                $"Email: {email}\n" +
                $"Password: {randomPassword}\n\n" +
                $"Visit the registration page on our main login page.\n\n" +
                $"Once you log in, kindly fill out the remaining information required to complete your registration. After completing this step, these credentials will serve as your permanent login information for daily use in our system.\n\n" +
                $"If you encounter any issues or have any questions, please do not hesitate to contact our support team.\n\n" +
                $"Best regards,\n" +
                $"The Account Manager Team\n" +
                $"TrashTrack";

            // Validation: Ensure all required fields are filled
            if (!string.IsNullOrEmpty(emp_firstname.Text) &&
                !string.IsNullOrEmpty(emp_lastname.Text) &&
                !string.IsNullOrEmpty(emp_email.Text) &&
                !string.IsNullOrEmpty(emp_address.Text) &&
                !string.IsNullOrEmpty(emp_contact.Text))
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
                            // Check if the email exists in any table and check its status
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

                    // If email exists and is suspended/inactive, prevent the addition of a new customer
                    if (emailExists)
                    {
                        if (isEmailSuspendedOrInactive)
                        {
                            Response.Write("<script>alert('The email is associated with an inactive or suspended account. Please use a different email.')</script>");
                        }
                        return;  // Exit the function if the email is invalid or already exists
                    }

                    // Proceed to insert the new customer
                    using (var cmd = new NpgsqlCommand(
                        @"INSERT INTO customer 
                    (cus_fname, cus_mname, cus_lname, cus_contact, cus_address, cus_email, cus_password, cus_profile, emp_id, cus_created_at, cus_updated_at, cus_otp) 
                    VALUES (@emp_fname, @emp_mname, @emp_lname, @emp_contact, @emp_address, @emp_email, @emp_password, @emp_profile, @acc_id, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, @emp_otp)", db))
                    {
                        // Adding parameters to prevent SQL injection
                        cmd.Parameters.AddWithValue("@emp_fname", emp_firstname.Text);
                        cmd.Parameters.AddWithValue("@emp_mname", emp_mi.Text);
                        cmd.Parameters.AddWithValue("@emp_lname", emp_lastname.Text);
                        cmd.Parameters.AddWithValue("@emp_contact", emp_contact.Text);
                        cmd.Parameters.AddWithValue("@emp_address", emp_address.Text);
                        cmd.Parameters.AddWithValue("@emp_email", email);
                        cmd.Parameters.AddWithValue("@emp_password", hashedPassword);  // Storing hashed random password
                        cmd.Parameters.AddWithValue("@emp_profile", imageData);  // Profile image as byte array
                        cmd.Parameters.AddWithValue("@acc_id", adminId);  // Handle nullable acc_id
                        cmd.Parameters.AddWithValue("@emp_otp", (object)null ?? DBNull.Value);  // Handle nullable emp_otp

                        // Execute the query and check how many rows were affected
                        int ctr = cmd.ExecuteNonQuery();
                        if (ctr >= 1)
                        {
                            // Success: Customer added
                            Response.Write("<script>alert('Customer Added!')</script>");
                            ContractList();  // Reload or update the list of Customers
                            Send_Email(toAddress, subject, body);  // Optionally send a welcome email
                        }
                        else
                        {
                            // Failure: Customer registration failed
                            Response.Write("<script>alert('Customer failed to Register!')</script>");
                        }
                    }

                    db.Close();
                }
            }
            else
            {
                // Validation error: Required fields are not filled
                Response.Write("<script>alert('Please fill up all the required fields!')</script>");
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

                // SQL query to update the password of the user by their email (for customer table)
                string updatePasswordQuery = "UPDATE customer SET cus_password = @newPassword WHERE cus_email = @userEmail";

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

            // Optionally, you can also send an email to the customer with the new password
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





        public static void Send_Email(string toAddress, string subject, string body)
        {
            //SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
            //smtpClient.UseDefaultCredentials = false;
            //smtpClient.Credentials = new NetworkCredential("fjunevincent369@gmail.com", "lgkw alyq ntyv mefw");
            //smtpClient.EnableSsl = true;
            //smtpClient.Port = 587;

            //MailMessage mailMessage = new MailMessage();
            //mailMessage.From = new MailAddress("fjunevincent369@gmail.com");
            //mailMessage.To.Add(toAddress);
            //mailMessage.Subject = subject;
            //mailMessage.Body = body;
            //mailMessage.IsBodyHtml = false;

            //smtpClient.Send(mailMessage);
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential("fjunevincent369@gmail.com", "lgkw alyq ntyv mefw");
            smtpClient.EnableSsl = true;
            smtpClient.Port = 587;

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("fjunevincent369@gmail.com");
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
            emp_contact.Text = "";
            //emp_role.SelectedIndex = 0;
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


        //IMODIFY PALANG
        protected void Update_Click(object sender, EventArgs e)
        {
            LinkButton btn = sender as LinkButton;
            int id = Convert.ToInt32(btn.CommandArgument);  // Get the admin ID from the button's CommandArgument
            //byte[] imageData = null;  // To hold the profile image data

            try
            {
                // Connect to PostgreSQL
                using (var db = new NpgsqlConnection(con))
                {
                    db.Open();

                    // Define the SQL query to get the admin details based on the admin ID (acc_id)
                    string query = @"
                SELECT cus_fname, cus_mname, cus_lname, cus_contact, cus_email, cus_profile 
                FROM customer 
                WHERE cus_id = @acc_id";

                    using (var cmd = new NpgsqlCommand(query, db))
                    {
                        cmd.Parameters.AddWithValue("@acc_id", id);

                        // Execute the query
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read()) // Check if data is available for the given admin ID
                            {
                                // Assign the data to the respective textboxes
                                txtbfirstname.Text = reader["cus_fname"].ToString();
                                txtmi.Text = reader["cus_mname"].ToString();
                                txtLastname.Text = reader["cus_lname"].ToString();
                                txtContact.Text = reader["cus_contact"].ToString();
                                txtEmail.Text = reader["cus_email"].ToString();
                                byte[] imageData = reader["cus_profile"] as byte[];  // Retrieve profile image data (byte array)

                                // Display profile image in the preview control
                                if (imagePreviewUpdate != null)
                                {
                                    if (imageData != null && imageData.Length > 0)
                                    {
                                        try
                                        {
                                            string base64String = Convert.ToBase64String(imageData);
                                            imagePreviewUpdate.ImageUrl = "data:image/jpeg;base64," + base64String;  // Set image as base64 string
                                        }
                                        catch (Exception ex)
                                        {
                                            Response.Write("<script>alert('Error converting image to Base64: " + ex.Message + "')</script>");
                                        }
                                    }
                                    else
                                    {
                                        imagePreviewUpdate.ImageUrl = "~/Pictures/blank_prof.png";  // Default image if no profile picture found
                                    }
                                }
                                else
                                {
                                    Response.Write("<script>alert('Image preview control is not found');</script>");
                                }
                            }
                            else
                            {
                                // Handle case when no data is found for the given admin ID
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
                                    "swal('Unsuccessful!', 'Admin not found.', 'error')", true);
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
            this.ModalPopupExtender2.Show();  // Show the modal popup

            // Optionally refresh the account manager list after the modal popup
            ContractList();
            //NonContractList();
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

                                ContractList();
                                //NonContractList();
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
                ContractList();
                //NonContractList();
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
                        cmd.CommandText = "UPDATE customer SET cus_status = 'Suspend' WHERE cus_id = @id";
                        cmd.Parameters.AddWithValue("@id", managerId);

                        var ctr = cmd.ExecuteNonQuery();
                        if (ctr >= 1)
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
                                "swal('Suspended!', 'Account Manager Suspended Successfully!', 'success')", true);
                            ContractList();
                            //NonContractList();
                        }
                    }
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
                        cmd.CommandText = "UPDATE customer SET cus_status = 'Active' WHERE cus_id = @id";
                        cmd.Parameters.AddWithValue("@id", managerId);

                        var ctr = cmd.ExecuteNonQuery();
                        if (ctr >= 1)
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
                                "swal('Unsuspended!', 'Account Manager Unsuspended Successfully!', 'success')", true);
                            ContractList();
                        }
                    }
                    db.Close();
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
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
                        cmd.CommandText = "UPDATE customer SET CUS_STATUS = 'Deleted' WHERE CUS_ID = @id";
                        cmd.Parameters.AddWithValue("@id", adminId);

                        var ctr = cmd.ExecuteNonQuery();
                        if (ctr >= 1)
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
                                "swal('Account Removed!', 'Account Manager Account Removed Successfully!', 'success')", true);
                            ContractList();
                            //NonContractList();
                        }
                    }

                    db.Close();
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
                        "swal('Unsuccessfull!', '" + ex.Message + "', 'error')", true);
            }
        }



    }
}