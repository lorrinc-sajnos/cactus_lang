using System.Formats.Asn1;
using CactusLang.Model.Symbols;
using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CactusLang.Tests.Model.Symbols;

[TestClass]
[TestSubject(typeof(FileID))]
public class FileIDTest {

	[TestMethod]
	public void TestEquality() {
		FileID a = new( ["foo"], "bar.test");
		FileID b = new( ["foo"], "bar.test");
		
		Assert.IsTrue(a.Equals(b));
		Assert.IsTrue(b.Equals(a));
		
		
		FileID a2 = new( ["foo", "damn"], "bar.test");
		FileID b2 = new( ["foo", "damn"], "bar.test");
		
		Assert.IsTrue(a2.Equals(b2));
		Assert.IsTrue(b2.Equals(a2));
		
		Assert.IsFalse(a.Equals(b2));
		Assert.IsFalse(b.Equals(a2));

	}
}