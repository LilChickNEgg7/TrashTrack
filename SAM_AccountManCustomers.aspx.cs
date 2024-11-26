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
    public partial class SAM_AccountManCustomers : System.Web.UI.Page
    {
        // Database Connection String
        private readonly string con = "Server=localhost;Port=5432;User Id=postgres;Password=123456;Database=trashtrack";
        
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                //emp_role.Items.FindByValue(string.Empty).Attributes.Add("disabled", "disabled");
                //LoadRoles();
                CustomerList();
                VerifyCustomerRequests();
                LoadProfile();
                //RequestsContractual();
                hfActiveTab.Value = "#sam"; // Set Tab 1 as the default
            }
            //if (IsPostBack && Request["__EVENTTARGET"] == "btnDecline")
            //{
            //    string[] args = Request["__EVENTARGUMENT"].Split('|');
            //    if (args.Length == 2)
            //    {
            //        int contId = Convert.ToInt32(args[0]);
            //        string declineReason = args[1];

            //        //DeclineContract(contId, declineReason);
            //    }
            //}
        }

        //private void DeclineContract(int contId, string declineReason)
        //{
        //    try
        //    {
        //        using (var db = new NpgsqlConnection(con))
        //        {
        //            db.Open();

        //            using (var cmd = db.CreateCommand())
        //            {
        //                // Update the contractual status to 'Declined' and insert the decline reason
        //                cmd.CommandText = "UPDATE contractual SET cont_status = 'Declined', cont_faileddesc = @declineReason WHERE cont_id = @id";
        //                cmd.Parameters.AddWithValue("@declineReason", declineReason);
        //                cmd.Parameters.AddWithValue("@id", contId);
        //                cmd.ExecuteNonQuery();
        //            }
        //            db.Close();
        //        }

        //        // Re-bind lists if necessary
        //        //RequestsContractual();

        //        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
        //            "swal('Success!', 'Contract declined!', 'success')", true);
        //    }
        //    catch (Exception ex)
        //    {
        //        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
        //            "swal('Unsuccessful!', '" + ex.Message + "', 'error')", true);
        //    }
        //}

        private void LoadProfile()
        {
            try
            {
                if (Session["sam_id"] == null)
                {
                    // Session expired or not set, redirect to login
                    Response.Redirect("LoginPage.aspx");
                    return;
                }

                int adminId = (int)Session["sam_id"];  // Retrieve admin ID from session
                string roleName = (string)Session["sam_rolename"];


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



        //Updated Approve_Click
        protected void Approve_Click(object sender, EventArgs e)
        {
            LinkButton btnApprove = (LinkButton)sender;

            // Split CommandArgument to get vc_id and cus_id
            string[] arguments = btnApprove.CommandArgument.Split(',');
            int vcId = Convert.ToInt32(arguments[0]); // First value is vc_id
            int cusId = Convert.ToInt32(arguments[1]); // Second value is cus_id

            int adminId = (int)Session["sam_id"]; // Retrieve admin ID from session

            using (var db = new NpgsqlConnection(con))
            {
                db.Open();

                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        // Check if valid_id and selfie are null
                        bool isValidPicNull = false;
                        bool isSelfiePicNull = false;

                        using (var checkCmd = db.CreateCommand())
                        {
                            checkCmd.CommandType = CommandType.Text;
                            checkCmd.CommandText = @"
                        SELECT vc_valid_id, vc_selfie 
                        FROM verified_customer 
                        WHERE vc_id = @vcId";
                            checkCmd.Parameters.AddWithValue("@vcId", vcId);
                            checkCmd.Transaction = transaction;

                            using (var reader = checkCmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    isValidPicNull = reader["vc_valid_id"] == DBNull.Value;
                                    isSelfiePicNull = reader["vc_selfie"] == DBNull.Value;
                                }
                            }
                        }

                        // Handle missing valid_id or selfie
                        if (isValidPicNull || isSelfiePicNull)
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "swal",
                                "Swal.fire({title: 'Error!', text: 'Both valid ID and selfie picture must be provided before approval.', icon: 'error', confirmButtonColor: '#d33'});", true);
                            transaction.Rollback();
                            return;
                        }

                        // Update verified_customer table
                        using (var cmd = db.CreateCommand())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = @"
                        UPDATE verified_customer 
                        SET vc_status = 'Approved', emp_id = @empid
                        WHERE vc_id = @vcId;";
                            cmd.Parameters.AddWithValue("@empid", adminId);
                            cmd.Parameters.AddWithValue("@vcId", vcId);
                            cmd.Transaction = transaction;
                            cmd.ExecuteNonQuery();
                        }

                        // Create the notification message
                        string message = "Your Account Has Been Verified ✅\n\n" +
                                         "------------------------------------------\n" +
                                         "Customer ID: " + cusId + "\n\n" +
                                         "Dear Customer,\n\n" +
                                         "Congratulations! Your account has been successfully verified. You can now easily book a service with just a few taps and have your garbage picked up conveniently.\n\n" +
                                         "Thank you for completing the verification process. 💜\n\n" +
                                         "Best regards,\n" +
                                         "TrashTrack Team";

                        // Insert notification for the customer
                        using (var cmd = db.CreateCommand())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = @"
                        INSERT INTO notification (notif_message, emp_id, cus_id, notif_type)
                        VALUES (@message, @empid, @cusid, @notiftype);";
                            cmd.Parameters.AddWithValue("@message", message);
                            cmd.Parameters.AddWithValue("@empid", adminId);
                            cmd.Parameters.AddWithValue("@cusid", cusId);
                            cmd.Parameters.AddWithValue("@notiftype", "Account Verification");
                            cmd.Transaction = transaction;
                            cmd.ExecuteNonQuery();
                        }

                        // Update customer table to set cus_isverified to true
                        using (var cmd = db.CreateCommand())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = @"
                        UPDATE customer 
                        SET cus_isverified = TRUE 
                        WHERE cus_id = @cusId;";
                            cmd.Parameters.AddWithValue("@cusId", cusId);
                            cmd.Transaction = transaction;
                            cmd.ExecuteNonQuery();
                        }

                        // Commit the transaction
                        transaction.Commit();

                        // Show success message
                        ClientScript.RegisterStartupScript(this.GetType(), "swal",
                            "Swal.fire({title: 'Success!', text: 'Customer successfully approved and verified!', icon: 'success', confirmButtonColor: '#3085d6'});", true);
                    }
                    catch (Exception ex)
                    {
                        // Rollback on error
                        transaction.Rollback();
                        ClientScript.RegisterStartupScript(this.GetType(), "swal",
                            $"Swal.fire({{title: 'Error!', text: 'An error occurred: {ex.Message}', icon: 'error', confirmButtonColor: '#d33'}});", true);
                        throw;
                    }
                }
            }

            // Refresh the grid view to reflect changes
            CustomerList();
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
                                    //hfActiveTab.Value = "#am"; // Set tab am as active

                                }
                                //hfActiveTab.Value = "#am"; // Set tab am as active

                                // Commit the transaction
                                transaction.Commit();

                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
                                    "swal('Success!', 'Contract accepted and customer type updated!', 'success')", true);

                                // Re-bind lists if necessary
                                CustomerList();
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


        //protected void Reject_Click(object sender, EventArgs e)
        //{
        //    // Retrieve the vc_id and cus_id from the hidden fields
        //    int vcId = Convert.ToInt32(hide_vcID.Value);
        //    int cusId = Convert.ToInt32(hide_cusID.Value);
        //    string declineReason = Request.Form["declineReason"]; // Get the value from the textarea

        //    using (var db = new NpgsqlConnection(con))
        //    {
        //        db.Open();

        //        // Start a transaction to ensure data consistency
        //        using (var transaction = db.BeginTransaction())
        //        {
        //            try
        //            {
        //                // Update vc_status to 'Rejected' in the verified_customer table
        //                using (var cmd = db.CreateCommand())
        //                {
        //                    cmd.CommandType = CommandType.Text;
        //                    cmd.CommandText = @"
        //                UPDATE verified_customer 
        //                SET vc_status = 'Rejected' 
        //                WHERE vc_id = @vcId;";
        //                    cmd.Parameters.AddWithValue("@vcId", vcId);
        //                    cmd.Transaction = transaction;
        //                    cmd.ExecuteNonQuery();
        //                }

        //                //// Optionally, set cus_isverified to false in the customer table (if needed)
        //                //using (var cmd = db.CreateCommand())
        //                //{
        //                //    cmd.CommandType = CommandType.Text;
        //                //    cmd.CommandText = @"
        //                //UPDATE customer 
        //                //SET cus_isverified = false 
        //                //WHERE cus_id = @cusId;";
        //                //    cmd.Parameters.AddWithValue("@cusId", cusId);
        //                //    cmd.Transaction = transaction;
        //                //    cmd.ExecuteNonQuery();
        //                //}

        //                // Commit the transaction
        //                transaction.Commit();
        //            }
        //            catch
        //            {
        //                // Rollback the transaction in case of an error
        //                transaction.Rollback();
        //                throw; // Optionally, log the error or handle it as needed
        //            }
        //        }

        //        db.Close();
        //    }

        //    // Refresh the grid view to reflect the changes
        //    VerifyCustomerRequests();
        //}

        //Original Reject_Click
        //protected void Reject_Click(object sender, EventArgs e)
        //{
        //    //string vcId = hide_vcID.Value;
        //    //string cusId = hide_cusID.Value;

        //    // Output values for testing (in the response)
        //    //Response.Write("<script>alert('Customer ID: " + cusId + " Verification ID: " + vcId + "');</script>");
        //    // Retrieve vc_id and cus_id from hidden fields
        //    int vcId = Convert.ToInt32(hide_vcID.Value);
        //    int cusId = Convert.ToInt32(hide_cusID.Value);
        //    hfActiveTab.Value = "#am"; // Set tab am as active

        //    // Retrieve the decline reason directly from the textarea control
        //    //string declineReason = Request.Form["declineReason"]; // Get the value from the textarea
        //    string declineReason = declineReasons.Text;
        //    using (var db = new NpgsqlConnection(con))
        //    {
        //        db.Open();

        //        // Start a transaction to ensure data consistency
        //        using (var transaction = db.BeginTransaction())
        //        {
        //            try
        //            {
        //                // Update vc_status to 'Rejected' and set vc_reason in the verified_customer table
        //                using (var cmd = db.CreateCommand())
        //                {
        //                    cmd.CommandType = CommandType.Text;
        //                    cmd.CommandText = @"
        //                UPDATE verified_customer 
        //                SET vc_status = 'Rejected', vc_reason = @declineReason 
        //                WHERE vc_id = @vcId;";
        //                    cmd.Parameters.AddWithValue("@vcId", vcId);
        //                    cmd.Parameters.AddWithValue("@declineReason", declineReason);
        //                    cmd.Transaction = transaction;
        //                    cmd.ExecuteNonQuery();
        //                    declineReason = "";
        //                    //VerifyCustomerRequests();
        //                    CustomerList();
        //                    hfActiveTab.Value = "#am"; // Set tab am as active
        //                }

        //                // Commit the transaction
        //                transaction.Commit();
        //            }
        //            catch
        //            {
        //                declineReason = "";

        //                // Rollback the transaction in case of an error
        //                transaction.Rollback();
        //                throw; // Optionally, log the error or handle it as needed
        //            }
        //        }

        //        db.Close();
        //    }
        //    hfActiveTab.Value = "#am"; // Set tab am as active

        //    // Refresh the grid view to reflect the changes
        //    //VerifyCustomerRequests();
        //    CustomerList();
        //}

        protected void Reject_Click(object sender, EventArgs e)
        {
            int vcId = Convert.ToInt32(hide_vcID.Value);
            int cusId = Convert.ToInt32(hide_cusID.Value);
            //hfActiveTab.Value = "#am"; // Set tab am as active

            //int adminId = 1011;
            int adminId = (int)Session["sam_id"];  // Retrieve admin ID from session
            string roleName = (string)Session["sam_rolename"];


            string declineReason = declineReasons.Text; // Retrieve decline reason

            if (string.IsNullOrWhiteSpace(declineReason))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "swal",
                           "Swal.fire({title: 'Error!', text: 'Please provide a reason for the rejection.', icon: 'error', confirmButtonColor: '#3085d6'});", true);
                return; // Stop further execution
            }


            using (var db = new NpgsqlConnection(con))
            {
                db.Open();

                // Start a transaction to ensure data consistency
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        string full_name = "";

                        // Update vc_status to 'Rejected' and set vc_reason in the verified_customer table
                        using (var cmd = db.CreateCommand())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = @"
                        UPDATE verified_customer 
                        SET vc_status = 'Rejected', vc_reason = @declineReason, emp_id = @empid
                        WHERE vc_id = @vcId;";
                            cmd.Parameters.AddWithValue("@vcId", vcId);
                            cmd.Parameters.AddWithValue("@declineReason", declineReason);
                            cmd.Parameters.AddWithValue("@empid", adminId);
                            cmd.Transaction = transaction;
                            cmd.ExecuteNonQuery();
                        }


                        using (var checkcusname = db.CreateCommand())
                        {
                            checkcusname.CommandType = CommandType.Text;
                            checkcusname.CommandText = @"SELECT
                                    TRIM(
                                            -- Concatenate non-empty address parts, only adding commas between them
                                            COALESCE(NULLIF(cus_fname, ''), '') ||
                                            CASE WHEN NULLIF(cus_fname, '') IS NOT NULL AND NULLIF(cus_mname, '') IS NOT NULL THEN ', ' ELSE '' END ||
                                            COALESCE(NULLIF(cus_mname, ''), '') ||
                                            CASE WHEN NULLIF(cus_mname, '') IS NOT NULL AND NULLIF(cus_lname, '') IS NOT NULL THEN ', ' ELSE '' END ||
                                            COALESCE(NULLIF(cus_lname, ''), '')
                                        ) AS full_name
                                    FROM 
                                        customer
                                    WHERE 
                                        cus_id = @cus_id
                                    ";
                            checkcusname.Parameters.AddWithValue("@cus_id", cusId);
                            //checkcusname.Transaction = transaction;

                            using (var readerr = checkcusname.ExecuteReader())
                            {
                                if (readerr.Read())
                                {
                                    full_name = readerr["full_name"].ToString();
                                    //cus_id = Convert.ToInt32(readerr["cus_id"]);

                                }
                            }
                        }


                        string message = "Your Account Verification Request: Rejected ❌\n\n" +
                 "------------------------------------------\n" +
                 "Customer ID: " + cusId + "\n\n" +
                 "Dear " + full_name + ",\n\n" +
                 declineReason +
                 "Best regards,\n" +
                 "TrashTrack Team";

                        using (var cmd = db.CreateCommand())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = @"INSERT into NOTIFICATION (notif_message, emp_id, cus_id, notif_type)
                                                VALUES (@message, @emp_id, @cus_id, @notif_type)
                        ";
                            cmd.Parameters.AddWithValue("@message", message);
                            cmd.Parameters.AddWithValue("@emp_id", adminId);
                            cmd.Parameters.AddWithValue("@cus_id", cusId);
                            cmd.Parameters.AddWithValue("@notif_type", "account verification");
                            //cmd.Transaction = transaction;
                            cmd.ExecuteNonQuery();
                        }

                        // Commit the transaction
                        transaction.Commit();

                        // Show success message using SweetAlert
                        ClientScript.RegisterStartupScript(this.GetType(), "swal",
                            "Swal.fire({title: 'Success!', text: 'Customer rejected successfully.', icon: 'success', confirmButtonColor: '#3085d6'});", true);
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction in case of an error
                        transaction.Rollback();

                        // Show error message using SweetAlert
                        ClientScript.RegisterStartupScript(this.GetType(), "swal",
                            $"Swal.fire({{title: 'Error!', text: 'Failed to reject customer. {ex.Message}', icon: 'error', confirmButtonColor: '#d33'}});", true);

                        throw; // Optionally rethrow the error after showing the alert
                    }
                }

                db.Close();
            }

            /* hfActiveTab.Value = "#am";*/ // Set tab am as active

            // Refresh the grid view to reflect the changes
            CustomerList();
        }





        //protected void VerificationDetails_Click(object sender, EventArgs e)
        //{
        //    // Retrieve vc_id from the hidden field
        //    int vcId = Convert.ToInt32(HiddenField1.Value);

        //    hfActiveTab.Value = "#am"; // Set tab am as active

        //    // Create a connection to the database and fetch the cus_id for the given vcId
        //    using (var db = new NpgsqlConnection(con))
        //    {
        //        db.Open();

        //        // Select the cus_id from verified_customer based on the vc_id
        //        using (var cmd = db.CreateCommand())
        //        {
        //            cmd.CommandType = CommandType.Text;
        //            cmd.CommandText = @"
        //        SELECT c.cus_id
        //        FROM verified_customer vc
        //        INNER JOIN customer c ON c.cus_id = vc.cus_id
        //        WHERE vc.vc_id = @vcId;
        //    ";
        //            cmd.Parameters.AddWithValue("@vcId", vcId);

        //            // Execute the query and get the cus_id
        //            int cusId = Convert.ToInt32(cmd.ExecuteScalar());

        //            // Set the cus_id in TextBox1
        //            //TextBox1.Text = cusId.ToString();
        //            //TextBox1.Text = HiddenField1.Value;
        //            TextBox1.Text = "hdshf";

        //        }

        //        db.Close();
        //    }

        //    // Refresh the grid view to reflect the changes
        //    VerifyCustomerRequests();
        //}


        //ORIGINAL CustomerList()
        //protected void CustomerList()
        //{
        //    using (var db = new NpgsqlConnection(con))
        //    {
        //        db.Open();
        //        using (var cmd = db.CreateCommand())
        //        {
        //            cmd.CommandType = CommandType.Text;
        //            // Modified the query to match the column names in the account_manager table
        //            cmd.CommandText = "SELECT * FROM customer WHERE cus_status != 'Deleted' ORDER BY cus_id, cus_status";

        //            // Ensure the parameter type is correct (assuming emp_id is an integer)
        //            //cmd.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Integer, Convert.ToInt32(Session["id"]));

        //            // Execute the query and bind to the GridView
        //            DataTable admin_datatable = new DataTable();
        //            NpgsqlDataAdapter admin_sda = new NpgsqlDataAdapter(cmd);
        //            admin_sda.Fill(admin_datatable);

        //            gridView1.DataSource = admin_datatable;
        //            gridView1.DataBind();
        //        }
        //    }
        //}

        //protected void CustomerList()
        //{
        //    using (var db = new NpgsqlConnection(con))
        //    {
        //        db.Open();
        //        using (var cmd = db.CreateCommand())
        //        {
        //            cmd.CommandType = CommandType.Text;
        //            // Modified the query to match the column names in the account_manager table
        //            cmd.CommandText = "SELECT * FROM customer WHERE cus_status != 'Deleted' ORDER BY cus_id, cus_status";

        //            // Ensure the parameter type is correct (assuming emp_id is an integer)
        //            //cmd.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Integer, Convert.ToInt32(Session["id"]));

        //            // Execute the query and bind to the GridView
        //            DataTable admin_datatable = new DataTable();
        //            NpgsqlDataAdapter admin_sda = new NpgsqlDataAdapter(cmd);
        //            admin_sda.Fill(admin_datatable);

        //            gridView1.DataSource = admin_datatable;
        //            gridView1.DataBind();
        //        }
        //    }
        //}

        protected void CustomerList()
        {
            using (var db = new NpgsqlConnection(con))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"
                        SELECT 
                        c.cus_id,
                        c.cus_isverified,
                        CONCAT(c.cus_fname, 
                               CASE 
                                   WHEN c.cus_mname IS NOT NULL AND c.cus_mname <> '' THEN CONCAT(' ', c.cus_mname) 
                                   ELSE '' 
                               END, 
                               ' ', c.cus_lname) AS FullName,
                        vc.vc_id,
                        vc.vc_status,
                        c.cus_status,
                        CASE 
                            WHEN vc.vc_valid_id IS NOT NULL THEN 'Yes' 
                            ELSE 'No' 
                        END AS valid_id_uploaded,
                        CASE 
                            WHEN vc.vc_selfie IS NOT NULL THEN 'Yes' 
                            ELSE 'No' 
                        END AS selfie_uploaded
                    FROM 
                        customer c
                    LEFT JOIN 
                        verified_customer vc ON c.cus_id = vc.cus_id
                    WHERE 
                        c.cus_status != 'Deleted'
                    ORDER BY 
                        c.cus_id, c.cus_status;

                    ";



                    //Original
                    //        cmd.CommandText = @"
                    //    SELECT 
                    //        c.cus_id,
                    //        c.cus_isverified,
                    //        c.cus_status,
                    //        CONCAT(c.cus_fname, 
                    //               CASE 
                    //                   WHEN c.cus_mname IS NOT NULL AND c.cus_mname <> '' THEN CONCAT(' ', c.cus_mname) 
                    //                   ELSE '' 
                    //               END, 
                    //               ' ', c.cus_lname) AS FullName,
                    //        vc.vc_id,
                    //        vc.vc_status,
                    //        vc.vc_valid_id,
                    //        vc.vc_selfie
                    //    FROM 
                    //        customer c
                    //    LEFT JOIN 
                    //        verified_customer vc ON c.cus_id = vc.cus_id
                    //    WHERE 
                    //        c.cus_status != 'Deleted'
                    //    ORDER BY 
                    //        c.cus_id, c.cus_status;
                    //";

                    DataTable admin_datatable = new DataTable();
                    NpgsqlDataAdapter admin_sda = new NpgsqlDataAdapter(cmd);
                    admin_sda.Fill(admin_datatable);

                    gridView1.DataSource = admin_datatable;
                    gridView1.DataBind();
                }
            }
        }




        //protected void VerifyCustomerRequests()
        //{
        //    using (var db = new NpgsqlConnection(con))
        //    {
        //        db.Open();
        //        using (var cmd = db.CreateCommand())
        //        {
        //            cmd.CommandType = CommandType.Text;
        //            cmd.CommandText = @"
        //        SELECT 
        //            vc.vc_status, 
        //            vc.vc_id, 
        //            c.cus_contact, 
        //            vc.vc_created_at, 
        //            CONCAT(c.cus_fname, ' ', COALESCE(c.cus_mname || ' ', ''), c.cus_lname) AS full_name, 
        //            c.cus_id
        //        FROM 
        //            customer c
        //        LEFT JOIN 
        //            verified_customer vc
        //        ON 
        //            c.cus_id = vc.cus_id
        //        WHERE 
        //            c.cus_status != 'Deleted' 
        //            AND c.cus_isverified = false
        //        ORDER BY 
        //            c.cus_id, c.cus_status;
        //    ";

        //            DataTable admin_datatable = new DataTable();
        //            NpgsqlDataAdapter admin_sda = new NpgsqlDataAdapter(cmd);
        //            admin_sda.Fill(admin_datatable);

        //            gridView2.DataSource = admin_datatable;
        //            gridView2.DataBind();
        //        }
        //        db.Close();
        //    }
        //}
        protected void VerifyCustomerRequests()
        {
            using (var db = new NpgsqlConnection(con))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"
                SELECT 
                    vc.vc_status, 
                    vc.vc_id, 
                    c.cus_contact, 
                    vc.vc_created_at, 
                    CONCAT(c.cus_fname, ' ', COALESCE(c.cus_mname || ' ', ''), c.cus_lname) AS full_name, 
                    c.cus_id
                FROM 
                    customer c
                INNER JOIN 
                    verified_customer vc
                ON 
                    c.cus_id = vc.cus_id
                WHERE 
                    c.cus_status != 'Deleted' 
                    AND c.cus_isverified = false
                    AND vc.vc_status = 'Pending'
                ORDER BY 
                    c.cus_id, c.cus_status;
            ";

                    DataTable admin_datatable = new DataTable();
                    NpgsqlDataAdapter admin_sda = new NpgsqlDataAdapter(cmd);
                    admin_sda.Fill(admin_datatable);

                    gridView2.DataSource = admin_datatable;
                    gridView2.DataBind();
                }
                db.Close();
            }
        }

        //Original Approve_Click
        //protected void Approve_Click(object sender, EventArgs e)
        //{
        //    LinkButton btnApprove = (LinkButton)sender;
        //    int vcId = Convert.ToInt32(btnApprove.CommandArgument);

        //    using (var db = new NpgsqlConnection(con))
        //    {
        //        db.Open();

        //        // Start a transaction to ensure data consistency
        //        using (var transaction = db.BeginTransaction())
        //        {
        //            try
        //            {

        //                // Update vc_status to 'Approved' in the verified_customer table
        //                using (var cmd = db.CreateCommand())
        //                {
        //                    cmd.CommandType = CommandType.Text;
        //                    cmd.CommandText = @"
        //                UPDATE verified_customer 
        //                SET vc_status = 'Approved' 
        //                WHERE vc_id = @vcId;";
        //                    cmd.Parameters.AddWithValue("@vcId", vcId);
        //                    cmd.Transaction = transaction;
        //                    cmd.ExecuteNonQuery();
        //                    //VerifyCustomerRequests();
        //                    CustomerList();

        //                }

        //                // Set cus_isverified to true in the customer table
        //                using (var cmd = db.CreateCommand())
        //                {
        //                    cmd.CommandType = CommandType.Text;
        //                    cmd.CommandText = @"
        //                UPDATE customer 
        //                SET cus_isverified = TRUE 
        //                WHERE cus_id = (
        //                    SELECT cus_id 
        //                    FROM verified_customer 
        //                    WHERE vc_id = @vcId
        //                );";
        //                    cmd.Parameters.AddWithValue("@vcId", vcId);
        //                    cmd.Transaction = transaction;
        //                    cmd.ExecuteNonQuery();
        //                    //VerifyCustomerRequests();
        //                    CustomerList();

        //                }

        //                // Commit the transaction
        //                transaction.Commit();
        //            }
        //            catch
        //            {
        //                // Rollback the transaction in case of an error
        //                transaction.Rollback();
        //                throw; // Optionally, log the error or handle it as needed
        //            }
        //        }

        //        db.Close();
        //    }

        //    // Refresh the grid view to reflect the changes
        //    //VerifyCustomerRequests();
        //    CustomerList();
        //}


        //protected void Approve_Click(object sender, EventArgs e)
        //{
        //    LinkButton btnApprove = (LinkButton)sender;
        //    int vcId = Convert.ToInt32(btnApprove.CommandArgument);            
        //    int adminId = (int)Session["sam_id"];  // Retrieve admin ID from session
        //    string roleName = (string)Session["sam_rolename"];

        //    using (var db = new NpgsqlConnection(con))
        //    {
        //        db.Open();

        //        // Start a transaction to ensure data consistency
        //        using (var transaction = db.BeginTransaction())
        //        {
        //            try
        //            {
        //                // Check if the valid_id_pic and valid_selfie are null
        //                bool isValidPicNull = false;
        //                bool isSelfiePicNull = false;
        //                int cus_id = 0;
        //                string full_name = "";
        //                // Execute the SELECT query to check for nulls in vc_valid_id and vc_selfie
        //                using (var checkCmd = db.CreateCommand())
        //                {
        //                    checkCmd.CommandType = CommandType.Text;
        //                    checkCmd.CommandText = @"
        //                SELECT vc_valid_id, vc_selfie, cus_id 
        //                FROM verified_customer 
        //                WHERE vc_id = @vcId";
        //                    checkCmd.Parameters.AddWithValue("@vcId", vcId);
        //                    checkCmd.Transaction = transaction;

        //                    using (var reader = checkCmd.ExecuteReader())
        //                    {
        //                        if (reader.Read())
        //                        {
        //                            // Check if valid_id_pic or selfie_pic are null
        //                            isValidPicNull = reader["vc_valid_id"] == DBNull.Value;
        //                            isSelfiePicNull = reader["vc_selfie"] == DBNull.Value;
        //                            cus_id = Convert.ToInt32(reader["cus_id"]);

        //                        }
        //                    }
        //                }

        //                // If either picture is null, show an alert and stop execution
        //                if (isValidPicNull || isSelfiePicNull)
        //                {
        //                    ClientScript.RegisterStartupScript(this.GetType(), "swal",
        //                        "Swal.fire({title: 'Error!', text: 'Both valid ID and selfie picture must be provided before approval.', icon: 'error', confirmButtonColor: '#d33'});", true);

        //                    transaction.Rollback(); // Rollback the transaction
        //                    return; // Stop further execution
        //                }

        //                // Proceed with approval if both pictures are not null
        //                // Update vc_status to 'Approved' in the verified_customer table
        //                using (var cmd = db.CreateCommand())
        //                {
        //                    cmd.CommandType = CommandType.Text;
        //                    cmd.CommandText = @"
        //                UPDATE verified_customer 
        //                SET vc_status = 'Approved',
        //                    emp_id = @empid
        //                WHERE vc_id = @vcId;";
        //                    cmd.Parameters.AddWithValue("@empid", adminId);
        //                    cmd.Parameters.AddWithValue("@vcId", vcId);
        //                    cmd.Transaction = transaction;
        //                    cmd.ExecuteNonQuery();
        //                }

        //                // Set cus_isverified to true in the customer table
        //                using (var cmd = db.CreateCommand())
        //                {
        //                    cmd.CommandType = CommandType.Text;
        //                    cmd.CommandText = @"
        //                UPDATE customer 
        //                SET cus_isverified = TRUE 
        //                WHERE cus_id = (
        //                    SELECT cus_id 
        //                    FROM verified_customer 
        //                    WHERE vc_id = @vcId
        //                );";
        //                    cmd.Parameters.AddWithValue("@vcId", vcId);
        //                    cmd.Transaction = transaction;
        //                    cmd.ExecuteNonQuery();
        //                }


        //                using (var checkcusname = db.CreateCommand())
        //                {
        //                    checkcusname.CommandType = CommandType.Text;
        //                    checkcusname.CommandText = @"SELECT
        //                            TRIM(
        //                                    -- Concatenate non-empty address parts, only adding commas between them
        //                                    COALESCE(NULLIF(cus_fname, ''), '') ||
        //                                    CASE WHEN NULLIF(cus_fname, '') IS NOT NULL AND NULLIF(cus_mname, '') IS NOT NULL THEN ', ' ELSE '' END ||
        //                                    COALESCE(NULLIF(cus_mname, ''), '') ||
        //                                    CASE WHEN NULLIF(cus_mname, '') IS NOT NULL AND NULLIF(cus_lname, '') IS NOT NULL THEN ', ' ELSE '' END ||
        //                                    COALESCE(NULLIF(cus_lname, ''), '')
        //                                ) AS full_name
        //                            FROM 
        //                                customer
        //                            WHERE 
        //                                cus_id = @cus_id
        //                            ";
        //                    checkcusname.Parameters.AddWithValue("@cus_id", cus_id);
        //                    checkcusname.Transaction = transaction;

        //                    using (var readerr = checkcusname.ExecuteReader())
        //                    {
        //                        if (readerr.Read())
        //                        {
        //                            full_name = readerr["full_name"].ToString();
        //                            //cus_id = Convert.ToInt32(readerr["cus_id"]);

        //                        }
        //                    }
        //                }




        //                string message = "Your Account Has Been Verified ✅\n\n" +
        //         "------------------------------------------\n" +
        //         "Customer ID: " + cus_id + "\n\n" +
        //         "Dear " + full_name + ",\n\n" +
        //         "Congratulations! Your account has been successfully verified. You can now easily book a service with just a few taps and have your garbage picked up conveniently.\n\n" +
        //         "Thank you for completing the verification process. 💜\n\n" +
        //         "Best regards,\n" +
        //         "TrashtTrack Team";

        //                using (var cmd = db.CreateCommand())
        //                {
        //                    cmd.CommandType = CommandType.Text;
        //                    cmd.CommandText = @"INSERT into NOTIFICATION (notif_message, emp_id, cus_id, notif_type)
        //                                        VALUES (@message, @emp_id, @cus_id, @notif_type)
        //                ";
        //                    cmd.Parameters.AddWithValue("@message", message);
        //                    cmd.Parameters.AddWithValue("@emp_id", adminId);
        //                    cmd.Parameters.AddWithValue("@cus_id", cus_id);
        //                    cmd.Parameters.AddWithValue("@notif_type", "account verification");
        //                    cmd.Transaction = transaction;
        //                    cmd.ExecuteNonQuery();
        //                }

        //                // Commit the transaction
        //                transaction.Commit();

        //                // Show success message using SweetAlert
        //                ClientScript.RegisterStartupScript(this.GetType(), "swal",
        //                    "Swal.fire({title: 'Success!', text: 'Customer successfully approved and verified!', icon: 'success', confirmButtonColor: '#3085d6'});", true);
        //            }
        //            catch (Exception ex)
        //            {
        //                // Rollback the transaction in case of an error
        //                transaction.Rollback();

        //                // Show error message using SweetAlert
        //                ClientScript.RegisterStartupScript(this.GetType(), "swal",
        //                    $"Swal.fire({{title: 'Error!', text: 'An error occurred: {ex.Message}', icon: 'error', confirmButtonColor: '#d33'}});", true);

        //                throw; // Optionally rethrow the error after showing the alert
        //            }
        //        }

        //        db.Close();
        //    }

        //    // Refresh the grid view to reflect the changes
        //    CustomerList();
        //}



        //protected void VerifyCustomerRequests()
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
                                CustomerList();
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
                        CustomerList();
                        ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
                        "Swal.fire({ icon: 'info', title: 'No Changes Detected', text: 'No changes detected in the customer information.', background: '#e9ecef', confirmButtonColor: '#6c757d' });", true);
                    }
                }
            }
        }

        //        protected void submitBtn_Click(object sender, EventArgs e)
        //        {
        //            int adminId = (int)Session["sam_id"];

        //            // Extracting user input
        //            //string customerType = emp_role.SelectedValue; // Get selected customer type
        //            string hashedPassword = HashPassword(emp_pass.Text);  // Hashing the password
        //            byte[] defaultImageData = File.ReadAllBytes(Server.MapPath("Pictures\\blank_prof.png"));  // Default profile image
        //            byte[] imageData = formFile.HasFile ? formFile.FileBytes : defaultImageData;  // Use uploaded image or default image
        //            string email = emp_email.Text;

        //            bool emailExists = false;
        //            bool isEmailSuspendedOrInactive = false;

        //            // Email Message
        //            string toAddress = email;
        //            string subject = "Important: Your Login Credentials for Completing Registration";
        //            string body = $"Dear Staff and Good Day!,\n\n" +
        //                $"As a part of our onboarding process, we have generated your initial login credentials. Please use the following information to access the designated registration website and complete your profile:\n\n" +
        //                $"Email: {email}\n" +
        //                $"Password: {emp_pass.Text}\n\n" +
        //                $"Visit the registration page on our main login page.\n\n" +
        //                $"Once you log in, kindly fill out the remaining information required to complete your registration. After completing this step, these credentials will serve as your permanent login information for daily use in our system.\n\n" +
        //                $"If you encounter any issues or have any questions, please do not hesitate to contact our support team.\n\n" +
        //                $"Best regards,\n" +
        //                $"The Account Manager Team\n" +
        //                $"TrashTrack";


        //            if (!string.IsNullOrEmpty(emp_firstname.Text) &&
        //                !string.IsNullOrEmpty(emp_lastname.Text) &&
        //                !string.IsNullOrEmpty(emp_email.Text) &&
        //                !string.IsNullOrEmpty(emp_pass.Text) &&
        //                !string.IsNullOrEmpty(emp_address.Text) &&
        //                !string.IsNullOrEmpty(emp_contact.Text)) // Check if a valid type is selected
        //            // Validation: Ensure all required fields are filled
        //            //if (!string.IsNullOrEmpty(emp_firstname.Text) &&
        //            //    !string.IsNullOrEmpty(emp_lastname.Text) &&
        //            //    !string.IsNullOrEmpty(emp_email.Text) &&
        //            //    !string.IsNullOrEmpty(emp_pass.Text) &&
        //            //    !string.IsNullOrEmpty(emp_address.Text) &&
        //            //    !string.IsNullOrEmpty(emp_contact.Text) &&
        //            //    !string.IsNullOrEmpty(customerType) && // Validate that a customer type is selected
        //            //    customerType != "") // Check if a valid type is selected
        //            {
        //                // Connect to PostgreSQL
        //                using (var db = new NpgsqlConnection(con))
        //                {
        //                    db.Open();

        //                    // SQL query to check if the email exists in any relevant table and retrieve status
        //                    string emailCheckQuery = @"
        //SELECT cus_email AS email, cus_status AS status FROM customer WHERE cus_email = @emp_email
        //UNION ALL
        //SELECT emp_email AS email, emp_status AS status FROM employee WHERE emp_email = @emp_email";

        //                    using (var cmd = new NpgsqlCommand(emailCheckQuery, db))
        //                    {
        //                        cmd.Parameters.AddWithValue("@emp_email", email);

        //                        using (var reader = cmd.ExecuteReader())
        //                        {
        //                            // Check if the email exists in any table and check its status
        //                            while (reader.Read())
        //                            {
        //                                emailExists = true;  // Email exists
        //                                string status = reader["status"].ToString().ToLower();

        //                                // Email is inactive or suspended
        //                                if (status == "inactive" || status == "suspend")
        //                                {
        //                                    isEmailSuspendedOrInactive = true;
        //                                    break;
        //                                }
        //                            }
        //                        }
        //                    }

        //                    // If email exists and is suspended/inactive, prevent the addition of a new customer
        //                    if (emailExists)
        //                    {
        //                        if (isEmailSuspendedOrInactive)
        //                        {
        //                            Response.Write("<script>alert('The email is associated with an inactive or suspended account. Please use a different email.')</script>");
        //                        }
        //                        return;  // Exit the function if the email is invalid or already exists
        //                    }

        //                    // Proceed to insert the new customer
        //                    using (var cmd = new NpgsqlCommand(
        //                        @"INSERT INTO customer 
        //                (cus_fname, cus_mname, cus_lname, cus_contact, cus_address, cus_email, cus_password, cus_profile, emp_id, cus_created_at, cus_updated_at, cus_otp) 
        //                VALUES (@emp_fname, @emp_mname, @emp_lname, @emp_contact, @emp_address, @emp_email, @emp_password, @emp_profile, @acc_id, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP, @emp_otp)", db))
        //                    {
        //                        // Adding parameters to prevent SQL injection
        //                        cmd.Parameters.AddWithValue("@emp_fname", emp_firstname.Text);
        //                        cmd.Parameters.AddWithValue("@emp_mname", emp_mi.Text);
        //                        cmd.Parameters.AddWithValue("@emp_lname", emp_lastname.Text);
        //                        cmd.Parameters.AddWithValue("@emp_contact", emp_contact.Text);
        //                        cmd.Parameters.AddWithValue("@emp_address", emp_address.Text);
        //                        cmd.Parameters.AddWithValue("@emp_email", email);
        //                        cmd.Parameters.AddWithValue("@emp_password", hashedPassword);
        //                        cmd.Parameters.AddWithValue("@emp_profile", imageData);  // Profile image as byte array
        //                        cmd.Parameters.AddWithValue("@acc_id", adminId);  // Handle nullable acc_id
        //                        cmd.Parameters.AddWithValue("@emp_otp", (object)null ?? DBNull.Value);  // Handle nullable emp_otp

        //                        // Execute the query and check how many rows were affected
        //                        int ctr = cmd.ExecuteNonQuery();
        //                        if (ctr >= 1)
        //                        {
        //                            // Success: Customer added
        //                            Response.Write("<script>alert('Customer Added!')</script>");
        //                            ContractList();  // Reload or update the list of Customers
        //                            //NonContractList();
        //                            Send_Email(toAddress, subject, body);  // Optionally send a welcome email
        //                        }
        //                        else
        //                        {
        //                            // Failure: Customer registration failed
        //                            Response.Write("<script>alert('Customer failed to Register!')</script>");
        //                            ContractList();  // Reload or update the list of Customers
        //                            //NonContractList();
        //                        }
        //                    }

        //                    db.Close();
        //                }
        //            }
        //            else
        //            {
        //                // Validation error: Required fields are not filled
        //                Response.Write("<script>alert('Please fill up all the required fields!')</script>");
        //            }
        //        }

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
            int adminId = (int)Session["sam_id"];

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
                            CustomerList();  // Reload or update the list of Customers

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
        //protected void Update_Click(object sender, EventArgs e)
        //{
        //    LinkButton btn = sender as LinkButton;
        //    int id = Convert.ToInt32(btn.CommandArgument);  // Get the admin ID from the button's CommandArgument
        //    //byte[] imageData = null;  // To hold the profile image data

        //    try
        //    {
        //        // Connect to PostgreSQL
        //        using (var db = new NpgsqlConnection(con))
        //        {
        //            db.Open();

        //            // Define the SQL query to get the admin details based on the admin ID (acc_id)
        //            string query = @"
        //        SELECT cus_fname, cus_mname, cus_lname, cus_contact, cus_email, cus_profile 
        //        FROM customer 
        //        WHERE cus_id = @acc_id";

        //            using (var cmd = new NpgsqlCommand(query, db))
        //            {
        //                cmd.Parameters.AddWithValue("@acc_id", id);

        //                // Execute the query
        //                using (var reader = cmd.ExecuteReader())
        //                {
        //                    if (reader.Read()) // Check if data is available for the given admin ID
        //                    {
        //                        // Assign the data to the respective textboxes
        //                        txtbfirstname.Text = reader["cus_fname"].ToString();
        //                        txtmi.Text = reader["cus_mname"].ToString();
        //                        txtLastname.Text = reader["cus_lname"].ToString();
        //                        txtContact.Text = reader["cus_contact"].ToString();
        //                        txtEmail.Text = reader["cus_email"].ToString();
        //                        byte[] imageData = reader["cus_profile"] as byte[];  // Retrieve profile image data (byte array)

        //                        // Display profile image in the preview control
        //                        if (imagePreviewUpdate != null)
        //                        {
        //                            if (imageData != null && imageData.Length > 0)
        //                            {
        //                                try
        //                                {
        //                                    string base64String = Convert.ToBase64String(imageData);
        //                                    imagePreviewUpdate.ImageUrl = "data:image/jpeg;base64," + base64String;  // Set image as base64 string
        //                                }
        //                                catch (Exception ex)
        //                                {
        //                                    Response.Write("<script>alert('Error converting image to Base64: " + ex.Message + "')</script>");
        //                                }
        //                            }
        //                            else
        //                            {
        //                                imagePreviewUpdate.ImageUrl = "~/Pictures/blank_prof.png";  // Default image if no profile picture found
        //                            }
        //                        }
        //                        else
        //                        {
        //                            Response.Write("<script>alert('Image preview control is not found');</script>");
        //                        }
        //                    }
        //                    else
        //                    {
        //                        // Handle case when no data is found for the given admin ID
        //                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
        //                            "swal('Unsuccessful!', 'Admin not found.', 'error')", true);
        //                        return; // Exit if no data is found
        //                    }
        //                }
        //            }

        //            db.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle any errors
        //        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
        //            "swal('Unsuccessful!', '" + ex.Message + "', 'error')", true);
        //        return; // Exit if there was an error
        //    }

        //    // Set the ID textbox and show the modal popup
        //    txtbxID.Text = id.ToString();
        //    this.ModalPopupExtender2.Show();  // Show the modal popup

        //    // Optionally refresh the account manager list after the modal popup
        //    CustomerList();
        //    //NonContractList();
        //}




        protected void Update_Click(object sender, EventArgs e)
        {
            LinkButton btn = sender as LinkButton;
            int id = Convert.ToInt32(btn.CommandArgument); // Get the customer ID from the button's CommandArgument

            try
            {
                // Connect to PostgreSQL
                using (var db = new NpgsqlConnection(con))
                {
                    db.Open();

                    // SQL query to join customer and verified_customer tables
                    string query = @"SELECT 
                                        c.cus_fname, c.cus_mname, c.cus_lname, c.cus_contact, c.cus_email, c.cus_profile, 
                                        vc.vc_valid_id, vc.vc_selfie, vc.vc_status, c.cus_status,
                                        TRIM(
                                            -- Concatenate non-empty address parts, only adding commas between them
                                            COALESCE(NULLIF(c.cus_street, ''), '') ||
                                            CASE WHEN NULLIF(c.cus_street, '') IS NOT NULL AND NULLIF(c.cus_brgy, '') IS NOT NULL THEN ', ' ELSE '' END ||
                                            COALESCE(NULLIF(c.cus_brgy, ''), '') ||
                                            CASE WHEN NULLIF(c.cus_brgy, '') IS NOT NULL AND NULLIF(c.cus_city, '') IS NOT NULL THEN ', ' ELSE '' END ||
                                            COALESCE(NULLIF(c.cus_city, ''), '') ||
                                            CASE WHEN NULLIF(c.cus_city, '') IS NOT NULL AND NULLIF(c.cus_province, '') IS NOT NULL THEN ', ' ELSE '' END ||
                                            COALESCE(NULLIF(c.cus_province, ''), '') ||
                                            CASE WHEN NULLIF(c.cus_province, '') IS NOT NULL AND NULLIF(c.cus_postal, '') IS NOT NULL THEN ', ' ELSE '' END ||
                                            COALESCE(NULLIF(c.cus_postal, ''), '')
                                        ) AS full_address
                                    FROM 
                                        customer c
                                    LEFT JOIN 
                                        verified_customer vc ON c.cus_id = vc.cus_id
                                    WHERE 
                                        c.cus_id = @cus_id
                                    ";

                    using (var cmd = new NpgsqlCommand(query, db))
                    {
                        cmd.Parameters.AddWithValue("@cus_id", id);

                        // Execute the query
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Populate customer details
                                txtbfirstname.Text = reader["cus_fname"].ToString();
                                txtmi.Text = reader["cus_mname"].ToString();
                                txtLastname.Text = reader["cus_lname"].ToString();
                                txtContact.Text = reader["cus_contact"].ToString();
                                txtEmail.Text = reader["cus_email"].ToString();
                                TextBox1.Text = reader["full_address"].ToString();
                                



                                // Display profile image
                                byte[] profileImage = reader["cus_profile"] as byte[];
                                if (profileImage != null && profileImage.Length > 0)
                                {
                                    string profileBase64 = Convert.ToBase64String(profileImage);
                                    imagePreviewUpdate.ImageUrl = "data:image/jpeg;base64," + profileBase64;
                                }
                                else
                                {
                                    imagePreviewUpdate.ImageUrl = "~/Pictures/blank_prof.png";
                                }

                                // Display Valid ID Image
                                byte[] validIdImage = reader["vc_valid_id"] as byte[];
                                if (validIdImage != null && validIdImage.Length > 0)
                                {
                                    string validIdBase64 = Convert.ToBase64String(validIdImage);
                                    valid_id.ImageUrl = "data:image/jpeg;base64," + validIdBase64;
                                }
                                else
                                {
                                    valid_id.ImageUrl = "~/Pictures/blank_prof.png"; // Default image
                                }

                                // Display Selfie Image
                                byte[] selfieImage = reader["vc_selfie"] as byte[];
                                if (selfieImage != null && selfieImage.Length > 0)
                                {
                                    string selfieBase64 = Convert.ToBase64String(selfieImage);
                                    valid_selfie.ImageUrl = "data:image/jpeg;base64," + selfieBase64;
                                }
                                else
                                {
                                    valid_selfie.ImageUrl = "~/Pictures/blank_prof.png"; // Default image
                                }

                                // Check cus_status and enable/disable the Update button
                                string customerStatus = reader["cus_status"].ToString();
                                btnUpdate.Enabled = customerStatus == "Active";

                                // Get the vc_status
                                //string vcStatus = reader["vc_status"].ToString();

                                //// Disable btnVerify and btnReject if vc_status is "Active" or "Rejected"
                                //if (vcStatus == "Approved" || vcStatus == "Rejected")
                                //{
                                //    btnVerify.Enabled = false;
                                //    btnReject.Enabled = false;
                                //}
                                //else
                                //{
                                //    btnVerify.Enabled = true;
                                //    btnReject.Enabled = true;
                                //}

                            }
                            else
                            {
                                // Handle case when no data is found
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
                                    "swal('Unsuccessful!', 'Customer not found.', 'error')", true);
                                return;
                            }
                        }
                    }
                    db.Close();
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
                    "swal('Unsuccessful!', '" + ex.Message + "', 'error')", true);
                return;
            }

            // Show the modal popup
            txtbxID.Text = id.ToString();
            this.ModalPopupExtender2.Show();

            // Refresh the account manager list (if needed)
            //ContractList();
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

                                CustomerList();
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
                CustomerList();
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
                            CustomerList();
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
                            CustomerList();
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
                            CustomerList();
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




        //protected void VerificationDetails_Click(object sender, EventArgs e)

        //{
        //    // Retrieve vc_id from the hidden field
        //    int vcId = Convert.ToInt32(HiddenField1.Value);

        //    using (var db = new NpgsqlConnection(con))
        //    {
        //        db.Open();

        //        using (var cmd = db.CreateCommand())
        //        {
        //            cmd.CommandType = CommandType.Text;
        //            cmd.CommandText = @"
        //        SELECT c.cus_id
        //        FROM verified_customer vc
        //        INNER JOIN customer c ON c.cus_id = vc.cus_id
        //        WHERE vc.vc_id = @vcId;
        //    ";
        //            cmd.Parameters.AddWithValue("@vcId", vcId);

        //            object result = cmd.ExecuteScalar();
        //            if (result != null)
        //            {
        //                TextBox1.Text = result.ToString();
        //            }
        //            else
        //            {
        //                TextBox1.Text = "No data found";
        //            }
        //        }

        //        db.Close();
        //    }
        //}


        //protected void VerificationDetails_Click(object sender, EventArgs e)

        //{
        //    //LinkButton linkButton = sender as LinkButton;

        //    //int vcId = Convert.ToInt32(linkButton.CommandArgument);
        //    //int cusId = vcId;

        //    ////Response.Write("<script>alert('Verified ID: "+vcId+" and Customer ID: "+cusId+"');</script>");
        //    ////Response.Write("<script>alert('vcId: " + vcId + "');</script>");

        //    ////using (var db = new NpgsqlConnection(con))
        //    ////{
        //    ////    db.Open();
        //    ////    using(var cmd = db.CreateCommand())
        //    ////    {
        //    ////        cmd.CommandType = CommandType.Text;
        //    ////        cmd.CommandText = "SELECT VC_ID, CUS_ID FROM verified_customer WHERE VC_ID = @vcid";
        //    ////        cmd.Parameters.AddWithValue("@vcid", vcId);

        //    ////        using(var reader = cmd.ExecuteReader())
        //    ////        {
        //    ////            if (reader.Read())
        //    ////            {
        //    ////                txtVerificationID.Text = reader["vc_id"].ToString();
        //    ////                //txtContactDetail.Text = reader["cus_contact"].ToString();
        //    ////                txtCustomerID.Text = reader["cus_id"].ToString();
        //    ////            }
        //    ////        }
        //    ////    }
        //    ////}


        //    //////Show modal popup
        //    ////ModalPopupExtenderDetails.Show();
        //    ///
        //    try
        //    {
        //        // Retrieve the LinkButton
        //        LinkButton linkButton = sender as LinkButton;

        //        if (linkButton == null || string.IsNullOrEmpty(linkButton.CommandArgument))
        //            throw new Exception("CommandArgument is missing.");

        //        // Parse the CommandArgument
        //        int vcId = Convert.ToInt32(linkButton.CommandArgument);

        //        // Debug CommandArgument value
        //        //Response.Write("<script>alert('vcId: " + vcId + "');</script>");

        //        using (var db = new NpgsqlConnection(con))
        //        {
        //            db.Open();
        //            using (var cmd = db.CreateCommand())
        //            {
        //                cmd.CommandType = CommandType.Text;
        //                cmd.CommandText = "SELECT VC_ID, CUS_ID FROM verified_customer WHERE VC_ID = @vcid";
        //                cmd.Parameters.AddWithValue("@vcid", vcId);

        //                using (var reader = cmd.ExecuteReader())
        //                {
        //                    if (reader.Read())
        //                    {
        //                        txtVerificationID.Text = reader["vc_id"].ToString();
        //                        txtCustomerID.Text = reader["cus_id"].ToString();
        //                    }
        //                    else
        //                    {
        //                        // Debug no records found
        //                        Response.Write("<script>alert('No records found for vcId: " + vcId + "');</script>");
        //                    }
        //                }
        //            }
        //        }

        //        // Show modal popup
        //        ModalPopupExtenderDetails.Show();
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle errors
        //        Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
        //    }


        //}

        //protected void VerificationDetails_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        // Get the clicked button and extract the CommandArgument
        //        LinkButton linkButton = sender as LinkButton;

        //        if (linkButton == null || string.IsNullOrEmpty(linkButton.CommandArgument))
        //            throw new Exception("CommandArgument is missing.");

        //        int vcId = Convert.ToInt32(linkButton.CommandArgument);

        //        // Initialize variables to store the database values
        //        string verificationID = string.Empty;
        //        string customerID = string.Empty;
        //        string contactDetail = string.Empty;

        //        // Database connection and data retrieval
        //        using (var db = new NpgsqlConnection(con))
        //        {
        //            db.Open();
        //            using (var cmd = db.CreateCommand())
        //            {
        //                cmd.CommandType = CommandType.Text;
        //                cmd.CommandText = "SELECT VC_ID, CUS_ID, CUS_CONTACT FROM verified_customer WHERE VC_ID = @vcid";
        //                cmd.Parameters.AddWithValue("@vcid", vcId);

        //                using (var reader = cmd.ExecuteReader())
        //                {
        //                    if (reader.Read())
        //                    {
        //                        verificationID = reader["vc_id"].ToString();
        //                        customerID = reader["cus_id"].ToString();
        //                        contactDetail = reader["cus_contact"].ToString();
        //                    }
        //                }
        //            }
        //        }

        //        // Populate the modal's TextBox controls
        //        txtVerificationID.Text = verificationID;
        //        txtCustomerID.Text = customerID;
        //        txtContactDetail.Text = contactDetail;

        //        // Show the modal using JavaScript (this prevents a page reload)
        //        string script = "var modal = new bootstrap.Modal(document.getElementById('fullscreenModal')); modal.show();";
        //        ClientScript.RegisterStartupScript(this.GetType(), "ShowModal", script, true);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Display error message (optional)
        //        Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
        //    }
        //}

        protected void valid_idbtndownload_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtbxID.Text))
            {
                Response.Write("<script>alert('Please enter a valid ID.');</script>");
                return;
            }

            int customerId = Convert.ToInt32(txtbxID.Text);
            byte[] imageData = null;
            string customerName = null;

            using (var db = new NpgsqlConnection(con))
            {
                db.Open();

                // Query to get the valid ID and customer name
                string query = @"
            SELECT vc_valid_id, CONCAT(cus_fname, '_', COALESCE(cus_mname, ''), '_', cus_lname) AS full_name
            FROM verified_customer vc
            JOIN customer c ON vc.cus_id = c.cus_id
            WHERE vc.cus_id = @id";

                using (var cmd = new NpgsqlCommand(query, db))
                {
                    cmd.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Integer, customerId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            imageData = reader["vc_valid_id"] as byte[];
                            customerName = reader["full_name"]?.ToString().Replace(" ", "_");
                        }
                        else
                        {
                            Response.Write("<script>alert('No data found for the specified ID.');</script>");
                            return;
                        }
                    }
                }
            }

            if (imageData != null && imageData.Length > 0)
            {
                string base64String = Convert.ToBase64String(imageData);
                string imageUrl = $"data:image/jpeg;base64,{base64String}";
                string filename = $"{customerName}_VALID_ID_{customerId}.jpg"; // Dynamic filename based on customer details

                string downloadScript = $"downloadImage('{imageUrl}', '{filename}');";
                ScriptManager.RegisterStartupScript(this, GetType(), "DownloadImageScript", downloadScript, true);
            }
            else
            {
                Response.Write("<script>alert('No image found.');</script>");
            }
        }



        protected void valid_selfiebtndownload_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtbxID.Text))
            {
                Response.Write("<script>alert('Please enter a valid ID.');</script>");
                return;
            }

            int customerId = Convert.ToInt32(txtbxID.Text);
            byte[] imageData = null;
            string customerName = null;

            using (var db = new NpgsqlConnection(con))
            {
                db.Open();

                // Query to get the selfie and customer name
                string query = @"
            SELECT vc_selfie, CONCAT(cus_fname, '_', COALESCE(cus_mname, ''), '_', cus_lname) AS full_name
            FROM verified_customer vc
            JOIN customer c ON vc.cus_id = c.cus_id
            WHERE vc.cus_id = @id";

                using (var cmd = new NpgsqlCommand(query, db))
                {
                    cmd.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Integer, customerId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            imageData = reader["vc_selfie"] as byte[];
                            customerName = reader["full_name"]?.ToString().Replace(" ", "_");
                        }
                        else
                        {
                            Response.Write("<script>alert('No data found for the specified ID.');</script>");
                            return;
                        }
                    }
                }
            }

            if (imageData != null && imageData.Length > 0)
            {
                string base64String = Convert.ToBase64String(imageData);
                string imageUrl = $"data:image/jpeg;base64,{base64String}";
                string filename = $"{customerName}_Valid_Selfie_{customerId}.jpg"; // Dynamic filename based on customer details

                string downloadScript = $"downloadImage('{imageUrl}', '{filename}');";
                ScriptManager.RegisterStartupScript(this, GetType(), "DownloadImageScript", downloadScript, true);
            }
            else
            {
                Response.Write("<script>alert('No image found.');</script>");
            }
        }


        protected void VCSeeDetails_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            int VCID = Convert.ToInt32(btn.CommandArgument);

            Response.Write("<script>alert('VC ID is: " + VCID + "');</script>");


        }

        protected void btnViewSlip_Click(object sender, EventArgs e)
        {

            Button btn = (Button)sender;
            int bookingId = Convert.ToInt32(btn.CommandArgument);
            //int bookingId = Convert.ToInt32(TextBox1.Text);  // Assuming you hardcode the bookingId for now. Adjust this as necessary.
            //TextBox2.Text = bookingId.ToString();
            byte[] imageData = null;

            // Define the PostgreSQL connection
            using (var db = new NpgsqlConnection(con))
            {
                db.Open();

                // PostgreSQL query to get the waste scale slip image
                string query = "SELECT vc_valid_id, vc_selfie FROM verified_customer WHERE VC_ID = @id";
                using (var cmd = new NpgsqlCommand(query, db))
                {
                    cmd.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Integer, bookingId);

                    // Execute the query and retrieve the image data
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            //Response.Write("<script>alert('DATA FOUND MOTHERCJERRR HELL YEAH!.');</script>");
                            imageData = reader["vc_valid_id"] as byte[];
                        }
                        else
                        {
                            // No image found for the specified booking ID
                            Response.Write("<script>alert('No data found for the specified ID.');</script>");
                            return;
                        }
                    }
                }
                db.Close();
            }

            // Load the image
            if (imageData != null && imageData.Length > 0)
            {
                string base64String = Convert.ToBase64String(imageData);

                // Use a script to set the image URL after the modal opens
                string script = $@"
                document.getElementById('Image2').style.display = 'block';
                document.getElementById('Image2').src = 'data:image/jpeg;base64,{base64String}';
            ";
                //        string script = $@"
                //    document.getElementById('loading-spinner').style.display = 'none';
                //    document.getElementById('Image2').style.display = 'block';
                //    document.getElementById('Image2').src = 'data:image/jpeg;base64,{base64String}';
                //";
                ScriptManager.RegisterStartupScript(this, GetType(), "LoadImageScript", script, true);
            }
            else
            {
                // If no image data, hide spinner and set default image
                string script = @"
                document.getElementById('Image2').style.display = 'block';
                document.getElementById('Image2').src = '~/Pictures/blank_prof.png';
            ";
                ScriptManager.RegisterStartupScript(this, GetType(), "DefaultImageScript", script, true);
                //        string script = @"
                //    document.getElementById('loading-spinner').style.display = 'none';
                //    document.getElementById('Image2').style.display = 'block';
                //    document.getElementById('Image2').src = '~/Pictures/blank_prof.png';
                //";
                //        ScriptManager.RegisterStartupScript(this, GetType(), "DefaultImageScript", script, true);
            }

            // Show the modal popup
            ModalPopupExtender4.Show();
        }

        //TESTING IF THERE'S VALUE 
        //protected void btnViewSlip_Click(object sender, EventArgs e)
        //{
        //    Button btn = sender as Button;

        //    if (btn == null || string.IsNullOrEmpty(btn.CommandArgument))
        //    {
        //        Response.Write("<script>alert('CommandArgument is null or empty.');</script>");
        //        return;
        //    }

        //    if (int.TryParse(btn.CommandArgument, out int bookingId))
        //    {
        //        Response.Write("<script>alert('VC_ID is = " + bookingId.ToString() + "');</script>");
        //    }
        //    else
        //    {
        //        Response.Write("<script>alert('CommandArgument is not a valid integer.');</script>");
        //    }
        //}


        protected void btnCloseSlip_Click(object sender, EventArgs e)
        {
            // Hide the modal
            ModalPopupExtender4.Hide();

            // Reset the image and show the spinner for the next time it's opened
            string resetScript = @"
        document.getElementById('Image2').style.display = 'none';
        document.getElementById('Image2').src = '';";

            //        string resetScript = @"
            //    document.getElementById('loading-spinner').style.display = 'block';
            //    document.getElementById('Image2').style.display = 'none';
            //    document.getElementById('Image2').src = '';
            //";
            ScriptManager.RegisterStartupScript(this, GetType(), "ResetModal", resetScript, true);

        }

        protected void btnReject_Click(object sender, EventArgs e)
        {

            //Button to Test if ID is not null
            //string cusID = txtbxID.Text;
            //Response.Write("<script>alert('Button is Clicked! Customer ID is:" + cusID + "');</script>");




            int custID = Int32.Parse(txtbxID.Text);
            //Test adminID
            //int adminId = 1004;
            int adminId = (int)Session["sam_id"];  // Retrieve admin ID from session
            string roleName = (string)Session["sam_rolename"];

            string cus_email = txtEmail.Text;
            //string cus_email = "imperialemperor123@gmail.com";


            try
            {
                using (var db = new NpgsqlConnection(con))
                {
                    db.Open();

                    // Update CUSTOMER table
                    //using (var cmd = db.CreateCommand())
                    //{
                    //    cmd.CommandType = CommandType.Text;
                    //    cmd.CommandText = "UPDATE CUSTOMER SET CUS_STATUS = 'Rejected' WHERE CUS_ID = @custID";
                    //    cmd.Parameters.AddWithValue("@custID", custID);
                    //    cmd.ExecuteNonQuery();
                    //}

                    // Update VERIFIED_CUSTOMER table
                    using (var cmd2 = db.CreateCommand())
                    {
                        cmd2.CommandType = CommandType.Text;
                        cmd2.CommandText = "UPDATE VERIFIED_CUSTOMER SET VC_STATUS = 'Rejected', EMP_ID = @empid WHERE CUS_ID = @custID";
                        cmd2.Parameters.AddWithValue("@custID", custID);
                        cmd2.Parameters.AddWithValue("@empid", adminId);
                        cmd2.ExecuteNonQuery();
                    }

                    string cus_lname = "";
                    string cus_fname = "";
                    string cus_mname = "";
                    string full_name = "";
                    //int cus_id = 0;
                    // Get Customer Info
                    using (var cmd3 = db.CreateCommand())
                    {
                        cmd3.CommandType = CommandType.Text;
                        cmd3.CommandText = "SELECT CUS_LNAME, CUS_FNAME, CUS_MNAME FROM CUSTOMER WHERE CUS_ID = @cus_id";
                        cmd3.Parameters.AddWithValue("@cus_id", custID);

                        using (var reader = cmd3.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                cus_lname = reader["cus_lname"]?.ToString() ?? ""; // Safeguard against NULL
                                cus_fname = reader["cus_fname"]?.ToString() ?? "";
                                cus_mname = reader["cus_mname"]?.ToString() ?? "";
                            }
                            else
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "swal",
                                    "Swal.fire({title: 'Error!', text: 'Customer record not found.', icon: 'error', confirmButtonColor: '#d33'});", true);
                            }
                        }
                    }



                    string subject = "Your Registration Has Been Rejected";
                    string body = $"Dear Mr./Mrs. {cus_fname} {cus_mname} {cus_lname},\n\n" +
                                  "We regret to inform you that your registration has been rejected.\n\n" +
                                  "This decision was made because the submitted ID and selfie did not match our records.\n\n" +
                                  "If you believe this is an error or would like to provide additional information, please do not hesitate to contact our support team at trashtrackspteam@gmail.com / 455-6399.\n\n" +
                                  "Thank you for your understanding.\n\n" +
                                  "Best regards,\n" +
                                  "TrashTrack Support Team";


                    Send_Email(cus_email, subject, body); // Function to send email with rejection information

                    CustomerList();

                    ClientScript.RegisterStartupScript(this.GetType(), "swal", "Swal.fire({title: 'Rejected!', text: 'Customer rejected successfully!', icon: 'success', confirmButtonColor: '#3085d6'});", true);





                    //ContractList();
                }
            }
            catch (Exception ex)
            {
                // Log error (use appropriate logging mechanism)
                Console.WriteLine(ex); // Example: Replace with logging framework

                // Show user-friendly error
                ClientScript.RegisterStartupScript(this.GetType(), "swal", "Swal.fire({title: 'Error!', text: 'An unexpected error occurred. Please try again later.', icon: 'error', confirmButtonColor: '#d33'});", true);
                CustomerList();
            }


        }

        protected void btnVerify_Click(object sender, EventArgs e)
        {
            //Button to Test if ID is not null
            //string cusID = txtbxID.Text;
            //Response.Write("<script>alert('Button is Clicked! Customer ID is:" + cusID + "');</script>");

            int custID = Int32.Parse(txtbxID.Text);

            //Test adminID
            //int adminId = 1004;

            int adminId = (int)Session["sam_id"];  // Retrieve admin ID from session
            string roleName = (string)Session["sam_rolename"];

            //Get the email of the current customer
            string cus_email = txtEmail.Text;

            //string cus_email = "imperialemperor123@gmail.com";


            try
            {
                using (var db = new NpgsqlConnection(con))
                {
                    db.Open();

                    //// Update CUSTOMER table
                    //using (var cmd = db.CreateCommand())
                    //{
                    //    cmd.CommandType = CommandType.Text;
                    //    cmd.CommandText = "UPDATE CUSTOMER SET CUS_STATUS = 'Active', CUS_ISVERIFIED = true WHERE CUS_ID = @custID";
                    //    cmd.Parameters.AddWithValue("@custID", custID);
                    //    cmd.ExecuteNonQuery();
                    //}

                    // Update VERIFIED_CUSTOMER table
                    using (var cmd2 = db.CreateCommand())
                    {
                        cmd2.CommandType = CommandType.Text;
                        cmd2.CommandText = "UPDATE VERIFIED_CUSTOMER SET VC_STATUS = 'Approved', EMP_ID = @empid WHERE CUS_ID = @custID";
                        cmd2.Parameters.AddWithValue("@custID", custID);
                        cmd2.Parameters.AddWithValue("@empid", adminId);
                        cmd2.ExecuteNonQuery();
                    }


                    string cus_lname = "";
                    string cus_fname = "";
                    string cus_mname = "";

                    // Get Customer Info
                    using (var cmd3 = db.CreateCommand())
                    {
                        cmd3.CommandType = CommandType.Text;
                        cmd3.CommandText = "SELECT CUS_LNAME, CUS_FNAME, CUS_MNAME FROM CUSTOMER WHERE CUS_ID = @cus_id";
                        cmd3.Parameters.AddWithValue("@cus_id", custID);

                        using (var reader = cmd3.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                cus_lname = reader["cus_lname"]?.ToString() ?? ""; // Safeguard against NULL
                                cus_fname = reader["cus_fname"]?.ToString() ?? "";
                                cus_mname = reader["cus_mname"]?.ToString() ?? "";
                            }
                            else
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "swal",
                                    "Swal.fire({title: 'Error!', text: 'Customer record not found.', icon: 'error', confirmButtonColor: '#d33'});", true);
                            }
                        }
                    }


                    string subject = "Your Registration Has Been Verified";
                    string body = $"Dear Mr./Mrs. {cus_fname} {cus_mname} {cus_lname},\n\n" +
                                   $"We are pleased to inform you that your registration has been successfully verified.\n" +
                                  $"After reviewing the submitted identification and selfie, we have confirmed their authenticity and consistency with our records. You can now access your account and take full advantage of our services.\n\n" +
                                  "If you have any questions or need assistance, please feel free to reach out to our support team.\n" +
                                  "Best regards,\n" +
                                  "TrashTrack Support Team";

                    Send_Email(cus_email, subject, body); // Function to send email with verification confirmation

                    ClientScript.RegisterStartupScript(this.GetType(), "swal", "Swal.fire({title: 'Verified!!', text: 'Customer verified successfully!', icon: 'success', confirmButtonColor: '#3085d6'});", true);
                    //ContractList();


                }
            }
            catch (Exception ex)
            {
                // Log error (use appropriate logging mechanism)
                Console.WriteLine(ex); // Example: Replace with logging framework

                // Show user-friendly error
                ClientScript.RegisterStartupScript(this.GetType(), "swal", "Swal.fire({title: 'Error!', text: 'An unexpected error occurred. Please try again later.', icon: 'error', confirmButtonColor: '#d33'});", true);
            }


        }


    }
}
