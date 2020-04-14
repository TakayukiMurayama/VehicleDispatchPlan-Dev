using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VehicleDispatchPlan.Models;

/**
 * 教習生管理コントローラ
 *
 * @author t-murayama
 * @version 1.0
 * ----------------------------------
 * 2020/03/01 t-murayama 新規作成
 *
 */
namespace VehicleDispatchPlan.Controllers
{
    public class TraineeController : Controller
    {
        // データベースコンテキスト
        private MyDatabaseContext db = new MyDatabaseContext();

        /// <summary>
        /// 一覧表示
        /// </summary>
        /// <param name="planDateFrom">入校予定日From</param>
        /// <param name="planDateTo">入校予定日To</param>
        /// <returns></returns>
        public ActionResult List(DateTime? planDateFrom, DateTime? planDateTo)
        {
            Trace.WriteLine("GET /Trainee/List");

            // 教習生一覧情報を取得
            List<Q_Trainee> tarineeList = this.GetTraineeList(planDateFrom, planDateTo);

            return View(tarineeList);
        }

        /// <summary>
        /// 詳細表示
        /// </summary>
        /// <param name="id">教習生ID</param>
        /// <returns></returns>
        public ActionResult Details(int? id)
        {
            Trace.WriteLine("GET /Trainee/Details/" + id);

            // IDが空の場合、エラー
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // 教習生情報を取得
            Q_Trainee trainee = this.GetTraineeInfo(id);

            return View(trainee);
        }

        /// <summary>
        /// 登録表示
        /// </summary>
        /// <returns></returns>
        public ActionResult Regist()
        {
            Trace.WriteLine("GET /Trainee/Regist");

            // 教習生のインスタンスを生成
            T_Trainee trainee = new T_Trainee { EntrancePlanDate = DateTime.Now };

            // ドロップダウンリストの選択肢を設定
            this.SetSelectItem(trainee);

            return View(trainee);
        }

        /// <summary>
        /// 登録処理
        /// </summary>
        /// <param name="trainee">教習生情報</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Regist([Bind(Include = "TraineeName,AttendTypeCd,TrainingCourseCd,,EntrancePlanDate,LodgingCd,AgentCd")] T_Trainee trainee)
        {
            Trace.WriteLine("POST /Trainee/Regist/" + trainee.TraineeId);
            
            if (ModelState.IsValid)
            {
                // 登録処理
                db.Trainee.Add(trainee);
                db.SaveChanges();
                // 一覧へリダイレクト
                return RedirectToAction("List");
            }

            // ドロップダウンリストの選択肢を設定
            this.SetSelectItem(trainee);

            return View(trainee);
        }

        /// <summary>
        /// 編集表示
        /// </summary>
        /// <param name="id">教習生ID</param>
        /// <returns></returns>
        public ActionResult Edit(int? id)
        {
            Trace.WriteLine("GET /Trainee/Edit/" + id);

            // 教習生情報を取得
            T_Trainee trainee = db.Trainee.Find(id);
            if (trainee == null)
            {
                return HttpNotFound();
            }

            // ドロップダウンリストの選択肢を設定
            this.SetSelectItem(trainee);

            return View(trainee);
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        /// <param name="trainee">教習生情報</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TraineeId,TraineeName,AttendTypeCd,TrainingCourseCd,EntrancePlanDate,LodgingCd,AgentCd")] T_Trainee trainee)
        {
            Trace.WriteLine("POST /Trainee/Edit/" + trainee.TraineeId);

            if (ModelState.IsValid)
            {
                // 更新処理
                db.Entry(trainee).State = EntityState.Modified;
                db.SaveChanges();
                // 一覧へリダイレクト
                return RedirectToAction("List");
            }

            // ドロップダウンリストの選択肢を設定
            this.SetSelectItem(trainee);

            return View(trainee);
        }

        /// <summary>
        /// 削除処理
        /// </summary>
        /// <param name="id">教習生ID</param>
        /// <returns></returns>
        public ActionResult Delete(int? id)
        {
            Trace.WriteLine("GET /Trainee/Delete/" + id);

            // IDが空の場合、エラー
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // 教習生情報を取得
            Q_Trainee trainee = this.GetTraineeInfo(id);
            if (trainee == null)
            {
                return HttpNotFound();
            }

            return View(trainee);
        }

        /// <summary>
        /// 削除処理
        /// </summary>
        /// <param name="id">教習生ID</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Trace.WriteLine("POST /Trainee/Delete/" + id);

            // 教習生データを取得
            T_Trainee trainee = db.Trainee.Find(id);
            // 削除
            db.Trainee.Remove(trainee);
            db.SaveChanges();

            // 一覧へリダイレクト
            return RedirectToAction("List");
        }

        /// <summary>
        /// 教習生一覧取得
        /// </summary>
        /// <param name="planDateFrom">入校予定日From</param>
        /// <param name="planDateTo">入校予定日To</param>
        /// <returns>教習生一覧</returns>
        private List<Q_Trainee> GetTraineeList(DateTime? planDateFrom, DateTime? planDateTo)
        {
            bool whereFlg = false;

            string sql = "SELECT "
                + "  Trainee.TraineeId "
                + "  , Trainee.TraineeName "
                + "  , AttendType.Value AS AttendTypeName "
                + "  , TrainingCourse.TrainingCourseName "
                + "  , Trainee.EntrancePlanDate "
                + "  , LodgingFacility.LodgingName "
                + "  , Agent.AgentName "
                + "FROM "
                + "  T_Trainee Trainee "
                + "  LEFT OUTER JOIN M_CodeMaster AttendType "
                + "    ON AttendType.Div = '01' "
                + "    AND Trainee.AttendTypeCd = AttendType.Cd "
                + "  LEFT OUTER JOIN M_TrainingCourse TrainingCourse "
                + "    ON Trainee.TrainingCourseCd = TrainingCourse.TrainingCourseCd "
                + "  LEFT OUTER JOIN M_LodgingFacility LodgingFacility "
                + "    ON Trainee.LodgingCd = LodgingFacility.LodgingCd "
                + "  LEFT OUTER JOIN M_Agent Agent "
                + "    ON Trainee.AgentCd = Agent.AgentCd ";
            
            if (planDateFrom != null)
            {
                sql = sql + "WHERE Trainee.EntrancePlanDate >= '" + planDateFrom + "'";
                whereFlg = true;
            }

            if (planDateTo != null)
            {
                sql = sql + (whereFlg ? " AND " : " WHERE ") + "Trainee.EntrancePlanDate <= '" + planDateTo + "'";
            }

            return db.Database.SqlQuery<Q_Trainee>(sql).ToList();
        }

        /// <summary>
        /// 教習生情報取得
        /// </summary>
        /// <param name="id">教習生ID</param>
        /// <returns>教習生情報</returns>
        private Q_Trainee GetTraineeInfo(int? id)
        {
            string sql = "SELECT "
                + "  Trainee.TraineeId "
                + "  , Trainee.TraineeName "
                + "  , AttendType.Value AS AttendTypeName "
                + "  , TrainingCourse.TrainingCourseName "
                + "  , Trainee.EntrancePlanDate "
                + "  , LodgingFacility.LodgingName "
                + "  , Agent.AgentName "
                + "FROM "
                + "  T_Trainee Trainee "
                + "  LEFT OUTER JOIN M_CodeMaster AttendType "
                + "    ON AttendType.Div = '01' "
                + "    AND Trainee.AttendTypeCd = AttendType.Cd "
                + "  LEFT OUTER JOIN M_TrainingCourse TrainingCourse "
                + "    ON Trainee.TrainingCourseCd = TrainingCourse.TrainingCourseCd "
                + "  LEFT OUTER JOIN M_LodgingFacility LodgingFacility "
                + "    ON Trainee.LodgingCd = LodgingFacility.LodgingCd "
                + "  LEFT OUTER JOIN M_Agent Agent "
                + "    ON Trainee.AgentCd = Agent.AgentCd "
                + "WHERE "
                + "  Trainee.TraineeId = " + id;

            return db.Database.SqlQuery<Q_Trainee>(sql).ToList().FirstOrDefault();
        }

        /// <summary>
        /// ドロップダウンリストの選択肢を設定
        /// </summary>
        private void SetSelectItem(T_Trainee trainee)
        {
            // 通学種別の選択データ取得
            ViewBag.SelectAttendType = new SelectList(db.CodeMaster.Where(x => "01".Equals(x.Div)).ToList(), "Cd", "Value", trainee.AttendTypeCd);
            // 教習コースの選択データ取得
            ViewBag.SelectTraining = new SelectList(db.TrainingCourse.ToList(), "TrainingCourseCd", "TrainingCourseName", trainee.TrainingCourseCd);
            // 宿泊施設の選択データ取得
            ViewBag.SelectLodging = new SelectList(db.LodgingFacility.ToList(), "LodgingCd", "LodgingName", trainee.LodgingCd);
            // エージェントの選択データ取得
            ViewBag.SelectAgent = new SelectList(db.Agent.ToList(), "AgentCd", "AgentName", trainee.AgentCd);
        }

        /// <summary>
        /// データベース接続の破棄
        /// </summary>
        /// <param name="disposing">破棄有無</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}