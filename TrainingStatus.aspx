<%@ Page Title="Training Status" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="TrainingStatus.aspx.cs" Inherits="TrainingStatus" StylesheetTheme="TrainingStatus" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageTitle" runat="Server">
    <h1>Training Status</h1>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">

    <ajaxToolkit:ToolkitScriptManager runat="Server" EnableScriptGlobalization="true"
        EnableScriptLocalization="true" ID="ScriptManager1" ScriptMode="Debug" CombineScripts="false" />

    <div id="MainContent">
        <p align="center">

            <asp:DropDownList ID="ddlTraining" runat="server" OnSelectedIndexChanged="ddlTraining_SelectedIndexChanged" AutoPostBack="true" /><br />
            <asp:Label ID="lblTraining" runat="server" Text=""></asp:Label><br />

            <asp:Label ID="lblMajorCommands" runat="server" Text="Major Command: "></asp:Label>

            <asp:DropDownList ID="ddlMajorCommands" runat="server" AutoPostBack="True"
                OnSelectedIndexChanged="ddlMajorCommands_SelectedIndexChanged">
            </asp:DropDownList>
            &nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="lblCommands" runat="server" Text="Command: "></asp:Label>
            <asp:DropDownList ID="ddlCommands" runat="server" AutoPostBack="True"
                OnSelectedIndexChanged="ddlCommands_SelectedIndexChanged">
            </asp:DropDownList>
            &nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="labelOrganizations" runat="server" Text="Office Symbol: "></asp:Label>
            <asp:DropDownList ID="ddlOrgainizations" runat="server" AutoPostBack="True"
                OnSelectedIndexChanged="ddlOrgainizations_SelectedIndexChanged">
            </asp:DropDownList>

            <asp:RadioButtonList ID="rblPersonnelType" RepeatDirection="Horizontal"
                runat="server"
                OnSelectedIndexChanged="rblPersonnelType_SelectedIndexChanged"
                AutoPostBack="True"
                Style="list-style:center;" align="center"
                >
                <asp:ListItem Value="V">Military</asp:ListItem>
                <asp:ListItem Value="C">Civilian</asp:ListItem>
                <asp:ListItem Value="E">Contractor</asp:ListItem>
                <asp:ListItem Selected="True" Value=" All ">All</asp:ListItem>
            </asp:RadioButtonList>


            <asp:CheckBox ID="chkNoCompletion" runat="server" AutoPostBack="true" Text="No Completion" TextAlign="Left" OnCheckedChanged="chkNoCompletion_CheckedChanged" />
            &nbsp;&nbsp;&nbsp;&nbsp;

            <asp:CheckBox ID="chkLT30" runat="server" AutoPostBack="true" Text="< 30 Days" TextAlign="Left" OnCheckedChanged="chkLT30_CheckedChanged" />
            &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:CheckBox ID="chkOverdue" runat="server" AutoPostBack="true" Text="Overdue" TextAlign="Left" OnCheckedChanged="chkOverdue_CheckedChanged" />

            &nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btnEmailOverdue" runat="server" Text="Email Overdue" OnClick="btnEmailOverdue_Click" Visible="false" />
            <asp:Label ID="lblEmailResult" runat="server" Text="" Visible="false"></asp:Label>

            <br />
        <asp:Label ID="lblMessage" runat="server" Text="" Visible="false"></asp:Label>
            </p>
        <asp:DataList ID="dlPersonnel" runat="server" CellPadding="4" ForeColor="#333333" Visible="false">
            <HeaderTemplate>
                <table border="0" cellpadding="2" cellspacing="0" width="100%">
                    <tr>
                        <td align="left">Name
                        </td>
                        <td align="left">Email
                        </td>
                        <td align="right">Phone/DSN
                        </td>
                        <td align="right">Assigned
                        </td>
                        <td align="right">Completed
                        </td>
                        <td align="right">Due Next
                        </td>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr class="row">
                    <td class="row" align="left">
                        <asp:Label ID="lblEDIPI" runat="server" Text='<%# Eval("WindowsLogonUserName") %>' Visible="false"></asp:Label>
                        <asp:HyperLink ID="hlWhoAmI" NavigateUrl='<%# Link2UserTraining() %>' Text='<%# DataBinder.Eval(Container.DataItem, "DisplayName")%>' runat="server" />
                    </td>
                    <td class="row" align="left">
                        <asp:hyperlink id="hlEmail" navigateurl='<%# SendEmail() %>' text='<%# Eval("EMAILADDRESS")%>' runat="server" />
                    </td>
                    <td align="right" class="row" nowrap>
                        <%# PhoneOrDSN() %>
                    </td>
                    <td class="rowPadLeft" nowrap>
                        <%#  Convert.ToDateTime(Eval("DateAssigned")).ToString("MM/dd/yyyy") %>
                    </td>
                    <td class="rowPadLeft" nowrap>
                        <%# FormatLastCompletedDate() %>
                    </td>
                    <td class="rowPadLeft" nowrap>
                        <%# FormatNextRequiredDate() %>
                    </td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr class="alternateRow">

                    <td class="alternateRow" align="left">
                        <asp:Label ID="lblEDIPI" runat="server" Text='<%# Eval("WindowsLogonUserName") %>' Visible="false"></asp:Label>
                        <asp:HyperLink ID="hlWhoAmI" NavigateUrl='<%# Link2UserTraining() %>' Text='<%# DataBinder.Eval(Container.DataItem, "DisplayName")%>' runat="server" />
                    </td>
                    <td class="alternateRow" align="left">
                        <asp:hyperlink id="hlEmail" navigateurl='<%# SendEmail() %>' text='<%# Eval("EMAILADDRESS")%>' runat="server" />
                    </td>
                    <td align="right" class="alternateRow" nowrap>
                        <%# PhoneOrDSN() %>
                    </td>
                    <td class="alternateRowPadLeft" nowrap>
                        <%#  Convert.ToDateTime(Eval("DateAssigned")).ToString("MM/dd/yyyy") %>
                    </td>
                    <td class="alternateRowPadLeft" nowrap>
                        <%# FormatLastCompletedDate() %>
                    </td>
                    <td class="alternateRowPadLeft" nowrap>
                        <%# FormatNextRequiredDate() %>
                    </td>
                </tr>
            </AlternatingItemTemplate>
            <FooterTemplate>
            </table>
            </FooterTemplate>
        </asp:DataList>
        </div>

</asp:Content>
