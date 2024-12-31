using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using WebControls = System.Web.UI.WebControls;
using System.IO;
using System.Web.UI.WebControls;
using iText.Kernel.Pdf;
using ITextDocument = iText.Layout.Document;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Colors;
using iText.IO.Image;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Layout.Borders;
using System.Web.Services;


using System.Linq;
using static Capstone.PaymentController;
using System.Data.SqlClient;




namespace Capstone
{
    public partial class BO_Billing : System.Web.UI.Page
    {
        public static readonly string con = "Server=localhost;Port=5432;User Id=postgres;Password=123456;Database=trashtrack";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadProfile();
                LoadBookingList();
                GeneratedBillList();
                PopulateWasteCategory();
                LoadBookingWasteData();
                //DetailsLoadBookingWasteData();
                hfActiveTab.Value = "#tab1"; // Set Tab 1 as the default
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
                    //GetUnreadNotificationCount();
                    GetUnreadNotificationCount();
                }
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
                        GetUnreadNotificationCount();

                    }
                    else
                    {
                        // Optionally show an error message
                        ScriptManager.RegisterStartupScript(this, GetType(), "updateError",
                            "Swal.fire({ icon: 'error', title: 'Error', text: 'Unable to delete the notification.', confirmButtonColor: '#dc3545' });", true);
                    }
                }
            }

            // Refresh the notifications list
            BindNotifications();

            //// Update the UpdatePanel to reflect the changes on the UI
            //UpdatePanelNotifications1.Update();
        }



        //protected void DeleteNotification_Click(object sender, EventArgs e)
        //{
        //    // Get the ID of the notification to be deleted from the CommandArgument
        //    LinkButton btnDelete = (LinkButton)sender;
        //    string notifId = btnDelete.CommandArgument;

        //    // Logic to mark the notification as deleted in the database
        //    DeleteNotificationFromDatabase(notifId);
        //    GetUnreadNotificationCount();
        //    // Refresh the notification list by re-binding the repeater
        //    BindNotifications();

        //    // Update the UpdatePanel to reflect the changes on the UI
        //    UpdatePanelNotifications1.Update();
        //}

        private void DeleteNotificationFromDatabase(string notifId)
        {
            string query = "UPDATE notification SET notif_status = 'Deleted', notif_read = true WHERE notif_id = @notifId AND notif_type IN ('slip', 'payment');";
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

        public class Notification1
        {
            public int NotifId1 { get; set; }
            public string NotifMessage1 { get; set; }
            public DateTime NotifCreatedAt1 { get; set; }
            public bool NotifRead1 { get; set; }
            public string NotifType1 { get; set; }
            public int CusId1 { get; set; }
            public string NotifStatus1 { get; set; }
            public string NotifIcon1 { get; set; }  // Ensure this property is included
        }

        /// <summary>
        /// MUGANAAAA LATEST 11/30/24 12:21
        /// </summary>
        //private void BindNotifications()
        //{
        //    var notifications = GetNotificationsFromDb();  // This gets a List<Notification>
        //    NotificationRepeater.DataSource = notifications;
        //    NotificationRepeater.DataBind();
        //}
        //private List<Notification> GetNotificationsFromDb()
        //{
        //    string query = "SELECT notif_id, notif_message, notif_created_at, notif_read, notif_type FROM notification WHERE notif_type = 'request verification' ORDER BY notif_created_at DESC;";
        //    var notifications = new List<Notification>();

        //    using (var connection = new NpgsqlConnection(con))
        //    {
        //        connection.Open();
        //        using (var command = new NpgsqlCommand(query, connection))
        //        using (var reader = command.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                notifications.Add(new Notification
        //                {
        //                    NotifId = reader.GetInt32(0),
        //                    NotifMessage = reader.GetString(1),
        //                    NotifCreatedAt = reader.GetDateTime(2),
        //                    NotifRead = reader.GetBoolean(3),
        //                    NotifType = reader.GetString(4)
        //                });
        //            }
        //        }
        //    }

        //    return notifications;
        //}
        //protected void Notification_Click(object sender, EventArgs e)
        //{
        //    LinkButton btn = sender as LinkButton;
        //    if (btn != null)
        //    {
        //        int notifId = Convert.ToInt32(btn.CommandArgument);
        //        MarkNotificationAsRead(notifId);
        //        Response.Redirect($"SAM_AccountManCustomers.aspx");

        //        // Rebind notifications to reflect the change
        //        BindNotifications();
        //    }
        //}

        //private void MarkNotificationAsRead(int notifId)
        //{
        //    string query = "UPDATE notification SET notif_read = true WHERE notif_id = @notifId;";
        //    using (var connection = new NpgsqlConnection(con))
        //    {
        //        connection.Open();
        //        using (var command = new NpgsqlCommand(query, connection))
        //        {
        //            command.Parameters.AddWithValue("@notifId", notifId);
        //            command.ExecuteNonQuery();
        //        }
        //    }
        //}

        ////Helper function to get the corresponding icon based on the status
        //protected string GetNotificationIcon(string status)
        //{
        //    switch (status)
        //    {
        //        case "Pending":
        //            return "bi bi-exclamation-circle text-warning";
        //        case "Declined":
        //            return "bi bi-x-circle text-danger";
        //        case "Approved":
        //            return "bi bi-check-circle text-success";
        //        default:
        //            return "bi bi-info-circle text-primary";
        //    }
        //}
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

        //not real-time
        //[WebMethod]
        protected void LoadBookingList()
        {
            using (var db = new NpgsqlConnection(con))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    // Query to fetch booking data from the database
                    cmd.CommandText = @"SELECT 
                        b.bk_id, 
                        b.bk_date,
                        b.bk_created_at,
                        b.bk_fullname,
                        b.bk_status, 
                        b.bk_waste_scale_slip,
                        CONCAT(b.bk_street, ', ', b.bk_brgy, ', ', b.bk_city, ', ', b.bk_province, ' ', b.bk_postal) AS location,
                        c.cus_id,
                        c.cus_email
                    FROM 
                        booking b
                    JOIN 
                        customer c ON b.cus_id = c.cus_id
                    WHERE 
                        b.bk_status NOT IN ('Completed', 'Billed', 'Cancelled', 'Failed') 
                    ORDER BY 
                        b.bk_id DESC, b.bk_date DESC";


                    // Execute the query and bind the results to the GridView
                    DataTable bookingsDataTable = new DataTable();
                    NpgsqlDataAdapter bookingsAdapter = new NpgsqlDataAdapter(cmd);
                    bookingsAdapter.Fill(bookingsDataTable);

                    // Bind the data to the GridView
                    gridViewBookings.DataSource = bookingsDataTable;
                    gridViewBookings.DataBind();

                }
                db.Close();
            }
        }

        protected void GeneratedBillList()
        {
            using (var db = new NpgsqlConnection(con))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    //gb.gb_total_amnt_interest, 
                    //p.p_date_paid DESC NULLS LAST,

                    // SQL query to fetch bill and payment data
                    cmd.CommandText = @"
                                        SELECT 
                                            gb.gb_id, 
                                            gb.gb_date_issued, 
                                            gb.gb_date_due, 
                                            gb.bk_id, 
                                            gb.gb_total_sales, 
                                            gb.gb_status, 
                                            COALESCE(p.p_amount, 0) AS p_amount, 
                                            COALESCE(p.p_method, 'N/A') AS p_method, 
                                            p.p_date_paid, 
                                            p.p_checkout_id, 
                                            p.p_trans_id
                                        FROM 
                                            generate_bill gb
                                        LEFT JOIN 
                                            payment p ON gb.gb_id = p.gb_id
                                        ORDER BY 
                                            gb.gb_date_issued DESC,
                                            gb.gb_id DESC
                                    ";



                    // Execute the query and fill the DataTable
                    DataTable bookingsDataTable = new DataTable();
                    NpgsqlDataAdapter bookingsAdapter = new NpgsqlDataAdapter(cmd);
                    bookingsAdapter.Fill(bookingsDataTable);

                    // Bind data to the GridView
                    gridView1.DataSource = bookingsDataTable;
                    gridView1.DataBind();
                }
                db.Close();
            }
        }


        //protected void GeneratedBillList()
        //{
        //    using (var db = new NpgsqlConnection(con))
        //    {
        //        db.Open();
        //        using (var cmd = db.CreateCommand())
        //        {
        //            cmd.CommandType = CommandType.Text;
        //            //gb.gb_total_amnt_interest, 
        //            //p.p_date_paid DESC NULLS LAST,

        //            // SQL query to fetch bill and payment data
        //            cmd.CommandText = @"
        //                                SELECT 
        //                                    gb.gb_id, 
        //                                    gb.gb_date_issued, 
        //                                    gb.gb_date_due, 
        //                                    gb.bk_id, 
        //                                    gb.gb_total_sales, 
        //                                    gb.gb_status, 
        //                                    COALESCE(p.p_amount, 0) AS p_amount, 
        //                                    COALESCE(p.p_method, 'N/A') AS p_method, 
        //                                    COALESCE(p.p_date_paid, NULL) AS p_date_paid, 
        //                                    COALESCE(p.p_checkout_id, 'N/A') AS p_checkout_id,
        //                                    COALESCE(p.p_trans_id, 'N/A') AS p_trans_id
        //                                FROM 
        //                                    generate_bill gb
        //                                LEFT JOIN 
        //                                    payment p ON gb.gb_id = p.gb_id
        //                                ORDER BY 
        //                                    gb.gb_date_issued DESC,
        //                                    gb.gb_id DESC;
        //                            ";



        //            // Execute the query and fill the DataTable
        //            DataTable bookingsDataTable = new DataTable();
        //            NpgsqlDataAdapter bookingsAdapter = new NpgsqlDataAdapter(cmd);
        //            bookingsAdapter.Fill(bookingsDataTable);

        //            // Bind data to the GridView
        //            gridView1.DataSource = bookingsDataTable;
        //            gridView1.DataBind();
        //        }
        //        db.Close();
        //    }
        //}


        protected void Remove_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;

            // Get the bw_id (Booking Waste ID) from CommandArgument
            int bwId = Convert.ToInt32(btn.CommandArgument);
            int bookingId = 0; // Initialize bookingId

            try
            {
                using (var db = new NpgsqlConnection(con))
                {
                    db.Open();

                    // First, get the associated booking ID
                    using (var getBookingIdCmd = db.CreateCommand())
                    {
                        getBookingIdCmd.CommandText = "SELECT bk_id FROM BOOKING_WASTE WHERE bw_id = @bwId";
                        getBookingIdCmd.Parameters.AddWithValue("@bwId", bwId);

                        // Execute the query to retrieve the booking ID
                        object result = getBookingIdCmd.ExecuteScalar();
                        if (result != null)
                        {
                            bookingId = Convert.ToInt32(result);
                            PopulateWasteCategory();
                        }
                        else
                        {
                            // Handle case where no booking ID is found
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
                                "swal('Not Found!', 'No associated booking ID found for this Booking Waste record.', 'warning')", true);
                            return; // Exit if no booking ID found
                        }
                    }

                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "DELETE FROM BOOKING_WASTE WHERE bw_id = @id";
                        cmd.Parameters.AddWithValue("@id", bwId);

                        var ctr = cmd.ExecuteNonQuery();
                        if (ctr >= 1)
                        {
                            PopulateWasteCategory();
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
                                "swal('Record Deleted!', 'Booking Waste record deleted successfully!', 'success')", true);

                            // Refresh the data for the associated booking ID
                            LoadBookingWasteData(); // Call the method to refresh the data
                        }
                        else
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
                                "swal('Not Found!', 'No record found to delete.', 'warning')", true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
                        "swal('Unsuccessful!', '" + ex.Message + "', 'error')", true);
            }
        }

        //OPEN EDIT BOOK WASTE PANEL
        protected void editBookWaste_Click(object sender, EventArgs e)
        {
            LinkButton btn = sender as LinkButton;

            if (btn != null)
            {
                // Get the bw_id from CommandArgument
                int bw_id = Convert.ToInt32(btn.CommandArgument);

                //// Store bw_id in the hidden field for later use
                hfBwID.Value = bw_id.ToString(); // Store the id in the hidden field

                //// Display the bw_id from the hidden field in the textbox
                //txtbwID.Text = hfBwID.Value;
                try
                {
                    using (var db = new NpgsqlConnection(con))
                    {
                        db.Open();
                        using (var cmd = db.CreateCommand())
                        {
                            cmd.CommandText = "SELECT * FROM BOOKING_WASTE WHERE bw_id = @bw_id";
                            cmd.Parameters.AddWithValue("bw_id", bw_id);
                            using (NpgsqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    txtName.Text = reader["bw_name"].ToString();
                                    txtbwUnit.Text = reader["bw_unit"].ToString();
                                    txtTotalUnit.Text = reader["bw_total_unit"].ToString();
                                    txtUnitPrice.Text = reader["bw_price"].ToString();
                                    txtTotalUnitPrice.Text = reader["bw_total_price"].ToString();
                                    //Response.Write("<script>alert('Yeehaw!')</script>");
                                }
                            }

                        }

                    }
                }
                catch
                {

                }
                updatePanel.Update();

                // Show the modal popup
                ModalPopupExtender2.Show();
            }
        }

        protected void btnSaveChanges_Click(object sender, EventArgs e)
        {
            //int bw_id;
            int bw_id = Convert.ToInt32(hfBwID.Value);

            //// Retrieve the bw_id from the hidden field
            //if (!int.TryParse(hfBwID.Value, out bw_id))
            //{
            //    Response.Write("<script>alert('Invalid ID format.')</script>");
            //    return;
            //}

            // Fetch values from the textboxes and dropdown
            //string bw_name = ddlbwName.SelectedItem.Text; // Waste type name from the dropdown
            string bw_name = txtName.Text;
            string bw_unit = txtbwUnit.Text; // Waste unit from the textbox
            double bw_total_unit = Convert.ToDouble(txtTotalUnit.Text); // Total unit from the textbox
            double bw_price = Convert.ToDouble(txtUnitPrice.Text); // Price per unit from the textbox
            double bw_total_price = Convert.ToDouble(txtTotalUnitPrice.Text); // Total price from the textbox

            // Update query for booking_waste based on bw_id
            string updateBookingWasteQuery = @"
        UPDATE booking_waste 
        SET bw_total_unit = @bw_total_unit,
            bw_total_price = @bw_total_price
        WHERE bw_id = @bw_id";

            // Execute the update query
            using (NpgsqlConnection conn = new NpgsqlConnection(con))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand(updateBookingWasteQuery, conn))
                {
                    // Add parameters
                    cmd.Parameters.AddWithValue("@bw_id", bw_id);
                    cmd.Parameters.AddWithValue("@bw_name", bw_name);
                    cmd.Parameters.AddWithValue("@bw_unit", bw_unit);
                    cmd.Parameters.AddWithValue("@bw_total_unit", bw_total_unit);
                    cmd.Parameters.AddWithValue("@bw_price", bw_price);
                    cmd.Parameters.AddWithValue("@bw_total_price", bw_total_price);

                    conn.Open();
                    cmd.ExecuteNonQuery(); // Execute the command
                    updatePanel1.Update(); // If using UpdatePanel, force an update to reflect changes

                    LoadBookingWasteData();
                }
            }

            // Optionally, refresh the page or data after update
            this.ModalPopupExtender2.Hide(); // Close the modal after saving
        }

        //OPEN ADD BOOK WASTE PANEL
        protected void openAddBW_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            txtbwID1.Text = TextBox1.Text;
            this.ModalPopupExtender3.Show();

        }

        protected void addBookWaste_Click(object sender, EventArgs e)
        {
            try
            {
                // Fetch values from the textboxes
                int bk_id = Convert.ToInt32(TextBox1.Text); 
                string bw_name = ddlbwName1.SelectedItem.Text; 
                string bw_unit = txtbwUnit1.Text; 
                double bw_total_unit = Convert.ToDouble(txtTotalUnit1.Text); 
                double bw_price = Convert.ToDouble(txtUnitPrice1.Text); 
                double bw_total_price = Convert.ToDouble(txtTotalUnitPrice1.Text); 

                // Fetch the wc_id from the dropdown selection
                int wc_id = Convert.ToInt32(ddlbwName1.SelectedValue); 
                // Validate that Total Unit is greater than 0
                if (!double.TryParse(txtTotalUnit1.Text, out bw_total_unit) || bw_total_unit <= 0)
                {
                    // Display an error message and stop submission
                    ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
                        "Swal.fire({ icon: 'error', title: 'Invalid Input', text: 'Total Unit must be greater than 0.', background: '#f8d7da', confirmButtonColor: '#f5c6cb' });",
                        true);
                    return; // Prevent form submission
                }

                // Calculate the total price
                bw_total_price = bw_total_unit * bw_price;

                using (var db = new NpgsqlConnection(con))
                {
                    db.Open();

                    // Verify that the booking ID exists
                    using (var checkBookingCmd = db.CreateCommand())
                    {
                        checkBookingCmd.CommandText = "SELECT COUNT(*) FROM BOOKING WHERE bk_id = @bkId";
                        checkBookingCmd.Parameters.AddWithValue("@bkId", bk_id);

                        object result = checkBookingCmd.ExecuteScalar();
                        if (result == null || Convert.ToInt32(result) == 0)
                        {
                            // Display error if no associated booking ID is found
                            ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
                                "Swal.fire({ icon: 'error', title: 'Not Found!', text: 'No associated booking ID found for this Booking Waste.', background: '#f8d7da', confirmButtonColor: '#f5c6cb' });",
                                true);
                            return; // Prevent form submission
                        }
                    }

                    // Insert the new booking waste record, including wc_id
                    string insertBookingWasteQuery = @"
                INSERT INTO booking_waste 
                (bk_id, bw_name, bw_unit, bw_total_unit, bw_price, bw_total_price, wc_id)
                VALUES 
                (@bk_id, @bw_name, @bw_unit, @bw_total_unit, @bw_price, @bw_total_price, @wc_id)";

                    using (var cmd = new NpgsqlCommand(insertBookingWasteQuery, db))
                    {
                        // Add parameters to the command, including wc_id
                        cmd.Parameters.AddWithValue("@bk_id", bk_id);
                        cmd.Parameters.AddWithValue("@bw_name", bw_name);
                        cmd.Parameters.AddWithValue("@bw_unit", bw_unit);
                        cmd.Parameters.AddWithValue("@bw_total_unit", bw_total_unit);
                        cmd.Parameters.AddWithValue("@bw_price", bw_price);
                        cmd.Parameters.AddWithValue("@bw_total_price", bw_total_price);
                        cmd.Parameters.AddWithValue("@wc_id", wc_id); // Add the wc_id

                        var ctr = cmd.ExecuteNonQuery();
                        if (ctr >= 1)
                        {
                            LoadBookingWasteData();
                            updatePanel1.Update(); // Update the UpdatePanel to reflect changes
                            PopulateWasteCategory();
                            ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
                                "Swal.fire({ icon: 'success', title: 'Success!', text: 'Booking Waste record added successfully.', background: '#e9f7ef', confirmButtonColor: '#28a745' });",
                                true);
                            txtbwUnit1.Text = string.Empty;
                            txtTotalUnit1.Text = string.Empty;
                            txtUnitPrice1.Text = string.Empty;
                            txtTotalUnitPrice1.Text = string.Empty;
                            ddlbwName1.SelectedIndex = 0; // Reset the dropdown to its default selection
                        }
                        else
                        {
                            PopulateWasteCategory();
                            ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
                                "Swal.fire({ icon: 'warning', title: 'Failed!', text: 'Failed to add Booking Waste record.', background: '#fff3cd', confirmButtonColor: '#ffc107' });",
                                true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Display error for unexpected issues
                ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
                    $"Swal.fire({{ icon: 'error', title: 'Error!', text: '{ex.Message}', background: '#f8d7da', confirmButtonColor: '#f5c6cb' }});",
                    true);
            }
            finally
            {
                LoadBookingWasteData();
                PopulateWasteCategory();
                this.ModalPopupExtender3.Hide();
                txtTotalUnit1.Enabled = false;
                txtbwUnit1.Text = string.Empty;
                txtTotalUnit1.Text = string.Empty;
                txtUnitPrice1.Text = string.Empty;
                txtTotalUnitPrice1.Text = string.Empty;
                ddlbwName1.SelectedIndex = 0; // Reset the dropdown to its default selection
            }
        }


        private void LoadBookingWasteData()
        {
            int bookingId;

            // Try to convert the booking ID from TextBox1 and handle potential format errors
            if (!int.TryParse(TextBox1.Text, out bookingId))
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "swal('Invalid Input!', 'Please enter a valid booking ID.', 'error')", true);
                return; // Exit the method if the input is not valid
            }
            try
            {
                using (var db = new NpgsqlConnection(con))
                {
                    db.Open();

                    // Query to get booking details and bind to the GridView
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = @"SELECT bw.bw_id, bw.bw_name, bw.bw_unit, bw.bw_total_unit, bw.bw_price, bw.bw_total_price, bw.bk_id
                                    FROM BOOKING_WASTE bw
                                    INNER JOIN BOOKING b ON bw.bk_id = b.bk_id
                                    WHERE bw.bk_id = @bkId AND b.bk_status != 'Completed' ORDER BY bw.bw_id";

                        cmd.Parameters.AddWithValue("@bkId", bookingId);

                        DataTable bookingsDataTable = new DataTable();
                        NpgsqlDataAdapter bookingsAdapter = new NpgsqlDataAdapter(cmd);
                        bookingsAdapter.Fill(bookingsDataTable);
                        gridView2.DataSource = bookingsDataTable;
                        gridView2.DataBind();
                        //gridView3.DataSource = bookingsDataTable;
                        //gridView3.DataBind();
                    }

                    // Query to sum total price for the same booking ID and status != 'Completed'
                    double totalPrice = 0;
                    using (var cmdSum = db.CreateCommand())
                    {
                        cmdSum.CommandType = CommandType.Text;
                        cmdSum.CommandText = @"SELECT SUM(bw.bw_total_price) AS TotalPrice
                                       FROM BOOKING_WASTE bw
                                       INNER JOIN BOOKING b ON bw.bk_id = b.bk_id
                                       WHERE bw.bk_id = @bkId AND b.bk_status != 'Completed'";

                        cmdSum.Parameters.AddWithValue("@bkId", bookingId);
                        object result = cmdSum.ExecuteScalar();
                        if (result != DBNull.Value)
                        {
                            totalPrice = Convert.ToDouble(result);
                            netVatTxt.Text = totalPrice.ToString("N2");
                        }
                        else
                        {
                            netVatTxt.Text = "0";
                        }
                    }

                    // Query to get the payment term details, including tax and periods
                    double ptTax = 0;
                    using (var cmdTax = db.CreateCommand())
                    {
                        cmdTax.CommandType = CommandType.Text;
                        cmdTax.CommandText = @"SELECT pt_tax
                                       FROM payment_term 
                                       LIMIT 1"; // Assuming only one payment term is needed

                        using (var reader = cmdTax.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                ptTax = reader.IsDBNull(0) ? 0 : reader.GetDouble(0);  // Tax percentage
                                // Display payment term details in respective labels
                                taxLabel.Text = "Tax: " + ptTax + "%"; // Displaying tax percentage
                                
                            }
                        }
                    }

                    // Calculate Due Date based on current date + lead days
                    DateTime currentDate = DateTime.Now;
                    DateTime dueDate = DateTime.Now;
                    dueDateTxt.Text = dueDate.ToString("yyyy-MM-ddTHH:mm"); // Set the Due Date in the textbox

                    
                    // Calculate tax amount based on total price
                    double taxAmount = (ptTax / 100) * totalPrice;
                    vatAmntTxt.Text = taxAmount.ToString("N2");

                    // Calculate total sales (total price + tax amount)
                    double totalSales = totalPrice + taxAmount;
                    totSalesTxt.Text = totalSales.ToString("N2");

                    // Set the current date and time in the required format for DateTimeLocal
                    dateTodayTxt.Text = currentDate.ToString("yyyy-MM-ddTHH:mm");
                    PopulateWasteCategory();
                }
                PopulateWasteCategory();
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
                    $"swal('Unsuccessful!', '{ex.Message}', 'error')", true);
            }
        }


        //protected void openViewBill_Click(object sender, EventArgs e)
        //{
        //    LinkButton btn = (LinkButton)sender;
        //    int gb_id = Convert.ToInt32(btn.CommandArgument);
        //    int bk_id = 0;
        //    this.ModalPopupExtender5.Show(); // Show the modal
        //    dateEntered.Text = DateTime.Now.ToString("yyyy-MM-ddTHH:mm");
        //    hfActiveTab.Value = "#tab2";

        //    try
        //    {
        //        using (var db = new NpgsqlConnection(con))
        //        {
        //            db.Open();
        //            using (var cmd = db.CreateCommand())
        //            {
        //                cmd.CommandType = CommandType.Text;
        //                cmd.CommandText = @"
        //        SELECT gb.*, bk.bk_id
        //        FROM generate_bill gb
        //        LEFT JOIN booking bk ON gb.bk_id = bk.bk_id
        //        WHERE gb.gb_id = @gb_id";
        //                cmd.Parameters.AddWithValue("@gb_id", gb_id);

        //                using (var reader = cmd.ExecuteReader())
        //                {
        //                    if (reader.Read())
        //                    {

        //                        //// Display payment term details in respective labels
        //                        //Label4.Text = "Tax: " + ptTax + "%"; // Displaying tax percentage
        //                        //Label5.Text = "Interest: " + ptInterest + "%"; // Displaying interest
        //                        //Label6.Text = "Accrual Period: " + accrualPeriod + " day(s)"; // Displaying accrual period
        //                        //Label7.Text = "Suspension Period: " + suspPeriod + " day(s)"; // Displaying suspension period


        //                        //// Populate fields with data from the `generate_bill` table
        //                        //ptTax = reader["gb_tax"].ToString();
        //                        //ptInterest = reader["gb_interest"].ToString();
        //                        //accrualPeriod = reader["gb_accrual_period"].ToString();
        //                        //suspPeriod = reader["gb_suspend_period"].ToString();

        //                        // Populate fields with data from the `generate_bill` table
        //                        Label4.Text = reader["gb_tax"].ToString();
        //                        Label5.Text = reader["gb_interest"].ToString();
        //                        Label6.Text = reader["gb_accrual_period"].ToString();
        //                        Label7.Text = reader["gb_suspend_period"].ToString();
        //                        TextBox2.Text = reader["gb_id"].ToString();
        //                        Date.Text = Convert.ToDateTime(reader["gb_date_issued"]).ToString("yyyy-MM-ddTHH:mm");
        //                        TextBox7.Text = Convert.ToDateTime(reader["gb_date_due"]).ToString("yyyy-MM-ddTHH:mm");
        //                        TextBox4.Text = reader["gb_net_vat"].ToString();
        //                        TextBox5.Text = reader["gb_vat_amnt"].ToString();
        //                        TextBox6.Text = reader["gb_total_sales"].ToString();
        //                        TextBox8.Text = Convert.ToDateTime(reader["gb_accrual_date"]).ToString("yyyy-MM-ddTHH:mm");
        //                        TextBox9.Text = Convert.ToDateTime(reader["gb_suspend_date"]).ToString("yyyy-MM-ddTHH:mm");
        //                        TextBox10.Text = reader["gb_add_fees"].ToString();
        //                        TextBox11.Text = reader["gb_note"].ToString();

        //                        // Populate the associated `bk_id`
        //                        if (reader["bk_id"] != DBNull.Value)
        //                        {
        //                            bkidviewbill.Value = reader["bk_id"].ToString(); // Set the hidden field value
        //                            bk_id = Convert.ToInt32(reader["bk_id"]);
        //                            DetailsLoadBookingWasteData(bk_id);

        //                        }
        //                        else
        //                        {
        //                            bkidviewbill.Value = "No booking ID associated";
        //                        }
        //                    }
        //                }
        //            }
        //            hfActiveTab.Value = "#tab2"; // Set the default active tab
        //            updatePanel4.Update();       // Update the UpdatePanel
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ScriptManager.RegisterStartupScript(this, GetType(), "showError",
        //            $"Swal.fire({{ icon: 'error', title: 'Error', text: '{ex.Message}' }});", true);
        //    }
        //}
        protected void openViewBill_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            int gb_id = Convert.ToInt32(btn.CommandArgument);
            int bk_id = 0;
            this.ModalPopupExtender5.Show(); // Show the modal
            //dateEntered.Text = DateTime.Now.ToString("yyyy-MM-ddTHH:mm");
            hfActiveTab.Value = "#tab2";

            try
            {
                using (var db = new NpgsqlConnection(con))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = @"
                SELECT gb.*, bk.bk_id
                FROM generate_bill gb
                LEFT JOIN booking bk ON gb.bk_id = bk.bk_id
                WHERE gb.gb_id = @gb_id";
                        cmd.Parameters.AddWithValue("@gb_id", gb_id);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                //// Display payment term details in respective labels
                                //Label4.Text = "Tax: " + ptTax + "%"; // Displaying tax percentage
                                //Label5.Text = "Interest: " + ptInterest + "%"; // Displaying interest
                                //Label6.Text = "Accrual Period: " + accrualPeriod + " day(s)"; // Displaying accrual period
                                //Label7.Text = "Suspension Period: " + suspPeriod + " day(s)"; // Displaying suspension period


                                //// Populate fields with data from the `generate_bill` table
                                //ptTax = reader["gb_tax"].ToString();
                                //ptInterest = reader["gb_interest"].ToString();
                                //accrualPeriod = reader["gb_accrual_period"].ToString();
                                //suspPeriod = reader["gb_suspend_period"].ToString();

                                // Populate fields with data from the `generate_bill` table
                                Label4.Text = reader["gb_tax"].ToString();
                                TextBox2.Text = reader["gb_id"].ToString();
                                Date.Text = Convert.ToDateTime(reader["gb_date_issued"]).ToString("yyyy-MM-ddTHH:mm");
                                TextBox7.Text = Convert.ToDateTime(reader["gb_date_due"]).ToString("yyyy-MM-ddTHH:mm");
                                TextBox4.Text = reader["gb_net_vat"].ToString();
                                TextBox5.Text = reader["gb_vat_amnt"].ToString();
                                TextBox6.Text = reader["gb_total_sales"].ToString();
                                TextBox10.Text = reader["gb_add_fees"].ToString();
                                TextBox11.Text = reader["gb_note"].ToString();

                                // Populate the associated `bk_id`
                                if (reader["bk_id"] != DBNull.Value)
                                {
                                    bkidviewbill.Value = reader["bk_id"].ToString(); // Set the hidden field value
                                    bk_id = Convert.ToInt32(reader["bk_id"]);
                                    DetailsLoadBookingWasteData(bk_id);

                                }
                                else
                                {
                                    bkidviewbill.Value = "No booking ID associated";
                                }
                            }
                        }
                    }
                    hfActiveTab.Value = "#tab2"; // Set the default active tab
                    updatePanel4.Update();       // Update the UpdatePanel
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showError",
                    $"Swal.fire({{ icon: 'error', title: 'Error', text: '{ex.Message}' }});", true);
            }
        }


        private void DetailsLoadBookingWasteData(int bookingId)
        {
            //int bookingId;

            // Fetch the bk_id from the generate_bill table using gb_id
            try
            {
                using (var db = new NpgsqlConnection(con))
                {
                    db.Open();

                    // Query to get booking waste details and bind to the GridView
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = @"
                    SELECT bw.bw_id, bw.bw_name, bw.bw_unit, bw.bw_total_unit, bw.bw_price, bw.bw_total_price, bw.bk_id
                    FROM BOOKING_WASTE bw
                    INNER JOIN BOOKING b ON bw.bk_id = b.bk_id
                    WHERE bw.bk_id = @bkId AND b.bk_status != 'Completed'
                    ORDER BY bw.bw_id";

                        cmd.Parameters.AddWithValue("@bkId", bookingId);

                        DataTable bookingsDataTable = new DataTable();
                        NpgsqlDataAdapter bookingsAdapter = new NpgsqlDataAdapter(cmd);
                        bookingsAdapter.Fill(bookingsDataTable);

                        gridView3.DataSource = bookingsDataTable;
                        gridView3.DataBind();
                    }

                    // Optionally, you can display a message if no records are found
                    if (gridView3.Rows.Count == 0)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "info",
                            "swal('No Data!', 'No booking waste data found for the given Booking ID.', 'info')", true);
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
                    $"swal('Error!', '{ex.Message}', 'error')", true);
            }
        }


        protected void openBookWaste_Click(object sender, EventArgs e)
        {
            LinkButton btn = sender as LinkButton;
            int id = Convert.ToInt32(btn.CommandArgument);
            txtbwID1.Text = id.ToString();
            TextBox1.Text = id.ToString();
            LoadBookingWasteData(); // Load the relevant data for the booking ID
            hfActiveTab.Value = "#tab1";
            this.ModalPopupExtender1.Show();
        }


        protected void btnViewSlip1_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            // Retrieve gb_id from the input (e.g., TextBox)
            int gb_id = Convert.ToInt32(TextBox2.Text);
            byte[] imageData = null;
            int bookingId = 0;

            // Define the PostgreSQL connection
            using (var db = new NpgsqlConnection(con))
            {
                db.Open();

                // Query to get bk_id and bk_waste_scale_slip associated with the given gb_id
                string query = @"
            SELECT bk.bk_id, bk.bk_waste_scale_slip
            FROM generate_bill gb
            INNER JOIN booking bk ON gb.bk_id = bk.bk_id
            WHERE gb.gb_id = @gb_id";

                using (var cmd = new NpgsqlCommand(query, db))
                {
                    cmd.Parameters.AddWithValue("@gb_id", NpgsqlTypes.NpgsqlDbType.Integer, gb_id);

                    // Execute the query and retrieve data
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            bookingId = reader.GetInt32(reader.GetOrdinal("bk_id"));
                            imageData = reader["bk_waste_scale_slip"] as byte[];
                        }
                        else
                        {
                            // No data found for the specified gb_id
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
                ScriptManager.RegisterStartupScript(this, GetType(), "LoadImageScript", script, true);
            }
            else
            {
                // If no image data, hide spinner and set a default image
                string script = @"
            document.getElementById('Image2').style.display = 'block';
            document.getElementById('Image2').src = '~/Pictures/blank_prof.png';
        ";
                ScriptManager.RegisterStartupScript(this, GetType(), "DefaultImageScript", script, true);
            }

            // Show the modal popup
            ModalPopupExtender4.Show();
        }
        protected void btnOtherAction1_Click(object sender, EventArgs e)
        {
            int gb_id = Convert.ToInt32(TextBox2.Text); // Retrieve gb_id from the input
            byte[] imageData = null;

            using (var db = new NpgsqlConnection(con))
            {
                db.Open();

                // Query to get bk_waste_scale_slip associated with the given gb_id
                string query = @"
            SELECT bk.bk_waste_scale_slip
            FROM generate_bill gb
            INNER JOIN booking bk ON gb.bk_id = bk.bk_id
            WHERE gb.gb_id = @gb_id";

                using (var cmd = new NpgsqlCommand(query, db))
                {
                    cmd.Parameters.AddWithValue("@gb_id", NpgsqlTypes.NpgsqlDbType.Integer, gb_id);

                    // Execute the query and retrieve the image data
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            imageData = reader["bk_waste_scale_slip"] as byte[];
                        }
                        else
                        {
                            // Handle no data found
                            Response.Write("<script>alert('No data found for the specified ID.');</script>");
                            return;
                        }
                    }
                }
                db.Close();
            }

            if (imageData != null && imageData.Length > 0)
            {
                string base64String = Convert.ToBase64String(imageData);
                string imageUrl = $"data:image/jpeg;base64,{base64String}";

                // Suggest a filename for download
                string filename = "Scale Slip/ScaleSlip.jpg";

                // Pass the image URL and filename to JavaScript
                string downloadScript = $"downloadImage('{imageUrl}', '{filename}');";
                ScriptManager.RegisterStartupScript(this, GetType(), "DownloadImageScript", downloadScript, true);
            }
            else
            {
                // Handle case where no image is found
                Response.Write("<script>alert('No image found.');</script>");
            }
        }

        protected void btnOtherAction_Click(object sender, EventArgs e)
        {
            int bookingId = Convert.ToInt32(TextBox1.Text);
            byte[] imageData = null;

            using (var db = new NpgsqlConnection(con))
            {
                db.Open();
                string query = "SELECT bk_waste_scale_slip FROM booking WHERE bk_id = @id";
                using (var cmd = new NpgsqlCommand(query, db))
                {
                    cmd.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Integer, bookingId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            imageData = reader["bk_waste_scale_slip"] as byte[];
                        }
                        else
                        {
                            // Handle no data found
                            Response.Write("<script>alert('No data found for the specified ID.');</script>");
                            return;
                        }
                    }
                }
                db.Close();
            }

            if (imageData != null && imageData.Length > 0)
            {
                string base64String = Convert.ToBase64String(imageData);
                string imageUrl = $"data:image/jpeg;base64,{base64String}";

                // Suggest a filename for download
                string filename = "Scale Slip/ScaleSlip.jpg";

                // Pass the image URL and filename to JavaScript
                string downloadScript = $"downloadImage('{imageUrl}', '{filename}');";
                ScriptManager.RegisterStartupScript(this, GetType(), "DownloadImageScript", downloadScript, true);
            }
            else
            {
                // Handle case where no image is found
                Response.Write("<script>alert('No image found.');</script>");
            }
        }

        protected void btnViewSlip_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            int bookingId = Convert.ToInt32(TextBox1.Text);  // Assuming you hardcode the bookingId for now. Adjust this as necessary.
            //TextBox2.Text = bookingId.ToString();
            byte[] imageData = null;

            // Define the PostgreSQL connection
            using (var db = new NpgsqlConnection(con))
            {
                db.Open();

                // PostgreSQL query to get the waste scale slip image
                string query = "SELECT bk_waste_scale_slip FROM booking WHERE bk_id = @id";
                using (var cmd = new NpgsqlCommand(query, db))
                {
                    cmd.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Integer, bookingId);

                    // Execute the query and retrieve the image data
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            imageData = reader["bk_waste_scale_slip"] as byte[];
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

        private void PopulateWasteCategory()
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(con))
            {
                // Get the booking ID from the textbox
                int bk_id = string.IsNullOrWhiteSpace(txtbwID1.Text) ? 0 : Convert.ToInt32(TextBox1.Text);

                // SQL Query to fetch waste categories that are NOT already associated with the given bk_id
                string query = @"
                                SELECT wc.wc_id, wc.wc_name
                                FROM waste_category wc
                                WHERE wc.wc_status != 'Deleted' 
                                  AND wc.wc_id NOT IN (
                                      SELECT bw.wc_id
                                      FROM booking_waste bw
                                      WHERE bw.bk_id = @bk_id
                                  );
                            ";


                // Prepare command
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@bk_id", bk_id); // Pass the booking ID as a parameter

                // Execute query and get the results
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Bind the data to the dropdown list
                ddlbwName1.Items.Clear();
                ddlbwName1.DataSource = dt;
                ddlbwName1.DataTextField = "wc_name";
                ddlbwName1.DataValueField = "wc_id";
                ddlbwName1.DataBind();

                // Insert the default item at the top of the dropdown list
                WebControls.ListItem defaultItem1 = new WebControls.ListItem("--Select Waste Category--", "0");
                defaultItem1.Attributes.Add("disabled", "true"); // Disable the option
                defaultItem1.Attributes.Add("selected", "true"); // Set as selected
                ddlbwName1.Items.Insert(0, defaultItem1); // Insert it at the first position
            }
        }


        protected void CancelGenerateBill_Click(object sender, EventArgs e)
        {
            addFeeTxt.Text = string.Empty;
        }
        protected void addFeeTxt_TextChanged(object sender, EventArgs e)
        {
            try
            {
                // Parse additional fee input
                double addFee = string.IsNullOrWhiteSpace(addFeeTxt.Text) ? 0 : Convert.ToDouble(addFeeTxt.Text);
                double tax = 0, netVAT = 0;

                // Parse the booking ID from TextBox1
                if (!int.TryParse(TextBox1.Text, out int bookingId))
                {
                    // If parsing fails, show an error and return
                    ScriptManager.RegisterStartupScript(this, GetType(), "inputError",
                        "alert('Invalid booking ID. Please enter a valid number.');", true);
                    return;
                }

                using (NpgsqlConnection connection = new NpgsqlConnection(con))
                {
                    connection.Open();

                    // Fetch netVAT (sum of bw_total_price) from booking_waste
                    string queryNetVAT = "SELECT COALESCE(SUM(bw_total_price), 0) FROM booking_waste WHERE bk_id = @BkId";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(queryNetVAT, connection))
                    {
                        cmd.Parameters.AddWithValue("@BkId", bookingId);
                        netVAT = Convert.ToDouble(cmd.ExecuteScalar());
                    }

                    // Fetch tax value from payment_term
                    string queryTax = "SELECT pt_tax FROM payment_term LIMIT 1";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(queryTax, connection))
                    {
                        tax = Convert.ToDouble(cmd.ExecuteScalar());
                    }
                }

                // Perform calculations
                double taxRate = tax / 100;
                double netVATValue = netVAT + addFee;
                double vatAmount = netVATValue * taxRate;
                double totalSales = netVATValue + vatAmount;

                // Update textboxes
                netVatTxt.Text = netVATValue.ToString("0.00");
                vatAmntTxt.Text = vatAmount.ToString("0.00");
                totSalesTxt.Text = totalSales.ToString("0.00");

                // Update the UpdatePanel
                updatePanel1.Update();
            }
            catch (Exception ex)
            {
                // Display error
                ScriptManager.RegisterStartupScript(this, GetType(), "calculationError",
                    $"alert('Error: {ex.Message}');", true);
            }
        }


        protected void txtTotalUnit_TextChanged(object sender, EventArgs e)
        {
            // Calculate total price based on unit price and total unit
            decimal unitPrice = Convert.ToDecimal(txtUnitPrice.Text);
            decimal totalUnit = Convert.ToDecimal(txtTotalUnit.Text);

            txtTotalUnitPrice.Text = (unitPrice * totalUnit).ToString("0.00");
        }

        protected void txtTotalUnit_TextChanged1(object sender, EventArgs e)
        {
            // Calculate total price based on unit price and total unit
            decimal unitPrice = Convert.ToDecimal(txtUnitPrice1.Text);
            decimal totalUnit = Convert.ToDecimal(txtTotalUnit1.Text);

            txtTotalUnitPrice1.Text = (unitPrice * totalUnit).ToString("0.00");
        }

        protected void ddlWasteCategory_SelectedIndexChanged1(object sender, EventArgs e)
        {
            // Check for a valid selection
            if (string.IsNullOrEmpty(ddlbwName1.SelectedValue) || ddlbwName1.SelectedValue == "0")
            {
                // Reset all fields and disable Total Unit TextBox
                txtUnitPrice1.Text = "";
                txtbwUnit1.Text = "";
                txtTotalUnit1.Text = "";
                txtTotalUnitPrice1.Text = "";
                txtTotalUnit1.Enabled = false;
                return;
            }

            int selectedWasteID = Convert.ToInt32(ddlbwName1.SelectedValue);

            if (selectedWasteID > 0)
            {
                string query = "SELECT wc_unit, wc_price FROM waste_category WHERE wc_id = @wc_id";

                using (NpgsqlConnection connection = new NpgsqlConnection(con))
                {
                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@wc_id", selectedWasteID);

                        connection.Open();
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Populate the unit and price fields
                                txtUnitPrice1.Text = reader["wc_price"].ToString();
                                txtbwUnit1.Text = reader["wc_unit"].ToString();
                                txtTotalUnit1.Text = "";
                                txtTotalUnitPrice1.Text = "";

                                // Enable Total Unit TextBox
                                txtTotalUnit1.Enabled = true;
                            }
                        }
                        connection.Close();
                    }
                }
            }
        }


        private bool IsInitialLoad
        {
            get { return ViewState["IsInitialLoad"] as bool? ?? true; }
            set { ViewState["IsInitialLoad"] = value; }
        }


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

        protected void btncancel_Click(object sender, EventArgs e)
        {
            // Reset the fields to default or initial values
            //txtbwID.Text = string.Empty;
            txtbwUnit.Text = string.Empty;
            txtTotalUnit.Text = string.Empty;
            txtUnitPrice.Text = string.Empty;
            txtTotalUnitPrice.Text = string.Empty;
            //ddlbwName.SelectedIndex = 0; // Reset the dropdown to its default selection

            // Hide the panel/modal
            ModalPopupExtender2.Hide();
        }

        protected void btncancel_Click1(object sender, EventArgs e)
        {
            // Reset the fields to default or initial values
            //txtbwID1.Text = string.Empty;
            txtbwUnit1.Text = string.Empty;
            txtTotalUnit1.Text = string.Empty;
            txtUnitPrice1.Text = string.Empty;
            txtTotalUnitPrice1.Text = string.Empty;
            ddlbwName1.SelectedIndex = 0; // Reset the dropdown to its default selection
            txtTotalUnit1.Enabled = false;
            // Hide the panel/modal
            ModalPopupExtender3.Hide();
        }
        //protected void ViewBill_Click(object sender, EventArgs e)
        //{
        //    int gb_id = Convert.ToInt32(TextBox2.Text);

        //    string gb_note = null;
        //    decimal? gb_add_fees = null;
        //    decimal? gb_net_vat = null;
        //    decimal? gb_vat_amnt = null;
        //    decimal? gb_total_sales = null;
        //    DateTime? gb_date_issued = null;
        //    DateTime? gb_date_due = null;
        //    double? gb_interest = null;
        //    int? gb_lead_days = null;
        //    int? gb_accrual_period = null;
        //    int? gb_suspend_period = null;
        //    DateTime? gb_accrual_date = null;
        //    DateTime? gb_suspend_date = null;
        //    decimal? gb_tax = null;
        //    decimal? gb_total_amnt_interest = null;
        //    string gb_status = null;
        //    DateTime? gb_created_at = null;
        //    DateTime? gb_updated_at = null;

        //    int bk_id = 0;
        //    int emp_id = 1;

        //    // Retrieve payment term values from the database
        //    using (var conn = new NpgsqlConnection(con))
        //    {
        //        conn.Open();

        //        string findBillQuery = "SELECT * FROM generate_bill WHERE gb_id = @GbId";
        //        using (var cmd = new NpgsqlCommand(findBillQuery, conn))
        //        {
        //            cmd.Parameters.AddWithValue("@GbId", gb_id);
        //            using (var reader = cmd.ExecuteReader())
        //            {
        //                if (reader.Read())
        //                {
        //                    gb_note = reader["gb_note"]?.ToString();
        //                    gb_add_fees = reader["gb_add_fees"] as decimal?;
        //                    gb_net_vat = reader["gb_net_vat"] as decimal?;
        //                    gb_vat_amnt = reader["gb_vat_amnt"] as decimal?;
        //                    gb_total_sales = reader["gb_total_sales"] as decimal?;
        //                    gb_date_issued = reader["gb_date_issued"] as DateTime?;
        //                    gb_date_due = reader["gb_date_due"] as DateTime?;
        //                    gb_interest = reader["gb_interest"] as double?;
        //                    gb_lead_days = reader["gb_lead_days"] as int?;
        //                    gb_accrual_period = reader["gb_accrual_period"] as int?;
        //                    gb_suspend_period = reader["gb_suspend_period"] as int?;
        //                    gb_accrual_date = reader["gb_accrual_date"] as DateTime?;
        //                    gb_suspend_date = reader["gb_suspend_date"] as DateTime?;
        //                    gb_tax = reader["gb_tax"] as decimal?;
        //                    gb_total_amnt_interest = reader["gb_total_amnt_interest"] as decimal?;
        //                    gb_status = reader["gb_status"]?.ToString();
        //                    gb_created_at = reader["gb_created_at"] as DateTime?;
        //                    gb_updated_at = reader["gb_updated_at"] as DateTime?;
        //                    bk_id = Convert.ToInt32(reader["bk_id"]);
        //                    emp_id = Convert.ToInt32(reader["emp_id"]);
        //                }
        //                else
        //                {
        //                    throw new Exception("Bill not found.");
        //                }
        //            }
        //        }
        //    }
        //    LoadBookingList();

        //    int insertedBillId = gb_id;
        //    byte[] pdfBytes = GeneratePDFViewBill(insertedBillId, bk_id);
        //    LoadBookingList();

        //    // Set up PDF response and initiate download
        //    Response.Clear();
        //    Response.ContentType = "application/pdf";
        //    Response.AddHeader("content-disposition", $"attachment;filename=Bill_{insertedBillId}.pdf");
        //    Response.Buffer = true;
        //    Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //    Response.BinaryWrite(pdfBytes);
        //    LoadBookingList();
        //    // Use Flush instead of End to avoid threading issues
        //    Response.Flush();
        //    HttpContext.Current.ApplicationInstance.CompleteRequest();
        //}


        protected void ViewBill_Click(object sender, EventArgs e)
        {
            int gb_id = Convert.ToInt32(TextBox2.Text);

            string gb_note = null;
            decimal? gb_add_fees = null;
            decimal? gb_net_vat = null;
            decimal? gb_vat_amnt = null;
            decimal? gb_total_sales = null;
            DateTime? gb_date_issued = null;
            DateTime? gb_date_due = null;
            double? gb_interest = null;
            int? gb_lead_days = null;
            int? gb_accrual_period = null;
            int? gb_suspend_period = null;
            DateTime? gb_accrual_date = null;
            DateTime? gb_suspend_date = null;
            decimal? gb_tax = null;
            decimal? gb_total_amnt_interest = null;
            string gb_status = null;
            DateTime? gb_created_at = null;
            DateTime? gb_updated_at = null;

            int bk_id = 0;
            int emp_id = 1;

            // Retrieve payment term values from the database
            using (var conn = new NpgsqlConnection(con))
            {
                conn.Open();

                string findBillQuery = "SELECT * FROM generate_bill WHERE gb_id = @GbId";
                using (var cmd = new NpgsqlCommand(findBillQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@GbId", gb_id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            gb_note = reader["gb_note"]?.ToString();
                            gb_add_fees = reader["gb_add_fees"] as decimal?;
                            gb_net_vat = reader["gb_net_vat"] as decimal?;
                            gb_vat_amnt = reader["gb_vat_amnt"] as decimal?;
                            gb_total_sales = reader["gb_total_sales"] as decimal?;
                            gb_date_issued = reader["gb_date_issued"] as DateTime?;
                            gb_date_due = reader["gb_date_due"] as DateTime?;
                            gb_tax = reader["gb_tax"] as decimal?;
                            gb_status = reader["gb_status"]?.ToString();
                            gb_created_at = reader["gb_created_at"] as DateTime?;
                            gb_updated_at = reader["gb_updated_at"] as DateTime?;
                            bk_id = Convert.ToInt32(reader["bk_id"]);
                            emp_id = Convert.ToInt32(reader["emp_id"]);
                        }
                        else
                        {
                            throw new Exception("Bill not found.");
                        }
                    }
                }
            }
            LoadBookingList();

            int insertedBillId = gb_id;
            byte[] pdfBytes = GeneratePDFViewBill(insertedBillId, bk_id);
            LoadBookingList();

            // Set up PDF response and initiate download
            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", $"attachment;filename=Bill_{insertedBillId}.pdf");
            Response.Buffer = true;
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.BinaryWrite(pdfBytes);
            LoadBookingList();
            // Use Flush instead of End to avoid threading issues
            Response.Flush();
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        private byte[] GeneratePDFViewBill(int buttonText, int bkID)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(ms);
                PdfDocument pdf = new PdfDocument(writer);
                ITextDocument document = new ITextDocument(pdf);

                // Define fonts and colors
                PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                DeviceRgb redColor = new DeviceRgb(255, 0, 0);
                string fontPath = Server.MapPath("~/fonts/Roboto/Roboto-Regular.ttf");
                PdfFont font = PdfFontFactory.CreateFont(fontPath, "Identity-H", PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);



                // Create a table with one row and two columns for the logo and address
                iText.Layout.Element.Table headerTable = new iText.Layout.Element.Table(new float[] { 1, 3 })
                    .UseAllAvailableWidth();

                // Add company logo (if any)
                string logoPath = Server.MapPath("~/Pictures/logo_bgRM.png");
                iText.Layout.Element.Image logo = new iText.Layout.Element.Image(ImageDataFactory.Create(logoPath));
                logo.ScaleToFit(100, 50);

                iText.Layout.Element.Table logoTextTable = new iText.Layout.Element.Table(2)
                    .UseAllAvailableWidth();

                // Create the logo cell
                Cell logoCell = new Cell()
                    .SetBorder(Border.NO_BORDER)
                    .Add(logo);

                // Define the green color
                DeviceRgb greenColor = new DeviceRgb(0, 128, 0);

                // Create the TrashTrack text without margin and padding
                Paragraph trashTrackText = new Paragraph("TrashTrack")
                    .SetFont(boldFont)
                    .SetFontSize(30)
                    .SetFontColor(greenColor)
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetMargin(0)
                    .SetPadding(0);

                // Create a cell for the TrashTrack text
                Cell textCell = new Cell()
                    .SetBorder(Border.NO_BORDER)
                    .Add(trashTrackText);

                // Add both cells to the logoTextTable
                logoTextTable.AddCell(logoCell);
                logoTextTable.AddCell(textCell);

                // Add the logo and text table to the headerTable
                headerTable.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER)
                    .Add(logoTextTable));

                // Add Company Address to the second cell
                Paragraph address = new Paragraph("Binaliw Cebu Dumpsite\nCebu City, Cebu\nPhilippines")
                    .SetFont(font)
                    .SetTextAlignment(TextAlignment.RIGHT);

                headerTable.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER)
                    .Add(address));

                // Add the header table to the document
                document.Add(headerTable);

                // Initialize variables with default values to avoid uninitialized errors 
                
                int taxValue = 0;
                
                double totalSum = 0;
                double vat_Amnt = 0;
                double totalPayment = 0;
                double totalSales = 0;
                double addFee = 0;
                double netVat = 0;
                // Use a nullable DateTime for dateIssued in case it’s not set
                DateTime? dateIssued = null;
                DateTime? dueDate = null;

                double totAmnt = totalPayment + vat_Amnt;
                // Database connection and fetching values
                using (var db = new NpgsqlConnection(con))
                {
                    db.Open();

                    // Fetch interest and leadDays values from PostgreSQL
                    string controlQuery = "SELECT * FROM payment_term";
                    using (var cmd = new NpgsqlCommand(controlQuery, db))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                taxValue = Convert.ToInt32(reader["pt_tax"]);

                            }
                            else
                            {
                                Console.WriteLine("No payment term data found. Using default values.");
                            }
                        }
                    }

                    // Fetch bill details (e.g., date issued)
                    string billQuery = "SELECT * FROM generate_bill WHERE gb_id = @BillId";
                    using (var billCmd = new NpgsqlCommand(billQuery, db))
                    {
                        billCmd.Parameters.AddWithValue("@BillId", buttonText);

                        using (var billReader = billCmd.ExecuteReader())
                        {
                            if (billReader.Read())
                            {
                                addFee = billReader["gb_add_fees"] == DBNull.Value ? 0.0 : Convert.ToDouble(billReader["gb_add_fees"]);
                                dateIssued = Convert.ToDateTime(billReader["gb_date_issued"]);
                                totalSales = Convert.ToDouble(billReader["gb_total_sales"]);


                                
                            }
                        }
                    }

                    // Calculate total waste price for booking and VAT
                    string totalQuery = "SELECT SUM(bw_total_price) FROM booking_waste WHERE bk_id = @BkId";
                    using (var totalCmd = new NpgsqlCommand(totalQuery, db))
                    {
                        totalCmd.Parameters.AddWithValue("@BkId", bkID);
                        object result = totalCmd.ExecuteScalar();

                        if (result != DBNull.Value)
                        {
                            totalSum += Convert.ToDouble(result);
                            //vat_Amnt = totalSum * (taxValue / 100.0);
                            //totAmnt = vat_Amnt + totalSum;
                        }
                    }
                    //netVat = totalSum + addFee;
                    //totAmnt = vat_Amnt + totalPayment;
                    netVat = totalSum + addFee;
                    vat_Amnt = netVat * (taxValue / 100.0);
                    //totAmnt = vat_Amnt + totalSales;
                    // Add Title
                    Paragraph title = new Paragraph("Billing Statement")
                        .SetFont(boldFont)
                        .SetFontSize(16)
                        .SetTextAlignment(TextAlignment.CENTER);
                    document.Add(title);

                    // Create a 2-column table to align items in two columns
                    iText.Layout.Element.Table infoTable = new iText.Layout.Element.Table(2).UseAllAvailableWidth();

                    // Set table border to none
                    infoTable.SetBorder(Border.NO_BORDER);

                    // Left column: Bill ID
                    infoTable.AddCell(new Cell()
                        .SetBorder(Border.NO_BORDER)
                        .Add(new Paragraph($"Bill ID: {buttonText}")
                            .SetFont(boldFont)));

                    // Right column: Booking ID
                    infoTable.AddCell(new Cell()
                        .SetBorder(Border.NO_BORDER)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .Add(new Paragraph($"Booking ID: {bkID}")
                            .SetFont(boldFont)));

                    // Left column: Empty cell for spacing
                    infoTable.AddCell(new Cell()
                        .SetBorder(Border.NO_BORDER)
                        .Add(new Paragraph("")));

                    // Left column: Empty cell for spacing
                    infoTable.AddCell(new Cell()
                        .SetBorder(Border.NO_BORDER)
                        .Add(new Paragraph("")));
                    // Left column: Empty cell for spacing
                    infoTable.AddCell(new Cell()
                        .SetBorder(Border.NO_BORDER)
                        .Add(new Paragraph("")));

                    // Right column: Date Issued
                    infoTable.AddCell(new Cell()
                        .SetBorder(Border.NO_BORDER)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .Add(new Paragraph($"Date Issued: {(dateIssued?.ToString("MM/dd/yyyy") ?? "N/A")}")
                            .SetFont(boldFont)));

                    // Left column: Empty cell for spacing
                    infoTable.AddCell(new Cell()
                        .SetBorder(Border.NO_BORDER)
                        .Add(new Paragraph("")));

                    // Right column: Due Date
                    infoTable.AddCell(new Cell()
                        .SetBorder(Border.NO_BORDER)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .Add(new Paragraph($"Due Date: {(dueDate?.ToString("MM/dd/yyyy") ?? "N/A")}")
                            .SetFont(boldFont)));

                    // Add the table to the document
                    document.Add(infoTable);

                }


                // Add Terms
                Paragraph termsTitle = new Paragraph("TERMS:")
                        .SetFont(boldFont)
                        .SetFontSize(12);
                document.Add(termsTitle);

                // Create terms content paragraph with formatted strings using default values
                Paragraph termsContent = new Paragraph(
                    $"The bill shall be due for payment and collection on the day of issuance. Failure by the customer to make payment without valid and justifiable reason may result in the suspension of waste collection services. Additionally, TrashTrack reserves the right to withhold further processing and disposal services for the customer's premises to ensure compliance with payment obligations."
                )
                .SetFont(font)
                .SetTextAlignment(TextAlignment.JUSTIFIED)
                .SetFontSize(10);

                // Add the terms content to the document
                document.Add(termsContent);

                // Add Waste Details Table
                iText.Layout.Element.Table wasteTable = new iText.Layout.Element.Table(new float[] { 100, 50, 80, 80, 100 }).UseAllAvailableWidth();
                wasteTable.SetMarginTop(20);

                // Add table headers with bottom border
                wasteTable.AddHeaderCell(new Cell()
                    .Add(new Paragraph("Waste Type").SetFont(boldFont))
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetBorderTop(Border.NO_BORDER)
                    .SetBorderLeft(Border.NO_BORDER)
                    .SetBorderRight(Border.NO_BORDER)
                    .SetBorderBottom(new SolidBorder(1f)));  // Add bold bottom border for header

                wasteTable.AddHeaderCell(new Cell()
                    .Add(new Paragraph("Unit").SetFont(boldFont))
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetBorderTop(Border.NO_BORDER)
                    .SetBorderLeft(Border.NO_BORDER)
                    .SetBorderRight(Border.NO_BORDER)
                    .SetBorderBottom(new SolidBorder(1f)));  // Add bold bottom border for header

                wasteTable.AddHeaderCell(new Cell()
                    .Add(new Paragraph("Total Unit").SetFont(boldFont))
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetBorderTop(Border.NO_BORDER)
                    .SetBorderLeft(Border.NO_BORDER)
                    .SetBorderRight(Border.NO_BORDER)
                    .SetBorderBottom(new SolidBorder(1f)));  // Add bold bottom border for header

                wasteTable.AddHeaderCell(new Cell()
                    .Add(new Paragraph("Unit Price").SetFont(boldFont))
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetBorderTop(Border.NO_BORDER)
                    .SetBorderLeft(Border.NO_BORDER)
                    .SetBorderRight(Border.NO_BORDER)
                    .SetBorderBottom(new SolidBorder(1f)));  // Add bold bottom border for header

                wasteTable.AddHeaderCell(new Cell()
                    .Add(new Paragraph("Total Price").SetFont(boldFont))
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetBorderTop(Border.NO_BORDER)
                    .SetBorderLeft(Border.NO_BORDER)
                    .SetBorderRight(Border.NO_BORDER)
                    .SetBorderBottom(new SolidBorder(1f)));  // Add bold bottom border for header



                // Fetch booking_waste entries related to the booking ID
                using (var dbb = new NpgsqlConnection(con))
                {
                    dbb.Open();

                    string wasteQuery = @"SELECT bw_name, bw_unit, bw_total_unit, bw_price, bw_total_price 
                                  FROM booking_waste 
                                  WHERE bk_id = @BkId";
                    using (var wasteCmd = new NpgsqlCommand(wasteQuery, dbb))
                    {
                        wasteCmd.Parameters.AddWithValue("@BkId", bkID);

                        using (var wasteReader = wasteCmd.ExecuteReader())
                        {
                            while (wasteReader.Read())
                            {
                                // Add detail rows without borders
                                wasteTable.AddCell(new Cell()
                                    .Add(new Paragraph(wasteReader["bw_name"].ToString()))
                                    .SetFont(font)
                                    .SetTextAlignment(TextAlignment.LEFT)
                                    .SetBorder(Border.NO_BORDER));

                                wasteTable.AddCell(new Cell()
                                    .Add(new Paragraph(wasteReader["bw_unit"].ToString()))
                                    .SetTextAlignment(TextAlignment.LEFT)
                                    .SetBorder(Border.NO_BORDER));

                                wasteTable.AddCell(new Cell()
                                    .Add(new Paragraph(wasteReader["bw_total_unit"].ToString()))
                                    .SetTextAlignment(TextAlignment.LEFT)
                                    .SetBorder(Border.NO_BORDER));

                                wasteTable.AddCell(new Cell()
                                    .Add(new Paragraph("₱" + wasteReader["bw_price"].ToString()))
                                    .SetFont(font)
                                    .SetTextAlignment(TextAlignment.LEFT)
                                    .SetBorder(Border.NO_BORDER));

                                wasteTable.AddCell(new Cell()
                                    .Add(new Paragraph("₱ " + wasteReader["bw_total_price"].ToString()))
                                    .SetFont(font)
                                    .SetTextAlignment(TextAlignment.LEFT)
                                    .SetBorder(Border.NO_BORDER));
                            }
                        }
                    }

                }

                document.Add(wasteTable);


                // Define the width for the bottom line
                float[] bottomLineWidths = new float[] { 1 }; // Single column for the line
                iText.Layout.Element.Table btmLine = new iText.Layout.Element.Table(bottomLineWidths).UseAllAvailableWidth();

                // Add a cell for the line with a top border
                btmLine.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER)
                    .Add(new Paragraph("")
                        .SetBorder(Border.NO_BORDER)
                        .SetBorderTop(new SolidBorder(1f))
                    )
                );

                // Add the bottom line table to the document
                document.Add(btmLine);



                float[] columnWidths = new float[] { 100, 40, 30, 80, 100 }; // Set fixed pixel widths
                iText.Layout.Element.Table summarySection = new iText.Layout.Element.Table(columnWidths).UseAllAvailableWidth();

                // Method to add empty cells
                void AddEmptyCell(iText.Layout.Element.Table table)
                {
                    table.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(new Paragraph("").SetBorder(Border.NO_BORDER)));
                }


                for (int i = 0; i < 3; i++)
                {
                    AddEmptyCell(summarySection); // Adding empty cells for spacing
                }

                // Add Net of VAT label
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("Total Sum: ").SetFont(boldFont)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetBorder(Border.NO_BORDER)));

                // Add Net of VAT amount
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("₱" + totalSum.ToString("N2"))
                        .SetFont(boldFont)
                        .SetFont(font)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetBorder(Border.NO_BORDER)));

                // Add empty cells for spacing
                for (int i = 0; i < 3; i++)
                {
                    AddEmptyCell(summarySection);
                }

                // Add Additional Fee label
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER)
                    .Add(new Paragraph("Additional Fee: ")
                        .SetFont(boldFont)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetBorder(Border.NO_BORDER)));

                // Add Total Amount due
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER)
                    .Add(new Paragraph("₱" + (addFee.ToString("N2")))
                        .SetFont(font)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetBorder(Border.NO_BORDER)));


                for (int i = 0; i < 3; i++)
                {
                    AddEmptyCell(summarySection); // Adding empty cells for spacing
                }

                // Add Net of VAT label
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("Net of VAT: ").SetFont(boldFont)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetBorder(Border.NO_BORDER)));

                // Add Net of VAT amount
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("₱" + netVat.ToString("N2"))
                        .SetFont(boldFont)
                        .SetFont(font)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetBorder(Border.NO_BORDER)));

                // Add empty cells for spacing
                for (int i = 0; i < 3; i++)
                {
                    AddEmptyCell(summarySection);
                }

                // Add VAT label
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("VAT (" + taxValue + "%): ")
                        .SetFont(boldFont)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetBorder(Border.NO_BORDER)));

                // Add VAT amount
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("₱" + vat_Amnt.ToString("N2"))
                        .SetFont(boldFont)
                        .SetFont(font)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetBorder(Border.NO_BORDER)));

                // Add empty cells for spacing
                for (int i = 0; i < 3; i++)
                {
                    AddEmptyCell(summarySection);
                }

                // Add Total Sales label
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER)
                    .Add(new Paragraph("Total Amount: ")
                        .SetFont(boldFont)
                        .SetFontColor(redColor)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetBorder(Border.NO_BORDER)));

                // Add Total Sales amount
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER)
                    .Add(new Paragraph("₱" + totalSales.ToString("N2"))
                        .SetFont(font)
                        .SetFontColor(redColor)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetBorder(Border.NO_BORDER)));


                // Add empty cells for spacing
                for (int i = 0; i < 3; i++)
                {
                    AddEmptyCell(summarySection);
                }

                //// Add Total Amount label
                //summarySection.AddCell(new Cell()
                //    .SetBorder(Border.NO_BORDER)
                //    .Add(new Paragraph("Total Amount: ")
                //        .SetFont(boldFont)
                //        .SetFontColor(redColor)
                //        .SetTextAlignment(TextAlignment.LEFT)
                //        .SetBorder(Border.NO_BORDER)));

                //// Calculate Total Due
                ////double totalDue = totalPayment + (addFee.HasValue && addFee.Value > 0 ? addFee.Value : 0);

                //// Add Total Amount due
                //summarySection.AddCell(new Cell()
                //    .SetBorder(Border.NO_BORDER)
                //    .Add(new Paragraph("₱" + totalPayment.ToString("N2"))
                //        .SetFont(boldFont)
                //        .SetFontColor(redColor)
                //        .SetTextAlignment(TextAlignment.LEFT)
                //        .SetBorder(Border.NO_BORDER)));

                // Add the summary section table to the document
                document.Add(summarySection);


                // Close document
                document.Close();

                return ms.ToArray();

            }
        }

        ////VIEW PDF BILL
        //private byte[] GeneratePDFViewBill(int buttonText, int bkID)
        //{
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        PdfWriter writer = new PdfWriter(ms);
        //        PdfDocument pdf = new PdfDocument(writer);
        //        ITextDocument document = new ITextDocument(pdf);

        //        // Define fonts and colors
        //        PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
        //        DeviceRgb redColor = new DeviceRgb(255, 0, 0);
        //        string fontPath = Server.MapPath("~/fonts/Roboto/Roboto-Regular.ttf");
        //        PdfFont font = PdfFontFactory.CreateFont(fontPath, "Identity-H", PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);



        //        // Create a table with one row and two columns for the logo and address
        //        iText.Layout.Element.Table headerTable = new iText.Layout.Element.Table(new float[] { 1, 3 })
        //            .UseAllAvailableWidth();

        //        // Add company logo (if any)
        //        string logoPath = Server.MapPath("~/Pictures/logo_bgRM.png");
        //        iText.Layout.Element.Image logo = new iText.Layout.Element.Image(ImageDataFactory.Create(logoPath));
        //        logo.ScaleToFit(100, 50);

        //        iText.Layout.Element.Table logoTextTable = new iText.Layout.Element.Table(2)
        //            .UseAllAvailableWidth();

        //        // Create the logo cell
        //        Cell logoCell = new Cell()
        //            .SetBorder(Border.NO_BORDER)
        //            .Add(logo);

        //        // Define the green color
        //        DeviceRgb greenColor = new DeviceRgb(0, 128, 0);

        //        // Create the TrashTrack text without margin and padding
        //        Paragraph trashTrackText = new Paragraph("TrashTrack")
        //            .SetFont(boldFont)
        //            .SetFontSize(30)
        //            .SetFontColor(greenColor)
        //            .SetTextAlignment(TextAlignment.LEFT)
        //            .SetMargin(0)
        //            .SetPadding(0);

        //        // Create a cell for the TrashTrack text
        //        Cell textCell = new Cell()
        //            .SetBorder(Border.NO_BORDER)
        //            .Add(trashTrackText);

        //        // Add both cells to the logoTextTable
        //        logoTextTable.AddCell(logoCell);
        //        logoTextTable.AddCell(textCell);

        //        // Add the logo and text table to the headerTable
        //        headerTable.AddCell(new Cell()
        //            .SetBorder(Border.NO_BORDER)
        //            .Add(logoTextTable));

        //        // Add Company Address to the second cell
        //        Paragraph address = new Paragraph("Binaliw Cebu Dumpsite\nCebu City, Cebu\nPhilippines")
        //            .SetFont(font)
        //            .SetTextAlignment(TextAlignment.RIGHT);

        //        headerTable.AddCell(new Cell()
        //            .SetBorder(Border.NO_BORDER)
        //            .Add(address));

        //        // Add the header table to the document
        //        document.Add(headerTable);

        //        // Initialize variables with default values to avoid uninitialized errors 
        //        int leadDays = 0;
        //        int taxValue = 0;
        //        int accPerValue = 0;
        //        int susPerValue = 0;
        //        double totalSum = 0;
        //        double vat_Amnt = 0;
        //        double totalPayment = 0;
        //        double amntInterest = 0;
        //        double interestRate = 0;
        //        int accrualPeriod = 0;
        //        int suspendPeriod = 0;
        //        double totalSales = 0;
        //        double addFee = 0;
        //        double netVat = 0;
        //        // Use a nullable DateTime for dateIssued in case it’s not set
        //        DateTime? dateIssued = null;
        //        DateTime? dueDate = null;

        //        // Parse the entered date
        //        DateTime? currentDate = DateTime.TryParse(dateEntered.Text, out DateTime dateCurrent) ? (DateTime?)dateCurrent : null;
        //        //double totAmnt = 0;
        //        double totAmnt = totalPayment + vat_Amnt;
        //        // Database connection and fetching values
        //        using (var db = new NpgsqlConnection(con))
        //        {
        //            db.Open();

        //            // Fetch interest and leadDays values from PostgreSQL
        //            string controlQuery = "SELECT * FROM payment_term";
        //            using (var cmd = new NpgsqlCommand(controlQuery, db))
        //            {
        //                using (var reader = cmd.ExecuteReader())
        //                {
        //                    if (reader.Read())
        //                    {
        //                        taxValue = Convert.ToInt32(reader["pt_tax"]);

        //                    }
        //                    else
        //                    {
        //                        Console.WriteLine("No payment term data found. Using default values.");
        //                    }
        //                }
        //            }

        //            // Fetch bill details (e.g., date issued)
        //            string billQuery = "SELECT * FROM generate_bill WHERE gb_id = @BillId";
        //            using (var billCmd = new NpgsqlCommand(billQuery, db))
        //            {
        //                billCmd.Parameters.AddWithValue("@BillId", buttonText);

        //                using (var billReader = billCmd.ExecuteReader())
        //                {
        //                    if (billReader.Read())
        //                    {
        //                        addFee = billReader["gb_add_fees"] == DBNull.Value ? 0.0 : Convert.ToDouble(billReader["gb_add_fees"]);
        //                        dateIssued = Convert.ToDateTime(billReader["gb_date_issued"]);
        //                        totalSales = Convert.ToDouble(billReader["gb_total_sales"]);


        //                        dueDate = dateIssued?.AddDays(leadDays); // Calculate due date

        //                        // Calculate the initial total payment and interest if current date is past due date
        //                        totalPayment = totalSales;
        //                        if (currentDate > dueDate)
        //                        {
        //                            amntInterest = totalSales * (interestRate / 100);
        //                            totalPayment += amntInterest;
        //                        }

        //                        // Apply further interest based on accrual and suspension periods
        //                        DateTime accrualDate = dueDate?.AddDays(accrualPeriod) ?? DateTime.Now;
        //                        DateTime suspDate = dueDate?.AddDays(suspendPeriod) ?? DateTime.Now;

        //                        while (currentDate >= accrualDate && currentDate <= suspDate)
        //                        {
        //                            amntInterest = totalPayment * (interestRate / 100);
        //                            totalPayment += amntInterest;
        //                            accrualDate = accrualDate.AddDays(accrualPeriod); // Increment to next accrual period
        //                        }
        //                    }
        //                }
        //            }

        //            // Calculate total waste price for booking and VAT
        //            string totalQuery = "SELECT SUM(bw_total_price) FROM booking_waste WHERE bk_id = @BkId";
        //            using (var totalCmd = new NpgsqlCommand(totalQuery, db))
        //            {
        //                totalCmd.Parameters.AddWithValue("@BkId", bkID);
        //                object result = totalCmd.ExecuteScalar();

        //                if (result != DBNull.Value)
        //                {
        //                    totalSum += Convert.ToDouble(result);
        //                    //vat_Amnt = totalSum * (taxValue / 100.0);
        //                    //totAmnt = vat_Amnt + totalSum;
        //                }
        //            }
        //            //netVat = totalSum + addFee;
        //            //totAmnt = vat_Amnt + totalPayment;
        //            netVat = totalSum + addFee;
        //            vat_Amnt = netVat * (taxValue / 100.0);
        //            //totAmnt = vat_Amnt + totalSales;
        //            // Add Title
        //            Paragraph title = new Paragraph("Billing Statement")
        //                .SetFont(boldFont)
        //                .SetFontSize(16)
        //                .SetTextAlignment(TextAlignment.CENTER);
        //            document.Add(title);

        //            iText.Layout.Element.Table infoTable = new iText.Layout.Element.Table(2).UseAllAvailableWidth();

        //            infoTable.SetBorder(Border.NO_BORDER);

        //            // Bill ID cell
        //            infoTable.AddCell(new Cell()
        //                .SetBorder(Border.NO_BORDER)
        //                .Add(new Paragraph($"Bill ID: {buttonText}")
        //                    .SetFont(boldFont)
        //                    .SetBorder(Border.NO_BORDER)));

        //            // Invoice # cell, aligned to the right
        //            infoTable.AddCell(new Cell()
        //                .SetBorder(Border.NO_BORDER)
        //                .Add(new Paragraph($"Date Today: {currentDate?.ToString("MM/dd/yyyy") ?? "N/A"}")
        //                    .SetFont(boldFont)
        //                    .SetTextAlignment(TextAlignment.RIGHT)
        //                    .SetBorder(Border.NO_BORDER)));

        //            // Booking ID cell
        //            infoTable.AddCell(new Cell()
        //                .SetBorder(Border.NO_BORDER)
        //                .Add(new Paragraph($"Booking ID: {bkID}")
        //                    .SetFont(boldFont)
        //                    .SetBorder(Border.NO_BORDER)));

        //            // Date Issued cell, aligned to the right
        //            infoTable.AddCell(new Cell()
        //                .SetBorder(Border.NO_BORDER)
        //                .Add(new Paragraph($"Date Issued: {(dateIssued?.ToString("MM/dd/yyyy") ?? "N/A")}")
        //                    .SetFont(boldFont)
        //                    .SetTextAlignment(TextAlignment.RIGHT)
        //                    .SetBorder(Border.NO_BORDER)));

        //            // Empty cell for spacing
        //            infoTable.AddCell(new Cell()
        //                .SetBorder(Border.NO_BORDER)
        //                .Add(new Paragraph("").SetBorder(Border.NO_BORDER)));

        //            // Due Date cell, aligned to the right
        //            infoTable.AddCell(new Cell()
        //                .SetBorder(Border.NO_BORDER)
        //                .Add(new Paragraph($"Due Date: {(dueDate?.ToString("MM/dd/yyyy") ?? "N/A")}")
        //                    .SetFont(boldFont)
        //                    .SetTextAlignment(TextAlignment.RIGHT)
        //                    .SetBorder(Border.NO_BORDER)));

        //            // Add the table to the document
        //            document.Add(infoTable);
        //        }


        //        // Add Terms
        //        Paragraph termsTitle = new Paragraph("TERMS:")
        //                .SetFont(boldFont)
        //                .SetFontSize(12);
        //        document.Add(termsTitle);

        //        // Create terms content paragraph with formatted strings using default values
        //        Paragraph termsContent = new Paragraph(
        //            $"The bill shall be due for payment and collection ({leadDays}) day/s after issuance. " +
        //            $"Failure by the customer to make payment without valid and justifiable reason will result in a late payment charge of ({interestRate}%) " +
        //            $"per {accPerValue} day/s applied to any outstanding balance until {susPerValue} day/s. " +
        //            $"Additionally, TrashTrack reserves the right to stop collecting waste materials from the customer's premises if payment is not made, " +
        //            $"preventing further processing and disposal services."
        //        )
        //        .SetFont(font)
        //        .SetTextAlignment(TextAlignment.JUSTIFIED)
        //        .SetFontSize(10);

        //        // Add the terms content to the document
        //        document.Add(termsContent);

        //        // Add Waste Details Table
        //        iText.Layout.Element.Table wasteTable = new iText.Layout.Element.Table(new float[] { 100, 50, 80, 80, 100 }).UseAllAvailableWidth();
        //        wasteTable.SetMarginTop(20);

        //        // Add table headers with bottom border
        //        wasteTable.AddHeaderCell(new Cell()
        //            .Add(new Paragraph("Waste Type").SetFont(boldFont))
        //            .SetTextAlignment(TextAlignment.LEFT)
        //            .SetBorderTop(Border.NO_BORDER)
        //            .SetBorderLeft(Border.NO_BORDER)
        //            .SetBorderRight(Border.NO_BORDER)
        //            .SetBorderBottom(new SolidBorder(1f)));  // Add bold bottom border for header

        //        wasteTable.AddHeaderCell(new Cell()
        //            .Add(new Paragraph("Unit").SetFont(boldFont))
        //            .SetTextAlignment(TextAlignment.LEFT)
        //            .SetBorderTop(Border.NO_BORDER)
        //            .SetBorderLeft(Border.NO_BORDER)
        //            .SetBorderRight(Border.NO_BORDER)
        //            .SetBorderBottom(new SolidBorder(1f)));  // Add bold bottom border for header

        //        wasteTable.AddHeaderCell(new Cell()
        //            .Add(new Paragraph("Total Unit").SetFont(boldFont))
        //            .SetTextAlignment(TextAlignment.LEFT)
        //            .SetBorderTop(Border.NO_BORDER)
        //            .SetBorderLeft(Border.NO_BORDER)
        //            .SetBorderRight(Border.NO_BORDER)
        //            .SetBorderBottom(new SolidBorder(1f)));  // Add bold bottom border for header

        //        wasteTable.AddHeaderCell(new Cell()
        //            .Add(new Paragraph("Unit Price").SetFont(boldFont))
        //            .SetTextAlignment(TextAlignment.LEFT)
        //            .SetBorderTop(Border.NO_BORDER)
        //            .SetBorderLeft(Border.NO_BORDER)
        //            .SetBorderRight(Border.NO_BORDER)
        //            .SetBorderBottom(new SolidBorder(1f)));  // Add bold bottom border for header

        //        wasteTable.AddHeaderCell(new Cell()
        //            .Add(new Paragraph("Total Price").SetFont(boldFont))
        //            .SetTextAlignment(TextAlignment.LEFT)
        //            .SetBorderTop(Border.NO_BORDER)
        //            .SetBorderLeft(Border.NO_BORDER)
        //            .SetBorderRight(Border.NO_BORDER)
        //            .SetBorderBottom(new SolidBorder(1f)));  // Add bold bottom border for header



        //        // Fetch booking_waste entries related to the booking ID
        //        using (var dbb = new NpgsqlConnection(con))
        //        {
        //            dbb.Open();

        //            string wasteQuery = @"SELECT bw_name, bw_unit, bw_total_unit, bw_price, bw_total_price 
        //                          FROM booking_waste 
        //                          WHERE bk_id = @BkId";
        //            using (var wasteCmd = new NpgsqlCommand(wasteQuery, dbb))
        //            {
        //                wasteCmd.Parameters.AddWithValue("@BkId", bkID);

        //                using (var wasteReader = wasteCmd.ExecuteReader())
        //                {
        //                    while (wasteReader.Read())
        //                    {
        //                        // Add detail rows without borders
        //                        wasteTable.AddCell(new Cell()
        //                            .Add(new Paragraph(wasteReader["bw_name"].ToString()))
        //                            .SetFont(font)
        //                            .SetTextAlignment(TextAlignment.LEFT)
        //                            .SetBorder(Border.NO_BORDER));

        //                        wasteTable.AddCell(new Cell()
        //                            .Add(new Paragraph(wasteReader["bw_unit"].ToString()))
        //                            .SetTextAlignment(TextAlignment.LEFT)
        //                            .SetBorder(Border.NO_BORDER));

        //                        wasteTable.AddCell(new Cell()
        //                            .Add(new Paragraph(wasteReader["bw_total_unit"].ToString()))
        //                            .SetTextAlignment(TextAlignment.LEFT)
        //                            .SetBorder(Border.NO_BORDER));

        //                        wasteTable.AddCell(new Cell()
        //                            .Add(new Paragraph("₱" + wasteReader["bw_price"].ToString()))
        //                            .SetFont(font)
        //                            .SetTextAlignment(TextAlignment.LEFT)
        //                            .SetBorder(Border.NO_BORDER));

        //                        wasteTable.AddCell(new Cell()
        //                            .Add(new Paragraph("₱ " + wasteReader["bw_total_price"].ToString()))
        //                            .SetFont(font)
        //                            .SetTextAlignment(TextAlignment.LEFT)
        //                            .SetBorder(Border.NO_BORDER));
        //                    }
        //                }
        //            }

        //        }

        //        document.Add(wasteTable);


        //        // Define the width for the bottom line
        //        float[] bottomLineWidths = new float[] { 1 }; // Single column for the line
        //        iText.Layout.Element.Table btmLine = new iText.Layout.Element.Table(bottomLineWidths).UseAllAvailableWidth();

        //        // Add a cell for the line with a top border
        //        btmLine.AddCell(new Cell()
        //            .SetBorder(Border.NO_BORDER)
        //            .Add(new Paragraph("")
        //                .SetBorder(Border.NO_BORDER)
        //                .SetBorderTop(new SolidBorder(1f))
        //            )
        //        );

        //        // Add the bottom line table to the document
        //        document.Add(btmLine);



        //        float[] columnWidths = new float[] { 100, 40, 30, 80, 100 }; // Set fixed pixel widths
        //        iText.Layout.Element.Table summarySection = new iText.Layout.Element.Table(columnWidths).UseAllAvailableWidth();

        //        // Method to add empty cells
        //        void AddEmptyCell(iText.Layout.Element.Table table)
        //        {
        //            table.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(new Paragraph("").SetBorder(Border.NO_BORDER)));
        //        }


        //        for (int i = 0; i < 3; i++)
        //        {
        //            AddEmptyCell(summarySection); // Adding empty cells for spacing
        //        }

        //        // Add Net of VAT label
        //        summarySection.AddCell(new Cell()
        //            .SetBorder(Border.NO_BORDER) // No border for the cell
        //            .Add(new Paragraph("Total Sum: ").SetFont(boldFont)
        //                .SetTextAlignment(TextAlignment.LEFT)
        //                .SetBorder(Border.NO_BORDER)));

        //        // Add Net of VAT amount
        //        summarySection.AddCell(new Cell()
        //            .SetBorder(Border.NO_BORDER) // No border for the cell
        //            .Add(new Paragraph("₱" + totalSum.ToString("N2"))
        //                .SetFont(boldFont)
        //                .SetFont(font)
        //                .SetTextAlignment(TextAlignment.LEFT)
        //                .SetBorder(Border.NO_BORDER)));

        //        // Add empty cells for spacing
        //        for (int i = 0; i < 3; i++)
        //        {
        //            AddEmptyCell(summarySection);
        //        }

        //        // Add Additional Fee label
        //        summarySection.AddCell(new Cell()
        //            .SetBorder(Border.NO_BORDER)
        //            .Add(new Paragraph("Additional Fee: ")
        //                .SetFont(boldFont)
        //                .SetTextAlignment(TextAlignment.LEFT)
        //                .SetBorder(Border.NO_BORDER)));

        //        // Add Total Amount due
        //        summarySection.AddCell(new Cell()
        //            .SetBorder(Border.NO_BORDER)
        //            .Add(new Paragraph("₱" + (addFee.ToString("N2")))
        //                .SetFont(font)
        //                .SetTextAlignment(TextAlignment.LEFT)
        //                .SetBorder(Border.NO_BORDER)));


        //        for (int i = 0; i < 3; i++)
        //        {
        //            AddEmptyCell(summarySection); // Adding empty cells for spacing
        //        }

        //        // Add Net of VAT label
        //        summarySection.AddCell(new Cell()
        //            .SetBorder(Border.NO_BORDER) // No border for the cell
        //            .Add(new Paragraph("Net of VAT: ").SetFont(boldFont)
        //                .SetTextAlignment(TextAlignment.LEFT)
        //                .SetBorder(Border.NO_BORDER)));

        //        // Add Net of VAT amount
        //        summarySection.AddCell(new Cell()
        //            .SetBorder(Border.NO_BORDER) // No border for the cell
        //            .Add(new Paragraph("₱" + netVat.ToString("N2"))
        //                .SetFont(boldFont)
        //                .SetFont(font)
        //                .SetTextAlignment(TextAlignment.LEFT)
        //                .SetBorder(Border.NO_BORDER)));

        //        // Add empty cells for spacing
        //        for (int i = 0; i < 3; i++)
        //        {
        //            AddEmptyCell(summarySection);
        //        }

        //        // Add VAT label
        //        summarySection.AddCell(new Cell()
        //            .SetBorder(Border.NO_BORDER) // No border for the cell
        //            .Add(new Paragraph("VAT (" + taxValue + "%): ")
        //                .SetFont(boldFont)
        //                .SetTextAlignment(TextAlignment.LEFT)
        //                .SetBorder(Border.NO_BORDER)));

        //        // Add VAT amount
        //        summarySection.AddCell(new Cell()
        //            .SetBorder(Border.NO_BORDER) // No border for the cell
        //            .Add(new Paragraph("₱" + vat_Amnt.ToString("N2"))
        //                .SetFont(boldFont)
        //                .SetFont(font)
        //                .SetTextAlignment(TextAlignment.LEFT)
        //                .SetBorder(Border.NO_BORDER)));

        //        // Add empty cells for spacing
        //        for (int i = 0; i < 3; i++)
        //        {
        //            AddEmptyCell(summarySection);
        //        }

        //        // Add Total Sales label
        //        summarySection.AddCell(new Cell()
        //            .SetBorder(Border.NO_BORDER)
        //            .Add(new Paragraph("Total Sales: ")
        //                .SetFont(boldFont)
        //                .SetTextAlignment(TextAlignment.LEFT)
        //                .SetBorder(Border.NO_BORDER)));

        //        // Add Total Sales amount
        //        summarySection.AddCell(new Cell()
        //            .SetBorder(Border.NO_BORDER)
        //            .Add(new Paragraph("₱" + totalSales.ToString("N2"))
        //                .SetFont(font)
        //                .SetTextAlignment(TextAlignment.LEFT)
        //                .SetBorder(Border.NO_BORDER)));


        //        // Add empty cells for spacing
        //        for (int i = 0; i < 3; i++)
        //        {
        //            AddEmptyCell(summarySection);
        //        }

        //        // Add Total Amount label
        //        summarySection.AddCell(new Cell()
        //            .SetBorder(Border.NO_BORDER)
        //            .Add(new Paragraph("Total Amount: ")
        //                .SetFont(boldFont)
        //                .SetFontColor(redColor)
        //                .SetTextAlignment(TextAlignment.LEFT)
        //                .SetBorder(Border.NO_BORDER)));

        //        // Calculate Total Due
        //        //double totalDue = totalPayment + (addFee.HasValue && addFee.Value > 0 ? addFee.Value : 0);

        //        // Add Total Amount due
        //        summarySection.AddCell(new Cell()
        //            .SetBorder(Border.NO_BORDER)
        //            .Add(new Paragraph("₱" + totalPayment.ToString("N2"))
        //                .SetFont(boldFont)
        //                .SetFontColor(redColor)
        //                .SetTextAlignment(TextAlignment.LEFT)
        //                .SetBorder(Border.NO_BORDER)));

        //        // Add the summary section table to the document
        //        document.Add(summarySection);


        //        // Close document
        //        document.Close();

        //        return ms.ToArray();

        //    }
        //}



        ////VIEW PDF BILL
        //private byte[] GeneratePDFViewBill(int buttonText, int bkID)
        //{
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        PdfWriter writer = new PdfWriter(ms);
        //        PdfDocument pdf = new PdfDocument(writer);
        //        ITextDocument document = new ITextDocument(pdf);

        //        // Define fonts and colors
        //        PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
        //        DeviceRgb redColor = new DeviceRgb(255, 0, 0);
        //        string fontPath = Server.MapPath("~/fonts/Roboto/Roboto-Regular.ttf");
        //        PdfFont font = PdfFontFactory.CreateFont(fontPath, "Identity-H", PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);



        //        // Create a table with one row and two columns for the logo and address
        //        iText.Layout.Element.Table headerTable = new iText.Layout.Element.Table(new float[] { 1, 3 })
        //            .UseAllAvailableWidth();

        //        // Add company logo (if any)
        //        string logoPath = Server.MapPath("~/Pictures/logo_bgRM.png");
        //        iText.Layout.Element.Image logo = new iText.Layout.Element.Image(ImageDataFactory.Create(logoPath));
        //        logo.ScaleToFit(100, 50);

        //        iText.Layout.Element.Table logoTextTable = new iText.Layout.Element.Table(2)
        //            .UseAllAvailableWidth();

        //        // Create the logo cell
        //        Cell logoCell = new Cell()
        //            .SetBorder(Border.NO_BORDER)
        //            .Add(logo);

        //        // Define the green color
        //        DeviceRgb greenColor = new DeviceRgb(0, 128, 0);

        //        // Create the TrashTrack text without margin and padding
        //        Paragraph trashTrackText = new Paragraph("TrashTrack")
        //            .SetFont(boldFont)
        //            .SetFontSize(30)
        //            .SetFontColor(greenColor)
        //            .SetTextAlignment(TextAlignment.LEFT)
        //            .SetMargin(0)
        //            .SetPadding(0);

        //        // Create a cell for the TrashTrack text
        //        Cell textCell = new Cell()
        //            .SetBorder(Border.NO_BORDER)
        //            .Add(trashTrackText);

        //        // Add both cells to the logoTextTable
        //        logoTextTable.AddCell(logoCell);
        //        logoTextTable.AddCell(textCell);

        //        // Add the logo and text table to the headerTable
        //        headerTable.AddCell(new Cell()
        //            .SetBorder(Border.NO_BORDER)
        //            .Add(logoTextTable));

        //        // Add Company Address to the second cell
        //        Paragraph address = new Paragraph("Binaliw Cebu Dumpsite\nCebu City, Cebu\nPhilippines")
        //            .SetFont(font)
        //            .SetTextAlignment(TextAlignment.RIGHT);

        //        headerTable.AddCell(new Cell()
        //            .SetBorder(Border.NO_BORDER)
        //            .Add(address));

        //        // Add the header table to the document
        //        document.Add(headerTable);

        //        // Initialize variables with default values to avoid uninitialized errors 
        //        int leadDays = 0;
        //        int taxValue = 0;
        //        int accPerValue = 0;
        //        int susPerValue = 0;
        //        double totalSum = 0;
        //        double vat_Amnt = 0;
        //        double totalPayment = 0;
        //        double amntInterest = 0;
        //        double interestRate = 0;
        //        int accrualPeriod = 0;
        //        int suspendPeriod = 0;
        //        double totalSales = 0;
        //        double addFee = 0;
        //        double netVat = 0;
        //        // Use a nullable DateTime for dateIssued in case it’s not set
        //        DateTime? dateIssued = null;
        //        DateTime? dueDate = null;

        //        // Parse the entered date
        //        DateTime? currentDate = DateTime.TryParse(dateEntered.Text, out DateTime dateCurrent) ? (DateTime?)dateCurrent : null;
        //        //double totAmnt = 0;
        //        double totAmnt = totalPayment + vat_Amnt;
        //        // Database connection and fetching values
        //        using (var db = new NpgsqlConnection(con))
        //        {
        //            db.Open();

        //            // Fetch interest and leadDays values from PostgreSQL
        //            string controlQuery = "SELECT * FROM payment_term";
        //            using (var cmd = new NpgsqlCommand(controlQuery, db))
        //            {
        //                using (var reader = cmd.ExecuteReader())
        //                {
        //                    if (reader.Read())
        //                    {
        //                        leadDays = Convert.ToInt32(reader["pt_lead_days"]);
        //                        taxValue = Convert.ToInt32(reader["pt_tax"]);
        //                        accPerValue = Convert.ToInt32(reader["pt_accrual_period"]);
        //                        susPerValue = Convert.ToInt32(reader["pt_susp_period"]);
        //                    }
        //                    else
        //                    {
        //                        Console.WriteLine("No payment term data found. Using default values.");
        //                    }
        //                }
        //            }

        //            // Fetch bill details (e.g., date issued)
        //            string billQuery = "SELECT * FROM generate_bill WHERE gb_id = @BillId";
        //            using (var billCmd = new NpgsqlCommand(billQuery, db))
        //            {
        //                billCmd.Parameters.AddWithValue("@BillId", buttonText);

        //                using (var billReader = billCmd.ExecuteReader())
        //                {
        //                    if (billReader.Read())
        //                    {
        //                        addFee = billReader["gb_add_fees"] == DBNull.Value ? 0.0 : Convert.ToDouble(billReader["gb_add_fees"]);
        //                        dateIssued = Convert.ToDateTime(billReader["gb_date_issued"]);
        //                        totalSales = Convert.ToDouble(billReader["gb_total_sales"]);
        //                        interestRate = Convert.ToDouble(billReader["gb_interest"]);
        //                        accrualPeriod = Convert.ToInt32(billReader["gb_accrual_period"]);
        //                        suspendPeriod = Convert.ToInt32(billReader["gb_suspend_period"]);

        //                        dueDate = dateIssued?.AddDays(leadDays); // Calculate due date

        //                        // Calculate the initial total payment and interest if current date is past due date
        //                        totalPayment = totalSales;
        //                        if (currentDate > dueDate)
        //                        {
        //                            amntInterest = totalSales * (interestRate / 100);
        //                            totalPayment += amntInterest;
        //                        }

        //                        // Apply further interest based on accrual and suspension periods
        //                        DateTime accrualDate = dueDate?.AddDays(accrualPeriod) ?? DateTime.Now;
        //                        DateTime suspDate = dueDate?.AddDays(suspendPeriod) ?? DateTime.Now;

        //                        while (currentDate >= accrualDate && currentDate <= suspDate)
        //                        {
        //                            amntInterest = totalPayment * (interestRate / 100);
        //                            totalPayment += amntInterest;
        //                            accrualDate = accrualDate.AddDays(accrualPeriod); // Increment to next accrual period
        //                        }
        //                    }
        //                }
        //            }

        //            // Calculate total waste price for booking and VAT
        //            string totalQuery = "SELECT SUM(bw_total_price) FROM booking_waste WHERE bk_id = @BkId";
        //            using (var totalCmd = new NpgsqlCommand(totalQuery, db))
        //            {
        //                totalCmd.Parameters.AddWithValue("@BkId", bkID);
        //                object result = totalCmd.ExecuteScalar();

        //                if (result != DBNull.Value)
        //                {
        //                    totalSum += Convert.ToDouble(result);
        //                    //vat_Amnt = totalSum * (taxValue / 100.0);
        //                    //totAmnt = vat_Amnt + totalSum;
        //                }
        //            }
        //            //netVat = totalSum + addFee;
        //            //totAmnt = vat_Amnt + totalPayment;
        //            netVat = totalSum + addFee;
        //            vat_Amnt = netVat * (taxValue / 100.0);
        //            //totAmnt = vat_Amnt + totalSales;
        //            // Add Title
        //            Paragraph title = new Paragraph("Billing Statement")
        //                .SetFont(boldFont)
        //                .SetFontSize(16)
        //                .SetTextAlignment(TextAlignment.CENTER);
        //            document.Add(title);

        //            iText.Layout.Element.Table infoTable = new iText.Layout.Element.Table(2).UseAllAvailableWidth();

        //            infoTable.SetBorder(Border.NO_BORDER);

        //            // Bill ID cell
        //            infoTable.AddCell(new Cell()
        //                .SetBorder(Border.NO_BORDER)
        //                .Add(new Paragraph($"Bill ID: {buttonText}")
        //                    .SetFont(boldFont)
        //                    .SetBorder(Border.NO_BORDER)));

        //            // Invoice # cell, aligned to the right
        //            infoTable.AddCell(new Cell()
        //                .SetBorder(Border.NO_BORDER)
        //                .Add(new Paragraph($"Date Today: {currentDate?.ToString("MM/dd/yyyy") ?? "N/A"}")
        //                    .SetFont(boldFont)
        //                    .SetTextAlignment(TextAlignment.RIGHT)
        //                    .SetBorder(Border.NO_BORDER)));

        //            // Booking ID cell
        //            infoTable.AddCell(new Cell()
        //                .SetBorder(Border.NO_BORDER)
        //                .Add(new Paragraph($"Booking ID: {bkID}")
        //                    .SetFont(boldFont)
        //                    .SetBorder(Border.NO_BORDER)));

        //            // Date Issued cell, aligned to the right
        //            infoTable.AddCell(new Cell()
        //                .SetBorder(Border.NO_BORDER)
        //                .Add(new Paragraph($"Date Issued: {(dateIssued?.ToString("MM/dd/yyyy") ?? "N/A")}")
        //                    .SetFont(boldFont)
        //                    .SetTextAlignment(TextAlignment.RIGHT)
        //                    .SetBorder(Border.NO_BORDER)));

        //            // Empty cell for spacing
        //            infoTable.AddCell(new Cell()
        //                .SetBorder(Border.NO_BORDER)
        //                .Add(new Paragraph("").SetBorder(Border.NO_BORDER)));

        //            // Due Date cell, aligned to the right
        //            infoTable.AddCell(new Cell()
        //                .SetBorder(Border.NO_BORDER)
        //                .Add(new Paragraph($"Due Date: {(dueDate?.ToString("MM/dd/yyyy") ?? "N/A")}")
        //                    .SetFont(boldFont)
        //                    .SetTextAlignment(TextAlignment.RIGHT)
        //                    .SetBorder(Border.NO_BORDER)));

        //            // Add the table to the document
        //            document.Add(infoTable);
        //        }


        //        // Add Terms
        //        Paragraph termsTitle = new Paragraph("TERMS:")
        //                .SetFont(boldFont)
        //                .SetFontSize(12);
        //        document.Add(termsTitle);

        //        // Create terms content paragraph with formatted strings using default values
        //        Paragraph termsContent = new Paragraph(
        //            $"The bill shall be due for payment and collection ({leadDays}) day/s after issuance. " +
        //            $"Failure by the customer to make payment without valid and justifiable reason will result in a late payment charge of ({interestRate}%) " +
        //            $"per {accPerValue} day/s applied to any outstanding balance until {susPerValue} day/s. " +
        //            $"Additionally, TrashTrack reserves the right to stop collecting waste materials from the customer's premises if payment is not made, " +
        //            $"preventing further processing and disposal services."
        //        )
        //        .SetFont(font)
        //        .SetTextAlignment(TextAlignment.JUSTIFIED)
        //        .SetFontSize(10);

        //        // Add the terms content to the document
        //        document.Add(termsContent);

        //        // Add Waste Details Table
        //        iText.Layout.Element.Table wasteTable = new iText.Layout.Element.Table(new float[] { 100, 50, 80, 80, 100 }).UseAllAvailableWidth();
        //        wasteTable.SetMarginTop(20);

        //        // Add table headers with bottom border
        //        wasteTable.AddHeaderCell(new Cell()
        //            .Add(new Paragraph("Waste Type").SetFont(boldFont))
        //            .SetTextAlignment(TextAlignment.LEFT)
        //            .SetBorderTop(Border.NO_BORDER)
        //            .SetBorderLeft(Border.NO_BORDER)
        //            .SetBorderRight(Border.NO_BORDER)
        //            .SetBorderBottom(new SolidBorder(1f)));  // Add bold bottom border for header

        //        wasteTable.AddHeaderCell(new Cell()
        //            .Add(new Paragraph("Unit").SetFont(boldFont))
        //            .SetTextAlignment(TextAlignment.LEFT)
        //            .SetBorderTop(Border.NO_BORDER)
        //            .SetBorderLeft(Border.NO_BORDER)
        //            .SetBorderRight(Border.NO_BORDER)
        //            .SetBorderBottom(new SolidBorder(1f)));  // Add bold bottom border for header

        //        wasteTable.AddHeaderCell(new Cell()
        //            .Add(new Paragraph("Total Unit").SetFont(boldFont))
        //            .SetTextAlignment(TextAlignment.LEFT)
        //            .SetBorderTop(Border.NO_BORDER)
        //            .SetBorderLeft(Border.NO_BORDER)
        //            .SetBorderRight(Border.NO_BORDER)
        //            .SetBorderBottom(new SolidBorder(1f)));  // Add bold bottom border for header

        //        wasteTable.AddHeaderCell(new Cell()
        //            .Add(new Paragraph("Unit Price").SetFont(boldFont))
        //            .SetTextAlignment(TextAlignment.LEFT)
        //            .SetBorderTop(Border.NO_BORDER)
        //            .SetBorderLeft(Border.NO_BORDER)
        //            .SetBorderRight(Border.NO_BORDER)
        //            .SetBorderBottom(new SolidBorder(1f)));  // Add bold bottom border for header

        //        wasteTable.AddHeaderCell(new Cell()
        //            .Add(new Paragraph("Total Price").SetFont(boldFont))
        //            .SetTextAlignment(TextAlignment.LEFT)
        //            .SetBorderTop(Border.NO_BORDER)
        //            .SetBorderLeft(Border.NO_BORDER)
        //            .SetBorderRight(Border.NO_BORDER)
        //            .SetBorderBottom(new SolidBorder(1f)));  // Add bold bottom border for header



        //        // Fetch booking_waste entries related to the booking ID
        //        using (var dbb = new NpgsqlConnection(con))
        //        {
        //            dbb.Open();

        //            string wasteQuery = @"SELECT bw_name, bw_unit, bw_total_unit, bw_price, bw_total_price 
        //                          FROM booking_waste 
        //                          WHERE bk_id = @BkId";
        //            using (var wasteCmd = new NpgsqlCommand(wasteQuery, dbb))
        //            {
        //                wasteCmd.Parameters.AddWithValue("@BkId", bkID);

        //                using (var wasteReader = wasteCmd.ExecuteReader())
        //                {
        //                    while (wasteReader.Read())
        //                    {
        //                        // Add detail rows without borders
        //                        wasteTable.AddCell(new Cell()
        //                            .Add(new Paragraph(wasteReader["bw_name"].ToString()))
        //                            .SetFont(font)
        //                            .SetTextAlignment(TextAlignment.LEFT)
        //                            .SetBorder(Border.NO_BORDER));

        //                        wasteTable.AddCell(new Cell()
        //                            .Add(new Paragraph(wasteReader["bw_unit"].ToString()))
        //                            .SetTextAlignment(TextAlignment.LEFT)
        //                            .SetBorder(Border.NO_BORDER));

        //                        wasteTable.AddCell(new Cell()
        //                            .Add(new Paragraph(wasteReader["bw_total_unit"].ToString()))
        //                            .SetTextAlignment(TextAlignment.LEFT)
        //                            .SetBorder(Border.NO_BORDER));

        //                        wasteTable.AddCell(new Cell()
        //                            .Add(new Paragraph("₱" + wasteReader["bw_price"].ToString()))
        //                            .SetFont(font)
        //                            .SetTextAlignment(TextAlignment.LEFT)
        //                            .SetBorder(Border.NO_BORDER));

        //                        wasteTable.AddCell(new Cell()
        //                            .Add(new Paragraph("₱ " + wasteReader["bw_total_price"].ToString()))
        //                            .SetFont(font)
        //                            .SetTextAlignment(TextAlignment.LEFT)
        //                            .SetBorder(Border.NO_BORDER));
        //                    }
        //                }
        //            }

        //        }

        //        document.Add(wasteTable);


        //        // Define the width for the bottom line
        //        float[] bottomLineWidths = new float[] { 1 }; // Single column for the line
        //        iText.Layout.Element.Table btmLine = new iText.Layout.Element.Table(bottomLineWidths).UseAllAvailableWidth();

        //        // Add a cell for the line with a top border
        //        btmLine.AddCell(new Cell()
        //            .SetBorder(Border.NO_BORDER)
        //            .Add(new Paragraph("")
        //                .SetBorder(Border.NO_BORDER)
        //                .SetBorderTop(new SolidBorder(1f))
        //            )
        //        );

        //        // Add the bottom line table to the document
        //        document.Add(btmLine);



        //        float[] columnWidths = new float[] { 100, 40, 30, 80, 100 }; // Set fixed pixel widths
        //        iText.Layout.Element.Table summarySection = new iText.Layout.Element.Table(columnWidths).UseAllAvailableWidth();

        //        // Method to add empty cells
        //        void AddEmptyCell(iText.Layout.Element.Table table)
        //        {
        //            table.AddCell(new Cell().SetBorder(Border.NO_BORDER).Add(new Paragraph("").SetBorder(Border.NO_BORDER)));
        //        }


        //        for (int i = 0; i < 3; i++)
        //        {
        //            AddEmptyCell(summarySection); // Adding empty cells for spacing
        //        }

        //        // Add Net of VAT label
        //        summarySection.AddCell(new Cell()
        //            .SetBorder(Border.NO_BORDER) // No border for the cell
        //            .Add(new Paragraph("Total Sum: ").SetFont(boldFont)
        //                .SetTextAlignment(TextAlignment.LEFT)
        //                .SetBorder(Border.NO_BORDER)));

        //        // Add Net of VAT amount
        //        summarySection.AddCell(new Cell()
        //            .SetBorder(Border.NO_BORDER) // No border for the cell
        //            .Add(new Paragraph("₱" + totalSum.ToString("N2"))
        //                .SetFont(boldFont)
        //                .SetFont(font)
        //                .SetTextAlignment(TextAlignment.LEFT)
        //                .SetBorder(Border.NO_BORDER)));

        //        // Add empty cells for spacing
        //        for (int i = 0; i < 3; i++)
        //        {
        //            AddEmptyCell(summarySection);
        //        }

        //        // Add Additional Fee label
        //        summarySection.AddCell(new Cell()
        //            .SetBorder(Border.NO_BORDER)
        //            .Add(new Paragraph("Additional Fee: ")
        //                .SetFont(boldFont)
        //                .SetTextAlignment(TextAlignment.LEFT)
        //                .SetBorder(Border.NO_BORDER)));

        //        // Add Total Amount due
        //        summarySection.AddCell(new Cell()
        //            .SetBorder(Border.NO_BORDER)
        //            .Add(new Paragraph("₱" + (addFee.ToString("N2")))
        //                .SetFont(font)
        //                .SetTextAlignment(TextAlignment.LEFT)
        //                .SetBorder(Border.NO_BORDER)));


        //        for (int i = 0; i < 3; i++)
        //        {
        //            AddEmptyCell(summarySection); // Adding empty cells for spacing
        //        }

        //        // Add Net of VAT label
        //        summarySection.AddCell(new Cell()
        //            .SetBorder(Border.NO_BORDER) // No border for the cell
        //            .Add(new Paragraph("Net of VAT: ").SetFont(boldFont)
        //                .SetTextAlignment(TextAlignment.LEFT)
        //                .SetBorder(Border.NO_BORDER)));

        //        // Add Net of VAT amount
        //        summarySection.AddCell(new Cell()
        //            .SetBorder(Border.NO_BORDER) // No border for the cell
        //            .Add(new Paragraph("₱" + netVat.ToString("N2"))
        //                .SetFont(boldFont)
        //                .SetFont(font)
        //                .SetTextAlignment(TextAlignment.LEFT)
        //                .SetBorder(Border.NO_BORDER)));

        //        // Add empty cells for spacing
        //        for (int i = 0; i < 3; i++)
        //        {
        //            AddEmptyCell(summarySection);
        //        }

        //        // Add VAT label
        //        summarySection.AddCell(new Cell()
        //            .SetBorder(Border.NO_BORDER) // No border for the cell
        //            .Add(new Paragraph("VAT (" + taxValue + "%): ")
        //                .SetFont(boldFont)
        //                .SetTextAlignment(TextAlignment.LEFT)
        //                .SetBorder(Border.NO_BORDER)));

        //        // Add VAT amount
        //        summarySection.AddCell(new Cell()
        //            .SetBorder(Border.NO_BORDER) // No border for the cell
        //            .Add(new Paragraph("₱" + vat_Amnt.ToString("N2"))
        //                .SetFont(boldFont)
        //                .SetFont(font)
        //                .SetTextAlignment(TextAlignment.LEFT)
        //                .SetBorder(Border.NO_BORDER)));

        //        // Add empty cells for spacing
        //        for (int i = 0; i < 3; i++)
        //        {
        //            AddEmptyCell(summarySection);
        //        }

        //        // Add Total Sales label
        //        summarySection.AddCell(new Cell()
        //            .SetBorder(Border.NO_BORDER)
        //            .Add(new Paragraph("Total Sales: ")
        //                .SetFont(boldFont)
        //                .SetTextAlignment(TextAlignment.LEFT)
        //                .SetBorder(Border.NO_BORDER)));

        //        // Add Total Sales amount
        //        summarySection.AddCell(new Cell()
        //            .SetBorder(Border.NO_BORDER)
        //            .Add(new Paragraph("₱" + totalSales.ToString("N2"))
        //                .SetFont(font)
        //                .SetTextAlignment(TextAlignment.LEFT)
        //                .SetBorder(Border.NO_BORDER)));


        //        // Add empty cells for spacing
        //        for (int i = 0; i < 3; i++)
        //        {
        //            AddEmptyCell(summarySection);
        //        }

        //        // Add Total Amount label
        //        summarySection.AddCell(new Cell()
        //            .SetBorder(Border.NO_BORDER)
        //            .Add(new Paragraph("Total Amount: ")
        //                .SetFont(boldFont)
        //                .SetFontColor(redColor)
        //                .SetTextAlignment(TextAlignment.LEFT)
        //                .SetBorder(Border.NO_BORDER)));

        //        // Calculate Total Due
        //        //double totalDue = totalPayment + (addFee.HasValue && addFee.Value > 0 ? addFee.Value : 0);

        //        // Add Total Amount due
        //        summarySection.AddCell(new Cell()
        //            .SetBorder(Border.NO_BORDER)
        //            .Add(new Paragraph("₱" + totalPayment.ToString("N2"))
        //                .SetFont(boldFont)
        //                .SetFontColor(redColor)
        //                .SetTextAlignment(TextAlignment.LEFT)
        //                .SetBorder(Border.NO_BORDER)));

        //        // Add the summary section table to the document
        //        document.Add(summarySection);


        //        // Close document
        //        document.Close();

        //        return ms.ToArray();

        //    }
        //}

        protected void btnGenerateBill_Click(object sender, EventArgs e)
        {
            int empId = (int)Session["bo_id"]; // Retrieve admin ID from session

            // Retrieve values from your controls
            string note = noteTxt.Text;
            double? additionalFees = string.IsNullOrWhiteSpace(addFeeTxt.Text) ? (double?)null : Convert.ToDouble(addFeeTxt.Text);
            double? netVat = string.IsNullOrWhiteSpace(netVatTxt.Text) ? (double?)null : Convert.ToDouble(netVatTxt.Text);
            double? vatAmount = string.IsNullOrWhiteSpace(vatAmntTxt.Text) ? (double?)null : Convert.ToDouble(vatAmntTxt.Text);
            double? totalSales = string.IsNullOrWhiteSpace(totSalesTxt.Text) ? (double?)null : Convert.ToDouble(totSalesTxt.Text);
            DateTime? dateIssued = DateTime.TryParse(dateTodayTxt.Text, out DateTime issuedDate) ? (DateTime?)issuedDate : null;
            DateTime? dateDue = DateTime.TryParse(dueDateTxt.Text, out DateTime dueDate) ? (DateTime?)dueDate : null;


            // Variables for payment terms
            int? tax = null;
            int bk_id = 0, cus_id = 0, insertedBillId = 0;
            string cus_fullname = "";
            double totalSum = 0, vat_Amnt = 0;
            bool isProcessSuccessful = true;
            this.ModalPopupExtender1.Hide();
            try
            {
                GeneratedBillList();
                LoadBookingList();
                this.ModalPopupExtender1.Hide();
                using (var conn = new NpgsqlConnection(con))
                {
                    conn.Open();

                    // Step 1: Retrieve payment term values
                    string paymentTermQuery = "SELECT pt_tax FROM payment_term";
                    using (var cmd = new NpgsqlCommand(paymentTermQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@EmpId", empId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                
                                tax = reader.IsDBNull(0) ? (int?)null : reader.GetInt32(0);

                            }

                        }
                    }


                    // Step 2: Retrieve booking details
                    string findBkID = "SELECT * FROM booking WHERE bk_id = @BkId";
                    using (var cmd = new NpgsqlCommand(findBkID, conn))
                    {
                        cmd.Parameters.AddWithValue("@BkId", Convert.ToInt32(TextBox1.Text));
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                bk_id = Convert.ToInt32(reader["bk_id"]);
                                cus_id = Convert.ToInt32(reader["cus_id"]);
                                cus_fullname = reader["bk_fullname"].ToString();

                                if (reader["bk_waste_scale_slip"] == DBNull.Value)
                                {
                                    ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
                                        "Swal.fire({ icon: 'error', title: 'Cannot Generate Bill', text: 'Scale slip is not yet available for this booking.', background: '#f8d7da', confirmButtonColor: '#dc3545' });",
                                        true);

                                    return;
                                }

                                ModalPopupExtender1.Hide();
                                LoadBookingList();
                            }
                            else
                            {
                                throw new Exception("Booking ID not found.");
                            }
                        }
                    }

                    LoadBookingList();
                    ModalPopupExtender1.Hide();
                    // Step 3: Validate `bw_total_price`
                    string totalQuery = @"SELECT SUM(bw_total_price) AS total, 
                                               COUNT(*) FILTER (WHERE bw_total_price IS NULL OR bw_total_price = 0) AS invalid_count
                                        FROM booking_waste 
                                        WHERE bk_id = @BkId";

                    using (var totalCmd = new NpgsqlCommand(totalQuery, conn))
                    {
                        totalCmd.Parameters.AddWithValue("@BkId", bk_id);
                        using (var reader = totalCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int invalidCount = reader.GetInt32(1);
                                if (invalidCount > 0)
                                {
                                    ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
                                        "Swal.fire({ icon: 'error', title: 'Empty Total Units!', text: 'Total Units has not been entered yet', background: '#e9f7ef', confirmButtonColor: '#28a745' });",
                                        true);
                                    return; // Stop further execution
                                }
                                LoadBookingList();
                                ModalPopupExtender1.Hide();
                                object totalResult = reader["total"];
                                double totalResultValue = totalResult != DBNull.Value ? Convert.ToDouble(totalResult) : 0;

                                if (totalResult != DBNull.Value)
                                {
                                    totalSum = totalResultValue + (additionalFees ?? 0);
                                    vat_Amnt = totalSum * (tax.HasValue ? tax.Value / 100.0 : 0);
                                    LoadBookingList();
                                    gridViewBookings.DataBind();
                                    ModalPopupExtender1.Hide();

                                }
                            }
                        }
                    }
                    // Step 4: Update booking status
                    string updateStatusQuery = "UPDATE booking SET bk_status = 'Billed' WHERE bk_id = @BkId";
                    using (var updateCmd = new NpgsqlCommand(updateStatusQuery, conn))
                    {
                        updateCmd.Parameters.AddWithValue("@BkId", bk_id);
                        int rowsAffected = updateCmd.ExecuteNonQuery();
                        if (rowsAffected <= 0)
                        {
                            throw new Exception("Failed to update booking status.");
                        }
                        ModalPopupExtender1.Hide();
                        LoadBookingList();
                        GeneratedBillList();
                    }

                    // Step 5: Insert bill details
                    string insertBillQuery = @"
                                            INSERT INTO generate_bill (
                                                gb_note, gb_add_fees, gb_net_vat, gb_vat_amnt, gb_total_sales, 
                                                gb_date_issued, gb_tax, gb_status, bk_id, emp_id
                                            ) 
                                            VALUES (
                                                @Note, @AddFees, @NetVat, @VatAmount, @TotalSales, 
                                                @DateIssued, @Tax, @Status, @BkId, @EmpId
                                            ) 
                                            RETURNING gb_id;";

                    using (var cmd = new NpgsqlCommand(insertBillQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Note", note ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@AddFees", additionalFees ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@NetVat", netVat ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@VatAmount", vatAmount);
                        cmd.Parameters.AddWithValue("@TotalSales", totalSales);
                        cmd.Parameters.AddWithValue("@DateIssued", dateIssued ?? (object)DBNull.Value);
                        //cmd.Parameters.AddWithValue("@DateDue", dateDue ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Tax", tax ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Status", "Unpaid");
                        cmd.Parameters.AddWithValue("@BkId", bk_id);
                        cmd.Parameters.AddWithValue("@EmpId", empId);

                        var result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            insertedBillId = Convert.ToInt32(result);
                            LoadBookingList();
                            GeneratedBillList();
                            gridViewBookings.DataBind();
                            this.ModalPopupExtender1.Hide();

                        }
                        else
                        {
                            throw new Exception("Failed to insert bill.");
                        }
                    }

                    string notif_message = "Your bill is now available for payment 🧾. \n\n" +
                                       "------------------------------------------\n" +
                                       "BILL# " + insertedBillId + "\n\n" +
                                       "Dear " + cus_fullname + ",\n\n" +
                                       "You can now review the details at your convenience. Please check it as soon as possible to avoid any delays. " +
                                       "Thank you for your cooperation 💜";
                    string queryNotif = @"
                                        INSERT INTO notification (
                                            notif_message, notif_type, emp_id, cus_id, bk_id, gb_id
                                        ) 
                                        VALUES (
                                            @Message, 'billed', @EmpId, @CusId, @BkId, @GbId
                                        ) 
                                        RETURNING gb_id;";

                    using (var cmd = new NpgsqlCommand(queryNotif, conn))
                    {
                        cmd.Parameters.AddWithValue("@Message", notif_message ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@EmpId", empId);
                        cmd.Parameters.AddWithValue("@CusId", cus_id);
                        cmd.Parameters.AddWithValue("@BkId", Convert.ToInt32(TextBox1.Text));
                        cmd.Parameters.AddWithValue("@GbId", insertedBillId);

                        var result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            insertedBillId = Convert.ToInt32(result);
                            updatePanel1.Update();
                            LoadBookingList();
                        }
                        else
                        {
                            throw new Exception("No ID returned from insert.");
                        }
                    }
                    GeneratedBillList();
                    LoadBookingList();
                }
                GeneratedBillList();
                LoadBookingList();
                this.ModalPopupExtender1.Hide();

                if (isProcessSuccessful)
                {
                    this.ModalPopupExtender1.Hide();
                    
                    ScriptManager.RegisterStartupScript(this, GetType(), "DownloadPdf",
                    "window.open('DownloadPdfHandler.ashx?billId=" + insertedBillId + "&bkId=" + bk_id + "', '_blank');", true);

                    GeneratedBillList();
                    LoadBookingList();
                    gridViewBookings.DataBind();
                    ScriptManager.RegisterStartupScript(this, GetType(), "showSuccessAlert",
        "Swal.fire({ icon: 'success', title: 'Bill Generated Successfully!', text: 'The bill has been generated and is ready for download.', background: '#e9f7ef', confirmButtonColor: '#28a745' });",
        true);
                    this.ModalPopupExtender1.Hide();
                }
            }
            catch (Exception ex)
            {
                LoadBookingList();
                this.ModalPopupExtender1.Hide();
                isProcessSuccessful = false;
                ScriptManager.RegisterStartupScript(this, GetType(), "errorAlert",
                    $"Swal.fire({{ icon: 'error', title: 'Error!', text: '{ex.Message}', background: '#f8d7da', confirmButtonColor: '#dc3545' }});",
                    true);
            }
            finally
            {
                LoadBookingList();
                this.ModalPopupExtender1.Hide(); // Ensure the modal always closes
                //ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
                //                        "Swal.fire({ icon: 'error', title: 'Empty Total Units!', text: 'Total Units has not been entered yet', background: '#e9f7ef', confirmButtonColor: '#28a745' });",
                //                        true);
            }
            this.ModalPopupExtender1.Hide();
        }



        //////LATEST 11/26/2024 nga MUGANA, WITH INTEREST DUE DATE, ACCRUAL DATE, SUSPENSION DATE, ACCRUAL PERIOD, SUSPENSION PERIOD 
        //protected void btnGenerateBill_Click(object sender, EventArgs e)
        //{
        //    int empId = (int)Session["bo_id"]; // Retrieve admin ID from session

        //    // Retrieve values from your controls
        //    string note = noteTxt.Text;
        //    double? additionalFees = string.IsNullOrWhiteSpace(addFeeTxt.Text) ? (double?)null : Convert.ToDouble(addFeeTxt.Text);
        //    double? netVat = string.IsNullOrWhiteSpace(netVatTxt.Text) ? (double?)null : Convert.ToDouble(netVatTxt.Text);
        //    double? vatAmount = string.IsNullOrWhiteSpace(vatAmntTxt.Text) ? (double?)null : Convert.ToDouble(vatAmntTxt.Text);
        //    double? totalSales = string.IsNullOrWhiteSpace(totSalesTxt.Text) ? (double?)null : Convert.ToDouble(totSalesTxt.Text);
        //    DateTime? dateIssued = DateTime.TryParse(dateTodayTxt.Text, out DateTime issuedDate) ? (DateTime?)issuedDate : null;
        //    DateTime? dateDue = DateTime.TryParse(dueDateTxt.Text, out DateTime dueDate) ? (DateTime?)dueDate : null;
        //    DateTime? accrualDate = DateTime.TryParse(accDateTxt.Text, out DateTime accDate) ? (DateTime?)accDate : null;
        //    DateTime? suspensionDate = DateTime.TryParse(susDateTxt.Text, out DateTime susDate) ? (DateTime?)susDate : null;

        //    // Variables for payment terms
        //    double? interest = null;
        //    int? leadDays = null;
        //    int? accrualPeriod = null;
        //    int? suspensionPeriod = null;
        //    int? tax = null;
        //    int bk_id = 0, cus_id = 0, insertedBillId = 0;
        //    string cus_fullname = "";
        //    double totalSum = 0, vat_Amnt = 0;
        //    bool isProcessSuccessful = true;
        //    this.ModalPopupExtender1.Hide();
        //    try
        //    {
        //        GeneratedBillList();
        //        LoadBookingList();
        //        this.ModalPopupExtender1.Hide();
        //        using (var conn = new NpgsqlConnection(con))
        //        {
        //            conn.Open();

        //            // Step 1: Retrieve payment term values
        //            string paymentTermQuery = "SELECT pt_interest, pt_lead_days, pt_accrual_period, pt_susp_period, pt_tax FROM payment_term WHERE emp_id = @EmpId";
        //            using (var cmd = new NpgsqlCommand(paymentTermQuery, conn))
        //            {
        //                cmd.Parameters.AddWithValue("@EmpId", empId);
        //                using (var reader = cmd.ExecuteReader())
        //                {
        //                    if (reader.Read())
        //                    {
        //                        interest = reader.IsDBNull(0) ? (double?)null : reader.GetDouble(0);
        //                        leadDays = reader.IsDBNull(1) ? (int?)null : reader.GetInt32(1);
        //                        accrualPeriod = reader.IsDBNull(2) ? (int?)null : reader.GetInt32(2);
        //                        suspensionPeriod = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3);
        //                        tax = reader.IsDBNull(4) ? (int?)null : reader.GetInt32(4);

        //                    }

        //                }
        //            }


        //            // Step 2: Retrieve booking details
        //            string findBkID = "SELECT * FROM booking WHERE bk_id = @BkId";
        //            using (var cmd = new NpgsqlCommand(findBkID, conn))
        //            {
        //                cmd.Parameters.AddWithValue("@BkId", Convert.ToInt32(TextBox1.Text));
        //                using (var reader = cmd.ExecuteReader())
        //                {
        //                    if (reader.Read())
        //                    {
        //                        bk_id = Convert.ToInt32(reader["bk_id"]);
        //                        cus_id = Convert.ToInt32(reader["cus_id"]);
        //                        cus_fullname = reader["bk_fullname"].ToString();

        //                        if (reader["bk_waste_scale_slip"] == DBNull.Value)
        //                        {
        //                            ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
        //                                "Swal.fire({ icon: 'error', title: 'Cannot Generate Bill', text: 'Scale slip is not yet available for this booking.', background: '#f8d7da', confirmButtonColor: '#dc3545' });",
        //                                true);

        //                            return; 
        //                        }

        //                        ModalPopupExtender1.Hide();
        //                        LoadBookingList();
        //                    }
        //                    else
        //                    {
        //                        throw new Exception("Booking ID not found.");
        //                    }
        //                }
        //            }

        //            LoadBookingList();
        //            ModalPopupExtender1.Hide();
        //            // Step 3: Validate `bw_total_price`
        //            string totalQuery = @"SELECT SUM(bw_total_price) AS total, 
        //                                       COUNT(*) FILTER (WHERE bw_total_price IS NULL OR bw_total_price = 0) AS invalid_count
        //                                FROM booking_waste 
        //                                WHERE bk_id = @BkId";

        //            using (var totalCmd = new NpgsqlCommand(totalQuery, conn))
        //            {
        //                totalCmd.Parameters.AddWithValue("@BkId", bk_id);
        //                using (var reader = totalCmd.ExecuteReader())
        //                {
        //                    if (reader.Read())
        //                    {
        //                        int invalidCount = reader.GetInt32(1);
        //                        if (invalidCount > 0)
        //                        {
        //                            ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
        //                                "Swal.fire({ icon: 'error', title: 'Empty Total Units!', text: 'Total Units has not been entered yet', background: '#e9f7ef', confirmButtonColor: '#28a745' });",
        //                                true);
        //                            return; // Stop further execution
        //                        }
        //                        LoadBookingList();
        //                        ModalPopupExtender1.Hide();
        //                        object totalResult = reader["total"];
        //                        double totalResultValue = totalResult != DBNull.Value ? Convert.ToDouble(totalResult) : 0;

        //                        if (totalResult != DBNull.Value)
        //                        {
        //                            totalSum = totalResultValue + (additionalFees ?? 0);
        //                            vat_Amnt = totalSum * (tax.HasValue ? tax.Value / 100.0 : 0);
        //                            LoadBookingList();
        //                            gridViewBookings.DataBind();
        //                            ModalPopupExtender1.Hide();

        //                        }
        //                    }
        //                }
        //            }
        //            // Step 4: Update booking status
        //            string updateStatusQuery = "UPDATE booking SET bk_status = 'Billed' WHERE bk_id = @BkId";
        //            using (var updateCmd = new NpgsqlCommand(updateStatusQuery, conn))
        //            {
        //                updateCmd.Parameters.AddWithValue("@BkId", bk_id);
        //                int rowsAffected = updateCmd.ExecuteNonQuery();
        //                if (rowsAffected <= 0)
        //                {
        //                    throw new Exception("Failed to update booking status.");
        //                }
        //                ModalPopupExtender1.Hide();
        //                LoadBookingList();
        //                GeneratedBillList();
        //            }

        //            // Step 5: Insert bill details
        //            string insertBillQuery = @"
        //                                    INSERT INTO generate_bill (
        //                                        gb_note, gb_add_fees, gb_net_vat, gb_vat_amnt, gb_total_sales, 
        //                                        gb_date_issued, gb_date_due, gb_interest, gb_lead_days, 
        //                                        gb_accrual_period, gb_suspend_period, gb_accrual_date, 
        //                                        gb_suspend_date, gb_tax, gb_status, bk_id, emp_id
        //                                    ) 
        //                                    VALUES (
        //                                        @Note, @AddFees, @NetVat, @VatAmount, @TotalSales, 
        //                                        @DateIssued, @DateDue, @Interest, @LeadDays, 
        //                                        @AccrualPeriod, @SuspensionPeriod, @AccrualDate, 
        //                                        @SuspensionDate, @Tax, @Status, @BkId, @EmpId
        //                                    ) 
        //                                    RETURNING gb_id;";

        //            using (var cmd = new NpgsqlCommand(insertBillQuery, conn))
        //            {
        //                cmd.Parameters.AddWithValue("@Note", note ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@AddFees", additionalFees ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@NetVat", netVat ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@VatAmount", vatAmount);
        //                cmd.Parameters.AddWithValue("@TotalSales", totalSales);
        //                cmd.Parameters.AddWithValue("@DateIssued", dateIssued ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@DateDue", dateDue ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@Interest", interest ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@LeadDays", leadDays ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@AccrualPeriod", accrualPeriod ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@SuspensionPeriod", suspensionPeriod ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@AccrualDate", accrualDate ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@SuspensionDate", suspensionDate ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@Tax", tax ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@Status", "unpaid");
        //                cmd.Parameters.AddWithValue("@BkId", bk_id);
        //                cmd.Parameters.AddWithValue("@EmpId", empId);

        //                var result = cmd.ExecuteScalar();
        //                if (result != null)
        //                {
        //                    insertedBillId = Convert.ToInt32(result);
        //                    LoadBookingList();
        //                    GeneratedBillList();
        //                    gridViewBookings.DataBind();
        //                    this.ModalPopupExtender1.Hide();

        //                }
        //                else
        //                {
        //                    throw new Exception("Failed to insert bill.");
        //                }
        //            }

        //            string notif_message = "Your bill is now available for payment 🧾. \n\n" +
        //                               "------------------------------------------\n" +
        //                               "BILL# " + insertedBillId + "\n\n" +
        //                               "Dear " + cus_fullname + ",\n\n" +
        //                               "You can now review the details at your convenience. Please check it as soon as possible to avoid any delays. " +
        //                               "Thank you for your cooperation 💜";
        //            string queryNotif = @"
        //                                INSERT INTO notification (
        //                                    notif_message, notif_type, emp_id, cus_id, bk_id, gb_id
        //                                ) 
        //                                VALUES (
        //                                    @Message, 'billed', @EmpId, @CusId, @BkId, @GbId
        //                                ) 
        //                                RETURNING gb_id;";

        //            using (var cmd = new NpgsqlCommand(queryNotif, conn))
        //            {
        //                cmd.Parameters.AddWithValue("@Message", notif_message ?? (object)DBNull.Value);
        //                cmd.Parameters.AddWithValue("@EmpId", empId);
        //                cmd.Parameters.AddWithValue("@CusId", cus_id);
        //                cmd.Parameters.AddWithValue("@BkId", Convert.ToInt32(TextBox1.Text));
        //                cmd.Parameters.AddWithValue("@GbId", insertedBillId);

        //                var result = cmd.ExecuteScalar();
        //                if (result != null)
        //                {
        //                    insertedBillId = Convert.ToInt32(result);
        //                    updatePanel1.Update();
        //                    LoadBookingList();
        //                }
        //                else
        //                {
        //                    throw new Exception("No ID returned from insert.");
        //                }
        //            }
        //            GeneratedBillList();
        //            LoadBookingList();
        //        }
        //        GeneratedBillList();
        //        LoadBookingList();
        //        this.ModalPopupExtender1.Hide();
        //        //ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
        //        //                        "Swal.fire({ icon: 'error', title: 'Empty Total Units!', text: 'Total Units has not been entered yet', background: '#e9f7ef', confirmButtonColor: '#28a745' });",
        //        //                        true);

        //        if (isProcessSuccessful)
        //        {
        //            this.ModalPopupExtender1.Hide();
        //            ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
        //                                    "Swal.fire({ icon: 'error', title: 'Empty Total Units!', text: 'Total Units has not been entered yet', background: '#e9f7ef', confirmButtonColor: '#28a745' });",
        //                                    true);


        //            ScriptManager.RegisterStartupScript(this, GetType(), "DownloadPdf",
        //            "window.open('DownloadPdfHandler.ashx?billId=" + insertedBillId + "&bkId=" + bk_id + "', '_blank');", true);

        //            // Generate PDF if everything was successful
        //            //byte[] pdfBytes = GeneratePDFForRow(insertedBillId, bk_id);

        //            ////// Send the PDF for download. Ang problema kay naa dri maong di maclose and murefresh ang updatePanel
        //            //Response.Clear();
        //            //Response.ContentType = "application/pdf";
        //            //Response.AddHeader("content-disposition", $"attachment;filename=Bill_{insertedBillId}.pdf");
        //            //Response.Buffer = true;
        //            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //            //Response.BinaryWrite(pdfBytes);
        //            //Response.End();
        //            GeneratedBillList();
        //            LoadBookingList();
        //            gridViewBookings.DataBind();
        //            ScriptManager.RegisterStartupScript(this, GetType(), "showSuccessAlert",
        //"Swal.fire({ icon: 'success', title: 'Bill Generated Successfully!', text: 'The bill has been generated and is ready for download.', background: '#e9f7ef', confirmButtonColor: '#28a745' });",
        //true);
        //            this.ModalPopupExtender1.Hide();
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        LoadBookingList();
        //        this.ModalPopupExtender1.Hide();
        //        isProcessSuccessful = false;
        //        ScriptManager.RegisterStartupScript(this, GetType(), "errorAlert",
        //            $"Swal.fire({{ icon: 'error', title: 'Error!', text: '{ex.Message}', background: '#f8d7da', confirmButtonColor: '#dc3545' }});",
        //            true);
        //    }
        //    finally
        //    {
        //        LoadBookingList();
        //        this.ModalPopupExtender1.Hide(); // Ensure the modal always closes
        //        ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
        //                                "Swal.fire({ icon: 'error', title: 'Empty Total Units!', text: 'Total Units has not been entered yet', background: '#e9f7ef', confirmButtonColor: '#28a745' });",
        //                                true);
        //    }
        //    this.ModalPopupExtender1.Hide();
        //}

        protected void Unpaid_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int managerId = Convert.ToInt32(btn.CommandArgument);

            try
            {
                using (var db = new NpgsqlConnection(con))
                {
                    db.Open();

                    // Begin a transaction to ensure both updates happen together
                    using (var transaction = db.BeginTransaction())
                    {
                        try
                        {
                            // Update the gb_status in the generate_bill table
                            using (var cmd = db.CreateCommand())
                            {
                                cmd.CommandType = CommandType.Text;
                                cmd.CommandText = "UPDATE generate_bill SET gb_status = 'Unpaid' WHERE gb_id = @id";
                                cmd.Parameters.AddWithValue("@id", managerId);
                                cmd.Transaction = transaction;
                                cmd.ExecuteNonQuery();
                            }

                            // Update p_amount to zero in the payment table
                            using (var cmd = db.CreateCommand())
                            {
                                cmd.CommandType = CommandType.Text;
                                cmd.CommandText = "UPDATE payment SET p_amount = 0 WHERE gb_id = @id";
                                cmd.Parameters.AddWithValue("@id", managerId);
                                cmd.Transaction = transaction;
                                cmd.ExecuteNonQuery();
                            }

                            // Commit the transaction
                            transaction.Commit();

                            // Show success dialog with SweetAlert
                            ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
                                "Swal.fire({ icon: 'success', title: 'Marked Unpaid!', text: 'Bill marked unpaid successfully and payment reset!', background: '#e9f7ef', confirmButtonColor: '#28a745' });", true);

                            hfActiveTab.Value = "#tab2"; // Set the tab to display
                            GeneratedBillList(); // Refresh the list
                            LoadProfile(); // Reload profile data if needed
                        }
                        catch (Exception)
                        {
                            // Rollback in case of any error
                            transaction.Rollback();
                            throw; // Re-throw the exception to handle it outside
                        }
                    }

                    db.Close();
                }
            }
            catch (Exception ex)
            {
                hfActiveTab.Value = "#tab2"; // Set the tab to display

                // Show error dialog with SweetAlert
                ScriptManager.RegisterStartupScript(this, GetType(), "showError",
                    $"Swal.fire({{ icon: 'error', title: 'Unsuccessful!', text: '{ex.Message}', background: '#f8d7da', confirmButtonColor: '#dc3545' }});", true);
            }
        }


        protected void Paid_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                int managerId = Convert.ToInt32(btn.CommandArgument);

                // Set values for the modal popup and other controls
                hfActiveTab.Value = "#tab2";
                TextBox3.Text = managerId.ToString();
                hfManagerId.Value = managerId.ToString();

                // Hide the first modal and show the second one
                ModalPopupExtender1.Hide();
                ModalPopupExtender6.Show();

                // Database query to retrieve gb_total_sales
                using (NpgsqlConnection conn = new NpgsqlConnection(con))
                {
                    string query = @"
                SELECT gb_total_sales
                FROM generate_bill WHERE gb_id = @ManagerId";

                    NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ManagerId", managerId);

                    conn.Open();
                    object result = cmd.ExecuteScalar();

                    // Handle the result and assign to the textbox
                    if (result != null && result != DBNull.Value)
                    {
                        decimal totalSales = Convert.ToDecimal(result);
                        txtAmntPaid.Text = totalSales.ToString("N2"); // Format to 2 decimal places
                    }
                    else
                    {
                        txtAmntPaid.Text = "0.00"; // Default if no result is found
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle any errors
                Console.WriteLine("Error: " + ex.Message);
            }
        }


        protected void ChangeToPaid_Click(object sender, EventArgs e)
        {
            int empId = (int)Session["bo_id"]; // Retrieve admin ID from session

            try
            {
                int gbID = int.Parse(hfManagerId.Value); // Retrieve gb_id from hidden field
                double amntPaid = Convert.ToDouble(txtAmntPaid.Text); // Corrected conversion to double

                using (var db = new NpgsqlConnection(con))
                {
                    db.Open();

                    // Update generate_bill table to mark as paid
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        //cmd.CommandText = "UPDATE generate_bill SET gb_status = 'paid' WHERE gb_id = @id";
                        cmd.CommandText = "UPDATE generate_bill SET gb_status = 'Paid', gb_updated_at = CURRENT_TIMESTAMP WHERE gb_id = @id";
                        cmd.Parameters.AddWithValue("@id", gbID);

                        var ctr = cmd.ExecuteNonQuery();
                        //Response.Write("<script>alert('No '"+ amntPaid + "" + gbID + "' found for the specified ID.')</script>");

                        if (ctr >= 1)
                        {
                            //Response.Write("<script>alert('No '" + amntPaid + "" + gbID + "' found for the specified ID.')</script>");

                            // Insert a new record into the payment table
                            using (var paymentCmd = db.CreateCommand())
                            {
                                paymentCmd.CommandType = CommandType.Text;
                                paymentCmd.CommandText = @"
                        INSERT INTO payment (p_amount, p_status, p_method, gb_id, p_date_paid)
                        VALUES (@amount, 'paid', 'cash', @gbId, CURRENT_TIMESTAMP)";

                                paymentCmd.Parameters.AddWithValue("@amount", amntPaid);
                                paymentCmd.Parameters.AddWithValue("@gbId", gbID);
                                //paymentCmd.Parameters.AddWithValue("@emp_id", empId);


                                paymentCmd.ExecuteNonQuery();
                                GeneratedBillList(); // Refresh the list

                                ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
    "Swal.fire({ icon: 'success', title: 'Updated to Paid', text: 'Bill has been changed to paid successfully!', background: '#e9f7ef', confirmButtonColor: '#28a745' });", true);
                            }
                            hfActiveTab.Value = "#tab2"; // Set Tab 1 as the default
                            ModalPopupExtender6.Hide();
                            ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
    "Swal.fire({ icon: 'success', title: 'Updated to Paid', text: 'Bill has been changed to paid successfully!', background: '#e9f7ef', confirmButtonColor: '#28a745' });", true);
                            GeneratedBillList(); // Refresh the list
                            LoadProfile();
                        }
                        else
                        {
                            // Display failure message with SweetAlert
                            ScriptManager.RegisterStartupScript(this, GetType(), "showFailure",
                                "Swal.fire({ icon: 'error', title: 'Update Failed', text: 'Failed to update status', background: '#f8d7da', confirmButtonColor: '#dc3545' });", true);
                        }
                    }
                    GeneratedBillList();
                }
            }
            catch (Exception ex)
            {
                // Display exception message with SweetAlert
                ScriptManager.RegisterStartupScript(this, GetType(), "showError",
                    $"Swal.fire({{ icon: 'error', title: 'Error!', text: '{ex.Message}', background: '#f8d7da', confirmButtonColor: '#dc3545' }});", true);
                GeneratedBillList();
            }
        }


        // validate if the admin status is Suspend
        protected Boolean IsSuspended(string status)
        {
            return status == "Unpaid";
        }

        // validate if the admin status is Unsuspend
        protected Boolean IsActive(string status)
        {
            return status == "Paid";
        }

    }
}