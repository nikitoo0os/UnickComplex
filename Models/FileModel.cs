using System.ComponentModel.DataAnnotations.Schema;

namespace ComplexProject.Models
{
    public class FileModel
    {
        public int Id { get; set; }
        public string FileTitle { get; set; }
        [NotMapped]
        public IFormFile File { get; set; }
    }
}
