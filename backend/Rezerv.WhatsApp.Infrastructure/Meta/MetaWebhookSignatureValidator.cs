using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;

namespace Rezerv.WhatsApp.Infrastructure.Meta;

public class MetaWebhookSignatureValidator
{
    private readonly MetaWebhookSettings _settings;

    public MetaWebhookSignatureValidator(IOptions<MetaWebhookSettings> settings)
    {
        _settings = settings.Value;
    }

    public bool IsValid(string? signatureHeader, string rawBody)
    {
        if (string.IsNullOrWhiteSpace(signatureHeader))
            return false;

        if (!signatureHeader.StartsWith("sha256=", StringComparison.OrdinalIgnoreCase))
            return false;

        var receivedSignature = signatureHeader["sha256=".Length..];

        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_settings.AppSecret));
        var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(rawBody));
        var expectedSignature = Convert.ToHexString(hashBytes).ToLowerInvariant();

        return CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(receivedSignature),
            Encoding.UTF8.GetBytes(expectedSignature));
    }
}