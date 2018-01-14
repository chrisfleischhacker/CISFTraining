using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using Personnel;
using System.Diagnostics;
using TrainingLibrary;

public partial class TrainingUsers : System.Web.UI.Page
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
            Session["rLetter"] = " All ";
            rblPersonnelType.SelectedValue = " All ";
            //rblIsUserAccountEnabled.SelectedValue = " All ";
            //ddlMajorCommands.SelectedValue = " All ";
            //ddlCommands.SelectedValue = " All ";
            //ddlOrgainizations.SelectedValue = " All ";

            BindLetters();
            GetPersonnelCollection();
            GetMajorCommandCollection();
            GetCommandCollection();
            GetOrganizationCollection();
            BindTrainingList();
        }
    }

    protected void GetPersonnelCollection()
    {
        Stopwatch watch = new Stopwatch();
        watch.Start();

        ArrayList cPersonnelCollection = new ArrayList(Personnel.AllPersonnel.GetCachedPersonnelCollection());

        watch.Stop();
        Label mplblMessage = (Label)((SiteMaster)this.Master).FindControl("lblMessage");
        mplblMessage.Text = cPersonnelCollection.Count.ToString() + " Total Records";
        mplblMessage.Text += " Loaded in " + watch.Elapsed.Seconds.ToString() + " seconds.";
        dlPersonnel.DataSource = cPersonnelCollection;
        dlPersonnel.DataBind();
        dlPersonnel.Visible = true;
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

    private void BindLetters()
    {
        ArrayList values = new ArrayList();
        string[] letters = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", 
                             "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", 
                             "U", "V", "W", "X", "Y", "Z", " All "};
        foreach (string letter in letters)
        {
            values.Add(new PositionData(letter));
        }
        rLetters.DataSource = values;
        rLetters.DataBind();
    }

    public class PositionData
    {
        private string letter;
        public PositionData(string letter)
        {
            this.letter = letter;
        }
        public string Letter
        {
            get
            {
                return letter;
            }
        }
    }

    protected void btnRefreshCache_Click(object sender, EventArgs e)
    {
        if (Cache["PersonnelCollection"] != null)
        { Cache.Remove("PersonnelCollection"); }
        Response.Redirect(".");
    }

    protected void rLetters_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        Session["rLetter"] = ((Button)e.CommandSource).Text;
        foreach (RepeaterItem rItem in rLetters.Items)
        {
            btnFind = (Button)(rItem.FindControl("btnLetter"));
            if (btnFind.Text == Session["rLetter"].ToString())
            {
                //btnFind.ForeColor = System.Drawing.Color.Blue;
                //btnFind.Font.Bold = true;
                btnFind.CssClass = "buttonTextBold";
            }
            else
            {
                btnFind.CssClass = "buttonText";
                //btnFind.ForeColor = System.Drawing.Color.Black;
                //btnFind.Font.Bold = false;
            }
        }
        if (Session["rLetter"].ToString() == " All ")
        {
            txtFind.Text = "";
            rblPersonnelType.SelectedValue = " All ";
            //rblIsUserAccountEnabled.SelectedValue = " All ";
            ddlOrgainizations.SelectedValue = " All ";
        }
        ExtractSubCollection();
    }

    protected void rblPersonnelType_SelectedIndexChanged(object sender, EventArgs e)
    {
        ExtractSubCollection();
    }

    protected void rblIsUserAccountEnabled_SelectedIndexChanged(object sender, EventArgs e)
    {
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

        ArrayList cachedUserCollection = new ArrayList(Personnel.AllPersonnel.GetCachedPersonnelCollection());

        ArrayList alFromFind = new ArrayList();
        if (txtFind.Text != "")
        {
            foreach (Person userFind in cachedUserCollection)
            {
                if ((userFind.DisplayName.ToUpper().Contains(txtFind.Text.ToUpper()) == true)
                    || ((userFind.EmailAddress != null) && (userFind.EmailAddress.ToUpper().Contains(txtFind.Text.ToUpper()) == true))
                    || ((userFind.DsnNumber != null) && (userFind.DsnNumber.ToUpper().Contains(txtFind.Text.ToUpper()) == true))
                    || ((userFind.PhoneNumber != null) && (userFind.PhoneNumber.ToUpper().Contains(txtFind.Text.ToUpper()) == true))
                    || ((userFind.Organization != null) && (userFind.Organization.ToUpper().Contains(txtFind.Text.ToUpper()) == true))
                    || ((userFind.Rank != null) && (userFind.Rank.ToUpper().Contains(txtFind.Text.ToUpper()) == true))
                    || (userFind.WindowsLogonUserName.ToUpper().Contains(txtFind.Text.ToUpper()) == true)
                    )
                {
                    alFromFind.Add(userFind);
                }
            }
        }
        else
        {
            alFromFind = cachedUserCollection;
        }
        ArrayList alFromSelectedLetter = new ArrayList();
        if ((Session["rLetter"].ToString() == " All ") || (Session["rLetter"].ToString() == ""))
        {
            alFromSelectedLetter = alFromFind;
        }
        else
        {
            foreach (Person userLetter in alFromFind)
            {
                if (userLetter.DisplayName.Substring(0, 1).ToUpper() == Session["rLetter"].ToString().ToUpper())
                {
                    alFromSelectedLetter.Add(userLetter);
                }
            }
        }
        ArrayList alFromSelectedPersonnelType = new ArrayList();
        if (rblPersonnelType.SelectedItem.Value == " All ")
        {
            alFromSelectedPersonnelType = alFromSelectedLetter;
        }
        else
        {
            foreach (Person userType in alFromSelectedLetter)
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
        //ArrayList alFromSelectedIsEnabled = new ArrayList();
        //if (rblIsUserAccountEnabled.SelectedItem.Value == " All ")
        //{
        //    alFromSelectedIsEnabled = alFromSelectedPersonnelType;
        //}
        //else
        //{
        //    foreach (Person userAccountEnabled in alFromSelectedPersonnelType)
        //    {
        //        if (userAccountEnabled.IsUserAccountEnabled == Convert.ToBoolean(rblIsUserAccountEnabled.SelectedItem.Value))
        //        {
        //            alFromSelectedIsEnabled.Add(userAccountEnabled);
        //        }
        //    }
        //}

        ArrayList alFromSelectedMajorCommand = new ArrayList();
        if (ddlMajorCommands.SelectedItem.Value == " All ")
        {
            alFromSelectedMajorCommand = alFromSelectedPersonnelType;
        }
        else
        {
            foreach (Person userMajorCommands in alFromSelectedPersonnelType)
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

        watch.Stop();
        Label mplblMessage = (Label)((SiteMaster)this.Master).FindControl("lblMessage");
        mplblMessage.Text = alFromSelectedOrganization.Count.ToString() + " Total Records";
        mplblMessage.Text += " Loaded in " + watch.Elapsed.Seconds.ToString() + " seconds.";
        dlPersonnel.DataSource = alFromSelectedOrganization;
        dlPersonnel.DataBind();
        dlPersonnel.Visible = true;
        BindTrainingList();
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

    protected bool IsEnabled()
    {
        bool isEnabled = false;
        if (Convert.ToBoolean(Eval("IsUserAccountEnabled")) == true)
        {
            isEnabled = true;
        }
        return isEnabled;
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
        if (Eval("PhoneNumber") != DBNull.Value)
        {
            phone = Eval("PhoneNumber").ToString();
        }
        else if (Eval("DSNNumber") != DBNull.Value)
        {
            phone = Eval("DSNNumber").ToString();
        }
        return phone;
    }

    private void BindTrainingList()
    {
        DropDownList ddlTraining = (DropDownList)this.dlPersonnel.Controls[this.dlPersonnel.Controls.Count - 1].Controls[0].FindControl("ddlTraining"); //finds the control in the footer of a datalist
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
}


