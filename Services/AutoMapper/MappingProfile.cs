using AutoMapper;
using Marketplace.DAL.Models;
using Marketplace.Services.DTOs.Product;

namespace Marketplace.Services.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductCreateDto>().ReverseMap();
            CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.CategoryName,
                opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.VendorName,
                opt => opt.MapFrom(src => src.Vendor.UserName))
            .ForMember(dest => dest.AverageRating,
                opt => opt.MapFrom(src =>
                    src.Reviews.Any()
                        ? MathF.Round((float)src.Reviews.Average(r => r.Rating), 2)
                        : 0))
            .ForMember(dest => dest.IsSaved, opt => opt.Ignore()) 
            .AfterMap((src, dest, context) =>
            {
                var userId = context.Items["customerId"] as string;
                if (!string.IsNullOrEmpty(userId))
                {
                    dest.IsSaved = src.SavedProducts.Any(sp => sp.CustomerId == userId);
                }
            });
        }
    }
}
