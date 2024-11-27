using AjaxControlToolkit;
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
using AjaxControlToolkit;
using System.EnterpriseServices;
using System.Web.Services.Description;

namespace Capstone
{
    /// <summary>
    /// Summary description for DownloadPdfHandler
    /// </summary>
    public class DownloadPdfHandler : IHttpHandler
    {
        private readonly string con = "Server=localhost;Port=5432;User Id=postgres;Password=123456;Database=trashtrack";

        public void ProcessRequest(HttpContext context)
        {
            int insertedBillId = int.Parse(context.Request.QueryString["billId"]);
            int bkId = int.Parse(context.Request.QueryString["bkId"]);

            // Generate PDF bytes
            byte[] pdfBytes = GeneratePDFForRow(insertedBillId, bkId);

            // Send the PDF for download
            context.Response.Clear();
            context.Response.ContentType = "application/pdf";
            context.Response.AddHeader("content-disposition",
                $"attachment;filename=Bill_{insertedBillId}.pdf");
            context.Response.BinaryWrite(pdfBytes);
            context.Response.End();
        }
        public bool IsReusable => false;

        private byte[] GeneratePDFForRow(int buttonText, int bkID)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(ms);
                PdfDocument pdf = new PdfDocument(writer);
                ITextDocument document = new ITextDocument(pdf);

                // Define fonts and colors
                PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                DeviceRgb redColor = new DeviceRgb(255, 0, 0);
                string fontPath = HttpContext.Current.Server.MapPath("~/fonts/Roboto/Roboto-Regular.ttf");
                PdfFont font = PdfFontFactory.CreateFont(fontPath, "Identity-H", PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);

                

                // Create a table with one row and two columns for the logo and address
                iText.Layout.Element.Table headerTable = new iText.Layout.Element.Table(new float[] { 1, 3 })
                    .UseAllAvailableWidth();

                // Add company logo (if any)
                string logoPath = HttpContext.Current.Server.MapPath("~/Pictures/logo_bgRM.png");
                iText.Layout.Element.Image logo = new iText.Layout.Element.Image(ImageDataFactory.Create(logoPath));
                logo.ScaleToFit(100, 50);

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
                    .SetFont(boldFont)
                    .SetFontSize(30)
                    .SetFontColor(greenColor)
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetMargin(0)
                    .SetPadding(0);

                // Create a cell for the TrashTrack text
                Cell textCell = new Cell()
                    .SetBorder(Border.NO_BORDER)
                    .Add(trashTrackText);

                // Add both cells to the logoTextTable
                logoTextTable.AddCell(logoCell);
                logoTextTable.AddCell(textCell);

                // Add the logo and text table to the headerTable
                headerTable.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER)
                    .Add(logoTextTable));

                // Add Company Address to the second cell
                Paragraph address = new Paragraph("Binaliw Cebu Dumpsite\nCebu City, Cebu\nPhilippines")
                    .SetFont(font)
                    .SetTextAlignment(TextAlignment.RIGHT);

                headerTable.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER)
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
                double netVat = 0;
                double addFee = 0;
                DateTime dateIssued = DateTime.Now;
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
                    string billQuery = @"SELECT * FROM generate_bill WHERE gb_id = @BillId";
                    using (var billCmd = new NpgsqlCommand(billQuery, db))
                    {
                        billCmd.Parameters.AddWithValue("@BillId", buttonText);
                        using (var reader = billCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                dateIssued = Convert.ToDateTime(reader["gb_date_issued"]);
                                addFee = reader["gb_add_fees"] != DBNull.Value ? Convert.ToDouble(reader["gb_add_fees"]) : 0;
                                //vat_Amnt = Convert.ToDouble(reader[""]);
                                //totalSum = Convert.ToDouble(reader[""]);

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
                            vat_Amnt = (totalSum + addFee) * (taxValue / 100.0);
                            
                        }
                    }
                }
                netVat = totalSum + addFee;
                double totAmnt = vat_Amnt + netVat;
                DateTime dueDate = dateIssued.AddDays(leadDays);

                // Add Title
                Paragraph title = new Paragraph("Billing Statement")
                    .SetFont(boldFont)
                    .SetFontSize(16)
                    .SetTextAlignment(TextAlignment.CENTER);
                document.Add(title);

                iText.Layout.Element.Table infoTable = new iText.Layout.Element.Table(2).UseAllAvailableWidth();

                // Remove all borders for each cell and content
                infoTable.SetBorder(Border.NO_BORDER);

                // Bill ID cell
                infoTable.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER)
                    .Add(new Paragraph($"Bill ID: {buttonText}")
                        .SetFont(boldFont)
                        .SetBorder(Border.NO_BORDER)));

                // Invoice # cell, aligned to the right
                infoTable.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER)
                    .Add(new Paragraph($"Invoice #: ######")
                        .SetFont(boldFont)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetBorder(Border.NO_BORDER)));

                // Booking ID cell
                infoTable.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER)
                    .Add(new Paragraph($"Booking ID: {bkID}")
                        .SetFont(boldFont)
                        .SetBorder(Border.NO_BORDER)));

                // Date Issued cell, aligned to the right
                infoTable.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER)
                    .Add(new Paragraph($"Date Issued: {DateTime.Now.ToString("MM/dd/yyyy")}")
                        .SetFont(boldFont)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetBorder(Border.NO_BORDER)));

                // Empty cell for spacing
                infoTable.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER)
                    .Add(new Paragraph("").SetBorder(Border.NO_BORDER)));

                // Due Date cell, aligned to the right
                infoTable.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER)
                    .Add(new Paragraph($"Due Date: {dueDate:MM/dd/yyyy}")
                        .SetFont(boldFont)
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .SetBorder(Border.NO_BORDER)));

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
                    .SetBorderBottom(new SolidBorder(1f)));

                wasteTable.AddHeaderCell(new Cell()
                    .Add(new Paragraph("Unit").SetFont(boldFont))
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetBorderTop(Border.NO_BORDER)
                    .SetBorderLeft(Border.NO_BORDER)
                    .SetBorderRight(Border.NO_BORDER)
                    .SetBorderBottom(new SolidBorder(1f)));

                wasteTable.AddHeaderCell(new Cell()
                    .Add(new Paragraph("Total Unit").SetFont(boldFont))
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetBorderTop(Border.NO_BORDER)
                    .SetBorderLeft(Border.NO_BORDER)
                    .SetBorderRight(Border.NO_BORDER)
                    .SetBorderBottom(new SolidBorder(1f)));

                wasteTable.AddHeaderCell(new Cell()
                    .Add(new Paragraph("Unit Price").SetFont(boldFont))
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetBorderTop(Border.NO_BORDER)
                    .SetBorderLeft(Border.NO_BORDER)
                    .SetBorderRight(Border.NO_BORDER)
                    .SetBorderBottom(new SolidBorder(1f)));

                wasteTable.AddHeaderCell(new Cell()
                    .Add(new Paragraph("Total Price").SetFont(boldFont))
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetBorderTop(Border.NO_BORDER)
                    .SetBorderLeft(Border.NO_BORDER)
                    .SetBorderRight(Border.NO_BORDER)
                    .SetBorderBottom(new SolidBorder(1f)));



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
                    .SetBorder(Border.NO_BORDER)
                    .Add(new Paragraph("")
                        .SetBorder(Border.NO_BORDER)
                        .SetBorderTop(new SolidBorder(1f))
                    )
                );
                //netVat
                // Add the bottom line table to the document
                document.Add(btmLine);


                float[] columnWidths = new float[] { 100, 40, 30, 80, 100 }; // Set fixed pixel widths
                iText.Layout.Element.Table summarySection = new iText.Layout.Element.Table(columnWidths).UseAllAvailableWidth();

                // Add Bill ID cell (empty for spacing)
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER)
                    .Add(new Paragraph("").SetBorder(Border.NO_BORDER)));
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER)
                    .Add(new Paragraph("").SetBorder(Border.NO_BORDER)));
                // Add Bill ID cell (empty for spacing)
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER)
                    .Add(new Paragraph("").SetBorder(Border.NO_BORDER)));
                // Add Bill ID cell (empty for spacing)
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER)
                    .Add(new Paragraph("Total Sum: ").SetFont(boldFont)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetBorder(Border.NO_BORDER)));
                //totAmnt, totAmnt + addFee;
                // Add Sum Amount cell, aligned to the left
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER)
                    .Add(new Paragraph("₱" + totalSum.ToString("N2"))
                        .SetFont(boldFont)
                        .SetFont(font)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetBorder(Border.NO_BORDER)));



                // Add Bill ID cell (empty for spacing)
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER)
                    .Add(new Paragraph("").SetBorder(Border.NO_BORDER)));
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER)
                    .Add(new Paragraph("").SetBorder(Border.NO_BORDER)));
                // Add Bill ID cell (empty for spacing)
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER)
                    .Add(new Paragraph("").SetBorder(Border.NO_BORDER)));
                // Add Bill ID cell (empty for spacing)
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER)
                    .Add(new Paragraph("Added Fee: ").SetFont(boldFont)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetBorder(Border.NO_BORDER)));
                //totAmnt, totAmnt + addFee;
                // Add Sum Amount cell, aligned to the left
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER)
                    .Add(new Paragraph("₱" + addFee.ToString("N2"))
                        .SetFont(boldFont)
                        .SetFont(font)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetBorder(Border.NO_BORDER)));


                // Add Bill ID cell (empty for spacing)
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER)
                    .Add(new Paragraph("").SetBorder(Border.NO_BORDER)));
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER)
                    .Add(new Paragraph("").SetBorder(Border.NO_BORDER)));
                // Add Bill ID cell (empty for spacing)
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER)
                    .Add(new Paragraph("").SetBorder(Border.NO_BORDER)));
                // Add Bill ID cell (empty for spacing)
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER)
                    .Add(new Paragraph("Net of VAT: ").SetFont(boldFont)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetBorder(Border.NO_BORDER)));

                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER)
                    .Add(new Paragraph("₱" + netVat.ToString("N2"))
                        .SetFont(boldFont)
                        .SetFont(font)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetBorder(Border.NO_BORDER)));

                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER)
                    .Add(new Paragraph("").SetBorder(Border.NO_BORDER)));

                // Add Bill ID cell (empty for spacing)
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER)
                    .Add(new Paragraph("").SetBorder(Border.NO_BORDER)));

                // Add Bill ID cell (empty for spacing)
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER)
                    .Add(new Paragraph("").SetBorder(Border.NO_BORDER)));

                // Add empty cell for spacing
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER)
                    .Add(new Paragraph("VAT (12%): ")
                        .SetFont(boldFont)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetBorder(Border.NO_BORDER)));

                // Add VAT Amount cell, aligned to the left
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER)
                    .Add(new Paragraph("₱" + vat_Amnt.ToString("N2"))
                        .SetFont(boldFont)
                        .SetFont(font)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetBorder(Border.NO_BORDER)));


                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER)
                    .Add(new Paragraph("").SetBorder(Border.NO_BORDER)));
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER)
                    .Add(new Paragraph("").SetBorder(Border.NO_BORDER)));
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER)
                    .Add(new Paragraph("").SetBorder(Border.NO_BORDER)));

                // Add empty cell for spacing
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER)
                    .Add(new Paragraph("Total Amount: ")
                        .SetFont(boldFont)
                        .SetFontColor(redColor)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetBorder(Border.NO_BORDER)));

                // Add Total Amount cell, aligned to the left
                summarySection.AddCell(new Cell()
                    .SetBorder(Border.NO_BORDER)
                    .Add(new Paragraph("₱" + totAmnt.ToString("N2"))
                        .SetFont(font)
                        .SetFontColor(redColor)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetBorder(Border.NO_BORDER)));

                // Add the summary section table to the document
                document.Add(summarySection);


                

                // Close document
                document.Close();

                return ms.ToArray();

            }

        }


    }
}