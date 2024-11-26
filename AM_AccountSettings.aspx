<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AM_AccountSettings.aspx.cs" Inherits="Capstone.AM_AccountSettings" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8">
    <meta content="width=device-width, initial-scale=1.0" name="viewport">

    <title>Account Settings</title>
    <meta content="" name="description">
    <meta content="" name="keywords">
    <%--hide unhide eye--%>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css" />
    <!-- Favicons -->
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
    <link href="assets/vendor/simple-datatables/style.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/apexcharts"></script>

    <!-- Template Main CSS File -->
    <link href="assets/css/style.css" rel="stylesheet">

    <%--<style>
        .square-image {
            width: 150px;
            height: 150px;
            object-fit: cover; /* Ensures the image fills the square without distortion */
            border-radius: 0; /* Optional: Ensures no rounded corners */
        }
    </style>--%>
    <style>
        .autocomplete-suggestions {
            border: 1px solid #ccc;
            border-top: none;
            position: absolute;
            z-index: 999;
            background: white;
            max-height: 200px;
            overflow-y: auto;
            width: 100%; /* Match width of the input */
        }

        .suggestion-item {
            padding: 10px;
            cursor: pointer;
        }

            .suggestion-item:hover {
                background-color: #f0f0f0;
            }




        /* Square image styling */
        .square-image {
            border-radius: 8px; /* Rounded corners */
        }

        /* Hidden file upload */
        .d-none {
            display: none;
        }

        /* Upload buttontyling */
        .btn-primary i {
            color: white;
        }
    </style>
</head>

<div>
    <body style="background-color: #041d06">

        <!-- ======= Header ======= -->
        <%--#9ee2a0, #9ee2a0, #9ee2a0--%>
        <%--  <header style="background-image: linear-gradient(to right, #000000, #061f0d, #000000); height: 80px" id="header" class="header fixed-top d-flex align-items-center">--%>
        <header style="background-color: black; height: 80px" id="header" class="header fixed-top d-flex align-items-center">

            <div class="d-flex align-items-center justify-content-between">
                <a href="Customer_Dashboard.aspx" class="logo d-flex align-items-center">
                    <img style="border-radius: 1px" src="Pictures/logo_bgRM.png" alt="" />
                    <span style="color: aqua; font-weight: 900; font-family: 'Agency FB'" class="d-none d-lg-block">TrashTrack</span>
                </a>
                <i style="color: aqua" class="bi bi-list toggle-sidebar-btn"></i>
            </div>
            <!-- End Logo -->

            <nav class="header-nav ms-auto">
                <ul class="d-flex align-items-center">

                    <li class="nav-item dropdown">

                        <ul class="dropdown-menu dropdown-menu-end dropdown-menu-arrow notifications">
                            <li>
                                <hr class="dropdown-divider">
                            </li>

                            <li>
                                <hr class="dropdown-divider">
                            </li>

                            <li>
                                <hr class="dropdown-divider">
                            </li>

                            <li>
                                <hr class="dropdown-divider">
                            </li>
                        </ul>
                        <!-- End Notification Dropdown Items -->

                    </li>
                    <!-- End Notification Nav -->

                    <li class="nav-item dropdown pe-3">

                        <a class="nav-link nav-profile d-flex align-items-center pe-0" href="#" data-bs-toggle="dropdown" style="color: aqua">
                            <asp:ImageMap ID="profile_image" runat="server" alt="Profile" class="rounded-circle"></asp:ImageMap>
                            <span style="color: aqua" class="d-none d-md-block dropdown-toggle ps-2">
                                <asp:Label ID="Label2" runat="server" Text=""></asp:Label></span>
                        </a>
                        <!-- End Profile Image Icon -->

                        <ul class="dropdown-menu dropdown-menu-end dropdown-menu-arrow profile">
                            <li class="dropdown-header">
                                <h6>
                                    <asp:Label ID="Label1" runat="server" Text=""></asp:Label></h6>
                                <span>
                                    <asp:Label ID="Label3" runat="server" Text="Customer"></asp:Label></span>
                            </li>
                            <li>
                                <hr class="dropdown-divider">
                            </li>
                            <li>
                                <hr class="dropdown-divider">
                            </li>

                            <li>
                                <a class="dropdown-item d-flex align-items-center" href="SAM_AccountSettings.aspx">
                                    <i class="bi bi-gear"></i>
                                    <span>Account Settings</span>
                                </a>
                            </li>
                            <li>
                                <hr class="dropdown-divider">
                            </li>

                            <li>
                                <hr class="dropdown-divider">
                            </li>

                            <li>
                                <a class="dropdown-item d-flex align-items-center" href="#" onclick="confirmLogout()">
                                    <i class="bi bi-box-arrow-right"></i>
                                    <span>Sign Out</span>
                                </a>
                            </li>
                            <script>
                                function confirmLogout() {
                                    var isConfirmed = confirm("Are you sure you want to log out?");
                                    if (isConfirmed) {
                                        // If confirmed, redirect to the logout page
                                        window.location.href = "LOGIN.aspx";
                                    } else {
                                        // If not confirmed, do nothing or handle as needed
                                    }
                                }
                            </script>
                        </ul>
                        <!-- End Profile Dropdown Items -->
                    </li>
                    <!-- End Profile Nav -->

                </ul>
            </nav>
            <!-- End Icons Navigation -->

        </header>
        <!-- End Header -->
        <form id="form1" runat="server">
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <!-- ======= Sidebar ======= -->
            <%--#052507--%>
            <aside style="padding-top: 50px" id="sidebar" class="sidebar">
                <ul class="sidebar-nav" id="sidebar-nav">

                    <li class="nav-item">
                        <a class="nav-link collapsed" href="AM_Dashboard.aspx">
                            <i class="bi bi-grid"></i>
                            <span>Dashboard</span>
                        </a>

                    </li>
                    <!-- End Employee Nav -->


                    <li class="nav-item">
                        <a class="nav-link collapsed" data-bs-target="#forms-nav" data-bs-toggle="collapse" href="#">
                            <i class="bi bi-people"></i><span>Manage Account</span><i class="bi bi-chevron-down ms-auto"></i>

                        </a>
                        <ul id="forms-nav" class="nav-content collapse" data-bs-parent="#sidebar-nav">
                            <li>
                                <a href="AM_AccountMan.aspx">
                                    <i class="bi bi-circle"></i><span>Employees</span>
                                </a>
                            </li>
                            <li>
                                <a href="AM_AccountManCustomers.aspx">
                                    <i class="bi bi-circle"></i><span>Customers</span>
                                </a>
                            </li>

                        </ul>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link collapsed" href="AM_Reports.aspx">
                            <i class="bi bi-grid"></i>
                            <span>Reports</span>
                        </a>

                    </li>
                </ul>
            </aside>
            <!-- End Sidebar-->

            <main id="main" class="main">

                <section class="section dashboard">
                    <div class="row">

                        <!-- Left side columns -->
                        <div class="col-lg-8">
                            <div class="row">
                            </div>
                        </div>
                        <!-- End Left side columns -->

                        <!-- Right side columns -->
                        <div class="col-lg-4">
                        </div>
                        <!-- End Right side columns -->
                    </div>
                </section>


                <div class="pagetitle">
                    <h1 style="padding-top: 20px; color: chartreuse">Profile</h1>
                    <%--<nav>
        <ol class="breadcrumb">
          <li class="breadcrumb-item"><a href="index.html">Home</a></li>
          <li class="breadcrumb-item">Users</li>
          <li class="breadcrumb-item active">Profile</li>
        </ol>
      </nav>--%>
                </div>
                <!-- End Page Title -->

                <section class="section profile">
                    <div class="row">
                        <div class="col-xl-4">

                            <div class="card">
                                <div class="card-body profile-card pt-4 d-flex flex-column align-items-center">

                                    <img alt="Profile" class="rounded-circle" id="prof" runat="server" style="object-fit: cover; overflow: hidden;">
                                    <h2>
                                        <asp:Label ID="profile_name" runat="server" Text=""></asp:Label></h2>
                                    <h3>
                                        <asp:Label ID="role_name" runat="server" Text=""></asp:Label></h3>
                                    <%--<div class="social-links mt-2">
                                    <a href="#" class="twitter"><i class="bi bi-twitter"></i></a>
                                    <a href="https://www.youtube.com/@michaelbacalso1168" class="facebook"><i class="bi bi-facebook"></i></a>
                                    <a href="https://www.youtube.com/@michaelbacalso1168" class="instagram"><i class="bi bi-instagram"></i></a>
                                    <a href="#" class="linkedin"><i class="bi bi-linkedin"></i></a>
                                </div>--%>
                                </div>
                            </div>

                        </div>

                        <div class="col-xl-8">

                            <div class="card">
                                <div class="card-body pt-3">
                                    <!-- Bordered Tabs -->
                                    <ul class="nav nav-tabs nav-tabs-bordered">

                                        <li class="nav-item">
                                            <button class="nav-link active" data-bs-toggle="tab" data-bs-target="#profile-overview">Overview</button>
                                        </li>

                                        <li class="nav-item">
                                            <button class="nav-link" data-bs-toggle="tab" data-bs-target="#profile-edit">Edit Profile</button>
                                        </li>

                                        <li class="nav-item">
                                            <button class="nav-link" data-bs-toggle="tab" data-bs-target="#profile-change-password">Change Password</button>
                                        </li>
                                    </ul>



                                    <!-- Hidden Field to Store Active Tab Index -->
                                    <asp:HiddenField ID="hfActiveTab" runat="server" />

                                    <div class="tab-content pt-2">

                                        <div class="tab-pane fade show active profile-overview" id="profile-overview">
                                            <%--<h5 class="card-title">About</h5>
                  <p class="small fst-italic">Sunt est soluta temporibus accusantium neque nam maiores cumque temporibus. Tempora libero non est unde veniam est qui dolor. Ut sunt iure rerum quae quisquam autem eveniet perspiciatis odit. Fuga sequi sed ea saepe at unde.</p>--%>

                                            <h5 class="card-title">Profile Details</h5>

                                            <div class="row">
                                                <div class="col-lg-3 col-md-4 label ">Full Name</div>
                                                <div class="col-lg-9 col-md-8">
                                                    <asp:Label ID="Label10" runat="server" Text=""></asp:Label>
                                                </div>
                                            </div>

                                            <div class="row">
                                                <div class="col-lg-3 col-md-4 label">Address</div>
                                                <div class="col-lg-9 col-md-8">
                                                    <asp:Label ID="Label6" runat="server" Text=""></asp:Label>
                                                </div>
                                            </div>

                                            <div class="row">
                                                <div class="col-lg-3 col-md-4 label">Phone</div>
                                                <div class="col-lg-9 col-md-8">
                                                    <asp:Label ID="Label7" runat="server" Text=""></asp:Label>
                                                </div>
                                            </div>

                                            <div class="row">
                                                <div class="col-lg-3 col-md-4 label">Email</div>
                                                <div class="col-lg-9 col-md-8">
                                                    <asp:Label ID="Label8" runat="server" Text=""></asp:Label>
                                                </div>
                                            </div>

                                        </div>

                                        <%--Profile Edit Form --%>
                                        <div class="tab-pane fade profile-edit pt-3" id="profile-edit">

                                            <div class="row mb-3">
                                                <label for="profileImage" class="col-md-4 col-lg-3 col-form-label">Profile Image</label>
                                                <div class="col-md-8 col-lg-9">
                                                    <!-- Profile Image Display -->
                                                    <asp:ImageMap
                                                        ID="image_edit"
                                                        runat="server"
                                                        alt="Profile"
                                                        CssClass="img-thumbnail square-image"
                                                        ImageUrl="~/Pictures/blank_prof.png"
                                                        Width="150px"
                                                        Height="150px" Style="object-fit: cover;">
                                                    </asp:ImageMap>

                                                    <!-- Custom Upload Button and FileUpload Control -->
                                                    <div class="pt-2">
                                                        <!-- Upload Button -->
                                                        <a href="javascript:void(0);" class="btn btn-primary btn-sm" title="Upload new profile image" onclick="triggerFileUpload()">
                                                            <i class="bi bi-upload"></i>
                                                        </a>

                                                        <!-- Hidden File Upload Control -->
                                                        <asp:FileUpload ID="formFile" runat="server" class="d-none" accept="image/*" onchange="previewImageUpdate()" />

                                                        <!-- Remove Profile Image Button -->

                                                        <a href="javascript:void(0);" class="btn btn-danger btn-sm" title="Remove my profile image" onclick="removeProfileImage(); return false;">
                                                            <i class="bi bi-trash"></i>
                                                            <asp:Button ID="btnHiddenRemoveImage" runat="server" Style="display: none;" />
                                                        </a>
                                                        <!-- Hidden field to track image removal -->
                                                        <asp:HiddenField ID="hfImageRemoved" runat="server" Value="false" />
                                                    </div>

                                                    <!-- Error Message -->
                                                    <div id="fileError" style="display: none; color: red;">Invalid file. Please upload a valid image.</div>
                                                </div>
                                            </div>

                                            <div class="row mb-3">


                                                <label for="firstname" class="col-md-4 col-lg-3 col-form-label">First Name</label>
                                                <div class="col-md-8 col-lg-9">
                                                    <asp:TextBox ID="firstname" runat="server" CssClass="form-control" onkeyup="validateFirstname()"></asp:TextBox>
                                                    <div class="valid-feedback">Looks good!</div>
                                                    <div class="invalid-feedback">Please provide a valid firstname.</div>
                                                </div>
                                            </div>

                                            <div class="row mb-3">
                                                <label for="m_initial" class="col-md-4 col-lg-3 col-form-label">M.I</label>
                                                <div class="col-md-8 col-lg-9">
                                                    <asp:TextBox ID="m_initial" runat="server" CssClass="form-control" onkeyup="validateMiddleInitialOptional()"></asp:TextBox>
                                                    <div class="valid-feedback">Looks good!</div>
                                                    <%--<div class="invalid-feedback">Please provide a valid firstname.</div>--%>
                                                </div>
                                            </div>

                                            <div class="row mb-3">
                                                <label for="lastname" class="col-md-4 col-lg-3 col-form-label">Last Name</label>
                                                <div class="col-md-8 col-lg-9">
                                                    <asp:TextBox ID="lastname" runat="server" CssClass="form-control" onkeyup="validateLastname()"></asp:TextBox>
                                                    <div class="invalid-feedback">Please provide a valid fastname.</div>
                                                </div>
                                            </div>

                                            <div class="row mb-3">
                                                <label for="address" class="col-md-4 col-lg-3 col-form-label">Address</label>
                                                <div class="col-md-8 col-lg-9">
                                                    <asp:TextBox ID="address" runat="server" CssClass="form-control" Placeholder="Enter your address" autocomplete="off" onkeyup="validateAddress()"></asp:TextBox>
                                                    <div id="suggestions" class="suggestions-list"></div>
                                                </div>
                                            </div>

                                            <div class="row mb-3">
                                                <label for="phone" class="col-md-4 col-lg-3 col-form-label">Phone</label>
                                                <div class="col-md-8 col-lg-9">
                                                    <asp:TextBox ID="phone" runat="server" CssClass="form-control" onkeyup="validateContact()"></asp:TextBox>
                                                </div>
                                            </div>

                                            <div class="row mb-3">
                                                <label for="email" class="col-md-4 col-lg-3 col-form-label">Email</label>
                                                <div class="col-md-8 col-lg-9">
                                                    <asp:TextBox ID="email" runat="server" CssClass="form-control" onkeyup="validateUpdateEmail()" AutoPostBack="true"></asp:TextBox>
                                                    <div class="valid-feedback">Looks good!</div>
                                                    <div class="invalid-feedback">Please provide a valid email.</div>
                                                    <!-- Placeholder for API error -->
                                                    <div id="emailError" class="text-danger" style="display: none;">Invalid email address.</div>
                                                </div>
                                            </div>

                                            <div class="text-center">
                                                <asp:Button ID="Button1" runat="server" CssClass="btn btn-primary" Text="Save Changes" OnClick="Button1_Click" />
                                                <asp:Button ID="Button2" runat="server" CssClass="btn btn-primary" Text="Edit" />
                                            </div>
                                        </div>
                                        <!-- End Profile Edit Form -->
                                        <%--<div class="tab-pane fade pt-3" id="profile-settings">

                              </div>--%>

                                        <div class="tab-pane fade pt-3" id="profile-change-password">
                                            <!-- Change Password Form -->

                                            <div class="row mb-3">
                                                <label for="currentPassword" class="col-md-4 col-lg-3 col-form-label">Current Password</label>
                                                <div class="col-md-8 col-lg-9">
                                                    <div class="input-group">
                                                        <asp:TextBox ID="currentpassword" runat="server" CssClass="form-control" TextMode="Password" oninput="validatePasswords()"></asp:TextBox>
                                                        <span class="input-group-text" style="cursor: pointer;" onclick="togglePasswordVisibility('currentpassword', 'currentPasswordIcon')">
                                                            <i id="currentPasswordIcon" class="fas fa-eye-slash"></i>
                                                        </span>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="row mb-3">
                                                <label for="newPassword" class="col-md-4 col-lg-3 col-form-label">New Password</label>
                                                <div class="col-md-8 col-lg-9">
                                                    <div class="input-group">
                                                        <asp:TextBox ID="changetxt" runat="server" CssClass="form-control" TextMode="Password" oninput="validatePasswords()"></asp:TextBox>
                                                        <span class="input-group-text" style="cursor: pointer;" onclick="togglePasswordVisibility('changetxt', 'newPasswordIcon')">
                                                            <i id="newPasswordIcon" class="fas fa-eye-slash"></i>
                                                        </span>
                                                    </div>
                                                    <small id="newPassMessage" class="text-danger"></small>
                                                </div>
                                            </div>

                                            <div class="row mb-3">
                                                <label for="renewPassword" class="col-md-4 col-lg-3 col-form-label">Re-enter New Password</label>
                                                <div class="col-md-8 col-lg-9">
                                                    <div class="input-group">
                                                        <asp:TextBox ID="confirmtxt" runat="server" CssClass="form-control" TextMode="Password" oninput="validatePasswords()"></asp:TextBox>
                                                        <span class="input-group-text" style="cursor: pointer;" onclick="togglePasswordVisibility('confirmtxt', 'confirmPasswordIcon')">
                                                            <i id="confirmPasswordIcon" class="fas fa-eye-slash"></i>
                                                        </span>
                                                    </div>
                                                    <small id="confirmPassMessage" class="text-danger"></small>
                                                </div>
                                            </div>

                                            <div class="text-center">
                                                <asp:Button ID="changepassword" runat="server" CssClass="btn btn-primary" Text="Change Password" OnClick="changepassword_Click" />

                                            </div>

                                            <!-- End Change Password Form -->
                                        </div>
                                    </div>
                                    <!-- End Bordered Tabs -->
                                    <%--                      </form>--%>
                                </div>
                            </div>

                        </div>
                    </div>

                </section>

            </main>
            <!-- End #main -->

            <!-- ======= Footer ======= -->
            <%--<footer id="footer" class="footer" style="border-top-color: chartreuse">
                <div class="copyright" style="color: #d4f3cf">
                    &copy; Copyright <strong><span style="color: #d4f3cf">Pinoy Basurero Corporation</span></strong>. All Rights Reserved
                </div>
            </footer>--%>
            <!-- End Footer -->

            <a href="#" class="back-to-top d-flex align-items-center justify-content-center"><i class="bi bi-arrow-up-short"></i></a>

            <%--inside script--%>
            <!-- Include jQuery and Bootstrap JS -->
            <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
            <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.9.2/dist/umd/popper.min.js"></script>
            <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>

            <!-- Script to Maintain Tab State -->
            <%--<script type="text/javascript">
    $(document).ready(function () {
        // Set the active tab based on the value in the hidden field.
        var activeTab = $("#<%= hfActiveTab.ClientID %>").val();
        if (activeTab) {
            $('#myTab button[data-bs-target="' + activeTab + '"]').tab('show');
        }

        // Store the active tab in the hidden field whenever a tab is shown.
        $('button[data-bs-toggle="tab"]').on('shown.bs.tab', function (e) {
            var activeTab = $(e.target).attr("data-bs-target");
            $("#<%= hfActiveTab.ClientID %>").val(activeTab);
        });

        // Save the active tab in the hidden field before any button triggers a postback.
        $('form').on('submit', function () {
            var activeTab = $('.nav-tabs .nav-link.active').attr("data-bs-target");
            $("#<%= hfActiveTab.ClientID %>").val(activeTab);
        });
    });
</script>--%>

            <script>

                const addressField = document.getElementById('address');
                const suggestionsContainer = document.getElementById('suggestions');
                const apiKey = '550eeed7c49946e98cfb4958e0c6d726'; // Replace with your Geoapify API key

                addressField.addEventListener('input', async () => {
                    const query = addressField.value.trim();
                    suggestionsContainer.innerHTML = '';

                    if (query.length > 0) {
                        const suggestions = await fetchSuggestions(query);
                        if (suggestions && suggestions.features) {
                            suggestions.features.forEach(suggestion => {
                                const div = document.createElement('div');
                                div.classList.add('suggestion-item');
                                div.innerText = suggestion.properties.formatted; // Use the formatted address
                                div.addEventListener('click', () => {
                                    addressField.value = suggestion.properties.formatted; // Set input to selected address
                                    suggestionsContainer.innerHTML = ''; // Clear suggestions
                                });
                                suggestionsContainer.appendChild(div);
                            });
                        }
                    }
                });

                async function fetchSuggestions(query) {
                    const requestOptions = {
                        method: 'GET',
                    };
                    const response = await fetch(`https://api.geoapify.com/v1/geocode/autocomplete?text=${encodeURIComponent(query)}&apiKey=${apiKey}`, requestOptions);
                    const data = await response.json();
                    return data; // Returns suggestions
                }

                // Close suggestions when clicking outside
                document.addEventListener('click', (event) => {
                    if (!addressField.contains(event.target)) {
                        suggestionsContainer.innerHTML = ''; // Clear suggestions if clicking outside
                    }
                });

                // Validation Functions for Add Account Manager

                // Validate Firstname to allow multiple words
                function validateFirstname() {
                    const firstname = document.getElementById('<%= firstname.ClientID %>');

                    // Allow alphabets, spaces, and hyphens for multiple-word first names
                    if (!/^[A-Za-z]+(?:[\s-][A-Za-z]+)*$/.test(firstname.value)) {
                        firstname.classList.add('is-invalid');
                        firstname.classList.remove('is-valid');
                    } else {
                        firstname.classList.remove('is-invalid');
                        firstname.classList.add('is-valid');
                    }
                }

                // Validate Middle Initial (Optional)
                function validateMiddleInitialOptional() {
                    const middleInitial = document.getElementById('<%= m_initial.ClientID %>');

                    // Validate the input
                    if (middleInitial.value.length > 1 || !/^[A-Za-z]$/.test(middleInitial.value)) {
                        middleInitial.classList.add('is-invalid');
                        middleInitial.classList.remove('is-valid');
                    } else {
                        middleInitial.classList.remove('is-invalid');
                        middleInitial.classList.add('is-valid');
                    }
                }

                // Validate Lastname
                function validateLastname() {
                    const lastname = document.getElementById('<%= lastname.ClientID %>');
                    if (!/^[A-Za-z]+$/.test(lastname.value)) {
                        lastname.classList.add('is-invalid');
                        lastname.classList.remove('is-valid');
                    } else {
                        lastname.classList.remove('is-invalid');
                        lastname.classList.add('is-valid');
                    }
                }

                // Validate Address (check if not empty and valid)
                function validateAddress() {
                    const address = document.getElementById('<%= address.ClientID %>');
                    if (address.value.trim() === "") {
                        address.classList.add('is-invalid');
                        address.classList.remove('is-valid');
                    } else {
                        address.classList.remove('is-invalid');
                        address.classList.add('is-valid');
                    }
                }

                // Validate Contact Number
                function validateContact() {
                    const contact = document.getElementById('<%= phone.ClientID %>');

                    // Remove non-numeric characters and limit to 11 digits
                    contact.value = contact.value.replace(/\D/g, ''); // Allow only numbers
                    if (contact.value.length > 11) {
                        contact.value = contact.value.substring(0, 11); // Limit to 11 digits
                    }

                    // Validation pattern for Philippines contact numbers
                    const contactPattern = /^09\d{9}$/; // Should start with 09 and followed by 9 digits
                    if (!contactPattern.test(contact.value)) {
                        contact.classList.add('is-invalid');
                        contact.classList.remove('is-valid');
                    } else {
                        contact.classList.remove('is-invalid');
                        contact.classList.add('is-valid');
                    }
                }

                function validateUpdateEmail() {
                    const email = document.getElementById('<%=email.ClientID%>');
                    const emailPattern = /^[a-zA-Z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,}$/;
                    if (!emailPattern.test(email.value)) {
                        email.classList.add('is-invalid');
                        email.classList.remove('is-valid');
                    } else {
                        email.classList.remove('is-invalid');
                        email.classList.add('is-valid');
                    }
                }

        <%--// Function to preview the selected image
        function previewImage() {
            const fileInput = document.getElementById('<%= formFile.ClientID %>');
            const imagePreview = document.getElementById('<%= image_edit.ClientID %>');
            const file = fileInput.files[0]; // Get the selected file
            const allowedExtensions = ["image/jpeg", "image/jpg", "image/png", "image/gif"];
            const fileError = document.getElementById('fileError');

            // Validate the file type and display preview if valid
            if (file && allowedExtensions.includes(file.type)) {
                const reader = new FileReader();
                reader.onload = function (e) {
                    imagePreview.src = e.target.result; // Set image preview to the selected file content
                };
                reader.readAsDataURL(file); // Read the file as a Data URL
                fileError.style.display = "none"; // Hide error message
            } else {
                // Set to default image and show error message if invalid file
                imagePreview.src = "~/Pictures/blank_prof.png";
                fileError.style.display = "block";
            }
        }--%>

        <%--function previewImageUpdate() {
            const fileInput = document.getElementById('<%=formFile.ClientID%>');
            const imagePreview = document.getElementById('<%=image_edit.ClientID%>');
            const file = fileInput.files[0];
            const allowedExtensions = ["image/jpeg", "image/png", "image/gif"];
            const fileError = document.getElementById('fileErrorUpdate');

            if (file && allowedExtensions.includes(file.type)) {
                const reader = new FileReader();
                reader.onload = function (e) {
                    imagePreview.src = e.target.result;
                    fileError.style.display = 'none';
                };
                reader.readAsDataURL(file);
            } else {
                /*imagePreview.src = "";*/
                style.display = 'block';
            }
        }--%>



                // Function to allow only letter input
                function isLetter(event) {
                    const char = String.fromCharCode(event.which);
                    return /^[A-Za-z]$/.test(char);
                }

                // Toggle password visibility
                function togglePasswordVisibility(textBoxId, iconId) {
                    const passwordField = document.getElementById(textBoxId);
                    const icon = document.getElementById(iconId);

                    if (passwordField.type === "password") {
                        passwordField.type = "text";
                        icon.classList.remove("fa-eye-slash");
                        icon.classList.add("fa-eye");
                    } else {
                        passwordField.type = "password";
                        icon.classList.remove("fa-eye");
                        icon.classList.add("fa-eye-slash");
                    }
                }

                // Validate Passwords in Real-Time
                function validatePasswords() {
                    const currentPassword = document.getElementById('<%= currentpassword.ClientID %>').value;
            const newPassword = document.getElementById('<%= changetxt.ClientID %>').value;
            const confirmPassword = document.getElementById('<%= confirmtxt.ClientID %>').value;
            const newPassMessage = document.getElementById('newPassMessage');
            const confirmPassMessage = document.getElementById('confirmPassMessage');
            const changePasswordButton = document.getElementById('<%= changepassword.ClientID %>');

                    // Password pattern to check if the new password meets the criteria
                    const passwordPattern = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,}$/;

                    // Flag to determine if the button should be enabled
                    let isValid = true;

                    // Check if the new password is the same as the current password
                    if (newPassword === currentPassword && currentPassword !== "") {
                        newPassMessage.textContent = "New password cannot be the same as the current password.";
                        isValid = false;
                    } else {
                        newPassMessage.textContent = ""; // Clear the message if passwords do not match
                    }

                    // Validate the new password format (minimum requirements)
                    if (!passwordPattern.test(newPassword) && newPassword !== "") {
                        newPassMessage.textContent = "Password must contain at least one uppercase letter, one lowercase letter, and one digit, and must be at least 6 characters long.";
                        isValid = false;
                    }

                    // Check if the confirmation password matches the new password
                    if (newPassword !== confirmPassword && confirmPassword !== "") {
                        confirmPassMessage.textContent = "The confirmation password does not match the new password.";
                        isValid = false;
                    } else {
                        confirmPassMessage.textContent = ""; // Clear the message if passwords match
                    }

                    // Enable or disable the Change Password button based on validation
                    changePasswordButton.disabled = !isValid;
                }

        <%--// Function to trigger the hidden file upload control
        function triggerFileUpload() {
            const fileUploadControl = document.getElementById('<%= formFile.ClientID %>');
            if (fileUploadControl) {
                fileUploadControl.click(); // Simulate a click to open file selector
            }
        }

        // Function to remove the profile image and reset to default
        function removeProfileImage() {
            const profileImage = document.getElementById('<%= image_edit.ClientID %>');
            if (profileImage) {
                // Use single quotes to avoid conflicts with double quotes
                profileImage.src = '<%= ResolveUrl("~/Pictures/blank_prof.png") %>';
            }
            const fileInput = document.getElementById('<%= formFile.ClientID %>');
            if (fileInput) {
                fileInput.value = ""; // Clear the file input control
            }


            // Set the hidden field to indicate the image was removed
            document.getElementById('<%= hfImageRemoved.ClientID %>').value = "true";
        }--%>


                // Function to trigger the hidden file upload control
                function triggerFileUpload() {
                    const fileUploadControl = document.getElementById('<%= formFile.ClientID %>');
                    if (fileUploadControl) {
                        fileUploadControl.click(); // Simulate a click to open file selector
                    }
                }

                // Function to remove the profile image and reset to default
                function removeProfileImage() {
                    const profileImage = document.getElementById('<%= image_edit.ClientID %>');
            if (profileImage) {
                // Set the image source to the default profile image
                profileImage.src = '<%= ResolveUrl("~/Pictures/blank_prof.png") %>';
            }
            const fileInput = document.getElementById('<%= formFile.ClientID %>');
            if (fileInput) {
                fileInput.value = ""; // Clear the file input control to reset any selected image
            }
            // Set the hidden field to indicate the image was removed
            document.getElementById('<%= hfImageRemoved.ClientID %>').value = "true";
                }

                // Function to handle previewing a new image and resetting the image removal status
                function previewImageUpdate() {
                    const fileInput = document.getElementById('<%= formFile.ClientID %>');
            const profileImage = document.getElementById('<%= image_edit.ClientID %>');

            // Check if a file was selected
            if (fileInput && fileInput.files && fileInput.files[0]) {
                // Create an object URL for the selected image to preview it
                const objectURL = URL.createObjectURL(fileInput.files[0]);
                if (profileImage) {
                    profileImage.src = objectURL;
                }
                // Reset the image removal hidden field if a new image is uploaded
                document.getElementById('<%= hfImageRemoved.ClientID %>').value = "false";
                    }
                }


        <%--$(document).ready(function () {
            // Set the active tab based on the value in the hidden field when the page loads.
            var activeTab = $("#<%= hfActiveTab.ClientID %>").val();
            if (activeTab) {
                // Show the stored active tab by matching the data-bs-target with the value from the hidden field
                $('button[data-bs-target="' + activeTab + '"]').tab('show');
            }

            // Store the active tab in the hidden field whenever a tab is shown.
            $('button[data-bs-toggle="tab"]').on('shown.bs.tab', function (e) {
                var activeTab = $(e.target).attr("data-bs-target");
                $("#<%= hfActiveTab.ClientID %>").val(activeTab);
            });

            // Save the active tab before form submission.1
            $('form').on('submit', function () {
                var activeTab = $('.nav-tabs .nav-link.active').attr("data-bs-target");
                $("#<%= hfActiveTab.ClientID %>").val(activeTab);
            });
        });--%>

                $(document).ready(function () {
                    // Retrieve the active tab from the hidden field
                    var activeTab = $("#<%= hfActiveTab.ClientID %>").val();

            // Check if there is an active tab stored in the hidden field
            if (activeTab) {
                console.log("Active tab on load: ", activeTab);  // Debugging output
                // Activate the stored tab
                $('button[data-bs-target="' + activeTab + '"]').tab('show');
            }

            // Store the active tab in the hidden field whenever a new tab is shown
            $('button[data-bs-toggle="tab"]').on('shown.bs.tab', function (e) {
                var activeTab = $(e.target).attr("data-bs-target");
                console.log("New active tab: ", activeTab);  // Debugging output
                $("#<%= hfActiveTab.ClientID %>").val(activeTab);
            });

            // Save the active tab before form submission
            $('form').on('submit', function () {
                var activeTab = $('.nav-tabs .nav-link.active').attr("data-bs-target");
                console.log("Form submission active tab: ", activeTab);  // Debugging output
                $("#<%= hfActiveTab.ClientID %>").val(activeTab);
            });
        });




            </script>


            <%--<!-- Include Google Maps API -->
    <script src="https://maps.googleapis.com/maps/api/js?key=YOUR_API_KEY&libraries=places&callback=initAutocomplete" async defer></script>--%>
            <!-- Include Geopify Maps API -->
            <script src="https://api.geoapify.com/v1/geocode/autocomplete?text=YOUR_TEXT&format=json&apiKey=550eeed7c49946e98cfb4958e0c6d726"></script>
            <%--    https://api.geoapify.com/v1/geocode/autocomplete?text=YOUR_TEXT&format=json&apiKey=550eeed7c49946e98cfb4958e0c6d726--%>


            <!-- Include Cropper.js CSS -->
            <link rel="stylesheet" href="https://unpkg.com/cropperjs/dist/cropper.min.css" />
            <!-- Include Cropper.js JS -->
            <script src="https://unpkg.com/cropperjs/dist/cropper.min.js"></script>
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
            <!-- SweetAlert2 CDN -->
            <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>

        </form>

    </body>
</div>
</html>

