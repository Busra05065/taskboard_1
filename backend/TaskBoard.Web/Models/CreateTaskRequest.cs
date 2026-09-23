using System.ComponentModel.DataAnnotations;

namespace TaskBoard.Web.Models
{
    public class CreateTaskRequest
    {
        [Required(ErrorMessage = "Başlık zorunludur.")]
        public string Title { get; set; } = string.Empty;

        public string Priority { get; set; } = "normal";
    }

    public class UpdateTaskStatusRequest
    {
        [Required]
        public string Status { get; set; } = string.Empty;
    }
}