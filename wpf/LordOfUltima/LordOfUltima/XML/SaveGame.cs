using System;
using System.IO;
using System.Xml;
using LordOfUltima.MGameboard;
using LordOfUltima.Error;

namespace LordOfUltima.XML
{
    class SaveGame
    {
        private static SaveGame _instance;
        public static SaveGame Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new SaveGame();
                }
                return _instance;
            }
        }

        private User.User _user;
        private Gameboard _gameboard;
        private SaveGame() 
        {
            _user = User.User.getInstance();
            _gameboard = Gameboard.getInstance();
        }

        public void Save()
        {
            // visual reset
            MainWindow.m_ins.setVisibleBuildingMenu(false);
            MainWindow.m_ins.setVisibleBuildingDetails(false);
            _gameboard.resetSelectionBorder();

            // No username set
            if (_user.Name == "")
                return;

            Element[,] elements = _gameboard.getMap();
            XmlWriter xmlWriter = XmlWriter.Create( _user.Name + ".xml");

            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("save");

            for (int i = 0; i < _gameboard.CountXY; i++ )
            {
                for (int j = 0; j < _gameboard.CountXY; j++ )
                {
                    Element currentElement = elements[i, j];
                    xmlWriter.WriteStartElement("Element");
                    xmlWriter.WriteAttributeString("X", i.ToString());
                    xmlWriter.WriteAttributeString("Y", j.ToString());
                    
                    // Information on the element

                    // Type
                    IElementType elementType = elements[i,j].GetElementType();
                    string elementName = (elementType != null )? elementType.GetElementType().ToString() : "null";
                    xmlWriter.WriteAttributeString("elementType", elementName);

                    // Level
                    xmlWriter.WriteAttributeString("level", currentElement.Level.ToString());

                    xmlWriter.WriteEndElement();
                }
            }
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
        }

        public bool Load()
        {
            // visual reset
            MainWindow.m_ins.setVisibleBuildingMenu(false);
            MainWindow.m_ins.setVisibleBuildingDetails(false);
            _gameboard.resetSelectionBorder();

            if(!File.Exists(_user.Name + ".xml"))
                return false;

            Element[,] elements = _gameboard.getMap();

            try
            {
                // Create an XML reader for this file.
	            using (XmlReader reader = XmlReader.Create(_user.Name + ".xml"))
	            {
	                while (reader.Read())
	                {
		                // Only detect start elements.
		                if (reader.IsStartElement())
		                {
                            switch(reader.Name)
                            {
                                case "save":
                                    break;
                                case "Element":
                                    int x = Convert.ToInt32(reader["X"]);
                                    int y = Convert.ToInt32(reader["Y"]);
                                    if(x >= 0 && x < _gameboard.CountXY && y >= 0 && y < _gameboard.CountXY)
                                    {
                                        Element element = elements[x, y];
                                        string elementType = reader["elementType"];
                                        int level = Convert.ToInt32(reader["level"]);

                                        if(level >=0 && level <= 10)
                                        {
                                            // build the building
                                            element.Level = level;

                                            if(elementType != "null")
                                            {
                                                IElementType type = ElementType.getElementFromType(elementType);
                                                if(type != null)
                                                {
                                                    element.setElementType(type);

                                                    if (type.GetElementType() == ElementType.type.RESSOURCE_FIELDS)
                                                    {
                                                        element.HasElement = false;
                                                    }
                                                }
                                            }
                                            
                                        }
                                        else { throw new LoadException("Invalid level range : " + level); }
                                    }
                                    else { throw new LoadException("Invalid X and Y range. X = " + x + " Y = " + y); }
                                    break;

                                default:
                                    break;
                            }
		                }
	                }
	            }
                // set the neighbouring ressources count
                _gameboard.cheakAllNeighbourRessources();
                return true;
            }
            catch(Exception)
            {
                // reset map if there was an exception
                _gameboard.resetMap();
                _gameboard.initialiseNewGame();
                return false;
            }

        }
    }
}
