using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
//using System.Web.UI.WebControls;

using WebControls = System.Web.UI.WebControls;
using MongoDB.Driver.Linq;
using System.Data.SqlClient;
using Amazon.SecurityToken.Model;
//using AjaxControlToolkit.HtmlEditor.ToolbarButtons;
using Amazon.Runtime.Documents;
using Npgsql.Internal;
using System.IO;


using System.Web.UI.WebControls;
using iText.Kernel.Pdf;
using ITextDocument = iText.Layout.Document;
using iText.Layout.Element;
using iText.Layout.Properties;


using iText.Kernel.Colors;
using iText.IO.Image;


using iText.Kernel.Colors;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Layout.Borders;
using AjaxControlToolkit;


namespace Capstone
{
    public partial class BO_Billing : System.Web.UI.Page
    {
        private readonly string con = "Server=localhost;Port=5432;User Id=postgres;Password=123456;Database=trashtrack";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadProfile();
                LoadBookingList();
                GeneratedBillList();
                PopulateWasteCategory();
                LoadBookingWasteData();
                hfActiveTab.Value = "#tab1"; // Set Tab 1 as the default

            }
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

        protected void LoadBookingList()
        {
            using (var db = new NpgsqlConnection(con))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    // Query to fetch booking data from the database
                    cmd.CommandText = @"
                SELECT bk_id, bk_date, bk_status, bk_province, bk_city, bk_brgy, bk_street, bk_postal
                FROM booking WHERE bk_status NOT IN ('Completed', 'Billed') ORDER BY bk_id, bk_date";
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
                    // Query to fetch booking data from the database
                    cmd.CommandText = @"SELECT * FROM generate_bill ORDER BY gb_status DESC, gb_id";  // Sorting by date for recent bookings
                    // Execute the query and bind the results to the GridView
                    DataTable bookingsDataTable = new DataTable();
                    NpgsqlDataAdapter bookingsAdapter = new NpgsqlDataAdapter(cmd);
                    bookingsAdapter.Fill(bookingsDataTable);

                    // Bind the data to the GridView
                    gridView1.DataSource = bookingsDataTable;
                    gridView1.DataBind();
                }
                db.Close();
            }
        }



        // Deletion of the admin or update the status to Inactive if the admin is inactive anymore
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
                        }
                        else
                        {
                            // Handle case where no booking ID is found
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
                                "swal('Not Found!', 'No associated booking ID found for this Booking Waste record.', 'warning')", true);
                            return; // Exit if no booking ID found
                        }
                    }

                    // Now proceed to delete the record
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;

                        // Execute a DELETE statement to remove the record
                        cmd.CommandText = "DELETE FROM BOOKING_WASTE WHERE bw_id = @id";
                        cmd.Parameters.AddWithValue("@id", bwId);

                        var ctr = cmd.ExecuteNonQuery();
                        if (ctr >= 1)
                        {
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
                updatePanel.Update(); // If using UpdatePanel, force an update to reflect changes

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

            // Refresh data in grid or UI here (if applicable)
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
            // Fetch values from the textboxes
            int bk_id = Convert.ToInt32(TextBox1.Text); // Booking ID
            string bw_name = ddlbwName1.SelectedItem.Text; // Waste type name from the dropdown (Text, not value)
            string bw_unit = txtbwUnit1.Text; // Waste unit from the textbox
            double bw_total_unit = Convert.ToDouble(txtTotalUnit1.Text); // Total unit from the textbox
            double bw_price = Convert.ToDouble(txtUnitPrice1.Text); // Price per unit from the textbox
            double bw_total_price = Convert.ToDouble(txtTotalUnitPrice1.Text); // Total price from the textbox (already computed in UI)

            try
            {
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
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
                                "swal('Not Found!', 'No associated booking ID found for this Booking Waste.', 'warning')", true);
                            return; // Exit if no booking ID found
                        }
                    }

                    // Insert the new booking waste record
                    string insertBookingWasteQuery = @"INSERT INTO booking_waste 
                                               (bk_id, bw_name, bw_unit, bw_total_unit, bw_price, bw_total_price)
                                               VALUES 
                                               (@bk_id, @bw_name, @bw_unit, @bw_total_unit, @bw_price, @bw_total_price)";

                    using (var cmd = new NpgsqlCommand(insertBookingWasteQuery, db))
                    {
                        cmd.Parameters.AddWithValue("@bk_id", bk_id);
                        cmd.Parameters.AddWithValue("@bw_name", bw_name);
                        cmd.Parameters.AddWithValue("@bw_unit", bw_unit);
                        cmd.Parameters.AddWithValue("@bw_total_unit", bw_total_unit);
                        cmd.Parameters.AddWithValue("@bw_price", bw_price);
                        cmd.Parameters.AddWithValue("@bw_total_price", bw_total_price);
                        var ctr = cmd.ExecuteNonQuery();
                        if (ctr >= 1)
                        {
                            LoadBookingWasteData();
                            updatePanel1.Update(); // Update the UpdatePanel to reflect changes
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
                                "swal('Record Deleted!', 'Booking Waste record deleted successfully!', 'success')", true);

                            // Refresh the data for the associated booking ID
                            /*LoadBookingWasteData(bk_id);*/ // Call the method to refresh the data
                        }
                        else
                        {
                            LoadBookingWasteData();
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
                                "swal('Not Found!', 'No record found to delete.', 'warning')", true);
                        }
                    }
                }

                // Reload data after insertion
                LoadBookingWasteData();

                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
                    "swal('Success!', 'Booking Waste record added successfully!', 'success')", true);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
                    $"swal('Unsuccessful!', '{ex.Message}', 'error')", true);
            }
            finally
            {
                LoadBookingWasteData();
                // Close the modal
                this.ModalPopupExtender3.Hide();

                //this.ModalPopupExtender1.Show();

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
                    double ptLeadDays = 0;
                    int accrualPeriod = 0, suspPeriod = 0, ptInterest = 0;
                    using (var cmdTax = db.CreateCommand())
                    {
                        cmdTax.CommandType = CommandType.Text;
                        cmdTax.CommandText = @"SELECT pt_tax, pt_lead_days, pt_accrual_period, pt_susp_period, pt_interest
                                       FROM payment_term 
                                       LIMIT 1"; // Assuming only one payment term is needed

                        using (var reader = cmdTax.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                ptTax = reader.IsDBNull(0) ? 0 : reader.GetDouble(0);  // Tax percentage
                                ptLeadDays = reader.IsDBNull(1) ? 0 : reader.GetDouble(1);  // Lead days
                                accrualPeriod = reader.IsDBNull(2) || reader.GetInt32(2) == 0 ? 0 : reader.GetInt32(2);  // Accrual period (check for NULL or zero)
                                suspPeriod = reader.IsDBNull(3) || reader.GetInt32(3) == 0 ? 0 : reader.GetInt32(3);  // Suspension period (check for NULL or zero)
                                ptInterest = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);  // Interest

                                // Display payment term details in respective labels
                                taxLabel.Text = "Tax: " + ptTax + "%"; // Displaying tax percentage
                                interstLabel.Text = "Interest: " + ptInterest + "%"; // Displaying interest
                                accrPerLabel.Text = "Accrual Period: " + accrualPeriod + " day(s)"; // Displaying accrual period
                                susPerLabel.Text = "Suspension Period: " + suspPeriod + " day(s)"; // Displaying suspension period
                            }
                        }
                    }

                    // Store hidden fields
                    ptLeadDaysHiddenField.Value = ptLeadDays.ToString();
                    ptAccrualPeriodHiddenField.Value = accrualPeriod.ToString();
                    ptSuspPeriodHiddenField.Value = suspPeriod.ToString();

                    // Calculate Due Date based on current date + lead days
                    DateTime currentDate = DateTime.Now;
                    DateTime dueDate = currentDate.AddDays(ptLeadDays);
                    dueDateTxt.Text = dueDate.ToString("yyyy-MM-ddTHH:mm"); // Set the Due Date in the textbox

                    // Handle Accrual Date
                    if (accrualPeriod > 0)
                    {
                        DateTime accrualDate = dueDate.AddDays(accrualPeriod);
                        accDateTxt.Text = accrualDate.ToString("yyyy-MM-ddTHH:mm"); // Set the Accrual Date in the textbox
                    }
                    else
                    {
                        accDateTxt.Text = "No accrual date"; // Display "No accrual date" if accrual period is zero or not found
                    }

                    // Handle Suspension Date
                    if (suspPeriod > 0)
                    {
                        DateTime suspensionDate = dueDate.AddDays(suspPeriod);
                        susDateTxt.Text = suspensionDate.ToString("yyyy-MM-ddTHH:mm"); // Set the Suspension Date in the textbox
                    }
                    else
                    {
                        susDateTxt.Text = "No suspension date"; // Display "No suspension date" if suspension period is zero or not found
                    }

                    // Calculate tax amount based on total price
                    double taxAmount = (ptTax / 100) * totalPrice;
                    vatAmntTxt.Text = taxAmount.ToString("N2");

                    // Calculate total sales (total price + tax amount)
                    double totalSales = totalPrice + taxAmount;
                    totSalesTxt.Text = totalSales.ToString("N2");

                    // Set the current date and time in the required format for DateTimeLocal
                    dateTodayTxt.Text = currentDate.ToString("yyyy-MM-ddTHH:mm");
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
                    $"swal('Unsuccessful!', '{ex.Message}', 'error')", true);
            }
        }

        protected void openBookWaste_Click(object sender, EventArgs e)
        {
            LinkButton btn = sender as LinkButton;
            int id = Convert.ToInt32(btn.CommandArgument);
            txtbwID1.Text = id.ToString();
            TextBox1.Text = id.ToString();
            LoadBookingWasteData(); // Load the relevant data for the booking ID
            this.ModalPopupExtender1.Show();
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


        //protected void openBookWaste_Click(object sender, EventArgs e)
        //{
        //    LinkButton btn = sender as LinkButton;
        //    int id = Convert.ToInt32(btn.CommandArgument);

        //    txtbwID1.Text = id.ToString();
        //    TextBox1.Text = id.ToString();
        //    LoadBookingWasteData(id);


        //    //txtbwID.Text = id.ToString();
        //    try
        //    {
        //        using (var db = new NpgsqlConnection(con))
        //        {
        //            db.Open();

        //            //// First query to get booking details and bind to the GridView
        //            //using (var cmd = db.CreateCommand())
        //            //{
        //            //    cmd.CommandType = CommandType.Text;
        //            //    cmd.CommandText = @"SELECT bw.bw_id, bw.bw_name, bw.bw_unit, bw.bw_total_unit, bw.bw_price, bw.bw_total_price, bw.bk_id
        //            //                FROM BOOKING_WASTE bw
        //            //                INNER JOIN BOOKING b ON bw.bk_id = b.bk_id
        //            //                WHERE bw.bk_id = @bkId AND b.bk_status != 'Completed'";

        //            //    cmd.Parameters.AddWithValue("@bkId", id);

        //            //    DataTable bookingsDataTable = new DataTable();
        //            //    NpgsqlDataAdapter bookingsAdapter = new NpgsqlDataAdapter(cmd);
        //            //    bookingsAdapter.Fill(bookingsDataTable);
        //            //    gridView2.DataSource = bookingsDataTable;
        //            //    gridView2.DataBind();
        //            //}

        //            // Second query to sum total price for the same booking ID and status != 'Completed'
        //            double totalPrice = 0;
        //            using (var cmdSum = db.CreateCommand())
        //            {
        //                cmdSum.CommandType = CommandType.Text;
        //                cmdSum.CommandText = @"SELECT SUM(bw.bw_total_price) AS TotalPrice
        //                               FROM BOOKING_WASTE bw
        //                               INNER JOIN BOOKING b ON bw.bk_id = b.bk_id
        //                               WHERE bw.bk_id = @bkId AND b.bk_status != 'Completed'";

        //                cmdSum.Parameters.AddWithValue("@bkId", id);
        //                object result = cmdSum.ExecuteScalar();
        //                if (result != DBNull.Value)
        //                {
        //                    totalPrice = Convert.ToDouble(result);
        //                    netVatTxt.Text = totalPrice.ToString("N2");
        //                }
        //                else
        //                {
        //                    netVatTxt.Text = "0";
        //                }
        //            }

        //            // Third query to get the payment term details, including tax and periods
        //            double ptTax = 0;
        //            double ptLeadDays = 0;
        //            int accrualPeriod = 0, suspPeriod = 0, ptInterest = 0;
        //            using (var cmdTax = db.CreateCommand())
        //            {
        //                cmdTax.CommandType = CommandType.Text;
        //                cmdTax.CommandText = @"SELECT pt_tax, pt_lead_days, pt_accrual_period, pt_susp_period, pt_interest
        //                               FROM payment_term 
        //                               LIMIT 1"; // Assuming only one payment term is needed

        //                using (var reader = cmdTax.ExecuteReader())
        //                {
        //                    if (reader.Read())
        //                    {
        //                        ptTax = reader.IsDBNull(0) ? 0 : reader.GetDouble(0);  // Tax percentage
        //                        ptLeadDays = reader.IsDBNull(1) ? 0 : reader.GetDouble(1);  // Lead days
        //                        accrualPeriod = reader.IsDBNull(2) || reader.GetInt32(2) == 0 ? 0 : reader.GetInt32(2);  // Accrual period (check for NULL or zero)
        //                        suspPeriod = reader.IsDBNull(3) || reader.GetInt32(3) == 0 ? 0 : reader.GetInt32(3);  // Suspension period (check for NULL or zero)
        //                        ptInterest = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);  // Suspension period

        //                        // Display payment term details in respective labels
        //                        taxLabel.Text = "Tax: " + ptTax + "%"; // Displaying tax percentage
        //                        interstLabel.Text = "Interest: " + ptInterest + "%"; // Displaying interest
        //                        accrPerLabel.Text = "Accrual Period: " + accrualPeriod + " day(s)"; // Displaying accrual period
        //                        susPerLabel.Text = "Suspension Period: " + suspPeriod + " day(s)"; // Displaying suspension period



        //                    }
        //                }
        //            }


        //            ptLeadDaysHiddenField.Value = ptLeadDays.ToString();
        //            ptAccrualPeriodHiddenField.Value = accrualPeriod.ToString();
        //            ptSuspPeriodHiddenField.Value = suspPeriod.ToString();

        //            // Calculate Due Date based on current date + lead days
        //            DateTime currentDate = DateTime.Now;
        //            DateTime dueDate = currentDate.AddDays(ptLeadDays);
        //            dueDateTxt.Text = dueDate.ToString("yyyy-MM-ddTHH:mm"); // Set the Due Date in the textbox

        //            // Handle Accrual Date
        //            if (accrualPeriod > 0)
        //            {
        //                DateTime accrualDate = dueDate.AddDays(accrualPeriod);
        //                accDateTxt.Text = accrualDate.ToString("yyyy-MM-ddTHH:mm"); // Set the Accrual Date in the textbox
        //            }
        //            else
        //            {
        //                accDateTxt.Text = "No accrual date"; // Display "No accrual date" if accrual period is zero or not found
        //            }

        //            // Handle Suspension Date
        //            if (suspPeriod > 0)
        //            {
        //                DateTime suspensionDate = dueDate.AddDays(suspPeriod);
        //                susDateTxt.Text = suspensionDate.ToString("yyyy-MM-ddTHH:mm"); // Set the Suspension Date in the textbox
        //            }
        //            else
        //            {
        //                susDateTxt.Text = "No suspension date"; // Display "No suspension date" if suspension period is zero or not found
        //            }

        //            // Calculate tax amount based on total price
        //            double taxAmount = (ptTax / 100) * totalPrice;
        //            vatAmntTxt.Text = taxAmount.ToString("N2");

        //            // Calculate total sales (total price + tax amount)
        //            double totalSales = totalPrice + taxAmount;
        //            totSalesTxt.Text = totalSales.ToString("N2");

        //            // Set the current date and time in the required format for DateTimeLocal
        //            dateTodayTxt.Text = currentDate.ToString("yyyy-MM-ddTHH:mm");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
        //            $"swal('Unsuccessful!', '{ex.Message}', 'error')", true);
        //        return;
        //    }

        //    this.ModalPopupExtender1.Show();
        //}


        private void PopulateWasteCategory()
        {
            
            using (NpgsqlConnection conn = new NpgsqlConnection(con))
            {
                // Example of fetching waste categories from the PostgreSQL database
                string query = "SELECT wc_id, wc_name FROM waste_category";

                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ////EDIT BOOK WASTE PANEL
                //// Binding the first dropdown list
                //ddlbwName.Items.Clear();
                //ddlbwName.DataSource = dt;
                //ddlbwName.DataTextField = "wc_name";
                //ddlbwName.DataValueField = "wc_id";
                //ddlbwName.DataBind();

                //// Inserting the default item and disabling it
                //WebControls.ListItem defaultItem = new WebControls.ListItem("--Select Waste Category--", "0");
                //defaultItem.Attributes.Add("disabled", "true");  // Disable the option
                //defaultItem.Attributes.Add("selected", "true");  // Set as selected
                //ddlbwName.Items.Insert(0, defaultItem);  // Insert it at the first position



                //ADD BOOK WASTE PANEL
                // Binding the second dropdown list
                ddlbwName1.Items.Clear();
                ddlbwName1.DataSource = dt;
                ddlbwName1.DataTextField = "wc_name";
                ddlbwName1.DataValueField = "wc_id";
                ddlbwName1.DataBind();

                // Inserting the default item and disabling it for the second dropdown
                WebControls.ListItem defaultItem1 = new WebControls.ListItem("--Select Waste Category--", "0");
                defaultItem1.Attributes.Add("disabled", "true");  // Disable the option
                defaultItem1.Attributes.Add("selected", "true");  // Set as selected
                ddlbwName1.Items.Insert(0, defaultItem1);  // Insert it at the first position
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


        //EDIT BOOK WASTE PANEL
        //protected void ddlWasteCategory_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    int selectedWasteID = Convert.ToInt32(ddlbwName.SelectedValue);

        //    if (selectedWasteID > 0)
        //    {
        //        // Query to get the unit and price for the selected waste category
        //        string query = "SELECT wc_unit, wc_price FROM waste_category WHERE wc_id = @wc_id";

        //        using (NpgsqlConnection connection = new NpgsqlConnection(con))
        //        {
        //            NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
        //            cmd.Parameters.AddWithValue("@wc_id", selectedWasteID); // Setting parameter value for wc_id

        //            connection.Open();
        //            NpgsqlDataReader reader = cmd.ExecuteReader();
        //            if (reader.Read())
        //            {
        //                txtUnitPrice.Text = reader["wc_price"].ToString(); // Assign price to the textbox
        //                                                                   // If you need to display the unit in another control, use:
        //                txtbwUnit.Text = reader["wc_unit"].ToString();
        //                txtTotalUnit.Text = "";
        //                txtTotalUnitPrice.Text = "";
        //            }
        //            connection.Close();
        //        }
        //    }
        //}


        //ADD BOOK WASTE PANEL
        protected void ddlWasteCategory_SelectedIndexChanged1(object sender, EventArgs e)
        {
            int selectedWasteID = Convert.ToInt32(ddlbwName1.SelectedValue);

            if (selectedWasteID > 0)
            {
                // Query to get the unit and price for the selected waste category
                string query = "SELECT wc_unit, wc_price FROM waste_category WHERE wc_id = @wc_id";

                using (NpgsqlConnection connection = new NpgsqlConnection(con))
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@wc_id", selectedWasteID); // Setting parameter value for wc_id

                    connection.Open();
                    NpgsqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        txtUnitPrice1.Text = reader["wc_price"].ToString(); // Assign price to the textbox
                                                                           // If you need to display the unit in another control, use:
                        txtbwUnit1.Text = reader["wc_unit"].ToString();
                        txtTotalUnit1.Text = "";
                        txtTotalUnitPrice1.Text = "";
                    }
                    connection.Close();
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

            // Hide the panel/modal
            ModalPopupExtender3.Hide();
        }


        private void ClearValues()
        {
            //// Clear the values in TextBoxes
            //txtSumKilo.Text = string.Empty;
            //txtNetVat.Text = string.Empty;
            //txtVatAmnt.Text = string.Empty;
            //txtTotSales.Text = string.Empty;
        }
        //protected void openViewBill_Click(object sender, EventArgs e)
        //{
        //    LinkButton btn = (LinkButton)sender;
        //    int gb_id = Convert.ToInt32(btn.CommandArgument);
        //    TextBox2.Text = gb_id.ToString();
        //    this.ModalPopupExtender5.Show();
        //    updatePanel4.Update();

        //    LoadBillDetails(gb_id);

        //    //LinkButton btn = sender as LinkButton;
        //    //int id = Convert.ToInt32(btn.CommandArgument);
        //    //txtbwID1.Text = id.ToString();
        //    //TextBox1.Text = id.ToString();
        //    //LoadBookingWasteData(); // Load the relevant data for the booking ID
        //    //this.ModalPopupExtender1.Show();
        //}

        protected void openViewBill_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            int gb_id = Convert.ToInt32(btn.CommandArgument);
            this.ModalPopupExtender5.Show(); // Show the modal
            try
            {
                using (var db = new NpgsqlConnection(con))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = @"SELECT gb_id, gb_date_issued, gb_date_due, gb_status, bk_id, gb_total_amnt_interest, gb_total_sales, 
                                    gb_note, gb_add_fees, gb_net_vat, gb_vat_amnt, gb_interest, gb_lead_days, gb_accrual_period, gb_suspend_period
                                    FROM generate_bill WHERE gb_id = @gb_id";
                        cmd.Parameters.AddWithValue("@gb_id", gb_id);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                TextBox2.Text = reader["gb_id"].ToString();
                                dateEntered.Text = Convert.ToDateTime(reader["gb_date_issued"]).ToString("yyyy-MM-ddTHH:mm");
                                TextBox7.Text = Convert.ToDateTime(reader["gb_date_due"]).ToString("yyyy-MM-ddTHH:mm");
                                TextBox4.Text = reader["gb_net_vat"].ToString();
                                TextBox5.Text = reader["gb_vat_amnt"].ToString();
                                TextBox6.Text = reader["gb_total_sales"].ToString();
                                TextBox8.Text = reader["gb_accrual_period"].ToString();
                                TextBox9.Text = reader["gb_suspend_period"].ToString();
                                TextBox10.Text = reader["gb_add_fees"].ToString();
                                TextBox11.Text = reader["gb_note"].ToString();
                            }
                        }
                    }
                    //LoadBillDetails(gb_id);  // Load data into the GridView
                    updatePanel4.Update();   // Update the UpdatePanel
                    //this.ModalPopupExtender5.Show(); // Show the modal
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showError",
                    $"Swal.fire({{ icon: 'error', title: 'Error', text: '{ex.Message}' }});", true);
            }
        }

        //// Load the GridView with relevant data from generate_bill
        //protected void LoadBillDetails(int gb_id)
        //{
        //    using (var db = new NpgsqlConnection(con))
        //    {
        //        db.Open();
        //        using (var cmd = db.CreateCommand())
        //        {
        //            cmd.CommandType = CommandType.Text;
        //            cmd.CommandText = "SELECT * FROM generate_bill WHERE gb_id = @gb_id";
        //            cmd.Parameters.AddWithValue("@gb_id", gb_id);

        //            using (var adapter = new NpgsqlDataAdapter(cmd))
        //            {
        //                DataTable dt = new DataTable();
        //                adapter.Fill(dt);
        //                gridView3.DataSource = dt;
        //                gridView3.DataBind();
        //            }
        //        }
        //    }
        //}


        //protected void ViewBill_Click(object sender, EventArgs e)
        //{
        //    //LinkButton btn = (LinkButton)sender;
        //    //int gb_id = Convert.ToInt32(btn.CommandArgument);
        //    int gb_id = Convert.ToInt32(TextBox2.Text);
        //    //int gb_id = 37;
        //    //updatePanel4.Update();


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
        //    //int gb_id = 1; // Assuming you are fetching by `gb_id = 1`

        //    //DateTime? currentDate = DateTime.TryParse(dateEntered.Text, out DateTime EntDate) ? (DateTime?)EntDate : null;

        //    // Retrieve payment term values from the database
        //    using (var conn = new NpgsqlConnection(con)) // Use NpgsqlConnection
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

        //    // Continue with bill generation and PDF creation
        //    int insertedBillId = gb_id;
        //    byte[] pdfBytes = GeneratePDFViewBill(insertedBillId, bk_id);
        //    LoadBookingList();

        //    // Send the PDF for download.
        //    Response.Clear();
        //    Response.ContentType = "application/pdf";
        //    Response.AddHeader("content-disposition", $"attachment;filename=Bill_{insertedBillId}.pdf");
        //    Response.Buffer = true;
        //    Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //    Response.BinaryWrite(pdfBytes);
        //    Response.End();
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
                            gb_interest = reader["gb_interest"] as double?;
                            gb_lead_days = reader["gb_lead_days"] as int?;
                            gb_accrual_period = reader["gb_accrual_period"] as int?;
                            gb_suspend_period = reader["gb_suspend_period"] as int?;
                            gb_accrual_date = reader["gb_accrual_date"] as DateTime?;
                            gb_suspend_date = reader["gb_suspend_date"] as DateTime?;
                            gb_tax = reader["gb_tax"] as decimal?;
                            gb_total_amnt_interest = reader["gb_total_amnt_interest"] as decimal?;
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
                //PdfFont font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
                PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                DeviceRgb redColor = new DeviceRgb(255, 0, 0);
                // Ensure this font file exists in your project folder
                string fontPath = Server.MapPath("~/fonts/Roboto/Roboto-Regular.ttf");
                // Create the font, specifying Unicode support with "Identity-H"
                PdfFont font = PdfFontFactory.CreateFont(fontPath, "Identity-H", PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);



                // Create a table with one row and two columns for the logo and address
                iText.Layout.Element.Table headerTable = new iText.Layout.Element.Table(new float[] { 1, 3 })
                    .UseAllAvailableWidth();

                // Add company logo (if any)
                string logoPath = Server.MapPath("~/Pictures/logo_bgRM.png");
                iText.Layout.Element.Image logo = new iText.Layout.Element.Image(ImageDataFactory.Create(logoPath));
                logo.ScaleToFit(100, 50); // Scale the logo to fit within defined dimensions

                // Create a table for the logo and TrashTrack text
                iText.Layout.Element.Table logoTextTable = new iText.Layout.Element.Table(2)
                    .UseAllAvailableWidth();

                // Create the logo cell
                Cell logoCell = new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(logo); // Add the logo

                // Define the green color
                DeviceRgb greenColor = new DeviceRgb(0, 128, 0); // Dark green color

                // Create the TrashTrack text without margin and padding
                Paragraph trashTrackText = new Paragraph("TrashTrack")
                    .SetFont(boldFont) // Use bold font for the text
                    .SetFontSize(30) // Set the font size
                    .SetFontColor(greenColor) // Set the font color to green
                    .SetTextAlignment(TextAlignment.LEFT) // Align text left
                    .SetMargin(0) // No margin for the paragraph
                    .SetPadding(0); // No padding for the paragraph

                // Create a cell for the TrashTrack text
                Cell textCell = new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(trashTrackText); // Add the text

                // Add both cells to the logoTextTable
                logoTextTable.AddCell(logoCell);
                logoTextTable.AddCell(textCell);

                // Add the logo and text table to the headerTable
                headerTable.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(logoTextTable)); // Add the table with logo and text

                // Add Company Address to the second cell
                Paragraph address = new Paragraph("Binaliw Cebu Dumpsite\nCebu City, Cebu\nPhilippines")
                    .SetFont(font)
                    .SetTextAlignment(TextAlignment.RIGHT); // Adjust text alignment as needed

                headerTable.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(address));

                // Add the header table to the document
                document.Add(headerTable);

                // Initialize variables with default values to avoid uninitialized errors 
                int leadDays = 0;
                int taxValue = 0;
                int accPerValue = 0;
                int susPerValue = 0;
                double totalSum = 0;
                double vat_Amnt = 0;
                double totalPayment = 0;
                double amntInterest = 0;
                double interestRate = 0;
                int accrualPeriod = 0;
                int suspendPeriod = 0;
                double totalSales = 0;

                // Use a nullable DateTime for dateIssued in case it’s not set
                DateTime? dateIssued = null;
                DateTime? dueDate = null;

                // Parse the entered date
                DateTime? currentDate = DateTime.TryParse(dateEntered.Text, out DateTime dateCurrent) ? (DateTime?)dateCurrent : null;

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
                                leadDays = Convert.ToInt32(reader["pt_lead_days"]);
                                taxValue = Convert.ToInt32(reader["pt_tax"]);
                                accPerValue = Convert.ToInt32(reader["pt_accrual_period"]);
                                susPerValue = Convert.ToInt32(reader["pt_susp_period"]);
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
                                dateIssued = Convert.ToDateTime(billReader["gb_date_issued"]);
                                totalSales = Convert.ToDouble(billReader["gb_total_sales"]);
                                interestRate = Convert.ToDouble(billReader["gb_interest"]);
                                accrualPeriod = Convert.ToInt32(billReader["gb_accrual_period"]);
                                suspendPeriod = Convert.ToInt32(billReader["gb_suspend_period"]);

                                dueDate = dateIssued?.AddDays(leadDays); // Calculate due date

                                // Calculate the initial total payment and interest if current date is past due date
                                totalPayment = totalSales;
                                if (currentDate > dueDate)
                                {
                                    amntInterest = totalSales * (interestRate / 100);
                                    totalPayment += amntInterest;
                                }

                                // Apply further interest based on accrual and suspension periods
                                DateTime accrualDate = dueDate?.AddDays(accrualPeriod) ?? DateTime.Now;
                                DateTime suspDate = dueDate?.AddDays(suspendPeriod) ?? DateTime.Now;

                                while (currentDate >= accrualDate && currentDate <= suspDate)
                                {
                                    amntInterest = totalPayment * (interestRate / 100);
                                    totalPayment += amntInterest;
                                    accrualDate = accrualDate.AddDays(accrualPeriod); // Increment to next accrual period
                                }
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
                            vat_Amnt = totalSum * (taxValue / 100.0);
                        }
                    }

                    double totAmnt = vat_Amnt + totalSum;

                    // Add Title
                    Paragraph title = new Paragraph("Billing Statement")
                        .SetFont(boldFont)
                        .SetFontSize(16)
                        .SetTextAlignment(TextAlignment.CENTER);
                    document.Add(title);

                    // Add Booking and Bill ID Information without borders
                    iText.Layout.Element.Table infoTable = new iText.Layout.Element.Table(2).UseAllAvailableWidth();

                    // Remove all borders for each cell and content
                    infoTable.SetBorder(Border.NO_BORDER);

                    // Bill ID cell
                    infoTable.AddCell(new Cell()
                        .SetBorder(Border.NO_BORDER)
                        .Add(new Paragraph($"Bill ID: {buttonText}")
                            .SetFont(boldFont)
                            .SetBorder(Border.NO_BORDER)));

                    // Invoice # cell, aligned to the right
                    infoTable.AddCell(new Cell()
                        .SetBorder(Border.NO_BORDER)
                        .Add(new Paragraph($"Date Today: {currentDate?.ToString("MM/dd/yyyy") ?? "N/A"}")
                            .SetFont(boldFont)
                            .SetTextAlignment(TextAlignment.RIGHT)
                            .SetBorder(Border.NO_BORDER)));

                    // Booking ID cell
                    infoTable.AddCell(new Cell()
                        .SetBorder(Border.NO_BORDER)
                        .Add(new Paragraph($"Booking ID: {bkID}")
                            .SetFont(boldFont)
                            .SetBorder(Border.NO_BORDER)));

                    // Date Issued cell, aligned to the right
                    infoTable.AddCell(new Cell()
                        .SetBorder(Border.NO_BORDER)
                        .Add(new Paragraph($"Date Issued: {(dateIssued?.ToString("MM/dd/yyyy") ?? "N/A")}")
                            .SetFont(boldFont)
                            .SetTextAlignment(TextAlignment.RIGHT)
                            .SetBorder(Border.NO_BORDER)));

                    // Empty cell for spacing
                    infoTable.AddCell(new Cell()
                        .SetBorder(Border.NO_BORDER)
                        .Add(new Paragraph("").SetBorder(Border.NO_BORDER)));

                    // Due Date cell, aligned to the right
                    infoTable.AddCell(new Cell()
                        .SetBorder(Border.NO_BORDER)
                        .Add(new Paragraph($"Due Date: {(dueDate?.ToString("MM/dd/yyyy") ?? "N/A")}")
                            .SetFont(boldFont)
                            .SetTextAlignment(TextAlignment.RIGHT)
                            .SetBorder(Border.NO_BORDER)));

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
                    $"The bill shall be due for payment and collection ({leadDays}) day/s after issuance. " +
                    $"Failure by the customer to make payment without valid and justifiable reason will result in a late payment charge of ({interestRate}%) " +
                    $"per {accPerValue} day/s applied to any outstanding balance until {susPerValue} day/s. " +
                    $"Additionally, TrashTrack reserves the right to stop collecting waste materials from the customer's premises if payment is not made, " +
                    $"preventing further processing and disposal services."
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
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("") // Empty paragraph
                        .SetBorder(Border.NO_BORDER) // No border for the paragraph
                        .SetBorderTop(new SolidBorder(1f)) // Add solid top border
                    )
                );

                // Add the bottom line table to the document
                document.Add(btmLine);


                float[] columnWidths = new float[] { 100, 40, 30, 80, 100 }; // Set fixed pixel widths
                iText.Layout.Element.Table summarySection = new iText.Layout.Element.Table(columnWidths).UseAllAvailableWidth();


                // Add Bill ID cell (empty for spacing)
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("").SetBorder(Border.NO_BORDER))); // No border for empty content
                                                                          // Add Bill ID cell (empty for spacing)
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("").SetBorder(Border.NO_BORDER))); // No border for empty content
                                                                          // Add Bill ID cell (empty for spacing)
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("").SetBorder(Border.NO_BORDER))); // No border for empty content
                                                                          // Add Bill ID cell (empty for spacing)
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("Net of VAT: ").SetFont(boldFont)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetBorder(Border.NO_BORDER))); // No border for empty content

                // Add Sum Amount cell, aligned to the left
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("₱" + totalSum.ToString("N2"))
                        .SetFont(boldFont)
                        .SetFont(font)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetBorder(Border.NO_BORDER))); // No border for the content

                // Add Bill ID cell (empty for spacing)
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("").SetBorder(Border.NO_BORDER))); // No border for empty content

                // Add Bill ID cell (empty for spacing)
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("").SetBorder(Border.NO_BORDER))); // No border for empty content

                // Add Bill ID cell (empty for spacing)
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("").SetBorder(Border.NO_BORDER))); // No border for empty content

                // Add empty cell for spacing
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("VAT (" + taxValue + "%): ")
                        .SetFont(boldFont)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetBorder(Border.NO_BORDER))); // No border for empty content

                // Add VAT Amount cell, aligned to the left
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("₱" + vat_Amnt.ToString("N2"))
                        .SetFont(boldFont)
                        .SetFont(font)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetBorder(Border.NO_BORDER))); // No border for the content

                // Add Bill ID cell (empty for spacing)
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("").SetBorder(Border.NO_BORDER))); // No border for empty content
                                                                          // Add Bill ID cell (empty for spacing)
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("").SetBorder(Border.NO_BORDER))); // No border for empty content
                                                                          // Add Bill ID cell (empty for spacing)
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("").SetBorder(Border.NO_BORDER))); // No border for empty content


                // Add empty cell for spacing
                summarySection.AddCell(new Cell()
                        .SetBorder(Border.NO_BORDER) // No border for the cell
                        .Add(new Paragraph("Total Sales: ")
                            .SetFont(font)
                            .SetFontColor(redColor)
                            .SetTextAlignment(TextAlignment.LEFT)
                            .SetBorder(Border.NO_BORDER))); // No border for empty content

                // Add Total Amount cell, aligned to the left
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("₱" + totalSales.ToString("N2") + "")
                        .SetFont(font)
                        .SetFontColor(redColor)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetBorder(Border.NO_BORDER))); // No border for the content

                // Add Bill ID cell (empty for spacing)
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("").SetBorder(Border.NO_BORDER))); // No border for empty content
                                                                          // Add Bill ID cell (empty for spacing)
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("").SetBorder(Border.NO_BORDER))); // No border for empty content
                                                                          // Add Bill ID cell (empty for spacing)
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("").SetBorder(Border.NO_BORDER))); // No border for empty content


                // Add empty cell for spacing
                summarySection.AddCell(new Cell()
                        .SetBorder(Border.NO_BORDER) // No border for the cell
                        .Add(new Paragraph("Total Amount: ")
                            .SetFont(font)
                            .SetFontColor(redColor)
                            .SetTextAlignment(TextAlignment.LEFT)
                            .SetBorder(Border.NO_BORDER))); // No border for empty content

                // Add Total Amount cell, aligned to the left
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("₱" + totalPayment.ToString("N2") + "")
                        .SetFont(font)
                        .SetFontColor(redColor)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetBorder(Border.NO_BORDER))); // No border for the content

                // Add the summary section table to the document
                document.Add(summarySection);


                //totalPayment

                // Close document
                document.Close();

                return ms.ToArray();

            }
        }

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
            DateTime? accrualDate = DateTime.TryParse(accDateTxt.Text, out DateTime accDate) ? (DateTime?)accDate : null;
            DateTime? suspensionDate = DateTime.TryParse(susDateTxt.Text, out DateTime susDate) ? (DateTime?)susDate : null;

            // Variables for payment terms
            double? interest = null;
            int? leadDays = null;
            int? accrualPeriod = null;
            int? suspensionPeriod = null;
            int? tax = null;

            // Retrieve payment term values from the database
            using (var conn = new NpgsqlConnection(con))
            {
                conn.Open();
                string paymentTermQuery = "SELECT pt_interest, pt_lead_days, pt_accrual_period, pt_susp_period, pt_tax FROM payment_term WHERE emp_id = @EmpId";
                using (var cmd = new NpgsqlCommand(paymentTermQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@EmpId", empId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            interest = reader.IsDBNull(0) ? (double?)null : reader.GetDouble(0);
                            leadDays = reader.IsDBNull(1) ? (int?)null : reader.GetInt32(1);
                            accrualPeriod = reader.IsDBNull(2) ? (int?)null : reader.GetInt32(2);
                            suspensionPeriod = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3);
                            tax = reader.IsDBNull(4) ? (int?)null : reader.GetInt32(4);
                        }
                    }
                }
            }

            int bk_id, cus_id;
            string cus_fullname;

            using (var conn = new NpgsqlConnection(con))
            {
                conn.Open();
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
                            LoadBookingList();
                        }
                        else
                        {
                            throw new Exception("Booking ID not found.");
                        }
                    }
                }

                string updateStatusQuery = "UPDATE booking SET bk_status = 'Billed' WHERE bk_id = @BkId";
                using (var updateCmd = new NpgsqlCommand(updateStatusQuery, conn))
                {
                    updateCmd.Parameters.AddWithValue("@BkId", bk_id);
                    int rowsAffected = updateCmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Booking status updated to 'Billed' successfully.");
                        updatePanel1.Update();
                        LoadBookingList();
                    }
                    else
                    {
                        throw new Exception("Failed to update booking status.");
                    }
                }
            }

            int insertedBillId;

            using (var conn = new NpgsqlConnection(con))
            {
                conn.Open();
                string query = @"
            INSERT INTO generate_bill (
                gb_note, gb_add_fees, gb_net_vat, gb_vat_amnt, gb_total_sales, 
                gb_date_issued, gb_date_due, gb_interest, gb_lead_days, 
                gb_accrual_period, gb_suspend_period, gb_accrual_date, 
                gb_suspend_date, gb_tax, gb_status, bk_id, emp_id
            ) 
            VALUES (
                @Note, @AddFees, @NetVat, @VatAmount, @TotalSales, 
                @DateIssued, @DateDue, @Interest, @LeadDays, 
                @AccrualPeriod, @SuspensionPeriod, @AccrualDate, 
                @SuspensionDate, @Tax, @Status, @BkId, @EmpId
            ) 
            RETURNING gb_id;";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Note", note ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@AddFees", additionalFees ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@NetVat", netVat ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@VatAmount", vatAmount ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@TotalSales", totalSales ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@DateIssued", dateIssued ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@DateDue", dateDue ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Interest", interest ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@LeadDays", leadDays ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@AccrualPeriod", accrualPeriod ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@SuspensionPeriod", suspensionPeriod ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@AccrualDate", accrualDate ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@SuspensionDate", suspensionDate ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Tax", tax ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Status", "unpaid");
                    cmd.Parameters.AddWithValue("@BkId", Convert.ToInt32(TextBox1.Text));
                    cmd.Parameters.AddWithValue("@EmpId", empId);

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
            }

            string notif_message = "Your bill is now available for payment 🧾. \n\n" +
                                   "------------------------------------------\n" +
                                   "BILL# " + insertedBillId + "\n\n" +
                                   "Dear " + cus_fullname + ",\n\n" +
                                   "You can now review the details at your convenience. Please check it as soon as possible to avoid any delays. " +
                                   "Thank you for your cooperation 💜";

            using (var conn = new NpgsqlConnection(con))
            {
                conn.Open();
                string query = @"
            INSERT INTO notification (
                notif_message, emp_id, cus_id, bk_id, gb_id
            ) 
            VALUES (
                @Message, @EmpId, @CusId, @BkId, @GbId
            ) 
            RETURNING gb_id;";

                using (var cmd = new NpgsqlCommand(query, conn))
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
            }
            LoadBookingList();
            byte[] pdfBytes = GeneratePDFForRow(insertedBillId, bk_id);
            LoadBookingList();
            // Send the PDF for download.
            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", $"attachment;filename=Bill_{insertedBillId}.pdf");
            Response.Buffer = true;
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.BinaryWrite(pdfBytes);
            Response.End();
            LoadBookingList();
        }

        private byte[] GeneratePDFForRow(int buttonText, int bkID)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(ms);
                PdfDocument pdf = new PdfDocument(writer);
                ITextDocument document = new ITextDocument(pdf);

                // Define fonts and colors
                //PdfFont font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
                PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                DeviceRgb redColor = new DeviceRgb(255, 0, 0);
                // Ensure this font file exists in your project folder
                string fontPath = Server.MapPath("~/fonts/Roboto/Roboto-Regular.ttf");
                // Create the font, specifying Unicode support with "Identity-H"
                PdfFont font = PdfFontFactory.CreateFont(fontPath, "Identity-H", PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);



                // Create a table with one row and two columns for the logo and address
                iText.Layout.Element.Table headerTable = new iText.Layout.Element.Table(new float[] { 1, 3 })
                    .UseAllAvailableWidth();

                // Add company logo (if any)
                string logoPath = Server.MapPath("~/Pictures/logo_bgRM.png");
                iText.Layout.Element.Image logo = new iText.Layout.Element.Image(ImageDataFactory.Create(logoPath));
                logo.ScaleToFit(100, 50); // Scale the logo to fit within defined dimensions

                // Create a table for the logo and TrashTrack text
                iText.Layout.Element.Table logoTextTable = new iText.Layout.Element.Table(2)
                    .UseAllAvailableWidth();

                // Create the logo cell
                Cell logoCell = new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(logo); // Add the logo

                // Define the green color
                DeviceRgb greenColor = new DeviceRgb(0, 128, 0); // Dark green color

                // Create the TrashTrack text without margin and padding
                Paragraph trashTrackText = new Paragraph("TrashTrack")
                    .SetFont(boldFont) // Use bold font for the text
                    .SetFontSize(30) // Set the font size
                    .SetFontColor(greenColor) // Set the font color to green
                    .SetTextAlignment(TextAlignment.LEFT) // Align text left
                    .SetMargin(0) // No margin for the paragraph
                    .SetPadding(0); // No padding for the paragraph

                // Create a cell for the TrashTrack text
                Cell textCell = new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(trashTrackText); // Add the text

                // Add both cells to the logoTextTable
                logoTextTable.AddCell(logoCell);
                logoTextTable.AddCell(textCell);

                // Add the logo and text table to the headerTable
                headerTable.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(logoTextTable)); // Add the table with logo and text

                // Add Company Address to the second cell
                Paragraph address = new Paragraph("Binaliw Cebu Dumpsite\nCebu City, Cebu\nPhilippines")
                    .SetFont(font)
                    .SetTextAlignment(TextAlignment.RIGHT); // Adjust text alignment as needed

                headerTable.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(address));

                // Add the header table to the document
                document.Add(headerTable);

                // Initialize variables with default values to avoid uninitialized errors
                int leadDays = 0;
                int taxValue = 0;
                int accPerValue = 0;
                int susPerValue = 0;
                double totalSum = 0;
                double vat_Amnt = 0;

                DateTime dateIssued = DateTime.Now; // Default value, will be overwritten
                // Database connection and fetching values
                using (var db = new NpgsqlConnection(con))
                {
                    db.Open();
                    // Fetch interest and leadDays values from PostgreSQL
                    string controlQuery = @"SELECT * FROM payment_term";

                    using (var cmd = new NpgsqlCommand(controlQuery, db))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Read values from the database
                                leadDays = Convert.ToInt32(reader["pt_lead_days"]);
                                taxValue = Convert.ToInt32(reader["pt_tax"]);
                                accPerValue = Convert.ToInt32(reader["pt_accrual_period"]);
                                susPerValue = Convert.ToInt32(reader["pt_susp_period"]);
                            }
                            else
                            {
                                // Handle the case where no rows are returned (optional)
                                Console.WriteLine("No payment term data found. Using default values.");
                            }
                        }
                    }
                    // Fetch date issued from generate_bill
                    string billQuery = @"SELECT * FROM generate_bill WHERE gb_id = @BillId"; // Adjust query as needed
                    using (var billCmd = new NpgsqlCommand(billQuery, db))
                    {
                        billCmd.Parameters.AddWithValue("@BillId", buttonText); // Use buttonText for bill ID
                        using (var reader = billCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                dateIssued = Convert.ToDateTime(reader["gb_date_issued"]);
                            }
                            else
                            {
                                // Handle the case where no rows are returned (optional)
                                Console.WriteLine("No payment term data found. Using default values.");
                            }
                        }

                    }
                    

                    string totalQuery = @"SELECT SUM(bw_total_price) FROM booking_waste WHERE bk_id = @BkId";
                    using (var totalCmd = new NpgsqlCommand(totalQuery, db))
                    {
                        totalCmd.Parameters.AddWithValue("@BkId", bkID);
                        object result = totalCmd.ExecuteScalar(); // Execute the query and get the total

                        // Check if result is not null and assign it to totalSum
                        if (result != DBNull.Value)
                        {
                            totalSum += Convert.ToDouble(result);
                            vat_Amnt = totalSum * (taxValue / 100.0);
                        }
                    }
                }

                double totAmnt = vat_Amnt + totalSum;
                DateTime dueDate = dateIssued.AddDays(leadDays);

                // Add Title
                Paragraph title = new Paragraph("Billing Statement")
                    .SetFont(boldFont)
                    .SetFontSize(16)
                    .SetTextAlignment(TextAlignment.CENTER);
                document.Add(title);

                // Add Booking and Bill ID Information without borders
                iText.Layout.Element.Table infoTable = new iText.Layout.Element.Table(2).UseAllAvailableWidth();

                // Remove all borders for each cell and content
                infoTable.SetBorder(Border.NO_BORDER); // Ensure the table itself has no border

                // Bill ID cell
                infoTable.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph($"Bill ID: {buttonText}")
                        .SetFont(boldFont)
                        .SetBorder(Border.NO_BORDER))); // No border for the content

                // Invoice # cell, aligned to the right
                infoTable.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph($"Invoice #: ######")
                        .SetFont(boldFont)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetBorder(Border.NO_BORDER))); // No border for the content

                // Booking ID cell
                infoTable.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph($"Booking ID: {bkID}")
                        .SetFont(boldFont)
                        .SetBorder(Border.NO_BORDER))); // No border for the content

                // Date Issued cell, aligned to the right
                infoTable.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph($"Date Issued: {DateTime.Now.ToString("MM/dd/yyyy")}")
                        .SetFont(boldFont)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetBorder(Border.NO_BORDER))); // No border for the content

                // Empty cell for spacing
                infoTable.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("").SetBorder(Border.NO_BORDER))); // No border for empty content

                // Due Date cell, aligned to the right
                infoTable.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph($"Due Date: {dueDate:MM/dd/yyyy}")
                        .SetFont(boldFont)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetBorder(Border.NO_BORDER))); // No border for the content

                // Add the table to the document
                document.Add(infoTable);

                
                // Add Terms
                Paragraph termsTitle = new Paragraph("TERMS:")
                    .SetFont(boldFont)
                    .SetFontSize(12);
                document.Add(termsTitle);

                // Create terms content paragraph with formatted strings using default values
                Paragraph termsContent = new Paragraph(
                    $"The bill shall be due for payment and collection ({leadDays}) day/s after issuance. " +
                    $"Failure by the customer to make payment without valid and justifiable reason will result in a late payment charge of ({taxValue}%) " +
                    $"per {accPerValue} day/s applied to any outstanding balance until {susPerValue} day/s. " +
                    $"Additionally, TrashTrack reserves the right to stop collecting waste materials from the customer's premises if payment is not made, " +
                    $"preventing further processing and disposal services."
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
                using (var db = new NpgsqlConnection(con))
                {
                    db.Open();

                    string wasteQuery = @"SELECT bw_name, bw_unit, bw_total_unit, bw_price, bw_total_price 
                                  FROM booking_waste 
                                  WHERE bk_id = @BkId";
                    using (var wasteCmd = new NpgsqlCommand(wasteQuery, db))
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
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("") // Empty paragraph
                        .SetBorder(Border.NO_BORDER) // No border for the paragraph
                        .SetBorderTop(new SolidBorder(1f)) // Add solid top border
                    )
                );

                // Add the bottom line table to the document
                document.Add(btmLine);


                float[] columnWidths = new float[] { 100, 40, 30, 80, 100 }; // Set fixed pixel widths
                iText.Layout.Element.Table summarySection = new iText.Layout.Element.Table(columnWidths).UseAllAvailableWidth();


                // Add Bill ID cell (empty for spacing)
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("").SetBorder(Border.NO_BORDER))); // No border for empty content
                // Add Bill ID cell (empty for spacing)
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("").SetBorder(Border.NO_BORDER))); // No border for empty content
                // Add Bill ID cell (empty for spacing)
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("").SetBorder(Border.NO_BORDER))); // No border for empty content
                // Add Bill ID cell (empty for spacing)
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("Net of VAT: ").SetFont(boldFont)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetBorder(Border.NO_BORDER))); // No border for empty content

                // Add Sum Amount cell, aligned to the left
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("₱" + totalSum.ToString("N2"))
                        .SetFont(boldFont)
                        .SetFont(font)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetBorder(Border.NO_BORDER))); // No border for the content

                // Add Bill ID cell (empty for spacing)
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("").SetBorder(Border.NO_BORDER))); // No border for empty content

                // Add Bill ID cell (empty for spacing)
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("").SetBorder(Border.NO_BORDER))); // No border for empty content

                // Add Bill ID cell (empty for spacing)
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("").SetBorder(Border.NO_BORDER))); // No border for empty content

                // Add empty cell for spacing
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("VAT (12%): ")
                        .SetFont(boldFont)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetBorder(Border.NO_BORDER))); // No border for empty content

                // Add VAT Amount cell, aligned to the left
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("₱" + vat_Amnt.ToString("N2"))
                        .SetFont(boldFont)
                        .SetFont(font)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetBorder(Border.NO_BORDER))); // No border for the content

                //// Add Bill ID cell (empty for spacing)
                //summarySection.AddCell(new Cell()
                //    .SetBorder(Border.NO_BORDER) // No border for the cell
                //    .Add(new Paragraph("").SetBorder(Border.NO_BORDER))); // No border for empty content
                //// Add Bill ID cell (empty for spacing)
                //summarySection.AddCell(new Cell()
                //    .SetBorder(Border.NO_BORDER) // No border for the cell
                //    .Add(new Paragraph("").SetBorder(Border.NO_BORDER))); // No border for empty content
                //// Add Bill ID cell (empty for spacing)
                //summarySection.AddCell(new Cell()
                //    .SetBorder(Border.NO_BORDER) // No border for the cell
                //    .Add(new Paragraph("").SetBorder(Border.NO_BORDER))); // No border for empty content


                //// Add empty cell for spacing
                //summarySection.AddCell(new Cell()
                //    .SetBorder(Border.NO_BORDER) // No border for the cell
                //    .Add(new Paragraph("Interest Amount: ")
                //        .SetFont(font)
                //        .SetFontColor(redColor)
                //        .SetTextAlignment(TextAlignment.LEFT)
                //        .SetBorder(Border.NO_BORDER))); // No border for empty content

                //// Add Total Amount cell, aligned to the left
                //summarySection.AddCell(new Cell()
                //    .SetBorder(Border.NO_BORDER) // No border for the cell
                //    .Add(new Paragraph("₱ 20,115.31")
                //        .SetFont(font)
                //        .SetFontColor(redColor)
                //        .SetTextAlignment(TextAlignment.LEFT)
                //        .SetBorder(Border.NO_BORDER))); // No border for the content


                // Add Bill ID cell (empty for spacing)
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("").SetBorder(Border.NO_BORDER))); // No border for empty content
                // Add Bill ID cell (empty for spacing)
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("").SetBorder(Border.NO_BORDER))); // No border for empty content
                // Add Bill ID cell (empty for spacing)
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("").SetBorder(Border.NO_BORDER))); // No border for empty content

                // Add empty cell for spacing
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("Total Amount: ")
                        .SetFont(font)
                        .SetFontColor(redColor)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetBorder(Border.NO_BORDER))); // No border for empty content

                // Add Total Amount cell, aligned to the left
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("₱" + totAmnt.ToString("N2"))
                        .SetFont(font)
                        .SetFontColor(redColor)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetBorder(Border.NO_BORDER))); // No border for the content

                // Add the summary section table to the document
                document.Add(summarySection);




                // Close document
                document.Close();

                return ms.ToArray();

            }
        }

        protected void Unpaid_Click(object sender, EventArgs e)
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
                        cmd.CommandText = "UPDATE generate_bill SET gb_status = 'unpaid' WHERE gb_id = @id";
                        cmd.Parameters.AddWithValue("@id", managerId);

                        var ctr = cmd.ExecuteNonQuery();
                        if (ctr >= 1)
                        {
                            // Show success dialog with SweetAlert
                            ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
                                "Swal.fire({ icon: 'success', title: 'Marked Unpaid!', text: 'Bill Marked Unpaid Successfully!', background: '#e9f7ef', confirmButtonColor: '#28a745' });", true);

                            GeneratedBillList(); // Refresh the list
                        }
                    }
                    GeneratedBillList();
                    LoadProfile();
                    db.Close();
                }
            }
            catch (Exception ex)
            {
                // Show error dialog with SweetAlert
                ScriptManager.RegisterStartupScript(this, GetType(), "showError",
                    $"Swal.fire({{ icon: 'error', title: 'Unsuccessful!', text: '{ex.Message}', background: '#f8d7da', confirmButtonColor: '#dc3545' }});", true);
            }
        }


        protected void Paid_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int managerId = Convert.ToInt32(btn.CommandArgument);

            hfActiveTab.Value = "#tab2"; // Set the tab to display
            TextBox3.Text = managerId.ToString(); // Set ID in TextBox
                                                  // Store the manager ID in a hidden field for later use
            hfManagerId.Value = managerId.ToString();
            ModalPopupExtender1.Hide();
            // Show the modal popup
            ModalPopupExtender6.Show();
            // Optional: Load other details related to managerId if needed
            this.ModalPopupExtender6.Show(); // Show the modal
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
                        cmd.CommandText = "UPDATE generate_bill SET gb_status = 'paid', gb_updated_at = CURRENT_TIMESTAMP WHERE gb_id = @id";
                        cmd.Parameters.AddWithValue("@id", gbID);

                        var ctr = cmd.ExecuteNonQuery();
                        if (ctr >= 1)
                        {
                            // Insert a new record into the payment table
                            using (var paymentCmd = db.CreateCommand())
                            {
                                paymentCmd.CommandType = CommandType.Text;
                                paymentCmd.CommandText = @"
                        INSERT INTO payment (p_amount, p_status, p_method, gb_id, p_date_paid, emp_id)
                        VALUES (@amount, 'paid', 'walk-in', @gbId, CURRENT_TIMESTAMP, @emp_id)";

                                paymentCmd.Parameters.AddWithValue("@amount", amntPaid);
                                paymentCmd.Parameters.AddWithValue("@gbId", gbID);
                                paymentCmd.Parameters.AddWithValue("@emp_id", empId);


                                paymentCmd.ExecuteNonQuery();
                            }

                            // Display success message with SweetAlert
                            ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
                                "Swal.fire({ icon: 'success', title: 'Update Successful', text: 'Account Manager updated to paid successfully!', background: '#e9f7ef', confirmButtonColor: '#28a745' });", true);
                            ModalPopupExtender6.Hide();
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
            return status == "unpaid";
        }

        // validate if the admin status is Unsuspend
        protected Boolean IsActive(string status)
        {
            return status == "paid";
        }




    }
}