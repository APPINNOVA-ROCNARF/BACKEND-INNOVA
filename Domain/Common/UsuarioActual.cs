﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    public class UsuarioActual
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public string Rol { get; set; } = "";
    }
}
