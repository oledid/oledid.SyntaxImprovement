using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text.RegularExpressions;

namespace oledid.SyntaxImprovement
{
	public static class StringExtensions
	{
		/// <summary>
		/// Converts the first char in the string to uppercase and returns the whole string.
		/// </summary>
		public static string Capitalize(this string str)
		{
			if (string.IsNullOrEmpty(str)) return str;
			return char.ToUpper(str.First()) + str.Substring(1);
		}

		/// <summary>
		/// Converts the first character to lowercase.
		/// </summary>
		public static string ToLowerCamelCase(this string input)
		{
			return input.HasValue() == false
				? input
				: string.Join(string.Empty, input.Substring(0, 1).ToLower(), input.Substring(1));
		}

		/// <param name="isWhitespaceValue">True if a string containing only whitespace counts as having value, false if not.</param>
		public static bool HasValue(this string str, bool isWhitespaceValue = true)
		{
			return isWhitespaceValue
				? string.IsNullOrEmpty(str) == false
				: string.IsNullOrWhiteSpace(str) == false;
		}

		public static string RemoveFromEnd(this string str, string stringToRemove)
		{
			return str.EndsWith(stringToRemove)
				? str.RemoveFromEnd(stringToRemove.Length)
				: str;
		}

		public static string RemoveFromEnd(this string str, int numberOfChars)
		{
			if (numberOfChars > str.Length)
			{
				return string.Empty;
			}
			return str.Substring(0, Math.Max(str.Length - numberOfChars, 0));
		}

		public static string RemoveFromEnd(this string str, char character)
		{
			return str.RemoveFromEnd(character.ToString());
		}

		public static string RemoveFromStart(this string str, string stringToRemove)
		{
			if (str == null)
				return null;
			return str.StartsWith(stringToRemove)
				? str.Substring(stringToRemove.Length)
				: str;
		}

		public static string RemoveFromStart(this string str, char charToRemove)
		{
			return str.RemoveFromStart(charToRemove.ToString());
		}

		public static string RemoveFromStart(this string str, int numberOfChars)
		{
			if (numberOfChars > str.Length)
			{
				return string.Empty;
			}
			return str.Substring(numberOfChars);
		}

		/// <summary>
		/// Returns value of the string until (and excluding) the first occurrence of parameter 'stringToFind'.
		/// </summary>
		public static string UntilFirst(this string input, string stringToFind)
		{
			if (input == null)
				return null;
			var index = input.IndexOf(stringToFind ?? string.Empty, StringComparison.Ordinal);
			return index == -1
				? input
				: input.Substring(0, index);
		}

		/// <summary>
		/// Returns value of the string until (and excluding) the last occurrence of parameter 'stringToFind'.
		/// </summary>
		public static string UntilLast(this string input, string stringToFind)
		{
			if (input == null)
				return null;
			var index = input.LastIndexOf(stringToFind ?? string.Empty, StringComparison.Ordinal);
			return index == -1
				? input
				: input.Substring(0, index);
		}

		[Pure]
		public static string AfterFirst(this string str, char matchChar, bool includeMatchCharInResult = false)
		{
			return str.AfterFirst(matchChar.ToString(), includeMatchCharInResult);
		}

		[Pure]
		public static string AfterFirst(this string str, string matchString, bool includeMatchStringInResult = false)
		{
			if (str == null)
				return string.Empty;

			var index = str.IndexOf(matchString);
			if (index == -1)
				return string.Empty;

			var substring = str.Substring(index + matchString.Length);
			return includeMatchStringInResult
				? (matchString + substring)
				: substring;
		}

		[Pure]
		public static string AfterLast(this string str, char matchChar, bool includeMatchCharInResult = false)
		{
			return str.AfterLast(matchChar.ToString(), includeMatchCharInResult);
		}

		[Pure]
		public static string AfterLast(this string str, string matchString, bool includeMatchStringInResult = false)
		{
			if (str == null)
				return string.Empty;

			var index = str.LastIndexOf(matchString);
			if (index == -1)
				return string.Empty;

			var substring = str.Substring(index + matchString.Length);
			return includeMatchStringInResult
				? (matchString + substring)
				: substring;
		}

		/// <summary>
		/// returns Regex.Replace(input, pattern, replacement, options);
		/// </summary>
		/// <param name="input">The string to search for a match</param>
		/// <param name="pattern">The regular expression pattern to match</param>
		/// <param name="replacement">The replacement string</param>
		/// <param name="options">Regex option flags</param>
		public static string RegexReplace(this string input, string pattern, string replacement, RegexOptions options)
		{
			return Regex.Replace(input, pattern, replacement, options);
		}
	}
}
