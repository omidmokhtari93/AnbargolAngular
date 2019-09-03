using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AnbargolAngular.Controllers
{
  [Route("api/[controller]")]
  public class NewFlower : Controller
  {
    private readonly IHostingEnvironment _hostingEnv;
    public NewFlower(IHostingEnvironment environment)
    {
      _hostingEnv = environment;
    }
    private readonly SqlConnection con = new SqlConnection(connectionString: "Data Source=.;Initial Catalog=flower_depot;Integrated Security=True");
    [HttpPost("/api/NewFlower")]
    public JsonResult Get()
    {
      var data = Request.Form[key: "flower-data"];
      var obj = JsonConvert.DeserializeObject<Gol>(data);
      con.Open();
      var cmd =
        new SqlCommand(
          "INSERT INTO [flower_depot].[dbo].[flower_entry](flower_name , flower_code" +
          ",flower_color,flower_colortype,flower_format,customer_name,company_name,enter_date,comment)" +
          " values ('" + obj.Name + "', '" + obj.Code.ToUpper() + "' " +
          " ," + obj.Color + " ," + obj.ColorType + ", " + obj.Format + "  , " + obj.Customer + " ," +
          "" + obj.Company + " , '" + obj.EnterDate + "' , '" + obj.Comment + "') SELECT CAST(scope_identity() AS int)", con);
      var fileName = cmd.ExecuteScalar().ToString();
      con.Close();
      if (Request.Form.Files.Count != 0)
      {
        var file = Request.Form.Files[0];
        var flowerImagesDirectory = Path.Combine(_hostingEnv.WebRootPath, "flower_images");
        var filePath = Path.Combine(flowerImagesDirectory, fileName + ".jpg");
        file.CopyTo(new FileStream(filePath, FileMode.Create));
      }
      return new JsonResult(new { type = "success", message = "با موفقیت ثبت شد" });
    }

    [HttpGet("/api/GetAllFlowers")]
    public JsonResult GetAllFlowers(int take, int skip)
    {
      con.Open();
      var cmd = new SqlCommand("", con);
      return new JsonResult("");
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
