<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProductsList.ascx.cs"
  Inherits="ProductsList" %>
<asp:Label ID="pagingLabel" runat="server" CssClass="PagingText" Visible="false" />
&nbsp;&nbsp;
<asp:HyperLink ID="previousLink" runat="server" CssClass="PagingText" Visible="false">Previous</asp:HyperLink>
&nbsp;&nbsp;
<asp:HyperLink ID="nextLink" runat="server" CssClass="PagingText" Visible="false">Next</asp:HyperLink>
<asp:DataList ID="list" Runat="server" RepeatColumns="2">
  <ItemTemplate>
    <table cellPadding="0" align="left">
      <tr height="105">
        <td align="center" width="110">
          <a href='Product.aspx?ProductID=<%# Eval("ProductID")%>'>
            <img width="100" src='ProductImages/<%# Eval("Image1FileName") %>' border="0"/>
          </a>
        </td>
        <td vAlign="top" width="250">
          <a class="ProductName" href='Product.aspx?ProductID=<%# Eval("ProductID")%>'>
            <%# Eval("Name") %>
          </a>
          <br/>
          <span class="ProductDescription">
            <%# Eval("Description") %>
            <br/><br/>
            Price: 
          </span>
          <span class="ProductPrice">
            <%# Eval("Price", "{0:c}") %>
          </span>
        </td>
      </tr>
    </table>
  </ItemTemplate>
</asp:DataList>
