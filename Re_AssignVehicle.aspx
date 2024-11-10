<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Re_AssignVehicle.aspx.cs" Inherits="Capstone.Re_AssignVehicle" %>

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
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>

    <!-- Template Main CSS File -->
    <link href="assets/css/style.css" rel="stylesheet">
   


    <style>
    .modal-overlay {
        display: flex;
        justify-content: center;
        align-items: center;
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background: rgba(0, 0, 0, 0.5); /* Semi-transparent background */
        z-index: 1000; /* Above other elements */
        display: none; /* Initially hidden, set to block when shown */
    }

    .card {
        position: relative;
        z-index: 1001; /* Ensure card is above overlay */
        /* Add max-width or width to control size */
        max-width: 600px;
        margin: 0 auto; /* Center the card horizontally */
    }
    .search-section {
    background-color: #d9f9d9; /* Light green background */
    border: 1px solid #c2e1c2; /* Slightly darker border */
    border-radius: 5px;
    padding: 10px;
}
    .form-control {
    border-radius: 4px !important;  /* Ensure rounded corners */
    padding: 10px;                 /* Same padding for all */
    height: 38px;                  /* Uniform height */
    width: 100%;                   /* Full-width for uniformity */
}


.input-group input.form-control {
    height: 50px; /* Set the desired height for the input */
}

.input-group .input-group-append {
    margin-left: 10px; /* Add some space between the input and the button */
}

.input-group .input-group-append .search-button {
    width: 60px; /* Set a smaller width for the button */
    height: 50px; /* Match the input height */
    padding: 0; /* Remove default padding */
    border: none; /* Remove border if you want a flat look */
    background-color: transparent; /* Adjust background if needed */
    cursor: pointer; /* Change cursor to pointer */
}

</style>

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
                                        <asp:Label ID="Label3" runat="server" Text="Administrator"></asp:Label></span>
                                </li>
                                <li>
                                    <hr class="dropdown-divider">
                                </li>

                              
                                <li>
                                    <hr class="dropdown-divider">
                                </li>

                                <li>
                                    <a class="dropdown-item d-flex align-items-center" href="Admin_Change_Pass.aspx">
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
                                <a href="Dispatcher_AddSlip.aspx">
                                    <i class="bi bi-circle"></i><span>Manage Booking Slip</span>
                                </a>
                            </li>
                            <li>
                                <a href="Re_AssignVehicle.aspx"  class="active">
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
                    <h1 style="padding-top: 20px; color: chartreuse; font-size: 24px;" >Operational Dispatcher</h1>
                    <nav>
                        <ol class="breadcrumb">
                            <li class="breadcrumb-item"><a href="WAREHOUSE_DASHBOARD.aspx" style="font-size: 18px;">Management</a></li>
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
                <%-- searchhh --%>
                        <div class="search-section" style="margin-bottom: 15px; text-align: left; padding-left: 0.5in;">
                            <div class="input-group" style="max-width: 400px; margin: 0;">
                                <input type="text" id="txtSearch" runat="server" class="form-control" placeholder="Search..." style="height: 50px;" />
                                <div class="input-group-append" style="margin-left: 10px;">
                                    <asp:ImageButton ID="btnSearch" runat="server"
                                        ImageUrl="~/Pictures/search.png"
                                        CommandArgument='<%# Eval("v_id") %>'
                                        OnClick="btnSearch_Click" 
                                        CssClass="search-button" />
                                </div>
                            </div>
                        </div>

                        <%-- GridView to re-assign vehicle --%>
                        <div class="gridview-container" style="max-height: 600px; overflow-y: auto;">
                            <asp:GridView Style="width: 100%; word-break: break-all; table-layout: fixed" 
                                ID="gridViewDispatcher" runat="server" 
                                AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
                                DataKeyNames="v_id" AllowPaging="False" CellPadding="20" GridLines="None">

                                <AlternatingRowStyle BackColor="white" ForeColor="Black" />

                                <Columns>
                                    <asp:BoundField DataField="v_id" HeaderText="Vehicle ID" ReadOnly="True" ItemStyle-Width="100px">
                                        <ItemStyle Width="100px" Wrap="true" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="v_plate" HeaderText="Vehicle Plate" ReadOnly="True" ItemStyle-Width="100px">
                                        <ItemStyle Width="100px" Wrap="true" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="driver_id" HeaderText="Hauler ID" ReadOnly="True" ItemStyle-Width="100px">
                                        <ItemStyle Width="100px" Wrap="true" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="emp_name" HeaderText="Hauler Name" ItemStyle-Width="150px">
                                        <ItemStyle Width="150px" Wrap="true" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="vtype_name" HeaderText="Vehicle Type" ItemStyle-Width="150px">
                                        <ItemStyle Width="150px" Wrap="true" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="v_capacity" HeaderText="Vehicle Capacity" ItemStyle-Width="100px">
                                        <ItemStyle Width="100px" Wrap="true" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="v_assigned_at" HeaderText="Date Assigned" DataFormatString="{0:MM/dd/yyyy}" ItemStyle-Width="150px">
                                        <ItemStyle Width="150px" Wrap="true" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="v_updated_at" HeaderText="Assign Updated" DataFormatString="{0:MM/dd/yyyy}" ItemStyle-Width="150px">
                                        <ItemStyle Width="150px" Wrap="true" />
                                    </asp:BoundField>
                                   <asp:TemplateField HeaderText="Actions" >
                                        <ItemTemplate>
                                            <div style="text-align: center;">
                                                <asp:ImageButton ID="btnAssign" runat="server" ImageUrl="~/Pictures/moreIcon.png"
                                                    CommandArgument='<%# Eval("v_id") %>'
                                                    OnClick="btnAssign_Click" Width="35px" Height="35px" />
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Remove Vehicle">
                                        <ItemTemplate>
                                            <div style="text-align: center;">
                                                <asp:ImageButton ID="btnremove" runat="server" ImageUrl="~/Pictures/removeBtn.png"
                                                    CommandArgument='<%# Eval("v_id") %>'
                                                    OnClick="btnremove_Click" Width="40px" Height="40px" />
                                            </div>
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

                        <asp:LinkButton ID="LinkButton1" runat="server" Text="Assign Hauler"></asp:LinkButton>
                        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

                        <!-- Modal Panel for Vehicle Assignment -->
                        <asp:Panel ID="updatePanel" CssClass="card shadow-lg scrollable-panel" runat="server"
                            Style="background-color: #052507; border: 1px solid aquamarine; border-radius: 8px;">
                            <contenttemplate>
                                <div class="card bg-light" style="background-color: #052507;">
                                    <div class="card-header text-center" style="background-color: #052507; color: aquamarine;">
                                        <h4 class="mb-0">Change of Vehicle</h4>
                                    </div>
                                    <div class="card-body" style="background-color: #052507;">
                                        <div class="row">
                                            <div class="col-12 mb-3">
                                                <div class="input-group input-group-sm">
                                                    <div class="input-group-prepend">
                                                        <span class="input-group-text">Vehicle Category</span>
                                                    </div>
                                                    <asp:DropDownList ID="ddlVehicle" runat="server" CssClass="form-control" AutoPostBack="True" 
                                                        OnSelectedIndexChanged="ddlVehicle_SelectedIndexChanged" AppendDataBoundItems="true">
                                                        <asp:ListItem Text="Select Category" Value="" />
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-12 mb-3">
                                                <div class="input-group input-group-sm">
                                                    <div class="input-group-prepend">
                                                        <span class="input-group-text">Vehicle Plate</span>
                                                    </div>
                                                    <asp:DropDownList ID="ddlvplate" runat="server" CssClass="form-control" AutoPostBack="True" 
                                                        OnSelectedIndexChanged="ddlvplate_SelectedIndexChanged" AppendDataBoundItems="true">
                                                        <asp:ListItem Text="Select Plate" Value="" />
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="col-12 mb-3">
                                                <div class="input-group input-group-sm">
                                                    <div class="input-group-prepend">
                                                        <span class="input-group-text">Vehicle Capacity</span>
                                                    </div>
                                                    <asp:HiddenField ID="txtVehicle_ID" runat="server"/>
                                                    <asp:TextBox ID="txtVehicle_Cap" runat="server" CssClass="form-control" ClientIDMode="Static" Enabled="false"></asp:TextBox>
                                                    <asp:HiddenField ID="hiddenDriverId" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="card-footer text-center" style="background-color: #052507; color: aquamarine;">
                                        <asp:Button ID="btncancel" CssClass="btn btn-secondary" runat="server" Text="Cancel" />
                                        <asp:Button ID="btnAssignVehicle" CssClass="btn btn-primary" runat="server" Text="Assign" OnClick="btnAssignVehicle_Click" />
                                    </div>
                                </div>
                            </contenttemplate>
                        </asp:Panel>

                        <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server"
                            CancelControlID="btncancel" PopupControlID="updatePanel" TargetControlID="LinkButton1"
                            BackgroundCssClass="Background" DropShadow="True">
                        </ajaxToolkit:ModalPopupExtender>

                        <asp:LinkButton ID="LinkButton2" runat="server" Text=""></asp:LinkButton>

                        <!-- Modal Panel for Vehicle Removal -->
                        <asp:Panel ID="Panel2" CssClass="card shadow-lg scrollable-panel" runat="server"
                            Style="background-color: #052507; border: 1px solid aquamarine; border-radius: 8px;">
                            <contenttemplate>
                                <div class="card bg-light" style="background-color: #052507;">
                                    <div class="card-header text-center" style="background-color: #052507; color: aquamarine;">
                                        <h4 class="mb-0">Remove Totally Damaged Vehicle</h4>
                                    </div>
                                    <div class="card-body" style="background-color: #052507;">
                                        <div class="row">
                                            <div class="col-12 mb-3">
                                                <div class="input-group input-group-sm">
                                                    <div class="input-group-prepend">
                                                        <span class="input-group-text">Vehicle Plate</span>
                                                    </div>
                                                    <asp:HiddenField ID="haulerId" runat="server"/>
                                                    <asp:HiddenField ID="vehicleID" runat="server"/>
                                                    <asp:TextBox ID="removeVehicle" runat="server" CssClass="form-control" ClientIDMode="Static" Enabled="false"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="card-footer text-center" style="background-color: #052507; color: aquamarine;">
                                        <asp:Button ID="btnCancel2" CssClass="btn btn-secondary" runat="server" Text="Cancel" />
                                        <asp:Button ID="btnremovevehicle" CssClass="btn btn-primary" runat="server" Text="Remove" OnClick="btnremovevehicle_Click" />
                                    </div>
                                </div>
                            </contenttemplate>
                        </asp:Panel>

                        <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender3" runat="server"
                            CancelControlID="btnCancel2" PopupControlID="Panel2" TargetControlID="LinkButton2"
                            BackgroundCssClass="Background" DropShadow="True">
                        </ajaxToolkit:ModalPopupExtender>

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