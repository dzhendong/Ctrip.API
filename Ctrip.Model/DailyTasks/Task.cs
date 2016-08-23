using System;

namespace Ctrip.Model
{
    public class Task
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public int Priority { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
