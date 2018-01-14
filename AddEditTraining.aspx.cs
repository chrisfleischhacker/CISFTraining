using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using TrainingLibrary;
using Personnel;

public partial class AddEditTraining : System.Web.UI.Page
{
    Person IAM;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack) // must bind controls here otherwise values get overwritten
        {
            if (Request.QueryString["mode"] == "Edit")
            {
                EditTraining(Convert.ToInt32(Request.QueryString["training"]));
            }
        }
    }

    protected void EditTraining(int t)
    {
        fvTraining.ChangeMode(FormViewMode.Edit);
        fvTraining.DataSource = Test.TrainingAndQuestionsAndAnswers(t);
        fvTraining.DataBind();
        SetControls();
    }

    protected void SetControls()
    {
        TextBox txtPassingScore = (TextBox)fvTraining.FindControl("txtPassingScore");
        Label lblQuestionsAndAnswers = (Label)fvTraining.FindControl("lblQuestionsAndAnswers");
        Repeater rQuestions = (Repeater)fvTraining.FindControl("rQuestions");

        if ((txtPassingScore.Text == "") || (txtPassingScore.Text == "0"))
        {
            lblQuestionsAndAnswers.Visible = false;
            rQuestions.Visible = false;
        }
        else
        {
            lblQuestionsAndAnswers.Visible = true;
            rQuestions.Visible = true;
        }
    }

    protected void btnSaveTraining_Click(object sender, EventArgs e)
    {
        int t = 0;
        TextBox txtTrainingName = (TextBox)fvTraining.FindControl("txtTrainingName");
        TextBox txtTrainingNotes = (TextBox)fvTraining.FindControl("txtTrainingNotes");
        TextBox txtTrainingURL = (TextBox)fvTraining.FindControl("txtTrainingURL");
        TextBox txtTrainingRetakeMonths = (TextBox)fvTraining.FindControl("txtTrainingRetakeMonths");
        TextBox txtPassingScore = (TextBox)fvTraining.FindControl("txtPassingScore");

        Training training = new Training();
        training.Name = txtTrainingName.Text;
        training.Notes = txtTrainingNotes.Text;
        training.URL = txtTrainingURL.Text;
        training.RenewalMonths = (txtTrainingRetakeMonths.Text != "" ? (int?)Convert.ToInt32(txtTrainingRetakeMonths.Text) : null);
        training.PassingScore = (txtPassingScore.Text != "" ? (int?)Convert.ToInt32(txtPassingScore.Text) : null);

        if (fvTraining.CurrentMode == FormViewMode.Insert)
        {
            t = Training.InsertTraining(training);
            TrainingOwner.InsertTrainingOwner(t, Person.LogonUserIdentity());
        }
        else if (fvTraining.CurrentMode == FormViewMode.Edit)
        {
            Label lblTrainingIdent = (Label)fvTraining.FindControl("lblTrainingIdent");
            training.Ident = Convert.ToInt32(lblTrainingIdent.Text);
            Training.UpdateTraining(training);
            t = training.Ident;
            TrainingOwner.InsertTrainingOwner(t, Person.LogonUserIdentity());
        }
        EditTraining(t);
    }

    protected void btnSaveQuestion_Click(object sender, CommandEventArgs e)
    {
        Question question = new Question();
        Label lblTrainingIdent = (Label)fvTraining.FindControl("lblTrainingIdent");
        Repeater rQuestions = (Repeater)fvTraining.FindControl("rQuestions");
        if (e.CommandName == "InsertQuestion")
        {
            TextBox txtAddQuestion = (TextBox)rQuestions.Controls[0].Controls[0].FindControl("txtAddQuestion");
            if (txtAddQuestion.Text.Trim() != "")
            {
                question.Training_Ident = Convert.ToInt32(lblTrainingIdent.Text);
                question.Question_Text = txtAddQuestion.Text;
                int question_ident = Question.InsertQuestion(question);
            }
        }
        if (e.CommandName == "UpdateQuestion")
        {
            foreach (RepeaterItem rqItem in rQuestions.Items)
            {
                TextBox txtEditQuestion = (TextBox)rqItem.FindControl("txtEditQuestion");
                Label lblQuestionIdent = (Label)rqItem.FindControl("lblQuestionIdent");
                if ((txtEditQuestion != null) && (txtEditQuestion.Text.Trim() != "") && (Convert.ToInt32(lblQuestionIdent.Text) == Convert.ToInt32(e.CommandArgument)))
                {
                    question.Question_Ident = Convert.ToInt32(lblQuestionIdent.Text);
                    question.Question_Text = txtEditQuestion.Text;
                    Question.UpdateQuestion(question);
                }
            }
        }
        EditTraining(Convert.ToInt32(lblTrainingIdent.Text));
    }

    protected void btnDeleteQuestion_Click(object sender, CommandEventArgs e)
    {
        Question.DeleteQuestion(Convert.ToInt32(e.CommandArgument));
        Label lblTrainingIdent = (Label)fvTraining.FindControl("lblTrainingIdent");
        EditTraining(Convert.ToInt32(lblTrainingIdent.Text));
    }

    protected void btnSaveAnswer_Click(object sender, CommandEventArgs e)
    {
        Answer answer = new Answer();
        Label lblTrainingIdent = (Label)fvTraining.FindControl("lblTrainingIdent");
        Repeater rQuestions = (Repeater)fvTraining.FindControl("rQuestions");
        foreach (RepeaterItem rqItem in rQuestions.Items)
        {
            Repeater rAnswers = (Repeater)rqItem.FindControl("rAnswers");
            if (rAnswers != null)
            {
                if (e.CommandName == "InsertAnswer")
                {
                    TextBox txtAddAnswer = (TextBox)rAnswers.Controls[rAnswers.Controls.Count - 1].Controls[0].FindControl("txtAddAnswer");
                    CheckBox chkAddCorrect = (CheckBox)rAnswers.Controls[rAnswers.Controls.Count - 1].Controls[0].FindControl("chkAddCorrect");
                    if ((txtAddAnswer != null) && (txtAddAnswer.Text.Trim() != ""))
                    {
                        answer.Question_Ident = Convert.ToInt32(e.CommandArgument);
                        answer.Answer_Text = txtAddAnswer.Text;
                        answer.Answer_Correct = chkAddCorrect.Checked;
                        Answer.InsertAnswer(answer);
                    }
                }
                if (e.CommandName == "UpdateAnswer")
                {
                    foreach (RepeaterItem ratItem in rAnswers.Items)
                    {
                        Label lblQuestionIdent = (Label)ratItem.FindControl("lblQuestionIdent");
                        Label lblAnswerIdent = (Label)ratItem.FindControl("lblAnswerIdent");
                        TextBox txtEditAnswer = (TextBox)ratItem.FindControl("txtEditAnswer");
                        CheckBox chkEditCorrect = (CheckBox)ratItem.FindControl("chkEditCorrect");
                        if ((txtEditAnswer != null) && (txtEditAnswer.Text.Trim() != "") && (Convert.ToInt32(lblAnswerIdent.Text) == Convert.ToInt32(e.CommandArgument)))
                        {
                            answer.Answer_Ident = Convert.ToInt32(lblAnswerIdent.Text);
                            answer.Question_Ident =  Convert.ToInt32(lblQuestionIdent.Text);
                            answer.Answer_Text = txtEditAnswer.Text;
                            answer.Answer_Correct = chkEditCorrect.Checked;
                            Answer.UpdateAnswer(answer);
                            if (chkEditCorrect.Checked)
                            {
                                Answer.UpdateIncorrectAnswers(answer);
                            }
                        }
                    }
                }
            }
        }
        EditTraining(Convert.ToInt32(lblTrainingIdent.Text));
    }

    protected void btnDeleteAnswer_Click(object sender, CommandEventArgs e)
    {
        Answer.DeleteAnswer(Convert.ToInt32(e.CommandArgument));
        Label lblTrainingIdent = (Label)fvTraining.FindControl("lblTrainingIdent");
        EditTraining(Convert.ToInt32(lblTrainingIdent.Text));
    }

    protected void Assign_Training(object sender, CommandEventArgs e)
    {
        Response.Redirect("AssignTraining.aspx?training=" + e.CommandArgument.ToString());
    }
    
    protected void txtPassingScore_TextChanged(object sender, EventArgs e)
    {
        Label lblTrainingIdent = (Label)fvTraining.FindControl("lblTrainingIdent");
        TextBox txtPassingScore = (TextBox)fvTraining.FindControl("txtPassingScore");
        if (txtPassingScore.Text == "")
        {
            Question.DeleteAllQuestions(Convert.ToInt32(lblTrainingIdent.Text));
        }
    }
    
    protected string ThisAnswer()
    {
        string answer = "";
        if (Convert.ToBoolean(Eval("answer_correct")) == true)
        {
            answer = Eval("Answer_Text") + @"&nbsp;&nbsp;<span class='correct'>correct</span>";
        }
        else
        {
            answer = Eval("Answer_Text").ToString();
        }
        return answer;
    }

    protected string FormatRetake()
    {
        string retake = "";
        if (Eval("URL").ToString().ToUpper().Contains("52TDKA"))
        {
            retake = "from the assignment date.";
        }
        else
        {
            retake = "from the completion date.";
        }
        return retake;
    }

    protected bool IsCorrect()
    {
        bool correct = false;
        if (Convert.ToBoolean(Eval("answer_correct")) == true)
        {
            correct = true;
        }
        else
        {
            correct = false;
        }
        return correct;
    }

    protected string txtEditQuestionID()
    {
        string id = "";
        id = @"txtEditQuestion" + Eval("Question_Ident").ToString();
        return id;
    }
}