using Microsoft.EntityFrameworkCore;
using RESTAPI_Backend.Data;
using RESTAPI_Backend.DTOs;
using RESTAPI_Backend.Models;

namespace RESTAPI_Backend.Services
{
    public class ToDoService : IToDoService
    {
        private readonly AppDbContext _context;
        public ToDoService (AppDbContext context)
        {
            _context = context;
        }

        public async Task<ToDoDTO[]> GetAll()
        {
            ToDoDTO[] response = await _context
                .Todos
                .Select(toDo => new ToDoDTO
                {
                    Id = toDo.Id,
                    Title = toDo.Title,
                    Description = toDo.Description,
                    ExpiryDateTime = toDo.ExpiryDateTime,
                    PercentComplete = toDo.PercentComplete,
                    IsDone = toDo.IsDone,
                })
                .ToArrayAsync();
            return response;
        }
        public async Task<ToDoDTO> AddToDo(SaveToDoDTO saveToDoDTO)
        {
            Todo addToDo = new Todo
            {
                Title = saveToDoDTO.Title,
                Description = saveToDoDTO.Description,
                ExpiryDateTime = saveToDoDTO.ExpiryDateTime,
                PercentComplete = saveToDoDTO.PercentComplete,
                IsDone = saveToDoDTO.IsDone
            };
            
            _context
                .Todos
                .Add(addToDo);

            await _context.SaveChangesAsync();

            return new ToDoDTO
            {
                Id = addToDo.Id,
                Title = addToDo.Title,
                Description = addToDo.Description,
                ExpiryDateTime = addToDo.ExpiryDateTime,
                PercentComplete = addToDo.PercentComplete,
                IsDone = addToDo.IsDone
            };
        }
        public async Task<bool> DeleteToDo(int id)
        {
            Todo removeToDo = await _context
                .Todos
                .FindAsync(id);
            if (removeToDo == null)
            {
                throw new NullReferenceException($"Could not find id: {id} ");
            }
            _context
                .Todos
                .Remove(removeToDo);
            _context.SaveChangesAsync();
            return true;
        }
        public async Task<ToDoDTO> UpdateToDo(int id, SaveToDoDTO toDoDto)
        {
            Todo existingToDo = await _context
                .Todos
                .FindAsync(id);
            if (existingToDo == null)
            {
                return null;
            }
            existingToDo.Title = toDoDto.Title;
            existingToDo.Description = toDoDto.Description;
            existingToDo.ExpiryDateTime = toDoDto.ExpiryDateTime;
            existingToDo.PercentComplete = toDoDto.PercentComplete;
            existingToDo.IsDone = toDoDto.IsDone;
            await _context.SaveChangesAsync();

            return new ToDoDTO
            {
                Id = existingToDo.Id,
                Title = existingToDo.Title,
                Description = existingToDo.Description,
                ExpiryDateTime = existingToDo.ExpiryDateTime,
                PercentComplete = existingToDo.PercentComplete,
                IsDone = existingToDo.IsDone
            };
        }
    }
}
