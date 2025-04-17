using Application.DTO.ViaticoDTO;
using Application.Interfaces.IViatico;
using Domain.Entities.Viaticos;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ViaticoRepository : IViaticoRepository
    {
        private readonly ViaticosDbContext _context;

        public ViaticoRepository(ViaticosDbContext context)
        {
            _context = context;
        }

        public async Task<int> CrearViaticoAsync(CrearViaticoDTO dto)
        {
            // Validar que no exista una factura con el mismo número y RUC
            bool facturaDuplicada = await _context.FacturasViatico.AnyAsync(f =>
                f.NumeroFactura == dto.Factura.NumeroFactura &&
                f.RucProveedor == dto.Factura.RucProveedor);

            if (facturaDuplicada)
            {
                throw new InvalidOperationException("Ya existe una factura con ese número y RUC.");
            }

            dto.Viatico.Factura = dto.Factura;

            dto.Viatico.Monto = dto.Monto;

            _context.Viaticos.Add(dto.Viatico);
            await _context.SaveChangesAsync();

            return dto.Viatico.Id;
        }
    }
}
