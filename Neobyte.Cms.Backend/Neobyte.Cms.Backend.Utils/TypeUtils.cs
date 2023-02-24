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
		for (int i = 0; i < types.Length; i++) {
			var t = types[i];
			if (type.IsInterface) {
				if (t.GetInterfaces().Any(i => i == type))
					output.Add(t);
			} else {
				if (t.IsSubclassOf(type))
					output.Add(t);
			}
		}

		return output.ToArray();
	}

}