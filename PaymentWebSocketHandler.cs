using System;
using System.Drawing;
using System.Timers;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Npgsql;

namespace Capstone.Handlers
{
    public class PaymentHub : Hub
    {
        private static Timer _timer;
        private readonly string con = "Server=localhost;Port=5432;User Id=postgres;Password=123456;Database=trashtrack";

        public PaymentHub()
        {
            if (_timer == null)
            {
                _timer = new Timer(5000); // Set up a timer for every 5 seconds
                _timer.Elapsed += SendMonthlyData;
                _timer.Start();
            }
        }

        private void SendMonthlyData(object sender, ElapsedEventArgs e)
        {
            var monthlyTotalSalesPaid = GetMonthlyTotalSales("paid");
            var monthlyTotalSalesUnpaid = GetMonthlyTotalSales("unpaid");

            var data = new
            {
                monthlyTotalSalesPaid,
                monthlyTotalSalesUnpaid
            };

            // Broadcast to all connected clients
            Clients.All.updateChart(JsonConvert.SerializeObject(data));
        }

        private decimal[] GetMonthlyTotalSales(string status)
        {
            decimal[] monthlyTotals = new decimal[12];

            using (var conn = new NpgsqlConnection(con)) // Use your actual connection string here
            {
                conn.Open();
                DateTime currentDate = DateTime.Now;

                for (int month = 0; month < 12; month++)
                {
                    // Set the start and end dates for the month
                    DateTime startDate = new DateTime(currentDate.Year, month + 1, 1);
                    DateTime endDate = startDate.AddMonths(1);

                    // Get the total sales from your existing method
                    monthlyTotals[month] = GetTotalSales(conn, startDate, endDate, status, currentDate);
                }
            }

            return monthlyTotals;
        }
        private decimal GetTotalSales(NpgsqlConnection conn, DateTime startDate, DateTime endDate, string billStatus, DateTime? currentDate = null)
        {
            decimal totalBill = 0.00m;
            string query;

            if (billStatus == "unpaid" && currentDate.HasValue)
            {
                query = @"SELECT SUM(p_amount) 
          FROM public.payment 
          WHERE p_created_at < @currentDate 
            AND p_status = 'unpaid' 
            AND p_updated_at < @endDate";
            }
            else
            {
                query = "SELECT SUM(p_amount) FROM public.payment WHERE p_updated_at >= @startDate AND p_updated_at < @endDate AND p_status = @billStatus";
            }

            using (var cmd = new NpgsqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("startDate", startDate);
                cmd.Parameters.AddWithValue("endDate", endDate);
                if (currentDate.HasValue)
                {
                    cmd.Parameters.AddWithValue("currentDate", currentDate.Value);
                }
                cmd.Parameters.AddWithValue("billStatus", billStatus);

                var result = cmd.ExecuteScalar();

                if (result != DBNull.Value)
                {
                    totalBill = Convert.ToDecimal(result);
                }
            }

            return totalBill;
        }


    }
}
