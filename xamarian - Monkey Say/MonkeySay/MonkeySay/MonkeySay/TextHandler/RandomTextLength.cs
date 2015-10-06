using System;

namespace MonkeySay.TextHandler
{
    class RandomTextLength
    {
        private readonly Random _random;
        public RandomTextLength()
        {
            _random = new Random();
        }

        public int GetRandomTextLength()
        {
            var value = 0;
            var randomNumber = _random.Next(1, 100);
            switch (randomNumber)
            {
                case 1:
                    value = 1;
                    break;
                case 2:
                    value = 2;
                    break;
                default:
                    if (randomNumber < 6)
                        value = 3;
                    else if (randomNumber < 13)
                        value = 4;
                    else if (randomNumber < 23)
                        value = 5;
                    else if (randomNumber < 36)
                        value = 6;
                    else if (randomNumber < 50)
                        value = 7;
                    else if (randomNumber < 63)
                        value = 8;
                    else if (randomNumber < 75)
                        value = 9;
                    else if (randomNumber < 85)
                        value = 10;
                    else if (randomNumber < 92)
                        value = 11;
                    else if (randomNumber < 96)
                        value = 12;
                    else switch (randomNumber)
                    {
                        case 97:
                            value = 13;
                            break;
                        case 98:
                            value = 14;
                            break;
                        case 99:
                            value = 15;
                            break;
                        case 100:
                            value = 16;
                            break;
                    }
                    break;
            }
            return value;
        }

        public int GetRandomSyllablesCount()
        {
            var value = 0;
            var randomNumber = _random.Next(1, 100);

            if (randomNumber < 10)
                value = 1;
            else if (randomNumber < 55)
                value = 2;
            else if (randomNumber < 73)
                value = 3;
            else if (randomNumber < 90)
                value = 4;
            else if (randomNumber < 95)
                value = 5;
            else if (randomNumber < 98)
                value = 6;
            else if (randomNumber < 100)
                value = 7;

            return value;
        }
    }
}
