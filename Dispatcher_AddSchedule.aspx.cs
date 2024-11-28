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
    public partial class Dispatcher_AddSchedule : System.Web.UI.Page
    {
        private readonly string con = "Server=localhost;Port=5432;User Id=postgres;Password=123456;Database=trashtrack";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadBookingLimitValues();
                ValidationSettings.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
                StartandStopBTNEnabling();
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

        private void LoadBookingLimitValues()
        {
            try
            {
                using (var db = new NpgsqlConnection(con))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT BL_MAX_DAY, BL_MAX_COUNT, BL_CLOSE_TIME, BL_DATE_LAST, BL_STOP " +
                                          "FROM BOOKING_LIMIT WHERE BL_ID = 2";  // Adjust the WHERE clause as needed

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Populate the form fields
                                maxdaylimit.Text = reader["BL_MAX_DAY"].ToString();
                                maxcount.Text = reader["BL_MAX_COUNT"].ToString();

                                int closingTimeInMinutes = int.Parse(reader["BL_CLOSE_TIME"].ToString());
                                TimeSpan closingTimeValue = TimeSpan.FromMinutes(closingTimeInMinutes);
                                closingtime.Text = closingTimeValue.ToString(@"hh\:mm"); // Format as "HH:mm"

                                DateTime lastDate = DateTime.Parse(reader["BL_DATE_LAST"].ToString());
                                datelast.Text = lastDate.ToString("yyyy-MM-dd");

                                bool bl_stop_value = reader.GetBoolean(reader.GetOrdinal("BL_STOP"));

                                // Update the booking status text
                                if (!bl_stop_value)  // Booking is active
                                {
                                    bkstatusstartorstop.Text = "Booking Started";
                                }
                                else  // Booking is stopped
                                {
                                    bkstatusstartorstop.Text = "Booking Stopped";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "swal", "Swal.fire('Error', '" + ex.Message + "', 'error');", true);
            }
        }

        private void StartandStopBTNEnabling()
        {
            try
            {
                using (var db = new NpgsqlConnection(con))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT BL_STOP FROM BOOKING_LIMIT WHERE BL_ID = 2";

                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                bool bl_stop_value = reader.GetBoolean(reader.GetOrdinal("BL_STOP"));

                                // Enable/disable buttons based on BL_STOP value
                                if (!bl_stop_value)
                                {
                                    // Booking is active, disable start button and enable stop button
                                    startbkbtn.Enabled = false;
                                    stopbkbtn.Enabled = true;

                                    // Ensure proper CSS classes for both buttons
                                    //startbkbtn.CssClass = "btn btn-primary disabled"; // Keeping the style for disabled state
                                    //stopbkbtn.CssClass = "btn btn-danger";             // Active stop button
                                }
                                else
                                {
                                    // Booking is stopped, enable start button and disable stop button
                                    startbkbtn.Enabled = true;
                                    stopbkbtn.Enabled = false;

                                    // Ensure proper CSS classes for both buttons
                                    //startbkbtn.CssClass = "btn btn-primary";           // Active start button
                                    //stopbkbtn.CssClass = "btn btn-danger disabled";    // Keeping the style for disabled state
                                }
                            }
                            else
                            {
                                Response.Write("No data found for BL_ID = ");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write($"Error: {ex.Message}");
            }
        }


        protected void startbkbtn_Click(object sender, EventArgs e)
        {
            try
            {
                using (var db = new NpgsqlConnection(con))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        // Set BL_STOP to false (booking active)
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "UPDATE BOOKING_LIMIT SET BL_STOP = false WHERE BL_ID = 2";

                        var ctr = cmd.ExecuteNonQuery();
                        if (ctr >= 1)
                        {
                            startbkbtn.Enabled = false;  // Disable start button
                            //startbkbtn.CssClass = "btn btn-primary disabled"; // Ensure consistent styling
                            stopbkbtn.Enabled = true;    // Enable stop button
                            //stopbkbtn.CssClass = "btn btn-danger"; // Ensure consistent styling
                            LoadBookingLimitValues();

                            // Success message using SweetAlert
                            ClientScript.RegisterStartupScript(this.GetType(), "swal", "Swal.fire({title: 'Booking Started!', text: 'Booking has been successfully started.', icon: 'success', confirmButtonColor: '#3085d6'});", true);
                        }
                        else
                        {
                            LoadBookingLimitValues();

                            // Error message using SweetAlert
                            ClientScript.RegisterStartupScript(this.GetType(), "swal", "Swal.fire('Failed!', 'Failed to start booking!', 'error');", true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Error message with exception details
                ClientScript.RegisterStartupScript(this.GetType(), "swal", $"Swal.fire('Error!', 'Failed to start booking. Problem: {ex.Message}', 'error');", true);
            }
        }

        protected void stopbkbtn_Click(object sender, EventArgs e)
        {
            try
            {
                using (var db = new NpgsqlConnection(con))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        // Set BL_STOP to true (booking stopped)
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "UPDATE BOOKING_LIMIT SET BL_STOP = true WHERE BL_ID = 2";

                        var ctr = cmd.ExecuteNonQuery();
                        if (ctr >= 1)
                        {
                            stopbkbtn.Enabled = false;   // Disable stop button
                            //stopbkbtn.CssClass = "btn btn-danger disabled"; // Ensure consistent styling
                            startbkbtn.Enabled = true;   // Enable start button
                            startbkbtn.CssClass = "btn btn-primary"; // Ensure consistent styling
                            LoadBookingLimitValues();

                            // Success message using SweetAlert
                            ClientScript.RegisterStartupScript(this.GetType(), "swal", "Swal.fire({title: 'Booking Stopped!', text: 'Booking has been successfully stopped.', icon: 'success', confirmButtonColor: '#3085d6'});", true);
                        }
                        else
                        {
                            LoadBookingLimitValues();

                            // Error message using SweetAlert
                            ClientScript.RegisterStartupScript(this.GetType(), "swal", "Swal.fire('Failed!', 'Failed to stop booking!', 'error');", true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Error message with exception details
                ClientScript.RegisterStartupScript(this.GetType(), "swal", $"Swal.fire('Error!', 'Failed to stop booking. Problem: {ex.Message}', 'error');", true);
            }
        }



        //private void LoadProfile()
        //{
        //    //imagePreview.

        //    try
        //    {
        //        //if (Session["emp_id"] == null)
        //        //{
        //        //    // Session expired or not set, redirect to login
        //        //    Response.Redirect("Login.aspx");
        //        //    return;
        //        //}

        //        /*int empID = (int)Session["emp_id"]; */ // Retrieve admin ID from session


        //        //As if naa sulod
        //        int empID = 1026;
        //        byte[] imageData = null;  // Initialize imageData

        //        // Define the PostgreSQL connection
        //        using (var db = new NpgsqlConnection(con))
        //        {
        //            db.Open();

        //            // PostgreSQL query to get profile image
        //            string query = "SELECT emp_profile FROM EMPLOYEE WHERE emp_id = @id";
        //            using (var cmd = new NpgsqlCommand(query, db))
        //            {
        //                // Set the parameter for admin ID
        //                cmd.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Integer, empID);

        //                // Execute the query and retrieve the image data
        //                var result = cmd.ExecuteScalar();

        //                if (result != null && result != DBNull.Value)
        //                {
        //                    imageData = (byte[])result;  // Cast the result to byte array
        //                }
        //            }
        //        }

        //        int role_id = (int)Session["role_id"];
        //        using (var db2 = new NpgsqlConnection(con))
        //        {
        //            db2.Open();
        //            using (var cmd2 = db2.CreateCommand())
        //            {
        //                cmd2.CommandType = CommandType.Text;
        //                cmd2.CommandText = "SELECT ROLE_NAME FROM ROLES WHERE ROLE_ID = @role_id ";

        //                cmd2.Parameters.AddWithValue("@role_id", role_id);
        //                using (var reader2 = cmd2.ExecuteReader())
        //                {
        //                    if (reader2.Read())
        //                    {
        //                        Employee_Role.Text = reader2["ROLE_NAME"].ToString();
        //                    }
        //                }
        //            }
        //        }


        //        // Check if the profile_image control exists and is not null
        //        if (profile_image != null)
        //        {
        //            if (imageData != null && imageData.Length > 0)
        //            {
        //                string base64String = Convert.ToBase64String(imageData);
        //                profile_image.ImageUrl = "data:image/jpeg;base64," + base64String;  // Set image as base64 string
        //                //imagePreviewUpdate.ImageUrl = "data:image/jpeg;base64," + base64String;
        //            }
        //            else
        //            {
        //                profile_image.ImageUrl = "~/Pictures/blank_prof.png";  // Default image if no profile picture found
        //            }
        //        }
        //        else
        //        {
        //            Response.Write("<script>alert('Profile image control is not found');</script>");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception or handle it
        //        Response.Write("<script>alert('Error loading profile image: " + ex.Message + "');</script>");
        //        profile_image.ImageUrl = "~/Pictures/blank_prof.png";  // Fallback in case of an error
        //    }
        //}


        protected void schedulebooking_Click(object sender, EventArgs e)
        {
            //Response.Write("<script>alert('BUTTON IS CLICKED!');</script>");
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


                /*int emp_id = 1009;*/  // Example emp_id value

                int emp_id = adminId;

                // Get values from the input fields
                string maxSchedule = max_schedule.Text;
                string maxCount = max_count.Text;
                string closingTime = closing_time.Text;
                string endDate = end_date.Text;

                // Convert values to the appropriate types
                int maxScheduleValue = int.Parse(maxSchedule);
                int maxCountValue = int.Parse(maxCount);

                // Convert closingTime to integer assuming it represents hours or minutes.
                TimeSpan closingTimeValue = TimeSpan.Parse(closingTime); // Use TimeSpan for "HH:mm" format
                int closingTimeInMinutes = (int)closingTimeValue.TotalMinutes;  // Store as total minutes

                // Parse dates with a known format, assuming yyyy-MM-dd
                DateTime endDateValue;

                // Using TryParseExact to ensure format
                if (!DateTime.TryParseExact(endDate, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out endDateValue))
                {
                    throw new FormatException("End Date is not in the correct format.");
                }

                using (var db = new NpgsqlConnection(con))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;

                        // Update statement without start date
                        cmd.CommandText = "UPDATE BOOKING_LIMIT " +
                                          "SET BL_MAX_DAY = @BLMAXDAY, " +
                                          "BL_MAX_COUNT = @BLMAXCOUNT, " +
                                          "BL_CLOSE_TIME = @BLCLOSETIME, " +
                                          "BL_DATE_LAST = @BLDATELAST, " +  // End Date (last date) field
                                          "EMP_ID = @empid " +
                                          "WHERE BL_ID = 2";  // Adjust BL_ID dynamically if necessary

                        // Bind the parameters with the appropriate values
                        cmd.Parameters.AddWithValue("@blmaxday", maxScheduleValue);
                        cmd.Parameters.AddWithValue("@blmaxcount", maxCountValue);
                        cmd.Parameters.AddWithValue("@blclosetime", closingTimeInMinutes);
                        cmd.Parameters.AddWithValue("@bldatelast", endDateValue);  // End Date (last date)
                        cmd.Parameters.AddWithValue("@empid", emp_id);

                        // Execute the command
                        var ctr = cmd.ExecuteNonQuery();

                        if (ctr >= 1)
                        {

                            Response.Write("<script>alert('BOOKING SCHEDULE HAS BEEN SET!');</script>");
                            ClientScript.RegisterStartupScript(this.GetType(), "swal", "Swal.fire({title: 'Success', text: 'Booking Limit Scheduled Successfully!', icon: 'success', confirmButtonColor: '#3085d6', cancelButtonColor: '#d33'});", true);



                            // Clear the input fields
                            max_schedule.Text = "";
                            max_count.Text = "";
                            closing_time.Text = "";
                            end_date.Text = "";

                            // Reload the updated booking limit values into the input fields
                            LoadBookingLimitValues();
                        }
                        else
                        {

                            Response.Write("<script>alert('BUTTON IS 12312321!');</script>");
                            ClientScript.RegisterStartupScript(this.GetType(), "swal", "Swal.fire('Failed!', 'Failed to Schedule Booking Limit!', 'error');", true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                Response.Write("<script>alert('Failed!', '" + ex.Message + "', 'error');</script>");
                ClientScript.RegisterStartupScript(this.GetType(), "swal", "Swal.fire('Failed!', '" + ex.Message + "', 'error');", true);
            }
        }







        //protected void meowbtn_Click(object sender, EventArgs e)
        //{
        //    //Response.Write("<script>alert('MEOW');</script>");
        //    //ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('MEOW');", true);

        //    try
        //    {
        //        using (var db = new NpgsqlConnection(con))
        //        {
        //            db.Open();
        //            using (var cmd = db.CreateCommand())
        //            {
        //                // Set BL_STOP to false when starting booking (booking active)
        //                cmd.CommandType = CommandType.Text;
        //                cmd.CommandText = "UPDATE BOOKING_LIMIT SET BL_STOP = true WHERE BL_ID = 1";

        //                var ctr = cmd.ExecuteNonQuery();
        //                if (ctr >= 1)
        //                {
        //                    meowbtn.Enabled = false;  // Disable start button
        //                    stopmowbtn.Enabled = true;    // Enable stop button
        //                    LoadBookingLimitValues();
        //                    Response.Write("<script>alert('Booking Started!');</script>");
        //                }
        //                else
        //                {
        //                    meowbtn.Enabled = true;   // Keep start button enabled
        //                    stopmowbtn.Enabled = false;   // Keep stop button disabled
        //                    LoadBookingLimitValues();
        //                    Response.Write("<script>alert('Failed to start booking!');</script>");
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.Write("<script>alert('Failed to start Booking. Problem: " + ex.Message + "');</script>");
        //    }

        //}

        //protected void stopmowbtn_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        using (var db = new NpgsqlConnection(con))
        //        {
        //            db.Open();
        //            using (var cmd = db.CreateCommand())
        //            {
        //                // Set BL_STOP to true when stopping booking (booking stopped)
        //                cmd.CommandType = CommandType.Text;
        //                cmd.CommandText = "UPDATE BOOKING_LIMIT SET BL_STOP = false WHERE BL_ID = 1";

        //                var ctr = cmd.ExecuteNonQuery();
        //                if (ctr >= 1)
        //                {
        //                    stopmowbtn.Enabled = false;   // Disable stop button
        //                    meowbtn.Enabled = true;   // Enable start button
        //                    LoadBookingLimitValues();
        //                    Response.Write("<script>alert('Booking Stopped!');</script>");
        //                }
        //                else
        //                {
        //                    stopmowbtn.Enabled = true;    // Keep stop button enabled
        //                    meowbtn.Enabled = false;  // Keep start button disabled
        //                    LoadBookingLimitValues();
        //                    Response.Write("<script>alert('Failed to stop booking!');</script>");
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.Write("<script>alert('Failed to stop Booking. Problem: " + ex.Message + "');</script>");
        //    }
        //}
    }
}