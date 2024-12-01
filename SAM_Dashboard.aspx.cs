using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using Npgsql;
using System.Web.UI.WebControls;
using ITextDocument = iText.Layout.Document;
using static Capstone.PaymentController;
using System.Web.Http; // Make sure to add a reference to System.Web.Http

namespace Capstone
{
    public partial class Account_Manager_Dashboard : System.Web.UI.Page
    {
        // Database Connection String
        private readonly string con = "Server=localhost;Port=5432;User Id=postgres;Password=123456;Database=trashtrack";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadProfile();
                CustomerCount();
                SAMCount();
                AMCount();
                BOCount();
                ODCount();
                HaulerCount();
                PaymentStatus();
                BindNotifications();
                GetUnreadNotificationCount();
                //CalculateMonthlyPayments();
                //TotalSalesPaidMonthly();
                //TotalSalesUnpaidMonthly();
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

        protected void CustomerCount()
        {
            using (var db = new NpgsqlConnection(con))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT COUNT(*) FROM CUSTOMER";
                    //cmd.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Integer, Convert.ToInt32(Session["sam_id"]));

                    // Execute the command and read the data
                    int totalCustomerCount = Convert.ToInt32(cmd.ExecuteScalar());

                    // For total customers, you might want a different query
                    cmd.CommandText = "SELECT COUNT(*) FROM CUSTOMER WHERE CUS_STATUS = 'Active'";
                    int activeCustomerCount = Convert.ToInt32(cmd.ExecuteScalar());

                    cmd.CommandText = "SELECT COUNT(*) FROM CUSTOMER WHERE CUS_STATUS = 'Suspended'";
                    int suspCustomerCount = Convert.ToInt32(cmd.ExecuteScalar());



                    // Bind to labels
                    totalcustomer.Text = totalCustomerCount.ToString();
                    activecustomer.Text = activeCustomerCount.ToString();
                    suspcustomer.Text = suspCustomerCount.ToString();
                }
            }
        }

        protected void AMCount()
        {
            using (var db = new NpgsqlConnection(con))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT COUNT(*) FROM EMPLOYEE WHERE ROLE_ID = 1";
                    //cmd.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Integer, Convert.ToInt32(Session["sam_id"]));

                    // Execute the command and read the data
                    int totalAccountManager = Convert.ToInt32(cmd.ExecuteScalar());

                    // For total customers, you might want a different query
                    cmd.CommandText = "SELECT COUNT(*) FROM EMPLOYEE WHERE EMP_STATUS = 'Active' AND ROLE_ID = 1";
                    int activeAccountManager = Convert.ToInt32(cmd.ExecuteScalar());
                    // For total customers, you might want a different query
                    cmd.CommandText = "SELECT COUNT(*) FROM EMPLOYEE WHERE EMP_STATUS = 'Suspended' AND ROLE_ID = 1";
                    int suspAccountManager = Convert.ToInt32(cmd.ExecuteScalar());

                    // Bind to labels
                    totalAM.Text = totalAccountManager.ToString();
                    activeAM.Text = activeAccountManager.ToString();
                    suspAM.Text = suspAccountManager.ToString();
                }
            }
        }

        protected void SAMCount()
        {
            using (var db = new NpgsqlConnection(con))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT COUNT(*) FROM EMPLOYEE WHERE ROLE_ID = 2";
                    //cmd.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Integer, Convert.ToInt32(Session["sam_id"]));

                    // Execute the command and read the data
                    int totalSAMcount = Convert.ToInt32(cmd.ExecuteScalar());

                    // For total customers, you might want a different query
                    cmd.CommandText = "SELECT COUNT(*) FROM EMPLOYEE WHERE EMP_STATUS = 'Active' AND ROLE_ID = 2";
                    int activeSAMcount = Convert.ToInt32(cmd.ExecuteScalar());
                    // For total customers, you might want a different query
                    cmd.CommandText = "SELECT COUNT(*) FROM EMPLOYEE WHERE EMP_STATUS = 'Suspended' AND ROLE_ID = 2";
                    int suspSAMcount = Convert.ToInt32(cmd.ExecuteScalar());

                    // Bind to labels
                    totalSAM.Text = totalSAMcount.ToString();
                    activeSAM.Text = activeSAMcount.ToString();
                    suspSAM.Text = suspSAMcount.ToString();
                }
            }
        }

        protected void HaulerCount()
        {
            using (var db = new NpgsqlConnection(con))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT COUNT(*) FROM EMPLOYEE WHERE ROLE_ID = 4";
                    //cmd.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Integer, Convert.ToInt32(Session["sam_id"]));

                    // Execute the command and read the data
                    int totalBOcount = Convert.ToInt32(cmd.ExecuteScalar());

                    // For total customers, you might want a different query
                    cmd.CommandText = "SELECT COUNT(*) FROM EMPLOYEE WHERE EMP_STATUS = 'Active' AND ROLE_ID = 4";
                    int activeBOcount = Convert.ToInt32(cmd.ExecuteScalar());
                    // For total customers, you might want a different query
                    cmd.CommandText = "SELECT COUNT(*) FROM EMPLOYEE WHERE EMP_STATUS = 'Suspended' AND ROLE_ID = 4";
                    int suspBOcount = Convert.ToInt32(cmd.ExecuteScalar());

                    // Bind to labels
                    totalHauler.Text = totalBOcount.ToString();
                    activeHauler.Text = activeBOcount.ToString();
                    suspHauler.Text = suspBOcount.ToString();
                }
            }
        }

        protected void BOCount()
        {
            using (var db = new NpgsqlConnection(con))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT COUNT(*) FROM EMPLOYEE WHERE ROLE_ID = 3";
                    //cmd.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Integer, Convert.ToInt32(Session["sam_id"]));

                    // Execute the command and read the data
                    int totalBOcount = Convert.ToInt32(cmd.ExecuteScalar());

                    // For total customers, you might want a different query
                    cmd.CommandText = "SELECT COUNT(*) FROM EMPLOYEE WHERE EMP_STATUS = 'Active' AND ROLE_ID = 3";
                    int activeBOcount = Convert.ToInt32(cmd.ExecuteScalar());
                    // For total customers, you might want a different query
                    cmd.CommandText = "SELECT COUNT(*) FROM EMPLOYEE WHERE EMP_STATUS = 'Suspended' AND ROLE_ID = 3";
                    int suspBOcount = Convert.ToInt32(cmd.ExecuteScalar());

                    // Bind to labels
                    totalBO.Text = totalBOcount.ToString();
                    activeBO.Text = activeBOcount.ToString();
                    suspBO.Text = suspBOcount.ToString();
                }
            }
        }
        protected void ODCount()
        {
            using (var db = new NpgsqlConnection(con))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT COUNT(*) FROM EMPLOYEE WHERE ROLE_ID = 5";
                    //cmd.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Integer, Convert.ToInt32(Session["sam_id"]));

                    // Execute the command and read the data
                    int totalODcount = Convert.ToInt32(cmd.ExecuteScalar());

                    // For total customers, you might want a different query
                    cmd.CommandText = "SELECT COUNT(*) FROM EMPLOYEE WHERE EMP_STATUS = 'Active' AND ROLE_ID = 5";
                    int activeODcount = Convert.ToInt32(cmd.ExecuteScalar());
                    // For total customers, you might want a different query
                    cmd.CommandText = "SELECT COUNT(*) FROM EMPLOYEE WHERE EMP_STATUS = 'Suspended' AND ROLE_ID = 5";
                    int suspODcount = Convert.ToInt32(cmd.ExecuteScalar());

                    // Bind to labels
                    totalOD.Text = totalODcount.ToString();
                    activeOD.Text = activeODcount.ToString();
                    suspOD.Text = suspODcount.ToString();
                }
            }
        }

        protected void PaymentStatus()
        {
            using (var db = new NpgsqlConnection(con))
            {
                db.Open();
                int paidCount, unpaidCount;

                // Retrieve count for paid payments
                using (var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM payment WHERE p_status = 'paid'", db))
                {
                    paidCount = Convert.ToInt32(cmd.ExecuteScalar());
                }

                // Retrieve count for unpaid payments
                using (var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM payment WHERE p_status = 'unpaid'", db))
                {
                    unpaidCount = Convert.ToInt32(cmd.ExecuteScalar());
                }

                // Register JavaScript to update the pie chart on the client side
                string script = $"updatePieChart({paidCount}, {unpaidCount});";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "updatePieChart", script, true);
            }
        }


        //private void TotalSalesPaidMonthly()
        //{
        //    decimal[] monthlyTotalSalesPaid = new decimal[12]; // Array to hold totals for each month

        //    using (var conn = new NpgsqlConnection(con))
        //    {
        //        conn.Open();

        //        // Loop through each month
        //        for (int month = 0; month < 12; month++)
        //        {
        //            DateTime startDate = new DateTime(2024, month + 1, 1); // Month is 0-based
        //            DateTime endDate = startDate.AddMonths(1); // End date is the first of the next month

        //            monthlyTotalSalesPaid[month] = GetTotalSales(conn, startDate, endDate, "paid");
        //        }

        //        // Pass the data to the client-side script
        //        ClientScript.RegisterArrayDeclaration("monthlyTotalSalesPaid",
        //            string.Join(",", monthlyTotalSalesPaid.Select(s => s.ToString())));
        //    }
        //}
        //private void TotalSalesUnpaidMonthly()
        //{
        //    decimal[] monthlyTotalSalesUnpaid = new decimal[12]; // Array to hold totals for each month

        //    using (var conn = new NpgsqlConnection(con))
        //    {
        //        conn.Open();

        //        // Loop through each month
        //        for (int month = 0; month < 12; month++)
        //        {
        //            DateTime startDate = new DateTime(2024, month + 1, 1); // Month is 0-based
        //            DateTime endDate = startDate.AddMonths(1); // End date is the first of the next month

        //            monthlyTotalSalesUnpaid[month] = GetTotalSales(conn, startDate, endDate, "unpaid");
        //        }

        //        // Pass the data to the client-side script
        //        ClientScript.RegisterArrayDeclaration("monthlyTotalSalesUnpaid",
        //            string.Join(",", monthlyTotalSalesUnpaid.Select(s => s.ToString())));
        //    }
        //}
        //private decimal GetTotalSales(NpgsqlConnection conn, DateTime startDate, DateTime endDate, string billStatus)
        //{
        //    decimal totalBill = 0.00m;
        //    string query = "SELECT SUM(p_amount) FROM public.payment WHERE p_updated_at >= @startDate AND p_updated_at < @endDate AND p_status = @billStatus";

        //    using (var cmd = new NpgsqlCommand(query, conn))
        //    {
        //        cmd.Parameters.AddWithValue("startDate", startDate);
        //        cmd.Parameters.AddWithValue("endDate", endDate);
        //        cmd.Parameters.AddWithValue("billStatus", billStatus);

        //        var result = cmd.ExecuteScalar();

        //        if (result != DBNull.Value)
        //        {
        //            totalBill = Convert.ToDecimal(result);
        //        }
        //    }

        //    return totalBill;
        //}


        //private void TotalSalesPaidMonthly()
        //{
        //    decimal[] monthlyTotalSalesPaid = new decimal[12]; // Array to hold totals for each month

        //    using (var conn = new NpgsqlConnection(con))
        //    {
        //        conn.Open();

        //        // Get the current date
        //        DateTime currentDate = DateTime.Now;

        //        // Loop through each month
        //        for (int month = 0; month < 12; month++)
        //        {
        //            // Start date for the month
        //            DateTime startDate = new DateTime(currentDate.Year, month + 1, 1);
        //            // End date for the next month
        //            DateTime endDate = startDate.AddMonths(1);

        //            // Sum payments that were either paid in this month or were unpaid but are still open
        //            monthlyTotalSalesPaid[month] = GetTotalSales(conn, startDate, endDate, "paid");
        //            monthlyTotalSalesPaid[month] += GetTotalSales(conn, startDate, endDate, "unpaid", currentDate);
        //        }

        //        // Pass the data to the client-side script
        //        ClientScript.RegisterArrayDeclaration("monthlyTotalSalesPaid",
        //            string.Join(",", monthlyTotalSalesPaid.Select(s => s.ToString())));
        //    }
        //}

        //private void TotalSalesUnpaidMonthly()
        //{
        //    decimal[] monthlyTotalSalesUnpaid = new decimal[12]; // Array to hold totals for each month

        //    using (var conn = new NpgsqlConnection(con))
        //    {
        //        conn.Open();

        //        // Get the current date
        //        DateTime currentDate = DateTime.Now;

        //        // Loop through each month
        //        for (int month = 0; month < 12; month++)
        //        {
        //            // Start date for the month
        //            DateTime startDate = new DateTime(currentDate.Year, month + 1, 1);
        //            // End date for the next month
        //            DateTime endDate = startDate.AddMonths(1);

        //            // Get the total unpaid payments for this month
        //            monthlyTotalSalesUnpaid[month] = GetTotalSales(conn, startDate, endDate, "unpaid", currentDate);
        //        }

        //        // Pass the data to the client-side script
        //        ClientScript.RegisterArrayDeclaration("monthlyTotalSalesUnpaid",
        //            string.Join(",", monthlyTotalSalesUnpaid.Select(s => s.ToString())));
        //    }
        //}

        //private decimal GetTotalSales(NpgsqlConnection conn, DateTime startDate, DateTime endDate, string billStatus, DateTime? currentDate = null)
        //{
        //    decimal totalBill = 0.00m;
        //    string query;

        //    if (billStatus == "unpaid" && currentDate.HasValue)
        //    {
        //        // For unpaid payments, consider those created before the current month and unpaid till now
        //        query = @"SELECT SUM(p_amount) 
        //          FROM public.payment 
        //          WHERE p_created_at < @currentDate 
        //            AND p_status = 'unpaid' 
        //            AND p_updated_at < @endDate";
        //    }
        //    else
        //    {
        //        query = "SELECT SUM(p_amount) FROM public.payment WHERE p_updated_at >= @startDate AND p_updated_at < @endDate AND p_status = @billStatus";
        //    }

        //    using (var cmd = new NpgsqlCommand(query, conn))
        //    {
        //        cmd.Parameters.AddWithValue("startDate", startDate);
        //        cmd.Parameters.AddWithValue("endDate", endDate);
        //        if (currentDate.HasValue)
        //        {
        //            cmd.Parameters.AddWithValue("currentDate", currentDate.Value);
        //        }
        //        cmd.Parameters.AddWithValue("billStatus", billStatus);

        //        var result = cmd.ExecuteScalar();

        //        if (result != DBNull.Value)
        //        {
        //            totalBill = Convert.ToDecimal(result);
        //        }
        //    }

        //    return totalBill;
        //}



        private decimal GetUnpaidPayments(NpgsqlConnection conn, DateTime currentDate)
        {
            decimal totalUnpaidAmount = 0.00m;

            // Query to get unpaid payments that are still open
            string query = @"
        SELECT gb_id, gb_total_sales, gb_interest, gb_date_due, gb_accrual_period, 
               gb_suspend_period, gb_accrual_date, gb_suspend_date 
        FROM public.generate_bill 
        WHERE gb_status = 'unpaid' AND gb_date_due <= @currentDate";

            using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("currentDate", currentDate);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        decimal totalSales = reader.GetDecimal(reader.GetOrdinal("gb_total_sales"));
                        decimal interestRate = reader.GetDecimal(reader.GetOrdinal("gb_interest"));
                        DateTime dueDate = reader.GetDateTime(reader.GetOrdinal("gb_date_due"));
                        int accrualPeriod = reader.GetInt32(reader.GetOrdinal("gb_accrual_period"));
                        int suspendPeriod = reader.GetInt32(reader.GetOrdinal("gb_suspend_period"));
                        DateTime accrualDate = reader.GetDateTime(reader.GetOrdinal("gb_accrual_date"));
                        DateTime? suspendDate = reader.IsDBNull(reader.GetOrdinal("gb_suspend_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("gb_suspend_date"));

                        // Apply interest if current date is past the due date
                        if (currentDate > dueDate)
                        {
                            decimal interestAmount = (totalSales * interestRate / 100);
                            totalSales += interestAmount;
                        }

                        // Accrue interest for each accrual period
                        while (currentDate >= accrualDate && (suspendDate == null || currentDate <= suspendDate))
                        {
                            decimal interestAmount = (totalSales * interestRate / 100);
                            totalSales += interestAmount;

                            accrualDate = accrualDate.AddDays(accrualPeriod); // Move to next accrual date
                        }

                        // Add total amount for this unpaid payment to the total
                        totalUnpaidAmount += totalSales;
                    }
                }
            }

            return totalUnpaidAmount;
        }
        private decimal GetPaidPayments(NpgsqlConnection conn, DateTime startDate, DateTime endDate)
        {
            decimal totalPaidAmount = 0.00m;

            string query = @"
        SELECT SUM(p_amount) 
        FROM public.payment 
        WHERE p_status = 'paid' 
        AND p_created_at >= @startDate 
        AND p_created_at < @endDate";

            using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("startDate", startDate);
                cmd.Parameters.AddWithValue("endDate", endDate);

                var result = cmd.ExecuteScalar();

                if (result != DBNull.Value)
                {
                    totalPaidAmount = Convert.ToDecimal(result);
                }
            }

            return totalPaidAmount;
        }
        private void CalculateMonthlyPayments()
        {
            decimal[] monthlyPaidPayments = new decimal[12];  // Store monthly paid totals
            decimal[] monthlyUnpaidPayments = new decimal[12];  // Store monthly unpaid totals

            DateTime currentDate = DateTime.Now; // Set the current date

            // Database connection string
            string connString = "Server=localhost;Port=5432;User Id=postgres;Password=123456;Database=trashtrack";

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open(); // Open the connection

                // Loop through each month of the current year
                for (int month = 0; month < 12; month++)
                {
                    DateTime startDate = new DateTime(currentDate.Year, month + 1, 1);
                    DateTime endDate = startDate.AddMonths(1);

                    // Get paid payments for the month
                    monthlyPaidPayments[month] = GetPaidPayments(conn, startDate, endDate);

                    // Get unpaid payments for the month
                    monthlyUnpaidPayments[month] = GetUnpaidPayments(conn, currentDate);
                }

                // Pass the data to the client-side script (for updating the chart)
                ClientScript.RegisterArrayDeclaration("monthlyPaidPayments",
                    string.Join(",", monthlyPaidPayments.Select(s => s.ToString())));

                ClientScript.RegisterArrayDeclaration("monthlyUnpaidPayments",
                    string.Join(",", monthlyUnpaidPayments.Select(s => s.ToString())));
            }
        }


        

    }
}
