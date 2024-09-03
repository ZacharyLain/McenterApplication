using System.Collections.ObjectModel;
using McenterTest.UI.Core;
using McenterTest.UI.MVVM.View.MmrpSubView;

namespace McenterTest.UI.MVVM.ViewModel
{
    public class MmrpMainViewModel : ObservableObject
    {
        private object _selectedViewModel;
        public ObservableCollection<object> ViewModels { get; set; }
        public RelayCommand GetToolInstanceCommand { get; set; }

        public object SelectedViewModel
        {
            get => _selectedViewModel;
            set
            {
                _selectedViewModel = value;
                OnPropertyChanged();
            }
        }

        public MmrpMainViewModel()
        {
            ViewModels = new ObservableCollection<object>
            {
                new GetNCPackagesView(),
                // new CreateNCPackagesView(),
                // new DeleteNCPackagesView(),
            };

            GetToolInstanceCommand = new RelayCommand(o => SelectedViewModel = new GetNCPackagesView());

            // Set the default view model
            SelectedViewModel = ViewModels[0];
        }
    }
}