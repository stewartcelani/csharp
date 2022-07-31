using System.Threading.Tasks;

namespace CityInfo.API.Services;

public interface IMailService
{
    Task Send(string subject, string message);
}