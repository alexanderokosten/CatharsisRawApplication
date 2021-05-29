using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CatharsisRawWebApplication.Domain.Entities
{
    public class StatusOrders
    {
        public StatusOrders()
        {
            UserOrders = new HashSet<UserOrder>();
        }
        [Key]
        [Required]
        public int StatusId { get; set; }
        [Required]
        public string StatusName { get; set; }

        public virtual ICollection<UserOrder> UserOrders { get; set; }
    }
}
