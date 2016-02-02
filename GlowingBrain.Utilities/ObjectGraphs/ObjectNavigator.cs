using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Text.RegularExpressions;

namespace GlowingBrain.Utilities.ObjectGraphs
{
	public class ObjectNavigator
	{
		static readonly Regex _elementRegex = new Regex (@"^(?<property>[A-Za-z\s0-9_]+)?(\[(?<index>\d+)\])?$");

		public static ObjectNavigator Default = new ObjectNavigator ();

		public object GetValue (object instance, string propertyPath)
		{
			var components = propertyPath.Divide ('.');
			var result = GetComponentValue (instance, components.Item1);
			if (!string.IsNullOrEmpty (components.Item2)) {
				result = GetValue (result, components.Item2);
			}

			return result;
		}

		public void SetValue (object instance, string propertyPath, object value)
		{
			var components = propertyPath.Divide ('.');
			if (!string.IsNullOrEmpty (components.Item2)) {
				var childInstance = GetValue (instance, components.Item1);
				SetValue (childInstance, components.Item2, value);
			} else {
				SetComponentValue (instance, components.Item1, value);
			}
		}

		object GetComponentValue (object instance, string component)
		{
			object result;

			var match = _elementRegex.Match (component);

			var indexGroup = match.Groups ["index"];
			var propertyGroup = match.Groups ["property"];

			if (propertyGroup.Success) {
				var pi = instance.GetType ().GetRuntimeProperty (propertyGroup.Value);
				if (pi != null) {
					result = pi.GetValue (instance);
				} else {
					var dict = (IDictionary)instance;
					result = dict [propertyGroup.Value];
				}

				if (indexGroup.Success) {
					result = ((IList)result) [int.Parse (indexGroup.Value)];
				}
			} else {
				// must be indexer
				var pi = instance.GetType ().GetRuntimeProperties ().First (x => x.GetIndexParameters ().Length == 1);
				result = pi.GetValue (instance, new object[] { int.Parse (indexGroup.Value) });

			}

			return result;
		}

		void SetComponentValue (object instance, string component, object value)
		{
			var match = _elementRegex.Match (component);

			var indexGroup = match.Groups ["index"];
			var propertyGroup = match.Groups ["property"];
            
			if (propertyGroup.Success) {
				var pi = instance.GetType ().GetRuntimeProperty (propertyGroup.Value);
				if (pi != null) {
					if (indexGroup.Success) {
						((IList)pi.GetValue (instance)) [int.Parse (indexGroup.Value)] = value;
					} else {
						pi.SetValue (instance, value);
					}
				} else {
					var dict = (IDictionary)instance;
					if (indexGroup.Success) {
						((IList)dict [propertyGroup.Value]) [int.Parse (indexGroup.Value)] = value;
					} else {
						dict [propertyGroup.Value] = value;
					}
				}
			} else {
				// must be indexer
				var pi = instance.GetType ().GetRuntimeProperties ().First (x => x.GetIndexParameters ().Length == 1);
				pi.SetValue (instance, value, new object[] { int.Parse (indexGroup.Value) });
			}     
		}
	}
}
