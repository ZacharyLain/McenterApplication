using McenterTest.UI.Core;
using McenterTest.UI.MVVM.ViewModel;

namespace McenterTest.UI.MVVM.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        private object _currentView;
        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand MmrtViewCommand { get; set; }

        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            HomeViewCommand = new RelayCommand(o => CurrentView = new HomeViewModel());
            MmrtViewCommand = new RelayCommand(o => CurrentView = new MmrtMainViewModel());

            // Set the default view
            CurrentView = new HomeViewModel();
        }
    }
}