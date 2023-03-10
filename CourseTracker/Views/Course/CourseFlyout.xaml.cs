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

namespace CourseTracker.Views.Course
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CourseFlyout : ContentPage
    {
        public ListView ListView;

        public CourseFlyout()
        {
            InitializeComponent();

            BindingContext = new CourseFlyoutViewModel();
            ListView = MenuItemsListView;
        }

        class CourseFlyoutViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<CourseFlyoutMenuItem> MenuItems { get; set; }

            public CourseFlyoutViewModel()
            {
                MenuItems = new ObservableCollection<CourseFlyoutMenuItem>(new[]
                {
                    new CourseFlyoutMenuItem { Id = 0, Title = "Page 1" },
                    new CourseFlyoutMenuItem { Id = 1, Title = "Page 2" },
                    new CourseFlyoutMenuItem { Id = 2, Title = "Page 3" },
                    new CourseFlyoutMenuItem { Id = 3, Title = "Page 4" },
                    new CourseFlyoutMenuItem { Id = 4, Title = "Page 5" },
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