namespace eCommerceOrders.SendEmail.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body, bool isBodyHtml = false);
    }
}