using Domain.DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Commands
{
    public class CreateTasksCommandResult
    {
        public Task Payload { get; set; }
    }
}
