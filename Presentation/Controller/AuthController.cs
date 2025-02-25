using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using Presentation.Cache;
using Presentation.DTO;
using Presentation.Interfaces;

namespace Presentation.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        ////Only works with Email without Mfa
        //[HttpPost("request-login")]
        //public async Task<IActionResult> RequestLogin([FromBody] LoginDto request)
        //{
        //    if (string.IsNullOrEmpty(request.Email))
        //    {
        //        return BadRequest(new { message = "Email is required." });
        //    }

        //    var token = await _authService.RequestLoginAsync(request.Email);
        //    if (!string.IsNullOrEmpty(token))
        //        return Ok(new { token });
        //    return BadRequest("Failed to send login link.");
        //}

        //Works with Mfa
        [HttpPost("request-login")]
        public async Task<IActionResult> RequestLogin([FromBody] LoginDto loginRequest)
        {
            // Generate a secure 6-digit MFA code
            var code = _authService.GenerateRandomCode();

            // Store the code for the given email (overwriting any previous code)
            MfaStore.Codes[loginRequest.Email] = code;

            // Create the email message using MimeKit
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("SecurityApplication", "your_email@gmail.com"));
            message.To.Add(MailboxAddress.Parse(loginRequest.Email));
            message.Subject = "Your MFA Code";
            message.Body = new TextPart("plain")
            {
                Text = $"Your MFA code is: {code}"
            };

            try
            {
                using (var client = new SmtpClient())
                {
                    // For local testing, you can bypass certificate validation (but tighten this in production)
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    // Connect to Gmail's SMTP server using STARTTLS
                    await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

                    // Authenticate using your Gmail address and your app-specific password
                    await client.AuthenticateAsync("your-email@gmail.com", "your_password");

                    // Send the email
                    await client.SendAsync(message);

                    // Disconnect cleanly
                    await client.DisconnectAsync(true);
                }

                return Ok(new { message = "MFA code sent to Gmail", code });
            }
            catch (System.Exception ex)
            {
                // Log the exception as needed
                return StatusCode(500, new { error = "Error sending email", detail = ex.Message });
            }
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyMfa([FromBody] VerifyMfaDto verifyDto)
        {
            // Try to retrieve the stored MFA code
            if (!MfaStore.Codes.TryGetValue(verifyDto.Email, out var storedCode))
            {
                return Unauthorized(new { error = "MFA code not found or expired" });
            }
            if (storedCode != verifyDto.Code)
            {
                return Unauthorized(new { error = "Invalid MFA code" });
            }

            // Optionally, remove the code once verified
            MfaStore.Codes.TryRemove(verifyDto.Email, out _);

            // Call your login service to generate the JWT token.
            // RequestLoginAsync searches for the user, validates login, and returns a token.
            var token = await _authService.RequestLoginAsync(verifyDto.Email);

            if (!string.IsNullOrEmpty(token))
                return Ok(new { token });
            return Unauthorized(new { error = "User not found or login failed" });
        }

    }
}
