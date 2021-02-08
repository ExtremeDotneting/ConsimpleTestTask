using System;
using System.Collections.Generic;

namespace ConsimpleTestTask.WebApp.Models
{
    public class PurchaseModel: IBaseModel
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public decimal TotalCost { get; set; }

        public int UserId { get; set; }
        public UserModel User { get; set; }

        public ICollection<PurchasePositionModel> PurchasePositions { get; set; } = new List<PurchasePositionModel>();

        public void RecalculateTotalCost()
        {
            decimal totalCost = 0;
            foreach (var position in PurchasePositions)
            {
                totalCost += position.Product.Price * position.ProductsMultiplier;
            }
            TotalCost = totalCost;
        }
    }
}