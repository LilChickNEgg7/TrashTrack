using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
    public partial class Z_Trial : System.Web.UI.Page
    {
        private readonly string con = "Server=localhost;Port=5432;User Id=postgres;Password=123456;Database=trashtrack";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set the default active tab
                hfActiveTab.Value = "#tab1"; // Set Tab 1 as the default
            }
        }

        protected void btnTab1_Click(object sender, EventArgs e)
        {

        }
    //    protected void btnGenerateBill_Click(object sender, EventArgs e)
    //    {
    //        int empId = (int)Session["bo_id"]; // Retrieve admin ID from session

    //        // Retrieve values from your controls
    //        string note = noteTxt.Text;
    //        double? additionalFees = string.IsNullOrWhiteSpace(addFeeTxt.Text) ? (double?)null : Convert.ToDouble(addFeeTxt.Text);
    //        double? netVat = string.IsNullOrWhiteSpace(netVatTxt.Text) ? (double?)null : Convert.ToDouble(netVatTxt.Text);
    //        double? vatAmount = string.IsNullOrWhiteSpace(vatAmntTxt.Text) ? (double?)null : Convert.ToDouble(vatAmntTxt.Text);
    //        double? totalSales = string.IsNullOrWhiteSpace(totSalesTxt.Text) ? (double?)null : Convert.ToDouble(totSalesTxt.Text);
    //        DateTime? dateIssued = DateTime.TryParse(dateTodayTxt.Text, out DateTime issuedDate) ? (DateTime?)issuedDate : null;
    //        DateTime? dateDue = DateTime.TryParse(dueDateTxt.Text, out DateTime dueDate) ? (DateTime?)dueDate : null;
    //        DateTime? accrualDate = DateTime.TryParse(accDateTxt.Text, out DateTime accDate) ? (DateTime?)accDate : null;
    //        DateTime? suspensionDate = DateTime.TryParse(susDateTxt.Text, out DateTime susDate) ? (DateTime?)susDate : null;

    //        // Variables for payment terms
    //        double? interest = null;
    //        int? leadDays = null;
    //        int? accrualPeriod = null;
    //        int? suspensionPeriod = null;
    //        int? tax = null;

    //        // Retrieve payment term values from the database
    //        using (var conn = new NpgsqlConnection(con)) // Use NpgsqlConnection
    //        {
    //            conn.Open();

    //            string paymentTermQuery = "SELECT pt_interest, pt_lead_days, pt_accrual_period, pt_susp_period, pt_tax FROM payment_term WHERE emp_id = @EmpId";

    //            using (var cmd = new NpgsqlCommand(paymentTermQuery, conn)) // Use NpgsqlCommand
    //            {
    //                cmd.Parameters.AddWithValue("@EmpId", empId); // Ensure emp_id is defined

    //                using (var reader = cmd.ExecuteReader())
    //                {
    //                    if (reader.Read())
    //                    {
    //                        interest = reader.IsDBNull(0) ? (double?)null : reader.GetDouble(0);
    //                        leadDays = reader.IsDBNull(1) ? (int?)null : reader.GetInt32(1);
    //                        accrualPeriod = reader.IsDBNull(2) ? (int?)null : reader.GetInt32(2);
    //                        suspensionPeriod = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3);
    //                        tax = reader.IsDBNull(4) ? (int?)null : reader.GetInt32(4);
    //                    }
    //                }
    //            }
    //        }

    //        int bk_id, cus_id;
    //        string cus_fullname;
    //        // Retrieve payment term values from the database
    //        using (var conn = new NpgsqlConnection(con)) // Use NpgsqlConnection
    //        {
    //            conn.Open();

    //            string findBkID = "SELECT * FROM booking WHERE bk_id = @BkId";

    //            using (var cmd = new NpgsqlCommand(findBkID, conn)) // Use NpgsqlCommand
    //            {
    //                cmd.Parameters.AddWithValue("@BkId", Convert.ToInt32(TextBox1.Text)); // Assuming you get Booking ID from TextBox1

    //                using (var reader = cmd.ExecuteReader())
    //                {
    //                    if (reader.Read())
    //                    {
    //                        bk_id = Convert.ToInt32(reader["bk_id"]); // Get the current role ID
    //                        cus_id = Convert.ToInt32(reader["cus_id"]);
    //                        cus_fullname = reader["bk_fullname"].ToString();
    //                    }
    //                    else
    //                    {
    //                        // Handle case when no `bk_id` is found
    //                        throw new Exception("Booking ID not found.");
    //                    }
    //                }
    //            }
    //        }


    //        int insertedBillId;
    //        // Insert data into the database and get the inserted bill ID
    //        using (var conn = new NpgsqlConnection(con)) // Use NpgsqlConnection
    //        {
    //            conn.Open();

    //            // Use RETURNING to fetch the inserted gb_id
    //            string query = @"
    //        INSERT INTO generate_bill (
    //            gb_note, gb_add_fees, gb_net_vat, gb_vat_amnt, gb_total_sales, 
    //            gb_date_issued, gb_date_due, gb_interest, gb_lead_days, 
    //            gb_accrual_period, gb_suspend_period, gb_accrual_date, 
    //            gb_suspend_date, gb_tax, gb_status, bk_id, emp_id
    //        ) 
    //        VALUES (
    //            @Note, @AddFees, @NetVat, @VatAmount, @TotalSales, 
    //            @DateIssued, @DateDue, @Interest, @LeadDays, 
    //            @AccrualPeriod, @SuspensionPeriod, @AccrualDate, 
    //            @SuspensionDate, @Tax, @Status, @BkId, @EmpId
    //        ) 
    //        RETURNING gb_id;";

    //            using (var cmd = new NpgsqlCommand(query, conn)) // Use NpgsqlCommand
    //            {
    //                cmd.Parameters.AddWithValue("@Note", note ?? (object)DBNull.Value);
    //                cmd.Parameters.AddWithValue("@AddFees", additionalFees ?? (object)DBNull.Value);
    //                cmd.Parameters.AddWithValue("@NetVat", netVat ?? (object)DBNull.Value);
    //                cmd.Parameters.AddWithValue("@VatAmount", vatAmount ?? (object)DBNull.Value);
    //                cmd.Parameters.AddWithValue("@TotalSales", totalSales ?? (object)DBNull.Value);
    //                cmd.Parameters.AddWithValue("@DateIssued", dateIssued ?? (object)DBNull.Value);
    //                cmd.Parameters.AddWithValue("@DateDue", dateDue ?? (object)DBNull.Value);
    //                cmd.Parameters.AddWithValue("@Interest", interest ?? (object)DBNull.Value);
    //                cmd.Parameters.AddWithValue("@LeadDays", leadDays ?? (object)DBNull.Value);
    //                cmd.Parameters.AddWithValue("@AccrualPeriod", accrualPeriod ?? (object)DBNull.Value);
    //                cmd.Parameters.AddWithValue("@SuspensionPeriod", suspensionPeriod ?? (object)DBNull.Value);
    //                cmd.Parameters.AddWithValue("@AccrualDate", accrualDate ?? (object)DBNull.Value);
    //                cmd.Parameters.AddWithValue("@SuspensionDate", suspensionDate ?? (object)DBNull.Value);
    //                cmd.Parameters.AddWithValue("@Tax", tax ?? (object)DBNull.Value);
    //                cmd.Parameters.AddWithValue("@Status", "Pending"); // Set status as needed
    //                cmd.Parameters.AddWithValue("@BkId", Convert.ToInt32(TextBox1.Text)); // Assuming you get Booking ID from TextBox1
    //                cmd.Parameters.AddWithValue("@EmpId", empId); // Set emp_id as needed

    //                // Execute the command and retrieve the inserted bill ID
    //                var result = cmd.ExecuteScalar();
    //                if (result != null)
    //                {
    //                    insertedBillId = Convert.ToInt32(result); // Safe to cast now
    //                }
    //                else
    //                {
    //                    throw new Exception("No ID returned from insert.");
    //                }
    //            }
    //        }


    //        string notif_message = "Your bill is now available for payment. \n\n" +
    //                   "------------------------------------------\n" +
    //                   "BILL# " + insertedBillId + "\n\n" +
    //                   "Dear " + cus_fullname + ",\n\n" +
    //                   "You can now review the details at your convenience. Please check it as soon as possible to avoid any delays. " +
    //                   "Thank you for your cooperation ";

    //        using (var conn = new NpgsqlConnection(con))
    //        {
    //            conn.Open();

    //            // Corrected query with @GbId parameter and no trailing comma
    //            string query = @"
    //INSERT INTO notification (
    //    notif_message, emp_id, cus_id, bk_id, gb_id
    //) 
    //VALUES (
    //    @Message, @EmpId, @CusId, @BkId, @GbId
    //) 
    //RETURNING gb_id;";

    //            using (var cmd = new NpgsqlCommand(query, conn))
    //            {
    //                cmd.Parameters.AddWithValue("@Message", notif_message ?? (object)DBNull.Value);
    //                cmd.Parameters.AddWithValue("@EmpId", empId);
    //                cmd.Parameters.AddWithValue("@CusId", cus_id);
    //                cmd.Parameters.AddWithValue("@BkId", Convert.ToInt32(TextBox1.Text)); // Assuming TextBox1 has booking ID
    //                cmd.Parameters.AddWithValue("@GbId", insertedBillId);

    //                // Execute the command and retrieve the inserted gb_id
    //                var result = cmd.ExecuteScalar();
    //                if (result != null)
    //                {
    //                    insertedBillId = Convert.ToInt32(result); // If you need to use this value elsewhere
    //                }
    //                else
    //                {
    //                    throw new Exception("No ID returned from insert.");
    //                }
    //            }
    //        }






    //        // Generate PDF for the inserted bill.
    //        byte[] pdfBytes = GeneratePDFForRow(insertedBillId, bk_id);

    //        // Send the PDF for download.
    //        Response.Clear();
    //        Response.ContentType = "application/pdf";
    //        Response.AddHeader("content-disposition", $"attachment;filename=Bill_{insertedBillId}.pdf");
    //        Response.Buffer = true;
    //        Response.Cache.SetCacheability(HttpCacheability.NoCache);
    //        Response.BinaryWrite(pdfBytes);
    //        Response.End();
    //    }

    //    private byte[] GeneratePDFForRow(int buttonText, int bkID)
    //    {
    //        using (MemoryStream ms = new MemoryStream())
    //        {
    //            PdfWriter writer = new PdfWriter(ms);
    //            PdfDocument pdf = new PdfDocument(writer);
    //            ITextDocument document = new ITextDocument(pdf);

    //            // Define fonts and colors
    //            //PdfFont font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
    //            PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
    //            DeviceRgb redColor = new DeviceRgb(255, 0, 0);
    //            // Ensure this font file exists in your project folder
    //            string fontPath = Server.MapPath("~/fonts/Roboto/Roboto-Regular.ttf");
    //            // Create the font, specifying Unicode support with "Identity-H"
    //            PdfFont font = PdfFontFactory.CreateFont(fontPath, "Identity-H", PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);



    //            // Create a table with one row and two columns for the logo and address
    //            iText.Layout.Element.Table headerTable = new iText.Layout.Element.Table(new float[] { 1, 3 })
    //                .UseAllAvailableWidth();

    //            // Add company logo (if any)
    //            string logoPath = Server.MapPath("~/Pictures/logo_bgRM.png");
    //            iText.Layout.Element.Image logo = new iText.Layout.Element.Image(ImageDataFactory.Create(logoPath));
    //            logo.ScaleToFit(100, 50); // Scale the logo to fit within defined dimensions

    //            // Create a table for the logo and TrashTrack text
    //            iText.Layout.Element.Table logoTextTable = new iText.Layout.Element.Table(2)
    //                .UseAllAvailableWidth();

    //            // Create the logo cell
    //            Cell logoCell = new Cell()
    //                .SetBorder(Border.NO_BORDER) // No border for the cell
    //                .Add(logo); // Add the logo

    //            // Define the green color
    //            DeviceRgb greenColor = new DeviceRgb(0, 128, 0); // Dark green color

    //            // Create the TrashTrack text without margin and padding
    //            Paragraph trashTrackText = new Paragraph("TrashTrack")
    //                .SetFont(boldFont) // Use bold font for the text
    //                .SetFontSize(30) // Set the font size
    //                .SetFontColor(greenColor) // Set the font color to green
    //                .SetTextAlignment(TextAlignment.LEFT) // Align text left
    //                .SetMargin(0) // No margin for the paragraph
    //                .SetPadding(0); // No padding for the paragraph

    //            // Create a cell for the TrashTrack text
    //            Cell textCell = new Cell()
    //                .SetBorder(Border.NO_BORDER) // No border for the cell
    //                .Add(trashTrackText); // Add the text

    //            // Add both cells to the logoTextTable
    //            logoTextTable.AddCell(logoCell);
    //            logoTextTable.AddCell(textCell);

    //            // Add the logo and text table to the headerTable
    //            headerTable.AddCell(new Cell()
    //                .SetBorder(Border.NO_BORDER) // No border for the cell
    //                .Add(logoTextTable)); // Add the table with logo and text

    //            // Add Company Address to the second cell
    //            Paragraph address = new Paragraph("Binaliw Cebu Dumpsite\nCebu City, Cebu\nPhilippines")
    //                .SetFont(font)
    //                .SetTextAlignment(TextAlignment.RIGHT); // Adjust text alignment as needed

    //            headerTable.AddCell(new Cell()
    //                .SetBorder(Border.NO_BORDER) // No border for the cell
    //                .Add(address));

    //            // Add the header table to the document
    //            document.Add(headerTable);





    //            // Add Title
    //            Paragraph title = new Paragraph("Billing Statement")
    //                .SetFont(boldFont)
    //                .SetFontSize(16)
    //                .SetTextAlignment(TextAlignment.CENTER);
    //            document.Add(title);

    //            // Add Booking and Bill ID Information without borders
    //            iText.Layout.Element.Table infoTable = new iText.Layout.Element.Table(2).UseAllAvailableWidth();

    //            // Remove all borders for each cell and content
    //            infoTable.SetBorder(Border.NO_BORDER); // Ensure the table itself has no border

    //            // Bill ID cell
    //            infoTable.AddCell(new Cell()
    //                .SetBorder(Border.NO_BORDER) // No border for the cell
    //                .Add(new Paragraph($"Bill ID: {buttonText}")
    //                    .SetFont(boldFont)
    //                    .SetBorder(Border.NO_BORDER))); // No border for the content

    //            // Invoice # cell, aligned to the right
    //            infoTable.AddCell(new Cell()
    //                .SetBorder(Border.NO_BORDER) // No border for the cell
    //                .Add(new Paragraph($"Invoice #: ######")
    //                    .SetFont(boldFont)
    //                    .SetTextAlignment(TextAlignment.RIGHT)
    //                    .SetBorder(Border.NO_BORDER))); // No border for the content

    //            // Booking ID cell
    //            infoTable.AddCell(new Cell()
    //                .SetBorder(Border.NO_BORDER) // No border for the cell
    //                .Add(new Paragraph($"Booking ID: {bkID}")
    //                    .SetFont(boldFont)
    //                    .SetBorder(Border.NO_BORDER))); // No border for the content

    //            // Date Issued cell, aligned to the right
    //            infoTable.AddCell(new Cell()
    //                .SetBorder(Border.NO_BORDER) // No border for the cell
    //                .Add(new Paragraph($"Date Issued: {DateTime.Now.ToString("MM/dd/yyyy")}")
    //                    .SetFont(boldFont)
    //                    .SetTextAlignment(TextAlignment.RIGHT)
    //                    .SetBorder(Border.NO_BORDER))); // No border for the content

    //            // Empty cell for spacing
    //            infoTable.AddCell(new Cell()
    //                .SetBorder(Border.NO_BORDER) // No border for the cell
    //                .Add(new Paragraph("").SetBorder(Border.NO_BORDER))); // No border for empty content

    //            // Due Date cell, aligned to the right
    //            infoTable.AddCell(new Cell()
    //                .SetBorder(Border.NO_BORDER) // No border for the cell
    //                .Add(new Paragraph($"Due Date: 11/07/2024")
    //                    .SetFont(boldFont)
    //                    .SetTextAlignment(TextAlignment.RIGHT)
    //                    .SetBorder(Border.NO_BORDER))); // No border for the content

    //            // Add the table to the document
    //            document.Add(infoTable);




    //            // Initialize variables with default values to avoid uninitialized errors
    //            int daysValue = 0;
    //            int taxValue = 0;
    //            int accPerValue = 0;
    //            int susPerValue = 0;

    //            // Database connection and fetching values
    //            using (var db = new NpgsqlConnection(con))
    //            {
    //                db.Open();
    //                // Fetch interest and leadDays values from PostgreSQL
    //                string controlQuery = @"SELECT * FROM payment_term";

    //                using (var cmd = new NpgsqlCommand(controlQuery, db))
    //                {
    //                    using (var reader = cmd.ExecuteReader())
    //                    {
    //                        if (reader.Read())
    //                        {
    //                            // Read values from the database
    //                            daysValue = Convert.ToInt32(reader["pt_lead_days"]);
    //                            taxValue = Convert.ToInt32(reader["pt_tax"]);
    //                            accPerValue = Convert.ToInt32(reader["pt_accrual_period"]);
    //                            susPerValue = Convert.ToInt32(reader["pt_susp_period"]);
    //                        }
    //                        else
    //                        {
    //                            // Handle the case where no rows are returned (optional)
    //                            Console.WriteLine("No payment term data found. Using default values.");
    //                        }
    //                    }
    //                }
    //            }

    //            // Add Terms
    //            Paragraph termsTitle = new Paragraph("TERMS:")
    //                .SetFont(boldFont)
    //                .SetFontSize(12);
    //            document.Add(termsTitle);

    //            // Create terms content paragraph with formatted strings using default values
    //            Paragraph termsContent = new Paragraph(
    //                $"The bill shall be due for payment and collection ({daysValue}) day/s after issuance. " +
    //                $"Failure by the customer to make payment without valid and justifiable reason will result in a late payment charge of ({taxValue}%) " +
    //                $"per {accPerValue} day/s applied to any outstanding balance until {susPerValue} day/s. " +
    //                $"Additionally, TrashTrack reserves the right to stop collecting waste materials from the customer's premises if payment is not made, " +
    //                $"preventing further processing and disposal services."
    //            )
    //            .SetFont(font)
    //            .SetTextAlignment(TextAlignment.JUSTIFIED)
    //            .SetFontSize(10);

    //            // Add the terms content to the document
    //            document.Add(termsContent);



    //            // Add Waste Details Table
    //            iText.Layout.Element.Table wasteTable = new iText.Layout.Element.Table(new float[] { 100, 50, 80, 80, 100 }).UseAllAvailableWidth();
    //            wasteTable.SetMarginTop(20);

    //            // Add table headers with bottom border
    //            wasteTable.AddHeaderCell(new Cell()
    //                .Add(new Paragraph("Waste Type").SetFont(boldFont))
    //                .SetTextAlignment(TextAlignment.LEFT)
    //                .SetBorderTop(Border.NO_BORDER)
    //                .SetBorderLeft(Border.NO_BORDER)
    //                .SetBorderRight(Border.NO_BORDER)
    //                .SetBorderBottom(new SolidBorder(1f)));  // Add bold bottom border for header

    //            wasteTable.AddHeaderCell(new Cell()
    //                .Add(new Paragraph("Unit").SetFont(boldFont))
    //                .SetTextAlignment(TextAlignment.LEFT)
    //                .SetBorderTop(Border.NO_BORDER)
    //                .SetBorderLeft(Border.NO_BORDER)
    //                .SetBorderRight(Border.NO_BORDER)
    //                .SetBorderBottom(new SolidBorder(1f)));  // Add bold bottom border for header

    //            wasteTable.AddHeaderCell(new Cell()
    //                .Add(new Paragraph("Total Unit").SetFont(boldFont))
    //                .SetTextAlignment(TextAlignment.LEFT)
    //                .SetBorderTop(Border.NO_BORDER)
    //                .SetBorderLeft(Border.NO_BORDER)
    //                .SetBorderRight(Border.NO_BORDER)
    //                .SetBorderBottom(new SolidBorder(1f)));  // Add bold bottom border for header

    //            wasteTable.AddHeaderCell(new Cell()
    //                .Add(new Paragraph("Unit Price").SetFont(boldFont))
    //                .SetTextAlignment(TextAlignment.LEFT)
    //                .SetBorderTop(Border.NO_BORDER)
    //                .SetBorderLeft(Border.NO_BORDER)
    //                .SetBorderRight(Border.NO_BORDER)
    //                .SetBorderBottom(new SolidBorder(1f)));  // Add bold bottom border for header

    //            wasteTable.AddHeaderCell(new Cell()
    //                .Add(new Paragraph("Total Price").SetFont(boldFont))
    //                .SetTextAlignment(TextAlignment.LEFT)
    //                .SetBorderTop(Border.NO_BORDER)
    //                .SetBorderLeft(Border.NO_BORDER)
    //                .SetBorderRight(Border.NO_BORDER)
    //                .SetBorderBottom(new SolidBorder(1f)));  // Add bold bottom border for header




    //            // Fetch booking_waste entries related to the booking ID
    //            using (var db = new NpgsqlConnection(con))
    //            {
    //                db.Open();

    //                string wasteQuery = @"SELECT bw_name, bw_unit, bw_total_unit, bw_price, bw_total_price 
    //                              FROM booking_waste 
    //                              WHERE bk_id = @BkId";
    //                using (var wasteCmd = new NpgsqlCommand(wasteQuery, db))
    //                {
    //                    wasteCmd.Parameters.AddWithValue("@BkId", bkID);

    //                    using (var wasteReader = wasteCmd.ExecuteReader())
    //                    {
    //                        while (wasteReader.Read())
    //                        {
    //                            // Add detail rows without borders
    //                            wasteTable.AddCell(new Cell()
    //                                .Add(new Paragraph(wasteReader["bw_name"].ToString()))
    //                                .SetFont(font)
    //                                .SetTextAlignment(TextAlignment.LEFT)
    //                                .SetBorder(Border.NO_BORDER));

    //                            wasteTable.AddCell(new Cell()
    //                                .Add(new Paragraph(wasteReader["bw_unit"].ToString()))
    //                                .SetTextAlignment(TextAlignment.LEFT)
    //                                .SetBorder(Border.NO_BORDER));

    //                            wasteTable.AddCell(new Cell()
    //                                .Add(new Paragraph(wasteReader["bw_total_unit"].ToString()))
    //                                .SetTextAlignment(TextAlignment.LEFT)
    //                                .SetBorder(Border.NO_BORDER));

    //                            wasteTable.AddCell(new Cell()
    //                                .Add(new Paragraph("₱" + wasteReader["bw_price"].ToString()))
    //                                .SetFont(font)
    //                                .SetTextAlignment(TextAlignment.LEFT)
    //                                .SetBorder(Border.NO_BORDER));

    //                            wasteTable.AddCell(new Cell()
    //                                .Add(new Paragraph("₱ " + wasteReader["bw_total_price"].ToString()))
    //                                .SetFont(font)
    //                                .SetTextAlignment(TextAlignment.LEFT)
    //                                .SetBorder(Border.NO_BORDER));
    //                        }
    //                    }
    //                }
    //            }

    //            document.Add(wasteTable);




    //            // Define the width for the bottom line
    //            float[] bottomLineWidths = new float[] { 1 }; // Single column for the line
    //            iText.Layout.Element.Table btmLine = new iText.Layout.Element.Table(bottomLineWidths).UseAllAvailableWidth();

    //            // Add a cell for the line with a top border
    //            btmLine.AddCell(new Cell()
    //                .SetBorder(Border.NO_BORDER) // No border for the cell
    //                .Add(new Paragraph("") // Empty paragraph
    //                    .SetBorder(Border.NO_BORDER) // No border for the paragraph
    //                    .SetBorderTop(new SolidBorder(1f)) // Add solid top border
    //                )
    //            );

    //            // Add the bottom line table to the document
    //            document.Add(btmLine);


    //            float[] columnWidths = new float[] { 100, 40, 30, 80, 100 }; // Set fixed pixel widths
    //            iText.Layout.Element.Table summarySection = new iText.Layout.Element.Table(columnWidths).UseAllAvailableWidth();


    //            // Add Bill ID cell (empty for spacing)
    //            summarySection.AddCell(new Cell()
    //                .SetBorder(Border.NO_BORDER) // No border for the cell
    //                .Add(new Paragraph("").SetBorder(Border.NO_BORDER))); // No border for empty content
    //            // Add Bill ID cell (empty for spacing)
    //            summarySection.AddCell(new Cell()
    //                .SetBorder(Border.NO_BORDER) // No border for the cell
    //                .Add(new Paragraph("").SetBorder(Border.NO_BORDER))); // No border for empty content
    //            // Add Bill ID cell (empty for spacing)
    //            summarySection.AddCell(new Cell()
    //                .SetBorder(Border.NO_BORDER) // No border for the cell
    //                .Add(new Paragraph("").SetBorder(Border.NO_BORDER))); // No border for empty content
    //            // Add Bill ID cell (empty for spacing)
    //            summarySection.AddCell(new Cell()
    //                .SetBorder(Border.NO_BORDER) // No border for the cell
    //                .Add(new Paragraph("Net of VAT: ").SetFont(boldFont)
    //                    .SetTextAlignment(TextAlignment.LEFT)
    //                    .SetBorder(Border.NO_BORDER))); // No border for empty content

    //            // Add Sum Amount cell, aligned to the left
    //            summarySection.AddCell(new Cell()
    //                .SetBorder(Border.NO_BORDER) // No border for the cell
    //                .Add(new Paragraph("17,960.10")
    //                    .SetFont(boldFont)
    //                    .SetTextAlignment(TextAlignment.LEFT)
    //                    .SetBorder(Border.NO_BORDER))); // No border for the content

    //            // Add Bill ID cell (empty for spacing)
    //            summarySection.AddCell(new Cell()
    //                .SetBorder(Border.NO_BORDER) // No border for the cell
    //                .Add(new Paragraph("").SetBorder(Border.NO_BORDER))); // No border for empty content

    //            // Add Bill ID cell (empty for spacing)
    //            summarySection.AddCell(new Cell()
    //                .SetBorder(Border.NO_BORDER) // No border for the cell
    //                .Add(new Paragraph("").SetBorder(Border.NO_BORDER))); // No border for empty content

    //            // Add Bill ID cell (empty for spacing)
    //            summarySection.AddCell(new Cell()
    //                .SetBorder(Border.NO_BORDER) // No border for the cell
    //                .Add(new Paragraph("").SetBorder(Border.NO_BORDER))); // No border for empty content

    //            // Add empty cell for spacing
    //            summarySection.AddCell(new Cell()
    //                .SetBorder(Border.NO_BORDER) // No border for the cell
    //                .Add(new Paragraph("VAT (12%): ").SetFont(boldFont)
    //                    .SetTextAlignment(TextAlignment.LEFT)
    //                    .SetBorder(Border.NO_BORDER))); // No border for empty content

    //            // Add VAT Amount cell, aligned to the left
    //            summarySection.AddCell(new Cell()
    //                .SetBorder(Border.NO_BORDER) // No border for the cell
    //                .Add(new Paragraph("₱ 2,155.21")
    //                    .SetFont(boldFont)
    //                    .SetFont(font)
    //                    .SetTextAlignment(TextAlignment.LEFT)
    //                    .SetBorder(Border.NO_BORDER))); // No border for the content

    //            // Add Bill ID cell (empty for spacing)
    //            summarySection.AddCell(new Cell()
    //                .SetBorder(Border.NO_BORDER) // No border for the cell
    //                .Add(new Paragraph("").SetBorder(Border.NO_BORDER))); // No border for empty content
    //            // Add Bill ID cell (empty for spacing)
    //            summarySection.AddCell(new Cell()
    //                .SetBorder(Border.NO_BORDER) // No border for the cell
    //                .Add(new Paragraph("").SetBorder(Border.NO_BORDER))); // No border for empty content
    //            // Add Bill ID cell (empty for spacing)
    //            summarySection.AddCell(new Cell()
    //                .SetBorder(Border.NO_BORDER) // No border for the cell
    //                .Add(new Paragraph("").SetBorder(Border.NO_BORDER))); // No border for empty content

    //            // Add empty cell for spacing
    //            summarySection.AddCell(new Cell()
    //                .SetBorder(Border.NO_BORDER) // No border for the cell
    //                .Add(new Paragraph("Total Amount: ")
    //                    .SetFont(font)
    //                    .SetFontColor(redColor)
    //                    .SetTextAlignment(TextAlignment.LEFT)
    //                    .SetBorder(Border.NO_BORDER))); // No border for empty content

    //            // Add Total Amount cell, aligned to the left
    //            summarySection.AddCell(new Cell()
    //                .SetBorder(Border.NO_BORDER) // No border for the cell
    //                .Add(new Paragraph("₱ 20,115.31")
    //                    .SetFont(font)
    //                    .SetFontColor(redColor)
    //                    .SetTextAlignment(TextAlignment.LEFT)
    //                    .SetBorder(Border.NO_BORDER))); // No border for the content

    //            // Add the summary section table to the document
    //            document.Add(summarySection);




    //            // Close document
    //            document.Close();

    //            return ms.ToArray();
    //        }
    //    }


    }
}