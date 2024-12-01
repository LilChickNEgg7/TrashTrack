<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SAM_Dashboard.aspx.cs" Inherits="Capstone.Account_Manager_Dashboard" %>

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
    <style>
        .notifications {
    max-height: 431px;
    overflow-y: auto;
}

.notification-item {
    padding: 10px;
}

.notification-item.read-notification {
    background-color: #f8f9fa;
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

                        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>


                        <li class="nav-item dropdown">
                            <asp:UpdatePanel ID="UpdatePanelNotifications" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:LinkButton
                                        data-bs-toggle="dropdown"
                                        ID="LinkButton3"
                                        runat="server"
                                        OnClick="NotificationBell_Click"
                                        aria-expanded="false"
                                        CssClass="nav-link nav-icon">
                                        <i class="bi bi-bell"></i>
                                        <span id="notificationCount" runat="server" class="badge bg-primary badge-number" style="display: none;">0</span>
                                    </asp:LinkButton>
                                            <asp:Timer ID="NotificationTimer" runat="server" Interval="5000" OnTick="NotificationTimer_Tick" />

                                    <!-- Notification Dropdown -->
                                    <ul class="dropdown-menu dropdown-menu-end dropdown-menu-arrow notifications" id="notificationDropdown">
                                        <!-- Header -->
                                        <li class="dropdown-header">You have <span id="notificationHeader" runat="server">0</span> new notifications
                   
                                            <asp:LinkButton
                                                ID="lnkViewAllNotifications"
                                                runat="server"
                                                OnClick="ViewAllNotifications_Click"
                                                CssClass="badge rounded-pill bg-primary p-2 ms-2">
                        View all
                    </asp:LinkButton>
                                        </li>
                                        <li>
                                            <hr class="dropdown-divider">
                                        </li>

                                        <!-- Scrollable Repeater Container -->
                                        <div style="max-height: 305px; overflow-y: auto;">
                                            <asp:Repeater ID="NotificationRepeater" runat="server">
                                                <ItemTemplate>
                                                    <!-- Notification Item -->




                                                    <li id="notifReadHighLight" class="notification-item <%# Eval("NotifRead").ToString() == "True" ? "" : "bg-highlight" %>">
                                                        <i id="notifTypee" class="<%# GetNotificationIcon(Eval("NotifType").ToString()) %> me-2"></i>
                                                        <div>
                                                            <h4>
                                                                <div class="d-flex justify-content-between align-items-center">
                                                                    <!-- Notification Type -->
                                                                    <asp:LinkButton
                                                                        ID="LinkButton2"
                                                                        runat="server"
                                                                        CommandArgument='<%# Eval("NotifId") %>'
                                                                        OnClick="Notification_Click"
                                                                        CssClass="notification-header"
                                                                        Style="color: inherit;"
                                                                        onmouseover="this.style.color='black'; this.style.textDecoration='none';"
                                                                        onmouseout="this.style.color='inherit';">
                                                <%# Eval("NotifType") %>
                                                                    </asp:LinkButton>

                                                                    <!-- Badge for New Notifications -->
                                                                    <asp:Literal
                                                                        ID="litNewBadge"
                                                                        runat="server"
                                                                        Visible='<%# Eval("NotifRead").ToString() == "False" %>'>
                                                <span style="margin-left: 5px" class="badge bg-success text-white">New</span>
                                                                    </asp:Literal>

                                                                    <!-- Delete Button -->
                                                                    <asp:LinkButton
                                                                        ID="btnDeleteNotification"
                                                                        runat="server"
                                                                        CommandArgument='<%# Eval("NotifId") %>'
                                                                        OnClick="DeleteNotification_Click"
                                                                        CssClass="bi bi-x-circle-fill text-danger ms-auto">
                                                                    </asp:LinkButton>
                                                                </div>
                                                            </h4>
                                                            <p>
                                                                <asp:LinkButton
                                                                    ID="lnkNotification"
                                                                    runat="server"
                                                                    CommandArgument='<%# Eval("NotifId") %>'
                                                                    OnClick="Notification_Click"
                                                                    CssClass="notification-link"
                                                                    Style="color: inherit;"
                                                                    onmouseover="this.style.color='black'; this.style.textDecoration='none';"
                                                                    onmouseout="this.style.color='inherit';">
                                            <%# Eval("NotifMessage") %>
                                                                </asp:LinkButton>
                                                            </p>
                                                            <p class="notification-footer">
                                                                <span id="createdAt" class="text-muted"><%# Eval("NotifCreatedAt", "{0:yyyy-MM-dd HH:mm}") %></span>
                                                                <span id="custID" class="text-muted ms-2">Customer ID: <%# Eval("CusId") %></span>
                                                            </p>
                                                        </div>
                                                    </li>
                                                    <li>
                                                        <hr class="dropdown-divider">
                                                    </li>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </div>

                                        <!-- Footer -->
                                        <li class="dropdown-footer">
                                            <asp:LinkButton ID="btnDeleteAllNotifications" runat="server" OnClick="DeleteAllNotifications_Click" CssClass="btn btn-link">
                        Delete all notifications
                    </asp:LinkButton>
                                        </li>
                                    </ul>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="LinkButton3" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="NotificationTimer" EventName="Tick" />

                                </Triggers>
                            </asp:UpdatePanel>
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

            <!-- ======= Sidebar ======= -->
            <%--#052507--%>
            <aside style="padding-top: 50px" id="sidebar" class="sidebar">

                <ul class="sidebar-nav" id="sidebar-nav">

                    <li class="nav-item">
                        <a class="nav-link " href="SAM_Dashboard.aspx">
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
                                <a href="SAM_AccountMan.aspx">
                                    <i class="bi bi-circle"></i><span>Employees</span>
                                </a>
                            </li>
                            <li>
                                <a href="SAM_AccountManCustomers.aspx">
                                    <i class="bi bi-circle"></i><span>Customers</span>
                                </a>
                            </li>                            

                        </ul>
                    </li>

                    <li class="nav-item">
                        <a class="nav-link collapsed" href="SAM_Reports.aspx">
                            <i class="ri-secure-payment-line"></i><span>Reports</span>
                        </a>

                    </li>

                    <%--<li class="nav-item">
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
                    </li>--%>


                </ul>
            </aside>
            <!-- End Sidebar-->

            <main id="main" class="main">

                <div class="pagetitle">
                    <h1 style="padding-top: 20px; color: chartreuse">Dashboard</h1>
                    <nav>
                        <ol class="breadcrumb">
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
                                    <div class="col-lg-6" style="background-color: #052507">
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
                                                                <asp:Label ID="totalSAM" runat="server" CssClass="form-control" ReadOnly="true" Style="background-color: transparent; color: #cb3ee4; font-size: 30px; font-weight: 800"></asp:Label>
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
                                                                #activeSAM {
                                                                    border: none; /* Remove border if not needed */
                                                                }
                                                            </style>
                                                            <h6 class="mb-0 ps-2">
                                                                <asp:Label ID="activeSAM" runat="server" CssClass="form-control" ReadOnly="true" Style="background-color: transparent; color: #1187e8; font-size: 30px; font-weight: 800"></asp:Label>
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
                                                                #suspSAM {
                                                                    border: none; /* Remove border if not needed */
                                                                }
                                                            </style>
                                                            <h6 class="mb-0 ps-2">
                                                                <asp:Label ID="suspSAM" runat="server" CssClass="form-control" ReadOnly="true" Style="background-color: transparent; color: red; font-size: 30px; font-weight: 800"></asp:Label>
                                                            </h6>
                                                        </div>
                                                        <small class="text-center" style="color: darkgray;">Suspended</small>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- End Sales Card -->


                                    <!-- Customers Card -->
                                    <div class="col-lg-6" style="background-color: #052507">
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
                                                                <asp:Label ID="totalAM" runat="server" CssClass="form-control" ReadOnly="true" Style="background-color: transparent; color: #cb3ee4; font-size: 30px; font-weight: 800"></asp:Label>
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
                                                                #activeAM {
                                                                    border: none; /* Remove border if not needed */
                                                                }
                                                            </style>
                                                            <h6 class="mb-0 ps-2">
                                                                <asp:Label ID="activeAM" runat="server" CssClass="form-control" ReadOnly="true" Style="background-color: transparent; color: #1187e8; font-size: 30px; font-weight: 800"></asp:Label>
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
                                                                #suspAM {
                                                                    border: none; /* Remove border if not needed */
                                                                }
                                                            </style>
                                                            <h6 class="mb-0 ps-2">
                                                                <asp:Label ID="suspAM" runat="server" CssClass="form-control" ReadOnly="true" Style="background-color: transparent; color: red; font-size: 30px; font-weight: 800"></asp:Label>
                                                            </h6>
                                                        </div>
                                                        <small class="text-center" style="color: darkgray;">Suspended</small>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
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
                                                        <small class="text-center" style="color: darkgray;">Total</small>
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
                                                        <small class="text-center" style="color: darkgray;">Active</small>
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
                                    <div class="col-lg-6" style="background-color: #052507">
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
                                                                <asp:Label ID="totalOD" runat="server" CssClass="form-control" ReadOnly="true" Style="background-color: transparent; color: #cb3ee4; font-size: 30px; font-weight: 800"></asp:Label>
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
                                                                #activeOD {
                                                                    border: none; /* Remove border if not needed */
                                                                }
                                                            </style>
                                                            <h6 class="mb-0 ps-2">
                                                                <asp:Label ID="activeOD" runat="server" CssClass="form-control" ReadOnly="true" Style="background-color: transparent; color: #1187e8; font-size: 30px; font-weight: 800"></asp:Label>
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
                                                                #suspOD {
                                                                    border: none; /* Remove border if not needed */
                                                                }
                                                            </style>
                                                            <h6 class="mb-0 ps-2">
                                                                <asp:Label ID="suspOD" runat="server" CssClass="form-control" ReadOnly="true" Style="background-color: transparent; color: red; font-size: 30px; font-weight: 800"></asp:Label>
                                                            </h6>
                                                        </div>
                                                        <small class="text-center" style="color: darkgray;">Suspended</small>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- End OD Card -->
                                    <!-- OD Card -->
                                    <div class="col-lg-6" style="background-color: #052507">
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
                                                                <asp:Label ID="totalHauler" runat="server" CssClass="form-control" ReadOnly="true" Style="background-color: transparent; color: #cb3ee4; font-size: 30px; font-weight: 800"></asp:Label>
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
                                                                #activeHauler {
                                                                    border: none; /* Remove border if not needed */
                                                                }
                                                            </style>
                                                            <h6 class="mb-0 ps-2">
                                                                <asp:Label ID="activeHauler" runat="server" CssClass="form-control" ReadOnly="true" Style="background-color: transparent; color: #1187e8; font-size: 30px; font-weight: 800"></asp:Label>
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
                                                                #suspHauler {
                                                                    border: none; /* Remove border if not needed */
                                                                }
                                                            </style>
                                                            <h6 class="mb-0 ps-2">
                                                                <asp:Label ID="suspHauler" runat="server" CssClass="form-control" ReadOnly="true" Style="background-color: transparent; color: red; font-size: 30px; font-weight: 800"></asp:Label>
                                                            </h6>
                                                        </div>
                                                        <small class="text-center" style="color: darkgray;">Suspended</small>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
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

                                    <%--PAYMENT STATUS LATESTT--%>
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
                                                        document.getElementById('paidPaymentCount').textContent = paidAmount.toFixed(2);
                                                        document.getElementById('unpaidPaymentCount').textContent = unpaidAmount.toFixed(2);
                                                    }

                                                    // Function to fetch the payment status from the API
                                                    function fetchPaymentStatus() {
                                                        fetch('/api/payment/monthlyTotals')
                                                            .then(response => response.json())
                                                            .then(data => {
                                                                console.log('Payment Status Data:', data); // Log the API response
                                                                const totalPaid = data.monthlyTotalSalesPaid.reduce((sum, amount) => sum + amount, 0);
                                                                const totalUnpaid = data.monthlyTotalSalesUnpaid.reduce((sum, amount) => sum + amount, 0);

                                                                console.log('Total Paid:', totalPaid); // Log the total paid value
                                                                console.log('Total Unpaid:', totalUnpaid); // Log the total unpaid value

                                                                // Update chart with new values
                                                                updatePieChart(totalPaid, totalUnpaid);
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
                                                                    document.getElementById('paidCount').textContent = ` (${paidCount})`; // Update paid count
                                                                    document.getElementById('unpaidCount').textContent = ` (${unpaidCount})`; // Update unpaid count
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
                                                setInterval(fetchMonthlyTotals, 2000);
                                            });
                                        </script>
                                        <!-- End Bar Chart -->
                                    </div>
                                </div>
                            </div>
                    </section>
                </section>
                                <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>

                <script>

                    function updateNotificationCount() {
                        $.ajax({
                            type: 'GET',
                            url: '/api/payment/notificationCount',  // Your API endpoint
                            success: function (response) {
                                var count = response.unreadCount;  // The unread notification count
                                // Only update the count and avoid closing the dropdown if it's open
                                if (count > 0) {
                                    $('#notificationCount').text(count).show();
                                    $('#notificationHeader').text(count);
                                } else {
                                    $('#notificationCount').hide();
                                    $('#notificationHeader').text('0');
                                }
                            },
                            error: function () {
                                console.log('Error fetching notification count');
                            }
                        });
                    }

                    // Set interval to update the count
                    setInterval(updateNotificationCount, 100); // Run every 10 seconds instead of 100ms


                    let isDropdownOpen = false;

                    // Detect if the dropdown is open before the server refresh
                    function detectDropdownState() {
                        const dropdown = document.querySelector('#notificationDropdown');
                        isDropdownOpen = dropdown && dropdown.classList.contains('show');
                    }

                    // Reapply the open state after server refresh
                    function restoreDropdownState() {
                        const dropdown = document.querySelector('#notificationDropdown');
                        const dropdownToggle = document.querySelector('[data-bs-toggle="dropdown"]');
                        if (isDropdownOpen && dropdown && dropdownToggle) {
                            dropdown.classList.add('show');
                            dropdownToggle.setAttribute('aria-expanded', 'true');
                        }
                    }

                    // Hook into ASP.NET UpdatePanel lifecycle events
                    Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(() => detectDropdownState());
                    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(() => restoreDropdownState());

                    // Enable closing only by clicking outside
                    document.addEventListener('click', (event) => {
                        const dropdown = document.querySelector('#notificationDropdown');
                        const dropdownToggle = document.querySelector('[data-bs-toggle="dropdown"]');

                        // Only close the dropdown if it's open and clicked outside
                        if (dropdown && dropdownToggle && dropdown.classList.contains('show')) {
                            const isClickInside = dropdown.contains(event.target) || dropdownToggle.contains(event.target);

                            if (!isClickInside) {
                                dropdown.classList.remove('show');
                                dropdownToggle.setAttribute('aria-expanded', 'false');
                                isDropdownOpen = false;
                            }
                        }
                    });

                    function markAllNotificationsAsDelete() {
                        $.ajax({
                            type: 'POST',
                            url: '/api/payment/deleteAllNotifications',
                            success: function (updatedNotifications) {
                                // Update the notification UI (list of notifications)
                                updateNotificationUI(updatedNotifications);

                                // After updating the notifications, refresh the notification count
                                updateNotificationCount();
                            },
                            error: function () {
                                console.error('Failed to delete all notifications.');
                            }
                        });
                    }

                    function markNotificationAsRead(notifId) {
                        $.ajax({
                            type: 'POST',
                            url: '/api/payment/markNotificationAsRead',  // Your API endpoint
                            data: { notifId: notifId },
                            success: function () {
                                loadNotifications();  // Reload notifications after marking as read
                                updateNotificationCount();  // Update count after marking as read
                            },
                            error: function () {
                                console.log('Error marking notification as read');
                            }
                        });
                    }
                </script>


                <!-- End General Form Elements -->
            </main>
            <!-- End #main -->
            <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
                <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.9.2/dist/umd/popper.min.js"></script>
                <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>


                <script src="https://code.jquery.com/jquery-3.6.4.min.js"></script>
            <!-- ======= Footer ======= -->
            <%--<footer id="footer" class="footer" style="border-top-color: chartreuse">
                <div class="copyright" style="color: #d4f3cf">
                    &copy; Copyright <strong><span style="color: #d4f3cf">TrashTrack</span></strong>. All Rights Reserved
                </div>
            </footer>--%>
            <!-- End Footer -->
            <script src="/Scripts/jquery.signalR-{version}.js"></script>
<script src="/signalr/hubs"></script>
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
