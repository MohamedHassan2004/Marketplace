using AutoMapper;
using Marketplace.DAL.Models;
using Marketplace.Services.DTOs.Product;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Marketplace.Services.AutoMapper
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Product, ProductCreateDto>().ReverseMap();

            CreateMap<Product, AcceptedProductDto>()
            .ForMember(dest => dest.CategoryName,
                opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.VendorName,
                opt => opt.MapFrom(src => src.Vendor.UserName))
            .ForMember(dest => dest.AdminApprovedName,
                opt => opt.MapFrom(src => src.AdminChecked.UserName))
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

            CreateMap<Product, RejectedProductDto>()
            .ForMember(dest => dest.CategoryName,
                opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.VendorName,
                opt => opt.MapFrom(src => src.Vendor.UserName))
            .ForMember(dest => dest.AdminRejectedName,
                opt => opt.MapFrom(src => src.AdminChecked.UserName));

            CreateMap<Product, WaitingProductDto>()
            .ForMember(dest => dest.CategoryName,
                opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.VendorName,
                opt => opt.MapFrom(src => src.Vendor.UserName));

        }
    }
}
