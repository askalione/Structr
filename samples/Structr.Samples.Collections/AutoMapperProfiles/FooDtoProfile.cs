using AutoMapper;
using Structr.Samples.Collections.Dto;
using Structr.Samples.Collections.Entities;

namespace Structr.Samples.Collections.AutoMapperProfiles
{
    public class FooDtoProfile : Profile
    {
        public FooDtoProfile()
        {
            CreateMap<Foo, FooDto>()
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount.ToString() + " " + src.Currency.ToString().ToUpper()));
        }
    }
}
