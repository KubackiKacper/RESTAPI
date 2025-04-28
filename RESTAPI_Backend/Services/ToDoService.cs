using Microsoft.EntityFrameworkCore;
using RESTAPI_Backend.Data;
using RESTAPI_Backend.DTOs;
using RESTAPI_Backend.Models;
using RESTAPI_Backend.Enums;

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
        public async Task<ToDoDTO> GetSpecificToDo (int id)
        {
            Todo toDo = await _context
                .Todos
                .FindAsync(id);
            if (toDo == null)
            {
                return null;
            }
            return new ToDoDTO
            {
                Id = toDo.Id,
                Title = toDo.Title,
                Description = toDo.Description,
                ExpiryDateTime = toDo.ExpiryDateTime,
                PercentComplete = toDo.PercentComplete,
                IsDone = toDo.IsDone
            };
        }
        public async Task<ToDoDTO> AddToDo(SaveToDoDTO saveToDoDTO)
        {
            Todo addToDo = new Todo
            {
                Title = saveToDoDTO.Title,
                Description = saveToDoDTO.Description,
                ExpiryDateTime = saveToDoDTO.ExpiryDateTime,
                PercentComplete = saveToDoDTO.PercentComplete,
                IsDone = false
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

        public async Task<ToDoDTO> SetToDoPercentComplete(int id, int percentComplete)
        {
            Todo existingToDo = await _context
                .Todos
                .FindAsync(id);
            if (existingToDo == null)
            {
                return null;
            }

            if (percentComplete < 0 || percentComplete > 100)
            {
                return null;
            }

            existingToDo.PercentComplete = percentComplete;

            await _context
                .SaveChangesAsync();

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

        public async Task<ToDoDTO> MarkToDoAsDone(int id, bool isDone)
        {
            Todo existingToDo = await _context
                .Todos
                .FindAsync(id);
            if (existingToDo == null)
            {
                return null;
            }
            existingToDo.IsDone = isDone;

            await _context
                .SaveChangesAsync();

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
        public async Task<IEnumerable<ToDoDTO>> GetIncomingToDos(DateFilter filter)
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);
            var endOfWeek = today.AddDays(7 - (int)today.DayOfWeek);

            IQueryable<Todo> query = _context.Todos;

            switch (filter)
            {
                case DateFilter.Today:
                    query = query.Where(t => t.ExpiryDateTime.Date == today);
                    break;

                case DateFilter.Tomorrow:
                    query = query.Where(t => t.ExpiryDateTime.Date == tomorrow);
                    break;

                case DateFilter.Week:
                    query = query.Where(t => t.ExpiryDateTime.Date >= today && t.ExpiryDateTime.Date <= endOfWeek);
                    break;

                case DateFilter.Expired:
                    query = query.Where(t => t.ExpiryDateTime.Date < today);
                    break;

                default:
                    return null;
            }

            var todos = await query.ToListAsync();

            return todos.Select(todo => new ToDoDTO
            {
                Id = todo.Id,
                Title = todo.Title,
                Description = todo.Description,
                ExpiryDateTime = todo.ExpiryDateTime,
                PercentComplete = todo.PercentComplete,
                IsDone = todo.IsDone
            });
        }
    }
}
