using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NZWalks.API.Controllers.Models.Domain;
using NZWalks.API.Data;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repository;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("Regions")]
    public class RegionsController : Controller
    {
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            _regionRepository = regionRepository;
            _mapper = mapper;
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

            var regionsDTO = _mapper.Map<List<DTO.Region>>(regions);

            return Ok(regionsDTO);

        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetRegionById")]
        public async Task<IActionResult> GetRegionById(Guid id)
        {
            var region = await _regionRepository.GetRegionByIdAsync(id);
            if (region == null)
            {
                return NotFound();
            }
            var regionDTO = _mapper.Map<DTO.Region>(region);
            return Ok(regionDTO);
        }


        [HttpPost]
        public async Task<IActionResult> AddRegionAsync(AddRegionRequest addRegion)
        {
            //Convert Request DTO to domain Model
            var region = new Models.Domain.Region()
            {
                Code = addRegion.Code,
                Area = addRegion.Area,
                Lat = addRegion.Lat,
                Long = addRegion.Long,
                Name = addRegion.Name,
                Population = addRegion.Population
            };

            //Pass details to Repository(Database)
            region = await _regionRepository.AddAsync(region);

            //Convert back to DTO
            var regionDTO = new Controllers.DTO.Region()
            {
                Id = region.Id,
                Area = region.Area,
                Name = region.Name,
                Code = region.Code,
                Lat = region.Lat,
                Long = region.Long,
                Population = region.Population
            };
            return CreatedAtAction(nameof(GetRegionById), new { id = regionDTO.Id }, region);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteRegionByIdAsync(Guid id)
        {
            // Get Region from DB and Delete it.

            var region = await _regionRepository.DeleteAsync(id);

            // If NULL NotFound
            if (region == null)
            {
                return NotFound();
            }

            //Convert response back to DTO Model
            var regionDTO = new Controllers.DTO.Region()
            {
                Id = region.Id,
                Area = region.Area,
                Name = region.Name,
                Code = region.Code,
                Lat = region.Lat,
                Long = region.Long,
                Population = region.Population
            };

            //return OK response.

            return Ok(regionDTO);

        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateRegionsById([FromRoute] Guid id, [FromBody] UpdateRegionRequest updateRegionRequest)
        {
            //Convert DTO to Domain Model
            var region = new Controllers.Models.Domain.Region()
            {
                Name = updateRegionRequest.Name,
                Area = updateRegionRequest.Area,
                Code = updateRegionRequest.Code,
                Lat = updateRegionRequest.Lat,
                Long = updateRegionRequest.Long,
                Population = updateRegionRequest.Population
            };

            //Update region using repository
            var regionResult =  await _regionRepository.UpdateAsync(id, region);

            //If Null then NotFound
            if (regionResult == null)
            {
                return NotFound();
            }

            //convert Domain back to DTO
            var regionDTO = new Controllers.DTO.Region()
            {
                 Area = regionResult.Area,
                 Code=regionResult.Code,
                 Name = regionResult.Name,
                 Lat = regionResult.Lat,
                 Long= regionResult.Long,
                 Population= regionResult.Population
            };

            //Return Ok Response
            return Ok(regionDTO);

        }
    }
}
