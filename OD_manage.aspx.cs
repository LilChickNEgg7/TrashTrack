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

namespace Capstone
{
    public partial class OD_manage : System.Web.UI.Page
    {
        private readonly string con = "Server=localhost;Port=5432;User Id=postgres;Password=123456;Database=trashtrack";

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    Vehicle_TypeDDL(); // Call the method to populate the dropdown
            //}
            if (!IsPostBack)
            {
                LoadHaulers();
                Vehicle_TypeDDL();
                VehicleGridView();
                ValidationSettings.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
                LoadProfile();
            }
            Page.MaintainScrollPositionOnPostBack = true;
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
                    // SQL query to filter based on vehicle ID or plate and specific vehicle status
                    cmd.CommandText = @"
                SELECT 
                    v.v_id, 
                    v.v_plate, 
                    vt.vtype_name, 
                    v.v_capacity, 
                    v.v_status,
                    v.v_created_at, 
                    v.v_updated_at, 
                    v.driver_id
                FROM vehicle v
                INNER JOIN vehicle_type vt ON v.v_typeid = vt.vtype_id
                WHERE v.driver_id IS NULL
                AND (v.v_status = 'Awaiting Driver' OR v.v_status = 'For Repair')  
                AND (v.v_id::text ILIKE '%' || @searchQuery || '%' 
                OR v.v_plate ILIKE '%' || @searchQuery || '%')
                ORDER BY v.v_created_at DESC";

                    cmd.Parameters.AddWithValue("@searchQuery", searchQuery);

                    DataTable admin_datatable = new DataTable();
                    NpgsqlDataAdapter admin_sda = new NpgsqlDataAdapter(cmd);
                    admin_sda.Fill(admin_datatable);

                    gridViewDispatcher.DataSource = admin_datatable;
                    gridViewDispatcher.DataBind();

                    if (admin_datatable.Rows.Count == 0)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "swal", "Swal.fire('No Search Found', 'Try again.', 'info');", true);
                    }
                }
            }
        }

        private void VehicleGridView()
        {
            using (var db = new NpgsqlConnection(con))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    // Filter vehicles where the v_status is 'Awaiting Driver' AND the driver_id is NULL
                    cmd.CommandText = @"
                SELECT 
                    v.v_id, 
                    v.v_plate, 
                    vt.vtype_name, 
                    v.v_capacity, 
                    v.v_status,
                    v.v_created_at, 
                    v.v_updated_at, 
                    v.driver_id
                FROM vehicle v
                INNER JOIN vehicle_type vt ON v.v_typeid = vt.vtype_id
                WHERE (v.v_status = 'Awaiting Driver' OR v.v_status = 'For Repair')  
                AND v.driver_id IS NULL  -- driver_id must be NULL
                ORDER BY v.v_created_at DESC";  // Most recent vehicles first

                    DataTable admin_datatable = new DataTable();
                    NpgsqlDataAdapter admin_sda = new NpgsqlDataAdapter(cmd);
                    admin_sda.Fill(admin_datatable);

                    // Bind the data to the GridView
                    gridViewDispatcher.DataSource = admin_datatable;
                    gridViewDispatcher.DataBind();

                }
            }
        }


        private void Vehicle_TypeDDL()
        {
            using (var db = new NpgsqlConnection(con))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandText = "SELECT VTYPE_ID, VTYPE_NAME FROM VEHICLE_TYPE";
                    NpgsqlDataAdapter VehicleNameAdapter = new NpgsqlDataAdapter(cmd);
                    DataTable VehicleName = new DataTable();
                    VehicleNameAdapter.Fill(VehicleName);

                    vehicle_type_ddl.DataSource = VehicleName;
                    vehicle_type_ddl.DataTextField = "VTYPE_NAME";
                    vehicle_type_ddl.DataValueField = "VTYPE_ID";
                    vehicle_type_ddl.DataBind();

                    // Add a default item
                    vehicle_type_ddl.Items.Insert(0, new ListItem("Select Truck Category", "0"));
                }
            }
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            int emp_id = (int)Session["od_id"];

            //int emp_id = 1007;  // Replace with dynamic employee ID if necessary
            string v_plate = vehicle_PLATES.Text.Trim(); // Trim whitespace
            string v_cap = vehicle_cap.Text.Trim(); // Trim whitespace
            string vehicleCapacityUnit = "TONS";    // Fixed value for vehicle capacity unit
            string v_type = vehicle_type_ddl.SelectedValue;

            // Validate inputs
            if (string.IsNullOrWhiteSpace(v_plate) || string.IsNullOrWhiteSpace(v_cap))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "swal", "Swal.fire('Error', 'Vehicle plate or vehicle capacity cannot be empty!', 'error');", true);
                return;
            }

            if (v_type == "0")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "swal", "Swal.fire('Error', 'Please select a valid vehicle type!', 'error');", true);
                return;
            }

            // Check if the plate number already exists
            using (var db = new NpgsqlConnection(con))
            {
                db.Open();
                using (var checkCmd = db.CreateCommand())
                {
                    checkCmd.CommandType = CommandType.Text;
                    checkCmd.CommandText = "SELECT COUNT(*) FROM VEHICLE WHERE V_PLATE = @v_plate";
                    checkCmd.Parameters.AddWithValue("@v_plate", v_plate);

                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (count > 0)
                    {
                        // Plate number already exists
                        ClientScript.RegisterStartupScript(this.GetType(), "swal", "Swal.fire('Warning', 'Vehicle plate number already exists!', 'warning');", true);
                        return;
                    }
                }

                // Proceed to insert the vehicle
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"
            INSERT INTO VEHICLE(V_PLATE, V_CAPACITY, V_CAPACITY_UNIT, V_TYPEID, EMP_ID)
            VALUES(@v_plate, @v_cap, @v_cap_unit, @v_typeid, @emp_id)";

                    cmd.Parameters.AddWithValue("@v_plate", v_plate);
                    cmd.Parameters.AddWithValue("@v_cap", Convert.ToInt32(v_cap));
                    cmd.Parameters.AddWithValue("@v_cap_unit", vehicleCapacityUnit);  // Always use 'TONS'
                    cmd.Parameters.AddWithValue("@v_typeid", Convert.ToInt32(v_type));
                    cmd.Parameters.AddWithValue("@emp_id", emp_id);

                    var ctr = cmd.ExecuteNonQuery();

                    if (ctr >= 1)
                    {
                        // Clear form fields
                        vehicle_PLATES.Text = "";
                        vehicle_cap.Text = "";
                        vehicle_type_ddl.SelectedIndex = 0;

                        // Show SweetAlert for successful registration
                        ClientScript.RegisterStartupScript(this.GetType(), "swal", "Swal.fire({title: 'Success', text: 'Vehicle Registered Successfully!', icon: 'success', confirmButtonColor: '#3085d6', cancelButtonColor: '#d33'});", true);

                        // Refresh the GridView to reflect changes
                        VehicleGridView();
                    }
                    else
                    {
                        // Handle failure case
                        ClientScript.RegisterStartupScript(this.GetType(), "swal", "Swal.fire('Error', 'Vehicle Registration Failed!', 'error');", true);
                    }
                }
            }
        }


        private void LoadHaulers()
        {
            // Clear existing items in the dropdown list
            ddlHauler.Items.Clear();

            // Modified query to load both haulers with driver_id IS NULL and driver_id IS NOT NULL
            string query = @"
    SELECT emp.emp_id, emp.emp_fname, emp.emp_mname, emp.emp_lname
    FROM employee emp
    WHERE emp.role_id = 4 
    AND (emp.emp_id NOT IN (SELECT v.driver_id FROM vehicle v WHERE v.driver_id IS NOT NULL)
    OR emp.emp_id IN (SELECT v.driver_id FROM vehicle v WHERE v.driver_id IS NOT NULL))";

            using (NpgsqlConnection conn = new NpgsqlConnection(con))
            {
                conn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                {
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Concatenate first, middle, and last names to create the full name
                            string fullName = $"{reader["emp_fname"]} {reader["emp_mname"]} {reader["emp_lname"]}".Trim();

                            // Create a new list item for each hauler and add it to the dropdown list
                            ListItem item = new ListItem(fullName, reader["emp_id"].ToString());
                            ddlHauler.Items.Add(item);
                        }
                    }
                }
            }

            // Insert a default item at the top of the dropdown list
            ddlHauler.Items.Insert(0, new ListItem("-- Select Hauler --", ""));
        }

        protected void btnAssignHauler_Click(object sender, EventArgs e)
        {
            int v_id = Convert.ToInt32(txtV_ID.Value);  // Get the selected vehicle ID
            string selected_hauler = ddlHauler.SelectedValue;  // Get the selected hauler ID

            if (string.IsNullOrEmpty(selected_hauler))
            {
                // Show SweetAlert warning if no hauler is selected
                ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
                    "Swal.fire({ icon: 'warning', title: 'No hauler selected', text: 'Please select a hauler before assigning.', confirmButtonColor: '#3085d6' });",
                    true);
                return;
            }

            // Convert selected hauler ID to integer
            int driverId = Convert.ToInt32(selected_hauler);

            using (var db = new NpgsqlConnection(con))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    // Start a transaction to ensure both updates happen together
                    using (var transaction = db.BeginTransaction())
                    {
                        try
                        {
                            // 1. Update the previous vehicle that the driver was assigned to (set driver_id = NULL and v_status = 'Awaiting Driver')
                            cmd.CommandText = @"
                        UPDATE vehicle 
                        SET driver_id = NULL, 
                            v_status = 'Awaiting Driver', 
                            driver_date_updated_at = NOW()  -- Current timestamp for updated_at
                        WHERE driver_id = @driver_id";  // Find the vehicle assigned to this driver
                            cmd.Parameters.AddWithValue("@driver_id", driverId);
                            cmd.ExecuteNonQuery(); // Execute the command to update the previous vehicle

                            // 2. Now, assign the new vehicle to the driver (set driver_id and v_status)
                            cmd.CommandText = @"
                        UPDATE vehicle 
                        SET driver_id = @driver_id, 
                            v_status = 'Assigned', 
                            driver_date_assigned_at = @assignedAt, 
                            driver_date_updated_at = @updatedAt
                        WHERE v_id = @v_id";  // Update the new vehicle
                            cmd.Parameters.AddWithValue("@driver_id", driverId);
                            cmd.Parameters.AddWithValue("@v_id", v_id);
                            cmd.Parameters.AddWithValue("@assignedAt", DateTime.Now);
                            cmd.Parameters.AddWithValue("@updatedAt", DateTime.Now);
                            cmd.ExecuteNonQuery(); // Execute the command to assign the new vehicle

                            // Commit the transaction
                            transaction.Commit();

                            // Success: Show SweetAlert message for successful assignment
                            ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
                                "Swal.fire({ icon: 'success', title: 'Hauler Assigned!', text: 'The hauler was successfully assigned to the new vehicle.', background: '#e9f7ef', confirmButtonColor: '#28a745' });",
                                true);

                            VehicleGridView(); // Refresh GridView to reflect changes
                        }
                        catch (Exception ex)
                        {
                            // If any error occurs, roll back the transaction
                            transaction.Rollback();
                            ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
                                "Swal.fire({ icon: 'error', title: 'Assignment Failed', text: 'An error occurred: " + ex.Message + "', confirmButtonColor: '#d33' });",
                                true);
                        }
                    }
                }
            }
        }



        private void DisplayVehicleName(int v_id)
        {
            try
            {
                using (var db = new NpgsqlConnection(con))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT v_plate FROM VEHICLE WHERE v_id = @v_id";
                        cmd.Parameters.AddWithValue("@v_id", v_id); // Ensure this matches the parameter name in the SQL command

                        var reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            txtbxVehiclePlate.Text = reader["v_plate"].ToString();
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

        protected void viewassignhauler_Click(object sender, EventArgs e)
        {
            // Cast the sender to LinkButton to get the button that was clicked
            LinkButton btn = sender as LinkButton;

            if (btn == null)
            {
                // Log or handle the case where the sender is not a LinkButton
                return;
            }

            // Check if CommandArgument is null
            if (btn.CommandArgument == null)
            {
                // Handle the case where CommandArgument is null
                return;
            }

            // Get the CommandArgument (which contains both vehicle ID and plate)
            string[] commandArgs = btn.CommandArgument.Split(';');

            // Ensure commandArgs has the expected number of elements
            if (commandArgs.Length < 2)
            {
                // Handle error: CommandArgument does not contain enough elements
                return;
            }

            // Parse vehicle ID and vehicle plate from commandArgs
            int vehicleId = Convert.ToInt32(commandArgs[0]); // Vehicle ID
            string vehiclePlate = commandArgs[1]; // Vehicle Plate
            txtV_ID.Value = vehicleId.ToString();
            // Set the Vehicle Plate in the textbox (disabled)
            txtbxVehiclePlate.Text = vehiclePlate; // Set the plate number in the textbox

            // Load haulers into the dropdown list
            LoadHaulers();

            // Show the modal popup
            ModalPopupExtender2.Show();
        }
        protected void btnAddCategory_Click(object sender, ImageClickEventArgs e)
        {
            // Open the remove vehicle modal with selected vehicle ID
            ImageButton btnAddCategory = (ImageButton)sender;
            ModalPopupExtender3.Show();
        }
        protected void addbtncategory_Click(object sender, EventArgs e)
        {
            // Get the value entered in the textbox (txtaddCategory)
            string categoryName = txtaddCategory.Text.Trim();  // Trim any leading or trailing whitespace

            // Check if the input is not empty
            if (!string.IsNullOrEmpty(categoryName))
            {
                using (var db = new NpgsqlConnection(con))
                {
                    db.Open();

                    // Check if the category already exists
                    using (var checkCmd = db.CreateCommand())
                    {
                        checkCmd.CommandText = "SELECT COUNT(*) FROM vehicle_type WHERE vtype_name = @vtype_name";
                        checkCmd.Parameters.AddWithValue("@vtype_name", categoryName);

                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (count > 0)
                        {
                            // Category already exists, show a message
                            ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
                                "Swal.fire({ icon: 'error', title: 'Duplicate Category', text: 'The category already exists.', background: '#f8d7da', confirmButtonColor: '#dc3545' });", true);
                            return; // Exit the method to prevent insertion
                        }
                    }

                    // Proceed with the insert if the category doesn't exist
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandText = "INSERT INTO vehicle_type (vtype_name) VALUES (@vtype_name)";
                        cmd.Parameters.AddWithValue("@vtype_name", categoryName);
                        cmd.ExecuteNonQuery();
                    }
                }

                // Show a success message after saving the category
                ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
                    "Swal.fire({ icon: 'success', title: 'New Category Saved!', text: 'Vehicle category added successfully..', background: '#e9f7ef', confirmButtonColor: '#28a745' });", true);
                Vehicle_TypeDDL();

            }
            else
            {
                // If the textbox is empty, show an error message
                ScriptManager.RegisterStartupScript(this, GetType(), "showAlert",
                    "Swal.fire({ icon: 'error', title: 'Error', text: 'Category name cannot be empty!', background: '#f8d7da', confirmButtonColor: '#dc3545' });", true);
            }
        }

    }
}