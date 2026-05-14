using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MG_Project.Test
{
    public class AddressServiceTests : IClassFixture<InMemoryServicesFixture>
    {
        private readonly InMemoryServicesFixture _fixture;

        public AddressServiceTests(InMemoryServicesFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void AddAddress_ShouldCreateAddress()
        {
            var addr = _fixture.AddressService.AddAddress(
                "Nowa", "Warszawa", "Polska", "00-001", 10, 5);

            var fetched = _fixture.AddressService.Get(addr.AddressId);

            Assert.NotNull(fetched);
            Assert.Equal("Nowa", fetched.Street);
        }

        [Fact]
        public void FullAddr_ShouldReturnAllAddresses()
        {
            var all = _fixture.AddressService.FullAddr();
            Assert.NotEmpty(all);
        }
    }
}
