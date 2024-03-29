﻿using Infrastructure.Repositories.Contracts;
using Domain.Dto;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Domain.Dto.Stores;
using Common.Models;
using Microsoft.AspNetCore.Http;

namespace Application.Queries.Stores
{
    public static class GetAllStoreLocationData
    {
        // Query
        public record Query() : IRequest<Response>;


        // Handler
        public class Handler : IRequestHandler<Query, Response>
        {
            protected IHttpContextAccessor _httpContextAccessor { get; set; }
            protected IStoreRepository _storeRepository { get; set; }

            public Handler(IHttpContextAccessor httpContextAccessor,
                IStoreRepository storeRepository)
            {
                _httpContextAccessor = httpContextAccessor;
                _storeRepository = storeRepository;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var response = new ApiResponseModel<IEnumerable<StoreLocationGroup>>();

                var loggedInUserCompanyId = _httpContextAccessor.HttpContext.User.FindFirst("CompanyId")?.Value;

                var allStoresGrouped = _storeRepository
                        .GetAllQueryable()
                            .Include(x => x.StoreLocation)
                        .Where(x => x.CompanyId == Guid.Parse(loggedInUserCompanyId))
                        .GroupBy(x => x.StoreLocation.Country)
                        .ToList();

                var data = allStoresGrouped.Select(x => new StoreLocationGroup
                {
                    Id = x.FirstOrDefault().StoreLocation.Id,
                    Country = x.FirstOrDefault().StoreLocation.Country,
                    CountryCode = x.FirstOrDefault().StoreLocation.CountryCode,
                    Stores = x.Select(r => new StoreLocationInfo
                    {
                        StoreName = r.Name,
                        Lat = r.StoreLocation.LatLng.Split(" ")[0],
                        Lng = r.StoreLocation.LatLng.Split(" ")[1]
                    }).ToList()
                });

                response.Data = data;

                return new Response(response);
            }
        }


        // Response
        public record Response(ApiResponseModel<IEnumerable<StoreLocationGroup>> response);
    }
}

