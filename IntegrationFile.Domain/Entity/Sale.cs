using System.Collections.Generic;
using System.Linq;

namespace IntegrationFiles.Domain.Entity
{
    public class Sale
    {
        public Sale()
        {
            Items = new List<Item>();
        }

        public Sale(string code, string saleId, string itemLine, string salesManName)
            : this()
        {
            Code = code;
            SaleId = saleId;
            SalesManName = salesManName;
            Item.Create(itemLine, this);
        }

        public string Code { get; set; }
        public string SaleId { get; set; }
        public IList<Item> Items { get; set; }
        public string SalesManName { get; set; }
        public decimal TotalAmount => Items != null ? Items.Select(x => x.Price).Sum() : 0;
    }
}
