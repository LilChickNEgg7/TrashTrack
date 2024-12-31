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

        //// Endpoint to get notification count
        //[HttpGet]
        //[Route("api/payment/notificationCount")]
        //public IHttpActionResult GetNotificationCount()
        //{
        //    int unreadCount = GetUnreadNotificationCount();
        //    return Ok(new { unreadCount });
        //}

        //// Fetch unread notifications count
        //private int GetUnreadNotificationCount()
        //{
        //    string query = "SELECT COUNT(*) FROM notification WHERE notif_type = 'request verification' AND notif_read = false AND notif_status = 'Active';";
        //    using (var connection = new NpgsqlConnection(con))
        //    {
        //        connection.Open();
        //        using (var command = new NpgsqlCommand(query, connection))
        //        {
        //            return Convert.ToInt32(command.ExecuteScalar());
        //        }
        //    }
        //}


        // Endpoint to get notification count
        [HttpGet]
        [Route("api/payment/notificationCount")]
        public IHttpActionResult GetNotificationCount()
        {
            try
            {
                int unreadCount = GetUnreadNotificationCount();
                return Ok(new { unreadCount });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = $"Error fetching notification count: {ex.Message}" });
            }
        }

        // Fetch unread notifications count
        private int GetUnreadNotificationCount()
        {
            // SQL query to fetch the unread notifications count
            string query = "SELECT COUNT(*) FROM notification WHERE notif_type = 'request verification' AND notif_read = false AND notif_status != 'Deleted';";

            using (var connection = new NpgsqlConnection(con))
            {
                connection.Open();

                // Execute the query and return the count
                using (var command = new NpgsqlCommand(query, connection))
                {
                    try
                    {
                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error executing query: {ex.Message}");
                    }
                }
            }
        }


        [HttpGet]
        [Route("api/payment/notifications")]
        public IHttpActionResult GetNotifications()
        {
            try
            {
                var notifications = GetNotificationsFromDb();
                return Ok(notifications);  // Return the list of notifications as a response
            }
            catch (Exception ex)
            {
                // Log the error for debugging purposes
                Console.WriteLine("Error fetching notifications: " + ex.Message);
                return InternalServerError(ex);  // Handle any errors
            }
        }


        // Fetch unread notifications count
        private int GetNotificationsFromDb()
        {
            string query = "SELECT * FROM notification WHERE notif_type = 'request verification' AND notif_status != 'Deleted';";
            using (var connection = new NpgsqlConnection(con))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }








        protected string GetNotificationIcon(string status)
        {
            switch (status)
            {
                case "Pending":
                    return "bi bi-exclamation-circle text-warning";
                case "Deleted":
                    return "bi bi-exclamation-circle text-warning";
                case "Declined":
                    return "bi bi-x-circle text-danger";
                case "Approved":
                    return "bi bi-check-circle text-success";
                default:
                    return "bi bi-info-circle text-primary";
            }
        }


        [HttpPost]
        [Route("api/payment/markNotificationAsRead")]
        public IHttpActionResult MarkNotificationAsRead(int notifId)
        {
            MarkNotificationAsReadInDb(notifId);
            return Ok();
        }

        // Mark notification as read in the database
        private void MarkNotificationAsReadInDb(int notifId)
        {
            string query = "UPDATE notification SET notif_read = true WHERE notif_id = @notifId;";
            using (var connection = new NpgsqlConnection(con))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@notifId", notifId);
                    command.ExecuteNonQuery();
                }
            }
        }



        [HttpPost]
        [Route("api/payment/markAllAsRead")]
        public IHttpActionResult MarkAllAsRead()
        {
            try
            {
                string updateQuery = "UPDATE notification SET notif_read = true WHERE notif_read = false AND notif_type = 'request verification';";
                string fetchQuery = "SELECT notif_id, notif_message, notif_created_at, notif_read, notif_type, notif_status, cus_id " +
                                    "FROM notification WHERE notif_status != 'Deleted' ORDER BY notif_created_at DESC;";

                using (var connection = new NpgsqlConnection(con))
                {
                    connection.Open();

                    // Update query
                    using (var updateCommand = new NpgsqlCommand(updateQuery, connection))
                    {
                        updateCommand.ExecuteNonQuery();
                    }

                    // Fetch updated data
                    using (var fetchCommand = new NpgsqlCommand(fetchQuery, connection))
                    using (var reader = fetchCommand.ExecuteReader())
                    {
                        var notifications = new List<Notification>();
                        while (reader.Read())
                        {
                            notifications.Add(new Notification
                            {
                                NotifId = reader.GetInt32(0),
                                NotifMessage = reader.GetString(1),
                                NotifCreatedAt = reader.GetDateTime(2),
                                NotifRead = reader.GetBoolean(3),
                                NotifType = reader.GetString(4),
                                NotifStatus = reader.GetString(5),
                                CusId = reader.GetInt32(6)

                            });
                        }
                        return Ok(notifications);
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = $"Error: {ex.Message}" });
            }
        }


        private IHttpActionResult BadRequest(object value)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("api/payment/deleteAllNotifications")]
        public IHttpActionResult DeleteAllNotifications()
        {
            try
            {
                string updateQuery = "UPDATE notification SET notif_status = 'Deleted' WHERE notif_type = 'request verification' AND notif_status != 'Deleted';";
                string fetchQuery = "SELECT notif_id, notif_message, notif_created_at, notif_read, notif_type, notif_status, cus_id " +
                                    "FROM notification WHERE notif_status != 'Deleted' ORDER BY notif_created_at DESC;";

                using (var connection = new NpgsqlConnection(con))
                {
                    connection.Open();

                    // Update query
                    using (var updateCommand = new NpgsqlCommand(updateQuery, connection))
                    {
                        updateCommand.ExecuteNonQuery();
                    }

                    // Fetch updated data
                    using (var fetchCommand = new NpgsqlCommand(fetchQuery, connection))
                    using (var reader = fetchCommand.ExecuteReader())
                    {
                        var notifications = new List<Notification>();
                        while (reader.Read())
                        {
                            notifications.Add(new Notification
                            {
                                NotifId = reader.GetInt32(0),
                                NotifMessage = reader.GetString(1),
                                NotifCreatedAt = reader.GetDateTime(2),
                                NotifRead = reader.GetBoolean(3),
                                NotifType = reader.GetString(4),
                                NotifStatus = reader.GetString(5),
                                CusId = reader.GetInt32(6)
                            });
                        }
                        return Ok(notifications);
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = $"Error: {ex.Message}" });
            }
        }


        public class Notification
        {
            public int NotifId { get; set; }
            public string NotifMessage { get; set; }
            public DateTime NotifCreatedAt { get; set; }
            public bool NotifRead { get; set; }
            public string NotifType { get; set; }
            public int CusId { get; set; }
            public string NotifStatus { get; set; }
            public string NotifIcon { get; set; }  // Ensure this property is included
        }
















        ////////////////////////////
        /////FOR BILLING
        ///////////////////////////
        //// Endpoint to get notification count
        [HttpGet]
        [Route("api/payment/notificationCount1")]
        public IHttpActionResult GetNotificationCount1()
        {
            try
            {
                int unreadCount1 = GetUnreadNotificationCount1();
                return Ok(new { unreadCount1 });
            }
            catch (Exception ex)
            {
                return BadRequest1(new { error = $"Error fetching notification count: {ex.Message}" });
            }
        }

        // Fetch unread notifications count
        // Fetch unread notifications count             string query = "SELECT COUNT(*) FROM notification WHERE notif_type = 'request verification' AND notif_read = false AND notif_status != 'Deleted';";

        private int GetUnreadNotificationCount1()
        {
            string query = "SELECT COUNT(*) FROM notification WHERE notif_read = false AND notif_type IN ('slip', 'payment');";
            using (var connection = new NpgsqlConnection(con))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    try
                    {
                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Error executing query: {ex.Message}");
                    }
                }
            }
        }


        [HttpGet]
        [Route("api/payment/notifications1")]
        public IHttpActionResult GetNotifications1()
        {
            try
            {
                var notifications1 = GetNotificationsFromDb1();
                return Ok(notifications1);  // Return the list of notifications as a response
            }
            catch (Exception ex)
            {
                // Log the error for debugging purposes
                Console.WriteLine("Error fetching notifications: " + ex.Message);
                return InternalServerError(ex);  // Handle any errors
            }
        }



        // Fetch unread notifications from DB
        private List<Notification1> GetNotificationsFromDb1()
        {
            string query = "SELECT * FROM notification WHERE (notif_type = 'payment' OR notif_type = 'slip') AND notif_status != 'Deleted';";
            using (var connection = new NpgsqlConnection(con))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        var notifications1 = new List<Notification1>();
                        while (reader.Read())
                        {
                            notifications1.Add(new Notification1
                            {
                                NotifId1 = reader.GetInt32(0),
                                NotifMessage1 = reader.GetString(1),
                                NotifCreatedAt1 = reader.GetDateTime(2),
                                NotifRead1 = reader.GetBoolean(3),
                                NotifType1 = reader.GetString(4),
                                NotifStatus1 = reader.GetString(5),
                                CusId1 = reader.GetInt32(6)
                            });
                        }
                        return notifications1;
                    }
                }
            }
        }


        protected string GetNotificationIcon1(string status)
        {
            switch (status)
            {
                case "Pending":
                    return "bi bi-exclamation-circle text-warning";
                case "Deleted":
                    return "bi bi-exclamation-circle text-warning";
                case "Declined":
                    return "bi bi-x-circle text-danger";
                case "Approved":
                    return "bi bi-check-circle text-success";
                default:
                    return "bi bi-info-circle text-primary";
            }
        }


        // Mark notification as read in the database
        private void MarkNotificationAsReadInDb1(int notifId)
        {
            string query = "UPDATE notification SET notif_read = true WHERE notif_id = @notifId AND (notif_type = 'slip' OR notif_type = 'payment');";
            using (var connection = new NpgsqlConnection(con))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@notifId", notifId);
                    command.ExecuteNonQuery();
                }
            }
        }

        [HttpPost]
        [Route("api/payment/markAllAsRead1")]
        public IHttpActionResult MarkAllAsRead1()
        {
            try
            {
                string updateQuery = "UPDATE notification SET notif_read = true WHERE notif_read = false AND (notif_type = 'slip' OR notif_type = 'payment');";
                string fetchQuery = "SELECT notif_id, notif_message, notif_created_at, notif_read, notif_type, notif_status, cus_id " +
                                     "FROM notification " +
                                     "WHERE notif_status != 'Deleted' AND (notif_type = 'slip' OR notif_type = 'payment') " +
                                     "ORDER BY notif_created_at DESC;";

                using (var connection = new NpgsqlConnection(con))
                {
                    connection.Open();

                    // Update query
                    using (var updateCommand = new NpgsqlCommand(updateQuery, connection))
                    {
                        updateCommand.ExecuteNonQuery();
                    }

                    // Fetch updated data
                    using (var fetchCommand = new NpgsqlCommand(fetchQuery, connection))
                    using (var reader = fetchCommand.ExecuteReader())
                    {
                        var notifications1 = new List<Notification1>();
                        while (reader.Read())
                        {
                            notifications1.Add(new Notification1
                            {
                                NotifId1 = reader.GetInt32(0),
                                NotifMessage1 = reader.GetString(1),
                                NotifCreatedAt1 = reader.GetDateTime(2),
                                NotifRead1 = reader.GetBoolean(3),
                                NotifType1 = reader.GetString(4),
                                NotifStatus1 = reader.GetString(5),
                                CusId1 = reader.GetInt32(6)
                            });
                        }
                        return Ok(notifications1);
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest1(new { error = $"Error: {ex.Message}" });
            }
        }



        private IHttpActionResult BadRequest1(object value)
        {
            return BadRequest1(value); // Use the built-in BadRequest response
        }



        [HttpPost]
        [Route("api/payment/deleteAllNotifications1")]
        public IHttpActionResult DeleteAllNotifications1()
        {
            try
            {
                string updateQuery = "UPDATE notification SET notif_status = 'Deleted' WHERE notif_type IN ('slip', 'payment');";
                string fetchQuery = "SELECT notif_id, notif_message, notif_created_at, notif_read, notif_type, notif_status, cus_id " +
                                    "FROM notification WHERE notif_status != 'Deleted' AND notif_type IN ('payment', 'slip') " +
                                    "ORDER BY notif_created_at DESC;";

                using (var connection = new NpgsqlConnection(con))
                {
                    connection.Open();

                    // Update query
                    using (var updateCommand = new NpgsqlCommand(updateQuery, connection))
                    {
                        updateCommand.ExecuteNonQuery();
                    }

                    // Fetch updated data
                    using (var fetchCommand = new NpgsqlCommand(fetchQuery, connection))
                    using (var reader = fetchCommand.ExecuteReader())
                    {
                        var notifications1 = new List<Notification1>();
                        while (reader.Read())
                        {
                            notifications1.Add(new Notification1
                            {
                                NotifId1 = reader.GetInt32(0),
                                NotifMessage1 = reader.GetString(1),
                                NotifCreatedAt1 = reader.GetDateTime(2),
                                NotifRead1 = reader.GetBoolean(3),
                                NotifType1 = reader.GetString(4),
                                NotifStatus1 = reader.GetString(5),
                                CusId1 = reader.GetInt32(6)
                            });
                        }
                        return Ok(notifications1);
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest1(new { error = $"Error: {ex.Message}" });
            }
        }


        public class Notification1
        {
            public int NotifId1 { get; set; }
            public string NotifMessage1 { get; set; }
            public DateTime NotifCreatedAt1 { get; set; }
            public bool NotifRead1 { get; set; }
            public string NotifType1 { get; set; }
            public int CusId1 { get; set; }
            public string NotifStatus1 { get; set; }
            public string NotifIcon1 { get; set; }  // Ensure this property is included
        }





        /// <summary>
        /// //////////////////////////////
        /// </summary>
        /// <returns></returns>

        [HttpGet]
[Route("api/payment/status")]
public IHttpActionResult GetPaymentStatus()
{
    using (var db = new NpgsqlConnection(con))
    {
        db.Open();

        // Query to get paid bookings details
        int paidCount = 0;
        decimal totalPaidAmount = 0;
        using (var cmdPaid = new NpgsqlCommand(@"
            SELECT COUNT(*), COALESCE(SUM(p.p_amount), 0) 
            FROM payment p
            JOIN generate_bill gb ON p.gb_id = gb.gb_id
            JOIN booking b ON gb.bk_id = b.bk_id
            WHERE p.p_status = 'paid' AND b.bk_status NOT IN ('Cancelled', 'Failed');", db))
        {
            using (var reader = cmdPaid.ExecuteReader())
            {
                if (reader.Read())
                {
                    paidCount = reader.GetInt32(0); // Count of paid payments
                    totalPaidAmount = reader.GetDecimal(1); // Sum of paid amounts
                }
            }
        }

        // Query to get unpaid bookings details
        int unpaidCount = 0;
        decimal totalUnpaidAmount = 0;
        List<object> unpaidDetails = new List<object>();

        using (var cmdUnpaid = new NpgsqlCommand(@"
            SELECT b.bk_id, b.bk_date, b.bk_status, gb.gb_total_sales 
            FROM booking b
            LEFT JOIN generate_bill gb ON b.bk_id = gb.bk_id
            WHERE b.bk_status NOT IN ('Cancelled', 'Failed') AND gb.gb_status = 'Unpaid';", db))
        {
            using (var reader = cmdUnpaid.ExecuteReader())
            {
                while (reader.Read())
                {
                    // Increment unpaid count and total amount
                    unpaidCount++;
                    decimal totalSales = reader.IsDBNull(reader.GetOrdinal("gb_total_sales")) ? 0 : reader.GetDecimal(reader.GetOrdinal("gb_total_sales"));
                    totalUnpaidAmount += totalSales;

                    // Collect unpaid booking details
                    unpaidDetails.Add(new
                    {
                        BookingId = reader.GetInt32(reader.GetOrdinal("bk_id")),
                        BookingDate = reader.GetDateTime(reader.GetOrdinal("bk_date")),
                        BookingStatus = reader.GetString(reader.GetOrdinal("bk_status")),
                        TotalSales = totalSales
                    });
                }
            }
        }

        // Return the counts and sums in a JSON response
        return Ok(new
        {
            paidCount,
            totalPaidAmount,
            unpaidCount,
            totalUnpaidAmount,
            unpaidDetails
        });
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
                    monthlyTotalSalesUnpaid[month] = GetTotalUnpaidSales(db, startDate, endDate);
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
        private decimal GetTotalUnpaidSales(NpgsqlConnection conn, DateTime startDate, DateTime endDate)
        {
            decimal totalUnpaidAmount = 0.00m;

            string query = @"
        SELECT COALESCE(SUM(gb.gb_total_sales), 0) 
        FROM public.generate_bill gb
        JOIN public.booking b ON gb.bk_id = b.bk_id
        WHERE gb.gb_status = 'Unpaid' 
        AND b.bk_status NOT IN ('Cancelled', 'Failed') 
        AND gb_created_at >= @startDate 
        AND gb_created_at < @endDate";

            using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("startDate", startDate);
                cmd.Parameters.AddWithValue("endDate", endDate);

                var result = cmd.ExecuteScalar();

                if (result != DBNull.Value)
                {
                    totalUnpaidAmount = Convert.ToDecimal(result);
                }
            }

            return totalUnpaidAmount;
        }














        //[HttpGet]
        //[Route("api/payment/status")]
        //public IHttpActionResult GetPaymentStatus()
        //{
        //    DateTime currentDate = DateTime.Now; // Define currentDate for calculations
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
        //        decimal[] monthlyUnpaidAmounts = new decimal[12]; // Monthly totals for each month in the year

        //        using (var cmdUnpaid = new NpgsqlCommand("SELECT gb_id, gb_total_sales, gb_interest, gb_created_at, gb_updated_at, gb_date_due, gb_accrual_period, gb_suspend_period, gb_accrual_date, gb_suspend_date FROM public.generate_bill WHERE gb_status = 'unpaid'", db))
        //        {
        //            using (var reader = cmdUnpaid.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    // Extract values from the query results
        //                    decimal totalSales = reader.GetDecimal(reader.GetOrdinal("gb_total_sales"));
        //                    decimal interestRate = reader.GetDecimal(reader.GetOrdinal("gb_interest"));
        //                    DateTime createdAt = reader.GetDateTime(reader.GetOrdinal("gb_created_at"));
        //                    DateTime? updatedAt = reader.IsDBNull(reader.GetOrdinal("gb_updated_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("gb_updated_at"));
        //                    DateTime dueDate = reader.GetDateTime(reader.GetOrdinal("gb_date_due"));
        //                    DateTime accrualDate = reader.GetDateTime(reader.GetOrdinal("gb_accrual_date"));
        //                    DateTime endDate = updatedAt ?? currentDate; // End date for calculation
        //                    DateTime monthDate = new DateTime(createdAt.Year, createdAt.Month, 1);

        //                    // Loop through each month from created_at to endDate
        //                    while (monthDate <= endDate)
        //                    {
        //                        int monthIndex = monthDate.Month - 1;

        //                        // Apply interest if past the due date
        //                        if (monthDate > dueDate)
        //                        {
        //                            decimal interestAmount = totalSales * interestRate / 100;
        //                            totalSales += interestAmount;
        //                        }

        //                        // Add unpaid amount to the monthly array and accumulate total unpaid
        //                        monthlyUnpaidAmounts[monthIndex] += totalSales;
        //                        totalUnpaidAmount += totalSales;

        //                        // Move to the next month
        //                        monthDate = monthDate.AddMonths(1);
        //                    }

        //                    unpaidCount++; // Increment the count of unpaid records
        //                }
        //            }
        //        }

        //        // Return the counts and sum amounts in a JSON response
        //        return Ok(new { paidCount, unpaidCount, totalPaidAmount, totalUnpaidAmount, monthlyUnpaidAmounts });
        //    }
        //}



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

        //            // Get total paid amount for the month
        //            monthlyTotalSalesPaid[month] = GetTotalPaidSales(db, startDate, endDate);

        //            // Get total unpaid amount for the month
        //            var monthlyUnpaidAmounts = GetTotalUnpaidSales(db, startDate, DateTime.Now);

        //            // Populate the monthly unpaid totals array
        //            monthlyTotalSalesUnpaid = monthlyUnpaidAmounts;
        //        }
        //    }

        //    Console.WriteLine($"Monthly Total Paid Sales: {string.Join(", ", monthlyTotalSalesPaid)}");
        //    Console.WriteLine($"Monthly Total Unpaid Sales: {string.Join(", ", monthlyTotalSalesUnpaid)}");

        //    return Ok(new { monthlyTotalSalesPaid, monthlyTotalSalesUnpaid });
        //}



        //// Helper method to get total paid sales
        //private decimal GetTotalPaidSales(NpgsqlConnection conn, DateTime startDate, DateTime endDate)
        //{
        //    decimal totalPaidAmount = 0.00m;

        //    string query = @"
        //        SELECT SUM(p_amount) 
        //        FROM public.payment 
        //        WHERE p_status = 'paid' 
        //        AND p_updated_at >= @startDate 
        //        AND p_updated_at < @endDate";

        //    using (var cmd = new NpgsqlCommand(query, conn))
        //    {
        //        cmd.Parameters.AddWithValue("startDate", startDate);
        //        cmd.Parameters.AddWithValue("endDate", endDate);

        //        var result = cmd.ExecuteScalar();

        //        if (result != DBNull.Value)
        //        {
        //            totalPaidAmount = Convert.ToDecimal(result);
        //        }
        //    }

        //    return totalPaidAmount;
        //}

        //// Helper method to get total unpaid sales
        //private decimal[] GetTotalUnpaidSales(NpgsqlConnection conn, DateTime startDate, DateTime currentDate)
        //{
        //    // Initialize an array to store unpaid amounts for each month
        //    decimal[] monthlyUnpaidAmounts = new decimal[12];

        //    string query = @"
        //SELECT gb_id, gb_total_sales, gb_interest, gb_created_at, gb_updated_at,
        //       gb_date_due, gb_accrual_period, gb_suspend_period, gb_accrual_date, gb_suspend_date 
        //FROM public.generate_bill 
        //WHERE gb_status = 'unpaid'";

        //    using (var cmd = new NpgsqlCommand(query, conn))
        //    {
        //        cmd.Parameters.AddWithValue("currentDate", currentDate);

        //        using (var reader = cmd.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                decimal totalSales = reader.GetDecimal(reader.GetOrdinal("gb_total_sales"));
        //                decimal interestRate = reader.GetDecimal(reader.GetOrdinal("gb_interest"));
        //                DateTime createdAt = reader.GetDateTime(reader.GetOrdinal("gb_created_at"));
        //                DateTime? updatedAt = reader.IsDBNull(reader.GetOrdinal("gb_updated_at")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("gb_updated_at"));
        //                DateTime dueDate = reader.GetDateTime(reader.GetOrdinal("gb_date_due"));
        //                int accrualPeriod = reader.GetInt32(reader.GetOrdinal("gb_accrual_period"));
        //                DateTime accrualDate = reader.GetDateTime(reader.GetOrdinal("gb_accrual_date"));
        //                DateTime? suspendDate = reader.IsDBNull(reader.GetOrdinal("gb_suspend_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("gb_suspend_date"));

        //                DateTime endDate = updatedAt ?? currentDate;  // Calculate until the updated date or current date
        //                DateTime monthDate = new DateTime(createdAt.Year, createdAt.Month, 1);

        //                // Loop through each month from created_at to endDate
        //                while (monthDate <= endDate)
        //                {
        //                    // Check the index for the month in the current year
        //                    int monthIndex = monthDate.Month - 1;

        //                    // Apply interest if past the due date
        //                    if (monthDate > dueDate)
        //                    {
        //                        decimal interestAmount = (totalSales * interestRate / 100);
        //                        totalSales += interestAmount;
        //                    }

        //                    // Add unpaid amount to the monthly array
        //                    monthlyUnpaidAmounts[monthIndex] += totalSales;

        //                    // Move to the next month
        //                    monthDate = monthDate.AddMonths(1);
        //                }
        //            }
        //        }
        //    }

        //    // Log unpaid amounts for debugging
        //    Console.WriteLine($"Monthly Unpaid Amounts: {string.Join(", ", monthlyUnpaidAmounts)}");

        //    return monthlyUnpaidAmounts;
        //}











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