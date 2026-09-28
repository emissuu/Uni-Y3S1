namespace Lab4;

class CoffeeMachine : ICoffeeMachine
    {
        private bool _isWaterHeatUp = false;
        private int _coffeeBeans;
        private int _amountWater;
        
        public CoffeeMachine(int coffeeBeans,  int amountWater) => 
        (_coffeeBeans, _amountWater) = (coffeeBeans, amountWater);

        public bool IsWaterHeatUp => _isWaterHeatUp;

        public int HowMuchCoffeeBeans => _coffeeBeans;

        public int HowMuchWater => _amountWater;

        public void MakeEspresso()
        {
            Console.WriteLine("Making espresso...");
            Thread.Sleep(500);
            if (!isEnoughWater(500) || !GrindBeans(20)) return;
            HeatWater(500);
            Console.WriteLine("Finishing...");
            Thread.Sleep(500);
            Console.WriteLine("Your espresso is finished!");
        }

        public void MakeLatte()
        {
            Console.WriteLine("Making latte...");
            Thread.Sleep(500);
            if (!isEnoughWater(600) || !GrindBeans(25)) return;
            HeatWater(600);
            Console.WriteLine("Finishing...");
            Thread.Sleep(700);
            Console.WriteLine("Your latte is finished!");
        }

        private bool GrindBeans(int amountBeans)
        {
            if (_coffeeBeans >= amountBeans)
            {
                Console.WriteLine($"Grinding {amountBeans}g of beans...");
                Thread.Sleep(35 * amountBeans);
                _coffeeBeans -= amountBeans;
                return true;
            }
            else
            {
                Console.WriteLine($"Not enough beans. No drinks for you D:");
                return false;
            }
        }

        private bool isEnoughWater(int amountWater)
        {
            if (_amountWater >= amountWater)
            {
                return true;
            }
            Console.WriteLine($"Not enough water. No drinks for you D:");
            return false;
        }

        private void HeatWater(int amountWater)
        {
            if (!_isWaterHeatUp)
            {
                Console.WriteLine("Heating water...");
                _amountWater -= amountWater;
                Thread.Sleep(700);
            }
            else
            {
                Console.WriteLine("Water is hot enough");
            }
        }
    }