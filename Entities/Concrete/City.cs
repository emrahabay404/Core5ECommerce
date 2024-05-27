using System.ComponentModel.DataAnnotations;

namespace Entities.Concrete
{
    public class City
    {
        [Key]
        public int CityID { get; set; }
        public string CityName { get; set; }
        public int CountryID { get; set; }
        public List<Address> Address { get; set; }
    }
}
