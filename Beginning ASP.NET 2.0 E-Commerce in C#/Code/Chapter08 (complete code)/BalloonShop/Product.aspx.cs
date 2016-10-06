using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class Product : System.Web.UI.Page
{
  protected void Page_Load(object sender, EventArgs e)
  {
    // don't reload data during postbacks
    if (!IsPostBack)
    {
      PopulateControls();
    }
  }

  // Fill the control with data
  private void PopulateControls()
  {
    // Retrieve ProductID from the query string
    string productId = Request.QueryString["ProductID"];
    // stores product details
    ProductDetails pd;
    // Retrieve product details 
    pd = CatalogAccess.GetProductDetails(productId);
    // Display product details
    titleLabel.Text = pd.Name;
    descriptionLabel.Text = pd.Description;
    priceLabel.Text = String.Format("{0:c}", pd.Price);
    productImage.ImageUrl = "ProductImages/" + pd.Image2FileName;
    // Set the title of the page
    this.Title = BalloonShopConfiguration.SiteName +
                 " : Product : " + pd.Name;
    // Create the "Add to Cart" PayPal link
    string link = 
        "JavaScript: OpenPayPalWindow(\"https://www.paypal.com/cgi-bin/webscr" +
        "?cmd=_cart" + // open shopping cart command
        "&business=youremail@yourserver.com" + // your PayPal account
        "&item_name=" + pd.Name + // product name
        "&amount=" + String.Format("{0:0.00}", pd.Price) + // product price
        "&currency=USD" + // currency
        "&add=1" + // quantity to add to the shopping cart
        "&return=www.yourwebsite.com" + // return address
        "&cancel_return=www.yourwebsite.com\")"; // cancel return address)
    // Encode link characters to be included in HTML file
    string encodedLink = Server.HtmlEncode(link);
    // The the link of the HTML Server Control
    addToCartLink.HRef = encodedLink;
    }
}
