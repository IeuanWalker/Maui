using CommunityToolkit.Maui.Sample.ViewModels.Behaviors;

namespace CommunityToolkit.Maui.Sample.Pages.Behaviors;

public partial class MatchBehaviorPage : BasePage<MatchBehaviorViewModel>
{
	public MatchBehaviorPage(MatchBehaviorViewModel matchedBehaviorViewModel) : base(matchedBehaviorViewModel)
	{
		InitializeComponent();
	}

	private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
	{

	}
}