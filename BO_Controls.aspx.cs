using AjaxControlToolkit;
using MongoDB.Bson;
using MongoDB.Driver;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static Capstone.PaymentController;


namespace Capstone
{
    public partial class BO_Controls : System.Web.UI.Page
    {
        //string connString = "mongodb+srv://admin1:V1ncent123@cluster0.pbtzqh3.mongodb.net/";
        // Database Connection String
        private readonly string con = "Server=localhost;Port=5432;User Id=postgres;Password=123456;Database=trashtrack";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //LoadCategoryData();
                //LoadProfileImage();
                //showInterestAndLeadDays();
                showPaymentTerm();
                LoadProfile();
                WasteCatList();
                GetUnreadNotificationCount();
                BindNotifications();
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

        private void LoadProfile()
        {
            //// Store necessary session data
            //Session["bo_email"] = loginEmail;
            //Session["bo_password"] = storedHashedPassword;
            //Session["bo_id"] = userId;  // Store the user ID in the session
            //Session["bo_rolename"] = roleName;
            try
            {
                if (Session["bo_id"] == null)
                {
                    // Session expired or not set, redirect to login
                    Response.Redirect("Login.aspx");
                    return;
                }

                int adminId = (int)Session["bo_id"];  // Retrieve admin ID from session
                string roleName = (string)Session["bo_rolename"];


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
                        db.Close();
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
            //AccountManList();
            //SupAccountManList();
            //BillinOfficerList();
            //OperationalDispList();

        }



        protected void WasteCatList()
        {
            using (var db = new NpgsqlConnection(con))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    // Modified the query to fetch only required fields and avoid deleted records
                    cmd.CommandText = "SELECT wc_id, wc_name, wc_price, wc_unit, wc_max FROM waste_category WHERE wc_status != 'Deleted' order by wc_id";

                    //// Assuming that 'emp_id' is associated with the current session user's ID
                    //cmd.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Integer, Convert.ToInt32(Session["bo_id"]));

                    // Execute the query and bind to the GridView
                    DataTable admin_datatable = new DataTable();
                    NpgsqlDataAdapter admin_sda = new NpgsqlDataAdapter(cmd);
                    admin_sda.Fill(admin_datatable);

                    // Bind the data table to the GridView
                    gridViewWaste.DataSource = admin_datatable;
                    gridViewWaste.DataBind();
                }
            }
        }
        protected void submitBtn_Click(object sender, EventArgs e)
        {
            // Validate Description field
            if (string.IsNullOrWhiteSpace(waste_desc.Text))
            {
                Response.Write("<script>alert('Description cannot be empty. Please provide a description.')</script>");
                return;
            }

            // Establish the PostgreSQL connection using Npgsql
            using (var conn = new NpgsqlConnection(con))
            {
                conn.Open();

                // Check if the waste name already exists
                string checkWasteQuery = "SELECT COUNT(*) FROM waste_category WHERE wc_name = @name";
                using (var checkCmd = new NpgsqlCommand(checkWasteQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@name", waste_name.Text);
                    int existingCount = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (existingCount > 0)
                    {
                        Response.Write("<script>alert('Waste name already exists. Please choose a different waste name.')</script>");
                        return;
                    }
                }

                // Validate and convert the price
                if (!decimal.TryParse(price.Text, out decimal wastePrice))
                {
                    Response.Write("<script>alert('Invalid price. Please enter a valid decimal value.')</script>");
                    return;
                }

                // Round price to two decimal places
                wastePrice = Math.Round(wastePrice, 2);

                // Insert the new waste category
                string insertQuery = @"
            INSERT INTO waste_category (wc_name, wc_unit, wc_price, wc_status, emp_id, wc_max, wc_desc) 
            VALUES (@name, @unit, @price, 'Active', @empId, @max, @desc)";

                using (var insertCmd = new NpgsqlCommand(insertQuery, conn))
                {
                    insertCmd.Parameters.AddWithValue("@name", waste_name.Text);
                    insertCmd.Parameters.AddWithValue("@unit", unit.Text);
                    insertCmd.Parameters.AddWithValue("@price", wastePrice);
                    insertCmd.Parameters.AddWithValue("@empId", Convert.ToInt32(Session["bo_id"])); // Assuming 'emp_id' is stored in the session
                    insertCmd.Parameters.AddWithValue("@max", Convert.ToInt32(max.Text));
                    insertCmd.Parameters.AddWithValue("@desc", waste_desc.Text);

                    insertCmd.ExecuteNonQuery();
                    Response.Write("<script>alert('Waste added!')</script>");
                }

                WasteCatList();
                conn.Close();
            }
        }


        //protected void submitBtn_Click(object sender, EventArgs e)
        //{
        //    // Establish the PostgreSQL connection using Npgsql
        //    using (var conn = new NpgsqlConnection(con))
        //    {
        //        conn.Open();

        //        // Check if the waste name already exists
        //        string checkWasteQuery = "SELECT COUNT(*) FROM waste_category WHERE wc_name = @name";
        //        using (var checkCmd = new NpgsqlCommand(checkWasteQuery, conn))
        //        {
        //            checkCmd.Parameters.AddWithValue("@name", waste_name.Text);
        //            int existingCount = Convert.ToInt32(checkCmd.ExecuteScalar());

        //            if (existingCount > 0)
        //            {
        //                // Display an error message indicating that the waste name already exists
        //                Response.Write("<script>alert('Waste name already exists. Please choose a different waste name.')</script>");
        //                return;
        //            }
        //        }

        //        // Validate and convert the price
        //        if (!decimal.TryParse(price.Text, out decimal wastePrice))
        //        {
        //            // Display an error message for invalid price
        //            Response.Write("<script>alert('Invalid price. Please enter a valid decimal value.')</script>");
        //            return;
        //        }

        //        // Round to two decimal places
        //        wastePrice = Math.Round(wastePrice, 2);

        //        // Insert the new waste category into the PostgreSQL database
        //        string insertQuery = @"
        //    INSERT INTO waste_category (wc_name, wc_unit, wc_price, wc_status, emp_id, wc_max) 
        //    VALUES (@name, @unit, @price, 'Active', @empId, @max)";

        //        using (var insertCmd = new NpgsqlCommand(insertQuery, conn))
        //        {
        //            insertCmd.Parameters.AddWithValue("@name", waste_name.Text);
        //            insertCmd.Parameters.AddWithValue("@unit", unit.Text);
        //            insertCmd.Parameters.AddWithValue("@price", wastePrice);
        //            insertCmd.Parameters.AddWithValue("@empId", Convert.ToInt32(Session["bo_id"])); // Assuming 'emp_id' is stored in the session
        //            insertCmd.Parameters.AddWithValue("@max", Convert.ToInt32(max.Text));

        //            insertCmd.ExecuteNonQuery();
        //            Response.Write("<script>alert('Waste added!')</script>");
        //            WasteCatList();
        //        }
        //        WasteCatList();
        //        conn.Close();
        //    }

        //    // Load or refresh the category data (implement this method as needed)
        //    //LoadCategoryData();
        //}

        ////WITH INTEREST AND SUCH OR MURAG LOAN
        ////protected void changeTerm_Click(object sender, EventArgs e)
        ////{
        ////    // Validate inputs
        ////    if (double.TryParse(interest.Text, out double interestValue) &&
        ////        int.TryParse(leaddays.Text, out int leadDays) &&
        ////        double.TryParse(vat.Text, out double vatValue) &&
        ////        int.TryParse(acc_per.Text, out int accrualPeriod) &&
        ////        int.TryParse(susp_per.Text, out int suspensionPeriod))
        ////    {
        ////        using (var conn = new NpgsqlConnection(con))
        ////        {
        ////            conn.Open();

        ////            // Check if there are any existing records in the PAYMENT_TERM table
        ////            string checkQuery = "SELECT COUNT(*) FROM PAYMENT_TERM";
        ////            using (var checkCmd = new NpgsqlCommand(checkQuery, conn))
        ////            {
        ////                int recordCount = Convert.ToInt32(checkCmd.ExecuteScalar());

        ////                if (recordCount > 0)
        ////                {
        ////                    // Update the existing record
        ////                    string updateQuery = @"
        ////                UPDATE PAYMENT_TERM 
        ////                SET pt_interest = @interestValue, 
        ////                    pt_lead_days = @leadDays,
        ////                    pt_tax = @vatValue,
        ////                    pt_accrual_period = @accrualPeriod,
        ////                    pt_susp_period = @suspensionPeriod,
        ////                    pt_updated_at = NOW()"; // Specify the control_id or appropriate condition

        ////                    using (var updateCmd = new NpgsqlCommand(updateQuery, conn))
        ////                    {
        ////                        updateCmd.Parameters.AddWithValue("@interestValue", interestValue);
        ////                        updateCmd.Parameters.AddWithValue("@leadDays", leadDays);
        ////                        updateCmd.Parameters.AddWithValue("@vatValue", vatValue);
        ////                        updateCmd.Parameters.AddWithValue("@accrualPeriod", accrualPeriod);
        ////                        updateCmd.Parameters.AddWithValue("@suspensionPeriod", suspensionPeriod);
        ////                        updateCmd.ExecuteNonQuery();
        ////                    }

        ////                    Response.Write("<script>alert('Payment term updated successfully!')</script>");
        ////                    showPaymentTerm();
        ////                    WasteCatList();
        ////                }
        ////                else
        ////                {
        ////                    // Insert a new record if no records exist
        ////                    string insertQuery = @"
        ////                INSERT INTO PAYMENT_TERM (pt_interest, pt_lead_days, pt_tax, pt_accrual_period, pt_susp_period, emp_id, pt_created_at, pt_updated_at) 
        ////                VALUES (@interestValue, @leadDays, @vatValue, @accrualPeriod, @suspensionPeriod, @empId, NOW(), NOW())";

        ////                    using (var insertCmd = new NpgsqlCommand(insertQuery, conn))
        ////                    {
        ////                        insertCmd.Parameters.AddWithValue("@interestValue", interestValue);
        ////                        insertCmd.Parameters.AddWithValue("@leadDays", leadDays);
        ////                        insertCmd.Parameters.AddWithValue("@vatValue", vatValue);
        ////                        insertCmd.Parameters.AddWithValue("@accrualPeriod", accrualPeriod);
        ////                        insertCmd.Parameters.AddWithValue("@suspensionPeriod", suspensionPeriod);
        ////                        insertCmd.Parameters.AddWithValue("@empId", Convert.ToInt32(Session["bo_id"])); // Using session value for emp_id
        ////                        insertCmd.ExecuteNonQuery();
        ////                    }

        ////                    Response.Write("<script>alert('Payment term added successfully!')</script>");
        ////                    showPaymentTerm();
        ////                    LoadProfile();
        ////                    WasteCatList();
        ////                }
        ////            }

        ////            conn.Close(); // Closing the connection
        ////        }
        ////    }
        ////    else
        ////    {
        ////        // Handle the case where any input is not valid
        ////        Response.Write("<script>alert('Invalid input values. Please enter valid values for all fields.')</script>");
        ////    }
        ////}


        ////IMODIFY PALANG
        protected void changeTerm_Click(object sender, EventArgs e)
        {
            // Validate inputs
            if (double.TryParse(vat.Text, out double vatValue))
            {
                using (var conn = new NpgsqlConnection(con))
                {
                    conn.Open();

                    // Check if there are any existing records in the PAYMENT_TERM table
                    string checkQuery = "SELECT COUNT(*) FROM PAYMENT_TERM";
                    using (var checkCmd = new NpgsqlCommand(checkQuery, conn))
                    {
                        int recordCount = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (recordCount > 0)
                        {
                            // Update the existing record
                            string updateQuery = @"UPDATE PAYMENT_TERM 
                                                    SET pt_tax = @vatValue,
                                                        pt_updated_at = NOW()"; // Specify the control_id or appropriate condition

                            using (var updateCmd = new NpgsqlCommand(updateQuery, conn))
                            {
                                updateCmd.Parameters.AddWithValue("@vatValue", vatValue);
                                updateCmd.ExecuteNonQuery();
                            }

                            Response.Write("<script>alert('Value-Added Tax updated successfully!')</script>");
                            showPaymentTerm();
                            WasteCatList();
                        }
                        else
                        {
                            // Insert a new record if no records exist
                            string insertQuery = @"
                        INSERT INTO PAYMENT_TERM (pt_tax, emp_id, pt_created_at, pt_updated_at) 
                        VALUES (@vatValue, @empId, NOW(), NOW())";

                            using (var insertCmd = new NpgsqlCommand(insertQuery, conn))
                            {

                                insertCmd.Parameters.AddWithValue("@vatValue", vatValue);
                                insertCmd.Parameters.AddWithValue("@empId", Convert.ToInt32(Session["bo_id"])); // Using session value for emp_id
                                insertCmd.ExecuteNonQuery();
                            }

                            Response.Write("<script>alert('Value-Added Tax added successfully!')</script>");
                            showPaymentTerm();
                            LoadProfile();
                            WasteCatList();
                        }
                    }

                    conn.Close(); // Closing the connection
                }
            }
            else
            {
                // Handle the case where any input is not valid
                Response.Write("<script>alert('Invalid input values. Please enter valid values for all fields.')</script>");
            }
        }

        protected void Update_Click(object sender, EventArgs e)
        {
            LinkButton btn = sender as LinkButton;
            int id = Convert.ToInt32(btn.CommandArgument);  // Get the admin ID from the button's CommandArgument
            //byte[] imageData = null;  // To hold the profile image data
            //txtbxID txtbxnewName txtbxnewUnit txtbxnewPrice
            try
            {
                // Connect to PostgreSQL
                using (var db = new NpgsqlConnection(con))
                {
                    db.Open();

                    // Define the SQL query to get the admin details based on the admin ID (acc_id)
                    string query = @"
                SELECT * FROM waste_category WHERE wc_id = @wc_id";

                    using (var cmd = new NpgsqlCommand(query, db))
                    {
                        cmd.Parameters.AddWithValue("@wc_id", id);

                        // Execute the query
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read()) // Check if data is available for the given admin ID
                            {
                                // Assign the data to the respective textboxes
                                txtbxnewName.Text = reader["wc_name"].ToString();
                                txtDescription.Text = reader["wc_desc"].ToString();
                                txtbxnewUnit.Text = reader["wc_unit"].ToString();
                                txtbxnewPrice.Text = reader["wc_price"].ToString();
                                txtLimit.Text = reader["wc_max"].ToString();
                                //byte[] imageData = reader["emp_profile"] as byte[];  // Retrieve profile image data (byte array)
                                showPaymentTerm();
                                WasteCatList();
                            }
                            else
                            {
                                // Handle case when no data is found for the given admin ID
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
                                    "swal('Unsuccessful!', 'Waste Category not found.', 'error')", true);
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
            //AccountManList();
        }

        protected void Remove_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;

            // Retrieve the waste ID (assuming it's passed via the CommandArgument property)
            int waste_id = Convert.ToInt32(btn.CommandArgument);

            try
            {
                using (var conn = new NpgsqlConnection(con))
                {
                    conn.Open();

                    // Update the status to "Deleted" in the "waste_category" table
                    string updateQuery = @"
                UPDATE waste_category
                SET wc_status = 'Deleted'
                WHERE wc_id = @id";

                    using (var updateCmd = new NpgsqlCommand(updateQuery, conn))
                    {
                        updateCmd.Parameters.AddWithValue("@id", waste_id);

                        int rowsAffected = updateCmd.ExecuteNonQuery();

                        if (rowsAffected >= 1)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
    "Swal.fire({ icon: 'success', title: 'Deleted!', text: 'Waste category deleted successfully!', background: '#e9f7ef', confirmButtonColor: '#28a745' });", true);
                            WasteCatList();
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
    "Swal.fire({ icon: 'error', title: 'Unsuccessful!', text: 'Waste category not found or not updated!', background: '#f8d7da', confirmButtonColor: '#dc3545' });", true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
                    "swal('Error!', '" + ex.Message + "', 'error')", true);
            }
        }


        //WITH INTEREST
        //protected void showPaymentTerm()
        //{
        //    using (var conn = new NpgsqlConnection(con))
        //    {
        //        conn.Open();

        //        // Query to get all relevant fields from the PAYMENT_TERM table
        //        string query = "SELECT pt_lead_days, pt_interest, pt_tax, pt_accrual_period, pt_susp_period FROM PAYMENT_TERM";

        //        using (var cmd = new NpgsqlCommand(query, conn))
        //        {
        //            using (var reader = cmd.ExecuteReader())
        //            {
        //                if (reader.Read())
        //                {
        //                    // Set the values for lead days, interest, VAT, accrual period, and suspension period
        //                    leaddays.Text = reader["pt_lead_days"] != DBNull.Value ? reader["pt_lead_days"].ToString() : "0";
        //                    interest.Text = reader["pt_interest"] != DBNull.Value ? reader["pt_interest"].ToString() : "0";
        //                    vat.Text = reader["pt_tax"] != DBNull.Value ? reader["pt_tax"].ToString() : "0";
        //                    acc_per.Text = reader["pt_accrual_period"] != DBNull.Value ? reader["pt_accrual_period"].ToString() : "0";
        //                    susp_per.Text = reader["pt_susp_period"] != DBNull.Value ? reader["pt_susp_period"].ToString() : "0";
        //                }
        //            }
        //        }
        //    }
        //}

        protected void showPaymentTerm()
        {
            using (var conn = new NpgsqlConnection(con))
            {
                conn.Open();

                // Query to get all relevant fields from the PAYMENT_TERM table
                string query = "SELECT pt_tax FROM PAYMENT_TERM";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Set the values for lead days, interest, VAT, accrual period, and suspension period
                            vat.Text = reader["pt_tax"] != DBNull.Value ? reader["pt_tax"].ToString() : "0";
                        }
                    }
                }
            }
        }

        protected void UpdateWasteCategory(object sender, EventArgs e)
        {
            int id;
            if (!int.TryParse(txtbxID.Text, out id))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showError",
                    "Swal.fire({ icon: 'error', title: 'Invalid ID', text: 'Please enter a valid numeric ID.', confirmButtonColor: '#dc3545' });", true);
                return;
            }

            string name = txtbxnewName.Text;
            string unit = txtbxnewUnit.Text;
            double limit;
            decimal price;

            if (!double.TryParse(txtLimit.Text, out limit) || limit <= 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showValidationError",
                    "Swal.fire({ icon: 'error', title: 'Invalid Limit', text: 'Max Limit must be a number greater than zero.', confirmButtonColor: '#dc3545' });", true);
                return;
            }

            if (!decimal.TryParse(txtbxnewPrice.Text, out price) || price <= 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showValidationError",
                    "Swal.fire({ icon: 'error', title: 'Invalid Price', text: 'Price must be a number greater than zero.', confirmButtonColor: '#dc3545' });", true);
                return;
            }

            string description = txtDescription.Text.Trim();
            if (string.IsNullOrWhiteSpace(description))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showError",
                    "Swal.fire({ icon: 'error', title: 'Empty Description', text: 'Description cannot be empty.', confirmButtonColor: '#dc3545' });", true);
                return;
            }

            using (var conn = new NpgsqlConnection(con))
            {
                conn.Open();

                // Check if the wc_name already exists in the database (excluding the current record)
                string checkQuery = "SELECT COUNT(*) FROM waste_category WHERE wc_name = @name AND wc_id <> @id";
                using (var checkCmd = new NpgsqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@name", name);
                    checkCmd.Parameters.AddWithValue("@id", id);

                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (count > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showDuplicateAlert",
                            "Swal.fire({ icon: 'error', title: 'Duplicate Found', text: 'This waste category name already exists in the database.', confirmButtonColor: '#dc3545' });", true);
                        ModalPopupExtender2.Hide();
                        return;
                    }
                }

                // Proceed with the update if no duplicates are found
                string updateQuery = "UPDATE waste_category SET wc_name = @name, wc_unit = @unit, wc_price = @price, wc_max = @max, wc_desc = @desc WHERE wc_id = @id";
                using (var updateCmd = new NpgsqlCommand(updateQuery, conn))
                {
                    updateCmd.Parameters.AddWithValue("@name", name);
                    updateCmd.Parameters.AddWithValue("@unit", unit);
                    updateCmd.Parameters.AddWithValue("@price", price);
                    updateCmd.Parameters.AddWithValue("@max", limit);
                    updateCmd.Parameters.AddWithValue("@desc", description);
                    updateCmd.Parameters.AddWithValue("@id", id);

                    int rowsAffected = updateCmd.ExecuteNonQuery(); // Execute once
                    if (rowsAffected >= 1)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showSuccessAlert",
                            "Swal.fire({ icon: 'success', title: 'Updated Successfully', text: 'Waste category updated successfully!', confirmButtonColor: '#28a745' });", true);

                        // Close the modal and refresh the data
                        ModalPopupExtender2.Hide();
                        WasteCatList(); // Ensure this method binds the updated data
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
                            "Swal.fire({ icon: 'error', title: 'Unsuccessful!', text: 'There was an error updating the waste category!', confirmButtonColor: '#dc3545' });", true);
                        ModalPopupExtender2.Hide();
                    }
                }
            }
        }


        //protected void UpdateWasteCategory(object sender, EventArgs e)
        //{
        //    int id;
        //    if (!int.TryParse(txtbxID.Text, out id))
        //    {
        //        ScriptManager.RegisterStartupScript(this, GetType(), "showError",
        //            "Swal.fire({ icon: 'error', title: 'Invalid ID', text: 'Please enter a valid numeric ID.', confirmButtonColor: '#dc3545' });", true);
        //        return;
        //    }

        //    string name = txtbxnewName.Text;
        //    string unit = txtbxnewUnit.Text;
        //    double limit;
        //    decimal price;

        //    if (!double.TryParse(txtLimit.Text, out limit) || !decimal.TryParse(txtbxnewPrice.Text, out price))
        //    {
        //        ScriptManager.RegisterStartupScript(this, GetType(), "showValidationError",
        //            "Swal.fire({ icon: 'error', title: 'Invalid Input', text: 'Please ensure limit and price are valid numeric values.', confirmButtonColor: '#dc3545' });", true);
        //        return;
        //    }

        //    string description = txtDescription.Text.Trim();

        //    if (string.IsNullOrWhiteSpace(description))
        //    {
        //        ScriptManager.RegisterStartupScript(this, GetType(), "showError",
        //            "Swal.fire({ icon: 'error', title: 'Empty Description', text: 'Description cannot be empty.', confirmButtonColor: '#dc3545' });", true);
        //        return;
        //    }

        //    using (var conn = new NpgsqlConnection(con))
        //    {
        //        conn.Open();

        //        // Check if the wc_name already exists in the database (excluding the current record)
        //        string checkQuery = "SELECT COUNT(*) FROM waste_category WHERE wc_name = @name AND wc_id <> @id";
        //        using (var checkCmd = new NpgsqlCommand(checkQuery, conn))
        //        {
        //            checkCmd.Parameters.AddWithValue("@name", name);
        //            checkCmd.Parameters.AddWithValue("@id", id);

        //            int count = Convert.ToInt32(checkCmd.ExecuteScalar());
        //            if (count > 0)
        //            {
        //                ScriptManager.RegisterStartupScript(this, GetType(), "showDuplicateAlert",
        //                    "Swal.fire({ icon: 'error', title: 'Duplicate Found', text: 'This waste category name already exists in the database.', confirmButtonColor: '#dc3545' });", true);
        //                ModalPopupExtender2.Hide();
        //                return;
        //            }
        //        }

        //        // Proceed with the update if no duplicates are found
        //        string updateQuery = "UPDATE waste_category SET wc_name = @name, wc_unit = @unit, wc_price = @price, wc_max = @max, wc_desc = @desc WHERE wc_id = @id";
        //        using (var updateCmd = new NpgsqlCommand(updateQuery, conn))
        //        {
        //            updateCmd.Parameters.AddWithValue("@name", name);
        //            updateCmd.Parameters.AddWithValue("@unit", unit);
        //            updateCmd.Parameters.AddWithValue("@price", price);
        //            updateCmd.Parameters.AddWithValue("@max", limit);
        //            updateCmd.Parameters.AddWithValue("@desc", description);
        //            updateCmd.Parameters.AddWithValue("@id", id);

        //            int rowsAffected = updateCmd.ExecuteNonQuery(); // Execute once
        //            if (rowsAffected >= 1)
        //            {
        //                ScriptManager.RegisterStartupScript(this, GetType(), "showSuccessAlert",
        //                    "Swal.fire({ icon: 'success', title: 'Updated Successfully', text: 'Waste category updated successfully!', confirmButtonColor: '#28a745' });", true);

        //                // Close the modal and refresh the data
        //                ModalPopupExtender2.Hide();
        //                WasteCatList(); // Ensure this method binds the updated data
        //            }
        //            else
        //            {
        //                ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
        //                    "Swal.fire({ icon: 'error', title: 'Unsuccessful!', text: 'There was an error updating the waste category!', confirmButtonColor: '#dc3545' });", true);
        //                ModalPopupExtender2.Hide();
        //            }
        //        }
        //    }
        //}



        //protected void UpdateWasteCategory(object sender, EventArgs e)
        //{
        //    int id;
        //    if (!int.TryParse(txtbxID.Text, out id))
        //    {
        //        ScriptManager.RegisterStartupScript(this, GetType(), "showError",
        //            "Swal.fire({ icon: 'error', title: 'Invalid ID', text: 'Please enter a valid numeric ID.', confirmButtonColor: '#dc3545' });", true);
        //        return;
        //    }

        //    string name = txtbxnewName.Text;
        //    string unit = txtbxnewUnit.Text;
        //    double limit = double.Parse(txtLimit.Text);
        //    decimal price = decimal.Parse(txtbxnewPrice.Text);

        //    using (var conn = new NpgsqlConnection(con))
        //    {
        //        conn.Open();

        //        // Check if the wc_name already exists in the database (excluding the current record)
        //        string checkQuery = "SELECT COUNT(*) FROM waste_category WHERE wc_name = @name AND wc_id <> @id";
        //        using (var checkCmd = new NpgsqlCommand(checkQuery, conn))
        //        {
        //            checkCmd.Parameters.AddWithValue("@name", name);
        //            checkCmd.Parameters.AddWithValue("@id", id);

        //            int count = Convert.ToInt32(checkCmd.ExecuteScalar());
        //            if (count > 0)
        //            {
        //                // If a duplicate wc_name exists, show an error dialog
        //                ScriptManager.RegisterStartupScript(this, GetType(), "showDuplicateAlert",
        //                    "Swal.fire({ icon: 'error', title: 'Duplicate Found', text: 'This waste category name already exists in the database.', confirmButtonColor: '#dc3545' });", true);
        //                ModalPopupExtender2.Hide();
        //                return;
        //            }
        //        }

        //        // Proceed with the update if no duplicates are found
        //        string updateQuery = "UPDATE waste_category SET wc_name = @name, wc_unit = @unit, wc_price = @price, wc_max = @max WHERE wc_id = @id";
        //        using (var updateCmd = new NpgsqlCommand(updateQuery, conn))
        //        {
        //            updateCmd.Parameters.AddWithValue("@name", name);
        //            updateCmd.Parameters.AddWithValue("@unit", unit);
        //            updateCmd.Parameters.AddWithValue("@price", price);
        //            updateCmd.Parameters.AddWithValue("@id", id);
        //            updateCmd.Parameters.AddWithValue("@max", limit);

        //            int rowsAffected = updateCmd.ExecuteNonQuery(); // Execute once
        //            if (rowsAffected >= 1)
        //            {
        //                // Success dialog after updating the waste category
        //                ScriptManager.RegisterStartupScript(this, GetType(), "showSuccessAlert",
        //                    "Swal.fire({ icon: 'success', title: 'Updated Successfully', text: 'Waste category updated successfully!', confirmButtonColor: '#28a745' });", true);

        //                // Close the modal and refresh the data
        //                ModalPopupExtender2.Hide();
        //                WasteCatList(); // Ensure this method is binding the updated data
        //            }
        //            else
        //            {
        //                ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
        //                    "Swal.fire({ icon: 'error', title: 'Unsuccessful!', text: 'There was an error updating the waste category!', background: '#f8d7da', confirmButtonColor: '#dc3545' });", true);
        //                ModalPopupExtender2.Hide();
        //            }
        //        }
        //    }
        //}


        //    protected void UpdateWasteCategory(object sender, EventArgs e)
        //    {
        //        int id;
        //        if (!int.TryParse(txtbxID.Text, out id))
        //        {
        //            ScriptManager.RegisterStartupScript(this, GetType(), "showError",
        //                "Swal.fire({ icon: 'error', title: 'Invalid ID', text: 'Please enter a valid numeric ID.', confirmButtonColor: '#dc3545' });", true);
        //            return;
        //        }

        //        string name = txtbxnewName.Text;
        //        string unit = txtbxnewUnit.Text;
        //        double limit = double.Parse(txtLimit.Text);
        //        decimal price = decimal.Parse(txtbxnewPrice.Text);

        //        using (var conn = new NpgsqlConnection(con))
        //        {
        //            conn.Open();

        //            // Check if the wc_name already exists in the database (excluding the current record)
        //            string checkQuery = "SELECT COUNT(*) FROM waste_category WHERE wc_name = @name AND wc_id <> @id";
        //            using (var checkCmd = new NpgsqlCommand(checkQuery, conn))
        //            {
        //                checkCmd.Parameters.AddWithValue("@name", name);
        //                checkCmd.Parameters.AddWithValue("@id", id);

        //                int count = Convert.ToInt32(checkCmd.ExecuteScalar());
        //                if (count > 0)
        //                {
        //                    // If a duplicate wc_name exists, show an error dialog
        //                    ScriptManager.RegisterStartupScript(this, GetType(), "showDuplicateAlert",
        //                        "Swal.fire({ icon: 'error', title: 'Duplicate Found', text: 'This waste category name already exists in the database.', confirmButtonColor: '#dc3545' });", true);
        //                    ModalPopupExtender2.Hide();

        //                    return;
        //                }
        //            }

        //            // Proceed with the update if no duplicates are found
        //            string updateQuery = "UPDATE waste_category SET wc_name = @name, wc_unit = @unit, wc_price = @price, wc_max = @max WHERE wc_id = @id";
        //            using (var updateCmd = new NpgsqlCommand(updateQuery, conn))
        //            {
        //                updateCmd.Parameters.AddWithValue("@name", name);
        //                updateCmd.Parameters.AddWithValue("@unit", unit);
        //                updateCmd.Parameters.AddWithValue("@price", price);
        //                updateCmd.Parameters.AddWithValue("@id", id);
        //                updateCmd.Parameters.AddWithValue("@max", limit);

        //                updateCmd.ExecuteNonQuery();
        //                int rowsAffected = updateCmd.ExecuteNonQuery();
        //                if (rowsAffected >= 1)
        //                {
        //                    // Success dialog after updating the waste category
        //                    ScriptManager.RegisterStartupScript(this, GetType(), "showSuccessAlert",
        //                    "Swal.fire({ icon: 'success', title: 'Updated Successfully', text: 'Waste category updated successfully!', confirmButtonColor: '#28a745' });", true);

        //                    // Refresh data or close modal
        //                    showPaymentTerm();
        //                    ModalPopupExtender2.Hide();
        //                    WasteCatList();
        //                }
        //                else
        //                {
        //                    ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
        //"Swal.fire({ icon: 'error', title: 'Unsuccessful!', text: 'There's an error in updating!', background: '#f8d7da', confirmButtonColor: '#dc3545' });", true);
        //                    ModalPopupExtender2.Hide();

        //                }
        //                //// Success dialog after updating the waste category
        //                //ScriptManager.RegisterStartupScript(this, GetType(), "showSuccessAlert",
        //                //    "Swal.fire({ icon: 'success', title: 'Updated Successfully', text: 'Waste category updated successfully!', confirmButtonColor: '#28a745' });", true);

        //                // Refresh data or close modal
        //                showPaymentTerm();
        //                ModalPopupExtender2.Hide();
        //                WasteCatList();
        //            }
        //        }
        //    }



        //// Update the new name or password of the employee account
        //protected void UpdateWasteCategory(object sender, EventArgs e)
        //{
        //    int id;
        //    if (!int.TryParse(txtbxID.Text, out id))
        //    {
        //        Response.Write("<script>alert('Invalid ID format.')</script>");
        //        return;
        //    }
        //    //int id = int.Parse(txtbxID.Text);
        //    string name = txtbxnewName.Text;
        //    string unit = txtbxnewUnit.Text;
        //    double limit = double.Parse(txtLimit.Text);
        //    decimal price = decimal.Parse(txtbxnewPrice.Text);

        //    using (var conn = new NpgsqlConnection(con))
        //    {
        //        conn.Open();
        //        string updateQuery = "UPDATE waste_category SET wc_name = @name, wc_unit = @unit, wc_price = @price, wc_max = @max WHERE wc_id = @id";

        //        using (var updateCmd = new NpgsqlCommand(updateQuery, conn))
        //        {
        //            updateCmd.Parameters.AddWithValue("@name", name);
        //            updateCmd.Parameters.AddWithValue("@unit", unit);
        //            updateCmd.Parameters.AddWithValue("@price", price);
        //            updateCmd.Parameters.AddWithValue("@id", id);
        //            updateCmd.Parameters.AddWithValue("@max", limit);

        //            updateCmd.ExecuteNonQuery();
        //            showPaymentTerm();
        //            ModalPopupExtender2.Hide();
        //            WasteCatList();
        //        }
        //    }
        //    ModalPopupExtender2.Hide();
        //    // Optionally refresh the grid or list showing waste categories
        //    // Response.Redirect(Request.RawUrl); // Example to refresh the page after update
        //    Response.Write("<script>alert('Waste category updated successfully!');</script>");
        //    showPaymentTerm();
        //    WasteCatList();
        //}

    }
}