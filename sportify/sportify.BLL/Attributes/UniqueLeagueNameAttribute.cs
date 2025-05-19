using sportify.DAL.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sportify.BLL.Attributes
{
    public class UniqueLeagueNameAttribute : ValidationAttribute
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

            string LeagueName = value.ToString();
            bool LeagueNameExist = context.Leagues.Any(league => league.Name == LeagueName);

            return LeagueNameExist
                ? new ValidationResult($"League Name ({LeagueName}) is already exist.")
                : ValidationResult.Success;
        }
    }
}
