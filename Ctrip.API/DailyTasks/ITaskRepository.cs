
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ctrip.API
{
    public interface ITaskRepository
    {
        IEnumerable<Task> Get();

        Task Get(int id);

        Task Post(Task Task);

        Task Put(Task Task);

        bool Delete(int id);
    }
}
