using Microsoft.EntityFrameworkCore;
using salian_api.Dtos.Feature;
using salian_api.Entities;
using salian_api.Interface;
using salian_api.Response;

namespace salian_api.Services
{
    public class FeatureService(ApplicationDbContext _dbContex) : IFeatureService
    {
        public async Task<BaseResponse<FeatureResponse?>> Create(FeatureCreateDto param)
        {
            try
            {
                FeatureEntity feature = new FeatureEntity
                {
                    Name = param.Name,
                };

                var newFeature = _dbContex.Features.Add(feature).Entity;
                await _dbContex.SaveChangesAsync();

                FeatureResponse response = new FeatureResponse
                {
                    Id = newFeature.Id,
                    Name = newFeature.Name,
                };

                return new BaseResponse<FeatureResponse>(response);
            }catch (Exception ex)
            {
                return new BaseResponse<FeatureResponse>(null,400,ex.Message);
            }

        }

        public async Task<BaseResponse> Delete(long id)
        {
            var feature = await _dbContex.Features
                .FirstOrDefaultAsync(l => l.Id == id);
            if (feature == null) return new BaseResponse<FeatureResponse?>(null, 400, "Feature Not Found");

            feature.DeletedAt = DateTime.UtcNow;

            _dbContex.Features.Update(feature);
            await _dbContex.SaveChangesAsync();

            return new BaseResponse<FeatureResponse?>(null, 200, "Feature Successfully Is Deleted");
        }

        public async Task<BaseResponse<List<FeatureResponse>>> GetAll()
        {
            List<FeatureResponse> features = await _dbContex.Features
                .AsNoTracking()
                .Select(l => new FeatureResponse
                {
                    Id = l.Id,
                    Name = l.Name,
                })
                .ToListAsync();

            return new BaseResponse<List<FeatureResponse>>(features);
        }

        public async Task<BaseResponse<FeatureResponse?>> GetByID(long Id)
        {
            var feature = await _dbContex.Features.AsNoTracking()
                .Select(item => new FeatureResponse
                {
                    Id = item.Id,
                    Name = item.Name,
                })
                .FirstOrDefaultAsync(l => l.Id == Id);

            if (feature == null) return new BaseResponse<FeatureResponse?>(null, 400, "Feature Not Found");
            
            return new BaseResponse<FeatureResponse>(feature);
        }

        public async Task<BaseResponse<List<FeatureResponse>>> Search(SearchFeatureDto param)
        {
            var query = _dbContex.Features.AsQueryable();
            if (!string.IsNullOrWhiteSpace(param.Name)) query = query.Where(x => x.Name.Contains(param.Name));

            List<FeatureResponse> features = await query.Select(l => new FeatureResponse
            {
                Id = l.Id,
                Name = l.Name,
            }).ToListAsync();

            return new BaseResponse<List<FeatureResponse>>(features);
        }

        public async Task<BaseResponse<FeatureResponse?>> Update(FeatureUpdateDto param)
        {
            var feature = await _dbContex.Features
                .FirstOrDefaultAsync(l => l.Id == param.Id);

            if (feature == null) return new BaseResponse<FeatureResponse?>(null, 400, "Feature Not Found");

            if (param.Name != null && param.Name != feature.Name) feature.Name = param.Name;

            _dbContex.Update(feature);
            await _dbContex.SaveChangesAsync();

            var response = new FeatureResponse
            {
                Id = feature.Id,
                Name = feature.Name,

            };

            return new BaseResponse<FeatureResponse?>(response);
        }
    }
}
