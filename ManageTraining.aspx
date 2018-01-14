<%@ Page EnableEventValidation="false" Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ManageTraining.aspx.cs" Inherits="ManageTraining" StylesheetTheme="SkinFile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <script type="text/javascript">
        function CheckAll(obj) {
            var list = document.getElementById("<%=dlTraining.ClientID%>");
            var chklist = list.getElementsByTagName("input");
            for (var i = 0; i < chklist.length; i++) {
                if (chklist[i].type == "checkbox" && chklist[i] != obj) {
                    chklist[i].checked = obj.checked;
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageTitle" runat="Server">
    <h1>Manage Training</h1>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">

    <div id="MainContent">

        <h2>Training I Manage</h2>

        <p></p>

        <asp:DataList ID="dlTraining" Width="98%" runat="server" CellPadding="4" ForeColor="#333333">
            <HeaderTemplate>
                <table width="100%" border="0" cellpadding="2" cellspacing="0">
                    <tr><td width="100%" colspan ="3" align="center">
                        <asp:LinkButton ID="btnAdd" runat="server" Text="Add New Training" OnCommand="Add_Training" CommandName="Add" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lbUploadADLSSS" PostBackUrl="UploadADLSSS.aspx" runat="server">Upload ADLS SS</asp:LinkButton>
                        </td></tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr class="row">
                    <td class="row" valign="top">
                        <asp:Label ID="Label1" Text='<%# Eval("Name") %>' runat="server" />
                    </td>
                    <td width="20px" class="row" valign="top" align="right">
                        <asp:Label ID="Label3" Text='<%# Eval("AssignedIndividuals") %>' runat="server" />
                    </td>
                    <td width="300px" class="row" valign="top" align="right">
                        <asp:LinkButton ID="btnEdit" runat="server" Text="  Edit  " OnCommand="Edit_Training" CommandName="Edit" CommandArgument='<%# Eval("Ident") %>' CssClass="buttonText" />
&nbsp;&nbsp;
                        <asp:LinkButton ID="btnDelegate" runat="server" Text="Delegate" OnCommand="btnDelegate_Command" CommandName="Delegate" CommandArgument='<%# Eval("Ident") %>' CssClass="buttonText" />
&nbsp;&nbsp;
                        <asp:LinkButton ID="btnAssign" runat="server" Text="Assign" OnCommand="Assign_Training" CommandName="Assign" CommandArgument='<%# Eval("Ident") %>' CssClass="buttonText" />
/
                        <asp:LinkButton ID="btnCopyAssigned" runat="server" Text="Copy" OnCommand="Copy_Assigned" CommandName="Copy_Assigned" CommandArgument='<%# Eval("Ident") %>' CssClass="buttonText" />
&nbsp;&nbsp;
                        <asp:LinkButton ID="btnDelete" runat="server" Text="Delete" OnCommand="Delete_Training" CommandName="Delete" CommandArgument='<%# Eval("Ident") %>'  OnClientClick='<%# FormatDeleteAlert() %>' CssClass="buttonText" />
                    </td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr class="alternateRow">
                    <td class="alternateRow" valign="top">
                        <asp:Label ID="Label2" Text='<%# Eval("Name") %>' runat="server" />
                    </td>
                    <td width="20px" class="alternateRow" valign="top" align="right">
                        <asp:Label ID="Label3" Text='<%# Eval("AssignedIndividuals") %>' runat="server" />
                    </td>
                    <td width="300px" class="alternateRow" valign="top" align="right">
                        <asp:LinkButton ID="btnEdit" runat="server" Text="  Edit  " OnCommand="Edit_Training" CommandName="Edit" CommandArgument='<%# Eval("Ident") %>' CssClass="buttonText" />
&nbsp;&nbsp;
                        <asp:LinkButton ID="btnDelegate" runat="server" Text="Delegate" OnCommand="btnDelegate_Command" CommandName="Delegate" CommandArgument='<%# Eval("Ident") %>' CssClass="buttonText" />
&nbsp;&nbsp;
                        <asp:LinkButton ID="btnAssign" runat="server" Text="Assign" OnCommand="Assign_Training" CommandName="Assign" CommandArgument='<%# Eval("Ident") %>' CssClass="buttonText" />
/
                        <asp:LinkButton ID="btnCopyAssigned" runat="server" Text="Copy" OnCommand="Copy_Assigned" CommandName="Copy_Assigned" CommandArgument='<%# Eval("Ident") %>' CssClass="buttonText" />
&nbsp;&nbsp;
                        <asp:LinkButton ID="btnDelete" runat="server" Text="Delete" OnCommand="Delete_Training" CommandName="Delete" CommandArgument='<%# Eval("Ident") %>'  OnClientClick='<%# FormatDeleteAlert() %>' CssClass="buttonText" />
                    </td>
                </tr>
            </AlternatingItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:DataList>
    </div>
</asp:Content>

