﻿namespace ForumNet.Services.Common.Attributes
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.Extensions.DependencyInjection;

    using Contracts;

    public class ValidateCategoryNameAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var categoryService = validationContext.GetService<ICategoriesService>();
            var isExisting = categoryService.IsExisting((string) value).GetAwaiter().GetResult();
            if (isExisting)
            {
                return new ValidationResult(this.ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}