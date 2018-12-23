using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TickTackDebugger
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var appModel = (AppModel)DataContext;
            appModel.CodeSpan
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(x => SourceCodeText.Select(x.start, x.length));
        }

        public void FocusSourceCode()
        {
            SourceCodeText.Focus();
        }
    }
}
