$(document).ready(function() {
 
    

    // Mobile menu toggle
    $('.mobile-menu-toggle').on('click', function() {
      $('.nav-center').toggleClass('show-mobile-menu');
    });
    
    // File upload handler
    $('.upload-zone').on('click', function() {
      $('#bannerUpload').click();
    });
    
    $('#bannerUpload').on('change', function(e) {
      if (e.target.files.length > 0) {
        const fileName = e.target.files[0].name;
        $('.upload-zone p').text(`Selected: ${fileName}`);
        // Here you would typically handle the file upload
      }
    });
    
    // Form submission
    $('#leagueForm').on('submit', function(e) {
      e.preventDefault();
      // Show success message
      showToast('Changes saved successfully!', 'success');
    });
    
    // Demo dropdown toggle for mobile
    $('#profile').on('click', function() {
      $(this).find('.dropdown').toggle();
    });
    
    // Init tooltips
    $('[data-bs-toggle="tooltip"]').tooltip();
  });
  
 
  
  // Simple toast notification
  function showToast(message, type = 'info') {
    const toast = `
      <div class="toast-notification ${type}">
        <div class="toast-content">
          <i class="ph-bold ph-${type === 'success' ? 'check-circle' : 'info'}"></i>
          <span>${message}</span>
        </div>
        <button class="toast-close">&times;</button>
      </div>
    `;
    
    $('body').append(toast);
    
    setTimeout(() => {
      $('.toast-notification').addClass('show');
    }, 100);
    
    $('.toast-close').on('click', function() {
      $(this).parent('.toast-notification').removeClass('show');
      setTimeout(() => {
        $(this).parent('.toast-notification').remove();
      }, 300);
    });
    
    setTimeout(() => {
      $('.toast-notification').removeClass('show');
      setTimeout(() => {
        $('.toast-notification').remove();
      }, 300);
    }, 5000);
  }
  