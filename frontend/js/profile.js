document.addEventListener('DOMContentLoaded', () => {
    // const themeToggle = document.getElementById('themeToggle');
    // const themeIcon = themeToggle.querySelector('i');
    // const prefersDarkScheme = window.matchMedia('(prefers-color-scheme: dark)');
    
    // // Set initial theme
    // if (prefersDarkScheme.matches) {
    //   document.body.setAttribute('data-theme', 'dark');
    //   themeIcon.classList.remove('fa-moon');
    //   themeIcon.classList.add('fa-sun');
    // }
    
    // themeToggle.addEventListener('click', () => {
    //   const currentTheme = document.body.getAttribute('data-theme');
    //   if (currentTheme === 'dark') {
    //     document.body.removeAttribute('data-theme');
    //     themeIcon.classList.remove('fa-sun');
    //     themeIcon.classList.add('fa-moon');
    //   } else {
    //     document.body.setAttribute('data-theme', 'dark');
    //     themeIcon.classList.remove('fa-moon');
    //     themeIcon.classList.add('fa-sun');
    //   }
    // });
  
    // Toggle switches animation
    const toggles = document.querySelectorAll('.toggle input');
    toggles.forEach(toggle => {
      toggle.addEventListener('change', function() {
        const slider = this.nextElementSibling;
        if (this.checked) {
          slider.style.backgroundColor = 'var(--primary-green)';
        } else {
          slider.style.backgroundColor = 'var(--bg-tertiary)';
        }
      });
    });
  
    // Button hover effects
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