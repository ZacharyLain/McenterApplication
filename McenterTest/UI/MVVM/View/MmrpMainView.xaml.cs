using System.Windows;
using System.Windows.Controls;
using McenterTest.UI.MVVM.View.MmrpSubView;
using McenterTest.UI.MVVM.ViewModel.MmrpSubViewModel;

namespace McenterTest.UI.MVVM.View
{
    public partial class MmrpMainView : UserControl
    {
        public MmrpMainView()
        {
            InitializeComponent();
            SelectedViewModel = new GetNCPackagesViewModel();
        }

        public static readonly DependencyProperty SelectedViewModelProperty =
            DependencyProperty.Register("SelectedViewModel", typeof(object), typeof(MmrpMainView), new PropertyMetadata(null));

        public object SelectedViewModel
        {
            get { return GetValue(SelectedViewModelProperty); }
            set { SetValue(SelectedViewModelProperty, value); }
        }
    }
}
