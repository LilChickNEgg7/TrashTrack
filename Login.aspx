<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Capstone.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <meta name="description" content="">
    <meta name="author" content="">

    <title>TrashTrack Login</title>

    <!-- Custom fonts for this template-->
    <link href="vendor/fontawesome-free/css/all.min.css" rel="stylesheet" type="text/css">
    <link
        href="https://fonts.googleapis.com/css?family=Nunito:200,200i,300,300i,400,400i,600,600i,700,700i,800,800i,900,900i"
        rel="stylesheet">
    <!-- Custom styles for this template-->
    <link href="css/sb-admin-2.min.css" rel="stylesheet">


      <%--hide unhide pass--%>
  <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css" />

<link href="Pictures/logo_bgRM.png" rel="icon">
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
    <script src="https://cdn.jsdelivr.net/npm/apexcharts"></script>

<!-- Template Main CSS File -->
<link href="assets/css/style.css" rel="stylesheet">


    <style>

        /* Base Styles for the Button */
        .hover-toggle {
            transition: all 0.2s ease; /* Smooth transition for hover effects */
        }

        /* Hover Effect */
        .hover-toggle:hover {
            background-color: #031c02; /* Complementary dark background color */
            color: #fff; /* White text color for contrast */
            border-color: #2ba352; /* Change border color on hover */
            box-shadow: 0 8px 16px rgba(43, 163, 82, 0.5), 0 12px 24px rgba(43, 163, 82, 0.2); /* Soft green glow shadow */
            transform: scale(1.05); /* Slight scale up effect */
            text-shadow: 1px 1px 5px rgba(43, 163, 82, 0.7); /* Subtle text shadow */
        }

        /* Optional: Additional Styles for Smooth Hover */
        .hover-toggle:active {
            transform: scale(0.98); /* Slight scale down effect on click */
            box-shadow: 0 4px 8px rgba(43, 163, 82, 0.2); /* Reduced shadow effect */
        }

        /* Base Styles for the Button */
        .hover-toggle {
            transition: all 0.2s ease; /* Smooth transition for hover effects */
        }

        /* Hover Effect */
        .hover-toggle:hover {
            background-color: #031c02; /* Complementary dark background color */
            color: #fff; /* White text color for contrast */
            border-color: #2ba352; /* Change border color on hover */
            box-shadow: 0 8px 16px rgba(43, 163, 82, 0.5), 0 12px 24px rgba(43, 163, 82, 0.2); /* Soft green glow shadow */
            transform: scale(1.05); /* Slight scale up effect */
            text-shadow: 1px 1px 5px rgba(43, 163, 82, 0.7); /* Subtle text shadow */
        }

        /* Optional: Additional Styles for Smooth Hover */
        .hover-toggle:active {
            transform: scale(0.98); /* Slight scale down effect on click */
            box-shadow: 0 4px 8px rgba(43, 163, 82, 0.2); /* Reduced shadow effect */
        }


        /*eye*/
        .toggle-password i {
            font-size: 1.2rem;
            color: gray;
        }

        .toggle-password:hover i {
            color: #2ba352; /* Green color on hover */
        }
        /*end of eye*/

        /* Wrapper for the input and label */
        .custom-floating {
            position: relative;
            margin-bottom: 1.8rem; /* Adequate spacing between email and password */
            width: 100%;
        }

            /* Style for the asp:TextBox (input box) */
            .custom-floating input {
                width: 100%;
                padding: 25px 15px; /* More balanced padding */
                border: 2px solid #005c3b;
                background-color: #031c02;
                font-size: 16px;
                color: white;
                outline: none;
                border-radius: 30px;
                caret-color: #2ba352;
                transition: border-color 0.3s ease, box-shadow 0.3s ease; /* Smooth focus effect */
            }

            /* Style for the label */
            .custom-floating label {
                position: absolute;
                top: 50%; /* Perfect centering */
                left: 20px; /* Align with input padding */
                transform: translateY(-50%);
                font-size: 16px;
                color: gray;
                transition: all 0.3s ease; /* Smooth transition for floating effect */
                pointer-events: none; /* Prevents the label from blocking input focus */
            }

            /* When the input is focused or has content, float the label */
            .custom-floating input:focus + label,
            .custom-floating input:not(:placeholder-shown) + label {
                top: 1px;
                left: 20px;
                font-size: 12px;
                color: chartreuse;
                background-color: #031c02; /* Match the input background */
                padding: 0 8px; /* Small padding to fit within input */
            }

            /* Input border and shadow on focus */
            .custom-floating input:focus {
                border-color: #2ba352; /* Green border on focus */
                box-shadow: 0 0 5px rgba(43, 163, 82, 0.5); /* Slight glow effect */
            }



        .bg-login-image {
            position: relative;
            overflow: hidden; /* Hide any overflowing content */
        }

            .bg-login-image img {
                position: absolute;
                right: -12px; /* Adjust the right position to shift the image further to the right */
                top: 0;
                width: 100%;
                height: 100%;
                object-fit: cover;
                display: block;
            }

            .bg-login-image::after {
                content: "";
                position: absolute;
                top: 0;
                right: 0;
                bottom: 0;
                left: 0;
                background: rgba(0, 0, 0, 0.1); /* Adjust the alpha value to make it darker */
                z-index: 1; /* Ensure the overlay is on top of the image */
            }

        /* Style the label to create a custom checkbox appearance */
        .custom-control-input:checked + .custom-control-label::before {
            background-color: #2ba352; /* Change this to your desired background color */
            border-color: #2ba352; /* Change this to your desired border color */
            box-shadow: 0 0 0 0.1rem #005c3b; /* Adjust the color and spread as needed */
        }
    </style>
</head>

<body class="bg-gradient-primary" style="background-image: url('Pictures//nature_deer.png'); background-size: cover; background-position: center; background-repeat: no-repeat" >

    <div class="container" style="margin-top: 100px">

        <!-- Outer Row -->
        <div class="row justify-content-center">

            <div class="col-xl-10 col-lg-12 col-md-9">

                <div class="card o-hidden border-0 shadow-lg my-5">
                    <div class="card-body p-0" >
                        <!-- Nested Row within Card Body -->
                        <div class="row">
                            <div class="col-lg-6 d-none d-lg-block bg-login-image"><img src="Pictures//logo_tt.png" alt="logo"></div>
                            <div class="col-lg-6" style="background: #031c02">
                                <div class="p-5">
                                    <div class="text-center"">
                                            <h4  style="color: #2ba352; margin-bottom: 50px; font-weight: 700">TrashTrack</h4>

                                    </div>
                                    <form class="user" runat="server">

                                        <div class="custom-floating">
                                            <asp:TextBox ID="exampleInputEmail" runat="server" class="form-control"
                                                Style="background-color: #031c02; border-color: #005c3b; border-radius: 30px; border-width: 3px; color: white;"
                                                placeholder=" " aria-describedby=""></asp:TextBox>
                                            <label for="exampleInputEmail">Email Address</label>
                                        </div>

                                        <!-- Password Field -->
                                        <div class="custom-floating" style="position: relative;">
                                            <asp:TextBox ID="exampleInputPassword" runat="server" TextMode="Password" class="form-control"
                                                Style="background-color: #031c02; border-color: #005c3b; border-radius: 30px; border-width: 3px; color: white; padding-right: 40px;"
                                                placeholder=" " aria-describedby="" OnTextChanged="exampleInputPassword_TextChanged"></asp:TextBox>
                                            <label for="exampleInputPassword">Password</label>

                                            <!-- Eye Icon for toggling password visibility -->
                                            <span class="toggle-password" onclick="togglePassword()" style="position: absolute; right: 15px; top: 50%; transform: translateY(-50%); cursor: pointer;">
                                                <i id="toggleIcon" class="fas fa-eye-slash"  style="color: gray;"></i>
                                            </span>
                                        </div>

                                        <%--<!-- Remember Me Checkbox -->
                                        <div class="form-group">
                                            <div class="custom-control custom-checkbox small">
                                                <input type="checkbox" class="custom-control-input" id="customCheck" style="background-color: #2ba352" onchange="toggleRememberMe()">
                                                <label class="custom-control-label" for="customCheck" style="color: gray;">Remember Me</label>
                                                <div id="feedbackMessage" style="display:none; color: #2ba352; text-align:center; margin-top: 10px;"></div>

                                            </div>
                                        </div>--%>

                                        <!-- Login Button -->
                                        <asp:Button ID="login_btn" runat="server" class="btn btn-primary btn-user btn-block hover-toggle"
                                            Style="background-color: #2ba352; border-color: #005c3b; font-weight: 700; font-size: large;"
                                            Text="Login" OnClick="login_btn_Click" />
                                    </form>

                                    <hr>
                                    <div class="text-center">
                                        <a class="small" href="ForgotPassword.aspx" style="color: #2ba352">Forgot Password?</a>
                                    </div>

                                    <%--<div class="text-center">
                                        <a class="small" href="register.html">Create an Account!</a>
                                    </div>--%>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>

        </div>

    </div>

    <%--FUNCTION SCRITPS--%>
    <script>
        <%--function togglePassword() {
            var passwordField = document.getElementById('<%= exampleInputPassword.ClientID %>');
            var toggleIcon = document.getElementById('toggleIcon');

            if (passwordField.type === "password") {
                passwordField.type = "text";
                toggleIcon.classList.remove("fa-eye");
                toggleIcon.classList.add("fa-eye-slash");
            } else {
                passwordField.type = "password";
                toggleIcon.classList.remove("fa-eye-slash");
                toggleIcon.classList.add("fa-eye");
            }
        }--%>
        // Function to handle Remember Me functionality with expiration
        function toggleRememberMe() {
            const emailField = document.getElementById('<%= exampleInputEmail.ClientID %>');
            const rememberMe = document.getElementById('customCheck');

            if (rememberMe.checked) {
                // Save email to localStorage with an expiration time
                const email = emailField.value;
                const expirationTime = new Date().getTime() + (7 * 24 * 60 * 60 * 1000); // 7 days from now
                localStorage.setItem('rememberedEmail', email);
                localStorage.setItem('emailExpiration', expirationTime);
            } else {
                // Clear remembered email and expiration if unchecked
                localStorage.removeItem('rememberedEmail');
                localStorage.removeItem('emailExpiration');
            }
        }

        // On page load, check if there's a remembered email and if it's still valid
        window.onload = function () {
            const rememberedEmail = localStorage.getItem('rememberedEmail');
            const expirationTime = localStorage.getItem('emailExpiration');
            const currentTime = new Date().getTime();

            if (rememberedEmail && expirationTime && currentTime < expirationTime) {
                document.getElementById('<%= exampleInputEmail.ClientID %>').value = rememberedEmail;
                document.getElementById('customCheck').checked = true;
            } else {
                // Clear expired data
                localStorage.removeItem('rememberedEmail');
                localStorage.removeItem('emailExpiration');
            }
        };
        function showFeedbackMessage(message) {
            const feedbackMessage = document.getElementById('feedbackMessage');
            feedbackMessage.textContent = message;
            feedbackMessage.style.display = 'block';
            setTimeout(() => {
                feedbackMessage.style.display = 'none';
            }, 2000); // Hide after 2 seconds
        }

        function toggleRememberMe() {
            const emailField = document.getElementById('<%= exampleInputEmail.ClientID %>');
            const rememberMe = document.getElementById('customCheck');

            if (rememberMe.checked) {
                // Save email to localStorage with expiration time
                const email = emailField.value;
                const expirationTime = new Date().getTime() + (7 * 24 * 60 * 60 * 1000);
                localStorage.setItem('rememberedEmail', email);
                localStorage.setItem('emailExpiration', expirationTime);
                showFeedbackMessage("Email remembered!");
            } else {
                // Clear remembered email and expiration if unchecked
                localStorage.removeItem('rememberedEmail');
                localStorage.removeItem('emailExpiration');
                showFeedbackMessage("Email forgotten.");
            }
        }



        function togglePassword() {
            const password = document.getElementById('<%= exampleInputPassword.ClientID %>');
            const passIcon = document.getElementById('toggleIcon');
            if (password.type === "password") {
                password.type = "text";
                passIcon.classList.remove("fa-eye-slash");
                passIcon.classList.add("fa-eye");
            } else {
                password.type = "password";
                passIcon.classList.remove("fa-eye");
                passIcon.classList.add("fa-eye-slash");
            }
        }




        // Function to handle Remember Me functionality
        function toggleRememberMe() {
            var emailField = document.getElementById('<%= exampleInputEmail.ClientID %>');
            var rememberMe = document.getElementById('customCheck');

            if (rememberMe.checked) {
                // Save email to localStorage
                localStorage.setItem('rememberedEmail', emailField.value);
            } else {
                // Clear remembered email
                localStorage.removeItem('rememberedEmail');
            }
        }

        // On page load, check if there's a remembered email
        window.onload = function () {
            var rememberedEmail = localStorage.getItem('rememberedEmail');
            if (rememberedEmail) {
                document.getElementById('<%= exampleInputEmail.ClientID %>').value = rememberedEmail;
                document.getElementById('customCheck').checked = true;
            }
        };



        function triggerConfetti(event) {
            // Prevent the default behavior of the button click
            event.preventDefault();

            // Create a confetti burst effect at the button's position
            confetti({
                particleCount: 100,
                spread: 70,
                origin: {
                    x: event.clientX / window.innerWidth,
                    y: event.clientY / window.innerHeight
                },
                colors: ['#2ba352', '#031c02', '#ffffff']
            });

            // You can also add more animations here, like redirecting to a new page after the confetti ends
        }

    </script>
     <!-- Canvas Confetti Script -->
    <script src="https://cdn.jsdelivr.net/npm/canvas-confetti@1.4.0/dist/confetti.browser.min.js"></script>


    <!-- Bootstrap core JavaScript-->
    <script src="vendor/jquery/jquery.min.js"></script>
    <script src="vendor/bootstrap/js/bootstrap.bundle.min.js"></script>

    <!-- Core plugin JavaScript-->
    <script src="vendor/jquery-easing/jquery.easing.min.js"></script>

    <!-- Custom scripts for all pages-->
    <script src="js/sb-admin-2.min.js"></script>
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
