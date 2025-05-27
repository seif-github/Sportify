document.addEventListener('DOMContentLoaded', function () {
    const searchInput = document.getElementById('searchInput');
    const clearSearch = document.getElementById('clearSearch');
    const leagueCards = document.querySelectorAll('.league-card');
    const leaguesContainer = document.getElementById('leaguesContainer');
    const noResults = document.getElementById('noResults');
    const statusFilterItems = document.querySelectorAll('.status-filter');
    const statusFilterButton = document.getElementById('statusFilterButton');

    let currentSearchTerm = '';
    let currentStatusFilter = 'all';

    function filterLeagues() {
        let visibleCount = 0;

        leagueCards.forEach(card => {
            const leagueName = card.querySelector('.league-name').textContent.toLowerCase();
            const cardStatus = card.getAttribute('data-status');

            const matchesSearch = currentSearchTerm === '' ||
                leagueName.includes(currentSearchTerm);
            const matchesStatus = currentStatusFilter === 'all' ||
                cardStatus === currentStatusFilter;

            if (matchesSearch && matchesStatus) {
                card.style.display = 'block';
                visibleCount++;
            } else {
                card.style.display = 'none';
            }
        });

        if (visibleCount > 0) {
            noResults.style.display = 'none';
            leaguesContainer.style.display = 'flex';
        } else {
            leaguesContainer.style.display = 'none';
            noResults.style.display = 'block';
        }
    }

    searchInput.addEventListener('input', function () {
        currentSearchTerm = this.value.toLowerCase();
        filterLeagues();
    });

    statusFilterItems.forEach(item => {
        item.addEventListener('click', function (e) {
            e.preventDefault();
            currentStatusFilter = this.getAttribute('data-status');
            statusFilterButton.textContent = this.textContent;
            filterLeagues();
        });
    });

    clearSearch.addEventListener('click', function () {
        searchInput.value = '';
        currentSearchTerm = '';
        filterLeagues();
        searchInput.focus();
    });

    filterLeagues();
});