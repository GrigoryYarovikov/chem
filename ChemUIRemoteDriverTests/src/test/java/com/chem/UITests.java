package com.chem;

import org.openqa.selenium.By;
import org.openqa.selenium.WebDriver;
import org.junit.After;
import org.junit.Before;
import org.junit.Test;
import org.openqa.selenium.WebElement;
import org.openqa.selenium.remote.DesiredCapabilities;
import org.openqa.selenium.remote.RemoteWebDriver;

import java.net.URL;

import static org.junit.Assert.*;

public class UITests {

    private static final String BASE_URL = "http://localhost:37007/";
    private WebDriver driver;

    @Before
    public void setUp() throws Exception {
        final URL REMOTE_DRIVER = new URL("http://localhost:9515");
        this.driver = new RemoteWebDriver(REMOTE_DRIVER, DesiredCapabilities.chrome());
    }


    private String getTextByCss(String selector) {
        return driver.findElement(By.cssSelector(selector)).getText();
    }

    private void clickLinkByText(String text) {
        driver.findElement(By.linkText(text)).click();
    }

    private void inputByXpath(String xpath, String inputValue) {
        driver.findElement(By.xpath(xpath)).sendKeys(inputValue);
    }

    private String getElementInfo() {
        return driver.findElement(By.id("main")).getText();
    }

    private void clickByCssSelector(String selector) {
        driver.findElement(By.cssSelector(selector)).click();
    }

    private void assertTextContains(String element, String substring) {
        assertTrue(element.contains(substring));
    }

    private By byAttribute(String name, String value) {
        return By.xpath("//*[@" + name + "='" + value + "']");
    }

    private WebElement findSearchCriteriaInput(String modelKey) {
        return driver.findElement(byAttribute("data-ng-model", modelKey));
    }

    private void inputSearchCriteria(String modelKey, String value) {
        findSearchCriteriaInput("query." + modelKey).sendKeys(value);
    }

    private void openSearch() throws InterruptedException {
        openBasePage();
        clickByCssSelector("span.button");
        Thread.sleep(200);
    }

    private void searchByName(String name) throws InterruptedException {
        openBasePage();
        inputByXpath("//input[@type='text']", name);
        clickByCssSelector("input.button");
        waitSearchResult();
    }

    private void waitElementOpen() throws InterruptedException {
        Thread.sleep(500);
    }

    private void waitSearchResult() throws InterruptedException {
        Thread.sleep(1000);
    }

    private void openBasePage() {
        driver.get(BASE_URL);
    }

    private void openSearchResult() throws InterruptedException {
        clickByCssSelector("button.btn.btn-primary");
        waitSearchResult();
    }

    private void inputSearchRangeCriteria(String modelKey, String value) {
        inputSearchCriteria(modelKey + "1", value);
        inputSearchCriteria(modelKey + "2", value);
    }

    private void openLinkByText(String text) throws InterruptedException {
        clickLinkByText(text);
        waitElementOpen();
    }

    @Test
    public void testRefractiveIndexEqualTo() throws Exception {
        openSearch();
        inputSearchRangeCriteria("ri", "1.467");
        openSearchResult();

        String elemInfo = getElementInfo();
        assertTextContains(elemInfo, "2: 11-докозеновая кислота");

        assertEquals("Найдено 11 результатов", getTextByCss("h3.ng-binding"));
        assertTextContains(elemInfo, "2: 11-докозеновая кислота");
        assertTextContains(elemInfo, "3: Оксирен");
        assertTextContains(elemInfo, "5: 1,4-диоксин");
        assertTextContains(elemInfo, "6: Гадолеиновая кислота");
        assertTextContains(elemInfo, "1: Тачигарен");
        clickLinkByText("Тачигарен");
        waitElementOpen();
        assertEquals("Тачигарен", getTextByCss("h1.ng-binding"));
    }

    @Test
    public void testSearchResultCounterByElementFromExampleList() throws Exception {
        openBasePage();
        clickLinkByText("Соляная кислота");
        waitElementOpen();

        assertEquals("Найдено 1 результатов", getTextByCss("h3.ng-binding"));
        assertEquals("1: Соляная кислота", getTextByCss("div.search_result-table > div.ng-scope > h3.ng-binding"));

        String elementInfo = getElementInfo();
        assertTextContains(elementInfo, "Соляная кислота; " +
                "Хлороводород; хлористоводородная; " +
                "хлористый водород; хлороводородная; " +
                "Hydrochloric acid; Hydrochloride; " +
                "Hydrogen chloride; " +
                "Muriatic acid; Spirits of salt;");
        assertTextContains(elementInfo, "Кислота; Неорганические кислоты; Неорганическое вещество;");
    }


    @Test
    public void testMainPageLoaded() throws Exception {
        openBasePage();
        WebElement element = driver.findElement(By.id("main"));
        assertNotNull(element);
    }

    @Test
    public void testSearchResultElementsContentByExampleSearchRequest() throws Exception {
        openBasePage();
        clickLinkByText("H3CCH2CH2COOH");
        waitElementOpen();

        assertEquals("Найдено 13 результатов", getTextByCss("h3.ng-binding"));
        assertEquals("1: 3-гидроксибутаналь", getTextByCss("div.search_result-table > div.ng-scope > h3.ng-binding"));

        String elementInfo = getElementInfo();
        assertTextContains(elementInfo, "2: Масляная кислота");
        assertTextContains(elementInfo, "3: 1,4-диоксан");
        assertTextContains(elementInfo, "4: Этилацетат");
    }


    @Test
    public void testExampleSearchRequest() throws Exception {
        openBasePage();
        clickLinkByText("C5H10O4");
        waitElementOpen();

        assertEquals("Найдено 3 результатов", getTextByCss("h3.ng-binding"));
        assertEquals("1: трис-метилолацетальдегид", getTextByCss("div.search_result-table > div.ng-scope > h3.ng-binding"));

        String elementInfo = getElementInfo();
        assertTextContains(elementInfo, "2: D-Дезоксирибоза");
        assertTextContains(elementInfo, "3: Дезоксирибопираноза");
    }

    @Test
    public void testElementNameNotEmpty() throws Exception {
        openBasePage();
        clickLinkByText("D-Дезоксирибоза");
        waitElementOpen();

        assertEquals("Найдено 1 результатов", getTextByCss("h3.ng-binding"));
        assertEquals("1: D-Дезоксирибоза", getTextByCss("div.search_result-table > div.ng-scope > h3.ng-binding"));
        assertEquals("C5H10O4", getTextByCss("td.table_cell-value.ng-binding"));

        String elementInfo = getElementInfo();
        assertTextContains(elementInfo, "2-Deoxy-D-erythropentose; 2-Deoxy-D-ribose; " +
                "BRN 1721978; D-2-Deoxyribose; D-Deoxyribose; " +
                "DEOXYRIBOSE; EINECS 208-573-0; SBB067136;");
        assertTextContains(elementInfo, "Органическое вещество; Углеводы; Сахар; Моносахариды; Альдопентозы;");
    }

    @Test
    public void testMeltPointOnElementPage() throws Exception {
        searchByName("4-нитрофенол");

        assertEquals("Найдено 1 результатов", getTextByCss("h3.ng-binding"));
        assertEquals("1: 4-нитрофенол", getTextByCss("div.search_result-table > div.ng-scope > h3.ng-binding"));
        assertEquals("C6H5NO3", getTextByCss("td.table_cell-value.ng-binding"));
        assertTextContains(getElementInfo(), "4-нитрофенол; п-нитрофенол; 1-Hydroxy-4-nitrobenzene;");
        assertTextContains(getElementInfo(), "Органическое вещество; Ароматические соединения; Фенолы;");
        openLinkByText("4-нитрофенол");

        String elementInfo = getElementInfo();
        assertEquals("4-нитрофенол", getTextByCss("h1.ng-binding"));
        assertEquals("C6H5NO3", getTextByCss("td.table_cell-value.ng-binding"));
        assertTextContains(elementInfo, "Органическое вещество;");
        assertTextContains(elementInfo, "p-Nitrophenol;");
        assertTextContains(elementInfo, "139.111");
        assertTextContains(elementInfo, "279°C");
        assertTextContains(elementInfo, "141.9°C");
        assertTextContains(elementInfo, "0.00243мм рт.ст.");
        assertTextContains(elementInfo, "да");
        assertTextContains(elementInfo, "Xn");
    }


    @Test
    public void testElementFormula() throws Exception {
        searchByName("Тачигарен");

        assertEquals("Найдено 1 результатов", getTextByCss("h3.ng-binding"));
        assertEquals("1: Тачигарен", getTextByCss("div.search_result-table > div.ng-scope > h3.ng-binding"));
        assertEquals("C4H5NO2", getTextByCss("td.table_cell-value.ng-binding"));
        assertTextContains(getElementInfo(), "Тачигарен; гимексазол; 3 (2H)-Isoxazolone, 5-methyl-;");
        assertTextContains(getElementInfo(), "Органическое вещество; Гетероциклы; Пестициды; Фунгициды;");
        openLinkByText("Тачигарен");

        String elementInfo = getElementInfo();
        assertEquals("Тачигарен", getTextByCss("h1.ng-binding"));
        assertEquals("C4H5NO2", getTextByCss("td.table_cell-value.ng-binding"));
        assertTextContains(elementInfo, "Органическое вещество; Гетероциклы; Пестициды; Фунгициды;");
        assertTextContains(elementInfo, "Тачигарен; гимексазол; 3 (2H)-Isoxazolone, 5-methyl-;");
        assertTextContains(elementInfo, "99.090");
        assertTextContains(elementInfo, "228.2°C");
        assertTextContains(elementInfo, "91.8°C");
        assertTextContains(elementInfo, "1.185г/см3");
        assertTextContains(elementInfo, "0.0493мм рт.ст.");
        assertTextContains(elementInfo, "1.467");
    }

    @Test
    public void testCategoriesOnElementPage() throws Exception {
        searchByName("Кремнекислый калий");

        assertEquals("Найдено 1 результатов", getTextByCss("h3.ng-binding"));
        assertEquals("1: Кремнекислый калий", getTextByCss("div.search_result-table > div.ng-scope > h3.ng-binding"));
        assertEquals("K2O3Si", getTextByCss("td.table_cell-value.ng-binding"));
        assertTextContains(getElementInfo(), "Кремнекислый калий; Метасиликат калия; силикат калия;");
        assertTextContains(getElementInfo(), "Неорганическое вещество; Неорганические соли; Соли;");
        assertEquals("K2SiO3", getTextByCss("td.table_cell-img"));
        openLinkByText("Кремнекислый калий");

        assertEquals("Кремнекислый калий", getTextByCss("h1.ng-binding"));
        assertEquals("K2O3Si", getTextByCss("td.table_cell-value.ng-binding"));

        String elementInfo = getElementInfo();
        assertTextContains(elementInfo, "Неорганическое вещество; Неорганические соли; Соли;");
        assertTextContains(elementInfo, "Silicic acid (H2SiO3), dipotassium salt;");
        assertTextContains(elementInfo, "K2SiO3");
        assertTextContains(elementInfo, "154.280");
    }

    @Test
    public void testDensityOnElementPage() throws Exception {
        searchByName("α-циклодекстрин");

        assertEquals("Найдено 1 результатов", getTextByCss("h3.ng-binding"));
        assertEquals("1: α-циклодекстрин", getTextByCss("div.search_result-table > div.ng-scope > h3.ng-binding"));
        assertEquals("C36H60O30", getTextByCss("td.table_cell-value.ng-binding"));
        assertTextContains(getElementInfo(), "α-циклодекстрин; альфа-циклодекстрин; Alfadex; Celdex A 100;");
        assertTextContains(getElementInfo(), "Органическое вещество; Углеводы;");
        openLinkByText("α-циклодекстрин");

        String elementInfo = getElementInfo();
        assertEquals("α-циклодекстрин", getTextByCss("h1.ng-binding"));
        assertEquals("C36H60O30", getTextByCss("td.table_cell-value.ng-binding"));
        assertTextContains(elementInfo, "Органическое вещество; Углеводы;");
        assertTextContains(elementInfo, "α-циклодекстрин; альфа-циклодекстрин; Alfadex; Celdex A 100;");
        assertTextContains(elementInfo, "972.859");
        assertTextContains(elementInfo, "6.216%");
        assertTextContains(elementInfo, "1410.846°C");
        assertTextContains(elementInfo, "807.051°C");
        assertTextContains(elementInfo, "1.624г/см3");
        assertTextContains(elementInfo, "1.591");
    }

    @Test
    public void testElementDescription() throws Exception {
        searchByName("Пентахлорид фосфора");

        assertEquals("Найдено 1 результатов", getTextByCss("h3.ng-binding"));
        assertEquals("1: Пентахлорид фосфора", getTextByCss("div.search_result-table > div.ng-scope > h3.ng-binding"));
        assertEquals("Cl5P", getTextByCss("td.table_cell-value.ng-binding"));
        assertTextContains(getElementInfo(), "Пентахлорид фосфора; Хлорид фосфора(V); пятихлористый фосфор;");
        assertTextContains(getElementInfo(), "Неорганическое вещество; Бинарные соединения;");
        openLinkByText("Пентахлорид фосфора");

        String elementInfo = getElementInfo();
        assertEquals("Пентахлорид фосфора", getTextByCss("h1.ng-binding"));
        assertEquals("Cl5P", getTextByCss("td.table_cell-value.ng-binding"));
        assertTextContains(elementInfo, "Неорганическое вещество; Бинарные соединения;");
        assertTextContains(elementInfo, "Пентахлорид фосфора; Хлорид фосфора(V); пятихлористый фосфор;");
        assertTextContains(elementInfo, "208.239");
        assertTextContains(elementInfo, "ClХлор35.453585.126%\nPФосфор30.974114.874%");
        assertTextContains(elementInfo, "179°C");
        assertTextContains(elementInfo, "T+");
    }


    @Test
    public void testVaporPointOnElementPage() throws Exception {
        searchByName("Метилсалицилат");

        assertEquals("1: Метилсалицилат", getTextByCss("div.search_result-table > div.ng-scope > h3.ng-binding"));
        assertEquals("C8H8O3", getTextByCss("td.table_cell-value.ng-binding"));
        assertTextContains(getElementInfo(), "Метилсалицилат; метиловый эфир салициловой кислоты;");
        assertTextContains(getElementInfo(), "Органическое вещество; Ароматические соединения;");
        openLinkByText("Метилсалицилат");

        String elementInfo = getElementInfo();
        assertEquals("Метилсалицилат", getTextByCss("h1.ng-binding"));
        assertEquals("C8H8O3", getTextByCss("td.table_cell-value.ng-binding"));
        assertTextContains(elementInfo, "Органическое вещество; Ароматические соединения; Лекарственное средство; Сложные эфиры;");
        assertTextContains(elementInfo, "Метилсалицилат; метиловый эфир салициловой кислоты; 119-36-8;");
        assertTextContains(elementInfo, "152.151");
        assertTextContains(elementInfo, "CУглерод12.011863.154%\nHВодород1.00885.300%\nOКислород15.999331.547%");
        assertTextContains(elementInfo, "19.4°C");
        assertTextContains(elementInfo, "106.7°C");
        assertTextContains(elementInfo, "237.2°C");
        assertTextContains(elementInfo, "0.974г/см3");
        assertTextContains(elementInfo, "0.0296мм рт.ст.");
        assertTextContains(elementInfo, "1.523");
        assertTextContains(elementInfo, "Xn");
    }

    @Test
    public void testFlashPointOnElementPage() throws Exception {
        searchByName("L-Манноза");

        assertEquals("Найдено 1 результатов", getTextByCss("h3.ng-binding"));
        assertEquals("1: L-Манноза", getTextByCss("div.search_result-table > div.ng-scope > h3.ng-binding"));
        assertEquals("C6H12O6", getTextByCss("td.table_cell-value.ng-binding"));
        assertTextContains(getElementInfo(), "L-Манноза;");
        assertTextContains(getElementInfo(), "Органическое вещество; Углеводы; Сахар; Моносахариды; Альдогексозы;");
        openLinkByText("L-Манноза");

        String elementInfo = getElementInfo();
        assertEquals("L-Манноза", getTextByCss("h1.ng-binding"));
        assertEquals("C6H12O6", getTextByCss("td.table_cell-value.ng-binding"));
        assertTextContains(elementInfo, "Органическое вещество; Углеводы; Сахар; Моносахариды; Альдогексозы;");
        assertTextContains(elementInfo, "L-Манноза; (2R,3R,4S,5S)-2,3,4,5,6-pentahydroxyhexanal; AC1L335D;");
        assertTextContains(elementInfo, "180.159");
        assertTextContains(elementInfo, "40.002%");
        assertTextContains(elementInfo, "410.8°C");
        assertTextContains(elementInfo, "202.2°C");
        assertTextContains(elementInfo, "1.732г/см3");
        assertTextContains(elementInfo, "1.83e-8мм рт.ст.");
        assertTextContains(elementInfo, "1.635");
    }


    @Test
    public void testElementSubstance() throws Exception {
        searchByName("Бромид железа(III)");

        assertEquals("Найдено 1 результатов", getTextByCss("h3.ng-binding"));
        assertEquals("1: Бромид железа(III)", getTextByCss("div.search_result-table > div.ng-scope > h3.ng-binding"));
        assertEquals("Br3Fe", getTextByCss("td.table_cell-value.ng-binding"));
        assertTextContains(getElementInfo(), "Бромид железа(III);");
        assertTextContains(getElementInfo(), "Неорганическое вещество; Неорганические соли; Соли;");
        openLinkByText("Бромид железа(III)");

        String elementInfo = getElementInfo();
        assertEquals("Бромид железа(III)", getTextByCss("h1.ng-binding"));
        assertEquals("Br3Fe", getTextByCss("td.table_cell-value.ng-binding"));
        assertTextContains(elementInfo, "Неорганическое вещество; Неорганические соли; Соли;");
        assertTextContains(elementInfo, "Бромид железа(III); Трибомид железа; EINECS:233-089-1;");
        assertTextContains(elementInfo, "295.560");
        assertTextContains(elementInfo, "BrБром79.904381.105%\nFeЖелезо55.847118.895%");
    }


    @Test
    public void testContentOnElementPage() throws Exception {
        searchByName("Гидросульфат магния");

        assertEquals("Найдено 1 результатов", getTextByCss("h3.ng-binding"));
        assertEquals("1: Гидросульфат магния", getTextByCss("div.search_result-table > div.ng-scope > h3.ng-binding"));
        assertEquals("H2MgO8S2", getTextByCss("td.table_cell-value.ng-binding"));
        assertTextContains(getElementInfo(), "Гидросульфат магния; EINECS:233-073-4; Magnesium bis(hydrogen sulfate);");
        assertTextContains(getElementInfo(), "Неорганическое вещество; Неорганические соли; Соли; Кислые соли;");
        openLinkByText("Гидросульфат магния");

        String elementInfo = getElementInfo();
        assertEquals("Гидросульфат магния", getTextByCss("h1.ng-binding"));
        assertEquals("H2MgO8S2", getTextByCss("td.table_cell-value.ng-binding"));
        assertTextContains(elementInfo, "Неорганическое вещество; Неорганические соли; Соли; Кислые соли;");
        assertTextContains(elementInfo, "Гидросульфат магния; EINECS:233-073-4; Magnesium bis(hydrogen sulfate);");
        assertTextContains(elementInfo, "218.450");
        assertTextContains(elementInfo, "HВодород1.00820.923%\nMgМагний24.305111.126%\nOКислород15.999858.593%\nSСера32.067229.358%");
    }


    @Test
    public void testElementImage() throws Exception {
        searchByName("Трифенилфосфит");

        assertEquals("Найдено 1 результатов", getTextByCss("h3.ng-binding"));
        assertEquals("1: Трифенилфосфит", getTextByCss("div.search_result-table > div.ng-scope > h3.ng-binding"));
        assertEquals("C18H15O3P", getTextByCss("td.table_cell-value.ng-binding"));
        assertTextContains(getElementInfo(), "Трифенилфосфит; трифениловый эфир фосфористой кислоты; " +
                "Advance TPP; EFED; EINECS:202-908-4; Mellite 310; NSC 43789; " +
                "Phenyl phosphite; Phosclere T 36; Phosphorous Acid Triphenyl Ester;");
        assertTextContains(getElementInfo(), "Органическое вещество; Ароматические соединения; Фосфорорганические соединения;");
        openLinkByText("Трифенилфосфит");

        String elementInfo = getElementInfo();
        assertEquals("C18H15O3P", getTextByCss("td.table_cell-value.ng-binding"));
        assertTextContains(elementInfo, "Органическое вещество; Ароматические соединения; Фосфорорганические соединения;");
        assertTextContains(elementInfo, "Трифенилфосфит; трифениловый эфир фосфористой кислоты; Advance TPP;");
        assertTextContains(elementInfo, "310.291");
        assertTextContains(elementInfo, "69.676%");
        assertTextContains(elementInfo, "22°C");
        assertTextContains(elementInfo, "360°C");
        assertTextContains(elementInfo, "218.3°C");
        assertTextContains(elementInfo, "0.0000474мм рт.ст.");
        assertTextContains(elementInfo, "Xi,N");
    }

    @Test
    public void testMeltingPointCriteriaSearch() throws Exception {
        openSearch();
        inputSearchRangeCriteria("mp", "-259");
        openSearchResult();
        assertEquals("1: Водород", getTextByCss("div.search_result-table > div.ng-scope > h3.ng-binding"));
        assertEquals("Найдено 1 результатов", getTextByCss("h3.ng-binding"));
        openLinkByText("Водород");
        
        assertEquals("Водород", getTextByCss("h1.ng-binding"));
        assertTextContains(getElementInfo(), "-259°C");
    }

    @Test
    public void testBoilingPointCriteriaSearch() throws Exception {
        openSearch();
        inputSearchRangeCriteria("bp", "228");
        openSearchResult();
        assertEquals("Найдено 2 результатов", getTextByCss("h3.ng-binding"));
        assertTextContains(getElementInfo(), "2: 3,4-Ксилидин");
        assertEquals("1: бутилбутират", getTextByCss("div.search_result-table > div.ng-scope > h3.ng-binding"));
        openLinkByText("бутилбутират");

        assertEquals("бутилбутират", getTextByCss("h1.ng-binding"));
        assertTextContains(getElementInfo(), "228°C");
    }


    @Test
    public void testFlashPointCriteriaSearch() throws Exception {
        openSearch();
        inputSearchRangeCriteria("fp", "-100");
        openSearchResult();
        
        assertEquals("Найдено 1 результатов", getTextByCss("h3.ng-binding"));
        assertEquals("1: Этен", getTextByCss("div.search_result-table > div.ng-scope > h3.ng-binding"));
        openLinkByText("Этен");
        
        assertEquals("Этен", getTextByCss("h1.ng-binding"));
        assertTextContains(getElementInfo(), "-100°C");
    }

    @Test
    public void testDensitySearchCriteria() throws Exception {
        openSearch();
        inputSearchRangeCriteria("d", "1.185");
        openSearchResult();

        assertEquals("Найдено 3 результатов", getTextByCss("h3.ng-binding"));
        assertEquals("1: Тачигарен", getTextByCss("div.search_result-table > div.ng-scope > h3.ng-binding"));
        assertTextContains(getElementInfo(), "2: (E)-3-фенилпропеновая кислота");
        assertTextContains(getElementInfo(), "3: 2,5-диметокси-4-нитроамфетамин");
        openLinkByText("Тачигарен");
        
        assertEquals("Тачигарен", getTextByCss("h1.ng-binding"));
        assertTextContains(getElementInfo(), "1.185г/см3");
    }


    @Test
    public void testVaporPressureCriteriaSearch() throws Exception {
        openSearch();
        inputSearchRangeCriteria("vp", "0.0155");
        openSearchResult();

        assertEquals("Найдено 1 результатов", getTextByCss("h3.ng-binding"));
        assertEquals("1: 1-(4-метоксифенил)этанон", getTextByCss("div.search_result-table > div.ng-scope > h3.ng-binding"));
        openLinkByText("1-(4-метоксифенил)этанон");
        
        assertEquals("1-(4-метоксифенил)этанон", getTextByCss("h1.ng-binding"));
        assertTextContains(getElementInfo(), "0.0155мм рт.ст.");
    }

    @Test
    public void testWaterSolubilityCriteriaSearch() throws Exception {
        openSearch();
        inputSearchCriteria("ws", "да");
        openSearchResult();
        
        assertEquals("Представлены первые 100 элементов по запросу", getTextByCss("h3"));
        assertEquals("1: 4-нитрофенол", getTextByCss("div.search_result-table > div.ng-scope > h3.ng-binding"));
        openLinkByText("4-нитрофенол");
        
        assertEquals("4-нитрофенол", getTextByCss("h1.ng-binding"));
        assertTextContains(getElementInfo(), "да");
    }

    @Test
    public void testBoilingAndMeltingCriteriaSearch() throws Exception {
        openSearch();
        inputSearchRangeCriteria("mp", "112");
        inputSearchRangeCriteria("bp", "279");
        openSearchResult();

        assertEquals("Найдено 1 результатов", getTextByCss("h3.ng-binding"));
        assertEquals("1: 4-нитрофенол", getTextByCss("div.search_result-table > div.ng-scope > h3.ng-binding"));
        openLinkByText("4-нитрофенол");

        assertEquals("4-нитрофенол", getTextByCss("h1.ng-binding"));
        assertTextContains(getElementInfo(), "279°C");
        assertTextContains(getElementInfo(), "112°C");
    }

    @Test
    public void testMeltingAndDensityCriteriaSearch() throws Exception {
        openSearch();
        inputSearchRangeCriteria("mp", "181");
        inputSearchRangeCriteria("d", "1.208");
        openSearchResult();

        assertEquals("1: 4-Метоксибензойная кислота", getTextByCss("div.search_result-table > div.ng-scope > h3.ng-binding"));
        assertEquals("Найдено 1 результатов", getTextByCss("h3.ng-binding"));
        openLinkByText("4-Метоксибензойная кислота");

        assertEquals("4-Метоксибензойная кислота", getTextByCss("h1.ng-binding"));
        assertTextContains(getElementInfo(), "181°C");
        assertTextContains(getElementInfo(), "1.208г/см3");
    }


    @Test
    public void testBoilingAndDensityCriteriaSearch() throws Exception {
        openSearch();
        inputSearchRangeCriteria("bp", "408.2");
        inputSearchRangeCriteria("d", "0.948");
        openSearchResult();

        assertEquals("Найдено 1 результатов", getTextByCss("h3.ng-binding"));
        assertEquals("1: 13-(2-циклопентенил)тридекановая кислота", getTextByCss("div.search_result-table > div.ng-scope > h3.ng-binding"));
        openLinkByText("13-(2-циклопентенил)тридекановая кислота");
        
        assertEquals("13-(2-циклопентенил)тридекановая кислота", getTextByCss("h1.ng-binding"));
        assertTextContains(getElementInfo(), "408.2°C");
        assertTextContains(getElementInfo(), "0.948г/см3");
    }


    @After
    public void tearDown() throws Exception {
        driver.close();
    }
}
