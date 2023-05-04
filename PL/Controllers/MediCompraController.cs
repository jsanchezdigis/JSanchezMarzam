using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class MediCompraController : Controller
    {
        public ActionResult GetCards()
        {
            ML.Medicamento medicamento = new ML.Medicamento();
            ML.Result result = BL.Medicamento.GetAll();
            if (result.Correct)
            {
                medicamento.Medicamentos = result.Objects;
                return View(medicamento);
            }
            else
            {
                return View(medicamento);
            }
        }
        [HttpGet]
        public ActionResult Resumen(ML.CompraMedicamento compraMedicamento)
        {
            decimal costoTotal = 0;
            if (HttpContext.Session.GetString("Medicamento") == null)
            {
                return View();
            }
            else
            {
                compraMedicamento.Compras = new List<object>();
                GetCompra(compraMedicamento);
                compraMedicamento.Total = costoTotal;
            }
            return View(compraMedicamento);
        }

        [HttpGet]
        public ActionResult Compra(ML.Medicamento medicamento)
        {
            int IdMed = medicamento.IdMedicamento;
            ML.Result resultImg = BL.Medicamento.GetById(IdMed);
            ML.Medicamento med2 = new ML.Medicamento();
            med2 = (ML.Medicamento)resultImg.Object;
            medicamento.Imagen = med2.Imagen;
            //----------------------------------------------------------

            bool existe = false;
            ML.CompraMedicamento compraMedicamento = new ML.CompraMedicamento();
            compraMedicamento.Compras = new List<object>();

            if (HttpContext.Session.GetString("Medicamento") == null)
            {
                medicamento.Cantidad = medicamento.Cantidad = 1;
                medicamento.Imagen = medicamento.Imagen;
                medicamento.SubTotal = medicamento.Precio * medicamento.Cantidad;
                compraMedicamento.Compras.Add(medicamento);

                HttpContext.Session.SetString("Medicamento", Newtonsoft.Json.JsonConvert.SerializeObject(compraMedicamento.Compras));
                var session = HttpContext.Session.GetString("Medicamento");
            }
            else
            {
                GetCompra(compraMedicamento);
                foreach (ML.Medicamento venta in compraMedicamento.Compras.ToList())
                {
                    if (medicamento.IdMedicamento == venta.IdMedicamento)
                    {
                        venta.Cantidad = venta.Cantidad + 1;
                        venta.Imagen = venta.Imagen;
                        venta.SubTotal = venta.Precio * venta.Cantidad;
                        existe = true;
                    }
                    else
                    {
                        existe = false;
                    }

                    if (existe == true)
                    {
                        break;
                    }
                }
                if (existe == false)
                {
                    medicamento.Cantidad = medicamento.Cantidad = 1;
                    medicamento.Imagen = medicamento.Imagen;
                    medicamento.SubTotal = medicamento.Precio * medicamento.Cantidad;
                    compraMedicamento.Compras.Add(medicamento);
                }
                HttpContext.Session.SetString("Medicamento", Newtonsoft.Json.JsonConvert.SerializeObject(compraMedicamento.Compras));
            }
            if (HttpContext.Session.GetString("Medicamento") == null)
            {
                ViewBag.Message = "Error al intentar agregar el medicamento al carrito";
                return PartialView("Modal");
            }
            else
            {
                ViewBag.Message = "Medicamento agregado al carrito";
                return PartialView("Modal");
            }
        }

        public ML.CompraMedicamento GetCompra(ML.CompraMedicamento compraMedicamento)
        {
            var ventaSession = Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>>(HttpContext.Session.GetString("Medicamento"));

            foreach (var obj in ventaSession)
            {
                ML.Medicamento objMedicamento = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Medicamento>(obj.ToString());
                compraMedicamento.Compras.Add(objMedicamento);
            }
            return compraMedicamento;
        }
        [HttpGet]
        public ActionResult Add(int IdMedicamento)
        {
            bool existe = false;
            ML.CompraMedicamento compraMedicamento = new ML.CompraMedicamento();
            compraMedicamento.Compras = new List<object>();
            if (HttpContext.Session.GetString("Medicamento") == null)
            {
                return View();
            }
            GetCompra(compraMedicamento);
            foreach (ML.Medicamento medicamento in compraMedicamento.Compras.ToList())
            {
                if (IdMedicamento == medicamento.IdMedicamento)
                {
                    medicamento.Cantidad = medicamento.Cantidad + 1;
                    medicamento.Imagen = medicamento.Imagen;
                    medicamento.SubTotal = medicamento.Precio * medicamento.Cantidad;
                    existe = true;
                }
                else
                {
                    existe = false;
                }
            }
            HttpContext.Session.SetString("Medicamento", Newtonsoft.Json.JsonConvert.SerializeObject(compraMedicamento.Compras));
            //ViewBag.Message = "Medicamento agregado";
            //return PartialView("Modal");
            return RedirectToAction("Resumen", "MediCompra");
        }
        [HttpGet]
        public ActionResult Delete(int IdMedicamento)
        {
            bool existe = false;
            ML.CompraMedicamento compraMedicamento = new ML.CompraMedicamento();
            compraMedicamento.Compras = new List<object>();
            if (HttpContext.Session.GetString("Medicamento") == null)
            {
                return View();
            }
            GetCompra(compraMedicamento);
            foreach (ML.Medicamento medicamento in compraMedicamento.Compras.ToList())
            {
                if (IdMedicamento == medicamento.IdMedicamento)
                {
                    medicamento.Cantidad = medicamento.Cantidad - 1;
                    medicamento.Imagen = medicamento.Imagen;
                    medicamento.SubTotal = medicamento.Precio * medicamento.Cantidad;
                    existe = true;
                    if (medicamento.Cantidad == 0)
                    {
                        compraMedicamento.Compras.Remove(medicamento);
                    }
                }
                else
                {
                    existe = false;
                }
            }
            HttpContext.Session.SetString("Medicamento", Newtonsoft.Json.JsonConvert.SerializeObject(compraMedicamento.Compras));
            //ViewBag.Message = "Medicamento se retiro";
            //return PartialView("Modal");
            return RedirectToAction("Resumen","MediCompra");
        }
        public ActionResult Formulario()
        {
            return PartialView("FormularioPago");
        }
        public ActionResult Pagar()
        {
            HttpContext.Session.Clear();
            return PartialView("Pago");
        }
    }
}
