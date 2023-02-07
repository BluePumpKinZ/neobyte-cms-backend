namespace Neobyte.Cms.Backend.Identity.Configuration; 

public class JwtOptions {

	public const string Section = "Identity:Jwt";
	public string Issuer { get; set; } = string.Empty;
	public string Audience { get; set; } = string.Empty;
	public string Secret { get; set; } = string.Empty;
	public long ExpirationShort { get; set; } = 86_400_000; // 1 day
	public long ExpirationLong { get; set; } = 2_628_288_000; // 1 month
}