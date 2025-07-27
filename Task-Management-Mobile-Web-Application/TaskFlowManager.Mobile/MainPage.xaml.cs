using Microsoft.Maui.Controls;

namespace TaskFlowManager.Mobile;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        TaskWebView.Navigated += (s, e) => LoadingIndicator.IsVisible = false;
    }
}
