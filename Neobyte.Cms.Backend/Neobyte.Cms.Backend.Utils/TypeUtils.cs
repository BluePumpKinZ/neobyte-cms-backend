using System;
using System.Collections.Generic;
using System.Linq;

namespace Neobyte.Cms.Backend.Utils;

public class TypeUtils {

	public Type[] GetDerivedTypes<T> () => GetDerivedTypes(typeof(T));

	public Type[] GetDerivedTypes (Type type) {
		var output = new List<Type>();
		Type[] types = AppDomain.CurrentDomain.GetAssemblies()
			.SelectMany(a => a.GetTypes())
			.ToArray();
		foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
			Console.WriteLine(a.FullName);
		for (int i = 0; i < types.Length; i++)
			if (types[i].IsSubclassOf(type))
				output.Add(types[i]);
		

		return output.ToArray();
	}

}