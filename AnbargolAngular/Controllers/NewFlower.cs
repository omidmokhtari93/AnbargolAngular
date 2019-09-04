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
    public JsonResult GetAllFlowers()
    {
      con.Open();
      var gol = new List<Gol>();
      var cmd = new SqlCommand("SELECT top 10 ROW_NUMBER()over(order by id) as row,flower_entry.flower_name, flower_entry.flower_code, " +
"flower_colors.flow_color, flower_colortypes.flow_colortype, " +
"flower_formats.flow_format, flower_customers.customer_name, " +
"flower_companies.company_name, flower_entry.enter_date, " +
"flower_entry.comment, flower_colors.flowcolor_id, " +
"flower_colortypes.colortype_id, flower_formats.flowformat_id, " +
"flower_customers.customer_id, flower_companies.company_id, " +
"flower_entry.id FROM flower_entry INNER JOIN flower_colortypes  " +
"ON flower_entry.flower_colortype = flower_colortypes.colortype_id  " +
"INNER JOIN flower_formats ON flower_entry.flower_format = flower_formats.flowformat_id  " +
"INNER JOIN flower_customers ON flower_entry.customer_name = flower_customers.customer_id  " +
"INNER JOIN flower_companies ON flower_entry.company_name = flower_companies.company_id  " +
"INNER JOIN flower_colors ON flower_entry.flower_color = flower_colors.flowcolor_id  " +
"ORDER BY flower_entry.id DESC", con);
      var rd = cmd.ExecuteReader();
      while (rd.Read())
      {
        gol.Add(new Gol()
        {
          Id = Convert.ToInt32(rd["id"]),
          Name = rd["flower_name"].ToString(),
          Color = rd["flow_color"].ToString(),
          ColorType = rd["flow_colortype"].ToString(),
          Format = rd["flow_format"].ToString(),
          Code = rd["flower_code"].ToString(),
          EnterDate = rd["enter_date"].ToString(),
          Customer = rd["customer_name"].ToString(),
          Company = rd["company_name"].ToString(),
          Comment = rd["comment"].ToString()
        });
      }
      con.Close();
      return new JsonResult(gol);
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
