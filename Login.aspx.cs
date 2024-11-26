using System;
using System.Data;
using Npgsql;
using System.Security.Cryptography;
using System.Text;

namespace Capstone
{
    public partial class Login : System.Web.UI.Page
    {
        ////string connString = "Host=localhost;Port=5432;Password=123456;Database="PBC_CAPS";
        private readonly string con = "Server=localhost;Port=5432;User Id=postgres;Password=123456;Database=trashtrack"; 

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        //protected void login_btn_Click(object sender, EventArgs e)
        //{
        //    string loginUsername = exampleInputEmail.Text;
        //    string loginPass = exampleInputPassword.Text;

        //    using (var db = new NpgsqlConnection(con))
        //    {
        //        db.Open();

        //        using (var cmd = db.CreateCommand())
        //        {
        //            cmd.CommandType = CommandType.Text;
        //            cmd.CommandText = "SELECT * FROM users WHERE username = @username";
        //            cmd.Parameters.AddWithValue("@username", loginUsername);

        //            using (NpgsqlDataReader reader = cmd.ExecuteReader())
        //            {
        //                if (reader.Read())
        //                {
        //                    string storedHashedPassword = reader["password"].ToString();
        //                    string userRole = reader["role"].ToString();
        //                    int userId = Convert.ToInt32(reader["_id"]);
        //                    string userStatus = reader["status"].ToString();

        //                    if (PasswordHashMatches(loginPass, storedHashedPassword))
        //                    {
        //                        if (userStatus == "active")
        //                        {
        //                            if (userRole == "admin")
        //                            {
        //                                Session["admin_username"] = loginUsername;
        //                                Session["admin_password"] = storedHashedPassword;
        //                                Session["admin_id"] = userId;
        //                                Response.Redirect("Admin_Dashboard.aspx");
        //                            }
        //                            else if (userRole == "customer")
        //                            {
        //                                Session["customer_username"] = loginUsername;
        //                                Session["customer_password"] = storedHashedPassword;
        //                                Session["customer_id"] = userId;
        //                                Response.Redirect("Customer_Dashboard.aspx");
        //                            }
        //                            else if (userRole == "customer")
        //                            {
        //                                Session["customer_username"] = loginUsername;
        //                                Session["customer_password"] = storedHashedPassword;
        //                                Session["customer_id"] = userId;
        //                                Response.Redirect("Customer_Dashboard.aspx");
        //                            }

        //                        }
        //                        else if (userStatus == "suspended")
        //                        {
        //                            Response.Write("<script>alert('Your account is suspended!')</script>");
        //                        }
        //                        else
        //                        {
        //                            Response.Write("<script>alert('Invalid Username or Password!')</script>");
        //                        }
        //                    }
        //                    else
        //                    {
        //                        Response.Write("<script>alert('Invalid Username or Password!')</script>");
        //                    }
        //                }
        //                else
        //                {
        //                    Response.Write("<script>alert('Invalid Username or Password!')</script>");
        //                }
        //            }
        //        }

        //        db.Close();
        //    }
        //}

        protected void login_btn_Click(object sender, EventArgs e)
        {
            string loginEmail = exampleInputEmail.Text.Trim();
            string loginPass = exampleInputPassword.Text.Trim();

            int userId; // To hold the user ID after successful login
            string storedHashedPassword; // To store the hashed password from the database
            string roleName;

            // Check if user is an Admin
            if (IsSAM(loginEmail, loginPass, out userId, out storedHashedPassword, out roleName))
            {
                // Store necessary session data
                Session["sam_email"] = loginEmail;
                Session["sam_password"] = storedHashedPassword;
                Session["sam_id"] = userId;  // Store the user ID in the session
                Session["sam_rolename"] = roleName;
                // Redirect to Admin dashboard
                Response.Write($"<script>alert('Login Successful! WELCOME {roleName.ToUpper()}');</script>");
                Response.Redirect("SAM_Dashboard.aspx");
                
            }
            else if (IsAM(loginEmail, loginPass, out userId, out storedHashedPassword, out roleName))
            {
                // Store necessary session data
                Session["am_email"] = loginEmail;
                Session["am_password"] = storedHashedPassword;
                Session["am_id"] = userId;  // Store the user ID in the session
                Session["am_rolename"] = roleName;

                // Redirect to Admin dashboard
                Response.Redirect("AM_Dashboard.aspx");
                Response.Write($"<script>alert('Login Successful! WELCOME {roleName.ToUpper()}');</script>");
            }
            else if (IsBilling_Officer(loginEmail, loginPass, out userId, out storedHashedPassword, out roleName))
            {
                // Store necessary session data
                Session["bo_email"] = loginEmail;
                Session["bo_password"] = storedHashedPassword;
                Session["bo_id"] = userId;  // Store the user ID in the session
                Session["bo_rolename"] = roleName;

                // Redirect to Admin dashboard
                Response.Redirect("BO_Dashboard.aspx");
                Response.Write($"<script>alert('Login Successful! WELCOME {roleName.ToUpper()}');</script>");
            }
            else if (IsOperational_Dispatcher(loginEmail, loginPass, out userId, out storedHashedPassword, out roleName))
            {
                // Store necessary session data
                Session["od_email"] = loginEmail;
                Session["od_password"] = storedHashedPassword;
                Session["od_id"] = userId;  // Store the user ID in the session
                Session["od_rolename"] = roleName;

                // Redirect to Admin dashboard
                Response.Redirect("Dispatcher_Dashboard.aspx");
                Response.Write($"<script>alert('Login Successful! WELCOME {roleName.ToUpper()}');</script>");
            }
            else if (IsHauler(loginEmail, loginPass, out userId, out storedHashedPassword, out roleName))
            {
                // Store necessary session data
                //Session["od_email"] = loginEmail;
                //Session["od_password"] = storedHashedPassword;
                //Session["od_id"] = userId;  // Store the user ID in the session
                //Session["od_rolename"] = roleName;

                //// Redirect to Admin dashboard
                //Response.Redirect("Dispatcher_Dashboard.aspx");
                Response.Write($"<script>alert('Haulers need to login to Mobile Application! {roleName.ToUpper()}');</script>");
            }
            else
            {
                // Invalid login
                Response.Write("<script>alert('Login Failed. Invalid email or password.');</script>");
            }

            //else if(IsCustomer(loginUsername, loginPass))
            //{
            //    Response.Write("<script>alert('Login Successful!, WELCOME Contractual Customer!');</script>");
            //}

            //else if (IsHauler(loginUsername, loginPass))
            //{
            //    Response.Write("<script>alert('Login Successful!, WELCOME Hauler!');</script>");
            //}
            //else
            //{
            //    // Invalid login
            //    Response.Write("<script>alert('Login Failed. Invalid email or password.');</script>");
            //}
        }


        //FOR IsSuperAccountManager
        //private bool IsSAM(string email, string password, out int userId, out string storedHashedPassword)
        //{
        //    userId = 0; // Initialize userId
        //    storedHashedPassword = null; // Initialize storedHashedPassword

        //    try
        //    {
        //        using (var db = new NpgsqlConnection(con))
        //        {
        //            db.Open();

        //            using (var cmd = db.CreateCommand())
        //            {
        //                cmd.CommandType = CommandType.Text;

        //                // Query to get acc_id and acc_password for the provided email
        //                cmd.CommandText = "SELECT emp_id, emp_password FROM employee WHERE emp_email = @email AND role_id = 2 AND emp_status = 'Active'";
        //                cmd.Parameters.AddWithValue("@email", email);

        //                using (NpgsqlDataReader reader = cmd.ExecuteReader())
        //                {
        //                    if (reader.Read())
        //                    {
        //                        userId = reader.GetInt32(0); // Get acc_id
        //                        storedHashedPassword = reader.GetString(1); // Get acc_password (hashed)

        //                        // Hash the provided password
        //                        string hashedPassword = HashPassword(password);

        //                        // Compare the hashed password with the stored one
        //                        if (hashedPassword == storedHashedPassword)
        //                        {
        //                            return true; // Login successful
        //                        }
        //                    }
        //                    else
        //                    {
        //                        Response.Write("<script>alert('The login credentials associated was suspended!');</script>");
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
        //    }

        //    return false; // Login failed
        //}
        private bool IsSAM(string email, string password, out int userId, out string storedHashedPassword, out string roleName)
        {
            userId = 0; // Initialize userId
            storedHashedPassword = null; // Initialize storedHashedPassword
            roleName = null; // Initialize roleName

            try
            {
                using (var db = new NpgsqlConnection(con))
                {
                    db.Open();

                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;

                        // Query to get emp_id, emp_password, and role_name where role_id = 2
                        cmd.CommandText = @"
                SELECT e.emp_id, e.emp_password, r.role_name 
                FROM employee e
                JOIN roles r ON e.role_id = r.role_id 
                WHERE e.emp_email = @Email AND e.role_id = 2 AND e.emp_status = 'Active'";

                        cmd.Parameters.AddWithValue("@Email", email);

                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                userId = reader.GetInt32(0); // Get emp_id
                                storedHashedPassword = reader.GetString(1); // Get emp_password (hashed)
                                roleName = reader.GetString(2); // Get role_name

                                // Hash the provided password
                                string hashedPassword = HashPassword(password);

                                // Compare the hashed password with the stored one
                                if (hashedPassword == storedHashedPassword)
                                {
                                    return true; // Login successful
                                }
                            }
                            //else
                            //{
                            //    // If no matching user found, show error
                            //    Response.Write("<script>alert('The login credentials associated were not found or suspended!');</script>");
                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            }

            return false; // Login failed
        }







        //FOR IsAdmin
        private bool IsAM(string email, string password, out int userId, out string storedHashedPassword, out string roleName)
        {
            userId = 0; // Initialize userId
            storedHashedPassword = null; // Initialize storedHashedPassword
            roleName = null; // Initialize roleName

            try
            {
                using (var db = new NpgsqlConnection(con))
                {
                    db.Open();

                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;

                        // Query to get emp_id, emp_password, and role_name for the provided email where role_id = 1
                        cmd.CommandText = @"
                SELECT e.emp_id, e.emp_password, r.role_name 
                FROM employee e
                JOIN roles r ON e.role_id = r.role_id 
                WHERE e.emp_email = @Email AND e.role_id = 1 AND e.emp_status = 'Active'";

                        cmd.Parameters.AddWithValue("@Email", email);

                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                userId = reader.GetInt32(0); // Get emp_id
                                storedHashedPassword = reader.GetString(1); // Get emp_password (hashed)
                                roleName = reader.GetString(2); // Get role_name

                                // Hash the provided password
                                string hashedPassword = HashPassword(password);

                                // Compare the hashed password with the stored one
                                if (hashedPassword == storedHashedPassword)
                                {
                                    return true; // Login successful
                                }
                            }
                            //else
                            //{
                            //    Response.Write("<script>alert('The login credentials associated were not found or suspended!');</script>");
                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            }

            return false; // Login failed
        }




        //FOR IS BILLING OFFICER
        private bool IsBilling_Officer(string email, string password, out int userId, out string storedHashedPassword, out string roleName)
        {
            userId = 0; // Initialize userId
            storedHashedPassword = null; // Initialize storedHashedPassword
            roleName = null; // Initialize roleName

            try
            {
                using (var db = new NpgsqlConnection(con))
                {
                    db.Open();

                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;

                        // Query to get emp_id, emp_password, and role_name for the provided email where role_id = 3
                        cmd.CommandText = @"
                SELECT e.emp_id, e.emp_password, r.role_name 
                FROM employee e
                JOIN roles r ON e.role_id = r.role_id 
                WHERE e.emp_email = @Email AND e.role_id = 3 AND e.emp_status = 'Active'";

                        cmd.Parameters.AddWithValue("@Email", email);

                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                userId = reader.GetInt32(0); // Get emp_id
                                storedHashedPassword = reader.GetString(1); // Get emp_password (hashed)
                                roleName = reader.GetString(2); // Get role_name

                                // Hash the provided password
                                string hashedPassword = HashPassword(password);

                                // Compare the hashed password with the stored one
                                if (hashedPassword == storedHashedPassword)
                                {
                                    return true; // Login successful
                                }
                            }
                            //else
                            //{
                            //    Response.Write("<script>alert('The login credentials associated were not found or suspended!');</script>");
                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            }

            return false; // Login failed
        }



        //FOR IS OPERATIONAL DISPATCHER
        private bool IsOperational_Dispatcher(string email, string password, out int userId, out string storedHashedPassword, out string roleName)
        {
            userId = 0; // Initialize userId
            storedHashedPassword = null; // Initialize storedHashedPassword
            roleName = null; // Initialize roleName

            try
            {
                using (var db = new NpgsqlConnection(con))
                {
                    db.Open();

                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;

                        // Query to get emp_id, emp_password, and role_name for the provided email where role_id = 4
                        cmd.CommandText = @"
                SELECT e.emp_id, e.emp_password, r.role_name 
                FROM employee e
                JOIN roles r ON e.role_id = r.role_id 
                WHERE e.emp_email = @Email AND e.role_id = 5 AND e.emp_status = 'Active'";

                        cmd.Parameters.AddWithValue("@Email", email);

                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                userId = reader.GetInt32(0); // Get emp_id
                                storedHashedPassword = reader.GetString(1); // Get emp_password (hashed)
                                roleName = reader.GetString(2); // Get role_name

                                // Hash the provided password
                                string hashedPassword = HashPassword(password);

                                // Compare the hashed password with the stored one
                                if (hashedPassword == storedHashedPassword)
                                {
                                    return true; // Login successful
                                }
                            }
                            //else
                            //{
                            //    Response.Write("<script>alert('The login credentials associated were not found or suspended!');</script>");
                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            }

            return false; // Login failed
        }

        private bool IsHauler(string email, string password, out int userId, out string storedHashedPassword, out string roleName)
        {
            userId = 0; // Initialize userId
            storedHashedPassword = null; // Initialize storedHashedPassword
            roleName = null; // Initialize roleName

            try
            {
                using (var db = new NpgsqlConnection(con))
                {
                    db.Open();

                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;

                        // Query to get emp_id, emp_password, and role_name where role_id = 2
                        cmd.CommandText = @"
                SELECT e.emp_id, e.emp_password, r.role_name 
                FROM employee e
                JOIN roles r ON e.role_id = r.role_id 
                WHERE e.emp_email = @Email AND e.role_id = 4 AND e.emp_status = 'Active'";

                        cmd.Parameters.AddWithValue("@Email", email);

                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                userId = reader.GetInt32(0); // Get emp_id
                                storedHashedPassword = reader.GetString(1); // Get emp_password (hashed)
                                roleName = reader.GetString(2); // Get role_name

                                // Hash the provided password
                                string hashedPassword = HashPassword(password);

                                // Compare the hashed password with the stored one
                                if (hashedPassword == storedHashedPassword)
                                {
                                    return true; // Login successful
                                }
                            }
                            //else
                            //{
                            //    // If no matching user found, show error
                            //    Response.Write("<script>alert('The login credentials associated were not found or suspended!');</script>");
                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            }

            return false; // Login failed
        }


        ////FOR IsHauler
        //private bool IsHauler(string email, string password)
        //{
        //    try
        //    {
        //        using (var db = new NpgsqlConnection(con))
        //        {
        //            db.Open();

        //            using (var cmd = db.CreateCommand())
        //            {
        //                cmd.CommandType = CommandType.Text;
        //                // Retrieve salt and hashed password from the database
        //                cmd.CommandText = "SELECT HAUL_SALT, HAUl_PASSWORD FROM HAULER WHERE HAUL_EMAIL = @email AND HAUL_STATUS = 'Active'";
        //                cmd.Parameters.AddWithValue("@email", email);

        //                using (NpgsqlDataReader read = cmd.ExecuteReader())
        //                {
        //                    if (read.Read())
        //                    {
        //                        string storedSalt = read.GetString(0);
        //                        string storedHash = read.GetString(1);

        //                        // Hash the provided password with the stored salt
        //                        string hashedPassword = HashPassword(password);

        //                        // Compare the hashed password
        //                        if (hashedPassword == storedHash)
        //                        {
        //                            Response.Write("<script>alert('Login Successful!, WELCOME HAULER!');</script>");
        //                            return true;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.Write("<script>alert('Message " + ex.Message + "');</script>");
        //    }

        //    return false;
        //}

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                var saltedPassword = password;
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));

                // Convert the hashed bytes to a hexadecimal string
                StringBuilder stringBuilder = new StringBuilder();
                foreach (byte b in hashedBytes)
                {
                    stringBuilder.Append(b.ToString("x2"));
                }

                return stringBuilder.ToString();
            }
        }

        //private bool PasswordHashMatches(string enteredPassword, string storedHashedPassword)
        //{
        //    using (SHA256 sha256 = SHA256.Create())
        //    {
        //        byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(enteredPassword));

        //        StringBuilder stringBuilder = new StringBuilder();
        //        foreach (byte b in hashedBytes)
        //        {
        //            stringBuilder.Append(b.ToString("x2"));
        //        }

        //        return stringBuilder.ToString() == storedHashedPassword;
        //    }
        //}

        protected void exampleInputPassword_TextChanged(object sender, EventArgs e)
        {
        }
    }
}
