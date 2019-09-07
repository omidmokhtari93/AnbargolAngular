using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;

namespace AnbargolAngular.Controllers
{
    [Route("api/[controller]")]
    public class FlowerArranges : Controller
    {
        private readonly SqlConnection con = new SqlConnection(connectionString: "Data Source=.;Initial Catalog=flower_depot;Integrated Security=True");
        [HttpPost("/api/SaveArrange")]
        public JsonResult SaveArrange()
        {
            return new JsonResult(new
            {
                type = "success",
                message = "با موفقیت ثبت شد"
            });
        }
    }
}