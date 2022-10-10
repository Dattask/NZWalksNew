using AutoMapper;
using NZWalks.API.Controllers.DTO;
using NZWalks.API.Controllers.Models.Domain;

namespace NZWalks.API.Profiles
{
    public class RegionsProfile :Profile
    {
        public RegionsProfile()
        {
            CreateMap<Controllers.Models.Domain.Region, API.Controllers.DTO.Region>()
              //  .ForMember(options => options)
                .ReverseMap();
        }
    }
}
