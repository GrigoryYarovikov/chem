using Xunit;

namespace Chem.Tests
{
    public class SubstancesServiceTests
    {
 #region CreateSubstanceTests
        [Fact()]
        public void CreateSubstancesServiceTest()
        {
            Assert.NotNull(new SubstancesService());
            //Assert.True(new SubstancesService(), "cannot create service");
        }
#endregion // CreateSubstanceTests

 #region GetByIdTest
        [Fact()]
        public void GetByIdTest()
        {
            Assert.True(false, "This test needs an implementation");
        }
#endregion // GetByIdTest

#region GetByQueryTests
        [Fact()]
        public void GetByEmptyModelTest()
        {
            var service = new SubstancesService();
            var model = new Models.Search.QueryModel();
            var result = service.GetByQuery(model);

            Assert.True(result.Count == 100, "return first 100 elements from db if model is empty");
        }

#region BoilingPointTests
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
#endregion // BoilingPointTests
#endregion // GetByQueryTests

#region GetReactionListTests
        [Fact()]
        public void GetReactionListTest()
        {
            Assert.True(false, "This test needs an implementation");
        }
#endregion // GetReactionListTests
    }
}