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
    public JsonResult GetFlowerData(int flowerId, int pageSize)
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
      var formNumbers = new List<Items>();
      cmd = new SqlCommand("select id , form_number from flower_forms_entry where flower_id = " + flowerId + " " +
                           "order by flower_forms_entry.form_number", con);
      rd = cmd.ExecuteReader();
      while (rd.Read())
      {
        formNumbers.Add(new Items()
        {
          Id = Convert.ToInt32(rd["id"]),
          Name = rd["form_number"].ToString()
        });
      }
      con.Close();
      con.Open();
      var forms = new Forms();
      cmd = new SqlCommand("SELECT top(1) dbo.flower_forms_entry.id, dbo.flower_forms_entry.flower_id, dbo.flower_forms_entry.form_number, " +
                           "dbo.flower_forms_entry.sheetcount, dbo.flower_forms_entry.last_enter_date, " +
                           "dbo.flower_forms_entry.mark_type, dbo.flower_forms_entry.comment, dbo.arrange_type.arrange_type, " +
                           "dbo.flower_dimensions.flow_dimension FROM dbo.flower_forms_entry INNER JOIN " +
                           "dbo.arrange_type ON dbo.flower_forms_entry.arrange_type = dbo.arrange_type.arrange_id INNER JOIN " +
                           "dbo.flower_dimensions ON dbo.flower_forms_entry.dimension = dbo.flower_dimensions.dimension_id " +
                           "where flower_id = " + flowerId + " order by flower_forms_entry.form_number", con);
      rd = cmd.ExecuteReader();
      while (rd.Read())
      {
        forms = new Forms()
        {
          Id = Convert.ToInt32(rd["id"]),
          FormName = rd["form_number"].ToString(),
          ArrangeType = rd["arrange_type"].ToString(),
          Dimension = rd["flow_dimension"].ToString(),
          Count = Convert.ToInt32(rd["sheetcount"]),
          Mark = rd["mark_type"].ToString(),
          Comment = rd["comment"].ToString(),
          EnterDate = rd["last_enter_date"].ToString()
        };
      }
      con.Close();
      con.Open();
      var formItems = new List<FormItems>();
      cmd = new SqlCommand("SELECT dbo.items.item_name, dbo.flower_arrange_items.lent_of_item, dbo.flower_forms_entry.id, " +
                           "dbo.flower_arrange_items.item_insheet_count * dbo.flower_forms_entry.sheetcount AS sum, " +
                           "dbo.flower_arrange_items.item_insheet_count, dbo.flower_arrange_items.comment" +
                           " FROM dbo.flower_arrange_items INNER JOIN " +
                           "dbo.items ON dbo.flower_arrange_items.item_name = dbo.items.item_id INNER JOIN " +
                           "dbo.flower_forms_entry ON dbo.flower_arrange_items.form_id = dbo.flower_forms_entry.id" +
                           " where form_id = " + forms.Id + " order by items.item_name", con);
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
      con.Open();
      var orders = new List<Order>();
      cmd = new SqlCommand("select * from (SELECT ROW_NUMBER() over (order by id)as rn , " +
                           "[id],[flower_id], ot.order_type,[order_count],[order_enter_date] " +
                           ",[order_complete_date],[order_remain],[comment] FROM[dbo].[orders] " +
                           "inner join order_type ot on orders.order_type = ot.order_id where flower_id = " + flowerId + ")i " +
                           "where i.rn <= " + pageSize + " " +
                           "order by i.order_complete_date desc", con);
      rd = cmd.ExecuteReader();
      while (rd.Read())
      {
        orders.Add(new Order()
        {
          Id = Convert.ToInt32(rd["id"]),
          OrderType = rd["order_type"].ToString(),
          Count = rd["order_count"].ToString(),
          OrderDate = rd["order_enter_date"].ToString(),
          OrderCompleteDate = rd["order_complete_date"].ToString(),
          Comment = rd["comment"].ToString(),
          Remain = rd["order_remain"].ToString()
        });
      }
      con.Close();
      return new JsonResult(new
      {
        formNumbers,
        gol,
        forms,
        formItems,
        orders
      });
    }

    [HttpGet("/api/GetFlowForm")]
    public JsonResult GetFlowForm(int formId)
    {
      con.Open();
      var form = new Forms();
      var cmd = new SqlCommand("SELECT dbo.flower_forms_entry.id, dbo.flower_forms_entry.flower_id, dbo.flower_forms_entry.form_number, " +
                               "dbo.flower_forms_entry.sheetcount, dbo.flower_forms_entry.last_enter_date, " +
                               "dbo.flower_forms_entry.mark_type, dbo.flower_forms_entry.comment, dbo.arrange_type.arrange_type, " +
                               "dbo.flower_dimensions.flow_dimension FROM dbo.flower_forms_entry INNER JOIN " +
                               "dbo.arrange_type ON dbo.flower_forms_entry.arrange_type = dbo.arrange_type.arrange_id INNER JOIN " +
                               "dbo.flower_dimensions ON dbo.flower_forms_entry.dimension = dbo.flower_dimensions.dimension_id " +
                               "where id = " + formId + " order by flower_forms_entry.form_number", con);
      var rd = cmd.ExecuteReader();
      while (rd.Read())
      {
        form = new Forms()
        {
          Id = Convert.ToInt32(rd["id"]),
          FormName = rd["form_number"].ToString(),
          ArrangeType = rd["arrange_type"].ToString(),
          Dimension = rd["flow_dimension"].ToString(),
          Count = Convert.ToInt32(rd["sheetcount"]),
          Mark = rd["mark_type"].ToString(),
          Comment = rd["comment"].ToString(),
          EnterDate = rd["last_enter_date"].ToString()
        };
      }
      con.Close();
      con.Open();
      var formItems = new List<FormItems>();
      cmd = new SqlCommand("SELECT dbo.items.item_name, dbo.flower_arrange_items.lent_of_item, dbo.flower_forms_entry.id, " +
                           "dbo.flower_arrange_items.item_insheet_count * dbo.flower_forms_entry.sheetcount AS sum, " +
                           "dbo.flower_arrange_items.item_insheet_count, dbo.flower_arrange_items.comment" +
                           " FROM dbo.flower_arrange_items INNER JOIN " +
                           "dbo.items ON dbo.flower_arrange_items.item_name = dbo.items.item_id INNER JOIN " +
                           "dbo.flower_forms_entry ON dbo.flower_arrange_items.form_id = dbo.flower_forms_entry.id" +
                           " where form_id = " + formId + " order by items.item_name", con);
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
        form,
        formItems
      });
    }

    [HttpGet("/api/GetOrderForms")]
    public JsonResult GetOrderForms(int orderId)
    {
      con.Open();
      var list = new List<OrderForms>();
      var cmd = new SqlCommand("SELECT form_number, recieve_date, sheet_count FROM order_sheet_count WHERE (order_id = " + orderId + ")", con);
      var rd = cmd.ExecuteReader();
      while (rd.Read())
      {
        list.Add(new OrderForms()
        {
          FormName = rd["form_number"].ToString(),
          ReceiveDate = rd["recieve_date"].ToString(),
          SheetCount = rd["sheet_count"].ToString(),
        });
      }
      con.Close();
      return new JsonResult(list);
    }

    [HttpGet("/api/Cutted")]
    public JsonResult GetCutted(int flowerId)
    {
      CaculateCutted(flowerId);
      con.Open();
      var list = new List<Cutted>();
      var cmd = new SqlCommand("SELECT cutted_and_remain.ID, cutted_and_remain.flower_id, cutted_and_remain.total, " +
                               "cutted_and_remain.cutted, cutted_and_remain.falleh, cutted_and_remain.service, " +
                               "cutted_and_remain.comment, cutted_and_remain.record1, cutted_and_remain.record2, " +
                               "cutted_and_remain.record3, cutted_and_remain.record4, items.item_name, cutted_and_remain.td1, " +
                               "cutted_and_remain.td2, cutted_and_remain.td3, cutted_and_remain.td4 FROM cutted_and_remain " +
                               "INNER JOIN items ON cutted_and_remain.item_name = items.item_id WHERE(cutted_and_remain.flower_id = " + flowerId + ") " +
                               "ORDER BY items.item_name", con);
      var rd = cmd.ExecuteReader();
      while (rd.Read())
      {
        list.Add(new Cutted()
        {
          Id = rd["ID"].ToString(),
          FlowerId = rd["flower_id"].ToString(),
          Total = rd["total"].ToString(),
          Cuttedd = rd["cutted"].ToString(),
          Falleh = rd["falleh"].ToString(),
          Service = rd["service"].ToString(),
          Comment = rd["comment"].ToString(),
          Record1 = rd["record1"].ToString(),
          Record2 = rd["record2"].ToString(),
          Record3 = rd["record3"].ToString(),
          Record4 = rd["record4"].ToString(),
          ItemName = rd["item_name"].ToString(),
          ChangeTimeDate1 = rd["td1"].ToString(),
          ChangeTimeDate2 = rd["td2"].ToString(),
          ChangeTimeDate3 = rd["td3"].ToString(),
          ChangeTimeDate4 = rd["td4"].ToString()
        });
      }
      con.Close();
      return new JsonResult(list);
    }
    public void CaculateCutted(int flowerId)
    {
      con.Open();
      var cmd = new SqlCommand("update cutted_and_remain set falleh = j.falleh ,service = j.service from " +
                               "(select item,case when[1] is null then 0 else [1] end as service " +
                               ",case when[2] is null then 0 else [2] end as falleh from " +
                               "((select item, arrange_type, (count * tedad) as tedad from new_halfcut " +
                               "inner join new_halfcutRiz on new_halfcut.id = new_halfcutRiz.hid " +
                               "inner join flower_forms_entry on new_halfcut.formid = flower_forms_entry.id " +
                               "where flowid = " + flowerId + ") union all " +
                               "(select fai.item_name as item, ffe.arrange_type, " +
                               "(fai.item_insheet_count * ffe.sheetcount) as tedad " +
                               "from flower_arrange_items as fai inner join flower_forms_entry as ffe " +
                               "on fai.form_id = ffe.id where fai.flower_id = " + flowerId + ")) as src " +
                               "pivot(sum(tedad) for arrange_type in ([1],[2]))as piv)j " +
                               "where j.item = cutted_and_remain.item_name and cutted_and_remain.flower_id = " + flowerId + "" +
                               " " +
                               "insert into cutted_and_remain (flower_id, item_name, service, falleh) " +
                               "select " + flowerId + ", j.item, j.service, j.falleh from " +
                               "(select item,case when[1] is null then 0 else [1] end as service " +
                               ",case when[2] is null then 0 else [2] end as falleh from " +
                               "((select item, arrange_type, (count * tedad) as tedad from new_halfcut " +
                               "inner join new_halfcutRiz on new_halfcut.id = new_halfcutRiz.hid " +
                               "inner join flower_forms_entry on new_halfcut.formid = flower_forms_entry.id " +
                               "where flowid = " + flowerId + ") union all(select fai.item_name as item, ffe.arrange_type, " +
                               "(fai.item_insheet_count * ffe.sheetcount) as tedad " +
                               "from flower_arrange_items as fai inner join flower_forms_entry as ffe " +
                               "on fai.form_id = ffe.id where fai.flower_id = " + flowerId + ")) as src " +
                               "pivot(sum(tedad) for arrange_type in ([1],[2]))as piv)j " +
                               "where j.item not in(select item_name as item from " +
                               "cutted_and_remain where flower_id = " + flowerId + ")" +
                               " " +
                               "update cutted_and_remain set total = falleh + service + cutted  where flower_id = " + flowerId + " ", con);
      cmd.ExecuteNonQuery();
      con.Close();
    }
  }
}
