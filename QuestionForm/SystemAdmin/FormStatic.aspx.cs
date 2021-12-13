using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using ActiveQuestion.DBSource;
using System.Web.UI.DataVisualization.Charting;

namespace QuestionForm.SystemAdmin
{
    public partial class FormStatic : System.Web.UI.Page
    {
        public string PieData;
        protected void Page_Load(object sender, EventArgs e)
        {
            string id = this.Request.QueryString["ID"];

            if (!IsPostBack)
            {
                if (!string.IsNullOrWhiteSpace(id) && id.Length == 36)
                {
                    DataRow QuesRow = QuestionnaireManager.GetQuestionnaireDataRow(Guid.Parse(id)); // 從 DB 抓問卷
                    DataTable ProblemDT = ProblemManager.GetProblem(Guid.Parse(id)); // 從 DB 抓問題

                    //先判斷 QuesGuid 是否有誤
                    if (QuesRow == null || QuesRow["M_Guid"].ToString() != id)
                    {
                        Response.Write("<Script language='JavaScript'>alert(' Guid 錯誤，將您導向回列表頁'); location.href='FormList.aspx'; </Script>");
                        return;
                    }
                    else // 印出問卷名稱及描述內容
                    {
                        this.ltlCaption.Text = "<h2>" + QuesRow["M_title"].ToString() + "</h2><br />";
                        this.ltlDescription.Text = QuesRow["Summary"].ToString();
                    }

                    DataTable dt = StaticData.DrawStatic(Guid.Parse(id));
                    for (int perProblem = 0; perProblem < ProblemDT.Rows.Count; perProblem++) // 跑每個問題
                    {
                        string probGuid = ProblemDT.Rows[perProblem]["D_Guid"].ToString(); // 先以 QuesGuid 找問題DT
                        DataTable StaticDT = StaticData.GetStatic(Guid.Parse(probGuid)); // 再以問題DT的 ProbGuid 找統計DT >> 每一題

                        Label problemText = new Label();
                        int type = Convert.ToInt32(ProblemDT.Rows[perProblem]["SelectionType"]);
                        if (type == 0 || type == 1) // 單選、複選
                        {
                            problemText.Text = (perProblem + 1).ToString() + "." + ProblemDT.Rows[perProblem]["D_title"];
                            PlaceHolder1.Controls.Add(problemText); // 印出問題名，替代Chart的Title(圖形的標題集合)

                            Panel perChart = new Panel();// 一個圖表給他一個Panel，可佔去一行(自動換行)
                            //BindChart(perProblem, perChart);
                            BindChart(perChart, StaticDT);
                            PlaceHolder1.Controls.Add(perChart); // Panel再裝進PlaceHolder(可裝控制項的容器)
                        }
                        else // 文字
                        {
                            problemText.Text = (perProblem + 1).ToString() + "." + ProblemDT.Rows[perProblem]["D_title"] + "<br />&nbsp &nbsp -<br /><br /><br />";
                            PlaceHolder1.Controls.Add(problemText);
                        }
                    }

                }
                else
                    Response.Write("<Script language='JavaScript'>alert(' QueryString 錯誤，將您導向回列表頁'); location.href='FormList.aspx'; </Script>");

            }
        }
        private void BindChart(Panel panelChart, DataTable StaticDT)
        {
            //一個Chart之中，可以有多個ChartArea，一個ChartArea可以有多個Series，一個Series對應一個Legend。
            Chart chart = new Chart(); // 圖表本身(根類別)
            ChartArea area = new ChartArea("Area"); // 圖表的區域集合(圖表的繪圖區)，要放進上一行的Chart
            Series series = new Series("Series"); // 圖形集合(匯入資料實際呈現的圖形樣式、形狀)，可於ChartArea中添加多個Series
            Legend legend = new Legend("Legend"); // Series的圖例集合，標注圖形中各個線條或顏色的含義(圖例說明)
                                                  // Annotations : 圖形的註解集合，可設置註解物件的放置位置、呈現顏色、大小、文字內容樣式等常見屬性。

            chart.ChartAreas.Add(area); // 圖表區域集合
            chart.Series.Add(series); // 數據序列集合
            chart.Legends.Add(legend); // 圖例集合說明

            chart.Width = 570;
            chart.Height = 300;
            chart.ChartAreas["Area"].Area3DStyle.Enable3D = true; // 3D
            chart.ChartAreas["Area"].AxisX.Interval = 1;
            chart.Series["Series"].ChartType = SeriesChartType.Pie; // 圓餅圖
            chart.Series["Series"].Label = "#PERCENT{P2}"; // 顯示百分比

            // 參考來源：http://blog.sina.com.cn/s/blog_51beaf0e0100yffo.html
            LegendCellColumn CellColumns1 = new LegendCellColumn();
            LegendCellColumn CellColumns2 = new LegendCellColumn();
            chart.Legends["Legend"].CellColumns.Add(CellColumns1);
            chart.Legends["Legend"].CellColumns.Add(CellColumns2);
            chart.Legends["Legend"].CellColumns[0].ColumnType = LegendCellColumnType.SeriesSymbol;
            chart.Legends["Legend"].CellColumns[1].ColumnType = LegendCellColumnType.Text;
            chart.Legends["Legend"].CellColumns[1].Text = "#VALX  =>  #VALY 票";

            for (int j = 0; j < StaticDT.Rows.Count; j++) // X 軸為選項， Y 軸為數量
                chart.Series["Series"].Points.AddXY(StaticDT.Rows[j]["OptionText"], StaticDT.Rows[j]["Count"]);

            panelChart.Controls.Add(chart);
        }

    }
}