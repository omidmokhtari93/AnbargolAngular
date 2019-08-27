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
  public class CuttedAndRemain : Controller
  {
    private readonly SqlConnection con = new SqlConnection(connectionString: "Data Source=.;Initial Catalog=flower_depot;Integrated Security=True");
    [HttpGet("/api/Cutted")]
    public JsonResult GetCutted(int flowerId)
    {
      CalculateCutted.calc(flowerId);
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

    [HttpGet("/api/UpdateComment")]
    public void UpdateComment(int id, string comment)
    {
      con.Open();
      var cmd = new SqlCommand("UPDATE [dbo].[cutted_and_remain] SET [comment] = '" + comment + "' " +
                               "where ID = " + id + " ", con);
      cmd.ExecuteNonQuery();
      con.Close();
    }

    [HttpGet("/api/UpdateCutted")]
    public JsonResult UpdateCutted(int id, int change, bool plus)
    {
      con.Open();
      if (!plus)
      {
        var cmd = new SqlCommand("if (select cutted from cutted_and_remain where id = " + id + ") < " + change + " " +
                                 "select N'خطا در مقدار وارد شده' else " +
                                 "UPDATE [dbo].[cutted_and_remain] SET [cutted] = [cutted] - " + change + " " +
                                 "where ID = " + id + " ", con);
        var messege = cmd.ExecuteScalar().ToString();
        con.Close();
        return string.IsNullOrEmpty(messege) ? new JsonResult(new { type = "success", message = "با موفقیت ثبت شد" })
          : new JsonResult(new { type = "error", message = messege });
      }
      else
      {
        var cmd = new SqlCommand("UPDATE [dbo].[cutted_and_remain] SET [cutted] = [cutted] + " + change + " " +
                                 "where ID = " + id + " ", con);
        cmd.ExecuteNonQuery();
        con.Close();
        return new JsonResult(new { type = "success", message = "با موفقیت ثبت شد" });
      }
    }
  }
}
