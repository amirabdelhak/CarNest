using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entity
{
    public class Model
    {
        [Key]
        public int ModelId { get; set; }
        [Required, MaxLength(128)]
        public string ModelName { get; set; }

        public virtual ICollection<Car> Cars { get; set; } = new List<Car>();

    }
}