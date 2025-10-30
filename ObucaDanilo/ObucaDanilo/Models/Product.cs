using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ObucaDanilo.Models
{
    public class Product
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required, StringLength(100)]
        public string Title { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required, Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [StringLength(255)]
        public string? ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        // ← Prevent binding & validation
        [BindNever]
        [ValidateNever]
        public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();

        // ← Also skip these if you get similar errors on Cart/Order navs:
        [BindNever]
        [ValidateNever]
        public ICollection<Cart>? Carts { get; set; }

        [BindNever]
        [ValidateNever]
        public ICollection<OrderItem>? OrderItems { get; set; }
    }
}