using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CommunityToolkit.Maui.Sample.ViewModels.Behaviors;

public partial class MatchBehaviorViewModel : BaseViewModel
{
	[ObservableProperty]
	string text = $"Hello, here is a list of hashtags, mentions, links: #DotNetMAUI #Community #OpenSource <a href=\"https://www.xamarin.com\">Xamarin</a>  <a href=\"www.example.com\">Test</a> <a href=\"www.github.com\">Github</a> @xamarinhq @dotnet @microsoft #MicrosoftToolkitXamarinForms #Xamarin #Monkeys #XamarinForms @planetxamarin #ILoveXamarin #CSharp #Behaviors";

	public ICommand TagTappedCommand => new Command<string>(async s => await Application.Current.MainPage.DisplayAlert("Tag Tapped:", s, "Ok"));
}