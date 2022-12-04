using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace CommunityToolkit.Maui.Behaviors;
/// <summary>
/// 
/// </summary>
public class MatchBehavior : BaseBehavior<Label>
{ 
	
	/// <summary>
/// 
/// </summary>
	public static readonly BindableProperty CommandProperty
		= BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(MatchBehavior));
	/// <summary>
	/// 
	/// </summary>
	public static readonly BindableProperty MatchTypesProperty
		= BindableProperty.Create(nameof(MatchTypes), typeof(IEnumerable<MatchTypes.MatchType>), typeof(MatchBehavior), Enumerable.Empty<MatchTypes.MatchType>());
	  /// <summary>
	  /// 
	  /// </summary>
	public ICommand Command
	{
		get => (ICommand)GetValue(CommandProperty);
		set => SetValue(CommandProperty, value);
	}
	/// <summary>
	/// 
	/// </summary>
	public IEnumerable<MatchTypes.MatchType> MatchTypes
	{
		get => (IEnumerable<MatchTypes.MatchType>)GetValue(MatchTypesProperty);
		set => SetValue(MatchTypesProperty, value);
	}

	IDictionary<Span, string>? currentMatches;
	/// <summary>
	/// 
	/// </summary>
	/// <param name="bindable"></param>
	protected override void OnAttachedTo(Label bindable)
	{
		base.OnAttachedTo(bindable);
		DetectAndStyleMatches();
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected override void OnViewPropertyChanged(Label sender, PropertyChangedEventArgs e)
	{
		base.OnViewPropertyChanged(sender, e);
		if (e.PropertyName == Label.FormattedTextProperty.PropertyName)
		{
			DetectAndStyleMatches();
		}
	}
	
	/// <summary>
	/// 
	/// </summary>
	/// <param name="propertyName"></param>
	protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		base.OnPropertyChanged(propertyName);

		if (propertyName == CommandProperty.PropertyName && Command != null)
		{
			ConfigureGestures(View, AddGestureRecognizer);
		}
		else if (propertyName == MatchTypesProperty.PropertyName)
		{
			DetectAndStyleMatches();
		}
	}
	/// <summary>
	/// 
	/// </summary>
	/// <param name="bindable"></param>
	protected override void OnDetachingFrom(Label bindable)
	{
		currentMatches?.Clear();
		ConfigureGestures(bindable, RemoveGestureRecognizer);
		base.OnDetachingFrom(bindable);
	}

	IList<StringSection> ProcessString(string rawText)
	{
		var collection = MatchTypes.SelectMany(c => c.Regex.Value.Matches(rawText).OfType<Match>().Where(m => m.Success).Select(x => new { Match = x, Type = c })).OrderBy(x => x.Match.Index);

		var sections = new List<StringSection>();

		var lastIndex = 0;

		foreach (var item in collection)
		{
			sections.Add(new StringSection(rawText.Substring(lastIndex, item.Match.Index - lastIndex)));

			lastIndex = item.Match.Index + item.Match.Length;

			sections.Add(new StringSection(item.Type.GetText(item.Match.Value), item.Type.GetValue(item.Match.Value), item.Type.Style, true));
		}

		sections.Add(new StringSection(rawText.Substring(lastIndex)));

		return sections;
	}

	void DetectAndStyleMatches()
	{
		if (!string.IsNullOrWhiteSpace(View?.FormattedText?.ToString()))
		{
			var textValue = View.FormattedText?.ToString();
			View.FormattedText.Spans.Clear();
			currentMatches = new Dictionary<Span, string>();
			var formatted = View.FormattedText;

			foreach (var item in ProcessString(textValue))
			{
				formatted.Spans.Add(CreateSpan(item));
			}
		}
	}

	void ConfigureGestures(Label label, Action<KeyValuePair<Span, string>> configAction)
	{
		if (currentMatches?.Any() ?? false)
		{
			foreach (var section in currentMatches)
			{
				configAction(section);
			}
		}
	}

	Span CreateSpan(StringSection section)
	{
		var span = new Span()
		{
			Text = section.Text,
			Style = section.Style
		};

		if (section.IsMatch)
		{
			currentMatches?.Add(span, section.Value);
		}

		return span;
	}


	void AddGestureRecognizer(KeyValuePair<Span, string> match)
	{
		var gesture = match.Key.GestureRecognizers.FirstOrDefault();
		if (gesture is TapGestureRecognizer tapRecognizer)
		{
			tapRecognizer.Command = Command;
			tapRecognizer.CommandParameter = match.Value;
		}
		else
		{
			match.Key.GestureRecognizers.Add(new TapGestureRecognizer()
			{
				Command = Command,
				CommandParameter = match.Value
			});
		}
	}

	void RemoveGestureRecognizer(KeyValuePair<Span, string> match)
	{
		var gesture = match.Key.GestureRecognizers.FirstOrDefault();
		if (gesture is TapGestureRecognizer tapRecognizer)
		{
			match.Key.GestureRecognizers.Remove(tapRecognizer);
		}
	}

	class StringSection
	{
		public StringSection(string text, string? value = null, Style? style = null, bool isMatch = false)
		{
			Text = text;
			Value = value;
			Style = style;
			IsMatch = isMatch;
		}

		public bool IsMatch { get; }
		public Style? Style { get; }
		public string Text { get; }
		public string? Value { get; }
	}
}
