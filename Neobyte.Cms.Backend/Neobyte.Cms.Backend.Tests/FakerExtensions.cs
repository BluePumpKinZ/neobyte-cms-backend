using Bogus.DataSets;

namespace Neobyte.Cms.Backend.Tests; 

public static class FakerExtensions {

	public static string PasswordCustom (this Internet internet) {

		var r = internet.Random;

		var letters = r.Replace("??");
		var lowerLetters = r.Replace("?????").ToLower();
		var numbers = r.Replace("##");
		var symbols = r.Replace("!!");
		var all = letters + lowerLetters + numbers + symbols;
		return new string (r.Shuffle(all).ToArray());

	}

}