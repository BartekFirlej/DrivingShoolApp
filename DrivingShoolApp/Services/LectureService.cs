using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;
using DrivingSchoolApp.Models;

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
        private readonly ILectureService _lectureService;
        private readonly ILecturerService _lecturerService;
        private readonly ICourseSubjectService _courseSubjectService;
        private readonly IClassroomService _classroomService;

        public LectureService(ILectureService lectureService, ILecturerService lecturerService, ICourseSubjectService courseSubjectService, IClassroomService classroomService)
        {
            _lectureService = lectureService;
            _courseSubjectService = courseSubjectService;
            _classroomService = classroomService;
            _lecturerService = lecturerService;
        }

        public async Task<ICollection<LectureGetDTO>> GetLectures()
        {
            var lectures = await _lectureService.GetLectures();
            if (!lectures.Any())
                throw new NotFoundLecturesException();
            return lectures;
        }

        public async Task<LectureGetDTO> GetLecture(int lectureId)
        {
            var lecture = await _lectureService.GetLecture(lectureId);
            if (lecture == null)
                throw new NotFoundLectureException(lectureId);
            return lecture;
        }

        public async Task<LectureGetDTO> PostLecture(LecturePostDTO lectureDetails)
        {
            var lecturer = await _lecturerService.GetLecturer(lectureDetails.LecturerId);
            var courseSubject = await _courseSubjectService.GetCourseSubject(lectureDetails.CourseId, lectureDetails.SubjectId);
            var classroom = await _classroomService.GetClassroom(lectureDetails.ClassroomId);
            var addedLecture = await _lectureService.PostLecture(lectureDetails);
            return await _lectureService.GetLecture(addedLecture.Id);
        }
    }
}
