using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using Personnel;
using System.Diagnostics;
using System.Text;
using TrainingLibrary;

public partial class DelegateTraining : System.Web.UI.Page
{

    string iAm;
    Training training;
    ArrayList owners;
    ArrayList owned;
    ArrayList notowned;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        iAm = Person.LogonUserIdentity();        
        training = Training.ThisTraining(Convert.ToInt32(Request.QueryString["training"]));

        if (!IsPostBack)
        {
            LoadTraining();
            BindOwners();
        }
    }

    protected void BindOwners()
    {
        owners = TrainingOwnerList.TrainingOwners(training.Ident);
        owned = (ArrayList)owners[1];

        dlOwner.DataSource = owned;
        dlOwner.DataBind();

        if (owned.Count > 0)
        {
            lblDelegates.Text = owned.Count.ToString() + " individuals manage this training.";
        }
        else
        {
            lblDelegates.Text = "No one manages this training.";
        }

        notowned = (ArrayList)owners[0];
        ArrayList subnotowned = new ArrayList();
        if (rblPersonnelType.SelectedItem.Value == " All ")
        {
            subnotowned = notowned;   //notassigned 0 
        }
        else
        {
            foreach (Person userType in notowned)
            {
                if (((userType.EmployeeType != null)
                    && ((userType.EmployeeType.ToUpper() == rblPersonnelType.SelectedItem.Value.ToUpper())))
                    || ((userType.EmployeeType != null)
                    && ((rblPersonnelType.SelectedItem.Value.ToUpper() == "V")
                    && (userType.EmployeeType.ToUpper() == "A"))))
                {
                    subnotowned.Add(userType);
                }
            }
        }
        dlNotOwner.DataSource = subnotowned;
        dlNotOwner.DataBind();
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
        if (training.URL != "")
        {
            trainingdetails.Append("<p><a href='" + training.URL + "' target=_blank>" + training.URL + "</a></p>");
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
    }

    protected void rblPersonnelType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindOwners();
    }

    protected void btnAssign_Click(object sender, EventArgs e)
    {
        foreach (Control var in dlNotOwner.Controls)
        {
            CheckBox ch = (CheckBox)var.FindControl("chkSelect");
            if (ch != null)
            {
                if (ch.Checked)
                {
                    Label lblEDIPI = (Label)var.FindControl("lblEDIPI");
                    int newowner = TrainingOwner.InsertTrainingOwner(training.Ident, lblEDIPI.Text);
                }
            }
        }
        BindOwners();
    }

    protected void btnUnAssign_Click(object sender, EventArgs e)
    {
        foreach (Control var in dlOwner.Controls) 
        {
            CheckBox ch = (CheckBox)var.FindControl("chkSelect"); 
            if (ch != null) 
            { 
                if (ch.Checked) 
            {
                Label lblEDIPI = (Label)var.FindControl("lblEDIPI");
                TrainingOwner.DeleteTrainingOwnerByTrainingAndEDIPI(training.Ident, lblEDIPI.Text);
                } 
            } 
        }
        BindOwners();
    }

}