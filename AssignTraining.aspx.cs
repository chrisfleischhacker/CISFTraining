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

public partial class AssignTraining : System.Web.UI.Page
{

    string iAm;
    Training training;
    ArrayList assignments;
    ArrayList assigned;
    ArrayList notassigned;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        iAm = Person.LogonUserIdentity();        
        training = Training.ThisTraining(Convert.ToInt32(Request.QueryString["training"]));

        if (!IsPostBack)
        {
            LoadTraining();
            GetMajorCommandCollection();
            GetCommandCollection();
            GetOrganizationCollection();
            BindIndividualAssignments();
            CleanUpAssignmentsAndCompletions(Convert.ToInt32(Request.QueryString["training"]));
        }
    }

    private void CleanUpAssignmentsAndCompletions(int training_ident)
    {
        ArrayList cachedpersonnelcollection = new ArrayList(AllPersonnel.GetCachedPersonnelCollection());
        List<string> assignedlist = EdipiList.GetAssignedEdipis(training_ident);
        List<string> completedlist = EdipiList.GetCompletedEdipis(training_ident);
        List<string> cachedpersonlist = new List<string>();
        foreach (Person p in cachedpersonnelcollection)
        {
            cachedpersonlist.Add(p.WindowsLogonUserName.ToString());
        }
        foreach (string ae in assignedlist)
        {
            if (!cachedpersonlist.Contains(ae))
            {
                Assignment.DeleteAssignmentByTrainingAndEDIPI(training_ident, ae);
            }
        }
        foreach (string ce in completedlist)
        {
            if (!cachedpersonlist.Contains(ce))
            {
                Assignment.DeleteCompletionByTrainingAndEDIPI(training_ident, ce);
            }
        }
    }

    protected void BindIndividualAssignments()
    {
        assignments = AssignmentList.AssignedPersonnel(training.Ident);
        assigned = (ArrayList)assignments[1];

        dlAssigned.DataSource = assigned;
        dlAssigned.DataBind();

        if (assigned.Count > 0)
        {
            lblAssignments.Text = assigned.Count.ToString() + " individuals are assigned this training.";
        }
        else
        {
            lblAssignments.Text = "No one is assigned this training.";
        }

        notassigned = (ArrayList)assignments[0];
        ArrayList subnotassigned = new ArrayList();
        if (rblPersonnelType.SelectedItem.Value == " All ")
        {
            subnotassigned = notassigned;   //notassigned 0 
        }
        else
        {
            foreach (Person userType in notassigned)
            {
                if (((userType.EmployeeType != null)
                    && ((userType.EmployeeType.ToUpper() == rblPersonnelType.SelectedItem.Value.ToUpper())))
                    || ((userType.EmployeeType != null)
                    && ((rblPersonnelType.SelectedItem.Value.ToUpper() == "V")
                    && (userType.EmployeeType.ToUpper() == "A"))))
                {
                    subnotassigned.Add(userType);
                }
            }
        }

        ArrayList alFromSelectedMajorCommand = new ArrayList();
        if (ddlMajorCommands.SelectedItem.Value == " All ")
        {
            alFromSelectedMajorCommand = subnotassigned;
        }
        else
        {
            foreach (Person userMajorCommands in subnotassigned)
            {
                if (userMajorCommands.MajorCommand.ToUpper() == ddlMajorCommands.SelectedItem.Value.ToUpper())
                {
                    alFromSelectedMajorCommand.Add(userMajorCommands);
                }
            }
        }

        ArrayList alFromSelectedCommand = new ArrayList();
        if (ddlCommands.SelectedItem.Value == " All ")
        {
            alFromSelectedCommand = alFromSelectedMajorCommand;
        }
        else
        {
            foreach (Person userCommands in alFromSelectedMajorCommand)
            {
                if (userCommands.Command.ToUpper() == ddlCommands.SelectedItem.Value.ToUpper())
                {
                    alFromSelectedCommand.Add(userCommands);
                }
            }
        }

        ArrayList alFromSelectedOrganization = new ArrayList();
        if (ddlOrgainizations.SelectedItem.Value == " All ")
        {
            alFromSelectedOrganization = alFromSelectedCommand;
        }
        else
        {
            foreach (Person userOfficeSymbols in alFromSelectedCommand)
            {
                if (userOfficeSymbols.Organization.ToUpper() == ddlOrgainizations.SelectedItem.Value.ToUpper())
                {
                    alFromSelectedOrganization.Add(userOfficeSymbols);
                }
            }
        }
        dlNotAssigned.DataSource = alFromSelectedOrganization;
        dlNotAssigned.DataBind();
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
        SetControls();
    }

    protected void SetControls()
    {
        rblPersonnelType.SelectedValue = " All ";
    }

    protected void rblPersonnelType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindIndividualAssignments();
    }


    protected void btnAssignSelected_Click(object sender, EventArgs e)
    {
        TextBox txtDateAssignSelected = (TextBox)dlNotAssigned.Controls[0].Controls[0].FindControl("txtDateAssignSelected"); //find control in header
        DateTime dt = ((!String.IsNullOrEmpty(txtDateAssignSelected.Text)) ? Convert.ToDateTime(txtDateAssignSelected.Text) : DateTime.Today);
        foreach (Control var in dlNotAssigned.Controls)
        {
            CheckBox ch = (CheckBox)var.FindControl("chkSelect");
            if (ch != null)
            {
                if (ch.Checked)
                {
                    Label lblEDIPI = (Label)var.FindControl("lblEDIPI");
                    int newassignment = Assignment.InsertAssignment(training.Ident, dt, lblEDIPI.Text);
                }
            }
        }
        BindIndividualAssignments();
    }

    protected void btnAssignIndividual_Click(object sender, DataListCommandEventArgs e)
    {
        Label lblEDIPI = (Label)e.Item.FindControl("lblEDIPI");
        TextBox txtDateAssignIndividual = (TextBox)e.Item.FindControl("txtDateAssignIndividual");
        DateTime dt = ((!String.IsNullOrEmpty(txtDateAssignIndividual.Text)) ? Convert.ToDateTime(txtDateAssignIndividual.Text) : DateTime.Today);
        Assignment.DeleteAssignmentByTrainingAndEDIPI(training.Ident, lblEDIPI.Text);
        int newassignment = Assignment.InsertAssignment(training.Ident, dt, lblEDIPI.Text);
        LoadTraining();
        BindIndividualAssignments();
    }

    protected void btnUpdateSelectedAssigned_Click(object sender, EventArgs e)
    {
        TextBox txtDateSelectedAssigned = (TextBox)dlAssigned.Controls[0].Controls[0].FindControl("txtDateSelectedAssigned"); //find control in header
        DateTime dt = ((!String.IsNullOrEmpty(txtDateSelectedAssigned.Text)) ? Convert.ToDateTime(txtDateSelectedAssigned.Text) : DateTime.Today);
        foreach (Control var in dlAssigned.Controls)
        {
            CheckBox ch = (CheckBox)var.FindControl("chkSelect");
            if (ch != null)
            {
                if (ch.Checked)
                {
                    Label lblEDIPI = (Label)var.FindControl("lblEDIPI");
                    if (lblEDIPI != null)
                    {
                        Assignment.UpdateAssignmentByTrainingIdentEDIPI(training.Ident, Convert.ToDateTime(txtDateSelectedAssigned.Text), null, lblEDIPI.Text);
                    }
                }
            }
        }
        BindIndividualAssignments();
    }

    protected void btnUpdateAssignedIndividual_Click(object sender, DataListCommandEventArgs e)
    {
        Label lblEDIPI = (Label)e.Item.FindControl("lblEDIPI");
        TextBox txtDateIndividualAssigned = (TextBox)e.Item.FindControl("txtDateIndividualAssigned");
        DateTime dt = ((!String.IsNullOrEmpty(txtDateIndividualAssigned.Text)) ? Convert.ToDateTime(txtDateIndividualAssigned.Text) : DateTime.Today);
        Assignment.UpdateAssignmentByTrainingIdentEDIPI(training.Ident, Convert.ToDateTime(txtDateIndividualAssigned.Text), null, lblEDIPI.Text);
        LoadTraining();
        BindIndividualAssignments();
    }

    protected void btnUnAssignSelected_Click(object sender, EventArgs e)
    {
        foreach (Control var in dlAssigned.Controls)
        {
            CheckBox ch = (CheckBox)var.FindControl("chkSelect");
            if (ch != null)
            {
                if (ch.Checked)
                {
                    Label lblEDIPI = (Label)var.FindControl("lblEDIPI");
                    Assignment.DeleteAssignmentByTrainingAndEDIPI(training.Ident, lblEDIPI.Text);
                }
            }
        }
        BindIndividualAssignments();
    }

    protected void GetOrganizationCollection()
    {
        ArrayList cOrganizationCollection = new ArrayList(Personnel.Organizations.GetCachedOrganizationCollection());
        ddlOrgainizations.DataSource = cOrganizationCollection;
        ddlOrgainizations.DataBind();
        ddlOrgainizations.Items.Insert(0, " All ");
    }

    protected void GetCommandCollection()
    {
        ArrayList cCommandCollection = new ArrayList(Personnel.Organizations.GetCachedCommandCollection());
        ddlCommands.DataSource = cCommandCollection;
        ddlCommands.DataBind();
        ddlCommands.Items.Insert(0, " All ");
    }

    protected void GetMajorCommandCollection()
    {
        ArrayList cMajorCommandCollection = new ArrayList(Personnel.Organizations.GetCachedMajorCommandCollection());
        ddlMajorCommands.DataSource = cMajorCommandCollection;
        ddlMajorCommands.DataBind();
        ddlMajorCommands.Items.Insert(0, " All ");
    }

    protected void ddlMajorCommands_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindIndividualAssignments();
    }

    protected void ddlCommands_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindIndividualAssignments();
    }

    protected void ddlOrgainizations_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindIndividualAssignments();
    }
}