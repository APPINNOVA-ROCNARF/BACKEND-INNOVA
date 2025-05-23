﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Sistema
{
    public class Fuerza
    {
        public int Id { get; set; }            
        public string Codigo { get; set; }     
        public string Nombre { get; set; }       

        public ICollection<Seccion> Secciones { get; set; }
        public ICollection<GuiaProducto> GuiasProducto { get; set; } = new List<GuiaProducto>();
    }

}
