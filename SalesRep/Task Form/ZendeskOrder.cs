using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using System.Threading;
using System.IO;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Task_Form
{
  /// <summary>
  /// Potential way to deal with updating ZendeskOrders from Orders coming from the Email Information.
  /// </summary>
  public class ZendeskOrder
  {

    GmailService service;
    UserCredential credential;
    static string[] Scopes = { GmailService.Scope.GmailReadonly };
    static string ApplicationName = "Email Script";


    public ZendeskOrder()
    {
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

    public static Google.Apis.Gmail.v1.Data.Message GetMessage(GmailService service, String userId, String messageId)
    {
      try
      {
        return service.Users.Messages.Get(userId, messageId).Execute();
      }
      catch (Exception e)
      {
        Console.WriteLine("An error occurred: " + e.Message);
      }
      return null;
    }


    public Cart ConvertToCartWithProducts(List<string> lists)
    {
      Cart cart = new Cart();

      for(int i = 0; i < lists.Count; i++)
      {
        string[] words = lists[i].Split(',');

        cart.productsInCart.Add(new Product(words[0], words[1], words[2],
                                            words[3], words[4] ));
      }


      return cart;
    }


    // This will take the html body of an email with order details 
    //     and parse information into a list that can be used in a 
    //     quick manner.
    public List<string> ConvertToLineItems(Google.Apis.Gmail.v1.Data.Message message, GoogleAPI googleAPI)
    {
      string body              = FormatBody(googleAPI.GetEmailBody(message));
      string[] words           = RemoveBrackets(body);
      List<string> productList = RemoveSkus(words);
      productList              = FormatProductList(productList);

      return productList;
    }

    public List<string> FormatProductList(List<string> productList)
    {
      List<string> returnList = new List<string>();

      for(int i = 0; i < productList.Count; i++)
      {
        string[] words = productList[i].Split(',');
        string returnString = "";

        for(int j = 0; j < words.Length; j++)
          if(!string.IsNullOrEmpty( words[j]))
            returnString += words[j] + ",";

        returnList.Add(returnString);
      }

      return returnList;
    }

    public List<string> RemoveSkus(string[] words)
    {
      List<string> productList = new List<string>();

      for (int i = 0; i < words.Length; i++)
        if (words[i].Contains("SKU:"))
          productList.Add(words[i].Replace("SKU:", ","));

      for (int i = 0; i < productList.Count; i++)
        productList[i] = productList[i].Replace(",,", ",");

      return productList;
    }

    public string[] RemoveBrackets( string body)
    {
      string[] words = body.Split(',');
      for (int i = 0; i < words.Length; i++)
        words[i] = RemoveBetween(words[i], '<', '>');

      return words;
    }

    public string FormatBody(string body)
    {
      body = body.Replace("<p>", "");
      body = body.Replace("</p>", "");
      body = body.Replace("\r", "");
      body = body.Replace("\n", "");
      body = body.Replace("<br/>", "");
      body = body.Replace("<br />", "");
      body = body.Replace("</tr>", ",");

      return body;
    }

    public string RemoveBetween(string s, char begin, char end)
    {
      Regex  regex = new Regex(string.Format("\\{0}.*?\\{1}", begin, end));
      return regex.Replace(s, ",");
    }
  }
}
