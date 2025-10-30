using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ObucaDanilo.Models
{
    public class User
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required, StringLength(50)]
        public string FirstName { get; set; }

        [Required, StringLength(50)]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        // Renamed from PasswordHash -> Password
        [Required]
        public string Password { get; set; }

        public bool IsAdmin { get; set; } = false;

        // New role flag
        public bool IsManager { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        // Prevent binding & validation on nav props
        [BindNever]
        [ValidateNever]
        public ICollection<Order>? Orders { get; set; }

        [BindNever]
        [ValidateNever]
        public ICollection<Cart>? Carts { get; set; }
    }
}