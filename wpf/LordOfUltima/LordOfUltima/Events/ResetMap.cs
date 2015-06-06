using System;
using System.Collections.Generic;
using System.Linq;
using LordOfUltima.MGameboard;

namespace LordOfUltima.Events
{
    class ResetMap
    {
        private static ResetMap _instance;
        public static ResetMap Instance
        {
            get { return _instance ?? (_instance = new ResetMap()); }
        }

        private static Gameboard _gameboard;
        private ResetMap()
        {
            _gameboard = Gameboard.Instance;
        }

        /*
         * Verify each element to see if they are valid
        */
        public void VerifyMap()
        {
            Element[,] map = _gameboard.GetMap();

            // coin haut gauche
            map[0, 0].setInvalid(); map[0, 1].setInvalid(); map[1, 0].setInvalid();
            // coin bas gauche
            map[0, 18].setInvalid(); map[0, 17].setInvalid(); map[1, 18].setInvalid();
            // coin haut droit
            map[18, 0].setInvalid(); map[17, 0].setInvalid(); map[18, 1].setInvalid();
            // coin bas droit
            map[18, 18].setInvalid(); map[18, 17].setInvalid(); map[17, 18].setInvalid();

            for (int i = 0; i < 5; i++)
            {
                map[i, 9].setInvalid(); map[18 - i, 9].setInvalid();
                map[9, i].setInvalid(); map[9, 18 - i].setInvalid();
            }

            for (int i = 0; i < 7; i++)
            {
                map[6 + i, 4].setInvalid(); map[6 + i, 14].setInvalid();
                map[4, 6 + i].setInvalid(); map[14, 6 + i].setInvalid();
            }

            map[5, 5].setInvalid(); map[6, 5].setInvalid(); map[5, 6].setInvalid();
            map[5, 13].setInvalid(); map[5, 12].setInvalid(); map[6, 13].setInvalid();

            map[13, 5].setInvalid(); map[12, 5].setInvalid(); map[13, 6].setInvalid();
            map[13, 13].setInvalid(); map[13, 12].setInvalid(); map[12, 13].setInvalid();

            // Default img for townhall
            map[9, 9].setElementType(new TownHallElementType());
            map[9, 9].Level = 1;
        }

        public void InitialiseNewGame()
        {
            Element element;
            for (int i = 0; i < 6; i++)
            {
                // wood ressources
                element = getRandomValid();
                element.setElementType(new ForestElementType());
                expandRessource(element, ElementType.Type.RESSOURCE_FOREST);

                // stone ressources
                element = getRandomValid();
                element.setElementType(new StoneElementType());
                expandRessource(element, ElementType.Type.RESSOURCE_STONE);

                // iron ressources
                element = getRandomValid();
                element.setElementType(new IronElementType());
                expandRessource(element, ElementType.Type.RESSOURCE_IRON);
            }
            for (int i = 0; i < 4; i++)
            {
                // Lake ressources
                element = getRandomValid();
                element.setElementType(new LakeElementType());
            }
        }

        private Element getRandomValid()
        {
            Element[,] map = _gameboard.GetMap();
            int frameCount = _gameboard.GetFrameCount;
            int x,y;
            do
            {
                x = Random.Next(0, frameCount);
                y = Random.Next(0, frameCount);
            } while (!map[x, y].getInvalid() || map[x, y].HasElement);

            return map[x, y];
        }

        private void expandRessource(Element element, ElementType.Type elementType)
        {
            List<Element> firstLayer = new List<Element>();

            firstLayer = firstLayer.Concat(rotateElement(element, elementType, 2)).ToList();

            foreach (var secondElement in firstLayer)
            {
                rotateElement(secondElement, elementType, 3);
            }
        }

        private static readonly Random Random = new Random(Guid.NewGuid().GetHashCode());
        private List<Element> rotateElement(Element element, ElementType.Type elementType, int chance)
        {
            List<Element> newElements = new List<Element>();
            List<Element> testList = new List<Element>();
            Element[,] map = Gameboard.Instance.GetMap();
            int frameCount = Gameboard.Instance.GetFrameCount;

            if (element.PositionX > 0)
            {
                if (Random.Next(chance) == chance)
                {
                    testList.Add(map[element.PositionX - 1, element.PositionY]);
                }
            }
            if (element.PositionX < frameCount - 1)
            {
                if (Random.Next(chance) == chance - 1)
                {
                    testList.Add(map[element.PositionX + 1, element.PositionY]);
                }
            }
            if (element.PositionY > 0)
            {
                if (Random.Next(chance) == chance - 1)
                {
                    testList.Add(map[element.PositionX, element.PositionY - 1]);
                }
            }
            if (element.PositionY < frameCount - 1)
            {
                if (Random.Next(chance) == chance - 1)
                {
                    testList.Add(map[element.PositionX, element.PositionY + 1]);
                }
            }

            foreach (var elementCheck in testList)
            {
                if (!elementCheck.HasElement)
                {
                    newElements.Add(elementCheck);
                    elementCheck.setElementType(ElementType.GetTypeObject(elementType));
                }
            }

            return newElements;
        }
    }
}
