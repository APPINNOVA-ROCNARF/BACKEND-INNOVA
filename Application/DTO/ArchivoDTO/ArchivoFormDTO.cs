using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Application.DTO.ArchivoDTO
{
    public class ArchivoFormDTO
    {
        [Required]
        public IFormFile File { get; set; } = null!;
    }
}
