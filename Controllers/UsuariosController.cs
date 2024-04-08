using System; // Importa el espacio de nombres System, que contiene clases fundamentales y clases base que definen tipos de datos de valor y referencia comúnmente utilizados, eventos y manejadores de eventos.
using System.Collections.Generic; // Proporciona interfaces y clases que definen colecciones genéricas, lo que permite a los usuarios crear colecciones fuertemente tipadas que ofrecen mejor seguridad de tipo y rendimiento que las colecciones fuertemente tipadas no genéricas.
using System.Linq; // Importa el espacio de nombres System.Linq, que proporciona clases e interfaces que admiten consultas que utilizan Language-Integrated Query (LINQ).
using System.Threading.Tasks; // Proporciona tipos que simplifican el trabajo de escribir código concurrente y asíncrono. Los principales tipos son Task, que representa una operación asíncrona que puede devolver un valor, y Task<T>, que es una tarea que puede devolver un resultado.
using Microsoft.AspNetCore.Mvc; // Proporciona clases y atributos que son necesarios para crear controladores y acciones en ASP.NET Core MVC.
using Microsoft.AspNetCore.Mvc.Rendering; // Proporciona soporte para renderizar controles HTML en las vistas.
using Microsoft.EntityFrameworkCore; // Importa el espacio de nombres Microsoft.EntityFrameworkCore, proporcionando herramientas para trabajar con bases de datos usando Entity Framework Core.
using IngenieriaWebP.Models;
using IngenieriaWebP.Filters; // Importa el espacio de nombres de tu proyecto donde se definen los modelos, en este caso, es probable que contenga la definición del modelo `Usuario` y el contexto de la base de datos `IngenieriaWebContext`.

namespace IngenieriaWebP.Controllers // Define el espacio de nombres para los controladores de tu aplicación. Esto ayuda a organizar el código y permite el uso de clases con el mismo nombre en diferentes espacios de nombres.
{
    public class UsuariosController : Controller // Declara la clase `UsuariosController`, que hereda de la clase `Controller` de ASP.NET Core MVC, proporcionando funcionalidades para crear acciones de controlador.
    {
        private readonly IngenieriaWebContext _context; // Declara un campo privado de solo lectura `_context` de tipo `IngenieriaWebContext`. Este campo se utilizará para interactuar con la base de datos.

        public UsuariosController(IngenieriaWebContext context) // Constructor de la clase que toma una instancia de `IngenieriaWebContext` como argumento. Esto permite la inyección de dependencias, una práctica de diseño de software que reduce el acoplamiento entre clases.
        {
            _context = context; // Asigna el contexto de la base de datos proporcionado al campo `_context`. Esto inicializa el campo con el contexto de la base de datos que se utilizará en las operaciones de la base de datos.
        }

        [RequireLogin]
        // GET: Usuarios
        public async Task<IActionResult> Index() // Define una acción asincrónica `Index` que maneja las solicitudes GET para la ruta "/Usuarios". Esta acción devuelve una vista que muestra una lista de usuarios.
        {
            return View(await _context.Usuarios.ToListAsync()); // Utiliza el contexto de la base de datos para obtener una lista asincrónica de todos los usuarios en la base de datos y pasa esta lista a la vista.
        }

        [RequireLogin]
        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id) // Define una acción asincrónica `Details` que maneja las solicitudes GET para la ruta "/Usuarios/Details/{id}". Esta acción devuelve una vista que muestra los detalles de un usuario específico.
        {
            if (id == null) // Verifica si el parámetro `id` es nulo, lo cual indicaría una solicitud no válida.
            {
                return NotFound(); // Si `id` es nulo, devuelve un resultado que indica que el recurso solicitado no se encontró.
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == id); // Intenta encontrar el primer usuario en la base de datos cuyo `Id` coincida con el `id` proporcionado. Si no se encuentra ningún usuario, `usuario` será nulo.
            if (usuario == null) // Verifica si se encontró un usuario.
            {
                return NotFound(); // Si `usuario` es nulo, significa que no se encontró un usuario con el `id` proporcionado y devuelve un resultado que indica que el recurso solicitado no se encontró.
            }

            return View(usuario); // Si se encuentra un usuario, pasa el usuario encontrado a la vista para mostrar sus detalles.
        }

        [RequireLogin]
        // GET: Usuarios/Create
        public IActionResult Create() // Define una acción `Create` que maneja las solicitudes GET para la ruta "/Usuarios/Create". Esta acción devuelve una vista que contiene un formulario para crear un nuevo usuario.
        {
            return View(); // Devuelve la vista para crear un nuevo usuario. Como no se pasa ningún modelo a la vista, se utilizará un modelo vacío para el formulario.
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [RequireLogin]
        [HttpPost] // Especifica que esta acción solo maneja solicitudes POST, que son enviadas por el formulario de creación de usuarios.
        [ValidateAntiForgeryToken] // Activa la validación de tokens anti-falsificación, una medida de seguridad para prevenir ataques de falsificación de solicitud en sitios cruzados (CSRF).
        public async Task<IActionResult> Create([Bind("Id,Nombre,Apellido,Cedula,Celular,Correo,Contrasenia,Edad")] Usuario usuario) // Define una acción asincrónica `Create` que toma un modelo `Usuario` como parámetro, donde solo ciertas propiedades están habilitadas para la vinculación de modelo debido al atributo `[Bind]`. Esto previene la asignación masiva no deseada.
        {
            if (ModelState.IsValid) // Verifica si el modelo `usuario` es válido, lo que significa que pasa todas las validaciones de datos definidas en el modelo.
            {
                _context.Add(usuario); // Agrega el usuario nuevo al contexto de la base de datos, preparándolo para ser guardado en la base de datos.
                await _context.SaveChangesAsync(); // Guarda todos los cambios en el contexto de la base de datos de forma asincrónica, incluyendo el nuevo usuario, en la base de datos.
                return RedirectToAction(nameof(Index)); // Redirige al usuario a la acción `Index`, que muestra la lista de usuarios, después de crear el usuario con éxito.
            }
            return View(usuario); // Si el modelo `usuario` no es válido, vuelve a mostrar la vista con el modelo `usuario` proporcionado para que el usuario pueda corregir los errores.
        }

        [RequireLogin]
        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id) // Define una acción asincrónica `Edit` que maneja las solicitudes GET para la ruta "/Usuarios/Edit/{id}". Esta acción devuelve una vista que contiene un formulario para editar un usuario existente.
        {
            if (id == null) // Verifica si el parámetro `id` es nulo.
            {
                return NotFound(); // Si `id` es nulo, devuelve un resultado que indica que el recurso solicitado no se encontró.
            }

            var usuario = await _context.Usuarios.FindAsync(id); // Busca asincrónicamente un usuario por su `Id` en el contexto de la base de datos. Si no se encuentra, `usuario` será nulo.
            if (usuario == null) // Verifica si se encontró un usuario.
            {
                return NotFound(); // Si `usuario` es nulo, significa que no se encontró un usuario con el `id` proporcionado y devuelve un resultado que indica que el recurso solicitado no se encontró.
            }
            return View(usuario); // Si se encuentra un usuario, pasa el usuario a la vista para mostrar el formulario de edición con los datos del usuario.
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [RequireLogin]
        [HttpPost] // Especifica que esta acción solo maneja solicitudes POST, que son enviadas por el formulario de edición de usuarios.
        [ValidateAntiForgeryToken] // Activa la validación de tokens anti-falsificación para prevenir ataques CSRF.
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Apellido,Cedula,Celular,Correo,Contrasenia,Edad")] Usuario usuario) // Define una acción asincrónica `Edit` para actualizar un usuario existente. Utiliza el atributo `[Bind]` para especificar las propiedades del modelo `Usuario` que se deben vincular.
        {
            if (id != usuario.Id) // Verifica si el `id` proporcionado coincide con el `Id` del modelo `usuario`. Esto es una medida de seguridad para asegurar que se está editando el usuario correcto.
            {
                return NotFound(); // Si los `id` no coinciden, devuelve un resultado que indica que el recurso solicitado no se encontró.
            }

            if (ModelState.IsValid) // Verifica si el modelo `usuario` es válido.
            {
                try
                {
                    _context.Update(usuario); // Actualiza el usuario en el contexto de la base de datos con los valores modificados.
                    await _context.SaveChangesAsync(); // Guarda todos los cambios en el contexto de la base de datos de forma asincrónica.
                }
                catch (DbUpdateConcurrencyException) // Captura excepciones que pueden ocurrir si hay un conflicto de concurrencia, es decir, si otro usuario ha modificado el usuario al mismo tiempo.
                {
                    if (!UsuarioExists(usuario.Id)) // Verifica si el usuario todavía existe en la base de datos.
                    {
                        return NotFound(); // Si el usuario no existe, devuelve un resultado que indica que el recurso solicitado no se encontró.
                    }
                    else
                    {
                        throw; // Si el usuario existe pero aún así hay un conflicto de concurrencia, relanza la excepción para manejarla en otro lugar.
                    }
                }
                return RedirectToAction(nameof(Index)); // Redirige al usuario a la acción `Index` después de actualizar el usuario con éxito.
            }
            return View(usuario); // Si el modelo `usuario` no es válido, vuelve a mostrar la vista con el modelo `usuario` para que el usuario pueda corregir los errores.
        }

        // GET: Usuarios/Delete/5
        [RequireLogin]
        public async Task<IActionResult> Delete(int? id) // Define una acción asincrónica `Delete` que maneja las solicitudes GET para la ruta "/Usuarios/Delete/{id}". Esta acción devuelve una vista que permite a los usuarios confirmar la eliminación de un usuario.
        {
            if (id == null) // Verifica si el parámetro `id` es nulo.
            {
                return NotFound(); // Si `id` es nulo, devuelve un resultado que indica que el recurso solicitado no se encontró.
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == id); // Intenta encontrar el primer usuario en la base de datos cuyo `Id` coincida con el `id` proporcionado. Si no se encuentra ningún usuario, `usuario` será nulo.
            if (usuario == null) // Verifica si se encontró un usuario.
            {
                return NotFound(); // Si `usuario` es nulo, significa que no se encontró un usuario con el `id` proporcionado y devuelve un resultado que indica que el recurso solicitado no se encontró.
            }

            return View(usuario); // Si se encuentra un usuario, pasa el usuario a la vista para mostrar la confirmación de eliminación.
        }

        // POST: Usuarios/Delete/5
        [RequireLogin]
        [HttpPost, ActionName("Delete")] // Especifica que esta acción maneja solicitudes POST para la acción `Delete`, permitiendo una sobrecarga del método `Delete` con diferentes firmas.
        [ValidateAntiForgeryToken] // Activa la validación de tokens anti-falsificación para prevenir ataques CSRF.
        public async Task<IActionResult> DeleteConfirmed(int id) // Define una acción asincrónica `DeleteConfirmed` que realmente elimina un usuario de la base de datos. Esta es la acción que se invoca cuando un usuario confirma la eliminación en la vista.
        {
            var usuario = await _context.Usuarios.FindAsync(id); // Busca asincrónicamente el usuario por `Id` en el contexto de la base de datos.
            if (usuario != null) // Verifica si se encontró el usuario.
            {
                _context.Usuarios.Remove(usuario); // Elimina el usuario del contexto de la base de datos.
            }

            await _context.SaveChangesAsync(); // Guarda todos los cambios en el contexto de la base de datos de forma asincrónica, incluida la eliminación del usuario.
            return RedirectToAction(nameof(Index)); // Redirige al usuario a la acción `Index` después de eliminar el usuario con éxito.
        }

        private bool UsuarioExists(int id) // Define un método privado `UsuarioExists` que verifica si un usuario existe en la base de datos buscando por `Id`.
        {
            return _context.Usuarios.Any(e => e.Id == id); // Utiliza LINQ para consultar el contexto de la base de datos y verificar si algún usuario tiene un `Id` que coincida con el proporcionado.
        }

        // GET: Usuarios/Login
        
        public IActionResult Login()
        {
            return View();
        }

        // POST: Usuarios/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string correo, string contrasenia)
        {
            if (string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(contrasenia))
            {
                // Manejar error de que el correo o contraseña no pueden estar vacíos.
                ViewBag.ErrorMessage = "El correo y la contraseña son requeridos.";
                return View();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Correo == correo && u.Contrasenia == contrasenia);

            if (usuario == null)
            {
                // Manejar el error de credenciales incorrectas.
                ViewBag.ErrorMessage = "Correo o contraseña incorrectos.";
                return View();
            }

            // Aquí se maneja la lógica de sesión
            HttpContext.Session.SetString("IsLoggedIn", "true"); // Esto es lo que parece estar faltando en tu implementación

            return RedirectToAction("Index"); // Redirigir a la página principal o dashboard tras un inicio de sesión exitoso.
        }


    }
}
