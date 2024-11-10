<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Z3.aspx.cs" Inherits="Capstone.Z3" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.min.js"></script>
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <style>
        
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Button ID="btnViewSlip" CssClass="btn btn-primary" runat="server" Text="View Slip" OnClick="btnViewSlip_Click" />

            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <asp:LinkButton ID="LinkButton4" runat="server"></asp:LinkButton>
            <div id="draggable" class="modal-overlay">
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

            <!-- Modal Popup Extender -->
            <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender4" runat="server"
                CancelControlID="Button6" PopupControlID="updatePanel3" TargetControlID="LinkButton4"
                BackgroundCssClass="Background" DropShadow="True">
            </ajaxToolkit:ModalPopupExtender>
        </div>
        <script type="text/javascript">
            // Make the UpdatePanel draggable after the modal has shown
            function makeDraggable() {
                $(".modal-overlay .card").draggable({
                    handle: ".card-header"
                });
            }

            // Delay to wait for modal rendering
            setTimeout(makeDraggable, 500);
        </script>
    </form>
</body>
</html>
