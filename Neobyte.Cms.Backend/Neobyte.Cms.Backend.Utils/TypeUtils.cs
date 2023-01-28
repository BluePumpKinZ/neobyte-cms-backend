using System;
using System.Collections.Generic;
using System.Reflection;

namespace Neobyte.Cms.Backend.Utils;

public class TypeUtils {

	public Type[] GetDerivedTypes<T> () => GetDerivedTypes(typeof(T));

	public Type[] GetDerivedTypes (Type type) {
		var output = new List<Type>();
		Assembly assembly = Assembly.GetExecutingAssembly();
		Type[] types = assembly.GetTypes();
		for (int i = 0; i < types.Length; i++)
			if (types[i].IsSubclassOf(type))
				output.Add(types[i]);

		return output.ToArray();
	}

}