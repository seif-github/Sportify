document.addEventListener('DOMContentLoaded', () => {
    // Password visibility toggle with Edge support
    const togglePasswordVisibility = (input, eyeIcon, eyeOffIcon) => {
        if (input.type === 'password') {
            input.type = 'text';
            eyeIcon.classList.add('hidden');
            eyeOffIcon.classList.remove('hidden');
        } else {
            input.type = 'password';
            eyeIcon.classList.remove('hidden');
            eyeOffIcon.classList.add('hidden');
        }
    };

    document.querySelectorAll('.toggle-password').forEach(btn => {
        const input = btn.closest('.input-group').querySelector('input');
        const eyeIcon = btn.querySelector('.eye-icon');
        const eyeOffIcon = btn.querySelector('.eye-off-icon');

        // Only add our toggle if Edge's native toggle is hidden
        if (window.navigator.userAgent.includes('Edge') ||
            window.navigator.userAgent.includes('Edg')) {
            input.style.paddingRight = '2.5rem';
        }

        btn.addEventListener('click', () => {
            togglePasswordVisibility(input, eyeIcon, eyeOffIcon);
        });
    });
});