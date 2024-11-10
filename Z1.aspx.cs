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
    public partial class Z1 : System.Web.UI.Page
    {
        private readonly string con = "Server=localhost;Port=5432;User Id=postgres;Password=123456;Database=trashtrack";

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnViewSlip_Click(object sender, EventArgs e)
        {
            byte[] imageData = null;

            try
            {
                using (var db = new NpgsqlConnection(con))
                {
                    db.Open();
                    string query = "SELECT bk_waste_scale_slip FROM booking WHERE bk_id = 1"; // Use a valid booking ID
                    using (var cmd = new NpgsqlCommand(query, db))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                imageData = reader["bk_waste_scale_slip"] as byte[];
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // You can log this error as needed
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Error: " + ex.Message + "');", true);
                return;
            }

            if (imageData != null && imageData.Length > 0)
            {
                string base64String = Convert.ToBase64String(imageData);
                Image2.Attributes["src"] = "data:image/jpeg;base64," + base64String; // Set image source
                Image2.Style["display"] = "block"; // Show the image
            }
            else
            {
                // Fallback image if no image data is found
                Image2.Attributes["src"] = "~/Pictures/blank_prof.png"; // Use a default image
                Image2.Style["display"] = "block"; // Show the image
            }
        }


        protected void btnCloseSlip_Click(object sender, EventArgs e)
        {
            // Reset the image and hide it when the modal is closed
            string resetScript = @"
                document.getElementById('Image2').style.display = 'none';
                document.getElementById('Image2').src = '';";
            ScriptManager.RegisterStartupScript(this, GetType(), "ResetModal", resetScript, true);
        }

        private void ShowAlert(string message)
        {
            string script = $"alert('{message}');";
            ScriptManager.RegisterStartupScript(this, GetType(), "Alert", script, true);
        }

        //private int GetBookingId()
        //{
        //    // Implement logic to dynamically retrieve the booking ID
        //    // For example, you might fetch it from the session or a hidden field
        //    return 1; // Replace with actual logic
        //}
    }
}