using System;
using System.Linq;

namespace IntegrationFiles.Domain.Entity
{
    public class Item
    {
        public Item(string code, int quantity, decimal price)
        {
            Code = code;
            Quantity = quantity;
            Price = price;
        }

        public string Code { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalAmount => Quantity * Price;

        public static void Create(string itemLine, Sale sale)
        {
            var itemLineSplited = itemLine.Replace("[","").Replace("]","").Split(",");

            itemLineSplited.ToList().ForEach(x => 
            {
                var itemSplited = x.Split("-");

                var item = new Item(itemSplited[0], Convert.ToInt32(itemSplited[1]), Convert.ToDecimal(itemSplited[2]));

                sale.Items.Add(item);
            });
        }
    }
}
