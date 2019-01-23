using System;
using Xunit;
using NSubstitute;
using System.Collections.Generic;

namespace TNDStudios.Testing.NSubstitute.Tests
{
    /// <summary>
    /// Interface to handle patterns for domain objects
    /// </summary>
    public interface IDomainObject
    {
        Int32 Id { get; set; } // All domain objects need an identity field
    }

    /// <summary>
    /// A concrete implementation of the domain object interface
    /// </summary>
    public class DomainObject : IDomainObject
    {
        public Int32 Id { get; set; } // All domain objects need an identity field
    }

    /// <summary>
    /// Interface used to mock up a service bus implementation
    /// </summary>
    public interface IServiceBusHandler<T> where T : IDomainObject
    {
        /// <summary>
        /// Get the content of the message and it's sequence number
        /// </summary>
        /// <returns>List of content with the sequence number as the key</returns>
        List<KeyValuePair<Int64, T>> Get();
    }

    /// <summary>
    /// Example Tests
    /// </summary>
    public class UnitTests
    {
        [Fact]
        public void ExampleTest()
        {
            // Arrange
            IServiceBusHandler<IDomainObject> mockedServiceBusHandler =
                Substitute.For<IServiceBusHandler<IDomainObject>>();

            mockedServiceBusHandler
                .Get()
                .Returns(
                    new List<KeyValuePair<Int64, IDomainObject>>()
                    {
                        new KeyValuePair<Int64, IDomainObject>(1, new DomainObject(){ Id = 1 }),
                        new KeyValuePair<Int64, IDomainObject>(2, new DomainObject(){ Id = 2 })
                    }); // Mock up the get end point for the service bus handler

            List<KeyValuePair<Int64, IDomainObject>> result = null; // Invalid results set to start

            // Act
            result = mockedServiceBusHandler.Get(); // Execute the get to get the mocked up service bus items

            // Assert
            Assert.NotNull(result); // The result set was populated at least
            Assert.Equal(2, result.Count); // Correct amount of rows?         
        }
    }
}
