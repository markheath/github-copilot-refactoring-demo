using GloboticketWeb.Utils;

namespace GloboticketWeb.Tests.Utils;

[TestFixture]
public class CsvUtilsTests
{
    [TestCase("a,b,c", "a,b,c")]
    [TestCase("a, b, c", "a,b,c")]
    [TestCase("a,,c", "a,c")]
    [TestCase(" a , b , c ", "a,b,c")]
    [TestCase(",,", "")]
    [TestCase("", "")]
    [TestCase("a", "a")]
    [TestCase(" , , ", "")]
    [TestCase("a,  ,c", "a,c")]
    public void SanitizeCsvValue_WithVariousInputs_ReturnsExpectedResult(string input, string expected)
    {
        // Act
        var result = CsvUtils.SanitizeCsvValue(input);

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }
}
