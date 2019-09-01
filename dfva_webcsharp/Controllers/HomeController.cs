using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using dfva_webcsharp.Models;
using Newtonsoft.Json;

namespace dfva_webcsharp.Controllers
{
    public class HomeController : Controller
    {

        private string filename;
        public ActionResult Index()
        {
            var mvcName = typeof(Controller).Assembly.GetName();
            var isMono = Type.GetType("Mono.Runtime") != null;

            ViewData["Version"] = mvcName.Version.Major + "." + mvcName.Version.Minor;
            ViewData["Runtime"] = isMono ? "Mono" : ".NET";

            return View();
        }
        [HttpPost]
        public ActionResult ShowSignBnt(DocumentModel document)
        {
            string file_name = Request.Files.Get(0).FileName;
            System.IO.Stream file = Request.Files.Get(0).InputStream;

            byte[] buffer = new byte[file.Length];
            file.Read(buffer, 0, (int)file.Length);
            ViewData["document"] = document;
            ViewData["document_hash"] = document.GetHashCode();
            ViewData["document_resumen"] = document.resumen;
            save_document(document, Convert.ToBase64String(buffer), file_name);

            return View();
        }

        public ActionResult Download(int id) {
            byte[] doc = getDocumentSigned(id);
            return File(doc, System.Net.Mime.MediaTypeNames.Application.Octet,
                filename);
        }

        public void save_document(DocumentModel document, string b64doc, string filename)
        {
            Dictionary<string, object> dev = new Dictionary<string, object>
            {
                {"file_uploaded", b64doc},
                {"file_name", filename},
                {"identificacion", document.identificacion},
                {"resumen", document.resumen},
                {"doc_format", document.doc_format},
                {"razon", document.razon},
                {"lugar", document.lugar},

                {"signed", false}
            };
            String data = JsonConvert.SerializeObject(dev);
            string temp = System.IO.Path.GetTempPath();

            using (StreamWriter outputFile = new StreamWriter(
            Path.Combine(temp, document.GetHashCode().ToString()
            )))
            {

                outputFile.Write(data);
            }
        }


        private byte[] getDocumentSigned(int id)
        {
            string temp = System.IO.Path.GetTempPath();
            string data;
            using (StreamReader inFile = new StreamReader(
                 Path.Combine(temp, id.ToString()
            )))
            {

                data = inFile.ReadToEnd();
            }
            Dictionary<string, string> readdata =Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(data);
            string signed_doc;
            readdata.TryGetValue("sign_document", out signed_doc);
            readdata.TryGetValue("file_name", out filename);
            return Convert.FromBase64String(signed_doc);
        }


    }
}
