using Microsoft.AspNetCore.SignalR;


namespace sportify.PL.Hubs
{
    public class ScoreHub : Hub
    {
        private readonly ILogger<ScoreHub> _logger;

        public ScoreHub(ILogger<ScoreHub> logger)
        {
            _logger = logger;
        }

        public async Task JoinLeagueGroup(int leagueId)
        {
            _logger.LogInformation($"Client {Context.ConnectionId} joining league group {leagueId}");
            await Groups.AddToGroupAsync(Context.ConnectionId, $"league-{leagueId}");
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _logger.LogInformation($"Client {Context.ConnectionId} disconnected");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
