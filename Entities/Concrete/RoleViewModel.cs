using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class RoleViewModel
    {
        [Required(ErrorMessage = "Lütfen role adı girin")]
        public string Name { get; set; }
    }
}
