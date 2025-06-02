
using DIS.DataAccess.Entity.Settings;
using DIS.Infrastructure.Common;

namespace DIS.DataAccess.Interfaces.Settings
{
    public interface IDisasterSubCategoryRepository : IReadWriteRepositoryBase<DisasterSubCategory>
    {
        DisasterSubCategory? FindByName(string name);
        List<DisasterSubCategory>? GetSubCategorybyCategory(int id);
        DisasterSubCategory Getbyid(int? id);
        List<DisasterSubCategory>? GetByName(string name);
    }
}