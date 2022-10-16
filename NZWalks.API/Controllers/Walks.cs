using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repository;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NZWalks.API.Controllers.Models.Domain;


namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("Walks")]
    public class Walks : Controller
    {
        private readonly IWalkRepository _walkRepository;
        private readonly IMapper _mapper;

        public Walks(IWalkRepository walkRepository, IMapper mapper)
        {
            _walkRepository = walkRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalksAsync()
        {
            // Get data from database
            var walkList = await _walkRepository.GetAllAsync();

            //Mapped to DTO using IMapper object
            var walksDTO = _mapper.Map<List<API.Models.DTO.Walk>>(walkList);

            //Return Response
            return Ok(walksDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkByIdAsync")]
        public async Task<IActionResult> GetWalkByIdAsync(Guid id)
        {
            // Get the Walk details from DB
            var walk = await _walkRepository.GetWalkByIdAsync(id);

            // Convert to Domain object to DTO
            var walkDTO = _mapper.Map<API.Models.DTO.Walk>(walk);

            //Return the response
            return Ok(walkDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkAsync(API.Models.DTO.AddWalkRequest addWalkRequest)
        {
            //convert  DTO to Domain
            var walkDomain = new Models.Domain.Walk()
            {
                Length = addWalkRequest.Length,
                RegionId = addWalkRequest.RegionId,
                Name = addWalkRequest.Name,
                WalkDifficultyId = addWalkRequest.WalkDifficultyId
            };

            //Pass Domain object to persist this.
            walkDomain = await _walkRepository.AddWalkAsync(walkDomain);

            // Convert Domain object to DTO 
            var walkDTO = new API.Models.DTO.Walk()
            {
                Id = walkDomain.Id,
                Name = walkDomain.Name,
                Length = walkDomain.Length,
                RegionId = walkDomain.RegionId,
                WalkDifficultyId = walkDomain.WalkDifficultyId
            };

            //Return the response
            return CreatedAtAction(nameof(GetWalkByIdAsync), new { id = walkDTO.Id }, walkDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, [FromBody] API.Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            // Convert Domain to DTO
            var walkDomain = new Models.Domain.Walk()
            {
                Length = updateWalkRequest.Length,
                Name = updateWalkRequest.Name,
                RegionId = updateWalkRequest.RegionId,
                WalkDifficultyId = updateWalkRequest.WalkDifficultyId
            };

            //Pass the DTO to Repository object to save the changes.
            walkDomain = await _walkRepository.UpdateWalkAsync(id, walkDomain);

            //Handle Null
            if (walkDomain == null)
            {
                return NotFound();
            }

            //Convert Back Domain to DTO object
            var walkDTO = new API.Models.DTO.Walk()
            {
                Id = walkDomain.Id,
                Length = walkDomain.Length,
                Name = walkDomain.Name,
                WalkDifficultyId = walkDomain.WalkDifficultyId,
                RegionId = walkDomain.RegionId
            };

            //return the response
            return Ok(walkDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkAsync([FromRoute] Guid id)
        {
            //Get Walk from Db and Delete it
            var walk = await _walkRepository.DeleteWalkByIdAsync(id);

            //Handle Null
            if (walk == null)
            {
                return NotFound();
            }

            //Convert Domain to DTO
            var walkDTO = new API.Models.DTO.Walk()
            {
                Id = walk.Id,
                Name = walk.Name,
                Length = walk.Length,
                WalkDifficultyId = walk.WalkDifficultyId,
                RegionId = walk.RegionId
            };

            //Return the response
            return Ok(walkDTO);
        }
    }
}
