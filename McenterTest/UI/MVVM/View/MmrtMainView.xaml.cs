using System.Windows;
using System.Windows.Controls;

namespace McenterTest.UI.MVVM.View
{
    public partial class MmrtMainView : UserControl
    {
        public MmrtMainView()
        {
            InitializeComponent();
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