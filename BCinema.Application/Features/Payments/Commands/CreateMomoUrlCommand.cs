using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using BCinema.Application.Exceptions;
using BCinema.Application.Momo;
using BCinema.Domain.Entities;
using BCinema.Domain.Interfaces.IRepositories;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;

namespace BCinema.Application.Features.Payments.Commands;

public class PaymentInfoCommand : IRequest<string>
{
    [Required]
    public Guid PaymentId { get; set; }
    
    public class PaymentInfoCommandHandler(IOptions<MomoOptionModel> options, IPaymentRepository paymentRepository) 
        : IRequestHandler<PaymentInfoCommand, string>
    {
        public async Task<string> Handle(PaymentInfoCommand request, CancellationToken cancellationToken)
        {
            var payment = await paymentRepository.GetPaymentByIdAsync(request.PaymentId, cancellationToken)
                          ?? throw new NotFoundException(nameof(Payment));
            
            var paymentId = payment.Id.ToString();
            var paymentAmount = payment.TotalPrice.ToString(CultureInfo.CurrentCulture);
            var paymentInfo = "Thanh toán vé xem phim";
            var rawData =
                $"partnerCode={options.Value.PartnerCode}" +
                $"&accessKey={options.Value.AccessKey}" +
                $"&requestId={paymentId}" +
                $"&amount={paymentAmount}" +
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
                amount = paymentAmount,
                orderInfo = paymentInfo,
                requestId = paymentId,
                extraData = "",
                signature = signature
            };

            req.AddParameter("application/json", JsonConvert.SerializeObject(requestData), ParameterType.RequestBody);

            var response = await client.ExecuteAsync(req, cancellationToken);
            var resp = JsonConvert.DeserializeObject<MomoCreatePayment>(response.Content!)!;
            return resp.PayUrl;
        }
        
        private static string ComputeHmacSha256(string message, string secretKey)
        {
            var keyBytes = Encoding.UTF8.GetBytes(secretKey);
            var messageBytes = Encoding.UTF8.GetBytes(message);

            byte[] hashBytes;
            using (var hmac = new HMACSHA256(keyBytes))
            {
                hashBytes = hmac.ComputeHash(messageBytes);
            }
            var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            return hashString;
        }
    }
}