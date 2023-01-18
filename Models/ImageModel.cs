using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ComplexProject.Models
{
    public class ImageModel
    {
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Title { get; set; }

        [Column(TypeName = "nvarchar(150)")]
        public string ImageName { get; set; }
        [NotMapped]
        [Display(Name = "Загрузить файл поддерживаемы форматы (jpg,jpeg)")]
        public IFormFile File { get; set; }
    }
}
