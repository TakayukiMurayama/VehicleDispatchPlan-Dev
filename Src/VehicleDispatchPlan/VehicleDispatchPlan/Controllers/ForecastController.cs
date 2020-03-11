using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.DataVisualization.Charting;
using VehicleDispatchPlan.Constants;
using VehicleDispatchPlan.Models;

/**
 * 受入予測コントローラ
 *
 * @author t-murayama
 * @version 1.0
 * ----------------------------------
 * 2020/03/01 t-murayama 新規作成
 *
 */
namespace VehicleDispatchPlan.Controllers
{
    public class ForecastController : Controller
    {
        // データベースコンテキスト
        private MyDatabaseContext db = new MyDatabaseContext();

        /// <summary>
        /// 編集表示
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit()
        {
            //// 現在の年を取得
            //string year = DateTime.Now.ToString("yyyy");
            //// 現在の期を取得
            //string period = string.Empty;
            //switch (DateTime.Now.ToString("MM"))
            //{
            //    // 4～9月の場合
            //    case "04":
            //    case "05":
            //    case "06":
            //    case "07":
            //    case "08":
            //    case "09":
            //        period = AppConstant.CD_PERIOD_FIRST_HALF;
            //        break;
            //    // 10～3月の場合
            //    case "10":
            //    case "11":
            //    case "12":
            //    case "01":
            //    case "02":
            //    case "03":
            //        period = AppConstant.CD_PERIOD_SECOND_HALF;
            //        break;
            //    default:
            //        break;
            //}

            //// 年と期間を指定して予測条件を取得
            //T_Forecast forecast = db.Forecast.Where(x => year.Equals(x.Year) && period.Equals(x.Period)).FirstOrDefault();
            //// 取得されなかった場合
            //if (forecast == null)
            //{
            //    forecast = new T_Forecast();
            //    forecast.Year = year;
            //    forecast.Period = period;
            //}

            // 画面項目の設定
            this.SetDisplayItem();

            //return View(forecast);
            return View(new T_Forecast());
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        /// <param name="search"></param>
        /// <param name="update"></param>
        /// <param name="forecast"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string search, string update, [Bind(Include = "Year,Period,InstructorAmt,ClassQty,LodgingRatio,NotDrivingRatio")] T_Forecast forecast)
        {
            Trace.WriteLine("POST /Trainee/Edit/" + forecast.Year + "&" + forecast.Period);

            // 画面項目の設定
            this.SetDisplayItem();

            // 検索ボタンが押された場合
            if (search != null)
            {
                if (!string.IsNullOrEmpty(forecast.Year) && !string.IsNullOrEmpty(forecast.Period))
                {
                    // ステータスをクリア（画面表示を反映させる）
                    ModelState.Clear();
                    // 年と期間を指定して予測条件を取得
                    T_Forecast getForecast = db.Forecast.Where(x => forecast.Year.Equals(x.Year) && forecast.Period.Equals(x.Period)).FirstOrDefault();
                    // 取得された場合、バインドモデルに反映
                    if (getForecast != null)
                    {
                        forecast = getForecast;
                    }
                    // 対象期間を設定
                    ViewBag.LabelTargetPeriod = forecast.Year + "年" + (AppConstant.CD_PERIOD_FIRST_HALF.Equals(forecast.Period) ? ViewBag.LabelFirstHalf : ViewBag.LabelSecondHalf);
                }
            }

            // 更新ボタンが押された場合
            if (update != null)
            {
                if (ModelState.IsValid)
                {
                    // 登録済か確認
                    if (db.Forecast.Where(x => forecast.Year.Equals(x.Year) && forecast.Period.Equals(x.Period)).Count() == 0)
                    {
                        // 登録処理
                        db.Forecast.Add(forecast);
                        db.SaveChanges();
                    }
                    else
                    {
                        // 更新処理
                        db.Entry(forecast).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
            }

            return View(forecast);
        }

        /// <summary>
        /// グラフの作成
        /// </summary>
        /// <returns></returns>
        public ActionResult Chart(string year, string period)
        {
            // グラフを作成
            var chart = new Chart()
            {
                Height = 500,
                Width = 1500,
                ImageType = ChartImageType.Png,
                ChartAreas =
                {
                    new ChartArea
                    {
                        Name = "Default",
                        AxisY = new Axis
                        {
                            IsStartedFromZero = true
                        }
                    }
                },
                Legends =
                    {
                        new Legend
                        {
                            Title = "凡例"
                        }
                    }
            };

            chart.Series.Clear();

            // 受入可能人数
            chart.Series.Add(AppConstant.SERIES_FORECAST);
            chart.Series[AppConstant.SERIES_FORECAST].ChartType = SeriesChartType.Area;
            chart.Series[AppConstant.SERIES_FORECAST].Color = Color.FromArgb(100, 190, 70, 70);
            chart.Series[AppConstant.SERIES_FORECAST].MarkerStyle = MarkerStyle.Circle;
            chart.Series[AppConstant.SERIES_FORECAST].MarkerColor = Color.FromArgb(190, 70, 70);
            chart.Series[AppConstant.SERIES_FORECAST].BorderWidth = 2;
            chart.Series[AppConstant.SERIES_FORECAST].BorderColor = Color.FromArgb(190, 70, 70);

            // 合宿実績人数
            chart.Series.Add(AppConstant.SERIES_LODGING);
            chart.Series[AppConstant.SERIES_LODGING].ChartType = SeriesChartType.Area;
            chart.Series[AppConstant.SERIES_LODGING].Color = Color.FromArgb(100, 30, 90, 160);
            chart.Series[AppConstant.SERIES_LODGING].MarkerStyle = MarkerStyle.Circle;
            chart.Series[AppConstant.SERIES_LODGING].MarkerColor = Color.FromArgb(30, 90, 160);
            chart.Series[AppConstant.SERIES_LODGING].BorderWidth = 2;
            chart.Series[AppConstant.SERIES_LODGING].BorderColor = Color.FromArgb(30, 90, 160);

            // 通学実績人数
            chart.Series.Add(AppConstant.SERIES_COMMUTING);
            chart.Series[AppConstant.SERIES_COMMUTING].ChartType = SeriesChartType.Area;
            chart.Series[AppConstant.SERIES_COMMUTING].Color = Color.FromArgb(100, 30, 160, 60);
            chart.Series[AppConstant.SERIES_COMMUTING].MarkerStyle = MarkerStyle.Circle;
            chart.Series[AppConstant.SERIES_COMMUTING].MarkerColor = Color.FromArgb(30, 160, 60);
            chart.Series[AppConstant.SERIES_COMMUTING].BorderWidth = 2;
            chart.Series[AppConstant.SERIES_COMMUTING].BorderColor = Color.FromArgb(30, 160, 60);

            if (!string.IsNullOrEmpty(year) && !string.IsNullOrEmpty(period))
            {
                // 対象期間を設定（TODO：上期・下期の期間はマスタに持つ必要あり？）
                DateTime dateFrom;
                DateTime dateTo;
                if (AppConstant.CD_PERIOD_FIRST_HALF.Equals(period))
                {
                    dateFrom = Convert.ToDateTime(year + "/04/01");
                    dateTo = Convert.ToDateTime(year + "/09/30");
                }
                else
                {
                    dateFrom = Convert.ToDateTime(year + "/10/01");
                    dateTo = Convert.ToDateTime(year + "/03/31").AddYears(1);
                }

                // 日付範囲を±15日で設定
                dateFrom = dateFrom.AddDays(-15);
                dateTo = dateTo.AddDays(15);

                // 教習生データを取得
                string sql = "SELECT "
                    + "  SUB.* "
                    + "FROM ("
                    + "  SELECT "
                    + "    Trainee.TraineeId "
                    + "    , Trainee.AttendTypeCd "
                    + "    , Trainee.EntrancePlanDate "
                    + "    , DATEADD(DAY, TrainingCourse.GraduationDays - 1, Trainee.EntrancePlanDate) AS GraduatePlanDate "
                    + "  FROM "
                    + "    T_Trainee Trainee "
                    + "    LEFT OUTER JOIN M_TrainingCourse TrainingCourse "
                    + "      ON Trainee.TrainingCourseCd = TrainingCourse.TrainingCourseCd "
                    + ") AS SUB "
                    + "WHERE "
                    + "  SUB.EntrancePlanDate BETWEEN '" + dateFrom.ToShortDateString() + "' AND '" + dateTo.ToShortDateString() + "' "
                    + "  OR SUB.GraduatePlanDate BETWEEN '" + dateFrom.ToShortDateString() + "' AND '" + dateTo.ToShortDateString() + "' ";
                List<Q_Trainee> traineeList = db.Database.SqlQuery<Q_Trainee>(sql).ToList();

                // 日付単位でグラフにプロット
                DateTime targetDate;
                for (int day = 0; dateFrom.AddDays(day) <= dateTo; day++)
                {
                    targetDate = dateFrom.AddDays(day);
                    // 受入可能人数
                    chart.Series[AppConstant.SERIES_FORECAST].Points.AddXY(targetDate.ToString("M/d"), 125.5);
                    // 合宿実績人数
                    chart.Series[AppConstant.SERIES_LODGING].Points.AddXY(targetDate.ToString("M/d"),
                        traineeList.Where(x => AppConstant.CD_ATTEND_TYPE_LODGING.Equals(x.AttendTypeCd)
                        && Math.Abs(targetDate.CompareTo(x.EntrancePlanDate) + targetDate.CompareTo(x.EntrancePlanDate)) < 2).Count());
                    // 通学実績人数
                    chart.Series[AppConstant.SERIES_COMMUTING].Points.AddXY(targetDate.ToString("M/d"),
                        traineeList.Where(x => AppConstant.CD_ATTEND_TYPE_COMMUTING.Equals(x.AttendTypeCd)
                        && Math.Abs(targetDate.CompareTo(x.EntrancePlanDate) + targetDate.CompareTo(x.EntrancePlanDate)) < 2).Count());
                }
            }

            // TODO:テーブルを元に計算
            //chart.Series["受入可能人数"].Points.AddXY("1/15", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("1/16", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("1/17", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("1/18", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("1/19", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("1/20", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("1/21", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("1/22", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("1/23", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("1/24", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("1/25", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("1/26", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("1/27", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("1/28", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("1/29", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("1/30", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("1/31", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("2/1", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("2/2", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("2/3", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("2/4", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("2/5", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("2/6", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("2/7", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("2/8", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("2/9", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("2/10", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("2/11", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("2/12", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("2/13", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("2/14", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("2/15", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("2/16", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("2/17", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("2/18", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("2/19", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("2/20", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("2/21", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("2/22", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("2/23", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("2/24", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("2/25", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("2/26", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("2/27", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("2/28", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("2/29", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("3/1", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("3/2", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("3/3", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("3/4", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("3/5", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("3/6", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("3/7", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("3/8", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("3/9", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("3/10", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("3/11", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("3/12", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("3/13", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("3/14", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("3/15", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("3/16", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("3/17", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("3/18", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("3/19", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("3/20", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("3/21", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("3/22", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("3/23", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("3/24", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("3/25", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("3/26", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("3/27", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("3/28", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("3/29", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("3/30", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("3/31", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("4/1", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("4/2", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("4/3", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("4/4", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("4/5", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("4/6", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("4/7", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("4/8", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("4/9", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("4/10", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("4/11", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("4/12", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("4/13", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("4/14", 125.5);
            //chart.Series["受入可能人数"].Points.AddXY("4/15", 125.5);

            // TODO:教習生データを取得
            //chart.Series["受入実績人数"].Points.AddXY("1/15", 10);
            //chart.Series["受入実績人数"].Points.AddXY("1/16", 10);
            //chart.Series["受入実績人数"].Points.AddXY("1/17", 10);
            //chart.Series["受入実績人数"].Points.AddXY("1/18", 10);
            //chart.Series["受入実績人数"].Points.AddXY("1/19", 10);
            //chart.Series["受入実績人数"].Points.AddXY("1/20", 15);
            //chart.Series["受入実績人数"].Points.AddXY("1/21", 18);
            //chart.Series["受入実績人数"].Points.AddXY("1/22", 20);
            //chart.Series["受入実績人数"].Points.AddXY("1/23", 30);
            //chart.Series["受入実績人数"].Points.AddXY("1/24", 35);
            //chart.Series["受入実績人数"].Points.AddXY("1/25", 35);
            //chart.Series["受入実績人数"].Points.AddXY("1/26", 40);
            //chart.Series["受入実績人数"].Points.AddXY("1/27", 41);
            //chart.Series["受入実績人数"].Points.AddXY("1/28", 41);
            //chart.Series["受入実績人数"].Points.AddXY("1/29", 44);
            //chart.Series["受入実績人数"].Points.AddXY("1/30", 45);
            //chart.Series["受入実績人数"].Points.AddXY("1/31", 45);
            //chart.Series["受入実績人数"].Points.AddXY("2/1", 50);
            //chart.Series["受入実績人数"].Points.AddXY("2/2", 51);
            //chart.Series["受入実績人数"].Points.AddXY("2/3", 55);
            //chart.Series["受入実績人数"].Points.AddXY("2/4", 60);
            //chart.Series["受入実績人数"].Points.AddXY("2/5", 63);
            //chart.Series["受入実績人数"].Points.AddXY("2/6", 66);
            //chart.Series["受入実績人数"].Points.AddXY("2/7", 70);
            //chart.Series["受入実績人数"].Points.AddXY("2/8", 80);
            //chart.Series["受入実績人数"].Points.AddXY("2/9", 84);
            //chart.Series["受入実績人数"].Points.AddXY("2/10", 95);
            //chart.Series["受入実績人数"].Points.AddXY("2/11", 100);
            //chart.Series["受入実績人数"].Points.AddXY("2/12", 101);
            //chart.Series["受入実績人数"].Points.AddXY("2/13", 110);
            //chart.Series["受入実績人数"].Points.AddXY("2/14", 110);
            //chart.Series["受入実績人数"].Points.AddXY("2/15", 110);
            //chart.Series["受入実績人数"].Points.AddXY("2/16", 110);
            //chart.Series["受入実績人数"].Points.AddXY("2/17", 110);
            //chart.Series["受入実績人数"].Points.AddXY("2/18", 115);
            //chart.Series["受入実績人数"].Points.AddXY("2/19", 117);
            //chart.Series["受入実績人数"].Points.AddXY("2/20", 117);
            //chart.Series["受入実績人数"].Points.AddXY("2/21", 117);
            //chart.Series["受入実績人数"].Points.AddXY("2/22", 117);
            //chart.Series["受入実績人数"].Points.AddXY("2/23", 120);
            //chart.Series["受入実績人数"].Points.AddXY("2/24", 122);
            //chart.Series["受入実績人数"].Points.AddXY("2/25", 124);
            //chart.Series["受入実績人数"].Points.AddXY("2/26", 126);
            //chart.Series["受入実績人数"].Points.AddXY("2/27", 126);
            //chart.Series["受入実績人数"].Points.AddXY("2/28", 126);
            //chart.Series["受入実績人数"].Points.AddXY("2/29", 126);
            //chart.Series["受入実績人数"].Points.AddXY("3/1", 120);
            //chart.Series["受入実績人数"].Points.AddXY("3/2", 120);
            //chart.Series["受入実績人数"].Points.AddXY("3/3", 120);
            //chart.Series["受入実績人数"].Points.AddXY("3/4", 120);
            //chart.Series["受入実績人数"].Points.AddXY("3/5", 115);
            //chart.Series["受入実績人数"].Points.AddXY("3/6", 115);
            //chart.Series["受入実績人数"].Points.AddXY("3/7", 115);
            //chart.Series["受入実績人数"].Points.AddXY("3/8", 115);
            //chart.Series["受入実績人数"].Points.AddXY("3/9", 115);
            //chart.Series["受入実績人数"].Points.AddXY("3/10", 115);
            //chart.Series["受入実績人数"].Points.AddXY("3/11", 120);
            //chart.Series["受入実績人数"].Points.AddXY("3/12", 122);
            //chart.Series["受入実績人数"].Points.AddXY("3/13", 122);
            //chart.Series["受入実績人数"].Points.AddXY("3/14", 122);
            //chart.Series["受入実績人数"].Points.AddXY("3/15", 122);
            //chart.Series["受入実績人数"].Points.AddXY("3/16", 122);
            //chart.Series["受入実績人数"].Points.AddXY("3/17", 124);
            //chart.Series["受入実績人数"].Points.AddXY("3/18", 124);
            //chart.Series["受入実績人数"].Points.AddXY("3/19", 124);
            //chart.Series["受入実績人数"].Points.AddXY("3/20", 118);
            //chart.Series["受入実績人数"].Points.AddXY("3/21", 116);
            //chart.Series["受入実績人数"].Points.AddXY("3/22", 116);
            //chart.Series["受入実績人数"].Points.AddXY("3/23", 110);
            //chart.Series["受入実績人数"].Points.AddXY("3/24", 100);
            //chart.Series["受入実績人数"].Points.AddXY("3/25", 95);
            //chart.Series["受入実績人数"].Points.AddXY("3/26", 90);
            //chart.Series["受入実績人数"].Points.AddXY("3/27", 80);
            //chart.Series["受入実績人数"].Points.AddXY("3/28", 66);
            //chart.Series["受入実績人数"].Points.AddXY("3/29", 52);
            //chart.Series["受入実績人数"].Points.AddXY("3/30", 45);
            //chart.Series["受入実績人数"].Points.AddXY("3/31", 30);
            //chart.Series["受入実績人数"].Points.AddXY("4/1", 25);
            //chart.Series["受入実績人数"].Points.AddXY("4/2", 25);
            //chart.Series["受入実績人数"].Points.AddXY("4/3", 25);
            //chart.Series["受入実績人数"].Points.AddXY("4/4", 20);
            //chart.Series["受入実績人数"].Points.AddXY("4/5", 20);
            //chart.Series["受入実績人数"].Points.AddXY("4/6", 18);
            //chart.Series["受入実績人数"].Points.AddXY("4/7", 18);
            //chart.Series["受入実績人数"].Points.AddXY("4/8", 16);
            //chart.Series["受入実績人数"].Points.AddXY("4/9", 16);
            //chart.Series["受入実績人数"].Points.AddXY("4/10", 16);
            //chart.Series["受入実績人数"].Points.AddXY("4/11", 12);
            //chart.Series["受入実績人数"].Points.AddXY("4/12", 12);
            //chart.Series["受入実績人数"].Points.AddXY("4/13", 12);
            //chart.Series["受入実績人数"].Points.AddXY("4/14", 10);
            //chart.Series["受入実績人数"].Points.AddXY("4/15", 10);

            using (var stream = new System.IO.MemoryStream())
            {
                chart.SaveImage(stream);

                return File(stream.ToArray(), "image/png");
            }
        }

        /// <summary>
        /// 画面項目を設定
        /// </summary>
        private void SetDisplayItem()
        {
            // 年の選択肢を設定
            string year = DateTime.Now.ToString("yyyy");
            List<SelectListItem> selectYear = new List<SelectListItem>()
            {
                new SelectListItem() { Text = (Convert.ToInt32(year) - 2).ToString(), Value=(Convert.ToInt32(year) - 2).ToString() }
                , new SelectListItem() { Text = (Convert.ToInt32(year) - 1).ToString(), Value=(Convert.ToInt32(year) - 1).ToString() }
                , new SelectListItem() { Text = (Convert.ToInt32(year) + 0).ToString(), Value=(Convert.ToInt32(year) + 0).ToString() }
                , new SelectListItem() { Text = (Convert.ToInt32(year) + 1).ToString(), Value=(Convert.ToInt32(year) + 1).ToString() }
                , new SelectListItem() { Text = (Convert.ToInt32(year) + 2).ToString(), Value=(Convert.ToInt32(year) + 2).ToString() }
            };
            ViewBag.SelectYear = selectYear;

            // 期間の選択肢を設定
            ViewBag.FirstHalf = AppConstant.CD_PERIOD_FIRST_HALF;
            ViewBag.SecondHalf = AppConstant.CD_PERIOD_SECOND_HALF;
            ViewBag.LabelFirstHalf = db.CodeMaster.Where(x => AppConstant.DIV_PERIOD.Equals(x.Div) && AppConstant.CD_PERIOD_FIRST_HALF.Equals(x.Cd)).Select(x => x.Value).FirstOrDefault();
            ViewBag.LabelSecondHalf = db.CodeMaster.Where(x => AppConstant.DIV_PERIOD.Equals(x.Div) && AppConstant.CD_PERIOD_SECOND_HALF.Equals(x.Cd)).Select(x => x.Value).FirstOrDefault();

            // 対象期間の初期化
            ViewBag.LabelTargetPeriod = " ";
        }
    }
}