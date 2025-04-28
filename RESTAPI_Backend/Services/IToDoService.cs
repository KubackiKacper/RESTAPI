using RESTAPI_Backend.DTOs;
using RESTAPI_Backend.Enums;

namespace RESTAPI_Backend.Services
{
    public interface IToDoService
    {
        Task<ToDoDTO[]> GetAll();
        Task<ToDoDTO> AddToDo(SaveToDoDTO saveToDoDTO);
        Task<bool> DeleteToDo(int id);
        Task<ToDoDTO> UpdateToDo(int id, SaveToDoDTO toDoDto);
        Task<ToDoDTO> GetSpecificToDo(int id);
        Task<ToDoDTO> SetToDoPercentComplete(int id, int percentComplete);
        Task<ToDoDTO> MarkToDoAsDone(int id, bool isDone);
        Task<IEnumerable<ToDoDTO>> GetIncomingToDos(DateFilter filter);
    }
}
