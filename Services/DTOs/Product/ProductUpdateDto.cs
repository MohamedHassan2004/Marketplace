﻿using Marketplace.DAL.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Marketplace.Services.DTOs.Product
{
    public class ProductUpdateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public int Quantity { get; set; }
        public int CategoryId { get; set; }
    }
}
