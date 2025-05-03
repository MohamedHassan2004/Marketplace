using AutoMapper;
using Marketplace.BLL.DTOs;
using Marketplace.DAL.Models;
using Marketplace.DAL.Models.Users;
using Marketplace.Services.DTOs;
using Marketplace.Services.DTOs.Category;
using Marketplace.Services.DTOs.Order;
using Marketplace.Services.DTOs.Product;
using Marketplace.Services.DTOs.Vendor;
using System.Data;

namespace Marketplace.Services.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {

            CreateMap<Product, ProductCreateDto>().ReverseMap();
            CreateMap<Product, ProductUpdateDto>().ReverseMap();


            CreateMap<Product, ProductWithSavedDto>()
                .ForMember(dest => dest.CategoryName,
                    opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.VendorName,
                    opt => opt.MapFrom(src => src.Vendor.UserName))
                .ForMember(dest => dest.AdminCheckedName,
                    opt => opt.MapFrom(src => src.AdminChecked.UserName))
                .ForMember(dest => dest.IsSaved, opt => opt.Ignore())
                .ForMember(dest => dest.CanBeDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.CanBeUpdated, opt => opt.Ignore())
                .AfterMap((src, dest, context) =>
                {
                    var role = context.Items["role"] as string;
                    var userId = context.Items["userId"] as string;

                    // IsSaved logic
                    if (!string.IsNullOrEmpty(userId) && role == "Customer")
                    {
                        dest.IsSaved = src.SavedProducts.Any(sp => sp.CustomerId == userId);
                    }

                    // CanBeDeleted logic
                    if (!string.IsNullOrEmpty(userId))
                    {
                        if (role == "Admin")
                        {
                            dest.CanBeDeleted = true;
                        }
                        else if (role == "Vendor")
                        {
                            dest.CanBeDeleted = src.VendorId == userId;
                        }
                        else if (role == "Customer")
                        {
                            dest.CanBeDeleted = false;
                        }
                    }

                    // CanBeUpdated logic
                    dest.CanBeUpdated = !string.IsNullOrEmpty(userId) && userId == src.VendorId;

                    // CanBuy logic
                    dest.CanBuy = role == "Customer" && src.Quantity > 0;
                });


            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.CategoryName,
                    opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.VendorName,
                    opt => opt.MapFrom(src => src.Vendor.UserName))
                .ForMember(dest => dest.AdminCheckedName,
                    opt => opt.MapFrom(src => src.AdminChecked.UserName))
                .ForMember(dest => dest.CanBeDeleted, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.CanBeUpdated, opt => opt.Ignore())
                .AfterMap((src, dest, context) =>
                {
                    var userId = context.Items["userId"] as string;

                    // CanBeUpdated logic
                    dest.CanBeUpdated = !string.IsNullOrEmpty(userId) && userId == src.VendorId;
                });


            CreateMap<Category, CategoryDto>();
            CreateMap<Category, CategoryCreateDto>().ReverseMap();
            CreateMap<Vendor, VendorDto>();
            CreateMap<VendorPermission, VendorPermissionDto>();
            CreateMap<Permission, PermissionDto>().ReverseMap();


            CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src =>
                src.OrderItems.Sum(item => item.Quantity * item.Product.Price) + src.ShippingCost
            ));

            CreateMap<Notification, NotificationDto>();

        }
    }
}
