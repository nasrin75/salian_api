using Microsoft.EntityFrameworkCore;
using salian_api.Dtos.Equipment;
using salian_api.Entities;
using salian_api.Interface;
using salian_api.Response;
using salian_api.Response.Equipment;
using salian_api.Response.Feature;

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
                    IsShowInMenu = param.IsShowInMenu,
                };

                var newEquipment = _dbContex.Equipments.Add(equipment).Entity;
                await _dbContex.SaveChangesAsync();

                EquipmentResponse response = new EquipmentResponse
                {
                    Id = newEquipment.Id,
                    Name = newEquipment.Name,
                    Type = newEquipment.Type,
                    IsShowInMenu = newEquipment.IsShowInMenu,
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
            var Equipment = await _dbContex.Equipments.FirstOrDefaultAsync(l => l.Id == id);
            if (Equipment == null)
                return new BaseResponse<EquipmentResponse?>(null, 400, "EQUIPMENT_NOT_FOUND");

            Equipment.DeletedAt = DateTime.UtcNow;

            _dbContex.Equipments.Update(Equipment);
            await _dbContex.SaveChangesAsync();

            return new BaseResponse<EquipmentResponse?>(
                null,
                200,
                "Equipment Successfully Is Deleted"
            );
        }

        public async Task<BaseResponse<List<EquipmentListResponse>>> GetAll()
        {
            List<EquipmentListResponse> Equipments = await _dbContex
                .Equipments.AsNoTracking()
                .OrderByDescending(x => x.Id)
                .Select(e => new EquipmentListResponse
                {
                    Id = e.Id,
                    Name = e.Name,
                    Type = e.Type,
                })
                .ToListAsync();

            return new BaseResponse<List<EquipmentListResponse>>(Equipments);
        }

        public async Task<BaseResponse<EquipmentResponse?>> GetByID(long EquipmentID)
        {
            var Equipment = await _dbContex
                .Equipments.AsNoTracking()
                .Select(item => new EquipmentResponse
                {
                    Id = item.Id,
                    Name = item.Name,
                    Type = item.Type,
                    IsShowInMenu = item.IsShowInMenu,
                })
                .FirstOrDefaultAsync(e => e.Id == EquipmentID);

            if (Equipment == null)
                return new BaseResponse<EquipmentResponse?>(null, 400, "EQUIPMENT_NOT_FOUND");

            return new BaseResponse<EquipmentResponse>(Equipment);
        }

        public async Task<BaseResponse<List<FeatureResponse>>> GetFeaturesByEquipmentID(long id)
        {
            var equipment = await _dbContex
                .Equipments.Include("Features")
                .FirstOrDefaultAsync(x => x.Id == id);
            if (equipment == null)
                return new BaseResponse<List<FeatureResponse>>(null, 400, "EQUIPMENT_NOT_FOUND");

            var response = equipment
                .Features.Select(f => new FeatureResponse { Id = f.Id, Name = f.Name })
                .ToList();

            return new BaseResponse<List<FeatureResponse>>(response);
        }

        public async Task<BaseResponse<List<EquipmentResponse>>> Search(SearchEquipmentDto param)
        {
            var query = _dbContex.Equipments.AsQueryable();
            if (!string.IsNullOrWhiteSpace(param.Name))
                query = query.Where(x => x.Name.Contains(param.Name));
            if (param.Type != null)
                query = query.Where(l => l.Type == param.Type);

            List<EquipmentResponse> equipments = await query
                .Select(l => new EquipmentResponse
                {
                    Id = l.Id,
                    Name = l.Name,
                    Type = l.Type,
                    IsShowInMenu = l.IsShowInMenu,
                })
                .ToListAsync();

            return new BaseResponse<List<EquipmentResponse>>(equipments);
        }

        public async Task<BaseResponse<EquipmentResponse?>> Update(EquipmentUpdateDto param)
        {
            var equipment = await _dbContex.Equipments.FirstOrDefaultAsync(l => l.Id == param.Id);

            if (equipment == null)
                return new BaseResponse<EquipmentResponse?>(null, 400, "EQUIPMENT_NOT_FOUND");

            if (param.Name != null && param.Name != equipment.Name)
                equipment.Name = param.Name;
            if (param.Type != null && param.Type != equipment.Type)
                equipment.Type = param.Type;
            if (param.IsShowInMenu != null && param.IsShowInMenu != equipment.IsShowInMenu)
                equipment.IsShowInMenu = param.IsShowInMenu;

            _dbContex.Update(equipment);
            await _dbContex.SaveChangesAsync();

            var response = new EquipmentResponse
            {
                Id = equipment.Id,
                Name = equipment.Name,
                Type = equipment.Type,
                IsShowInMenu = equipment.IsShowInMenu,
            };

            return new BaseResponse<EquipmentResponse?>(response);
        }

        public async Task<BaseResponse<List<string>>> GetInventorySubMenu()
        {
            var subMenus = await _dbContex
                .Equipments.Where(x => x.IsShowInMenu == true)
                .Select(x => x.Name)
                .ToListAsync();

            return new BaseResponse<List<string>>(subMenus);
        }
    }
}
