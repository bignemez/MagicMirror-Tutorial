﻿using MagicMirror.DataAccess.Configuration;
using MagicMirror.DataAccess.Entities.Entities;
using MagicMirror.DataAccess.Entities.Traffic;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MagicMirror.DataAccess.Repos
{
    public class GoogleMapsRepo : Repository<GoogleMapsEntity>, ITrafficRepo
    {
        private string _start;
        private string _destination;

        public async Task<TrafficEntity> GetTrafficInfoAsync(string start, string destination)
        {
            FillInputData(start, destination);
            HttpResponseMessage message = await GetHttpResponseMessageAsync();
            GoogleMapsEntity entity = await GetEntityFromJsonAsync(message);

            if (entity.Rows[0].Elements[0].Distance == null)
            {
                throw new HttpRequestException("Unable to retrieve traffic information");
            }

            return entity;
        }

        private void FillInputData(string start, string destination)
        {
            _apiId = DataAccessConfig.GoogleMapsApiId;
            _apiUrl = DataAccessConfig.GoogleMapsApiUrl;
            _start = start;
            _destination = destination;

            ValidateInput();

            _url = $"{_apiUrl}?origins={start}&destinations={destination}&key={_apiId}";
        }

        protected override void ValidateInput()
        {
            base.ValidateInput();
            if (string.IsNullOrWhiteSpace(_start)) { throw new ArgumentNullException("A start location has to be provided"); }
            if (string.IsNullOrWhiteSpace(_destination)) { throw new ArgumentNullException("A destination has to be provided"); }
        }
    }
}