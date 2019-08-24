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
    private readonly SqlConnection _cnn = new SqlConnection(connectionString: "Data Source=.;Initial Catalog=flower_depot;Integrated Security=True");
    // GET: api/<controller>
    [HttpGet("/api/SearchGol")]
    public JsonResult Get(string golName)
    {
      var farsiPart1 = golName.Replace("ک", "ك").Replace("ی", "ي").Replace("ة", "ه");
      var farsiPart2 = golName.Replace("ك", "ک").Replace("ي", "ی").Replace("ه", "ة");
      _cnn.Open();
      var sel = new SqlCommand("SELECT flower_entry.flower_name,flower_entry.flower_name + ' / ' + flower_colors.flow_color + ' / '  " +
                               "+ flower_colortypes.flow_colortype + ' / ' + flower_formats.flow_format AS gol, flower_entry.id " +
                               "FROM flower_depot.dbo.flower_entry INNER JOIN flower_depot.dbo.flower_colors ON " +
                               "flower_depot.dbo.flower_entry.flower_color = flower_depot.dbo.flower_colors.flowcolor_id INNER JOIN " +
                               "flower_depot.dbo.flower_colortypes ON flower_depot.dbo.flower_entry.flower_colortype = " +
                               "flower_depot.dbo.flower_colortypes.colortype_id INNER JOIN " +
                               "flower_depot.dbo.flower_formats ON flower_depot.dbo.flower_entry.flower_format = " +
                               "flower_depot.dbo.flower_formats.flowformat_id where flower_name like N'%" + golName + "%' OR " +
                               " flower_name like N'%" + farsiPart2 + "%' OR " +
                               "flower_name like N'" + farsiPart1 + "'  order by flower_entry.flower_color", _cnn);
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
      _cnn.Close();
      return new JsonResult(list);
    }
  }

  public class Gols
  {
    public int Id { get; set; }
    public string Name { get; set; }
  }
}
