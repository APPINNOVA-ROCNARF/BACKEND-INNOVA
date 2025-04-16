using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers
{
    public static class RutaArchivoHelper
    {
        public static string GenerarRutaRelativaFacturaViatico(
            int usuarioAppId,
            int cicloId,
            string nombreArchivo)
        {
            return Path.Combine(
                "uploads", "viaticos", "facturas",
                usuarioAppId.ToString(),
                cicloId.ToString(),
                nombreArchivo
            ).Replace("\\", "/");
        }

        public static string GenerarRutaCompleta(string webRootPath, string rutaRelativa)
        {
            return Path.Combine(webRootPath, rutaRelativa);
        }

        public static string GenerarNombreUnico(string nombreOriginal)
        {
            var extension = Path.GetExtension(nombreOriginal);
            var nombreUnico = Guid.NewGuid().ToString();
            return nombreUnico + extension;
        }
    }

}
