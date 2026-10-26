using System.Numerics;

namespace Lab11;

public class Calculator<T> where T : INumber<T>
{
    private readonly string[] _supported = ["Int32", "Single", "Double", "Decimal"];
    public Calculator()
    {
        if (!_supported.Contains(typeof(T).Name))
            throw new NotSupportedException($"{typeof(T).Name} is not supported");
    }
    
    public T Add(T a, T b) => a + b;

    public T Subtract(T a, T b) => a - b;

    public T Multiply(T a, T b) => a * b;

    public T Divide(T a, T b)
    {
        if (b == T.Zero)
            throw new DivideByZeroException();
        return a / b;
    }

    public T Pow(T a, int exp)
    {
        if (exp == 0) return T.One;
        else
        {
            T prod = T.One;
            for(int i = 0; i < Math.Abs(exp); i++)
                prod *= a;
            if (exp > 0) return prod;
            else return T.One / prod;
        }
    }
}