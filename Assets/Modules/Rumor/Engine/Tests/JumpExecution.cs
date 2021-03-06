using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Exodrifter.Rumor.Engine.Tests
{
	public static class JumpExecution
	{
		[Test]
		public static void JumpSuccess()
		{
			var rumor = new Rumor(
				new Dictionary<string, List<Node>>
				{
					{ Rumor.MainIdentifier, new List<Node>()
						{ new JumpNode("foobar")
						, new WaitNode()
						}
					},
					{ "foobar", new List<Node>()
						{ new WaitNode()
						}
					}
				}
			);

			rumor.Start();
			Assert.IsTrue(rumor.Executing);

			rumor.Advance();
			Assert.IsFalse(rumor.Executing);
			Assert.AreEqual(1, rumor.FinishCount);
			Assert.AreEqual(0, rumor.CancelCount);
		}

		[Test]
		public static void JumpFailure()
		{
			var rumor = new Rumor(
				new Dictionary<string, List<Node>>
				{
					{ Rumor.MainIdentifier, new List<Node>()
						{ new JumpNode("foobar")
						}
					}
				}
			);

			var exception = Assert.Throws<InvalidOperationException>(() =>
				rumor.Start()
			);
			Assert.AreEqual(
				"The label \"foobar\" does not exist!",
				exception.Message
			);
		}
	}
}
