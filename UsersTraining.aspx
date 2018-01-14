<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="UsersTraining.aspx.cs" Inherits="UsersTraining" StylesheetTheme="SkinFile" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <script type="text/javascript">
        function CheckAllAssignments(obj) {
            var list = document.getElementById("<%=dlUserTraining.ClientID%>");
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
    <h1>User Training</h1>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <ajaxToolkit:ToolkitScriptManager runat="Server" EnableScriptGlobalization="true"
        EnableScriptLocalization="true" ID="ScriptManager1" ScriptMode="Debug" CombineScripts="false" />
    <p>
        <asp:Label ID="lblMyInformation" runat="server" Text=""></asp:Label>
    </p>
    <h2>Assigned Training</h2>
    <asp:DataList ID="dlUserTraining" runat="server" CellPadding="4" ForeColor="#333333">
        <HeaderTemplate>
            <table border="0" cellpadding="2" cellspacing="0">
                <tr>
                    <th>Training</th>
                    <th nowrap>Assigned</th>
                    <th nowrap>Last Completed</th>
                    <th nowrap>Required Next</th>
                    <th align="right">
                        <asp:CheckBox Enabled="true" ID="chkSelectAll" Text="&radic; All" TextAlign="Left" onclick="CheckAllAssignments(this);" runat="server" />&nbsp;&nbsp;&nbsp;&nbsp;
                    </th>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr class="row">
                <td class="row" valign="top">
                    <asp:Label ID="lblTrainingName" Text='<%# Eval("Name") %>' runat="server" />
                    <asp:Label ID="lblIdent" Text='<%# Eval("Id") %>' Visible="false" runat="server" />
                </td>
                <td class="row" valign="top" align="center" nowrap>
                    <asp:Label ID="Label1" Text='<%# Convert.ToDateTime(Eval("DateAssigned")).ToString("MM/dd/yyyy") %>' runat="server" />
                </td>
                <td class="row" valign="top" align="center" nowrap>
                    <asp:Label ID="Label2" Text='<%# FormatLastCompletedDate() %>' runat="server" />
                </td>
                <td class="row" valign="top" align="center" nowrap>
                    <asp:Label ID="Label4" Text='<%# FormatNextRequiredDate() %>' runat="server" />
                </td>
                <td class="row" align="right">
                    <asp:CheckBox Enabled="true" ID="chkSelect" runat="server" />&nbsp;&nbsp;&nbsp;&nbsp;
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            <tr>
                <td colspan="5" align="right">
                    <asp:Label ID="lblCompletionDate" Text="Completion Date: " runat="server" />
                    <asp:TextBox runat="server" ID="txtCompletionDate" Text="" Width="60px" autocomplete="off" />
                    <asp:Button ID="btnCreditSelected" Text="Credit Selected" OnClick="btnCreditSelected_Click" Width="100px" CssClass="buttonText" runat="server" />
                    <ajaxToolkit:CalendarExtender ID="defaultCalendarExtender" runat="server" TargetControlID="txtCompletionDate" />
                </td>
            </tr>
            </table>
        </FooterTemplate>
    </asp:DataList>
</asp:Content>

