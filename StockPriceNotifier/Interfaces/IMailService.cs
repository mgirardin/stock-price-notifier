using System;

namespace StockPriceNotifier
{
    public interface IMailService
    {
        public void SendEmail(String emailBody, String subject);
    }
}