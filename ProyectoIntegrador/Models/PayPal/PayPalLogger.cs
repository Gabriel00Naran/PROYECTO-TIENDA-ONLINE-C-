using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace ProyectoIntegrador.Models
{
    public class PayPalLogger
    {

        // pay pay mensajes de error para verificar que estre realizando la transacion
        public static string LogDirectoryPath = Environment.CurrentDirectory;
        public static void Log(String messages)
        {
            try
            {
                StreamWriter strw = new StreamWriter(LogDirectoryPath+"\\PaypalError.log",true);
                strw.WriteLine("{0}--->{1}", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"), messages);
            }
            catch (Exception) 
            {
                throw;
            }
        }
    }
}