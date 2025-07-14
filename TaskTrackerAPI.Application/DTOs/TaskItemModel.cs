using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTrackerAPI.Application.DTOs
{
    public class TaskItemModel
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string? Status { get; set; }
        public Guid? AssignedToUserId { get; set; }
    }
}
