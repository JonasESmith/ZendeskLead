// Small class for the salesRepresentatives

namespace Task_Form
{
  /// <summary>
  ///  Takes Name, Email, and Owner_id for zendesk
  /// </summary>
  public class SalesRep
  {
    public string name  { get; set; }
    public string email { get; set; }
    public int owner_id { get; set; }

    /// <summary>
    /// Takes Name, Email, and Owner_id for zendesk
    /// </summary>
    /// <param name="_name"></param>
    /// <param name="_email"></param>
    /// <param name="_owner_id"></param>
    public SalesRep(string _name, string _email, int _owner_id)
    {
      name = _name;
      email = _email;
      owner_id = _owner_id;
    }

    public SalesRep()
    {
      name = "";
      email = "";
      owner_id = 0;
    }
  }
}
