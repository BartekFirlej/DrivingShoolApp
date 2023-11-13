using DrivingSchoolApp.DTOs;
using DrivingSchoolApp.Exceptions;

namespace DrivingSchoolAppTests.Models
{
    [TestClass]
    public class PagedListTests
    {
        [TestMethod]
        public async Task PagedList_ThrowsPageIndexMustBeGreaterThanZeroException()
        {
            var source = Enumerable.Range(1, 10).AsQueryable();
            var pageIndex = -1;
            var pageSize = 5;

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await PagedList<int>.Create(source, pageIndex, pageSize));
        }

        [TestMethod]
        public async Task PagedList_ThrowsPageSizeMustBeGreaterThanZeroException()
        {
            var source = Enumerable.Range(1, 10).AsQueryable();
            var pageIndex = 1;
            var pageSize = -5;

            await Assert.ThrowsExceptionAsync<ValueMustBeGreaterThanZeroException>(async () => await PagedList<int>.Create(source, pageIndex, pageSize));
        }
    }
}
