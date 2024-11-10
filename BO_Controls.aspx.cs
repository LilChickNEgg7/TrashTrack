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

                    // Assuming that 'emp_id' is associated with the current session user's ID
                    cmd.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Integer, Convert.ToInt32(Session["bo_id"]));

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
                        // Display an error message indicating that the waste name already exists
                        Response.Write("<script>alert('Waste name already exists. Please choose a different waste name.')</script>");
                        return;
                    }
                }

                // Validate and convert the price
                if (!decimal.TryParse(price.Text, out decimal wastePrice))
                {
                    // Display an error message for invalid price
                    Response.Write("<script>alert('Invalid price. Please enter a valid decimal value.')</script>");
                    return;
                }

                // Round to two decimal places
                wastePrice = Math.Round(wastePrice, 2);

                // Insert the new waste category into the PostgreSQL database
                string insertQuery = @"
            INSERT INTO waste_category (wc_name, wc_unit, wc_price, wc_status, emp_id, wc_max) 
            VALUES (@name, @unit, @price, 'Active', @empId, @max)";

                using (var insertCmd = new NpgsqlCommand(insertQuery, conn))
                {
                    insertCmd.Parameters.AddWithValue("@name", waste_name.Text);
                    insertCmd.Parameters.AddWithValue("@unit", unit.Text);
                    insertCmd.Parameters.AddWithValue("@price", wastePrice);
                    insertCmd.Parameters.AddWithValue("@empId", Convert.ToInt32(Session["bo_id"])); // Assuming 'emp_id' is stored in the session
                    insertCmd.Parameters.AddWithValue("@max", Convert.ToInt32(max.Text));

                    insertCmd.ExecuteNonQuery();
                    Response.Write("<script>alert('Waste added!')</script>");
                    WasteCatList();
                }
                WasteCatList();
                conn.Close();
            }

            // Load or refresh the category data (implement this method as needed)
            //LoadCategoryData();
        }


        protected void changeTerm_Click(object sender, EventArgs e)
        {
            // Validate inputs
            if (double.TryParse(interest.Text, out double interestValue) &&
                int.TryParse(leaddays.Text, out int leadDays) &&
                double.TryParse(vat.Text, out double vatValue) &&
                int.TryParse(acc_per.Text, out int accrualPeriod) &&
                int.TryParse(susp_per.Text, out int suspensionPeriod))
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
                            string updateQuery = @"
                        UPDATE PAYMENT_TERM 
                        SET pt_interest = @interestValue, 
                            pt_lead_days = @leadDays,
                            pt_tax = @vatValue,
                            pt_accrual_period = @accrualPeriod,
                            pt_susp_period = @suspensionPeriod,
                            pt_updated_at = NOW()"; // Specify the control_id or appropriate condition

                            using (var updateCmd = new NpgsqlCommand(updateQuery, conn))
                            {
                                updateCmd.Parameters.AddWithValue("@interestValue", interestValue);
                                updateCmd.Parameters.AddWithValue("@leadDays", leadDays);
                                updateCmd.Parameters.AddWithValue("@vatValue", vatValue);
                                updateCmd.Parameters.AddWithValue("@accrualPeriod", accrualPeriod);
                                updateCmd.Parameters.AddWithValue("@suspensionPeriod", suspensionPeriod);
                                updateCmd.ExecuteNonQuery();
                            }

                            Response.Write("<script>alert('Payment term updated successfully!')</script>");
                            showPaymentTerm();
                            WasteCatList();
                        }
                        else
                        {
                            // Insert a new record if no records exist
                            string insertQuery = @"
                        INSERT INTO PAYMENT_TERM (pt_interest, pt_lead_days, pt_tax, pt_accrual_period, pt_susp_period, emp_id, pt_created_at, pt_updated_at) 
                        VALUES (@interestValue, @leadDays, @vatValue, @accrualPeriod, @suspensionPeriod, @empId, NOW(), NOW())";

                            using (var insertCmd = new NpgsqlCommand(insertQuery, conn))
                            {
                                insertCmd.Parameters.AddWithValue("@interestValue", interestValue);
                                insertCmd.Parameters.AddWithValue("@leadDays", leadDays);
                                insertCmd.Parameters.AddWithValue("@vatValue", vatValue);
                                insertCmd.Parameters.AddWithValue("@accrualPeriod", accrualPeriod);
                                insertCmd.Parameters.AddWithValue("@suspensionPeriod", suspensionPeriod);
                                insertCmd.Parameters.AddWithValue("@empId", Convert.ToInt32(Session["bo_id"])); // Using session value for emp_id
                                insertCmd.ExecuteNonQuery();
                            }

                            Response.Write("<script>alert('Payment term added successfully!')</script>");
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


        //IMODIFY PALANG
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
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
                                "swal('Deleted!', 'Waste category deleted successfully!', 'success')", true);
                        }
                        else
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
                                "swal('Unsuccessful!', 'Waste category not found or not updated!', 'error')", true);
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



        protected void showPaymentTerm()
        {
            using (var conn = new NpgsqlConnection(con))
            {
                conn.Open();

                // Query to get all relevant fields from the PAYMENT_TERM table
                string query = "SELECT pt_lead_days, pt_interest, pt_tax, pt_accrual_period, pt_susp_period FROM PAYMENT_TERM";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Set the values for lead days, interest, VAT, accrual period, and suspension period
                            leaddays.Text = reader["pt_lead_days"] != DBNull.Value ? reader["pt_lead_days"].ToString() : "0";
                            interest.Text = reader["pt_interest"] != DBNull.Value ? reader["pt_interest"].ToString() : "0";
                            vat.Text = reader["pt_tax"] != DBNull.Value ? reader["pt_tax"].ToString() : "0";
                            acc_per.Text = reader["pt_accrual_period"] != DBNull.Value ? reader["pt_accrual_period"].ToString() : "0";
                            susp_per.Text = reader["pt_susp_period"] != DBNull.Value ? reader["pt_susp_period"].ToString() : "0";
                        }
                    }
                }
            }
        }






        // Update the new name or password of the employee account
        protected void UpdateWasteCategory(object sender, EventArgs e)
        {
            int id;
            if (!int.TryParse(txtbxID.Text, out id))
            {
                Response.Write("<script>alert('Invalid ID format.')</script>");
                return;
            }
            //int id = int.Parse(txtbxID.Text);
            string name = txtbxnewName.Text;
            string unit = txtbxnewUnit.Text;
            double limit = double.Parse(txtLimit.Text);
            decimal price = decimal.Parse(txtbxnewPrice.Text);

            using (var conn = new NpgsqlConnection(con))
            {
                conn.Open();
                string updateQuery = "UPDATE waste_category SET wc_name = @name, wc_unit = @unit, wc_price = @price, wc_max = @max WHERE wc_id = @id";

                using (var updateCmd = new NpgsqlCommand(updateQuery, conn))
                {
                    updateCmd.Parameters.AddWithValue("@name", name);
                    updateCmd.Parameters.AddWithValue("@unit", unit);
                    updateCmd.Parameters.AddWithValue("@price", price);
                    updateCmd.Parameters.AddWithValue("@id", id);
                    updateCmd.Parameters.AddWithValue("@max", limit);

                    updateCmd.ExecuteNonQuery();
                    showPaymentTerm();
                    WasteCatList();
                }
            }

            // Optionally refresh the grid or list showing waste categories
            // Response.Redirect(Request.RawUrl); // Example to refresh the page after update
            Response.Write("<script>alert('Waste category updated successfully!');</script>");
            showPaymentTerm();
            WasteCatList();
        }

    }
}