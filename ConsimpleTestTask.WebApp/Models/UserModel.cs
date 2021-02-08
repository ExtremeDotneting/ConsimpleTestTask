using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ConsimpleTestTask.WebApp.Models
{
    public class UserModel: IBaseModel
    {
        public int Id { get; set; }

        [StringLength(50)]
        [Required]
        public string FullName { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime RegistrationDate { get; set; }

        public ICollection<PurchaseModel> Purchases { get; set; } = new List<PurchaseModel>();
    }
}