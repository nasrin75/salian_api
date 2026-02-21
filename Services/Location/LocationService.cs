using Microsoft.EntityFrameworkCore;
using salian_api.Dtos.Location;
using salian_api.Entities;
using salian_api.Infrastructure.Data;
using salian_api.Response;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace salian_api.Services.Location
{
    public class LocationService(ApplicationDbContext _dbContex) : ILocationService
    {
        public async Task<BaseResponse<LocationResponse>> Create(LocationCreateDto dto)
        {
            LocationEntity location = new LocationEntity
            {
                Title = dto.Title,
                Abbreviation = dto.Abbreviation,
                IsShow = dto.IsShow,
            };

            var newLocation = _dbContex.Locations.Add(location).Entity;
            await _dbContex.SaveChangesAsync();

            LocationResponse response = new LocationResponse
            {
                Id = newLocation.Id,
                Title = newLocation.Title,
                Abbreviation = newLocation.Abbreviation,
                IsShow = newLocation.IsShow,
            };

            return new BaseResponse<LocationResponse>(response);
        }

        public async Task<BaseResponse> Delete(long id)
        {
            var location = await _dbContex.Locations.FirstOrDefaultAsync(l => l.Id == id);
            if (location == null)
                return new BaseResponse<LocationResponse?>(null, 400, "LOCATION_NOT_FOUND");

            location.DeletedAt = DateTime.UtcNow;

            _dbContex.Locations.Update(location);
            await _dbContex.SaveChangesAsync();

            return new BaseResponse<LocationResponse?>(
                null,
                200,
                "Location Successfully Is Deleted"
            );
        }

        public async Task<BaseResponse<List<LocationResponse>>> GetAll(string status)
        {
            var query = _dbContex.Locations.AsQueryable();

            Console.WriteLine("upppp ::: " + status);
            if (!string.IsNullOrEmpty(status) && status != "ALL")
            {
                Console.WriteLine("inside ::: " + status);
                query.Where(x => x.IsShow != true);
            }

            /*       List<LocationResponse> locations = await _dbContex.Locations
                       .Where(x => x.IsShow == isShow)
                        .OrderByDescending(x => x.Id)
                       .Select(l => new LocationResponse
                   {
                       Id = l.Id,
                       Title = l.Title,
                       Abbreviation = l.Abbreviation,
                       IsShow = l.IsShow,
                   })
                       .ToListAsync();*/

            List<LocationResponse> locations = await _dbContex
                .Locations.Select(l => new LocationResponse
                {
                    Id = l.Id,
                    Title = l.Title,
                    Abbreviation = l.Abbreviation,
                    IsShow = l.IsShow,
                })
                .ToListAsync();
            return new BaseResponse<List<LocationResponse>>(locations);
        }

        public async Task<BaseResponse<LocationResponse?>> GetByID(long locationID)
        {
            var location = await _dbContex.Locations.FirstOrDefaultAsync(l => l.Id == locationID);
            if (location == null)
                return new BaseResponse<LocationResponse?>(null, 400, "LOCATION_NOT_FOUND");

            var response = new LocationResponse
            {
                Id = location.Id,
                Title = location.Title,
                Abbreviation = location.Abbreviation,
                IsShow = location.IsShow,
            };

            return new BaseResponse<LocationResponse>(response);
        }

        public async Task<BaseResponse<LocationResponse?>> Update(LocationUpdateDto param)
        {
            var location = await _dbContex.Locations.FirstOrDefaultAsync(l => l.Id == param.Id);

            if (location == null)
                return new BaseResponse<LocationResponse?>(null, 400, "LOCATION_NOT_FOUND");

            if (param.Title != null && param.Title != location.Title)
                location.Title = param.Title;
            if (param.Abbreviation != null && param.Abbreviation != location.Abbreviation)
                location.Abbreviation = param.Abbreviation;
            if (param.IsShow != null && param.IsShow != location.IsShow)
                location.IsShow = param.IsShow.Value;

            _dbContex.Locations.Update(location);
            await _dbContex.SaveChangesAsync();

            LocationResponse response = new LocationResponse
            {
                Id = location.Id,
                Title = location.Title,
                Abbreviation = location.Abbreviation,
                IsShow = location.IsShow,
            };

            return new BaseResponse<LocationResponse?>(response);
        }

        public async Task<BaseResponse<List<LocationResponse>>> Search(SearchLocationDto param)
        {
            var query = _dbContex.Locations.AsQueryable();
            if (!string.IsNullOrWhiteSpace(param.Title))
                query = query.Where(x => x.Title.Contains(param.Title));
            if (!string.IsNullOrWhiteSpace(param.Abbreviation))
                query = query.Where(l => l.Abbreviation == param.Abbreviation);

            List<LocationResponse> locations = await query
                .Select(l => new LocationResponse
                {
                    Id = l.Id,
                    Title = l.Title,
                    Abbreviation = l.Abbreviation,
                    IsShow = l.IsShow,
                })
                .ToListAsync();

            return new BaseResponse<List<LocationResponse>>(locations);
        }
    }
}
