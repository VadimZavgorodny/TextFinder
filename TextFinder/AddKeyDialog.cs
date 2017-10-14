using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace TextFinder {
    class AddKeyDialog: Window {

        Label l;
        RowDefinition rd;
        public TextBox tb;
        Grid grid;

        public AddKeyDialog(double x = 100, double y = 100) {

            this.Left = x;
            this.Top = y;

            this.Width = 230;
            this.Height = 140;
            this.ResizeMode = ResizeMode.NoResize;

            grid = new Grid();
            this.AddChild(grid);

            rd = new RowDefinition();
            rd.Height = new GridLength(40);
            grid.RowDefinitions.Add(rd);

            l = new Label();
            l.Content = "Ключ:";
            l.Margin = new Thickness(10, 3, 0, 0);

            tb = new TextBox();
            tb.Width = 100;
            tb.Height = 20;
            tb.Margin = new Thickness(100, 0, 0, 0);

            Grid.SetRow(l, 0);
            Grid.SetRow(tb, 0);
            grid.Children.Add(l);
            grid.Children.Add(tb);

            rd = new RowDefinition();
            rd.Height = new GridLength(40);
            grid.RowDefinitions.Add(rd);

            Button ok = new Button();
            ok.Width = 80;
            ok.Height = 24;
            ok.Content = "OK";
            ok.Margin = new Thickness(-120, 0, 0, 0);
            ok.Click += new RoutedEventHandler(OK);

            Button cancel = new Button();
            cancel.Width = 100;
            cancel.Height = 24;
            cancel.Content = "Cancel";
            cancel.Margin = new Thickness(100, 0, 0, 0);
            cancel.Click += new RoutedEventHandler(Cancel);

            Grid.SetRow(ok, 1);
            Grid.SetRow(cancel, 1);

            grid.Children.Add(ok);
            grid.Children.Add(cancel);
        }

        void Cancel(object sender, RoutedEventArgs e) {
            this.DialogResult = false;
        }

        void OK(object sender, RoutedEventArgs e) {
            this.DialogResult = true;
        }
    }
}
