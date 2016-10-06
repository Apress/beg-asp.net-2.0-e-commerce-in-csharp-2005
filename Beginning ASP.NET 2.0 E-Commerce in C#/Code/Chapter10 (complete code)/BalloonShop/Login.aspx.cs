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

public partial class Login : System.Web.UI.Page
{
  protected void Page_Load(object sender, EventArgs e)
  {
    // get references to the button, checkbox and textboxes
    TextBox usernameTextBox = (TextBox)login.FindControl("UserName");
    TextBox passwordTextBox = (TextBox)login.FindControl("Password");
    CheckBox persistCheckBox = (CheckBox)login.FindControl("RememberMe");
    Button loginButton = (Button)login.FindControl("LoginButton");
    // tie the two textboxes and the checkbox to the button
    Utilities.TieButton(this.Page, usernameTextBox, loginButton);
    Utilities.TieButton(this.Page, passwordTextBox, loginButton);
    Utilities.TieButton(this.Page, persistCheckBox, loginButton);
    // set the page title
    this.Title = BalloonShopConfiguration.SiteName + " : Login";
    // set focus on the username textbox when the page loads
    usernameTextBox.Focus();
  }

}
