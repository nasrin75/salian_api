using Microsoft.AspNetCore.Mvc;
using salian_api.Models;

namespace salian_api.Controllers
{
    public class UserController : Controller
    {
        public IActionResult List()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            var user = new User();
            return View(user);
        }

        [HttpPost]
        public IActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                ViewBag.message = "کاربر با موفقیت افزوده شد. ";
                ViewBag.alertColor = "success";
                return View(user);
            }

            ViewBag.message = "اطلاعات وارد شده صحیح نمی باشد.";
            ViewBag.alertColor = "danger";
            return View(user);

        }
    }
}
