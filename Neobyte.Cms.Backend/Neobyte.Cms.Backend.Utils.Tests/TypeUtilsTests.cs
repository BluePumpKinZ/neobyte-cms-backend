using System;

namespace Neobyte.Cms.Backend.Utils.Tests;

public class TypeUtilsTests {

	public class Fruit { }
	public class Apple : Fruit { }
	public class Orange : Fruit { }
	public class Banana : Fruit { }

	public abstract class Vehicle { }
	public class Car : Vehicle { }
	public class Truck : Vehicle { }
	public class Motorcycle : Vehicle { }

	private readonly TypeUtils _typeUtils;

	public TypeUtilsTests () {
		_typeUtils = new TypeUtils();
	}

	[Fact]
	public void GetDerivedTypes_ShouldReturnEmptyArrayForTypeWithNoDerivedTypes () {
		var derivedTypes = _typeUtils.GetDerivedTypes(typeof(string));

		Assert.Empty(derivedTypes);
	}

	[Fact]
	public void GetDerivedTypes_ShouldReturnArrayContainingAllDerivedTypes () {
		var derivedTypes = _typeUtils.GetDerivedTypes(typeof(Fruit));

		Assert.Equal(new Type[] { typeof(Apple), typeof(Orange), typeof(Banana) }, derivedTypes);
	}

	[Fact]
	public void GetDerivedTypes_ShouldHandleInterfacesCorrectly () {
		var derivedTypes = _typeUtils.GetDerivedTypes(typeof(IComparable));

		Assert.Contains(typeof(string), derivedTypes);
		Assert.Contains(typeof(int), derivedTypes);
		Assert.Contains(typeof(decimal), derivedTypes);
	}

	[Fact]
	public void GetDerivedTypes_ShouldHandleAbstractBaseClassesCorrectly () {
		var derivedTypes = _typeUtils.GetDerivedTypes(typeof(Vehicle));

		Assert.Equal(new Type[] { typeof(Car), typeof(Truck), typeof(Motorcycle) }, derivedTypes);
	}


}