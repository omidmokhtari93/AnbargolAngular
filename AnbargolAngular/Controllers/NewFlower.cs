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
        private readonly SqlConnection con = new SqlConnection(connectionString: "Server=.;Database=bornatek_anbar;User ID=bornatek_ir;Password=Omid1993");
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

        [HttpGet("/api/CopyFlower")]
        public JsonResult CopyFlower(int flowerId)
        {
            con.Open();
            var cmd = new SqlCommand("INSERT INTO [dbo].[flower_entry]([flower_name],[flower_code],[flower_color] " +
                                     ",[flower_colortype],[flower_format],[customer_name],[company_name],[enter_date] " +
                                     ",[comment])select[flower_name],[flower_code],[flower_color],[flower_colortype] " +
                                     ",[flower_format],[customer_name],[company_name],[enter_date],[comment]" +
                                     " from flower_entry where id = " + flowerId + " " +
                                     "SELECT CAST(scope_identity() AS int)", con);
            var flowid = Convert.ToInt32(cmd.ExecuteScalar());
            var list = new List<Forms>();
            cmd = new SqlCommand("select [id],[form_number],[arrange_type],[dimension] " +
                                 ",[sheetcount],[last_enter_date],[mark_type],[comment] " +
                                 "from flower_forms_entry where flower_id = " + flowerId + " ", con);
            var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                list.Add(new Forms()
                {
                    Id = Convert.ToInt32(rd["id"]),
                    FlowerId = flowid,
                    FormName = rd["form_number"].ToString(),
                    ArrangeTypeId = Convert.ToInt32(rd["arrange_type"]),
                    DimensionId = Convert.ToInt32(rd["dimension"]),
                    Count = Convert.ToInt32(rd["sheetcount"]),
                    EnterDate = rd["last_enter_date"].ToString(),
                    Mark = rd["mark_type"].ToString(),
                    Comment = rd["comment"].ToString()
                });
            }
            con.Close();
            con.Open();
            foreach (var form in list)
            {

                cmd = new SqlCommand("INSERT INTO [dbo].[flower_forms_entry]([flower_id],[form_number],[arrange_type]" +
                                     ",[dimension],[sheetcount],[last_enter_date],[mark_type],[comment]) " +
                                     "values(" + form.FlowerId + " ,'" + form.FormName + "' , " + form.ArrangeTypeId + " , " + form.Dimension + "," +
                                     " " + form.Count + " , '" + form.EnterDate + "' , '" + form.Mark + "' , '" + form.Comment + "') " +
                                     "SELECT CAST(scope_identity() AS int)", con);
                var formid = cmd.ExecuteScalar();
                cmd = new SqlCommand("INSERT INTO [dbo].[flower_arrange_items]([flower_id],[form_id],[form_number]," +
                                     "[item_name],[item_insheet_count],[lent_of_item],[comment])" +
                                     "select " + flowid + "," + formid + ",[form_number],[item_name],[item_insheet_count]," +
                                     "[lent_of_item],[comment] from flower_arrange_items where flower_id = " + flowerId + " " +
                                     "and form_id = " + form.Id + " ", con);
                cmd.ExecuteNonQuery();
            }
            con.Close();
            con.Open();
            var listorders = new List<OrderList>();
            cmd = new SqlCommand("SELECT [id],[flower_id],[order_type],[order_count],[order_enter_date]," +
                                 "[order_complete_date],[order_remain],[comment]FROM [dbo].[orders] where flower_id = " + flowerId + "", con);
            rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                var Id = Convert.ToInt32(rd["id"]);
                var FlowerID = Convert.ToInt32(rd["flower_id"]);
                var OrderType = Convert.ToInt32(rd["order_type"]);
                var OrderCount = Convert.ToInt32(rd["order_count"]);
                var EnterDate = rd["order_enter_date"].ToString();
                var CompleteDate = rd["order_complete_date"].ToString();
                var Remain = rd["order_remain"].ToString();
                var Comment = rd["comment"].ToString();
                listorders.Add(new OrderList()
                {
                    Id = Convert.ToInt32(rd["id"]),
                    FlowerID = Convert.ToInt32(rd["flower_id"]),
                    OrderType = Convert.ToInt32(rd["order_type"]),
                    OrderCount = Convert.ToInt32(rd["order_count"]),
                    EnterDate = rd["order_enter_date"].ToString(),
                    CompleteDate = rd["order_complete_date"].ToString(),
                    Remain = rd["order_remain"].ToString(),
                    Comment = rd["comment"].ToString()
                });
            }
            con.Close();
            foreach (var order in listorders)
            {
                con.Open();
                cmd = new SqlCommand("INSERT INTO [dbo].[orders]([flower_id],[order_type],[order_count],[order_enter_date] " +
                                     ",[order_complete_date],[order_remain],[comment])" +
                                     " values(" + flowid + "," + order.OrderType + "," + order.OrderCount + ",'" + order.EnterDate + "'," +
                                     "'" + order.CompleteDate + "','" + order.Remain + "','" + order.Comment + "') " +
                                     " SELECT CAST(scope_identity() AS int)", con);
                var orderid = cmd.ExecuteScalar();
                cmd = new SqlCommand("INSERT INTO [dbo].[order_sheet_count]([flower_id],[order_id],[form_number],[form_id],[recieve_date],[sheet_count]) " +
                                     "select " + flowid + " , " + orderid + " , form_number , form_id , recieve_date , sheet_count from order_sheet_count " +
                                     "where order_id = " + order.Id + " ", con);
                cmd.ExecuteNonQuery();
                con.Close();
            }
            con.Close();
            return new JsonResult(new
            {
                type = "success",
                message = "با موفقیت کپی شد"
            });
        }
    }
}
