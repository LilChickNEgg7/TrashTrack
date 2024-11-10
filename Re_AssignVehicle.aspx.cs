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
using MongoDB.Driver.Core.Configuration;
using System.Data.SqlClient;

namespace Capstone
{
    public partial class Re_AssignVehicle : System.Web.UI.Page
    {
        private readonly string con = "Server=localhost;Port=5432;User Id=postgres;Password=123456;Database=trashtrack";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                VehicleGridView(); // Populate the GridView.
                PopulateVehicleTypes(); // Populate the vehicle types dropdown in the modal.
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
        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            string searchQuery = txtSearch.Value.Trim();
            // Call your method to filter the GridView based on the search query
            FilterGridView(searchQuery);
        }

        private void FilterGridView(string searchQuery)
        {
            using (var db = new NpgsqlConnection(con))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandText = @"
                SELECT 
                    v.v_id,
                    v.driver_id, 
                    v.v_plate,
                    e.emp_fname || ' ' || e.emp_lname AS emp_name,
                    vt.vtype_name,
                    v.v_capacity, 
                    v.driver_date_assigned_at AS v_assigned_at, 
                    v.driver_date_updated_at AS v_updated_at
                FROM vehicle v
                LEFT JOIN employee e ON v.driver_id = e.emp_id  
                INNER JOIN vehicle_type vt ON v.v_typeid = vt.vtype_id  
                WHERE v.driver_id IS NOT NULL 
                AND (v.driver_id::text ILIKE '%' || @searchQuery || '%' OR 
                     v.v_plate ILIKE '%' || @searchQuery || '%' OR 
                     e.emp_lname ILIKE '%' || @searchQuery || '%' OR 
                     e.emp_fname ILIKE '%' || @searchQuery || '%')
                ORDER BY v_assigned_at DESC";

                    cmd.Parameters.AddWithValue("@searchQuery", searchQuery);

                    DataTable admin_datatable = new DataTable();
                    NpgsqlDataAdapter admin_sda = new NpgsqlDataAdapter(cmd);
                    admin_sda.Fill(admin_datatable);

                    gridViewDispatcher.DataSource = admin_datatable;
                    gridViewDispatcher.DataBind();

                    // Show SweetAlert if no results found
                    if (admin_datatable.Rows.Count == 0)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "swal", "Swal.fire('No Search Found', 'Try again.', 'info');", true);
                    }
                }
            }
        }



        // Populates the vehicle type dropdown (used in the modal).
        private void PopulateVehicleTypes()
        {
            using (var db = new NpgsqlConnection(con))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandText = "SELECT vtype_id, vtype_name FROM vehicle_type";
                    var reader = cmd.ExecuteReader();
                    ddlVehicle.DataSource = reader;
                    ddlVehicle.DataTextField = "vtype_name";
                    ddlVehicle.DataValueField = "vtype_id";
                    ddlVehicle.DataBind();
                }
            }
        }

        // Triggered when the vehicle type is selected.
        protected void ddlVehicle_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateVehiclePlates();
            txtVehicle_Cap.Text = "";
            ModalPopupExtender2.Show();
        }
        // Populate the vehicle plates dropdown based on the selected vehicle type and check for unassigned vehicles.
        private void PopulateVehiclePlates()
        {
            ddlvplate.Items.Clear();
            ddlvplate.Items.Add(new ListItem("Select Plate", "")); // Placeholder for the vehicle plate

            int selectedTypeId = int.Parse(ddlVehicle.SelectedValue);
            int currentDriverId = int.Parse(hiddenDriverId.Value); // Get the current driver ID
            int? currentVehicleId = null;

            // Get the currently assigned vehicle ID for the driver
            using (var db = new NpgsqlConnection(con))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandText = "SELECT v_id FROM vehicle WHERE driver_id = @currentDriverId";
                    cmd.Parameters.AddWithValue("@currentDriverId", currentDriverId);
                    currentVehicleId = (int?)cmd.ExecuteScalar();
                }
            }

            using (var db = new NpgsqlConnection(con))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    // Query only vehicles with the selected category that are not 'Unavailable' and not assigned to the driver
                    cmd.CommandText = @"
                SELECT v_id, v_plate, v_capacity 
                FROM vehicle 
                WHERE v_typeid = @typeId 
                AND v_status <> 'Unavailable' 
                AND v_id <> @currentVehicleId";
                    cmd.Parameters.AddWithValue("@typeId", selectedTypeId);
                    cmd.Parameters.AddWithValue("@currentVehicleId", currentVehicleId.HasValue ? (object)currentVehicleId.Value : DBNull.Value);

                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        // Populate vehicle plates dropdown
                        ddlvplate.Items.Add(new ListItem(reader["v_plate"].ToString(), reader["v_id"].ToString()));
                    }
                }
            }
        }

        // Triggered when a vehicle plate is selected.
        protected void ddlvplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedPlateId = ddlvplate.SelectedValue; // Get the selected value
            if (!string.IsNullOrEmpty(selectedPlateId))
            {
                LoadVehicleCapacity(selectedPlateId); // Load capacity based on the selected plate
            }
            else
            {
                txtVehicle_Cap.Text = ""; // Clear if no plate is selected
            }

            // Log the selected value for debugging
            System.Diagnostics.Debug.WriteLine("Selected Plate ID: " + selectedPlateId);

            // Keep the modal open after selecting a plate
            ModalPopupExtender2.Show();
        }


        // Loads the vehicle capacity into the disabled textbox.
        private void LoadVehicleCapacity(string vehicleId)
        {
            using (var db = new NpgsqlConnection(con))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandText = "SELECT v_capacity FROM vehicle WHERE v_id = @vehicleId";

                    // Convert vehicleId to an integer before passing it to the parameter
                    cmd.Parameters.AddWithValue("@vehicleId", Convert.ToInt32(vehicleId));

                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        txtVehicle_Cap.Text = result.ToString(); // Set the textbox with the capacity
                    }
                    else
                    {
                        txtVehicle_Cap.Text = ""; // Clear if no result
                    }
                }
            }
        }

        // Assign a vehicle to a driver.
        protected void btnAssignVehicle_Click(object sender, EventArgs e)
        {
            // Validate that a vehicle plate and driver ID are selected
            if (string.IsNullOrEmpty(ddlvplate.SelectedValue))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Please select a vehicle plate.');", true);
                return; // Exit the method if no vehicle is selected
            }

            if (string.IsNullOrEmpty(hiddenDriverId.Value))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Driver ID is invalid. Please try again.');", true);
                return; // Exit the method if driver ID is not valid
            }

            int newVehicleId = int.Parse(ddlvplate.SelectedValue);
            int newDriverId = int.Parse(hiddenDriverId.Value); // Get new driver_id from hidden field

            using (var db = new NpgsqlConnection(con))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    // First, update the old vehicle's driver_id to NULL and set v_status to 'Awaiting Driver'
                    cmd.CommandText = @"
                UPDATE vehicle 
                SET driver_id = NULL, 
                    v_status = 'Awaiting Driver' 
                WHERE driver_id = @newDriverId";

                    cmd.Parameters.AddWithValue("@newDriverId", newDriverId);
                    cmd.ExecuteNonQuery(); // Execute to remove the old assignment and update status

                    // Now, assign the new vehicle to the driver
                    cmd.CommandText = @"
                UPDATE vehicle 
                SET driver_id = @newDriverId, 
                    driver_date_assigned_at = @assignedAt, 
                    driver_date_updated_at = @updatedAt, 
                    v_status = 'Assigned' 
                WHERE v_id = @newVehicleId";

                    cmd.Parameters.AddWithValue("@newVehicleId", newVehicleId);
                    cmd.Parameters.AddWithValue("@assignedAt", DateTime.Now);
                    cmd.Parameters.AddWithValue("@updatedAt", DateTime.Now);

                    try
                    {
                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
                            "Swal.fire({ icon: 'success', title: 'Hauler Assigned!', text: 'Vehicle was successfully assigned.', background: '#e9f7ef', confirmButtonColor: '#28a745' });",
                            true);
                            VehicleGridView(); // Refresh the vehicle GridView
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
                           "Swal.fire({ icon: 'error', title: 'Assignment Failed', text: 'Unable to assign vehicle. Please try again.', confirmButtonColor: '#d33' });",
                           true);
                        }
                    }
                    catch (Exception ex)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error: " + ex.Message + "');", true);
                    }
                }
            }
        }


        // Populate GridView with vehicles that already have a driver assigned.
        private void VehicleGridView()
        {
            using (var db = new NpgsqlConnection(con))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandText = @"
                SELECT 
                    v.v_id,
                    v.driver_id, 
                    v.v_plate,
                    e.emp_fname || ' ' || e.emp_lname AS emp_name,
                    vt.vtype_name,
                    v.v_capacity, 
                    v.driver_date_assigned_at AS v_assigned_at, 
                    v.driver_date_updated_at AS v_updated_at
                FROM vehicle v
                LEFT JOIN employee e ON v.driver_id = e.emp_id  
                INNER JOIN vehicle_type vt ON v.v_typeid = vt.vtype_id  
                WHERE v.v_status <> 'Unavailable'  -- Exclude vehicles marked as unavailable.
                AND v.driver_id IS NOT NULL  -- Show only vehicles with drivers.
                ORDER BY v_assigned_at DESC";

                    DataTable admin_datatable = new DataTable();
                    NpgsqlDataAdapter admin_sda = new NpgsqlDataAdapter(cmd);
                    admin_sda.Fill(admin_datatable);

                    gridViewDispatcher.DataSource = admin_datatable;
                    gridViewDispatcher.DataBind();
                }
            }
        }


        // Opens the modal popup for vehicle reassignment.
        protected void btnAssign_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            string vehicleId = btn.CommandArgument;

            // Get the driver_id from the GridView row
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            string driverId = row.Cells[2].Text; // Use the correct index for the driver ID

            txtVehicle_ID.Value = vehicleId;
            hiddenDriverId.Value = driverId; // Store driver_id in a hidden field
            ModalPopupExtender2.Show();
        }
        protected void btnremove_Click(object sender, ImageClickEventArgs e)
        {
            // Open the remove vehicle modal with selected vehicle ID
            ImageButton btnremove = (ImageButton)sender;
            string drive_id = btnremove.CommandArgument;

            GridViewRow row = (GridViewRow)btnremove.NamingContainer;
            string driverId = row.Cells[2].Text; // Use the correct index for the driver ID

            vehicleID.Value = drive_id;
            haulerId.Value = driverId;

            // Fetch the vehicle plate to display in the modal
            using (var db = new NpgsqlConnection(con))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandText = @"
                SELECT v_plate
                FROM vehicle
                WHERE v_id = @v_id";

                    // Convert drive_id to an integer before adding to parameters
                    cmd.Parameters.AddWithValue("@v_id", int.Parse(drive_id));

                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        removeVehicle.Text = result.ToString(); // Set the textbox with the plate
                    }
                    else
                    {
                        removeVehicle.Text = ""; // Clear if no result
                    }
                }
            }

            ModalPopupExtender3.Show();
        }


        protected void btnremovevehicle_Click(object sender, EventArgs e)
        {
            int vehicleId = int.Parse(vehicleID.Value); // Get vehicle ID from hidden field

            using (var db = new NpgsqlConnection(con))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    // Update the vehicle status to 'Unavailable', clear driver_id, and update the driver_date_updated_at
                    cmd.CommandText = @"
                UPDATE vehicle
                SET v_status = 'Unavailable', 
                    driver_id = NULL,
                    driver_date_updated_at = NOW()  -- Set current timestamp for updated date
                WHERE v_id = @v_id";

                    cmd.Parameters.AddWithValue("@v_id", vehicleId);
                    cmd.ExecuteNonQuery(); // Execute the update command
                }
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
                            "Swal.fire({ icon: 'success', title: 'Vehicle Removed!', text: 'The vehicle was successfully removed.', background: '#e9f7ef', confirmButtonColor: '#28a745' });",
                            true);
            VehicleGridView(); // Refresh GridView data to reflect changes

        }


    }
}
