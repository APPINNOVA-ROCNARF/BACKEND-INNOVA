using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Options
{
    public class ArchivosOptions
    {
        public string RutaBase { get; set; } = "uploads";
        public int MaximoMB { get; set; } = 5;
        public List<string> ExtensionesPermitidas { get; set; } = new();
    }
}
