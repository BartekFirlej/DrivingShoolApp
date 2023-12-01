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
        public Task<CustomerLectureCheckDTO> CheckCustomerLecture(int customerId, int lectureId);
        public Task<CustomerLectureCheckDTO> DeleteCustomerLecture(Customer customer, Lecture lectureToDelete);
        public Task<CustomerLectureResponseDTO> PostCustomerLecture(CustomerLectureRequestDTO customerLectureDetails);
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
            return await _dbContext.Lectures
                        .AsNoTracking()
                        .SelectMany(lecture => lecture.Customers, (lecture, customerLecture) => new CustomerLectureGetDTO
                        {
                            LectureId = lecture.LecturerId,
                            LectureDate = lecture.LectureDate,
                            CustomerId = customerLecture.Id,
                            CustomerName = customerLecture.Name
                        }).ToListAsync();
        }

        public async Task<ICollection<CustomerLectureGetDTO>> GetCustomerLectures(int customerId)
        {
            return await _dbContext.Customers
                    .AsNoTracking()
                    .Where(customer => customer.Id == customerId)
                    .SelectMany(customer => customer.Lectures, (customer, lecture) => new CustomerLectureGetDTO
                    {
                        LectureId = lecture.LecturerId,
                        LectureDate = lecture.LectureDate,
                        CustomerId = customer.Id,
                        CustomerName = customer.Name
                    }).ToListAsync();
        }

        public async Task<ICollection<CustomerLectureGetDTO>> GetCustomersLecture(int lectureId)
        {
            return await _dbContext.Lectures
                    .AsNoTracking()
                    .Where(lecture => lecture.Id == lectureId)
                    .SelectMany(lecture => lecture.Customers, (lecture, customer) => new CustomerLectureGetDTO
                    {
                        LectureId = lecture.LecturerId,
                        LectureDate = lecture.LectureDate,
                        CustomerId = customer.Id,
                        CustomerName = customer.Name
                    }).ToListAsync();
        }

        public async Task<CustomerLectureGetDTO> GetCustomerLecture(int customerId, int lectureId)
        {
            return await _dbContext.Lectures
                    .AsNoTracking()
                    .Where(lecture => lecture.Id == lectureId)
                    .SelectMany(lecture => lecture.Customers.Where(customer => customer.Id == customerId), (lecture, customer) => new CustomerLectureGetDTO
                    {
                        LectureId = lecture.LecturerId,
                        LectureDate = lecture.LectureDate,
                        CustomerId = customer.Id,
                        CustomerName = customer.Name
                    }).FirstOrDefaultAsync();
        }

        public async Task<CustomerLectureResponseDTO> PostCustomerLecture(CustomerLectureRequestDTO customerLectureDetails)
        {
            Customer customer = await _dbContext.Customers.Where(c => c.Id == customerLectureDetails.CustomerId).FirstOrDefaultAsync();
            Lecture lecture = await _dbContext.Lectures.Where(l => l.Id == customerLectureDetails.LectureId).FirstOrDefaultAsync();
            customer.Lectures.Add(lecture);
            await _dbContext.SaveChangesAsync();
            return new CustomerLectureResponseDTO { CustomerId = customer.Id, LectureId = lecture.Id };
        }

        public async Task<CustomerLectureCheckDTO> CheckCustomerLecture(int customerId, int lectureId)
        {
            return await _dbContext.Lectures
                        .AsNoTracking()
                        .Where(l => l.Id == lectureId && l.Customers.Any(c => c.Id == customerId))
                        .Select(l => new CustomerLectureCheckDTO
                        {
                            CustomerId = customerId,
                            LectureId = l.Id
                        })
                        .FirstOrDefaultAsync();
        }

        public async Task<CustomerLectureCheckDTO> DeleteCustomerLecture(Customer customer, Lecture lectureToDelete)
        {
            customer.Lectures.Remove(lectureToDelete);
            await _dbContext.SaveChangesAsync();
            return new CustomerLectureCheckDTO { CustomerId = customer.Id, LectureId = lectureToDelete.Id };
        }
    }
}
