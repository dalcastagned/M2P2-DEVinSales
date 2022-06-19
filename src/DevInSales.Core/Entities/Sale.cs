using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DevInSales.Core.Entities
{
    public class Sale : Entity
    {

        public Sale(int buyerId, int sellerId, DateTime saleDate)
        {
            BuyerId = buyerId;
            SellerId = sellerId;
            SaleDate = saleDate;
        }
        public int BuyerId { get; private set; }
        public int SellerId { get; private set; }

        public void SetSaleDateToToday()
        {
            SaleDate = DateTime.Now.ToUniversalTime();
        }

        public DateTime SaleDate { get; private set; }        

        [JsonIgnore]        
        public virtual User Buyer { get; private set; }
        [JsonIgnore]     
        public virtual User Seller { get; private set; }
    }
}