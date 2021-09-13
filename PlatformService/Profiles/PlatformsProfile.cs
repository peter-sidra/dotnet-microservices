using AutoMapper;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.Profiles {
    public class PlatformsProfile : Profile {
        public PlatformsProfile() {
            // This works without additional configuration because of 
            // the same naming convention used in both source and target classes

            // Source -> Target
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<PlatformCreateDto, Platform>();
            CreateMap<PlatformReadDto, PlatformPublishedDto>();
        }
    }
}