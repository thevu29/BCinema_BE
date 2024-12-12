using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;

namespace BCinema.Application.Momo;

public class MomoService(IOptions<MomoOptionModel> options) : IMomoService
{
    public async Task<string> CreateMomoPaymentUrl(string paymentId, string paymentInfo, string amount)
    {
        Console.WriteLine($"Request Details:");
        Console.WriteLine($"PaymentId: {paymentId}");
        Console.WriteLine($"PaymentInfo: {paymentInfo}");
        Console.WriteLine($"Amount: {amount}");
        
        var encodedPaymentInfo = Uri.EscapeDataString(paymentInfo);
        
        var rawData =
            $"partnerCode={options.Value.PartnerCode}" +
            $"&accessKey={options.Value.AccessKey}" +
            $"&requestId={paymentId}" +
            $"&amount={amount}" +
            $"&orderId={paymentId}" +
            $"&orderInfo={encodedPaymentInfo}" +
            $"&returnUrl={options.Value.ReturnUrl}" +
            $"&notifyUrl={options.Value.NotifyUrl}" +
            $"&extraData=";

        var signature = ComputeHmacSha256(rawData, options.Value.SecretKey);

        var client = new RestClient(options.Value.MomoApiUrl);
        var req = new RestRequest() { Method = Method.Post };
        req.AddHeader("Content-Type", "application/json; charset=UTF-8");

        var requestData = new
        {
            accessKey = options.Value.AccessKey,
            partnerCode = options.Value.PartnerCode,
            requestType = options.Value.RequestType,
            notifyUrl = options.Value.NotifyUrl,
            returnUrl = options.Value.ReturnUrl,
            orderId = paymentId,
            amount = amount,
            orderInfo = paymentInfo,
            requestId = paymentId,
            signature = signature,
            extraData = ""
        };
        
        req.AddParameter("application/json", JsonConvert.SerializeObject(requestData), ParameterType.RequestBody);

        var response = await client.ExecuteAsync(req);
        Console.WriteLine($"Response Status: {response.StatusCode}");
        Console.WriteLine($"Response Content: {response.Content}");
        var resp = JsonConvert.DeserializeObject<MomoCreatePayment>(response.Content!)!;
        
        return resp.PayUrl;
    }
    
    private static string ComputeHmacSha256(string rawData, string secretKey)
    {
        Console.WriteLine($"Raw Data: {rawData}");
        Console.WriteLine($"Secret Key: {secretKey}");
        
        var keyBytes = Encoding.UTF8.GetBytes(secretKey);
        var messageBytes = Encoding.UTF8.GetBytes(rawData);

        byte[] hashBytes;
        using (var hmac = new HMACSHA256(keyBytes))
        {
            hashBytes = hmac.ComputeHash(messageBytes);
        }
        
        var hashString = Convert.ToBase64String(hashBytes);
    
        Console.WriteLine($"Computed Signature: {hashString}");
    
        return hashString;
    }
}