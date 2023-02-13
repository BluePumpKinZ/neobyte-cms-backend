﻿namespace Neobyte.Cms.Backend.Monitoring.Configuration; 

public class MonitoringOptions {
	public const string SectionName = "Monitoring";
	public string ServiceName { get; set; } = string.Empty;
	public string JaegerHost { get; set; } = string.Empty;
	public int JaegerPort { get; set; } = 0;
}