using System;
using System.Collections.Generic;
using AdminServerStub.Infrastructure;
using AdminServerStub.Models;
using Microsoft.AspNetCore.Mvc;

namespace AdminServerStub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NetworkPortController : ControllerBase
    {
        [HttpPost]
        public ActionResult Submit([FromBody] NetworkPortData data)
        {
            if (data == null || string.IsNullOrWhiteSpace(data.AgentId))
                return BadRequest("agentId required");

            InMemoryStore.NetworkPorts[data.AgentId] = data.Connections ?? new List<object>();
            InMemoryStore.GetOrAddAgent(data.AgentId).LastHeartbeat = DateTime.UtcNow;
            return Ok(new { success = true });
        }

        [HttpGet("{agentId}")]
        public ActionResult<IEnumerable<object>> Get(string agentId)
        {
            if (InMemoryStore.NetworkPorts.TryGetValue(agentId, out var list))
                return Ok(list);
            return Ok(Array.Empty<object>());
        }

        [HttpGet("{agentId}/latest")]
        public ActionResult<IEnumerable<object>> GetLatest(string agentId)
        {
            if (InMemoryStore.NetworkPorts.TryGetValue(agentId, out var list))
                return Ok(list);
            return Ok(Array.Empty<object>());
        }
    }
}
