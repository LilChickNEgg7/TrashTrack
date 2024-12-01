using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;


using System.Web.UI.WebControls;
using iText.Kernel.Pdf;
using ITextDocument = iText.Layout.Document;

using System.Reflection.Emit;
using AjaxControlToolkit;
using static Capstone.PaymentController;

namespace Capstone
{
    public partial class AM_Reports : System.Web.UI.Page
    {
        private readonly string con = "Server=localhost;Port=5432;User Id=postgres;Password=123456;Database=trashtrack";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadProfile();
                HistoryBookingList();
                HistoryBillList();
                //HistoryPaymentList();
                //PopulateWasteCategory();
                //LoadBookingWasteData();
                hfActiveTab.Value = "#tab1"; // Set Tab 1 as the default
                BindNotifications();
                GetUnreadNotificationCount();
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
                if (Session["am_id"] == null)
                {
                    // Session expired or not set, redirect to login
                    Response.Redirect("Login.aspx");
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
        protected void HistoryBookingList()
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
                    ORDER BY 
                        b.bk_date DESC, b.bk_id DESC";

                    //    cmd.CommandText = @"
                    //SELECT bk_id, bk_date, bk_status, bk_province, bk_city, bk_brgy, bk_street, bk_postal
                    //FROM booking WHERE bk_status != 'Completed'
                    //ORDER BY bk_id, bk_date";  // Sorting by date for recent bookings

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
            HistoryBookingList();

            int insertedBillId = gb_id;
            byte[] pdfBytes = GeneratePDFViewBill(insertedBillId, bk_id);
            HistoryBookingList();

            // Set up PDF response and initiate download
            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", $"attachment;filename=Bill_{insertedBillId}.pdf");
            Response.Buffer = true;
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.BinaryWrite(pdfBytes);
            HistoryBookingList();
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
                                dueDate = Convert.ToDateTime(billReader["gb_date_due"]);



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


                    //iText.Layout.Element.Table infoTable = new iText.Layout.Element.Table(3).UseAllAvailableWidth();

                    //infoTable.SetBorder(Border.NO_BORDER);

                    //// Bill ID cell
                    //infoTable.AddCell(new Cell()
                    //    .SetBorder(Border.NO_BORDER)
                    //    .Add(new Paragraph($"Bill ID: {buttonText}")
                    //        .SetFont(boldFont)
                    //        .SetBorder(Border.NO_BORDER)));

                    //// Booking ID cell
                    //infoTable.AddCell(new Cell()
                    //    .SetBorder(Border.NO_BORDER)
                    //    .Add(new Paragraph($"")
                    //        .SetFont(boldFont)
                    //        .SetBorder(Border.NO_BORDER)));
                    //// Booking ID cell
                    //infoTable.AddCell(new Cell()
                    //    .SetBorder(Border.NO_BORDER)
                    //    .Add(new Paragraph($"Booking ID: {bkID}")
                    //        .SetFont(boldFont)
                    //        .SetBorder(Border.NO_BORDER)));

                    //// Booking ID cell
                    //infoTable.AddCell(new Cell()
                    //    .SetBorder(Border.NO_BORDER)
                    //    .Add(new Paragraph($"")
                    //        .SetFont(boldFont)
                    //        .SetBorder(Border.NO_BORDER)));

                    //// Date Issued cell, aligned to the right
                    //infoTable.AddCell(new Cell()
                    //    .SetBorder(Border.NO_BORDER)
                    //    .Add(new Paragraph($"Date Issued: {(dateIssued?.ToString("MM/dd/yyyy") ?? "N/A")}")
                    //        .SetFont(boldFont)
                    //        .SetTextAlignment(TextAlignment.RIGHT)
                    //        .SetBorder(Border.NO_BORDER)));

                    //// Empty cell for spacing
                    //infoTable.AddCell(new Cell()
                    //    .SetBorder(Border.NO_BORDER)
                    //    .Add(new Paragraph("").SetBorder(Border.NO_BORDER)));

                    //// Due Date cell, aligned to the right
                    //infoTable.AddCell(new Cell()
                    //    .SetBorder(Border.NO_BORDER)
                    //    .Add(new Paragraph($"Due Date: {(dueDate?.ToString("MM/dd/yyyy") ?? "N/A")}")
                    //        .SetFont(boldFont)
                    //        .SetTextAlignment(TextAlignment.RIGHT)
                    //        .SetBorder(Border.NO_BORDER)));

                    //// Add the table to the document
                    //document.Add(infoTable);
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
        protected void btnViewSlip_Click(object sender, EventArgs e)
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
        protected void btnOtherAction_Click(object sender, EventArgs e)
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

        protected void HistoryBillList()
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
                                            COALESCE(p.p_date_paid, NULL) AS p_date_paid, 
                                            COALESCE(p.p_checkout_id, 'N/A') AS p_checkout_id,
                                            COALESCE(p.p_trans_id, 'N/A') AS p_trans_id
                                        FROM 
                                            generate_bill gb
                                        LEFT JOIN 
                                            payment p ON gb.gb_id = p.gb_id
                                        ORDER BY 
                                            gb.gb_date_issued DESC,
                                            gb.gb_id DESC;
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


        //protected void HistoryPaymentList()
        //{
        //    using (var db = new NpgsqlConnection(con))
        //    {
        //        db.Open();
        //        using (var cmd = db.CreateCommand())
        //        {
        //            cmd.CommandType = CommandType.Text;
        //            // Query to fetch booking data from the database
        //            cmd.CommandText = @"SELECT * FROM payment WHERE p_status = 'paid'";  // Sorting by date for recent bookings
        //            // Execute the query and bind the results to the GridView
        //            DataTable bookingsDataTable = new DataTable();
        //            NpgsqlDataAdapter bookingsAdapter = new NpgsqlDataAdapter(cmd);
        //            bookingsAdapter.Fill(bookingsDataTable);

        //            // Bind the data to the GridView
        //            gridView2.DataSource = bookingsDataTable;
        //            gridView2.DataBind();
        //        }
        //        db.Close();
        //    }
        //}


    }
}