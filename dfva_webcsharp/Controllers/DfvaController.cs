using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using dfva_csharp.dfva;
using Newtonsoft.Json;

namespace dfva_webcsharp.Controllers
{
    public class DfvaController : Controller
    {
        private Dictionary<string, string> getDocument(int id)
        {
            string temp = System.IO.Path.GetTempPath();
            string data;
            using (StreamReader inFile = new StreamReader(
                 Path.Combine(temp, id.ToString()
            )))
            {

                data = inFile.ReadToEnd();
            }
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(data);
        }


        public ActionResult Sign(int id)
        {
            Dictionary<string, String> doc = getDocument(id);
            Settings settings = new Settings();
            settings.load();
            var client = new Client(settings);
            string identification;
            string result;
            string format;
            string resumen;
            string razon;
            string place;
            //doc['resumen']
            doc.TryGetValue("file_uploaded", out result);
            doc.TryGetValue("doc_format", out format);
            doc.TryGetValue("identificacion", out identification);
            doc.TryGetValue("resumen", out resumen);
            doc.TryGetValue("razon", out razon);
            doc.TryGetValue("lugar", out place);

            Console.Write(razon + " ---- " + place);
            Dictionary<string, Object> response = client.sign(
               identification,
                Convert.FromBase64String(result), format,
                resumen, reason: razon, place: place);

            object code;
            object status_text;
            object status;
            object id_solicitud;
            response.TryGetValue("status", out status);
            response.TryGetValue("code", out code);
            response.TryGetValue("id_transaction", out id_solicitud);
            response.TryGetValue("status_text", out status_text);

            int statusint = Convert.ToInt32(status);
            bool success = statusint == 0;

            return Json(new
            {
                FueExitosaLaSolicitud = success,
                TiempoMaximoDeFirmaEnSegundos = 240,
                TiempoDeEsperaParaConsultarLaFirmaEnSegundos = 2,
                CodigoDeVerificacion = Convert.ToString(code),
                IdDeLaSolicitud = Convert.ToUInt32(id_solicitud),
                DebeMostrarElError = !success,
                DescripcionDelError = Convert.ToString(status_text),
                ResumenDelDocumento = Convert.ToString(resumen)
            });
        }
        public ActionResult SignCheck(int id)
        {
            Settings settings = new Settings();
            settings.load();
            var client = new Client(settings);
            string callback = Request.QueryString["callback"].ToString();
            string id_solicitud = Request.QueryString["IdDeLaSolicitud"].ToString();

            Dictionary<string, Object> response = client.sign_check(id_solicitud);

            object status_text;
            object statusobj;
            object received_notification;
            response.TryGetValue("status", out statusobj);
            response.TryGetValue("status_text", out status_text);
            response.TryGetValue("received_notification", out received_notification);
            int statusrev = Convert.ToInt32(statusobj);
            bool status = statusrev == 0;
            bool done = Convert.ToBoolean(received_notification);

            if(status && done) {
                save_signed_doc(id, response);
            }

            Dictionary<string, object> dev = new Dictionary<string, object>
            {
             { "DebeMostrarElError" ,    !status   },
             { "DescripcionDelError", Convert.ToString(status_text) },
             { "FueExitosa", status },
             { "SeRealizo", done}
            };
            String data = JsonConvert.SerializeObject(dev);
            return Content(callback + "(" + data + ")");
        }

        public void save_signed_doc(int id, Dictionary<string, Object> response)
        {

            Dictionary<string, string> doc = getDocument(id);

            object obj_sign_document;
            object obj_hash_docsigned;
            string sign_document;
            string hash_docsigned;

            response.TryGetValue("sign_document", out obj_sign_document);
            response.TryGetValue("hash_docsigned", out obj_hash_docsigned);

            sign_document = Convert.ToString(obj_sign_document);
            hash_docsigned = Convert.ToString(obj_hash_docsigned);
            doc.Add("sign_document", sign_document);
            doc.Add("hash_docsigned", hash_docsigned);

            String data = JsonConvert.SerializeObject(doc);
            string temp = System.IO.Path.GetTempPath();

            using (StreamWriter outputFile = new StreamWriter(
            Path.Combine(temp, id.ToString())))
            {
                outputFile.Write(data);
            }
        }
    }
}
