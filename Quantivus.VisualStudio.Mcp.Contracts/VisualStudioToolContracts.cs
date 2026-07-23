using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Quantivus.VisualStudio.Mcp.Contracts
{
    public static class VisualStudioMcpProtocol
    {
        public const string Version = "2026-07-01";
        public const int MaximumMessageBytes = 4 * 1024 * 1024;
    }

    public sealed class VisualStudioToolRequest
    {
        [JsonProperty("id", Required = Required.Always)]
        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        [JsonProperty("tool", Required = Required.Always)]
        public string Tool { get; set; } = string.Empty;

        [JsonProperty("arguments")]
        public JObject Arguments { get; set; } = new JObject();

        [JsonProperty("protocolVersion")]
        public string ProtocolVersion { get; set; } = VisualStudioMcpProtocol.Version;
    }

    public sealed class VisualStudioToolResponse
    {
        [JsonProperty("id", Required = Required.Always)]
        public string Id { get; set; } = string.Empty;

        [JsonProperty("success", Required = Required.Always)]
        public bool Success { get; set; }

        [JsonProperty("result")]
        public JToken? Result { get; set; }

        [JsonProperty("error")]
        public VisualStudioToolError? Error { get; set; }

        [JsonProperty("protocolVersion")]
        public string ProtocolVersion { get; set; } = VisualStudioMcpProtocol.Version;

        public static VisualStudioToolResponse Ok(string id, object? result) => new VisualStudioToolResponse
        {
            Id = id,
            Success = true,
            Result = result == null ? JValue.CreateNull() : JToken.FromObject(result)
        };

        public static VisualStudioToolResponse Fail(string id, string code, string message) => new VisualStudioToolResponse
        {
            Id = id,
            Success = false,
            Error = new VisualStudioToolError { Code = code, Message = message }
        };
    }

    public sealed class VisualStudioToolError
    {
        [JsonProperty("code", Required = Required.Always)]
        public string Code { get; set; } = "unknown_error";

        [JsonProperty("message", Required = Required.Always)]
        public string Message { get; set; } = string.Empty;

        [JsonProperty("details")]
        public JObject? Details { get; set; }
    }

    public sealed class DebuggerSnapshot
    {
        [JsonProperty("mode")]
        public string Mode { get; set; } = "stopped";

        [JsonProperty("solution")]
        public string Solution { get; set; } = string.Empty;

        [JsonProperty("startupProjects")]
        public string StartupProjects { get; set; } = string.Empty;

        [JsonProperty("isSolutionOpen")]
        public bool IsSolutionOpen { get; set; }
    }
}
