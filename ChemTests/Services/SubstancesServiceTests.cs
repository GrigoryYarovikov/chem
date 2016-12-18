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
        }
#endregion // CreateSubstanceTests

        #region GetByIdTest
        [Fact()]
        public void GetByInvalidIdTest()
        {
            var service = new SubstancesService();
            Assert.Throws<System.NullReferenceException>(() => service.GetById(-1));
        }
        [Fact()]
        public void GetByValidIdTest()
        {
            const string Formula = "C2H6O";
            var service = new SubstancesService();
            var model = new Models.Search.QueryModel();
            model.q = Formula;

            var result = service.GetByQuery(model);

            Assert.True(result.Count > 0);
            foreach (var previewElem in result)
            {
                var elem = service.GetById(previewElem.Id);
                Assert.True(elem.Formula == "C2H6O");
            }
        }
        #endregion // GetByIdTest

        #region GetByQueryTests

        #region GetByEmptyModelTests
        [Fact()]
            public void GetByEmptyModelTest()
            {
                var service = new SubstancesService();
                var model = new Models.Search.QueryModel();
                var result = service.GetByQuery(model);

                Assert.True(result.Count == 100, "return first 100 elements from db if model is empty");
            }
            #endregion // GetByEmptyModelTests

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

            #region GetByDensityTests
            [Fact()]
            public void GetByDensityGreatThanTest()
            {
                const int density = 1;
                var service = new SubstancesService();
                var model = new Models.Search.QueryModel();
                model.d1 = density;

                var result = service.GetByQuery(model);

                Assert.True(result.Count > 0);
                foreach (var previewElem in result)
                {
                    var elem = service.GetById(previewElem.Id);
                    Assert.True(elem.Density >= density);
                }
            }
            [Fact()]
            public void GetByDensityLessThanTest()
            {
                const int density = 1;
                var service = new SubstancesService();
                var model = new Models.Search.QueryModel();
                model.d2 = density;

                var result = service.GetByQuery(model);

                Assert.True(result.Count > 0);
                foreach (var previewElem in result)
                {
                    var elem = service.GetById(previewElem.Id);
                    Assert.True(elem.Density <= density);
                }
            }

            [Fact()]
            public void GetByEqualDensityTest()
            {
                const int density = 1;
                var service = new SubstancesService();
                var model = new Models.Search.QueryModel();
                model.d1 = density;
                model.d2 = density;

                var result = service.GetByQuery(model);

                Assert.True(result.Count > 0);
                foreach (var previewElem in result)
                {
                    var elem = service.GetById(previewElem.Id);
                    Assert.True(elem.Density == density);
                }
            }

            [Fact()]
            public void GetByUnreachableDensityTest()
            {
                const int density = 1;
                var service = new SubstancesService();
                var model = new Models.Search.QueryModel();
                model.d1 = density;
                model.d2 = 0;

                var result = service.GetByQuery(model);

                Assert.True(result.Count == 0);
            }
            #endregion // GetByDensityTests

            #region GetByFlashPointTests
            [Fact()]
            public void GetByFlashPointThanTest()
            {
                const int flashpoint = 100;
                var service = new SubstancesService();
                var model = new Models.Search.QueryModel();
                model.fp1 = flashpoint;

                var result = service.GetByQuery(model);

                Assert.True(result.Count > 0);
                foreach (var previewElem in result)
                {
                    var elem = service.GetById(previewElem.Id);
                    Assert.True(elem.FlashPoint >= flashpoint);
                }
            }
            [Fact()]
            public void GetByFlashPointLessThanTest()
            {
                const int flashpoint = 100;
                var service = new SubstancesService();
                var model = new Models.Search.QueryModel();
                model.fp2 = flashpoint;

                var result = service.GetByQuery(model);

                Assert.True(result.Count > 0);
                foreach (var previewElem in result)
                {
                    var elem = service.GetById(previewElem.Id);
                    Assert.True(elem.FlashPoint <= flashpoint);
                }
            }

            [Fact()]
            public void GetByEqualFlashPointTest()
            {
                const int flashpoint = 100;
                var service = new SubstancesService();
                var model = new Models.Search.QueryModel();
                model.fp1 = flashpoint;
                model.fp2 = flashpoint;

                var result = service.GetByQuery(model);

                Assert.True(result.Count > 0);
                foreach (var previewElem in result)
                {
                    var elem = service.GetById(previewElem.Id);
                    Assert.True(elem.FlashPoint == flashpoint);
                }
            }

            [Fact()]
            public void GetByUnreachableFlashPointTest()
            {
                const int flashpoint = 100;
                var service = new SubstancesService();
                var model = new Models.Search.QueryModel();
                model.fp1 = flashpoint;
                model.fp2 = 0;

                var result = service.GetByQuery(model);

                Assert.True(result.Count == 0);
            }
            #endregion // GetByFlashPointTests

            #region GetByMeltingPointTests
            [Fact()]
            public void GetByMeltingPointThanTest()
            {
                const int MeltingPoint = 0;
                var service = new SubstancesService();
                var model = new Models.Search.QueryModel();
                model.mp1 = MeltingPoint;

                var result = service.GetByQuery(model);

                Assert.True(result.Count > 0);
                foreach (var previewElem in result)
                {
                    var elem = service.GetById(previewElem.Id);
                    Assert.True(elem.MeltingPoint >= MeltingPoint);
                }
            }
            [Fact()]
            public void GetByMeltingPointLessThanTest()
            {
                const int MeltingPoint = 0;
                var service = new SubstancesService();
                var model = new Models.Search.QueryModel();
                model.mp2 = MeltingPoint;

                var result = service.GetByQuery(model);

                Assert.True(result.Count > 0);
                foreach (var previewElem in result)
                {
                    var elem = service.GetById(previewElem.Id);
                    Assert.True(elem.MeltingPoint <= MeltingPoint);
                }
            }

            [Fact()]
            public void GetByEqualMeltingPointTest()
            {
                const int MeltingPoint = 0;
                var service = new SubstancesService();
                var model = new Models.Search.QueryModel();
                model.mp1 = MeltingPoint;
                model.mp2 = MeltingPoint;

                var result = service.GetByQuery(model);

                Assert.True(result.Count > 0);
                foreach (var previewElem in result)
                {
                    var elem = service.GetById(previewElem.Id);
                    Assert.True(elem.MeltingPoint == MeltingPoint);
                }
            }

            [Fact()]
            public void GetByUnreachableMeltingPointTest()
            {
                const int MeltingPoint = 0;
                var service = new SubstancesService();
                var model = new Models.Search.QueryModel();
                model.mp1 = MeltingPoint;
                model.mp2 = -10;

                var result = service.GetByQuery(model);

                Assert.True(result.Count == 0);
            }
            #endregion // GetByMeltingPointTests
        
            #region GetByRefractiveIndexTests
            [Fact()]
            public void GetByRefractiveIndexThanTest()
            {
                const double RefractiveIndex = 1.329;
                var service = new SubstancesService();
                var model = new Models.Search.QueryModel();
                model.ri1 = RefractiveIndex;

                var result = service.GetByQuery(model);

                Assert.True(result.Count > 0);
                foreach (var previewElem in result)
                {
                    var elem = service.GetById(previewElem.Id);
                    Assert.True(elem.RefractiveIndex >= RefractiveIndex);
                }
            }
            [Fact()]
            public void GetByRefractiveIndexLessThanTest()
            {
                const double RefractiveIndex = 1.329;
                var service = new SubstancesService();
                var model = new Models.Search.QueryModel();
                model.ri2 = RefractiveIndex;

                var result = service.GetByQuery(model);

                Assert.True(result.Count > 0);
                foreach (var previewElem in result)
                {
                    var elem = service.GetById(previewElem.Id);
                    Assert.True(elem.RefractiveIndex <= RefractiveIndex);
                }
            }

            [Fact()]
            public void GetByEqualRefractiveIndexTest()
            {
                const double RefractiveIndex = 1.329;
                var service = new SubstancesService();
                var model = new Models.Search.QueryModel();
                model.ri1 = RefractiveIndex;
                model.ri2 = RefractiveIndex;

                var result = service.GetByQuery(model);

                Assert.True(result.Count > 0);
                foreach (var previewElem in result)
                {
                    var elem = service.GetById(previewElem.Id);
                    Assert.True(elem.RefractiveIndex == RefractiveIndex);
                }
            }

            [Fact()]
            public void GetByUnreachableRefractiveIndexTest()
            {
                const double RefractiveIndex = 1.329;
                var service = new SubstancesService();
                var model = new Models.Search.QueryModel();
                model.ri1 = RefractiveIndex;
                model.ri2 = 0;

                var result = service.GetByQuery(model);

                Assert.True(result.Count == 0);
            }
            #endregion // GetByRefractiveIndexTests

            #region GetByVapourPressurTests
            [Fact()]
            public void GetByVapourPressurThanTest()
            {
                const double VapourPressur = 0.158;
                var service = new SubstancesService();
                var model = new Models.Search.QueryModel();
                model.vp1 = VapourPressur;

                var result = service.GetByQuery(model);

                Assert.True(result.Count > 0);
                foreach (var previewElem in result)
                {
                    var elem = service.GetById(previewElem.Id);
                    Assert.True(elem.VapourPressur >= VapourPressur);
                }
            }
            [Fact()]
            public void GetByVapourPressurLessThanTest()
            {
                const double VapourPressur = 0.158;
                var service = new SubstancesService();
                var model = new Models.Search.QueryModel();
                model.vp2 = VapourPressur;

                var result = service.GetByQuery(model);

                Assert.True(result.Count > 0);
                foreach (var previewElem in result)
                {
                    var elem = service.GetById(previewElem.Id);
                    Assert.True(elem.VapourPressur <= VapourPressur);
                }
            }

            [Fact()]
            public void GetByEqualVapourPressurTest()
            {
                const double VapourPressur = 0.158;
                var service = new SubstancesService();
                var model = new Models.Search.QueryModel();
                model.vp1 = VapourPressur;
                model.vp2 = VapourPressur;

                var result = service.GetByQuery(model);

                Assert.True(result.Count > 0);
                foreach (var previewElem in result)
                {
                    var elem = service.GetById(previewElem.Id);
                    Assert.True(elem.VapourPressur == VapourPressur);
                }
            }

            [Fact()]
            public void GetByUnreachableVapourPressurTest()
            {
                const double VapourPressur = 0.158;
                var service = new SubstancesService();
                var model = new Models.Search.QueryModel();
                model.vp1 = VapourPressur;
                model.vp2 = 0;

                var result = service.GetByQuery(model);

                Assert.True(result.Count == 0);
            }
            #endregion // GetByVapourPressurTests

            #region GetByWaterSolubilityTests
            [Fact()]
            public void GetByWaterSolubilityThanTest()
            {
                const bool WaterSolubility = true;
                var service = new SubstancesService();
                var model = new Models.Search.QueryModel();
                model.ws = WaterSolubility;

                var result = service.GetByQuery(model);

                Assert.True(result.Count > 0);
                foreach (var previewElem in result)
                {
                    var elem = service.GetById(previewElem.Id);
                    Assert.True(elem.WaterSolubility == "да");
                }
            }
            [Fact()]
            public void GetByWaterUnSolubilityTest()
            {
                const bool WaterSolubility = false;
                var service = new SubstancesService();
                var model = new Models.Search.QueryModel();
                model.ws = WaterSolubility;

                var result = service.GetByQuery(model);

                Assert.True(result.Count > 0);
                foreach (var previewElem in result)
                {
                    var elem = service.GetById(previewElem.Id);
                    Assert.True(elem.WaterSolubility == "нет");
                }
            }
            #endregion // GetByFormulaTests

            #region GetByFormulaTests
            [Fact()]
            public void GetByEqualFormulaTest()
            {
                const string Formula = "C2H6O";
                var service = new SubstancesService();
                var model = new Models.Search.QueryModel();
                model.q = Formula;

                var result = service.GetByQuery(model);

                Assert.True(result.Count > 0);
                foreach (var previewElem in result)
                {
                    var elem = service.GetById(previewElem.Id);
                    Assert.True(elem.Formula == "C2H6O");
                }
            }
            [Fact()]
            public void GetByEqualFormulasTest()
            {
                const string Formula = "C2H5OH";
                const string extendedFormula = "C4H10O2H2";
                const string foldedFormula = "C2H6O";
                var service = new SubstancesService();
                var model = new Models.Search.QueryModel();
                model.q = Formula;

                var resultByFormula = service.GetByQuery(model);

                model.q = extendedFormula;
                var resultByExtendedFormula = service.GetByQuery(model);

                model.q = foldedFormula;
                var resultByFoldedFormula = service.GetByQuery(model);

                Assert.True(resultByFormula.Count > 0);
                Assert.True(resultByFormula.Equals(resultByExtendedFormula));
                Assert.True(resultByFormula.Equals(resultByFoldedFormula));
            }
            #endregion // GetByFormulaTests

        #endregion // GetByQueryTests

        #region GetReactionListTests
        [Fact()]
        public void GetReactionListTest()
        {
            // Nothing that can be tested
            //Assert.True(false, "This test needs an implementation");
        }
#endregion // GetReactionListTests
    }
}