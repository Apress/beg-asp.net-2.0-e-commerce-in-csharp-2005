using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
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

    // report / hide place order button / shipping selection
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
    shippingSelection.Visible = addressOK && cardOK;

    // Populate shipping selection
    if (addressOK && cardOK)
    {
      int shippingRegionId = int.Parse(Profile.ShippingRegion);
      List<ShippingInfo> shippingInfoData = CommerceLibAccess.GetShippingInfo(shippingRegionId);
      foreach (ShippingInfo shippingInfo in shippingInfoData)
      {
        shippingSelection.Items.Add(new ListItem(shippingInfo.ShippingType, shippingInfo.ShippingID.ToString()));
      }
      shippingSelection.SelectedIndex = 0;
    }
  }

  protected void placeOrderButton_Click(object sender,
  EventArgs e)
  {
    // Store the total amount because the cart 
    // is emptied when creating the order
    decimal amount = ShoppingCartAccess.GetTotalAmount();
    // Get shipping ID or default to 0
    int shippingId = 0;
    try
    {
      shippingId = int.Parse(shippingSelection.SelectedValue);
    }
    catch
    {
    }
    // Get tax ID or default to "No tax"
    string shippingRegion = (HttpContext.Current.Profile as ProfileCommon).ShippingRegion;
    int taxId;
    switch (shippingRegion)
    {
      case "2":
        taxId = 1;
        break;
      default:
        taxId = 2;
        break;
    }

    // Create the order and store the order ID
    string orderId = ShoppingCartAccess.CreateCommerceLibOrder(shippingId, taxId);
    // Redirect to the conformation page
    Response.Redirect("OrderPlaced.aspx");
  }
}
