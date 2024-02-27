using AutoMapper;
using PingAndCrop.Objects.Responses;
using PingAndCrop.Objects.ViewModels;

namespace PingAndCrop.RestAPI.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<PacResponse, PacResponseVm>()
                .ForMember(destiny => destiny.MessageId, source => source.MapFrom(src => src.Message.MessageId));
        }
    }
}
