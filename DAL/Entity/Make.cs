using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entity
{
    public class Make
    {
        [Key]
        public int MakeId { get; set; }

        [Required, MaxLength(128)]
        public string MakeName { get; set; }

        public virtual ICollection<Model> Models { get; set; } = new List<Model>();
    }
}
