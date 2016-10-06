using System;
using System.Configuration;

/// <summary>
/// Repository for BalloonShop configuration settings
/// </summary>
public static class BalloonShopConfiguration
{
  // Store the number of days for shopping cart expiration
  private readonly static int cartPersistDays;
  // Caches the connection string
  private readonly static string dbConnectionString;
  // Caches the data provider name 
  private readonly static string dbProviderName;
  // Store the number of products per page
  private readonly static int productsPerPage;
  // Store the product description length for product lists
  private readonly static int productDescriptionLength;
  // Store the name of your shop
  private readonly static string siteName;

  // Initialize various properties in the constructor
  static BalloonShopConfiguration()
  {
    cartPersistDays = Int32.Parse(ConfigurationManager.AppSettings["CartPersistDays"]);
    dbConnectionString = ConfigurationManager.ConnectionStrings["BalloonShopConnection"].ConnectionString;
    dbProviderName = ConfigurationManager.ConnectionStrings["BalloonShopConnection"].ProviderName;
    productsPerPage = Int32.Parse(ConfigurationManager.AppSettings["ProductsPerPage"]);
    productDescriptionLength = Int32.Parse(ConfigurationManager.AppSettings["ProductDescriptionLength"]);
    siteName = ConfigurationManager.AppSettings["SiteName"];
  }

  // Returns the number of days for shopping cart expiration
  public static int CartPersistDays
  {
    get
    {
      return cartPersistDays;
    }
  }

  // Returns the connection string for the BalloonShop database
  public static string DbConnectionString
  {
    get
    {
      return dbConnectionString;
    }
  }

  // Returns the data provider name
  public static string DbProviderName
  {
    get
    {
      return dbProviderName;
    }
  }

  // Returns the maximum number of products to be displayed on a page
  public static int ProductsPerPage
  {
    get
    {
      return productsPerPage;
    }
  }

  // Returns the length of product descriptions in products lists
  public static int ProductDescriptionLength
  {
    get
    {
      return productDescriptionLength;
    }
  }

  // Returns the address of the mail server
  public static string MailServer
  {
    get
    {
      return ConfigurationManager.AppSettings["MailServer"];
    }
  }

  // Send error log emails?
  public static bool EnableErrorLogEmail
  {
    get
    {
      return bool.Parse(ConfigurationManager.AppSettings["EnableErrorLogEmail"]);
    }
  }

  // Returns the email address where to send error reports
  public static string ErrorLogEmail
  {
    get
    {
      return ConfigurationManager.AppSettings["ErrorLogEmail"];
    }
  }

  // Returns the length of product descriptions in products lists
  public static string SiteName
  {
    get
    {
      return siteName;
    }
  }

  // Amazon ECS REST URL
  public static string AmazonRestUrl
  {
    get
    {
      return ConfigurationManager.AppSettings["AmazonRestUrl"];
    }
  }

  // subscription ID to access ECS
  public static string SubscriptionId
  {
    get
    {
      return ConfigurationManager.AppSettings["AmazonSubscriptionID"];
    }
  }

  // the Amazon.com associate ID
  public static string AssociateId
  {
    get
    {
      return ConfigurationManager.AppSettings["AmazonAssociateID"];
    }
  }

  // keywords used to do the Amazon search
  public static string SearchKeywords
  {
    get
    {
      return ConfigurationManager.AppSettings["AmazonSearchKeywords"];
    }
  }

  // search location
  public static string SearchIndex
  {
    get
    {
      return ConfigurationManager.AppSettings["AmazonSearchIndex"];
    }
  }

  // the Amazon response groups
  public static string ResponseGroups
  {
    get
    {
      return ConfigurationManager.AppSettings["AmazonResponseGroups"];
    }
  }
}
