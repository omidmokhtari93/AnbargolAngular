using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;

namespace AnbargolAngular.Controllers
{
    [Route("api/[controller]")]
    public class FlowerArranges : Controller
    {
        private readonly SqlConnection con = new SqlConnection(connectionString: "Data Source=.;Initial Catalog=flower_depot;Integrated Security=True");
        [HttpGet("/api/GetArragnes")]
        public JsonResult GetArranges(int flowerId)
        {
            con.Open();
            var listArranges = new List<Forms>();
            var cmd = new SqlCommand("SELECT flower_forms_entry.form_number, arrange_type.arrange_type, " +
                                     "flower_dimensions.flow_dimension, flower_forms_entry.sheetcount, " +
                                    "flower_forms_entry.mark_type, flower_forms_entry.comment, " +
                                    "flower_forms_entry.id, flower_forms_entry.flower_id,  " +
                                    "flower_forms_entry.last_enter_date FROM flower_forms_entry  " +
                                    "INNER JOIN arrange_type ON flower_forms_entry.arrange_type =  " +
                                    "arrange_type.arrange_id INNER JOIN flower_dimensions ON  " +
                                    "flower_forms_entry.dimension = flower_dimensions.dimension_id " +
                                    "WHERE (flower_forms_entry.flower_id = " + flowerId + ") ORDER BY flower_forms_entry.form_number", con);
            var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                listArranges.Add(new Forms()
                {
                    Id = Convert.ToInt32(rd["id"]),
                    FormName = rd["form_number"].ToString(),
                    ArrangeType = rd["arrange_type"].ToString(),
                });
            }
            return new JsonResult("");
        }


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