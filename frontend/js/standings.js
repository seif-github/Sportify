// Sample data
const teams = [
    { 
        id: 'mci', 
        name: 'Manchester City',
        logo: 'https://images.pexels.com/photos/9402002/pexels-photo-9402002.jpeg?auto=compress&cs=tinysrgb&w=40&h=40&dpr=1',
        primaryColor: '#6CABDD'
    },
    { 
        id: 'liv', 
        name: 'Liverpool',
        logo: 'https://images.pexels.com/photos/9402002/pexels-photo-9402002.jpeg?auto=compress&cs=tinysrgb&w=40&h=40&dpr=1',
        primaryColor: '#C8102E'
    },
    // ... Add more teams as needed
];

const standings = [
    { team: teams[0], matchesPlayed: 38, wins: 29, draws: 6, losses: 3, goalsFor: 94, goalsAgainst: 26, points: 93 },
    { team: teams[1], matchesPlayed: 38, wins: 28, draws: 8, losses: 2, goalsFor: 94, goalsAgainst: 26, points: 92 },
    // ... Add more standings as needed
];

// Utility functions
function calculateGoalDifference(goalsFor, goalsAgainst) {
    return goalsFor - goalsAgainst;
}

function sortStandings(standings) {
    return [...standings].sort((a, b) => {
        if (b.points !== a.points) return b.points - a.points;
        
        const aGD = calculateGoalDifference(a.goalsFor, a.goalsAgainst);
        const bGD = calculateGoalDifference(b.goalsFor, b.goalsAgainst);
        
        if (bGD !== aGD) return bGD - aGD;
        return b.goalsFor - a.goalsFor;
    });
}

function createTeamRow(standing, rank) {
    const { team, matchesPlayed, wins, draws, losses, goalsFor, goalsAgainst, points } = standing;
    const goalDifference = calculateGoalDifference(goalsFor, goalsAgainst);
    
    return `
        <tr>
            <td class="text-center">
                <div class="rank-indicator mx-auto" style="background-color: ${team.primaryColor}">
                    ${rank}
                </div>
            </td>
            <td>
                <div class="d-flex align-items-center gap-3">
                    <img src="${team.logo}" alt="${team.name} logo" class="team-logo">
                    <span class="fw-medium">${team.name}</span>
                </div>
            </td>
            <td class="text-center d-none d-md-table-cell">${matchesPlayed}</td>
            <td class="text-center d-none d-md-table-cell">${wins}</td>
            <td class="text-center d-none d-md-table-cell">${draws}</td>
            <td class="text-center d-none d-md-table-cell">${losses}</td>
            <td class="text-center d-none d-lg-table-cell">${goalsFor}</td>
            <td class="text-center d-none d-lg-table-cell">${goalsAgainst}</td>
            <td class="text-center d-none d-sm-table-cell">${goalDifference}</td>
            <td class="text-center fw-bold">${points}</td>
        </tr>
    `;
}

function updateStandingsTable(standings) {
    const tableBody = document.getElementById('standingsTableBody');
    const sortedStandings = sortStandings(standings);
    
    tableBody.innerHTML = sortedStandings
        .map((standing, index) => createTeamRow(standing, index + 1))
        .join('');
}

// Search functionality
document.getElementById('teamSearch').addEventListener('input', (e) => {
    const searchTerm = e.target.value.toLowerCase();
    const filteredStandings = standings.filter(s => 
        s.team.name.toLowerCase().includes(searchTerm)
    );
    updateStandingsTable(filteredStandings);
});

// Initialize table
document.addEventListener('DOMContentLoaded', () => {
    updateStandingsTable(standings);
});