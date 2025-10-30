using System;
using System.Collections.Generic;
using System.Linq;
using AdminServerStub.Infrastructure;
using AdminServerStub.Models;
using Microsoft.AspNetCore.Mvc;

namespace AdminServerStub.Controllers
{
    [ApiController]
    [Route("api/Admin")]
    public class AdminController : ControllerBase
    {
        [HttpGet("agents")]
        public ActionResult<IEnumerable<AgentIdentity>> GetAllAgents([FromQuery] bool onlineOnly = false, [FromQuery] int minutes = 5)
        {
            var list = InMemoryStore.Agents.Values.ToList();
            if (onlineOnly)
            {
                var cutoff = DateTime.UtcNow.AddMinutes(-Math.Abs(minutes));
                list = list.Where(a => a.LastHeartbeat.HasValue && a.LastHeartbeat.Value >= cutoff).ToList();
            }
            return Ok(list);
        }

        [HttpGet("agents/{agentId}")]
        public ActionResult<AgentIdentity> GetAgent(string agentId)
        {
            if (InMemoryStore.Agents.TryGetValue(agentId, out var agent))
                return Ok(agent);
            return NotFound();
        }

        [HttpGet("agents/{agentId}/metrics")]
        public ActionResult<SystemMetrics> GetAgentMetrics(string agentId)
        {
            if (InMemoryStore.LatestMetrics.TryGetValue(agentId, out var metrics))
                return Ok(metrics);
            // Return empty array sometimes as AdminApiService handles both array/object
            return Ok(Array.Empty<SystemMetrics>());
        }

        [HttpGet("agents/{agentId}/metrics/aggregated")]
        public ActionResult<object> GetAgentMetricsAggregated(string agentId)
        {
            return Ok(new { agentId, avgCpu = 0.0, avgMemory = 0.0 });
        }

        [HttpGet("agents/{agentId}/metrics/average")]
        public ActionResult<object> GetAgentMetricsAverage(string agentId)
        {
            return Ok(new { agentId, avgCpu = 0.0, avgMemory = 0.0 });
        }

        [HttpGet("agents/{agentId}/metrics/trend")]
        public ActionResult<object> GetAgentMetricsTrend(string agentId)
        {
            return Ok(new { agentId, points = new object[0] });
        }
    }
}
