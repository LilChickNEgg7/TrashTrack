using AjaxControlToolkit;
using Npgsql;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace Capstone
{
    public partial class Z2 : System.Web.UI.Page
    {
        private readonly string con = "Server=localhost;Port=5432;User Id=postgres;Password=123456;Database=trashtrack";

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnOtherAction_Click(object sender, EventArgs e)
        {

        }

        protected void editBookWaste_Click(object sender, EventArgs e)
        {
            LinkButton btn = sender as LinkButton;

            //if (btn != null)
            //{
            //    // Get the bw_id from CommandArgument
            //    int bw_id = Convert.ToInt32(btn.CommandArgument);

            //    //// Store bw_id in the hidden field for later use
            //    hfBwID.Value = bw_id.ToString(); // Store the id in the hidden field

            //    //// Display the bw_id from the hidden field in the textbox
            //    //txtbwID.Text = hfBwID.Value;
            //    try
            //    {
            //        using (var db = new NpgsqlConnection(con))
            //        {
            //            db.Open();
            //            using (var cmd = db.CreateCommand())
            //            {
            //                cmd.CommandText = "SELECT * FROM BOOKING_WASTE WHERE bw_id = @bw_id";
            //                cmd.Parameters.AddWithValue("bw_id", bw_id);
            //                using (NpgsqlDataReader reader = cmd.ExecuteReader())
            //                {
            //                    if (reader.Read())
            //                    {
            //                        txtName.Text = reader["bw_name"].ToString();
            //                        txtbwUnit.Text = reader["bw_unit"].ToString();
            //                        txtTotalUnit.Text = reader["bw_total_unit"].ToString();
            //                        txtUnitPrice.Text = reader["bw_price"].ToString();
            //                        txtTotalUnitPrice.Text = reader["bw_total_price"].ToString();


            //                        //Response.Write("<script>alert('Yeehaw!')</script>");
            //                    }
            //                }


            //            }

            //        }
            //    }
            //    catch
            //    {

            //    }


            //    // Ensure UpdatePanel is refreshed (if needed)
            //    updatePanel.Update(); // If using UpdatePanel, force an update to reflect changes

            //    // Show the modal popup
            //    ModalPopupExtender2.Show();
            //}
        }

        protected void btnViewSlip_Click(object sender, EventArgs e)
        {
            int bookingId = 1; // Hardcoded for testing
            byte[] imageData = null;

            using (var db = new NpgsqlConnection(con))
            {
                db.Open();
                string query = "SELECT bk_waste_scale_slip FROM booking WHERE bk_id = @id";
                using (var cmd = new NpgsqlCommand(query, db))
                {
                    cmd.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Integer, bookingId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            imageData = reader["bk_waste_scale_slip"] as byte[];
                        }
                        else
                        {
                            // Handle no data found
                            Response.Write("<script>alert('No data found for the specified ID.');</script>");
                            return;
                        }
                    }
                }
                db.Close();
            }

            if (imageData != null && imageData.Length > 0)
            {
                string base64String = Convert.ToBase64String(imageData);
                string script = $"loadImage('data:image/jpeg;base64,{base64String}');";
                ScriptManager.RegisterStartupScript(this, GetType(), "LoadImageScript", script, true);
            }
            else
            {
                Image2.Attributes["src"] = "~/Pictures/blank_prof.png"; // Default image
                Image2.Style["display"] = "block";

                // Show the modal without blocking background interaction
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "showModal();", true);
            }
        }



    }
}
