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
        public Task<LectureGetDTO> PostLecture(LecturePostDTO lectureDetails);
        public Task<bool> GetCourseLectureSubject(int courseId, int subjectId);
        public Task<Lecture> CheckLecture(int lectureId);
        public Task<Lecture> DeleteLecture(int lectureId);
    }
    public class LectureService : ILectureService
    {
        private readonly ILectureRepository _lectureRepository;
        private readonly ILecturerService _lecturerService;
        private readonly ICourseSubjectService _courseSubjectService;
        private readonly IClassroomService _classroomService;

        public LectureService(ILectureRepository lectureRepository, ILecturerService lecturerService, ICourseSubjectService courseSubjectService, IClassroomService classroomService)
        {
            _lectureRepository = lectureRepository;
            _courseSubjectService = courseSubjectService;
            _classroomService = classroomService;
            _lecturerService = lecturerService;
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

        public async Task<bool> GetCourseLectureSubject(int courseId, int subjectId)
        {
            var lecture = await _lectureRepository.GetCourseLectureSubject(courseId, subjectId);
            if (lecture == null)
                return false;
            return true;
        }

        public async Task<LectureGetDTO> PostLecture(LecturePostDTO lectureDetails)
        {
            if (lectureDetails.LectureDate == DateTime.MinValue)
                throw new DateTimeException("lecture date");
            var lecturer = await _lecturerService.GetLecturer(lectureDetails.LecturerId);
            var courseSubject = await _courseSubjectService.GetCourseSubject(lectureDetails.CourseId, lectureDetails.SubjectId);
            var classroom = await _classroomService.GetClassroom(lectureDetails.ClassroomId);
            if (!GetCourseLectureSubject(lectureDetails.CourseId, lectureDetails.SubjectId).Result)
                throw new SubjectAlreadyConductedLectureException(lectureDetails.CourseId, lectureDetails.SubjectId);
            var addedLecture = await _lectureRepository.PostLecture(lectureDetails);
            return await _lectureRepository.GetLecture(addedLecture.Id);
        }

        public async Task<Lecture> CheckLecture(int lectureId)
        {
            var lecture = await _lectureRepository.CheckLecture(lectureId);
            if (lecture == null)
                throw new NotFoundLectureException(lectureId);
            return lecture;
        }

        public async Task<Lecture> DeleteLecture(int lectureId)
        {
            var lectureToDelete = await CheckLecture(lectureId);
            return await _lectureRepository.DeleteLecture(lectureToDelete);
        }
    }
}
