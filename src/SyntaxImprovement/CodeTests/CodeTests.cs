using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Collections;

namespace oledid.SyntaxImprovement.CodeTests
{
	public interface IAssert
	{
		void False(bool actual, string error);
		void Empty(IEnumerable errorMatches);
	}

	public class CodeTests
	{
		private readonly IAssert Assert;
		private readonly DirectoryInfo SourcePath;

		public CodeTests(IAssert assert, DirectoryInfo sourcePath)
		{
			Assert = assert;
			SourcePath = sourcePath;
		}

		public void It_closes_db_connections()
		{
			// var connection
			{
				var regex = new Regex(@"^(\s+)?(?!using)(\s+)?(var connection = await)", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
				foreach (var csfile in SourcePath.EnumerateFiles("*.cs", SearchOption.AllDirectories))
				{
					var content = File.ReadAllText(csfile.FullName);
					Assert.False(regex.Matches(content).Any(), "Missing using-statement before var connection = await. Location: " + csfile);
				}
			}

			// SqlConnection connection
			{
				var regex = new Regex(@"^(\s+)?(?!using)(\s+)?(SqlConnection connection = await)", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
				foreach (var csfile in SourcePath.EnumerateFiles("*.cs", SearchOption.AllDirectories))
				{
					var content = File.ReadAllText(csfile.FullName);
					Assert.False(regex.Matches(content).Any(), "Missing using-statement before SqlConnection connection = await. Location: " + csfile);
				}
			}

			// inside method
			{
				var regex = new Regex(@"(\((\s+)?await OpenConnectionAsync)|(\((\s+)?await AppDatabaseRepository\.OpenConnectionAsync)", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
				foreach (var csfile in SourcePath.EnumerateFiles("*.cs", SearchOption.AllDirectories))
				{
					var content = File.ReadAllText(csfile.FullName);
					Assert.False(regex.Matches(content).Any(), "Missing using-statement when opening connection inside method. Location: " + csfile);
				}
			}
		}

		public void It_disposes_transactions()
		{
			var regex = new Regex(@"^(\s+)?(?!using)(\s+)?(var transaction = await)", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
			foreach (var csfile in SourcePath.EnumerateFiles("*.cs", SearchOption.AllDirectories))
			{
				var content = File.ReadAllText(csfile.FullName);
				Assert.False(regex.Matches(content).Any(), "Missing using-statement before var transaction = await. Location: " + csfile);
			}
		}

		public void It_awaits_transactions()
		{
			var regex = new Regex(@"^(\s+)?(?!await)(\s+)?(transaction\.)", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
			foreach (var csfile in SourcePath.EnumerateFiles("*.cs", SearchOption.AllDirectories))
			{
				var content = File.ReadAllText(csfile.FullName);
				Assert.False(regex.Matches(content).Any(), "Transactions must be async. Location: " + csfile);
			}
		}

		public void It_awaits_queries()
		{
			var regex = new Regex(@"connection.((Query(FirstOrDefault)?)|(Execute))((\()|(\<))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			foreach (var csfile in SourcePath.EnumerateFiles("*.cs", SearchOption.AllDirectories))
			{
				var content = File.ReadAllText(csfile.FullName);
				Assert.False(regex.Matches(content).Any(), "Non-async dapper-call. Location: " + csfile);
			}
		}

		public void External_links_target_blank()
		{
			var linkRegex = new Regex(@"<a [^>]*href=""http[^""]*""[^>]*>", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			var errorMatches = new List<(string filename, string match)>();
			foreach (var file in SourcePath.EnumerateFiles("*", SearchOption.AllDirectories)
				.Where(e => e.Extension.In(".html", ".vue", ".ts")))
			{
				if (file.FullName.Contains(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? @"\obj\" : "/obj/"))
				{
					continue;
				}

				var content = File.ReadAllText(file.FullName);
				foreach (var error in linkRegex.Matches(content).Where(match => match.Value.Contains("target=\"_blank\"") == false))
				{
					if (error.Value.Contains("data-ignore-target-blank-test"))
					{
						continue;
					}

					errorMatches.Add((file.FullName, error.Value));
				}
			}
			Assert.Empty(errorMatches);
		}
	}
}
