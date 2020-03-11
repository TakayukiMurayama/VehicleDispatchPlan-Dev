using System;

/**
 * 教習生モデル
 *
 * @author t-murayama
 * @version 1.0
 * ----------------------------------
 * 2020/03/01 t-murayama 新規作成
 *
 */
namespace VehicleDispatchPlan.Models
{
    public class Q_ResultAmt
    {
        /// <summary>教習生ID</summary>
        public int TraineeId { get; set; }

        /// <summary>入校予定日</summary>
        public DateTime EntrancePlanDate { get; set; }

        /// <summary>卒業予定日</summary>
        public DateTime GraduatePlanDate { get; set; }
    }
}