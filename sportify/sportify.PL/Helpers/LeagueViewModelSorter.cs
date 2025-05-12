using sportify.PL.ViewModels;

namespace sportify.PL.Helpers
{
    public static class LeagueViewModelSorter
    {
        public static List<LeagueWithOrganizerViewModel> SortLeagues<TKey>(
            List<LeagueWithOrganizerViewModel?>? leagues,
            Func<LeagueWithOrganizerViewModel, TKey> keySelector,
            bool asceding = true)
        {
            if (leagues == null || leagues.Count == 0) return new List<LeagueWithOrganizerViewModel>();
            if (leagues.Count == 1) return leagues;

            var filteredLeagues = leagues
                .Where(l => l != null && l.League != null)
                .ToList();

            return asceding
                ? filteredLeagues.OrderBy(keySelector).ToList()
                : filteredLeagues.OrderByDescending(keySelector).ToList();
        }

        public static List<LeagueWithOrganizerViewModel> SortLeaguesByDate(
                        List<LeagueWithOrganizerViewModel?>? leagues,
                        bool ascending = true)
        {
            return SortLeagues(leagues, l => l!.League.StartDate, ascending);
        }

        public static List<LeagueWithOrganizerViewModel> SortLeaguesByTeamCount(
                        List<LeagueWithOrganizerViewModel?>? leagues,
                        bool ascending = true)
        {
            return SortLeagues(leagues, l => l!.League.NumberOfTeams, ascending);
        }

        public static List<LeagueWithOrganizerViewModel> SortLeaguesByName(
                        List<LeagueWithOrganizerViewModel?>? leagues,
                        bool ascending = true)
        {
            return SortLeagues(leagues, l => l!.League.Name, ascending);
        }
    }
}
