namespace Neobyte.Cms.Backend.Monitoring.Configuration; 

public class MonitoringFrontendTracingOptions {

	public string Cluster { get; set; } = string.Empty;
	public string Host { get; set; } = string.Empty;
	public int Port { get; set; } = 0;

}