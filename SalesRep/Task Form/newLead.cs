// Small class for the salesRepresentatives

using System.Collections.Generic;

namespace Task_Form
{

  /// <summary>
  /// 
  /// </summary>
  public class Lead
  {
    public string first_name        { get; set; }
    public string last_name         { get; set; }
    public string organization_name { get; set; }
    public string source_id         { get; set; }
    public string owner_id          { get; set; }
    public string title             { get; set; }
    public string description       { get; set; }
    public string industry          { get; set; }
    public string website           { get; set; }
    public string email             { get; set; }
    public string phone             { get; set; }
    public string mobile            { get; set; }
    public string fax               { get; set; }
    public string twitter           { get; set; }
    public string facebook          { get; set; }
    public string linkedin          { get; set; }
    // address fields
    public string lineOne           { get; set; }
    public string city              { get; set; }
    public string postal_code       { get; set; }
    public string state             { get; set; }
    public string country           { get; set; }


    // may need to create a tags object. 
    string tags = "";

    // may need to create a custom_fields objcet.
    List<string> custom_fields;

    /// <summary>
    /// This is the basic format for leads inside of Zendesk
    /// </summary>
    public Lead() { }

    /// <summary>
    /// This is the basic format for leads inside of Zendesk
    /// </summary>
    /// <param name="_firstName"></param>
    /// <param name="_lastName"></param>
    /// <param name="_organization"></param>
    /// <param name="_sourceid"></param>
    /// <param name="_title"></param>
    /// <param name="_description"></param>
    /// <param name="_industry"></param>
    /// <param name="_website"></param>
    /// <param name="_email"></param>
    /// <param name="_phone"></param>
    /// <param name="_mobile"></param>
    /// <param name="_fax"></param>
    /// <param name="_twitter"></param>
    /// <param name="_facebook"></param>
    /// <param name="_linkdin"></param>
    // the general constructor for the Lead Object
    public Lead(string _firstName, string _lastName,    string _organization, string _sourceid, 
                string _title,     string _description, string _industry,     string _website, 
                string _email,     string _phone,       string _mobile,       string _fax, 
                string _twitter,   string _facebook,    string _linkdin)
    {
      first_name = _firstName;
      last_name = _lastName;
      organization_name = _organization;
      source_id = _sourceid;
      title = _title;
      description = _description;
      industry = _industry;
      website = _website;
      email = _email;
      phone = _phone;
      mobile = _mobile;
      fax = _fax;
      twitter = _twitter;
      facebook = _facebook;
      linkedin = _linkdin;

      // will add the object constructors for Address, Tags, and custom_Fields
    }
  }
}
