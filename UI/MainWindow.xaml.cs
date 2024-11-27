using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Msagl.Core.Geometry.Curves;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.Layout.Layered;
using Microsoft.Msagl.WpfGraphControl;
using Microsoft.Data.SqlClient; 
using static Microsoft.Msagl.Core.Layout.LgNodeInfo;

namespace WPFGraphX
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Graph graph = new Graph();
            //graph.AddEdge("47", "58");
            //graph.AddEdge("70", "71");
            //var tn = graph.AddNode("test");

            //graph.AddEdge("test", "47");

            //var subgraph = new Subgraph("subgraph1");
            //subgraph.Label.Text = "Outer";
            //graph.RootSubgraph.AddSubgraph(subgraph);
            //subgraph.AddNode(graph.FindNode("47"));
            //subgraph.AddNode(graph.FindNode("58"));

            //var subgraph2 = new Subgraph("subgraph2");
            //subgraph2.Label.Text = "Inner";
            //subgraph2.Attr.Color = Microsoft.Msagl.Drawing.Color.Black;
            //subgraph2.Attr.FillColor = Microsoft.Msagl.Drawing.Color.Yellow;
            //subgraph2.Attr.ClusterLabelMargin = LabelPlacement.Bottom;
            //subgraph2.AddNode(graph.FindNode("70"));
            //subgraph2.AddNode(graph.FindNode("71"));
            //subgraph.AddSubgraph(subgraph2);
            //graph.AddEdge("58", subgraph2.Id);

            //graph.Attr.LayerDirection = LayerDirection.LR;
            ////graph.LayoutAlgorithmSettings.EdgeRoutingSettings.EdgeRoutingMode = EdgeRoutingMode.Rectilinear;

            //var global = (SugiyamaLayoutSettings)graph.LayoutAlgorithmSettings;
            //var local = (SugiyamaLayoutSettings)global.Clone();
            //local.Transformation = PlaneTransformation.Rotation(-Math.PI / 2);
            //subgraph2.LayoutSettings = local;   // for Collapsing\Expanding
            ////global.ClusterSettings.Add(subgraph2, local);

            //graphControl.Graph = graph;
        }

        private void LoadGraph_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection sqlConnection = new SqlConnection("Server=nindev10;Database=GraphDemo;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;"))
            using (SqlCommand sqlCommand = new SqlCommand("GetGraph", sqlConnection))
            {
                sqlConnection.Open();
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                Graph graph = new Graph();

                graph.LayoutAlgorithmSettings.PackingMethod = Microsoft.Msagl.Core.Layout.PackingMethod.Compact;

                // People
                while(sqlDataReader.Read())
                {
                    Node personNode = graph.AddNode($"Person-{sqlDataReader.GetInt32(0)}");
                    personNode.Attr.Shape = Microsoft.Msagl.Drawing.Shape.Hexagon;
                    personNode.Attr.FillColor = Microsoft.Msagl.Drawing.Color.Yellow;
                    personNode.LabelText = sqlDataReader.GetString(1);
                }

                // Cities
                sqlDataReader.NextResult();

                while (sqlDataReader.Read())
                {
                    Node cityNode = graph.AddNode($"City-{sqlDataReader.GetInt32(0)}");
                    cityNode.Attr.FillColor = Microsoft.Msagl.Drawing.Color.DarkViolet;
                    cityNode.LabelText = sqlDataReader.GetString(1);
                    cityNode.Label.FontColor = Microsoft.Msagl.Drawing.Color.White;
                }

                // Friends
                sqlDataReader.NextResult();

                while(sqlDataReader.Read())
                {
                    Node friend = graph.FindNode($"Person-{sqlDataReader.GetInt32(0)}");
                    Node friendOf = graph.FindNode($"Person-{sqlDataReader.GetInt32(1)}");

                    Edge edge = new Edge(friend, friendOf, ConnectionToGraph.Connected);
                    edge.LabelText = "Friend Of";
                    edge.Label.FontSize = 8.0;

                    friend.AddOutEdge(edge);
                }

                // Lives in
                sqlDataReader.NextResult();

                while (sqlDataReader.Read())
                {
                    Node person = graph.FindNode($"Person-{sqlDataReader.GetInt32(0)}");
                    Node city = graph.FindNode($"City-{sqlDataReader.GetInt32(1)}");

                    Edge edge = new Edge(person, city, ConnectionToGraph.Connected);
                    edge.LabelText = "Lives In";
                    edge.Label.FontSize = 8.0;

                    person.AddOutEdge(edge);
                }

                graphControl.Graph = graph;
            }
        }

        private void graphControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {


        }

        private void ShowGridButton_Click(object sender, RoutedEventArgs e)
        {
            BlogPostWindow window = new BlogPostWindow();
            window.ShowDialog();
        }
    }
}