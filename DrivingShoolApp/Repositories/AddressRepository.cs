using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolApp.Repositories
{
    public interface IAddressRepository
    {
        public Task<ICollection<AddressGetDTO>> GetAddresses();
        public Task<AddressGetDTO> GetAddress(int addressId);
        public Task<Address> PostAddress(AddressPostDTO addressDetails);
    }
    public class AddressRepository : IAddressRepository
    {
        private readonly DrivingSchoolDbContext _dbContext;

        public AddressRepository(DrivingSchoolDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ICollection<AddressGetDTO>> GetAddresses()
        {
            return await _dbContext.Addresses.Select(a => new AddressGetDTO
                                                {
                                                    Id = a.Id,
                                                    City = a.City,
                                                    Street = a.Street,
                                                    Number = a.Number,
                                                    PostalCode = a.PostalCode
                                                 }).ToListAsync();
        }

        public async Task<AddressGetDTO> GetAddress(int addressId)
        {
            return await _dbContext.Addresses
                .Where(a => a.Id == addressId)
                .Select(a => new AddressGetDTO
                {
                    Id = a.Id,
                    City = a.City,
                    Street = a.Street,
                    Number = a.Number,
                    PostalCode = a.PostalCode
                }).FirstOrDefaultAsync();
        }

        public async Task<Address> PostAddress(AddressPostDTO addressDetails)
        {
            var addressToAdd = new Address
            {
                City = addressDetails.City,
                Street = addressDetails.Street,
                Number = addressDetails.Number,
                PostalCode = addressDetails.PostalCode
            };
            await _dbContext.Addresses.AddAsync(addressToAdd);
            await _dbContext.SaveChangesAsync();
            return addressToAdd;
        }
    }
}
