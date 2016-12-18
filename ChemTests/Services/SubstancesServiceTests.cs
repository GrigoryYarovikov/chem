using Xunit;
using Chem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chem.Tests
{
    public class SubstancesServiceTests
    {
        [Fact()]
        public void SubstancesServiceTest()
        {
            Assert.NotNull(new SubstancesService());
            //Assert.True(new SubstancesService(), "cannot create service");
        }

        [Fact()]
        public void GetByIdTest()
        {
            var service = new SubstancesService();
            var element = service.GetById(4);

            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void GetByQueryTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void GetReactionListTest()
        {
            Assert.True(false, "This test needs an implementation");
        }
    }
}