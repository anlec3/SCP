using System.ComponentModel.DataAnnotations;

namespace SCP.Models
{
    public class Question
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Bu Alan Boş Bırakılamaz!")]
        [MaxLength(200)]
        public string Text { get; set; }
        public string UserName { get; set; }
    }
}
