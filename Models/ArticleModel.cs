using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComplexProject.Models
{
    public class ArticleModel : Controller
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Название")]
        [StringLength(100, ErrorMessage = "Название должно содержать минимум 3 символа"), MinLength(3)]
        public string Title { get; set; }
        public string BlogName { get; set; }

        [Required]
        [Display(Name = "Дата последнего обновления")]
        public DateTime ReleseDate { get; set; }
        public string ImageURL { get; set; }

        [Display(Name = "Картинка болга")]
        [BindingBehavior(BindingBehavior.Optional)]
        public ImageModel HelloImage { get; set; }

        [Display(Name = "Категория")]
        [ForeignKey("CategoryId")]
        public int? CategoryId { get; set; }
        [Display(Name = "Теги")]
        [StringLength(100, ErrorMessage = "теги должны состоять не больше чем из 100 символов")]
        public string Tags { get; set; }
        public bool IsFavorit { get; set; } = false;
        [Display(Name = "Автор_")]
        public User Avtor { get; set; }
        //[Required]
        [Display(Name = "Автор")]
        public string AvtorName { get; set; }

        // AvtorId Avtor Tags CategoryId Category HelloImage HelloImageId ReleseDate Description ShortDesk Title Id
    }
}
