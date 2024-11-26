<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AM_Reports.aspx.cs" Inherits="Capstone.AM_Reports" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8">
    <meta content="width=device-width, initial-scale=1.0" name="viewport">

    <title>Billing</title>
    <meta content="" name="description">
    <meta content="" name="keywords">

    <%--draggable panel--%>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.min.js"></script>
    <link href="https://code.jquery.com/ui/1.12.1/themes/smoothness/jquery-ui.css" rel="stylesheet" />


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
        /*Container Styles */
        .gridview-container {
            max-height: 530px;
            overflow-y: auto;
            position: relative;
        }

            /* WebKit Scrollbar Styles */
            .gridview-container::-webkit-scrollbar {
                width: 12px; /* Width of scrollbar */
            }

            .gridview-container::-webkit-scrollbar-thumb {
    background: linear-gradient(180deg, chartreuse, aquamarine); /* Gradient from chartreuse to violet to red */
    border-radius: 15px; /* Rounded shape */
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.3); /* Larger shadow for depth */ /* Slight shadow for depth */
    transition: background 0.3s ease, transform 0.3s ease; /* Smooth transitions */
            }

            .gridview-container::-webkit-scrollbar-track {
                /*background-color: #051c2d;*/ /* Black track for high contrast */
                background-color: transparent; /* Black track for high contrast */
                border-radius: 10px;
                /*box-shadow: inset 0 0 5px rgba(0, 0, 0, 0.2);*/ /* Subtle inner shadow for depth */
            }

            /*Animation for Hover Effect */
            .gridview-container::-webkit-scrollbar-thumb:hover {
    background: linear-gradient(180deg, aquamarine, chartreuse); /* Reverse gradient on hover */
                /*box-shadow: 0 0 9px rgba(58, 192, 164, 0.7);*/  /*Increased glow on hover */
            }

            .gridview-container::-webkit-scrollbar-track:hover {
                /*background-color: grey;*/
                background-color: transparent;
            }

            .gridview-container::-webkit-scrollbar-thumb:active {
                background-color: #134f45;
            }

            .gridview-container::-webkit-scrollbar-track:active {
                /*background-color: #051c2d;*/
                background-color: transparent; /* Black track for high contrast */
            }


        /* Full-screen centered flex container for the modal */
        .centered-modal-container {
            display: flex;
            justify-content: center;
            align-items: center;
            position: fixed;
            top: 0;
            left: 0;
            width: 100vw;
            height: 100vh;
            background-color: rgba(0, 0, 0, 0.5); /* Optional dim background */
            /*        z-index: 9999;
*/
        }

        /* Panel styling adjustments for responsive centering */
        .card {
            max-width: 820px;
            width: 100%;
            border: 2px solid #26D8A8;
            border-radius: 12px;
            box-shadow: 0 5px 15px rgba(0, 0, 0, 0.3);
        }
        /*.card {
        max-height: 90vh;*/ /* Keep it from extending past the viewport */
        /*overflow-y: auto;*/ /* Enable vertical scrolling if needed */
        /*}*/

        /*SCROLLABLE PANEL*/
        .scrollable-content {
            max-height: 400px;
            overflow-y: auto;
            padding-right: 10px; /* Optional: to prevent overflow text from hiding under the scrollbar */
        }
        /*END SCROLLABLE PANEL*/



        /* Modal animations for smooth opening and closing */
        .animate-modal {
            opacity: 0;
            transform: scale(0.95);
            transition: opacity 0.3s ease, transform 0.3s ease;
        }

        .modal.show .animate-modal {
            opacity: 1;
            transform: scale(1);
        }

        /* Soft fade effect for close buttons */
        .btn-close {
            font-size: 1.2rem;
            transition: all 0.3s ease;
        }

            .btn-close:hover {
                color: #FF6F61;
                transform: rotate(90deg);
            }

        /* Button hover effects for footer buttons */
        .btn.btn-secondary:hover, .btn.btn-primary:hover {
            opacity: 0.8;
            transform: translateY(-2px);
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


        /* Styling for GridView Tables */
        #gridViewBookings, #gridView1, #gridView2, #gridView4 {
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
            #gridViewBookings th, #gridView1 th, #gridView2 th, #gridView4 th {
                background: #66CDAA; /* Luminous green for header */
                color: white;
                font-size: 16px; /* Slightly smaller font for a refined look */
                font-weight: 600; /* Medium weight for balanced appearance */
                text-align: center;
                padding: 12px;
                border-bottom: 1px solid #ddd; /* Light bottom border */
            }

            /* GridView Row Styles */
            #gridViewBookings td, #gridView1 td, #gridView2 td, #gridView4 td {
                padding: 12px;
                font-size: 14px; /* Smaller font size */
                font-weight: 500; /* Medium weight for row text */
                color: #333; /* Darker text color */
                border-bottom: 1px solid #ddd; /* Light row separators */
                word-wrap: break-word; /* Text wraps neatly */
                text-align: center; /* Centered text alignment */
            }

            /* Hover Effect for GridView Rows */
            #gridViewBookings tr:hover, #gridView1 tr:hover, #gridView2 tr:hover, #gridView4 tr:hover {
                background: #eef8ee; /* Subtle hover effect */
            }

            /* Styling the GridView Footer */
            #gridViewBookings .FooterStyle, #gridView1 .FooterStyle, #gridView2 .FooterStyle, #gridView4 .FooterStyle {
                border-radius: 0 0 12px 12px; /* Rounded bottom corners */
                background: #66CDAA;
                color: #fff;
                text-align: center;
                padding: 12px;
            }

            /* Optional: Styling for the Status and Action Buttons */
            #gridViewBookings .btnUnsuspend, #gridView1 .btnUnsuspend, #gridView2 .btnUnsuspend, #gridView4 .btnUnsuspend,
            #gridViewBookings .btnSuspend, #gridView1 .btnSuspend, #gridView2 .btnSuspend, #gridView4 .btnSuspend {
                font-size: 10px; /* Adjusted button font size */
                border-radius: 8px; /* Slightly rounded buttons */
                padding: 5px 10px; /* Comfortable padding */
            }

            #gridViewBookings .imgEdit, #gridView1 .imgEdit, #gridView2 .imgEdit, #gridView4 .imgEdit,
            #gridViewBookings .Image1, #gridView1 .Image1, #gridView2 .Image1, #gridView4 .Image1 {
                border-radius: 8px; /* Rounded corners for images */
                margin-right: 10px; /* Margin between images */
            }

        /*/* Add this style to change the modal header and footer background color */
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

        .customGridView tr {
            border-bottom: 1px solid #0a4d1d;
        }


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

            '<%= gridView1.ClientID %>',
            '<%= gridViewBookings.ClientID %>'
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
                  <a class="nav-link" href="AM_Reports.aspx">
                      <i class="bi bi-grid"></i>
                      <span>Reports</span>
                  </a>

              </li>
          </ul>
                  </aside>
            <!-- End Sidebar-->

            <main id="main" class="main">

                <div class="pagetitle">
                    <h1 style="padding-top: 20px; color: chartreuse">Reports</h1>
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




                <section style="background-color: #052507; padding: 25px; border-radius: 8px; box-shadow: 0 0 5px rgba(0, 0, 0, .2)">
                    <div>
                        <asp:ImageMap ID="ImageMap1" runat="server" ImageUrl="Pictures//box_format.png" Style="float: right; margin-right: 0px; margin-top: 0px; width: 50px"></asp:ImageMap>
                    </div>
                    <div style="margin-top: 50px; margin-bottom: 30px">
                        <asp:TextBox Style="border-radius: 10px; padding-left: 10px; padding: 2px; margin-top: 7px; border-color: aquamarine; border-width: 3px" placeholder="Search" ID="txtSearch" runat="server" oninput="search();" AutoPostBack="false"></asp:TextBox>
                    </div>

                    <asp:HiddenField ID="hfActiveTab" runat="server" />
                    <ul class="nav nav-tabs" id="myTab" role="tablist">
                        <li class="nav-item">
                            <a class="nav-link" id="tab1-tab" data-toggle="tab" href="#tab1" role="tab" aria-controls="tab1" aria-selected="false">Booking</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" id="tab2-tab" data-toggle="tab" href="#tab2" role="tab" aria-controls="tab2" aria-selected="false">Billing</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" id="tab3-tab" data-toggle="tab" href="#tab3" role="tab" aria-controls="tab3" aria-selected="false">Payment</a>
                        </li>
                    </ul>

                    <div class="tab-content" id="myTabContent">
                        <div class="tab-pane fade" id="tab1" role="tabpanel" aria-labelledby="tab1-tab">

                            <%--BOOKING GRIDVIEW--%>
<%--                            <div class="tab-pane fade show active" id="sam" role="tabpanel" aria-labelledby="sam-tab">--%>
                            <div class="gridview-container">
                                <asp:GridView ID="gridViewBookings" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" DataKeyNames="bk_id" AllowPaging="False" CellPadding="20" Font-Size="10px" ForeColor="Black" GridLines="None">
                                    <Columns>
                                        <asp:BoundField DataField="bk_id" HeaderText="Booking ID" SortExpression="bk_id" ItemStyle-Width="100px">
                                            <ItemStyle Width="100px"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="bk_date" HeaderText="Date" SortExpression="bk_date" DataFormatString="{0:yyyy-MM-dd HH:mm}" ItemStyle-Width="150px">
                                            <ItemStyle Width="150px"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="bk_status" HeaderText="Status" SortExpression="bk_status" ItemStyle-Width="150px">
                                            <ItemStyle Width="150px"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="bk_province" HeaderText="Province" SortExpression="bk_province" ItemStyle-Width="150px">
                                            <ItemStyle Width="150px"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="bk_city" HeaderText="City" SortExpression="bk_city" ItemStyle-Width="150px">
                                            <ItemStyle Width="150px"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="bk_brgy" HeaderText="Barangay" SortExpression="bk_brgy" ItemStyle-Width="150px">
                                            <ItemStyle Width="150px"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="bk_street" HeaderText="Street" SortExpression="bk_street" ItemStyle-Width="150px">
                                            <ItemStyle Width="150px"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="bk_postal" HeaderText="Postal Code" SortExpression="bk_postal" ItemStyle-Width="100px">
                                            <ItemStyle Width="100px"></ItemStyle>
                                        </asp:BoundField>

                                        <%--<asp:TemplateField HeaderText="Generate Bill">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="update" runat="server" OnClick="openBookWaste_Click" CommandArgument='<%# Eval("bk_id") %>'>
                                                    <asp:Image ID="imgEdit" runat="server" ImageUrl="~/Pictures/editlogo.png" Width="35%" Height="35%" Style="margin-right: 10px" AlternateText="Edit" />
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>

                                    </Columns>
                                    <RowStyle BackColor="White" ForeColor="Black" BorderStyle="Solid" BorderColor="#ccc" BorderWidth="1px" />
                                    <HeaderStyle BackColor="#66CDAA" Font-Bold="True" ForeColor="black" BorderStyle="Solid" BorderColor="#66CDAA" BorderWidth="1px" />
                                </asp:GridView>
                                </div>
<%--                            </div>--%>
                        </div>

                        <div class="tab-pane fade" id="tab2" role="tabpanel" aria-labelledby="tab2-tab">
<%--                            <div class="tab-pane fade show active" id="sam1" role="tabpanel" aria-labelledby="sam-tab">--%>
                            <div class="gridview-container">
                                <asp:GridView ID="gridView1" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" DataKeyNames="gb_id" AllowPaging="False" CellPadding="20" Font-Size="10px" ForeColor="Black" GridLines="None">
                                    <Columns>
                                        <asp:BoundField DataField="gb_id" HeaderText="Transaction No." SortExpression="gb_id" ItemStyle-Width="100px">
                                            <ItemStyle Width="100px"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="gb_date_issued" HeaderText="Issued Date" SortExpression="gb_date_issued" DataFormatString="{0:yyyy-MM-dd HH:mm}" ItemStyle-Width="150px">
                                            <ItemStyle Width="150px"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="gb_date_due" HeaderText="Due Date" SortExpression="gb_date_due" DataFormatString="{0:yyyy-MM-dd HH:mm}" ItemStyle-Width="150px">
                                            <ItemStyle Width="150px"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="gb_status" HeaderText="Status" SortExpression="gb_status" ItemStyle-Width="150px">
                                            <ItemStyle Width="150px"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="bk_id" HeaderText="Book ID" SortExpression="bk_id" ItemStyle-Width="150px">
                                            <ItemStyle Width="150px"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="gb_total_amnt_interest" HeaderText="Amount Interest" SortExpression="gb_total_amnt_interest" ItemStyle-Width="150px">
                                            <ItemStyle Width="150px"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="gb_total_sales" HeaderText="Total Sales" SortExpression="gb_total_sales" ItemStyle-Width="100px">
                                            <ItemStyle Width="100px"></ItemStyle>
                                        </asp:BoundField>

                                        <%--<asp:TemplateField HeaderText="Generate Bill">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="update" runat="server" OnClick="openBookWaste_Click" CommandArgument='<%# Eval("gb_id") %>'>
                                                    <asp:Image ID="imgEdit" runat="server" ImageUrl="~/Pictures/editlogo.png" Width="35%" Height="35%" Style="margin-right: 10px" AlternateText="Edit" />
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>

                                    </Columns>
                                    <RowStyle BackColor="White" ForeColor="Black" BorderStyle="Solid" BorderColor="#ccc" BorderWidth="1px" />
                                    <HeaderStyle BackColor="#66CDAA" Font-Bold="True" ForeColor="black" BorderStyle="Solid" BorderColor="#66CDAA" BorderWidth="1px" />
                                </asp:GridView>
                                </div>
                            <%--</div>--%>
                        </div>

                                                <div class="tab-pane fade" id="tab3" role="tabpanel" aria-labelledby="tab3-tab">
<%--                            <div class="tab-pane fade show active" id="sam1" role="tabpanel" aria-labelledby="sam-tab">--%>
                            <div class="gridview-container">
                                <asp:GridView ID="gridView2" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" DataKeyNames="p_id" AllowPaging="False" CellPadding="20" Font-Size="10px" ForeColor="Black" GridLines="None">
                                    <Columns>
                                        <asp:BoundField DataField="p_id" HeaderText="Transaction No." SortExpression="p_id" ItemStyle-Width="100px">
                                            <ItemStyle Width="100px"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="p_updated_at" HeaderText="Date Paid" SortExpression="p_updated_at" DataFormatString="{0:yyyy-MM-dd HH:mm}" ItemStyle-Width="150px">
                                            <ItemStyle Width="150px"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="p_created_at" HeaderText="Date Created" SortExpression="p_created_at" DataFormatString="{0:yyyy-MM-dd HH:mm}" ItemStyle-Width="150px">
                                            <ItemStyle Width="150px"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="p_status" HeaderText="Status" SortExpression="p_status" ItemStyle-Width="150px">
                                            <ItemStyle Width="150px"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="p_amount" HeaderText="Amount" SortExpression="p_amount" ItemStyle-Width="150px">
                                            <ItemStyle Width="150px"></ItemStyle>
                                        </asp:BoundField>
                                        <asp:BoundField DataField="p_method" HeaderText="Method" SortExpression="p_method" ItemStyle-Width="150px">
                                            <ItemStyle Width="150px"></ItemStyle>
                                        </asp:BoundField>
                                    </Columns>
                                    <RowStyle BackColor="White" ForeColor="Black" BorderStyle="Solid" BorderColor="#ccc" BorderWidth="1px" />
                                    <HeaderStyle BackColor="#66CDAA" Font-Bold="True" ForeColor="black" BorderStyle="Solid" BorderColor="#66CDAA" BorderWidth="1px" />
                                </asp:GridView>
                                </div>
                            <%--</div>--%>
                        </div>
                    </div>
                </section>




                <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
                <script>
                    $(function () {
                        $("input[name='EmpId']").on('input', function (e) {
                            $(this).val($(this).val().replace(/[^0-9]/g, ''));
                        });
                    });
                    // Update the TextBoxes every second
                    setInterval(updateDateTime, 1000);

                    // Initialize with current date and time when the page loads
                    window.onload = updateDateTime;

                    $(document).ready(function () {
                        // Make the card draggable
                        $(".draggable").draggable({
                            handle: ".card-header", // Make only the header draggable
                            containment: "window" // Restrict dragging within the window
                        });

                        // To allow interaction with other elements while the modal is open
                        $(".modal").removeClass("ui-dialog").css({ position: 'fixed' });
                    });

                </script>
                <script type="text/javascript">
                    $(document).ready(function () {
                        // Set the active tab based on the value in the hidden field.
                        var activeTab = $("#<%= hfActiveTab.ClientID %>").val();
                        if (activeTab) {
                            $('#myTab a[href="' + activeTab + '"]').tab('show');
                        }

                        // Store the active tab in the hidden field whenever a tab is shown.
                        $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
                            var activeTab = $(e.target).attr("href");
                            $("#<%= hfActiveTab.ClientID %>").val(activeTab);
                        });
                    });
                </script>


                <!-- Include Bootstrap JS, Popper.js, and jQuery -->
                <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
                <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.9.2/dist/umd/popper.min.js"></script>
                <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>


                <script src="https://code.jquery.com/jquery-3.6.4.min.js"></script>
                <!-- End General Form Elements -->
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
            <link href="https://stackpath.bootstrapcdn.com/bootstrap/5.1.3/css/bootstrap.min.css" rel="stylesheet">
            <script src="https://stackpath.bootstrapcdn.com/bootstrap/5.1.3/js/bootstrap.bundle.min.js"></script>

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

