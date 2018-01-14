using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using TrainingLibrary;
using System.Text;
using Personnel;

public partial class ManageTraining : System.Web.UI.Page
{
    string iAm;
    Person IAM;

    protected void Page_Load(object sender, EventArgs e)
    {
        iAm = Person.LogonUserIdentity();
        IAM = Person.GetPersonFromActiveDirectory(iAm);

        if ((!Training.IOwnTraining(iAm)) && (!System.Configuration.ConfigurationManager.AppSettings["TrainingSupportManagers"].Contains(iAm)))
        {
            Response.Redirect("Default.aspx");
        }

        if (!Page.IsPostBack)
        {
            GetTraining();
        }
    }

    private void GetTraining()
    {
        if (System.Configuration.ConfigurationManager.AppSettings["TrainingSupportManagers"].Contains(iAm))
        {
            dlTraining.DataSource = TrainingList.GetAllTraining();
        }
        else
        {
            dlTraining.DataSource = TrainingList.GetAllTrainingIOwn(iAm);
        }
        dlTraining.DataBind();

    }

    protected void Add_Training(object sender, CommandEventArgs e)
    {
        Response.Redirect("AddEditTraining.aspx");
    }

    protected void Assign_Training(object sender, CommandEventArgs e)
    {
        Response.Redirect("AssignTraining.aspx?training=" + e.CommandArgument.ToString());
    }

    protected void Copy_Assigned(object sender, CommandEventArgs e)
    {
        Response.Redirect("CopyAssignedTraining.aspx?training=" + e.CommandArgument.ToString());
    }

    protected void Edit_Training(object sender, CommandEventArgs e)
    {
        Response.Redirect("AddEditTraining.aspx?mode=Edit&training=" + e.CommandArgument.ToString());
    }

    protected void Delete_Training(object sender, CommandEventArgs e)
    {
        Training.DeleteTraining(Convert.ToInt32(e.CommandArgument));
        GetTraining();
    }

    protected string FormatDeleteAlert()
    {
        string trainingname = "";
        trainingname = Eval("Name").ToString().Replace("'", "\\'");
        StringBuilder sConfirm = new StringBuilder();
        sConfirm.Append("return confirm('Deleting the training - " + trainingname + " - will also delete it\\'s associated test, questions and answers and all assignment and completion records.  Are you sure?');");
        return sConfirm.ToString();
    }

    protected string AnswerIsCorrect()
    {
        string answeriscorrect = "";
        if (Convert.ToBoolean(Eval("correct")) == true)
        {
            answeriscorrect = "correct";
        }
        return answeriscorrect;        
    }

    protected void btnDelegate_Command(object sender, CommandEventArgs e)
    {
        Response.Redirect("DelegateTraining.aspx?training=" + e.CommandArgument.ToString());
    }
}
