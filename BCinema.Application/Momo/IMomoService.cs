namespace BCinema.Application.Momo;

public interface IMomoService
{
    Task<string> CreateMomoPaymentUrl(string orderId, string orderInfo, string amount);
}