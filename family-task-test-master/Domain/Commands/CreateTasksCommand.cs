using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Commands
{
    public class CreateTasksCommand
    {
   
        public string Subject { get; set; }
        public bool IsComplete { get; set; }
        public Guid AssignedToId { get; set; }
    }
}
