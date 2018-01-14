<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="DelegateTraining.aspx.cs" Inherits="DelegateTraining" StylesheetTheme="SkinFile" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <script type="text/javascript">
        function CheckAllAssigned(obj) {
            var list = document.getElementById("<%=dlOwner.ClientID%>");
            var chklist = list.getElementsByTagName("input");
            for (var i = 0; i < chklist.length; i++) {
                if (chklist[i].type == "checkbox" && chklist[i] != obj) {
                    chklist[i].checked = obj.checked;
                }
            }
        }
        function CheckNotAssigned(obj) {
            var list = document.getElementById("<%=dlNotOwner.ClientID%>");
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
    <h1>Delegate Training</h1>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">

    <asp:Label ID="lblTraining" runat="server" Text=""></asp:Label>
    <h4 align="center">
        <asp:Label ID="lblDelegates" runat="server" Text="No one manages this training."></asp:Label></h4>
    <asp:Panel ID="pnlSelectDelegates" runat="server" Visible="true">

        <table style="width: 100%;">
            <tr>
                <td colspan="2" align="center">
                    <asp:RadioButtonList ID="rblPersonnelType" RepeatDirection="Horizontal"
                        runat="server"
                        OnSelectedIndexChanged="rblPersonnelType_SelectedIndexChanged" TextAlign="Left"
                        AutoPostBack="True">
                        <asp:ListItem Value="V">Military</asp:ListItem>
                        <asp:ListItem Value="C">Civilian</asp:ListItem>
                        <asp:ListItem Value="E">Contractor</asp:ListItem>
                        <asp:ListItem Selected="True" Value=" All ">All</asp:ListItem>
                    </asp:RadioButtonList>

                </td>
            </tr>
            <tr>
                <th>Not Delegates<br />
                    <br />
                    <asp:Button ID="btnAssign" runat="server" Text=" >  Add Delegates  > " CssClass="buttonText" Width="140px" OnClick="btnAssign_Click" /></th>
                <th>Delegates<br />
                    <br />
                    <asp:Button ID="btnUnAssign" runat="server" Text=" <  Remove Delegates  < " CssClass="buttonText" Width="140px" OnClick="btnUnAssign_Click" /></th>
            </tr>
            <tr>
                <td valign="top" nowrap>
                    <asp:DataList ID="dlNotOwner" runat="server" CellPadding="4" ForeColor="#333333">
                        <HeaderTemplate>
                            <table border="0" cellpadding="2" cellspacing="0">
                                <tr>
                                    <td align="right">
                                        <asp:CheckBox Enabled="true" ID="chkSelectAll" Text="&radic; All" TextAlign="Left" onclick="CheckNotAssigned(this);" runat="server" />&nbsp;&nbsp;&nbsp;&nbsp;
                                    </td>
                                    <td>Name
                                    </td>
                                </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr class="row">
                                <td class="row" align="right">
                                    <asp:Label ID="lblEDIPI" runat="server" Text='<%# Eval("WindowsLogonUserName") %>' Visible="false"></asp:Label>
                                    <asp:CheckBox Enabled="true" ID="chkSelect" runat="server" />&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                                <td class="row">
                                    <asp:Label ID="lblDisplayName" runat="server" Text='<%# Eval("DisplayName") %>' ></asp:Label>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr class="alternateRow">
                                <td class="alternateRow" align="right">
                                    <asp:Label ID="lblEDIPI" runat="server" Text='<%# Eval("WindowsLogonUserName") %>' Visible="false"></asp:Label>
                                    <asp:CheckBox Enabled="true" ID="chkSelect" runat="server" />&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                                <td class="alternateRow">
                                    <asp:Label ID="lblDisplayName" runat="server" Text='<%# Eval("DisplayName") %>' ></asp:Label>
                                </td>
                            </tr>
                        </AlternatingItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:DataList>
                </td>
                <td valign="top" nowrap>
                    <asp:DataList ID="dlOwner" runat="server" CellPadding="4" ForeColor="#333333">
                        <HeaderTemplate>
                            <table border="0" cellpadding="2" cellspacing="0">
                                <tr>
                                    <td align="right">
                                        <asp:CheckBox Enabled="true" ID="chkSelectAll" Text="&radic; All" TextAlign="Left" onclick="CheckAllAssigned(this);" runat="server" />&nbsp;&nbsp;&nbsp;&nbsp;
                                    </td>
                                    <td>Name
                                    </td>
                                </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr class="row">
                                <td class="row" align="right">
                                    <asp:Label ID="lblEDIPI" runat="server" Text='<%# Eval("WindowsLogonUserName") %>' Visible="false"></asp:Label>
                                    <asp:CheckBox Enabled="true" ID="chkSelect" runat="server" />&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                                <td class="row">
                                    <asp:Label ID="lblDisplayName" runat="server" Text='<%# Eval("DisplayName") %>' ></asp:Label>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr class="alternateRow">
                                <td class="alternateRow" align="right">
                                    <asp:Label ID="lblEDIPI" runat="server" Text='<%# Eval("WindowsLogonUserName") %>' Visible="false"></asp:Label>
                                    <asp:CheckBox Enabled="true" ID="chkSelect" runat="server" />&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                                <td class="alternateRow">
                                    <asp:Label ID="lblDisplayName" runat="server" Text='<%# Eval("DisplayName") %>' ></asp:Label>
                                </td>
                            </tr>
                        </AlternatingItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:DataList>
                </td>
            </tr>
        </table>

    </asp:Panel>


</asp:Content>

