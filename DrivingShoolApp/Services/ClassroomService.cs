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
        private readonly IAddressRepository _addressRepository;

        public ClassroomService(IClassroomRepository classroomRepository, IAddressRepository addressRepository)
        {
            _classroomRepository = classroomRepository;
            _addressRepository = addressRepository;
        }

        public async Task<ICollection<ClassroomGetDTO>> GetClassrooms()
        {
            var classrooms = await _classroomRepository.GetClassrooms();
            if (!classrooms.Any())
                throw new NotFoundClassroomsException();
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
            var address = await _addressRepository.GetAddress(classroomDetails.AddressID);
            var addedClassroom = await _classroomRepository.PostClassroom(classroomDetails);
            return await _classroomRepository.GetClassroom(addedClassroom.Id);
        }
    }
}
