using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
//using System.Web.UI.WebControls;

using WebControls = System.Web.UI.WebControls;
using MongoDB.Driver.Linq;
using System.Data.SqlClient;
using Amazon.SecurityToken.Model;
//using AjaxControlToolkit.HtmlEditor.ToolbarButtons;
using Amazon.Runtime.Documents;
using Npgsql.Internal;
using System.IO;


using System.Web.UI.WebControls;
using iText.Kernel.Pdf;
using ITextDocument = iText.Layout.Document;
using iText.Layout.Element;
using iText.Layout.Properties;


using iText.Kernel.Colors;
using iText.IO.Image;


using iText.Kernel.Colors;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Layout.Borders;

namespace Capstone
{
    public partial class Z4 : System.Web.UI.Page
    {
        private readonly string con = "Server=localhost;Port=5432;User Id=postgres;Password=123456;Database=trashtrack";

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnGenerateBill_Click(object sender, EventArgs e)
        {
            // Variables for payment terms and bill details
            int? gb_num_trips = null;
            string gb_note = null;
            decimal? gb_add_fees = null;
            decimal? gb_net_vat = null;
            decimal? gb_vat_amnt = null;
            decimal? gb_total_sales = null;
            DateTime? gb_date_issued = null;
            DateTime? gb_date_due = null;
            double? gb_interest = null;
            int? gb_lead_days = null;
            int? gb_accrual_period = null;
            int? gb_suspend_period = null;
            DateTime? gb_accrual_date = null;
            DateTime? gb_suspend_date = null;
            decimal? gb_tax = null;
            decimal? gb_total_amnt_interest = null;
            string gb_status = null;
            DateTime? gb_created_at = null;
            DateTime? gb_updated_at = null;

            int bk_id = 0;
            int emp_id = 1;
            int gb_id = 1; // Assuming you are fetching by `gb_id = 1`

            DateTime? currentDate = DateTime.TryParse(dateEntered.Text, out DateTime EntDate) ? (DateTime?)EntDate : null;

            //SOLUTION
            if (gb_date_due > currentDate)
            {

            }
            else
            {

            }

            // Retrieve payment term values from the database
            using (var conn = new NpgsqlConnection(con)) // Use NpgsqlConnection
            {
                conn.Open();

                string findBillQuery = "SELECT * FROM generate_bill WHERE gb_id = @GbId";

                using (var cmd = new NpgsqlCommand(findBillQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@GbId", gb_id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            gb_num_trips = reader["gb_num_trips"] as int?;
                            gb_note = reader["gb_note"]?.ToString();
                            gb_add_fees = reader["gb_add_fees"] as decimal?;
                            gb_net_vat = reader["gb_net_vat"] as decimal?;
                            gb_vat_amnt = reader["gb_vat_amnt"] as decimal?;
                            gb_total_sales = reader["gb_total_sales"] as decimal?;
                            gb_date_issued = reader["gb_date_issued"] as DateTime?;
                            gb_date_due = reader["gb_date_due"] as DateTime?;
                            gb_interest = reader["gb_interest"] as double?;
                            gb_lead_days = reader["gb_lead_days"] as int?;
                            gb_accrual_period = reader["gb_accrual_period"] as int?;
                            gb_suspend_period = reader["gb_suspend_period"] as int?;
                            gb_accrual_date = reader["gb_accrual_date"] as DateTime?;
                            gb_suspend_date = reader["gb_suspend_date"] as DateTime?;
                            gb_tax = reader["gb_tax"] as decimal?;
                            gb_total_amnt_interest = reader["gb_total_amnt_interest"] as decimal?;
                            gb_status = reader["gb_status"]?.ToString();
                            gb_created_at = reader["gb_created_at"] as DateTime?;
                            gb_updated_at = reader["gb_updated_at"] as DateTime?;
                            bk_id = Convert.ToInt32(reader["bk_id"]);
                            emp_id = Convert.ToInt32(reader["emp_id"]);
                        }
                        else
                        {
                            throw new Exception("Bill not found.");
                        }
                    }
                }
            }

            // Continue with bill generation and PDF creation
            int insertedBillId = gb_id;
            byte[] pdfBytes = GeneratePDFForRow(insertedBillId, bk_id);
            LoadBookingList();

            // Send the PDF for download.
            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", $"attachment;filename=Bill_{insertedBillId}.pdf");
            Response.Buffer = true;
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.BinaryWrite(pdfBytes);
            Response.End();
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
                SELECT * FROM generate_bill WHERE gb_status != paid ORDER BY gb_id, gb_created_at";
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
        private byte[] GeneratePDFForRow(int buttonText, int bkID)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(ms);
                PdfDocument pdf = new PdfDocument(writer);
                ITextDocument document = new ITextDocument(pdf);

                // Define fonts and colors
                //PdfFont font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
                PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                DeviceRgb redColor = new DeviceRgb(255, 0, 0);
                // Ensure this font file exists in your project folder
                string fontPath = Server.MapPath("~/fonts/Roboto/Roboto-Regular.ttf");
                // Create the font, specifying Unicode support with "Identity-H"
                PdfFont font = PdfFontFactory.CreateFont(fontPath, "Identity-H", PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);



                // Create a table with one row and two columns for the logo and address
                iText.Layout.Element.Table headerTable = new iText.Layout.Element.Table(new float[] { 1, 3 })
                    .UseAllAvailableWidth();

                // Add company logo (if any)
                string logoPath = Server.MapPath("~/Pictures/logo_bgRM.png");
                iText.Layout.Element.Image logo = new iText.Layout.Element.Image(ImageDataFactory.Create(logoPath));
                logo.ScaleToFit(100, 50); // Scale the logo to fit within defined dimensions

                // Create a table for the logo and TrashTrack text
                iText.Layout.Element.Table logoTextTable = new iText.Layout.Element.Table(2)
                    .UseAllAvailableWidth();

                // Create the logo cell
                Cell logoCell = new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(logo); // Add the logo

                // Define the green color
                DeviceRgb greenColor = new DeviceRgb(0, 128, 0); // Dark green color

                // Create the TrashTrack text without margin and padding
                Paragraph trashTrackText = new Paragraph("TrashTrack")
                    .SetFont(boldFont) // Use bold font for the text
                    .SetFontSize(30) // Set the font size
                    .SetFontColor(greenColor) // Set the font color to green
                    .SetTextAlignment(TextAlignment.LEFT) // Align text left
                    .SetMargin(0) // No margin for the paragraph
                    .SetPadding(0); // No padding for the paragraph

                // Create a cell for the TrashTrack text
                Cell textCell = new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(trashTrackText); // Add the text

                // Add both cells to the logoTextTable
                logoTextTable.AddCell(logoCell);
                logoTextTable.AddCell(textCell);

                // Add the logo and text table to the headerTable
                headerTable.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(logoTextTable)); // Add the table with logo and text

                // Add Company Address to the second cell
                Paragraph address = new Paragraph("Binaliw Cebu Dumpsite\nCebu City, Cebu\nPhilippines")
                    .SetFont(font)
                    .SetTextAlignment(TextAlignment.RIGHT); // Adjust text alignment as needed

                headerTable.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(address));

                // Add the header table to the document
                document.Add(headerTable);

                // Initialize variables with default values to avoid uninitialized errors
                int leadDays = 0;
                int taxValue = 0;
                int accPerValue = 0;
                int susPerValue = 0;
                double totalSum = 0;
                double vat_Amnt = 0;

                DateTime dateIssued = DateTime.Now; // Default value, will be overwritten
                // Database connection and fetching values
                using (var db = new NpgsqlConnection(con))
                {
                    db.Open();
                    // Fetch interest and leadDays values from PostgreSQL
                    string controlQuery = @"SELECT * FROM payment_term";

                    using (var cmd = new NpgsqlCommand(controlQuery, db))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Read values from the database
                                leadDays = Convert.ToInt32(reader["pt_lead_days"]);
                                taxValue = Convert.ToInt32(reader["pt_tax"]);
                                accPerValue = Convert.ToInt32(reader["pt_accrual_period"]);
                                susPerValue = Convert.ToInt32(reader["pt_susp_period"]);
                            }
                            else
                            {
                                // Handle the case where no rows are returned (optional)
                                Console.WriteLine("No payment term data found. Using default values.");
                            }
                        }
                    }
                    // Fetch date issued from generate_bill
                    string billQuery = @"SELECT * FROM generate_bill WHERE gb_id = @BillId"; // Adjust query as needed
                    using (var billCmd = new NpgsqlCommand(billQuery, db))
                    {
                        billCmd.Parameters.AddWithValue("@BillId", buttonText); // Use buttonText for bill ID
                        using (var reader = billCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                dateIssued = Convert.ToDateTime(reader["gb_date_issued"]);
                            }
                            else
                            {
                                // Handle the case where no rows are returned (optional)
                                Console.WriteLine("No payment term data found. Using default values.");
                            }
                        }

                    }


                    string totalQuery = @"SELECT SUM(bw_total_price) FROM booking_waste WHERE bk_id = @BkId";
                    using (var totalCmd = new NpgsqlCommand(totalQuery, db))
                    {
                        totalCmd.Parameters.AddWithValue("@BkId", bkID);
                        object result = totalCmd.ExecuteScalar(); // Execute the query and get the total

                        // Check if result is not null and assign it to totalSum
                        if (result != DBNull.Value)
                        {
                            totalSum += Convert.ToDouble(result);
                            vat_Amnt = totalSum * (taxValue / 100.0);
                        }
                    }
                }

                double totAmnt = vat_Amnt + totalSum;
                DateTime dueDate = dateIssued.AddDays(leadDays);

                // Add Title
                Paragraph title = new Paragraph("Billing Statement")
                    .SetFont(boldFont)
                    .SetFontSize(16)
                    .SetTextAlignment(TextAlignment.CENTER);
                document.Add(title);

                // Add Booking and Bill ID Information without borders
                iText.Layout.Element.Table infoTable = new iText.Layout.Element.Table(2).UseAllAvailableWidth();

                // Remove all borders for each cell and content
                infoTable.SetBorder(Border.NO_BORDER); // Ensure the table itself has no border

                // Bill ID cell
                infoTable.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph($"Bill ID: {buttonText}")
                        .SetFont(boldFont)
                        .SetBorder(Border.NO_BORDER))); // No border for the content

                // Invoice # cell, aligned to the right
                infoTable.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph($"Invoice #: ######")
                        .SetFont(boldFont)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetBorder(Border.NO_BORDER))); // No border for the content

                // Booking ID cell
                infoTable.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph($"Booking ID: {bkID}")
                        .SetFont(boldFont)
                        .SetBorder(Border.NO_BORDER))); // No border for the content

                // Date Issued cell, aligned to the right
                infoTable.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph($"Date Issued: {DateTime.Now.ToString("MM/dd/yyyy")}")
                        .SetFont(boldFont)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetBorder(Border.NO_BORDER))); // No border for the content

                // Empty cell for spacing
                infoTable.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("").SetBorder(Border.NO_BORDER))); // No border for empty content

                // Due Date cell, aligned to the right
                infoTable.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph($"Due Date: {dueDate:MM/dd/yyyy}")
                        .SetFont(boldFont)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetBorder(Border.NO_BORDER))); // No border for the content

                // Add the table to the document
                document.Add(infoTable);


                // Add Terms
                Paragraph termsTitle = new Paragraph("TERMS:")
                    .SetFont(boldFont)
                    .SetFontSize(12);
                document.Add(termsTitle);

                // Create terms content paragraph with formatted strings using default values
                Paragraph termsContent = new Paragraph(
                    $"The bill shall be due for payment and collection ({leadDays}) day/s after issuance. " +
                    $"Failure by the customer to make payment without valid and justifiable reason will result in a late payment charge of ({taxValue}%) " +
                    $"per {accPerValue} day/s applied to any outstanding balance until {susPerValue} day/s. " +
                    $"Additionally, TrashTrack reserves the right to stop collecting waste materials from the customer's premises if payment is not made, " +
                    $"preventing further processing and disposal services."
                )
                .SetFont(font)
                .SetTextAlignment(TextAlignment.JUSTIFIED)
                .SetFontSize(10);

                // Add the terms content to the document
                document.Add(termsContent);



                // Add Waste Details Table
                iText.Layout.Element.Table wasteTable = new iText.Layout.Element.Table(new float[] { 100, 50, 80, 80, 100 }).UseAllAvailableWidth();
                wasteTable.SetMarginTop(20);

                // Add table headers with bottom border
                wasteTable.AddHeaderCell(new Cell()
                    .Add(new Paragraph("Waste Type").SetFont(boldFont))
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetBorderTop(Border.NO_BORDER)
                    .SetBorderLeft(Border.NO_BORDER)
                    .SetBorderRight(Border.NO_BORDER)
                    .SetBorderBottom(new SolidBorder(1f)));  // Add bold bottom border for header

                wasteTable.AddHeaderCell(new Cell()
                    .Add(new Paragraph("Unit").SetFont(boldFont))
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetBorderTop(Border.NO_BORDER)
                    .SetBorderLeft(Border.NO_BORDER)
                    .SetBorderRight(Border.NO_BORDER)
                    .SetBorderBottom(new SolidBorder(1f)));  // Add bold bottom border for header

                wasteTable.AddHeaderCell(new Cell()
                    .Add(new Paragraph("Total Unit").SetFont(boldFont))
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetBorderTop(Border.NO_BORDER)
                    .SetBorderLeft(Border.NO_BORDER)
                    .SetBorderRight(Border.NO_BORDER)
                    .SetBorderBottom(new SolidBorder(1f)));  // Add bold bottom border for header

                wasteTable.AddHeaderCell(new Cell()
                    .Add(new Paragraph("Unit Price").SetFont(boldFont))
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetBorderTop(Border.NO_BORDER)
                    .SetBorderLeft(Border.NO_BORDER)
                    .SetBorderRight(Border.NO_BORDER)
                    .SetBorderBottom(new SolidBorder(1f)));  // Add bold bottom border for header

                wasteTable.AddHeaderCell(new Cell()
                    .Add(new Paragraph("Total Price").SetFont(boldFont))
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetBorderTop(Border.NO_BORDER)
                    .SetBorderLeft(Border.NO_BORDER)
                    .SetBorderRight(Border.NO_BORDER)
                    .SetBorderBottom(new SolidBorder(1f)));  // Add bold bottom border for header




                // Fetch booking_waste entries related to the booking ID
                using (var db = new NpgsqlConnection(con))
                {
                    db.Open();

                    string wasteQuery = @"SELECT bw_name, bw_unit, bw_total_unit, bw_price, bw_total_price 
                                  FROM booking_waste 
                                  WHERE bk_id = @BkId";
                    using (var wasteCmd = new NpgsqlCommand(wasteQuery, db))
                    {
                        wasteCmd.Parameters.AddWithValue("@BkId", bkID);

                        using (var wasteReader = wasteCmd.ExecuteReader())
                        {
                            while (wasteReader.Read())
                            {
                                // Add detail rows without borders
                                wasteTable.AddCell(new Cell()
                                    .Add(new Paragraph(wasteReader["bw_name"].ToString()))
                                    .SetFont(font)
                                    .SetTextAlignment(TextAlignment.LEFT)
                                    .SetBorder(Border.NO_BORDER));

                                wasteTable.AddCell(new Cell()
                                    .Add(new Paragraph(wasteReader["bw_unit"].ToString()))
                                    .SetTextAlignment(TextAlignment.LEFT)
                                    .SetBorder(Border.NO_BORDER));

                                wasteTable.AddCell(new Cell()
                                    .Add(new Paragraph(wasteReader["bw_total_unit"].ToString()))
                                    .SetTextAlignment(TextAlignment.LEFT)
                                    .SetBorder(Border.NO_BORDER));

                                wasteTable.AddCell(new Cell()
                                    .Add(new Paragraph("₱" + wasteReader["bw_price"].ToString()))
                                    .SetFont(font)
                                    .SetTextAlignment(TextAlignment.LEFT)
                                    .SetBorder(Border.NO_BORDER));

                                wasteTable.AddCell(new Cell()
                                    .Add(new Paragraph("₱ " + wasteReader["bw_total_price"].ToString()))
                                    .SetFont(font)
                                    .SetTextAlignment(TextAlignment.LEFT)
                                    .SetBorder(Border.NO_BORDER));
                            }
                        }
                    }

                }

                document.Add(wasteTable);




                // Define the width for the bottom line
                float[] bottomLineWidths = new float[] { 1 }; // Single column for the line
                iText.Layout.Element.Table btmLine = new iText.Layout.Element.Table(bottomLineWidths).UseAllAvailableWidth();

                // Add a cell for the line with a top border
                btmLine.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("") // Empty paragraph
                        .SetBorder(Border.NO_BORDER) // No border for the paragraph
                        .SetBorderTop(new SolidBorder(1f)) // Add solid top border
                    )
                );

                // Add the bottom line table to the document
                document.Add(btmLine);


                float[] columnWidths = new float[] { 100, 40, 30, 80, 100 }; // Set fixed pixel widths
                iText.Layout.Element.Table summarySection = new iText.Layout.Element.Table(columnWidths).UseAllAvailableWidth();


                // Add Bill ID cell (empty for spacing)
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("").SetBorder(Border.NO_BORDER))); // No border for empty content
                // Add Bill ID cell (empty for spacing)
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("").SetBorder(Border.NO_BORDER))); // No border for empty content
                // Add Bill ID cell (empty for spacing)
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("").SetBorder(Border.NO_BORDER))); // No border for empty content
                // Add Bill ID cell (empty for spacing)
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("Net of VAT: ").SetFont(boldFont)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetBorder(Border.NO_BORDER))); // No border for empty content

                // Add Sum Amount cell, aligned to the left
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("₱" + totalSum.ToString("N2"))
                        .SetFont(boldFont)
                        .SetFont(font)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetBorder(Border.NO_BORDER))); // No border for the content

                // Add Bill ID cell (empty for spacing)
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("").SetBorder(Border.NO_BORDER))); // No border for empty content

                // Add Bill ID cell (empty for spacing)
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("").SetBorder(Border.NO_BORDER))); // No border for empty content

                // Add Bill ID cell (empty for spacing)
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("").SetBorder(Border.NO_BORDER))); // No border for empty content

                // Add empty cell for spacing
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("VAT (12%): ")
                        .SetFont(boldFont)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetBorder(Border.NO_BORDER))); // No border for empty content

                // Add VAT Amount cell, aligned to the left
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("₱" + vat_Amnt.ToString("N2"))
                        .SetFont(boldFont)
                        .SetFont(font)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetBorder(Border.NO_BORDER))); // No border for the content

                //// Add Bill ID cell (empty for spacing)
                //summarySection.AddCell(new Cell()
                //    .SetBorder(Border.NO_BORDER) // No border for the cell
                //    .Add(new Paragraph("").SetBorder(Border.NO_BORDER))); // No border for empty content
                //// Add Bill ID cell (empty for spacing)
                //summarySection.AddCell(new Cell()
                //    .SetBorder(Border.NO_BORDER) // No border for the cell
                //    .Add(new Paragraph("").SetBorder(Border.NO_BORDER))); // No border for empty content
                //// Add Bill ID cell (empty for spacing)
                //summarySection.AddCell(new Cell()
                //    .SetBorder(Border.NO_BORDER) // No border for the cell
                //    .Add(new Paragraph("").SetBorder(Border.NO_BORDER))); // No border for empty content


                //// Add empty cell for spacing
                //summarySection.AddCell(new Cell()
                //    .SetBorder(Border.NO_BORDER) // No border for the cell
                //    .Add(new Paragraph("Interest Amount: ")
                //        .SetFont(font)
                //        .SetFontColor(redColor)
                //        .SetTextAlignment(TextAlignment.LEFT)
                //        .SetBorder(Border.NO_BORDER))); // No border for empty content

                //// Add Total Amount cell, aligned to the left
                //summarySection.AddCell(new Cell()
                //    .SetBorder(Border.NO_BORDER) // No border for the cell
                //    .Add(new Paragraph("₱ 20,115.31")
                //        .SetFont(font)
                //        .SetFontColor(redColor)
                //        .SetTextAlignment(TextAlignment.LEFT)
                //        .SetBorder(Border.NO_BORDER))); // No border for the content


                // Add Bill ID cell (empty for spacing)
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("").SetBorder(Border.NO_BORDER))); // No border for empty content
                // Add Bill ID cell (empty for spacing)
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("").SetBorder(Border.NO_BORDER))); // No border for empty content
                // Add Bill ID cell (empty for spacing)
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("").SetBorder(Border.NO_BORDER))); // No border for empty content

                // Add empty cell for spacing
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("Total Amount: ")
                        .SetFont(font)
                        .SetFontColor(redColor)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetBorder(Border.NO_BORDER))); // No border for empty content

                // Add Total Amount cell, aligned to the left
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER) // No border for the cell
                    .Add(new Paragraph("₱" + totAmnt.ToString("N2"))
                        .SetFont(font)
                        .SetFontColor(redColor)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetBorder(Border.NO_BORDER))); // No border for the content

                // Add the summary section table to the document
                document.Add(summarySection);




                // Close document
                document.Close();

                return ms.ToArray();

            }
        }


        //protected void btnOtherAction_Click(object sender, EventArgs e)
        //{
        //    int bookingId = 1; // Hardcoded for testing
        //    byte[] imageData = null;

        //    using (var db = new NpgsqlConnection(con))
        //    {
        //        db.Open();
        //        string query = "SELECT bk_waste_scale_slip FROM booking WHERE bk_id = @id";
        //        using (var cmd = new NpgsqlCommand(query, db))
        //        {
        //            cmd.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Integer, bookingId);
        //            using (var reader = cmd.ExecuteReader())
        //            {
        //                if (reader.Read())
        //                {
        //                    imageData = reader["bk_waste_scale_slip"] as byte[];
        //                }
        //                else
        //                {
        //                    // Handle no data found
        //                    Response.Write("<script>alert('No data found for the specified ID.');</script>");
        //                    return;
        //                }
        //            }
        //        }
        //        db.Close();
        //    }

        //    if (imageData != null && imageData.Length > 0)
        //    {
        //        string base64String = Convert.ToBase64String(imageData);
        //        string script = $"loadImage('data:image/jpeg;base64,{base64String}');";
        //        ScriptManager.RegisterStartupScript(this, GetType(), "LoadImageScript", script, true);
        //    }
        //    else
        //    {
        //        Image2.Attributes["src"] = "~/Pictures/blank_prof.png"; // Default image
        //        Image2.Style["display"] = "block";

        //        // Show the modal without blocking background interaction
        //        ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "showModal();", true);
        //    }
        //}
        //protected void btnOtherAction_Click(object sender, EventArgs e)
        //{
        //    int bookingId = 1; // Hardcoded for testing
        //    byte[] imageData = null;

        //    using (var db = new NpgsqlConnection(con))
        //    {
        //        db.Open();
        //        string query = "SELECT bk_waste_scale_slip FROM booking WHERE bk_id = @id";
        //        using (var cmd = new NpgsqlCommand(query, db))
        //        {
        //            cmd.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Integer, bookingId);
        //            using (var reader = cmd.ExecuteReader())
        //            {
        //                if (reader.Read())
        //                {
        //                    imageData = reader["bk_waste_scale_slip"] as byte[];
        //                }
        //                else
        //                {
        //                    // Handle no data found
        //                    Response.Write("<script>alert('No data found for the specified ID.');</script>");
        //                    return;
        //                }
        //            }
        //        }
        //        db.Close();
        //    }

        //    if (imageData != null && imageData.Length > 0)
        //    {
        //        string base64String = Convert.ToBase64String(imageData);
        //        string imageUrl = $"data:image/jpeg;base64,{base64String}";

        //        // Suggest a filename for download
        //        string filename = "Scale Slip/ScaleSlip.jpg";

        //        // Pass the image URL and filename to JavaScript
        //        string downloadScript = $"downloadImage('{imageUrl}', '{filename}');";
        //        ScriptManager.RegisterStartupScript(this, GetType(), "DownloadImageScript", downloadScript, true);
        //    }
        //    else
        //    {
        //        // Handle case where no image is found
        //        Response.Write("<script>alert('No image found.');</script>");
        //    }
        //}








        //protected void editBookWaste_Click(object sender, EventArgs e)
        //{
        //    LinkButton btn = sender as LinkButton;

        //    //if (btn != null)
        //    //{
        //    //    // Get the bw_id from CommandArgument
        //    //    int bw_id = Convert.ToInt32(btn.CommandArgument);

        //    //    //// Store bw_id in the hidden field for later use
        //    //    hfBwID.Value = bw_id.ToString(); // Store the id in the hidden field

        //    //    //// Display the bw_id from the hidden field in the textbox
        //    //    //txtbwID.Text = hfBwID.Value;
        //    //    try
        //    //    {
        //    //        using (var db = new NpgsqlConnection(con))
        //    //        {
        //    //            db.Open();
        //    //            using (var cmd = db.CreateCommand())
        //    //            {
        //    //                cmd.CommandText = "SELECT * FROM BOOKING_WASTE WHERE bw_id = @bw_id";
        //    //                cmd.Parameters.AddWithValue("bw_id", bw_id);
        //    //                using (NpgsqlDataReader reader = cmd.ExecuteReader())
        //    //                {
        //    //                    if (reader.Read())
        //    //                    {
        //    //                        txtName.Text = reader["bw_name"].ToString();
        //    //                        txtbwUnit.Text = reader["bw_unit"].ToString();
        //    //                        txtTotalUnit.Text = reader["bw_total_unit"].ToString();
        //    //                        txtUnitPrice.Text = reader["bw_price"].ToString();
        //    //                        txtTotalUnitPrice.Text = reader["bw_total_price"].ToString();


        //    //                        //Response.Write("<script>alert('Yeehaw!')</script>");
        //    //                    }
        //    //                }


        //    //            }

        //    //        }
        //    //    }
        //    //    catch
        //    //    {

        //    //    }


        //    //    // Ensure UpdatePanel is refreshed (if needed)
        //    //    updatePanel.Update(); // If using UpdatePanel, force an update to reflect changes

        //    //    // Show the modal popup
        //    //    ModalPopupExtender2.Show();
        //    //}
        //}

        //protected void btnViewSlip_Click(object sender, EventArgs e)
        //{
        //    int bookingId = 1; // Hardcoded for testing
        //    byte[] imageData = null;

        //    using (var db = new NpgsqlConnection(con))
        //    {
        //        db.Open();
        //        string query = "SELECT bk_waste_scale_slip FROM booking WHERE bk_id = @id";
        //        using (var cmd = new NpgsqlCommand(query, db))
        //        {
        //            cmd.Parameters.AddWithValue("@id", NpgsqlTypes.NpgsqlDbType.Integer, bookingId);
        //            using (var reader = cmd.ExecuteReader())
        //            {
        //                if (reader.Read())
        //                {
        //                    imageData = reader["bk_waste_scale_slip"] as byte[];
        //                }
        //                else
        //                {
        //                    // Handle no data found
        //                    Response.Write("<script>alert('No data found for the specified ID.');</script>");
        //                    return;
        //                }
        //            }
        //        }
        //        db.Close();
        //    }

        //    if (imageData != null && imageData.Length > 0)
        //    {
        //        string base64String = Convert.ToBase64String(imageData);
        //        string script = $"loadImage('data:image/jpeg;base64,{base64String}');";
        //        ScriptManager.RegisterStartupScript(this, GetType(), "LoadImageScript", script, true);
        //    }
        //    else
        //    {
        //        Image2.Attributes["src"] = "~/Pictures/blank_prof.png"; // Default image
        //        Image2.Style["display"] = "block";

        //        // Show the modal without blocking background interaction
        //        ScriptManager.RegisterStartupScript(this, GetType(), "ShowModal", "showModal();", true);
        //    }
        //}



    }
}
