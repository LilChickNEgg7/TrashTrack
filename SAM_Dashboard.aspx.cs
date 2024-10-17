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
                //TotalAMCount();
                //TotalCusCount();
                //ActiveAMCount();
                //ActiveCusCount();
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

                    // Bind to labels
                    totalAM.Text = totalAccountManager.ToString();
                    activeAM.Text = activeAccountManager.ToString();
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

                    // Bind to labels
                    totalSAM.Text = totalSAMcount.ToString();
                    activeSAM.Text = activeSAMcount.ToString();
                }
            }
        }


        //protected void ActiveCusCount()
        //{
        //    using (var db = new NpgsqlConnection(con))
        //    {
        //        db.Open();
        //        using (var cmd = db.CreateCommand())
        //        {
        //            cmd.CommandType = CommandType.Text;
        //            // Query to count active admins
        //            cmd.CommandText = "SELECT COUNT(*) FROM customer WHERE cus_status = 'Active'";

        //            // Execute the query to get the count of admin users
        //            int totalAdmins = Convert.ToInt32(cmd.ExecuteScalar());

        //            // Display the count in the Label control
        //            //activeCus.Text = totalAdmins.ToString();
        //            totalcustomer2.Text = totalAdmins.ToString();
        //        }
        //        db.Close();
        //    }
        //}



    }
}