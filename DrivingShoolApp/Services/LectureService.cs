using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Models;
using DrivingSchoolApp.Repositories;

namespace DrivingSchoolApp.Services
{
    public interface ILectureService
    {
        public Task<ICollection<LectureGetDTO>> GetLectures();
        public Task<LectureGetDTO> GetLecture(int lectureId);
        public Task<LectureGetDTO> PostLecture(LecturePostDTO lectureDetails);
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

        public async Task<ICollection<LectureGetDTO>> GetLectures()
        {
            var lectures = await _lectureRepository.GetLectures();
            if (!lectures.Any())
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

        public async Task<LectureGetDTO> PostLecture(LecturePostDTO lectureDetails)
        {
            if (lectureDetails.LectureDate == DateTime.MinValue)
                throw new DateTimeException("lecture date");
            var lecturer = await _lecturerService.GetLecturer(lectureDetails.LecturerId);
            var courseSubject = await _courseSubjectService.GetCourseSubject(lectureDetails.CourseId, lectureDetails.SubjectId);
            var classroom = await _classroomService.GetClassroom(lectureDetails.ClassroomId);
            var addedLecture = await _lectureRepository.PostLecture(lectureDetails);
            return await _lectureRepository.GetLecture(addedLecture.Id);
        }
    }
}
