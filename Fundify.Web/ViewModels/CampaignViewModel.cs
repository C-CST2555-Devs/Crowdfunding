using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Fundify.Web.ViewModels
{
    public class CampaignViewModel
    {
        [Required(ErrorMessage = "Please enter a project title")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a category")]
        public string Category { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a funding goal")]
        [Range(1000, double.MaxValue, ErrorMessage = "Goal must be at least $1,000")]
        public decimal Goal { get; set; }

        [Required(ErrorMessage = "Please select a campaign duration")]
        [Range(30, 60, ErrorMessage = "Duration must be between 30 and 60 days")]
        public int DurationInDays { get; set; }

        // Make these optional since they're for later steps
        public string? Description { get; set; }
        public List<RewardViewModel> Rewards { get; set; } = new();

        [Display(Name = "Project Image")]
        [AllowedExtensions(new[] { ".jpg", ".jpeg", ".png" })]
        [MaxFileSize(5 * 1024 * 1024)] // 5MB
        public IFormFile? ImageFile { get; set; }
        public string? ImageFileName { get; set; }
    }

    public class RewardViewModel
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(1, double.MaxValue)]
        public decimal Amount { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        public int? Limit { get; set; }

        [Required]
        public DateTime EstimatedDelivery { get; set; }
    }

    // Add custom validation attributes
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _extensions;

        public AllowedExtensionsAttribute(string[] extensions)
        {
            _extensions = extensions;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName);
                if (!_extensions.Contains(extension.ToLower()))
                {
                    return new ValidationResult($"Only {string.Join(", ", _extensions)} files are allowed.");
                }
            }
            return ValidationResult.Success;
        }
    }

    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize;

        public MaxFileSizeAttribute(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                if (file.Length > _maxFileSize)
                {
                    return new ValidationResult($"File size cannot exceed {_maxFileSize / 1024 / 1024}MB.");
                }
            }
            return ValidationResult.Success;
        }
    }
} 