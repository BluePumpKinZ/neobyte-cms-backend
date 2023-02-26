using System.Net.Http.Headers;

namespace Neobyte.Cms.Backend.Tests;

public static class HttpClientExtensions {

	public static HttpClient Authorize (this HttpClient client, string token) {
		client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
		return client;
	}

}