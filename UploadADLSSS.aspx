<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="UploadADLSSS.aspx.cs" Inherits="UploadADLSSS" StylesheetTheme="SkinFile" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <script type="text/javascript">
        function SetUniqueRadioButton(nameregex, current) {
            re = new RegExp(nameregex);
            for (i = 0; i < document.forms[0].elements.length; i++) {
                elm = document.forms[0].elements[i]
                if (elm.type == 'radio') {
                    if (re.test(elm.name)) {
                        elm.checked = false;
                    }
                }
            }
            current.checked = true;
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageTitle" runat="Server">
    <h1>Upload ADLS Spread Sheet</h1>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <div id="MainContent">
        <p align="center">
            <asp:Label ID="lblInstructions" Text="Please select training from the drop down list, click 'Browse' to select an ADLS Spreadsheet, then click 'Upload' to credit users for the selected training." runat="server" />
        </p>
        <p align="center">
            <asp:DropDownList ID="ddlTraining" runat="server" />
        </p>
        <p align="center">
            <asp:FileUpload ID="fuUploadADLSSS" runat="server" Width="300px" />
        </p>
        <p align="center">
            <asp:Button ID="btnUploadADLSSS" runat="server" Text="Upload & Credit" OnClick="btnUploadADLSSS_Click" />
        </p>
        <p align="left">
            <asp:Label ID="lblUploadMessage" runat="server" />
        </p>
    </div>
</asp:Content>
