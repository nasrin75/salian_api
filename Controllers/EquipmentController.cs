using Microsoft.AspNetCore.Mvc;
using salian_api.Models;

namespace salian_api.Controllers
{
    public class EquipmentController : Controller
    {
        public IActionResult List()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Equipment equipment)
        {
            if (ModelState.IsValid)
            {
                ViewBag.message = "قطعه موردنظر با موفقیت افزوده شد.";
                ViewBag.alertColor = "success";
                return RedirectToAction("List");
            }

            ViewBag.message = "اطلاعات وارد شده صحیح نمی باشد.";
            ViewBag.alertColor = "danger";
            return View(equipment);
        }
    }
}
