using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Usuario
    {
        public static ML.Result GetByUserName(string UserName)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.JsanchezMarzamContext context = new DL.JsanchezMarzamContext())
                {
                    var query = context.Usuarios.FromSqlRaw($"UsuarioGetByUserName '{UserName}'").AsEnumerable().FirstOrDefault();
                    if (query != null)
                    {
                        result.Object = new List<object>();
                        var obj = query;

                        ML.Usuario usuario = new ML.Usuario();
                        usuario.UserName = obj.UserName;
                        usuario.Password = obj.Password;
                        result.Object = usuario;
                    }
                }
                result.Correct = true;
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
