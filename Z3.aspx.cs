using Npgsql;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Capstone
{
    public partial class Z3 : System.Web.UI.Page
    {
        private readonly string con = "Server=localhost;Port=5432;User Id=postgres;Password=123456;Database=trashtrack";

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnViewSlip_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            /*int bookingId = Convert.ToInt32(TextBox1.Text); */ // Assuming you hardcode the bookingId for now. Adjust this as necessary.
            int bookingId = 1;
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
        protected void openBookWaste_Click(object sender, EventArgs e)
        {

        }
        protected void btnCloseSlip_Click(object sender, EventArgs e)
        {
            // Hide the modal
            //ModalPopupExtender4.Hide();

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
    }
}