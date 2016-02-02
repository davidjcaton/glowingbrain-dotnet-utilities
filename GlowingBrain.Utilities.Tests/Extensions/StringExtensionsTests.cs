using NUnit.Framework;
using System;

namespace GlowingBrain.Utilities.Tests.Extensions
{
	[TestFixture]
	public class StringExtensionsTests
	{
		[Test]
		public void Verify_SubstringBeforeReturnsSubstring_WhenSearchCharaterIsPresent()
		{
			Assert.AreEqual("foo", "foo.bar".SubstringBefore('.'));
		}

		[Test]
		public void Verify_SubstringAfterReturnsSubstring_WhenSearchCharaterIsPresent()
		{
			Assert.AreEqual("bar", "foo.bar".SubstringAfter('.'));
		}
	}
}

