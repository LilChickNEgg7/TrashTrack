<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AM_Dashboard.aspx.cs" Inherits="Capstone.AM_Dashboard" %>

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
                                    <a class="dropdown-item d-flex align-items-center" href="AM_AccountSettings.aspx">
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
                        <a class="nav-link " href="AM_Dashboard.aspx">
                            <i class="bi bi-grid" style="color: aquamarine"></i>
                            <span style="color: aquamarine">Dashboard</span>
                        </a>

                    </li>
                    <!-- End Employee Nav -->


                    <li class="nav-item">
                        <a class="nav-link collapsed" data-bs-target="#forms-nav" data-bs-toggle="collapse" href="#">
                            <%--          <i class="bi bi-people" style="color:#52996F"></i><span style="color:#52996F">Manage Account</span><i class="bi bi-chevron-down ms-auto" style="color:aquamarine"></i>--%>
                            <i class="bi bi-people"></i><span>Manage Accounts</span><i class="bi bi-chevron-down ms-auto"></i>

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
                        <a class="nav-link collapsed" data-bs-target="#tables-nav" data-bs-toggle="collapse" href="#">
                            <i class="ri-bill-line"></i><span>Reports</span><i class="bi bi-chevron-down ms-auto"></i>
                        </a>
                        <ul id="tables-nav" class="nav-content collapse" data-bs-parent="#sidebar-nav">
                            <li>
                                <a href="Admin_Billing_GenerateBill.aspx">
                                    <i class="bi bi-circle"></i><span>Generate Bill</span>
                                </a>
                            </li>

                            <li>
                                <a href="Admin_Billing_Controls.aspx">
                                    <i class="bi bi-circle"></i><span>Controls</span>
                                </a>
                            </li>
                        </ul>
                    </li>


                </ul>
            </aside>
            <!-- End Sidebar-->

            <main id="main" class="main">

                <div class="pagetitle">
                    <h1 style="padding-top: 20px; color: chartreuse">Dashboard</h1>
                    <nav>
                        <ol class="breadcrumb">
                            <li class="breadcrumb-item"><a href="WAREHOUSE_DASHBOARD.aspx">Dashboard</a></li>
                            <%--<li class="breadcrumb-item">Add Item</li>--%>
                        </ol>
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
                                        <div class="card info-card sales-card" style="background-color: #052507">
                                            <div class="card-body p-3" style="background-color: #053203; border-radius: 15px">
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
                                                                <asp:Label ID="totalcustomer" runat="server" CssClass="form-control" ReadOnly="true" Style="background-color: transparent; color: aquamarine; font-size: 30px; font-weight: 800"></asp:Label>
                                                            </h6>
                                                        </div>
                                                        <small class="text-center" style="color: chartreuse;">Total</small>
                                                    </div>

                                                    <!-- Second Icon and Count for Total Customers -->
                                                    <div class="d-flex flex-column align-items-center" style="background-color: #053203">
                                                        <div class="d-flex align-items-center">
                                                            <div class="card-icon rounded-circle d-flex align-items-center justify-content-center" style="background-color: #053203">
                                                                <i class="bi bi-people" style="color: #cb3ee4;"></i>
                                                            </div>
                                                            <style>
                                                                #activecustomer {
                                                                    border: none; /* Remove border if not needed */
                                                                }
                                                            </style>
                                                            <h6 class="mb-0 ps-2">
                                                                <asp:Label ID="activecustomer" runat="server" CssClass="form-control" ReadOnly="true" Style="background-color: transparent; color: aquamarine; font-size: 30px; font-weight: 800"></asp:Label>
                                                            </h6>
                                                        </div>
                                                        <small class="text-center" style="color: chartreuse;">Active</small>
                                                    </div>

                                                    <!-- Third Icon and Count for Suspended Customers -->
                                                    <div class="d-flex flex-column align-items-center" style="background-color: #053203">
                                                        <div class="d-flex align-items-center">
                                                            <div class="card-icon rounded-circle d-flex align-items-center justify-content-center" style="background-color: #053203">
                                                                <i class="bi bi-people" style="color: #cb3ee4;"></i>
                                                            </div>
                                                            <style>
                                                                #suspcustomer {
                                                                    border: none; /* Remove border if not needed */
                                                                }
                                                            </style>
                                                            <h6 class="mb-0 ps-2">
                                                                <asp:Label ID="suspcustomer" runat="server" CssClass="form-control" ReadOnly="true" Style="background-color: transparent; color: aquamarine; font-size: 30px; font-weight: 800"></asp:Label>
                                                            </h6>
                                                        </div>
                                                        <small class="text-center" style="color: chartreuse;">Suspended</small>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>



                                    <!-- End Sales Card -->

                                    <!-- Sales Card -->
                                    <div class="col-lg-6" style="background-color: #052507">
                                        <div class="card info-card sales-card" style="background-color: #052507">
                                            <div class="card-body p-3" style="background-color: #053203; border-radius: 15px">
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
                                    </div>



                                    <!-- End Sales Card -->


                                    <!-- Customers Card -->
                                    <div class="col-lg-6" style="background-color: #052507">
                                        <div class="card info-card sales-card" style="background-color: #052507">
                                            <div class="card-body p-3" style="background-color: #053203; border-radius: 15px">
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
                                    </div>
                                    <!-- End Customers Card -->

                                    <!-- Customers Card -->
                                    <div class="col-lg-6" style="background-color: #052507">

                                        <div class="card info-card customers-card" style="background-color: #052507">

                                            <div class="card-body" style="background-color: #053203; border-radius: 15px">
                                                <h5 class="card-title" style="color: chartreuse">Billing Officer</h5>

                                                <div class="d-flex align-items-center">
                                                    <div class="d-flex flex-column align-items-center me-4" style="background-color: #053203">
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
                                                                <asp:Label ID="totalBO" runat="server" CssClass="form-control" ReadOnly="true" Style="background-color: transparent; color: aquamarine; font-size: 30px; font-weight: 800"></asp:Label>
                                                            </h6>
                                                        </div>
                                                        <small class="text-center" style="color: chartreuse;">Active</small>
                                                    </div>

                                                    <div class="d-flex flex-column align-items-center" style="background-color: #053203">
                                                        <div class="d-flex align-items-center">
                                                            <div class="card-icon rounded-circle d-flex align-items-center justify-content-center" style="background-color: #053203">
                                                                <i class="bi bi-people" style="color: #cb3ee4;"></i>
                                                            </div>
                                                            <style>
                                                                #activeBO{
                                                                    border: none; /* Remove border if not needed */
                                                                }
                                                            </style>
                                                            <h6 class="mb-0 ps-2">
                                                                <asp:Label ID="activeBO" runat="server" CssClass="form-control" ReadOnly="true" Style="background-color: transparent; color: aquamarine; font-size: 30px; font-weight: 800"></asp:Label>
                                                            </h6>
                                                        </div>
                                                        <small class="text-center" style="color: chartreuse;">Total</small>
                                                    </div>

                                                </div>
                                            </div>

                                        </div>
                                    <!-- End Customers Card -->

                                    <!-- Revenue Card -->
                                    <div class="col-lg-16" style="background-color: #052507">

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
                                    </div>

                                    <!-- End Revenue Card -->


                                    <div class="col-lg-16" style="background-color: #052507">
                                        <div class="card" style="background-color: #052507">
                                            <div class="card-body" style="background-color: #053203; border-radius: 15px">
                                                <h5 class="card-title" style="color: chartreuse">Users Chart</h5>

                                                <!-- Pie Chart -->
                                                <canvas id="pieChart" style="max-height: 400px;"></canvas>
                                                <div style="display: flex; justify-content: space-around; margin-top: 10px; text-align: left;">
                                                    <div>
                                                        <span id="activeUsersLabel" style="color: aquamarine; font-weight: 400; font-size: 13px; text-shadow: 1px 1px 2px #000; display: inline-block;">Active Users: </span>
                                                        <span id="activeUsersCount" style="color: aquamarine; font-weight: 400; font-size: 13px; text-shadow: 1px 1px 2px #000; display: inline-block;">0</span>
                                                    </div>

                                                    <div style="text-align: right;">
                                                        <span id="inactiveUsersLabel" style="color: aquamarine; font-weight: 400; font-size: 13px; text-shadow: 1px 1px 2px #000;">Inactive Users: </span>
                                                        <span id="inactiveUsersCount" style="color: aquamarine; font-weight: 400; font-size: 13px; text-shadow: 1px 1px 2px #000;">0</span>
                                                    </div>
                                                </div>

                                                <script>
                                                    function updatePieChart(activeCount, inactiveCount) {
                                                        new Chart(document.querySelector('#pieChart'), {
                                                            type: 'pie',
                                                            data: {
                                                                labels: ['Active', 'Inactive'],
                                                                datasets: [{
                                                                    label: 'User Status',
                                                                    data: [activeCount, inactiveCount],
                                                                    backgroundColor: [
                                                                        'rgb(75, 192, 192)', // Active color
                                                                        'rgb(255, 99, 132)'  // Inactive color
                                                                    ],
                                                                    borderColor: '#052507', // Border color (green)
                                                                    borderWidth: 2, // Border width
                                                                    hoverOffset: 4
                                                                }]
                                                            },
                                                            options: {
                                                                plugins: {
                                                                    legend: {
                                                                        labels: {
                                                                            color: 'white' // Text color for labels
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        });

                                                        // Update active and inactive user counts
                                                        document.getElementById('activeUsersCount').textContent = activeCount;
                                                        document.getElementById('inactiveUsersCount').textContent = inactiveCount;
                                                    }

                                                    document.addEventListener("DOMContentLoaded", () => {
                                                        // Load initial chart with placeholder data
                                                        updatePieChart(0, 0);
                                                    });
                                                </script>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>


                        <div class="col-lg-16">
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
                        </div>

                        <div class="col-lg-16">
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
                        </div>
                    </section>
                </section>




                <!-- End General Form Elements -->
            </main>
            <!-- End #main -->

            <!-- ======= Footer ======= -->
            <footer id="footer" class="footer" style="border-top-color: chartreuse">
                <div class="copyright" style="color: #d4f3cf">
                    &copy; Copyright <strong><span style="color: #d4f3cf">Pinoy Basurero Corporation</span></strong>. All Rights Reserved
                </div>
            </footer>
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
