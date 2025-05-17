using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sportify.BLL.Services.Contracts
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string htmlContent);
        Task SendEmailVerificationAsync(string toEmail, string confirmationLink);
        Task SendPasswordResetEmailAsync(string email, string resetLink);

    }

}
