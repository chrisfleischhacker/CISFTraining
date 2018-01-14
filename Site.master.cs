using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using Personnel;
using TrainingLibrary;

public partial class SiteMaster : System.Web.UI.MasterPage
{
    string iAm;
    Person IAM;

    protected void Page_Load(object sender, EventArgs e)
    {
        iAm = Person.LogonUserIdentity();

        if ((Training.IOwnTraining(iAm)) || (System.Configuration.ConfigurationManager.AppSettings["TrainingSupportManagers"].Contains(iAm)))
        {
            MenuItem mi = new MenuItem();
            mi.Text = "Manage Training";
            mi.NavigateUrl = "ManageTraining.aspx";
            NavigationMenu.Items.Add(mi);

            MenuItem mit = new MenuItem();
            mit.Text = "Training Status";
            mit.NavigateUrl = "TrainingStatus.aspx";
            NavigationMenu.Items.Add(mit);
        }
    }
}
