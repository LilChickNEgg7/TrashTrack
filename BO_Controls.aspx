<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BO_Controls.aspx.cs" Inherits="Capstone.BO_Controls" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8">
    <meta content="width=device-width, initial-scale=1.0" name="viewport">

    <title>PBC - Dashboard</title>
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
    <link href="assets/vendor/simple-datatables/style.css" rel="stylesheet">
    <script src="https://cdn.jsdelivr.net/npm/apexcharts"></script>

    <!-- Template Main CSS File -->
    <link href="assets/css/style.css" rel="stylesheet">
    <%--#052507--%>
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



        /*.notification-link {
            color: black;*/ /* Initial color */
            /*text-decoration: none;*/ /* Optional: remove underline */
        /*}

            .notification-link:hover {
                color: gray;*/ /* Change to gray on hover */
                /*cursor: pointer;*/ /* Show pointer cursor */
            /*}

        .notification-item.read-notification {
            background-color: #f0f0f0;*/ /* Lighter background for read notifications */
            /*color: #999;*/ /* Dimmed text color */
        /*}

        .dropdown-menu.notifications {
            max-height: 300px;
            overflow-y: auto;
        }

        .read-notification {
            background-color: #f0f0f0;*/ /* Example color for read notifications */
            /*color: #999;*/ /* Optional: Change text color for read notifications */
        /*}*/


        /*Panel scrollable height*/
        /*.scrollable-panel {
    max-height: 95vh;*/ /* Adjust this value as needed */
        /*overflow-y: auto;*/ /* Enables vertical scrolling */
        /*}*/
        .scrollable-panel {
            max-height: 680px; /* Adjust the height as needed */
            overflow-y: auto; /* Enables vertical scrolling */
            overflow-x: hidden; /* Hides horizontal scrolling */
            border-radius: 8px; /* Rounded corners */
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1); /* Subtle shadow for depth */
            padding: 20px; /* Padding inside the panel */
            background-color: #f9f9f9; /* Light background color */
            border: 1px solid #ddd; /* Light border */
            box-sizing: border-box; /* Includes padding and border in element's total width and height */
        }

            .scrollable-panel::-webkit-scrollbar {
                width: 12px; /* Increase width to make scrollbar less intrusive */
            }

            .scrollable-panel::-webkit-scrollbar-thumb {
                background-color: #025539; /* Color of the scrollbar thumb */
                border-radius: 6px; /* Rounded scrollbar thumb */
            }

                .scrollable-panel::-webkit-scrollbar-thumb:hover {
                    background-color: #555; /* Darker color on hover */
                }

            .scrollable-panel::-webkit-scrollbar-track {
                background-color: #f1f1f1; /* Color of the scrollbar track */
                border-radius: 6px; /* Rounded scrollbar track */
            }




        /*start of tabs*/
        /* Main Container */
        #myTab {
            background: #052507;
            Green background for the tab container padding: 20px;
            border-radius: 10px 10px 0 0;
            Rounded corners for the top left and right margin-bottom: 0;
            Ensures no spacing or white line at the bottom
        }

        /*Tab Buttons */
        .nav-tabs {
            border-bottom: none; /* Remove any default bottom border */
        }

            .nav-tabs .nav-item .nav-link {
                background: #2f5f3e; /* Dark green for inactive tabs */
                color: white;
                font-weight: bold;
                margin-right: 10px;
                border-radius: 10px 10px 0 0; /* Keep tabs rounded at the top */
                padding: 12px 24px;
                transition: all 0.3s ease-in-out;
                border: none; /* Remove default border */
            }

                .nav-tabs .nav-item .nav-link:hover {
                    background: #5fa17b; /* Luminous green for hover state */
                    color: white;
                }

                .nav-tabs .nav-item .nav-link.active {
                    background: #5fa17b; /* Slightly darker green for active state */
                    color: #eaf7e6; /* Light green text */
                    border-bottom: none; /* Remove any bottom border */
                }

        /* Styling the GridView Container */
        .gridview-container {
            padding: 20px;
            border: 1px solid #ddd; /* Soft border around the grid */
            border-top: none; /* Remove border between tabs and gridview */
            border-radius: 0 0 12px 12px; /* Rounded bottom corners */
            background-color: #f9f9f9; /* Light background color */
            margin-top: -1px; /* Seamless connection between tabs and content */
            overflow: hidden; /* Prevent overflow and maintain rounded corners */
        }

            /* GridView Header Styles */
            .gridview-container .gridview-header {
                background: #66CDAA; /* Luminous green for header */
                color: white;
                font-size: 16px; /* Slightly smaller font for a refined look */
                font-weight: 600; /* Medium weight for balanced appearance */
                text-align: center;
                padding: 12px;
                border-radius: 12px 12px 0 0; /* Rounded top corners */
            }

            /* GridView Row Styles */
            .gridview-container .gridview-row {
                border-bottom: 1px solid #ddd; /* Light row separators */
                padding: 12px;
                font-size: 14px; /* Smaller font size */
                font-weight: 500; /* Medium weight for row text */
                color: #333; /* Darker text color */
                word-wrap: break-word; /* Text wraps neatly */
            }

                /* Hover Effect for Grid Rows */
                .gridview-container .gridview-row:hover {
                    background: #eef8ee; /* Subtle hover effect */
                }

            /* GridView Text Styles */
            .gridview-container .gridview-row,
            .gridview-container .gridview-header {
                font-family: 'Arial', sans-serif; /* Clean, formal font */
                line-height: 1.5; /* Comfortable line height */
            }

            /* Optional: Adding Footer Rounded Corners */
            .gridview-container .gridview-footer {
                border-radius: 0 0 12px 12px; /* Ensure footer has rounded corners */
            }

            /* Additional Styling for Buttons and Labels */
            .gridview-container .gridview-row .btnUnsuspend, .gridview-container .gridview-row .btnSuspend {
                font-size: 12px; /* Adjusted button font size */
                border-radius: 8px; /* Slightly rounded buttons */
                padding: 5px 10px; /* Comfortable padding */
            }

            .gridview-container .gridview-row .imgEdit, .gridview-container .gridview-row .Image1 {
                border-radius: 8px; /* Rounded corners for images */
            }
        /*end of tabs design*/



        /* Styling for GridView Tables */
        #gridViewWaste, #gridView2, #gridView3, #gridView4 {
            width: 100%;
            border-collapse: separate; /* Allows styling of borders separately */
            border-spacing: 0; /* Removes additional spacing */
            border-radius: 0 0 12px 12px; /* Rounded bottom corners */
            overflow: hidden; /* Prevents content overflow */
            background-color: #f9f9f9; /* Light background color */
            margin-top: -1px; /* Seamless connection between tabs and content */
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1); /* Soft shadow for better depth */
        }

            /* GridView Header Styling */
            #gridViewWaste th, #gridView2 th, #gridView3 th, #gridView4 th {
                background: #66CDAA; /* Luminous green for header */
                color: white;
                font-size: 16px; /* Slightly smaller font for a refined look */
                font-weight: 600; /* Medium weight for balanced appearance */
                text-align: center;
                padding: 12px;
                border-bottom: 1px solid #ddd; /* Light bottom border */
            }

            /* GridView Row Styles */
            #gridViewWaste td, #gridView2 td, #gridView3 td, #gridView4 td {
                padding: 12px;
                font-size: 14px; /* Smaller font size */
                font-weight: 500; /* Medium weight for row text */
                color: #333; /* Darker text color */
                border-bottom: 1px solid #ddd; /* Light row separators */
                word-wrap: break-word; /* Text wraps neatly */
                text-align: center; /* Centered text alignment */
            }

            /* Hover Effect for GridView Rows */
            #gridViewWaste tr:hover, #gridView2 tr:hover, #gridView3 tr:hover, #gridView4 tr:hover {
                background: #eef8ee; /* Subtle hover effect */
            }

            /* Styling the GridView Footer */
            #gridViewWaste .FooterStyle, #gridView2 .FooterStyle, #gridView3 .FooterStyle, #gridView4 .FooterStyle {
                border-radius: 0 0 12px 12px; /* Rounded bottom corners */
                background: #66CDAA;
                color: #fff;
                text-align: center;
                padding: 12px;
            }

            /* Optional: Styling for the Status and Action Buttons */
            #gridViewWaste .btnUnsuspend, #gridView2 .btnUnsuspend, #gridView3 .btnUnsuspend, #gridView4 .btnUnsuspend,
            #gridViewWaste .btnSuspend, #gridView2 .btnSuspend, #gridView3 .btnSuspend, #gridView4 .btnSuspend {
                font-size: 10px; /* Adjusted button font size */
                border-radius: 8px; /* Slightly rounded buttons */
                padding: 5px 10px; /* Comfortable padding */
            }

            #gridViewWaste .imgEdit, #gridView2 .imgEdit, #gridView3 .imgEdit, #gridView4 .imgEdit,
            #gridViewWaste .Image1, #gridView2 .Image1, #gridView3 .Image1, #gridView4 .Image1 {
                border-radius: 8px; /* Rounded corners for images */
                margin-right: 10px; /* Margin between images */
            }

        /* Add this style to change the modal header and footer background color */
        .modal-header,
        .modal-footer {
            background-color: #041d06;
            border-top-color: #052507;
            border-bottom-color: #052507;
        }

        .modal-content .modal-header .btn-close {
            color: aquamarine !important;
        }
        /* Add this style to change the modal title text color */
        .modal-title {
            color: mediumaquamarine; /* You can adjust the text color as needed */
            /*        text-align: center !important;
*/
        }

        /* Add this style to change the modal body background color */
        .modal-body {
            background-color: #052507; /* You can adjust the background color as needed */
            /*        border-color: aquamarine;
*/
        }

        .modal-content .modal-header button.custom-close-button {
            color: aquamarine !important;
            background-color: aquamarine !important; /* Background color */
            border: none; /* Remove the default border */
            padding: 0; /* Remove padding */
        }
        /* Add this style to change the color of the close button */
        .btn-close {
            color: #ffffff; /* You can adjust the text color as needed */
        }

        .modal-content {
            border: 2.5px solid aquamarine; /* You can adjust the border color as needed */
            border-radius: 10px; /* You can adjust the border-radius as needed */
        }

        .modal-header .btn-close {
            color: aquamarine;
            background-color: aquamarine;
        }

        .gridview-container .columns_label {
            white-space: normal;
            word-wrap: break-word;
        }
    </style>
    <style type="text/css">
        .customGridView tr {
            border-bottom: 1px solid #0a4d1d;
        }
    </style>

    <style>
        .arrow-button {
            background-color: transparent;
            /*        color: orangered;
*/ border: none;
            cursor: pointer;
            font-size: 16px; /* Adjust the font size as needed */
        }

            .arrow-button::before {
                content: "\2191"; /* Unicode character for the arrow-up */
            }
    </style>
    <style>
        .edit-icon {
            filter: invert(68%) sepia(54%) saturate(2180%) hue-rotate(161deg) brightness(91%) contrast(87%);
        }
    </style>
    <script src="https://code.jquery.com/jquery-3.6.4.min.js"></script>
    <script type="text/javascript">
        function search() {
            var searchText = document.getElementById('<%= txtSearch.ClientID %>').value.toUpperCase();

        // Array of your GridView IDs
        var gridViewIds = [

        '<%= gridViewWaste.ClientID %>'
            ];

            gridViewIds.forEach(function (gridId) {
                var grid = document.getElementById(gridId);
                if (grid) {
                    for (var i = 1; i < grid.rows.length; i++) {
                        var row = grid.rows[i];
                        var cellContent = Array.from(row.cells).map(cell => cell.innerHTML.toUpperCase());

                        // Check if any cell contains the search text
                        var match = cellContent.some(content => content.indexOf(searchText) > -1);

                        row.style.display = match ? '' : 'none'; // Show or hide row based on match
                    }
                }
            });
        }
    </script>


    </script>

</head>
<form id="form2" runat="server">
    <div>
        <body style="background-color: #041d06">

            <!-- ======= Header ======= -->
            <%--#9ee2a0, #9ee2a0, #9ee2a0--%>
            <header style="background-color: black; height: 80px" id="header" class="header fixed-top d-flex align-items-center">

                <div class="d-flex align-items-center justify-content-between">
                    <a href="WAREHOUSE_ADD_ITEM.aspx" class="logo d-flex align-items-center">
                        <img style="border-radius: 1px" src="Pictures/logo_bgRM.png" alt="" />
                        <span style="color: aqua; font-weight: 900; font-family: 'Agency FB'" class="d-none d-lg-block">TrashTrack</span>
                    </a>
                    <i style="color: aqua" class="bi bi-list toggle-sidebar-btn"></i>
                </div>
                <!-- End Logo -->
                <nav class="header-nav ms-auto">
                    <ul class="d-flex align-items-center">
                        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>


                        <li class="nav-item dropdown">
                            <asp:UpdatePanel ID="UpdatePanelNotifications1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:LinkButton
                                        data-bs-toggle="dropdown"
                                        ID="LinkButton7"
                                        runat="server"
                                        OnClick="NotificationBell_Click"
                                        aria-expanded="false"
                                        CssClass="nav-link nav-icon">
                                        <i class="bi bi-bell"></i>
                                        <span id="notificationCount1" runat="server" class="badge bg-primary badge-number" style="display: none;">0</span>
                                    </asp:LinkButton>
                                    <asp:Timer ID="NotificationTimer1" runat="server" Interval="5000" OnTick="NotificationTimer_Tick" />

                                    <!-- Notification Dropdown -->
                                    <ul class="dropdown-menu dropdown-menu-end dropdown-menu-arrow notifications" id="notificationDropdown1">
                                        <!-- Header -->
                                        <li class="dropdown-header">You have <span id="notificationHeader1" runat="server">0</span> new notifications
                   
                                            <asp:LinkButton
                                                ID="lnkViewAllNotifications1"
                                                runat="server"
                                                OnClick="ViewAllNotifications_Click"
                                                CssClass="badge rounded-pill bg-primary p-2 ms-2">
                        Mark all read
                    </asp:LinkButton>
                                        </li>
                                        <li>
                                            <hr class="dropdown-divider">
                                        </li>

                                        <!-- Scrollable Repeater Container -->
                                        <div style="max-height: 305px; overflow-y: auto;">
                                            <asp:Repeater ID="NotificationRepeater1" runat="server">
                                                <ItemTemplate>
                                                    <!-- Notification Item -->




                                                    <li id="notifReadHighLight1" class="notification-item <%# Eval("NotifRead1").ToString() == "True" ? "" : "bg-highlight" %>">
                                                        <i id="notifTypee1" class="<%# GetNotificationIcon(Eval("NotifType1").ToString()) %> me-2"></i>
                                                        <div>
                                                            <h4>
                                                                <div class="d-flex justify-content-between align-items-center">
                                                                    <!-- Notification Type -->
                                                                    <asp:LinkButton
                                                                        ID="LinkButton2"
                                                                        runat="server"
                                                                        CommandArgument='<%# Eval("NotifId1") %>'
                                                                        OnClick="Notification_Click"
                                                                        CssClass="notification-header"
                                                                        Style="color: inherit;"
                                                                        onmouseover="this.style.color='black'; this.style.textDecoration='none';"
                                                                        onmouseout="this.style.color='inherit';">
                                                <%# Eval("NotifType1") %>
                                                                    </asp:LinkButton>

                                                                    <!-- Badge for New Notifications -->
                                                                    <asp:Literal
                                                                        ID="litNewBadge1"
                                                                        runat="server"
                                                                        Visible='<%# Eval("NotifRead1").ToString() == "False" %>'>
                                                <span style="margin-left: 5px" class="badge bg-success text-white">New</span>
                                                                    </asp:Literal>

                                                                    <!-- Delete Button -->
                                                                    <asp:LinkButton
                                                                        ID="btnDeleteNotification1"
                                                                        runat="server"
                                                                        CommandArgument='<%# Eval("NotifId1") %>'
                                                                        OnClick="DeleteNotification_Click"
                                                                        CssClass="bi bi-x-circle-fill text-danger ms-auto">
                                                                    </asp:LinkButton>
                                                                </div>
                                                            </h4>
                                                            <p>
                                                                <asp:LinkButton
                                                                    ID="lnkNotification1"
                                                                    runat="server"
                                                                    CommandArgument='<%# Eval("NotifId1") %>'
                                                                    OnClick="Notification_Click"
                                                                    CssClass="notification-link"
                                                                    Style="color: inherit;"
                                                                    onmouseover="this.style.color='black'; this.style.textDecoration='none';"
                                                                    onmouseout="this.style.color='inherit';">
                                            <%# Eval("NotifMessage1") %>
                                                                </asp:LinkButton>
                                                            </p>
                                                            <p class="notification-footer">
                                                                <span id="createdAt1" class="text-muted"><%# Eval("NotifCreatedAt1", "{0:yyyy-MM-dd HH:mm}") %></span>
                                                                <span id="custID1" class="text-muted ms-2">Customer ID: <%# Eval("CusId1") %></span>
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
                                            <asp:LinkButton ID="btnDeleteAllNotifications1" runat="server" OnClick="DeleteAllNotifications_Click" CssClass="btn btn-link">
                        Delete all notifications
                    </asp:LinkButton>
                                        </li>
                                    </ul>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="LinkButton7" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="NotificationTimer1" EventName="Tick" />

                                </Triggers>
                            </asp:UpdatePanel>
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
                                        <asp:Label ID="Label3" runat="server" Text="Administrator"></asp:Label></span>
                                </li>
                                <li>
                                    <hr class="dropdown-divider">
                                </li>
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
                        <a class="nav-link collapsed" href="BO_Dashboard.aspx">
                            <i class="bi bi-grid"></i>
                            <span>Dashboard</span>
                        </a>

                    </li>
                    <!-- End Employee Nav -->


                    <li class="nav-item">
                        <a class="nav-link" data-bs-target="#tables-nav" data-bs-toggle="collapse" href="#">
                            <i class="ri-bill-line"></i><span>Billing</span><i class="bi bi-chevron-down ms-auto"></i>
                        </a>
                        <ul id="tables-nav" class="nav-content collapse show" data-bs-parent="#sidebar-nav">
                            <li>
                                <a href="BO_Billing.aspx">
                                    <i class="bi bi-circle"></i><span>Generate Bill</span>
                                </a>
                            </li>

                            <li>
                                <a href="BO_Controls.aspx" class="active">
                                    <i class="bi bi-circle"></i><span>Controls</span>
                                </a>
                            </li>
                        </ul>
                    </li>

                    <li class="nav-item">
                        <a class="nav-link collapsed" href="BO_History.aspx">
                            <i class="ri-secure-payment-line"></i><span>Transaction History</span>
                        </a>

                    </li>
                    <%--<li class="nav-item">
            <a class="nav-link collapsed" data-bs-target="#receipt-nav" data-bs-toggle="collapse" href="#">
                <i class="ri-secure-payment-line"></i><span>Payment</span><i class="bi bi-chevron-down ms-auto"></i>
            </a>
            <ul id="receipt-nav" class="nav-content collapse" data-bs-parent="#sidebar-nav">
                <li>
                    <a href="Admin_Payment_ViewPays.aspx">
                        <i class="bi bi-circle"></i><span>View Payments</span>
                    </a>
                </li>

            </ul>
        </li>--%>


                    <%--        <li class="nav-item">
            <a class="nav-link collapsed" data-bs-target="#components-nav" data-bs-toggle="collapse" href="#">
          <i class="ri-bar-chart-2-line"></i><span>Reports</span><i class="bi bi-chevron-down ms-auto"></i>
        </a>
        <ul id="components-nav" class="nav-content collapse" data-bs-parent="#sidebar-nav">
          <li>
            <a href="WAREHOUSE_VIEW_REQUESTS.aspx">
              <i class="bi bi-circle"></i><span>Requests</span>
            </a>
          </li>
      </li>--%><!-- End Transaction Nav -->
                </ul>
            </aside>
            <!-- End Sidebar-->

            <main id="main" class="main">

                <div class="pagetitle">
                    <h1 style="padding-top: 20px; color: chartreuse">Billing</h1>
                    <%--<nav>
                        <ol class="breadcrumb">
                            <li class="breadcrumb-item"><a href="Admin_Dashboard.aspx">Dashboard</a></li>
                            <li class="breadcrumb-item"><a href="admin_manage_account.aspx">Billing</a></li>
                            <li class="breadcrumb-item">Controls</li>

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


                <div class="modal fade" id="exampleModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h1 class="modal-title fs-5" id="exampleModalLabel">Add Waste Category</h1>
                                <button type="button" class="btn-close custom-close-button" data-bs-dismiss="modal" aria-label="Close" style="color: aquamarine !important"></button>
                            </div>
                            <div class="modal-body">
                                <div class="mb-3">
                                    <asp:Label ID="Label13" runat="server" Text="Name" for="inputText" Style="color: chartreuse; margin-top: 10px"></asp:Label>
                                    <asp:TextBox ID="waste_name" runat="server" class="form-control" Style="margin-top: 10px"></asp:TextBox>
                                </div>
                                <div class="mb-3">
                                    <asp:Label ID="Label7" runat="server" Text="Description" for="waste_desc" Style="color: chartreuse"></asp:Label>
                                    <asp:TextBox ID="waste_desc" runat="server" TextMode="MultiLine" class="form-control" Rows="8"
                                        Style="margin-top: 10px; margin-bottom: 10px"></asp:TextBox>
                                </div>
                                <div class="mb-3">
                                    <asp:Label ID="Label4" runat="server" Text="Unit" for="inputText" Style="color: chartreuse"></asp:Label>
                                    <asp:TextBox ID="unit" runat="server" class="form-control" Style="margin-top: 10px; margin-bottom: 10px"></asp:TextBox>
                                </div>
                                <div class="mb-3">
                                    <asp:Label ID="Label5" runat="server" Text="Price" for="inputText" Style="color: chartreuse"></asp:Label>
                                    <asp:TextBox ID="price" runat="server" type="number" min="0.1" step="0.0000001" class="form-control" Style="margin-top: 10px; margin-bottom: 10px"></asp:TextBox>
                                </div>
                                <div class="mb-3">
                                    <asp:Label ID="Label12" runat="server" Text="Max Limit" for="inputText" Style="color: chartreuse"></asp:Label>
                                    <asp:TextBox ID="max" runat="server" type="number" min="0.1" step="0.0000001" class="form-control" Style="margin-top: 10px; margin-bottom: 10px"></asp:TextBox>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                                <asp:Button class="btn btn-primary" ID="submitBtn" runat="server" Text="Submit Form" OnClick="submitBtn_Click" />
                            </div>
                        </div>
                    </div>
                </div>


                <div class="modal fade" id="modalChangeTerm" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <%--<h1 class="modal-title fs-5" id="modalChangeInterestLabel">Change Payment Term</h1>--%>
                                <h1 class="modal-title fs-5" id="modalChangeInterestLabel">Change VAT (%)</h1>

                                <button type="button" class="btn-close custom-close-button" data-bs-dismiss="modal" aria-label="Close" style="color: aquamarine !important"></button>
                            </div>
                            <%--<div class="modal-body">
                                <div class="mb-3">
                                    <asp:Label ID="Label6" runat="server" Text="Overdue Interest (%)" for="inputText" Style="color: chartreuse; margin-top: 10px"></asp:Label>
                                    <asp:TextBox ID="interest" runat="server" type="number" min="0" class="form-control" Style="margin-top: 10px"></asp:TextBox>
                                </div>
                            </div>
                            <div class="modal-body">
                                <div class="mb-3">
                                    <asp:Label ID="Label7" runat="server" Text="Lead Day(s)" for="inputText" Style="color: chartreuse; margin-top: 10px"></asp:Label>
                                    <asp:TextBox ID="leaddays" runat="server" type="number" min="0" class="form-control" Style="margin-top: 10px"></asp:TextBox>
                                </div>
                            </div>--%>
                            <div class="modal-body">
                                <div class="mb-3">
                                    <asp:Label ID="Label8" runat="server" Text="VAT (%)" for="inputText" Style="color: chartreuse; margin-top: 10px"></asp:Label>
                                    <asp:TextBox ID="vat" runat="server" type="number" min="0" class="form-control" Style="margin-top: 10px"></asp:TextBox>
                                </div>
                            </div>
                            <%--<div class="modal-body">
                                <div class="mb-3">
                                    <asp:Label ID="Label9" runat="server" Text="Accrual Period" for="inputText" Style="color: chartreuse; margin-top: 10px"></asp:Label>
                                    <asp:TextBox ID="acc_per" runat="server" type="number" min="0" class="form-control" Style="margin-top: 10px"></asp:TextBox>
                                </div>
                            </div>
                            <div class="modal-body">
                                <div class="mb-3">
                                    <asp:Label ID="Label10" runat="server" Text="Suspension Period" for="inputText" Style="color: chartreuse; margin-top: 10px"></asp:Label>
                                    <asp:TextBox ID="susp_per" runat="server" type="number" min="0" class="form-control" Style="margin-top: 10px"></asp:TextBox>
                                </div>
                            </div>--%>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                                <asp:Button class="btn btn-primary" ID="Button1" runat="server" Text="Submit Form" OnClick="changeTerm_Click " />
                            </div>
                        </div>
                    </div>
                </div>



                <section style="background-color: #052507; padding: 25px; border-radius: 8px; box-shadow: 0 0 5px rgba(0, 0, 0, .2)">
                    <%--          <i class="bi bi-circle"></i><i class="bi bi-circle"></i>--%>
                    <div style="margin-top: 10px; margin-bottom: 10px; margin-left: 500px">
                        <asp:Label ID="lead_days" runat="server" Text="" CssClass="cool-label" Style="margin-right: 200px"></asp:Label>
                        <asp:Label ID="txtinterest" runat="server" Text="" CssClass="cool-label"></asp:Label>
                    </div>
                    <div style="margin-top: 10px; margin-bottom: 30px">
                        <asp:TextBox Style="border-radius: 10px; padding-left: 10px; padding: 2px; margin-top: 7px; border-color: aquamarine; border-width: 3px" placeholder="Search" ID="txtSearch" runat="server" oninput="search();" AutoPostBack="false"></asp:TextBox>
                        <button type="button" class="btn btn-primary" style="margin: 10px; float: right; background-color: #052507; border-color: aquamarine; border-radius: 8px; border-width: 3px" data-bs-toggle="modal" data-bs-target="#exampleModal">
                            Add Waste Category +
                        </button>
                        <button type="button" class="btn btn-primary" style="margin: 10px; float: right; background-color: #052507; border-color: aquamarine; border-radius: 8px; border-width: 3px" data-bs-toggle="modal" data-bs-target="#modalChangeTerm">
                            Change Payment Term ↻
                        </button>
                    </div>

                    <%--<div class="gridview-container">--%>
                    <asp:GridView Style="width: 100%; word-break: break-all; table-layout: fixed; border-radius: 0 0 15px 15px; overflow: hidden;"
                        ID="gridViewWaste" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                        DataKeyNames="wc_id" AllowPaging="False" CellPadding="20" Font-Size="10px" ForeColor="White" GridLines="None">

                        <AlternatingRowStyle BackColor="white" ForeColor="Black" />

                        <Columns>
                            <asp:BoundField DataField="wc_id" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="wc_id" ItemStyle-Width="100px">
                                <ItemStyle Width="100px" Wrap="true" />
                            </asp:BoundField>

                            <asp:TemplateField HeaderText="Name" SortExpression="name" ItemStyle-CssClass="columns_label">
                                <EditItemTemplate>
                                    <asp:TextBox ID="name" runat="server" Text='<%# Bind("wc_name") %>' Width="80px"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="name" runat="server" Text='<%# Eval("wc_name") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Price" SortExpression="price" ItemStyle-CssClass="columns_label">
                                <EditItemTemplate>
                                    <asp:TextBox ID="price" runat="server" Text='<%# Bind("wc_price") %>' Width="80px"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblprice" runat="server" Text='<%# Eval("wc_price") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="100px" Wrap="true" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Unit" SortExpression="unit" ItemStyle-CssClass="columns_label">
                                <EditItemTemplate>
                                    <asp:TextBox ID="unit" runat="server" Text='<%# Bind("wc_unit") %>' Width="80px"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblunit" runat="server" Text='<%# Eval("wc_unit") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="100px" Wrap="true" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Max" SortExpression="unit" ItemStyle-CssClass="columns_label">
                                <EditItemTemplate>
                                    <asp:TextBox ID="limit" runat="server" Text='<%# Bind("wc_max") %>' Width="80px"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblimit" runat="server" Text='<%# Eval("wc_max") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="100px" Wrap="true" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <asp:LinkButton ID="update" runat="server" OnClick="Update_Click" CommandArgument='<%# Eval("wc_id") %>'>
                                        <asp:Image ID="imgEdit" runat="server" ImageUrl="~/Pictures/editlogo.png" Width="15%" Style="margin-right: 10px; width: 2em; height: auto; max-height: 100%;"  AlternateText="Edit" CssClass="edit-icon" />
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="Remove" runat="server" OnClick="Remove_Click" CommandArgument='<%# Eval("wc_id") %>' OnClientClick="return confirm('Are you sure you want to remove this category?');">
                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/Pictures/removeBtn.png" Style="margin-right: 10px; width: 2em; height: auto; max-height: 100%;"  AlternateText="Remove" />
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>

                        <RowStyle BackColor="White" ForeColor="Black" />
                        <EditRowStyle BackColor="#90EE90" />
                        <FooterStyle BackColor="Black" Font-Bold="True" ForeColor="#f9cfb4" />
                        <HeaderStyle BackColor="#66CDAA" Font-Bold="True" ForeColor="black" BorderStyle="None" />
                        <PagerStyle BorderColor="#CC9900" Font-Size="20px" BackColor="White" ForeColor="#f9cfb4" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="White" />
                        <SortedAscendingCellStyle BackColor="Black" />
                        <SortedAscendingHeaderStyle BackColor="#246B61" />
                        <SortedDescendingCellStyle BackColor="Black" />
                        <SortedDescendingHeaderStyle BackColor="#15524A" />
                    </asp:GridView>
                    <%--</div>--%>



                    <asp:Label ID="Label11" runat="server" Text="" Style="color: white"></asp:Label>

                </section>














<asp:LinkButton ID="LinkButton1" runat="server"></asp:LinkButton>

                <div class="container" style="max-height: 100vh; overflow-y: hidden; display: flex; justify-content: center; align-items: center; z-index: 200">
                    <!-- Main Panel Design -->
                    <asp:UpdatePanel ID="updatePanel" runat="server" CssClass="card shadow-lg scrollable-panel" UpdateMode="Conditional" ChildrenAsTriggers="true" style="position: relative;">
                        <ContentTemplate>
                            <!-- Card Container -->
                            <div class="card shadow-lg" style="max-width: 1000px; padding: 0; border: 2px solid #26D8A8; border-radius: 12px; overflow: hidden; box-shadow: 0 5px 15px rgba(0, 0, 0, 0.3);">

                                <div class="modal-header" style="background-color: #0D342D; color: #26D8A8;">
<%--                                    <asp:Button ID="Button3" class="btn-close" runat="server" OnClick="btncancel_Click" />--%>
                                </div>

                                <!-- Card Header Design -->
                                <div class="card-header text-center" style="background-color: #0D342D; color: #26D8A8; padding: 15px;">
                                    <h4>Update Waste Category</h4>
                                </div>

                                <!-- Card Body with Form Elements - Scrollable Section -->
                                <div class="card-body scrollable-content" style="padding: 30px; background-color: #052507; max-height: 400px; overflow-y: auto;">
                                    <div class="row" style="margin-top: 15px;">

                                        <div class="col-6">

                                            <div class="input-group input-group-sm mb-3">
                                                <span class="input-group-text" id="inputGroup-sizing-sm" style="width: 140px">ID</span>
                                                <asp:TextBox ID="txtbxID" runat="server" CssClass="form-control" ClientIDMode="Static" aria-label="Small" aria-describedby="inputGroup-sizing-sm" Enabled="false"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-6">
                                            <div class="input-group input-group-sm mb-3">
                                                <span class="input-group-text" id="inputGroup-sizing-sm" style="width: 140px">Name</span>
                                                <asp:TextBox ID="txtbxnewName" runat="server" CssClass="form-control" ClientIDMode="Static" aria-label="Small" aria-describedby="inputGroup-sizing-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-6">
                                            <div class="input-group input-group-sm mb-3">
                                                <span class="input-group-text" id="inputGroup-sizing-sm" style="width: 140px; height: 100px;">Description</span>
                                                <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" ClientIDMode="Static"
                                                    TextMode="MultiLine" Rows="10" Style="height: 100px;"
                                                    aria-label="Small" aria-describedby="inputGroup-sizing-sm"></asp:TextBox>
                                            </div>
                                        </div>



                                        <div class="col-6">
                                            <div class="input-group input-group-sm mb-3">
                                                <span class="input-group-text" id="inputGroup-sizing-sm" style="width: 140px">Unit</span>
                                                <asp:TextBox ID="txtbxnewUnit" runat="server" CssClass="form-control" ClientIDMode="Static" aria-label="Small" aria-describedby="inputGroup-sizing-sm"></asp:TextBox>
                                            </div>
                                            <div class="input-group input-group-sm mb-3">
                                                <span class="input-group-text" id="inputGroup-sizing-sm" style="width: 140px">Price</span>
                                                <asp:TextBox ID="txtbxnewPrice" runat="server" CssClass="form-control" ClientIDMode="Static" type="number" min="0.1" step="0.0000001" aria-label="Small" aria-describedby="inputGroup-sizing-sm"></asp:TextBox>
                                            </div>
                                        </div>
                                        <%--<div class="col-6">
                                        </div>--%>
                                        <div class="col-6">
                                            <div class="input-group input-group-sm mb-3">
                                                <span class="input-group-text" id="inputGroup-sizing-sm" style="width: 140px">Max Limit</span>
                                                <asp:TextBox ID="txtLimit" runat="server" CssClass="form-control" ClientIDMode="Static" type="number" min="0.1" step="0.0000001" aria-label="Small" aria-describedby="inputGroup-sizing-sm"></asp:TextBox>
                                            </div>
                                        </div>


                                    </div>
                                </div>
                                <!-- Footer Design -->
                                <div class="card-footer text-center" style="background-color: #0D342D; color: #26D8A8; padding: 10px;">
                                    <asp:Button ID="btncancel" CssClass="btn btn-secondary" runat="server" Text="Cancel" />
                                <asp:Button ID="btnUpdate" CssClass="btn btn-primary" runat="server" Text="Update" OnClick="UpdateWasteCategory" OnClientClick="return confirm('Are you sure you want to update category?');" />
                                </div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btncancel" />
                        <asp:PostBackTrigger ControlID="btnUpdate" />

                        </Triggers>
                    </asp:UpdatePanel>

                </div>












                
                <%--<asp:UpdatePanel ID="updatePanel" CssClass="card" runat="server" Style="background-color: #052507; border: 1px solid aquamarine; width: 80%; left: 8%">
                    <ContentTemplate>
                        <div class="card bg-light" style="background-color: #052507">
                            <div class="card-header" style="background-color: #052507; color: aquamarine;">
                                <h4>Update Waste Category</h4>
                            </div>
                            <div class="card-body" style="background-color: #052507; padding-top: 20px">
                                <div class="row">
                                    <div class="col-6">

                                        <div class="input-group input-group-sm mb-3">
                                            <span class="input-group-text" id="inputGroup-sizing-sm" style="width: 140px">ID</span>
                                            <asp:TextBox ID="txtbxID" runat="server" CssClass="form-control" ClientIDMode="Static" aria-label="Small" aria-describedby="inputGroup-sizing-sm" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-6">
                                        <div class="input-group input-group-sm mb-3">
                                            <span class="input-group-text" id="inputGroup-sizing-sm" style="width: 140px">Name</span>
                                            <asp:TextBox ID="txtbxnewName" runat="server" CssClass="form-control" ClientIDMode="Static" aria-label="Small" aria-describedby="inputGroup-sizing-sm"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-6">
                                        <div class="input-group input-group-sm mb-3">
                                            <span class="input-group-text" id="inputGroup-sizing-sm" style="width: 140px; height: 100px;">Description</span>
                                            <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" ClientIDMode="Static"
                                                TextMode="MultiLine" Rows="6" Style="height: 100px;"
                                                aria-label="Small" aria-describedby="inputGroup-sizing-sm"></asp:TextBox>
                                        </div>
                                    </div>



                                    <div class="col-6">
                                        <div class="input-group input-group-sm mb-3">
                                            <span class="input-group-text" id="inputGroup-sizing-sm" style="width: 140px">Unit</span>
                                            <asp:TextBox ID="txtbxnewUnit" runat="server" CssClass="form-control" ClientIDMode="Static" aria-label="Small" aria-describedby="inputGroup-sizing-sm"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-6">
                                        <div class="input-group input-group-sm mb-3">
                                            <span class="input-group-text" id="inputGroup-sizing-sm" style="width: 140px">Price</span>
                                            <asp:TextBox ID="txtbxnewPrice" runat="server" CssClass="form-control" ClientIDMode="Static" type="number" min="1" aria-label="Small" aria-describedby="inputGroup-sizing-sm"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-6">
                                        <div class="input-group input-group-sm mb-3">
                                            <span class="input-group-text" id="inputGroup-sizing-sm" style="width: 140px">Max Limit</span>
                                            <asp:TextBox ID="txtLimit" runat="server" CssClass="form-control" ClientIDMode="Static" type="number" min="1" aria-label="Small" aria-describedby="inputGroup-sizing-sm"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="card-footer" style="background-color: #052507; color: aquamarine;">
                                <asp:Button ID="btncancel" CssClass="btn btn-secondary" runat="server" Text="Cancel" />
                                <asp:Button ID="btnUpdate" CssClass="btn btn-primary" runat="server" Text="Update" OnClick="UpdateWasteCategory" OnClientClick="return confirm('Are you sure you want to update category?');" />
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                            <asp:PostBackTrigger ControlID="btncancel" />
                        <asp:PostBackTrigger ControlID="btnUpdate" />

                        </Triggers>
                </asp:UpdatePanel>--%>



                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" CancelControlID="btncancel" PopupControlID="updatePanel" TargetControlID="LinkButton1" BackgroundCssClass="Background" DropShadow="True"></ajaxToolkit:ModalPopupExtender>
                <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
                <script>
                    $(function () {
                        $("input[name='EmpId']").on('input', function (e) {
                            $(this).val($(this).val().replace(/[^0-9]/g, ''));
                        });
                    });



                </script>




                <%--<style>
                    /* Container Styles */
                    .gridview-container {
                        max-height: 530px;
                        overflow-y: auto;
                        position: relative;
                    }

                        /* WebKit Scrollbar Styles */
                        .gridview-container::-webkit-scrollbar {
                            width: 12px;
                        }

                        .gridview-container::-webkit-scrollbar-thumb {
                            background-color: #1C5E55;
                            border-radius: 10px;
                            border: 3px solid aquamarine;
                        }

                        .gridview-container::-webkit-scrollbar-track {
                            background-color: #f5f5f5;
                            border-radius: 10px;
                        }

                    /* Firefox Scrollbar Styles */
                    .gridview-container {
                        scrollbar-color: #1C5E55 #f5f5f5;
                    }

                        /* Table Styles */
                        .gridview-container table {
                            table-layout: fixed;
                            width: 100%;
                        }

                        .gridview-container th,
                        .gridview-container td {
                            white-space: nowrap;
                            overflow: hidden;
                            text-overflow: ellipsis;
                        }

                        .gridview-container .description-column {
                            white-space: normal;
                            word-wrap: break-word;
                        }

                        .gridview-container thead {
                            position: sticky;
                            top: 0;
                            background-color: #1C5E55;
                            color: white;
                            z-index: 2; /* Keep the header on top */
                        }

                            .gridview-container thead th {
                                position: sticky;
                                top: 0;
                                background-color: #1C5E55;
                                color: white;
                                z-index: 3; /* Increase z-index to keep the header text on top */
                            }

                        /* Animation for Hover Effect */
                        .gridview-container::-webkit-scrollbar-thumb:hover {
                            background-color: #167c6d;
                        }

                        .gridview-container::-webkit-scrollbar-track:hover {
                            background-color: #d8d8d8;
                        }

                        .gridview-container::-webkit-scrollbar-thumb:active {
                            background-color: #134f45;
                        }

                        .gridview-container::-webkit-scrollbar-track:active {
                            background-color: #c2c2c2;
                        }
                </style>--%>
                <script>
                    function updateNotificationCount1() {
                        $.ajax({
                            type: 'GET',
                            url: '/api/payment/notificationCount1',  // Your API endpoint
                            success: function (response) {
                                var count = response.unreadCount1;  // The unread notification count
                                if (count > 0) {
                                    $('#notificationCount1').text(count).show();
                                    $('#notificationHeader1').text(count);
                                } else {
                                    $('#notificationCount1').hide();
                                    $('#notificationHeader1').text('0');
                                }
                            },
                            error: function () {
                                console.log('Error fetching notification count');
                            }
                        });
                    }

                    // Set interval to update the count (every 10 seconds)
                    setInterval(updateNotificationCount1, 100); // Run every 10 seconds

                    let isDropdownOpen = false;

                    function detectDropdownState() {
                        const dropdown = document.querySelector('#notificationDropdown1');
                        isDropdownOpen = dropdown && dropdown.classList.contains('show');
                    }

                    function restoreDropdownState() {
                        const dropdown = document.querySelector('#notificationDropdown1');
                        const dropdownToggle = document.querySelector('[data-bs-toggle="dropdown"]');
                        if (isDropdownOpen && dropdown && dropdownToggle) {
                            dropdown.classList.add('show');
                            dropdownToggle.setAttribute('aria-expanded', 'true');
                        }
                    }

                    // Hook into ASP.NET UpdatePanel lifecycle events
                    Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(() => detectDropdownState());
                    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(() => restoreDropdownState());

                    function updateNotificationCount() {
                        fetch('/api/payment/notificationCount1', { method: 'GET' })
                            .then(response => response.json())
                            .then(data => {
                                // Assuming there's an element with ID 'notificationCount' to show the count
                                document.getElementById('notificationCount1').innerText = data.unreadCount1 || 0;
                            })
                            .catch(error => {
                                console.error('Error updating notification count:', error);
                            });
                    }




                    function markAllNotificationsAsDelete() {
                        $.ajax({
                            type: 'POST',
                            url: '/api/payment/deleteAllNotifications1',
                            success: function (updatedNotifications) {
                                updateNotificationUI(updatedNotifications);
                                updateNotificationCount1();  // Refresh count
                            },
                            error: function () {
                                console.error('Failed to delete all notifications.');
                            }
                        });
                    }

                    function markAllNotificationsAsRead() {
                        $.ajax({
                            type: 'POST',
                            url: '/api/payment/markAllAsRead1',
                            success: function (updatedNotifications) {
                                updateNotificationUI(updatedNotifications);
                                updateNotificationCount1();  // Refresh count
                            },
                            error: function () {
                                console.error('Failed to mark all notifications as read.');
                            }
                        });
                    }

                    document.addEventListener('click', (event) => {
                        const dropdown = document.querySelector('#notificationDropdown1');
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
                </script>
                <script src="https://code.jquery.com/jquery-3.6.4.min.js"></script>
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

