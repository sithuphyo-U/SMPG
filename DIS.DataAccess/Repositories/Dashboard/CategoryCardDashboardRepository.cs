//using DIS.DataAccess.Entity.Dashboard;
//using DIS.DataAccess.Interfaces.Dashboard;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace DIS.DataAccess.Repositories.Dashboard
//{
//    public class CategoryCardDashboardRepository: ICategoryCardDashboardRepository
//    {
//        private readonly DbContext _context;

//        public CategoryCardDashboardRepository(IDbContext context) 
//        {
//            _context = (DbContext)context;
//        }

//        public async Task<List<CategoryCardDashboard>> GetDisasterCountAsync()
//        {
//            return await _context.Set<CategoryCardDashboard>().FromSqlRaw("EXEC GetDisasterCountByCategory").ToListAsync();
//        }
//    }
//}
