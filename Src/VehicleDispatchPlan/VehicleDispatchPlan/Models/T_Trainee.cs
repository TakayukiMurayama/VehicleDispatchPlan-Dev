using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    /// <summary>
    /// 教習生モデル
    /// </summary>
    [Table("T_Trainee")]
    public class T_Trainee
    {
        /// <summary>教習生ID<summary>
        [Key]
        [DisplayName("ID")]
        public int TraineeId { get; set; }

        /// <summary>名前<summary>
        [Required]
        [DisplayName("教習者名")]
        public string TraineeName { get; set; }

        /// <summary>通学種別<summary>
        [Required]
        [DisplayName("通学種別")]
        public string AttendTypeCd { get; set; }

        /// <summary>教習コース<summary>
        [Required]
        [DisplayName("教習コース")]
        public string TrainingCourseCd { get; set;}

        /// <summary>入校予定日<summary>
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("入校予定日")]
        public DateTime EntrancePlanDate { get; set; }

        /// <summary>宿泊施設<summary>
        [DisplayName("宿泊施設")]
        public string LodgingCd { get; set; }

        /// <summary>紹介エージェント<summary>
        [DisplayName("エージェント")]
        public string AgentCd { get; set; }
    }
}