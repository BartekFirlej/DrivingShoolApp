using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Models;
using DrivingSchoolApp.Repositories;

namespace DrivingSchoolApp.Services
{
    public interface IClassroomService
    {
        public Task<PagedList<ClassroomGetDTO>> GetClassrooms(int page, int size);
        public Task<ClassroomGetDTO> GetClassroom(int classroomId);
        public Task<ClassroomGetDTO> PostClassroom(ClassroomPostDTO classroomDetails);
        public Task<Classroom> CheckClassroom(int classroomId);
        public Task<Classroom> DeleteClassroom(int classroomId);
        public Task<ClassroomGetDTO> UpdateClassroom(int classroomId, ClassroomPostDTO classroomUpdate);
    }
    public class ClassroomService : IClassroomService
    {
        private readonly IClassroomRepository _classroomRepository;
        private readonly IAddressService _addressService;

        public ClassroomService(IClassroomRepository classroomRepository, IAddressService addressService)
        {
            _classroomRepository = classroomRepository;
            _addressService = addressService;
        }

        public async Task<PagedList<ClassroomGetDTO>> GetClassrooms(int page, int size)
        {
            var classrooms = await _classroomRepository.GetClassrooms(page, size);
            if (!classrooms.PagedItems.Any())
                throw new NotFoundClassroomException();
            return classrooms;
        }

        public async Task<ClassroomGetDTO> GetClassroom(int classroomId)
        {
            var classroom = await _classroomRepository.GetClassroom(classroomId);
            if (classroom == null)
                throw new NotFoundClassroomException(classroomId);
            return classroom;
        }

        public async Task<ClassroomGetDTO> PostClassroom(ClassroomPostDTO classroomDetails)
        {
            if (classroomDetails.Number <= 0)
                throw new ValueMustBeGreaterThanZeroException("number");
            if (classroomDetails.Size <=0)
                throw new ValueMustBeGreaterThanZeroException("size");
            var address = await _addressService.CheckAddress(classroomDetails.AddressID);
            var addedClassroom = await _classroomRepository.PostClassroom(classroomDetails);
            return await _classroomRepository.GetClassroom(addedClassroom.Id);
        }

        public async Task<Classroom> CheckClassroom(int classroomId)
        {
            var classroom = await _classroomRepository.CheckClassroom(classroomId);
            if (classroom == null)
                throw new NotFoundClassroomException(classroomId);
            return classroom;
        }

        public async Task<Classroom> DeleteClassroom(int classroomId)
        {
            var classroomToDelete = await CheckClassroom(classroomId);
            return await _classroomRepository.DeleteClassroom(classroomToDelete);
        }

        public async Task<ClassroomGetDTO> UpdateClassroom(int classroomId, ClassroomPostDTO classroomUpdate)
        {
            await CheckClassroom(classroomId);
            await _addressService.CheckAddress(classroomUpdate.AddressID);
            await _classroomRepository.UpdateClassroom(classroomId, classroomUpdate);
            return await _classroomRepository.GetClassroom(classroomId);
        }
    }
}
