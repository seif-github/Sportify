using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sportify.BLL.Services.Contracts;
using sportify.DAL.Data;

namespace sportify.BLL.Attributes
{
    public class UniqueEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // If the value is null or empty, return success (let Required attribute handle it)
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return ValidationResult.Success;
            }

            var context = (SportifyContext)validationContext.GetService(typeof(SportifyContext));
            if (context == null)
            {
                throw new InvalidOperationException("Could not retrieve database context");
            }

            string email = value.ToString();
            bool emailExists = context.Users.Any(user => user.Email == email);

            return emailExists
                ? new ValidationResult($"Email {email} is already registered.")
                : ValidationResult.Success;
        }
    }

}