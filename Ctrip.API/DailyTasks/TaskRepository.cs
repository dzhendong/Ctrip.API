
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace Ctrip.API
{
    public class TaskRepository : ITaskRepository
    {
        private List<Task> Tasks
        {
            get
            {
                if (HttpContext.Current.Cache["Tasks"] == null)
                    HttpContext.Current.Cache["Tasks"] = new List<Task>();

                return HttpContext.Current.Cache["Tasks"] as List<Task>;
            }
            set
            {
                HttpContext.Current.Cache["Tasks"] = value;
            }
        }

        public IEnumerable<Task> Get()
        {
            return Tasks;
        }

        public Task Get(int id)
        {
            return Tasks.Find(t => t.Id == id);
        }

        public Task Post(Task task)
        {
            //task.Id = 1;//Tasks.Max(t => t.Id) + 1;
            Tasks.Add(task);

            return task;
        }

        public Task Put(Task task)
        {
            var t = Get(task.Id);

            if (t == null)
                throw new Exception(string.Format("Task with id {0} not exists.", task.Id));

            //t.Description = task.Description;
            //t.Priority = task.Priority;

            return t;
        }

        public bool Delete(int id)
        {
            var t = Get(id);

            if (t == null)
                return false;

            Tasks.Remove(t);

            return true;
        }
    }
}