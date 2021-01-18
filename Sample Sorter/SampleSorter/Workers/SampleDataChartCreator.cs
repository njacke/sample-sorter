using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Drawing;
using GemBox.Spreadsheet;
using GemBox.Spreadsheet.Charts;
using System.IO;

namespace SampleSorter.Workers
{
    class SampleDataChartCreator
    {
        public void ChartCreator(List<List<double>> performanceTimes)
        {

            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");

            var workbook = new ExcelFile();
            var worksheet = workbook.Worksheets.Add("Sorting Performance Report");
            var dataTable = new DataTable();            
          
            dataTable.Columns.Add("Table Size[rows]", typeof(double));
            dataTable.Columns.Add("DB Sort Time[log(ms)]", typeof(double));
            dataTable.Columns.Add("Bubble Sort Time[log(ms)]", typeof(double));
            dataTable.Columns.Add("DB Sort Time[ms])", typeof(double));
            dataTable.Columns.Add("Bubble Sort Time[ms])", typeof(double));

            foreach (List<double> performanceTime in performanceTimes)
            {
                dataTable.Rows.Add(new object[] { performanceTime[0], Math.Log(performanceTime[1]), Math.Log(performanceTime[2]), performanceTime[1], performanceTime[2], });
            }

            worksheet.Rows[0].Style.Font.Weight = ExcelFont.BoldWeight;
            worksheet.Columns[0].Width = (int)LengthUnitConverter.Convert(2, LengthUnit.Centimeter, LengthUnit.ZeroCharacterWidth256thPart);
            worksheet.Columns[1].Width = (int)LengthUnitConverter.Convert(3, LengthUnit.Centimeter, LengthUnit.ZeroCharacterWidth256thPart);
            worksheet.Columns[2].Width = (int)LengthUnitConverter.Convert(3, LengthUnit.Centimeter, LengthUnit.ZeroCharacterWidth256thPart);

            worksheet.PrintOptions.FitWorksheetWidthToPages = 1;
            worksheet.PrintOptions.FitWorksheetHeightToPages = 1;


            var chart = worksheet.Charts.Add(ChartType.Line, "G2", "N20");
            chart.SelectData(worksheet.Cells.GetSubrangeAbsolute(0, 0, performanceTimes.Count + 1, 2), true);
            chart.Legend.IsVisible = true;
            chart.Legend.Position = ChartLegendPosition.Top;
            chart.Title.IsVisible = true;
            chart.Title.Text = "Sorting Performance\n[x = rows, y = log(ms)]";

            worksheet.InsertDataTable(dataTable,
                new InsertDataTableOptions()
                {
                    ColumnHeaders = true,
                    StartRow = 0
                });
                        
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
            workbook.Save(projectDirectory + @"\PerformanceReport.xlsx");
        }
    }
}
