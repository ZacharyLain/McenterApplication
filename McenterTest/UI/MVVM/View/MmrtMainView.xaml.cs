using System.Windows;
using System.Windows.Controls;
using McenterTest.UI.MVVM.View.MmrtSubView;

namespace McenterTest.UI.MVVM.View
{
    public partial class MmrtMainView : UserControl
    {
        public MmrtMainView()
        {
            InitializeComponent();
            SelectedViewModel = new GetToolInstancesView();
        }

        public static readonly DependencyProperty SelectedViewModelProperty =
            DependencyProperty.Register("SelectedViewModel", typeof(object), typeof(MmrtMainView), new PropertyMetadata(null));

        public object SelectedViewModel
        {
            get { return GetValue(SelectedViewModelProperty); }
            set { SetValue(SelectedViewModelProperty, value); }
        }
    }
}