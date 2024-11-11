using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
                HistoryPaymentList();
                //PopulateWasteCategory();
                //LoadBookingWasteData();
                hfActiveTab.Value = "#tab1"; // Set Tab 1 as the default

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
                    cmd.CommandText = @"
                SELECT bk_id, bk_date, bk_status, bk_province, bk_city, bk_brgy, bk_street, bk_postal
                FROM booking WHERE bk_status != 'Completed'
                ORDER BY bk_id, bk_date";  // Sorting by date for recent bookings

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

        protected void HistoryBillList()
        {
            using (var db = new NpgsqlConnection(con))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    // Query to fetch booking data from the database
                    cmd.CommandText = @"SELECT * FROM generate_bill";  // Sorting by date for recent bookings
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


        protected void HistoryPaymentList()
        {
            using (var db = new NpgsqlConnection(con))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    // Query to fetch booking data from the database
                    cmd.CommandText = @"SELECT * FROM payment WHERE p_status = 'paid'";  // Sorting by date for recent bookings
                    // Execute the query and bind the results to the GridView
                    DataTable bookingsDataTable = new DataTable();
                    NpgsqlDataAdapter bookingsAdapter = new NpgsqlDataAdapter(cmd);
                    bookingsAdapter.Fill(bookingsDataTable);

                    // Bind the data to the GridView
                    gridView2.DataSource = bookingsDataTable;
                    gridView2.DataBind();
                }
                db.Close();
            }
        }


    }
}