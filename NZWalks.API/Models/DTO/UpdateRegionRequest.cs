using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class UpdateRegionRequest
    {
        //[MinLength(3, ErrorMessage = "Name should be greater than 2 characters!")]
        public string Name { get; set; }
        public string Code { get; set; }
        public double Area { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public long Population { get; set; }
    }
}
