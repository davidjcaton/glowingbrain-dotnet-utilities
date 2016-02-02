namespace System
{
	public static class StringExtensions
	{
		public static string SubstringBefore (this string value, char ch)
		{
			var i = value.IndexOf (ch);
			if (i < 0) {
				return string.Empty;
			}

			return value.Substring (0, i);
		}

		public static string SubstringAfter (this string value, char ch)
		{
			var i = value.IndexOf (ch);
			if (i < 0) {
				return string.Empty;
			}

			var result = value.Substring (i + 1);
			return result;
		}

		public static Tuple<string, string> Divide (this string value, char ch)
		{
			string lhs = value;
			string rhs = String.Empty;

			var i = value.IndexOf (ch);
			if (i >= 0) {
				lhs = value.Substring (0, i);
				rhs = value.Substring (i + 1);
			}

			return Tuple.Create (lhs, rhs);
		}
	}
}
