using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/**
 * 教習コースモデル
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
    /// 教習コースモデル
    /// </summary>
    [Table("M_TrainingCourse")]
    public class M_TrainingCourse
    {
        // 教習コースコード
        [Key]
        public string TrainingCourseCd { get; set; }

        // 教習コース名
        [Required]
        public string TrainingCourseName { get; set; }

        // 最短卒業日数
        [Required]
        public int GraduationDays { get; set; }
    }
}