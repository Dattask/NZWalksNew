using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NZWalks.API.Controllers.Models.Domain;
using NZWalks.API.Repository;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("Regions")]
    public class RegionsController : Controller
    {
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            _regionRepository = regionRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRegions()
        {
            var regions = await _regionRepository.GetAllAsync();

            // Use DTOs instead of Direct Model Classes
            // 1. Create DTO Class //2. Assign values to DTO fiels from Actual Model //3. Return the DTO.

            //var regionsDTO = new List<DTO.Region>();

            //regions.ToList().ForEach(regions =>
            //{
            //    var data = new DTO.Region()
            //    { 
            //        Id = regions.Id,
            //        RegionName = regions.Name,
            //        RegionCode = regions.Code,
            //        Area = regions.Area,
            //        Lat = regions.Lat,
            //        Long = regions.Long,
            //        Population = regions.Population
            //    };
            //    regionsDTO.Add(data);
            //});

            var regionsDTO = mapper.Map<List<DTO.Region>>(regions);

            return Ok(regionsDTO);
        }
    }
}
