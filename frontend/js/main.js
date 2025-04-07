// Theme Toggle
const themeToggle = document.querySelector('.theme-toggle');
const html = document.documentElement;
const themeIcon = themeToggle.querySelector('i');

function toggleTheme() {
    const currentTheme = html.getAttribute('data-theme');
    const newTheme = currentTheme === 'light' ? 'dark' : 'light';
    
    html.setAttribute('data-theme', newTheme);
    themeIcon.className = `ph ph-${newTheme === 'light' ? 'sun' : 'moon'}`;
    
    localStorage.setItem('theme', newTheme);
}

// Set initial theme
const savedTheme = localStorage.getItem('theme') || 'light';
html.setAttribute('data-theme', savedTheme);
themeIcon.className = `ph ph-${savedTheme === 'light' ? 'sun' : 'moon'}`;

themeToggle.addEventListener('click', toggleTheme);

// Mobile Menu Toggle
const mobileMenuToggle = document.querySelector('.mobile-menu-toggle');
const navCenter = document.querySelector('.nav-center');

mobileMenuToggle.addEventListener('click', () => {
    navCenter.classList.toggle('active');
    mobileMenuToggle.querySelector('i').className = 
        navCenter.classList.contains('active') ? 'ph ph-x' : 'ph ph-list';
});

// Close mobile menu when clicking outside
document.addEventListener('click', (e) => {
    if (!e.target.closest('.nav-center') && 
        !e.target.closest('.mobile-menu-toggle') && 
        navCenter.classList.contains('active')) {
        navCenter.classList.remove('active');
        mobileMenuToggle.querySelector('i').className = 'ph ph-list';
    }
});

// Smooth scrolling for navigation links
document.querySelectorAll('a[href^="#"]').forEach(anchor => {
    anchor.addEventListener('click', function (e) {
        e.preventDefault();
        const target = document.querySelector(this.getAttribute('href'));
        if (target) {
            target.scrollIntoView({
                behavior: 'smooth',
                block: 'start'
            });
            // Close mobile menu after clicking a link
            if (navCenter.classList.contains('active')) {
                navCenter.classList.remove('active');
                mobileMenuToggle.querySelector('i').className = 'ph ph-list';
            }
        }
    });
});

// Navbar background on scroll
window.addEventListener('scroll', () => {
    const navbar = document.querySelector('.navbar');
    if (window.scrollY > 50) {
        navbar.style.padding = '0.75rem 2rem';
    } else {
        navbar.style.padding = '1rem 2rem';
    }
});

// Intersection Observer for animations
const observerOptions = {
    threshold: 0.1,
    rootMargin: '0px 0px -50px 0px'
};

const observer = new IntersectionObserver((entries) => {
    entries.forEach(entry => {
        if (entry.isIntersecting) {
            entry.target.style.opacity = '1';
            entry.target.style.transform = 'translateY(0)';
        }
    });
}, observerOptions);

// Animate elements on scroll
document.querySelectorAll('.feature-card, .step-card').forEach(element => {
    element.style.opacity = '0';
    element.style.transform = 'translateY(20px)';
    element.style.transition = 'all 0.6s ease-out';
    observer.observe(element);
});

// Simulate user logged-in state (you can replace this with actual login logic)
let isLoggedIn = true;

// Function to handle sign-in
function showLogin() {
  // Logic to handle sign-in, e.g., show a login modal
  isLoggedIn = true;
  updateUI();
}

// Function to handle sign-up
function showSignUp() {
  // Logic to handle sign-up, e.g., show a sign-up modal
}

// Function to handle sign-out
function signOut() {
  // Logic to handle sign-out, e.g., clear session, localStorage, etc.
  isLoggedIn = false;
  updateUI();
}

// Function to update the UI based on login state
function updateUI() {
  if (isLoggedIn) {
    // Show user buttons (Profile, Notifications, Sign Out)
    document.querySelector('.auth-buttons').style.display = 'none';
    document.querySelector('.user-buttons').style.display = 'flex';
  } else {
    // Show auth buttons (Loginn, Sign Up)
    document.querySelector('.auth-buttons').style.display = 'flex';
    document.querySelector('.user-buttons').style.display = 'none';
  }
}

// Call updateUI to set initial state
updateUI();
