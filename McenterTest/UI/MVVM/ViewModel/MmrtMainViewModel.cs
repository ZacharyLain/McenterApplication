using McenterTest.UI.Core;
using McenterTest.UI.ModelViewViewModel.ViewModel.MmrtSubViewModel;
using System.Collections.ObjectModel;

namespace McenterTest.UI.ModelViewViewModel.ViewModel
{
    public class MmrtMainViewModel : ObservableObject
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

        public MmrtMainViewModel()
        {
            ViewModels = new ObservableCollection<object>
            {
                new GetToolInstancesViewModel(),
                new CreateToolInstancesViewModel(),
            };

            GetToolInstanceCommand = new RelayCommand(o => SelectedViewModel = new GetToolInstancesViewModel());

            // Set the default view model
            SelectedViewModel = ViewModels[0];
        }
    }
}