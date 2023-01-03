using Microsoft.AspNetCore.Mvc;

namespace ComplexProject.Models
{
    public class FileModel : Controller
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
    }
}
