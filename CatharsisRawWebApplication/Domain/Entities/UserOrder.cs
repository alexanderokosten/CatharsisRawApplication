using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CatharsisRawWebApplication.Domain.Entities
{
    public class UserOrder
    {
        [Key]
        [Required]
        public int IdUserOrder { get; set; }
        [Required]
        public string idClient { get; set; }
      
        [Required]
        public string NameClient { get; set; }
       
       
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        
        public DateTime PointDate { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        public string Comment { get; set; }

        public string FileRARName { get; set; }

        public int statusId { get; set; }
        public virtual StatusOrders status { get; set; }

        public Guid serviceId { get; set; }
        public virtual ServiceItem service { get; set; }
        [NotMapped]
        public string CorrectDateTime
        {
            get
            {
                return PointDate.ToShortDateString();
            }
        }
     




    }
}
