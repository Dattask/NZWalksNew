﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NZWalks.API.Repository;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("controller")]
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
        public async Task<IActionResult> AddWalkDifficulty(API.Models.DTO.AddWalkDifficulty walkDifficulty)
        {
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
        public async Task<IActionResult> UpdateWalkDifficulty([FromRoute] Guid id,
            [FromBody] API.Models.DTO.UpdateWalkDifficulty updateWalkDifficulty)
        {
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
    }
}
