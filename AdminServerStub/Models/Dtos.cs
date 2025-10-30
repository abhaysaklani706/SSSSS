using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AdminServerStub.Models
{
    public class AgentIdentity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string MachineName { get; set; } = Environment.MachineName;
        public string IpAddress { get; set; } = "127.0.0.1";
        public string MacAddress { get; set; } = "00:00:00:00:00:00";
        public string OperatingSystem { get; set; } = Environment.OSVersion.ToString();
        public DateTime? LastHeartbeat { get; set; } = DateTime.UtcNow;
        public string? Location { get; set; }
    }

    public class ProcessInfo
    {
        public required string Name { get; set; }
        public int Id { get; set; }
        public double CpuUsage { get; set; }
        public double MemoryUsage { get; set; }
    }

    public class SystemMetrics
    {
        [JsonPropertyName("agentId")] public required string AgentId { get; set; }
        [JsonPropertyName("timestamp")] public DateTime Timestamp { get; set; }
        [JsonPropertyName("cpuUsage")] public double CpuUsage { get; set; }
        [JsonPropertyName("memoryUsage")] public double MemoryUsage { get; set; }
        [JsonPropertyName("diskUsage")] public double DiskUsage { get; set; }
        [JsonPropertyName("networkSent")] public long NetworkSent { get; set; }
        [JsonPropertyName("networkReceived")] public long NetworkReceived { get; set; }
        [JsonPropertyName("topProcesses")] public List<ProcessInfo> TopProcesses { get; set; } = new();
    }

    public class CommandRequest
    {
        public required string CommandId { get; set; }
        public required string TargetAgentId { get; set; }
        public int CommandType { get; set; }
        public Dictionary<string, string> Parameters { get; set; } = new();
        public DateTime Timestamp { get; set; }
        public int Priority { get; set; }
        public int TimeoutSeconds { get; set; }
        public bool RequireConfirmation { get; set; }
    }

    public class CommandResponse
    {
        public required string CommandId { get; set; }
        public required string AgentId { get; set; }
        public string Status { get; set; } = "Completed";
        public DateTime StartTime { get; set; } = DateTime.UtcNow;
        public DateTime EndTime { get; set; } = DateTime.UtcNow;
        public long ExecutionTimeMs { get; set; }
        public required string Output { get; set; }
        public required string ErrorOutput { get; set; }
        public int ExitCode { get; set; } = 0;
    }

    public class InstalledSoftwareData
    {
        public required string AgentId { get; set; }
        // Timestamp when the data was collected (provided by agent)
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public List<object> SoftwareList { get; set; } = new();
    }

    /// <summary>
    /// DTO returned by the AdminServerStub for latest installed software.
    /// Matches what the Admin client expects: agentId, timestamp, softwareList.
    /// </summary>
    public class InstalledSoftwareInfoDto
    {
        [JsonPropertyName("agentId")] public required string AgentId { get; set; }
        [JsonPropertyName("timestamp")] public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        [JsonPropertyName("softwareList")] public List<object> SoftwareList { get; set; } = new();
    }

    public class NetworkPortData
    {
        public required string AgentId { get; set; }
        public List<object> Connections { get; set; } = new();
    }
}
