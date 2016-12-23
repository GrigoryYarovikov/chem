let chai = require('chai');
let chaiHttp = require('chai-http');
let should = chai.should();
let assert = require('assert');


chai.use(chaiHttp);

let url = "http://localhost:37007";


function getElementById(id) {
    return chai.request(url)
        .get('/api/substances/get')
        .query({ id: id });
}

function searchElements(query) {
    var queryValue = JSON.stringify(query);
    return chai.request(url)
        .get('/api/substances/getByQuery')
        .query({ query: queryValue });
}

describe('/GET element by id', () => {

    it('it should GET HCL', (done) => {
        const HCL_ID = 3338;
        getElementById(HCL_ID)
            .end((err, res) => {
                res.should.have.status(200);
                res.should.be.a('object');
                res.body.Formula.should.equals("HCl");
                res.body.MolecularWeight.should.equals(36.461047);
                res.body.MeltingPoint.should.equals(-35.0);
                res.body.BoilingPoint.should.equals(5.8);
                res.body.VapourPressur.should.equals(1560.0);
                res.body.WaterSolubility.should.equals("да");
                res.body.HazardSymbols.should.equals("C");
                res.body.Schemes.should.be.a('array');
                res.body.Elements.should.be.a('array');
                res.body.Names.should.be.a('array');
                res.body.Categories.should.be.a('array');
                done();
            });
    });

    it('it shouldn\'t return any element', (done) => {
        const NOT_EXIST_ELEMENT_ID = 1000000000;
        getElementById(NOT_EXIST_ELEMENT_ID)
            .end((err, res) => {
                res.should.have.status(404);
                done();
            });
    });
});

describe('/SEARCH elements without any conditions', () => {

    function expectNResults(res, numberOfExpectedResults) {
        res.should.have.status(200);
        res.body.should.be.a('array');
        res.body.should.have.length(numberOfExpectedResults);
    }

    it('it should return first 100 results (empty string)', (done) => {
        searchElements({ q: "" })
            .end((err, res) => {
                expectNResults(res, 100);
                done();
            })
    });

    it('it should return first 100 results (without query)', (done) => {
        searchElements({})
            .end((err, res) => {
                expectNResults(res, 100);
                done();
            })
    });

});

describe('/SEARCH elements with conditions', () => {


    function getElementById(id) {
        return (id && chai.request(url)
            .get('/api/substances/get')
            .query({ id: id })) || {};
    }

    function forEachElement(res, next) {
        let elements = res.body;
        for (let i = 0, l = elements.length; i < l; i++) {
            let element = getElementById(elements[i]._id);
            next && next(element)
        }
    }

    function allMatch(res, fieldName, predicator) {
        forEachElement(res, (elem) => {
            let isMatch = predicator(elem[fieldName]);
            assert(isMatch)
        });
    }

    function lte(value) {
        return (other) => {
            return !other || other >= value;
        };
    }

    function gte(value) {
        return (other) => {
            return !other || other <= value;
        };
    }

    function contain(substring) {
        return (string) => {
            return typeof string !== 'string' || string.toLowerCase().substr(substring) > -1;
        }
    }


    it("test melting point minimum ", (done) => {
        searchElements({mp1:0})
            .end((err, res) => {
                allMatch(res, 'MeltingPoint', gte(0));
                done();
            });
    });

    it("test melting point maximum", (done) => {
        searchElements({mp2:0})
            .end((err, res) => {
                allMatch(res, 'MeltingPoint', lte(0));
                done();
            });
    });

    it("test search by text", (done) => {
        searchElements({q:"кислота"})
            .end((err, res) => {
                allMatch(res, 'Name', contain("кислота"));
                done();
            });
    });

    it("test boiling point minimum", (done) => {
        searchElements({bp1:10})
            .end((err, res) => {
                allMatch(res, 'BoilingPoint', gte(10));
                done();
            });
    });

    it("test boiling point maximum", (done) => {
        searchElements({bp2:10})
            .end((err, res) => {
                allMatch(res, 'BoilingPoint', lte(10));
                done();
            });
    });

    it("test flash point minimum", (done) => {
        searchElements({fp1:100})
            .end((err, res) => {
                allMatch(res, 'FlashPoint', gte(100));
                done();
            });
    });

    it("test flash point maximum", (done) => {
        searchElements({fp2:100})
            .end((err, res) => {
                allMatch(res, 'FlashPoint', lte(100));
                done();
            });
    });

    it("test density minimum", (done) => {
        searchElements({d1:1})
            .end((err, res) => {
                allMatch(res, 'Density', gte(1));
                done();
            });
    });

    it("test density maximum", (done) => {
        searchElements({d2:1})
            .end((err, res) => {
                allMatch(res, 'Density', lte(1));
                done();
            });
    });

    it("test vapour pressure minimum", (done) => {
        searchElements({vp1:0.158})
            .end((err, res) => {
                allMatch(res, 'VapourPressur', gte(0.158));
                done();
            });
    });

    it("test vapour pressure maximum", (done) => {
        searchElements({vp2:0.158})
            .end((err, res) => {
                allMatch(res, 'VapourPressur', lte(0.158));
                done();
            });
    });

    it("test reflective index minimum", (done) => {
        searchElements({ri1:1.329})
            .end((err, res) => {
                allMatch(res, 'RefractiveIndex', gte(1.329));
                done();
            });
    });

    it("test reflective index maximum", (done) => {
        searchElements({ri2:1.329})
            .end((err, res) => {
                allMatch(res, 'RefractiveIndex', lte(1.329));
                done();
            });
    });

    it("test water sublimity", (done) => {
        searchElements({ws:"да"})
            .end((err, res) => {
                allMatch(res, 'WaterSolubility', contain("да"));
                done();
            });
    });

    it("test water un sublimity", (done) => {
        searchElements({ws:"нет"})
            .end((err, res) => {
                allMatch(res, 'WaterSolubility', contain("нет"));
                done();
            });
    });

    it("test several conditions", (done) => {
        searchElements({bp2:10, d2: 1})
            .end((err, res) => {
                allMatch(res, 'BoilingPoint', lte(10));
                allMatch(res, 'Density', lte(1));
                done();
            });
    });

});
