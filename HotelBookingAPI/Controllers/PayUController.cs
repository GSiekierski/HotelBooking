using Microsoft.AspNetCore.Mvc;

public class PaymentController : ControllerBase
{
    
    private readonly PayUService _payUService;

    public PaymentController(PayUService payUService)
    {
        _payUService = payUService;
    }

    [HttpGet("get-token")]
    public async Task<IActionResult> GetToken()
    {
        string clientId = "300746";
        string clientSecret = "2ee86a66e5d97e3fadc400c9f19b065d";

        var token = await _payUService.GetAccessTokenAsync(clientId, clientSecret);

        return Ok(token);
    }

    [HttpGet("create-order")]
    public async Task<IActionResult> CreateOrder()
    {
        var accessToken = await _payUService.GetAccessTokenAsync("300746", "2ee86a66e5d97e3fadc400c9f19b065d");
        var paymentLink = await _payUService.CreateOrderAsync(accessToken);

        return Ok(new { paymentLink });
    }
    
}
