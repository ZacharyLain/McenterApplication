using System.Collections.ObjectModel;
using McenterTest.UI.Core;
using McenterTest.UI.MVVM.ViewModel.MmrpSubViewModel;

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
                new GetNCPackagesViewModel(),
                // new CreateNCPackagesView(),
                // new DeleteNCPackagesView(),
            };

            GetToolInstanceCommand = new RelayCommand(o => SelectedViewModel = new GetNCPackagesViewModel());

            // Set the default view model
            SelectedViewModel = ViewModels[0];
        }
    }
}