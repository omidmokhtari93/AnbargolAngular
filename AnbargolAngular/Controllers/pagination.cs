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
      return new JsonResult(Convert.ToInt32(pages));
    }
  }
}
