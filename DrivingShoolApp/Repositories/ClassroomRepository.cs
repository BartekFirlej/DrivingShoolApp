using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolApp.Repositories
{
    public interface IClassroomRepository
    {
        public Task<ICollection<ClassroomGetDTO>> GetClassrooms();
        public Task<ClassroomGetDTO> GetClassroom(int classroomId);
        public Task<Classroom> PostClassroom(ClassroomPostDTO classroomDetails);
    }
    public class ClassroomRepository : IClassroomRepository
    {
        private readonly DrivingSchoolDbContext _dbContext;

        public ClassroomRepository(DrivingSchoolDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ICollection<ClassroomGetDTO>> GetClassrooms()
        {
            return await _dbContext.Classrooms.Include(c => c.Address)
                                      .Select(c => new ClassroomGetDTO
                                      {
                                          ClassroomId = c.Id,
                                          ClassroomNumber = c.Number,
                                          Size = c.Size,
                                          Address = new AddressGetDTO
                                          {
                                              ID = c.Address.Id,
                                              City = c.Address.City,
                                              Street = c.Address.Street,
                                              Number = c.Address.Number,
                                              PostalCode = c.Address.PostalCode
                                          }
                                      }).ToListAsync();
        }

        public async Task<ClassroomGetDTO> GetClassroom(int classroomId)
        {
            return await _dbContext.Classrooms.Include(c => c.Address)
                          .Where(c => c.Id == classroomId)
                          .Select(c => new ClassroomGetDTO
                          {
                              ClassroomId = c.Id,
                              ClassroomNumber = c.Number,
                              Size = c.Size,
                              Address = new AddressGetDTO
                              {
                                  ID = c.Address.Id,
                                  City = c.Address.City,
                                  Street = c.Address.Street,
                                  Number = c.Address.Number,
                                  PostalCode = c.Address.PostalCode
                              }
                          }).FirstOrDefaultAsync();
        }

        public async Task<Classroom> PostClassroom(ClassroomPostDTO classroomDetails)
        {
            var classroomToAdd = new Classroom
            {
                Number = classroomDetails.Number,
                Size = classroomDetails.Size,
                AddressId = classroomDetails.AddressID
            };
            await _dbContext.Classrooms.AddAsync(classroomToAdd);
            await _dbContext.SaveChangesAsync();
            return classroomToAdd;
        }
    }
}
