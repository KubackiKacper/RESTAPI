using RESTAPI_Backend.DTOs;

namespace RESTAPI_Backend.Services
{
    public interface IToDoService
    {
        Task<ToDoDTO[]> GetAll();
        Task<ToDoDTO> AddToDo(SaveToDoDTO saveToDoDTO);
        Task<bool> DeleteToDo(int id);
        Task<ToDoDTO> UpdateToDo(int id, SaveToDoDTO toDoDto);
    }
}
