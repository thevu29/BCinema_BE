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
        var rawData =
            $"partnerCode={options.Value.PartnerCode}" +
            $"&accessKey={options.Value.AccessKey}" +
            $"&requestId={paymentId}" +
            $"&amount={amount}" +
            $"&orderExpireTime=5" +
            $"&orderId={paymentId}" +
            $"&orderInfo={paymentInfo}" +
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
            orderExpireTime = 5,
            orderInfo = paymentInfo,
            requestId = paymentId,
            signature = signature
        };
        

        req.AddParameter("application/json", JsonConvert.SerializeObject(requestData), ParameterType.RequestBody);

        var response = await client.ExecuteAsync(req);
        var resp = JsonConvert.DeserializeObject<MomoCreatePayment>(response.Content!)!;
        return resp.PayUrl;
    }
    
    private string ComputeHmacSha256(string rawData, string secretKey)
    {
        var keyBytes = Encoding.UTF8.GetBytes(secretKey);
        var messageBytes = Encoding.UTF8.GetBytes(rawData);

        byte[] hashBytes;
        using (var hmac = new HMACSHA256(keyBytes))
        {
            hashBytes = hmac.ComputeHash(messageBytes);
        }
        var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        return hashString;
    }
}