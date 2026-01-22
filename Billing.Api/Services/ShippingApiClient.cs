using System.Text;
using System.Text.Json;
using PSSC.Shared.Events;

namespace Billing.Api.Services;

public class ShippingApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ShippingApiClient> _logger;

    public ShippingApiClient(HttpClient httpClient, ILogger<ShippingApiClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<ShipmentCreatedEvent?> SendInvoiceGeneratedEventAsync(InvoiceGeneratedEvent invoiceEvent, CancellationToken ct)
    {
        try
        {
            var json = JsonSerializer.Serialize(invoiceEvent);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            _logger.LogInformation("Sending InvoiceGeneratedEvent to Shipping API for OrderId: {OrderId}", invoiceEvent.OrderId);

            var response = await _httpClient.PostAsync("/api/shipping/events/invoice-generated", content, ct);

            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync(ct);
                var shipmentEvent = JsonSerializer.Deserialize<ShipmentCreatedEvent>(responseJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                _logger.LogInformation("Received ShipmentCreatedEvent for OrderId: {OrderId}", invoiceEvent.OrderId);
                return shipmentEvent;
            }
            else
            {
                _logger.LogWarning("Shipping API returned {StatusCode} for OrderId: {OrderId}", response.StatusCode, invoiceEvent.OrderId);
                return null;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send InvoiceGeneratedEvent to Shipping API for OrderId: {OrderId}", invoiceEvent.OrderId);
            return null;
        }
    }
}
