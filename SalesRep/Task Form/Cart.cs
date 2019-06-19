// Small class for the salesRepresentatives

using System.Collections.Generic;

namespace Task_Form
{
  /// <summary>
  /// Creates a simple cart with a ProductList, Price, Total, Tax, and Shipping
  /// </summary>
  public class Cart
  {
    public List<Product> productsInCart = new List<Product>();
    public double shipping { get; set; }
    public double total    { get; set; }
    public double price    { get; set; }
    public double tax      { get; set; }
  }
}
