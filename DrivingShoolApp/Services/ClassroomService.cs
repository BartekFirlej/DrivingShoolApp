﻿using AutoMapper;
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
        public Task<ClassroomResponseDTO> PostClassroom(ClassroomRequestDTO classroomDetails);
        public Task<Classroom> CheckClassroom(int classroomId);
        public Task<Classroom> CheckClassroomTracking(int classroomId);
        public Task<Classroom> DeleteClassroom(int classroomId);
        public Task<ClassroomResponseDTO> UpdateClassroom(int classroomId, ClassroomRequestDTO classroomUpdate);
    }
    public class ClassroomService : IClassroomService
    {
        private readonly IClassroomRepository _classroomRepository;
        private readonly IAddressService _addressService;
        private readonly IMapper _mapper;

        public ClassroomService(IClassroomRepository classroomRepository, IAddressService addressService, IMapper mapper)
        {
            _classroomRepository = classroomRepository;
            _addressService = addressService;
            _mapper = mapper;
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

        public async Task<ClassroomResponseDTO> PostClassroom(ClassroomRequestDTO classroomDetails)
        {
            if (classroomDetails.Number <= 0)
                throw new ValueMustBeGreaterThanZeroException("number");
            if (classroomDetails.Size <=0)
                throw new ValueMustBeGreaterThanZeroException("size");
            var address = await _addressService.CheckAddress(classroomDetails.AddressId);
            var addedClassroom = await _classroomRepository.PostClassroom(classroomDetails);
            return _mapper.Map<ClassroomResponseDTO>(addedClassroom);
        }

        public async Task<Classroom> CheckClassroom(int classroomId)
        {
            var classroom = await _classroomRepository.CheckClassroom(classroomId);
            if (classroom == null)
                throw new NotFoundClassroomException(classroomId);
            return classroom;
        }

        public async Task<Classroom> CheckClassroomTracking(int classroomId)
        {
            var classroom = await _classroomRepository.CheckClassroomTracking(classroomId);
            if (classroom == null)
                throw new NotFoundClassroomException(classroomId);
            return classroom;
        }

        public async Task<Classroom> DeleteClassroom(int classroomId)
        {
            var classroomToDelete = await CheckClassroom(classroomId);
            return await _classroomRepository.DeleteClassroom(classroomToDelete);
        }

        public async Task<ClassroomResponseDTO> UpdateClassroom(int classroomId, ClassroomRequestDTO classroomUpdate)
        {
            var classroomToUpdate = await CheckClassroomTracking(classroomId);
            if (classroomUpdate.Number <= 0)
                throw new ValueMustBeGreaterThanZeroException("number");
            if (classroomUpdate.Size <= 0)
                throw new ValueMustBeGreaterThanZeroException("size");
            await _addressService.CheckAddress(classroomUpdate.AddressId);
            var updatedClassroom = await _classroomRepository.UpdateClassroom(classroomToUpdate, classroomUpdate);
            return _mapper.Map<ClassroomResponseDTO>(updatedClassroom);
        }
    }
}
