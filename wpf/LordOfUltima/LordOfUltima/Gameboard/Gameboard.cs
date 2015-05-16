using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Windows;
using System.Windows.Controls;
using LordOfUltima.MGameboard;

namespace LordOfUltima
{
    class Gameboard
    {
        private static Gameboard m_ins = null;

        // Dimentions pour la fenetre du jeu
        private double width = 830;
        private double start_width = 55;
        private double height = 560;
        private double start_height = 55; //70
        
        // Information sur les divisions dans le jeu
        private double frame_width;
        private double frame_height;
        private int frame_count = 19;

        private Element[,] m_map;

        /*
         * Constructeur prive pour empecher l'acces au constructeur par
         * une autre classe.
        */
        private Gameboard()
        {
            frame_width = (width - start_width) / frame_count;
            frame_height = (height - start_height) / frame_count;

            // Initialisation de toutes les cases du jeu
            m_map = new Element[frame_count, frame_count];
            for (int i = 0; i < frame_count; i++)
            {
                for (int j = 0; j < frame_count; j++)
                {
                    m_map[i, j] = new Element();

                    //set x and Y
                    m_map[i, j].PositionX = i;
                    m_map[i, j].PositionY = j;
                    Rectangle element_rect = m_map[i, j].getElement();

                    // Position for building element
                    element_rect.Margin = new Thickness(i*frame_width + start_width, j*frame_height + (start_height-frame_height), 0, 0);

                    // Position for building level
                    Rectangle level_rect = m_map[i, j].getLevelElement();
                    level_rect.Margin = new Thickness(element_rect.Margin.Left + frame_width / 3, element_rect.Margin.Top + frame_height, 0, 0);

                    // Position for label level
                    double left = level_rect.Margin.Left;
                    double top = level_rect.Margin.Top;
                    m_map[i, j].getLevelLabel().Margin = new Thickness(left-3.1, top-7, 0, 0);

                    // Position for select element
                    Border select_rect = m_map[i, j].getSelectElement();
                    select_rect.Margin = new Thickness(i * frame_width + start_width, j * frame_height + (start_height - frame_height)+5, 0, 0);
                }
            }
            verifyMap();
        }

        /* Implementation du patron singleton
         * retourne l'instance de la classe
        */
        public static Gameboard getInstance()
        {
            if( m_ins == null)
            {
                m_ins = new Gameboard();
            }
            return m_ins;
        }

        public Element[,] getMap()
        {
            return m_map;
        }

        /*
         * Retourne la frame en X ou Y choisie par le curseur
        */
        public int getXFrame(double x)
        {
            int frame = (int)((x-start_width)/frame_width);
            return frame;
        }
        public int getYFrame(double y)
        {
            int frame = (int)((y - start_height)/frame_height);
            return frame;
        }

        /*
         * Verify each element to see if they are valid
        */
        private void verifyMap()
        {
            // coin haut gauche
            m_map[0, 0].setInvalid(); m_map[0, 1].setInvalid(); m_map[1, 0].setInvalid();
            // coin bas gauche
            m_map[0, 18].setInvalid(); m_map[0, 17].setInvalid(); m_map[1, 18].setInvalid();
            // coin haut droit
            m_map[18, 0].setInvalid(); m_map[17, 0].setInvalid(); m_map[18, 1].setInvalid();
            // coin bas droit
            m_map[18, 18].setInvalid(); m_map[18, 17].setInvalid(); m_map[17, 18].setInvalid();

            for(int i = 0; i < 5; i++)
            {
                m_map[i, 9].setInvalid(); m_map[18 - i, 9].setInvalid();
                m_map[9, i].setInvalid(); m_map[9, 18 - i].setInvalid();
            }

            for(int i = 0; i < 7; i++)
            {
                m_map[6 + i, 4].setInvalid(); m_map[6 + i, 14].setInvalid();
                m_map[4, 6 + i].setInvalid(); m_map[14, 6 + i].setInvalid();
            }

            m_map[5, 5].setInvalid(); m_map[6, 5].setInvalid(); m_map[5, 6].setInvalid();
            m_map[5, 13].setInvalid(); m_map[5, 12].setInvalid(); m_map[6, 13].setInvalid();

            m_map[13, 5].setInvalid(); m_map[12, 5].setInvalid(); m_map[13, 6].setInvalid();
            m_map[13, 13].setInvalid(); m_map[13, 12].setInvalid(); m_map[12, 13].setInvalid();

            // Default img for townhall
            m_map[9, 9].setPath("Media/building/building_townhall.png");
        }

        public void hideLevelIndicator()
        {
            foreach( Element element in m_map)
            {
                    element.hideLevelIndicator();
            }
        }

        public void showLevelIndicator()
        {
            foreach (Element element in m_map)
            {
                element.showLevelIndicator();
            }
        }

        public void resetSelectionBorder()
        {
            foreach(Element element in m_map)
            {
                element.hideSelectBorder();
            }
        }

        public void initialiseNewGame()
        {
            Element element = null;
            for(int i = 0; i < 6; i++)
            {
                // wood ressources
                element = getRandomValid();
                element.setElementType(new ForestElementType());
                expandRessource(element, ElementType.type.RESSOURCE_FOREST);

                // stone ressources
                element = getRandomValid();
                element.setElementType(new StoneElementType());
                expandRessource(element, ElementType.type.RESSOURCE_STONE);

                // iron ressources
                element = getRandomValid();
                element.setElementType(new IronElementType());
                expandRessource(element, ElementType.type.RESSOURCE_IRON);
            }
        }

        private Element getRandomValid()
        {
            int x = 0, y = 0;
            do
            {
                x = Random.Next(0, frame_count);
                y = Random.Next(0, frame_count);
            } while (!m_map[x, y].getInvalid() || m_map[x, y].HasElement);

            return m_map[x, y];
        }

        private void expandRessource(Element element, ElementType.type elementType)
        {
            List<Element> firstLayer = new List<Element>();

            firstLayer = firstLayer.Concat(rotateElement(element,elementType,2)).ToList();

            foreach (var secondElement in firstLayer)
            {
                rotateElement(secondElement, elementType, 3);
            }
        }

        private static readonly Random Random = new Random(1232);
        private List<Element> rotateElement(Element element, ElementType.type elementType, int chance)
        {
            List<Element> newElements = new List<Element>();
            List<Element> testList = new List<Element>();

            if (element.PositionX > 0)
            {
                if (Random.Next(chance) == chance)
                {
                    testList.Add(m_map[element.PositionX -1, element.PositionY]);
                }
            }
            if (element.PositionX < frame_count-1)
            {
                if (Random.Next(chance) == chance-1)
                {
                    testList.Add(m_map[element.PositionX + 1, element.PositionY]);
                }
            }
            if (element.PositionY > 0)
            {
                if (Random.Next(chance) == chance-1)
                {
                    testList.Add(m_map[element.PositionX, element.PositionY - 1]);
                } 
            }
            if (element.PositionY < frame_count-1)
            {
                if (Random.Next(chance) == chance-1)
                {
                    testList.Add(m_map[element.PositionX, element.PositionY + 1]);
                }
            }

            foreach (var elementCheck in testList)
            {
                if (!elementCheck.HasElement)
                {
                    newElements.Add(elementCheck);
                    elementCheck.setElementType(ElementType.getTypeObject(elementType));
                } 
            }

            return newElements;
        }


    }
}
