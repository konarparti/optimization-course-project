using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
using ChartDirector;
using OptimizatonMethods.Models;

namespace OptimizatonMethods
{
    /// <summary>
    /// Логика взаимодействия для Chart2DWindow.xaml
    /// </summary>
    public partial class Chart2DWindow : Window
    {
        private readonly List<Point3D> _dataList = new List<Point3D>();
        private ContourLayer contourLayer;

        // Keep track of mouse dragging state
        private enum DragState { None, Horizontal, Vertical };
        private DragState isDragging = DragState.None;
        private int dragOffset = 0;

        // Crosshair position
        private int crossHairX = 300;
        private int crossHairY = 300;
        public Chart2DWindow(List<Point3D> dataList)
        {
            _dataList = dataList;
            InitializeComponent();
        }

		private void drawChart(WPFChartViewer viewer)
		{
			// The x and y coordinates of the grid
            double[] dataX = new double[_dataList.Count];
            double[] dataY = new double[_dataList.Count];
            double[] dataZ = new double[_dataList.Count];

            for (int i = 0; i < dataX.Length; i++)
            {
                dataX[i] = _dataList[i].X;
                dataY[i] = _dataList[i].Y;
                dataZ[i] = _dataList[i].Z;
            }

			// Create a XYChart object of size 575 x 525 pixels
			XYChart c = new XYChart(575, 525);

			// Set the plotarea at (75, 30) and of size 450 x 450 pixels. Use semi-transparent black
			// (80000000) dotted lines for both horizontal and vertical grid lines
			PlotArea p = c.setPlotArea(75, 30, 450, 450, -1, -1, -1, c.dashLineColor(
				unchecked((int)0xaf000000), Chart.DotLine), -1);

			// Set the chart and axis titles
			c.addTitle("     <*block,bgcolor=FFFF00*> *** Drag Crosshair to Move Cross Section *** <*/*>",
				"Arial Bold", 15);
			c.xAxis().setTitle("X-Axis Title Place Holder", "Arial Bold Italic", 10);
			c.yAxis().setTitle("Y-Axis Title Place Holder", "Arial Bold Italic", 10);

			// Put the y-axis on the right side of the chart
			c.setYAxisOnRight();

			// Set x-axis and y-axis labels to use Arial Bold font
			c.xAxis().setLabelStyle("Arial", 10);
			c.yAxis().setLabelStyle("Arial", 10);

			// When auto-scaling, use tick spacing of 40 pixels as a guideline
			c.yAxis().setTickDensity(40);
			c.xAxis().setTickDensity(40);

			// Add a contour layer using the given data
			contourLayer = c.addContourLayer(dataX, dataY, dataZ);
			contourLayer.setContourLabelFormat("<*font=Arial Bold,size=10*>{value}<*/font*>");

			// Move the grid lines in front of the contour layer
			c.getPlotArea().moveGridBefore(contourLayer);

			// Add a vertical color axis at x = 0 at the same y-position as the plot area.
			ColorAxis cAxis = contourLayer.setColorAxis(0, p.getTopY(), Chart.TopLeft,
				p.getHeight(), Chart.Right);
			// Use continuous gradient coloring (as opposed to step colors)
			cAxis.setColorGradient(true);

			// Add a title to the color axis using 12 points Arial Bold Italic font
			cAxis.setTitle("Color Legend Title Place Holder", "Arial Bold Italic", 10);

			// Set color axis labels to use Arial Bold font
			cAxis.setLabelStyle("Arial", 10);

			// Set the chart image to the WinChartViewer
			viewer.Chart = c;

			// Tooltip for the contour chart
			viewer.ImageMap = c.getHTMLImageMap("", "",
				"title='<*cdml*><*font=Arial Bold*>X={x|2}<*br*>Y={y|2}<*br*>Z={z|2}'");

			// Initializse the crosshair position to the center of the chart
			crossHairX = p.getLeftX() + p.getWidth() / 2;
			crossHairY = p.getTopY() + p.getHeight() / 2;

			// Draw the cross section and crosshair
			drawCrossSectionX(crossSectionViewerX);
			drawCrossSectionY(crossSectionViewerY);
			drawCrossHair(viewer);
		}
		private void drawCrossSectionX(WPFChartViewer viewer)
		{
			// Get data of the vertical cross section data at the given x coordinate
			XYChart mainChart = (XYChart)WPFChartViewer1.Chart;
			PlotArea p = mainChart.getPlotArea();
			double[] z = contourLayer.getCrossSection(crossHairX, p.getBottomY(), crossHairX,
				p.getTopY());

			// Create XYChat of the same height as the main chart. Align the plot area with that of the 
			// main chart.
			XYChart c = new XYChart(100, mainChart.getHeight());
			c.setPlotArea(10, p.getTopY(), c.getWidth() - 22, p.getHeight(), -1, -1, -1,
				c.dashLineColor(unchecked((int)0xaf000000), Chart.DotLine), -1);

			// The vertical chart will have the x-axis vertical and y-axis horizontal. Synchroinze the
			// vertical axis (x-axis) with the y-axis of the main chart. Synchroinze the horizontal 
			// axis (y-axis) with the color axis.
			c.swapXY();
			c.xAxis().syncAxis(mainChart.yAxis());
			c.yAxis().syncScale(contourLayer.colorAxis());

			// The vertical axis (x-axis) does not need labels as it is aligned with the main chart y-axis.
			c.xAxis().setLabelStyle("normal", 8, Chart.Transparent);

			// Rotate the horizontal axis (y-axis) labels by 270 degrees
			c.yAxis().setLabelStyle("normal", 8, Chart.TextColor, 270);

			// Add an area layer using the cross section data and the color scale of the color axis.
			int scaleColor = c.yScaleColor(contourLayer.colorAxis().getColorScale());
			AreaLayer layer = c.addAreaLayer(z, scaleColor);
			layer.setBorderColor(Chart.SameAsMainColor);
			layer.setXData(mainChart.getYValue(p.getBottomY()), mainChart.getYValue(p.getTopY()));

			// Display the chart
			viewer.Chart = c;
			viewer.updateDisplay();
		}

		//
		// Draw the y cross section chart
		//
		private void drawCrossSectionY(WPFChartViewer viewer)
		{
			// Get the vertical horizontal section data at the given y coordinate
			XYChart mainChart = (XYChart)WPFChartViewer1.Chart;
			PlotArea p = mainChart.getPlotArea();
			double[] z = contourLayer.getCrossSection(p.getLeftX(), crossHairY, p.getRightX(),
				crossHairY);

			// Create XYChat of the same width as the main chart. Align the plot area with that of the 
			// main chart.
			XYChart c = new XYChart(mainChart.getWidth(), 100);
			c.setPlotArea(p.getLeftX(), 10, p.getWidth(), c.getHeight() - 22, -1, -1, -1,
				c.dashLineColor(unchecked((int)0xaf000000), Chart.DotLine), -1);

			// Synchroinze the x-axis with the x-axis of the main chart. Synchroinze the y-axis with the
			// color axis.
			c.xAxis().syncAxis(mainChart.xAxis());
			c.yAxis().syncScale(contourLayer.colorAxis());

			// The x-axis does not need labels as it is aligned with the main chart x-axis.
			c.xAxis().setLabelStyle("normal", 8, Chart.Transparent);

			// Add an area layer using the cross section data and the color scale of the color axis.
			int scaleColor = c.yScaleColor(contourLayer.colorAxis().getColorScale());
			AreaLayer layer = c.addAreaLayer(z, scaleColor);
			layer.setBorderColor(Chart.SameAsMainColor);
			layer.setXData(mainChart.getXValue(p.getLeftX()), mainChart.getXValue(p.getRightX()));

			// Display the chart
			viewer.Chart = c;
			viewer.updateDisplay();
		}
		private void drawCrossHair(WPFChartViewer viewer)
        {
            // Get the chart to draw the crosshair on.
            XYChart c = (XYChart)viewer.Chart;

            // The crosshair will be drawn on the dynamic layer of the chart.
            DrawArea d = c.initDynamicLayer();

            // Add two lines across the plot area of the chart
            PlotArea p = c.getPlotArea();
            d.line(crossHairX, p.getTopY(), crossHairX, p.getBottomY(), 0x000000, 2);
            d.line(p.getLeftX(), crossHairY, p.getRightX(), crossHairY, 0x000000, 2);

            // Update the display
            viewer.updateDisplay();
        }
		
        private void WPFChartViewer1_MouseMovePlotArea(object sender, MouseEventArgs e)
        {
			WPFChartViewer viewer = WPFChartViewer1;
            int mouseX = viewer.ChartMouseX;
            int mouseY = viewer.ChartMouseY;

            // Check if mouse button is down
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
				//
				// If the mouse is near the crosshair while the mouse button is pressed, then it is drag
				// dragging the crosshair and we need to update the contour projection.
				//

				if ((isDragging == DragState.Vertical) && (crossHairX != mouseX - dragOffset))
                {
                    // Is dragging the vertical crosshair line
                    crossHairX = viewer.PlotAreaMouseX;
                    drawCrossSectionX(crossSectionViewerX);
                    drawCrossHair(viewer);
                }

                if ((isDragging == DragState.Horizontal) && (crossHairY != mouseY - dragOffset))
                {
                    // Is dragging the horizontal crosshair line
                    crossHairY = viewer.PlotAreaMouseY;
                    drawCrossSectionY(crossSectionViewerY);
                    drawCrossHair(viewer);
                }
			}
            else
            {
                //
                // If mouse is near the crosshair, it can drag it by pressing the mouse button.
                //
                if (Math.Abs(dragOffset = mouseX - crossHairX) <= 8)
                    isDragging = DragState.Vertical;   // Mouse is a position to drag the vertical line
                else if (Math.Abs(dragOffset = mouseY - crossHairY) <= 8)
                    isDragging = DragState.Horizontal; // Mouse is a position to drag the horizontal line
                else
                    isDragging = DragState.None;
            }
		}

        private void Chart2DWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            drawChart(WPFChartViewer1);

            // Extended the plot area mouse event region to make it easier to drag the crosshair
            WPFChartViewer1.setPlotAreaMouseMargin(100);
        }
    }
}
