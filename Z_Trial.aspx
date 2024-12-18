﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Z_Trial.aspx.cs" Inherits="Capstone.Z_Trial" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Tabbed Interface</title>
    <!-- Include Bootstrap CSS -->
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.14.1/themes/base/jquery-ui.css">
  <link rel="stylesheet" href="/resources/demos/style.css">
    <style>
        .tab-pane {
            margin-top: 20px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container mt-5">
            <!-- Hidden Field to Store Active Tab Index -->
            <asp:HiddenField ID="hfActiveTab" runat="server" />

            <!-- Nav Tabs -->
            <ul class="nav nav-tabs" id="myTab" role="tablist">
                <li class="nav-item">
                    <a class="nav-link" id="tab1-tab" data-toggle="tab" href="#tab1" role="tab" aria-controls="tab1" aria-selected="false">Tab 1</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" id="tab2-tab" data-toggle="tab" href="#tab2" role="tab" aria-controls="tab2" aria-selected="false">Tab 2</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" id="tab3-tab" data-toggle="tab" href="#tab3" role="tab" aria-controls="tab3" aria-selected="false">Tab 3</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" id="tab4-tab" data-toggle="tab" href="#tab4" role="tab" aria-controls="tab4" aria-selected="false">Tab 4</a>
                </li>
            </ul>

            <!-- Tab Panes -->
            <div class="tab-content" id="myTabContent">
                <!-- Tab 1 Pane -->
                <div class="tab-pane fade" id="tab1" role="tabpanel" aria-labelledby="tab1-tab">
                    <h3>Content for Tab 1</h3>
                    <p>This is the content for Tab 1.</p>
                </div>

                <!-- Tab 2 Pane -->
                <div class="tab-pane fade" id="tab2" role="tabpanel" aria-labelledby="tab2-tab">
                    <h3>Content for Tab 2</h3>
                    <p>This is the content for Tab 2.</p>
                    <!-- Button added here -->
                    <asp:Button ID="btnTab1" runat="server" Text="Click Me" CssClass="btn btn-primary" OnClick="btnTab1_Click" />
                </div>

                <!-- Tab 3 Pane -->
                <div class="tab-pane fade" id="tab3" role="tabpanel" aria-labelledby="tab3-tab">
                    <h3>Content for Tab 3</h3>
                    <p>This is the content for Tab 3.</p>
                    <asp:Button ID="Button1" runat="server" Text="Click Me" CssClass="btn btn-primary" OnClick="btnTab1_Click" />

                </div>

                <!-- Tab 4 Pane -->
                <div class="tab-pane fade" id="tab4" role="tabpanel" aria-labelledby="tab4-tab">
                    <h3>Content for Tab 4</h3>
                    <p>This is the content for Tab 4.</p>
                </div>
            </div>


            <div class="col-6">
                <div class="input-group input-group-sm mb-3">
                    <div class="input-group-prepend">
                        <span class="input-group-text" id="inputGroup-sizing-sm" style="width: 135px">Date</span>
                    </div>
                    <asp:TextBox ID="dateTodayTxt" TextMode="DateTimeLocal" runat="server" CssClass="form-control" ClientIDMode="Static" aria-label="Small" aria-describedby="inputGroup-sizing-sm"></asp:TextBox>
                </div>
            </div>
<%--            <asp:Button ID="Button2" CssClass="btn btn-primary" runat="server" Text="Generate Bill" OnClick="btnGenerateBill_Click" OnClientClick="return confirm('Are you sure you want to generate bill?');" />--%>

            <%--<asp:Panel ID="pnlMyModalBox" runat="server" class="draggable" Visible="false" HorizontalAlign="Left"
                Style="position: absolute; left: 75px; z-index: 50000; top: 100px;">
                <uc1:myusercontrol id="mdMyUserControl" runat="server" visible="false" />
            </asp:Panel>--%>
            <div id="draggable" class="ui-widget-content">
                <p>Drag me around</p>
            </div>
            <style>
                #draggable { width: 150px; height: 150px; padding: 0.5em; }
            </style>

        </div>
    </form>
    <!-- Include Bootstrap JS, Popper.js, and jQuery -->
    <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.9.2/dist/umd/popper.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>


    <script src="https://code.jquery.com/jquery-3.7.1.js"></script>
  <script src="https://code.jquery.com/ui/1.14.1/jquery-ui.js"></script>

    <%--draggable panel--%>
    <script src="../../jquery-1.4.4.js"></script>
    <script src="../../ui/jquery.ui.core.js"></script>
    <script src="../../ui/jquery.ui.widget.js"></script>
    <script src="../../ui/jquery.ui.mouse.js"></script>
    <script src="../../ui/jquery.ui.draggable.js"></script>
    <%--These include tags have to be in this exact order because the lower one depend on the first ones --%>
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
        //$(document).ready(function () {
        //    dragAndDrop();
        //    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(dragAndDrop);
        //    //this makes the javascript method execute after an ajax action on the page
        //});

        function dragAndDrop() {
            $(".draggable").draggable();
        }
    </script>
</body>
</html>
