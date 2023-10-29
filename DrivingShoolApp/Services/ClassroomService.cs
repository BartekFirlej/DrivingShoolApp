using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Models;
using DrivingSchoolApp.Repositories;
using System.Reflection.Metadata.Ecma335;

namespace DrivingSchoolApp.Services
{
    public interface IClassroomService
    {
        public Task<ICollection<ClassroomGetDTO>> GetClassrooms();
        public Task<ClassroomGetDTO> GetClassroom(int classroomId);
        public Task<ClassroomGetDTO> PostClassroom(ClassroomPostDTO classroomDetails);
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

        public async Task<ICollection<ClassroomGetDTO>> GetClassrooms()
        {
            var classrooms = await _classroomRepository.GetClassrooms();
            if (!classrooms.Any())
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
            var address = await _addressService.GetAddress(classroomDetails.AddressID);
            var addedClassroom = await _classroomRepository.PostClassroom(classroomDetails);
            return await _classroomRepository.GetClassroom(addedClassroom.Id);
        }
    }
}
