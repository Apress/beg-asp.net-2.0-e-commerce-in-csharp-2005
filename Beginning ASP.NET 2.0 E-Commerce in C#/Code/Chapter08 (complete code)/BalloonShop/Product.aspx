<%@ Page Language="C#" MasterPageFile="~/BalloonShop.master" AutoEventWireup="true" CodeFile="Product.aspx.cs" Inherits="Product" Title="Untitled Page" %>
<asp:Content ID="content" ContentPlaceHolderID="contentPlaceHolder" Runat="Server">
<br/>
<asp:Label CssClass="ProductTitle" ID="titleLabel" Runat="server" Text="Label"></asp:Label>
<br/><br/>
<asp:Image ID="productImage" Runat="server" />
<br/>                                             
<asp:Label CssClass="ProductDescription" ID="descriptionLabel" Runat="server" Text="Label"></asp:Label>
<br/><br/>
<span class="ProductDescription">Price:</span>&nbsp;
<asp:Label CssClass="ProductPrice" ID="priceLabel" Runat="server" Text="Label" />
<br /><br />
<a runat="server" id="addToCartLink">
  <IMG src="Images/AddToCart.gif" border="0">
</a>

  <br />
</asp:Content>
