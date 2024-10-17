using MongoDB.Bson;
using MongoDB.Driver;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;



using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Npgsql;


namespace Capstone
{
    public partial class BO_Billing : System.Web.UI.Page
    {
        string connString = "mongodb+srv://admin1:V1ncent123@cluster0.pbtzqh3.mongodb.net/";
        private readonly string con = "Server=localhost;Port=5432;User Id=postgres;Password=123456;Database=trashtrack";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadProfile();
                LoadBookingList();
                //LoadCustomerBill();
                //LoadProfileImage();
                PopulateWasteCategories();
                UpdateOverdueBills();
                //PopulateWasteCategories();
                //BindWasteCategories();
                //UpdateInterestAndTotalAmountForAllCustomers();

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
                FROM public.booking
                ORDER BY bk_date DESC";  // Sorting by date for recent bookings

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


        //private void LoadCustomerBill()
        //{
        //    DataTable dt = RetrieveDataFromMongoDB();
        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        gridViewCustomers.DataSource = dt;
        //        gridViewCustomers.DataBind();
        //    }
        //}

        private DataTable RetrieveDataFromMongoDB()
        {
            var client = new MongoClient(connString);
            var database = client.GetDatabase("DBMSdb");
            var collection = database.GetCollection<BsonDocument>("users");

            var filter = Builders<BsonDocument>.Filter.And(
                Builders<BsonDocument>.Filter.Ne("status", "deleted"),
                Builders<BsonDocument>.Filter.Eq("role", "customer")

            );
            var result = collection.Find(filter).ToList();

            DataTable dt = new DataTable();

            // Add columns based on your data structure
            dt.Columns.Add("_id");
            dt.Columns.Add("comp_name");
            dt.Columns.Add("rep_name");
            dt.Columns.Add("username");
            dt.Columns.Add("contact");
            dt.Columns.Add("address");
            dt.Columns.Add("email");


            foreach (var document in result)
            {
                DataRow row = dt.NewRow();
                row["_id"] = document.GetValue("_id", "").ToString();
                row["comp_name"] = document.GetValue("comp_name", "").ToString();
                row["rep_name"] = document.GetValue("rep_name", "").ToString();
                row["username"] = document.GetValue("username", "").ToString();
                row["contact"] = document.GetValue("contact", "").ToString();
                row["address"] = document.GetValue("address", "").ToString();
                row["email"] = document.GetValue("email", "").ToString();
                dt.Rows.Add(row);
            }

            return dt;
        }



        private byte[] RetrieveImageData(int conditionValue)
        {
            var client = new MongoClient(connString);
            var database = client.GetDatabase("DBMSdb");
            var collection = database.GetCollection<BsonDocument>("users");

            var filter = Builders<BsonDocument>.Filter.Eq("_id", conditionValue);

            var document = collection.Find(filter).FirstOrDefault();

            if (document != null)
            {
                if (document.TryGetValue("profile", out var imageField) && imageField is BsonBinaryData binaryData)
                {
                    return binaryData.AsByteArray;
                }
                else
                {
                    // Handle the case where the "image" field is not of type BsonBinaryData or does not exist
                }
            }

            return null;
        }

        //protected void submitBtn_Click(object sender, EventArgs e)
        //{
        //    var client = new MongoClient(connString);
        //    var database = client.GetDatabase("DBMSdb");
        //    var collection = database.GetCollection<BsonDocument>("waste_cat");

        //    int currentId;

        //    // Check if there are documents in the collection
        //    var documentCount = collection.CountDocuments(new BsonDocument());

        //    // If no documents exist, start from 1
        //    currentId = documentCount == 0 ? 1 : (collection.Find(new BsonDocument())
        //        .Sort(Builders<BsonDocument>.Sort.Descending("_id"))
        //        .Limit(1)
        //        .FirstOrDefault()?["_id"].AsInt32 ?? 0) + 1;

        //    // Waste name
        //    //string name = waste_name.Text;

        //    //if (IsWasteExist(name))
        //    //{
        //    //    // Display an error message indicating that the waste name already exists
        //    //    Response.Write("<script>alert('Waste name already exists. Please choose a different waste name.')</script>");
        //    //    return;
        //    //    LoadCustomerBill();
        //    //}

        //    // Convert price to decimal and round to two decimal places
        //    //if (!decimal.TryParse(price.Text, out decimal wastePrice))
        //    //{
        //    //    // Display an error message for invalid price
        //    //    Response.Write("<script>alert('Invalid price. Please enter a valid decimal value.')</script>");
        //    //    return;
        //    //    LoadCustomerBill();
        //    //}

        //    // Round to two decimal places
        //    //wastePrice = Math.Round(wastePrice, 2);

        //    //var document = new BsonDocument
        //    //{
        //    //    { "_id", currentId },
        //    //    { "name", waste_name.Text },
        //    //    { "unit", unit.Text },
        //    //    { "price", (double)wastePrice  }, // Store the rounded price as decimal
        //    //    { "status", "active" }
        //    //};

        //    //collection.InsertOne(document);
        //    Response.Write("<script>alert('Waste added!')</script>");
        //    LoadCustomerBill();
        //}

        protected void update_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            // Access the GridViewRow that contains the Button
            GridViewRow row = (GridViewRow)btn.NamingContainer;

            // Access the DataKeys collection to get the value of "_id"
            int waste_id = Convert.ToInt32(gridViewBookings.DataKeys[row.RowIndex].Value);

            // Find controls within the GridViewRow
            TextBox update_waste_name = (TextBox)row.FindControl("update_waste_name");
            TextBox update_unit = (TextBox)row.FindControl("update_unit");
            TextBox update_price = (TextBox)row.FindControl("update_price");

            var client = new MongoClient(connString);
            var database = client.GetDatabase("DBMSdb");
            var collection = database.GetCollection<BsonDocument>("waste_cat");

            var filter = Builders<BsonDocument>.Filter.And(
                Builders<BsonDocument>.Filter.Eq("_id", waste_id)
            );

            var update = Builders<BsonDocument>.Update
                .Set("name", update_waste_name.Text)
                .Set("unit", update_unit.Text)
                .Set("price", update_price.Text);

            var result = collection.UpdateOne(filter, update);

            if (result.ModifiedCount > 0)
            {
                var count = result.ModifiedCount.ToString();
                Response.Write($"<script>alert('{count} Updated Successfully!')</script>");
                //LoadCustomerBill();
            }
            else
            {
                Response.Write("<script>alert('No matching document found to update!')</script>");
                //LoadCustomerBill();
            }
        }



        protected void Remove_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;

            int admID = Convert.ToInt32(btn.CommandArgument);
            try
            {
                var client = new MongoClient(connString);
                var database = client.GetDatabase("DBMSdb");
                var collection = database.GetCollection<BsonDocument>("waste_cat");

                var filter = Builders<BsonDocument>.Filter.Eq("_id", admID);
                var update = Builders<BsonDocument>.Update.Set("status", "deleted");

                var result = collection.UpdateOne(filter, update);

                if (result.ModifiedCount >= 1)
                {
                    // Perform necessary actions after unsuspension
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
                        "swal('Deleted!', 'Admin Deleted Successfully!', 'success')", true);
                    //LoadCustomerBill();
                }
                else
                {
                    // Handle case where no admin was updated (perhaps admin with given ID not found)
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
                        "swal('Unsuccessful!', 'Admin not found or not updated!', 'error')", true);
                    //LoadCustomerBill();
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
                    "swal('Error!', '" + ex.Message + "', 'error')", true);
                //LoadCustomerBill();
            }
        }
        // Action to show the the modal and pass the employee id


        // Get the Employee information on its account to be used for updation
        //public void GetCustomerInfo(int cusID)
        //{
        //    try
        //    {
        //        var client = new MongoClient(connString);
        //        var database = client.GetDatabase("DBMSdb");
        //        var collection = database.GetCollection<BsonDocument>("users");

        //        var filter = Builders<BsonDocument>.Filter.Eq("_id", cusID);
        //        var result = collection.Find(filter).FirstOrDefault();

        //        if (result != null)
        //        {
        //            txtEstablishment.Text = result["comp_name"].ToString();
        //            txtRep.Text = result["rep_name"].ToString();
        //            PopulateWasteCategories();
        //            LoadCustomerBill();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
        //                "swal('Unsuccessful!', '" + ex.Message + "', 'error')", true);
        //        LoadCustomerBill();
        //        PopulateWasteCategories();
        //    }
        //}

        private int GetLeadDaysFromControls()
        {
            try
            {
                var client = new MongoClient(connString);
                var database = client.GetDatabase("DBMSdb");
                var collection = database.GetCollection<BsonDocument>("controls");

                // Assuming there is only one document in the "controls" collection
                var controlsDocument = collection.Find(new BsonDocument()).FirstOrDefault();

                if (controlsDocument != null && controlsDocument.Contains("leadDays"))
                {
                    return controlsDocument["leadDays"].AsInt32;
                }
            }
            catch (Exception ex)
            {
                // Handle the exception, log, or display an error message
                Console.WriteLine($"Error getting lead days: {ex.Message}");
            }

            // Default to 0 if leadDays couldn't be retrieved
            return 0;
        }

        public void GetCustomerInfo(int cusID)
        {
            try
            {
                var client = new MongoClient(connString);
                var database = client.GetDatabase("DBMSdb");
                var collection = database.GetCollection<BsonDocument>("users");

                var filter = Builders<BsonDocument>.Filter.Eq("_id", cusID);
                var result = collection.Find(filter).FirstOrDefault();

                if (result != null)
                {
                    txtEstablishment.Text = result["comp_name"].ToString();
                    txtRep.Text = result["rep_name"].ToString();

                    // Get leadDays from the "controls" collection
                    int leadDays = GetLeadDaysFromControls(); // Replace with actual method

                    // Calculate the due date by adding leadDays to the current date
                    DateTime dueDate = DateTime.Now.AddDays(leadDays);

                    // Display the due date in txtDueDate
                    txtDueDate.Text = dueDate.ToString("yyyy-MM-dd HH:mm:ss");

                    PopulateWasteCategories();
                    //LoadCustomerBill();
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert",
                        "swal('Unsuccessful!', '" + ex.Message + "', 'error')", true);
                //LoadCustomerBill();
                PopulateWasteCategories();
            }
        }



        protected void genBill_Click(object sender, EventArgs e)
        {
            LinkButton btn = sender as LinkButton;
            //ImageButton btn = (ImageButton)sender;
            //ImageButton btn = sender as ImageButton;

            int id = Convert.ToInt32(btn.CommandArgument);
            txtbxID.Text = id.ToString();
            GetCustomerInfo(id);
            this.ModalPopupExtender2.Show();
            //LoadCustomerBill();
            PopulateWasteCategories();
        }
        private bool IsInitialLoad
        {
            get { return ViewState["IsInitialLoad"] as bool? ?? true; }
            set { ViewState["IsInitialLoad"] = value; }
        }
        //private void PopulateWasteCategories()
        //{
        //    if (!IsPostBack)
        //    {
        //        var client = new MongoClient(connString);
        //        var database = client.GetDatabase("DBMSdb");
        //        var collection = database.GetCollection<BsonDocument>("waste_cat");

        //        var distinctCategories = collection.Distinct<string>("name", "{}").ToList();
        //        distinctCategories.Insert(0, "Select Waste Category (disabled)");

        //        waste_cat.DataSource = distinctCategories;
        //        waste_cat.DataBind();

        //        foreach (ListItem item in waste_cat.Items)
        //        {
        //            item.Attributes["disabled"] = "disabled";
        //        }

        //        // Set the selected value if available
        //        if (!string.IsNullOrEmpty(Request.Form[waste_cat.UniqueID]))
        //        {
        //            waste_cat.SelectedValue = Request.Form[waste_cat.UniqueID];
        //        }
        //    }
        //}

        //private void PopulateWasteCategories()
        //{
        //    var client = new MongoClient(connString);
        //    var database = client.GetDatabase("DBMSdb");

        //    var filter = Builders<BsonDocument>.Filter.Ne("status", "deleted");

        //    string addClientCollectionName = "waste_cat";
        //    var addClientCollection = database.GetCollection<BsonDocument>(addClientCollectionName);

        //    var distinctEstablishments = addClientCollection.Distinct<string>("name", filter).ToList();

        //    waste_cat.DataSource = distinctEstablishments;
        //    waste_cat.DataBind();
        //}
        private void PopulateWasteCategories()
        {
            var client = new MongoClient(connString);
            var database = client.GetDatabase("DBMSdb");

            var filter = Builders<BsonDocument>.Filter.Ne("status", "deleted");

            string addClientCollectionName = "waste_cat";
            var addClientCollection = database.GetCollection<BsonDocument>(addClientCollectionName);

            var distinctEstablishments = addClientCollection.Distinct<string>("name", filter).ToList();

            // Clear existing items before adding new ones
            waste_cat.Items.Clear();

            // Set the default value and disable it
            ListItem defaultItem = new ListItem("Select Waste Category", "");
            defaultItem.Attributes["disabled"] = "disabled";
            waste_cat.Items.Add(defaultItem);

            // Add the distinct waste categories to the DropDownList
            foreach (var category in distinctEstablishments)
            {
                waste_cat.Items.Add(category);
            }

            // Bind the DropDownList
            waste_cat.DataBind();
        }





        //private void BindWasteCategories()
        //{
        //    List<string> wasteCategories = GetWasteCategories();

        //    waste_cat.DataSource = wasteCategories;
        //    waste_cat.DataBind();

        //    // Add the default item
        //    waste_cat.Items.Insert(0, new ListItem("-- Select Waste Category --", string.Empty));
        //    waste_cat.Items[0].Attributes["disabled"] = "disabled";
        //    waste_cat.Items[0].Selected = true;
        //}
        //protected void waste_cat_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    txtSumKilo.Text = string.Empty;
        //    txtNetVat.Text = string.Empty;
        //    txtVatAmnt.Text = string.Empty;
        //    txtTotSales.Text = string.Empty;
        //    txtSumKilo.Enabled = true;

        //    string selectedMaterial = waste_cat.SelectedValue;

        //    if (selectedMaterial == "Select Waste Category (disabled)")
        //    {
        //        // Handle the case when the disabled default option is selected
        //        txtPrice.Text = string.Empty;
        //    }
        //    else
        //    {
        //        var client = new MongoClient(connString);
        //        var database = client.GetDatabase("DBMSdb");
        //        var collection = database.GetCollection<BsonDocument>("waste_cat");

        //        var filter = Builders<BsonDocument>.Filter.Eq("name", selectedMaterial);
        //        var result = collection.Find(filter).FirstOrDefault();

        //        if (result != null && result.Contains("price"))
        //        {
        //            // Assuming "UNIT" is the field you want to retrieve from MongoDB
        //            txtPrice.Text = result["price"].ToString();
        //            PopulateWasteCategories();

        //        }
        //        else
        //        {
        //            txtPrice.Text = "Unit price not available for the selected category.";
        //            PopulateWasteCategories();

        //        }
        //    }
        //    PopulateWasteCategories();

        //}

        protected void waste_cat_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearTextBoxes();

            string selectedMaterial = waste_cat.SelectedValue;

            if (selectedMaterial != "Select Waste Category")
            {
                var client = new MongoClient(connString);
                var database = client.GetDatabase("DBMSdb");
                var collection = database.GetCollection<BsonDocument>("waste_cat");

                var filter = Builders<BsonDocument>.Filter.Eq("name", selectedMaterial);
                var result = collection.Find(filter).FirstOrDefault();

                if (result != null && result.Contains("price"))
                {
                    txtPrice.Text = result["price"].ToString();
                }
                else
                {
                    txtPrice.Text = "Unit price not available for the selected category.";
                }
            }
        }

        private void ClearTextBoxes()
        {
            txtSumKilo.Text = string.Empty;
            txtNetVat.Text = string.Empty;
            txtVatAmnt.Text = string.Empty;
            txtTotSales.Text = string.Empty;
            txtSumKilo.Enabled = true;
            txtPrice.Text = string.Empty;
        }


        protected void btncancel_Click(object sender, EventArgs e)
        {
            // Add any necessary logic for cancel button click
            //txtSumKilo.Text = string.Empty;
            //txtNetVat.Text = string.Empty;
            //txtVatAmnt.Text = string.Empty;
            //txtTotSales.Text = string.Empty;
            //txtSumKilo.Enabled = true;

            ClearValues();
        }


        //protected void txtSumKilo_TextChanged(object sender, EventArgs e)
        //{
        //    // Calculate the values when the sum of kilo changes
        //    CalculateValues();
        //}

        //private void CalculateValues()
        //{
        //    string selectedMaterial = waste_cat.SelectedValue;

        //    if (selectedMaterial == "Select Waste Category (disabled)")
        //    {
        //        // Handle the case when the disabled default option is selected
        //        ClearValues();
        //    }
        //    else
        //    {
        //        var client = new MongoClient(connString);
        //        var database = client.GetDatabase("DBMSdb");
        //        var collection = database.GetCollection<BsonDocument>("waste_cat");

        //        var filter = Builders<BsonDocument>.Filter.Eq("name", selectedMaterial);
        //        var result = collection.Find(filter).FirstOrDefault();

        //        if (result != null && result.Contains("price"))
        //        {
        //            // Assuming "UNIT" is the field you want to retrieve from MongoDB
        //            double unitPrice;
        //            if (double.TryParse(result["price"].ToString(), out unitPrice))
        //            {
        //                // Assuming you have TextBoxes txtSumKilo, txtPrice, txtNetVat, txtVatAmnt, and txtTotSales

        //                // Calculate the sum of kilo times unit price
        //                double sumKilo;
        //                if (double.TryParse(txtSumKilo.Text, out sumKilo))
        //                {
        //                    double netOfVat = sumKilo * unitPrice;
        //                    txtNetVat.Text = netOfVat.ToString("0.00");

        //                    // Calculate vat amount (netOfVat * 0.12)
        //                    double vatAmount = netOfVat * 0.12;
        //                    txtVatAmnt.Text = vatAmount.ToString("0.00");

        //                    // Calculate total sales (netOfVat + vatAmount)
        //                    double totalSales = netOfVat + vatAmount;
        //                    txtTotSales.Text = totalSales.ToString("0.00");
        //                }
        //            }
        //            else
        //            {
        //                ClearValues();
        //            }
        //        }
        //        else
        //        {
        //            ClearValues();
        //        }
        //    }
        //}

        //private void ClearValues()
        //{
        //    // Clear the values in TextBoxes
        //    txtSumKilo.Text = string.Empty;
        //    txtNetVat.Text = string.Empty;
        //    txtVatAmnt.Text = string.Empty;
        //    txtTotSales.Text = string.Empty;
        //}





        protected void txtSumKilo_TextChanged(object sender, EventArgs e)
        {
            // Calculate the values when the sum of kilo changes
            string selectedMaterial = waste_cat.SelectedValue;

            if (selectedMaterial == "Select Waste Category (disabled)")
            {
                // Handle the case when the disabled default option is selected
                ClearValues();
            }
            else
            {
                var client = new MongoClient(connString);
                var database = client.GetDatabase("DBMSdb");
                var collection = database.GetCollection<BsonDocument>("waste_cat");

                var filter = Builders<BsonDocument>.Filter.Eq("name", selectedMaterial);
                var result = collection.Find(filter).FirstOrDefault();

                if (result != null && result.Contains("price"))
                {
                    // Assuming "UNIT" is the field you want to retrieve from MongoDB
                    double unitPrice;
                    if (double.TryParse(result["price"].ToString(), out unitPrice))
                    {
                        // Assuming you have TextBoxes txtSumKilo, txtPrice, txtNetVat, txtVatAmnt, and txtTotSales

                        // Calculate the sum of kilo times unit price
                        double sumKilo;
                        if (double.TryParse(txtSumKilo.Text, out sumKilo))
                        {
                            double netOfVat = sumKilo * unitPrice;
                            txtNetVat.Text = netOfVat.ToString("0.00");

                            // Calculate vat amount (netOfVat * 0.12)
                            double vatAmount = netOfVat * 0.12;
                            txtVatAmnt.Text = vatAmount.ToString("0.00");

                            // Calculate total sales (netOfVat + vatAmount)
                            double totalSales = netOfVat + vatAmount;
                            txtTotSales.Text = totalSales.ToString("0.00");

                            // After setting the values, update the UpdatePanel
                            //updatePanel.Update();
                        }
                        updatePanel.Update();

                    }
                    else
                    {
                        ClearValues();
                        updatePanel.Update();

                    }
                }
                else
                {
                    ClearValues();
                    updatePanel.Update();

                }
            }
        }

        //private void CalculateValues()
        //{
        //    string selectedMaterial = waste_cat.SelectedValue;

        //    if (selectedMaterial == "Select Waste Category (disabled)")
        //    {
        //        // Handle the case when the disabled default option is selected
        //        ClearValues();
        //    }
        //    else
        //    {
        //        var client = new MongoClient(connString);
        //        var database = client.GetDatabase("DBMSdb");
        //        var collection = database.GetCollection<BsonDocument>("waste_cat");

        //        var filter = Builders<BsonDocument>.Filter.Eq("name", selectedMaterial);
        //        var result = collection.Find(filter).FirstOrDefault();

        //        if (result != null && result.Contains("price"))
        //        {
        //            // Assuming "UNIT" is the field you want to retrieve from MongoDB
        //            double unitPrice;
        //            if (double.TryParse(result["price"].ToString(), out unitPrice))
        //            {
        //                // Assuming you have TextBoxes txtSumKilo, txtPrice, txtNetVat, txtVatAmnt, and txtTotSales

        //                // Calculate the sum of kilo times unit price
        //                double sumKilo;
        //                if (double.TryParse(txtSumKilo.Text, out sumKilo))
        //                {
        //                    double netOfVat = sumKilo * unitPrice;
        //                    txtNetVat.Text = netOfVat.ToString("0.00");

        //                    // Calculate vat amount (netOfVat * 0.12)
        //                    double vatAmount = netOfVat * 0.12;
        //                    txtVatAmnt.Text = vatAmount.ToString("0.00");

        //                    // Calculate total sales (netOfVat + vatAmount)
        //                    double totalSales = netOfVat + vatAmount;
        //                    txtTotSales.Text = totalSales.ToString("0.00");

        //                    // After setting the values, update the UpdatePanel
        //                    //updatePanel.Update();
        //                }
        //                updatePanel.Update();

        //            }
        //            else
        //            {
        //                ClearValues();
        //                updatePanel.Update();

        //            }
        //        }
        //        else
        //        {
        //            ClearValues();
        //            updatePanel.Update();

        //        }
        //    }
        //}


        private void ClearValues()
        {
            // Clear the values in TextBoxes
            txtSumKilo.Text = string.Empty;
            txtNetVat.Text = string.Empty;
            txtVatAmnt.Text = string.Empty;
            txtTotSales.Text = string.Empty;
        }


        //private void UpdateOverdueBills()
        //{
        //    var client = new MongoClient(connString);
        //    var database = client.GetDatabase("DBMSdb");
        //    var collection = database.GetCollection<BsonDocument>("generate_bill");

        //    // Find overdue bills (date_due has passed, bill_status is still "pending")
        //    var filter = Builders<BsonDocument>.Filter.And(
        //        Builders<BsonDocument>.Filter.Lt("date_due", DateTime.Now),
        //        Builders<BsonDocument>.Filter.Eq("bill_status", "pending")
        //    );

        //    var overdueBills = collection.Find(filter).ToList();

        //    foreach (var bill in overdueBills)
        //    {
        //        // Calculate interest based on the remaining balance
        //        double remainingBalance = bill["remain_balance"].ToDouble();
        //        double getInterestNow = GetInterest(getInter);

        //        double interestRate = getInterestNow/100; // 6% interest per month
        //        double interest = remainingBalance * interestRate;

        //        // Update the bill with the calculated interest and new remaining balance
        //        var update = Builders<BsonDocument>.Update
        //            .Set("interest", interest)
        //            .Inc("total_amnt_interest", interest) // Increment total_amnt_interest
        //            .Inc("remain_balance", interest); // Increment remaining balance by the interest

        //        collection.UpdateOne(Builders<BsonDocument>.Filter.Eq("_id", bill["_id"]), update);
        //    }
        //}

        //private double GetInterest(double getInterest)
        //{
        //    // Fetch the most recent bill for the customer
        //    var client = new MongoClient(connString);
        //    var database = client.GetDatabase("DBMSdb");
        //    var collection = database.GetCollection<BsonDocument>("controls");

        //    var filter = Builders<BsonDocument>.Filter.Eq("_id", 2);
        //    var result = collection.Find(filter).FirstOrDefault();

        //    if (result != null)
        //    {
        //        // Check if the recent bill has a remaining balance
        //        if (result.Contains("interest"))
        //        {
        //            return result["interest"].AsDouble;
        //        }
        //    }

        //    return 0.0; // Default to 0 if there's no previous remaining balance
        //}



        private void UpdateOverdueBills()
        {
            var client = new MongoClient(connString);
            var database = client.GetDatabase("DBMSdb");
            var collection = database.GetCollection<BsonDocument>("generate_bill");

            // Find overdue bills (date_due has passed, bill_status is still "pending")
            var filter = Builders<BsonDocument>.Filter.And(
            Builders<BsonDocument>.Filter.Lt("date_due", DateTime.Now),
                Builders<BsonDocument>.Filter.Eq("bill_status", "pending")
            );

            var overdueBills = collection.Find(filter).ToList();

            foreach (var bill in overdueBills)
            {
                // Calculate interest based on the remaining balance
                double remainingBalance = bill["remain_balance"].ToDouble();
                double getInterestNow = GetInterest();

                double interestRate = getInterestNow / 100; // 6% interest per month
                double interest = remainingBalance * interestRate;

                // Update the bill with the calculated interest and new remaining balance
                var update = Builders<BsonDocument>.Update
                    .Set("interest", interest)
                    .Inc("total_amnt_interest", interest) // Increment total_amnt_interest
                    .Inc("remain_balance", interest); // Increment remaining balance by the interest

                collection.UpdateOne(Builders<BsonDocument>.Filter.Eq("_id", bill["_id"]), update);
            }
        }

        private double GetInterest()
        {
            // Fetch the most recent bill for the customer
            var client = new MongoClient(connString);
            var database = client.GetDatabase("DBMSdb");
            var collection = database.GetCollection<BsonDocument>("controls");

            var filter = Builders<BsonDocument>.Filter.Eq("_id", 2);

            try
            {
                var result = collection.Find(filter).FirstOrDefault();

                if (result != null && result.Contains("interest"))
                {
                    return result["interest"].AsDouble;
                }
            }
            catch (Exception ex)
            {
                // Handle the exception, log it, or take appropriate action
                Console.WriteLine($"Error fetching interest: {ex.Message}");
            }

            return 0.0; // Default to 0 if there's no previous remaining balance or in case of an error
        }


        protected void btnGenerateBill_Click(object sender, EventArgs e)
        {
            var client = new MongoClient(connString);
            var database = client.GetDatabase("DBMSdb");
            var collection = database.GetCollection<BsonDocument>("generate_bill");

            int currentId;

            // Check if there are documents in the collection
            var documentCount = collection.CountDocuments(new BsonDocument());

            // If no documents exist, start from 1
            currentId = documentCount == 0 ? 600000 : (collection.Find(new BsonDocument())
                .Sort(Builders<BsonDocument>.Sort.Descending("_id"))
                .Limit(1)
                .FirstOrDefault()?["_id"].AsInt32 ?? 0) + 1;

            // Check if there's a remaining balance from the previous bill
            double previousRemainingBalance = GetPreviousRemainingBalance(txtbxID.Text);

            // Check if there's an over_payment from the previous bill
            double previousOverPayment = GetPreviousOverPayment(txtbxID.Text);
            double totalSales = double.TryParse(txtTotSales.Text, out double kilo) ? kilo : 0.0;

            //double totalSales = txtTotSales.Text;
            // Calculate the total remaining balance for the new bill
            double totalRemainingBalance = previousRemainingBalance - previousOverPayment + totalSales;

            // Parse the txtDueDate.Text value
            if (DateTime.TryParse(txtDueDate.Text, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dueDate))
            {
                // Format it back to the desired format
                string formattedDueDate = dueDate.ToString("yyyy-MM-dd HH:mm:ss");

                // Now you can use formattedDueDate in your BsonDocument
                var document = new BsonDocument
                {
                    { "_id", currentId },
                    { "cus_id", txtbxID.Text },
                    { "comp_name", txtEstablishment.Text },
                    { "rep_name", txtRep.Text },
                    { "description", waste_cat.Text },
                    { "sum_kilo", int.TryParse(txtSumKilo.Text, out int sumKilo) ? sumKilo : 0 },
                    { "unit_price", double.TryParse(txtPrice.Text, out double unitPrice) ? unitPrice : 0.0 },
                    { "num_trips", int.TryParse(num_trips.Text, out int numTrips) ? numTrips : 0 },
                    { "net_vat", double.TryParse(txtNetVat.Text, out double netVat) ? netVat : 0.0 },
                    { "vat_amnt", double.TryParse(txtVatAmnt.Text, out double vatAmnt) ? vatAmnt : 0.0 },
                    { "tot_sales", double.TryParse(txtTotSales.Text, out double totSales) ? totSales : 0.0 },
                    { "date_issued", DateTime.Now},
                    { "date_due", dueDate},
                    { "interest", BsonDouble.Create(0) },
                    { "vat", "12%" },
                    { "total_amnt_interest", BsonDouble.Create(0) },
                    { "date_paid", BsonDateTime.Create(DateTime.MinValue) },
                    { "upload_receipt", BsonNull.Value },
                    { "amnt_paid", BsonNull.Value },
                    { "remain_balance", totalRemainingBalance }, // Add previous remaining balance, if any
                    { "over_payment", BsonNull.Value},
                    { "bill_status", "pending" },
                    { "status", "active" }
                };

                collection.InsertOne(document);
                Response.Write("<script>alert('Bill generated!')</script>");
                //LoadCustomerBill();
                UpdateOverdueBills();
            }
            else
            {
                // Handle the case where txtDueDate.Text is not a valid DateTime
                Response.Write("<script>alert('Invalid due date. Please enter a valid date and time.')</script>");
            }
            UpdateOverdueBills();
        }

        private double GetPreviousRemainingBalance(string customerId)
        {
            // Fetch the most recent bill for the customer
            var client = new MongoClient(connString);
            var database = client.GetDatabase("DBMSdb");
            var collection = database.GetCollection<BsonDocument>("generate_bill");
            var filter = Builders<BsonDocument>.Filter.And(
            Builders<BsonDocument>.Filter.Eq("cus_id", customerId),
                Builders<BsonDocument>.Filter.Eq("status", "active")
            );

            var sort = Builders<BsonDocument>.Sort.Descending("date_issued");

            var recentBill = collection.Find(filter).Sort(sort).FirstOrDefault();

            if (recentBill != null)
            {
                // Check if the recent bill has a remaining balance
                if (recentBill.Contains("remain_balance"))
                {
                    return recentBill["remain_balance"].AsDouble;
                }
            }

            return 0.0; // Default to 0 if there's no previous remaining balance
        }

        private double GetPreviousOverPayment(string customerId)
        {
            // Fetch the most recent bill for the customer
            var client = new MongoClient(connString);
            var database = client.GetDatabase("DBMSdb");
            var collection = database.GetCollection<BsonDocument>("generate_bill");
            var filter = Builders<BsonDocument>.Filter.And(
            Builders<BsonDocument>.Filter.Eq("cus_id", customerId),
                Builders<BsonDocument>.Filter.Eq("status", "active")
            );

            var sort = Builders<BsonDocument>.Sort.Descending("date_issued");

            var recentBill = collection.Find(filter).Sort(sort).FirstOrDefault();

            if (recentBill != null)
            {
                // Check if the recent bill has an over_payment field
                if (recentBill.Contains("over_payment"))
                {
                    // Get the over_payment field value
                    var overPaymentField = recentBill["over_payment"];

                    // Check if the over_payment field is not null
                    if (overPaymentField != BsonNull.Value)
                    {
                        return overPaymentField.AsDouble;
                    }
                }
            }

            return 0.0; // Default to 0 if there's no "over_payment" field or it's null
        }



    }
}