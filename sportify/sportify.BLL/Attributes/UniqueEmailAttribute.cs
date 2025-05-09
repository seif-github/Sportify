using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sportify.DAL.Data;

namespace sportify.BLL.Attributes
{
    public class UniqueEmailAttribute: ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var context = (SportifyContext)validationContext.GetService(typeof(SportifyContext));

            string email = value.ToString();
            bool emailExists = context.Users
                .Any(user => user.Email == email);
            return emailExists 
                ? new ValidationResult($"Email {email} is already registered.")
                : ValidationResult.Success;
        }
    }
}
