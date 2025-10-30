using System;
using System.Collections.Generic;
using System.Linq;
using AdminServerStub.Infrastructure;
using AdminServerStub.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AdminServerStub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommandController : ControllerBase
    {
        [HttpPost]
        public ActionResult<CommandRequest> Execute([FromBody] CommandRequest command)
        {
            if (command == null)
                return BadRequest();

            if (string.IsNullOrWhiteSpace(command.CommandId))
                command.CommandId = Guid.NewGuid().ToString();

            if (string.IsNullOrWhiteSpace(command.TargetAgentId))
                return BadRequest("targetAgentId required");

            var queue = InMemoryStore.PendingCommands.GetOrAdd(command.TargetAgentId, _ => new());
            queue.Enqueue(command);

            return Ok(command);
        }

        [HttpGet("pending/{agentId}")]
        public ActionResult<IEnumerable<CommandRequest>> GetPending(string agentId)
        {
            if (InMemoryStore.PendingCommands.TryGetValue(agentId, out var queue))
            {
                var list = new List<CommandRequest>();
                while (queue.TryDequeue(out var cmd))
                {
                    list.Add(cmd);
                }
                return Ok(list);
            }
            return Ok(Array.Empty<CommandRequest>());
        }

        [HttpGet("{commandId}")]
        public ActionResult<CommandResponse> GetById(string commandId)
        {
            if (InMemoryStore.CommandResults.TryGetValue(commandId, out var result))
                return Ok(result);
            return NotFound();
        }

        // Unified POST endpoint: if body contains commandId -> save result; otherwise return all results
        [HttpPost("result")]
        public ActionResult PostResultOrList([FromBody] JsonElement body)
        {
            if (body.ValueKind == JsonValueKind.Object && body.TryGetProperty("commandId", out var cmdIdProp) && cmdIdProp.ValueKind == JsonValueKind.String)
            {
                var response = body.Deserialize<CommandResponse>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (response == null || string.IsNullOrWhiteSpace(response.CommandId))
                    return BadRequest();
                InMemoryStore.CommandResults[response.CommandId] = response;
                return Ok(new { success = true });
            }

            // No commandId in body: return all results
            return Ok(InMemoryStore.CommandResults.Values.ToList());
        }
    }
}
