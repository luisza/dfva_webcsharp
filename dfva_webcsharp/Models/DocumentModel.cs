using System;
using System.Web;

namespace dfva_webcsharp.Models
{
    public class DocumentModel
    {
        public HttpPostedFile file_uploaded { get; set; }
        public string file_signed { get; set; }
        public string file_signed_hash { get; set; }
        public string file_name { get; set; }
        public string identificacion { get; set; }

        public string resumen { get; set; }
        public string doc_format { get; set; }
        public string razon { get; set; }
        public string lugar { get; set; }
        public int id_transaction { get; set; }
        public bool signed { get; set; }
    }
}
