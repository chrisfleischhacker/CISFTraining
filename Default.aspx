<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" StylesheetTheme="SkinFile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageTitle" runat="Server">
    <h1>My Training</h1>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">

    <p>
        <asp:Label ID="lblMyInformation" runat="server" Text=""></asp:Label>
    </p>
    <h2>Assigned Training</h2>
    <asp:DataList ID="dlMyTraining" runat="server" CellPadding="4" ForeColor="#333333">
        <HeaderTemplate>
            <table border="0" cellpadding="2" cellspacing="0">
                <tr>
                    <th>Training</th>
                    <th nowrap>Required Next</th>
                    <th nowrap></th>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr class="row">
                <td class="row" valign="top">
                    <h3><asp:Label ID="lblTrainingName" Text='<%# Eval("Name") %>' runat="server" /></h3>
                    <asp:Label ID="lblTrainingNotes" Text='<%# FormatNotes() %>' runat="server" />
                </td>
                <td class="row" valign="top" nowrap>
                    <asp:Label ID="Label4" Text='<%# FormatNextRequiredDate() %>' runat="server" />
                </td>
                <td class="row" valign="top" nowrap>
                    <asp:Button ID="btnCompleteAssignment" Text="Complete Assignment" PostBackUrl='<%# formatPostBackUrl() %>' CssClass="buttonText" Visible='<%# IsTimeToTakeTraining() %>' Width="120px" runat="server" />
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:DataList>

</asp:Content>

