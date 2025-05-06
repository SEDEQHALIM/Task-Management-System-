using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManegmentProject.Enums;
using TaskStatus = TaskManegmentProject.Enums.TaskStatus;

namespace TaskManegmentProject.DBcontcion.ViewModels
{
    public class TaskViewModel
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description can't be more than 500 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage ="Task States Is Required")]
        public TaskStatus Status { get; set; }

        [Required(ErrorMessage = "Task Priority Is Required")]
        public TaskPriority Priority { get; set; }

        [Required]
        public string WorkSpaceId { get; set; }

        public string? AssignTo{ get; set; }
        
    }
}
