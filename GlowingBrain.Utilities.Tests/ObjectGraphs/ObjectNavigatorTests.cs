using System;
using NUnit.Framework;
using System.Collections.Generic;

namespace GlowingBrain.Utilities.ObjectGraphs
{
	[TestFixture]
	public class ObjectNavigatorTests
	{
		[Test]
		public void Verify_GetReturnsSimpleValue_WhenPathIsSimplePropertyName()
		{
			var building = new Building
			{
				Price = 123.45
			};

			var nav = new ObjectNavigator();
			var result = nav.GetValue(building, "Price");
			Assert.AreEqual(123.45, result);
		}

		[Test]
		public void Verify_GetReturnsSimpleValue_WhenPathIsCompoundPropertyNames()
		{
			var building = new Building
			{
				Address = new Address
				{
					Street = "123 Oxford Road"
				}
			};

			var nav = new ObjectNavigator();
			var result = nav.GetValue(building, "Address.Street");
			Assert.AreEqual("123 Oxford Road", result);
		}

		[Test]
		public void Verify_GetReturnsSimpleValue_WhenPathIsSimpleDictionaryStringKey()
		{
			var dict = new Dictionary<string, int>();
			dict["The Answer"] = 42;

			var nav = new ObjectNavigator();
			var result = nav.GetValue(dict, "The Answer");
			Assert.AreEqual(42, result);
		}

		[Test]
		public void Verify_GetReturnsSimpleValue_WhenPathIsSimpleListIndex()
		{
			var list = new List<string> { "One", "Two", "Three" };

			var nav = new ObjectNavigator();
			var result = nav.GetValue(list, "[1]");
			Assert.AreEqual("Two", result);
		}

		[Test]
		public void Verify_GetReturnsSimpleValue_WhenPathIsCompoundListIndex()
		{
			var building = new Building
			{
				Rooms = new Room[]
				{
					new Room { Name = "Room Zero" },
					new Room { Name = "Room One" }
				}
			};

			var nav = new ObjectNavigator();
			var result = nav.GetValue(building, "Rooms[1].Name");
			Assert.AreEqual("Room One", result);
		}

		[Test]
		public void Verify_SetChangesValue_WhenPathIsSimplePropertyName()
		{
			var building = new Building();
			var nav = new ObjectNavigator();
			nav.SetValue(building, "Price", 123.45);
			Assert.AreEqual(123.45, building.Price);
		}

		[Test]
		public void Verify_SetChangesValue_WhenPathIsSimpleListIndex()
		{
			var list = new List<string> { "One", "Two", "Three" };

			var nav = new ObjectNavigator();
			nav.SetValue(list, "[2]", "THIRD");
			Assert.AreEqual("THIRD", list[2]);
		}

		[Test]
		public void Verify_SetChangesValue_WhenPathIsSimpleDictionaryStringKey()
		{
			var dict = new Dictionary<string, int>();
			dict["The Answer"] = 42;

			var nav = new ObjectNavigator();
			nav.SetValue(dict, "The Answer", 24);
			Assert.AreEqual(24, dict["The Answer"]);
		}

		[Test]
		public void Verify_SetChangesValue_WhenPathIsIndexedDictionaryStringKey()
		{
			var dict = new Dictionary<string, int[]>();
			dict["The Answer"] = new int[] { 1, 2, 3, 4 };

			var nav = new ObjectNavigator();
			nav.SetValue(dict, "The Answer[1]", 10002);
			Assert.AreEqual(10002, dict["The Answer"][1]);
		}

		[Test]
		public void Verify_SetChangesValue_WhenPathIsCompoundListIndex()
		{
			var building = new Building
			{
				Rooms = new Room[]
				{
					new Room { Name = "Room Zero" },
					new Room { Name = "Room One" }
				}
			};

			var nav = new ObjectNavigator();
			nav.SetValue(building, "Rooms[0].Name", "First Room");
			Assert.AreEqual("First Room", building.Rooms[0].Name);
		}

		class Building
		{
			public Building()
			{
				this.Address = new Address();
			}

			public double Price { get; set; }

			public Address Address { get; set; }

			public Room[] Rooms { get; set; }
		}

		class Address
		{
			public string Street { get; set; }
			public string City { get; set; }
		}

		class Room
		{
			public string Name { get; set; }
		}
	}
}

