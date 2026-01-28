using Microsoft.EntityFrameworkCore;
using salian_api.Dtos.Equipment;
using salian_api.Entities;
using salian_api.Interface;
using salian_api.Response;
using salian_api.Routes;


namespace salian_api.Services
{
    public class EquipmentService(ApplicationDbContext _dbContex) : IEquipmentService
    {
        public async Task<BaseResponse<EquipmentResponse>> Create(EquipmentCreateDto param)
        {
            try
            {
                EquipmentEntity equipment = new EquipmentEntity
                {
                    Name = param.Name,
                    Type = param.Type,
                };

                var newEquipment = _dbContex.Equipments.Add(equipment).Entity;
                await _dbContex.SaveChangesAsync();

                EquipmentResponse response = new EquipmentResponse
                {
                    Id = newEquipment.Id,
                    Name = newEquipment.Name,
                    Type = newEquipment.Type,
                };

                return new BaseResponse<EquipmentResponse>(response);
            }
            catch (Exception ex)
            {
                return new BaseResponse<EquipmentResponse>(null, 400, ex.Message);
            }
        }

        public async Task<BaseResponse> Delete(long id)
        {
            var Equipment = await _dbContex.Equipments
              .FirstOrDefaultAsync(l => l.Id == id);
            if (Equipment == null) return new BaseResponse<EquipmentResponse?>(null, 400, "Equipment Not Found");

            Equipment.DeletedAt = DateTime.UtcNow;

            _dbContex.Equipments.Update(Equipment);
            await _dbContex.SaveChangesAsync();

            return new BaseResponse<EquipmentResponse?>(null, 200, "Equipment Successfully Is Deleted");
        }

        public async Task<BaseResponse<List<EquipmentResponse>>> GetAll()
        {
            List<EquipmentResponse> Equipments = await _dbContex.Equipments
                .AsNoTracking()
                .Select(e => new EquipmentResponse
                {
                    Id = e.Id,
                    Name = e.Name,
                    Type = e.Type,
                })
                .ToListAsync();

            return new BaseResponse<List<EquipmentResponse>>(Equipments);
        }

        public async Task<BaseResponse<EquipmentResponse?>> GetByID(long EquipmentID)
        {
            var Equipment = await _dbContex.Equipments.AsNoTracking()
                .Select(item => new EquipmentResponse
                {
                    Id = item.Id,
                    Name = item.Name,
                    Type = item.Type,
                })
                .FirstOrDefaultAsync(e => e.Id == EquipmentID);

            if (Equipment == null) return new BaseResponse<EquipmentResponse?>(null, 400, "Equipment Not Found");

            return new BaseResponse<EquipmentResponse>(Equipment);
        }

        public async Task<BaseResponse<List<FeatureResponse>>> GetFeaturesByEquipmentID(long id)
        {
            var equipment = await _dbContex.Equipments.Include("Features").FirstOrDefaultAsync(x => x.Id == id);
            if (equipment == null) return new BaseResponse<List<FeatureResponse>>(null, 400, "Equipment Not Found");

           var response = equipment.Features.Select(f => new FeatureResponse
            {
                Id = f.Id,
                Name = f.Name,

            }).ToList();

            return new BaseResponse<List<FeatureResponse>>(response);
        }

        public async Task<BaseResponse<List<EquipmentResponse>>> Search(SearchEquipmentDto param)
        {
            var query = _dbContex.Equipments.AsQueryable();
            if (!string.IsNullOrWhiteSpace(param.Name)) query = query.Where(x => x.Name.Contains(param.Name));
            if (param.Type != null) query = query.Where(l => l.Type == param.Type);

            List<EquipmentResponse> equipments = await query.Select(l => new EquipmentResponse
            {
                Id = l.Id,
                Name = l.Name,
                Type = l.Type,
            }).ToListAsync();

            return new BaseResponse<List<EquipmentResponse>>(equipments);
        }

        public async Task<BaseResponse<EquipmentResponse?>> Update(EquipmentUpdateDto param)
        {
            var equipment = await _dbContex.Equipments
               .FirstOrDefaultAsync(l => l.Id == param.Id);

            if (equipment == null) return new BaseResponse<EquipmentResponse?>(null, 400, "Equipment Not Found");

            if (param.Name != null && param.Name != equipment.Name) equipment.Name = param.Name;
            if (param.Type != null && param.Type != equipment.Type) equipment.Type = param.Type;

            _dbContex.Update(equipment);
            await _dbContex.SaveChangesAsync();

            var response = new EquipmentResponse
            {
                Id = equipment.Id,
                Name = equipment.Name,
                Type = equipment.Type,
            };

            return new BaseResponse<EquipmentResponse?>(response);
        }

        
    }
}
