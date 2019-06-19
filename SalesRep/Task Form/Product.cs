// Small class for the salesRepresentatives

namespace Task_Form
{
  /// <summary>
  /// Basic product that will have Name, Sku, Quant, Price, and Total.
  /// </summary>
  public class Product
  {
    public string name  { get; set; }
    public string sku   { get; set; }
    public string quant { get; set; }
    public string price { get; set; }
    public string total { get; set; }

    /// <summary>
    /// Basic product that will have Name, Sku, Quant, Price, and Total.
    /// </summary>
    /// <param name="_name"></param>
    /// <param name="_sku"></param>
    /// <param name="_quant"></param>
    /// <param name="_price"></param>
    /// <param name="_total"></param>
    public Product(string _name, string _sku, string _quant, string _price, string _total)
    {
      sku   = _sku;
      name  = _name;
      quant = _quant;
      price = _price;
      total = _total;
    }
  }
}
