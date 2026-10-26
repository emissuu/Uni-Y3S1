namespace Lab11.Tests;

public class CalculatorTests
{
    private Calculator<int> _intCalc = null!;
    private Calculator<float> _floatCalc = null!;
    private Calculator<double> _doubleCalc = null!;
    private Calculator<decimal> _decimalCalc = null!;
    
    [SetUp]
    public void Setup()
    {
        _intCalc = new Calculator<int>();
        _floatCalc = new Calculator<float>();
        _doubleCalc = new Calculator<double>();
        _decimalCalc = new Calculator<decimal>();
    }

    [Test]
    public void OtherTypesAreNotAllowed()
    {
        Assert.Throws<NotSupportedException>(() => new Calculator<long>());
        Assert.Throws<NotSupportedException>(() => new Calculator<uint>());
        Assert.Throws<NotSupportedException>(() => new Calculator<sbyte>());
    }
    
    [Test]
    public void AddReturnsSum()
    {
        Assert.That(_intCalc.Add(1, -2), Is.EqualTo(-1));
        Assert.That(_floatCalc.Add(1.5f, -2f), Is.EqualTo(-0.5f));
        Assert.That(_doubleCalc.Add(1.5, -2), Is.EqualTo(-0.5));
        Assert.That(_decimalCalc.Add(1.5m, -2), Is.EqualTo(-0.5m));
    }
    
    [Test]
    public void SubtractReturnsDifference()
    {
        Assert.That(_intCalc.Subtract(1, -2), Is.EqualTo(3));
        Assert.That(_floatCalc.Subtract(1.5f, -2f), Is.EqualTo(3.5f));
        Assert.That(_doubleCalc.Subtract(1.5, -2), Is.EqualTo(3.5));
        Assert.That(_decimalCalc.Subtract(1.5m, -2), Is.EqualTo(3.5m));
    }
    
    [Test]
    public void MultiplyReturnsProduct()
    {
        Assert.That(_intCalc.Multiply(1, -2), Is.EqualTo(-2));
        Assert.That(_floatCalc.Multiply(1.5f, -2f), Is.EqualTo(-3f));
        Assert.That(_doubleCalc.Multiply(1.5, -2), Is.EqualTo(-3));
        Assert.That(_decimalCalc.Multiply(1.5m, -2), Is.EqualTo(-3m));
    }
    
    [Test]
    public void DivideReturnsQuotient()
    {
        Assert.That(_intCalc.Divide(78, 2), Is.EqualTo(39));
        Assert.That(_floatCalc.Divide(1.5f, -2f), Is.EqualTo(-0.75f));
        Assert.That(_doubleCalc.Divide(1.5, -2), Is.EqualTo(-0.75));
        Assert.That(_decimalCalc.Divide(1.5m, -2), Is.EqualTo(-0.75m));
    }

    [Test]
    public void DivideByZeroThrows()
    {
        Assert.Throws<DivideByZeroException>(() => _intCalc.Divide(562, 0));
        Assert.Throws<DivideByZeroException>(() => _floatCalc.Divide(562, 0));
        Assert.Throws<DivideByZeroException>(() => _doubleCalc.Divide(562, 0));
        Assert.Throws<DivideByZeroException>(() => _decimalCalc.Divide(562, 0));
    }

    [Test]
    public void PowWithZeroReturnsOne()
    {
        Assert.That(_intCalc.Pow(-1, 0), Is.EqualTo(1));
        Assert.That(_floatCalc.Pow(543, 0), Is.EqualTo(1));
        Assert.That(_doubleCalc.Pow(-1, 0), Is.EqualTo(1));
        Assert.That(_decimalCalc.Pow(4, 0), Is.EqualTo(1));
    }

    [Test]
    public void PowWithPositiveReturnsPower()
    {
        Assert.That(_intCalc.Pow(2, 5), Is.EqualTo(32));
        Assert.That(_floatCalc.Pow(2, 6), Is.EqualTo(64f));
        Assert.That(_doubleCalc.Pow(2, 7), Is.EqualTo(128));
        Assert.That(_decimalCalc.Pow(2, 8), Is.EqualTo(256m));
    }

    [Test]
    public void PowWithNegativeReturnsPower()
    {
        Assert.That(_intCalc.Pow(2, -5), Is.EqualTo(0));
        Assert.That(_floatCalc.Pow(2, -6), Is.EqualTo(1/64f));
        Assert.That(_doubleCalc.Pow(2, -7), Is.EqualTo(1/128d));
        Assert.That(_decimalCalc.Pow(2, -8), Is.EqualTo(1/256m));
    }
}