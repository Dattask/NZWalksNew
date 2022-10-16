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
        private readonly IRegionRepository _regionRepository;
        private readonly IWalkDifficultyRepository _walkDifficultyRepository;

        public Walks(IWalkRepository walkRepository, IMapper mapper, IRegionRepository regionRepository, IWalkDifficultyRepository walkDifficultyRepository)
        {
            _walkRepository = walkRepository;
            _mapper = mapper;
            _regionRepository = regionRepository;
            _walkDifficultyRepository = walkDifficultyRepository;
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
            if (!await ValidateAddWalk(addWalkRequest))
            {
                return BadRequest(ModelState);
            }

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
            if (! await ValidateUpdateWalk(updateWalkRequest))
            {
                return BadRequest(ModelState);
            }

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

        #region Private Methods
        private async Task<bool> ValidateAddWalk(API.Models.DTO.AddWalkRequest addWalkRequest)
        {
            //Below Commented instead used FLUENT VALIDATION FOR THESE 2 FIELDS

            //if (string.IsNullOrWhiteSpace(addWalkRequest.Name))
            //{
            //    ModelState.AddModelError(nameof(addWalkRequest.Name), "Name should not be empty");
            //}

            //if (addWalkRequest.Length <= 0)
            //{
            //    ModelState.AddModelError(nameof(addWalkRequest.Length), "Length should be greater than zero");
            //}

            var region = await _regionRepository.GetRegionByIdAsync(addWalkRequest.RegionId);
            if (region == null)
                ModelState.AddModelError(nameof(addWalkRequest.RegionId), "invalid region id");

            var walkDifficulty = await _walkRepository.GetWalkByIdAsync(addWalkRequest.WalkDifficultyId);
            if (walkDifficulty == null)
                ModelState.AddModelError(nameof(addWalkRequest.WalkDifficultyId), "invalid walk Difficulty Id");

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }

        private async Task<bool> ValidateUpdateWalk(API.Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            //Below Commented instead used FLUENT VALIDATION FOR THESE 2 FIELDS

            //if (string.IsNullOrWhiteSpace(updateWalkRequest.Name))
            //{
            //    ModelState.AddModelError(nameof(updateWalkRequest.Name), $"{updateWalkRequest.Name} can't be empty");
            //}

            //if (updateWalkRequest.Length <= 0)
            //{
            //    ModelState.AddModelError(nameof(updateWalkRequest.Length), "Lenth should not less than zero");
            //}

            //Used to check Given regionId Already present or not in Region table.(PK_FK)
            var regionId = await _regionRepository.GetRegionByIdAsync(updateWalkRequest.RegionId);
            if (regionId == null)
                ModelState.AddModelError(nameof(updateWalkRequest.RegionId), "invalid region Id");

            //Used to check Given WalkDifficulty Already present or not in WD table.(PK_FK)
            var walkDifficulty = await _walkDifficultyRepository.GetWalkDifficutlyByIdAsync(updateWalkRequest.WalkDifficultyId);
            if (walkDifficulty == null)
                ModelState.AddModelError(nameof(updateWalkRequest.WalkDifficultyId), "invalid WalkDifficulty Id!");

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }
    }
    #endregion
}
