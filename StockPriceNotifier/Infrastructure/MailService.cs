using System;
using System.Collections.Generic;
using MimeKit;
using Microsoft.Extensions.Configuration;

namespace StockPriceNotifier
{
    public class MailService : IMailService {
        private readonly IConfiguration _config;

        public MailService(IConfiguration config){
            _config = config;
        }

        public void SendEmail(String emailBody, String subject){
            String fromEmailAdress       = _config.GetValue<string>("FromEmailAddress");
            String FromEmailName         = _config.GetValue<string>("FromEmailName");
            String FromEmailPassword     = _config.GetValue<string>("FromEmailPassword");
            String EmailHost             = _config.GetValue<string>("EmailHost"); 
            int EmailHostPort            = _config.GetValue<int>("EmailHostPort");
            List<String> toEmailAdresses = _config.GetSection("EmailAddresses").Get<List<string>>();
            
            var message = new MimeMessage{
                Subject = subject
            };
                message.From.Add(new MailboxAddress(FromEmailName, fromEmailAdress));
                foreach(String email in toEmailAdresses){
                    message.To.Add(new MailboxAddress("", email));
                }
                message.Body = new TextPart("plain")
                {
                    Text = emailBody
                };
            try{
                using (var client = new MailKit.Net.Smtp.SmtpClient()){
                    client.Connect(EmailHost, EmailHostPort, false);
                    client.Authenticate(fromEmailAdress, FromEmailPassword);
                    client.Send(message);
                    client.Disconnect(true);
                };
            }
            catch(Exception e){
                Console.WriteLine(e);
            }
            return;
        }
    }
}