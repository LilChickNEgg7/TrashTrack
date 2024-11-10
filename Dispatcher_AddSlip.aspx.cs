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
using System;
using System.Reflection.Emit;


namespace Capstone
{
    public partial class Dispatcher_AddSlip : System.Web.UI.Page
    {
        private readonly string con = "Server=localhost;Port=5432;User Id=postgres;Password=123456;Database=trashtrack";
        protected void Page_Load(object sender, EventArgs e)
        {
            ValidationSettings.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
            if (!IsPostBack)
            {
                GridViewBooking();
                LoadProfile();  

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


     
        //private void GridViewOP()
        //{
        //    using (var db = new NpgsqlConnection(con))
        //    {
        //        db.Open();
        //        using (var cmd = db.CreateCommand())
        //        {
        //            cmd.CommandType = CommandType.Text;
        //            cmd.CommandText = "SELECT * FROM EMPLOYEE WHERE EMP_STATUS != 'Inactive' AND ROLE_ID = 5 ORDER BY emp_status = 'Active'";

        //            DataTable op_datatable = new DataTable();
        //            NpgsqlDataAdapter op_sda = new NpgsqlDataAdapter(cmd);
        //            op_sda.Fill(op_datatable);

        //            // Bind the DataTable to the GridView
        //            gridViewDispatcher.DataSource = op_datatable;
        //            gridViewDispatcher.DataBind();
        //        }
        //    }
        //}




        private void GridViewBooking()
        {
            using (var db = new NpgsqlConnection(con))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT bk_id, bk_date, bk_status, bk_waste_scale_slip, cus_id FROM booking ORDER BY bk_created_at DESC"; // Query to fetch relevant columns

                    DataTable booking_datatable = new DataTable();
                    NpgsqlDataAdapter booking_sda = new NpgsqlDataAdapter(cmd);
                    booking_sda.Fill(booking_datatable);

                    // Bind the DataTable to the GridView
                    gridViewBooking.DataSource = booking_datatable;
                    gridViewBooking.DataBind();
                }
            }
        }

        protected void btnadd_slip_Click(object sender, EventArgs e)
        {
            // Cast the sender back to LinkButton
            LinkButton btn = (LinkButton)sender;

            // Get the CommandArgument (bk_id)
            string bk_id = btn.CommandArgument;

            // Show the bk_id in an alert
            Response.Write("<script>alert('Button is clicked! Booking ID: " + bk_id + "');</script>");
        }
        protected void Update_Click(object sender, EventArgs e)
        {
            // Cast the sender to a LinkButton
            LinkButton btn = sender as LinkButton;

            // Get the Booking ID (bk_id) from the CommandArgument
            int bk_id = Convert.ToInt32(btn.CommandArgument);

            // Assign the bk_id to the TextBox inside the modal for display
            txtbxID.Text = bk_id.ToString();
            DisplayTruckSlip(bk_id);
            // Optionally, you can pre-fill other fields related to the booking here





            // Show the modal pop-up
            this.ModalPopupExtender2.Show();
        }

        private void DisplayTruckSlip(int bk_id)
        {
            int customer_id = 0;
            string province = "";
            string city = "";
            string barangay = "";
            string postal = "";
            string street = "";
            string book_status = "";

            try
            {
                using (var db = new NpgsqlConnection(con))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM BOOKING WHERE bk_id = @bk_id";
                        cmd.Parameters.AddWithValue("@bk_id", bk_id);

                        var reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            byte[] imageData = reader["bk_waste_scale_slip"] as byte[];
                            province = reader["bk_province"].ToString();
                            city = reader["bk_city"].ToString();
                            barangay = reader["bk_brgy"].ToString();
                            postal = reader["bk_postal"].ToString();
                            street = reader["bk_street"].ToString();
                            customer_id = Convert.ToInt32(reader["cus_id"]);
                            book_status = reader["bk_status"].ToString();

                            txtPROVINCE.Text = province;
                            txtCITY.Text = city;
                            txtSTREET.Text = street;
                            txtBRGY.Text = barangay;
                            txtPOSTAL.Text = postal;
                            txtCUSTOMERID.Text = customer_id.ToString();

                            // Check if image data is null or empty
                            if (imageData != null && imageData.Length > 0)
                            {
                                // Convert image data to base64 string
                                string base64String = Convert.ToBase64String(imageData);
                                string imageUrl = "data:image/png;base64," + base64String;

                                // Bind the image URL to the Image control
                                imagePreview.ImageUrl = imageUrl;
                            }
                            else
                            {
                                // Display a default or placeholder image if no truck slip exists
                                imagePreview.ImageUrl = "~/Pictures/blank_prof.png";
                            }

                            // Disable the Upload button if the booking is cancelled
                            if (book_status == "Cancelled" || book_status == "Failed")
                            {
                                btnAddTruckSlip.Enabled = false;  // Disable button if cancelled or failed
                            }
                            else
                            {
                                btnAddTruckSlip.Enabled = true;  // Enable button otherwise
                            }
                        }
                        else
                        {
                            // Display a default or placeholder image if no truck slip is found
                            imagePreview.ImageUrl = "~/Pictures/blank_prof.png";
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



        //protected void btnAddTruckSlip_Click(object sender, EventArgs e)
        //{
        //    string id = txtbxID.Text;
        //    int bk_id = Convert.ToInt32(id);

        //    // Check if a file is uploaded
        //    byte[] imageData = formFile.HasFile ? formFile.FileBytes : null;

        //    try
        //    {

        //        if (imageData == null)
        //        {
        //            // Display SweetAlert if no image is uploaded
        //            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
        //                "swal('No File!', 'Please upload a truck slip.', 'warning')", true);
        //            return; // Stop further execution
        //        }

        //        using (var db = new NpgsqlConnection(con))
        //        {
        //            db.Open();
        //            using (var cmd = db.CreateCommand())
        //            {
        //                cmd.CommandType = CommandType.Text;

        //                // Check if a new truck slip is uploaded or not
        //                if (imageData != null)
        //                {
        //                    // Updating truck slip for the given booking ID
        //                    cmd.CommandText = "UPDATE BOOKING SET truck_slip  = @truck_slip," +
        //                                      "BK_STATUS = 'Pending Payment' " +
        //                                      "WHERE bk_id = @bk_id";
        //                    cmd.Parameters.AddWithValue("@truck_slip", imageData);
        //                    cmd.Parameters.AddWithValue("@bk_id", bk_id);
        //                    ClientScript.RegisterStartupScript(this.GetType(), "swal", "Swal.fire({title: 'Success', text: 'Truck Slip Registered Successfully!', icon: 'success', confirmButtonColor: '#3085d6', cancelButtonColor: '#d33'});", true);
        //                }
        //                else
        //                {


        //                    ClientScript.RegisterStartupScript(this.GetType(), "swal", "Swal.fire('No File!', 'No new truck slip upload Registration Failed!', 'error');", true);
        //                    // Handle the case where no new image is uploaded (optional)
        //                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
        //                    //    "swal('No File!', 'No new truck slip uploaded.', 'warning')", true);

        //                }

        //                int result = cmd.ExecuteNonQuery();
        //                if (result >= 1)
        //                {
        //                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
        //                        "swal('Success!', 'Truck Slip Updated!', 'success')", true);

        //                    // Refresh the GridView if needed
        //                    GridViewBooking();
        //                }
        //                else
        //                {
        //                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
        //                        "swal('Failed!', 'Truck Slip Update Failed!', 'error')", true);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
        //                "swal('Unsuccessful!', '" + ex.Message + "', 'error')", true);
        //    }
        //}


        protected void btnAddTruckSlip_Click(object sender, EventArgs e)
        {
            string id = txtbxID.Text;
            int bk_id = Convert.ToInt32(id);

            // Check if a file is uploaded
            byte[] imageData = formFile.HasFile ? formFile.FileBytes : null;

            try
            {
                if (imageData == null)
                {
                    // Display SweetAlert if no image is uploaded
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
                    //    "swal('No File!', 'Please upload a truck slip.', 'warning')", true);

                    ClientScript.RegisterStartupScript(this.GetType(), "swal", "Swal.fire('Alert', 'Please upload a truck slip!', 'error');", true);

                    return; // Stop further execution
                }

                using (var db = new NpgsqlConnection(con))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;

                        // Updating truck slip for the given booking ID
                        cmd.CommandText = "UPDATE BOOKING SET bk_waste_scale_slip  = @truck_slip," +
                                          "BK_STATUS = 'Pending Payment' " +
                                          "WHERE bk_id = @bk_id";
                        cmd.Parameters.AddWithValue("@truck_slip", imageData);
                        cmd.Parameters.AddWithValue("@bk_id", bk_id);

                        int result = cmd.ExecuteNonQuery();
                        if (result >= 1)
                        {
                            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
                            //    "swal('Success!', 'Truck Slip Updated!', 'success')", true);


                            ClientScript.RegisterStartupScript(this.GetType(), "swal", "Swal.fire({title: 'Success', text: 'Truck Slip Updated!', icon: 'success', confirmButtonColor: '#3085d6', cancelButtonColor: '#d33'});", true);


                            // Refresh the GridView if needed
                            GridViewBooking();
                        }
                        else
                        {
                            //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
                            //    "swal('Failed!', 'Truck Slip Update Failed!', 'error')", true);

                            ClientScript.RegisterStartupScript(this.GetType(), "swal", "Swal.fire('Failed!', 'Truck Slip Update Failed!', 'error');", true);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
                //    "swal('Unsuccessful!', '" + ex.Message + "', 'error')", true);

                //ClientScript.RegisterStartupScript(this.GetType(), "swal", "Swal.fire('Failed!', 'Truck Slip Update Failed!', 'error');", true);

                ClientScript.RegisterStartupScript(this.GetType(), "swal",
    "Swal.fire('Failed!', '" + ex.Message + "', 'error');", true);


            }
        }



    }
}