using Microsoft.EntityFrameworkCore;
using System;
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
using System.Windows.Shapes;

namespace WPFGraphX
{
    /// <summary>
    /// Interaction logic for BlogPostWindow.xaml
    /// </summary>
    public partial class BlogPostWindow : Window
    {
        public BlogPostWindow()
        {
            InitializeComponent();
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            using (BloggingContext context = new BloggingContext())
            {
                PostGrid.ItemsSource = context.Blogs
                    .Include(b => b.Posts).FirstOrDefault()?.Posts ?? new List<Post>();
            }
        }
    }
}
