namespace ConsimpleTestTask.WebApp.Models
{
    public class PurchasePositionModel
    {
        public int Id { get; set; }

        public int PurchaseId { get; set; }
        public PurchaseModel Purchase { get; set; }

        public int ProductId { get; set; }
        public ProductModel Product { get; set; }

        public int ProductsMultiplier { get; set; }
    }
}