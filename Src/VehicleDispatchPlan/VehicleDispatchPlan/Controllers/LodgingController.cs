using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

/**
 * 宿泊管理コントローラ
 *
 * @author t-murayama
 * @version 1.0
 * ----------------------------------
 * 2020/03/01 t-murayama 新規作成
 *
 */
namespace VehicleDispatchPlan.Controllers
{
    public class LodgingController : Controller
    {
        // GET: Lodging
        public ActionResult List()
        {
            return View();
        }
    }
}