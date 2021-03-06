using System;
using System.Data;
using System.Data.Common;

/// <summary>
/// Wraps department details data
/// </summary>
public struct DepartmentDetails
{
  public string Name;
  public string Description;
}

/// <summary>
/// Wraps category details data
/// </summary>
public struct CategoryDetails
{
  public int DepartmentId;
  public string Name;
  public string Description;
}

/// <summary>
/// Wraps product details data
/// </summary>
public struct ProductDetails
{
  public string Name;
  public string Description;
  public decimal Price;
  public string Image1FileName;
  public string Image2FileName;
  public bool OnDepartmentPromotion;
  public bool OnCatalogPromotion;
}

/// <summary>
/// Product catalog business tier component
/// </summary>
public static class CatalogAccess
{
  static CatalogAccess()
  {
    //
    // TODO: Add constructor logic here
    //
  }

  // Retrieve the list of departments 
  public static DataTable GetDepartments()
  {
    // get a configured DbCommand object
    DbCommand comm = GenericDataAccess.CreateCommand();
    // set the stored procedure name
    comm.CommandText = "GetDepartments";
    // execute the stored procedure and return the results
    return GenericDataAccess.ExecuteSelectCommand(comm);
  }

  // get department details
  public static DepartmentDetails GetDepartmentDetails(string departmentId)
  {
    // get a configured DbCommand object
    DbCommand comm = GenericDataAccess.CreateCommand();
    // set the stored procedure name
    comm.CommandText = "GetDepartmentDetails";
    // create a new parameter
    DbParameter param = comm.CreateParameter();
    param.ParameterName = "@DepartmentID";
    param.Value = departmentId;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // execute the stored procedure
    DataTable table = GenericDataAccess.ExecuteSelectCommand(comm);
    // wrap retrieved data into a DepartmentDetails object
    DepartmentDetails details = new DepartmentDetails();
    if (table.Rows.Count > 0)
    {
      details.Name = table.Rows[0]["Name"].ToString();
      details.Description = table.Rows[0]["Description"].ToString();
    }
    // return department details
    return details;
  }

  // Get category details
  public static CategoryDetails GetCategoryDetails(string categoryId)
  {
    // get a configured DbCommand object
    DbCommand comm = GenericDataAccess.CreateCommand();
    // set the stored procedure name
    comm.CommandText = "GetCategoryDetails";
    // create a new parameter
    DbParameter param = comm.CreateParameter();
    param.ParameterName = "@CategoryID";
    param.Value = categoryId;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // execute the stored procedure
    DataTable table = GenericDataAccess.ExecuteSelectCommand(comm);
    // wrap retrieved data into a CategoryDetails object
    CategoryDetails details = new CategoryDetails();
    if (table.Rows.Count > 0)
    {
      details.DepartmentId = Int32.Parse(table.Rows[0]["DepartmentID"].ToString());
      details.Name = table.Rows[0]["Name"].ToString();
      details.Description = table.Rows[0]["Description"].ToString();
    }
    // return department details
    return details;
  }

  // Get product details
  public static ProductDetails GetProductDetails(string productId)
  {
    // get a configured DbCommand object
    DbCommand comm = GenericDataAccess.CreateCommand();
    // set the stored procedure name
    comm.CommandText = "GetProductDetails";
    // create a new parameter
    DbParameter param = comm.CreateParameter();
    param.ParameterName = "@ProductID";
    param.Value = productId;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // execute the stored procedure
    DataTable table = GenericDataAccess.ExecuteSelectCommand(comm);
    // wrap retrieved data into a ProductDetails object
    ProductDetails details = new ProductDetails();
    if (table.Rows.Count > 0)
    {
      // get the first table row
      DataRow dr = table.Rows[0];
      // get product details
      details.Name = dr["Name"].ToString();
      details.Description = dr["Description"].ToString();
      details.Price = Decimal.Parse(dr["Price"].ToString());
      details.Image1FileName = dr["Image1FileName"].ToString();
      details.Image2FileName = dr["Image2FileName"].ToString();
      details.OnDepartmentPromotion = bool.Parse(dr["OnDepartmentPromotion"].ToString());
      details.OnCatalogPromotion = bool.Parse(dr["OnCatalogPromotion"].ToString());
    }
    // return department details
    return details;
  }

  // retrieve the list of categories in a department
  public static DataTable GetCategoriesInDepartment(string departmentId)
  {
    // get a configured DbCommand object
    DbCommand comm = GenericDataAccess.CreateCommand();
    // set the stored procedure name
    comm.CommandText = "GetCategoriesInDepartment";
    // create a new parameter
    DbParameter param = comm.CreateParameter();
    param.ParameterName = "@DepartmentID";
    param.Value = departmentId;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // execute the stored procedure
    return GenericDataAccess.ExecuteSelectCommand(comm);
  }

  // Retrieve the list of products on catalog promotion
  public static DataTable GetProductsOnCatalogPromotion(string pageNumber, out int howManyPages)
  {
    // get a configured DbCommand object
    DbCommand comm = GenericDataAccess.CreateCommand();
    // set the stored procedure name
    comm.CommandText = "GetProductsOnCatalogPromotion";
    // create a new parameter
    DbParameter param = comm.CreateParameter();
    param.ParameterName = "@DescriptionLength";
    param.Value = BalloonShopConfiguration.ProductDescriptionLength;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // create a new parameter
    param = comm.CreateParameter();
    param.ParameterName = "@PageNumber";
    param.Value = pageNumber;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // create a new parameter
    param = comm.CreateParameter();
    param.ParameterName = "@ProductsPerPage";
    param.Value = BalloonShopConfiguration.ProductsPerPage;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // create a new parameter
    param = comm.CreateParameter();
    param.ParameterName = "@HowManyProducts";
    param.Direction = ParameterDirection.Output;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // execute the stored procedure and save the results in a DataTable
    DataTable table = GenericDataAccess.ExecuteSelectCommand(comm);
    // calculate how many pages of products and set the out parameter
    int howManyProducts = Int32.Parse(comm.Parameters["@HowManyProducts"].Value.ToString());
    howManyPages = (int)Math.Ceiling((double)howManyProducts /
                           (double)BalloonShopConfiguration.ProductsPerPage);
    // return the page of products
    return table;
  }

  // retrieve the list of products featured for a department
  public static DataTable GetProductsOnDepartmentPromotion(string departmentId, string pageNumber, out int howManyPages)
  {
    // get a configured DbCommand object
    DbCommand comm = GenericDataAccess.CreateCommand();
    // set the stored procedure name
    comm.CommandText = "GetProductsOnDepartmentPromotion";
    // create a new parameter
    DbParameter param = comm.CreateParameter();
    param.ParameterName = "@DepartmentID";
    param.Value = departmentId;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // create a new parameter
    param = comm.CreateParameter();
    param.ParameterName = "@DescriptionLength";
    param.Value = BalloonShopConfiguration.ProductDescriptionLength;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // create a new parameter
    param = comm.CreateParameter();
    param.ParameterName = "@PageNumber";
    param.Value = pageNumber;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // create a new parameter
    param = comm.CreateParameter();
    param.ParameterName = "@ProductsPerPage";
    param.Value = BalloonShopConfiguration.ProductsPerPage;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // create a new parameter
    param = comm.CreateParameter();
    param.ParameterName = "@HowManyProducts";
    param.Direction = ParameterDirection.Output;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // execute the stored procedure and save the results in a DataTable
    DataTable table = GenericDataAccess.ExecuteSelectCommand(comm);
    // calculate how many pages of products and set the out parameter
    int howManyProducts = Int32.Parse(comm.Parameters["@HowManyProducts"].Value.ToString());
    howManyPages = (int)Math.Ceiling((double)howManyProducts /
                   (double)BalloonShopConfiguration.ProductsPerPage);
    // return the page of products
    return table;
  }

  // retrieve the list of products in a category
  public static DataTable GetProductsInCategory(string categoryId, string pageNumber, out int howManyPages)
  {
    // get a configured DbCommand object
    DbCommand comm = GenericDataAccess.CreateCommand();
    // set the stored procedure name
    comm.CommandText = "GetProductsInCategory";
    // create a new parameter
    DbParameter param = comm.CreateParameter();
    param.ParameterName = "@CategoryID";
    param.Value = categoryId;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // create a new parameter
    param = comm.CreateParameter();
    param.ParameterName = "@DescriptionLength";
    param.Value = BalloonShopConfiguration.ProductDescriptionLength;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // create a new parameter
    param = comm.CreateParameter();
    param.ParameterName = "@PageNumber";
    param.Value = pageNumber;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // create a new parameter
    param = comm.CreateParameter();
    param.ParameterName = "@ProductsPerPage";
    param.Value = BalloonShopConfiguration.ProductsPerPage;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // create a new parameter
    param = comm.CreateParameter();
    param.ParameterName = "@HowManyProducts";
    param.Direction = ParameterDirection.Output;
    param.DbType = DbType.Int32;
    comm.Parameters.Add(param);
    // execute the stored procedure and save the results in a DataTable
    DataTable table = GenericDataAccess.ExecuteSelectCommand(comm);
    // calculate how many pages of products and set the out parameter
    int howManyProducts = Int32.Parse(comm.Parameters["@HowManyProducts"].Value.ToString());
    howManyPages = (int)Math.Ceiling((double)howManyProducts /
                   (double)BalloonShopConfiguration.ProductsPerPage);
    // return the page of products
    return table;
  }

}
