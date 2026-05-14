using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MG_Project.Test
{
    public class PersonServiceTests : IClassFixture<InMemoryServicesFixture>
    {
        private readonly InMemoryServicesFixture _fixture;

        public PersonServiceTests(InMemoryServicesFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void AddPerson_ShouldCreatePerson()
        {
            var addr = _fixture.AddressService.FullAddr()[0];

            var personId = _fixture.PersonService.AddPerson(
                "Jan", "Kowalski", "12345678901", DateTime.Now.AddYears(-30), addr);

            var person = _fixture.PersonService.Get(personId);

            Assert.NotNull(person);
            Assert.Equal("Jan", person.FirstName);
        }

        [Fact]
        public void FullAddr_ShouldReturnAllPersons()
        {
            var all = _fixture.PersonService.FullAddr();
            Assert.NotEmpty(all);
        }
    }
}
