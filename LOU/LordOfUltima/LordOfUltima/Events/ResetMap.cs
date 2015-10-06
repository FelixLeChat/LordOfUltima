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
            map[0, 0].IsValid = false; map[0, 1].IsValid = false; map[1, 0].IsValid = false;
            // coin bas gauche
            map[0, 18].IsValid = false; map[0, 17].IsValid = false; map[1, 18].IsValid = false;
            // coin haut droit
            map[18, 0].IsValid = false; map[17, 0].IsValid = false; map[18, 1].IsValid = false;
            // coin bas droit
            map[18, 18].IsValid = false; map[18, 17].IsValid = false; map[17, 18].IsValid = false;

            for (int i = 0; i < 5; i++)
            {
                map[i, 9].IsValid = false; map[18 - i, 9].IsValid = false;
                map[9, i].IsValid = false; map[9, 18 - i].IsValid = false;
            }

            for (int i = 0; i < 7; i++)
            {
                map[6 + i, 4].IsValid = false; map[6 + i, 14].IsValid = false;
                map[4, 6 + i].IsValid = false; map[14, 6 + i].IsValid = false;
            }

            map[5, 5].IsValid = false; map[6, 5].IsValid = false; map[5, 6].IsValid = false;
            map[5, 13].IsValid = false; map[5, 12].IsValid = false; map[6, 13].IsValid = false;

            map[13, 5].IsValid = false; map[12, 5].IsValid = false; map[13, 6].IsValid = false;
            map[13, 13].IsValid = false; map[13, 12].IsValid = false; map[12, 13].IsValid = false;

            // Default img for townhall
            map[9, 9].SetElementType(new TownHallElementType());
            map[9, 9].Level = 1;
        }

        public void InitialiseNewGame()
        {
            Element element;
            for (int i = 0; i < 6; i++)
            {
                // wood ressources
                element = getRandomValid();
                element.SetElementType(new ForestElementType());
                expandRessource(element, ElementType.Type.RESSOURCE_FOREST);

                // stone ressources
                element = getRandomValid();
                element.SetElementType(new StoneElementType());
                expandRessource(element, ElementType.Type.RESSOURCE_STONE);

                // iron ressources
                element = getRandomValid();
                element.SetElementType(new IronElementType());
                expandRessource(element, ElementType.Type.RESSOURCE_IRON);
            }
            for (int i = 0; i < 4; i++)
            {
                // Lake ressources
                element = getRandomValid();
                element.SetElementType(new LakeElementType());
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
            } while (!map[x, y].IsValid || map[x, y].HasElement);

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
                    elementCheck.SetElementType(ElementType.GetTypeObject(elementType));
                }
            }

            return newElements;
        }
    }
}
