using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Rezerv.WhatsApp.Application.DTO.Webhook
{
    public class MetaWebhookPayload
    {
        [JsonPropertyName("object")]
        public string? Object { get; init; }

        [JsonPropertyName("entry")]
        public List<MetaWebhookEntry> Entry { get; init; } = [];
    }

    public class MetaWebhookEntry
    {
        [JsonPropertyName("changes")]
        public List<MetaWebhookChange> Changes { get; init; } = [];
    }

    public class MetaWebhookChange
    {
        [JsonPropertyName("value")]
        public MetaWebhookValue? Value { get; init; }
    }

    public class MetaWebhookValue
    {
        [JsonPropertyName("statuses")]
        public List<MetaWebhookStatus> Statuses { get; init; } = [];
    }

    public class MetaWebhookStatus
    {
        [JsonPropertyName("id")]
        public string Id { get; init; } = default!;

        [JsonPropertyName("status")]
        public string Status { get; init; } = default!;

        [JsonPropertyName("timestamp")]
        public string Timestamp { get; init; } = default!;

        [JsonPropertyName("recipient_id")]
        public string? RecipientId { get; init; }
    }
}