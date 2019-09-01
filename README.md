# dfva_webcsharp

Este proyecto es una demostración de como integrar dfva_csharp en una aplicación web, permite demostrar cómo se realiza la firma.

Este proyecto fue desarrollado con asp.net 4.6 framework mvc y csharp usando mono en linux (puede que requiera ligeras modificaciones para correr en windows)

# Instalación

    pronto

# Archivos de interés 

Debido a que este es un proyecto de demostración y varios archivos no son necesarios para entender el procedimiento se describen cuales debe verificar

- `dfva_webcsharp/Controllers/DfvaController.cs` 

```
public ActionResult Sign(int id)   // Busca el archivo en disco y lo envía a firmar
public ActionResult SignCheck(int id) // verifica el estado de la transacción
```

- `dfva_webcsharp/Controllers/HomeController.cs` 

```
public ActionResult Index()  // Muestra el formulario de subida de archivos
public ActionResult ShowSignBnt(DocumentModel document) // Muestra el botón de firmado y la ventanda de proceso
public ActionResult Download(int id) // Descarga el documento ya firmado
```

- `dfva_webcsharp/App_Start/RouteConfig.cs`   Configura las rutas de respuesta del servidor
- `dfva_webcsharp/Content`  Archivos estáticos para archivos css e imágenes
- `dfva_webcsharp/Scripts/js`  Archívos de javascript para atender la aplicación

# Aspectos importantes 

Este proyecto solo pretende mostrar cómo usar dfva_java, por lo que no detalla algunas particulares de seguridad, por ejemplo es recomendable guardar variables de control en la sesión para prevenir que entes externos puedan consultar las transacciones. Además la información se almacena en disco y no en base de datos.
