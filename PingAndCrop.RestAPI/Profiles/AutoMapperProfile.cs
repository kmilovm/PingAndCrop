using AutoMapper;
using Azure.Storage.Queues.Models;
using Newtonsoft.Json;
using PingAndCrop.Objects.Models.Requests;
using PingAndCrop.Objects.Models.Responses;
using PingAndCrop.Objects.ViewModels;

namespace PingAndCrop.RestAPI.Profiles
{
    /// <summary>Provides the mapping between vms and models</summary>
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<PacResponse, PacResponseVm>();
            CreateMap<PacRequest, PacRequestVm>();
        }
    }
}
