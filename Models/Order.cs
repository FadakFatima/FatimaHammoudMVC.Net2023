using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace CmsShoppingCart.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string Items { get; set; }
        

    }
}
