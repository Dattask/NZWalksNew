using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NZWalks.API.Controllers.Models.Domain;
using NZWalks.API.Data;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repository;
using System.Net;
using System.Net.Sockets;

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
            //Validate the request
            if (!ValidateAddRegion(addRegion))
            {
                return BadRequest(ModelState);
            }

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
            if (!ValidateUpdateRegion(updateRegionRequest))
            {
                return BadRequest(ModelState);
            }
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
            var regionResult = await _regionRepository.UpdateAsync(id, region);

            //If Null then NotFound
            if (regionResult == null)
            {
                return NotFound();
            }

            //convert Domain back to DTO
            var regionDTO = new Controllers.DTO.Region()
            {
                Area = regionResult.Area,
                Code = regionResult.Code,
                Name = regionResult.Name,
                Lat = regionResult.Lat,
                Long = regionResult.Long,
                Population = regionResult.Population
            };

            //Return Ok Response
            return Ok(regionDTO);

        }

        #region Private Method
        private bool ValidateAddRegion(API.Models.DTO.AddRegionRequest addRegionRequest)
        {
            if (addRegionRequest == null)
            {
                ModelState.AddModelError(nameof(addRegionRequest), $"Add Region Data is required !");
                return false;
            }

            if (string.IsNullOrWhiteSpace(addRegionRequest.Code))
            {
                ModelState.AddModelError(addRegionRequest.Code,
                    $"{nameof(addRegionRequest.Code)} can't be empty or white space!");
            }

            if (string.IsNullOrWhiteSpace(addRegionRequest.Name))
            {
                ModelState.AddModelError(addRegionRequest.Name,
                    $"{nameof(addRegionRequest.Name)} can't be empty or white space!");
            }

            if (addRegionRequest.Area <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Area),
                    $"{nameof(addRegionRequest.Area)} cant't be less than zero");
            }
            if (addRegionRequest.Lat <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Lat),
                    $"{nameof(addRegionRequest.Lat)} cant't be less than zero");
            }
            if (addRegionRequest.Long <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Long),
                    $"{nameof(addRegionRequest.Long)} cant't be less than zero");
            }
            if (addRegionRequest.Population <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Population),
                    $"{nameof(addRegionRequest.Population)} cant't be less than zero");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }

        private bool ValidateUpdateRegion(API.Models.DTO.UpdateRegionRequest updateRegionRequest)
        {
            if (string.IsNullOrWhiteSpace(updateRegionRequest.Name))
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Name),
                    $"{updateRegionRequest.Name} Can't be empty!");
            }
            if (string.IsNullOrWhiteSpace(updateRegionRequest.Code))
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Code),
                    $"{updateRegionRequest.Code} cant't be empty");
            }
            if (updateRegionRequest.Name.Length <= 2)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Name),
                    "should be minumum 2 characters");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }
        #endregion

    }
}
