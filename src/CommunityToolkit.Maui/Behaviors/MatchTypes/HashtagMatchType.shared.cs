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
public class HashtagMatchType : MatchType
{
#if !NETSTANDARD1_0
	/// <summary>
	/// 
	/// </summary>
	public override Lazy<Regex> Regex { get; } = new Lazy<Regex>(() => new Regex(@"#\w+", RegexOptions.Compiled | RegexOptions.Singleline));
#else
        public override Lazy<Regex> Regex { get; } = new Lazy<Regex>(() => new Regex(@"#\w+", RegexOptions.Singleline));
#endif
	/// <summary>
	/// 
	/// </summary>
	public override Style Style => new Style(typeof(Span))
	{
		Class = "HashtagSpanStyle",
		Setters =
				{
					new Setter
					{
						Property = Span.TextColorProperty,
						Value = Colors.Green
					}
				}
	};
}