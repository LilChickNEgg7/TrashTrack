using Npgsql;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Capstone
{
    public class PaymentController : ApiController
    {
        private readonly string con = "Server=localhost;Port=5432;User Id=postgres;Password=123456;Database=trashtrack";

        //// GET api/payment/status
        //[HttpGet]
        //[Route("api/payment/status")]
        //public IHttpActionResult GetPaymentStatus()
        //{
        //    using (var db = new NpgsqlConnection(con))
        //    {
        //        db.Open();

        //        // Retrieve counts for paid and unpaid payments
        //        var cmd = new NpgsqlCommand("SELECT COUNT(*) FILTER (WHERE p_status = 'paid') AS paidCount, COUNT(*) FILTER (WHERE p_status = 'unpaid') AS unpaidCount FROM payment", db);
        //        using (var reader = cmd.ExecuteReader())
        //        {
        //            if (reader.Read())
        //            {
        //                int paidCount = reader.GetInt32(0);
        //                int unpaidCount = reader.GetInt32(1);

        //                // Return the counts in a JSON response
        //                return Ok(new { paidCount, unpaidCount });
        //            }
        //        }
        //    }
        //    return InternalServerError(); // Return an internal server error if something goes wrong
        //}
        //// GET api/payment/monthlyTotals
        //[HttpGet]
        //[Route("api/payment/monthlyTotals")]
        //public IHttpActionResult GetMonthlyTotals()
        //{
        //    decimal[] monthlyTotalSalesPaid = new decimal[12];
        //    decimal[] monthlyTotalSalesUnpaid = new decimal[12];

        //    using (var db = new NpgsqlConnection(con))
        //    {
        //        db.Open();

        //        for (int month = 0; month < 12; month++)
        //        {
        //            DateTime startDate = new DateTime(DateTime.Now.Year, month + 1, 1);
        //            DateTime endDate = startDate.AddMonths(1);

        //            monthlyTotalSalesPaid[month] = GetTotalSales(db, startDate, endDate, "paid");
        //            monthlyTotalSalesUnpaid[month] = GetTotalSales(db, startDate, endDate, "unpaid");
        //        }
        //    }

        //    return Ok(new { monthlyTotalSalesPaid, monthlyTotalSalesUnpaid });
        //}


        // Helper method to get total sales
        //private decimal GetTotalSales(NpgsqlConnection conn, DateTime startDate, DateTime endDate, string billStatus)
        //{
        //    decimal totalBill = 0.00m;
        //    string query = "SELECT SUM(p_amount) FROM public.payment WHERE p_updated_at >= @startDate AND p_updated_at < @endDate AND p_status = @billStatus";

        //    using (var cmd = new NpgsqlCommand(query, conn))
        //    {
        //        cmd.Parameters.AddWithValue("startDate", startDate);
        //        cmd.Parameters.AddWithValue("endDate", endDate);
        //        cmd.Parameters.AddWithValue("billStatus", billStatus);

        //        var result = cmd.ExecuteScalar();

        //        if (result != DBNull.Value)
        //        {
        //            totalBill = Convert.ToDecimal(result);
        //        }
        //    }

        //    return totalBill;
        //}

        //        // GET api/payment/monthlyTotals
        //        [HttpGet]
        //        [Route("api/payment/monthlyTotals")]
        //        public IHttpActionResult GetMonthlyTotals()
        //        {
        //            decimal[] monthlyPaidPayments = new decimal[12];  // Store monthly paid totals
        //            decimal[] monthlyUnpaidPayments = new decimal[12];  // Store monthly unpaid totals

        //            // Database connection string
        //            string connString = "Server=localhost;Port=5432;User Id=postgres;Password=123456;Database=trashtrack";

        //            using (var db = new NpgsqlConnection(connString))
        //            {
        //                db.Open();

        //                DateTime currentDate = DateTime.Now; // Get the current date

        //                // Loop through each month of the current year
        //                for (int month = 0; month < 12; month++)
        //                {
        //                    DateTime startDate = new DateTime(currentDate.Year, month + 1, 1);
        //                    DateTime endDate = startDate.AddMonths(1);

        //                    // Get paid payments for the month
        //                    monthlyPaidPayments[month] = GetPaidPayments(db, startDate, endDate);

        //                    // Get unpaid payments for the month
        //                    monthlyUnpaidPayments[month] = GetUnpaidPayments(db, currentDate);
        //                }
        //            }

        //            // Return the data as JSON to the client-side
        //            return Ok(new { monthlyPaidPayments, monthlyUnpaidPayments });
        //        }

        //        // Helper method to get paid payments
        //        private decimal GetPaidPayments(NpgsqlConnection conn, DateTime startDate, DateTime endDate)
        //        {
        //            decimal totalPaidAmount = 0.00m;

        //            string query = @"
        //SELECT SUM(p_amount) 
        //FROM public.payment 
        //WHERE p_status = 'paid' 
        //AND p_created_at >= @startDate 
        //AND p_created_at < @endDate";

        //            using (var cmd = new NpgsqlCommand(query, conn))
        //            {
        //                cmd.Parameters.AddWithValue("startDate", startDate);
        //                cmd.Parameters.AddWithValue("endDate", endDate);

        //                var result = cmd.ExecuteScalar();

        //                if (result != DBNull.Value)
        //                {
        //                    totalPaidAmount = Convert.ToDecimal(result);
        //                }
        //            }

        //            return totalPaidAmount;
        //        }

        //        // Helper method to get unpaid payments
        //        private decimal GetUnpaidPayments(NpgsqlConnection conn, DateTime currentDate)
        //        {
        //            decimal totalUnpaidAmount = 0.00m;

        //            string query = @"
        //SELECT gb_id, gb_total_sales, gb_interest, gb_date_due, gb_accrual_period, 
        //       gb_suspend_period, gb_accrual_date, gb_suspend_date 
        //FROM public.generate_bill 
        //WHERE gb_status = 'unpaid' AND gb_date_due <= @currentDate";

        //            using (var cmd = new NpgsqlCommand(query, conn))
        //            {
        //                cmd.Parameters.AddWithValue("currentDate", currentDate);

        //                using (var reader = cmd.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        decimal totalSales = reader.GetDecimal(reader.GetOrdinal("gb_total_sales"));
        //                        decimal interestRate = reader.GetDecimal(reader.GetOrdinal("gb_interest"));
        //                        DateTime dueDate = reader.GetDateTime(reader.GetOrdinal("gb_date_due"));
        //                        int accrualPeriod = reader.GetInt32(reader.GetOrdinal("gb_accrual_period"));
        //                        int suspendPeriod = reader.GetInt32(reader.GetOrdinal("gb_suspend_period"));
        //                        DateTime accrualDate = reader.GetDateTime(reader.GetOrdinal("gb_accrual_date"));
        //                        DateTime? suspendDate = reader.IsDBNull(reader.GetOrdinal("gb_suspend_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("gb_suspend_date"));

        //                        // Apply interest if current date is past the due date
        //                        if (currentDate > dueDate)
        //                        {
        //                            decimal interestAmount = (totalSales * interestRate / 100);
        //                            totalSales += interestAmount;
        //                        }

        //                        // Accrue interest for each accrual period
        //                        while (currentDate >= accrualDate && (suspendDate == null || currentDate <= suspendDate))
        //                        {
        //                            decimal interestAmount = (totalSales * interestRate / 100);
        //                            totalSales += interestAmount;

        //                            accrualDate = accrualDate.AddDays(accrualPeriod); // Move to next accrual date
        //                        }

        //                        // Add total amount for this unpaid payment to the total
        //                        totalUnpaidAmount += totalSales;
        //                    }
        //                }
        //            }

        //            return totalUnpaidAmount;
        //        }

        //[HttpGet]
        //[Route("api/payment/status")]
        //public IHttpActionResult GetPaymentStatus()
        //{
        //    using (var db = new NpgsqlConnection(con))
        //    {
        //        db.Open();

        //        // Query to get the count and sum of paid payments from the payment table
        //        int paidCount = 0;
        //        decimal totalPaidAmount = 0;
        //        using (var cmdPaid = new NpgsqlCommand("SELECT COUNT(*), SUM(p_amount) FROM payment WHERE p_status = 'paid'", db))
        //        {
        //            using (var reader = cmdPaid.ExecuteReader())
        //            {
        //                if (reader.Read())
        //                {
        //                    paidCount = reader.GetInt32(0);  // Count of paid payments
        //                    totalPaidAmount = reader.IsDBNull(1) ? 0 : reader.GetDecimal(1);  // Sum of paid amounts
        //                }
        //            }
        //        }

        //        // Query to get the unpaid payments details from the generate_bill table
        //        int unpaidCount = 0;
        //        decimal totalUnpaidAmount = 0;
        //        using (var cmdUnpaid = new NpgsqlCommand("SELECT COUNT(*), SUM(gb_total_sales), gb_issue_date, gb_accrual_period, gb_suspend_period, gb_interest FROM generate_bill WHERE gb_status = 'unpaid'", db))
        //        {
        //            using (var reader = cmdUnpaid.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    unpaidCount = reader.GetInt32(0); // Count of unpaid payments
        //                    decimal totalSales = reader.IsDBNull(1) ? 0 : reader.GetDecimal(1);  // Total sales amount
        //                    DateTime issueDate = reader.GetDateTime(2);  // Issue date of the bill
        //                    int accrualPeriod = reader.GetInt32(3);  // Accrual period (in days)
        //                    int suspendPeriod = reader.GetInt32(4);  // Suspend period (in days)
        //                    decimal interest = reader.GetDecimal(5);  // Interest rate (in percentage)

        //                    // Calculate the due date
        //                    DateTime dueDate = issueDate.AddDays(suspendPeriod);

        //                    // Initialize accrual and suspension dates
        //                    DateTime accrualDate = dueDate.AddDays(accrualPeriod);
        //                    DateTime suspDate = dueDate.AddDays(suspendPeriod);

        //                    // Check if the current date is greater than the due date
        //                    DateTime currentDate = DateTime.Now;

        //                    if (currentDate > dueDate)
        //                    {
        //                        // Calculate interest on the total sales amount
        //                        decimal amntInterest = (totalSales * interest / 100);
        //                        totalSales += amntInterest;

        //                        // Apply interest until the accrual period ends or the suspension period is reached
        //                        while (currentDate >= accrualDate && accrualDate <= suspDate)
        //                        {
        //                            amntInterest = (totalSales * interest / 100);
        //                            totalSales += amntInterest;
        //                            accrualDate = accrualDate.AddDays(accrualPeriod);  // Move to next accrual period
        //                        }
        //                    }

        //                    // Update the total unpaid amount with the final amount after interest calculations
        //                    totalUnpaidAmount += totalSales;
        //                }
        //            }
        //        }

        //        // Return the counts and sum amounts in a JSON response
        //        return Ok(new { paidCount, unpaidCount, totalPaidAmount, totalUnpaidAmount });
        //    }
        //}


        //[HttpGet]
        //[Route("api/payment/status")]
        //public IHttpActionResult GetPaymentStatus()
        //{
        //    using (var db = new NpgsqlConnection(con))
        //    {
        //        db.Open();

        //        // Query to get the count and sum of paid payments from the payment table
        //        int paidCount = 0;
        //        decimal totalPaidAmount = 0;
        //        using (var cmdPaid = new NpgsqlCommand("SELECT COUNT(*), SUM(p_amount) FROM payment WHERE p_status = 'paid'", db))
        //        {
        //            using (var reader = cmdPaid.ExecuteReader())
        //            {
        //                if (reader.Read())
        //                {
        //                    paidCount = reader.GetInt32(0);  // Count of paid payments
        //                    totalPaidAmount = reader.IsDBNull(1) ? 0 : reader.GetDecimal(1);  // Sum of paid amounts
        //                }
        //            }
        //        }

        //        // Query to get the unpaid payments details from the generate_bill table
        //        int unpaidCount = 0;
        //        decimal totalUnpaidAmount = 0;
        //        using (var cmdUnpaid = new NpgsqlCommand("SELECT COUNT(*), SUM(gb_total_sales) FROM generate_bill WHERE gb_status = 'unpaid'", db))
        //        {
        //            using (var reader = cmdUnpaid.ExecuteReader())
        //            {
        //                if (reader.Read())
        //                {
        //                    unpaidCount = reader.GetInt32(0); // Count of unpaid payments
        //                    totalUnpaidAmount = reader.IsDBNull(1) ? 0 : reader.GetDecimal(1);  // Total sales amount (unpaid)
        //                }
        //            }
        //        }

        //        // Return the counts and sum amounts in a JSON response
        //        return Ok(new { paidCount, unpaidCount, totalPaidAmount, totalUnpaidAmount });
        //    }
        //}


        [HttpGet]
        [Route("api/payment/status")]
        public IHttpActionResult GetPaymentStatus()
        {
            DateTime currentDate = DateTime.Now; // Define currentDate for calculations
            using (var db = new NpgsqlConnection(con))
            {
                db.Open();

                // Query to get the count and sum of paid payments from the payment table
                int paidCount = 0;
                decimal totalPaidAmount = 0;
                using (var cmdPaid = new NpgsqlCommand("SELECT COUNT(*), SUM(p_amount) FROM payment WHERE p_status = 'paid'", db))
                {
                    using (var reader = cmdPaid.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            paidCount = reader.GetInt32(0);  // Count of paid payments
                            totalPaidAmount = reader.IsDBNull(1) ? 0 : reader.GetDecimal(1);  // Sum of paid amounts
                        }
                    }
                }

                // Query to get the unpaid payments details from the generate_bill table
                int unpaidCount = 0;
                decimal totalUnpaidAmount = 0;
                decimal[] monthlyUnpaidAmounts = new decimal[12]; // Monthly totals for each month in the year

                using (var cmdUnpaid = new NpgsqlCommand("SELECT gb_id, gb_total_sales, gb_interest, gb_created_at, gb_updated_at, gb_date_due, gb_accrual_period, gb_suspend_period, gb_accrual_date, gb_suspend_date FROM public.generate_bill WHERE gb_status = 'unpaid'", db))
                {
                    using (var reader = cmdUnpaid.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Extract values from the query results
                            decimal totalSales = reader.GetDecimal(reader.GetOrdinal("gb_total_sales"));
                            decimal interestRate = reader.GetDecimal(reader.GetOrdinal("gb_interest"));
                            DateTime createdAt = reader.GetDateTime(reader.GetOrdinal("gb_created_at"));
                            DateTime? updatedAt = reader.IsDBNull(reader.GetOrdinal("gb_updated_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("gb_updated_at"));
                            DateTime dueDate = reader.GetDateTime(reader.GetOrdinal("gb_date_due"));
                            DateTime accrualDate = reader.GetDateTime(reader.GetOrdinal("gb_accrual_date"));
                            DateTime endDate = updatedAt ?? currentDate; // End date for calculation
                            DateTime monthDate = new DateTime(createdAt.Year, createdAt.Month, 1);

                            // Loop through each month from created_at to endDate
                            while (monthDate <= endDate)
                            {
                                int monthIndex = monthDate.Month - 1;

                                // Apply interest if past the due date
                                if (monthDate > dueDate)
                                {
                                    decimal interestAmount = totalSales * interestRate / 100;
                                    totalSales += interestAmount;
                                }

                                // Add unpaid amount to the monthly array and accumulate total unpaid
                                monthlyUnpaidAmounts[monthIndex] += totalSales;
                                totalUnpaidAmount += totalSales;

                                // Move to the next month
                                monthDate = monthDate.AddMonths(1);
                            }

                            unpaidCount++; // Increment the count of unpaid records
                        }
                    }
                }

                // Return the counts and sum amounts in a JSON response
                return Ok(new { paidCount, unpaidCount, totalPaidAmount, totalUnpaidAmount, monthlyUnpaidAmounts });
            }
        }






        [HttpGet]
        [Route("api/payment/monthlyTotals")]
        public IHttpActionResult GetMonthlyTotals()
        {
            decimal[] monthlyTotalSalesPaid = new decimal[12];
            decimal[] monthlyTotalSalesUnpaid = new decimal[12];

            using (var db = new NpgsqlConnection(con))
            {
                db.Open();

                for (int month = 0; month < 12; month++)
                {
                    DateTime startDate = new DateTime(DateTime.Now.Year, month + 1, 1);
                    DateTime endDate = startDate.AddMonths(1);

                    // Get total paid amount for the month
                    monthlyTotalSalesPaid[month] = GetTotalPaidSales(db, startDate, endDate);

                    // Get total unpaid amount for the month
                    var monthlyUnpaidAmounts = GetTotalUnpaidSales(db, startDate, DateTime.Now);

                    // Populate the monthly unpaid totals array
                    monthlyTotalSalesUnpaid = monthlyUnpaidAmounts;
                }
            }

            Console.WriteLine($"Monthly Total Paid Sales: {string.Join(", ", monthlyTotalSalesPaid)}");
            Console.WriteLine($"Monthly Total Unpaid Sales: {string.Join(", ", monthlyTotalSalesUnpaid)}");

            return Ok(new { monthlyTotalSalesPaid, monthlyTotalSalesUnpaid });
        }



        // Helper method to get total paid sales
        private decimal GetTotalPaidSales(NpgsqlConnection conn, DateTime startDate, DateTime endDate)
        {
            decimal totalPaidAmount = 0.00m;

            string query = @"
                SELECT SUM(p_amount) 
                FROM public.payment 
                WHERE p_status = 'paid' 
                AND p_updated_at >= @startDate 
                AND p_updated_at < @endDate";

            using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("startDate", startDate);
                cmd.Parameters.AddWithValue("endDate", endDate);

                var result = cmd.ExecuteScalar();

                if (result != DBNull.Value)
                {
                    totalPaidAmount = Convert.ToDecimal(result);
                }
            }

            return totalPaidAmount;
        }

        // Helper method to get total unpaid sales
        private decimal[] GetTotalUnpaidSales(NpgsqlConnection conn, DateTime startDate, DateTime currentDate)
        {
            // Initialize an array to store unpaid amounts for each month
            decimal[] monthlyUnpaidAmounts = new decimal[12];

            string query = @"
        SELECT gb_id, gb_total_sales, gb_interest, gb_created_at, gb_updated_at,
               gb_date_due, gb_accrual_period, gb_suspend_period, gb_accrual_date, gb_suspend_date 
        FROM public.generate_bill 
        WHERE gb_status = 'unpaid'";

            using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("currentDate", currentDate);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        decimal totalSales = reader.GetDecimal(reader.GetOrdinal("gb_total_sales"));
                        decimal interestRate = reader.GetDecimal(reader.GetOrdinal("gb_interest"));
                        DateTime createdAt = reader.GetDateTime(reader.GetOrdinal("gb_created_at"));
                        DateTime? updatedAt = reader.IsDBNull(reader.GetOrdinal("gb_updated_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("gb_updated_at"));
                        DateTime dueDate = reader.GetDateTime(reader.GetOrdinal("gb_date_due"));
                        int accrualPeriod = reader.GetInt32(reader.GetOrdinal("gb_accrual_period"));
                        DateTime accrualDate = reader.GetDateTime(reader.GetOrdinal("gb_accrual_date"));
                        DateTime? suspendDate = reader.IsDBNull(reader.GetOrdinal("gb_suspend_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("gb_suspend_date"));

                        DateTime endDate = updatedAt ?? currentDate;  // Calculate until the updated date or current date
                        DateTime monthDate = new DateTime(createdAt.Year, createdAt.Month, 1);

                        // Loop through each month from created_at to endDate
                        while (monthDate <= endDate)
                        {
                            // Check the index for the month in the current year
                            int monthIndex = monthDate.Month - 1;

                            // Apply interest if past the due date
                            if (monthDate > dueDate)
                            {
                                decimal interestAmount = (totalSales * interestRate / 100);
                                totalSales += interestAmount;
                            }

                            // Add unpaid amount to the monthly array
                            monthlyUnpaidAmounts[monthIndex] += totalSales;

                            // Move to the next month
                            monthDate = monthDate.AddMonths(1);
                        }
                    }
                }
            }

            // Log unpaid amounts for debugging
            Console.WriteLine($"Monthly Unpaid Amounts: {string.Join(", ", monthlyUnpaidAmounts)}");

            return monthlyUnpaidAmounts;
        }





        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}