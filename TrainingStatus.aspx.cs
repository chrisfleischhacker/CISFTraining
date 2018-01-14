using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using Personnel;
using System.Diagnostics;
using System.Text;
using TrainingLibrary;
using System.Net.Mail;

public partial class TrainingStatus : System.Web.UI.Page
{

    string iAm;
    Person IAM;

    protected void Page_Load(object sender, EventArgs e)
    {
        iAm = Person.LogonUserIdentity();

        if ((!Training.IOwnTraining(iAm)) && (!System.Configuration.ConfigurationManager.AppSettings["TrainingSupportManagers"].Contains(iAm)))
        {
            Response.Redirect("Default.aspx");
        }

        if (!IsPostBack)
        {
            BindTrainingList();
        
            ddlMajorCommands.SelectedValue = " All ";
            ddlCommands.SelectedValue = " All ";
            ddlOrgainizations.SelectedValue = " All ";
            rblPersonnelType.SelectedValue = " All ";
        }
    }

    private void BindTrainingList()
    {
        DataTable dt = new DataTable();
        if (System.Configuration.ConfigurationManager.AppSettings["TrainingSupportManagers"].Contains(iAm))
        {
            dt = TrainingList.GetAllTrainingDataTable();
        }
        else
        {
            dt = TrainingList.GetAllTrainingIOwnDataTable(iAm);
        }
        ddlTraining.DataSource = dt;
        ddlTraining.DataTextField = dt.Columns[0].ToString();
        ddlTraining.DataValueField = dt.Columns[1].ToString();
        ddlTraining.DataBind();
    }

    protected void ddlTraining_SelectedIndexChanged(object sender, EventArgs e)
    {
        //txtFind.Text = "";
        chkLT30.Checked = false;
        chkOverdue.Checked = false;
        int training_ident;
        training_ident = Convert.ToInt32(ddlTraining.SelectedValue);
        CleanUpAssignmentsAndCompletions(training_ident);
        LoadTraining(training_ident);
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

    protected void LoadTraining(int training_ident)
    {
        Training training = Training.ThisTraining(training_ident);
        StringBuilder trainingdetails = new StringBuilder();
        trainingdetails.Append("<h2 align='center'>" + training.Name + "</h2>");
        trainingdetails.Append("<p align='center'>");
        if (training.Notes != "")
        {
            trainingdetails.Append(training.Notes + "<br />");
        }
        if (!training.URL.ToString().ToLower().Contains("52tdka"))
        {
            trainingdetails.Append("This is an external training and you will get credited for its completion by your Training Support Manager.<br />");
        }
        if (training.RenewalMonths > 0)
        {
            trainingdetails.Append("Assigned personnel must retake this training every " + training.RenewalMonths + " months from the " + (training.URL.ToUpper().Contains("52TDKA") ? " assignment " : " completion ") + " date.<br />");
        }
        if (training.PassingScore > 0)
        {
            trainingdetails.Append("The passing score for the test is " + training.PassingScore + "%.</p>");
        }
        lblTraining.Text = trainingdetails.ToString();
        GetPersonnelCollection(training_ident);
    }

    protected void GetPersonnelCollection(int training_ident)
    {
        Stopwatch watch = new Stopwatch();
        watch.Start();

        ArrayList alTrainingPersonList = new ArrayList(TrainingPerson.TrainingPersonList(training_ident));

        watch.Stop();
        Label mplblMessage = (Label)((SiteMaster)this.Master).FindControl("lblMessage");
        mplblMessage.Text = alTrainingPersonList.Count.ToString() + " Total Records";
        mplblMessage.Text += " Loaded in " + watch.Elapsed.Seconds.ToString() + " seconds.";
        dlPersonnel.DataSource = alTrainingPersonList;
        dlPersonnel.DataBind();
        dlPersonnel.Visible = true;

        GetMajorCommandCollection();
        GetCommandCollection();
        GetOrganizationCollection();    
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
        ExtractSubCollection();
    }

    protected void ddlCommands_SelectedIndexChanged(object sender, EventArgs e)
    {
        ExtractSubCollection();
    }

    protected void ddlOrgainizations_SelectedIndexChanged(object sender, EventArgs e)
    {
        ExtractSubCollection();
    }

    protected void rblPersonnelType_SelectedIndexChanged(object sender, EventArgs e)
    {
        ExtractSubCollection();
    }


    protected void chkNoCompletion_CheckedChanged(object sender, EventArgs e)
    {
        chkLT30.Checked = false; 
        chkOverdue.Checked = false;
        ExtractSubCollection();
    }

    protected void chkLT30_CheckedChanged(object sender, EventArgs e)
    {
        chkNoCompletion.Checked = false;
        chkOverdue.Checked = false;
        ExtractSubCollection();
    }
    
    protected void chkOverdue_CheckedChanged(object sender, EventArgs e)
    {
        chkNoCompletion.Checked = false;
        chkLT30.Checked = false;
        if (chkOverdue.Checked) 
        { 
            btnEmailOverdue.Visible = true; 
        } else { 
            btnEmailOverdue.Visible = false;
            lblEmailResult.Text = "";
            lblEmailResult.Visible = false;
        }
        ExtractSubCollection();
    }

    protected void btnFind_Click(object sender, EventArgs e)
    {
        ExtractSubCollection();
    }

    private void ExtractSubCollection()
    {
        Stopwatch watch = new Stopwatch();
        watch.Start();

        int training_ident;
        training_ident = Convert.ToInt32(ddlTraining.SelectedValue);

        ArrayList alTrainingPersonList = new ArrayList(TrainingPerson.TrainingPersonList(training_ident));

        ArrayList alFromFind = new ArrayList();
        //if (txtFind.Text != "")
        //{
        //    foreach (TrainingPerson userFind in alTrainingPersonList)
        //    {
        //        if ((userFind.DisplayName.ToUpper().Contains(txtFind.Text.ToUpper()) == true)
        //            || ((userFind.EmailAddress != null) && (userFind.EmailAddress.ToUpper().Contains(txtFind.Text.ToUpper()) == true))
        //            || ((userFind.DsnNumber != null) && (userFind.DsnNumber.ToUpper().Contains(txtFind.Text.ToUpper()) == true))
        //            || ((userFind.PhoneNumber != null) && (userFind.PhoneNumber.ToUpper().Contains(txtFind.Text.ToUpper()) == true))
        //            || ((userFind.Organization != null) && (userFind.Organization.ToUpper().Contains(txtFind.Text.ToUpper()) == true))
        //            || (userFind.WindowsLogonUserName.ToUpper().Contains(txtFind.Text.ToUpper()) == true)
        //            )
        //        {
        //            alFromFind.Add(userFind);
        //        }
        //    }
        //}
        //else
        //{
            alFromFind = alTrainingPersonList;
        //}

        ArrayList alFromSelectedPersonnelType = new ArrayList();
        if (rblPersonnelType.SelectedItem.Value == " All ")
        {
            alFromSelectedPersonnelType = alFromFind;
        }
        else
        {
            foreach (TrainingPerson userType in alFromFind)
            {
                if (((userType.EmployeeType != null)
                    && ((userType.EmployeeType.ToUpper() == rblPersonnelType.SelectedItem.Value.ToUpper())))
                    || ((userType.EmployeeType != null)
                    && ((rblPersonnelType.SelectedItem.Value.ToUpper() == "V")
                    && (userType.EmployeeType.ToUpper() == "A"))))
                {
                    alFromSelectedPersonnelType.Add(userType);
                }
            }
        }

        ArrayList alFromSelectedMajorCommand = new ArrayList();
        if (ddlMajorCommands.SelectedItem.Value == " All ")
        {
            alFromSelectedMajorCommand = alFromSelectedPersonnelType;
        }
        else
        {
            foreach (TrainingPerson userMajorCommands in alFromSelectedPersonnelType)
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
            foreach (TrainingPerson userCommands in alFromSelectedMajorCommand)
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
            foreach (TrainingPerson userOfficeSymbols in alFromSelectedCommand)
            {
                if (userOfficeSymbols.Organization.ToUpper() == ddlOrgainizations.SelectedItem.Value.ToUpper())
                {
                    alFromSelectedOrganization.Add(userOfficeSymbols);
                }
            }
        }

        ArrayList alFromChkNoCompletion = new ArrayList();
        if (!chkNoCompletion.Checked)
        {
            alFromChkNoCompletion = alFromSelectedOrganization;
        }
        else
        {
            foreach (TrainingPerson userNoCompletion in alFromSelectedOrganization)
            {
                if (userNoCompletion.LastCompleted == null)
                {
                    alFromChkNoCompletion.Add(userNoCompletion);
                }
            }
        }

        ArrayList alFromChkLT30 = new ArrayList();
        if (!chkLT30.Checked)
        {
            alFromChkLT30 = alFromChkNoCompletion;
        }
        else
        {
            foreach (TrainingPerson userLT30 in alFromChkNoCompletion)
            {
                if (Convert.ToBoolean(userLT30.DueLT30))
                {
                    alFromChkLT30.Add(userLT30);
                }
            }
        }

        ArrayList alFromChkOverdue = new ArrayList();
        //StringBuilder recipientAddresses = new StringBuilder();
        if (!chkOverdue.Checked)
        {
            alFromChkOverdue = alFromChkLT30;
        }
        else
        {
            foreach (TrainingPerson userOverdue in alFromChkLT30)
            {
                if (Convert.ToBoolean(userOverdue.Overdue))
                {
                    alFromChkOverdue.Add(userOverdue);
                    //recipientAddresses.Append(" " + userOverdue.EmailAddress.ToString() + ";");
                }
            }
        }

        watch.Stop();
        Label mplblMessage = (Label)((SiteMaster)this.Master).FindControl("lblMessage");
        mplblMessage.Text = alFromChkOverdue.Count.ToString() + " Total Records";
        mplblMessage.Text += " Loaded in " + watch.Elapsed.Seconds.ToString() + " seconds.";
        dlPersonnel.DataSource = alFromChkOverdue;
        dlPersonnel.DataBind();
        dlPersonnel.Visible = true;

        //if (chkOverdue.Checked)
        //{
        //    HyperLink lbtnOverdue = new HyperLink();
        //    lbtnOverdue.Text = "Email Overdue";
        //    lbtnOverdue.NavigateUrl = @"mailto:" + recipientAddresses.ToString().TrimEnd(';') + @"?subject=Overdue%20Training&body=You%20are%20required%20to%20complete "+ training_name + @"%0A" + System.Configuration.ConfigurationManager.AppSettings["TakeTrainingURLString"].ToString() + training_ident.ToString();
        //    phEmailOverdue.Controls.Add(lbtnOverdue);
        //}
    }

    protected string Link2UserTraining()
    {
        string strID = "";
        if (Eval("WindowsLogonUserName") != DBNull.Value)
        {
            strID = "UsersTraining.aspx?user=" + Eval("WindowsLogonUserName");
        }
        return strID;
    }

    protected string PhoneOrDSN()
    {
        string phone = "";
        
        if ((Eval("PhoneNumber") != DBNull.Value) && (Eval("PhoneNumber") != null))
        {
            phone = Eval("PhoneNumber").ToString().Replace("(719)-", "").Replace("719-", "").Replace("(719) ", "").Replace("719 ", "");
        }
        else if ((Eval("DSNNumber") != DBNull.Value) && (Eval("PhoneNumber") != null))
        {
            phone = Eval("DSNNumber").ToString().Replace("(719)-", "").Replace("719-", "").Replace("(719) ", "").Replace("719 ", "");
        }
        return phone;
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
            if (Convert.ToBoolean(Eval("DueLT30"))) { formattedDate += @"<br />&#060; 30 Days"; }
            if (Convert.ToBoolean(Eval("Overdue"))) { formattedDate += @"<br />Overdue!"; }
        }
        if (Convert.ToDateTime(Eval("NextRequired")).Year > 9990)
        {
            formattedDate = "";
        }
        return formattedDate;
    }

    protected string SendEmail()
    {
        string strEmail = "";
        if (Eval("EMAILADDRESS") != DBNull.Value)
        {
            strEmail = "mailto:" + Eval("EMAILADDRESS");
        }
        return strEmail;
    }

    protected void btnUnAssignSelected_Click(object sender, EventArgs e)
    {
    }

    protected void btnEmailSelected_Click(object sender, EventArgs e)
    {
    }

    protected void btnCreditSelected_Click(object sender, EventArgs e)
    {
        string message = "";
        string trainees = "";
        try
        {
            DropDownList ddlTraining = (DropDownList)this.dlPersonnel.Controls[this.dlPersonnel.Controls.Count - 1].Controls[0].FindControl("ddlTraining"); //finds the control in the footer of a datalist
            int training_ident = Convert.ToInt32(ddlTraining.SelectedValue);
            message = "The following personnel have been credited for " + ddlTraining.SelectedItem.ToString();
            TextBox txtCompletionDate = (TextBox)this.dlPersonnel.Controls[dlPersonnel.Controls.Count - 1].Controls[0].FindControl("txtCompletionDate");
            DateTime dt = DateTime.Parse(txtCompletionDate.Text);
            if (dt != DateTime.MinValue && dt != DateTime.MaxValue)
            {
                foreach (Control var in dlPersonnel.Controls)
                {
                    CheckBox ch = (CheckBox)var.FindControl("chkSelect");
                    if (ch != null)
                    {
                        if (ch.Checked)
                        {
                            Label lblEDIPI = (Label)var.FindControl("lblEDIPI");
                            HyperLink hlWhoAmI = (HyperLink)var.FindControl("hlWhoAmI");
                            Assignment.InsertAssignmentCompletionDate(training_ident, Convert.ToDateTime(txtCompletionDate.Text), lblEDIPI.Text);
                            trainees += @"<br/>" + hlWhoAmI.Text;
                        }
                    }
                }
            }
        }
        catch (Exception eee)
        {
            message = eee.InnerException.ToString();
        }
        lblMessage.Text = message + trainees;
        lblMessage.Visible = true;
        dlPersonnel.Visible = false;
    }

    protected void btnEmailOverdue_Click(object sender, EventArgs e)
    {
        IAM = Person.GetPersonFromActiveDirectory(iAm);
        Training training = Training.ThisTraining(Convert.ToInt32(ddlTraining.SelectedValue));
        MailMessage mailMessage = new MailMessage();
        mailMessage.From = new MailAddress(IAM.EmailAddress.ToString());
        mailMessage.Subject = "Overdue Training";
        mailMessage.Body = "Please complete " + training.Name + " at " + System.Configuration.ConfigurationManager.AppSettings["TakeTrainingURLString"].ToString() + training.Ident.ToString();
        ArrayList alOverdue = new ArrayList(TrainingPerson.TrainingPersonList(training.Ident));
        foreach (TrainingPerson userOverdue in alOverdue)
        {
            if (Convert.ToBoolean(userOverdue.Overdue))
            {
                mailMessage.To.Add(userOverdue.EmailAddress.ToString());
            }
        }
        try
        {
            //string xxx = mailMessage.To.ToString();
            SmtpClient smtpClient = new SmtpClient(System.Configuration.ConfigurationManager.AppSettings["SMTPHost"].ToString());
            smtpClient.Send(mailMessage);
            lblEmailResult.ForeColor = System.Drawing.Color.Maroon;
            lblEmailResult.Text = "Overdue email sent.";
            btnEmailOverdue.Visible = false;
        }
        catch (Exception ex)
        {
            lblEmailResult.Text = ex.InnerException.ToString();
        }
        lblEmailResult.Visible = true;
    }
}

