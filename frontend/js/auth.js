document.addEventListener('DOMContentLoaded', () => {
    // Password visibility toggle
    const toggleBtns = document.querySelectorAll('.toggle-password');
  
    toggleBtns.forEach(btn => {
      btn.addEventListener('click', () => {
        const input = btn.parentElement.querySelector('input');
        const eyeIcon = btn.querySelector('.eye-icon');
        const eyeOffIcon = btn.querySelector('.eye-off-icon');
  
        if (input.type === 'password') {
          input.type = 'text';
          eyeIcon.classList.add('hidden');
          eyeOffIcon.classList.remove('hidden');
        } else {
          input.type = 'password';
          eyeIcon.classList.remove('hidden');
          eyeOffIcon.classList.add('hidden');
        }
      });
    });
  
    // Form submissions
    const loginForm = document.getElementById('loginForm');
    const signupForm = document.getElementById('signupForm');
  
    loginForm.addEventListener('submit', (e) => {
      e.preventDefault();
      const email = document.getElementById('loginEmail').value;
      const password = document.getElementById('loginPassword').value;
      const rememberMe = document.getElementById('rememberMe').checked;
      console.log('Login:', { email, password, rememberMe });
    });
  
    signupForm.addEventListener('submit', (e) => {
      e.preventDefault();
      const name = document.getElementById('signupName').value;
      const email = document.getElementById('signupEmail').value;
      const password = document.getElementById('signupPassword').value;
      const confirmPassword = document.getElementById('signupConfirmPassword').value;
      console.log('Signup:', { name, email, password, confirmPassword });
    });
  });