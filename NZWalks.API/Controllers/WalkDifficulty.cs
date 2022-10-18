using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NZWalks.API.Repository;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("WalkDifficulty")]
    public class WalkDifficulty : Controller
    {
        private readonly IWalkDifficultyRepository _walkDifficultyRepository;
        private readonly IMapper _mapper;

        public WalkDifficulty(IWalkDifficultyRepository walkDifficultyRepository, IMapper mapper)
        {
            _walkDifficultyRepository = walkDifficultyRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "reader")]
        public async Task<IActionResult> GetAllWalkDifficulty()
        {
            //Get data from Database
            var walkDifficulty = await _walkDifficultyRepository.GetAllAsync();

            // Convert Domain to DTO object
            var walkDifficultyDTO = _mapper.Map<List<API.Models.DTO.WalkDifficulty>>(walkDifficulty);

            //Return the response
            return Ok(walkDifficultyDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkDifficultyById")]
        [Authorize(Roles = "reader")]
        public async Task<IActionResult> GetWalkDifficultyById(Guid id)
        {
            // call the repository and get the details
            var walkDifficulty = await _walkDifficultyRepository.GetWalkDifficutlyByIdAsync(id);

            //handle the null
            if (walkDifficulty == null)
            {
                return NotFound();
            }

            // convert Domain to DTO object using Mapper
            var walkDifficultyDTO = _mapper.Map<API.Models.DTO.WalkDifficulty>(walkDifficulty);

            // return the response
            return Ok(walkDifficultyDTO);
        }

        [HttpPost]
        [Authorize( Roles ="writer")]
        public async Task<IActionResult> AddWalkDifficulty(API.Models.DTO.AddWalkDifficulty walkDifficulty)
        {
            //Validate the DTO before processing  -> commented instead used FLUENT VALIDATION
            //if (!ValidateAddWalkDifficulty(walkDifficulty))
            //{
            //    return BadRequest(ModelState);
            //}

            // Convert DTO to Domain
            var walkDifficultyDomain = new Controllers.Models.Domain.WalkDifficulty()
            {
                Code = walkDifficulty.Code
            };

            // Call Repository to post data
            walkDifficultyDomain = await _walkDifficultyRepository.AddWalkDifficultyAsync(walkDifficultyDomain);

            // convert Domain back to DTO using Mapper
            var walkDifficultyDTO = _mapper.Map<API.Models.DTO.WalkDifficulty>(walkDifficultyDomain);

            //convert Domain back to DTO using DTO Model
            //var walkDifficultyDTO = new API.Models.DTO.WalkDifficulty()
            //{
            //     Id = walkDifficultyDomain.Id,
            //     Code= walkDifficultyDomain.Code
            //};

            //return the response
            return CreatedAtAction(nameof(GetWalkDifficultyById), new { id = walkDifficultyDTO.Id }, walkDifficultyDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> DeleteWalkDifficulty(Guid id)
        {
            //call the Repository to delete the record
            var walkDifficulty = await _walkDifficultyRepository.DeleteWalkDifficultyAsync(id);

            // convert to DTO from Domain Model
            var walkDifficultyDTO = _mapper.Map<API.Models.DTO.WalkDifficulty>(walkDifficulty);

            //return the response
            return Ok(walkDifficultyDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> UpdateWalkDifficulty([FromRoute] Guid id,
            [FromBody] API.Models.DTO.UpdateWalkDifficulty updateWalkDifficulty)
        {
            //Validate Model before processing  - instead used FLUENT VALIDATION
            //if (!ValidateUpdateWalkDifficulty(updateWalkDifficulty))
            //{
            //    return BadRequest(ModelState);
            //}

            //Convert to Domain from DTO
            var walkDifficultyDomain = new Controllers.Models.Domain.WalkDifficulty()
            {
                Code = updateWalkDifficulty.Code
            };

            //Call the repository
            walkDifficultyDomain = await _walkDifficultyRepository.UpdateWalkDifficultyAsync(id, walkDifficultyDomain);

            //Convert back to DTO from Domain
            //var walkDifficultyDTO = new API.Models.DTO.WalkDifficulty()
            //{
            //    Id = walkDifficultyDomain.Id,
            //    Code=walkDifficultyDomain.Code
            //};

            var walkDifficultyDTO = _mapper.Map<API.Models.DTO.WalkDifficulty>(walkDifficultyDomain);

            //Return the response
            return Ok(walkDifficultyDTO);
        }

        #region Private Method
        private bool ValidateAddWalkDifficulty(API.Models.DTO.AddWalkDifficulty walkDifficulty)
        {
            if (string.IsNullOrWhiteSpace(walkDifficulty.Code))
            {
                ModelState.AddModelError(nameof(walkDifficulty.Code), $"{walkDifficulty.Code} can't be empty");
            }
            if (walkDifficulty.Code.Length < 2)
            {
                ModelState.AddModelError(nameof(walkDifficulty.Code), "Code should be less than 2 characters");
            }
            if (ModelState.ErrorCount > 0)
                return false;

            return true;
        }

        private bool ValidateUpdateWalkDifficulty(API.Models.DTO.UpdateWalkDifficulty updateWalkDifficulty)
        {
            if (string.IsNullOrWhiteSpace(updateWalkDifficulty.Code))
            {
                ModelState.AddModelError(nameof(updateWalkDifficulty.Code), $"Can't be less empty");
            }

            if (updateWalkDifficulty.Code.Length < 2)
            {
                ModelState.AddModelError(nameof(updateWalkDifficulty.Code), "Code should be greater than 2 characters");
            }

            if (ModelState.IsValid)
                return true;

            return false;
        }
        #endregion
    }
}
