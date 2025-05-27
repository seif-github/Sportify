using Microsoft.EntityFrameworkCore;
using sportify.DAL.Data;
using System.ComponentModel.DataAnnotations;

namespace sportify.BLL.Attributes
{
    public class UniqueTeamNameInLeagueAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return ValidationResult.Success;
            }

            var context = (SportifyContext)validationContext.GetService(typeof(SportifyContext));
            if (context == null)
            {
                throw new InvalidOperationException("Could not retrieve database context");
            }

            var teamName = value.ToString().Trim();
            var instance = validationContext.ObjectInstance;

            var leagueIdProperty = instance.GetType().GetProperty("LeagueID");
            if (leagueIdProperty == null)
            {
                throw new InvalidOperationException("LeagueID property not found");
            }

            var leagueId = (int)leagueIdProperty.GetValue(instance);

            bool TeamNameInLeagueExist = context.Teams.Any(team => team.Name == teamName && team.LeagueID == leagueId);

            return TeamNameInLeagueExist
                ? new ValidationResult($"Team Name ({teamName}) is already exist.")
                : ValidationResult.Success;
        }
    }
}