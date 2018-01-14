<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="AssignTraining.aspx.cs" Inherits="AssignTraining" StylesheetTheme="SkinFile" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <script type="text/javascript">
        function CheckAllAssigned(obj) {
            var list = document.getElementById("<%=dlAssigned.ClientID%>");
            var chklist = list.getElementsByTagName("input");
            for (var i = 0; i < chklist.length; i++) {
                if (chklist[i].type == "checkbox" && chklist[i] != obj) {
                    chklist[i].checked = obj.checked;
                }
            }
        }
        function CheckNotAssigned(obj) {
            var list = document.getElementById("<%=dlNotAssigned.ClientID%>");
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
    <h1>Assign Training</h1>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <ajaxToolkit:ToolkitScriptManager runat="Server" EnableScriptGlobalization="true"
        EnableScriptLocalization="true" ID="ScriptManager1" ScriptMode="Debug" CombineScripts="false" />

    <asp:Label ID="lblTraining" runat="server" Text=""></asp:Label>
    <asp:Panel ID="pnlSelectAssignments" runat="server">

        <table style="width: 100%;">
            <tr>
                <th>
                    <h4>Personnel Not Assigned This Training</h4><br />

                    <table width="100%" align="center" cellpadding="0">
                        <tr>
                            <td width="75%" align="right" valign="top">

                                <asp:Label ID="lblMajorCommands" runat="server" Text="Major Command: "></asp:Label>
                                <asp:DropDownList ID="ddlMajorCommands" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlMajorCommands_SelectedIndexChanged">
                                </asp:DropDownList><br />

                                &nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="lblCommands" runat="server" Text="Command: "></asp:Label>
                                <asp:DropDownList ID="ddlCommands" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlCommands_SelectedIndexChanged">
                                </asp:DropDownList><br />

                                &nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="labelOrganizations" runat="server" Text="Office Symbol: "></asp:Label>
                                <asp:DropDownList ID="ddlOrgainizations" runat="server" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddlOrgainizations_SelectedIndexChanged">
                                </asp:DropDownList>

                            </td>
                            <td width="25%"></td>
                        </tr>
                        <tr>
                            <td align="center" valign="top" colspan="2">
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
                    </table>

                </th>
                <th valign="bottom" style="border-left: 1px solid #4b6c9e">
                    <h4>
                        <asp:Label ID="lblAssignments" runat="server" Text="No one is assigned this training."></asp:Label></h4><br />
                </th>
            </tr>
            <tr>
                <td valign="top">
                    <asp:DataList ID="dlNotAssigned" runat="server" CellPadding="4" ForeColor="#333333" OnItemCommand="btnAssignIndividual_Click">
                        <HeaderTemplate>
                            <table border="0" cellpadding="2" cellspacing="0">
                                <tr>
                                    <td align="right" nowrap>
                                        <asp:CheckBox Enabled="true" ID="chkSelectAll" Text="&radic; All" TextAlign="Left" onclick="CheckNotAssigned(this);" runat="server" />&nbsp;&nbsp;&nbsp;&nbsp;
                                    </td>
                                    <td>Name</td>
                                    <td align="right">Assign selected on this date</td>
                                    <td align="right" nowrap>
                                        <asp:TextBox runat="server" ID="txtDateAssignSelected" Text='<%# DateTime.Now.ToString("MM/dd/yyyy") %>' autocomplete="off" Width="65px" />
                                        <ajaxToolkit:CalendarExtender ID="groupCalendarExtender" runat="server" TargetControlID="txtDateAssignSelected" />
                                        <asp:Button ID="btnAssignSelected" runat="server" Text="&#8594;" ToolTip="Assign selected this date." Width="30px" OnClick="btnAssignSelected_Click" />
                                    </td>
                                </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr class="assignRow">
                                <td class="assignRow" align="right">
                                    <asp:Label ID="lblEDIPI" runat="server" Text='<%# Eval("WindowsLogonUserName") %>' Visible="false"></asp:Label>
                                    <asp:CheckBox Enabled="true" ID="chkSelect" runat="server" />&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                                <td class="assignRow" colspan="2">
                                    <%# Eval("DisplayName") %>
                                </td>
                                <td class="assignRow" align="right" nowrap>
                                    <asp:TextBox runat="server" ID="txtDateAssignIndividual" Text='<%# DateTime.Now.ToString("MM/dd/yyyy") %>' autocomplete="off" Width="65px" />
                                    <asp:Button ID="btnAssignIndividual" runat="server" Text="&#8594;" ToolTip="Assign individual." Width="30px" CommandName="assign" />
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr class="assignAlternateRow">
                                <td class="assignAlternateRow" align="right">
                                    <asp:Label ID="lblEDIPI" runat="server" Text='<%# Eval("WindowsLogonUserName") %>' Visible="false"></asp:Label>
                                    <asp:CheckBox Enabled="true" ID="chkSelect" runat="server" />&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                                <td class="assignAlternateRow" colspan="2">
                                    <%# Eval("DisplayName") %>
                                </td>
                                <td class="assignAlternateRow" align="right" nowrap>
                                    <asp:TextBox runat="server" ID="txtDateAssignIndividual" Text='<%# DateTime.Now.ToString("MM/dd/yyyy") %>' autocomplete="off" Width="65px" />
                                    <asp:Button ID="btnAssignIndividual" runat="server" Text="&#8594;" ToolTip="Assign individual." Width="30px" CommandName="assign" />
                                </td>
                            </tr>
                        </AlternatingItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:DataList>
                </td>
                <td valign="top" style="border-left: 1px solid #4b6c9e">
                    <asp:DataList ID="dlAssigned" runat="server" CellPadding="4" ForeColor="#333333" OnItemCommand="btnUpdateAssignedIndividual_Click">
                        <HeaderTemplate>
                            <table border="0" cellpadding="2" cellspacing="0">
                                <tr>
                                    <td align="right" nowrap>
                                        <asp:CheckBox Enabled="true" ID="chkSelectAll" Text="&radic; All" TextAlign="Left" onclick="CheckAllAssigned(this);" runat="server" />&nbsp;&nbsp;&nbsp;&nbsp;
                                    </td>
                                    <td>Name</td>
                                    <td align="right">Assign selected on this date</td>
                                    <td align="right" nowrap>
                                        <asp:TextBox runat="server" ID="txtDateSelectedAssigned" Text='<%# DateTime.Now.ToString("MM/dd/yyyy") %>' autocomplete="off" Width="65px" />
                                        <ajaxToolkit:CalendarExtender ID="groupCalendarExtender" runat="server" TargetControlID="txtDateSelectedAssigned" />
                                        <asp:Button ID="btnUpdateSelectedAssigned" runat="server" Text="&#8595;" ToolTip="Update selected assigned to this date." Width="30px" OnClick="btnUpdateSelectedAssigned_Click" />
                                    </td>
                                </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr class="assignRow">
                                <td class="assignRow" align="right" nowrap>
                                    <asp:Label ID="lblEDIPI" runat="server" Text='<%# Eval("Edipi") %>' Visible="false"></asp:Label>
                                    <asp:CheckBox Enabled="true" ID="chkSelect" runat="server" />&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                                <td class="assignRow" colspan="2">
                                    <%# Eval("DisplayName") %>
                                </td>
                                <td class="assignRow" align="right" nowrap>
                                    <asp:TextBox runat="server" ID="txtDateIndividualAssigned" Text='<%# Convert.ToDateTime(Eval("Date_Assigned")).ToString("MM/dd/yyyy") %>' autocomplete="off" Width="65px" />
                                    <asp:Button ID="btnUpdateIndividualAssigned" runat="server" Text="&#8629;" ToolTip="Update assigned date." Width="30px" CommandName="update" />
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr class="assignAlternateRow">
                                <td class="assignAlternateRow" align="right">
                                    <asp:Label ID="lblEDIPI" runat="server" Text='<%# Eval("Edipi") %>' Visible="false"></asp:Label>
                                    <asp:CheckBox Enabled="true" ID="chkSelect" runat="server" />&nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                                <td class="assignAlternateRow" colspan="2">
                                    <%# Eval("DisplayName") %>
                                </td>
                                <td class="assignAlternateRow" align="right" nowrap>
                                    <asp:TextBox runat="server" ID="txtDateIndividualAssigned" Text='<%# Convert.ToDateTime(Eval("Date_Assigned")).ToString("MM/dd/yyyy") %>' autocomplete="off" Width="65px" />
                                    <asp:Button ID="btnUpdateIndividualAssigned" runat="server" Text="&#8629;" ToolTip="Update assigned date." Width="30px" CommandName="update" />
                                </td>
                            </tr>
                        </AlternatingItemTemplate>
                        <FooterTemplate>
                            <tr>
                                <td colspan="4">
                                    <asp:Button ID="btnUnAssignSelected" runat="server" Text="UnAssign Selected" CssClass="buttonText" Width="100px" OnClick="btnUnAssignSelected_Click" /></td>
                            </tr>
                            </table>
                        </FooterTemplate>
                    </asp:DataList>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>

