<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dispatcher_AddSlip.aspx.cs" Inherits="Capstone.Dispatcher_AddSlip" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8">
    <meta content="width=device-width, initial-scale=1.0" name="viewport">

    <title></title>
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

    <script src="https://code.jquery.com/jquery-3.6.4.min.js"></script>
    <script type="text/javascript">
        function search() {
            var searchText = document.getElementById('<%= txtSearch.ClientID %>').value.toUpperCase();
            var grid = document.getElementById('<%= gridViewBooking.ClientID %>');

            for (var i = 1; i < grid.rows.length; i++)
                var row = grid.rows[i];
            var id = row.cells[0].innerHTML.toUpperCase();
            var firstname = row.cells[1].innerHTML.toUpperCase();
            var mi = row.cells[2].innerHTML.toUpperCase();
            var lastname = row.cells[3].innerHTML.toUpperCase();
            var username = row.cells[4].innerHTML.toUpperCase();
            var contact = row.cells[5].innerHTML.toUpperCase();
            var email = row.cells[6].innerHTML.toUpperCase();
            var created_at = row.cells[7].innerHTML.toUpperCase();
            var updated_at = row.cells[8].innerHTML.toUpperCase();
            var status = row.cells[9].innerHTML.toUpperCase();
            //var date = row.cells[9].innerHTML.toUpperCase();
            //var stock_level = row.cells[10].innerHTML.toUpperCase();
            if (id.indexOf(searchText) > -1 || firstname.indexOf(searchText) > -1 || mi.indexOf(searchText) > -1 ||
                lastname.indexOf(searchText) > -1 || username.indexOf(searchText) > -1 ||
                contact.indexOf(searchText) > -1 || email.indexOf(searchText) > -1 ||
                created_at.indexOf(searchText) > -1 || updated_at.indexOf(searchText) > -1 ||
                status.indexOf(searchText) > -1) {
                row.style.display = '';
            } else {
                row.style.display = 'none';
            }
        }
    }


    </script>
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
                                <a href="Dispatcher_AddSchedule.aspx">
                                    <i class="bi bi-circle"></i><span>Manage Booking Limit</span>
                                </a>
                            </li>
                            <li>
                                <a href="Dispatcher_AddSlip.aspx" class="active">
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
                    <h1 style="padding-top: 20px; color: chartreuse">Manage Truck Scale Slip</h1>
                    <nav>
                        <ol class="breadcrumb">
                            <%--<li class="breadcrumb-item"><a href="WAREHOUSE_DASHBOARD.aspx">Dashboard</a></li>--%>
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



                    <div style="margin-top: 50px; margin-bottom: 30px">
                        <asp:TextBox Style="border-radius: 10px; padding-left: 10px; padding: 2px; margin-top: 7px; border-color: aquamarine; border-width: 3px" placeholder="Search" ID="txtSearch" runat="server" oninput="search();" AutoPostBack="false"></asp:TextBox>

                    </div>
                    <%-- GridView to add Truck Scale Slip --%>
                    <div class="gridview-container">
                        <asp:GridView Style="width: 100%; word-break: break-all; table-layout: fixed" ID="gridViewBooking" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                            DataKeyNames="bk_id" AllowPaging="False" CellPadding="20" GridLines="None">
                            <AlternatingRowStyle BackColor="white" ForeColor="Black" />

                            <Columns>
                                <asp:BoundField DataField="bk_id" HeaderText="Booking ID" InsertVisible="False" ReadOnly="True" SortExpression="bk_id" ItemStyle-Width="100px">
                                    <ItemStyle Width="100px" Wrap="true" />
                                </asp:BoundField>
                                <asp:BoundField DataField="bk_date" HeaderText="Booking Date" SortExpression="bk_date" ItemStyle-Width="150px">
                                    <ItemStyle Width="150px" Wrap="true" />
                                </asp:BoundField>
                                <asp:BoundField DataField="bk_status" HeaderText="Booking Status" SortExpression="bk_status" ItemStyle-Width="150px">
                                    <ItemStyle Width="150px" Wrap="true" />
                                </asp:BoundField>
                                <%--  <asp:BoundField DataField="trck_slip" HeaderText="Truck Slip ID" SortExpression="trck_slip" ItemStyle-Width="150px">
                                    <ItemStyle Width="150px" Wrap="true" />
                                </asp:BoundField>--%>
                                <asp:BoundField DataField="cus_id" HeaderText="Customer ID" SortExpression="cus_id" ItemStyle-Width="150px">
                                    <ItemStyle Width="150px" Wrap="true" />
                                </asp:BoundField>

                                <asp:TemplateField HeaderText="View Booking Action">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="update" runat="server" OnClick="Update_Click" CommandArgument='<%# Eval("bk_id") %>'>
                                            <%--<i class="bi bi-info-circle"> View Booking</i>--%>
                                            <asp:Image ID="imgEdit" runat="server" ImageUrl="~/Pictures/editlogo.png" Width="20%" Height="20%" Style="margin-right: 10px" AlternateText="View Booking" />
                                                          
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


                </section>

                <asp:LinkButton ID="LinkButton1" runat="server"></asp:LinkButton>
                <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                <!-- First Form (Update Info) -->
                <asp:Panel ID="updatePanel" CssClass="card shadow-lg scrollable-panel" runat="server" Style="max-width: 600px; background-color: #052507; border: 1px solid aquamarine; border-radius: 8px;">
                    <contenttemplate>
                        <div class="card bg-light" style="background-color: #052507;">
                            <!-- Header Section -->
                            <div class="card-header text-center" style="background-color: #052507; color: aquamarine;">
                                <h4 class="mb-0">Update Booking Information</h4>
                            </div>
                            
                            <!-- Body Section -->
                            <div class="card-body" style="background-color: #052507;">
                                <div class="row">
                                    <!-- ID (Disabled) -->
                                    <div class="col-12 mb-3">
                                        <div class="input-group input-group-sm">
                                            <div class="input-group-prepend">
                                                <span class="input-group-text">Booking ID</span>
                                            </div>
                                            <asp:TextBox ID="txtbxID" runat="server" CssClass="form-control" ClientIDMode="Static" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>

                                    <%-- City Address --%>

                                    <div class="col-12 mb-3">
                                        <div class="input-group input-group-sm">
                                            <div class="input-group-prepend">
                                                <span class="input-group-text">City</span>
                                            </div>
                                            <asp:TextBox ID="txtCITY" runat="server" CssClass="form-control" ClientIDMode="Static" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <%-- Street Addres --%>
                                    <div class="col-12 mb-3">
                                        <div class="input-group input-group-sm">
                                            <div class="input-group-prepend">
                                                <span class="input-group-text">Street</span>
                                            </div>
                                            <asp:TextBox ID="txtSTREET" runat="server" CssClass="form-control" ClientIDMode="Static" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <%-- Barangay Address --%>
                                    <div class="col-12 mb-3">
                                        <div class="input-group input-group-sm">
                                            <div class="input-group-prepend">
                                                <span class="input-group-text">Barangay</span>
                                            </div>
                                            <asp:TextBox ID="txtBRGY" runat="server" CssClass="form-control" ClientIDMode="Static" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>
                                    <%-- Province Address --%>

                                    <div class="col-12 mb-3">
                                        <div class="input-group input-group-sm">
                                            <div class="input-group-prepend">
                                                <span class="input-group-text">Province</span>
                                            </div>
                                            <asp:TextBox ID="txtPROVINCE" runat="server" CssClass="form-control" ClientIDMode="Static" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>

                                    <%-- Postal Address --%>

                                    <div class="col-12 mb-3">
                                        <div class="input-group input-group-sm">
                                            <div class="input-group-prepend">
                                                <span class="input-group-text">Postal</span>
                                            </div>
                                            <asp:TextBox ID="txtPOSTAL" runat="server" CssClass="form-control" ClientIDMode="Static" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>

                                    <%-- Customer ID --%>

                                    <div class="col-12 mb-3">
                                        <div class="input-group input-group-sm">
                                            <div class="input-group-prepend">
                                                <span class="input-group-text">Customer ID</span>
                                            </div>
                                            <asp:TextBox ID="txtCUSTOMERID" runat="server" CssClass="form-control" ClientIDMode="Static" Enabled="false"></asp:TextBox>
                                        </div>
                                    </div>






                                    <!-- Picture Upload -->
                                    <div class="mb-3">
                                        <asp:Label ID="Label12" runat="server" Text="Picture" for="inputText" Style="color: chartreuse"></asp:Label>
                                        <asp:FileUpload ID="formFile" accept="image/*" class="form-control" runat="server" OnChange="previewImage1()" />

                                        <!-- The image thumbnail that opens a modal on click -->
                                        <!-- Trigger the Modal -->
                                        <!-- Image that will trigger the modal on click -->
                                        <!-- The image thumbnail that opens a modal on click -->
                                        <asp:Image ID="imagePreview" runat="server" AlternateText="Image Preview" CssClass="img-thumbnail mt-2" Style="max-width: 150px; cursor: pointer;" OnClick="showImageModal();" />

                                        <!-- Modal to show the full-size image -->
                                        <%--<div class="modal fade" id="imageModal" tabindex="-1" role="dialog" aria-labelledby="imageModalLabel" aria-hidden="true">
                                            <div class="modal-dialog modal-dialog-centered" role="document">
                                                <div class="modal-content">
                                                    <div class="modal-header">
                                                        <h5 class="modal-title" id="imageModalLabel">Full Image</h5>
                                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                                            <span aria-hidden="true">&times;</span>
                                                        </button>
                                                    </div>
                                                    <div class="modal-body">
                                                        <!-- Display full-size image inside modal -->
                                                        <img id="fullImage" src="" class="img-fluid" alt="Full Image" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>--%>

                                        <div class="modal fade" id="imageModal" tabindex="-1" role="dialog" aria-labelledby="imageModalLabel" aria-hidden="true">
                                            <div class="modal-dialog modal-dialog-centered" role="document">
                                                <div class="modal-content">
                                                    <div class="modal-header">
                                                        <h5 class="modal-title" id="imageModalLabel">Full Image</h5>
                                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                                            <span aria-hidden="true">&times;</span>
                                                        </button>
                                                    </div>
                                                    <div class="modal-body">
                                                        <!-- Display full-size image inside modal with updated size -->
                                                        <img id="fullImage" src="" class="img-fluid" alt="Full Image" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>



                                        <script type="text/javascript">
                                            function showImageModal() {
                                                //Original Function to display the modal in its correct size
                                                var imageSrc = document.getElementById('<%= imagePreview.ClientID %>').src;
                                                document.getElementById('fullImage').src = imageSrc; // Set full-size image src to the modal
                                                $('#imageModal').modal('show'); // Show the modal

                                                <%--var imageSrc = document.getElementById('<%= imagePreview.ClientID %>').src;
                                                var fullImage = document.getElementById('fullImage');

                                                fullImage.src = imageSrc; // Set full-size image src to the modal
                                                fullImage.style.width = '1500px';  // Set width to 1000px
                                                fullImage.style.height = '1000px'; // Set height to 1000px

                                                $('#imageModal').modal('show'); // Show the modal--%>
                                            }
                                        </script>

                                        <span id="fileError" style="color: red; display: none;">File uploaded is not an image.</span>
                                    </div>
                                </div>
                            </div>

                            <!-- Footer Section -->
                            <div class="card-footer text-center" style="background-color: #052507; color: aquamarine;">
                                <asp:Button ID="btncancel" CssClass="btn btn-secondary" runat="server" Text="Cancel" />
                                <asp:Button ID="btnAddTruckSlip" CssClass="btn btn-primary" runat="server" Text="Upload Truck Slip" OnClick="btnAddTruckSlip_Click" />
                            </div>
                    </contenttemplate>
                </asp:Panel>



                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" CancelControlID="btncancel" PopupControlID="updatePanel" TargetControlID="LinkButton1" BackgroundCssClass="Background" DropShadow="True"></ajaxToolkit:ModalPopupExtender>
    </div>








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

    <script src="https://code.jquery.com/jquery-3.6.4.min.js"></script>
    <script>
        $(document).ready(function () {
            $('#<%= gridViewBooking.ClientID %> .edit-button').on('click', function () {
                var rowIndex = $(this).closest('tr').index();
                $('#<%= gridViewBooking.ClientID %> tr').height('auto'); // Reset all rows to auto height
                $('#<%= gridViewBooking.ClientID %> tr:eq(' + rowIndex + ')').height($('#<%= gridViewBooking.ClientID %> tr:eq(' + rowIndex + ')').height());
            });
        });

    </script>

    <%--<script type="text/javascript">
        function showImageModal() {
            var imageSrc = document.getElementById('<%= imagePreview.ClientID %>').src;
            document.getElementById('fullImage').src = imageSrc; // Set full-size image src to the modal
            $('#imageModal').modal('show'); // Show the modal
        }
    </script>--%>


    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script>
        function previewImage1() {
            const fileInput = document.getElementById('<%= formFile.ClientID %>');
            const imagePreview = document.getElementById('<%= imagePreview.ClientID %>');
            const file = fileInput.files[0];
            const allowedExtensions = ["image/jpeg", "image/jpg", "image/png"];
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

                // Replace the default alert with SweetAlert
                Swal.fire({
                    title: 'Invalid File',
                    text: 'Please upload a valid image (JPEG/JPG or PNG).',
                    icon: 'error',
                    confirmButtonColor: '#3085d6'
                });
            }
        }


     <%--   function previewImageUpdate() {
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
                 imagePreview.src = "";
                 fileError.style.display = 'block';
             }
         }--%>
</script>
    </div>

</form>
</html>