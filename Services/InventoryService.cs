using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using salian_api.Dtos.Inventory;
using salian_api.Entities;
using salian_api.Interface;
using salian_api.Response;
using salian_api.Response.Inventory;

namespace salian_api.Services
{
    public class InventoryService(ApplicationDbContext _dbContex) : IInventoryService
    {
        public async Task<BaseResponse<InventoryResponse>> Create([FromBody] InventoryCreateDto param)
        {
           InventoryEntity data = new InventoryEntity
            {
                ItNumber = param.ItNumber,
                ItParentNumber = param.ItParentNumber,
                UserId = 1,//param.UserId, // TODO:get login user
                EmployeeId = param.EmployeeId,
                EquipmentId = param.EquipmentId,
                LocationId = param.LocationId,
                Status = param.Status,
                PropertyNumber = param.PropertyNumber,
                SerialNumber = param.SerialNumber,
                InvoiceNumber = param.InvoiceNumber,
                InvoiceImage = param.InvoiceImage,
                Size = param.Size,
                Capacity = param.Capacity,
                BrandName = param.BrandName,
                ModelName = param.ModelName,
                DeliveryDate = param.DeliveryDate,
                Description = param.Description,
                ExpireWarrantyDate = param.ExpireWarrantyDate,
            };

            var inventory = _dbContex.Inventories.Add(data).Entity;
            await  _dbContex.SaveChangesAsync();
            // Add Features
            if (param.Features != null && param.Features.Any())
            {
                var features = param.Features.Select(f => new InventoryFeatureEntity
                {
                    InventoryId = inventory.Id,
                    FeatureId = f.FeatureId,
                    Value = f.Value
                }).ToList();

                _dbContex.InventoryFeatures.AddRange(features);
                await _dbContex.SaveChangesAsync();
            }
            InventoryResponse response = new InventoryResponse
            {
                Id = inventory.Id,
                ItNumber = inventory.ItNumber,
                ItParentNumber = inventory.ItParentNumber,
                UserId = inventory.UserId, // TODO:get login user
                EmployeeId = inventory.EmployeeId,
                EquipmentId = inventory.EquipmentId,
                LocationId = inventory.LocationId,
                Status = inventory.Status,
                PropertyNumber = inventory.PropertyNumber,
                SerialNumber = inventory.SerialNumber,
                InvoiceNumber = inventory.InvoiceNumber,
                InvoiceImage = inventory.InvoiceImage,
                Size = inventory.Size,
                Capacity = inventory.Capacity,
                BrandName = inventory.BrandName,
                ModelName = inventory.ModelName,
                DeliveryDate = inventory.DeliveryDate,
                Description = inventory.Description,
                ExpireWarrantyDate = inventory.ExpireWarrantyDate,
            };

            return new BaseResponse<InventoryResponse>(response);
        }

        public async Task<BaseResponse> Delete(long id)
        {
            var inventory = await _dbContex.Inventories
                .FirstOrDefaultAsync(l => l.Id == id);
            if (inventory == null) return new BaseResponse<InventoryResponse?>(null, 400, "Inventory Not Found");

            inventory.DeletedAt = DateTime.Now;
            inventory.UpdatedAt = DateTime.Now;

            _dbContex.Update(inventory);
            await _dbContex.SaveChangesAsync();

            return new BaseResponse<InventoryResponse>(null, 200, "Inventory Successfully Is Deleted");
        }

        public async Task<BaseResponse<List<InventoryListResponse>>> GetAll(string equipment)
        {
            var query =  _dbContex.Inventories.Include(x => x.Equipment).AsQueryable();

            if (!string.IsNullOrEmpty(equipment) && equipment.Trim() != "ALL")
            {
                query = query.Where(x => x.Equipment.Name.ToLower().Trim() == equipment.ToLower().Trim());
            }

            List<InventoryListResponse> inventories =await query.AsNoTracking()
                .OrderByDescending(x => x.Id)
                .Select(param => new InventoryListResponse
                {
                    Id = param.Id,
                    ItNumber = param.ItNumber,
                    ItParentNumber = param.ItParentNumber,
                    User = param.User.Username,
                    Employee = param.Employee.Name,
                    Equipment = param.Equipment.Name,
                    Location = param.Location.Title,
                    Status = param.Status,
                    PropertyNumber = param.PropertyNumber,
                    SerialNumber = param.SerialNumber,
                    InvoiceNumber = param.InvoiceNumber,
                    InvoiceImage = param.InvoiceImage,
                    Size = param.Size,
                    Capacity = param.Capacity,
                    BrandName = param.BrandName,
                    ModelName = param.ModelName,
                    DeliveryDate = param.DeliveryDate,
                    Description = param.Description,
                    ExpireWarrantyDate = param.ExpireWarrantyDate,
                    UpdatedAt = param.UpdatedAt,
                    Features = param.Features
                }).ToListAsync();

            
            return new BaseResponse<List<InventoryListResponse>>(inventories);
        }

        public async Task<BaseResponse<InventoryResponse?>> GetByID(long Id)
        {
            var inventory = await _dbContex.Inventories.FirstOrDefaultAsync(x => x.Id == Id);
            if (inventory == null)
                return new BaseResponse<InventoryResponse?>(null, 400, "Inventory Not Exists");

            var response = new InventoryResponse
            {
                Id = inventory.Id,
                ItNumber = inventory.ItNumber,
                ItParentNumber = inventory.ItParentNumber,
                UserId = inventory.UserId,
                EmployeeId = inventory.EmployeeId,
                EquipmentId = inventory.EquipmentId,
                LocationId = inventory.LocationId,
                Status = inventory.Status,
                PropertyNumber = inventory.PropertyNumber,
                SerialNumber = inventory.SerialNumber,
                InvoiceNumber = inventory.InvoiceNumber,
                InvoiceImage = inventory.InvoiceImage,
                Size = inventory.Size,
                Capacity = inventory.Capacity,
                BrandName = inventory.BrandName,
                ModelName = inventory.ModelName,
                DeliveryDate = inventory.DeliveryDate,
                Description = inventory.Description,
                ExpireWarrantyDate = inventory.ExpireWarrantyDate,
            };

            return new BaseResponse<InventoryResponse?>(response);
        }

        public async Task<BaseResponse<List<InventoryListResponse>>> Search(SearchInventoryDto param)
        {
            var query = _dbContex.Inventories.AsQueryable();

            if (param.ItNumber != null)
            {
                query = query.Where(x => x.ItNumber == param.ItNumber);
            }
            if (param.ItParentNumber != null) query = query.Where(x => x.ItParentNumber == param.ItParentNumber);

            if (param.Status != null) query = query.Where(x => x.Status == (StatusMap)param.Status); // TODO: don't fillter
        

            if (!string.IsNullOrWhiteSpace(param.PropertyNumber)) query = query.Where(x => x.PropertyNumber == param.PropertyNumber);
            if (!string.IsNullOrWhiteSpace(param.SerialNumber)) query = query.Where(x => x.SerialNumber == param.SerialNumber);
            if (!string.IsNullOrWhiteSpace(param.InvoiceNumber)) query = query.Where(x => x.InvoiceNumber == param.InvoiceNumber);
            if (!string.IsNullOrWhiteSpace(param.BrandName)) query = query.Where(x => x.BrandName.Contains(param.BrandName));
            if (!string.IsNullOrWhiteSpace(param.ModelName)) query = query.Where(x => x.ModelName.Contains(param.ModelName));

            //TODO ::: Add filter user.name , employee.name , equipment.name
            if (param.StartDate != null) query = query.Where(x => x.UpdatedAt >= param.StartDate); //TODO edit format to startOfDay
            if (param.EndDate != null) query = query.Where(x => x.UpdatedAt <= param.EndDate); //TODO edit format to endOfDay


            List<InventoryListResponse> inventories = await query.Select(item => new InventoryListResponse
            {
                Id = item.Id,
                ItNumber = item.ItNumber,
                ItParentNumber = item.ItParentNumber,
                User = item.User.Username,
                Employee = item.Employee.Name,
                Equipment = item.Equipment.Name,
                Location = item.Location.Title,
                Status = item.Status,
                PropertyNumber = item.PropertyNumber,
                SerialNumber = item.SerialNumber,
                InvoiceNumber = item.InvoiceNumber,
                InvoiceImage = item.InvoiceImage,
                Size = item.Size,
                Capacity = item.Capacity,
                BrandName = item.BrandName,
                ModelName = item.ModelName,
                DeliveryDate = item.DeliveryDate,
                Description = item.Description,
                ExpireWarrantyDate = item.ExpireWarrantyDate,
            }).ToListAsync();

            return new BaseResponse<List<InventoryListResponse>>(inventories);
        }

        public async Task<BaseResponse<InventoryResponse?>> Update(InventoryUpdateDto param)
        {
            var inventory = await _dbContex.Inventories
              .FirstOrDefaultAsync(l => l.Id == param.Id);

            if (inventory == null) 
                return new BaseResponse<InventoryResponse?>(null, 400, "Inventory Not Found");

            if (param.UserId != null && param.UserId != inventory.UserId) inventory.UserId = param.UserId.Value;
            if (param.EmployeeId != null && param.EmployeeId != inventory.EmployeeId) inventory.EmployeeId = param.EmployeeId.Value;
            if (param.EquipmentId != null && param.EquipmentId != inventory.EquipmentId) inventory.EquipmentId = param.EquipmentId.Value;
            if (param.LocationId != null && param.LocationId != inventory.LocationId) inventory.LocationId = param.LocationId.Value;

            if (param.ItNumber != null && param.ItNumber != inventory.ItNumber) inventory.ItNumber = param.ItNumber;
            if (param.ItParentNumber != null && param.ItParentNumber != inventory.ItParentNumber) inventory.ItParentNumber = param.ItParentNumber.Value;

            if (param.Status != null && param.Status != inventory.Status) inventory.Status = param.Status.Value;
            if (param.PropertyNumber != null && param.PropertyNumber != inventory.PropertyNumber) inventory.PropertyNumber = param.PropertyNumber;
            if (param.SerialNumber != null && param.SerialNumber != inventory.SerialNumber) inventory.SerialNumber = param.SerialNumber;
            if (param.InvoiceNumber != null && param.InvoiceNumber != inventory.InvoiceNumber) inventory.InvoiceNumber = param.InvoiceNumber;
            if (param.InvoiceImage != null && param.InvoiceImage != inventory.InvoiceImage) inventory.InvoiceImage = param.InvoiceImage;
            if (param.ModelName != null && param.ModelName != inventory.ModelName) inventory.ModelName = param.ModelName;
            if (param.BrandName != null && param.BrandName != inventory.BrandName) inventory.BrandName = param.BrandName;
            if (param.Size != null && param.Size != inventory.Size) inventory.Size = param.Size;
            if (param.Capacity != null && param.Capacity != inventory.Capacity) inventory.Capacity = param.Capacity;
            if (param.Description != null && param.Description != inventory.Description) inventory.Description = param.Description;
            if (param.DeliveryDate != null && param.DeliveryDate != inventory.DeliveryDate) inventory.DeliveryDate = param.DeliveryDate;
            if (param.ExpireWarrantyDate != null && param.ExpireWarrantyDate != inventory.ExpireWarrantyDate) inventory.ExpireWarrantyDate = param.ExpireWarrantyDate;

            inventory.UpdatedAt = DateTime.Now;
            _dbContex.Update(inventory);
            await _dbContex.SaveChangesAsync();

            var response = new InventoryResponse
            {
                Id = inventory.Id,
                ItNumber = inventory.ItNumber,
                ItParentNumber = inventory.ItParentNumber,
                UserId = inventory.UserId,
                EmployeeId = inventory.EmployeeId,
                EquipmentId = inventory.EquipmentId,
                LocationId = inventory.LocationId,
                Status = inventory.Status,
                PropertyNumber = inventory.PropertyNumber,
                SerialNumber = inventory.SerialNumber,
                InvoiceNumber = inventory.InvoiceNumber,
                InvoiceImage = inventory.InvoiceImage,
                Size = inventory.Size,
                Capacity = inventory.Capacity,
                BrandName = inventory.BrandName,
                ModelName = inventory.ModelName,
                DeliveryDate = inventory.DeliveryDate,
                Description = inventory.Description,
                ExpireWarrantyDate = inventory.ExpireWarrantyDate,
            };

            return new BaseResponse<InventoryResponse?>(response);
        }
    }
}
