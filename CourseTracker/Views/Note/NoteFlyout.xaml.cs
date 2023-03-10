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

namespace CourseTracker.Views.Note
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NoteFlyout : ContentPage
    {
        public ListView ListView;

        public NoteFlyout()
        {
            InitializeComponent();

            BindingContext = new NoteFlyoutViewModel();
            ListView = MenuItemsListView;
        }

        class NoteFlyoutViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<NoteFlyoutMenuItem> MenuItems { get; set; }

            public NoteFlyoutViewModel()
            {
                MenuItems = new ObservableCollection<NoteFlyoutMenuItem>(new[]
                {
                    new NoteFlyoutMenuItem { Id = 0, Title = "Page 1" },
                    new NoteFlyoutMenuItem { Id = 1, Title = "Page 2" },
                    new NoteFlyoutMenuItem { Id = 2, Title = "Page 3" },
                    new NoteFlyoutMenuItem { Id = 3, Title = "Page 4" },
                    new NoteFlyoutMenuItem { Id = 4, Title = "Page 5" },
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