// Theme Toggle
document.addEventListener('DOMContentLoaded', () => {
    const themeToggle = document.getElementById('themeToggle');
    const prefersDarkScheme = window.matchMedia('(prefers-color-scheme: dark)');
  
    if (prefersDarkScheme.matches) {
      document.body.setAttribute('data-theme', 'dark');
      themeToggle.textContent = '☀️';
    }
  
    themeToggle.addEventListener('click', () => {
      const currentTheme = document.body.getAttribute('data-theme');
      if (currentTheme === 'dark') {
        document.body.removeAttribute('data-theme');
        themeToggle.textContent = '🌙';
      } else {
        document.body.setAttribute('data-theme', 'dark');
        themeToggle.textContent = '☀️';
      }
    });
  
    // Button functionality
    document.querySelector('.create-league')?.addEventListener('click', () => {
      console.log('Create League clicked');
    });
  
    document.querySelector('.schedule-match')?.addEventListener('click', () => {
      console.log('Schedule Match clicked');
    });
  
    // Pagination
    document.querySelector('.prev-btn')?.addEventListener('click', () => {
      console.log('Previous page');
    });
  
    document.querySelector('.next-btn')?.addEventListener('click', () => {
      console.log('Next page');
    });
  
    // Smooth hover effect
    const buttons = document.querySelectorAll('button');
    buttons.forEach(button => {
      button.addEventListener('mousedown', () => {
        button.style.transform = 'scale(0.98)';
      });
      button.addEventListener('mouseup', () => {
        button.style.transform = '';
      });
      button.addEventListener('mouseleave', () => {
        button.style.transform = '';
      });
    });
  });
  