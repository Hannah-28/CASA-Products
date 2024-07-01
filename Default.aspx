<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ProductCreation.ProductCreation" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Create Product Page</title>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#<%= ddlCategory.ClientID %>').change(function () {
                if ($(this).val() === 'Others') {
                    $('#otherCategoryDiv').show();
                } else {
                    $('#otherCategoryDiv').hide();
                    $('#<%= txtOtherCategory.ClientID %>').val(''); // Clear the input field
                }

                // Disable the checkbox if "Current" is selected
                if ($(this).val() === 'Current') {
                    $('#<%= chkPaysInterest.ClientID %>').prop('checked', false).prop('disabled', true);
                } else {
                    $('#<%= chkPaysInterest.ClientID %>').prop('disabled', false);
                }
            });

            // Trigger the change event on page load to handle pre-selected values
            $('#<%= ddlCategory.ClientID %>').trigger('change');
        });

        function validateForm() {
            var productName = $('#<%= txtProductName.ClientID %>').val();
            if (productName === '') {
                alert('Product Name is required.');
                return false;
            }

            var productCategory = $('#<%= ddlCategory.ClientID %>').val();
            var otherCategoryName = $('#<%= txtOtherCategory.ClientID %>').val();
            if (productCategory === 'Others' && otherCategoryName === '') {
                alert('Other Category Name is required.');
                return false;
            }

            return true;
        }

    </script>
</head>
<body class="w-full m-0 p-0" style="height: 100%; background-color: orangered; color: black; font-size: 1em">
    <form style="min-width: 300px; max-width: 600px; margin: auto;" id="form1" runat="server" onsubmit="return validateForm();">
        <asp:HiddenField ID="hdnSuccessMessage" runat="server" />
        <div>
            <h2 style="text-align: center">Create CASA Product</h2>
            <asp:Label ID="Label1" runat="server" Text="Product Name"></asp:Label>
            <asp:TextBox ID="txtProductName" runat="server" Style="width: -webkit-fill-available; padding: 0.5em 0px 0.5em 0.5em; margin: 0.5em 0em"></asp:TextBox>
            <asp:Label ID="Label2" runat="server" Text="Category"></asp:Label>
            <asp:DropDownList ID="ddlCategory" runat="server" Style="width: -webkit-fill-available; padding: 0.5em 0em; margin: 0.5em 0em">
                <asp:ListItem Text="Regular Savings" Value="Regular Savings"></asp:ListItem>
                <asp:ListItem Text="Commitment Savings" Value="Commitment Savings"></asp:ListItem>
                <asp:ListItem Text="Current" Value="Current"></asp:ListItem>
                <asp:ListItem Text="Others" Value="Others"></asp:ListItem>
            </asp:DropDownList>

            <div id="otherCategoryDiv" style="display: none;">
                <asp:Label ID="lblOtherCategory" runat="server" Text="Other Category Name"></asp:Label>
                <asp:TextBox ID="txtOtherCategory" runat="server" Style="width: -webkit-fill-available; padding: 0.5em 0px 0.5em 0.5em; margin: 0.5em 0em"></asp:TextBox>
            </div>
            <asp:Label runat="server" Text="Please select all necessary options"></asp:Label>
            <div style="display: grid; grid-template-columns: 1fr 1fr; margin: 0.5em 0em">
                <div>
                    <asp:CheckBox ID="chkPaysInterest" runat="server" />
                    <asp:Label ID="Label3" runat="server" Text="Interest"></asp:Label>
                </div>
                <div>
                    <asp:CheckBox ID="chkApplyFees" runat="server" />
                    <asp:Label ID="Label4" runat="server" Text="Fees"></asp:Label>
                </div>
                <div>
                    <asp:CheckBox ID="chkSMSNotification" runat="server" />
                    <asp:Label ID="Label5" runat="server" Text="SMS Notifications"></asp:Label>
                </div>
                <div>
                    <asp:CheckBox ID="chkChannelServicesAccess" runat="server" />
                    <asp:Label ID="Label6" runat="server" Text="Channel Services"></asp:Label>
                </div>
            </div>
            <asp:Label runat="server" Text="Please select all applicable General Ledgers"></asp:Label>
            <div style="display: grid; grid-template-columns: 1fr 1fr; margin: 0.5em 0em">
                <div>
                    <asp:CheckBox ID="InterestPayable" runat="server" />
                    <asp:Label ID="Label7" runat="server" Text="Interest Payable GL"></asp:Label>
                </div>
                <div>
                    <asp:CheckBox ID="InterestExpense" runat="server" />
                    <asp:Label ID="Label8" runat="server" Text="Interest Expense GL"></asp:Label>
                </div>
                <div>
                    <asp:CheckBox ID="GeneralFeeIncome" runat="server" />
                    <asp:Label ID="Label9" runat="server" Text="General Fee Income GL"></asp:Label>
                </div>
                <div>
                    <asp:CheckBox ID="MaintenanceFee" runat="server" />
                    <asp:Label ID="Label10" runat="server" Text="Maintenance Fee GL"></asp:Label>
                </div>
            </div>
            <div style="display: grid; justify-content: space-between; grid-template-columns: auto auto; align-items: center;">
                <asp:Button ID="btnSubmit" runat="server" Text="Create Product" OnClick="BtnSubmit_Click" Style="border: none; margin: 0.5em 0em; padding: 0.5em 0.5em; color: orangered; background-color: white; border-radius: 0.8em; cursor: pointer" />
                <a runat="server" href="ViewProducts.aspx" target="_blank" style="margin: 0.5em 0em; padding: 0.4em 0.4em; color: orangered; background-color: white; font-size: 0.95em; border-radius: 0.8em; cursor: pointer; text-decoration: none">View Products</a>
            </div>

        </div>
    </form>
</body>
</html>
