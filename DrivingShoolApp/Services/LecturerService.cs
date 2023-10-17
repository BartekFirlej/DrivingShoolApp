using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Models;
using DrivingSchoolApp.Repositories;

namespace DrivingSchoolApp.Services
{
    public interface ILecturerService
    {
        public Task<ICollection<LecturerGetDTO>> GetLecturers();
        public Task<LecturerGetDTO> GetLecturer(int lecturerId);
        public Task<LecturerGetDTO> PostLecturer(LecturerPostDTO lecturerDetails);
    }
    public class LecturerService : ILecturerService
    {
        private readonly ILecturerRepository _lecturerRepository;

        public LecturerService(ILecturerRepository lecturerRepository)
        {
            _lecturerRepository = lecturerRepository;
        }

        public async Task<ICollection<LecturerGetDTO>> GetLecturers()
        {
            var lecturers = await _lecturerRepository.GetLecturers();
            if (!lecturers.Any())
                throw new NotFoundLecturersException();
            return lecturers;
        }

        public async Task<LecturerGetDTO> GetLecturer(int lecturerId)
        {
            var lecturer = await _lecturerRepository.GetLecturer(lecturerId);
            if (lecturer == null)
                throw new NotFoundLecturerException(lecturerId);
            return lecturer;
        }

        public async Task<LecturerGetDTO> PostLecturer(LecturerPostDTO lecturerDetails)
        {
            var addedLecturer = await _lecturerRepository.PostLecturer(lecturerDetails);
            return await _lecturerRepository.GetLecturer(addedLecturer.Id);
        }
    }
}
