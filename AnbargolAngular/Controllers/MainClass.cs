using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace AnbargolAngular.Controllers
{
    public class CalculateCutted
    {
        public static void calc(int flowerId)
        {
            var con = new SqlConnection(connectionString: "Server=.;Database=bornatek_anbar;User ID=bornatek_ir;Password=Omid1993");
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
    public class Items
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class Forms
    {
        public int Id { get; set; }
        public int FormId { get; set; }
        public int FlowerId { get; set; }
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
    public class Order
    {
        public int Id { get; set; }
        public int FlowerId { get; set; }
        public string OrderType { get; set; }
        public string Count { get; set; }
        public string OrderDate { get; set; }
        public string OrderCompleteDate { get; set; }
        public string Remain { get; set; }
        public string Comment { get; set; }
    }
    public class OrderForms
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int FormId { get; set; }
        public string FormName { get; set; }
        public string ReceiveDate { get; set; }
        public string SheetCount { get; set; }
    }
    public class Cutted
    {
        public string Id { get; set; }
        public string FlowerId { get; set; }
        public string Total { get; set; }
        public string Cuttedd { get; set; }
        public string Falleh { get; set; }
        public string Service { get; set; }
        public string Record1 { get; set; }
        public string Record2 { get; set; }
        public string Record3 { get; set; }
        public string Record4 { get; set; }
        public string ChangeTimeDate1 { get; set; }
        public string ChangeTimeDate2 { get; set; }
        public string ChangeTimeDate3 { get; set; }
        public string ChangeTimeDate4 { get; set; }
        public string ItemName { get; set; }
        public string Comment { get; set; }
    }

    public class Controls
    {
        public int Id { get; set; }
        public string Text { get; set; }
    }

    public class Gol
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
    public class OrderList
    {
        public int Id { get; set; }
        public int FlowerID { get; set; }
        public int OrderType { get; set; }
        public int OrderCount { get; set; }
        public string EnterDate { get; set; }
        public string CompleteDate { get; set; }
        public string Remain { get; set; }
        public string Comment { get; set; }
    }
}
