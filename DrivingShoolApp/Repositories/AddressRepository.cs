using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolApp.Repositories
{
    public interface IAddressRepository
    {
        public Task<PagedList<AddressGetDTO>> GetAddresses(int page, int size);
        public Task<AddressGetDTO> GetAddress(int addressId);
        public Task<Address> PostAddress(AddressPostDTO addressDetails);
        public Task<Address> CheckAddress(int addressId);
        public Task<Address> DeleteAddress(Address addressToDelete);
    }
    public class AddressRepository : IAddressRepository
    {
        private readonly DrivingSchoolDbContext _dbContext;

        public AddressRepository(DrivingSchoolDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedList<AddressGetDTO>> GetAddresses(int page, int size)
        {
            return await PagedList<AddressGetDTO>.Create(
                 _dbContext.Addresses
                 .AsNoTracking()
                 .Select(a => new AddressGetDTO
                    {
                        Id = a.Id,
                        City = a.City,
                        Street = a.Street,
                        Number = a.Number,
                        PostalCode = a.PostalCode
                    })
                 .OrderBy(a => a.Id), 
                page, size);
        }

        public async Task<AddressGetDTO> GetAddress(int addressId)
        {
            return await _dbContext.Addresses
                .AsNoTracking()
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

        public async Task<Address> CheckAddress(int addressId)
        {
            return await _dbContext.Addresses
                .AsNoTracking()
                .Where(a => a.Id == addressId)
                .FirstOrDefaultAsync();
        }

        public async Task<Address> DeleteAddress(Address addressToDelete)
        {
            var deletedAddress = _dbContext.Addresses.Remove(addressToDelete);
            await _dbContext.SaveChangesAsync();
            return deletedAddress.Entity;
        }
    } 
}
