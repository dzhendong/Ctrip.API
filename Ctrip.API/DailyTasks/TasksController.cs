using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Ctrip.API
{
    public class TasksController : ApiController
    {
        // GET /api/tasks
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET /api/tasks/5
        public string Get(int id)
        {
            return "value";
        }

        // POST /api/tasks
        public void Post(string value)
        {
        }

        // PUT /api/tasks/5
        public void Put(int id, string value)
        {
        }

        // DELETE /api/tasks/5
        public void Delete(int id)
        {
        }
    }

    public class TasksController1 : ApiController
    {
        private readonly ITaskRepository _taskRepository;

        public TasksController1(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public IEnumerable<Task> Get()
        {
            return _taskRepository.Get();
        }

        public Task Get(int id)
        {
            var task = _taskRepository.Get(id);

            if (task == null)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Content = new StringContent("Task not found")
                });
            }

            return task;
        }

        //public HttpResponseMessage<Task> Post(Task task)
        //{
        //    task = _taskRepository.Post(task);

        //    var response = new HttpResponseMessage<Task>(task, HttpStatusCode.Created);

        //    string uri = Url.Route(null, new { id = task.Id });
        //    response.Headers.Location = new Uri(Request.RequestUri, uri);

        //    return response;
        //}

        public Task Put(Task task)
        {
            try
            {
                task = _taskRepository.Put(task);
            }
            catch (Exception)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Content = new StringContent("Task not found")
                });
            }

            return task;
        }

        public HttpResponseMessage Delete(int id)
        {
            _taskRepository.Delete(id);

            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NoContent
            };
        }
    }
}
