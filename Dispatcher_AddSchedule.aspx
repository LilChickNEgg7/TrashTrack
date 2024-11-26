<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dispatcher_AddSchedule.aspx.cs" Inherits="Capstone.Dispatcher_AddSchedule" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8">
    <meta content="width=device-width, initial-scale=1.0" name="viewport">

    <title>TrashTrack</title>
    <meta content="" name="description">
    <meta content="" name="keywords">

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
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>

    <!-- Template Main CSS File -->
    <link href="assets/css/style.css" rel="stylesheet">

    <style>
        .form-control {
            background-color: #0a3a0b; /* Slightly lighter than #052507 */
            color: #ffffff; /* White text for readability */
            border: 1px solid #0a3a0b;
        }

            .form-control:focus {
                background-color: #0a3a0b;
                color: #ffffff;
                border-color: #5cb85c; /* Lighter green border on focus */
                box-shadow: 0 0 5px rgba(92, 184, 92, 0.5);
            }

            .form-control[disabled], .form-control[readonly] {
                background-color: #1a2e1b; /* Darker background for better contrast */
                color: #ffffff; /* White text for readability */
                opacity: 1; /* Remove transparency */
                border: 1px solid #1a2e1b;
            }

        .btn-primary {
            background-color: #0a3a0b;
            color: #ffffff;
            border: 1px solid #5cb85c; /* Matching border color */
        }

            .btn-primary:hover {
                background-color: #5cb85c; /* Slightly lighter green on hover */
                color: #052507;
            }
    </style>


    <%--<style>
  .nav-item:hover i, .nav-item:hover span {
    color: aquamarine !important;
  }
</style>--%>

    <%--<style>
  .nav-item:hover i,
  .nav-item:hover span,
  .nav-item.active i,
  .nav-item.active span {
    color: aquamarine !important;
  }
</style>

<script>
  document.addEventListener('DOMContentLoaded', function () {
    var navItems = document.querySelectorAll('.nav-item');

    navItems.forEach(function (item) {
      item.addEventListener('click', function () {
        // Remove 'active' class from all items
        navItems.forEach(function (otherItem) {
          otherItem.classList.remove('active');
        });

        // Add 'active' class to the clicked item
        item.classList.add('active');
      });
    });
  });
</script>--%>


    <%--<style>s
    table {
        border-collapse: collapse;
        width: 80%;
        margin-bottom: 20px;
    }

    table, th, td {
        border: 1px solid black;
    }

    th, td {
        padding: 10px;
        text-align: left;
    }

    #moreButton {
        margin-bottom: 10px;
    }
</style>--%>
</head>
<form id="form2" runat="server">
    <div>
        <body style="background-color: #041d06">

            <!-- ======= Header ======= -->
            <%--#9ee2a0, #9ee2a0, #9ee2a0--%>
            <%--  <header style="background-image: linear-gradient(to right, #000000, #061f0d, #000000); height: 80px" id="header" class="header fixed-top d-flex align-items-center">--%>
            <header style="background-color: black; height: 80px" id="header" class="header fixed-top d-flex align-items-center">

                <div class="d-flex align-items-center justify-content-between">
                    <a href="WAREHOUSE_ADD_ITEM.aspx" class="logo d-flex align-items-center">
                        <img style="border-radius: 1px" src="Pictures/logo_bgRM.png" alt="" />
                        <span style="color: aqua; font-weight: 900; font-family: 'Agency FB'" class="d-none d-lg-block">TrashTrack</span>
                    </a>
                    <i style="color: aqua" class="bi bi-list toggle-sidebar-btn"></i>
                </div>
                <!-- End Logo -->

                <%--<div class="search-bar">
      <form class="search-form d-flex align-items-center" method="POST" action="#">--%>
                <%--<asp:TextBox name="query" placeholder="Search" title="Enter search keyword" runat="server"></asp:TextBox>--%>
                <%--<input type="text" name="query" placeholder="Search" title="Enter search keyword">--%>
                <%--<asp:Button ID="Button1" title="Search"><i class="bi bi-search"></i> runat="server"/>--%>
                <%--<button type="submit" title="Search"><i class="bi bi-search"></i></button>--%>

                <%--</form>
    </div>--%><!-- End Search Bar -->

                <nav class="header-nav ms-auto">
                    <ul class="d-flex align-items-center">

                        <%--<li class="nav-item d-block d-lg-none">
          <a class="nav-link nav-icon search-bar-toggle " href="#">
            <i class="bi bi-search"></i>
          </a>
        </li>--%><!-- End Search Icon-->

                        <li class="nav-item dropdown">

                            <%--<a class="nav-link nav-icon" href="#" data-bs-toggle="dropdown">
            <i class="bi bi-bell"></i>
            <span class="badge bg-primary badge-number">4</span>
          </a>--%><!-- End Notification Icon -->

                            <ul class="dropdown-menu dropdown-menu-end dropdown-menu-arrow notifications">
                                <%--<li class="dropdown-header">
              You have 4 new notifications
              <a href="#"><span class="badge rounded-pill bg-primary p-2 ms-2">View all</span></a>
            </li>--%>
                                <li>
                                    <hr class="dropdown-divider">
                                </li>

                                <%--<li class="notification-item">
              <i class="bi bi-exclamation-circle text-warning"></i>
              <div>
                <h4>Lorem Ipsum</h4>
                <p>Quae dolorem earum veritatis oditseno</p>
                <p>30 min. ago</p>
              </div>
            </li>--%>

                                <li>
                                    <hr class="dropdown-divider">
                                </li>

                                <%--<li class="notification-item">
              <i class="bi bi-x-circle text-danger"></i>
              <div>
                <h4>Atque rerum nesciunt</h4>
                <p>Quae dolorem earum veritatis oditseno</p>
                <p>1 hr. ago</p>
              </div>
            </li>--%>

                                <li>
                                    <hr class="dropdown-divider">
                                </li>

                                <%--<li class="notification-item">
              <i class="bi bi-check-circle text-success"></i>
              <div>
                <h4>Sit rerum fuga</h4>
                <p>Quae dolorem earum veritatis oditseno</p>
                <p>2 hrs. ago</p>
              </div>
            </li>--%>

                                <li>
                                    <hr class="dropdown-divider">
                                </li>

                                <%--<li class="notification-item">
              <i class="bi bi-info-circle text-primary"></i>
              <div>
                <h4>Dicta reprehenderit</h4>
                <p>Quae dolorem earum veritatis oditseno</p>
                <p>4 hrs. ago</p>
              </div>
            </li>--%>

                                <%--<li>
              <hr class="dropdown-divider">
            </li>
            <li class="dropdown-footer">
              <a href="#">Show all notifications</a>
            </li>--%>
                            </ul>
                            <!-- End Notification Dropdown Items -->

                        </li>
                        <!-- End Notification Nav -->

                        <li class="nav-item dropdown pe-3">

                            <a class="nav-link nav-profile d-flex align-items-center pe-0" href="#" data-bs-toggle="dropdown" style="color: aqua">
                                <asp:ImageMap ID="profile_image" runat="server" alt="Profile" class="rounded-circle" Style="background-color: #053203"></asp:ImageMap>
                                <span style="color: aqua" class="d-none d-md-block dropdown-toggle ps-2">
                                    <asp:Label ID="Label2" runat="server" Text=""></asp:Label></span>
                            </a>
                            <!-- End Profile Image Icon -->

                            <ul class="dropdown-menu dropdown-menu-end dropdown-menu-arrow profile">
                                <li class="dropdown-header">
                                    <h6>
                                        <asp:Label ID="Label1" runat="server" Text=""></asp:Label></h6>
                                    <span>
                                        <asp:Label ID="Employee_Role" runat="server" Text=""></asp:Label></span>
                                </li>
                                <li>
                                    <hr class="dropdown-divider">
                                </li>

                                <%--<li>
              <a class="dropdown-item d-flex align-items-center" href="users-profile.html">
                <i class="bi bi-person"></i>
                <span>My Profile</span>
              </a>
            </li>--%>
                                <li>
                                    <hr class="dropdown-divider">
                                </li>

                                <li>
                                    <a class="dropdown-item d-flex align-items-center" href="Dispatcher_AccountSettings.aspx">
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

            <!-- ======= Sidebar ======= -->
            <%--#052507--%>
            <aside style="padding-top: 50px" id="sidebar" class="sidebar">

                <ul class="sidebar-nav" id="sidebar-nav">

                    <li class="nav-item">
                        <a class="nav-link collapsed" href="Dispatcher_Dashboard.aspx">
                            <i class="bi bi-grid" style="color: aquamarine"></i>
                            <span style="color: aquamarine">Dashboard</span>
                        </a>

                    </li>
                    <!-- End Employee Nav -->


                    <li class="nav-item">
                        <a class="nav-link  " data-bs-target="#forms-nav" data-bs-toggle="collapse" href="#">
                            <i class="bi bi-people"></i><span>Manage Actions</span><i class="bi bi-chevron-down ms-auto"></i>

                        </a>
                        <ul id="forms-nav" class="nav-content collapse show" data-bs-parent="#sidebar-nav">
                            <li>
                                <a href="OD_manage.aspx">
                                    <i class="bi bi-circle"></i><span>Manage Vehicle and Haulers</span>
                                </a>
                            </li>
                            <li>
                                <a href="Dispatcher_AddSchedule.aspx" class="active">
                                    <i class="bi bi-circle"></i><span>Manage Booking Limit</span>
                                </a>
                            </li>
                            <li>
                                <a href="Dispatcher_AddSlip.aspx">
                                    <i class="bi bi-circle"></i><span>Manage Booking Slip</span>
                                </a>
                            </li>
                            <li>
                                <a href="Re_AssignVehicle.aspx">
                                    <i class="bi bi-circle"></i><span>Assigns and Control</span>
                                </a>
                            </li>

                        </ul>
                    </li>

                </ul>
            </aside>
            <!-- End Sidebar-->

            <main id="main" class="main">

                <div class="pagetitle">
                    <h1 style="padding-top: 20px; color: chartreuse">Set Booking Limit</h1>
                    <nav>
                        <%--<ol class="breadcrumb">
                            <li class="breadcrumb-item"><a href="Dispatcher_AddSchedule.aspx">Dispatcher Add Schedule</a></li>
                            <!--<li class="breadcrumb-item">Add Item</li>-->
                        </ol>--%>
                    </nav>
                </div>
                <!-- End Page Title -->

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
                <div class="row">
                    <div class="col-lg-6">
                        <div class="card" style="background-color: #052507; color: #ffffff; border-radius: 8px; padding: 20px; box-shadow: 0 0 5px rgba(0, 0, 0, .2)">
                            <div class="card-body">
                                <h5 class="card-title" style="color: #ffffff;">Set Booking Limit</h5>
                                <form>
                                    <!-- Max Schedule for each day -->
                                    <div class="row mb-3">
                                        <label for="inputText" class="col-sm-2 col-form-label" style="color: #ffffff;">Max Schedule for Day</label>
                                        <div class="col-sm-10">
                                            <asp:TextBox CssClass="form-control" ID="max_schedule" runat="server"></asp:TextBox>
                                             <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="max_schedule"
                                                ErrorMessage="Max Schedule for Day is required" CssClass="text-danger" Display="Dynamic" ValidationGroup="SetSchedule"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>

                                    <!-- Max Booking Count -->
                                    <div class="row mb-3">
                                        <label for="inputText" class="col-sm-2 col-form-label" style="color: #ffffff;">Max Booking Count</label>
                                        <div class="col-sm-10">
                                            <asp:TextBox CssClass="form-control" ID="max_count" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="max_count"
                                                ErrorMessage="Max Booking Count is required" CssClass="text-danger" Display="Dynamic" ValidationGroup="SetSchedule"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>

                                    <!-- Closing Time for Booking -->
                                    <div class="row mb-3">
                                        <label for="TextBox1" class="col-sm-2 col-form-label" style="color: #ffffff;">Schedule Closing Time</label>
                                        <div class="col-sm-10">
                                            <asp:TextBox ID="closing_time" runat="server" TextMode="Time" CssClass="form-control"></asp:TextBox>
                                              <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="closing_time"
                                                ErrorMessage="Closing Time is required" CssClass="text-danger" Display="Dynamic" ValidationGroup="SetSchedule"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>

                                    <!-- Booking Limit Date Close -->
                                    <div class="row mb-3">
                                        <label for="DateBook_End" class="col-sm-2 col-form-label" style="color: #ffffff;">Booking Limit Date Close</label>
                                        <div class="col-sm-10">
                                            <asp:TextBox ID="end_date" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                                             <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="end_date"
                                                ErrorMessage="Booking Limit Date is required" CssClass="text-danger" Display="Dynamic" ValidationGroup="SetSchedule"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>

                                    <!-- Set Schedule Button -->
                                    <div class="row mb-3">
                                        <label for="TextBox1" class="col-sm-2 col-form-label" style="color: #ffffff;">Schedule Booking Button</label>
                                        <div class="col-sm-10 offset-sm-2">
                                            <asp:Button ID="schedulebooking" CssClass="btn btn-primary" runat="server" Text="Set Schedule"
                                                OnClick="schedulebooking_Click" ValidationGroup="SetSchedule" />
                                        </div>
                                    </div>

                                    <!-- Buttons to Stop or Start Booking -->
                                    <div class="row mb-3">
                                        <label for="TextBox1" class="col-sm-2 col-form-label" style="color: #ffffff;">Button Start Booking</label>
                                        <div class="col-sm-10 offset-sm-2">
                                            <asp:Button ID="startbkbtn" CssClass="btn btn-success" runat="server" Text="Start Booking"
                                                OnClick="startbkbtn_Click" CausesValidation="false" />
                                        </div>
                                    </div>

                                    <div class="row mb-3">
                                        <label for="TextBox1" class="col-sm-2 col-form-label" style="color: #ffffff;">Button Stop Booking</label>
                                        <div class="col-sm-10 offset-sm-2">
                                            <asp:Button ID="stopbkbtn" CssClass="btn btn-danger" runat="server" Text="Stop Booking"
                                                OnClick="stopbkbtn_Click" CausesValidation="false" />
                                        </div>
                                    </div>

                                </form>
                            </div>
                        </div>
                    </div>

                    <div class="col-lg-6">
                        <div class="card" style="background-color: #052507; color: #ffffff; border-radius: 8px; padding: 20px; box-shadow: 0 0 5px rgba(0, 0, 0, .2)">
                            <div class="card-body">
                                <h5 class="card-title" style="color: #ffffff;">Booking Limit Output</h5>
                                <form>
                                    <div class="row mb-3">
                                        <label for="inputText" class="col-sm-2 col-form-label" style="color: #ffffff;">Booking Limit Max Day</label>
                                        <div class="col-sm-10">
                                            <asp:TextBox ID="maxdaylimit" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="row mb-3">
                                        <label for="inputText" class="col-sm-2 col-form-label" style="color: #ffffff;">Booking Limit Max per Day</label>
                                        <div class="col-sm-10">
                                            <asp:TextBox ID="maxcount" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="row mb-3">
                                        <label for="inputText" class="col-sm-2 col-form-label" style="color: #ffffff;">Booking Limit Time Close</label>
                                        <div class="col-sm-10">
                                            <asp:TextBox ID="closingtime" runat="server" CssClass="form-control" TextMode="Time" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="row mb-3">
                                        <label for="inputText" class="col-sm-2 col-form-label" style="color: #ffffff;">Booking Limit Date Last</label>
                                        <div class="col-sm-10">
                                            <asp:TextBox ID="datelast" runat="server" CssClass="form-control" TextMode="Date" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>

                                    <!-- Start Booking and Stop Booking buttons without validation -->
                                    <div class="row mb-3">
                                        <label for="inputText" class="col-sm-2 col-form-label" style="color: #ffffff;">Booking Limit Status</label>
                                        <div class="col-sm-10">
                                            <asp:TextBox ID="bkstatusstartorstop" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>


                                </form>
                            </div>
                        </div>
                    </div>
                </div>


                <%--   <section style="background-color: #052507; padding: 50px; border-radius: 8px; box-shadow: 0 0 5px rgba(0, 0, 0, .2)">
                    <div class="row">
                        <div class="col-lg-6">
                            <div class="card" style="background-color: #052507; color: #ffffff; border-radius: 8px; padding: 20px; box-shadow: 0 0 5px rgba(0, 0, 0, .2)">
                                <div class="card-body">
                                    <h5 class="card-title" style="color: #ffffff;">Set Booking Limit</h5>
                                    <form>
                                        <!-- Max Schedule for each day -->
                                        <div class="row mb-3">
                                            <label for="inputText" class="col-sm-2 col-form-label" style="color: #ffffff;">Max Schedule for Day</label>
                                            <div class="col-sm-10">
                                                <asp:TextBox CssClass="form-control" ID="max_schedule" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="max_schedule"
                                                    ErrorMessage="Max Schedule for Day is required" CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>

                                        <!-- Max Booking Count -->
                                        <div class="row mb-3">
                                            <label for="inputText" class="col-sm-2 col-form-label" style="color: #ffffff;">Max Booking Count</label>
                                            <div class="col-sm-10">
                                                <asp:TextBox CssClass="form-control" ID="max_count" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="max_count"
                                                    ErrorMessage="Max Booking Count is required" CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>

                                        <!-- Closing Time for Booking -->
                                        <div class="row mb-3">
                                            <label for="TextBox1" class="col-sm-2 col-form-label" style="color: #ffffff;">Schedule Closing Time</label>
                                            <div class="col-sm-10">
                                                <asp:TextBox ID="closing_time" runat="server" TextMode="Time" CssClass="form-control"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="closing_time"
                                                    ErrorMessage="Closing Time is required" CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>

                                        <!-- Booking Limit Date Close -->
                                        <div class="row mb-3">
                                            <label for="DateBook_End" class="col-sm-2 col-form-label" style="color: #ffffff;">Booking Limit Date Close</label>
                                            <div class="col-sm-10">
                                                <asp:TextBox ID="end_date" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="end_date"
                                                    ErrorMessage="Booking Limit Date is required" CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>

                                        <!-- Set Schedule Button -->
                                        <div class="row mb-3">
                                            <div class="col-sm-10 offset-sm-2">
                                                <asp:Button ID="schedulebooking" CssClass="btn btn-primary" runat="server" Text="Set Schedule" OnClick="schedulebooking_Click" />
                                            </div>
                                        </div>
                                    </form>
                                </div>
                            </div>
                        </div>

                        <div class="col-lg-6">
                            <div class="card" style="background-color: #052507; color: #ffffff; border-radius: 8px; padding: 20px; box-shadow: 0 0 5px rgba(0, 0, 0, .2)">
                                <div class="card-body">
                                    <h5 class="card-title" style="color: #ffffff;">Booking Limit Output</h5>
                                    <form>
                                        <div class="row mb-3">
                                            <label for="inputText" class="col-sm-2 col-form-label" style="color: #ffffff;">Booking Limit Max Day</label>
                                            <div class="col-sm-10">
                                                <asp:TextBox ID="maxdaylimit" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="row mb-3">
                                            <label for="inputText" class="col-sm-2 col-form-label" style="color: #ffffff;">Booking Limit Max per Day</label>
                                            <div class="col-sm-10">
                                                <asp:TextBox ID="maxcount" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="row mb-3">
                                            <label for="inputText" class="col-sm-2 col-form-label" style="color: #ffffff;">Booking Limit Time Close</label>
                                            <div class="col-sm-10">
                                                <asp:TextBox ID="closingtime" runat="server" CssClass="form-control" TextMode="Time" Enabled="false"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="row mb-3">
                                            <label for="inputText" class="col-sm-2 col-form-label" style="color: #ffffff;">Booking Limit Date Last</label>
                                            <div class="col-sm-10">
                                                <asp:TextBox ID="datelast" runat="server" CssClass="form-control" TextMode="Date" Enabled="false"></asp:TextBox>
                                            </div>
                                        </div>
                                    </form>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>--%>



                <%--#043002--%>
                <%--<section style="background-color: #052507; padding: 50px; border-radius: 8px; box-shadow: 0 0 5px rgba(0, 0, 0, .2)">--%>
                <%-- <div class="row">
                        <div class="col-lg-6">

                            <div class="card">
                                <div class="card-body">
                                    <h5 class="card-title">Set Booking Limit</h5>--%>

                <!--ORIGINAL SET BOOKING LIMIT General Form Elements -->

                <%-- <form>
                                        <!-- This is to input max schedule for each day -->
                                        <div class="row mb-3">
                                            <label for="inputText" class="col-sm-2 col-form-label">Max Schedule for Day</label>
                                            <div class="col-sm-10">
                                                <asp:TextBox CssClass="form-control" ID="max_schedule" runat="server"></asp:TextBox>

                                            </div>
                                        </div>

                                        <!-- This is to input the maximum booking for each day -->
                                        <div class="row mb-3">
                                            <label for="inputText" class="col-sm-2 col-form-label">Max Booking Count</label>
                                            <div class="col-sm-10">
                                                <asp:TextBox CssClass="form-control" ID="max_count" runat="server"></asp:TextBox>



                                            </div>
                                        </div>

                                        <!-- This is to input the closing time for the booking -->
                                        <div class="row mb-3">
                                            <label for="TextBox1" class="col-sm-2 col-form-label">Schedule CLosing TIme for Booking</label>
                                            <div class="col-sm-10">
                                                <asp:TextBox ID="closing_time" runat="server" TextMode="Time" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>


                                        <!-- This is to input the Last Date for booking -->
                                        <div class="row mb-3">
                                            <label for="DateBook_End" class="col-sm-2 col-form-label">Booking Limit Date Close</label>
                                            <div class="col-sm-10">
                                                <asp:TextBox ID="end_date" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>



                                        <div class="row mb-3">
                                            <label class="col-sm-2 col-form-label">Schedule Booking Limit</label>
                                            <div class="col-sm-10">
                                                <asp:Button ID="schedulebooking" CssClass=" btn btn-primary" runat="server" Text="Set Schedule" OnClick="schedulebooking_Click" />

                                            </div>
                                        </div>

                                    </form>--%>

                <!--ORIGINAL SET BOOKING LIMIT WITH REQUIRED DATA FIELD! -->
                <%--  <form>
                                        <!-- This is to input max schedule for each day -->
                                        <div class="row mb-3">
                                            <label for="inputText" class="col-sm-2 col-form-label">Max Schedule for Day</label>
                                            <div class="col-sm-10">
                                                <asp:TextBox CssClass="form-control" ID="max_schedule" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="max_schedule"
                                                    ErrorMessage="Max Schedule for Day is required" CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>

                                        <!-- This is to input the maximum booking for each day -->
                                        <div class="row mb-3">
                                            <label for="inputText" class="col-sm-2 col-form-label">Max Booking Count</label>
                                            <div class="col-sm-10">
                                                <asp:TextBox CssClass="form-control" ID="max_count" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="max_count"
                                                    ErrorMessage="Max Booking Count is required" CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>

                                        <!-- This is to input the closing time for the booking -->
                                        <div class="row mb-3">
                                            <label for="TextBox1" class="col-sm-2 col-form-label">Schedule Closing Time for Booking</label>
                                            <div class="col-sm-10">
                                                <asp:TextBox ID="closing_time" runat="server" TextMode="Time" CssClass="form-control"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="closing_time"
                                                    ErrorMessage="Closing Time is required" CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>

                                        <!-- This is to input the Last Date for booking -->
                                        <div class="row mb-3">
                                            <label for="DateBook_End" class="col-sm-2 col-form-label">Booking Limit Date Close</label>
                                            <div class="col-sm-10">
                                                <asp:TextBox ID="end_date" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="end_date"
                                                    ErrorMessage="Booking Limit Date is required" CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>

                                        <div class="row mb-3">
                                            <label class="col-sm-2 col-form-label">Schedule Booking Limit</label>
                                            <div class="col-sm-10">
                                                <asp:Button ID="schedulebooking" CssClass="btn btn-primary" runat="server" Text="Set Schedule" OnClick="schedulebooking_Click" />
                                            </div>
                                        </div>
                                    </form>--%>






                <!-- End General Form Elements -->
                <%--      </div>
                            </div>

                        </div>

                        <div class="col-lg-6">

                            <div class="card">
                                <div class="card-body">
                                    <h5 class="card-title">Booking Limit Output</h5>--%>

                <!-- Advanced Form Elements -->
                <%--  <form>

                                        <div class="row mb-3">
                                            <label for="inputText" class="col-sm-2 col-form-label">Booking Limit Max Day</label>
                                            <div class="col-sm-10">
                                                <asp:TextBox ID="maxdaylimit" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>

                                            </div>
                                        </div>

                                        <div class="row mb-3">
                                            <label for="inputText" class="col-sm-2 col-form-label">Booking Limit Max per Day</label>
                                            <div class="col-sm-10">
                                                <asp:TextBox ID="maxcount" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="row mb-3">
                                            <label for="inputText" class="col-sm-2 col-form-label">Booking Limit Time Close</label>
                                            <div class="col-sm-10">
                                                <asp:TextBox ID="closingtime" runat="server" CssClass="form-control" TextMode="Time" Enabled="false"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="row mb-3">
                                            <label for="inputText" class="col-sm-2 col-form-label">Booking Limit Date Last</label>
                                            <div class="col-sm-10">
                                                <asp:TextBox ID="datelast" runat="server" CssClass="form-control" TextMode="Date" Enabled="false"></asp:TextBox>
                                            </div>
                                        </div>


                                    </form>--%>
                <!-- End General Form Elements -->

                <%--                                </div>
                            </div>

                        </div>
                    </div>--%>
    </div>


    </section>
                <section>
                    <!-- Link Button to Trigger Modal -->
                    <asp:LinkButton ID="LinkButton1" runat="server" Text="Assign Hauler"></asp:LinkButton>

                    <!-- Script Manager -->
                    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

                    <!-- Modal Panel for Hauler Assignment -->
                    <%--<asp:Panel ID="updatePanel" CssClass="card shadow-lg scrollable-panel" runat="server"
                        Style="max-width: 600px; background-color: #052507; border: 1px solid aquamarine; border-radius: 8px;">
                        <contenttemplate>
                            <div class="card bg-light" style="background-color: #052507;">
                                <!-- Header Section -->
                                <div class="card-header text-center" style="background-color: #052507; color: aquamarine;">
                                    <h4 class="mb-0">Assign Hauler</h4>
                                </div>

                                <!-- Body Section -->
                                <div class="card-body" style="background-color: #052507;">
                                    <div class="row">

                                        <!-- Hauler Dropdown (Retrieve from Employee Table where Role = Hauler) -->
                                        <div class="col-12 mb-3">
                                            <div class="input-group input-group-sm">
                                                <div class="input-group-prepend">
                                                    <span class="input-group-text">Select Hauler</span>
                                                </div>
                                                <asp:DropDownList ID="ddlHauler" runat="server" CssClass="form-control">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <!-- Vehicle ID (Disabled) -->
                                        <div class="col-12 mb-3">
                                            <div class="input-group input-group-sm">
                                                <div class="input-group-prepend">
                                                    <span class="input-group-text">Vehicle Plate</span>
                                                </div>
                                                <asp:TextBox ID="txtbxVehiclePlate" runat="server" CssClass="form-control" ClientIDMode="Static" Enabled="false"></asp:TextBox>
                                                <asp:HiddenField ID="txtV_ID" runat="server" />
                                            </div>
                                        </div>


                                    </div>
                                </div>
                            </div>

                            <!-- Footer Section -->
                            <div class="card-footer text-center" style="background-color: #052507; color: aquamarine;">
                                <asp:Button ID="btncancel" CssClass="btn btn-secondary" runat="server" Text="Cancel" />
                                <asp:Button ID="btnAssignHauler" CssClass="btn btn-primary" runat="server" Text="Assign" OnClick="btnAssignHauler_Click" />
                            </div>
                        </contenttemplate>
                    </asp:Panel>--%>

                    <!-- Modal Popup Extender -->
                    <%--<ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server"
                        CancelControlID="btncancel" PopupControlID="updatePanel" TargetControlID="LinkButton1"
                        BackgroundCssClass="Background" DropShadow="True">
                    </ajaxToolkit:ModalPopupExtender>--%>
                </section>

    <!-- End General Form Elements -->
    </main>
            <!-- End #main -->

    <!-- ======= Footer ======= -->
    <%--<footer id="footer" class="footer" style="border-top-color: chartreuse">
        <div class="copyright" style="color: #d4f3cf">
            &copy; Copyright <strong><span style="color: #d4f3cf">TrashTrack</span></strong>. All Rights Reserved
        </div>
    </footer>--%>
    <!-- End Footer -->

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
    </div>

</form>
</html>