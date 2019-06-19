// Small class for the salesRepresentatives

namespace Task_Form
{
  public class Email
  {
    public string emailID { get; set; }
    public string name { get; set; }
    public string time { get; set; }

    public Email(string _emailID, string _name, string _time)
    {
      emailID = _emailID;
      name    = _name;
      time    = _name;
    }
  }
}
