<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="TakeTraining.aspx.cs" Inherits="TakeTraining" StylesheetTheme="SkinFile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <script type="text/javascript">
    function SetUniqueRadioButton(nameregex, current)
{
   re = new RegExp(nameregex);
   for(i = 0; i < document.forms[0].elements.length; i++)
   {
      elm = document.forms[0].elements[i]
      if (elm.type == 'radio')
      {
         if (re.test(elm.name))
         {
            elm.checked = false;
         }
      }
   }
   current.checked = true;
}
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageTitle" runat="Server">
    <h1>Take Training</h1>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <div id="MainContent">
        <asp:Label ID="lblTraining" runat="server" Text=""></asp:Label>
        <p align="center"><asp:Label ID="lblTrainingMessage" runat="server" Text='<%# MessageText() %>' ></asp:Label></p>
        <p align="center">
        <asp:LinkButton ID="lbTrainingURL" OnCommand="btnOpenTraining_Command" runat="server"></asp:LinkButton>
        </p>

                <asp:Repeater ID="rQuestions" Visible="false" runat="server">
<%--                <asp:Repeater ID="Repeater1" Visible="false" runat="server" DataSource='<%# TrainingLibrary.Test.GetChildRelation(Container.DataItem, "Test_Questions")%>'>--%>
                    <ItemTemplate>
                        <span class="repeaterRow">
                            <h4><%# DataBinder.Eval(Container.DataItem, "Question_Text") %></h4>
                            <p>
                                <asp:Repeater ID="rAnswers" runat="server" DataSource='<%# TrainingLibrary.Test.GetChildRelation(Container.DataItem, "Test_Answers")%>' OnItemDataBound="rAnswers_ItemDataBound">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAnswerIdent" Text='<%# DataBinder.Eval(Container.DataItem, "Answer_Ident") %>' Visible="false" runat="server"/>
                                        <asp:Label ID="lblQuestionIdent" Text='<%# DataBinder.Eval(Container.DataItem, "Question_Ident") %>' Visible="false" runat="server"/>
                                        <asp:Label ID="lblAnswerCorrect" Text='<%# DataBinder.Eval(Container.DataItem, "Answer_Correct") %>' Visible="false" runat="server"/>
                                        <asp:RadioButton ID="rbAnswer" GroupName='<%# Eval("Question_Ident") %>' Checked='<%# AutoSelectCorrectAnswer() %>' Text='<%# DataBinder.Eval(Container.DataItem, "Answer_Text") %>' TextAlign="Right" runat="server" /><br />
                                    </ItemTemplate>
                                </asp:Repeater>
                            </p>
                        </span>
                    </ItemTemplate>
                    <%--                        <AlternatingItemTemplate>
                            <span class="repeaterAlternateRow">
                            <h4><%#DataBinder.Eval(Container.DataItem, "Text")%></h4><p>
                                <asp:Repeater ID="rAnswers" runat="server" EnableViewState="false" DataSource='<%# TrainingLibrary.Test.GetChildRelation(Container.DataItem, "Question_Answers")%>'>
                                    <ItemTemplate>
                                        <asp:RadioButton ID="rbAnswer" Text='<%# DataBinder.Eval(Container.DataItem, "Answer_Text") %>' TextAlign="Right" runat="server" /><br />
                                    </ItemTemplate>
                                </asp:Repeater>
                            </p>
                            </span>
                        </AlternatingItemTemplate>--%>                
                    <FooterTemplate>
                                <p align="center"><asp:Button ID="btnSubmitTest" OnCommand="btnSubmitTest_Command" runat="server" Text="Submit Test" /></p>
            </FooterTemplate>
                </asp:Repeater>            




        <%--                    <asp:Repeater ID="Repeater1" runat="server" EnableViewState="false">
                <ItemTemplate>
                    <h3>Test for <%# DataBinder.Eval(Container.DataItem, "Name") %></h3>
                    <asp:Repeater ID="rQuestions" runat="server" EnableViewState="false" DataSource='<%# GetChildRelation(Container.DataItem, "Test_Questions")%>'>
                        <ItemTemplate>
                            <span class="repeaterRow">
                            <h4><%#DataBinder.Eval(Container.DataItem, "Text")%></h4><p>
                                <asp:Repeater ID="rAnswers" runat="server" EnableViewState="false" DataSource='<%# GetChildRelation(Container.DataItem, "Question_Answers")%>'>
                                    <ItemTemplate>
                                        <asp:RadioButton ID="rbAnswer" Text='<%# DataBinder.Eval(Container.DataItem, "Text") %>' TextAlign="Right" runat="server" /><br />
                                    </ItemTemplate>
                                </asp:Repeater>
                            </p>
                            </span>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <span class="repeaterAlternateRow">
                            <h4><%#DataBinder.Eval(Container.DataItem, "Text")%></h4><p>
                                <asp:Repeater ID="rAnswers" runat="server" EnableViewState="false" DataSource='<%# GetChildRelation(Container.DataItem, "Question_Answers")%>'>
                                    <ItemTemplate>
                                        <asp:RadioButton ID="rbAnswer" Text='<%# DataBinder.Eval(Container.DataItem, "Text") %>' TextAlign="Right" runat="server" /><br />
                                    </ItemTemplate>
                                </asp:Repeater>
                            </p>
                            </span>
                        </AlternatingItemTemplate>
                    </asp:Repeater>
                    <p align="center"><asp:Button ID="btnSubmitTest" runat="server" Text="Submit Test" /></p>
                </ItemTemplate>
            </asp:Repeater>--%>
    </div>
</asp:Content>

