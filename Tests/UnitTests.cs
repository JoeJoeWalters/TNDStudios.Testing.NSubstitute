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
        /// <summary>
        /// All domain objects need an identity field
        /// </summary>
        Int64 Id { get; set; }
        
        /// <summary>
        /// How the objects are grouped
        /// </summary>
        Int64 GroupingKey { get; set; }
    }

    /// <summary>
    /// A concrete implementation of the domain object interface
    /// </summary>
    public class DomainObject : IDomainObject
    {
        /// <summary>
        /// All domain objects need an identity field
        /// </summary>
        public Int64 Id { get; set; }

        /// <summary>
        /// How the objects are grouped
        /// </summary>
        public Int64 GroupingKey { get; set; }
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
    /// Business logic functionality that is fed by the service bus handler to 
    /// get the information
    /// </summary>
    public class BusinessLogic<T> where T : IDomainObject
    {
        /// <summary>
        /// Local service bus handler that feeds the business logic
        /// </summary>
        private IServiceBusHandler<T> serviceBusHandler;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="serviceBusHandler">The service bus that feeds the logic</param>
        public BusinessLogic(IServiceBusHandler<T> serviceBusHandler)
        {
            this.serviceBusHandler = serviceBusHandler; // Assign the service bus
        }
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
            IServiceBusHandler<DomainObject> mockedServiceBusHandler =
                Substitute.For<IServiceBusHandler<DomainObject>>();

            mockedServiceBusHandler
                .Get()
                .Returns(
                    new List<KeyValuePair<Int64, DomainObject>>()
                    {
                        new KeyValuePair<Int64, DomainObject>(1, new DomainObject(){ Id = 1, GroupingKey = 1 }),
                        new KeyValuePair<Int64, DomainObject>(2, new DomainObject(){ Id = 2, GroupingKey = 1 }),
                        new KeyValuePair<Int64, DomainObject>(2, new DomainObject(){ Id = 3, GroupingKey = 2 })
                    }); // Mock up the get end point for the service bus handler

            List<KeyValuePair<Int64, DomainObject>> result = null; // Invalid results set to start

            BusinessLogic<DomainObject> businessLogic = 
                new BusinessLogic<DomainObject>(mockedServiceBusHandler); // Create the business logic fed from the service bus handler

            // Act
            result = mockedServiceBusHandler.Get(); // Execute the get to get the mocked up service bus items

            // Assert
            Assert.NotNull(result); // The result set was populated at least
            Assert.Equal(2, result.Count); // Correct amount of rows?         
        }
    }
}
