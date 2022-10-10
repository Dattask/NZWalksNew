﻿namespace NZWalks.API.Controllers.Models.Domain
{
    public class Walk
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Length { get; set; }

        public Guid RegionId { get; set; }
        public Guid WalkDifficultyId { get; set; }

        //Navigation Property
        public Region Regions { get; set; }
        public WalkDifficulty WalkDifficulty { get; set; }
    }
}
