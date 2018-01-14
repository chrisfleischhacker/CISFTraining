<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="TrainingUsers.aspx.cs" Inherits="TrainingUsers" StylesheetTheme="SkinFile" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <script type = "text/javascript">
        function CheckAll(obj) {
            var list = document.getElementById("<%=dlPersonnel.ClientID%>");
            var chklist = list.getElementsByTagName("input");
            for (var i = 0; i < chklist.length; i++) {
                if (chklist[i].type == "checkbox" && chklist[i] != obj) {
                    chklist[i].checked = obj.checked;
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageTitle" Runat="Server">
                <h1>Manage Users</h1>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" Runat="Server">
    <ajaxToolkit:ToolkitScriptManager runat="Server" EnableScriptGlobalization="true"
        EnableScriptLocalization="true" ID="ScriptManager1" ScriptMode="Debug" CombineScripts="false" />
            <asp:Panel ID="panelContent" runat="server">

    <p align="center">
        <asp:Repeater ID="rLetters" runat="server" 
            onitemcommand="rLetters_ItemCommand" >
        <HeaderTemplate><table border="0" cellpadding="0" cellspacing="0"><tr></HeaderTemplate>
            <ItemTemplate>
                <td>
                    <ASP:Button ID="btnLetter" CssClass="buttonText" Text=<%# DataBinder.Eval(Container.DataItem, "letter") %> runat="server" />
                </td>
            </ItemTemplate>
            <FooterTemplate>
            <td>&nbsp;&nbsp;</td>
            <td>
                <asp:Button ID="btnRefreshCache" CssClass="buttonText"  runat="server" Text="Refresh Cache" OnClick="btnRefreshCache_Click" />
            </td>
            </tr></table></FooterTemplate>
        </asp:Repeater>

        <asp:Label ID="lblMajorCommands" runat="server" Text="Major Command: " ></asp:Label>
            <asp:DropDownList ID="ddlMajorCommands" runat="server" AutoPostBack="True"
                onselectedindexchanged="ddlMajorCommands_SelectedIndexChanged">
            </asp:DropDownList>        
&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="lblCommands" runat="server" Text="Command: " ></asp:Label>
            <asp:DropDownList ID="ddlCommands" runat="server" AutoPostBack="True"
                onselectedindexchanged="ddlCommands_SelectedIndexChanged">
            </asp:DropDownList>        
&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="labelOrganizations" runat="server" Text="Office Symbol: " ></asp:Label>
            <asp:DropDownList ID="ddlOrgainizations" runat="server" AutoPostBack="True"
                onselectedindexchanged="ddlOrgainizations_SelectedIndexChanged">
            </asp:DropDownList>        




<%--        <asp:RadioButtonList ID="rblIsUserAccountEnabled" RepeatDirection="Horizontal" runat="server" AutoPostBack="true"
                OnSelectedIndexChanged="rblIsUserAccountEnabled_SelectedIndexChanged">
        <asp:ListItem Value="True">Enabled</asp:ListItem>
        <asp:ListItem Value="False">Disabled</asp:ListItem>
        <asp:ListItem  Value=" All " Selected="True">All</asp:ListItem>
        </asp:RadioButtonList>--%>

        

        <asp:RadioButtonList ID="rblPersonnelType" RepeatDirection="Horizontal" 
                runat="server" 
                OnSelectedIndexChanged="rblPersonnelType_SelectedIndexChanged" 
                AutoPostBack="True">
        <asp:ListItem Value="V">Military</asp:ListItem>
        <asp:ListItem Value="C">Civilian</asp:ListItem>
        <asp:ListItem Value="E">Contractor</asp:ListItem>
        <asp:ListItem Selected="True" Value=" All ">All</asp:ListItem>
        </asp:RadioButtonList>



        <asp:TextBox ID="txtFind" runat="server"></asp:TextBox>
        <asp:Button ID="btnFind" runat="server" CssClass="buttonText" Text="Find" TabIndex="1" onclick="btnFind_Click" />


        </p>     
        <asp:Label ID="lblMessage" runat="server" Text="" Visible="false"></asp:Label>

            <asp:DataList ID="dlPersonnel" runat="server" CellPadding="4" ForeColor="#333333" Visible="false">
            <HeaderTemplate>
                <table border="0" cellpadding="2" cellspacing="0">
                    <tr>
                        <td align="right">
                            <asp:CheckBox Enabled="true" ID="chkSelectAll" Text="&radic; All" TextAlign="Left" onclick = "CheckAll(this);" runat="server" />&nbsp;&nbsp;&nbsp;&nbsp;
                        </td>
                        <td>
                            Name
                        </td>
                        <td>
                            Overdue
                        </td>

                        <td>
                            Email
                        </td>
                        <td>
                            Phone / DSN
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
                         <asp:HyperLink ID="hlWhoAmI" NavigateUrl='<%# Link2UserTraining() %>' Text='<%# DataBinder.Eval(Container.DataItem, "DisplayName")%>' runat="server" />
<%--                        <%# DataBinder.Eval(Container.DataItem, "DisplayName")%>--%>
                    </td>
                    <td class="row">
                    </td>
                    <td class="row">
                        <asp:HyperLink ID="hlEmail" NavigateUrl='<%# SendEmail() %>' Text='<%# Eval("EMAILADDRESS")%>' runat="server" target="_blank" xmlns:asp="#unknown" />
                    </td>
                    <td class="row" nowrap>
                        <%# PhoneOrDSN() %>
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
                         <asp:HyperLink ID="hlWhoAmI" NavigateUrl='<%# Link2UserTraining() %>' Text='<%# DataBinder.Eval(Container.DataItem, "DisplayName")%>' runat="server" />
<%--                        <%# Eval("DisplayName")%>--%>
                    </td>
                    <td class="alternateRow">
                    </td>
                    <td class="alternateRow">
                        <asp:HyperLink ID="hlEmail" NavigateUrl='<%# SendEmail() %>' Text='<%# Eval("EMAILADDRESS")%>' runat="server" target="_blank" xmlns:asp="#unknown" />
                    </td>
                    <td class="alternateRow" nowrap>
                        <%# PhoneOrDSN() %>
                    </td>
                </tr>
            </AlternatingItemTemplate>
        <FooterTemplate>
            <tr>
                <td colspan="5" align="right">
                    <asp:DropDownList ID="ddlTraining" runat="server" />
                    <asp:Label ID="lblCompletionDate" Text="Completion Date: " runat="server" />
                    <asp:TextBox runat="server" ID="txtCompletionDate" Text="" Width="60px" autocomplete="off" />
                    <asp:Button ID="btnCreditSelected" Text="Credit Selected" OnClick="btnCreditSelected_Click" Width="100px" CssClass="buttonText" runat="server" />
                    <ajaxToolkit:CalendarExtender ID="defaultCalendarExtender" runat="server" TargetControlID="txtCompletionDate" />
                </td>
            </tr>
            </table>
        </FooterTemplate>
        </asp:DataList>      
               &nbsp;&nbsp;
    
        </asp:Panel>

</asp:Content>