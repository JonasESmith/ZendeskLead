// ************************************************************************************************************************************************************* //
//                                                                                                                                                               //
// PROGRAMMER     : JONAS SMITH                                                                                                                                  //
// PROGRAMNAME    : EMAIL_SCRIPT                                                                                                                                 //
// PURPOSE        : Look at the yourEmailAddress@gmail.com gmail account and retrieve all emails that are from pinnacledistro.com, then catergorize              //
//                      each email under new receipt or new register. If new register this application creates a bash script that will export important          //
//                      information to zendesk, this will allow sales reps to only have to use one service instead of multiple. For the receipts we              //
//                      will need to create a bash script as well, And send in a new deal to the associated sales rep.                                           //
// DOCUMENTATION  : 1. https://developers.getbase.com/docs/rest/reference/leads                                                                                  //
//                  2. https://stackoverflow.com/questions/15278583/creating-sh-file-to-execute-visual-studio-2010-project                                       //
//                  3.                                                                                                                                           //
//                                                                                                                                                               //
// ************************************************************************************************************************************************************* //


using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Data.SqlClient;
using Google.Apis.Gmail.v1;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net.Mail;
using System.Drawing;
using System.Linq;
using System.Net;
using System.IO;
using System;

namespace Task_Form
{
  public partial class EmailMain : Form
  {
    // paths
    string path    = AppDomain.CurrentDomain.BaseDirectory + "emailID.txt";
    string dayPath = AppDomain.CurrentDomain.BaseDirectory + "emailID" + DateTime.Now.ToString("-yyyy-MM-dd") + ".txt";

    // loading global variables.
    int count      = 0;
    int emailsSent = 0;
    SalesRep GlobalSales = new SalesRep();

    // this will receive the access token given to you by Zendesk
    const string AccessToken = "YOUR ACCESS TOKEN";
    Stopwatch stopWatch;
    System.Windows.Forms.Timer displayTimer;
    System.Timers.Timer timer = new System.Timers.Timer();
    GoogleAPI googleAPI;

    ZendeskOrder ZenOrder = new ZendeskOrder();

    // init global lists
    List<SalesRep> salesReps      = new List<SalesRep>();
    List<string>   emailIdList    = new List<string>();
    List<string>   emailDayIdList = new List<string>();
    // simple color that is used in the salesrep buttons
    Color panelColor = SystemColors.ControlLight;


    public EmailMain()
    {
      InitializeComponent(); googleAPI = new GoogleAPI();

      InitEmailIdList();     InitSalesReps();

      RunGoogleAPI();        StartTimers();

      LoadSalesButtons();

      // Google.Apis.Gmail.v1.Data.Message message = googleAPI.GetMessage("MessageID");
      // ZenOrder.ConvertToLineItems(message, googleAPI);
    }

    public void InitEmailIdList()
    {
      string[] lines = File.ReadAllLines(path);

      foreach (string line in lines)
      {
        emailIdList.Add(line);
      }

      if (File.Exists(dayPath))
      {
        string[] dayLines = File.ReadAllLines(dayPath);

        foreach (string line in dayLines)
        {
          emailDayIdList.Add(line);
        }
      }
    }

    // hard coded sales reps, this could be put into a file but for now I am fine with this. 
    public void InitSalesReps()
    {
      salesReps.Add(new SalesRep("Sales-Rep Name", "SalesRep@email.com", 000000));
    }

    // creates a list of all current emails and determines if any of the emails is from pinnacledistro.com
    public void RunGoogleAPI()
    {
      // this is the date element passed to know which days to check for new emails.
      string date = DateTime.Now.ToString("yyyy/MM/dd");
      List<Google.Apis.Gmail.v1.Data.Message> messageList = googleAPI.GetMessageList(string.Format("from:yourEmailAddress@gmail.com in:inbox after:{0}", date));

      Google.Apis.Gmail.v1.Data.Message message;

      // for all messages in the messageList iterate through and check if the email Id exists in our listing.
      for (int i = 0; i < messageList.Count; i++)
      {
        string subject = "";
        string value = emailIdList.Where(x => x.Contains(messageList[i].Id)).FirstOrDefault();

        // if the string value is null then it does not exist in the emails sent list so it will need to be sent
        // however we need to do a check inside of getNewEmail so we can determine if the email will be sent as a 
        // lead or as a order.
        if (string.IsNullOrEmpty(value))
        {
          // return value of email message
          message = googleAPI.GetMessage(messageList[i].Id);

          // return message subject
          subject = googleAPI.GetEmailSubject(message);

          // This will build the BashFile to update zendesk with the new lead.
          if(subject.Contains("New customer registration"))
          {
            BuildBashFile(messageList[i].Id);
          }
          // this for now is emailing the salesrep with a new order purchase.
          //    I want this to create a bashfile that will udpate zendesk with the 
          //    correct order information.
          else if (subject.Contains("Purchase Receipt for Order"))
          {
            // I will also have this email me personally with the information of the bash file 
            // this will allow me ot better understand who and when the project added the values.
            List<string> productList = ZenOrder.ConvertToLineItems(message, googleAPI);
            ZenOrder.ConvertToCartWithProducts(productList);
          }

          // return body 
          string body = googleAPI.GetEmailBody(message);
        }
      }
    }

    // starts timers for checking the email account for new purchases.
    public void StartTimers()
    {
      timer = new System.Timers.Timer(60 * 1000);
      timer.Elapsed += Timer_Elapsed;
      timer.Start();

      displayTimer = new System.Windows.Forms.Timer();
      displayTimer.Interval = (1000);
      displayTimer.Tick += DisplayTimer_Tick;
      displayTimer.Start();

      stopWatch = new Stopwatch();
      stopWatch.Start();
    }

    private void DisplayTimer_Tick(object sender, EventArgs e)
    {
      timer_Label.Text     = stopWatch.Elapsed.Seconds.ToString("00");
      script_runs.Text     = count.ToString("0000");
      num_Emails_Sent.Text = emailsSent.ToString("000");
      Application.DoEvents();
    }

    // Start GoogleAPI call 
    private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
      count++;
      // This will call the check method that I created!
      RunGoogleAPI();
    }




    // *********************************************** //
    //  This section creates a bash file, to be execu- //
    //    -ted to allow forz zendesk to be updated     //
    //    with important customer information.         //
    //                                                 //
    // *********************************************** //
    #region ZENDESK ADD LEAD...
    void Return_Customer_Attributes(ref Lead lead, string messageID)
    {
      try
      {
        Google.Apis.Gmail.v1.Data.Message message = googleAPI.GetMessage(messageID);

        string connectString = "Server=tcp:server.database.windows.net,1433;Initial Catalog={databaseName};Persist Security Info=False;User ID={yourIdHere};" +
                               "Password={yourPassword};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        string customerId = "";
        string body = googleAPI.GetEmailBody(message);

        body = body.Replace("<p>", "");
        body = body.Replace("</p>", "");
        body = body.Replace("\r", "");
        body = body.Replace("\n", "");
        body = body.Replace("<br/>", ",");
        body = body.Replace("<br />", ",");

        string[] words = body.Split(',');

        for (int i = 0; i < words.Length; i++)
        {
          if (words[i].Contains("Full name"))
          {
            string lastNames = "";

            string fullName = words[i].Replace("Full name: ", "");
            string[] names = fullName.Split(' ');

            List<string> nameList = new List<string>();

            for(int j = 0; j < names.Length; j++)
            {
              if(!string.IsNullOrEmpty(names[j]))
              {
                nameList.Add(names[j]);
              }
            }

            if (nameList.Count <= 2)
              lead.last_name = nameList[1];
            else
            {
              for (int j = 1; j < nameList.Count; j++)
              {
                lastNames += nameList[j] + " ";
              }
              lead.last_name = lastNames;
            }

            lead.first_name = nameList[0];
          }
          else if (words[i].Contains("Position:"))
          {
            string position = words[i].Replace("Position: ", "");
            lead.title = position;
          }
          else if (words[i].Contains("Store Name:"))
          {
            string storeName = words[i].Replace("Store Name: ", "");
            lead.organization_name = storeName;
          }
          else if (words[i].Contains("CustomerID:"))
          {
            customerId = Regex.Replace(words[i].Replace("CustomerID:", ""), @"\s+", "");
          }
          else if (words[i].Contains("Email:"))
          {
            lead.email = Regex.Replace(words[i].Replace("Email:", ""), @"\s+", "");
          }

          lead.industry = "CBD";
        }

        customerId = null;

        if (string.IsNullOrEmpty(customerId))
        {
          string idqueryString = string.Format("Select ID from Customer Where USERNAME = '{0}'", lead.email);


          using (SqlConnection connection = new SqlConnection(connectString))
          {

            SqlCommand command = new SqlCommand(idqueryString, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            try
            {
              while (reader.Read())
                if (!string.IsNullOrEmpty(reader["ID"].ToString()))
                  customerId = reader["ID"].ToString();
            }
            finally
            {
              reader.Close();
            }
          }
        }

        string queryString = string.Format( "Select * from Customer Where id = {0}", customerId );

        using (SqlConnection connection = new SqlConnection(connectString))
        {
          SqlCommand command = new SqlCommand(queryString, connection);
          // command.Parameters.AddWithValue();
          connection.Open();
          SqlDataReader reader = command.ExecuteReader();

          try
          {
            while(reader.Read())
              if (!string.IsNullOrEmpty( reader["Email"].ToString() ))
                lead.email = reader["email"].ToString();
          }
          finally
          {
            reader.Close();
          }
        }

        List<string> idList = new List<string>();
        queryString = string.Format("Select ID From GenericAttribute where EntityId = {0}", customerId);
        //queryString = string.Format("Select * From Address where id = {0}", shipingAddress);

        using (SqlConnection connection = new SqlConnection(connectString))
        {

          SqlCommand command = new SqlCommand(queryString, connection);
          connection.Open();
          SqlDataReader reader = command.ExecuteReader();

          try
          {
            while(reader.Read())
              idList.Add(reader["ID"].ToString());
          }
          finally
          {
            reader.Close();
          }
        }

        string customAttributeXML = "";

        for(int i = 0; i < idList.Count(); i++)
        {
          queryString = string.Format("Select * From GenericAttribute where EntityId = {0} and ID = {1}", customerId, idList[i]);

          using (SqlConnection connection = new SqlConnection(connectString))
          {

            SqlCommand command = new SqlCommand(queryString, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            try
            {
              while (reader.Read())
                if (reader["Key"].ToString() == "Phone")
                  lead.phone = reader["Value"].ToString();
                else if (reader["Key"].ToString() == "Company")
                  lead.organization_name = reader["Value"].ToString();
                else if(reader["Key"].ToString() == "CustomCustomerAttributes")
                  customAttributeXML = reader["Value"].ToString();
                else if(reader["Key"].ToString() == "StreetAddress")
                  lead.lineOne = reader["Value"].ToString();
                else if (reader["Key"].ToString() == "City")
                  lead.city = reader["Value"].ToString();
                else if (reader["Key"].ToString() == "StateProvinceId")
                  lead.state = reader["Value"].ToString();
                else if (reader["Key"].ToString() == "CountryId")
                  lead.country = reader["Value"].ToString();
            }
            finally
            { reader.Close(); }
          }
        }

        string CustomAttributes = ProcessXML( customAttributeXML);
        string[] custom         = CustomAttributes.Split(',');
        custom[1]               = custom[1].Replace("'","");
        lead.description        = string.Format("TaxID - {0}", custom[1]);

        SalesRepCheck(custom[0]);

        queryString = string.Format("Select Name From Country where Id = {0}", lead.country);

        using (SqlConnection connection = new SqlConnection(connectString))
        {
          SqlCommand command = new SqlCommand(queryString, connection);
          connection.Open();
          SqlDataReader reader = command.ExecuteReader();

          try
          {
            while (reader.Read())
              if (!string.IsNullOrEmpty(reader["Name"].ToString()))
                lead.country = reader["Name"].ToString();
          }
          finally
          {
            reader.Close();
          }
        }

        if (lead.state != "0")
        {

          queryString = string.Format("Select Name From StateProvince where Id = {0}", lead.state);

          using (SqlConnection connection = new SqlConnection(connectString))
          {
            SqlCommand command = new SqlCommand(queryString, connection);

            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            try
            {
              while (reader.Read())
                if (!string.IsNullOrEmpty(reader["Name"].ToString()))
                  lead.state = reader["Name"].ToString();
            }
            finally
            {
              reader.Close();
            }
          }
        }
        else
          lead.state = null;
      }
      catch (SqlException e)
      {
        Console.WriteLine(e.ToString());
      }
    }

    /// <summary>
    /// Creates the bash file pulling information from an Azure Database, Email, and Zendesk to create a bash file
    ///   to upload a zendesk lead. 
    /// </summary>
    /// <param name="messageId"></param>
    public void BuildBashFile(string messageId)
    {
      string path = AppDomain.CurrentDomain.BaseDirectory + "MakeNewLead.sh";
      Lead newLead = new Lead();

      Return_Customer_Attributes(ref newLead, messageId);

      string originalCurl = string.Format("curl -v POST https://api.futuresimple.com/v2/leads -H \"Accept: application/json\" -H \"Content-Type: application/json\" " +
                                          "-H \"Authorization: Bearer {0} \" ", AccessToken);
      string dataString   = "-d '{ \"data\": { ";
      string firstName    = string.Format("\"first_name\": \"{0}\" ", newLead.first_name ?? "NULL");
      string lastName     = string.Format("\"last_name\": \"{0}\"", newLead.last_name ?? "NULL");
      string organization = string.Format("\"organization_name\": \"{0}\"", newLead.organization_name ?? "NULL");
      string title        = string.Format("\"title\": \"{0}\"", newLead.title ?? "NULL");
      string description  = string.Format("\"description\": \"{0}\"", newLead.description ?? "NULL");
      string industry     = string.Format("\"industry\": \"{0}\"", newLead.industry ?? "NULL");
      string website      = string.Format("\"website\": \"{0}\"", newLead.website ?? "NULL");
      string email        = "";

      if (!string.IsNullOrEmpty(newLead.email))
             email        = string.Format(",\"email\": \"{0}\"", newLead.email ?? " ");
      string phone        = string.Format(",\"phone\": \"{0}\"", newLead.phone ?? "NULL");
      string mobile       = string.Format("\"mobile\": \"{0}\"", newLead.mobile ?? "NULL");
      string fax          = string.Format("\"fax\": \"{0}\"", newLead.fax ?? "NULL");
      string twitter      = string.Format("\"twitter\": \"{0}\"", newLead.twitter ?? "NULL");
      string facebook     = string.Format("\"facebook\": \"{0}\"", newLead.facebook ?? "NULL");
      string linkedin     = string.Format("\"linkedin\": \"{0}\"", newLead.linkedin ?? "NULL");
      string addressStrt  = ("\"address\": { ");
      string address      = string.Format("\"line1\": \"{0}\" , \"city\": \"{1}\", \"postal_code\": \"{2}\", \"state\": \"{3}\", \"country\": \"{4}\"", 
                                          newLead.lineOne,                   newLead.city,     newLead.postal_code ?? "", 
                                          newLead.state ?? "no state given", newLead.country);
      string tags         = (" }, \"tags\": [  ] ");
      string customfields = ("\"custom_fields\": { }");
      string endingString = ("} } '");

      // This needs to retrieve which salesRep the customer is using and determines which owner id to use
      // 
      string ownerID = string.Format(" \"owner_id\": {0}", GlobalSales.owner_id);
      string formatString = string.Format("{0} {1} {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9} {10} {11}, {12}, {13}, {14}, {15}, {16} {17} {18}, {19} {20}", 
                                            originalCurl, dataString,  firstName, lastName,     organization, 
                                            title,   ownerID,     description, industry,  website,      email, 
                                            phone,        fax ,        twitter,   facebook,     linkedin, 
                                            addressStrt , address,     tags,      customfields, endingString);

      // writes the constructed bash script to a file.
      File.WriteAllText(path, formatString);

      // this tries to create a lead inside of zendesk by running the script this 
      // will run the script in a shell form but cannot output any text
      try
      {
        ProcessStartInfo psi = new ProcessStartInfo();
        psi.FileName = path;
        psi.UseShellExecute = true;

        Process p = Process.Start(psi);
        p.WaitForExit();
      }
      catch (Exception e)
      { MessageBox.Show(e.ToString(), "Message with script", MessageBoxButtons.OK); throw;  }
    }

    public void SalesRepCheck(string salesRepName)
    {
      bool hasSales = false;

      salesRepName = salesRepName.ToLower();
      for(int i = 0; i < salesReps.Count; i++)
      {
        if(salesRepName == salesReps[i].name.ToLower())
        {
          GlobalSales = salesReps[i];
          hasSales = true;
          break;
        }
      }

      if (hasSales)
        GlobalSales = salesReps[0];
    }


    // this process will serialize the customXML string and return the data needed.
    public string ProcessXML( string xmlString )
    {
      xmlString = FormatXML(xmlString);

      string[] words = xmlString.Split(',');
      List<string> values = words.ToList<string>();
      values.RemoveAt(values.Count - 1);
      string[,] objectMatrix = new string[words.Length / 2, 2];

      for(int i = 0; i < objectMatrix.Length / 2; i++)
      {
        objectMatrix[i, 0] = values[i*2];
        objectMatrix[i, 1] = values[(i*2) + 1];
      }

      string SalesRep = "";
      string TaxId = "";

        int salesRepId = 6;
        int IENID = 4;

        for (int i = 0; i < objectMatrix.Length / 2; i++)
        {
          if (objectMatrix[i, 0] == salesRepId.ToString())
          {
            SalesRep = objectMatrix[i, 1];
          }
          else if (objectMatrix[i, 0] == IENID.ToString())
          {
            TaxId = objectMatrix[i, 1];
          }
        }


      return salesReps + "," + TaxId;
    }

    public string FormatXML(string xmlString)
    {
      string returnString = "";

      xmlString = xmlString.Replace("><", ">,<");

      string[] xmlWords = xmlString.Split(',');

      for(int i = 0; i < xmlWords.Length; i++)
      {
        string value = xmlWords[i];
        if(value != "<Attributes>" && value != "<CustomerAttributeValue>" && value != "</CustomerAttributeValue>" 
          && value != "</CustomerAttribute>" && value != "</Attributes>")
        {
          if(value.Contains("<CustomerAttribute ID=\""))
          {
            value = value.Replace("<CustomerAttribute ID=\"", "");
            value = value.Replace("\">","");
            returnString += value + ",";
          }
          else if(value.Contains("<Value>"))
          {
            value = value.Replace("<Value>", "");
            value = value.Replace("</Value>", "");
            returnString += value + ",";
          }
        }
      }
      return returnString;
    }

    #endregion




    // *********************************************** //
    //  This is the section for dealing with user      //
    //    emails, these methods send, receive, and     //
    //    format emails to be sent to sales reps/users //
    //                                                 //
    // *********************************************** //
    #region Sending Email methods...

    // sends the email from passed variables. 
    private void send(string repName, string repEmail, string messageId)
    { // creating a mailClient to send to customers
      SmtpClient mailClient                     = new SmtpClient("smtp.gmail.com", 587);
      mailClient.EnableSsl                      = true;
      mailClient.UseDefaultCredentials          = false;
      mailClient.Credentials                    = new NetworkCredential("{yourEmailAddress@gmail.com}", "{yourEmailPassword}");
      MailAddress to                            = new MailAddress(repEmail, repName);
      System.Net.Mail.MailMessage msgMail       = new System.Net.Mail.MailMessage();
      msgMail.From                              = new MailAddress("{yourEmailAddress@gmail.com}", "{name}");
      List<MailAddress> cc                      = new List<MailAddress>();
      Google.Apis.Gmail.v1.Data.Message message = googleAPI.GetMessage(messageId);
      msgMail.To.Add(to);

      foreach (MailAddress addr in cc)
        msgMail.CC.Add(addr);

      msgMail.Subject          = "{subjectLine}";
      msgMail.Body             = googleAPI.GetEmailBody(message);
      msgMail.IsBodyHtml       = true;
      mailClient.Send(msgMail);
      msgMail.Dispose();
      mailClient.Dispose();
    }

    // this just adds a string to a list
    public void AddEmailID(string messageID)
    {
      emailIdList.Add(messageID);
    }

    #endregion


    // *********************************************** //
    //  This is the section for general auxiliary      //
    //    methods for this application, weather it be  //
    //    updating the UI or loading files             //
    //                                                 //
    // *********************************************** //
    #region Aux Methods...

    private void EmailMain_FormClosing(object sender, FormClosingEventArgs e)
    {
      // update emailID list and file with new emailID's
      UpdateEmailIDFile();
      UpdateDayIDFile();
    }

    private void UpdateEmailIDFile()
    {
      TextWriter textWriter = new StreamWriter(path);

      foreach(String emailID in emailIdList)
      {
        textWriter.WriteLine(emailID);
      }

      textWriter.Close();
    }

    private void UpdateDayIDFile()
    {
      TextWriter textWriter = new StreamWriter(dayPath);

      foreach (String emailID in emailDayIdList)
      {
        textWriter.WriteLine(emailID);
      }

      textWriter.Close();
    }

    // Loads the styles for the sales-Rows
    public void LoadSalesButtons()
    {
      int padding = 5;

      Panel topBufferPanel = new Panel();
      topBufferPanel.Height = 5;
      topBufferPanel.Dock = DockStyle.Top;

      Panel bottomPanel = new Panel();
      bottomPanel.Width = Sales_Reps.Width;
      bottomPanel.Height = 40;
      bottomPanel.Dock = DockStyle.Top;
      bottomPanel.Left = 0;
      bottomPanel.Top = padding;

      Button addButton = new Button();
      addButton.Text = "ADD NEW";
      addButton.Dock = DockStyle.Right;
      addButton.Width = 70;
      addButton.BackColor = panelColor;
      addButton.FlatAppearance.BorderSize = 0;
      addButton.FlatStyle = FlatStyle.Flat;
      addButton.FlatAppearance.MouseOverBackColor = SystemColors.ScrollBar;


      bottomPanel.Controls.Add(addButton);

      Sales_Reps.Controls.Add(bottomPanel);

      Sales_Reps.Controls.Add(topBufferPanel);
     

      foreach (SalesRep sales in salesReps)
      {
        Panel salesRepPanel = new Panel();
        salesRepPanel.Width = Sales_Reps.Width;
        salesRepPanel.Height = 40;
        salesRepPanel.BackColor = panelColor;
        salesRepPanel.Dock = DockStyle.Top;
        salesRepPanel.Left = 0;
        salesRepPanel.Top = padding;

        Panel PicturePanel = new Panel();
        PicturePanel.Width = 40;
        PicturePanel.Dock = DockStyle.Left;
        PicturePanel.BackColor = SystemColors.ControlLight;

        System.Windows.Forms.Label mainLetter = new System.Windows.Forms.Label();
        mainLetter.AutoSize = false;
        mainLetter.Width = 40;
        mainLetter.Height = 40;
        mainLetter.Text = sales.name[0].ToString();
        mainLetter.Font = new Font("consolas", salesRepPanel.Font.Size + 17);
        mainLetter.Dock = DockStyle.Fill;
        mainLetter.TextAlign = ContentAlignment.MiddleCenter;


        PicturePanel.Controls.Add(mainLetter);


        System.Windows.Forms.Label NameLabel = new System.Windows.Forms.Label();
        NameLabel.Text = sales.name;
        NameLabel.Dock = DockStyle.Left;
        NameLabel.AutoSize = false;
        NameLabel.Width = 200;
        NameLabel.TextAlign = ContentAlignment.MiddleLeft;


        Button editButton = new Button();
        editButton.Text = "EDIT";
        editButton.Dock = DockStyle.Right;
        editButton.Width = 70;
        editButton.FlatAppearance.BorderSize = 0;
        editButton.FlatStyle = FlatStyle.Flat;
        editButton.FlatAppearance.MouseOverBackColor = SystemColors.ScrollBar;


        Panel bufferPanel = new Panel();
        bufferPanel.Height = 5;
        bufferPanel.Dock = DockStyle.Top;


        salesRepPanel.Controls.Add(NameLabel);
        salesRepPanel.Controls.Add(PicturePanel);
        salesRepPanel.Controls.Add(editButton);


        Sales_Reps.Controls.Add(salesRepPanel);
        Sales_Reps.Controls.Add(bufferPanel);
        //padding += salesRepPanel.Height + 5;
      }
    }

    #endregion
  }
}
