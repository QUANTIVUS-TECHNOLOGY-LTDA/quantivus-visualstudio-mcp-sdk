using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Quantivus.VisualStudio.Mcp.Contracts.Protocol
{
    public sealed class NamedPipeEnvelope
    {
        [JsonProperty("messageId", Required = Required.Always)]
        public string MessageId { get; set; } = Guid.NewGuid().ToString("N");

        [JsonProperty("kind", Required = Required.Always)]
        public string Kind { get; set; } = "tool_request";

        [JsonProperty("payload", Required = Required.Always)]
        public JToken Payload { get; set; } = JValue.CreateNull();

        [JsonProperty("sentAtUtc", Required = Required.Always)]
        public DateTimeOffset SentAtUtc { get; set; } = DateTimeOffset.UtcNow;

        [JsonProperty("protocolVersion", Required = Required.Always)]
        public string ProtocolVersion { get; set; } = VisualStudioMcpProtocol.Version;

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(MessageId))
                throw new InvalidOperationException("messageId is required.");
            if (string.IsNullOrWhiteSpace(Kind))
                throw new InvalidOperationException("kind is required.");
            if (!string.Equals(ProtocolVersion, VisualStudioMcpProtocol.Version, StringComparison.Ordinal))
                throw new NotSupportedException($"Unsupported protocol version '{ProtocolVersion}'.");
        }
    }
}
