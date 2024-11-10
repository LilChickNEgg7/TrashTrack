<%--<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Z1.aspx.cs" Inherits="Capstone.Z1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <!-- jQuery UI CSS -->
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.13.2/themes/base/jquery-ui.css">
    <style>
        /* Draggable container styling */
        #draggable {
            position: absolute;
            width: 820px;
            /* Center the div initially */
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            z-index: 1000;
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <asp:LinkButton ID="LinkButton4" runat="server"></asp:LinkButton>
        
        <div id="draggable" class="ui-widget-content" style="display: none;">
            <!-- Main Panel Design -->
            <asp:UpdatePanel ID="updatePanel3" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                <ContentTemplate>
                    <div class="card shadow-lg" style="max-width: 820px; padding: 0; border: 2px solid #26D8A8; border-radius: 12px; overflow: hidden; box-shadow: 0 5px 15px rgba(0, 0, 0, 0.3);">
                        <div class="card-header text-center" style="background-color: #0D342D; color: #26D8A8; padding: 20px;">
                            <h4>Scale Slip</h4>
                        </div>
                        <div class="card-body" style="padding: 40px; background-color: #052507;">
                            <div class="row d-flex justify-content-center" style="margin-top: 5px;">
                                <asp:Image ID="Image2" runat="server" alt="Scale Slip" Style="display: none; width: 100%; height: auto;" />
                            </div>
                        </div>
                        <div class="card-footer text-center" style="background-color: #0D342D; color: #26D8A8; padding: 15px;">
                            <asp:Button ID="Button6" CssClass="btn btn-secondary" runat="server" Text="Close" OnClick="btnCloseSlip_Click" />
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnViewSlip" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        
        <asp:Button ID="btnViewSlip" CssClass="btn btn-primary" runat="server" Text="View Slip" OnClick="btnViewSlip_Click" />

        <!-- Modal Popup Extender -->
        <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender4" runat="server"
            CancelControlID="Button6" PopupControlID="draggable" TargetControlID="btnViewSlip"
            BackgroundCssClass="Background" DropShadow="True">
        </ajaxToolkit:ModalPopupExtender>
    </form>

    <!-- jQuery and jQuery UI JavaScript -->
    <script src="https://code.jquery.com/jquery-3.7.1.min.js"></script>
    <script src="https://code.jquery.com/ui/1.13.2/jquery-ui.min.js"></script>
    <script>
        $(function () {
            // Make the draggable div draggable
            $("#draggable").draggable();

            // Center the panel when opened
            $("#btnViewSlip").on("click", function () {
                $("#draggable").css({
                    top: "50%",
                    left: "50%",
                    transform: "translate(-50%, -50%)",
                    display: "block"
                });
            });

            // Hide the panel on close
            $("#Button6").on("click", function () {
                $("#draggable").hide();
            });
        });
    </script>
</body>
</html>--%>

<%--<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Z1.aspx.cs" Inherits="Capstone.Z1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.13.2/themes/base/jquery-ui.css">
        <link rel="stylesheet" href="https://code.jquery.com/ui/1.13.2/themes/base/jquery-ui.css">
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

    <style>
        #draggable {
            width: 820px;
            padding: 0;
            border: 2px solid #26D8A8;
            border-radius: 12px;
            overflow: hidden;
            box-shadow: 0 5px 15px rgba(0, 0, 0, 0.3);
            position: absolute;
            z-index: 1000;
            /*display: none;*/ /* Start hidden */
/*            width: 820px;
*/        }
    </style>
</head>

<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <asp:LinkButton ID="LinkButton4" runat="server"></asp:LinkButton>

        <!-- Modal Content -->
        <div id="draggable" style="display: none; width: 70%; height: 90%;">
            <asp:UpdatePanel ID="draggable" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                <ContentTemplate>
                    <div class="card shadow-lg">
                        <div class="card-header text-center" style="background-color: #0D342D; color: #26D8A8; padding: 20px;">
                            <h4>Scale Slip</h4>
                        </div>
                        <div class="card-body" style="padding: 40px; background-color: #052507;">
                            <div class="row d-flex justify-content-center" style="margin-top: 5px;">
                                <asp:Image ID="Image2" runat="server" alt="Scale Slip" Style="display: none; width: 30%; height: 30%;" />
                            </div>
                        </div>
                        <div class="card-footer text-center" style="background-color: #0D342D; color: #26D8A8; padding: 15px;">
                            <asp:Button ID="Button6" CssClass="btn btn-secondary" runat="server" Text="Close" OnClick="btnCloseSlip_Click" />
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnViewSlip" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <!-- Modal Popup Extender -->
        <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender4" runat="server"
            CancelControlID="Button6" PopupControlID="draggable" TargetControlID="LinkButton4" DropShadow="True">
        </ajaxToolkit:ModalPopupExtender>

        <asp:Button ID="btnViewSlip" CssClass="btn btn-primary" runat="server" Text="View Slip" OnClientClick="return false;" />
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
    </form>

    <!-- jQuery and jQuery UI JavaScript -->
    <script src="https://code.jquery.com/jquery-3.7.1.min.js"></script>
    <script src="https://code.jquery.com/ui/1.13.2/jquery-ui.min.js"></script>
    <script>
        $(function () {
            // Make the panel draggable
            $("#draggable").draggable();

            // Center the panel and show it when "View Slip" is clicked
            $("#btnViewSlip").on("click", function () {
                const panel = $("#draggable");
                const winWidth = $(window).width();
                const winHeight = $(window).height();
                const panelWidth = panel.outerWidth();
                const panelHeight = panel.outerHeight();

                // Center the panel in the viewport
                panel.css({
                    top: `${(winHeight - panelHeight) / 2}px`,
                    left: `${(winWidth - panelWidth) / 2}px`,
                    display: "block" // Show the panel
                });
            });
            // Ensure modal remains centered on resize
            $(window).on("resize", function () {
                if ($("#draggable").is(":visible")) {
                    centerModal();
                }
            });

            // Hide panel on close button click
            $("#Button6").on("click", function () {
                $("#draggable").hide();
            });
        });
    </script>
</body>
</html>--%>


<%--<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Z1.aspx.cs" Inherits="Capstone.Z1" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.13.2/themes/base/jquery-ui.css">
    <style>
        #draggable {
            width: 820px;
            padding: 0;
            border: 2px solid #26D8A8;
            border-radius: 12px;
            overflow: hidden;
            box-shadow: 0 5px 15px rgba(0, 0, 0, 0.3);
            background-color: #ffffff;
            position: fixed; /* Keeps it centered in the viewport */
            z-index: 1000;
            display: none; /* Start hidden */
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <!-- LinkButton as the Target Control for ModalPopupExtender -->
        <asp:LinkButton ID="LinkButton4" runat="server" Style="display: none;"></asp:LinkButton>

        <!-- Modal Content -->
        <div id="draggable" class="ui-widget-content">
            <asp:UpdatePanel ID="updatePanel3" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                <ContentTemplate>
                    <div class="card shadow-lg">
                        <div class="card-header text-center" style="background-color: #0D342D; color: #26D8A8; padding: 20px;">
                            <h4>Scale Slip</h4>
                        </div>
                        <div class="card-body" style="padding: 40px; background-color: #052507;">
                            <div class="row d-flex justify-content-center" style="margin-top: 5px;">
                                <asp:Image ID="Image2" runat="server" alt="Scale Slip" Style="display: none; width: 100%; height: auto;" />
                            </div>
                        </div>
                        <div class="card-footer text-center" style="background-color: #0D342D; color: #26D8A8; padding: 15px;">
                            <asp:Button ID="Button6" CssClass="btn btn-secondary" runat="server" Text="Close" OnClick="btnCloseSlip_Click" />
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnViewSlip" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>

        <!-- Modal Popup Extender -->
        <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender4" runat="server"
            CancelControlID="Button6" PopupControlID="draggable" TargetControlID="LinkButton4" DropShadow="True">
        </ajaxToolkit:ModalPopupExtender>

        <!-- Button to Trigger the Modal -->
        <asp:Button ID="btnViewSlip" CssClass="btn btn-primary" runat="server" Text="View Slip" OnClientClick="return false;" />
    </form>

    <!-- jQuery and jQuery UI JavaScript -->
    <script src="https://code.jquery.com/jquery-3.7.1.min.js"></script>
    <script src="https://code.jquery.com/ui/1.13.2/jquery-ui.min.js"></script>
    <script>
        $(function () {
            // Make the panel draggable
            $("#draggable").draggable();

            // Show the modal and center it when "View Slip" is clicked
            $("#btnViewSlip").on("click", function () {
                // Show the modal popup using the ModalPopupExtender
                $find('<%= ModalPopupExtender4.ClientID %>').show();
                centerModal();
            });

            // Center the modal on screen
            function centerModal() {
                const panel = $("#draggable");
                const winWidth = $(window).width();
                const winHeight = $(window).height();
                const panelWidth = panel.outerWidth();
                const panelHeight = panel.outerHeight();

                panel.css({
                    //top: `${(winHeight - panelHeight) / 2}px`,
                    //left: `${(winWidth - panelWidth) / 2}px`,
                    display: "block" // Show the panel
                });
            }

            // Ensure modal remains centered on resize
            $(window).on("resize", function () {
                if ($("#draggable").is(":visible")) {
                    centerModal();
                }
            });

            // Close the modal on "Close" button click
            $("#Button6").on("click", function () {
                $find('<%= ModalPopupExtender4.ClientID %>').hide();
            });
        });
    </script>
</body>
</html>--%>



<%--<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Z1.aspx.cs" Inherits="Capstone.Z1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.13.2/themes/base/jquery-ui.css">
        <link rel="stylesheet" href="https://code.jquery.com/ui/1.13.2/themes/base/jquery-ui.css">
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

    <style>
        #draggable {
            width: 820px;
            padding: 0;
            border: 2px solid #26D8A8;
            border-radius: 12px;
            overflow: hidden;
            box-shadow: 0 5px 15px rgba(0, 0, 0, 0.3);
            position: absolute;
            z-index: 1000;
            /*display: none;*/ /* Start hidden */
            /*width: 820px;*/
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <asp:LinkButton ID="LinkButton4" runat="server"></asp:LinkButton>

        <!-- Modal Content -->

            <asp:UpdatePanel ID="draggable" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true" style="display: none; width: 70%; height: 90%;">
                <ContentTemplate>
                    <div class="card shadow-lg">
                        <div class="card-header text-center" style="background-color: #0D342D; color: #26D8A8; padding: 20px;">
                            <h4>Scale Slip</h4>
                        </div>
                        <div class="card-body" style="padding: 40px; background-color: #052507;">
                            <div class="row d-flex justify-content-center" style="margin-top: 5px;">
                                <asp:Image ID="Image2" runat="server" alt="Scale Slip" Style="display: none; width: 30%; height: 30%;" />
                            </div>
                        </div>
                        <div class="card-footer text-center" style="background-color: #0D342D; color: #26D8A8; padding: 15px;">
                            <asp:Button ID="Button6" CssClass="btn btn-secondary" runat="server" Text="Close" OnClick="btnCloseSlip_Click" />
                        </div>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnViewSlip" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        <!-- Modal Popup Extender -->
        <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender4" runat="server"
            CancelControlID="Button6" PopupControlID="draggable" TargetControlID="LinkButton4" DropShadow="True">
        </ajaxToolkit:ModalPopupExtender>

        <asp:Button ID="btnViewSlip" CssClass="btn btn-primary" runat="server" Text="View Slip" OnClientClick="return false;" />
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
    </form>

    <!-- jQuery and jQuery UI JavaScript -->
    <script src="https://code.jquery.com/jquery-3.7.1.min.js"></script>
    <script src="https://code.jquery.com/ui/1.13.2/jquery-ui.min.js"></script>
    <script>
        $(function () {
            // Make the panel draggable
            $("#draggable").draggable();

            // Center the panel and show it when "View Slip" is clicked
            $("#btnViewSlip").on("click", function () {
                const panel = $("#draggable");
                const winWidth = $(window).width();
                const winHeight = $(window).height();
                const panelWidth = panel.outerWidth();
                const panelHeight = panel.outerHeight();

                // Center the panel in the viewport
                panel.css({
                    top: `${(winHeight - panelHeight) / 2}px`,
                    left: `${(winWidth - panelWidth) / 2}px`,
                    display: "block" // Show the panel
                });
            });
            // Ensure modal remains centered on resize
            $(window).on("resize", function () {
                if ($("#draggable").is(":visible")) {
                    centerModal();
                }
            });

            // Hide panel on close button click
            $("#Button6").on("click", function () {
                $("#draggable").hide();
            });
        });
    </script>
</body>
</html>--%>



<%--<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Z1.aspx.cs" Inherits="Capstone.Z1" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Scale Slip Modal</title>
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/5.1.3/css/bootstrap.min.css">
    <style>
        /* Styling for the draggable modal */
        #draggable {
            width: 70%;
            max-width: 1000px;
            padding: 0;
            border: 2px solid #26D8A8;
            border-radius: 12px;
            overflow: hidden;
            box-shadow: 0 5px 15px rgba(0, 0, 0, 0.3);
            background-color: white;
            z-index: 1000;
            position: fixed;
            display: none; /* Start hidden */
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <!-- Trigger Button for Modal -->
        <asp:Button ID="btnViewSlip" CssClass="btn btn-primary" runat="server" Text="View Slip" OnClientClick="return false;" />

        <!-- Modal Content (without overlay) -->
        <div id="draggable">
            <div class="card shadow-lg">
                <div class="card-header text-center" style="background-color: #0D342D; color: #26D8A8; padding: 20px;">
                    <h4>Scale Slip</h4>
                </div>
                <div class="card-body text-center" style="padding: 40px; background-color: #052507;">
                    <asp:Image ID="Image2" runat="server"  alt="Scale Slip" Style="width: 50%; height: auto;" />
                </div>
                <div class="card-footer text-center" style="background-color: #0D342D; color: #26D8A8; padding: 15px;">
                    <asp:Button ID="Button6" CssClass="btn btn-secondary" runat="server" Text="Close" OnClientClick="return false;" />
                </div>
            </div>
        </div>
    </form>

    <!-- jQuery and jQuery UI JavaScript -->
    <script src="https://code.jquery.com/jquery-3.7.1.min.js"></script>
    <script src="https://code.jquery.com/ui/1.13.2/jquery-ui.min.js"></script>
    <script>
        $(document).ready(function () {
            // Enable draggable functionality on the modal
            $("#draggable").draggable();

            // Show and center the modal when "View Slip" is clicked
            $("#btnViewSlip").on("click", function () {
                const panel = $("#draggable");

                // Center the modal
                const winWidth = $(window).width();
                const winHeight = $(window).height();
                const panelWidth = panel.outerWidth();
                const panelHeight = panel.outerHeight();
                panel.css({
                    top: `${(winHeight - panelHeight) / 2}px`,
                    left: `${(winWidth - panelWidth) / 2}px`,
                    display: "block" // Show the modal
                });
            });

            // Hide modal when "Close" button is clicked
            $("#Button6").on("click", function () {
                $("#draggable").hide();
            });
        });
    </script>
</body>
</html>--%>





<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Z1.aspx.cs" Inherits="Capstone.Z1" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Scale Slip Modal</title>
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/5.1.3/css/bootstrap.min.css">
    <style>
        /* Styling for the draggable modal */
        #draggable {
            width: 70%;
/*            height: 70%;
*/            max-width: 1000px;
            padding: 0;
            border: 2px solid #26D8A8;
            border-radius: 12px;
            overflow: hidden;
            box-shadow: 0 5px 15px rgba(0, 0, 0, 0.3);
            background-color: white;
            z-index: 1000;
            position: fixed;
            display: none; /* Start hidden */
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <!-- Trigger Button for Modal -->
        <asp:Button ID="btnViewSlip" CssClass="btn btn-primary" runat="server" Text="View Slip" OnClientClick="return false;" />

        <!-- Modal Content (without overlay) -->
        <div id="draggable">
            <div class="card shadow-lg" style="height: 80%">
                <div class="card-header text-center" style="background-color: #0D342D; color: #26D8A8; padding: 20px;">
                    <h4>Scale Slip</h4>
                </div>
                <div class="card-body" style="padding: 40px; background-color: #052507;">
                    <div class="row d-flex justify-content-center" style="margin-top: 5px;">
                        <asp:Image ID="Image2" runat="server" alt="Scale Slip" Style="width: 400px; height: 400px;" />
                    </div>
                </div>
                <div class="card-footer text-center" style="background-color: #0D342D; color: #26D8A8; padding: 15px;">
                    <asp:Button ID="Button6" CssClass="btn btn-secondary" runat="server" Text="Close" OnClientClick="return false;" />
                </div>
            </div>
        </div>
    </form>

    <!-- jQuery and jQuery UI JavaScript -->
    <script src="https://code.jquery.com/jquery-3.7.1.min.js"></script>
    <script src="https://code.jquery.com/ui/1.13.2/jquery-ui.min.js"></script>
    <script>
        $(document).ready(function () {
            // Enable draggable functionality on the modal
            $("#draggable").draggable();

            // Show and center the modal when "View Slip" is clicked
            $("#<%= btnViewSlip.ClientID %>").on("click", function () {
                const panel = $("#draggable");

                // Center the modal
                const winWidth = $(window).width();
                const winHeight = $(window).height();
                const panelWidth = panel.outerWidth();
                const panelHeight = panel.outerHeight();
                panel.css({
                    top: `${(winHeight - panelHeight) / 2}px`,
                    left: `${(winWidth - panelWidth) / 2}px`,
                    display: "block" // Show the modal
                });
            });

            // Hide modal when "Close" button is clicked
            $("#<%= Button6.ClientID %>").on("click", function () {
                $("#draggable").hide();
            });
        });
    </script>
</body>
</html>



<%--<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Z1.aspx.cs" Inherits="Capstone.Z1" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Scale Slip Modal</title>
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/5.1.3/css/bootstrap.min.css">
    <style>
        /* Styling for the draggable modal */
        #draggable {
            width: 70%;
            max-width: 1000px;
            padding: 0;
            border: 2px solid #26D8A8;
            border-radius: 12px;
            overflow: hidden;
            box-shadow: 0 5px 15px rgba(0, 0, 0, 0.3);
            background-color: white;
            z-index: 1000;
            position: fixed;
            display: none; /* Start hidden */
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <!-- Trigger Button for Modal -->
        <asp:Button ID="btnViewSlip" CssClass="btn btn-primary" runat="server" Text="View Slip" OnClick="btnViewSlip_Click" />

        <!-- Modal Content -->
        <div id="draggable">
            <div class="card shadow-lg">
                <div class="card-header text-center" style="background-color: #0D342D; color: #26D8A8; padding: 20px;">
                    <h4>Scale Slip</h4>
                </div>
                <div class="card-body text-center" style="padding: 40px; background-color: #052507;">
                    <asp:Image ID="Image2" runat="server" alt="Scale Slip" Style="display: none; width: 100%; height: auto;" />
                </div>
                <div class="card-footer text-center" style="background-color: #0D342D; color: #26D8A8; padding: 15px;">
                    <asp:Button ID="Button6" CssClass="btn btn-secondary" runat="server" Text="Close" OnClientClick="return false;" />
                </div>
            </div>
        </div>
    </form>

    <script src="https://code.jquery.com/jquery-3.7.1.min.js"></script>
    <script>
        $(document).ready(function () {
            // Show the modal when the button is clicked
            $("#<%= btnViewSlip.ClientID %>").on("click", function () {
                $("#draggable").css({
                    top: `${($(window).height() - $('#draggable').outerHeight()) / 2}px`,
                    left: `${($(window).width() - $('#draggable').outerWidth()) / 2}px`,
                    display: "block" // Show the modal
                });
            });

            // Hide modal when "Close" button is clicked
            $("#Button6").on("click", function () {
                $("#draggable").hide();
                $("#Image2").hide(); // Hide the image as well
            });
        });
    </script>
</body>
</html>--%>


<%--<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Z1.aspx.cs" Inherits="Capstone.Z1" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Scale Slip Modal</title>
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/5.1.3/css/bootstrap.min.css">
    <style>
        /* Styling for the draggable modal */
        #draggable {
            width: 70%;
            max-width: 1000px;
            padding: 0;
            border: 2px solid #26D8A8;
            border-radius: 12px;
            overflow: hidden;
            box-shadow: 0 5px 15px rgba(0, 0, 0, 0.3);
            background-color: white;
            z-index: 1000;
            position: fixed;
            display: none; /* Start hidden */
            transition: opacity 0.3s ease; /* Smooth transition */
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <!-- Trigger Button for Modal -->
        <asp:Button ID="btnViewSlip" CssClass="btn btn-primary" runat="server" Text="View Slip" OnClick="btnViewSlip_Click" />

        <!-- Modal Content -->
        <div id="draggable">
            <div class="card shadow-lg" style="height: 80%">
                <div class="card-header text-center" style="background-color: #0D342D; color: #26D8A8; padding: 20px;">
                    <h4>Scale Slip</h4>
                </div>
                <div class="card-body text-center" style="padding: 40px; background-color: #052507;">
                    <asp:Image ID="Image2" runat="server" alt="Scale Slip" Style="display: none; width: 400px; height: 400px;" />
                </div>
                <div class="card-footer text-center" style="background-color: #0D342D; color: #26D8A8; padding: 15px;">
                    <asp:Button ID="Button6" CssClass="btn btn-secondary" runat="server" Text="Close" OnClientClick="return false;" />
                </div>
            </div>
        </div>
    </form>

    <!-- jQuery and jQuery UI JavaScript -->
    <script src="https://code.jquery.com/jquery-3.7.1.min.js"></script>
    <script src="https://code.jquery.com/ui/1.13.2/jquery-ui.min.js"></script>
    <script>
        $(document).ready(function () {
            // Enable draggable functionality on the modal
            $("#draggable").draggable();

            // Show and center the modal when "View Slip" is clicked
            $("#<%= btnViewSlip.ClientID %>").on("click", function () {
                const panel = $("#draggable");

                // Center the modal
                const winWidth = $(window).width();
                const winHeight = $(window).height();
                const panelWidth = panel.outerWidth();
                const panelHeight = panel.outerHeight();
                panel.css({
                    top: `${(winHeight - panelHeight) / 2}px`,
                    left: `${(winWidth - panelWidth) / 2}px`,
                    display: "block", // Show the modal
                    opacity: 1
                });
            });

            // Hide modal when "Close" button is clicked
            $("#<%= Button6.ClientID %>").on("click", function () {
                $("#draggable").hide();
                $("#Image2").hide(); // Hide the image as well
            });

            // Re-center modal on window resize
            $(window).resize(function () {
                const panel = $("#draggable");
                if (panel.is(":visible")) {
                    const winWidth = $(window).width();
                    const winHeight = $(window).height();
                    const panelWidth = panel.outerWidth();
                    const panelHeight = panel.outerHeight();
                    panel.css({
                        top: `${(winHeight - panelHeight) / 2}px`,
                        left: `${(winWidth - panelWidth) / 2}px`
                    });
                }
            });
        });
    </script>
</body>
</html>--%>
