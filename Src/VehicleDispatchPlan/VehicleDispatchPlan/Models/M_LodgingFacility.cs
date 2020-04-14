using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

/**
 * 宿泊施設モデル
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
    /// 宿泊施設モデル
    /// </summary>
    [Table("M_LodgingFacility")]
    public class M_LodgingFacility
    {
        // 宿泊施設コード
        [Key]
        public string LodgingCd { get; set; }

        // 宿泊施設名
        [Required]
        public string LodgingName { get; set; }

        // 郵便番号
        public string PostalNo { get; set; }

        // 住所
        public string Address { get; set; }
    }
}