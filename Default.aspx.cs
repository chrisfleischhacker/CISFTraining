using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using Personnel;
using TrainingLibrary;
using System.Text.RegularExpressions;
using System.Diagnostics;

public partial class _Default : System.Web.UI.Page
{
    string iAm;
    Person IAM;

    protected void Page_Load(object sender, EventArgs e)
    {
        iAm = Person.LogonUserIdentity();
        IAM = Person.GetPersonFromActiveDirectory(iAm);
        GetMyInformation();
        GetMyTraining();
    }

    private void GetMyInformation()
    {
        lblMyInformation.Text = IAM.DisplayName;
    }

    private void GetMyTraining()
    {
        dlMyTraining.DataSource = MyTrainings.GetMyTraining(IAM.WindowsLogonUserName);
        dlMyTraining.DataBind();
    }

    protected string FormatNotes()
    {
        string formatnotes = "";

        formatnotes = Eval("Notes").ToString();

        if (Eval("LastCompleted") != null)
        {
            formatnotes += @"  Completed: " + Convert.ToDateTime(Eval("LastCompleted")).ToString("MM/dd/yyyy") + ".";
        }
        return formatnotes;
    }

    protected string FormatNextRequiredDate()
    {
        string formattedDate = "";
        if (Eval("NextRequired") != null)
        {
            formattedDate = Convert.ToDateTime(Eval("NextRequired")).ToString("MM/dd/yyyy");
        }
        if ((Eval("NextRequired") != null) 
            && (DateTime.Now >= Convert.ToDateTime(Eval("NextRequired")).AddMonths(3)))
        {
            formattedDate += @"<br /><span class='error'>OVERDUE</span>";
        }
        if (Convert.ToDateTime(Eval("NextRequired")).Year > 9990)
        {
            formattedDate = "";
        }
        return formattedDate;
    }

    protected bool IsTimeToTakeTraining()
    {
        bool itis = false;
        if ((Eval("NextRequired") != null) 
            && (DateTime.Now >= Convert.ToDateTime(Eval("NextRequired"))))
        {
            itis = true;
        }
        else
        {
            itis = false;
        }
        return itis;
    }

    protected string formatPostBackUrl()
    {
        string formatURL = @"TakeTraining.aspx?training=" + Eval("Id").ToString();
        return formatURL;
    }

}