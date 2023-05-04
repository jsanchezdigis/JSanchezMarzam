using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class UsuarioController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string UserName, string Password)
        {
            ML.Result result = BL.Usuario.GetByUserName(UserName);
            if (result.Correct)
            {
                ML.Usuario usuario = (ML.Usuario)result.Object;
                if (Password == usuario.Password)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Message = "Contraseña incorrecta";
                    return PartialView("ModalLogin");
                }
            }
            else
            {
                ViewBag.Message = "Contraseña incorrecta";
                return PartialView("ModalLogin");
            }
        }
    }
}
