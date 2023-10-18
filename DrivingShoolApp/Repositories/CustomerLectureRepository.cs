using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolApp.Repositories
{
    public interface ICustomerLectureRepository
    {
        public Task<ICollection<CustomerLectureGetDTO>> GetCustomersLectures();
        public Task<ICollection<CustomerLectureGetDTO>> GetCustomerLectures(int customerId);
        public Task<ICollection<CustomerLectureGetDTO>> GetCustomersLecture(int lectureId);
        public Task<CustomerLectureGetDTO> GetCustomerLecture(int customerId, int lectureId);
        public Task<CustomerLectureGetDTO> PostCustomerLecture(CustomerLecturePostDTO customerLectureDetails);
    }
    public class CustomerLectureRepository : ICustomerLectureRepository
    {
        private readonly DrivingSchoolDbContext _dbContext;

        public CustomerLectureRepository(DrivingSchoolDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ICollection<CustomerLectureGetDTO>> GetCustomersLectures()
        {
            return await _dbContext.Lectures.Include(l => l.Customers)
                                   .Select(l => new CustomerLectureGetDTO
                                   {
                                       LectureId = l.LecturerId,
                                       LectureDate = l.LectureDate,
                                       CustomerId = l.Customers.FirstOrDefault().Id,
                                       CustomerName = l.Customers.FirstOrDefault().Name
                                   }).ToListAsync();
        }

        public async Task<ICollection<CustomerLectureGetDTO>> GetCustomerLectures(int customerId)
        {
            return await _dbContext.Lectures.Include(l => l.Customers)
                       .Where(l => l.Customers.FirstOrDefault().Id == customerId)
                       .Select(l => new CustomerLectureGetDTO
                       {
                           LectureId = l.LecturerId,
                           LectureDate = l.LectureDate,
                           CustomerId = l.Customers.FirstOrDefault().Id,
                           CustomerName = l.Customers.FirstOrDefault().Name
                       }).ToListAsync();
        }

        public async Task<ICollection<CustomerLectureGetDTO>> GetCustomersLecture(int lectureId)
        {
            return await _dbContext.Lectures.Include(l => l.Customers)
                .Where(l => l.Id == lectureId)
                 .Select(l => new CustomerLectureGetDTO
                 {
                     LectureId = l.LecturerId,
                    LectureDate = l.LectureDate,
                     CustomerId = l.Customers.FirstOrDefault().Id,
                     CustomerName = l.Customers.FirstOrDefault().Name
                 }).ToListAsync();
        }

        public async Task<CustomerLectureGetDTO> GetCustomerLecture(int customerId, int lectureId)
        {
            return await _dbContext.Lectures.Include(l => l.Customers)
                .Where(l => l.Id == lectureId && l.Customers.FirstOrDefault().Id == customerId)
                 .Select(l => new CustomerLectureGetDTO
                 {
                     LectureId = l.LecturerId,
                     LectureDate = l.LectureDate,
                     CustomerId = l.Customers.FirstOrDefault().Id,
                     CustomerName = l.Customers.FirstOrDefault().Name
                 }).FirstOrDefaultAsync();
        }

        public async Task<CustomerLectureGetDTO> PostCustomerLecture(CustomerLecturePostDTO customerLectureDetails)
        {
            Customer customer = await _dbContext.Customers.Where(c => c.Id == customerLectureDetails.CustomerId).FirstOrDefaultAsync();
            Lecture lecture = await _dbContext.Lectures.Where(l => l.Id == customerLectureDetails.LectureId).FirstOrDefaultAsync();
            customer.Lectures.Add(lecture);
            await _dbContext.SaveChangesAsync();
            return await GetCustomerLecture(customer.Id, lecture.Id);
        }
    }
}
