using System;
using System.Collections.Generic;
using System.Linq;

namespace Xamarin.Forms
{
	public static class MarkdownParser
	{
		public static Color HyperlinkTextColor { get; set; } = Color.Accent;

		// To modify markdown flags, modify the following patterns
		private const string urlStartPattern = "[";
		private const string urlIntermediatePattern = "](";
		private const string urlStopPattern = ")";
		private const string boldPattern = "**";
		private const string italicStarPattern = "*";
		private const string italicUnderPattern = "_";
		private const string boldAndItalicPattern = "***";
		private const string underlinePattern = "__";

		// Markup used to split text into list of strings => can be changed if needed
		private const string markdown = "MARKDOWN:";
		private const string markdownStart = "[";
		private const string markdownStop = "]";
		private const string markdownCloseStart = "[/";
		private const string markdownCloseStop = markdownStop;

		// Flags to construct markups (ex : [MARKDOWN:bold] is [MARKDOWN: + boldFlag + ])
		private const string basicFlag = "text";
		private const string linkFlag = "link";
		private const string urlFlag = "url";
		private const string boldFlag = "bold";
		private const string italicFlag = "italic";
		private const string boldAnditalicFlag = "bolditalic";
		private const string underlineFlag = "underline";

		private static readonly Dictionary<string, string> patternToFlagDictionary = new Dictionary<string, string>
		{
			{ underlinePattern, underlineFlag },
			{ boldAndItalicPattern, boldAnditalicFlag },
			{ boldPattern, boldFlag },
			{ italicUnderPattern, italicFlag },
			{ italicStarPattern, italicFlag }
		};

		/*
		 * XAMARIN.FORMS - SPECIFIC METHOD
		 * Parse a pre-formatted text (after parsing Markdown to Markup) to a Xamarin.Forms.FormattedString
		 */
		public static FormattedString Parse(string text)
		{
			FormattedString formattedString = new FormattedString();

			List<KeyValuePair<string, string>> spanList = MarkupToDictionnary(ParseMarkdownToMarkup(text));

			foreach (KeyValuePair<string, string> span in spanList)
			{
				switch (span.Key)
				{
					case (linkFlag):
						formattedString.Spans.Add(new Span
						{
							Text = span.Value,
							TextDecorations = TextDecorations.Underline,
							TextColor = HyperlinkTextColor
						});
						break;

					case (urlFlag):
						formattedString.Spans.Last<Span>().GestureRecognizers.Add(new TapGestureRecognizer
						{
							Command = new Command(() => Device.OpenUri(new Uri(span.Value)))
						});
						break;

					case (boldFlag):
						formattedString.Spans.Add(new Span
						{
							Text = span.Value,
							FontAttributes = FontAttributes.Bold
						});
						break;

					case (italicFlag):
						formattedString.Spans.Add(new Span
						{
							Text = span.Value,
							FontAttributes = FontAttributes.Italic
						});
						break;

					case (boldAnditalicFlag):
						formattedString.Spans.Add(new Span
						{
							Text = span.Value,
							FontAttributes = FontAttributes.Bold | FontAttributes.Italic
						});
						break;

					case (underlineFlag):
						formattedString.Spans.Add(new Span
						{
							Text = span.Value,
							TextDecorations = TextDecorations.Underline
						});
						break;

					default:
					case (basicFlag):
						formattedString.Spans.Add(new Span { Text = span.Value });
						break;
				}
			}

			return formattedString;
		}


		/*
		 * 
		 * NO-SPECIFIC METHODS : USABLE ON EACH UI SYSTEM
		 * 
		 */

		// Split a string in a list of Flag => Substring, based on markup
		public static List<KeyValuePair<string, string>> MarkupToDictionnary(string text)
		{
			List<KeyValuePair<string, string>> textStrings = new List<KeyValuePair<string, string>>();

			/*
			 * While text is not empty : loop on it to find the next markup
			 * If no markup found : add a new Basic string to the list
			 * If markup found : verify if the markup is complete and valid => if not, add a new Basic string from 0 index to the unvalid markup
			 * If the markup is complete and valid : add a new special string identified by the Flag Key
			 * At each step : text is a substring from last verified index to end of text
			 */
			while (!String.IsNullOrEmpty(text))
			{
				int markdownStartIndex = text.IndexOf(markdownStart + markdown);

				if (markdownStartIndex >= 0)
				{
					int markdownStopIndex = text.Substring(markdownStartIndex + markdownStart.Length + markdown.Length).IndexOf(markdownStop);

					if (markdownStopIndex > 0)
					{
						string markdownFlag = text.Substring(markdownStartIndex + markdownStart.Length + markdown.Length, markdownStopIndex);
						int markdownCloseIndex = text.Substring(markdownStartIndex + markdownStart.Length + markdown.Length + markdownStopIndex + markdownStop.Length).IndexOf(markdownCloseStart + markdown + markdownFlag + markdownCloseStop);

						if (markdownFlag.Length > 0 && markdownCloseIndex > 0)
						{
							if (markdownStartIndex > 0) textStrings.Add(new KeyValuePair<string, string>(basicFlag, text.Substring(0, markdownStartIndex)));
							textStrings.Add(new KeyValuePair<string, string>(markdownFlag, text.Substring(markdownStartIndex + markdownStart.Length + markdown.Length + markdownStopIndex + markdownStop.Length, markdownCloseIndex)));
							text = text.Substring(markdownStartIndex + markdownStart.Length + markdown.Length + markdownStopIndex + markdownStop.Length + markdownCloseIndex + markdownCloseStart.Length + markdown.Length + markdownFlag.Length + markdownCloseStop.Length);
						}
						else
						{
							textStrings.Add(new KeyValuePair<string, string>(basicFlag, text.Substring(0, markdownStartIndex + markdownStart.Length + markdown.Length + markdownStopIndex + markdownStop.Length)));
							text = text.Substring(markdownStartIndex + markdownStart.Length + markdown.Length + markdownStopIndex + markdownStop.Length);
						}
					}
					else
					{
						textStrings.Add(new KeyValuePair<string, string>(basicFlag, text.Substring(0, markdownStartIndex + markdownStart.Length + markdown.Length)));
						text = text.Substring(markdownStartIndex + markdownStart.Length + markdown.Length);
					}
				}
				else
				{
					textStrings.Add(new KeyValuePair<string, string>(basicFlag, text));
					text = null;
				}
			}
			return textStrings;
		}

		// Parse a text from Markdown to markup ; order set to avoid wrong parsing (ex : *** must not be parsed as ** or *)
		public static string ParseMarkdownToMarkup(string text)
		{
			return
				TextDecorationParser(italicUnderPattern,
					TextDecorationParser(italicStarPattern,
						TextDecorationParser(boldPattern,
							TextDecorationParser(boldAndItalicPattern,
								TextDecorationParser(underlinePattern,
									HyperlinkParser(text))))));
		}

		// Search and add markups for hypertext links
		public static string HyperlinkParser(string text)
		{
			int urlStartIndex = text.IndexOf(urlStartPattern);
			int urlIntermediateIndex = text.IndexOf(urlIntermediatePattern);
			if (urlStartIndex < 0 || urlIntermediateIndex < urlStartIndex) return text;

			int urlStopIndex = text.Substring(urlIntermediateIndex + 2).IndexOf(urlStopPattern);
			if (urlStopIndex <= 0) return text;

			return
				text.Substring(0, urlStartIndex) // Before link
				+ markdownStart + markdown + linkFlag + markdownStop + text.Substring(urlStartIndex + urlStartPattern.Length, urlIntermediateIndex - urlStartIndex - urlStartPattern.Length) + markdownCloseStart + markdown + linkFlag + markdownCloseStop // Link text
				+ markdownStart + markdown + urlFlag + markdownStop + text.Substring(urlIntermediateIndex + urlIntermediatePattern.Length, urlStopIndex) + markdownCloseStart + markdown + urlFlag + markdownCloseStop // Link URL
				+ HyperlinkParser(text.Substring(urlIntermediateIndex + urlIntermediatePattern.Length + urlStopIndex + urlStopPattern.Length)); // After link
		}

		// Parse text to find valid markdown, and replace it with markups
		// Note : patternToFlagDictionary contains associations between markdown patterns, and markup flags
		public static string TextDecorationParser(string markdownPattern, string text)
		{
			int startIndex = text.IndexOf(markdownPattern);
			if (startIndex < 0) return text;

			int stopIndex = text.Substring(startIndex + markdownPattern.Length).IndexOf(markdownPattern);
			if (stopIndex <= 0) return text;

			// Parsing stops if substring already contains a valid markup => avoid wrong double parsing
			if (text.Substring(startIndex + markdownPattern.Length, stopIndex).Contains(markdownStart + markdown)) return text;

			return
				text.Substring(0, startIndex)
				+ markdownStart + markdown + patternToFlagDictionary[markdownPattern] + markdownStop
				+ text.Substring(startIndex + markdownPattern.Length, stopIndex)
				+ markdownCloseStart + markdown + patternToFlagDictionary[markdownPattern] + markdownCloseStop
				+ TextDecorationParser(markdownPattern, text.Substring(startIndex + markdownPattern.Length + stopIndex + markdownPattern.Length));
		}
	}
}
