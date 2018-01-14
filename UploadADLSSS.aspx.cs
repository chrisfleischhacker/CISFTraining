using System;
using System.Data;
using System.Data.Common;
using System.Collections;
using TrainingLibrary;
using Personnel;

public partial class UploadADLSSS : System.Web.UI.Page
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

    protected void btnUploadADLSSS_Click(object sender, EventArgs e)
    {
        if ((fuUploadADLSSS.PostedFile != null) && (fuUploadADLSSS.PostedFile.ContentLength > 0) && (fuUploadADLSSS.PostedFile.FileName.ToLower().Contains(".xlsx")))
        {
            try
            {
                fuUploadADLSSS.PostedFile.SaveAs(Server.MapPath("XLS") + "\\ADLSSS.xlsx");
                ProcessSpreadSheet(Server.MapPath("XLS") + "\\ADLSSS.xlsx");
                DeleteExcelFile(Server.MapPath("XLS") + "\\ADLSSS.xlsx");
            }
            catch (Exception ex)
            {
                lblUploadMessage.Text = ex.ToString();
            }
        }
        else
        {
            lblUploadMessage.Text = "There is a problem with the file. It is either empty or not an Excel spreadsheet.";
        }
    }

    private void ProcessSpreadSheet(string path)
    {
        int training = Convert.ToInt32(ddlTraining.SelectedValue);
        string message = "";
        string creditedtrainees = "";
        string uncreditedtrainees = "";
        bool credited = false;
        string edipi = "";
        message = "The following personnel have been credited for " + ddlTraining.SelectedItem.ToString();
        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source='" + path + @"';Extended Properties=""Excel 12.0;HDR=YES;""";
        DbProviderFactory factory = DbProviderFactories.GetFactory("System.Data.OleDb");
        using (DbConnection connection = factory.CreateConnection())
        {
            connection.ConnectionString = connectionString;
            using (DbCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT [User], [Done], [Last Completion] FROM [Sheet1$]";
                connection.Open();
                using (DbDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        if ((dr["User"] != System.DBNull.Value) && (dr["Done"] != System.DBNull.Value) && (dr["Last Completion"] != System.DBNull.Value))
                        {
                            credited = false;
                            DateTime datetime;
                            string user = dr["User"].ToString().ToUpper();
                            string done = dr["Done"].ToString().ToUpper();
                            string lc = (DateTime.TryParse(dr["Last Completion"].ToString(), out datetime) ? Convert.ToDateTime(dr["Last Completion"]).ToString("MM/dd/yyyy") : "");
                            if (done == "YES")
                            {
                                edipi =  MatchPersonToEDIPI(user);
                                if (edipi != "")
                                {
                                    credited = CreditUserTraining(training, Convert.ToDateTime(lc), edipi);
                                    if (credited)
                                    {
                                        creditedtrainees += "<br/>" + user;
                                    }
                                }

                            }
                            else { uncreditedtrainees += "<br/>" + user; }
                        }
                    }
                }
                connection.Close();                        
            }
        }
        lblUploadMessage.Text = message + creditedtrainees;
    }

    private string MatchPersonToEDIPI(string user)
    {
        string edipi = "";
        ArrayList cachedUserCollection = new ArrayList(Personnel.AllPersonnel.GetCachedPersonnelCollection());
        foreach (Person userFind in cachedUserCollection)
        {
            if (userFind.DisplayName.ToUpper().Contains(user.ToUpper()) == true)
            {
                edipi = userFind.WindowsLogonUserName;
            }
        }
        return edipi;
    }

    private static bool CreditUserTraining(int trainingident, DateTime lastcompleted, string edipi)
    {
        int added = 0;
        added = Assignment.InsertAssignmentCompletionDate(trainingident, lastcompleted, edipi);
        if (added > 0){return true;}else{return false;}
    }
    
    public void DeleteExcelFile(string path)
    {
        //Delete temporary Excel file from the Server path
        if (System.IO.File.Exists(path))
        {
            System.IO.File.Delete(path);
        }
    }
}
