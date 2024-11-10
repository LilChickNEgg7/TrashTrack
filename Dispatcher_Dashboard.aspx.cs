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
using System.Web.UI.HtmlControls;
using AjaxControlToolkit;
using Newtonsoft.Json;
using System.Drawing;

namespace Capstone
{
    public partial class Dispatcher_Dashboard : System.Web.UI.Page
    {
        private readonly string con = "Server=localhost;Port=5432;User Id=postgres;Password=123456;Database=trashtrack";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDashboardData();
                RetrieveVehicleAvailability();
                LoadDispatcherData();
                LoadHaulerData();
                LoadCustomerData();
                VehicleAvailability();
                LoadProfile();
            }

            else
            {
                // Update pie chart data every time the modal opens
                RetrieveVehicleAvailability();
                Page.MaintainScrollPositionOnPostBack = true;
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
                if (Session["od_id"] == null)
                {
                    // Session expired or not set, redirect to login
                    Response.Redirect("Login.aspx");
                    return;
                }

                int adminId = (int)Session["od_id"];  // Retrieve admin ID from session
                string roleName = (string)Session["od_rolename"];


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
        private void LoadDispatcherData()
        {
            using (var db = new NpgsqlConnection(con))
            {
                db.Open();

                // Query to retrieve dispatcher data (role_id = 5 corresponds to Dispatchers)
                string dispatcherQuery = @"
        SELECT e.emp_id,
               CONCAT(e.emp_fname, ' ', COALESCE(e.emp_mname, ''), ' ', e.emp_lname) AS emp_name,
               e.emp_contact,
               e.emp_address,
               e.emp_profile,
               e.emp_status
        FROM employee e
        WHERE e.role_id = 5;"; // Dispatcher role_id = 5

                using (var cmd = new NpgsqlCommand(dispatcherQuery, db))
                using (var reader = cmd.ExecuteReader())
                {
                    var dispatcherTable = new DataTable();
                    dispatcherTable.Load(reader);

                    // Add a new column for the processed profile image URL
                    dispatcherTable.Columns.Add("profile_image", typeof(string));

                    // Process each row to set the 'profile_image' column using GetProfileImage
                    foreach (DataRow row in dispatcherTable.Rows)
                    {
                        row["profile_image"] = GetProfileImage(row["emp_profile"]);
                    }

                    // Bind the processed data to the GridView
                    gridViewDispatcher.DataSource = dispatcherTable;
                    gridViewDispatcher.DataBind();
                }
            }
        }





        //Original LoadHaulerData
        //private void LoadHaulerData()
        //{
        //    using (var db = new NpgsqlConnection(con))
        //    {
        //        db.Open();

        //        // Query to retrieve dispatcher data
        //        string dispatcherQuery = @"
        //        SELECT e.emp_id,
        //               CONCAT(e.emp_fname, ' ', COALESCE(e.emp_mname, ''), ' ', e.emp_lname) AS emp_name,
        //               e.emp_contact,
        //               e.emp_address,
        //               e.emp_profile,
        //               e.emp_status
        //        FROM employee e
        //        WHERE e.role_id = 4;"; // Assuming role_id 5 corresponds to Dispatchers

        //        using (var cmd = new NpgsqlCommand(dispatcherQuery, db))
        //        using (var reader = cmd.ExecuteReader())
        //        {
        //            var dispatcherTable = new DataTable();
        //            dispatcherTable.Load(reader);
        //            gridViewHauler.DataSource = dispatcherTable;
        //            gridViewHauler.DataBind();
        //        }
        //    }
        //}
        protected string GetProfileImage(object profileImage)
        {
            // Check if profileImage is null or empty
            if (profileImage == DBNull.Value || profileImage == null)
            {
                return "Pictures/no-image.png"; // Return default image path if profile is empty
            }

            // Otherwise, return the actual profile image URL (this can be a path or base64 string if stored as bytea)
            return "data:image/png;base64," + Convert.ToBase64String((byte[])profileImage);
        }
        protected void LoadHaulerData()
        {
            // Ensure your PostgreSQL connection string is set correctly
            //string con = "your_connection_string_here"; // Set your DB connection string here

            using (var db = new NpgsqlConnection(con))
            {
                db.Open();

                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;

                    // Using PostgreSQL string concatenation operator (||) for emp_fname and emp_lname
                    cmd.CommandText = "SELECT emp_id, emp_fname || ' ' || emp_lname AS emp_name, emp_contact, emp_address, emp_profile, emp_status " +
                                      "FROM employee WHERE role_id = 4";

                    // Use NpgsqlDataAdapter to fill the DataTable
                    NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    // Bind the DataTable to the GridView
                    gridViewHauler.DataSource = dt;
                    gridViewHauler.DataBind();
                }
            }
        }


        //Original LoadCustomerData
        //private void LoadCustomerData()
        //{
        //    using (var db = new NpgsqlConnection(con))
        //    {
        //        db.Open();

        //        // Query to retrieve dispatcher data
        //        string dispatcherQuery = @"
        //        SELECT c.cus_id,
        //               CONCAT(c.cus_fname, ' ', COALESCE(c.cus_mname, ''), ' ', c.cus_lname) AS cus_name,
        //               c.cus_contact,
        //               c.cus_address,
        //               c.cus_profile,
        //               c.cus_status
        //        FROM customer c;";

        //        using (var cmd = new NpgsqlCommand(dispatcherQuery, db))
        //        using (var reader = cmd.ExecuteReader())
        //        {
        //            var dispatcherTable = new DataTable();
        //            dispatcherTable.Load(reader);
        //            gridViewCustomer.DataSource = dispatcherTable;
        //            gridViewCustomer.DataBind();
        //        }
        //    }
        //}


        private void LoadCustomerData()
        {
            using (var db = new NpgsqlConnection(con))
            {
                db.Open();

                // Query to retrieve customer data
                string customerQuery = @"
        SELECT c.cus_id,
               CONCAT(c.cus_fname, ' ', COALESCE(c.cus_mname, ''), ' ', c.cus_lname) AS cus_name,
               c.cus_contact,
               c.cus_address,
               c.cus_profile,
               c.cus_status
        FROM customer c;";

                using (var cmd = new NpgsqlCommand(customerQuery, db))
                using (var reader = cmd.ExecuteReader())
                {
                    var customerTable = new DataTable();
                    customerTable.Load(reader);

                    // Apply GetProfileImage to the cus_profile column
                    customerTable.Columns.Add("profile_image", typeof(string)); // Add new column for processed image

                    foreach (DataRow row in customerTable.Rows)
                    {
                        // Apply GetProfileImage to the cus_profile data
                        row["profile_image"] = GetProfileImage(row["cus_profile"]);
                    }

                    // Bind the processed data to the GridView
                    gridViewCustomer.DataSource = customerTable;
                    gridViewCustomer.DataBind();
                }
            }
        }

        private void BindDashboardData()
        {
            try
            {
                using (var db = new NpgsqlConnection(con))
                {
                    db.Open();

                    // Retrieve total haulers based on distinct driver_id in the vehicle table
                    using (var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM employee WHERE role_id = 4", db))
                    {
                        int totalHaulers = Convert.ToInt32(cmd.ExecuteScalar());
                        totalhauler.Text = totalHaulers.ToString();
                    }

                    // Retrieve total vehicles based on v_id
                    using (var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM vehicle", db)) // Count all v_id
                    {
                        int totalVehicles = Convert.ToInt32(cmd.ExecuteScalar());
                        totalvehicle.Text = totalVehicles.ToString();
                    }

                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
            }
        }


        private void VehicleAvailability()
        {
            var vehicleAvailabilityData = RetrieveVehicleAvailability();

            // Use a Literal control to inject the JSON data into the page
            vehicleAvailabilityLiteral.Text = vehicleAvailabilityData;
        }

        private string RetrieveVehicleAvailability()
        {
            using (var db = new NpgsqlConnection(con))
            {
                db.Open();

                string query = @"
       SELECT vt.vtype_name, COUNT(v.v_typeid) AS vehicle_count
            FROM vehicle v
            JOIN vehicle_type vt ON v.v_typeid = vt.vtype_id
            WHERE v.driver_id IS NULL
            GROUP BY vt.vtype_name
            ORDER BY vt.vtype_name;";

                using (var cmd = new NpgsqlCommand(query, db))
                using (var reader = cmd.ExecuteReader())
                {
                    List<string> labels = new List<string>();
                    List<int> data = new List<int>();

                    while (reader.Read())
                    {
                        labels.Add(reader.GetString(0));  // Vehicle type name (vtype_name)
                        data.Add(reader.GetInt32(1));     // Vehicle count
                    }

                    // Serialize the data into a JSON string
                    var vehicleData = new { labels, data };
                    return JsonConvert.SerializeObject(vehicleData);  // Serialize to JSON string
                }
            }
        }


        protected void imgBtnDispatcher_Click(object sender, ImageClickEventArgs e)
        {
            LoadDispatcherData();
            ModalPopupExtender1.Show();
        }
        protected void imgBtnHauler_Click(object sender, ImageClickEventArgs e)
        {
            LoadHaulerData();
            ModalPopupExtender2.Show();
        }

        protected void imgBtnCustomer_Click(object sender, ImageClickEventArgs e)
        {
            LoadCustomerData();
            ModalPopupExtender3.Show();

        }
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            ModalPopupExtender3.Hide();
            ModalPopupExtender2.Hide();
            ModalPopupExtender1.Show();
            //RetrieveVehicleAvailability();
        }

        protected void LinkButton2_Click(object sender, EventArgs e)
        {

            ModalPopupExtender1.Hide();
            ModalPopupExtender2.Show();
            ModalPopupExtender3.Hide();
            //RetrieveVehicleAvailability();

        }
        protected void LinkButton3_Click(object sender, EventArgs e)
        {

            ModalPopupExtender1.Hide();
            ModalPopupExtender2.Hide();
            ModalPopupExtender3.Show();
            //RetrieveVehicleAvailability();

        }



        protected void btnClose_Click(object sender, EventArgs e)
        {

            ModalPopupExtender1.Hide();
        }

        protected void btnClose1_Click(object sender, EventArgs e)
        {

            ModalPopupExtender2.Hide();
        }
        protected void btnClose2_Click(object sender, EventArgs e)
        {

            ModalPopupExtender3.Hide();
        }


    }
}