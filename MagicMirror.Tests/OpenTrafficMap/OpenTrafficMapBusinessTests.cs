﻿using MagicMirror.Business.Models;
using MagicMirror.Business.Services;
using MagicMirror.DataAccess.Entities.Entities;
using MagicMirror.DataAccess.Entities.OpenMap;
using MagicMirror.DataAccess.Repos;
using Moq;
using Xunit;

namespace MagicMirror.Tests.OpenTrafficMap
{
    public class OpenTrafficMapBusinessTests
    {
        private IOpenTrafficService _service;

        /// Mock Data
        private const int Duration = 42;

        private const int Distance = 76;
        private const string Origin = "London, Uk";
        private const string Destination = "Leeds, Uk";

        public OpenTrafficMapBusinessTests()
        {
            var mockRepo = new Mock<ITrafficRepo>();
            _service = new OpenTrafficService(mockRepo.Object);
        }

        [Fact]
        public void Can_Map_From_Entity()
        {
            // Arrange
            OpenTrafficMapEntity entity = GetMockEntity();

            // Act
            TrafficModel model = _service.MapFromEntity(entity);

            // Assert
            Assert.Equal(Distance, model.Distance);
            Assert.Equal(Duration, model.Duration);
            Assert.Equal(Destination, model.Destination);
            Assert.Equal(Origin, model.Origin);
        }

        private OpenTrafficMapEntity GetMockEntity()
        {
            var start = new Location { street = Origin };
            var destination = new Location { street = Destination };

            var route = new Route
            {
                distance = Distance,
                time = Duration,
                locations = new Location[] { start, destination }
            };

            var mockEntity = new OpenTrafficMapEntity
            {
                Route = route
            };

            return mockEntity;
        }
    }
}