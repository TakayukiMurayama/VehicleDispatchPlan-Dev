using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VehicleDispatchPlan.Models
{
    [Table("M_CodeMaster")]
    public class M_CodeMaster
    {
        // 区分
        [Key]
        [Column(Order = 1)]
        public string Div { get; set; }

        // コード
        [Key]
        [Column(Order = 2)]
        public string Cd { get; set; }

        // 値
        public string Value { get; set; }
    }
}