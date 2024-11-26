<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BO_Dashboard.aspx.cs" Inherits="Capstone.BO_Dashboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8">
    <meta content="width=device-width, initial-scale=1.0" name="viewport">

    <title>Dashboard</title>
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

    <!-- Template Main CSS File -->
    <link href="assets/css/style.css" rel="stylesheet">
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
                                        <asp:Label ID="Label3" runat="server" Text=""></asp:Label></span>
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
                                    <a class="dropdown-item d-flex align-items-center" href="BO_AccountSettings.aspx">
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
                        <a class="nav-link " href="BO_Dashboard.aspx">
                            <i class="bi bi-grid" style="color: aquamarine"></i>
                            <span style="color: aquamarine">Dashboard</span>
                        </a>

                    </li>
                    <!-- End Employee Nav -->


                    <li class="nav-item">
                        <a class="nav-link collapsed" data-bs-target="#tables-nav" data-bs-toggle="collapse" href="#">
                            <i class="ri-bill-line"></i><span>Billing</span><i class="bi bi-chevron-down ms-auto"></i>
                        </a>
                        <ul id="tables-nav" class="nav-content collapse" data-bs-parent="#sidebar-nav">
                            <li>
                                <a href="BO_Billing.aspx">
                                    <i class="bi bi-circle"></i><span>Generate Bill</span>
                                </a>
                            </li>

                            <li>
                                <a href="BO_Controls.aspx">
                                    <i class="bi bi-circle"></i><span>Controls</span>
                                </a>
                            </li>
                        </ul>
                    </li>

                    <%--<li class="nav-item">
                        <a class="nav-link collapsed" data-bs-target="#receipt-nav" data-bs-toggle="collapse" href="#">
                            <i class="ri-secure-payment-line"></i><span>Payment</span><i class="bi bi-chevron-down ms-auto"></i>
                        </a>
                        <ul id="receipt-nav" class="nav-content collapse" data-bs-parent="#sidebar-nav">
                            <li>
                                <a href="BO_ViewHistory.aspx">
                                    <i class="bi bi-circle"></i><span>View Payments</span>
                                </a>
                            </li>
                        </ul>
                    </li>--%>
                    <li class="nav-item">
                        <a class="nav-link collapsed" href="BO_History.aspx">
                            <i class="ri-secure-payment-line"></i><span>Transaction History</span>
                        </a>

                    </li>


                </ul>
            </aside>
            <!-- End Sidebar-->

            <main id="main" class="main">

                <div class="pagetitle">
                    <h1 style="padding-top: 20px; color: chartreuse">Dashboard</h1>
                    <%--<nav>
                        <ol class="breadcrumb">
                            <li class="breadcrumb-item"><a href="WAREHOUSE_DASHBOARD.aspx">Dashboard</a></li>
                            <li class="breadcrumb-item">Add Item</li>
                        </ol>
                    </nav>--%>
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




                <%--#043002--%>
                <section style="background-color: #052507; padding: 50px; border-radius: 8px; box-shadow: 0 0 5px rgba(0, 0, 0, .2)">



                    <section class="section dashboard">
                        <div class="row">

                            <!-- Left side columns -->
                            <div class="col-lg-12">
                                <div class="row">

                                    <!-- Sales Card -->
                                    <%--<div class="col-lg-6" style="background-color: #052507">
                                        <div class="card info-card sales-card" style="background-color: #052507">
                                            <div class="card-body" style="background-color: #053203; border-radius: 15px">
                                                <h5 class="card-title" style="color: chartreuse">Total Customers </h5>

                                                <div class="d-flex align-items-center">
                                                    <div class="card-icon rounded-circle d-flex align-items-center justify-content-center" style="background-color: #053203">
                                                        <i class="bi bi-people" style="color: #cb3ee4;"></i>
                                                    </div>
                                                    <div class="ps-3" style="background-color: #053203">
                                                        <style>
                                                            #totalcustomer {
                                                                border: none; /* Remove border if not needed */
                                                            }
                                                        </style>
                                                        <h6>
                                                            <asp:Label ID="totalcustomer" runat="server" CssClass="form-control" ReadOnly="true" Style="background-color: transparent; color: aquamarine; font-size: 30px; font-weight: 800"></asp:Label>
                                                        </h6>
                                                    </div>
                                                    <div class="card-icon rounded-circle d-flex align-items-center justify-content-center" style="background-color: #053203">
                                                        <i class="bi bi-people" style="color: #cb3ee4;"></i>
                                                    </div>
                                                    <div class="ps-3" style="background-color: #053203">
                                                        <style>
                                                            #totalcustomer2 {
                                                                border: none; /* Remove border if not needed */
                                                            }
                                                        </style>
                                                        <h6>
                                                            <asp:Label ID="totalcustomer2" runat="server" CssClass="form-control" ReadOnly="true" Style="background-color: transparent; color: aquamarine; font-size: 30px; font-weight: 800"></asp:Label>
                                                        </h6>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                    </div>--%>
                                    <div class="col-lg-6" style="background-color: #052507">
                                        <div class="card info-card customers-card" style="background-color: #052507">
                                            <div class="card-body" style="background-color: #053203; border-radius: 15px">
                                                <!-- Adjusted padding to p-3 for smaller gap -->
                                                <h5 class="card-title" style="color: chartreuse">Customers</h5>

                                                <!-- Adjusted d-flex container for smaller gaps -->
                                                <div class="d-flex align-items-center gap-5">
                                                    <!-- Added gap-2 for smaller uniform spacing -->

                                                    <!-- First Icon and Count for Active Customers -->
                                                    <div class="d-flex flex-column align-items-center" style="background-color: #053203">
                                                        <div class="d-flex align-items-center">
                                                            <div class="card-icon rounded-circle d-flex align-items-center justify-content-center" style="background-color: #053203">
                                                                <i class="bi bi-people" style="color: #cb3ee4;"></i>
                                                            </div>
                                                            <style>
                                                                #totalcustomer {
                                                                    border: none; /* Remove border if not needed */
                                                                }
                                                            </style>
                                                            <h6 class="mb-0 ps-2">
                                                                <asp:Label ID="totalcustomer" runat="server" CssClass="form-control" ReadOnly="true" Style="background-color: transparent; color: #cb3ee4; font-size: 30px; font-weight: 800"></asp:Label>
                                                            </h6>
                                                        </div>
                                                        <small class="text-center" style="color: darkgray;">Total</small>
                                                    </div>

                                                    <!-- Second Icon and Count for Total Customers -->
                                                    <div class="d-flex flex-column align-items-center" style="background-color: #053203">
                                                        <div class="d-flex align-items-center">
                                                            <div class="card-icon rounded-circle d-flex align-items-center justify-content-center" style="background-color: #053203">
                                                                <i class="bi bi-people" style="color: #1187e8;"></i>
                                                            </div>
                                                            <style>
                                                                #activecustomer {
                                                                    border: none; /* Remove border if not needed */
                                                                }
                                                            </style>
                                                            <h6 class="mb-0 ps-2">
                                                                <asp:Label ID="activecustomer" runat="server" CssClass="form-control" ReadOnly="true" Style="background-color: transparent; color: #1187e8; font-size: 30px; font-weight: 800"></asp:Label>
                                                            </h6>
                                                        </div>
                                                        <small class="text-center" style="color: darkgray;">Active</small>
                                                    </div>

                                                    <!-- Third Icon and Count for Suspended Customers -->
                                                    <div class="d-flex flex-column align-items-center" style="background-color: #053203">
                                                        <div class="d-flex align-items-center">
                                                            <div class="card-icon rounded-circle d-flex align-items-center justify-content-center" style="background-color: #053203">
                                                                <i class="bi bi-people" style="color: red;"></i>
                                                            </div>
                                                            <style>
                                                                #suspcustomer {
                                                                    border: none; /* Remove border if not needed */
                                                                }
                                                            </style>
                                                            <h6 class="mb-0 ps-2">
                                                                <asp:Label ID="suspcustomer" runat="server" CssClass="form-control" ReadOnly="true" Style="background-color: transparent; color: red; font-size: 30px; font-weight: 800"></asp:Label>
                                                            </h6>
                                                        </div>
                                                        <small class="text-center" style="color: darkgray;">Suspended</small>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>



                                    <!-- End Sales Card -->

                                    <!-- Sales Card -->
                                    <%--<div class="col-lg-6" style="background-color: #052507">
                                        <div class="card info-card sales-card" style="background-color: #052507">
                                            <div class="card-body" style="background-color: #053203; border-radius: 15px">
                                                <!-- Adjusted padding to p-3 for smaller gap -->
                                                <h5 class="card-title" style="color: chartreuse">Super Account Managers</h5>

                                                <!-- Adjusted d-flex container for smaller gaps -->
                                                <div class="d-flex align-items-center gap-5">
                                                    <!-- Added gap-2 for smaller uniform spacing -->

                                                    <!-- First Icon and Count for Active Customers -->
                                                    <div class="d-flex flex-column align-items-center" style="background-color: #053203">
                                                        <div class="d-flex align-items-center">
                                                            <div class="card-icon rounded-circle d-flex align-items-center justify-content-center" style="background-color: #053203">
                                                                <i class="bi bi-people" style="color: #cb3ee4;"></i>
                                                            </div>
                                                            <style>
                                                                #totalSAM {
                                                                    border: none; /* Remove border if not needed */
                                                                }
                                                            </style>
                                                            <h6 class="mb-0 ps-2">
                                                                <asp:Label ID="totalSAM" runat="server" CssClass="form-control" ReadOnly="true" Style="background-color: transparent; color: aquamarine; font-size: 30px; font-weight: 800"></asp:Label>
                                                            </h6>
                                                        </div>
                                                        <small class="text-center" style="color: chartreuse;">Active</small>
                                                    </div>

                                                    <!-- Second Icon and Count for Total Customers -->
                                                    <div class="d-flex flex-column align-items-center" style="background-color: #053203">
                                                        <div class="d-flex align-items-center">
                                                            <div class="card-icon rounded-circle d-flex align-items-center justify-content-center" style="background-color: #053203">
                                                                <i class="bi bi-people" style="color: #cb3ee4;"></i>
                                                            </div>
                                                            <style>
                                                                #activeSAM {
                                                                    border: none; /* Remove border if not needed */
                                                                }
                                                            </style>
                                                            <h6 class="mb-0 ps-2">
                                                                <asp:Label ID="activeSAM" runat="server" CssClass="form-control" ReadOnly="true" Style="background-color: transparent; color: aquamarine; font-size: 30px; font-weight: 800"></asp:Label>
                                                            </h6>
                                                        </div>
                                                        <small class="text-center" style="color: chartreuse;">Total</small>
                                                    </div>

                                                    <!-- Third Icon and Count for Suspended Customers -->
                                                    <div class="d-flex flex-column align-items-center" style="background-color: #053203">
                                                        <div class="d-flex align-items-center">
                                                            <div class="card-icon rounded-circle d-flex align-items-center justify-content-center" style="background-color: #053203">
                                                                <i class="bi bi-people" style="color: #cb3ee4;"></i>
                                                            </div>
                                                            <style>
                                                                #suspSAM {
                                                                    border: none; /* Remove border if not needed */
                                                                }
                                                            </style>
                                                            <h6 class="mb-0 ps-2">
                                                                <asp:Label ID="suspSAM" runat="server" CssClass="form-control" ReadOnly="true" Style="background-color: transparent; color: aquamarine; font-size: 30px; font-weight: 800"></asp:Label>
                                                            </h6>
                                                        </div>
                                                        <small class="text-center" style="color: chartreuse;">Suspended</small>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>--%>



                                    <!-- End Sales Card -->


                                    <!-- Customers Card -->
                                    <%--<div class="col-lg-6" style="background-color: #052507">
                                        <div class="card info-card sales-card" style="background-color: #052507">
                                            <div class="card-body" style="background-color: #053203; border-radius: 15px">
                                                <!-- Adjusted padding to p-3 for smaller gap -->
                                                <h5 class="card-title" style="color: chartreuse">Account Managers</h5>

                                                <!-- Adjusted d-flex container for smaller gaps -->
                                                <div class="d-flex align-items-center gap-5">
                                                    <!-- Added gap-2 for smaller uniform spacing -->

                                                    <!-- First Icon and Count for Active Customers -->
                                                    <div class="d-flex flex-column align-items-center" style="background-color: #053203">
                                                        <div class="d-flex align-items-center">
                                                            <div class="card-icon rounded-circle d-flex align-items-center justify-content-center" style="background-color: #053203">
                                                                <i class="bi bi-people" style="color: #cb3ee4;"></i>
                                                            </div>
                                                            <style>
                                                                #totalAM {
                                                                    border: none; /* Remove border if not needed */
                                                                }
                                                            </style>
                                                            <h6 class="mb-0 ps-2">
                                                                <asp:Label ID="totalAM" runat="server" CssClass="form-control" ReadOnly="true" Style="background-color: transparent; color: aquamarine; font-size: 30px; font-weight: 800"></asp:Label>
                                                            </h6>
                                                        </div>
                                                        <small class="text-center" style="color: chartreuse;">Active</small>
                                                    </div>

                                                    <!-- Second Icon and Count for Total Customers -->
                                                    <div class="d-flex flex-column align-items-center" style="background-color: #053203">
                                                        <div class="d-flex align-items-center">
                                                            <div class="card-icon rounded-circle d-flex align-items-center justify-content-center" style="background-color: #053203">
                                                                <i class="bi bi-people" style="color: #cb3ee4;"></i>
                                                            </div>
                                                            <style>
                                                                #activeAM {
                                                                    border: none; /* Remove border if not needed */
                                                                }
                                                            </style>
                                                            <h6 class="mb-0 ps-2">
                                                                <asp:Label ID="activeAM" runat="server" CssClass="form-control" ReadOnly="true" Style="background-color: transparent; color: aquamarine; font-size: 30px; font-weight: 800"></asp:Label>
                                                            </h6>
                                                        </div>
                                                        <small class="text-center" style="color: chartreuse;">Total</small>
                                                    </div>

                                                    <!-- Third Icon and Count for Suspended Customers -->
                                                    <div class="d-flex flex-column align-items-center" style="background-color: #053203">
                                                        <div class="d-flex align-items-center">
                                                            <div class="card-icon rounded-circle d-flex align-items-center justify-content-center" style="background-color: #053203">
                                                                <i class="bi bi-people" style="color: #cb3ee4;"></i>
                                                            </div>
                                                            <style>
                                                                #suspAM {
                                                                    border: none; /* Remove border if not needed */
                                                                }
                                                            </style>
                                                            <h6 class="mb-0 ps-2">
                                                                <asp:Label ID="suspAM" runat="server" CssClass="form-control" ReadOnly="true" Style="background-color: transparent; color: aquamarine; font-size: 30px; font-weight: 800"></asp:Label>
                                                            </h6>
                                                        </div>
                                                        <small class="text-center" style="color: chartreuse;">Suspended</small>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>--%>
                                    <!-- End Customers Card -->
                                    <!-- BO Card -->
                                    <div class="col-lg-6" style="background-color: #052507">
                                        <div class="card info-card sales-card" style="background-color: #052507">
                                            <div class="card-body" style="background-color: #053203; border-radius: 15px">
                                                <!-- Adjusted padding to p-3 for smaller gap -->
                                                <h5 class="card-title" style="color: chartreuse">Billing Officer</h5>

                                                <!-- Adjusted d-flex container for smaller gaps -->
                                                <div class="d-flex align-items-center gap-5">
                                                    <!-- Added gap-2 for smaller uniform spacing -->

                                                    <!-- First Icon and Count for Active Customers -->
                                                    <div class="d-flex flex-column align-items-center" style="background-color: #053203">
                                                        <div class="d-flex align-items-center">
                                                            <div class="card-icon rounded-circle d-flex align-items-center justify-content-center" style="background-color: #053203">
                                                                <i class="bi bi-people" style="color: #cb3ee4;"></i>
                                                            </div>
                                                            <style>
                                                                #totalBO {
                                                                    border: none; /* Remove border if not needed */
                                                                }
                                                            </style>
                                                            <h6 class="mb-0 ps-2">
                                                                <asp:Label ID="totalBO" runat="server" CssClass="form-control" ReadOnly="true" Style="background-color: transparent; color: #cb3ee4; font-size: 30px; font-weight: 800"></asp:Label>
                                                            </h6>
                                                        </div>
                                                        <small class="text-center" style="color: darkgray;">Active</small>
                                                    </div>

                                                    <!-- Second Icon and Count for Total Customers -->
                                                    <div class="d-flex flex-column align-items-center" style="background-color: #053203">
                                                        <div class="d-flex align-items-center">
                                                            <div class="card-icon rounded-circle d-flex align-items-center justify-content-center" style="background-color: #053203">
                                                                <i class="bi bi-people" style="color: #1187e8;"></i>
                                                            </div>
                                                            <style>
                                                                #activeBO {
                                                                    border: none; /* Remove border if not needed */
                                                                }
                                                            </style>
                                                            <h6 class="mb-0 ps-2">
                                                                <asp:Label ID="activeBO" runat="server" CssClass="form-control" ReadOnly="true" Style="background-color: transparent; color: #1187e8; font-size: 30px; font-weight: 800"></asp:Label>
                                                            </h6>
                                                        </div>
                                                        <small class="text-center" style="color: darkgray;">Total</small>
                                                    </div>

                                                    <!-- Third Icon and Count for Suspended Customers -->
                                                    <div class="d-flex flex-column align-items-center" style="background-color: #053203">
                                                        <div class="d-flex align-items-center">
                                                            <div class="card-icon rounded-circle d-flex align-items-center justify-content-center" style="background-color: #053203">
                                                                <i class="bi bi-people" style="color: red;"></i>
                                                            </div>
                                                            <style>
                                                                #suspBO {
                                                                    border: none; /* Remove border if not needed */
                                                                }
                                                            </style>
                                                            <h6 class="mb-0 ps-2">
                                                                <asp:Label ID="suspBO" runat="server" CssClass="form-control" ReadOnly="true" Style="background-color: transparent; color: red; font-size: 30px; font-weight: 800"></asp:Label>
                                                            </h6>
                                                        </div>
                                                        <small class="text-center" style="color: darkgray;">Suspended</small>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- End BO Card -->

                                    <!-- OD Card -->
                                    <%--<div class="col-lg-6" style="background-color: #052507">
                                        <div class="card info-card sales-card" style="background-color: #052507">
                                            <div class="card-body" style="background-color: #053203; border-radius: 15px">
                                                <!-- Adjusted padding to p-3 for smaller gap -->
                                                <h5 class="card-title" style="color: chartreuse">Operational DIspatcher</h5>

                                                <!-- Adjusted d-flex container for smaller gaps -->
                                                <div class="d-flex align-items-center gap-5">
                                                    <!-- Added gap-2 for smaller uniform spacing -->

                                                    <!-- First Icon and Count for Active Customers -->
                                                    <div class="d-flex flex-column align-items-center" style="background-color: #053203">
                                                        <div class="d-flex align-items-center">
                                                            <div class="card-icon rounded-circle d-flex align-items-center justify-content-center" style="background-color: #053203">
                                                                <i class="bi bi-people" style="color: #cb3ee4;"></i>
                                                            </div>
                                                            <style>
                                                                #totalOD {
                                                                    border: none; /* Remove border if not needed */
                                                                }
                                                            </style>
                                                            <h6 class="mb-0 ps-2">
                                                                <asp:Label ID="totalOD" runat="server" CssClass="form-control" ReadOnly="true" Style="background-color: transparent; color: aquamarine; font-size: 30px; font-weight: 800"></asp:Label>
                                                            </h6>
                                                        </div>
                                                        <small class="text-center" style="color: chartreuse;">Active</small>
                                                    </div>

                                                    <!-- Second Icon and Count for Total Customers -->
                                                    <div class="d-flex flex-column align-items-center" style="background-color: #053203">
                                                        <div class="d-flex align-items-center">
                                                            <div class="card-icon rounded-circle d-flex align-items-center justify-content-center" style="background-color: #053203">
                                                                <i class="bi bi-people" style="color: #cb3ee4;"></i>
                                                            </div>
                                                            <style>
                                                                #activeOD {
                                                                    border: none; /* Remove border if not needed */
                                                                }
                                                            </style>
                                                            <h6 class="mb-0 ps-2">
                                                                <asp:Label ID="activeOD" runat="server" CssClass="form-control" ReadOnly="true" Style="background-color: transparent; color: aquamarine; font-size: 30px; font-weight: 800"></asp:Label>
                                                            </h6>
                                                        </div>
                                                        <small class="text-center" style="color: chartreuse;">Total</small>
                                                    </div>

                                                    <!-- Third Icon and Count for Suspended Customers -->
                                                    <div class="d-flex flex-column align-items-center" style="background-color: #053203">
                                                        <div class="d-flex align-items-center">
                                                            <div class="card-icon rounded-circle d-flex align-items-center justify-content-center" style="background-color: #053203">
                                                                <i class="bi bi-people" style="color: #cb3ee4;"></i>
                                                            </div>
                                                            <style>
                                                                #suspOD {
                                                                    border: none; /* Remove border if not needed */
                                                                }
                                                            </style>
                                                            <h6 class="mb-0 ps-2">
                                                                <asp:Label ID="suspOD" runat="server" CssClass="form-control" ReadOnly="true" Style="background-color: transparent; color: aquamarine; font-size: 30px; font-weight: 800"></asp:Label>
                                                            </h6>
                                                        </div>
                                                        <small class="text-center" style="color: chartreuse;">Suspended</small>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>--%>
                                    <!-- End OD Card -->
                                    <!-- OD Card -->
                                    <%--<div class="col-lg-6" style="background-color: #052507">
                                        <div class="card info-card sales-card" style="background-color: #052507">
                                            <div class="card-body" style="background-color: #053203; border-radius: 15px">
                                                <!-- Adjusted padding to p-3 for smaller gap -->
                                                <h5 class="card-title" style="color: chartreuse">Hauler</h5>

                                                <!-- Adjusted d-flex container for smaller gaps -->
                                                <div class="d-flex align-items-center gap-5">
                                                    <!-- Added gap-2 for smaller uniform spacing -->

                                                    <!-- First Icon and Count for Active Customers -->
                                                    <div class="d-flex flex-column align-items-center" style="background-color: #053203">
                                                        <div class="d-flex align-items-center">
                                                            <div class="card-icon rounded-circle d-flex align-items-center justify-content-center" style="background-color: #053203">
                                                                <i class="bi bi-people" style="color: #cb3ee4;"></i>
                                                            </div>
                                                            <style>
                                                                #totalHauler {
                                                                    border: none; /* Remove border if not needed */
                                                                }
                                                            </style>
                                                            <h6 class="mb-0 ps-2">
                                                                <asp:Label ID="totalHauler" runat="server" CssClass="form-control" ReadOnly="true" Style="background-color: transparent; color: aquamarine; font-size: 30px; font-weight: 800"></asp:Label>
                                                            </h6>
                                                        </div>
                                                        <small class="text-center" style="color: chartreuse;">Active</small>
                                                    </div>

                                                    <!-- Second Icon and Count for Total Customers -->
                                                    <div class="d-flex flex-column align-items-center" style="background-color: #053203">
                                                        <div class="d-flex align-items-center">
                                                            <div class="card-icon rounded-circle d-flex align-items-center justify-content-center" style="background-color: #053203">
                                                                <i class="bi bi-people" style="color: #cb3ee4;"></i>
                                                            </div>
                                                            <style>
                                                                #activeHauler {
                                                                    border: none; /* Remove border if not needed */
                                                                }
                                                            </style>
                                                            <h6 class="mb-0 ps-2">
                                                                <asp:Label ID="activeHauler" runat="server" CssClass="form-control" ReadOnly="true" Style="background-color: transparent; color: aquamarine; font-size: 30px; font-weight: 800"></asp:Label>
                                                            </h6>
                                                        </div>
                                                        <small class="text-center" style="color: chartreuse;">Total</small>
                                                    </div>

                                                    <!-- Third Icon and Count for Suspended Customers -->
                                                    <div class="d-flex flex-column align-items-center" style="background-color: #053203">
                                                        <div class="d-flex align-items-center">
                                                            <div class="card-icon rounded-circle d-flex align-items-center justify-content-center" style="background-color: #053203">
                                                                <i class="bi bi-people" style="color: #cb3ee4;"></i>
                                                            </div>
                                                            <style>
                                                                #suspHauler {
                                                                    border: none; /* Remove border if not needed */
                                                                }
                                                            </style>
                                                            <h6 class="mb-0 ps-2">
                                                                <asp:Label ID="suspHauler" runat="server" CssClass="form-control" ReadOnly="true" Style="background-color: transparent; color: aquamarine; font-size: 30px; font-weight: 800"></asp:Label>
                                                            </h6>
                                                        </div>
                                                        <small class="text-center" style="color: chartreuse;">Suspended</small>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>--%>
                                    <!-- End OD Card -->
                                   
                                    <!-- Revenue Card -->
                                    <%--<div class="col-lg-16" style="background-color: #052507">

                                        <div class="card info-card revenue-card" style="background-color: #052507">

                                            <div class="card-body" style="background-color: #053203; border-radius: 15px">
                                                <h5 class="card-title" style="color: chartreuse">Total Payments </h5>

                                                <div class="d-flex align-items-center justify-content-center">
                                                    <div class="card-icon rounded-circle d-flex align-items-center justify-content-center" style="background-color: #053203">
                                                        <span style="font-style: normal;">₱</span>
                                                    </div>
                                                    <div class="ps-3" style="background-color: #053203; min-width: 100px; max-width: 300px;">
                                                        <style>
                                                            #totalsales {
                                                                border: none; /* Remove border if not needed */
                                                            }
                                                        </style>
                                                        <h6>
                                                            <asp:Label ID="totalsales" runat="server" CssClass="form-control" ReadOnly="true" Style="background-color: transparent; color: aquamarine; font-size: 30px; font-weight: 800"></asp:Label>
                                                        </h6>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                    </div>--%>

                                    <!-- End Revenue Card -->


                                    <%--REALLY WORKING--%>
                                    <%--<div class="col-lg-16" style="background-color: #052507">
                                        <div class="card" style="background-color: #052507">
                                            <div class="card-body" style="background-color: #053203; border-radius: 15px">
                                                <h5 class="card-title" style="color: chartreuse">Payment Status Chart</h5>

                                                <!-- Pie Chart -->
                                                <canvas id="pieChart" style="max-height: 400px;"></canvas>
                                                <div style="display: flex; justify-content: space-around; margin-top: 10px; text-align: left;">
                                                    <div>
                                                        <span id="paidPaymentLabel" style="color: aquamarine; font-weight: 400; font-size: 13px; text-shadow: 1px 1px 2px #000;">Paid: </span>
                                                        <span id="paidPaymentCount" style="color: aquamarine; font-weight: 400; font-size: 13px; text-shadow: 1px 1px 2px #000;">0</span>
                                                    </div>

                                                    <div style="text-align: right;">
                                                        <span id="unpaidPaymentLabel" style="color: aquamarine; font-weight: 400; font-size: 13px; text-shadow: 1px 1px 2px #000;">Unpaid: </span>
                                                        <span id="unpaidPaymentCount" style="color: aquamarine; font-weight: 400; font-size: 13px; text-shadow: 1px 1px 2px #000;">0</span>
                                                    </div>
                                                </div>

                                                <script>
                                                    function updatePieChart(paidCount, unpaidCount) {
                                                        new Chart(document.querySelector('#pieChart'), {
                                                            type: 'pie',
                                                            data: {
                                                                labels: ['Paid', 'Unpaid'],
                                                                datasets: [{
                                                                    label: 'Count',
                                                                    data: [paidCount, unpaidCount],
                                                                    backgroundColor: [
                                                                        'rgb(75, 192, 192)', // Paid color
                                                                        'rgb(255, 99, 132)'  // Unpaid color
                                                                    ],
                                                                    borderColor: '#052507',
                                                                    borderWidth: 2,
                                                                    hoverOffset: 4
                                                                }]
                                                            },
                                                            options: {
                                                                plugins: {
                                                                    legend: {
                                                                        labels: {
                                                                            color: 'white' // Label color
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        });

                                                        // Update user counts in the labels
                                                        document.getElementById('paidPaymentCount').textContent = paidCount;
                                                        document.getElementById('unpaidPaymentCount').textContent = unpaidCount;
                                                    }

                                                    document.addEventListener("DOMContentLoaded", () => {
                                                        // Placeholder chart loaded on page load; actual counts are updated via C# script
                                                        updatePieChart(0, 0);
                                                    });
                                                </script>
                                            </div>
                                        </div>
                                    </div>--%>

                                    <%--<div class="col-lg-16" style="background-color: #052507">
                                        <div class="card" style="background-color: #052507">
                                            <div class="card-body" style="background-color: #053203; border-radius: 15px">
                                                <h5 class="card-title" style="color: chartreuse">Payment Status Chart</h5>

                                                <!-- Pie Chart -->
                                                <canvas id="pieChart" style="max-height: 400px;"></canvas>
                                                <div style="display: flex; justify-content: space-around; margin-top: 10px; text-align: left;">
                                                    <div>
                                                        <span id="paidPaymentLabel" style="color: aquamarine; font-weight: 400; font-size: 13px; text-shadow: 1px 1px 2px #000;">Paid: </span>
                                                        <span id="paidPaymentCount" style="color: aquamarine; font-weight: 400; font-size: 13px; text-shadow: 1px 1px 2px #000;">0</span>
                                                    </div>

                                                    <div style="text-align: right;">
                                                        <span id="unpaidPaymentLabel" style="color: aquamarine; font-weight: 400; font-size: 13px; text-shadow: 1px 1px 2px #000;">Unpaid: </span>
                                                        <span id="unpaidPaymentCount" style="color: aquamarine; font-weight: 400; font-size: 13px; text-shadow: 1px 1px 2px #000;">0</span>
                                                    </div>
                                                </div>

                                                <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
                                                <script>
                                                    let myChart;

                                                    // Function to update the pie chart with new data
                                                    function updatePieChart(paidCount, unpaidCount) {
                                                        const ctx = document.querySelector('#pieChart');

                                                        // If the chart already exists, just update it
                                                        if (myChart) {
                                                            myChart.data.datasets[0].data = [paidCount, unpaidCount];
                                                            myChart.update();
                                                        } else {
                                                            // Create a new chart
                                                            myChart = new Chart(ctx, {
                                                                type: 'pie',
                                                                data: {
                                                                    labels: ['Paid', 'Unpaid'],
                                                                    datasets: [{
                                                                        label: 'Count',
                                                                        data: [paidCount, unpaidCount],
                                                                        backgroundColor: [
                                                                            'rgb(75, 192, 192)', // Paid color
                                                                            'rgb(255, 99, 132)'  // Unpaid color
                                                                        ],
                                                                        borderColor: '#052507',
                                                                        borderWidth: 2,
                                                                        hoverOffset: 4
                                                                    }]
                                                                },
                                                                options: {
                                                                    plugins: {
                                                                        legend: {
                                                                            labels: {
                                                                                color: 'white' // Label color
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            });
                                                        }

                                                        // Update user counts in the labels
                                                        document.getElementById('paidPaymentCount').textContent = paidCount;
                                                        document.getElementById('unpaidPaymentCount').textContent = unpaidCount;
                                                    }

                                                    // Function to fetch the payment status from the API
                                                    function fetchPaymentStatus() {
                                                        fetch('/api/payment/status')
                                                            .then(response => response.json())
                                                            .then(data => {
                                                                updatePieChart(data.paidCount, data.unpaidCount);
                                                            })
                                                            .catch(error => console.error('Error fetching payment status:', error));
                                                    }

                                                    // Event listener for DOMContentLoaded to initiate the chart
                                                    document.addEventListener("DOMContentLoaded", () => {
                                                        // Initial load
                                                        fetchPaymentStatus();

                                                        // Polling every 1 second (1000 ms)
                                                        setInterval(fetchPaymentStatus, 1000);
                                                    });
                                                </script>
                                            </div>
                                        </div>
                                    </div>--%>

                                    <div class="col-lg-16" style="background-color: #052507">
                                        <div class="card" style="background-color: #052507">
                                            <div class="card-body" style="background-color: #053203; border-radius: 15px">
                                                <h5 class="card-title" style="color: chartreuse">Payment Status Chart</h5>

                                                <!-- Pie Chart -->
                                                <canvas id="pieChart" style="max-height: 400px;"></canvas>
                                                <div style="display: flex; justify-content: space-around; margin-top: 10px; text-align: left;">
                                                    <div>
                                                        <span id="paidPaymentLabel" style="color: aquamarine; font-weight: 400; font-size: 13px; text-shadow: 1px 1px 2px #000;">Paid: </span>
                                                        <span id="paidCount" style="color: aquamarine; font-weight: 400; font-size: 13px; text-shadow: 1px 1px 2px #000;">(0)</span>
                                                    </div>

                                                    <div style="text-align: right;">
                                                        <span id="unpaidPaymentLabel" style="color: aquamarine; font-weight: 400; font-size: 13px; text-shadow: 1px 1px 2px #000;">Unpaid: </span>
                                                        <span id="unpaidCount" style="color: aquamarine; font-weight: 400; font-size: 13px; text-shadow: 1px 1px 2px #000;">(0)</span>
                                                    </div>
                                                </div>

                                                <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
                                                <script>
                                                    let myChart;

                                                    // Function to fetch the payment status from the API
                                                    function fetchPaymentStatus() {
                                                        fetch('/api/payment/status')
                                                            .then(response => response.json())
                                                            .then(data => {
                                                                console.log('Payment Status Data:', data); // Log the data received from API

                                                                const paidCount = data.paidCount; // Fetch paid count
                                                                const unpaidCount = data.unpaidCount; // Fetch unpaid count

                                                                console.log('Paid Count:', paidCount); // Log the count
                                                                console.log('Unpaid Count:', unpaidCount); // Log the count

                                                                // Ensure the values are valid and update the chart/labels
                                                                if (!isNaN(paidCount) && !isNaN(unpaidCount)) {
                                                                    updatePieChart(data.totalPaidAmount, data.totalUnpaidAmount);
                                                                    document.getElementById('paidCount').textContent = ` ${paidCount}`; // Update paid count
                                                                    document.getElementById('unpaidCount').textContent = ` ${unpaidCount}`; // Update unpaid count
                                                                } else {
                                                                    console.error('Invalid data returned from the API.');
                                                                }
                                                            })
                                                            .catch(error => console.error('Error fetching payment status:', error));
                                                    }

                                                    // Function to update the pie chart with new data
                                                    function updatePieChart(paidAmount, unpaidAmount) {
                                                        const ctx = document.querySelector('#pieChart');

                                                        // If the chart already exists, just update it
                                                        if (myChart) {
                                                            myChart.data.datasets[0].data = [paidAmount, unpaidAmount];
                                                            myChart.update();
                                                        } else {
                                                            // Create a new chart
                                                            myChart = new Chart(ctx, {
                                                                type: 'pie',
                                                                data: {
                                                                    labels: ['Paid', 'Unpaid'],
                                                                    datasets: [{
                                                                        label: 'Amount',
                                                                        data: [paidAmount, unpaidAmount],
                                                                        backgroundColor: [
                                                                            'rgb(75, 192, 192)', // Paid color
                                                                                'rgb(255, 99, 132)'  // Unpaid color
                                                                            //'rgb(75, 192, 192)',
                                                                            //'rgb(255, 48, 92)'
                                                                        ],
                                                                        borderColor: '#052507',
                                                                        borderWidth: 2,
                                                                        hoverOffset: 4
                                                                    }]
                                                                },
                                                                options: {
                                                                    plugins: {
                                                                        legend: {
                                                                            labels: {
                                                                                color: 'white' // Label color
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            });
                                                        }
                                                    }

                                                    // Event listener for DOMContentLoaded to initiate the chart
                                                    document.addEventListener("DOMContentLoaded", () => {
                                                        // Initial load
                                                        fetchPaymentStatus();

                                                        // Polling every 5 seconds (5000 ms) to reduce unnecessary calls
                                                        setInterval(fetchPaymentStatus, 2000);
                                                    });
                                                </script>
                                            </div>
                                        </div>
                                    </div>








                                </div>
                            </div>



                            <%--//////////////////////////////--%>
                        <%--<div class="col-lg-16">
                            <div class="card">
                                <div class="card-body">
                                    <h5 class="card-title">Pending Payments Total for a Specific Month</h5>


                                    <!-- Bar Chart -->
                                    <canvas id="barChart" style="max-height: 400px;"></canvas>
                                    <script>
                                        document.addEventListener("DOMContentLoaded", () => {
                                            const allMonthsData = monthlyTotalSales;

                                            const chart = new Chart(document.querySelector('#barChart'), {
                                                type: 'bar',
                                                data: {
                                                    labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
                                                    datasets: [{
                                                        label: 'Bar Chart',
                                                        data: allMonthsData,
                                                        backgroundColor: [
                                                            'rgba(255, 99, 132, 0.2)',
                                                            'rgba(255, 159, 64, 0.2)',
                                                            'rgba(255, 205, 86, 0.2)',
                                                            'rgba(75, 192, 192, 0.2)',
                                                            'rgba(54, 162, 235, 0.2)',
                                                            'rgba(153, 102, 255, 0.2)',
                                                            'rgba(201, 203, 207, 0.2)'
                                                        ],
                                                        borderColor: [
                                                            'rgb(255, 99, 132)',
                                                            'rgb(255, 159, 64)',
                                                            'rgb(255, 205, 86)',
                                                            'rgb(75, 192, 192)',
                                                            'rgb(54, 162, 235)',
                                                            'rgb(153, 102, 255)',
                                                            'rgb(201, 203, 207)'
                                                        ],
                                                        borderWidth: 1
                                                    }]
                                                },
                                                options: {
                                                    scales: {
                                                        y: {
                                                            beginAtZero: true
                                                        }
                                                    }
                                                }
                                            });
                                        });

                                    </script>
                                    <!-- End Bar Chart -->

                                </div>
                            </div>
                        </div>--%>
                            <%--////////////////////////////////////////////////--%>


<%--                        <div class="col-lg-16">
                            <div class="card">
                                <div class="card-body">
                                    <h5 class="card-title">Paid Payments Total for a Specific Month</h5>

                                    <!-- Bar Chart -->
                                    <canvas id="barChartpaid" style="max-height: 400px;"></canvas>
                                    <script>
                                        document.addEventListener("DOMContentLoaded", () => {
                                            const allMonthsData = monthlyTotalSalesPaid;

                                            const chart = new Chart(document.querySelector('#barChartpaid'), {
                                                type: 'bar',
                                                data: {
                                                    labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
                                                    datasets: [{
                                                        label: 'Bar Chart',
                                                        data: allMonthsData,
                                                        backgroundColor: [
                                                            'rgba(255, 99, 132, 0.2)',
                                                            'rgba(255, 159, 64, 0.2)',
                                                            'rgba(255, 205, 86, 0.2)',
                                                            'rgba(75, 192, 192, 0.2)',
                                                            'rgba(54, 162, 235, 0.2)',
                                                            'rgba(153, 102, 255, 0.2)',
                                                            'rgba(201, 203, 207, 0.2)'
                                                        ],
                                                        borderColor: [
                                                            'rgb(255, 99, 132)',
                                                            'rgb(255, 159, 64)',
                                                            'rgb(255, 205, 86)',
                                                            'rgb(75, 192, 192)',
                                                            'rgb(54, 162, 235)',
                                                            'rgb(153, 102, 255)',
                                                            'rgb(201, 203, 207)'
                                                        ],
                                                        borderWidth: 1
                                                    }]
                                                },
                                                options: {
                                                    scales: {
                                                        y: {
                                                            beginAtZero: true
                                                        }
                                                    }
                                                }
                                            });
                                        });

                                    </script>
                                    <!-- End Bar Chart -->







                                </div>
                            </div>
                        </div>--%>


<%--                            <div class="col-lg-16">
                                <div class="card">
                                    <div class="card-body">
                                        <h5 class="card-title">Paid Payments Total for a Specific Month</h5>

                                        <!-- Bar Chart -->
                                        <canvas id="barChartpaid" style="max-height: 400px;"></canvas>
                                        <script>
                                            document.addEventListener("DOMContentLoaded", () => {
                                                const allMonthsData = monthlyTotalSalesPaid; // This should be provided from your C# code

                                                const chart = new Chart(document.querySelector('#barChartpaid'), {
                                                    type: 'bar',
                                                    data: {
                                                        labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
                                                        datasets: [{
                                                            label: 'Total Paid Payments',
                                                            data: allMonthsData,
                                                            backgroundColor: [
                                                                'rgba(255, 99, 132, 0.2)',
                                                                'rgba(255, 159, 64, 0.2)',
                                                                'rgba(255, 205, 86, 0.2)',
                                                                'rgba(75, 192, 192, 0.2)',
                                                                'rgba(54, 162, 235, 0.2)',
                                                                'rgba(153, 102, 255, 0.2)',
                                                                'rgba(201, 203, 207, 0.2)'
                                                            ],
                                                            borderColor: [
                                                                'rgb(255, 99, 132)',
                                                                'rgb(255, 159, 64)',
                                                                'rgb(255, 205, 86)',
                                                                'rgb(75, 192, 192)',
                                                                'rgb(54, 162, 235)',
                                                                'rgb(153, 102, 255)',
                                                                'rgb(201, 203, 207)'
                                                            ],
                                                            borderWidth: 1
                                                        }]
                                                    },
                                                    options: {
                                                        scales: {
                                                            y: {
                                                                beginAtZero: true
                                                            }
                                                        }
                                                    }
                                                });
                                            });
                                        </script>
                                        <!-- End Bar Chart -->
                                    </div>
                                </div>
                            </div>--%>

                            <%--<div class="col-lg-16">
                                <div class="card">
                                    <div class="card-body">
                                        <h5 class="card-title">Paid Payments Total for a Specific Month</h5>

                                        <!-- Bar Chart -->
                                        <canvas id="barChartpaid" style="max-height: 400px;"></canvas>
                                        <script>
                                            document.addEventListener("DOMContentLoaded", () => {
                                                const allMonthsData = monthlyTotalSalesPaid; // This should be provided from your C# code

                                                const chart = new Chart(document.querySelector('#barChartpaid'), {
                                                    type: 'bar',
                                                    data: {
                                                        labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
                                                        datasets: [{
                                                            label: 'Total Paid Payments',
                                                            data: allMonthsData,
                                                            backgroundColor: [
                                                                'rgba(255, 99, 132, 0.2)',
                                                                'rgba(255, 159, 64, 0.2)',
                                                                'rgba(255, 205, 86, 0.2)',
                                                                'rgba(75, 192, 192, 0.2)',
                                                                'rgba(54, 162, 235, 0.2)',
                                                                'rgba(153, 102, 255, 0.2)',
                                                                'rgba(201, 203, 207, 0.2)'
                                                            ],
                                                            borderColor: [
                                                                'rgb(255, 99, 132)',
                                                                'rgb(255, 159, 64)',
                                                                'rgb(255, 205, 86)',
                                                                'rgb(75, 192, 192)',
                                                                'rgb(54, 162, 235)',
                                                                'rgb(153, 102, 255)',
                                                                'rgb(201, 203, 207)'
                                                            ],
                                                            borderWidth: 1
                                                        }]
                                                    },
                                                    options: {
                                                        scales: {
                                                            y: {
                                                                beginAtZero: true
                                                            }
                                                        }
                                                    }
                                                });
                                            });
                                        </script>
                                        <!-- End Bar Chart -->
                                    </div>
                                </div>
                            </div>--%>

                            <%--<div class="col-lg-16">
    <div class="card">
        <div class="card-body">
            <h5 class="card-title">Paid Payments Total for Each Month</h5>

            <!-- Bar Chart -->
            <canvas id="barChartpaid" style="max-height: 400px;"></canvas>
            <script>
                document.addEventListener("DOMContentLoaded", () => {
                    const allMonthsData = monthlyTotalSalesPaid; // This should be provided from your C# code

                    const labels = [
                        'January', 'February', 'March', 'April', 'May',
                        'June', 'July', 'August', 'September', 'October',
                        'November', 'December'
                    ];

                    const backgroundColors = [
                        'rgba(255, 99, 132, 0.2)', // January
                        'rgba(255, 159, 64, 0.2)', // February
                        'rgba(255, 205, 86, 0.2)', // March
                        'rgba(75, 192, 192, 0.2)', // April
                        'rgba(54, 162, 235, 0.2)', // May
                        'rgba(153, 102, 255, 0.2)', // June
                        'rgba(201, 203, 207, 0.2)', // July
                        'rgba(255, 99, 132, 0.2)', // August
                        'rgba(255, 159, 64, 0.2)', // September
                        'rgba(255, 205, 86, 0.2)', // October
                        'rgba(75, 192, 192, 0.2)', // November
                        'rgba(54, 162, 235, 0.2)'  // December
                    ];

                    const borderColors = [
                        'rgb(255, 99, 132)', // January
                        'rgb(255, 159, 64)', // February
                        'rgb(255, 205, 86)', // March
                        'rgb(75, 192, 192)', // April
                        'rgb(54, 162, 235)', // May
                        'rgb(153, 102, 255)', // June
                        'rgb(201, 203, 207)', // July
                        'rgb(255, 99, 132)', // August
                        'rgb(255, 159, 64)', // September
                        'rgb(255, 205, 86)', // October
                        'rgb(75, 192, 192)', // November
                        'rgb(54, 162, 235)'  // December
                    ];

                    const datasets = allMonthsData.map((data, index) => ({
                        label: labels[index], // Unique label for each month
                        data: [data], // Single data point for each month
                        backgroundColor: backgroundColors[index], // Unique background color for each month
                        borderColor: borderColors[index], // Unique border color for each month
                        borderWidth: 1
                    }));

                    const chart = new Chart(document.querySelector('#barChartpaid'), {
                        type: 'bar',
                        data: {
                            labels: labels,
                            datasets: datasets
                        },
                        options: {
                            scales: {
                                y: {
                                    beginAtZero: true
                                }
                            }
                        }
                    });
                });
            </script>
            <!-- End Bar Chart -->
        </div>
    </div>
</div>--%>


                            <%--<div class="col-lg-16">
                                <div class="card">
                                    <div class="card-body">
                                        <h5 class="card-title">Paid Payments Total for Each Month</h5>

                                        <!-- Bar Chart -->
                                        <canvas id="barChartpaid" style="max-height: 400px;"></canvas>
                                        <script>
                                            document.addEventListener("DOMContentLoaded", () => {
                                                const allMonthsData = monthlyTotalSalesPaid; // This should be provided from your C# code

                                                const labels = [
                                                    'Unpaid', 'Paid'
                                                ];

                                                const backgroundColors = [
                                                    'rgba(255, 99, 132, 0.2)', // January
                                                    'rgba(255, 159, 64, 0.2)', // February
                                                    'rgba(255, 205, 86, 0.2)', // March
                                                    'rgba(75, 192, 192, 0.2)', // April
                                                    'rgba(54, 162, 235, 0.2)', // May
                                                    'rgba(153, 102, 255, 0.2)', // June
                                                    'rgba(201, 203, 207, 0.2)', // July
                                                    'rgba(255, 99, 132, 0.2)', // August
                                                    'rgba(255, 159, 64, 0.2)', // September
                                                    'rgba(255, 205, 86, 0.2)', // October
                                                    'rgba(75, 192, 192, 0.2)', // November
                                                    'rgba(54, 162, 235, 0.2)'  // December
                                                ];

                                                const borderColors = [
                                                    'rgb(255, 99, 132)', // January
                                                    'rgb(255, 159, 64)', // February
                                                    'rgb(255, 205, 86)', // March
                                                    'rgb(75, 192, 192)', // April
                                                    'rgb(54, 162, 235)', // May
                                                    'rgb(153, 102, 255)', // June
                                                    'rgb(201, 203, 207)', // July
                                                    'rgb(255, 99, 132)', // August
                                                    'rgb(255, 159, 64)', // September
                                                    'rgb(255, 205, 86)', // October
                                                    'rgb(75, 192, 192)', // November
                                                    'rgb(54, 162, 235)'  // December
                                                ];

                                                const datasets = allMonthsData.map((data, index) => ({
                                                    label: labels[index], // Unique label for each month
                                                    data: [data], // Single data point for each month
                                                    backgroundColor: backgroundColors[index], // Unique background color for each month
                                                    borderColor: borderColors[index], // Unique border color for each month
                                                    borderWidth: 1
                                                }));

                                                const chart = new Chart(document.querySelector('#barChartpaid'), {
                                                    type: 'bar',
                                                    data: {
                                                        labels: labels,
                                                        datasets: datasets
                                                    },
                                                    options: {
                                                        scales: {
                                                            y: {
                                                                beginAtZero: true
                                                            }
                                                        }
                                                    }
                                                });
                                            });
                                        </script>
                                        <!-- End Bar Chart -->
                                    </div>
                                </div>
                            </div>--%>



<%--<div class="col-lg-16">
    <div class="card">
        <div class="card-body">
            <h5 class="card-title">Payments Total for Each Month</h5>

            <!-- Bar Chart -->
            <canvas id="barChartPayments" style="max-height: 400px;"></canvas>
            <script>
                document.addEventListener("DOMContentLoaded", () => {
                    const allMonthsDataPaid = monthlyTotalSalesPaid; // Data for paid payments
                    const allMonthsDataUnpaid = monthlyTotalSalesUnpaid; // Data for unpaid payments

                    const labels = [
                        'January', 'February', 'March', 'April', 'May', 'June',
                        'July', 'August', 'September', 'October', 'November', 'December'
                    ];

                    // Define colors for the datasets
                    const backgroundColors = [
                        'rgba(75, 192, 192, 0.2)', // Color for Paid
                        'rgba(255, 99, 132, 0.2)'  // Color for Unpaid
                    ];

                    const borderColors = [
                        'rgb(75, 192, 192)', // Border color for Paid
                        'rgb(255, 99, 132)'  // Border color for Unpaid
                    ];

                    const datasets = [
                        {
                            label: 'Total Paid Payments',
                            data: allMonthsDataPaid, // Monthly data for paid payments
                            backgroundColor: backgroundColors[0],
                            borderColor: borderColors[0],
                            borderWidth: 1
                        },
                        {
                            label: 'Total Unpaid Payments',
                            data: allMonthsDataUnpaid, // Monthly data for unpaid payments
                            backgroundColor: backgroundColors[1],
                            borderColor: borderColors[1],
                            borderWidth: 1
                        }
                    ];

                    const chart = new Chart(document.querySelector('#barChartPayments'), {
                        type: 'bar',
                        data: {
                            labels: labels,
                            datasets: datasets
                        },
                        options: {
                            scales: {
                                y: {
                                    beginAtZero: true
                                }
                            }
                        }
                    });
                });
            </script>
            <!-- End Bar Chart -->
        </div>
    </div>
</div>--%>

                            <%--<div class="col-lg-16">
    <div class="card">
        <div class="card-body">
            <h5 class="card-title">Payments Total for Each Month</h5>

            <!-- Bar Chart -->
            <canvas id="barChartPayments" style="max-height: 400px;"></canvas>
            <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
            <script>
                let myBarChart; // Declare variable to hold the chart instance

                // Function to update the bar chart with new data
                function updateBarChart(monthlyTotalSalesPaid, monthlyTotalSalesUnpaid) {
                    const labels = [
                        'January', 'February', 'March', 'April', 'May', 'June',
                        'July', 'August', 'September', 'October', 'November', 'December'
                    ];

                    // Define colors for the datasets
                    const backgroundColors = [
                        'rgba(75, 192, 192, 0.2)', // Color for Paid
                        'rgba(255, 99, 132, 0.2)'  // Color for Unpaid
                    ];

                    const borderColors = [
                        'rgb(75, 192, 192)', // Border color for Paid
                        'rgb(255, 99, 132)'  // Border color for Unpaid
                    ];

                    const datasets = [
                        {
                            label: 'Total Paid Payments',
                            data: monthlyTotalSalesPaid,
                            backgroundColor: backgroundColors[0],
                            borderColor: borderColors[0],
                            borderWidth: 1
                        },
                        {
                            label: 'Total Unpaid Payments',
                            data: monthlyTotalSalesUnpaid,
                            backgroundColor: backgroundColors[1],
                            borderColor: borderColors[1],
                            borderWidth: 1
                        }
                    ];

                    // If the chart already exists, update it
                    if (myBarChart) {
                        myBarChart.data.datasets = datasets;
                        myBarChart.update();
                    } else {
                        // Create a new chart
                        myBarChart = new Chart(document.querySelector('#barChartPayments'), {
                            type: 'bar',
                            data: {
                                labels: labels,
                                datasets: datasets
                            },
                            options: {
                                scales: {
                                    y: {
                                        beginAtZero: true
                                    }
                                }
                            }
                        });
                    }
                }

                // Function to fetch monthly totals from the API
                function fetchMonthlyTotals() {
                    fetch('/api/payment/monthlyTotals')
                        .then(response => response.json())
                        .then(data => {
                            updateBarChart(data.monthlyTotalSalesPaid, data.monthlyTotalSalesUnpaid);
                        })
                        .catch(error => console.error('Error fetching monthly totals:', error));
                }

                // Event listener for DOMContentLoaded to initiate the chart
                document.addEventListener("DOMContentLoaded", () => {
                    // Initial load
                    fetchMonthlyTotals();

                    // Polling every 5 seconds (5000 ms)
                    setInterval(fetchMonthlyTotals, 1000);
                });
            </script>
            <!-- End Bar Chart -->
        </div>
    </div>
</div>--%>


                            <%--LATEST 5--%>
                            <%--<div class="col-lg-16">
                                <div class="card" style="background-color: #053203">
                                    <div class="card-body">
                                        <h5 class="card-title" style="color: chartreuse">Payments Total for Each Month</h5>

                                        <!-- Bar Chart -->
                                        <canvas id="barChartPayments" style="max-height: 400px;"></canvas>
                                        <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
                                        <script>
                                            let myBarChart; // Variable to hold the chart instance
                                            let lastMonthlyTotalsPaid = []; // To store last known monthly totals paid
                                            let lastMonthlyTotalsUnpaid = []; // To store last known monthly totals unpaid

                                            // Function to update the bar chart with new data
                                            function updateBarChart(monthlyTotalSalesPaid, monthlyTotalSalesUnpaid) {
                                                const labels = [
                                                    'January', 'February', 'March', 'April', 'May', 'June',
                                                    'July', 'August', 'September', 'October', 'November', 'December'
                                                ];

                                                const backgroundColors = [
                                                    'rgba(75, 192, 192, 0.2)', // Color for Paid
                                                    'rgba(255, 99, 132, 0.2)'  // Color for Unpaid
                                                ];

                                                const borderColors = [
                                                    'rgb(75, 192, 192)', // Border color for Paid
                                                    'rgb(255, 99, 132)'  // Border color for Unpaid
                                                ];

                                                const datasets = [
                                                    {
                                                        label: 'Total Paid Payments',
                                                        data: monthlyTotalSalesPaid,
                                                        backgroundColor: backgroundColors[0],
                                                        borderColor: borderColors[0],
                                                        borderWidth: 1
                                                    },
                                                    {
                                                        label: 'Total Unpaid Payments',
                                                        data: monthlyTotalSalesUnpaid,
                                                        backgroundColor: backgroundColors[1],
                                                        borderColor: borderColors[1],
                                                        borderWidth: 1
                                                    }
                                                ];

                                                // Update the chart only if the data has changed
                                                if (JSON.stringify(lastMonthlyTotalsPaid) !== JSON.stringify(monthlyTotalSalesPaid) ||
                                                    JSON.stringify(lastMonthlyTotalsUnpaid) !== JSON.stringify(monthlyTotalSalesUnpaid)) {

                                                    if (myBarChart) {
                                                        myBarChart.data.datasets = datasets;
                                                        myBarChart.update();
                                                    } else {
                                                        myBarChart = new Chart(document.querySelector('#barChartPayments'), {
                                                            type: 'bar',
                                                            data: {
                                                                labels: labels,
                                                                datasets: datasets
                                                            },
                                                            options: {
                                                                scales: {
                                                                    y: {
                                                                        beginAtZero: true
                                                                    }
                                                                }
                                                            }
                                                        });
                                                    }

                                                    // Store the last totals
                                                    lastMonthlyTotalsPaid = monthlyTotalSalesPaid;
                                                    lastMonthlyTotalsUnpaid = monthlyTotalSalesUnpaid;
                                                }
                                            }

                                            // Function to fetch monthly totals from the API
                                            function fetchMonthlyTotals() {
                                                fetch('/api/payment/monthlyTotals')
                                                    .then(response => response.json())
                                                    .then(data => {
                                                        updateBarChart(data.monthlyTotalSalesPaid, data.monthlyTotalSalesUnpaid);
                                                    })
                                                    .catch(error => console.error('Error fetching monthly totals:', error));
                                            }

                                            // Event listener for DOMContentLoaded to initiate the chart
                                            document.addEventListener("DOMContentLoaded", () => {
                                                // Initial load
                                                fetchMonthlyTotals();

                                                // Polling every 5 seconds (5000 ms)
                                                setInterval(fetchMonthlyTotals, 5000);
                                            });
                                        </script>
                                        <!-- End Bar Chart -->
                                    </div>
                                </div>
                            </div>--%>

                            <div class="col-lg-16">
                                <div class="card" style="background-color: #053203">
                                    <div class="card-body" style="border-radius: 15px">
                                        <h5 class="card-title" style="color: chartreuse">Payments for Each Month</h5>

                                        <!-- Bar Chart -->
                                        <canvas id="barChartPayments" style="max-height: 400px;"></canvas>
                                        <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
                                        <script>
                                            let myBarChart;
                                            let lastMonthlyTotalsPaid = [];
                                            let lastMonthlyTotalsUnpaid = [];

                                            function updateBarChart(monthlyTotalSalesPaid, monthlyTotalSalesUnpaid) {
                                                const labels = [
                                                    'January', 'February', 'March', 'April', 'May', 'June',
                                                    'July', 'August', 'September', 'October', 'November', 'December'
                                                ];

                                                const backgroundColors = [
                                                    'rgba(54, 162, 235, 0.2)', // Blue for Paid
                                                    'rgba(255, 99, 132, 0.2)'  // Red for Unpaid
                                                ];

                                                const borderColors = [
                                                    'rgb(54, 162, 235)', // Blue for Paid
                                                    'rgb(255, 99, 132)'  // Red for Unpaid
                                                ];

                                                const datasets = [
                                                    {
                                                        label: 'Total Paid Payments',
                                                        data: monthlyTotalSalesPaid,
                                                        backgroundColor: backgroundColors[0],
                                                        borderColor: borderColors[0],
                                                        borderWidth: 1
                                                    },
                                                    {
                                                        label: 'Total Unpaid Payments',
                                                        data: monthlyTotalSalesUnpaid,
                                                        backgroundColor: backgroundColors[1],
                                                        borderColor: borderColors[1],
                                                        borderWidth: 1
                                                    }
                                                ];

                                                if (JSON.stringify(lastMonthlyTotalsPaid) !== JSON.stringify(monthlyTotalSalesPaid) ||
                                                    JSON.stringify(lastMonthlyTotalsUnpaid) !== JSON.stringify(monthlyTotalSalesUnpaid)) {

                                                    if (myBarChart) {
                                                        myBarChart.data.datasets = datasets;
                                                        myBarChart.update();
                                                    } else {
                                                        myBarChart = new Chart(document.querySelector('#barChartPayments'), {
                                                            type: 'bar',
                                                            data: {
                                                                labels: labels,
                                                                datasets: datasets
                                                            },
                                                            options: {
                                                                scales: {
                                                                    x: {
                                                                        ticks: {
                                                                            color: 'white' // Set color of x-axis labels (months)
                                                                        },
                                                                        grid: {
                                                                            color: 'rgba(128, 128, 128, 0.2)' // Set color of x-axis grid lines to gray
                                                                        }
                                                                    },
                                                                    y: {
                                                                        beginAtZero: true,
                                                                        ticks: {
                                                                            color: 'white' // Set color of y-axis numbers
                                                                        },
                                                                        grid: {
                                                                            color: 'rgba(128, 128, 128, 0.2)' // Set color of y-axis grid lines to gray
                                                                        }
                                                                    }
                                                                },
                                                                plugins: {
                                                                    legend: {
                                                                        labels: {
                                                                            color: 'white' // Set color of legend text
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        });
                                                    }

                                                    lastMonthlyTotalsPaid = monthlyTotalSalesPaid;
                                                    lastMonthlyTotalsUnpaid = monthlyTotalSalesUnpaid;
                                                }
                                            }

                                            function fetchMonthlyTotals() {
                                                fetch('/api/payment/monthlyTotals')
                                                    .then(response => response.json())
                                                    .then(data => {
                                                        updateBarChart(data.monthlyTotalSalesPaid, data.monthlyTotalSalesUnpaid);
                                                    })
                                                    .catch(error => console.error('Error fetching monthly totals:', error));
                                            }

                                            document.addEventListener("DOMContentLoaded", () => {
                                                fetchMonthlyTotals();
                                                setInterval(fetchMonthlyTotals, 5000);
                                            });
                                        </script>
                                        <!-- End Bar Chart -->
                                    </div>
                                </div>
                            </div>









                            <%--                            <div class="col-lg-16">
                                <div class="card">
                                    <div class="card-body">
                                        <h5 class="card-title">Payments Total for Each Month</h5>

                                        <!-- Bar Chart -->
                                        <canvas id="barChartPayments" style="max-height: 400px;"></canvas>
                                        <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
                                        <script>
                                            let barChart;
                                            const connection = $.hubConnection(); // Create a connection
                                            const paymentHub = connection.createHubProxy('PaymentHub'); // Create a hub proxy

                                            // Handle incoming messages from SignalR
                                            paymentHub.on('updateChart', function (data) {
                                                const parsedData = JSON.parse(data);
                                                updateBarChart(parsedData.monthlyTotalSalesPaid, parsedData.monthlyTotalSalesUnpaid);
                                            });

                                            // Start the connection
                                            connection.start().done(function () {
                                                console.log("SignalR connection established.");
                                            }).fail(function (error) {
                                                console.log("SignalR connection error: ", error);
                                            });

                                            function updateBarChart(monthlyTotalSalesPaid, monthlyTotalSalesUnpaid) {
                                                const labels = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];

                                                const datasets = [
                                                    {
                                                        label: 'Total Paid Payments',
                                                        data: monthlyTotalSalesPaid,
                                                        backgroundColor: 'rgba(75, 192, 192, 0.2)',
                                                        borderColor: 'rgb(75, 192, 192)',
                                                        borderWidth: 1
                                                    },
                                                    {
                                                        label: 'Total Unpaid Payments',
                                                        data: monthlyTotalSalesUnpaid,
                                                        backgroundColor: 'rgba(255, 99, 132, 0.2)',
                                                        borderColor: 'rgb(255, 99, 132)',
                                                        borderWidth: 1
                                                    }
                                                ];

                                                if (barChart) {
                                                    // Update existing chart data and refresh it
                                                    barChart.data.datasets = datasets;
                                                    barChart.update();
                                                } else {
                                                    // Create a new chart if it doesn’t exist
                                                    barChart = new Chart(document.getElementById("barChartPayments"), {
                                                        type: 'bar',
                                                        data: { labels: labels, datasets: datasets },
                                                        options: { scales: { y: { beginAtZero: true } } }
                                                    });
                                                }
                                            }
                                        </script>


                                        <!-- End Bar Chart -->
                                    </div>
                                </div>
                            </div>--%>





                    </section>
                </section>

                <script>
                    //$(function () {
                    //    var hub = $.connection.paymentHub;

                    //    hub.client.updateChart = function (data) {
                    //        var parsedData = JSON.parse(data);
                    //        updateBarChart(parsedData.monthlyTotalSalesPaid, parsedData.monthlyTotalSalesUnpaid);
                    //    };

                    //    $.connection.hub.start().done(function () {
                    //        console.log("Connected to the PaymentHub!");
                    //    }).fail(function (error) {
                    //        console.log("Could not connect to the PaymentHub: " + error);
                    //    });
                    //});

                </script>


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
            <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
            <%--websocket hub--%>
            <%--<script src="https://ajax.aspnetcdn.com/ajax/jquery/jquery-3.5.1.min.js"></script>
            <script src="https://ajax.aspnetcdn.com/ajax/signalr/jquery.signalr-2.4.2.min.js"></script>--%>
            <%--<script src="https://<your-domain>/signalr/hubs"></script>--%>

<%--            <script src="https://ajax.aspnetcdn.com/ajax/signalr/jquery.signalr-2.4.1.min.js"></script>
<script src="//signalr/hubs"></script>--%>


            <%--websocket--%>
            <script src="https://cdnjs.cloudflare.com/ajax/libs/microsoft-signalr/3.1.11/signalr.min.js"></script>

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
