using AutoMapper;
using salian_api.Contracts.Role;
using salian_api.Models;

namespace salian_api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() { 
            CreateMap<CreateRequest,Role>();
            CreateMap<UpdateRequest,Role>();
        }
    }
}
