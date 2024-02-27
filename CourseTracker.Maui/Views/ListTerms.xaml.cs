using Android.Runtime;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.ViewModels;

namespace CourseTracker.Maui.Views;

[Preserve(AllMembers = true)]
public partial class ListTerms : ContentPage
{
    readonly ListTermsVM _listTermsVM;
    public ListTerms()
    {
        InitializeComponent();
        BindingContext = _listTermsVM = new ListTermsVM();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _listTermsVM.OnAppearing();
    }

    private void TermListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item is Term selectedTerm)
        {
            _listTermsVM.ShowActionSheet(selectedTerm);
        }
        ((ListView)sender).SelectedItem = null;
    }
}