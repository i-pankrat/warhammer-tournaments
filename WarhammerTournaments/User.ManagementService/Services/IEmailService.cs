using User.ManagementService.Models;

namespace User.ManagementService.Services;

public interface IEmailService
{
    void SentEmail(Message message);
}