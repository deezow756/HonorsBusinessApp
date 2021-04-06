using BusinessApp.Models;
using BusinessApp.Utilities;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessApp.Controllers
{
    public class ProfitsController
    {
        PlotModel model;

        public ProfitsController()
        {
            Themes.ThemeHelper.ThemeChanged += ThemeChanged;
        }

        public async Task<List<Order>> GetOrders(Company company)
        {
            FirebaseHelper helper = new FirebaseHelper();
            return await helper.GetAllOrders(company.CompanyNumber);
        }

        public async Task<List<StockItem>> GetStocks(Company company)
        {
            FirebaseHelper helper = new FirebaseHelper();
            return await helper.GetAllStockItems(company.CompanyNumber);
        }

        public PlotModel CreateBarChart(bool stacked, double[] profitValues)
        {
            model = new PlotModel
            {
                Title = "",
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.BottomCenter,
                LegendOrientation = LegendOrientation.Horizontal,
                LegendBorderThickness = 0                
            };

            var s1 = new ColumnSeries { Title = "", IsStacked = stacked, StrokeThickness = 1 };
            s1.Items.Add(new ColumnItem { Value = profitValues[0], Color = OxyColors.Gray });
            s1.Items.Add(new ColumnItem { Value = profitValues[1], Color = OxyColors.Gray });
            s1.Items.Add(new ColumnItem { Value = profitValues[2], Color = OxyColors.Gray });
            s1.Items.Add(new ColumnItem { Value = profitValues[3], Color = OxyColors.Gray });
            s1.Items.Add(new ColumnItem { Value = profitValues[4], Color = OxyColors.Gray });
            s1.Items.Add(new ColumnItem { Value = profitValues[5], Color = OxyColors.Gray });
            s1.Items.Add(new ColumnItem { Value = profitValues[6], Color = OxyColors.Gray });
            s1.Items.Add(new ColumnItem { Value = profitValues[7], Color = OxyColors.Gray });
            s1.Items.Add(new ColumnItem { Value = profitValues[8], Color = OxyColors.Gray });
            s1.Items.Add(new ColumnItem { Value = profitValues[9], Color = OxyColors.Gray });
            s1.Items.Add(new ColumnItem { Value = profitValues[10], Color = OxyColors.Gray });
            s1.Items.Add(new ColumnItem { Value = profitValues[11], Color = OxyColors.Gray });

            var categoryAxis = new CategoryAxis { Position = ValueAxisPosition() };
            categoryAxis.Labels.Add("Jan");
            categoryAxis.Labels.Add("Feb");
            categoryAxis.Labels.Add("Mar");
            categoryAxis.Labels.Add("Apr");
            categoryAxis.Labels.Add("May");
            categoryAxis.Labels.Add("June");
            categoryAxis.Labels.Add("July");
            categoryAxis.Labels.Add("Aug");
            categoryAxis.Labels.Add("Sep");
            categoryAxis.Labels.Add("Oct");
            categoryAxis.Labels.Add("Nov");
            categoryAxis.Labels.Add("Dec");
            var valueAxis = new LinearAxis { Position = CategoryAxisPosition(), MinimumPadding = 0, MaximumPadding = 0.06, AbsoluteMinimum = 0 };
            model.Series.Add(s1);
            model.Axes.Add(categoryAxis);
            model.Axes.Add(valueAxis);
            return model;
        }

        private AxisPosition CategoryAxisPosition()
        {
            if (typeof(BarSeries) == typeof(ColumnSeries))
            {
                return AxisPosition.Bottom;
            }

            return AxisPosition.Left;
        }

        private AxisPosition ValueAxisPosition()
        {
            if (typeof(BarSeries) == typeof(ColumnSeries))
            {
                return AxisPosition.Left;
            }

            return AxisPosition.Bottom;
        }

        //private PlotModel CreatePieChart()
        //{
        //    var model = new PlotModel { Title = "World population by continent" };

        //    var ps = new PieSeries
        //    {
        //        StrokeThickness = .25,
        //        InsideLabelPosition = .25,
        //        AngleSpan = 360,
        //        StartAngle = 0
        //    };

        //    // http://www.nationsonline.org/oneworld/world_population.htm  
        //    // http://en.wikipedia.org/wiki/Continent  
        //    ps.Slices.Add(new PieSlice("Africa", 1030) { IsExploded = false });
        //    ps.Slices.Add(new PieSlice("Americas", 929) { IsExploded = false });
        //    ps.Slices.Add(new PieSlice("Asia", 4157));
        //    ps.Slices.Add(new PieSlice("Europe", 739) { IsExploded = false });
        //    ps.Slices.Add(new PieSlice("Oceania", 35) { IsExploded = false });
        //    model.Series.Add(ps);
        //    return model;
        //}

        //public PlotModel CreateAreaChart()
        //{
        //    var plotModel1 = new PlotModel { Title = "AreaSeries with crossing lines" };
        //    var areaSeries1 = new AreaSeries();
        //    areaSeries1.Points.Add(new DataPoint(0, 50));
        //    areaSeries1.Points.Add(new DataPoint(10, 140));
        //    areaSeries1.Points.Add(new DataPoint(20, 60));
        //    areaSeries1.Points2.Add(new DataPoint(0, 60));
        //    areaSeries1.Points2.Add(new DataPoint(5, 80));
        //    areaSeries1.Points2.Add(new DataPoint(20, 70));
        //    plotModel1.Series.Add(areaSeries1);
        //    return plotModel1;
        //}

        public void ThemeChanged(Object sender, EventArgs e)
        {
            if (model == null)
                return;
            if(Themes.ThemeHelper.CurrentTheme == Themes.ThemeType.Dark)
            {
                model.TitleColor = OxyColors.White;
                model.TextColor = OxyColors.White;
                model.SubtitleColor = OxyColors.White;
                model.SelectionColor = OxyColors.White;
                model.PlotAreaBorderColor = OxyColors.White;
            }
            else
            {
                model.TitleColor = OxyColors.Black;
                model.TextColor = OxyColors.Black;
                model.SubtitleColor = OxyColors.Black;
                model.SelectionColor = OxyColors.Black;
                model.PlotAreaBorderColor = OxyColors.Black;
            }
        }

        public async void Displayhelp()
        {
            await Dialog.Show("Help", "The profiles chart can by filtered by year by clicking the year in the top right and selecting a year", "Ok");
        }
    }
}
