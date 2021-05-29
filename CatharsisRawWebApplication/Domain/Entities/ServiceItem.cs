using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatharsisRawWebApplication.Domain.Entities
{
    public class ServiceItem : EntityBase
    {
        public ServiceItem()
        {
            UserOrder = new HashSet<UserOrder>();
        }

        [Required(ErrorMessage = "Заполните название услуги")]
        [Display(Name = "Название услуги")]
        public override string Title { get; set; }

        [Display(Name = "Краткое описание услуги")]
        public override string Subtitle { get; set; }

        [Display(Name = "Полное описание услуги")]
        public override string Text { get; set; }

        [Required(ErrorMessage ="Заполните цену услуги")]
        public override int Price { get; set; }

        [NotMapped]
        public string Description { 
            get 
            {
                return Title + " (" + Price + " " + " рублей )";
            } 
        }
        public virtual ICollection<UserOrder> UserOrder { get; set; }
    }
}
