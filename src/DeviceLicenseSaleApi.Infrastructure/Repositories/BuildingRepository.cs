using DeviceLicenseSaleApi.Models;
using DeviceLicenseSaleApi.Data;
using Microsoft.EntityFrameworkCore;

namespace DeviceLicenseSaleApi.Repositories
{
    public class BuildingRepository : IBuildingRepository
    {
        private readonly AppDbContext _context;

        public BuildingRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Building> GetAll()
        {
            return _context.Buildings
                .Include(b => b.Company)
                .ToList();
        }

        public Building GetById(int id)
        {
            return _context.Buildings
                .Include(b => b.Company)
                .FirstOrDefault(b => b.Id == id);
        }

        public bool Exists(int id)
        {
            return _context.Buildings.Any(x => x.Id == id);
        }

        public void Add(Building building)
        {
            _context.Buildings.Add(building);
            _context.SaveChanges();
        }

        public void Update(Building building)
        {
            _context.Buildings.Update(building);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var building = _context.Buildings.Find(id);

            if (building != null)
            {
                _context.Buildings.Remove(building);
                _context.SaveChanges();
            }
        }
    }
}
