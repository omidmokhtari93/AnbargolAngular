using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AnbargolAngular.Controllers
{
  [Route("api/[controller]")]
  public class NewFlower : Controller
  {
    private readonly SqlConnection con = new SqlConnection(connectionString: "Data Source=.;Initial Catalog=flower_depot;Integrated Security=True");
    // GET: api/<controller>
    [HttpPost("/api/NewFlower")]
    public JsonResult Get([FromBody]Gol obj)
    {
      con.Open();
      var cmd =
        new SqlCommand(
          "INSERT INTO [flower_depot].[dbo].[flower_entry](flower_name , flower_code" +
          ",flower_color,flower_colortype,flower_format,customer_name,company_name,enter_date,comment)" +
          " values ('" + obj.Name + "', '" + obj.Code.ToUpper() + "' " +
          " ," + obj.Color + " ," + obj.ColorType + "" +
          ", " + obj.Format + "  , " + obj.Customer + " ," +
          "" + obj.Company + " , '" + obj.EnterDate + "' , '" + obj.Comment + "')", con);
      cmd.ExecuteNonQuery();
      con.Close();
      return new JsonResult(new { type = "success", message = "با موفقیت ثبت شد" });
    }
  }
}

public class Gol
{
  public int Id { get; set; }
  public string Name { get; set; }
  public string Code { get; set; }
  public string Color { get; set; }
  public int ColorId { get; set; }
  public string ColorType { get; set; }
  public int ColorTypeId { get; set; }
  public string Format { get; set; }
  public int FormatId { get; set; }
  public string Customer { get; set; }
  public int CustomerId { get; set; }
  public string Company { get; set; }
  public int CompanyId { get; set; }
  public string EnterDate { get; set; }
  public string Comment { get; set; }
}
