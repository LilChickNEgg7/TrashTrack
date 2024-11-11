<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="Capstone.ForgotPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8">
    <meta content="width=device-width, initial-scale=1.0" name="viewport">
    <%--hide unhide pass--%>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css" />
    <title>Users / Profile - NiceAdmin Bootstrap Template</title>
    <script src="https://code.jquery.com/jquery-3.6.4.min.js"></script>
    <%--<script type="text/javascript">
        $(document).ready(function () {
            // Attach event handler to both textboxes on input change
            $('#<%= changetxt.ClientID %>').on('input', function () {
                  compareTextboxes();
              });

              $('#<%= confirmtxt.ClientID %>').on('input', function () {
                  compareTextboxes();
              });
          });
        function compareTextboxes() {
            var value1 = $('#<%= changetxt.ClientID %>').val();
              var value2 = $('#<%= confirmtxt.ClientID %>').val();

              var resultLabel = $('#<%= lblResult.ClientID %>');

            if (value1 !== "" && value2 !== "") {
                if (value1 === value2) {
                    // Values match
                    resultLabel.text('Values match!');
                    resultLabel.css('color', 'green');
                } else {
                    // Values do not match
                    resultLabel.text('Values do not match!');
                    resultLabel.css('color', 'red');
                }
                resultLabel.show(); // Show the label
            } else {
                // Either one or both values are empty
                resultLabel.hide(); // Hide the label
            }
        }

        function togglePasswordVisibility() {
            var changeTxt = document.getElementById('<%= changetxt.ClientID %>');


            // Toggle the type attribute between 'password' and 'text'
            changeTxt.type = (changeTxt.type === 'password') ? 'text' : 'password';

        }

        function togglePasswordVisibility2() {

            var confirmTxt = document.getElementById('<%= confirmtxt.ClientID %>');

            // Toggle the type attribute between 'password' and 'text'

            confirmTxt.type = (confirmTxt.type === 'password') ? 'text' : 'password';
        }

        function goToLoginPage() {
            window.location.href = "LOGIN.aspx";
        }
    </script>--%>
    <%--<script>

</script>--%>
    <meta content="" name="description">
    <meta content="" name="keywords">

    <!-- Favicons -->
    <link href="assets/img/favicon.png" rel="icon">
    <link href="assets/img/apple-touch-icon.png" rel="apple-touch-icon">

    <!-- Google Fonts -->
    <link href="https://fonts.gstatic.com" rel="preconnect">
    <link href="https://fonts.googleapis.com/css?family=Open+Sans:300,300i,400,400i,600,600i,700,700i|Nunito:300,300i,400,400i,600,600i,700,700i|Poppins:300,300i,400,400i,500,500i,600,600i,700,700i" rel="stylesheet">

    <!-- Vendor CSS Files -->
    <link href="assets/vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet">
    <link href="assets/vendor/bootstrap-icons/bootstrap-icons.css" rel="stylesheet">
    <link href="assets/vendor/boxicons/css/boxicons.min.css" rel="stylesheet">
    <link href="assets/vendor/quill/quill.snow.css" rel="stylesheet">
    <link href="assets/vendor/quill/quill.bubble.css" rel="stylesheet">
    <link href="assets/vendor/remixicon/remixicon.css" rel="stylesheet">
    <link href="assets/vendor/simple-datatables/style.css" rel="stylesheet">

    <!-- Template Main CSS File -->
    <link href="assets/css/style.css" rel="stylesheet">

    <style>
        /* Style for the toggle password eye icon */
        .toggle-password i {
            font-size: 1.2rem;
            color: gray;
        }

        .toggle-password:hover i {
            color: #2ba352; /* Green color on hover */
        }

        /* Disable style */
        .form-control[disabled] {
            background-color: #4b4b4b;
            opacity: 0.6;
        }

        /* Wrapper for input fields */
        .custom-floating {
            position: relative;
            margin-bottom: 1.5rem;
            width: 100%;
        }

            /* Style for input boxes */
            .custom-floating input {
                width: 100%;
                padding: 20px;
                border: 2px solid #005c3b;
                background-color: #031c02;
                font-size: 16px;
                color: white;
                outline: none;
                border-radius: 30px;
                caret-color: #2ba352;
                transition: border-color 0.3s ease, box-shadow 0.3s ease;
            }

            /* Style for the label */
            .custom-floating label {
                position: absolute;
                top: 50%;
                left: 20px;
                transform: translateY(-50%);
                font-size: 16px;
                color: gray;
                transition: all 0.3s ease;
                pointer-events: none;
            }

            /* Floating effect for label */
            .custom-floating input:focus + label,
            .custom-floating input:not(:placeholder-shown) + label {
                top: 5px;
                left: 20px;
                font-size: 12px;
                color: #2ba352;
                background-color: #031c02;
                padding: 0 8px;
            }

            /* Input focus effect */
            .custom-floating input:focus {
                border-color: #2ba352;
                box-shadow: 0 0 5px rgba(43, 163, 82, 0.5);
            }

        /* Button styles */
        .btn {
            background-color: #2ba352;
            border-color: #005c3b;
            color: white;
            padding: 10px 20px;
            font-size: 16px;
            border-radius: 30px;
            font-weight: bold;
        }

            .btn:hover {
                background-color: #28a745;
            }

        .btn-secondary {
            background-color: #6c757d;
            border-color: #6c757d;
            color: white;
        }

            .btn-secondary:hover {
                background-color: #5a6268;
            }
    </style>
</head>

<body>



    <main id="main" class="main">
        <%--<section class="section profile" style="background-color: #000000;">--%>
        <div class="col-xl-8">
            <div class="card" style="background-color: #043108;">
                <div class="card-body pt-3" style="color: chartreuse;">
                    <div class="tab-content pt-2">
                        <div class="tab-pane fade show active profile-overview" id="profile-overview">
                            <h5 class="card-title" style="color: aquamarine;">FORGOT PASSWORD</h5>
                            <form runat="server">
                                <%--                                <div class="col-md-8 col-lg-8 center">--%>
                                <div class="col-12 mb-3">

                                    <div class="custom-floating">
                                        <asp:TextBox ID="emailtxt" runat="server" class="form-control" Style="background-color: #031c02; border-color: #005c3b; border-radius: 30px; border-width: 3px; color: white;" placeholder=" " aria-describedby=""></asp:TextBox>
                                        <label for="emailtxt">Enter Your Email</label>
                                        <div class="valid-feedback">Looks good!</div>
                                        <div class="invalid-feedback">Please provide a valid email address.</div>
                                    </div>
                                </div>
                                <div class="text-center">
                                    <asp:Button ID="Button1" CssClass="btn btn-primary" runat="server" Text="Send Email" OnClick="Button1_Click" />
                                </div>
                                <br />
                                <div style="text-align: center; margin-top: 10px;">
                                    <label for="OTPtxt1" style="color: white; font-size: 18px;">Enter OTP From Email</label>
                                </div>
                                <div id="otpContainer" style="display: flex; gap: 15px; justify-content: center; align-items: center; padding: 20px;">
                                    <asp:TextBox ID="OTPtxt1" runat="server" class="form-control otp-input" MaxLength="1" TextMode="SingleLine" Enabled="false" Style="background-color: #031c02; border-color: #005c3b; border-radius: 8px; border-width: 2px; color: white; width: 60px; height: 60px; text-align: center; font-size: 28px;" />
                                    <asp:TextBox ID="OTPtxt2" runat="server" class="form-control otp-input" MaxLength="1" TextMode="SingleLine" Enabled="false" Style="background-color: #031c02; border-color: #005c3b; border-radius: 8px; border-width: 2px; color: white; width: 60px; height: 60px; text-align: center; font-size: 28px;" />
                                    <asp:TextBox ID="OTPtxt3" runat="server" class="form-control otp-input" MaxLength="1" TextMode="SingleLine" Enabled="false" Style="background-color: #031c02; border-color: #005c3b; border-radius: 8px; border-width: 2px; color: white; width: 60px; height: 60px; text-align: center; font-size: 28px;" />
                                    <asp:TextBox ID="OTPtxt4" runat="server" class="form-control otp-input" MaxLength="1" TextMode="SingleLine" Enabled="false" Style="background-color: #031c02; border-color: #005c3b; border-radius: 8px; border-width: 2px; color: white; width: 60px; height: 60px; text-align: center; font-size: 28px;" />
                                    <asp:TextBox ID="OTPtxt5" runat="server" class="form-control otp-input" MaxLength="1" TextMode="SingleLine" Enabled="false" Style="background-color: #031c02; border-color: #005c3b; border-radius: 8px; border-width: 2px; color: white; width: 60px; height: 60px; text-align: center; font-size: 28px;" />
                                    <asp:TextBox ID="OTPtxt6" runat="server" class="form-control otp-input" MaxLength="1" TextMode="SingleLine" Enabled="false" Style="background-color: #031c02; border-color: #005c3b; border-radius: 8px; border-width: 2px; color: white; width: 60px; height: 60px; text-align: center; font-size: 28px;" />
                                </div>
                                <div id="otpMessage" style="text-align: center; margin-top: 10px;">
                                    <asp:Label ID="lblOTPMessage" runat="server" Text="" ForeColor="Red" Font-Bold="True"></asp:Label>
                                </div>
                                <div class="text-center">
                                    <asp:Button ID="Button2" CssClass="btn btn-primary" runat="server" Text="Submit" OnClick="VerifyOTP" />
                                </div>
                                <br />
                                <br />
                                <br />
                                <div class="custom-floating">
                                    <div class="col-12 mb-3">

                                        <asp:TextBox ID="changetxt" runat="server" class="form-control" TextMode="Password"
                                            Style="background-color: #031c02; border-color: #005c3b; border-radius: 30px; border-width: 3px; color: white;"
                                            placeholder=" " aria-describedby="" Enabled="False" oninput="compareTextboxes()"></asp:TextBox>
                                        <label for="changetxt">Change Password</label>
                                    </div>
                                    <!-- Eye Icon for toggling password visibility -->
                                    <span class="toggle-password" onclick="togglePassword('<%= changetxt.ClientID %>', 'toggleIcon1')"
                                        style="position: absolute; right: 15px; top: 50%; transform: translateY(-50%); cursor: pointer; padding-right: 25px;">
                                        <i id="toggleIcon1" class="fas fa-eye-slash" style="color: gray;"></i>
                                    </span>
                                    
                                </div>
                                <br /><span class="valid-feedback">Looks good! Password is strong.</span>
                                    <span class="invalid-feedback">Password must be at least 6 characters, include one uppercase letter, one number, and one special character.</span>

                                <div class="custom-floating">
                                    <asp:TextBox ID="confirmtxt" runat="server" class="form-control" TextMode="Password"
                                        Style="background-color: #031c02; border-color: #005c3b; border-radius: 30px; border-width: 3px; color: white;"
                                        placeholder=" " aria-describedby="" Enabled="False" oninput="compareTextboxes()"></asp:TextBox>

                                    <label for="confirmtxt">Confirm Password</label>
                                    <asp:Label ID="lblResult" runat="server" Text="" Style="display: none;"></asp:Label>

                                    <!-- Eye Icon for toggling password visibility -->
                                    <span class="toggle-password" onclick="togglePassword('<%= confirmtxt.ClientID %>', 'toggleIcon2')"
                                        style="position: absolute; right: 15px; top: 50%; transform: translateY(-50%); cursor: pointer; padding-right: 25px;">
                                        <i id="toggleIcon2" class="fas fa-eye-slash" style="color: gray;"></i>
                                    </span>
                                </div>

                                <br />
                                <br />
                                  <br />
                                <br />

                                <div class="text-center">
                                    <label for="currentPassword" class="col-md-4 col-lg-3 col-form-label"></label>

                                    <div class="text-center">
                                        <a class="small" href="Login.aspx" style="color: #2ba352">Back to Login</a>
                                    </div>
                                    <br />
                                    <asp:Button ID="changepassword" runat="server" CssClass="btn btn-warning" Text="Change Password" Enabled="False" OnClick="changepassword_Click" />
                                </div>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <%--    </section>--%>
    </main>
    <!-- End #main -->
    <script>
        document.addEventListener("DOMContentLoaded", function () {
            // OTP Input Handling
            const otpInputs = document.querySelectorAll(".otp-input");

            otpInputs.forEach((input, index) => {
                // Handle input event
                input.addEventListener("input", function () {
                    // Allow only numbers
                    this.value = this.value.replace(/[^0-9]/g, '');

                    // Move focus to next input if current input is filled
                    if (this.value.length === 1 && index < otpInputs.length - 1) {
                        otpInputs[index + 1].focus();
                    }

                    // Check if all OTP inputs are filled
                    if (Array.from(otpInputs).every(input => input.value.length === 1)) {
                        // Get the OTP entered by the user
                        const otp = Array.from(otpInputs).map(input => input.value).join('');

                        // Proceed to OTP verification
                        verifyOtp(otp); // Call server-side OTP verification function
                    }
                });

                // Handle keydown event (Backspace to move focus to previous input)
                input.addEventListener("keydown", function (e) {
                    if (e.key === "Backspace" && this.value === "" && index > 0) {
                        otpInputs[index - 1].focus();
                    }
                });
            });

            function verifyOTP() {
                // Collect OTP values from inputs
                const otp = Array.from(document.querySelectorAll(".otp-input")).map(input => input.value).join("");

                // Send OTP to server for validation
                fetch("verifyOTP.aspx", {  // Assuming a separate page or handler for OTP validation
                    method: "POST",
                    headers: {
                        "Content-Type": "application/x-www-form-urlencoded"
                    },
                    body: `otp=${otp}`
                })
                    .then(response => response.json())
                    .then(data => {
                        if (data.valid) {
                            // OTP is valid, enable password fields
                            document.getElementById('<%= changetxt.ClientID %>').disabled = false;
                            document.getElementById('<%= confirmtxt.ClientID %>').disabled = false;
                            document.getElementById('<%= changepassword.ClientID %>').disabled = false;
                            document.getElementById('lblOTPMessage').innerText = "OTP Verified. You can now change your password.";
                            document.getElementById('lblOTPMessage').style.color = 'green';
                        } else {
                            // OTP is invalid, show error
                            document.getElementById('lblOTPMessage').innerText = "Incorrect OTP. Please try again.";
                            document.getElementById('lblOTPMessage').style.color = 'red';
                        }
                    })
                    .catch(error => {
                        console.error("Error verifying OTP:", error);
                    });
            }

            // OTP Verification Function
            function verifyOtp(otp) {
                // Example: Perform server-side OTP verification using __doPostBack
                // You can customize this according to your server-side logic
                __doPostBack('VerifyOTP', otp);
            }

            // Password Validation
            document.getElementById("<%= changetxt.ClientID %>").addEventListener("input", validatePassword);

            function validatePassword() {
                const password = document.getElementById('<%= changetxt.ClientID %>');
                const passwordPattern = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,}$/;
                if (!passwordPattern.test(password.value)) {
                    password.classList.add('is-invalid');
                    password.classList.remove('is-valid');
                } else {
                    password.classList.remove('is-invalid');
                    password.classList.add('is-valid');
                }
            }

            // Email Validation
            document.getElementById("<%= emailtxt.ClientID %>").addEventListener("input", validateEmail);

            function validateEmail() {
                const email = document.getElementById('<%= emailtxt.ClientID %>');
                const emailPattern = /^[a-zA-Z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,}$/;
                if (!emailPattern.test(email.value)) {
                    email.classList.add('is-invalid');
                    email.classList.remove('is-valid');
                } else {
                    email.classList.remove('is-invalid');
                    email.classList.add('is-valid');
                }
            }

            // Compare Textboxes (for password confirmation)
            function compareTextboxes() {
                var value1 = $('#<%= changetxt.ClientID %>').val();
                var value2 = $('#<%= confirmtxt.ClientID %>').val();

                var resultLabel = $('#<%= lblResult.ClientID %>');

                if (value1 !== "" && value2 !== "") {
                    if (value1 === value2) {
                        resultLabel.text('Values match!');
                        resultLabel.css('color', 'green');
                    } else {
                        resultLabel.text('Values do not match!');
                        resultLabel.css('color', 'red');
                    }
                    resultLabel.show();
                } else {
                    resultLabel.hide();
                }
            }

            // Toggle Password Visibility
            function togglePassword(passwordId, iconId) {
                const passwordField = document.getElementById(passwordId);
                const passIcon = document.getElementById(iconId);

                if (passwordField.type === "password") {
                    passwordField.type = "text";
                    passIcon.classList.remove("fa-eye-slash");
                    passIcon.classList.add("fa-eye");
                } else {
                    passwordField.type = "password";
                    passIcon.classList.remove("fa-eye");
                    passIcon.classList.add("fa-eye-slash");
                }
            }

            window.togglePassword = togglePassword; // Expose function to global scope if needed
        });
    </script>



    <a href="#" class="back-to-top d-flex align-items-center justify-content-center"><i class="bi bi-arrow-up-short"></i></a>

    <!-- Vendor JS Files -->
    <script src="assets/vendor/apexcharts/apexcharts.min.js"></script>
    <script src="assets/vendor/bootstrap/js/bootstrap.bundle.min.js"></script>
    <script src="assets/vendor/chart.js/chart.umd.js"></script>
    <script src="assets/vendor/echarts/echarts.min.js"></script>
    <script src="assets/vendor/quill/quill.min.js"></script>
    <script src="assets/vendor/simple-datatables/simple-datatables.js"></script>
    <script src="assets/vendor/tinymce/tinymce.min.js"></script>
    <script src="assets/vendor/php-email-form/validate.js"></script>

    <!-- Template Main JS File -->
    <script src="assets/js/main.js"></script>

</body>

</html>
