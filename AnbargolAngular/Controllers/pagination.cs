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
  public class pagination : Controller
  {
    private readonly SqlConnection con = new SqlConnection(connectionString: "Data Source=.;Initial Catalog=flower_depot;Integrated Security=True");
    // GET: api/<controller>
    [HttpGet("/api/OrderPagesCount")]
    public JsonResult Get(int flowerId, int pageSize)
    {
      con.Open();
      var cmd = new SqlCommand("SELECT count(*)FROM [dbo].[orders]where flower_id = " + flowerId + " ", con);
      var rows = Convert.ToDecimal(cmd.ExecuteScalar());
      var pages = Math.Ceiling(rows / pageSize);
      con.Close();
      return new JsonResult(Convert.ToInt32(pages));
    }

    [HttpGet("/api/Orders")]
    public JsonResult GetOrders(int flowerId, int take, int skip)
    {
      con.Open();
      var list = new List<Order>();
      var cmd = new SqlCommand("select * from (SELECT ROW_NUMBER() over (order by id)as rn , " +
                               "[id],[flower_id], ot.order_type,[order_count],[order_enter_date] " +
                               ",[order_complete_date],[order_remain],[comment] FROM[dbo].[orders] " +
                               "inner join order_type ot on orders.order_type = ot.order_id where flower_id = " + flowerId + ")i " +
                               "where i.rn > " + take + " and i.rn <= " + skip + " ", con);
      var rd = cmd.ExecuteReader();
      while (rd.Read())
      {
        list.Add(new Order()
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
      return new JsonResult(list);
    }


  }
}
