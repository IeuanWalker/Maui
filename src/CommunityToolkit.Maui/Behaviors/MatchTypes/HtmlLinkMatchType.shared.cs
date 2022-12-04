using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CommunityToolkit.Maui.Behaviors.MatchTypes;
/// <summary>
/// 
/// </summary>
public class HtmlLinkMatchType : MatchType
{
#if !NETSTANDARD1_0
	/// <summary>
	/// 
	/// </summary>
	Lazy<Regex> extractValueRegex = new Lazy<Regex>(() => new Regex("(?<=href=\\\")[\\S]+(?=\\\")", RegexOptions.Compiled | RegexOptions.Singleline));
	/// <summary>
	/// 
	/// </summary>
	Lazy<Regex> extractTextRegex = new Lazy<Regex>(() => new Regex("<.*?>", RegexOptions.Compiled | RegexOptions.Singleline));
	/// <summary>
	/// 
	/// </summary>
	public override Lazy<Regex> Regex { get; } = new Lazy<Regex>(() => new Regex(@"(<a.*?>.*?</a>)", RegexOptions.Compiled | RegexOptions.Singleline));
#else
        Lazy<Regex> extractValueRegex = new Lazy<Regex>(() => new Regex("(?<=href=\\\")[\\S]+(?=\\\")", RegexOptions.Singleline));

        Lazy<Regex> extractTextRegex = new Lazy<Regex>(() => new Regex("<.*?>", RegexOptions.Singleline));

        public override Lazy<Regex> Regex { get; } = new Lazy<Regex>(() => new Regex(@"(<a.*?>.*?</a>)", RegexOptions.Singleline));
rdelrosario marked this conversation as resolved.
#endif
	/// <summary>
	/// 
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public override string GetValue(string value) => extractValueRegex.Value.Match(value).Value;
	/// <summary>
	/// 
	/// </summary>
	/// <param name="text"></param>
	/// <returns></returns>
	public override string GetText(string text) => extractTextRegex.Value.Replace(text, string.Empty);
	/// <summary>
	/// 
	/// </summary>
	public override Style Style => new Style(typeof(Span))
	{
		Class = "HtmlLinkSpanStyle",
		Setters =
				{
					new Setter
					{
						Property = Span.TextColorProperty,
						Value = Colors.Blue
					}
				}
	};
}
