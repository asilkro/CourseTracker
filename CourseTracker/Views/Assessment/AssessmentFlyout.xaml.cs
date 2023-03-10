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

namespace CourseTracker.Views.Assessment
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AssessmentFlyout : ContentPage
    {
        public ListView ListView;

        public AssessmentFlyout()
        {
            InitializeComponent();

            BindingContext = new AssessmentFlyoutViewModel();
            ListView = MenuItemsListView;
        }

        class AssessmentFlyoutViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<AssessmentFlyoutMenuItem> MenuItems { get; set; }

            public AssessmentFlyoutViewModel()
            {
                MenuItems = new ObservableCollection<AssessmentFlyoutMenuItem>(new[]
                {
                    new AssessmentFlyoutMenuItem { Id = 0, Title = "Page 1" },
                    new AssessmentFlyoutMenuItem { Id = 1, Title = "Page 2" },
                    new AssessmentFlyoutMenuItem { Id = 2, Title = "Page 3" },
                    new AssessmentFlyoutMenuItem { Id = 3, Title = "Page 4" },
                    new AssessmentFlyoutMenuItem { Id = 4, Title = "Page 5" },
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