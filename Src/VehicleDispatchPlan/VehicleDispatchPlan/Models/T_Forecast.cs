using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VehicleDispatchPlan.Models
{
    public class T_Forecast
    {
        // 対象年
        [Key]
        [Column(Order = 1)]
        public string Year { get; set; }

        // 期間
        [Key]
        [Column(Order = 2)]
        public string Period { get; set; }

        // 指導員数
        [DisplayName("指導員数")]
        public int InstructorAmt { get; set; }

        // 時限数
        [DisplayName("学科時限数")]
        public int ClassQty { get; set; }

        // 合宿比率
        [Range(0,100)]
        [DisplayName("合宿比率")]
        public double LodgingRatio { get; set; }

        // 教習外業務比率
        [Range(0, 100)]
        [DisplayName("教習外業務比率")]
        public double NotDrivingRatio { get; set; }
    }
}