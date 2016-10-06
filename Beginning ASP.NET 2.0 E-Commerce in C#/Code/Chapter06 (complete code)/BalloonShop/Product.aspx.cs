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
  }
}
