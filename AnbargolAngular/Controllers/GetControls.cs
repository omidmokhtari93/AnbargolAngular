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
    public class GetControls : Controller
    {
        private readonly SqlConnection con = new SqlConnection(connectionString: "Data Source=.;Initial Catalog=flower_depot;Integrated Security=True");
        // GET: api/<controller>
        [HttpGet("/api/GetControls")]
        public JsonResult Get()
        {
            con.Open();
            var cm = new SqlCommand("select item_id as id ,item_name as name FROM items where item_id <> 0 order by item_name " +
                                    "select flowformat_id as id, flow_format as name from flower_formats " +
                                    "select customer_id as id, customer_name as name from flower_customers " +
                                    "select company_id  as id, company_name as name from flower_companies " +
                                    "SELECT [flowcolor_id] as id,[flow_color] as name FROM [dbo].[flower_colors] " +
                                    "SELECT [colortype_id] as id,[flow_colortype] as name FROM [dbo].[flower_colortypes] " +
                                    "SELECT [dimension_id] as id,[flow_dimension] as name FROM [dbo].[flower_dimensions]", con);
            var r = cm.ExecuteReader();
            var items = new List<Controls>();
            while (r.Read())
            {
                items.Add(new Controls()
                {
                    Id = Convert.ToInt32(r["id"]),
                    Text = r["name"].ToString()
                });
            }
            r.NextResult();
            var format = new List<Controls>();
            while (r.Read())
            {
                format.Add(new Controls()
                {
                    Id = Convert.ToInt32(r["id"]),
                    Text = r["name"].ToString()
                });
            }
            r.NextResult();
            var customer = new List<Controls>();
            while (r.Read())
            {
                customer.Add(new Controls()
                {
                    Id = Convert.ToInt32(r["id"]),
                    Text = r["name"].ToString()
                });
            }
            r.NextResult();
            var company = new List<Controls>();
            while (r.Read())
            {
                company.Add(new Controls()
                {
                    Id = Convert.ToInt32(r["id"]),
                    Text = r["name"].ToString()
                });
            }
            r.NextResult();
            var color = new List<Controls>();
            while (r.Read())
            {
                color.Add(new Controls()
                {
                    Id = Convert.ToInt32(r["id"]),
                    Text = r["name"].ToString()
                });
            }
            r.NextResult();
            var colorType = new List<Controls>();
            while (r.Read())
            {
                colorType.Add(new Controls()
                {
                    Id = Convert.ToInt32(r["id"]),
                    Text = r["name"].ToString()
                });
            }
            r.NextResult();
            var dimensions = new List<Controls>();
            while (r.Read())
            {
                dimensions.Add(new Controls()
                {
                    Id = Convert.ToInt32(r["id"]),
                    Text = r["name"].ToString()
                });
            }
            con.Close();
            return new JsonResult(new
            {
                items = items,
                format = format,
                customer = customer,
                company = company,
                color = color,
                colorType = colorType,
                dimension = dimensions
            });
        }
    }
}
public class Controls
{
    public int Id { get; set; }
    public string Text { get; set; }

}
