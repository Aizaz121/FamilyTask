using Domain.DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Queries
{
    public class GetAllTasksQueryResult
    {
        public IEnumerable<Task> Payload { get; set; }
    }
}
