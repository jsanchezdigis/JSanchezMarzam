using Microsoft.EntityFrameworkCore;

namespace BL
{
    public class Medicamento
    {
        public static ML.Result Add(ML.Medicamento medicamento)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.JsanchezMarzamContext context = new DL.JsanchezMarzamContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"MedicamentoAdd '{medicamento.Nombre}','{medicamento.Precio}','{medicamento.Imagen}'");
                    if (query >= 1)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Error al insertar los datos";
                    }
                    result.Correct = true;
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }
        public static ML.Result Update(ML.Medicamento medicamento)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.JsanchezMarzamContext context = new DL.JsanchezMarzamContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"MedicamentoUpdate '{medicamento.IdMedicamento}','{medicamento.Nombre}','{medicamento.Precio}','{medicamento.Imagen}'");
                    if (query >= 1)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Error al actualizar los datos";
                    }
                    result.Correct = true;
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }
        public static ML.Result Delete(int IdMedicamento)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.JsanchezMarzamContext context = new DL.JsanchezMarzamContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"MedicamentoDelete '{IdMedicamento}'");
                    if (query >= 1)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Error al eliminar los datos";
                    }
                    result.Correct = true;
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.JsanchezMarzamContext context = new DL.JsanchezMarzamContext())
                {
                    var query = context.Medicamentos.FromSqlRaw($"MedicamentoGetAll").ToList();
                    if (query != null)
                    {
                        result.Objects = new List<object>();
                        foreach (var obj in query)
                        {
                            ML.Medicamento medicamento = new ML.Medicamento();
                            medicamento.IdMedicamento = obj.IdMedicamento;
                            medicamento.Nombre = obj.Nombre;
                            medicamento.Precio = obj.Precio.Value;
                            medicamento.Imagen = obj.Imagen;
                            result.Objects.Add(medicamento);
                        }
                    }
                    result.Correct = true;
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }
        public static ML.Result GetById(int IdMedicamento)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.JsanchezMarzamContext context = new DL.JsanchezMarzamContext())
                {
                    var query = context.Medicamentos.FromSqlRaw($"MedicamentoGetById '{IdMedicamento}'").AsEnumerable().FirstOrDefault();
                    if (query != null)
                    {
                        result.Object = new List<object>();
                        var obj = query;
                        ML.Medicamento medicamento = new ML.Medicamento();
                        medicamento.IdMedicamento = obj.IdMedicamento;
                        medicamento.Nombre = obj.Nombre;
                        medicamento.Precio = obj.Precio.Value;
                        medicamento.Imagen = obj.Imagen;
                        result.Object = medicamento;
                    }
                    result.Correct = true;
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }
    }
}