using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Text.RegularExpressions;
using Personnel;
using TrainingLibrary;

public partial class TakeTraining : System.Web.UI.Page
{
    string iAm;
    Person IAM;
    Training training;

    protected void Page_Load(object sender, EventArgs e)
    {
        iAm = Person.LogonUserIdentity();
        IAM = Person.GetPersonFromActiveDirectory(iAm);
        training = Training.ThisTraining(Convert.ToInt32(Request.QueryString["training"]));

        if (!Page.IsPostBack) // must bind controls here otherwise values get overwritten
        {
            LoadTraining();
            BindControls();
        }
    }

    protected void BindControls()
    {
        rQuestions.DataSource = Test.QuestionsAndAnswers(Convert.ToInt32(training.Ident));
        rQuestions.DataBind();
    }

    protected void LoadTraining()
    {
        StringBuilder trainingdetails = new StringBuilder();
        trainingdetails.Append("<h2>" + training.Name + "</h2>");
        trainingdetails.Append("<p>");
        if (training.Notes != "")
        {
            trainingdetails.Append(training.Notes + "</p>");
        }
        if (!training.URL.ToString().ToLower().Contains("52tdka"))
        {
            trainingdetails.Append("This is an external training and you will get credited for its completion by your Training Support Manager.");
        }
        if (training.RenewalMonths > 0)
        {
            trainingdetails.Append("<p>Assigned personnel must retake this training every " + training.RenewalMonths + " months from the " + (training.URL.ToUpper().Contains("52TDKA") ? " assignment " : " completion ") + " date.</p>");
        }
        if (training.PassingScore > 0)
        {
            trainingdetails.Append("<p>The passing score for the test is " + training.PassingScore + "%.</p>");
        }
        lblTraining.Text = trainingdetails.ToString();

        lbTrainingURL.Text = training.URL;
        lbTrainingURL.CommandArgument = training.URL;
        
    }

    protected string MessageText()
    {
        StringBuilder message = new StringBuilder();
        message.Append("<p align='center'>");
        message.Append("To take this training, click on the link below.");
        if (Convert.ToInt32(Eval("Passing_Score")) > 0)
        {
            message.Append("  Return to this windows to take the test.");
        }
        message.Append("</p>");
        return message.ToString();
    }

    protected bool AutoSelectCorrectAnswer()
    {
        bool correct = false;        
        string[] TAA = Regex.Split(System.Configuration.ConfigurationManager.AppSettings["TrainingAutoAnswer"], ",");
        foreach (string taa in TAA)
        {
            if (iAm == taa)
            {
                correct = Convert.ToBoolean(Eval("Answer_Correct"));
            }
        } 
        return correct;
    }

    protected void btnOpenTraining_Command(object sender, CommandEventArgs e)
    {
        Page.ClientScript.RegisterStartupScript(this.GetType(), null, "window.open('" + e.CommandArgument.ToString() + "', '_newtab')", true);

        if (Convert.ToInt32(training.PassingScore) > 0)  // Take Test
        {
            lblTrainingMessage.Text = "";
            lblTrainingMessage.Visible = false;
            rQuestions.Visible = true;
        }
        else  // No Test. Insert Training Completion Record
        {
            if (e.CommandArgument.ToString().ToLower().Contains("52tdka"))
            {
                int newcompletionident = Assignment.InsertAssignmentCompletion(training.Ident, IAM.WindowsLogonUserName);
            }
        }
    }

    protected void btnSubmitTest_Command(object sender, CommandEventArgs e)
    {
        
        //Answer answer = new Answer();
        double numberofquestions = 0;
        double numberofcorrectanswers = 0;
        lblTrainingMessage.Text = "";

        foreach (RepeaterItem rqItem in rQuestions.Items)
        {
            numberofquestions++;
            Repeater rAnswers = (Repeater)rqItem.FindControl("rAnswers");
            if (rAnswers != null)
            {
                foreach (RepeaterItem ratItem in rAnswers.Items)
                {
                    Label lblAnswerCorrect = (Label)ratItem.FindControl("lblAnswerCorrect");
                    CheckBox rbAnswer = (CheckBox)ratItem.FindControl("rbAnswer");
                    if ((rbAnswer != null) && (rbAnswer.Checked) && (lblAnswerCorrect != null))
                    {
                        if ((rbAnswer.Checked) && (Convert.ToBoolean(lblAnswerCorrect.Text) == true))
                        {
                            numberofcorrectanswers++;
                        }
                    }
                }
            }
        }
        if (((numberofcorrectanswers / numberofquestions) * 100) >= Convert.ToDouble(training.PassingScore))
        {  //passed the test. insert training completion record
            int newcompletionident = Assignment.InsertAssignmentCompletion(training.Ident, IAM.WindowsLogonUserName);
            lblTrainingMessage.Text = @"<h4>You passed the test for this training.</h4>";
            lblTrainingMessage.Visible = true;
        }
        else
        {  //failed test. try again.
            lblTrainingMessage.Text = @"<h4>You failed the test for this training.  View the training again to retake the test.</h4>";
            lblTrainingMessage.Visible = true;

        }
        numberofquestions = 0;
        numberofcorrectanswers = 0;
        rQuestions.Visible = false;
    }

    protected void rAnswers_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        Label lblQuestionIdent = (Label)e.Item.FindControl("lblQuestionIdent");
        if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
        RadioButton rdo = (RadioButton)e.Item.FindControl("rbAnswer");
        string script = @"SetUniqueRadioButton('rAnswers.*" + lblQuestionIdent.Text + "',this)";
        rdo.Attributes.Add("onclick", script);
    }

}