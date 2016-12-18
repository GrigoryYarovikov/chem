using Xunit;

namespace Chem.Tests
{
    public class SubstancesServiceTests
    {
        // CreateSubstance tests
        [Fact()]
        public void CreateSubstancesServiceTest()
        {
            Assert.NotNull(new SubstancesService());
            //Assert.True(new SubstancesService(), "cannot create service");
        }

        // GetById tests
        [Fact()]
        public void GetByIdTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        // GetByQuery tests
        [Fact()]
        public void GetByEmptyModelTest()
        {
            var service = new SubstancesService();
            var model = new Models.Search.QueryModel();
            var result = service.GetByQuery(model);

            Assert.True(result.Count == 100, "return first 100 elements from db if model is empty");
        }

        [Fact()]
        public void GetByBoilingPointGreatThanTest()
        {
            const int boilingPoint = 10;
            var service = new SubstancesService();
            var model = new Models.Search.QueryModel();
            model.bp1 = boilingPoint;

            var result = service.GetByQuery(model);

            Assert.True(result.Count > 0);
            foreach(var previewElem in result)
            {
                var elem = service.GetById(previewElem.Id);
                Assert.True(elem.BoilingPoint >= boilingPoint);
            }
        }

        [Fact()]
        public void GetByBoilingPointLessThanTest()
        {
            const int boilingPoint = 100;
            var service = new SubstancesService();
            var model = new Models.Search.QueryModel();
            model.bp2 = boilingPoint;

            var result = service.GetByQuery(model);

            Assert.True(result.Count > 0);
            foreach (var previewElem in result)
            {
                var elem = service.GetById(previewElem.Id);
                Assert.True(elem.BoilingPoint <= boilingPoint);
            }
        }

        [Fact()]
        public void GetByEqualBoilingPointTest()
        {
            const int boilingPoint = 100;
            var service = new SubstancesService();
            var model = new Models.Search.QueryModel();
            model.bp1 = boilingPoint;
            model.bp2 = boilingPoint;

            var result = service.GetByQuery(model);

            Assert.True(result.Count > 0);
            foreach (var previewElem in result)
            {
                var elem = service.GetById(previewElem.Id);
                Assert.True(elem.BoilingPoint == boilingPoint);
            }
        }

        [Fact()]
        public void GetByUnreachableBoilingPointTest()
        {
            const int boilingPoint = 100;
            var service = new SubstancesService();
            var model = new Models.Search.QueryModel();
            model.bp1 = boilingPoint;
            model.bp2 = 0;

            var result = service.GetByQuery(model);

            Assert.True(result.Count == 0);
        }

        // GetReactionList tests
        [Fact()]
        public void GetReactionListTest()
        {
            Assert.True(false, "This test needs an implementation");
        }
    }
}