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

public partial class CatalogAdmin : System.Web.UI.Page
{
  protected void Page_Load(object sender, EventArgs e)
  {
    // Set the title of the page
    this.Title = BalloonShopConfiguration.SiteName + " : Catalog Admin";
    // Get DepartmentID from the query string
    string departmentId = Request.QueryString["DepartmentID"];
    // Get CategoryID from the query string
    string categoryId = Request.QueryString["CategoryID"];
    // Get ProductID from the query string
    string productId = Request.QueryString["ProductID"];
    // Load the appropriate control into the place holder
    if (departmentId == null)
    {
      Control c = Page.LoadControl(Request.ApplicationPath + "/UserControls/DepartmentsAdmin.ascx");
      adminPlaceHolder.Controls.Add(c);
    }
    else if (categoryId == null)
    {
      Control c = Page.LoadControl(Request.ApplicationPath + "/UserControls/CategoriesAdmin.ascx");
      adminPlaceHolder.Controls.Add(c);
    }
    else if (productId == null)
    {
      Control c = Page.LoadControl(Request.ApplicationPath + "/UserControls/ProductsAdmin.ascx");
      adminPlaceHolder.Controls.Add(c);
    }
    else
    {
      Control c = Page.LoadControl(Request.ApplicationPath + "/UserControls/ProductDetailsAdmin.ascx");
      adminPlaceHolder.Controls.Add(c);
    }
  }
}