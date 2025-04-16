using AutoMapper;
using Marketplace.DAL.Models;
using Marketplace.Services.DTOs;
using Marketplace.Services.DTOs.Product;

namespace Marketplace.Services.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<Category, CategoryDto>().ReverseMap();
            
        }
    }
}
