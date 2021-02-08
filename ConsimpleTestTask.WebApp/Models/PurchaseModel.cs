using System.Collections.Generic;

namespace ConsimpleTestTask.WebApp.Models
{
    public class PurchaseModel
    {
        public int Id { get; set; }

        public decimal TotalCost { get; set; }

        public int UserId { get; set; }
        public UserModel User { get; set; }

        public ICollection<PurchasePositionModel> PurchasePositions { get; set; } = new List<PurchasePositionModel>();
    }
}