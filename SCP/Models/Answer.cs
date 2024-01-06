using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCP.Models
{
    public class Answer
    {
        
        public int Id { get; set; }


        public required string Text { get; set; }

        
        public required string UserName { get; set; }

        [ValidateNever]
        public int QuestionId { get; set; }
        [ForeignKey("QuestionId")]
        [ValidateNever]
        public Question Question { get; set; }
    }
}
