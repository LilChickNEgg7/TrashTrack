<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AM_AccountMan.aspx.cs" Inherits="Capstone.AM_AccountMan" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8">
    <meta content="width=device-width, initial-scale=1.0" name="viewport">

    <title>Dashboard</title>
    <meta content="" name="description">
    <meta content="" name="keywords">


    <%--hide unhide pass--%>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css" />


    <!-- Favicons -->
    <%--<link href="assets/img/favicon.png" rel="icon">
    <link href="assets/img/apple-touch-icon.png" rel="apple-touch-icon">--%>
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
        .notification-link {
            color: black; /* Initial color */
            text-decoration: none; /* Optional: remove underline */
        }

            .notification-link:hover {
                color: gray; /* Change to gray on hover */
                cursor: pointer; /* Show pointer cursor */
            }

        .notification-item.read-notification {
            background-color: #f0f0f0; /* Lighter background for read notifications */
            color: #999; /* Dimmed text color */
        }

        .dropdown-menu.notifications {
            max-height: 300px;
            overflow-y: auto;
        }

        .read-notification {
            background-color: #f0f0f0; /* Example color for read notifications */
            color: #999; /* Optional: Change text color for read notifications */
        }


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
        #gridView1, #gridView2, #gridView3, #gridView4 {
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
            #gridView1 th, #gridView2 th, #gridView3 th, #gridView4 th {
                background: #66CDAA; /* Luminous green for header */
                color: white;
                font-size: 16px; /* Slightly smaller font for a refined look */
                font-weight: 600; /* Medium weight for balanced appearance */
                text-align: center;
                padding: 12px;
                border-bottom: 1px solid #ddd; /* Light bottom border */
            }

            /* GridView Row Styles */
            #gridView1 td, #gridView2 td, #gridView3 td, #gridView4 td {
                padding: 12px;
                font-size: 14px; /* Smaller font size */
                font-weight: 500; /* Medium weight for row text */
                color: #333; /* Darker text color */
                border-bottom: 1px solid #ddd; /* Light row separators */
                word-wrap: break-word; /* Text wraps neatly */
                text-align: center; /* Centered text alignment */
            }

            /* Hover Effect for GridView Rows */
            #gridView1 tr:hover, #gridView2 tr:hover, #gridView3 tr:hover, #gridView4 tr:hover {
                background: #eef8ee; /* Subtle hover effect */
            }

            /* Styling the GridView Footer */
            #gridView1 .FooterStyle, #gridView2 .FooterStyle, #gridView3 .FooterStyle, #gridView4 .FooterStyle {
                border-radius: 0 0 12px 12px; /* Rounded bottom corners */
                background: #66CDAA;
                color: #fff;
                text-align: center;
                padding: 12px;
            }

            /* Optional: Styling for the Status and Action Buttons */
            #gridView1 .btnUnsuspend, #gridView2 .btnUnsuspend, #gridView3 .btnUnsuspend, #gridView4 .btnUnsuspend,
            #gridView1 .btnSuspend, #gridView2 .btnSuspend, #gridView3 .btnSuspend, #gridView4 .btnSuspend {
                font-size: 10px; /* Adjusted button font size */
                border-radius: 8px; /* Slightly rounded buttons */
                padding: 5px 10px; /* Comfortable padding */
            }

            #gridView1 .imgEdit, #gridView2 .imgEdit, #gridView3 .imgEdit, #gridView4 .imgEdit,
            #gridView1 .Image1, #gridView2 .Image1, #gridView3 .Image1, #gridView4 .Image1 {
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
    <%--start of searchbar script--%>
    <script type="text/javascript">
        function search() {
            var searchText = document.getElementById('<%= txtSearch.ClientID %>').value.toUpperCase();

            // Array of your GridView IDs
            var gridViewIds = [

            '<%= gridView2.ClientID %>',
            '<%= gridView3.ClientID %>',
            '<%= gridView4.ClientID %>'
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
</head>
<form id="form2" runat="server" class="row g-3 needs-validation" novalidate>
    <div>
        <body style="background-color: #041d06">

            <!-- ======= Header ======= -->
            <%--#9ee2a0, #9ee2a0, #9ee2a0--%>
            <header style="background-color: black; height: 80px" id="header" class="header fixed-top d-flex align-items-center">

                <div class="d-flex align-items-center justify-content-between">
                    <a href="SAM_AccountMan.aspx" class="logo d-flex align-items-center">
                        <img style="border-radius: 1px" src="Pictures/logo_bgRM.png" alt="" />
                        <span style="color: aqua; font-weight: 900; font-family: 'Agency FB'" class="d-none d-lg-block">TrashTrack</span>
                    </a>
                    <i style="color: aqua" class="bi bi-list toggle-sidebar-btn"></i>
                </div>
                <!-- End Logo -->
                <nav class="header-nav ms-auto">
                    <ul class="d-flex align-items-center">
                        <%--notification--%>
                        <li class="nav-item dropdown">
                            <a class="nav-link nav-icon" href="#" data-bs-toggle="dropdown">
                                <i class="bi bi-bell"></i>
                                <span id="notificationCount" runat="server" class="badge bg-primary badge-number">0</span>
                            </a>
                            <!-- Notification Dropdown -->
                            <ul class="dropdown-menu dropdown-menu-end dropdown-menu-arrow notifications" style="max-height: 300px; overflow-y: auto;">
                                <li class="dropdown-header">You have <span id="notificationHeader" runat="server">0</span> new notifications
                                    <a href="#"><span class="badge rounded-pill bg-primary p-2 ms-2">View all</span></a>
                                </li>
                                <li>
                                    <hr class="dropdown-divider">
                                </li>
                                <asp:Repeater ID="NotificationRepeater" runat="server">
                                    <ItemTemplate>
                                        <li class="notification-item <%# Eval("read_status").ToString() == "Read" ? "read-notification" : "" %>">
                                            <i class='<%# GetNotificationIcon(Eval("cont_status").ToString()) %>'></i>
                                            <div>
                                                <h4>
                                                    <asp:LinkButton
                                                        ID="lnkNotification"
                                                        runat="server"
                                                        CommandArgument='<%# Eval("cont_id") %>'
                                                        OnClick="Notification_Click"
                                                        CssClass="notification-link"><%# Eval("cont_rep_name") %> - <%# Eval("cont_comp_name") %></asp:LinkButton>
                                                    <asp:Literal ID="litNewBadge" runat="server" Visible='<%# Eval("read_status").ToString() == "Unread" %>'>
                                                        <span class="badge bg-danger" style="color: white;">New</span>
                                                    </asp:Literal>
                                                </h4>
                                                <p><%# Eval("cont_status") %></p>
                                                <p><%# Eval("cont_created_at") %> ago</p>
                                            </div>
                                        </li>
                                        <li>
                                            <hr class="dropdown-divider">
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>

                                <li class="dropdown-footer">
                                    <a href="#">Show all notifications</a>
                                </li>
                            </ul>
                        </li>

                        <%--end of notification--%>



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
                                        <asp:Label ID="Label3" runat="server" Text=""></asp:Label></span>
                                </li>
                                <li>
                                    <hr class="dropdown-divider">
                                </li>
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
                        <a class="nav-link collapsed" href="AM_Dashboard.aspx">
                            <i class="bi bi-grid"></i>
                            <span>Dashboard</span>
                        </a>

                    </li>
                    <!-- End Employee Nav -->


                    <li class="nav-item">
                        <a class="nav-link" data-bs-target="#forms-nav" data-bs-toggle="collapse" href="#">
                            <%--          <i class="bi bi-people" style="color:#52996F"></i><span style="color:#52996F">Manage Account</span><i class="bi bi-chevron-down ms-auto" style="color:aquamarine"></i>--%>
                            <i class="bi bi-people"></i><span>Manage Accounts</span><i class="bi bi-chevron-down ms-auto"></i>

                        </a>
                        <ul id="forms-nav" class="nav-content collapse show" data-bs-parent="#sidebar-nav">
                            <li>
                                <a href="AM_AccountMan.aspx" class="active">
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

<!-- End Transaction Nav -->
                </ul>
            </aside>
            <!-- End Sidebar-->

            <main id="main" class="main">

                <div class="pagetitle">
                    <h1 style="padding-top: -5px; color: chartreuse">Employees</h1>
                    <%--<nav>
                        <ol class="breadcrumb">
                            <li class="breadcrumb-item"><a href="SAM_Dashboard.aspx">Dashboard</a></li>
                            <li class="breadcrumb-item"><a href="admin_manage_account.aspx">Manage Accounts   </a></li>
                            <li class="breadcrumb-item">Account Manager </li>

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
                            <!-- Modal Header -->
                            <div class="modal-header" style="position: center">
                                <h1 class="modal-title fs-5" id="exampleModalLabel">Add Employee/Staff</h1>
                                <button type="button" class="btn-close custom-close-button" data-bs-dismiss="modal" aria-label="Close"></button>
                            </div>

                            <!-- Modal Body -->
                            <div class="modal-body">
                                <!-- Firstname -->
                                <div class="mb-3">
                                    <asp:Label ID="Label13" runat="server" class="form-label" Text="Firstname" for="inputText" Style="color: chartreuse"></asp:Label>
                                    <asp:TextBox ID="emp_firstname" runat="server" class="form-control" onkeyup="validateFirstname()"></asp:TextBox>
                                    <div class="valid-feedback">Looks good!</div>
                                    <div class="invalid-feedback">Please provide a valid firstname.</div>
                                </div>

                                <!-- Middle Initial -->
                                <div class="mb-3">
                                    <asp:Label ID="Label4" runat="server" Text="M.I" for="inputText" Style="color: chartreuse"></asp:Label>
                                    <asp:TextBox ID="emp_mi" class="form-control" runat="server" MaxLength="1" onkeypress="return isLetter(event)"></asp:TextBox>
                                    <div class="valid-feedback">Looks good!</div>
                                    <div class="invalid-feedback">Please provide a valid middle initial (optional).</div>
                                </div>

                                <!-- Lastname -->
                                <div class="mb-3">
                                    <asp:Label ID="Label5" runat="server" Text="Lastname" for="inputText" Style="color: chartreuse"></asp:Label>
                                    <asp:TextBox ID="emp_lastname" runat="server" class="form-control" onkeyup="validateLastname()"></asp:TextBox>
                                    <div class="valid-feedback">Looks good!</div>
                                    <div class="invalid-feedback">Please provide a valid lastname.</div>
                                </div>

                                <!-- Address -->
                                <div class="mb-3">
                                    <asp:Label ID="Label11" runat="server" Text="Address" for="inputText" Style="color: chartreuse"></asp:Label>
                                    <asp:TextBox ID="emp_address" runat="server" class="form-control" onkeyup="validateAddress()"></asp:TextBox>
                                    <div class="valid-feedback">Looks good!</div>
                                    <div class="invalid-feedback">Please provide a valid address.</div>
                                </div>

                                <!-- Email -->
                                <div class="mb-3">
                                    <asp:Label ID="Label10" runat="server" Text="Email" for="inputText" Style="color: chartreuse"></asp:Label>
                                    <asp:TextBox ID="emp_email" runat="server" class="form-control" onkeyup="validateEmail()"></asp:TextBox>
                                    <div class="valid-feedback">Looks good!</div>
                                    <div class="invalid-feedback">Please provide a valid email address.</div>
                                </div>

                                <!-- Password -->
                                <div class="col-12 mb-3">
                                    <asp:Label ID="Label7" runat="server" Text="Password" for="inputText" Style="color: chartreuse"></asp:Label>
                                    <div class="input-group">
                                        <asp:TextBox ID="emp_pass" runat="server" class="form-control" type="password" onkeyup="validatePassword()"></asp:TextBox>
                                        <span class="input-group-text" style="cursor: pointer;" onclick="togglePasswords()">
                                            <i id="passIcon" class="fas fa-eye-slash"></i>
                                        </span>
                                    </div>
                                    <div class="invalid-feedback">Password must be at least 6 characters long and contain at least one uppercase letter, one number, and one special character.</div>
                                </div>


                                <!-- Contact No. -->
                                <div class="mb-3">
                                    <asp:Label ID="Label8" runat="server" Text="Contact No." for="inputText" Style="color: chartreuse"></asp:Label>
                                    <asp:TextBox ID="emp_contact" runat="server" class="form-control" onkeyup="validateContact()"></asp:TextBox>
                                    <div class="valid-feedback">Looks good!</div>
                                    <div class="invalid-feedback">Please provide a valid Philippines contact number (e.g., 09XXXXXXXXX).</div>
                                </div>

                                <!-- Dropdown (Role) -->
                                <div class="mb-3">
                                    <asp:Label ID="Label6" runat="server" Text="Role" for="inputText" Style="color: chartreuse"></asp:Label>
                                    <asp:DropDownList ID="emp_role" class="form-select" aria-label="Default select example" runat="server" onChange="validateRole()">
                                    </asp:DropDownList>
                                    <div class="invalid-feedback">Please select a role.</div>
                                </div>

                                <!-- File Upload (with Image Preview) -->
                                <div class="mb-3">
                                    <asp:Label ID="Label12" runat="server" Text="Picture" for="inputText" Style="color: chartreuse"></asp:Label>
                                    <asp:FileUpload ID="formFile" accept="image/*" class="form-control" runat="server" OnChange="previewImage1()" />
                                    <asp:ImageMap ID="imagePreview" src="Pictures/blank_prof.png" runat="server" alt="Image Preview" CssClass="img-thumbnail mt-2" Style="max-width: 150px;"></asp:ImageMap>
                                    <span id="fileError" style="color: red; display: none;">File uploaded is not an image.</span>
                                </div>
                            </div>

                            <!-- Modal Footer -->
                            <div class="modal-footer">
                                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                                <%--                                <asp:Button ID="ClearForm" Text="Close" runat="server" class="btn btn-secondary" data-bs-dismiss="modal" OnClick="ClearForm_Click" />--%>
                                <asp:Button class="btn btn-primary" ID="submitBtn" runat="server" Text="Submit Form" OnClick="submitBtn_Click" />
                            </div>
                        </div>
                    </div>
                </div>





                <section style="background-color: #052507; padding: 25px; border-radius: 8px; box-shadow: 0 0 5px rgba(0, 0, 0, .2)">
                    <div>
                        <asp:ImageMap ID="ImageMap1" runat="server" ImageUrl="Pictures//box_format.png" Style="float: right; margin-right: 0px; margin-top: 0px; width: 50px"></asp:ImageMap>
                    </div>
                    <div style="margin-top: 50px; margin-bottom: 30px">
                        <asp:TextBox Style="border-radius: 10px; padding-left: 10px; padding: 2px; margin-top: 7px; border-color: aquamarine; border-width: 3px" placeholder="Search" ID="txtSearch" runat="server" oninput="search();" AutoPostBack="false"></asp:TextBox>
                        <button type="button" class="btn btn-primary" style="margin: 10px; float: right; background-color: #052507; border-color: aquamarine; border-radius: 8px; border-width: 3px" data-bs-toggle="modal" data-bs-target="#exampleModal">
                            Add Staff +
                        </button>
                    </div>

                    <%--ADD BUTTON--%>


                    <%-- START TABS DESIGN GRIDVIEW--%>


                    <ul class="nav nav-tabs" id="myTab" role="tablist">
                        <%--<li class="nav-item" role="presentation">
                            <button class="nav-link active" id="sam-tab" data-bs-toggle="tab" data-bs-target="#sam" type="button" role="tab" aria-controls="sam" aria-selected="true" style="color: #061f0d; font-weight: 900">Super Account Manager</button>
                        </li>--%>
                        <li class="nav-item" role="presentation">
                            <button class="nav-link" id="am-tab" data-bs-toggle="tab" data-bs-target="#am" type="button" role="tab" aria-controls="am" aria-selected="false" style="color: #061f0d; font-weight: 900">Account Manager</button>
                        </li>
                        <li class="nav-item" role="presentation">
                            <button class="nav-link" id="bo-tab" data-bs-toggle="tab" data-bs-target="#bo" type="button" role="tab" aria-controls="bo" aria-selected="false" style="color: #061f0d; font-weight: 900">Billing Officer</button>
                        </li>
                        <li class="nav-item" role="presentation">
                            <button class="nav-link" id="od-tab" data-bs-toggle="tab" data-bs-target="#od" type="button" role="tab" aria-controls="od" aria-selected="false" style="color: #061f0d; font-weight: 900">Operational Dispatcher</button>
                        </li>
                        <li class="nav-item" role="presentation">
                            <button class="nav-link" id="hauler-tab" data-bs-toggle="tab" data-bs-target="#hauler" type="button" role="tab" aria-controls="hauler" aria-selected="false" style="color: #061f0d; font-weight: 900">Hauler</button>
                        </li>
                    </ul>


                    <div id="myTabContent" class="tab-content pt-2">

                        <%--<div class="tab-pane fade show active" id="sam" role="tabpanel" aria-labelledby="sam-tab">
                            <asp:GridView Style="width: 100%; word-break: break-all; table-layout: fixed" ID="gridView1" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                                DataKeyNames="emp_id" AllowPaging="False" CellPadding="20" GridLines="None">
                                <AlternatingRowStyle BackColor="white" ForeColor="Black" />

                                <Columns>
                                    <asp:BoundField DataField="emp_id" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="emp_id" ItemStyle-Width="100px">
                                        <ItemStyle Width="100px" Wrap="true" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="emp_fname" HeaderText="First Name" SortExpression="emp_fname" ItemStyle-Width="150px">
                                        <ItemStyle Width="150px" Wrap="true" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="emp_mname" HeaderText="M.I" SortExpression="emp_mname" ItemStyle-Width="100px">
                                        <ItemStyle Width="100px" Wrap="true" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="emp_lname" HeaderText="Last Name" SortExpression="emp_lname" ItemStyle-Width="150px">
                                        <ItemStyle Width="150px" Wrap="true" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="emp_contact" HeaderText="Contact" SortExpression="emp_contact" ItemStyle-Width="100px">
                                        <ItemStyle Width="100px" Wrap="true" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="emp_email" HeaderText="Email" SortExpression="emp_email" ItemStyle-Width="200px">
                                        <ItemStyle Width="200px" Wrap="true" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="emp_created_at" HeaderText="Created At" SortExpression="emp_created_at" ItemStyle-Width="150px">
                                        <ItemStyle Width="150px" Wrap="true" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="emp_updated_at" HeaderText="Updated At" SortExpression="emp_updated_at" ItemStyle-Width="150px">
                                        <ItemStyle Width="150px" Wrap="true" />
                                    </asp:BoundField>

                                    <asp:TemplateField HeaderText="Status">
                                        <ItemTemplate>
                                            <asp:Button Style="font-size: 10px; color: orangered; font-weight: bold;" ID="btnUnsuspend" runat="server" Text='<%# Eval("emp_status") + " ▼"%>'
                                                OnClick="Unsuspend_Click"
                                                OnClientClick="return confirm('Are you sure you want to Unsuspend this account manager?');"
                                                Visible='<%# Eval("emp_status").ToString() == "Suspend" %>' CommandArgument='<%# Eval("emp_id") %>' />
                                            <asp:Label class="badge bg-success" ID="Label9" runat="server" Text='<%# Eval("emp_status")%>' Visible='<%# Eval("emp_status").ToString() == "Inactive" %>' />
                                            <asp:Button Style="font-size: 10px; color: lawngreen; font-weight: bold;" ID="btnSuspend" runat="server" Text='<%# Eval("emp_status") + " ▲"%>'
                                                OnClick="Suspend_Click"
                                                OnClientClick="return confirm('Are you sure you want to Suspend this account manager?');"
                                                Visible='<%# Eval("emp_status").ToString() == "Active" %>' CommandArgument='<%# Eval("emp_id") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="update" runat="server" OnClick="Update_Click" CommandArgument='<%# Eval("emp_id") %>'>
                                                <asp:Image ID="imgEdit" runat="server" ImageUrl="~/Pictures/editlogo.png" Width="35%" Height="35%" Style="margin-right: 10px" AlternateText="Edit" />
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="Remove" runat="server" OnClick="Remove_Click" CommandArgument='<%# Eval("emp_id") %>' OnClientClick="return confirm('Are you sure you want to remove this account manager?');">
                                                <asp:Image ID="Image1" runat="server" ImageUrl="~/Pictures/removeBtn.png" Width="35%" Height="35%" AlternateText="Remove" />
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
                        </div>--%>


                        <div class="tab-pane fade" id="am" role="tabpanel" aria-labelledby="am-tab">
                            <%--<h1 style="color: #f40f0f; padding-top: 50px; padding-bottom: 0; font-family: 'Raleway',sans-serif; font-size: 62px; font-weight: 800; line-height: 72px; margin: 0 0 24px; text-align: center; text-transform: uppercase;">Account Manager
                            </h1>--%>
                            <%--<div class="gridview-container">--%>
                            <asp:GridView Style="width: 100%; word-break: break-all; table-layout: fixed" ID="gridView2" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                                DataKeyNames="emp_id" AllowPaging="False" CellPadding="20" GridLines="None">
                                <AlternatingRowStyle BackColor="white" ForeColor="Black" />

                                <Columns>
                                    <asp:BoundField DataField="emp_id" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="emp_id" ItemStyle-Width="100px">
                                        <ItemStyle Width="100px" Wrap="true" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="emp_fname" HeaderText="First Name" SortExpression="emp_fname" ItemStyle-Width="150px">
                                        <ItemStyle Width="150px" Wrap="true" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="emp_mname" HeaderText="M.I" SortExpression="emp_mname" ItemStyle-Width="100px">
                                        <ItemStyle Width="100px" Wrap="true" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="emp_lname" HeaderText="Last Name" SortExpression="emp_lname" ItemStyle-Width="150px">
                                        <ItemStyle Width="150px" Wrap="true" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="emp_contact" HeaderText="Contact" SortExpression="emp_contact" ItemStyle-Width="100px">
                                        <ItemStyle Width="100px" Wrap="true" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="emp_email" HeaderText="Email" SortExpression="emp_email" ItemStyle-Width="200px">
                                        <ItemStyle Width="200px" Wrap="true" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="emp_created_at" HeaderText="Created At" SortExpression="emp_created_at" ItemStyle-Width="150px">
                                        <ItemStyle Width="150px" Wrap="true" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="emp_updated_at" HeaderText="Updated At" SortExpression="emp_updated_at" ItemStyle-Width="150px">
                                        <ItemStyle Width="150px" Wrap="true" />
                                    </asp:BoundField>

                                    <asp:TemplateField HeaderText="Status">
                                        <ItemTemplate>
                                            <asp:Button Style="font-size: 10px; color: orangered; font-weight: bold;" ID="btnUnsuspend" runat="server" Text='<%# Eval("emp_status") + " ▼"%>'
                                                OnClick="Unsuspend_Click"
                                                OnClientClick="return confirm('Are you sure you want to Unsuspend this account manager?');"
                                                Visible='<%# Eval("emp_status").ToString() == "Suspend" %>' CommandArgument='<%# Eval("emp_id") %>' />
                                            <asp:Label ID="Label9" runat="server" Text='<%# Eval("emp_status")%>' Visible='<%# Eval("emp_status").ToString() == "Inactive" %>' />
                                            <asp:Button Style="font-size: 10px; color: lawngreen; font-weight: bold;" ID="btnSuspend" runat="server" Text='<%# Eval("emp_status") + " ▲"%>'
                                                OnClick="Suspend_Click"
                                                OnClientClick="return confirm('Are you sure you want to Suspend this account manager?');"
                                                Visible='<%# Eval("emp_status").ToString() == "Active" %>' CommandArgument='<%# Eval("emp_id") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="update" runat="server" OnClick="Update_Click" CommandArgument='<%# Eval("emp_id") %>'>
                                                <asp:Image ID="imgEdit" runat="server" ImageUrl="~/Pictures/editlogo.png" Style="margin-right: 10px; width: 2em; height: auto; max-height: 100%;"  AlternateText="Edit" />
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="Remove" runat="server" OnClick="Remove_Click" CommandArgument='<%# Eval("emp_id") %>' OnClientClick="return confirm('Are you sure you want to remove this account manager?');">
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
                        </div>
                        <div class="tab-pane fade" id="bo" role="tabpanel" aria-labelledby="bo-tab">
                            <%--<h1 style="color: #f40f0f; padding-top: 50px; padding-bottom: 0; font-family: 'Raleway',sans-serif; font-size: 62px; font-weight: 800; line-height: 72px; margin: 0 0 24px; text-align: center; text-transform: uppercase;">Billing Officer
                            </h1>--%>

                            <%--<div class="gridview-container">--%>
                            <asp:GridView Style="width: 100%; word-break: break-all; table-layout: fixed" ID="gridView3" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                                DataKeyNames="emp_id" AllowPaging="False" CellPadding="20" GridLines="None">
                                <AlternatingRowStyle BackColor="white" ForeColor="Black" />

                                <Columns>
                                    <asp:BoundField DataField="emp_id" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="emp_id" ItemStyle-Width="100px">
                                        <ItemStyle Width="100px" Wrap="true" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="emp_fname" HeaderText="First Name" SortExpression="emp_fname" ItemStyle-Width="150px">
                                        <ItemStyle Width="150px" Wrap="true" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="emp_mname" HeaderText="M.I" SortExpression="emp_mname" ItemStyle-Width="100px">
                                        <ItemStyle Width="100px" Wrap="true" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="emp_lname" HeaderText="Last Name" SortExpression="emp_lname" ItemStyle-Width="150px">
                                        <ItemStyle Width="150px" Wrap="true" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="emp_contact" HeaderText="Contact" SortExpression="emp_contact" ItemStyle-Width="100px">
                                        <ItemStyle Width="100px" Wrap="true" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="emp_email" HeaderText="Email" SortExpression="emp_email" ItemStyle-Width="200px">
                                        <ItemStyle Width="200px" Wrap="true" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="emp_created_at" HeaderText="Created At" SortExpression="emp_created_at" ItemStyle-Width="150px">
                                        <ItemStyle Width="150px" Wrap="true" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="emp_updated_at" HeaderText="Updated At" SortExpression="emp_updated_at" ItemStyle-Width="150px">
                                        <ItemStyle Width="150px" Wrap="true" />
                                    </asp:BoundField>

                                    <asp:TemplateField HeaderText="Status">
                                        <ItemTemplate>
                                            <asp:Button Style="font-size: 10px; color: orangered; font-weight: bold;" ID="btnUnsuspend" runat="server" Text='<%# Eval("emp_status") + " ▼"%>'
                                                OnClick="Unsuspend_Click"
                                                OnClientClick="return confirm('Are you sure you want to Unsuspend this account manager?');"
                                                Visible='<%# Eval("emp_status").ToString() == "Suspend" %>' CommandArgument='<%# Eval("emp_id") %>' />
                                            <asp:Label ID="Label9" runat="server" Text='<%# Eval("emp_status")%>' Visible='<%# Eval("emp_status").ToString() == "Inactive" %>' />
                                            <asp:Button Style="font-size: 10px; color: lawngreen; font-weight: bold;" ID="btnSuspend" runat="server" Text='<%# Eval("emp_status") + " ▲"%>'
                                                OnClick="Suspend_Click"
                                                OnClientClick="return confirm('Are you sure you want to Suspend this account manager?');"
                                                Visible='<%# Eval("emp_status").ToString() == "Active" %>' CommandArgument='<%# Eval("emp_id") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="update" runat="server" OnClick="Update_Click" CommandArgument='<%# Eval("emp_id") %>'>
                                                <asp:Image ID="imgEdit" runat="server" ImageUrl="~/Pictures/editlogo.png" Width="35%" Height="35%" Style="margin-right: 10px" AlternateText="Edit" />
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="Remove" runat="server" OnClick="Remove_Click" CommandArgument='<%# Eval("emp_id") %>' OnClientClick="return confirm('Are you sure you want to remove this account manager?');">
                                                <asp:Image ID="Image1" runat="server" ImageUrl="~/Pictures/removeBtn.png" Width="35%" Height="35%" AlternateText="Remove" />
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
                        </div>
                        <div class="tab-pane fade" id="od" role="tabpanel" aria-labelledby="od-tab">

                            <%--<h1 style="color: #f40f0f; padding-top: 50px; padding-bottom: 0; font-family: 'Raleway',sans-serif; font-size: 62px; font-weight: 800; line-height: 72px; margin: 0 0 24px; text-align: center; text-transform: uppercase;">Operational Dispatcher
                            </h1>--%>

                            <%--<div class="gridview-container">--%>
                            <asp:GridView Style="width: 100%; word-break: break-all; table-layout: fixed" ID="gridView4" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                                DataKeyNames="emp_id" AllowPaging="False" CellPadding="20" GridLines="None">
                                <AlternatingRowStyle BackColor="white" ForeColor="Black" />

                                <Columns>
                                    <asp:BoundField DataField="emp_id" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="emp_id" ItemStyle-Width="100px">
                                        <ItemStyle Width="100px" Wrap="true" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="emp_fname" HeaderText="First Name" SortExpression="emp_fname" ItemStyle-Width="150px">
                                        <ItemStyle Width="150px" Wrap="true" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="emp_mname" HeaderText="M.I" SortExpression="emp_mname" ItemStyle-Width="100px">
                                        <ItemStyle Width="100px" Wrap="true" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="emp_lname" HeaderText="Last Name" SortExpression="emp_lname" ItemStyle-Width="150px">
                                        <ItemStyle Width="150px" Wrap="true" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="emp_contact" HeaderText="Contact" SortExpression="emp_contact" ItemStyle-Width="100px">
                                        <ItemStyle Width="100px" Wrap="true" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="emp_email" HeaderText="Email" SortExpression="emp_email" ItemStyle-Width="200px">
                                        <ItemStyle Width="200px" Wrap="true" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="emp_created_at" HeaderText="Created At" SortExpression="emp_created_at" ItemStyle-Width="150px">
                                        <ItemStyle Width="150px" Wrap="true" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="emp_updated_at" HeaderText="Updated At" SortExpression="emp_updated_at" ItemStyle-Width="150px">
                                        <ItemStyle Width="150px" Wrap="true" />
                                    </asp:BoundField>

                                    <asp:TemplateField HeaderText="Status">
                                        <ItemTemplate>
                                            <asp:Button Style="font-size: 10px; color: orangered; font-weight: bold;" ID="btnUnsuspend" runat="server" Text='<%# Eval("emp_status") + " ▼"%>'
                                                OnClick="Unsuspend_Click"
                                                OnClientClick="return confirm('Are you sure you want to Unsuspend this account manager?');"
                                                Visible='<%# Eval("emp_status").ToString() == "Suspend" %>' CommandArgument='<%# Eval("emp_id") %>' />
                                            <asp:Label ID="Label9" runat="server" Text='<%# Eval("emp_status")%>' Visible='<%# Eval("emp_status").ToString() == "Inactive" %>' />
                                            <asp:Button Style="font-size: 10px; color: lawngreen; font-weight: bold;" ID="btnSuspend" runat="server" Text='<%# Eval("emp_status") + " ▲"%>'
                                                OnClick="Suspend_Click"
                                                OnClientClick="return confirm('Are you sure you want to Suspend this account manager?');"
                                                Visible='<%# Eval("emp_status").ToString() == "Active" %>' CommandArgument='<%# Eval("emp_id") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="update" runat="server" OnClick="Update_Click" CommandArgument='<%# Eval("emp_id") %>'>
                                                <asp:Image ID="imgEdit" runat="server" ImageUrl="~/Pictures/editlogo.png" Width="35%" Height="35%" Style="margin-right: 10px" AlternateText="Edit" />
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="Remove" runat="server" OnClick="Remove_Click" CommandArgument='<%# Eval("emp_id") %>' OnClientClick="return confirm('Are you sure you want to remove this account manager?');">
                                                <asp:Image ID="Image1" runat="server" ImageUrl="~/Pictures/removeBtn.png" Width="35%" Height="35%" AlternateText="Remove" />
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
                            <%-- </div>--%>
                        </div>

                        <div class="tab-pane fade" id="hauler" role="tabpanel" aria-labelledby="hauler-tab">

                            <%--<h1 style="color: #f40f0f; padding-top: 50px; padding-bottom: 0; font-family: 'Raleway',sans-serif; font-size: 62px; font-weight: 800; line-height: 72px; margin: 0 0 24px; text-align: center; text-transform: uppercase;">Operational Dispatcher
    </h1>--%>

                            <%--<div class="gridview-container">--%>
                            <asp:GridView Style="width: 100%; word-break: break-all; table-layout: fixed" ID="gridView5" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                                DataKeyNames="emp_id" AllowPaging="False" CellPadding="20" GridLines="None">
                                <AlternatingRowStyle BackColor="white" ForeColor="Black" />

                                <Columns>
                                    <asp:BoundField DataField="emp_id" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="emp_id" ItemStyle-Width="100px">
                                        <ItemStyle Width="100px" Wrap="true" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="emp_fname" HeaderText="First Name" SortExpression="emp_fname" ItemStyle-Width="150px">
                                        <ItemStyle Width="150px" Wrap="true" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="emp_mname" HeaderText="M.I" SortExpression="emp_mname" ItemStyle-Width="100px">
                                        <ItemStyle Width="100px" Wrap="true" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="emp_lname" HeaderText="Last Name" SortExpression="emp_lname" ItemStyle-Width="150px">
                                        <ItemStyle Width="150px" Wrap="true" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="emp_contact" HeaderText="Contact" SortExpression="emp_contact" ItemStyle-Width="100px">
                                        <ItemStyle Width="100px" Wrap="true" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="emp_email" HeaderText="Email" SortExpression="emp_email" ItemStyle-Width="200px">
                                        <ItemStyle Width="200px" Wrap="true" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="emp_created_at" HeaderText="Created At" SortExpression="emp_created_at" ItemStyle-Width="150px">
                                        <ItemStyle Width="150px" Wrap="true" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="emp_updated_at" HeaderText="Updated At" SortExpression="emp_updated_at" ItemStyle-Width="150px">
                                        <ItemStyle Width="150px" Wrap="true" />
                                    </asp:BoundField>

                                    <asp:TemplateField HeaderText="Status">
                                        <ItemTemplate>
                                            <asp:Button Style="font-size: 10px; color: orangered; font-weight: bold;" ID="btnUnsuspend" runat="server" Text='<%# Eval("emp_status") + " ▼"%>'
                                                OnClick="Unsuspend_Click"
                                                OnClientClick="return confirm('Are you sure you want to Unsuspend this account manager?');"
                                                Visible='<%# Eval("emp_status").ToString() == "Suspend" %>' CommandArgument='<%# Eval("emp_id") %>' />
                                            <asp:Label ID="Label9" runat="server" Text='<%# Eval("emp_status")%>' Visible='<%# Eval("emp_status").ToString() == "Inactive" %>' />
                                            <asp:Button Style="font-size: 10px; color: lawngreen; font-weight: bold;" ID="btnSuspend" runat="server" Text='<%# Eval("emp_status") + " ▲"%>'
                                                OnClick="Suspend_Click"
                                                OnClientClick="return confirm('Are you sure you want to Suspend this account manager?');"
                                                Visible='<%# Eval("emp_status").ToString() == "Active" %>' CommandArgument='<%# Eval("emp_id") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="update" runat="server" OnClick="Update_Click" CommandArgument='<%# Eval("emp_id") %>'>
                                                <asp:Image ID="imgEdit" runat="server" ImageUrl="~/Pictures/editlogo.png" Width="35%" Height="35%" Style="margin-right: 10px" AlternateText="Edit" />
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="Remove" runat="server" OnClick="Remove_Click" CommandArgument='<%# Eval("emp_id") %>' OnClientClick="return confirm('Are you sure you want to remove this account manager?');">
                                                <asp:Image ID="Image1" runat="server" ImageUrl="~/Pictures/removeBtn.png" Width="35%" Height="35%" AlternateText="Remove" />
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
                        </div>


                    </div>

                </section>

                <%--END TABS DESIGN GRIDVIEW--%>









                <%--EDIT BUTTON--%>
                <%--<div class="gridview-container">
                    <asp:GridView Style="width: 100%; word-break: break-all; table-layout: fixed" ID="gridViewAccountMan" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                        DataKeyNames="emp_id" AllowPaging="False" CellPadding="20" GridLines="None">
                        <AlternatingRowStyle BackColor="white" ForeColor="Black" />

                        <Columns>
                            <asp:BoundField DataField="emp_id" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="emp_id" ItemStyle-Width="100px">
                                <ItemStyle Width="100px" Wrap="true" />
                            </asp:BoundField>
                            <asp:BoundField DataField="emp_fname" HeaderText="First Name" SortExpression="emp_fname" ItemStyle-Width="150px">
                                <ItemStyle Width="150px" Wrap="true" />
                            </asp:BoundField>
                            <asp:BoundField DataField="emp_mname" HeaderText="M.I" SortExpression="emp_mname" ItemStyle-Width="100px">
                                <ItemStyle Width="100px" Wrap="true" />
                            </asp:BoundField>
                            <asp:BoundField DataField="emp_lname" HeaderText="Last Name" SortExpression="emp_lname" ItemStyle-Width="150px">
                                <ItemStyle Width="150px" Wrap="true" />
                            </asp:BoundField>
                            <asp:BoundField DataField="emp_contact" HeaderText="Contact" SortExpression="emp_contact" ItemStyle-Width="100px">
                                <ItemStyle Width="100px" Wrap="true" />
                            </asp:BoundField>
                            <asp:BoundField DataField="emp_email" HeaderText="Email" SortExpression="emp_email" ItemStyle-Width="200px">
                                <ItemStyle Width="200px" Wrap="true" />
                            </asp:BoundField>
                            <asp:BoundField DataField="emp_created_at" HeaderText="Created At" SortExpression="emp_created_at" ItemStyle-Width="150px">
                                <ItemStyle Width="150px" Wrap="true" />
                            </asp:BoundField>
                            <asp:BoundField DataField="emp_updated_at" HeaderText="Updated At" SortExpression="emp_updated_at" ItemStyle-Width="150px">
                                <ItemStyle Width="150px" Wrap="true" />
                            </asp:BoundField>

                            <asp:TemplateField HeaderText="Status">
                                <ItemTemplate>
                                    <asp:Button Style="font-size: 10px; color: orangered; font-weight: bold;" ID="btnUnsuspend" runat="server" Text='<%# Eval("emp_status") + " ▼"%>'
                                        OnClick="Unsuspend_Click"
                                        OnClientClick="return confirm('Are you sure you want to Unsuspend this account manager?');"
                                        Visible='<%# Eval("emp_status").ToString() == "Suspend" %>' CommandArgument='<%# Eval("emp_id") %>' />
                                    <asp:Label ID="Label9" runat="server" Text='<%# Eval("emp_status")%>' Visible='<%# Eval("emp_status").ToString() == "Inactive" %>' />
                                    <asp:Button Style="font-size: 10px; color: lawngreen; font-weight: bold;" ID="btnSuspend" runat="server" Text='<%# Eval("emp_status") + " ▲"%>'
                                        OnClick="Suspend_Click"
                                        OnClientClick="return confirm('Are you sure you want to Suspend this account manager?');"
                                        Visible='<%# Eval("emp_status").ToString() == "Active" %>' CommandArgument='<%# Eval("emp_id") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <asp:LinkButton ID="update" runat="server" OnClick="Update_Click" CommandArgument='<%# Eval("emp_id") %>'>
                                        <asp:Image ID="imgEdit" runat="server" ImageUrl="~/Pictures/editlogo.png" Width="35%" Height="35%" Style="margin-right: 10px" AlternateText="Edit" />
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="Remove" runat="server" OnClick="Remove_Click" CommandArgument='<%# Eval("emp_id") %>' OnClientClick="return confirm('Are you sure you want to remove this account manager?');">
                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/Pictures/removeBtn.png" Width="35%" Height="35%" AlternateText="Remove" />
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
                </div>--%>





                <%--<div class="gridview-container">
    <asp:GridView ID="gridViewAccountMan" runat="server" AutoGenerateColumns="False" AllowPaging="False" GridLines="None" CssClass="table datatable">
        <HeaderStyle CssClass="thead-dark" />
        <RowStyle CssClass="table-row" />
        <AlternatingRowStyle CssClass="table-row-striped" />

        <Columns>
            <asp:BoundField DataField="acc_fname" SortExpression="acc_fname" ItemStyle-CssClass="column" />
            <asp:BoundField DataField="acc_contact" HeaderText="Contact" SortExpression="acc_contact" ItemStyle-CssClass="column" />
            <asp:BoundField DataField="acc_email" HeaderText="Email" SortExpression="acc_email" ItemStyle-CssClass="column" />
            <asp:BoundField DataField="acc_created_at" HeaderText="Created At" SortExpression="acc_created_at" ItemStyle-CssClass="column" />
            <asp:BoundField DataField="acc_status" HeaderText="Status" ItemStyle-CssClass="column" />
        </Columns>
    </asp:GridView>
</div>--%>








                <%--                </section>--%>






                <asp:LinkButton ID="LinkButton1" runat="server"></asp:LinkButton>
                <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

                <!-- Centering the panel and enhancing its look -->
                <div class="container" style="height: 100vh; display: flex; justify-content: center; align-items: center;">
                    <!-- First Form (Update Info) -->
                    <asp:Panel ID="updatePanel" CssClass="card shadow-lg scrollable-panel" runat="server" Style="max-width: 600px; background-color: #052507; border: 1px solid aquamarine; border-radius: 8px;">
                        <contenttemplate>
                            <div class="card bg-light" style="background-color: #052507;">
                                <!-- Header Section -->
                                <div class="card-header text-center" style="background-color: #052507; color: aquamarine;">
                                    <h4 class="mb-0">Update Information</h4>
                                </div>

                                <!-- Body Section -->
                                <div class="card-body" style="background-color: #052507;">
                                    <div class="row">
                                        <!-- ID (Disabled) -->
                                        <div class="col-12 mb-3">
                                            <div class="input-group input-group-sm">
                                                <div class="input-group-prepend">
                                                    <span class="input-group-text">ID</span>
                                                </div>
                                                <asp:TextBox ID="txtbxID" runat="server" CssClass="form-control" ClientIDMode="Static" Enabled="false"></asp:TextBox>
                                            </div>
                                        </div>

                                        <!-- Firstname -->
                                        <div class="col-6 mb-3">
                                            <asp:Label ID="Label17" runat="server" Text="Firstname" Style="color: chartreuse"></asp:Label>

                                            <asp:TextBox ID="txtbfirstname" runat="server" CssClass="form-control" ClientIDMode="Static" onkeyup="validateUpdateFirstname()"></asp:TextBox>
                                            <div class="valid-feedback">Looks good!</div>
                                            <div class="invalid-feedback">Please provide a valid firstname.</div>
                                        </div>

                                        <!-- Lastname -->
                                        <div class="col-6 mb-3">
                                            <asp:Label ID="Label16" runat="server" Text="Lastname" Style="color: chartreuse"></asp:Label>

                                            <asp:TextBox ID="txtLastname" runat="server" CssClass="form-control" ClientIDMode="Static" onkeyup="validateUpdateLastname()"></asp:TextBox>
                                            <div class="valid-feedback">Looks good!</div>
                                            <div class="invalid-feedback">Please provide a valid lastname.</div>
                                        </div>

                                        <!-- Middle Initial (Optional) -->
                                        <div class="col-6 mb-3">
                                            <asp:Label ID="Label14" runat="server" Text="Middle Initial" Style="color: chartreuse"></asp:Label>

                                            <asp:TextBox ID="txtmi" runat="server" CssClass="form-control" ClientIDMode="Static" MaxLength="1" onkeypress="return isLetter(event)"></asp:TextBox>
                                            <div class="valid-feedback">Looks good!</div>
                                            <div class="invalid-feedback">Please provide a valid middle initial (optional).</div>

                                        </div>

                                        <!-- Contact -->
                                        <div class="col-6 mb-3">
                                            <asp:Label ID="Label15" runat="server" Text="Contact No." Style="color: chartreuse"></asp:Label>

                                            <asp:TextBox ID="txtContact" runat="server" CssClass="form-control" ClientIDMode="Static" onkeyup="validateUpdateContact()"></asp:TextBox>
                                            <div class="valid-feedback">Looks good!</div>
                                            <div class="invalid-feedback">Please provide a valid Philippines contact number (e.g., 09XXXXXXXXX).</div>
                                        </div>
                                    </div>

                                    <!-- Dropdown (Role) -->
                                    <div class="col-6 mb-3">
                                        <asp:Label ID="Label21" runat="server" Text="Change Role" for="inputText" Style="color: chartreuse"></asp:Label>
                                        <asp:DropDownList ID="promoteddl" class="form-select" aria-label="Default select example" runat="server" onChange="validateRole()">
                                        </asp:DropDownList>
                                        <div class="invalid-feedback">Please select a role.</div>
                                    </div>


                                    <!-- Email -->
                                    <div class="col-12 mb-3">
                                        <div class="mb-3">
                                            <%--<div class="input-group-prepend">
                                <span class="input-group-text">Email</span>
                            </div>--%>
                                            <asp:Label ID="Label18" runat="server" Text="Email" Style="color: chartreuse"></asp:Label>

                                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" ClientIDMode="Static" onkeyup="validateUpdateEmail()"></asp:TextBox>
                                            <div class="valid-feedback">Looks good!</div>
                                            <div class="invalid-feedback">Please provide a valid email address.</div>
                                        </div>
                                    </div>

                                    <!-- Password -->
                                    <div class="col-12 mb-3">

                                        <asp:Label ID="Label19" runat="server" Text="Password" Style="color: chartreuse"></asp:Label>
                                        <div class="input-group">
                                            <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control mt-2" type="password" onkeyup="validateUpdatePassword()"></asp:TextBox>
                                            <span class="input-group-text mt-2" style="cursor: pointer;" onclick="toggleUpdatePassword()">
                                                <i id="updatePasswordIcon" class="fas fa-eye-slash"></i>
                                            </span>
                                        </div>
                                        <div class="invalid-feedback">Password must be at least 6 characters long and contain at least one uppercase letter, one number, and one special character.</div>
                                    </div>

                                    <!-- Picture Upload -->
                                    <div class="col-12 mb-3">
                                        <asp:Label ID="Label20" runat="server" Text="Picture" Style="color: chartreuse"></asp:Label>
                                        <asp:FileUpload ID="FileUpload1" accept="image/*" class="form-control" runat="server" OnChange="previewImageUpdate();" />
                                        <asp:Image ID="imagePreviewUpdate" runat="server" AlternateText="Image Preview" CssClass="img-thumbnail mt-2" Style="max-width: 150px;" />
                                        <span id="fileErrorUpdate" class="text-danger" style="display: none;">File uploaded is not an image.</span>
                                    </div>
                                </div>
                            </div>

                            <!-- Footer Section -->
                            <div class="card-footer text-center" style="background-color: #052507; color: aquamarine;">
                                <asp:Button ID="btncancel" CssClass="btn btn-secondary" runat="server" Text="Cancel" />
                                <asp:Button ID="btnUpdate" CssClass="btn btn-primary" runat="server" Text="Update" OnClick="UpdateAdminInfo" OnClientClick="return confirm('Are you sure you want to update category?');" />
                            </div>

                        </contenttemplate>
                    </asp:Panel>
                </div>




                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" CancelControlID="btncancel" PopupControlID="updatePanel" TargetControlID="LinkButton1" BackgroundCssClass="Background" DropShadow="True"></ajaxToolkit:ModalPopupExtender>

                <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>

                <script>
                    // Validation Functions for Add Account Manager
                    function validateFirstname() {

                        const firstname = document.getElementById('<%= emp_firstname.ClientID %>');

                        // Allow alphabets, spaces, and hyphens for multiple-word first names
                        if (!/^[A-Za-z]+(?:[\s-][A-Za-z]+)*$/.test(firstname.value)) {
                            firstname.classList.add('is-invalid');
                            firstname.classList.remove('is-valid');
                        } else {
                            firstname.classList.remove('is-invalid');
                            firstname.classList.add('is-valid');
                        }
                    }

                    // Validate Middle Initial (Required)
                    function validateMiddleInitialRequired() {
                        const middleInitial = document.getElementById('<%= emp_mi.ClientID %>');

                        // Validate the input
                        if (middleInitial.value.length > 1 || !/^[A-Za-z]$/.test(middleInitial.value)) {
                            middleInitial.classList.add('is-invalid');
                            middleInitial.classList.remove('is-valid');
                        } else {
                            middleInitial.classList.remove('is-invalid');
                            middleInitial.classList.add('is-valid');
                        }
                    }

                    function validateLastname() {
                        const lastname = document.getElementById('<%= emp_lastname.ClientID %>');
                        if (!/^[A-Za-z]+$/.test(lastname.value)) {
                            lastname.classList.add('is-invalid');
                            lastname.classList.remove('is-valid');
                        } else {
                            lastname.classList.remove('is-invalid');
                            lastname.classList.add('is-valid');
                        }
                    }

                    function validateEmail() {
                        const email = document.getElementById('<%= emp_email.ClientID %>');
                        const emailPattern = /^[a-zA-Z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,}$/;
                        if (!emailPattern.test(email.value)) {
                            email.classList.add('is-invalid');
                            email.classList.remove('is-valid');
                        } else {
                            email.classList.remove('is-invalid');
                            email.classList.add('is-valid');
                        }
                    }

          <%--function validateContact() {
              const contact = document.getElementById('<%= emp_contact.ClientID %>');
              const contactPattern = /^09\d{9}$/;
              if (!contactPattern.test(contact.value)) {
                  contact.classList.add('is-invalid');
                  contact.classList.remove('is-valid');
              } else {
                  contact.classList.remove('is-invalid');
                  contact.classList.add('is-valid');
              }
          }--%>
                    function validateContact() {
                        const contact = document.getElementById('<%= emp_contact.ClientID %>');

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


                    function validateAddress() {
                        const address = document.getElementById('<%= emp_address.ClientID %>');
                        if (address.value.trim() === "") {
                            address.classList.add('is-invalid');
                            address.classList.remove('is-valid');
                        } else {
                            address.classList.remove('is-invalid');
                            address.classList.add('is-valid');
                        }
                    }

                    function validatePassword() {
                        const password = document.getElementById('<%= emp_pass.ClientID %>');
                        const passwordPattern = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,}$/;
                        if (!passwordPattern.test(password.value)) {
                            password.classList.add('is-invalid');
                            password.classList.remove('is-valid');
                        } else {
                            password.classList.remove('is-invalid');
                            password.classList.add('is-valid');
                        }
                    }
                    function togglePasswords() {
                        const password = document.getElementById('<%= emp_pass.ClientID %>');
                        const passIcon = document.getElementById('passIcon');
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

                    function validateRole() {
                        const role = document.getElementById('<%= emp_role.ClientID %>');
                        if (role.value === "") {
                            role.classList.add('is-invalid');
                            role.classList.remove('is-valid');
                        } else {
                            role.classList.remove('is-invalid');
                            role.classList.add('is-valid');
                        }
                    }

                    function previewImage1() {
                        const fileInput = document.getElementById('<%= formFile.ClientID %>');
                        const imagePreview = document.getElementById('<%= imagePreview.ClientID %>');
                        const file = fileInput.files[0];
                        const allowedExtensions = ["image/jpeg", "image/jpg", "image/png", "image/gif"];
                        const fileError = document.getElementById('fileError');

                        if (file && allowedExtensions.includes(file.type)) {
                            const reader = new FileReader();
                            reader.onload = function (e) {
                                imagePreview.src = e.target.result;
                            };
                            reader.readAsDataURL(file);
                            fileError.style.display = "none";
                        } else {
                            imagePreview.src = "Pictures/blank_prof.png";
                            fileError.style.display = "block";
                            alert("Invalid file. Please upload a valid image.");
                        }
                    }

                    // Validation Functions for Update Information
                    function validateUpdateFirstname() {
                        const firstname = document.getElementById('<%= txtbfirstname.ClientID %>');

                        // Allow alphabets, spaces, and hyphens for multiple-word first names
                        if (!/^[A-Za-z]+(?:[\s-][A-Za-z]+)*$/.test(firstname.value)) {
                            firstname.classList.add('is-invalid');
                            firstname.classList.remove('is-valid');
                        } else {
                            firstname.classList.remove('is-invalid');
                            firstname.classList.add('is-valid');
                        }
                    }

                    // Function to allow only letter input
                    function isLetter(event) {
                        const char = String.fromCharCode(event.which);
                        return /^[A-Za-z]$/.test(char);
                    }

                    // Validate Middle Initial (Optional)
                    function validateMiddleInitialOptional() {
                        const middleInitial = document.getElementById('<%= txtmi.ClientID %>');

                        // Validate the input
                        if (middleInitial.value.length > 1 || !/^[A-Za-z]$/.test(middleInitial.value)) {
                            middleInitial.classList.add('is-invalid');
                            middleInitial.classList.remove('is-valid');
                        } else {
                            middleInitial.classList.remove('is-invalid');
                            middleInitial.classList.add('is-valid');
                        }
                    }

                    function validateUpdateLastname() {
                        const lastname = document.getElementById('<%=txtLastname.ClientID%>');
                        if (!/^[A-Za-z]+$/.test(lastname.value)) {
                            lastname.classList.add('is-invalid');
                            lastname.classList.remove('is-valid');
                        } else {
                            lastname.classList.remove('is-invalid');
                            lastname.classList.add('is-valid');
                        }
                    }

                    function validateUpdateEmail() {
                        const email = document.getElementById('<%=txtEmail.ClientID%>');
                        const emailPattern = /^[a-zA-Z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,}$/;
                        if (!emailPattern.test(email.value)) {
                            email.classList.add('is-invalid');
                            email.classList.remove('is-valid');
                        } else {
                            email.classList.remove('is-invalid');
                            email.classList.add('is-valid');
                        }
                    }

    <%--function validateUpdateContact() {
        const contact = document.getElementById('<%=txtContact.ClientID%>');
        const contactPattern = /^09\d{9}$/;
        if (!contactPattern.test(contact.value)) {
            contact.classList.add('is-invalid');
            contact.classList.remove('is-valid');
        } else {
            contact.classList.remove('is-invalid');
            contact.classList.add('is-valid');
        }
    }--%>

                    function validateUpdateContact() {
                        const contactInput = document.getElementById('<%= txtContact.ClientID %>');

                        // Only allow digits and limit to 11 characters
                        contactInput.value = contactInput.value.replace(/\D/g, ''); // Remove non-numeric characters
                        if (contactInput.value.length > 11) {
                            contactInput.value = contactInput.value.substring(0, 11); // Limit to 11 digits
                        }

                        // Validation feedback for contact number
                        const contactPattern = /^09\d{9}$/; // Philippines phone numbers starting with 09 followed by 9 digits
                        if (!contactPattern.test(contactInput.value)) {
                            contactInput.classList.add('is-invalid');
                            contactInput.classList.remove('is-valid');
                        } else {
                            contactInput.classList.remove('is-invalid');
                            contactInput.classList.add('is-valid');
                        }
                    }


                    function validateUpdatePassword() {
                        const password = document.getElementById('<%= TextBox1.ClientID %>');
                        const passwordPattern = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,}$/;
                        if (!passwordPattern.test(password.value)) {
                            password.classList.add('is-invalid');
                            password.classList.remove('is-valid');
                        } else {
                            password.classList.remove('is-invalid');
                            password.classList.add('is-valid');
                        }
                    }

                    function toggleUpdatePassword() {
                        const passwordField = document.getElementById('<%= TextBox1.ClientID %>');
                        const icon = document.getElementById('updatePasswordIcon');

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



                    function previewImageUpdate() {
                        const fileInput = document.getElementById('<%=FileUpload1.ClientID%>');
                        const imagePreview = document.getElementById('<%=imagePreviewUpdate.ClientID%>');
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
                    }


                    // Validate entire Update form
                    function validateUpdateForm() {
                        let isValid = true;

                        validateUpdateFirstname();
                        validateUpdateLastname();
                        validateUpdateEmail();
                        validateUpdateContact();
                        /*validateUpdatePassword();*/
                        previewImageUpdate();

                        if (document.querySelectorAll('.is-invalid').length > 0) {
                            isValid = false;
                            alert("Please fill in all required fields correctly!");
                        }

                        return isValid;
                    }


                    // Form submission handlers
                    document.getElementById('submitBtn').onclick = function (event) {
                        if (!validateForm()) {
                            event.preventDefault();
                        }
                    };

                    document.getElementById('btnUpdate').onclick = function (event) {
                        if (!validateUpdateForm()) {
                            event.preventDefault();
                        }
                    };



                    /*modal to not reload*/
                    document.addEventListener("DOMContentLoaded", function () {
                        // Check if a tab was previously selected and set it as active
                        const activeTab = localStorage.getItem("activeTab");
                        if (activeTab) {
                            const tab = document.querySelector(activeTab);
                            if (tab) {
                                // Deactivate all tabs and then activate the one stored in localStorage
                                document.querySelectorAll(".nav-link").forEach(link => {
                                    link.classList.remove("active");
                                    link.setAttribute("aria-selected", "false");
                                });
                                tab.classList.add("active");
                                tab.setAttribute("aria-selected", "true");

                                // Show the associated tab pane
                                const tabContentId = tab.getAttribute("data-bs-target");
                                document.querySelectorAll(".tab-pane").forEach(pane => {
                                    pane.classList.remove("show", "active");
                                });
                                document.querySelector(tabContentId).classList.add("show", "active");
                            }
                        }

                        // Add event listeners to tabs to save the selected tab
                        document.querySelectorAll(".nav-link").forEach(tab => {
                            tab.addEventListener("click", function () {
                                localStorage.setItem("activeTab", "#" + this.id);
                            });
                        });
                    });

                    // Close the modal and retain the active tab
                    $('#exampleModal').on('hidden.bs.modal', function () {
                        const activeTab = localStorage.getItem("activeTab");
                        if (activeTab) {
                            const tab = document.querySelector(activeTab);
                            if (tab) {
                                tab.click(); // Click the active tab to switch back
                            }
                        }
                    });


                </script>





                <style>
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
                </style>

                <script src="https://code.jquery.com/jquery-3.6.4.min.js"></script>


                <!-- End General Form Elements -->
            </main>
            <!-- End #main -->

            <!-- ======= Footer ======= -->
            <footer id="footer" class="footer" style="border-top-color: chartreuse">
                <div class="copyright" style="color: #d4f3cf">
                    &copy; Copyright <strong><span style="color: #d4f3cf">TrashTrack</span></strong>. All Rights Reserved
                </div>
            </footer>
            <!-- End Footer -->

            <a href="#" class="back-to-top d-flex align-items-center justify-content-center"><i class="bi bi-arrow-up-short"></i></a>

            <!-- Vendor JS Files -->
            <script src="assets/vendor/apexcharts/apexcharts.min.js"></script>
            <script src="assets/vendor/bootstrap/js/bootstrap.bundle.min.js"></script>
            fgen
            <script src="assets/vendor/chart.js/chart.umd.js"></script>
            <script src="assets/vendor/echarts/echarts.min.js"></script>
            <script src="assets/vendor/quill/quill.min.js"></script>
            <script src="assets/vendor/simple-datatables/simple-datatables.js"></script>
            <script src="assets/vendor/tinymce/tinymce.min.js"></script>
            <script src="assets/vendor/php-email-form/validate.js"></script>


            <%--Location Google API--%>
            <script src="https://maps.googleapis.com/maps/api/js?key=YOUR_API_KEY&libraries=places"></script>


            <!-- Template Main JS File -->
            <script src="assets/js/main.js"></script>
        </body>
    </div>

</form>
</html>
