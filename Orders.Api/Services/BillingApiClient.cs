using System.Text;
using System.Text.Json;
using PSSC.Shared.Events;

namespace Orders.Api.Services;

public class BillingApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<BillingApiClient> _logger;

    public BillingApiClient(HttpClient httpClient, ILogger<BillingApiClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<InvoiceGeneratedEvent?> SendOrderPlacedEventAsync(OrderPlacedEvent orderEvent, CancellationToken ct)
    {
        try
        {
            var json = JsonSerializer.Serialize(orderEvent);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            _logger.LogInformation("Sending OrderPlacedEvent to Billing API for OrderId: {OrderId}", orderEvent.OrderId);

            var response = await _httpClient.PostAsync("/api/billing/events/order-placed", content, ct);

            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync(ct);
                var invoiceEvent = JsonSerializer.Deserialize<InvoiceGeneratedEvent>(responseJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                _logger.LogInformation("Received InvoiceGeneratedEvent for OrderId: {OrderId}", orderEvent.OrderId);
                return invoiceEvent;
            }
            else
            {
                _logger.LogWarning("Billing API returned {StatusCode} for OrderId: {OrderId}", response.StatusCode, orderEvent.OrderId);
                return null;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send OrderPlacedEvent to Billing API for OrderId: {OrderId}", orderEvent.OrderId);
            return null;
        }
    }
}
