using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CourseTracker.Views.Term
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TermFlyout : ContentPage
    {
        public ListView ListView;

        public TermFlyout()
        {
            InitializeComponent();

            BindingContext = new TermFlyoutViewModel();
            ListView = MenuItemsListView;
        }

        class TermFlyoutViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<TermFlyoutMenuItem> MenuItems { get; set; }

            public TermFlyoutViewModel()
            {
                MenuItems = new ObservableCollection<TermFlyoutMenuItem>(new[]
                {
                    new TermFlyoutMenuItem { Id = 0, Title = "Page 1" },
                    new TermFlyoutMenuItem { Id = 1, Title = "Page 2" },
                    new TermFlyoutMenuItem { Id = 2, Title = "Page 3" },
                    new TermFlyoutMenuItem { Id = 3, Title = "Page 4" },
                    new TermFlyoutMenuItem { Id = 4, Title = "Page 5" },
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}