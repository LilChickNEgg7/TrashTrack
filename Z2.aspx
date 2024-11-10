<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Z2.aspx.cs" Inherits="Capstone.Z2" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>View Scale Slip</title>
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        .modal-custom {
            position: absolute; /* Allow positioning anywhere */
            top: 100px; /* Initial top position */
            left: 50%; /* Center horizontally */
            transform: translateX(-50%); /* Adjust for center alignment */
            background-color: white; /* Modal background */
            border: 1px solid #ccc; /* Border */
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2); /* Shadow for depth */
            z-index: 1050; /* Ensure it appears above other elements */
            width: 100%; /* Width of the modal */
            height: 100%; /* Height of the modal */
            display: none; /* Hidden by default */
            overflow: hidden; /* Hide overflow */
        }

        .modal-header {
            cursor: move; /* Indicate that the header can be dragged */
            background-color: #f5f5f5; /* Optional header background */
            padding: 10px; /* Padding inside header */
            border-bottom: 1px solid #ccc; /* Bottom border */
        }

        .modal-body {
            padding: 20px; /* Padding inside the modal */
            height: calc(100% - 40px); /* Adjust body height */
            overflow: auto; /* Allow scrolling if content overflows */
        }

        .resizer {
            width: 10px; /* Resizer handle width */
            height: 10px; /* Resizer handle height */
            background: #007bff; /* Resizer color */
            position: absolute; /* Position it relative to the modal */
            bottom: 0; /* Align to bottom */
            right: 0; /* Align to right */
            cursor: nwse-resize; /* Resize cursor */
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">

            <asp:Button ID="btnViewSlip" CssClass="btn btn-primary" runat="server" Text="View Slip" OnClick="btnViewSlip_Click" />
            <asp:Button ID="btnOtherAction" CssClass="btn btn-secondary" runat="server" Text="Other Action" OnClick="btnOtherAction_Click" />

            <!-- Custom Modal -->
            <div id="imageModal" class="modal-custom">
                <div class="modal-header">
                    <h5 class="modal-title">Scale Slip</h5>
                    <button type="button" class="close" onclick="closeModal()">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:Image ID="Image2" runat="server" alt="Scale Slip" Style="display: none; width: 100%; height: auto;" />
                </div>
                <div class="resizer"></div> <!-- Resizer Handle -->
            </div>
        </div>

        <script src="https://code.jquery.com/jquery-3.5.2.min.js"></script>
        <script>
            // Dragging functionality
            let isDragging = false;
            let offset = { x: 0, y: 0 };

            document.addEventListener('DOMContentLoaded', () => {
                const modal = document.getElementById('imageModal');
                const header = modal.querySelector('.modal-header');
                const resizer = modal.querySelector('.resizer');

                // Dragging
                header.addEventListener('mousedown', (event) => {
                    isDragging = true;
                    offset.x = event.clientX - modal.offsetLeft;
                    offset.y = event.clientY - modal.offsetTop;

                    document.addEventListener('mousemove', drag);
                    document.addEventListener('mouseup', stopDragging);
                });

                // Resizing
                resizer.addEventListener('mousedown', (event) => {
                    event.stopPropagation(); // Prevent triggering drag event
                    document.addEventListener('mousemove', resize);
                    document.addEventListener('mouseup', stopResizing);
                });

                function drag(event) {
                    if (isDragging) {
                        modal.style.left = (event.clientX - offset.x) + 'px';
                        modal.style.top = (event.clientY - offset.y) + 'px';
                    }
                }

                function stopDragging() {
                    isDragging = false;
                    document.removeEventListener('mousemove', drag);
                    document.removeEventListener('mouseup', stopDragging);
                }

                function resize(event) {
                    const width = event.clientX - modal.getBoundingClientRect().left;
                    const height = event.clientY - modal.getBoundingClientRect().top;

                    if (width > 300) { // Minimum width
                        modal.style.width = width + 'px';
                    }
                    if (height > 200) { // Minimum height
                        modal.style.height = height + 'px';
                    }
                }

                function stopResizing() {
                    document.removeEventListener('mousemove', resize);
                    document.removeEventListener('mouseup', stopResizing);
                }
            });

            function closeModal() {
                const modal = document.getElementById('imageModal');
                modal.style.display = 'none'; // Hide modal
            }

            function showModal() {
                const modal = document.getElementById('imageModal');
                modal.style.display = 'block'; // Show modal
            }

            // Call this function to show the modal with the image
            function loadImage(src) {
                const image = document.getElementById('<%= Image2.ClientID %>');
                image.src = src;
                image.style.display = 'block'; // Make image visible
                showModal(); // Show modal
            }
        </script>
    </form>
</body>
</html>
