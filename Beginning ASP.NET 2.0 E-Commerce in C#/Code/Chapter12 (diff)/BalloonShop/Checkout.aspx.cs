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

public partial class Checkout : System.Web.UI.Page
{
  protected override void OnInit(EventArgs e)
  {
    (Master as BalloonShop).EnforceSSL = true;
    base.OnInit(e);
  }

  protected void Page_Load(object sender, EventArgs e)
  {
    // Set the title of the page
    this.Title = BalloonShopConfiguration.SiteName +
                  " : Checkout";

    if (!IsPostBack)
      PopulateControls();
  }

  // fill controls with data
  private void PopulateControls()
  {
    // get the items in the shopping cart
    DataTable dt = ShoppingCartAccess.GetItems();
    // populate the list with the shopping cart contents
    grid.DataSource = dt;
    grid.DataBind();
    // setup controls
    titleLabel.Text =
      "These are the products in your shopping cart:";
    grid.Visible = true;
    // display the total amount
    decimal amount = ShoppingCartAccess.GetTotalAmount();
    totalAmountLabel.Text = String.Format("{0:c}", amount);

    // check customer details
    bool addressOK = true;
    bool cardOK = true;
    if (Profile.Address1 + Profile.Address2 == ""
      || Profile.ShippingRegion == ""
      || Profile.ShippingRegion == "1"
      || Profile.Country == "")
    {
      addressOK = false;
    }
    if (Profile.CreditCard == "")
    {
      cardOK = false;
    }

    // report / hide place order button
    if (!addressOK)
    {
      if (!cardOK)
      {
        InfoLabel.Text =
          "You must provide a valid address and credit card "
          + "before placing your order.";
      }
      else
      {
        InfoLabel.Text =
          "You must provide a valid address before placing your "
          + "order.";
      }
    }
    else if (!cardOK)
    {
      InfoLabel.Text = "You must provide a credit card before "
        + "placing your order.";
    }
    else
    {
      InfoLabel.Text = "Please confirm that the above details are "
        + "correct before proceeding.";
    }
    placeOrderButton.Visible = addressOK && cardOK;
  }

  protected void placeOrderButton_Click(object sender, EventArgs e)
  {
    // Store the total amount because the cart 
    // is emptied when creating the order
    decimal amount = ShoppingCartAccess.GetTotalAmount();
    // Create the order and store the order ID
    string orderId = ShoppingCartAccess.CreateOrder();
    // Create the PayPal redirect location
    string redirect = "";
    redirect +=
      "https://www.paypal.com/xclick/business=youremail@server.com";
    redirect += "&item_name=BallonShopOrder " + orderId;
    redirect += "&item_number=" + orderId;
    redirect += "&amount=" + String.Format("{0:c} ", amount);
    redirect += "&return=http://www.YourWebSite.com";
    redirect += "&cancel_return=http://www.YourWebSite.com";
    // Redirect to the payment page
    Response.Redirect(redirect);
  }
}
