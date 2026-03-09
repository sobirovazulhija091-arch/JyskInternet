public class Warehouse //(Склад)
{
    public int Id{get;set;}

    public string Location{get;set;}=null!;

    public int Capacity{get;set;}
    public List<WarehouseProduct> WarehouseProducts{get;set;}=[];
}
