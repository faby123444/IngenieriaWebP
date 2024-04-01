using System;
using System.Collections.Generic;

namespace IngenieriaWebP.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public string? Apellido { get; set; }

    public string? Cedula { get; set; }

    public string? Celular { get; set; }

    public string? Correo { get; set; }

    public string? Contrasenia { get; set; }

    public int? Edad { get; set; }
}
