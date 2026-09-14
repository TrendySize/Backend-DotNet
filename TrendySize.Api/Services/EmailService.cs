using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TrendySize.Api.DTOs;
using TrendySize.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Resend;
using TrendySize.Api.Services;


namespace TrendySize.Api.Services
{
    public class EmailService : IEmailService
    {
        // The EmailService class implements the IResend interface and provides functionality 
        //for sending email verification messages.
        private readonly IResend _resendClient;

        // Constructor for the EmailService class, accepting an IResend instance as a dependency.

        public EmailService(IResend resendClient)
        {
            _resendClient = resendClient;
        }
        // The SendEmailVerificationAsync method sends an email verification message to the specified recipient.
        public async Task<EmailResult> SendEmailVerificationAsync(string toEmail, string firstName, string confirmationLink)
        {
            var message = new EmailMessage
            {
                From = "onboarding@resend.dev", // Resend's test sender - swap for your verified domain later
                To = toEmail,
                Subject = "Confirm your TrendySize account",
                HtmlBody = $"<p>Hi {firstName},</p><p>Please confirm your account by clicking <a href=\"{confirmationLink}\">this link</a>.</p>"
            };

            var response = await _resendClient.EmailSendAsync(message);

            if (response.Success)
            {
                return new EmailResult { Succeeded = true };
            }

            return new EmailResult
            {
                Succeeded = false,
                ErrorMessage = "Failed to send confirmation email."
            };
                }

    }
}
