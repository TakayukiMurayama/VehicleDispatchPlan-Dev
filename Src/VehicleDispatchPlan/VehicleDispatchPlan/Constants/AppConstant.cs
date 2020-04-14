using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VehicleDispatchPlan.Constants
{
    public class AppConstant
    {
        /// <summary>区分_通学種別</summary>
        public const string DIV_ATTEND_TYPE = "01";
        /// <summary>ｺｰﾄﾞ_合宿</summary>
        public const string CD_ATTEND_TYPE_LODGING = "01";
        /// <summary>ｺｰﾄﾞ_通い</summary>
        public const string CD_ATTEND_TYPE_COMMUTING = "02";

        /// <summary>区分_期間</summary>
        public const string DIV_PERIOD = "02";
        /// <summary>ｺｰﾄﾞ_合宿</summary>
        public const string CD_PERIOD_FIRST_HALF = "01";
        /// <summary>ｺｰﾄﾞ_通い</summary>
        public const string CD_PERIOD_SECOND_HALF = "02";

        /// <summary>系統_受入可能人数</summary>
        public const string SERIES_FORECAST = "受入可能人数";
        /// <summary>系統_合宿実績人数</summary>
        public const string SERIES_LODGING = "合宿実績人数";
        /// <summary>系統_通学実績人数</summary>
        public const string SERIES_COMMUTING = "通学実績人数";
    }
}