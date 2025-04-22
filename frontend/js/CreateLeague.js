// import javascriptLogo from './javascript.svg'
// import viteLogo from 'vite.svg'
// import { setupCounter } from 'counter.js'



$(document).ready(function() {
    // Initialize datepickers
    $(".datepicker").datepicker({
      dateFormat: 'mm/dd/yy',
      showOtherMonths: true,
      selectOtherMonths: true,
      changeMonth: true,
      changeYear: true,
      yearRange: 'c-0:c+5'
    });
  
    // Function to update progress steps
    function updateSteps(currentStep) {
      // Remove active class from all steps
      $('.step').removeClass('active completed');
      
      // Add active class to current step
      $(`.step[data-step="${currentStep}"]`).addClass('active');
      
      // Add completed class to steps before current
      for (let i = 1; i < currentStep; i++) {
        $(`.step[data-step="${i}"]`).addClass('completed');
      }
      
      // Hide all form steps and show the current one
      $('.form-step').removeClass('active');
      $(`.form-step[data-step="${currentStep}"]`).addClass('active');
    }
  
    // Step 1 to Step 2
    $('#next-step-1').on('click', function() {
      // Validate form fields
      const leagueName = $('#league-name').val();
      const sportType = $('#sport-type').val();
      const startDate = $('#start-date').val();
      const endDate = $('#end-date').val();
      
      let isValid = true;
      
      if (!leagueName) {
        $('#league-name').css('border-color', 'red');
        isValid = false;
      } else {
        $('#league-name').css('border-color', '');
      }
      
      if (!sportType) {
        $('.custom-select').css('border-color', 'red');
        isValid = false;
      } else {
        $('.custom-select').css('border-color', '');
      }
      
      if (!startDate) {
        $('#start-date').css('border-color', 'red');
        isValid = false;
      } else {
        $('#start-date').css('border-color', '');
      }
      
      if (!endDate) {
        $('#end-date').css('border-color', 'red');
        isValid = false;
      } else {
        $('#end-date').css('border-color', '');
      }
      
      if (isValid) {
        generateTeams();
        updateSteps(2);
      }
    });
  
    // Step 2 to Step 1 (back)
    $('#prev-step-2').on('click', function() {
      updateSteps(1);
    });
  
    // Step 2 to Step 3
    $('#next-step-2').on('click', function() {
      updateSteps(3);
    });
  
    // Step 3 to Step 2 (back)
    $('#prev-step-3').on('click', function() {
      updateSteps(2);
    });
  
    // Finish form
    $('#finish').on('click', function() {
      // Collect all form data
      const formData = {
        leagueDetails: {
          name: $('#league-name').val(),
          sportType: $('#sport-type').val(),
          startDate: $('#start-date').val(),
          endDate: $('#end-date').val()
        },
        teams: getTeamData(),
        schedule: {
          type: $('#schedule-type').val(),
          matchDuration: $('#match-duration').val(),
          gameDays: getSelectedDays()
        }
      };
      
      // Here you would typically submit the data to your server
      console.log('Form data:', formData);
      
      // Show success message or redirect
      alert('League created successfully!');
      
      // For demo purposes, reset form
      resetForm();
    });
  
    // Function to generate team list based on team count
    function generateTeams() {
      const teamCount = parseInt($('#team-count').val()) || 8;
      let teamListHtml = '';
      
      for (let i = 1; i <= teamCount; i++) {
        teamListHtml += `
          <div class="team-item" data-team-id="${i}">
            <div class="team-number">${i}</div>
            <input type="text" class="form-control team-name" placeholder="Team ${i} name">
            <div class="team-actions">
              <button type="button" class="remove-team"><i class="ph ph-trash"></i></button>
            </div>
          </div>
        `;
      }
      
      $('#team-list').html(teamListHtml);
      
      // Add event listener for remove team button
      $('.remove-team').on('click', function() {
        $(this).closest('.team-item').remove();
        updateTeamNumbers();
      });
    }
  
    // Function to update team numbers after removal
    function updateTeamNumbers() {
      $('.team-item').each(function(index) {
        $(this).find('.team-number').text(index + 1);
        $(this).attr('data-team-id', index + 1);
        
        // Update placeholder if input is empty
        const nameInput = $(this).find('.team-name');
        if (!nameInput.val()) {
          nameInput.attr('placeholder', `Team ${index + 1} name`);
        }
      });
    }
  
    // Add new team
    $('#add-team').on('click', function() {
      const teamCount = $('.team-item').length + 1;
      
      const newTeamHtml = `
        <div class="team-item" data-team-id="${teamCount}">
          <div class="team-number">${teamCount}</div>
          <input type="text" class="form-control team-name" placeholder="Team ${teamCount} name">
          <div class="team-actions">
            <button type="button" class="remove-team"><i class="ph ph-trash"></i></button>
          </div>
        </div>
      `;
      
      $('#team-list').append(newTeamHtml);
      
      // Add event listener for new remove button
      $('.remove-team').last().on('click', function() {
        $(this).closest('.team-item').remove();
        updateTeamNumbers();
      });
    });
  
    // Update team count when manually changing the number
    $('#team-count').on('change', function() {
      generateTeams();
    });
  
    // Get team data
    function getTeamData() {
      const teams = [];
      
      $('.team-item').each(function() {
        const teamId = $(this).data('team-id');
        const teamName = $(this).find('.team-name').val() || `Team ${teamId}`;
        
        teams.push({
          id: teamId,
          name: teamName
        });
      });
      
      return teams;
    }
  
    // Get selected game days
    function getSelectedDays() {
      const selectedDays = [];
      
      $('.checkbox-group input:checked').each(function() {
        selectedDays.push($(this).val());
      });
      
      return selectedDays;
    }
  
    // Reset form
    function resetForm() {
      // Clear form fields
      $('#league-name').val('');
      $('#sport-type').val('');
      $('#start-date').val('');
      $('#end-date').val('');
      
      // Reset to step 1
      updateSteps(1);
    }
  
    // Input focus animations
    $('.form-control').on('focus', function() {
      $(this).parent().addClass('focused');
    }).on('blur', function() {
      $(this).parent().removeClass('focused');
    });
  
    // Initialize with step 1
    updateSteps(1);
    
    // Generate initial teams
    generateTeams();
  
    // Form validation visual feedback
    $('.form-control').on('input', function() {
      if ($(this).val()) {
        $(this).css('border-color', '');
      }
    });
  
    // Custom form validation styling
    $('.form-control').on('invalid', function(e) {
      e.preventDefault();
      $(this).css('border-color', 'red');
    });
  });
  
  document.addEventListener('DOMContentLoaded', () => {
    // const themeToggle = document.getElementById('themeToggle');
    // const prefersDarkScheme = window.matchMedia('(prefers-color-scheme: dark)');
  
    // if (prefersDarkScheme.matches) {
    //   document.body.setAttribute('data-theme', 'dark');
    //   themeToggle.textContent = '☀️';
    // }
  
    // themeToggle.addEventListener('click', () => {
    //   const currentTheme = document.body.getAttribute('data-theme');
    //   if (currentTheme === 'dark') {
    //     document.body.removeAttribute('data-theme');
    //     themeToggle.textContent = '🌙';
    //   } else {
    //     document.body.setAttribute('data-theme', 'dark');
    //     themeToggle.textContent = '☀️';
    //   }
    // });
  
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
  