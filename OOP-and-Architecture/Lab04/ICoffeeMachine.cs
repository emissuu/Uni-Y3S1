namespace Lab4;

interface ICoffeeMachine
{
    public bool IsWaterHeatUp { get; }
    public int HowMuchCoffeeBeans { get; }
    public int HowMuchWater { get; }

    public void MakeEspresso();
    public void MakeLatte();
}