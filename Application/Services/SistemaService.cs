using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTO.SistemaDTO;
using Application.Interfaces.ISistema;

namespace Application.Services
{
    public class SistemaService : ISistemaService
    {
        private readonly ISistemaRepository _repository;

        public SistemaService(ISistemaRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<CicloSelectDTO>> ObtenerCiclosSelectAsync()
        {
            return await _repository.ObtenerCiclosSelectAsync();
        }
    }
}
