using System.Collections.Generic;
using System.Linq;
using Chem.Managers.Fakes;
using Chem.Models;
using Chem.Services.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chem.Tests.Services
{
    [TestClass]
    public class SubstancesServiceTest
    {
        private SubstancesService _substancesService;
        private StubISubstanceManager _stubISubstanceManager;
        private StubIElementManager _stubIElementManager;
        private List<Substance> _substances;
        private List<Element> _elements;

        [TestInitialize]
        public void SubstancesService_TestInit()
        {
            _stubISubstanceManager = new StubISubstanceManager()
            {
                GetAll = () => _substances.AsQueryable(),
                GetByIdInt32 = (int id) => _substances.FirstOrDefault( item => item.Id == id)
            };

            _stubIElementManager = new StubIElementManager()
            {
                GetAll = () => _elements.AsQueryable()
            };

            _substancesService = new SubstancesService(_stubISubstanceManager, _stubIElementManager);
        }

        [TestMethod]
        public void SubstancesService_CreateNew_Success()
        {
            _substances = new List<Substance>() {};
            Assert.IsNotNull(_substancesService);
        }

        [TestMethod]
        public void SubstancesService_GetById_CorrectId_Success()
        {
            _substances = new List<Substance>()
            {
                new Substance()
                {
                     Id = 1,
                     Formula = "H2O",

                },
                new Substance()
                {
                     Id = 2,
                     Formula = "O2"
                }
            };

            _elements = new List<Element>();

            var result = _substancesService.GetById(1);
            Assert.AreEqual(result.Formula, "H2O");
        }

        [TestMethod]
        [ExpectedException(typeof(System.SystemException), "Not existing Id!")]
        public void SubstancesService_GetById_IncorrectId_Null()
        {
            _substances = new List<Substance>()
            {
                new Substance()
                {
                    Id = 1,
                    Formula = "H2O"
                },
                new Substance()
                {
                    Id = 2,
                    Formula = "O2"
                }
            };

            _elements = new List<Element>();
            _substancesService.GetById(0);
        }

        [TestMethod]
        public void SubstancesService_GetByQuery_FormulaExsitingElement_Success()
        {
            _substances = new List<Substance>()
            {
                new Substance()
                {
                    Id = 1,
                    Formula = "H2O"
                },
                new Substance()
                {
                    Id = 2,
                    Formula = "H2O"
                },
                new Substance()
                {
                    Id = 3,
                    Formula = "O2"
                }
            };

            _elements = new List<Element>()
            {
                new Element()
                {
                    Id = 1,
                    Sign = "H"
                },
                new Element()
                {
                    Id = 2,
                    Sign = "O"
                }
            };

            const string formula = "H2O";
            var model = new Models.Search.QueryModel {q = formula};
            var result = _substancesService.GetByQuery(model);

            Assert.IsTrue(result.Count == 2);
            Assert.IsTrue(result.TrueForAll(item => item.Formula == formula));
        }


//        [TestMethod]
//        public void SubstancesService_GetByQuery_FormulasExcitingElements_Sucess()
//        {
//            _substances = new List<Substance>()
//            {
//                new Substance()
//                {
//                    Id = 1,
//                    Formula = "O2H3"
//                },
//                new Substance()
//                {
//                    Id = 2,
//                    Formula = "H2O"
//                },
//                new Substance()
//                {
//                    Id = 3,
//                    Formula = "H3OH"
//                }
//            };
//
//            _elements = new List<Element>()
//            {
//                new Element()
//                {
//                    Id = 1,
//                    Sign = "H"
//                },
//                new Element()
//                {
//                    Id = 2,
//                    Sign = "O"
//                },
//                new Element()
//                {
//                    Id = 2,
//                    Sign = "C"
//                }
//            };
//
//            const string baseFormula = "H2O";
//            const string extendedFormula = "H3OH";
//            const string foldedFormula = "O2H3";
//
//            var model = new Models.Search.QueryModel();
//
//            model.q = baseFormula;
//            var resultByBaseFormula = _substancesService.GetByQuery(model);
//
//            model.q = extendedFormula;
//            var resultByExtendedFormula = _substancesService.GetByQuery(model);
//
//            model.q = foldedFormula;
//            var resultByFoldedFormula = _substancesService.GetByQuery(model);
//        }

        [TestMethod]
        public void SubstancesService_GetByQuery_FormulaNotExsitingElement_EmptyList()
        {
            _substances = new List<Substance>()
            {
                new Substance()
                {
                    Id = 1,
                    Formula = "H"
                },
                new Substance()
                {
                    Id = 2,
                    Formula = "O2"
                }
            };

            _elements = new List<Element>()
            {
                new Element()
                {
                    Id = 1,
                    Sign = "H"
                },
                new Element()
                {
                    Id = 2,
                    Sign = "O"
                }
            };

            const string formula = "H20";
            var model = new Models.Search.QueryModel {q = formula};
            var result = _substancesService.GetByQuery(model);

            Assert.IsTrue(result.Count == 0);
        }

        [TestMethod]
        public void SubstancesService_GetByQuery_EmptyModel_Success()
        {
            _substances = new List<Substance>()
            {
                new Substance()
                {
                    Id = 1,
                    Formula = "H2O"
                },
                new Substance()
                {
                    Id = 2,
                    Formula = "O2"
                },
                new Substance()
                {
                    Id = 3,
                    Formula = "H2"
                }
            };

            _elements = new List<Element>()
            {
                new Element()
                {
                    Id = 1,
                    Sign = "H"
                },
                new Element()
                {
                    Id = 2,
                    Sign = "O"
                }
            };

            var model = new Models.Search.QueryModel {};
            var result = _substancesService.GetByQuery(model);

            Assert.IsTrue(result.Count == 3);
        }

        [TestMethod]
        public void SubstancesService_GetByQuery_BoilingPointMoreOrEqualTo_Success()
        {
            _substances = new List<Substance>()
            {
                new Substance()
                {
                    Id = 1,
                    BoilingPoint = 0
                },
                new Substance()
                {
                    Id = 2,
                    BoilingPoint = 5
                },
                new Substance()
                {
                    Id = 3,
                    BoilingPoint = 10
                }
            };

            _elements = new List<Element>();

            const double boilingPointLowerBound = 5;
            var model = new Models.Search.QueryModel {bp1 = boilingPointLowerBound};
            var result = _substancesService.GetByQuery(model);

            Assert.IsTrue(result.Count == 2);
            Assert.IsTrue(
                result.TrueForAll(
                    item => _substances.First(x => x.Id.Equals(item.Id)).BoilingPoint >= boilingPointLowerBound));
        }

        [TestMethod]
        public void SubstancesService_GetByQuery_BoilingPointLessOrEqualTo_Success()
        {
            _substances = new List<Substance>()
            {
                new Substance()
                {
                    Id = 1,
                    BoilingPoint = 0
                },
                new Substance()
                {
                    Id = 2,
                    BoilingPoint = 5
                },
                new Substance()
                {
                    Id = 3,
                    BoilingPoint = 10
                }
            };

            _elements = new List<Element>();

            const double boilingPointUpperBound = 5;
            var model = new Models.Search.QueryModel {bp2 = boilingPointUpperBound};
            var result = _substancesService.GetByQuery(model);

            Assert.IsTrue(result.Count == 2);
            Assert.IsTrue(
                result.TrueForAll(
                    item => _substances.First(x => x.Id.Equals(item.Id)).BoilingPoint <= boilingPointUpperBound));
        }

        [TestMethod]
        public void SubstancesService_GetByQuery_BoilingPointMoreAndLessThan_Success()
        {
            _substances = new List<Substance>()
            {
                new Substance()
                {
                    Id = 1,
                    BoilingPoint = 0
                },
                new Substance()
                {
                    Id = 2,
                    BoilingPoint = 5
                },
                new Substance()
                {
                    Id = 3,
                    BoilingPoint = 10
                }
            };

            _elements = new List<Element>();

            const double boilingPointLowerBound = 5;
            const double boilingPointUpperBound = 10;
            var model = new Models.Search.QueryModel
            {
                bp1 = boilingPointLowerBound,
                bp2 = boilingPointUpperBound
            };
            var result = _substancesService.GetByQuery(model);

            Assert.IsTrue(result.Count == 2);
            Assert.IsTrue(
                result.TrueForAll(
                    item => _substances.First(x => x.Id.Equals(item.Id)).BoilingPoint <= boilingPointUpperBound &&
                    _substances.First(x => x.Id.Equals(item.Id)).BoilingPoint >= boilingPointLowerBound));
        }

        [TestMethod]
        public void SubstancesService_GetByQuery_BoilingPointEqualTo_Success()
        {
            _substances = new List<Substance>()
            {
                new Substance()
                {
                    Id = 1,
                    BoilingPoint = 0
                },
                new Substance()
                {
                    Id = 2,
                    BoilingPoint = 5
                },
                new Substance()
                {
                    Id = 3,
                    BoilingPoint = 5
                },
                new Substance()
                {
                    Id = 4,
                    BoilingPoint = 10
                }
            };

            _elements = new List<Element>();

            const double boilingPointLowerBound = 5;
            const double boilingPointUpperBound = 5;
            var model = new Models.Search.QueryModel
            {
                bp1 = boilingPointLowerBound,
                bp2 = boilingPointUpperBound
            };
            var result = _substancesService.GetByQuery(model);

            Assert.IsTrue(result.Count == 2);
            Assert.IsTrue(
                result.TrueForAll(
                    item => Equals(_substances.First(x => x.Id.Equals(item.Id)).BoilingPoint, boilingPointUpperBound)));
        }

        [TestMethod]
        public void SubstancesService_GetByQuery_BoilerPointWrongBounds_EmptyList()
        {
            _substances = new List<Substance>()
            {
                new Substance()
                {
                    Id = 1,
                    BoilingPoint = 0
                },
                new Substance()
                {
                    Id = 2,
                    BoilingPoint = 5
                }
            };

            _elements = new List<Element>();

            const double boilingPointLowerBound = 10;
            const double boilingPointUpperBound = 5;
            var model = new Models.Search.QueryModel
            {
                bp1 = boilingPointLowerBound,
                bp2 = boilingPointUpperBound
            };
            var result = _substancesService.GetByQuery(model);

            Assert.IsTrue(result.Count == 0);
        }

        [TestMethod]
        public void SubstancesService_GetByQuery_FlashPointMoreOrEqualTo_Success()
        {
            _substances = new List<Substance>()
            {
                new Substance()
                {
                    Id = 1,
                    FlashPoint = 0
                },
                new Substance()
                {
                    Id = 2,
                    FlashPoint = 5
                },
                new Substance()
                {
                    Id = 3,
                    FlashPoint = 10
                }
            };

            _elements = new List<Element>();

            const double flashPointLowerBound = 5;
            var model = new Models.Search.QueryModel { fp1 = flashPointLowerBound };
            var result = _substancesService.GetByQuery(model);

            Assert.IsTrue(result.Count == 2);
            Assert.IsTrue(
                result.TrueForAll(
                    item => _substances.First(x => x.Id.Equals(item.Id)).FlashPoint >= flashPointLowerBound));
        }

        [TestMethod]
        public void SubstancesService_GetByQuery_FlashPointLessOrEqualTo_Success()
        {
            _substances = new List<Substance>()
            {
                new Substance()
                {
                    Id = 1,
                    FlashPoint = 0
                },
                new Substance()
                {
                    Id = 2,
                    FlashPoint = 5
                },
                new Substance()
                {
                    Id = 3,
                    FlashPoint = 10
                }
            };

            _elements = new List<Element>();

            const double flashPointUpperBound = 5;
            var model = new Models.Search.QueryModel { fp2 = flashPointUpperBound };
            var result = _substancesService.GetByQuery(model);

            Assert.IsTrue(result.Count == 2);
            Assert.IsTrue(
                result.TrueForAll(
                    item => _substances.First(x => x.Id.Equals(item.Id)).FlashPoint <= flashPointUpperBound));
        }

        [TestMethod]
        public void SubstancesService_GetByQuery_FlashPointMoreAndLessThan_Success()
        {
            _substances = new List<Substance>()
            {
                new Substance()
                {
                    Id = 1,
                    FlashPoint = 0
                },
                new Substance()
                {
                    Id = 2,
                    FlashPoint = 5
                },
                new Substance()
                {
                    Id = 3,
                    FlashPoint = 10
                }
            };

            _elements = new List<Element>();

            const double flashPointLowerBound = 5;
            const double flashPointUpperBound = 10;
            var model = new Models.Search.QueryModel
            {
                fp1 = flashPointLowerBound,
                fp2 = flashPointUpperBound
            };
            var result = _substancesService.GetByQuery(model);

            Assert.IsTrue(result.Count == 2);
            Assert.IsTrue(
                result.TrueForAll(
                    item => _substances.First(x => x.Id.Equals(item.Id)).FlashPoint <= flashPointUpperBound &&
                    _substances.First(x => x.Id.Equals(item.Id)).FlashPoint >= flashPointLowerBound));
        }

        [TestMethod]
        public void SubstancesService_GetByQuery_FlashPointEqualTo_Success()
        {
            _substances = new List<Substance>()
            {
                new Substance()
                {
                    Id = 1,
                    FlashPoint = 0
                },
                new Substance()
                {
                    Id = 2,
                    FlashPoint = 5
                },
                new Substance()
                {
                    Id = 3,
                    FlashPoint = 5
                },
                new Substance()
                {
                    Id = 4,
                    FlashPoint = 10
                }
            };

            _elements = new List<Element>();

            const double flashPointLowerBound = 5;
            const double flashPointUpperBound = 5;
            var model = new Models.Search.QueryModel
            {
                fp1 = flashPointLowerBound,
                fp2 = flashPointUpperBound
            };
            var result = _substancesService.GetByQuery(model);

            Assert.IsTrue(result.Count == 2);
            Assert.IsTrue(
                result.TrueForAll(
                    item => Equals(_substances.First(x => x.Id.Equals(item.Id)).FlashPoint, flashPointUpperBound)));
        }

        [TestMethod]
        public void SubstancesService_GetByQuery_FlashPointWrongBounds_EmptyList()
        {
            _substances = new List<Substance>()
            {
                new Substance()
                {
                    Id = 1,
                    FlashPoint = 0
                },
                new Substance()
                {
                    Id = 2,
                    FlashPoint = 5
                }
            };

            _elements = new List<Element>();

            const double flashPointLowerBound = 10;
            const double flashPointUpperBound = 5;
            var model = new Models.Search.QueryModel
            {
                bp1 = flashPointLowerBound,
                bp2 = flashPointUpperBound
            };
            var result = _substancesService.GetByQuery(model);

            Assert.IsTrue(result.Count == 0);
        }

        [TestMethod]
        public void SubstancesService_GetByQuery_DensityMoreOrEqualTo_Success()
        {
            _substances = new List<Substance>()
            {
                new Substance()
                {
                    Id = 1,
                    Density = 0
                },
                new Substance()
                {
                    Id = 2,
                    Density = 5
                },
                new Substance()
                {
                    Id = 3,
                    Density = 10
                }
            };

            _elements = new List<Element>();

            const double densityLowerBound = 5;
            var model = new Models.Search.QueryModel { d1 = densityLowerBound };
            var result = _substancesService.GetByQuery(model);

            Assert.IsTrue(result.Count == 2);
            Assert.IsTrue(
                result.TrueForAll(
                    item => _substances.First(x => x.Id.Equals(item.Id)).Density >= densityLowerBound));
        }

        [TestMethod]
        public void SubstancesService_GetByQuery_DensityLessOrEqualTo_Success()
        {
            _substances = new List<Substance>()
            {
                new Substance()
                {
                    Id = 1,
                    Density = 0
                },
                new Substance()
                {
                    Id = 2,
                    Density = 5
                },
                new Substance()
                {
                    Id = 3,
                    Density = 10
                }
            };

            _elements = new List<Element>();

            const double densityUpperBound = 5;
            var model = new Models.Search.QueryModel { d2 = densityUpperBound };
            var result = _substancesService.GetByQuery(model);

            Assert.IsTrue(result.Count == 2);
            Assert.IsTrue(
                result.TrueForAll(
                    item => _substances.First(x => x.Id.Equals(item.Id)).Density <= densityUpperBound));
        }

        [TestMethod]
        public void SubstancesService_GetByQuery_DensityMoreAndLessThan_Success()
        {
            _substances = new List<Substance>()
            {
                new Substance()
                {
                    Id = 1,
                    Density = 0
                },
                new Substance()
                {
                    Id = 2,
                    Density = 5
                },
                new Substance()
                {
                    Id = 3,
                    Density = 10
                }
            };

            _elements = new List<Element>();

            const double densityLowerBound = 5;
            const double densityUpperBound = 10;
            var model = new Models.Search.QueryModel
            {
                d1 = densityLowerBound,
                d2 = densityUpperBound
            };
            var result = _substancesService.GetByQuery(model);

            Assert.IsTrue(result.Count == 2);
            Assert.IsTrue(
                result.TrueForAll(
                    item => _substances.First(x => x.Id.Equals(item.Id)).Density <= densityUpperBound &&
                    _substances.First(x => x.Id.Equals(item.Id)).Density >= densityLowerBound));
        }

        [TestMethod]
        public void SubstancesService_GetByQuery_DensityEqualTo_Success()
        {
            _substances = new List<Substance>()
            {
                new Substance()
                {
                    Id = 1,
                    Density = 0
                },
                new Substance()
                {
                    Id = 2,
                    Density = 5
                },
                new Substance()
                {
                    Id = 3,
                    Density = 5
                },
                new Substance()
                {
                    Id = 4,
                    Density = 10
                }
            };

            _elements = new List<Element>();

            const double densityLowerBound = 5;
            const double densityUpperBound = 5;
            var model = new Models.Search.QueryModel
            {
                d1 = densityLowerBound,
                d2 = densityUpperBound
            };
            var result = _substancesService.GetByQuery(model);

            Assert.IsTrue(result.Count == 2);
            Assert.IsTrue(
                result.TrueForAll(
                    item => Equals(_substances.First(x => x.Id.Equals(item.Id)).Density, densityUpperBound)));
        }

        [TestMethod]
        public void SubstancesService_GetByQuery_DensityWrongBounds_EmptyList()
        {
            _substances = new List<Substance>()
            {
                new Substance()
                {
                    Id = 1,
                    Density = 0
                },
                new Substance()
                {
                    Id = 2,
                    Density = 5
                }
            };

            _elements = new List<Element>();

            const double densityLowerBound = 10;
            const double densityUpperBound = 5;
            var model = new Models.Search.QueryModel
            {
                bp1 = densityLowerBound,
                bp2 = densityUpperBound
            };
            var result = _substancesService.GetByQuery(model);

            Assert.IsTrue(result.Count == 0);
        }

        [TestMethod]
        public void SubstancesService_GetByQuery_MeltingPointMoreOrEqualTo_Success()
        {
            _substances = new List<Substance>()
            {
                new Substance()
                {
                    Id = 1,
                    MeltingPoint = 0
                },
                new Substance()
                {
                    Id = 2,
                    MeltingPoint = 5
                },
                new Substance()
                {
                    Id = 3,
                    MeltingPoint = 10
                }
            };

            _elements = new List<Element>();

            const double meltingPointLowerBound = 5;
            var model = new Models.Search.QueryModel { mp1 = meltingPointLowerBound };
            var result = _substancesService.GetByQuery(model);

            Assert.IsTrue(result.Count == 2);
            Assert.IsTrue(
                result.TrueForAll(
                    item => _substances.First(x => x.Id.Equals(item.Id)).MeltingPoint >= meltingPointLowerBound));
        }

        [TestMethod]
        public void SubstancesService_GetByQuery_MeltingPointLessOrEqualTo_Success()
        {
            _substances = new List<Substance>()
            {
                new Substance()
                {
                    Id = 1,
                    MeltingPoint = 0
                },
                new Substance()
                {
                    Id = 2,
                    MeltingPoint = 5
                },
                new Substance()
                {
                    Id = 3,
                    MeltingPoint = 10
                }
            };

            _elements = new List<Element>();

            const double meltingPointUpperBound = 5;
            var model = new Models.Search.QueryModel { mp2 = meltingPointUpperBound };
            var result = _substancesService.GetByQuery(model);

            Assert.IsTrue(result.Count == 2);
            Assert.IsTrue(
                result.TrueForAll(
                    item => _substances.First(x => x.Id.Equals(item.Id)).MeltingPoint <= meltingPointUpperBound));
        }

        [TestMethod]
        public void SubstancesService_GetByQuery_MeltingPointMoreAndLessThan_Success()
        {
            _substances = new List<Substance>()
            {
                new Substance()
                {
                    Id = 1,
                    MeltingPoint = 0
                },
                new Substance()
                {
                    Id = 2,
                    MeltingPoint = 5
                },
                new Substance()
                {
                    Id = 3,
                    MeltingPoint = 10
                }
            };

            _elements = new List<Element>();

            const double meltingPointLowerBound = 5;
            const double meltingPointUpperBound = 10;
            var model = new Models.Search.QueryModel
            {
                mp1 = meltingPointLowerBound,
                mp2 = meltingPointUpperBound
            };
            var result = _substancesService.GetByQuery(model);

            Assert.IsTrue(result.Count == 2);
            Assert.IsTrue(
                result.TrueForAll(
                    item => _substances.First(x => x.Id.Equals(item.Id)).MeltingPoint <= meltingPointUpperBound &&
                    _substances.First(x => x.Id.Equals(item.Id)).MeltingPoint >= meltingPointLowerBound));
        }

        [TestMethod]
        public void SubstancesService_GetByQuery_MeltingPointEqualTo_Success()
        {
            _substances = new List<Substance>()
            {
                new Substance()
                {
                    Id = 1,
                    MeltingPoint = 0
                },
                new Substance()
                {
                    Id = 2,
                    MeltingPoint = 5
                },
                new Substance()
                {
                    Id = 3,
                    MeltingPoint = 5
                },
                new Substance()
                {
                    Id = 4,
                    MeltingPoint = 10
                }
            };

            _elements = new List<Element>();

            const double meltingPointLowerBound = 5;
            const double meltingPointUpperBound = 5;
            var model = new Models.Search.QueryModel
            {
                mp1 = meltingPointLowerBound,
                mp2 = meltingPointUpperBound
            };
            var result = _substancesService.GetByQuery(model);

            Assert.IsTrue(result.Count == 2);
            Assert.IsTrue(
                result.TrueForAll(
                    item => Equals(_substances.First(x => x.Id.Equals(item.Id)).MeltingPoint, meltingPointUpperBound)));
        }

        [TestMethod]
        public void SubstancesService_GetByQuery_MeltingPointWrongBounds_EmptyList()
        {
            _substances = new List<Substance>()
            {
                new Substance()
                {
                    Id = 1,
                    MeltingPoint = 0
                },
                new Substance()
                {
                    Id = 2,
                    MeltingPoint = 5
                }
            };

            _elements = new List<Element>();

            const double meltingPointLowerBound = 10;
            const double meltingPointUpperBound = 5;
            var model = new Models.Search.QueryModel
            {
                mp1 = meltingPointLowerBound,
                mp2 = meltingPointUpperBound
            };
            var result = _substancesService.GetByQuery(model);

            Assert.IsTrue(result.Count == 0);
        }

        [TestMethod]
        public void SubstancesService_GetByQuery_RefractiveIndexMoreOrEqualTo_Success()
        {
            _substances = new List<Substance>()
            {
                new Substance()
                {
                    Id = 1,
                    RefractiveIndex = 0
                },
                new Substance()
                {
                    Id = 2,
                    RefractiveIndex = 5
                },
                new Substance()
                {
                    Id = 3,
                    RefractiveIndex = 10
                }
            };

            _elements = new List<Element>();

            const double refractiveIndexLowerBound = 5;
            var model = new Models.Search.QueryModel { ri1 = refractiveIndexLowerBound };
            var result = _substancesService.GetByQuery(model);

            Assert.IsTrue(result.Count == 2);
            Assert.IsTrue(
                result.TrueForAll(
                    item => _substances.First(x => x.Id.Equals(item.Id)).RefractiveIndex >= refractiveIndexLowerBound));
        }

        [TestMethod]
        public void SubstancesService_GetByQuery_RefractiveIndexLessOrEqualTo_Success()
        {
            _substances = new List<Substance>()
            {
                new Substance()
                {
                    Id = 1,
                    RefractiveIndex = 0
                },
                new Substance()
                {
                    Id = 2,
                    RefractiveIndex = 5
                },
                new Substance()
                {
                    Id = 3,
                    RefractiveIndex = 10
                }
            };

            _elements = new List<Element>();

            const double refractiveIndexUpperBound = 5;
            var model = new Models.Search.QueryModel { ri2 = refractiveIndexUpperBound };
            var result = _substancesService.GetByQuery(model);

            Assert.IsTrue(result.Count == 2);
            Assert.IsTrue(
                result.TrueForAll(
                    item => _substances.First(x => x.Id.Equals(item.Id)).RefractiveIndex <= refractiveIndexUpperBound));
        }

        [TestMethod]
        public void SubstancesService_GetByQuery_RefractiveIndexMoreAndLessThan_Success()
        {
            _substances = new List<Substance>()
            {
                new Substance()
                {
                    Id = 1,
                    RefractiveIndex = 0
                },
                new Substance()
                {
                    Id = 2,
                    RefractiveIndex = 5
                },
                new Substance()
                {
                    Id = 3,
                    RefractiveIndex = 10
                }
            };

            _elements = new List<Element>();

            const double refractiveIndexLowerBound = 5;
            const double refractiveIndexUpperBound = 10;
            var model = new Models.Search.QueryModel
            {
                ri1 = refractiveIndexLowerBound,
                ri2 = refractiveIndexUpperBound
            };
            var result = _substancesService.GetByQuery(model);

            Assert.IsTrue(result.Count == 2);
            Assert.IsTrue(
                result.TrueForAll(
                    item => _substances.First(x => x.Id.Equals(item.Id)).RefractiveIndex <= refractiveIndexUpperBound &&
                    _substances.First(x => x.Id.Equals(item.Id)).RefractiveIndex >= refractiveIndexLowerBound));
        }

        [TestMethod]
        public void SubstancesService_GetByQuery_RefractiveIndexEqualTo_Success()
        {
            _substances = new List<Substance>()
            {
                new Substance()
                {
                    Id = 1,
                    RefractiveIndex = 0
                },
                new Substance()
                {
                    Id = 2,
                    RefractiveIndex = 5
                },
                new Substance()
                {
                    Id = 3,
                    RefractiveIndex = 5
                },
                new Substance()
                {
                    Id = 4,
                    RefractiveIndex = 10
                }
            };

            _elements = new List<Element>();

            const double refractiveIndexLowerBound = 5;
            const double refractiveIndexUpperBound = 5;
            var model = new Models.Search.QueryModel
            {
                ri1 = refractiveIndexLowerBound,
                ri2 = refractiveIndexUpperBound
            };
            var result = _substancesService.GetByQuery(model);

            Assert.IsTrue(result.Count == 2);
            Assert.IsTrue(
                result.TrueForAll(
                    item => Equals(_substances.First(x => x.Id.Equals(item.Id)).RefractiveIndex, refractiveIndexUpperBound)));
        }

        [TestMethod]
        public void SubstancesService_GetByQuery_RefractiveIndexWrongBounds_EmptyList()
        {
            _substances = new List<Substance>()
            {
                new Substance()
                {
                    Id = 1,
                    RefractiveIndex = 0
                },
                new Substance()
                {
                    Id = 2,
                    RefractiveIndex = 5
                }
            };

            _elements = new List<Element>();

            const double refractiveIndexLowerBound = 10;
            const double refractiveIndexUpperBound = 5;
            var model = new Models.Search.QueryModel
            {
                ri1 = refractiveIndexLowerBound,
                ri2 = refractiveIndexUpperBound
            };
            var result = _substancesService.GetByQuery(model);

            Assert.IsTrue(result.Count == 0);
        }

       [TestMethod]
        public void SubstancesService_GetByQuery_VapourPressurMoreOrEqualTo_Success()
        {
            _substances = new List<Substance>()
            {
                new Substance()
                {
                    Id = 1,
                    VapourPressur = 0
                },
                new Substance()
                {
                    Id = 2,
                    VapourPressur = 5
                },
                new Substance()
                {
                    Id = 3,
                    VapourPressur = 10
                }
            };

            _elements = new List<Element>();

            const double vapourPressurLowerBound = 5;
            var model = new Models.Search.QueryModel { vp1 = vapourPressurLowerBound };
            var result = _substancesService.GetByQuery(model);

            Assert.IsTrue(result.Count == 2);
            Assert.IsTrue(
                result.TrueForAll(
                    item => _substances.First(x => x.Id.Equals(item.Id)).VapourPressur >= vapourPressurLowerBound));
        }

        [TestMethod]
        public void SubstancesService_GetByQuery_VapourPressurLessOrEqualTo_Success()
        {
            _substances = new List<Substance>()
            {
                new Substance()
                {
                    Id = 1,
                    VapourPressur = 0
                },
                new Substance()
                {
                    Id = 2,
                    VapourPressur = 5
                },
                new Substance()
                {
                    Id = 3,
                    VapourPressur = 10
                }
            };

            _elements = new List<Element>();

            const double vapourPressurUpperBound = 5;
            var model = new Models.Search.QueryModel { vp2 = vapourPressurUpperBound };
            var result = _substancesService.GetByQuery(model);

            Assert.IsTrue(result.Count == 2);
            Assert.IsTrue(
                result.TrueForAll(
                    item => _substances.First(x => x.Id.Equals(item.Id)).VapourPressur <= vapourPressurUpperBound));
        }

        [TestMethod]
        public void SubstancesService_GetByQuery_VapourPressurMoreAndLessThan_Success()
        {
            _substances = new List<Substance>()
            {
                new Substance()
                {
                    Id = 1,
                    VapourPressur = 0
                },
                new Substance()
                {
                    Id = 2,
                    VapourPressur = 5
                },
                new Substance()
                {
                    Id = 3,
                    VapourPressur = 10
                }
            };

            _elements = new List<Element>();

            const double vapourPressurLowerBound = 5;
            const double vapourPressurUpperBound = 10;
            var model = new Models.Search.QueryModel
            {
                vp1 = vapourPressurLowerBound,
                vp2 = vapourPressurUpperBound
            };
            var result = _substancesService.GetByQuery(model);

            Assert.IsTrue(result.Count == 2);
            Assert.IsTrue(
                result.TrueForAll(
                    item => _substances.First(x => x.Id.Equals(item.Id)).VapourPressur <= vapourPressurUpperBound &&
                    _substances.First(x => x.Id.Equals(item.Id)).VapourPressur >= vapourPressurLowerBound));
        }

        [TestMethod]
        public void SubstancesService_GetByQuery_VapourPressurEqualTo_Success()
        {
            _substances = new List<Substance>()
            {
                new Substance()
                {
                    Id = 1,
                    VapourPressur = 0
                },
                new Substance()
                {
                    Id = 2,
                    VapourPressur = 5
                },
                new Substance()
                {
                    Id = 3,
                    VapourPressur = 5
                },
                new Substance()
                {
                    Id = 4,
                    VapourPressur = 10
                }
            };

            _elements = new List<Element>();

            const double vapourPressurLowerBound = 5;
            const double vapourPressurUpperBound = 5;
            var model = new Models.Search.QueryModel
            {
                vp1 = vapourPressurLowerBound,
                vp2 = vapourPressurUpperBound
            };
            var result = _substancesService.GetByQuery(model);

            Assert.IsTrue(result.Count == 2);
            Assert.IsTrue(
                result.TrueForAll(
                    item => Equals(_substances.First(x => x.Id.Equals(item.Id)).VapourPressur, vapourPressurUpperBound)));
        }

        [TestMethod]
        public void SubstancesService_GetByQuery_VapourPressurWrongBounds_EmptyList()
        {
            _substances = new List<Substance>()
            {
                new Substance()
                {
                    Id = 1,
                    VapourPressur = 0
                },
                new Substance()
                {
                    Id = 2,
                    VapourPressur = 5
                }
            };

            _elements = new List<Element>();

            const double vapourPressurLowerBound = 10;
            const double vapourPressurUpperBound = 5;
            var model = new Models.Search.QueryModel
            {
                vp1 = vapourPressurLowerBound,
                vp2 = vapourPressurUpperBound
            };
            var result = _substancesService.GetByQuery(model);

            Assert.IsTrue(result.Count == 0);
        }

        [TestMethod]
        public void SubstancesService_GetByQuery_WaterSolubilityTrue_Sucess()
        {
            _substances = new List<Substance>()
            {
                new Substance()
                {
                    Id = 1,
                    WaterSolubility = true
                },
                new Substance()
                {
                    Id = 2,
                    WaterSolubility = true
                },
                new Substance()
                {
                    Id = 3,
                    WaterSolubility = false
                }
            };

            _elements = new List<Element>();

            const bool isWaterSolubility = true;
            var model = new Models.Search.QueryModel { ws = isWaterSolubility};
            var result = _substancesService.GetByQuery(model);

            Assert.IsTrue(result.Count == 2);
            Assert.IsTrue(
                result.TrueForAll(
                    item => Equals(_substances.First(x => x.Id.Equals(item.Id)).WaterSolubility, isWaterSolubility)));
        }

        [TestMethod]
        public void SubstancesService_GetByQuery_WaterSolubilityFalse_Sucess()
        {
            _substances = new List<Substance>()
            {
                new Substance()
                {
                    Id = 1,
                    WaterSolubility = true
                },
                new Substance()
                {
                    Id = 2,
                    WaterSolubility = true
                },
                new Substance()
                {
                    Id = 3,
                    WaterSolubility = false
                }
            };

            _elements = new List<Element>();

            const bool isWaterSolubility = false;
            var model = new Models.Search.QueryModel { ws = isWaterSolubility };
            var result = _substancesService.GetByQuery(model);

            Assert.IsTrue(result.Count == 1);
            Assert.IsTrue(
                result.TrueForAll(
                    item => Equals(_substances.First(x => x.Id.Equals(item.Id)).WaterSolubility, isWaterSolubility)));
        }
    }
}