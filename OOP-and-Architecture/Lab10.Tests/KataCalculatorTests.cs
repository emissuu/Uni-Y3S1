namespace Lab10.Tests;

public class Tests
{
    private KataCalculator _calc;
    
    [SetUp]
    public void Setup()
    {
        _calc = new KataCalculator();
    }

    [Test]
    public void EmptyStringReturnsZero()
    {
        var result = _calc.Add("");
        
        Assert.That(result, Is.EqualTo("0"));
    }
    
    [Test]
    public void SingleNumberReturnsTheNumber()
    {
        var result = _calc.Add("2");
        
        Assert.That(result, Is.EqualTo("2"));
    }

    [Test]
    public void MultipleNumbersReturnsTheSum()
    {
        var result = _calc.Add("4,2");
        
        Assert.That(result, Is.EqualTo("6"));
    }

    [Test]
    public void MoreThanTwoNumbersReturnTheSum()
    {
        var result = _calc.Add("4,2,7,1");
        
        Assert.That(result, Is.EqualTo("14"));
    }

    [Test]
    public void NewlineSeparatorWorks()
    {
        var result = _calc.Add("4,2\n7,1");
        
        Assert.That(result, Is.EqualTo("14"));
    }
    
    [Test]
    public void NewlineSeparatorNearTypicalSeparatorReturnsError()
    {
        var result = _calc.Add("4,2,\n7,1");
        
        Assert.That(result, Is.EqualTo("Number expected but '\n' found at position 4."));
    }
    
    [Test]
    public void DelimiterAtTheEndReturnsError()
    {
        var result = _calc.Add("4,2,7,1,");
        
        Assert.That(result, Is.EqualTo("Number expected but EOF found."));
    }
    
    [Test]
    public void CustomDelimiterMustBeSupported()
    {
        var result = _calc.Add("//;\n4;2;7;1");
        
        Assert.That(result, Is.EqualTo("14"));
    }
    
    [Test]
    public void LongCustomDelimiterMustBeSupported()
    {
        var result = _calc.Add("//separator\n4separator2separator9separator1");
        
        Assert.That(result, Is.EqualTo("16"));
    }
    
    [Test]
    public void DelimiterNotSpecifiedReturnsError()
    {
        var result = _calc.Add("//;\n4,2;7;1");
        
        Assert.That(result, Is.EqualTo("';' expected but ',' found at position 1."));
    }
    
    [Test]
    public void NegativeNotAllowed()
    {
        var result = _calc.Add("-1,6,-4,2");
        
        Assert.That(result, Is.EqualTo("Negative not allowed: -1, -4"));
    }

    [Test]
    public void MultipleErrorsReturnAllOfThem()
    {
        var result = _calc.Add("-1,,2");
        
        Assert.That(result, Is.EqualTo("Number expected but ',' found at position 3.\nNegative not allowed: -1"));
    }

    [Test]
    public void IntAddMustThrowOnError()
    {
        var exception = Assert.Throws<FormatException>(() => _calc.IntAdd("-1,,2"));
        
        Assert.That(exception.Message, Is.EqualTo("Number expected but ',' found at position 3.\nNegative not allowed: -1"));
    }

    [Test]
    public void MultiplyReturnsProduct()
    {
        var result = _calc.Multiply("4,2");
        
        Assert.That(result, Is.EqualTo("8"));
    }
}