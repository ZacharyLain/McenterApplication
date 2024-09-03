using System.Windows;
using System.Windows.Controls;

namespace McenterTest.UI.MVVM.View
{
    /// <summary>
    /// Interaction logic for MmrpMainView.xaml
    /// </summary>
    public partial class MmrpMainView : UserControl
    {
        public MmrpMainView()
        {
            InitializeComponent();
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
