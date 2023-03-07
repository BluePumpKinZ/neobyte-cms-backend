using System;

namespace Neobyte.Cms.Backend.Utils; 

public static class StringExtensions {
	
	public static string Shuffle(this string str) {
		if (str.Length <= 1) {
			return str;
		}
        
		char[] arr = str.ToCharArray();
		Random rand = new Random();
        
		for (int i = arr.Length - 1; i > 0; i--) {
			int j = rand.Next(i + 1);
			(arr[i], arr[j]) = (arr[j], arr[i]);
		}
        
		return new string(arr);
	}
}