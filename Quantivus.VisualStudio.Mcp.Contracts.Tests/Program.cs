using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quantivus.VisualStudio.Mcp.Contracts;
using Quantivus.VisualStudio.Mcp.Contracts.Protocol;

static void Assert(bool condition, string message)
{
    if (!condition) throw new InvalidOperationException(message);
}

var request = new VisualStudioToolRequest
{
    Id = "contract-test",
    Tool = "vs_breakpoint_add",
    Arguments = new JObject
    {
        ["file"] = "Program.cs",
        ["line"] = 42
    }
};

var json = JsonConvert.SerializeObject(request);
var roundTrip = JsonConvert.DeserializeObject<VisualStudioToolRequest>(json)
    ?? throw new InvalidOperationException("Request could not be deserialized.");

Assert(roundTrip.Id == request.Id, "Request ID was not preserved.");
Assert(roundTrip.Tool == request.Tool, "Tool name was not preserved.");
Assert(roundTrip.Arguments["line"]?.Value<int>() == 42, "Arguments were not preserved.");
Assert(roundTrip.ProtocolVersion == VisualStudioMcpProtocol.Version, "Protocol version mismatch.");

var envelope = new NamedPipeEnvelope
{
    MessageId = "envelope-test",
    Payload = JObject.FromObject(request)
};
envelope.Validate();

var response = VisualStudioToolResponse.Ok(request.Id, new { added = true });
Assert(response.Success, "Successful response is not marked successful.");
Assert(response.Result?["added"]?.Value<bool>() == true, "Response payload was not created.");

var failed = VisualStudioToolResponse.Fail(request.Id, "invalid_argument", "line is required");
Assert(!failed.Success, "Failed response is marked successful.");
Assert(failed.Error?.Code == "invalid_argument", "Error code was not preserved.");

Console.WriteLine("Quantivus Visual Studio MCP contract checks passed.");
