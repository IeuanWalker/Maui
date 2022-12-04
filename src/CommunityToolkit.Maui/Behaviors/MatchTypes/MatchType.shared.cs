using System.Text.RegularExpressions;

namespace CommunityToolkit.Maui.Behaviors.MatchTypes;
/// <summary>
/// 
/// </summary>
public abstract class MatchType
{
	/// <summary>
	/// 
	/// </summary>
	public abstract Lazy<Regex> Regex { get; }
	/// <summary>
	/// 
	/// </summary>
	public abstract Style Style { get; }
	/// <summary>
	/// 
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public virtual string GetValue(string value) => value;
	/// <summary>
	/// 
	/// </summary>
	/// <param name="text"></param>
	/// <returns></returns>
	public virtual string GetText(string text) => text;
}
