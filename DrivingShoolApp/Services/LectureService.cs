﻿using AutoMapper;
using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Models;
using DrivingSchoolApp.Repositories;

namespace DrivingSchoolApp.Services
{
    public interface ILectureService
    {
        public Task<PagedList<LectureGetDTO>> GetLectures(int page, int size);
        public Task<LectureGetDTO> GetLecture(int lectureId);
        public Task<LectureResponseDTO> PostLecture(LectureRequestDTO lectureDetails);
        public Task<bool> CheckLectureAtCourseAboutSubject(int courseId, int subjectId);
        public Task<Lecture> CheckLecture(int lectureId);
        public Task<Lecture> CheckLectureTracking(int lectureId);
        public Task<Lecture> DeleteLecture(int lectureId);
        public Task<LectureResponseDTO> UpdateLecture(int lectureId, LectureRequestDTO lectureUpdate);
    }
    public class LectureService : ILectureService
    {
        private readonly ILectureRepository _lectureRepository;
        private readonly ILecturerService _lecturerService;
        private readonly ICourseSubjectService _courseSubjectService;
        private readonly IClassroomService _classroomService;
        private readonly IMapper _mapper;

        public LectureService(ILectureRepository lectureRepository, ILecturerService lecturerService, ICourseSubjectService courseSubjectService, IClassroomService classroomService, IMapper mapper)
        {
            _lectureRepository = lectureRepository;
            _courseSubjectService = courseSubjectService;
            _classroomService = classroomService;
            _lecturerService = lecturerService;
            _mapper = mapper;
        }

        public async Task<PagedList<LectureGetDTO>> GetLectures(int page, int size)
        {
            var lectures = await _lectureRepository.GetLectures(page, size);
            if (!lectures.PagedItems.Any())
                throw new NotFoundLectureException();
            return lectures;
        }

        public async Task<LectureGetDTO> GetLecture(int lectureId)
        {
            var lecture = await _lectureRepository.GetLecture(lectureId);
            if (lecture == null)
                throw new NotFoundLectureException(lectureId);
            return lecture;
        }

        public async Task<bool> CheckLectureAtCourseAboutSubject(int courseId, int subjectId)
        {
            var lecture = await _lectureRepository.CheckLectureAtCourseAboutSubject(courseId, subjectId);
            if (lecture == null)
                return false;
            return true;
        }

        public async Task<LectureResponseDTO> PostLecture(LectureRequestDTO lectureDetails)
        {
            if (lectureDetails.LectureDate == DateTime.MinValue)
                throw new DateTimeException("lecture date");
            await _lecturerService.CheckLecturer(lectureDetails.LecturerId);
            await _courseSubjectService.CheckCourseSubject(lectureDetails.CourseId, lectureDetails.SubjectId);
            await _classroomService.CheckClassroom(lectureDetails.ClassroomId);
            if (CheckLectureAtCourseAboutSubject(lectureDetails.CourseId, lectureDetails.SubjectId).Result)
                throw new SubjectAlreadyConductedLectureException(lectureDetails.CourseId, lectureDetails.SubjectId);
            var addedLecture = await _lectureRepository.PostLecture(lectureDetails);
            return _mapper.Map<LectureResponseDTO>(addedLecture);
        }

        public async Task<Lecture> CheckLecture(int lectureId)
        {
            var lecture = await _lectureRepository.CheckLecture(lectureId);
            if (lecture == null)
                throw new NotFoundLectureException(lectureId);
            return lecture;
        }

        public async Task<Lecture> CheckLectureTracking(int lectureId)
        {
            var lecture = await _lectureRepository.CheckLectureTracking(lectureId);
            if (lecture == null)
                throw new NotFoundLectureException(lectureId);
            return lecture;
        }

        public async Task<Lecture> DeleteLecture(int lectureId)
        {
            var lectureToDelete = await CheckLecture(lectureId);
            return await _lectureRepository.DeleteLecture(lectureToDelete);
        }

        public async Task<LectureResponseDTO> UpdateLecture(int lectureId, LectureRequestDTO lectureUpdate)
        {
            var lectureToUpdate = await CheckLectureTracking(lectureId);
            if (lectureUpdate.LectureDate == DateTime.MinValue)
                throw new DateTimeException("lecture date");
            await _lecturerService.CheckLecturer(lectureUpdate.LecturerId);
            await _courseSubjectService.CheckCourseSubject(lectureUpdate.CourseId, lectureUpdate.SubjectId);
            await _classroomService.CheckClassroom(lectureUpdate.ClassroomId);
            if (CheckLectureAtCourseAboutSubject(lectureUpdate.CourseId, lectureUpdate.SubjectId).Result)
                throw new SubjectAlreadyConductedLectureException(lectureUpdate.CourseId, lectureUpdate.SubjectId);
            var updatedLecture = await _lectureRepository.UpdateLecture(lectureToUpdate, lectureUpdate);
            return _mapper.Map<LectureResponseDTO>(updatedLecture);
        }
    }
}
