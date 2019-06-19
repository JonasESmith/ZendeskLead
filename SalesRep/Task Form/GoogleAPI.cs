using System.Text.RegularExpressions;
using System.Collections.Generic;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using System.Threading;
using System.Text;
using System.IO;
using System;

namespace Task_Form
{
  public class GoogleAPI
  {
    GmailService service;
    UserCredential credential;
    static string[] Scopes = { GmailService.Scope.GmailReadonly };
    static string ApplicationName = "Email Script";
    string userId;

    /// <summary>
    /// Creates all services needed to work with the Gmail API
    /// </summary>
    public GoogleAPI()
    {
      userId = "me";

      using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
      {
        string credPath = "token.json";
        credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
            GoogleClientSecrets.Load(stream).Secrets,
            Scopes,
            "user",
            CancellationToken.None,
            new FileDataStore(credPath, true)).Result;
        Console.WriteLine("Credential file saved to: " + credPath);
      }
      service = new GmailService(new BaseClientService.Initializer()
      {
        HttpClientInitializer = credential,
        ApplicationName = ApplicationName,
      });
    }


    // more methods to get proper elements of email.
    static String GetNestedBodyParts(IList<MessagePart> part, string curr)
    {
      string str = curr;
      if (part == null)
        return str;
      else
        foreach (var parts in part)
          if (parts.Parts == null)
            if (parts.Body != null && parts.Body.Data != null)
            { var ts = DecodeBase64String(parts.Body.Data);  str += ts;   }
          else
            return GetNestedBodyParts(parts.Parts, str);

        return str;
    }

    // Gets elements from email and decodes them into a readable form. 
    static String DecodeBase64String(string s)
    {
      var ts  = s.Replace("-", "+");
      ts      = ts.Replace("_", "/");
      var bc  = Convert.FromBase64String(ts);
      var tts = Encoding.UTF8.GetString(bc);

      return tts;
    }

    // gets the newest emails from contact@pinnacledistro
    public void GetNewEmail(String messageId)
    {
      var emailInfoReq = service.Users.Messages.Get("me", messageId);
      var emailInfoResponse = emailInfoReq.Execute();

      if (emailInfoResponse != null)
      {
        String parsedBody = "";
        String from = "";
        String date = "";
        String subject = "";
        String body = "";
        String emailTo = "";


        //loop through the headers and get the fields we need...
        foreach (var mParts in emailInfoResponse.Payload.Headers)
        {
          if (mParts.Name == "Date")
            date = mParts.Value;
          else if (mParts.Name == "From")
            from = mParts.Value;
          else if (mParts.Name == "Subject")
            subject = mParts.Value;

          if (date != "" && from != "")
            if (emailInfoResponse.Payload.Parts == null && emailInfoResponse.Payload.Body != null)
              body = DecodeBase64String(emailInfoResponse.Payload.Body.Data);
            else
              body = GetNestedBodyParts(emailInfoResponse.Payload.Parts, "");
        }

        parsedBody = body.Replace("<p>", "");
        parsedBody = body.Replace("</p>", "");
        parsedBody = body.Replace("\r", "");
        parsedBody = body.Replace("\n", "");
        parsedBody = body.Replace("<br/>", ",");
        parsedBody = body.Replace("<br />", ",");

        string[] words = parsedBody.Split(',');
        for (int i = 0; i < words.Length; i++)
          if (words[i].Contains("Email:"))
            emailTo = Regex.Replace(words[i].Replace("Email:", ""), @"\s+", "");
      }
    }

    public string GetEmailSubject(Google.Apis.Gmail.v1.Data.Message message)
    {
      String subject = "";

      foreach (var mParts in message.Payload.Headers)
        if (mParts.Name == "Subject")
          subject = mParts.Value;

      return subject;
    }

    // gets the email body! :)
    public string GetEmailBody(Google.Apis.Gmail.v1.Data.Message message)
    {
      String from = "";
      String date = "";
      String subject = "";
      String body = "";

      foreach (var mParts in message.Payload.Headers)
      {
        if (mParts.Name == "Date")
        {
          date = mParts.Value;
        }
        else if (mParts.Name == "From")
        {
          from = mParts.Value;
        }
        else if (mParts.Name == "Subject")
        {
          subject = mParts.Value;
        }

        if (date != "" && from != "")
        {
          if (message.Payload.Parts == null && message.Payload.Body != null)
          {
            body = DecodeBase64String(message.Payload.Body.Data);
            break;
          }
          else
          {
            body = GetNestedBodyParts(message.Payload.Parts, "");
          }

          //now you have the data you want....
        }
      }
      return body;
    }

    // currently not working I get a message about permissions
    public static Google.Apis.Gmail.v1.Data.Message MarkMessageAsRead(string MessageID, GmailService service)
    {
      List<string> modsList = new List<string>(new string[] { "UNREAD" });

      ModifyMessageRequest mods = new ModifyMessageRequest();
      mods.RemoveLabelIds = modsList;

      try
      {
        return service.Users.Messages.Modify(mods, "me", MessageID).Execute();
      }
      catch (Exception e)
      {
        Console.WriteLine("An error occurred: " + e.Message);

        throw;
      }
    }

    public Google.Apis.Gmail.v1.Data.Message GetMessage(String messageId)
    { try
      {
        return service.Users.Messages.Get(userId, messageId).Execute();
      }
      catch (Exception e)
      {
        Console.WriteLine("An error occurred: " + e.Message);
      }
      return null;
    }

    public List<Google.Apis.Gmail.v1.Data.Message> GetMessageList(String query)
    { List<Google.Apis.Gmail.v1.Data.Message> result = new List<Google.Apis.Gmail.v1.Data.Message>();
      UsersResource.MessagesResource.ListRequest request = service.Users.Messages.List(userId);
      request.Q = query;

      do
      { try
        { ListMessagesResponse response = request.Execute();
          result.AddRange(response.Messages);
          request.PageToken = response.NextPageToken;  }
        catch (Exception e)
        { Console.WriteLine("An error occurred: " + e.Message); }
      } while (!String.IsNullOrEmpty(request.PageToken));
      return result;
    }
  }
}
