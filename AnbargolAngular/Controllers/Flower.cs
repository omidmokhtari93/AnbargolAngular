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
  public class Flower : Controller
  {
    private readonly SqlConnection con = new SqlConnection(connectionString: "Data Source=.;Initial Catalog=flower_depot;Integrated Security=True");
    // GET: api/<controller>
    [HttpGet("/api/SearchGol")]
    public JsonResult SearchGol(string golName)
    {
      var farsiPart1 = golName.Replace("ک", "ك").Replace("ی", "ي").Replace("ة", "ه");
      var farsiPart2 = golName.Replace("ك", "ک").Replace("ي", "ی").Replace("ه", "ة");
      con.Open();
      var sel = new SqlCommand("SELECT flower_entry.flower_name,flower_entry.flower_name + ' / ' + flower_colors.flow_color + ' / '  " +
                               "+ flower_colortypes.flow_colortype + ' / ' + flower_formats.flow_format AS gol, flower_entry.id " +
                               "FROM flower_depot.dbo.flower_entry INNER JOIN flower_depot.dbo.flower_colors ON " +
                               "flower_depot.dbo.flower_entry.flower_color = flower_depot.dbo.flower_colors.flowcolor_id INNER JOIN " +
                               "flower_depot.dbo.flower_colortypes ON flower_depot.dbo.flower_entry.flower_colortype = " +
                               "flower_depot.dbo.flower_colortypes.colortype_id INNER JOIN " +
                               "flower_depot.dbo.flower_formats ON flower_depot.dbo.flower_entry.flower_format = " +
                               "flower_depot.dbo.flower_formats.flowformat_id where flower_name like N'%" + golName + "%' OR " +
                               " flower_name like N'%" + farsiPart2 + "%' OR " +
                               "flower_name like N'" + farsiPart1 + "'  order by flower_entry.flower_color", con);
      var r = sel.ExecuteReader();
      var list = new List<Gols>();
      while (r.Read())
      {
        list.Add(new Gols()
        {
          Id = Convert.ToInt32(r["id"]),
          Name = r["gol"].ToString()
        });
      }
      con.Close();
      return new JsonResult(list);
    }

    [HttpGet("/api/GetFlowerData")]
    public JsonResult GetFlowerData(int flowerId)
    {
      con.Open();
      string imgFilePath = "/assets/flower_images/" + flowerId + ".JPG" + "?" + new Random().Next();
      var gol = new Gols();
      var cmd = new SqlCommand("SELECT dbo.flower_colors.flow_color, dbo.flower_colortypes.flow_colortype, flower_entry.id, " +
                               "dbo.flower_formats.flow_format, dbo.flower_customers.customer_name, dbo.flower_companies.company_name " +
                               ", flower_entry.flower_name, flower_entry.flower_code, flower_entry.enter_date " +
                               ", flower_entry.comment FROM dbo.flower_entry INNER JOIN " +
                               "dbo.flower_colors ON dbo.flower_entry.flower_color = dbo.flower_colors.flowcolor_id INNER JOIN " +
                               "dbo.flower_colortypes ON dbo.flower_entry.flower_colortype = dbo.flower_colortypes.colortype_id INNER JOIN " +
                               "dbo.flower_formats ON dbo.flower_entry.flower_format = dbo.flower_formats.flowformat_id INNER JOIN " +
                               "dbo.flower_customers ON dbo.flower_entry.customer_name = dbo.flower_customers.customer_id INNER JOIN " +
                               "dbo.flower_companies ON dbo.flower_entry.company_name = dbo.flower_companies.company_id " +
                               " where flower_entry.id = " + flowerId + " ", con);
      var rd = cmd.ExecuteReader();
      while (rd.Read())
      {
        gol.ImagePath = imgFilePath;
        gol.Id = Convert.ToInt32(rd["id"]);
        gol.Name = rd["flower_name"].ToString();
        gol.Color = rd["flow_color"].ToString();
        gol.ColorType = rd["flow_colortype"].ToString();
        gol.Format = rd["flow_format"].ToString();
        gol.Code = rd["flower_code"].ToString();
        gol.EnterDate = rd["enter_date"].ToString();
        gol.Customer = rd["customer_name"].ToString();
        gol.Company = rd["company_name"].ToString();
        gol.Comment = rd["comment"].ToString();
      }
      con.Close();
      con.Open();
      var forms = new List<Forms>();
      cmd = new SqlCommand("SELECT dbo.flower_forms_entry.id, dbo.flower_forms_entry.flower_id, dbo.flower_forms_entry.form_number, " +
                           "dbo.flower_forms_entry.sheetcount, dbo.flower_forms_entry.last_enter_date, " +
                           "dbo.flower_forms_entry.mark_type, dbo.flower_forms_entry.comment, dbo.arrange_type.arrange_type, " +
                           "dbo.flower_dimensions.flow_dimension FROM dbo.flower_forms_entry INNER JOIN " +
                           "dbo.arrange_type ON dbo.flower_forms_entry.arrange_type = dbo.arrange_type.arrange_id INNER JOIN " +
                           "dbo.flower_dimensions ON dbo.flower_forms_entry.dimension = dbo.flower_dimensions.dimension_id " +
                           "where flower_id = " + flowerId + " ", con);
      rd = cmd.ExecuteReader();
      while (rd.Read())
      {
        forms.Add(new Forms()
        {
          Id = Convert.ToInt32(rd["id"]),
          FormName = rd["form_number"].ToString(),
          ArrangeType = rd["arrange_type"].ToString(),
          Dimension = rd["flow_dimension"].ToString(),
          Count = Convert.ToInt32(rd["sheetcount"]),
          Mark = rd["mark_type"].ToString(),
          Comment = rd["comment"].ToString(),
          EnterDate = rd["last_enter_date"].ToString()
        });
      }
      con.Close();
      con.Open();
      var formItems = new List<FormItems>();
      cmd = new SqlCommand("SELECT dbo.items.item_name, dbo.flower_arrange_items.lent_of_item, dbo.flower_forms_entry.id, "+
                           "dbo.flower_arrange_items.item_insheet_count * dbo.flower_forms_entry.sheetcount AS sum, " +
                           "dbo.flower_arrange_items.item_insheet_count, dbo.flower_arrange_items.comment" +
                           " FROM dbo.flower_arrange_items INNER JOIN " +
                           "dbo.items ON dbo.flower_arrange_items.item_name = dbo.items.item_id INNER JOIN " +
                           "dbo.flower_forms_entry ON dbo.flower_arrange_items.form_id = dbo.flower_forms_entry.id" +
                           " where form_id = " + forms.ElementAt(0).Id + " ", con);
      rd = cmd.ExecuteReader();
      while (rd.Read())
      {
        formItems.Add(new FormItems()
        {
          Name = rd["item_name"].ToString(),
          ItemInSheet = Convert.ToDecimal(rd["item_insheet_count"]),
          LentInSheet = Convert.ToDecimal(rd["lent_of_item"]),
          Sum = Convert.ToDecimal(rd["sum"]),
          Comment = rd["comment"].ToString()
        });
      }
      con.Close();
      return new JsonResult(new
      {
        gol,
        forms,
        formItems
      });
    }
  }
  public class Gols
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
    public string ImagePath { get; set; }
  }

  public class Forms
  {
    public int Id { get; set; }
    public int FormId { get; set; }
    public string FormName { get; set; }
    public int ArrangeTypeId { get; set; }
    public string ArrangeType { get; set; }
    public int DimensionId { get; set; }
    public string Dimension { get; set; }
    public int Count { get; set; }
    public string Mark { get; set; }
    public string EnterDate { get; set; }
    public string Comment { get; set; }
  }
  public class FormItems
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal ItemInSheet { get; set; }
    public decimal LentInSheet { get; set; }
    public decimal Sum { get; set; }
    public string Comment { get; set; }
  }

}
