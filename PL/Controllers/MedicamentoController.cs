using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class MedicamentoController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

        public MedicamentoController(IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }
        public ActionResult GetAll()
        {
            ML.Medicamento medicamento = new ML.Medicamento();
            ML.Result result = new ML.Result();
            result.Objects = new List<object>();

            try
            {

                using (var client = new HttpClient())
                {
                    string urlApi = _configuration["urlApi"];
                    client.BaseAddress = new Uri(urlApi);

                    var responseTask = client.GetAsync("Medicamento/GetAll");
                    responseTask.Wait();

                    var resultServicio = responseTask.Result;

                    if (resultServicio.IsSuccessStatusCode)
                    {
                        var readTask = resultServicio.Content.ReadAsAsync<ML.Result>();
                        readTask.Wait();

                        foreach (var resultItem in readTask.Result.Objects)
                        {
                            ML.Medicamento resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Medicamento>(resultItem.ToString());
                            result.Objects.Add(resultItemList);
                        }
                    }
                    medicamento.Medicamentos = result.Objects;
                }

            }
            catch (Exception ex)
            {
            }

            return View(medicamento);
        }

        [HttpGet]
        //Formulario
        public ActionResult Form(int? IdMedicamento)
        {
            ML.Medicamento medicamento = new ML.Medicamento();
            if (IdMedicamento == null)
            {
                //Add Formulario vacio
                return View(medicamento);
            }
            else
            {
                //GetById
                ML.Result result = new ML.Result();

                using (var client = new HttpClient())
                {
                    try
                    {
                        client.BaseAddress = new Uri(_configuration["urlApi"]);
                        //GET
                        var responseTask = client.GetAsync("Medicamento/GetById/" + IdMedicamento);
                        responseTask.Wait();

                        var resultService = responseTask.Result;

                        if (resultService.IsSuccessStatusCode)
                        {
                            var readTask = resultService.Content.ReadAsAsync<ML.Result>();
                            readTask.Wait();


                            ML.Medicamento resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Medicamento>(readTask.Result.Object.ToString());
                            result.Object = resultItemList;

                            medicamento = (ML.Medicamento)result.Object;
                            return View(medicamento);
                        }
                        else
                        {
                            result.Correct = false;
                            result.ErrorMessage = "No se puedo hacer la consulta";
                        }
                    }
                    catch (Exception ex)
                    {
                        result.Correct = false;
                        result.ErrorMessage = ex.Message;
                    }
                    return View("Modal1");
                }
            }
        }
        [HttpPost]
        public ActionResult Form(ML.Medicamento medicamento)
        {
            IFormFile file = Request.Form.Files["fuImage"];
            if (file != null)
            {
                byte[] imagen = ConvertToBytes(file);
                medicamento.Imagen = Convert.ToBase64String(imagen);
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration["urlApi"]);
                if (medicamento.IdMedicamento == 0)
                {
                    //POST
                    var postTask = client.PostAsJsonAsync<ML.Medicamento>("Medicamento/Add", medicamento);
                    postTask.Wait();

                    var result = postTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        ViewBag.Message = "Se completo el registro satisfactoriamente";
                    }
                    else
                    {
                        ViewBag.Message = "Ocurrio un error al insertar el registro";
                    }
                    return View("Modal1");
                }
                else
                {
                    var postTask = client.PostAsJsonAsync<ML.Medicamento>("Medicamento/Update", medicamento);
                    postTask.Wait();

                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        ViewBag.Message = "Se actualizo el registro satisfactoriamente";
                    }
                    else
                    {
                        ViewBag.Message = "Ocurrio un error al actualizar el registro";
                    }
                    return View("Modal1");
                }
                //return View("Modal");
            }
        }

        [HttpGet]
        public ActionResult Delete(ML.Medicamento medicamento)
        {
            int IdMedicamento = medicamento.IdMedicamento;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration["urlApi"]);

                var postTask = client.PostAsJsonAsync<ML.Medicamento>("Usuario/Delete/" + IdMedicamento, medicamento);
                postTask.Wait();

                var result = postTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    ViewBag.Message = "Se elimino el registro satisfactoriamente";
                }
                else
                {
                    ViewBag.Message = "Ocurrio un error al eliminar el registro";
                }
                return View("Modal1");
            }
        }

        public static byte[] ConvertToBytes(IFormFile imagen)
        {
            using var fileStream = imagen.OpenReadStream();
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, (int)fileStream.Length);

            return bytes;
        }
    }
}
