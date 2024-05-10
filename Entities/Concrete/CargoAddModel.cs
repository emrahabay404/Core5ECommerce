using Microsoft.AspNetCore.Http;

namespace Entities.Concrete
{
   public class CargoAddModel
    {
        public string CargoName { get; set; }
        public IFormFile CargoLogo { get; set; }

        public bool Status { get; set; }
    }
}
