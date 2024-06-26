﻿using System.Net.Mail;
using System.Net;
using ReadEase_C_.Interface;

namespace ReadEase_C_.Helpers
{
    public class Mail : IMail
    {
        private readonly IConfiguration _configuration;

        public Mail(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public void SendEmail(string recipientEmail)
        {
            try
            {
                string senderEmail = _configuration["Mail:e-mail"];
                string senderPassword = _configuration["Mail:pass"];

                MailMessage mail = new(senderEmail, recipientEmail);
                SmtpClient client = new()
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(senderEmail, senderPassword)
                };


                mail.Subject = "Registration Successful";
                mail.Body = "Congratulations! You have successfully registered.";

                client.Send(mail);
            }
            catch (Exception ex)
            {
                throw new Exception("Bad Email" + ex.Message);

            }
        }

        public void RecoverEmail(string recipientEmail, string pass)
        {
            try
            {
                string senderEmail = _configuration["Mail:e-mail"];
                string senderPassword = _configuration["Mail:pass"];

                MailMessage mail = new(senderEmail, recipientEmail);
                SmtpClient client = new()
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(senderEmail, senderPassword)
                };


                mail.Subject = "Recovered Password Successfuly";
                mail.Body = "Your new password is " + pass;

                client.Send(mail);
            }
            catch (Exception ex)
            {
                throw new Exception("Bad Email" + ex.Message);

            }
        }


    }
}
