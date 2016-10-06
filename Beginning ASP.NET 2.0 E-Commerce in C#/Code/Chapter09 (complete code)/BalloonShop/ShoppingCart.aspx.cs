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

public partial class ShoppingCart : System.Web.UI.Page
{
  protected void Page_Load(object sender, EventArgs e)
  {
    // populate the control only on the initial page load
    if (!IsPostBack)
      PopulateControls();
  }

  // fill shopping cart controls with data
  private void PopulateControls()
  {
    // set the title of the page
    this.Title = BalloonShopConfiguration.SiteName + " : Shopping Cart";
    // get the items in the shopping cart
    DataTable dt = ShoppingCartAccess.GetItems();
    // if the shopping cart is empty...
    if (dt.Rows.Count == 0)
    {
      titleLabel.Text = "Your shopping cart is empty!";
      grid.Visible = false;
      updateButton.Enabled = false;
      totalAmountLabel.Text = String.Format("{0:c}", 0);
    }
    else
    // if the shopping cart is not empty...
    {
      // populate the list with the shopping cart contents
      grid.DataSource = dt;
      grid.DataBind();
      // setup controls
      titleLabel.Text = "These are the products in your shopping cart:";
      grid.Visible = true;
      updateButton.Enabled = true;
      // display the total amount
      decimal amount = ShoppingCartAccess.GetTotalAmount();
      totalAmountLabel.Text = String.Format("{0:c}", amount);
    }
  }

  // remove a product from the cart
  protected void grid_RowDeleting(object sender, GridViewDeleteEventArgs e)
  {
    // Index of the row being deleted
    int rowIndex = e.RowIndex;
    // The ID of the product being deleted
    string productId = grid.DataKeys[rowIndex].Value.ToString();
    // Remove the product from the shopping cart
    bool success = ShoppingCartAccess.RemoveItem(productId);
    // Display status
    statusLabel.Text = success ? "<br />Product successfully removed!<br />" :
                      "<br />There was an error removing the product!<br />";
    // Repopulate the control
    PopulateControls();
  }

  // update shopping cart product quantities
  protected void updateButton_Click(object sender, EventArgs e)
  {
    // Number of rows in the GridView
    int rowsCount = grid.Rows.Count;
    // Will store a row of the GridView
    GridViewRow gridRow;
    // Will reference a quantity TextBox in the GridView
    TextBox quantityTextBox;
    // Variables to store product ID and quantity
    string productId;
    int quantity;
    // Was the update successful?
    bool success = true;
    // Go through the rows of the GridView
    for (int i = 0; i < rowsCount; i++)
    {
      // Get a row
      gridRow = grid.Rows[i];
      // The ID of the product being deleted
      productId = grid.DataKeys[i].Value.ToString();
      // Get the quantity TextBox in the Row
      quantityTextBox = (TextBox)gridRow.FindControl("editQuantity");
      // Get the quantity, guarding against bogus values
      if (Int32.TryParse(quantityTextBox.Text, out quantity))
      {
        // Update product quantity
        success = success && ShoppingCartAccess.UpdateItem(productId, quantity);
      }
      else
      {
        // if TryParse didn't succeed
        success = false;
      }
      // Display status message
      statusLabel.Text = success ?
        "<br />Your shopping cart was successfully updated!<br />" :
        "<br />Some quantity updates failed! Please verify your cart!<br />";
    }
    // Repopulate the control
    PopulateControls();
  }

  // Redirects to the previously visited catalog page 
  // (an alternate to the functionality implemented here is to to 
  // Request.UrlReferrer, although that way you have no control to 
  // what pages you forward your visitor back to)
  protected void continueShoppingButton_Click(object sender, EventArgs e)
  {
    // redirect to the last visited catalog page, or to the
    // main page of the catalog
    object page;
    if ((page = Session["LastVisitedCatalogPage"]) != null)
      Response.Redirect(page.ToString());
    else
      Response.Redirect(Request.ApplicationPath);
  }
}
