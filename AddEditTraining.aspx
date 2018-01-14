<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="AddEditTraining.aspx.cs" Inherits="AddEditTraining" StylesheetTheme="SkinFile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageTitle" runat="Server">
    <h1>Add / Edit Training</h1>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">

    <div id="MainContent">

        <h2>Training Details</h2>

        <asp:FormView ID="fvTraining" DefaultMode="Insert" runat="server">
            <InsertItemTemplate>
                <table class="AddEditTraining">
                    <tr>
                        <td valign="top">
                            <p>
                                Training Name:<br />
                                <asp:TextBox ID="txtTrainingName" runat="server" Width="400px"></asp:TextBox>
                            </p>
                        </td>
                        <td valign="top">
                            <p>
                                Location:<br />
                                <asp:TextBox ID="txtTrainingURL" runat="server" Width="400px"></asp:TextBox>
                            </p>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <p>
                                Training Notes:<br />
                                <asp:TextBox ID="txtTrainingNotes" TextMode="MultiLine" runat="server" Width="400px" Height="100px"></asp:TextBox>
                            </p>
                        </td>
                        <td valign="middle">
                            <p align="center">
                                ReTake training every 
                                <asp:TextBox ID="txtTrainingRetakeMonths" runat="server" Width="30px"></asp:TextBox>
                                &nbsp;months <asp:Label ID="lblRetake" runat="server"></asp:Label></p>
                            <p align="center">
                                Passing Test Score:&nbsp;<asp:TextBox ID="txtPassingScore" runat="server" Width="30px"></asp:TextBox>.
                            </p>
                        </td>
                        <tr>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <p>
                                    <asp:Button ID="btnSaveTraining" runat="server" Text="     Save Training     " CssClass="buttonText" OnClick="btnSaveTraining_Click" />
                                </p>
                            </td>
                        </tr>
                </table>
            </InsertItemTemplate>
            <EditItemTemplate>
                <table class="AddEditTraining">
                    <tr>
                        <td valign="top">
                            <p>
                                <asp:Label ID="lblTrainingIdent" Text='<%# DataBinder.Eval(Container.DataItem, "Ident") %>' runat="server" Visible="false"></asp:Label>
                                Training Name:<br />
                                <asp:TextBox ID="txtTrainingName" Text='<%# DataBinder.Eval(Container.DataItem, "Name") %>' runat="server" Width="400px"></asp:TextBox>
                            </p>
                        </td>
                        <td valign="top">
                            <p>
                                Location:<br />
                                <asp:TextBox ID="txtTrainingURL" Text='<%# DataBinder.Eval(Container.DataItem, "URL") %>' runat="server" Width="400px"></asp:TextBox>
                            </p>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <p>
                                Training Notes:<br />
                                <asp:TextBox ID="txtTrainingNotes" Text='<%# DataBinder.Eval(Container.DataItem, "Notes") %>' TextMode="MultiLine" Width="400px" Height="100px" runat="server"></asp:TextBox>
                            </p>
                        </td>
                        <td valign="middle">
                            <p align="center">
                                Retake training every 
                                <asp:TextBox ID="txtTrainingRetakeMonths" Text='<%# DataBinder.Eval(Container.DataItem, "renewal_Months") %>' runat="server" Width="30px"></asp:TextBox>
                                &nbsp;months <asp:Label ID="lblRetake" runat="server" Text='<%# FormatRetake() %>'></asp:Label></p>
                            <p align="center">
                                Passing Test Score:&nbsp;
                                    <asp:TextBox ID="txtPassingScore" Text='<%# DataBinder.Eval(Container.DataItem, "passing_score") %>' OnTextChanged="txtPassingScore_TextChanged" runat="server" Width="30px"></asp:TextBox>
                            </p>
                            <p align="center">
                                <asp:Button ID="btnAssign" runat="server" Text="Assign This Training" OnCommand="Assign_Training" CommandName="Assign" CommandArgument='<%# Eval("Ident") %>' CssClass="buttonText" />
                            </p>
                        </td>
                        <tr>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                    <asp:Button ID="btnSaveTraining" runat="server" Text="     Save Training     " CssClass="buttonText" OnClick="btnSaveTraining_Click" />
                            </td>
                        </tr>
                </table>

                <asp:Label ID="lblQuestionsAndAnswers" runat="server" Text='<h4>Questions and Answers</h4>'></asp:Label>

                <asp:Repeater ID="rQuestions" runat="server" DataSource='<%# TrainingLibrary.Test.GetChildRelation(Container.DataItem, "Test_Questions")%>'>
                    <HeaderTemplate>
                        <table style="width: 100%;">
                            <tr class="repeaterRow">
                                <td colspan="2">
                                    <asp:TextBox ID="txtAddQuestion" TextMode="MultiLine" Text="" runat="server" Width="600px" Height="30px"></asp:TextBox>
                                </td>
                                <td colspan="2">
                                    <asp:Button ID="btnSaveQuestion" runat="server" Text=" Save " OnCommand="btnSaveQuestion_Click" CommandName="InsertQuestion" CommandArgument='<%# Eval("Ident") %>' CssClass="buttonText" />
                                </td>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr class="repeaterRow">
                            <td colspan="2">
                                <br />
                                Question:<br />
                                <asp:Label ID="lblQuestionIdent" runat="server" Text='<%#Eval("Question_Ident")%>' Visible="false"></asp:Label>
                                <asp:TextBox ID="txtEditQuestion" Text='<%#DataBinder.Eval(Container.DataItem, "Question_Text")%>' TextMode="MultiLine" runat="server" Width="600px" Height="30px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Button ID="btnSaveQuestion" runat="server" Text=" Save " OnCommand="btnSaveQuestion_Click" CommandName="UpdateQuestion" CommandArgument='<%# Eval("Question_Ident") %>' CssClass="buttonText" />
                            </td>
                            <td>
                                <asp:Button ID="btnDeleteQuestion" runat="server" Text="Delete" OnCommand="btnDeleteQuestion_Click" CommandName="DeleteQuestion" CommandArgument='<%# Eval("Question_Ident") %>' CssClass="buttonText" />
                            </td>
                        </tr>
                        <tr class="repeaterRow">
                            <td colspan="4">Answers:</td>
                        </tr>
                        <asp:Repeater ID="rAnswers" runat="server" DataSource='<%# TrainingLibrary.Test.GetChildRelation(Container.DataItem, "Question_Answers")%>'>
                            <ItemTemplate>
                                <tr class="repeaterRow">
                                    <td align="right">
                                        <asp:Label ID="lblQuestionIdent" runat="server" Text='<%# DataBinder.Eval(Container.NamingContainer.NamingContainer, "DataItem.Question_Ident") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblAnswerIdent" runat="server" Text='<%#Eval("Answer_Ident")%>' Visible="false"></asp:Label>
                                        <asp:TextBox ID="txtEditAnswer" Text='<%#DataBinder.Eval(Container.DataItem, "Answer_Text")%>' TextMode="MultiLine" runat="server" Width="500px" Height="30px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkEditCorrect" Text="Correct?" Checked='<%# IsCorrect() %>' runat="server" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnSaveAnswer" runat="server" Text=" Save " OnCommand="btnSaveAnswer_Click" CommandName="UpdateAnswer" CommandArgument='<%# Eval("Answer_Ident") %>' CssClass="buttonText" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnDeleteAnswer" runat="server" Text="Delete" OnCommand="btnDeleteAnswer_Click" CommandName="DeleteAnswer" CommandArgument='<%# Eval("Answer_Ident") %>' CssClass="buttonText" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                <tr class="repeaterRow">
                                    <td align="right">
                                        <asp:TextBox ID="txtAddAnswer" TextMode="MultiLine" Text="" runat="server" Width="500px" Height="30px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkAddCorrect" Text="Correct?" runat="server" />
                                    </td>
                                    <td colspan="2">
                                        <asp:Button ID="btnSaveAnswer" runat="server" Text=" Save " OnCommand="btnSaveAnswer_Click" CommandName="InsertAnswer" CommandArgument='<%# DataBinder.Eval(Container.NamingContainer.NamingContainer, "DataItem.Question_Ident") %>' CssClass="buttonText" />
                                    </td>
                                </tr>
                            </FooterTemplate>
                        </asp:Repeater>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            </EditItemTemplate>
        </asp:FormView>
    </div>
</asp:Content>

