using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolApp.Repositories
{
    public interface ILecturerRepository
    {
        public Task<PagedList<LecturerGetDTO>> GetLecturers(int page, int size);
        public Task<LecturerGetDTO> GetLecturer(int lecturerId);
        public Task<Lecturer> PostLecturer(LecturerPostDTO lecturerDetails);
    }
    public class LecturerRepository : ILecturerRepository
    {
        private readonly DrivingSchoolDbContext _dbContext;

        public LecturerRepository(DrivingSchoolDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedList<LecturerGetDTO>> GetLecturers(int page, int size)
        {
            return await PagedList<LecturerGetDTO>.Create(
                            _dbContext.Lecturers.Select(l => new LecturerGetDTO
                            {
                                Id = l.Id,
                                Name = l.Name,
                                SecondName = l.SecondName
                            }).OrderBy(l => l.Id),
                            page, size);
        }

        public async Task<LecturerGetDTO> GetLecturer(int lecturerId)
        {
            return await _dbContext.Lecturers
                             .Where(l => l.Id == lecturerId)
                             .Select(l => new LecturerGetDTO
                               {
                                Id = l.Id,
                                Name = l.Name,
                                SecondName = l.SecondName
                               }).FirstOrDefaultAsync();
        }

        public async Task<Lecturer> PostLecturer(LecturerPostDTO lecturerDetails)
        {
            var lecturerToAdd = new Lecturer
            {
                Name = lecturerDetails.Name,
                SecondName = lecturerDetails.SecondName
            };
            await _dbContext.Lecturers.AddAsync(lecturerToAdd);
            await _dbContext.SaveChangesAsync();
            return lecturerToAdd;
        }
    }
}
