using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Personnel;
using TrainingLibrary;

public partial class UsersTraining : System.Web.UI.Page
{
    string iAm, user;
    Person IAM, USER;

    protected void Page_Load(object sender, EventArgs e)
    {
        iAm = Person.LogonUserIdentity();

        if ((!Training.IOwnTraining(iAm)) && (!System.Configuration.ConfigurationManager.AppSettings["TrainingSupportManagers"].Contains(iAm)))
        {
            Response.Redirect("Default.aspx");
        }

        user = Request.QueryString["user"];  //used to lookup someone else
        USER = Person.GetPersonFromActiveDirectory(user);
        if (!Page.IsPostBack)
        {
            GetUserInformation();
            GetUserTraining();
        }
    }

    private void GetUserInformation()
    {
        lblMyInformation.Text = USER.DisplayName;
    }

    private void GetUserTraining()
    {
        if (Training.IOwnTraining(iAm))
        {
            dlUserTraining.DataSource = MyTrainings.GetAllTrainingIOwnAssignedToThisPerson(iAm, USER.WindowsLogonUserName);
        }
        if (System.Configuration.ConfigurationManager.AppSettings["TrainingSupportManagers"].Contains(iAm))
        {
            dlUserTraining.DataSource = MyTrainings.GetMyTraining(USER.WindowsLogonUserName);
        }
        dlUserTraining.DataBind();
    }

    protected string FormatLastCompletedDate()
    {
        string formattedDate = "";
        if (Eval("LastCompleted") != null)
        {
            formattedDate = Convert.ToDateTime(Eval("LastCompleted")).ToString("MM/dd/yyyy");
        }
        return formattedDate;
    }

    protected string FormatNextRequiredDate()
    {
        string formattedDate = "";
        if (Eval("NextRequired") != null)
        {
            formattedDate = Convert.ToDateTime(Eval("NextRequired")).ToString("MM/dd/yyyy");
        }
        if ((Eval("NextRequired") != null) 
            && (DateTime.Now >= Convert.ToDateTime(Eval("NextRequired")).AddMonths(3)) 
            )
        {
            formattedDate += @"<br /><span class='error'>OVERDUE</span>";
        }
        if (Convert.ToDateTime(Eval("NextRequired")).Year > 9990)
        {
            formattedDate = "";
        }
        return formattedDate;
    }

    //protected bool MakeCreditTrainingVisible()
    //{
    //    bool showit = false;
    //    showit = ((System.Configuration.ConfigurationManager.AppSettings["TrainingSupportManagers"].Contains(iAm))
    //         || (Training.IOwnTraining(iAm)));
    //    return showit;
    //}

    protected void btnCreditSelected_Click(object sender, EventArgs e)
    {
        TextBox txtCompletionDate = (TextBox)dlUserTraining.Controls[dlUserTraining.Controls.Count - 1].Controls[0].FindControl("txtCompletionDate");
        DateTime dt = DateTime.Parse(txtCompletionDate.Text);
        if (dt != DateTime.MinValue && dt != DateTime.MaxValue)
        {
            foreach (Control var in dlUserTraining.Controls)
            {
                CheckBox ch = (CheckBox)var.FindControl("chkSelect");
                if (ch != null)
                {
                    if (ch.Checked)
                    {
                        Label lblIdent = (Label)var.FindControl("lblIdent");
                        Assignment.InsertAssignmentCompletionDate(Convert.ToInt32(lblIdent.Text), Convert.ToDateTime(txtCompletionDate.Text), USER.WindowsLogonUserName);
                    }
                }
            }
        }
        GetUserTraining();
    }
}