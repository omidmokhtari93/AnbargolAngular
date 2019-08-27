using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnbargolAngular.Controllers
{
  public class Items
  {
    public int Id { get; set; }
    public string Name { get; set; }
  }
  public class Gols
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

  public class Forms
  {
    public int Id { get; set; }
    public int FormId { get; set; }
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
}
