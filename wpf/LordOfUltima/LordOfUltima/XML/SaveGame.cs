using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using LordOfUltima.MGameboard;
using LordOfUltima.Error;
using LordOfUltima.Events;
using LordOfUltima.RessourcesProduction;

namespace LordOfUltima.XML
{
    class SaveGame
    {
        private static SaveGame _instance;
        public static SaveGame Instance
        {
            get { return _instance ?? (_instance = new SaveGame()); }
        }

        private readonly User.User _user;
        private readonly Gameboard _gameboard;
        private SaveGame() 
        {
            _user = User.User.Instance;
            _gameboard = Gameboard.Instance;
        }

        public void Save()
        {
            // visual reset
            BuildingMenuVisibility.Instance.HideBuildingMenu();
            BuildingDetailsVisibility.Instance.HideBuildingDetails();
            ResetMapElementBorder.Instance.ResetSelectionBorder();

            // No username set
            if (_user.Name == "")
                return;

            Element[,] elements = _gameboard.GetMap();
            XmlWriter xmlWriter = XmlWriter.Create( _user.Name + ".xml");

            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("save");

            for (int i = 0; i < _gameboard.GetFrameCount; i++)
            {
                for (int j = 0; j < _gameboard.GetFrameCount; j++)
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

            // Save ressources
            
            Ressources ressources = Ressources.Instance;
            double wood = Math.Round(ressources.WoodQty);
            double stone = Math.Round(ressources.StoneQty);
            double iron = Math.Round(ressources.IronQty);
            double food = Math.Round(ressources.FoodQty);
            xmlWriter.WriteStartElement("Ressources");
            xmlWriter.WriteAttributeString("Wood", wood.ToString());
            xmlWriter.WriteAttributeString("Stone", stone.ToString());
            xmlWriter.WriteAttributeString("Iron", iron.ToString());
            xmlWriter.WriteAttributeString("Food", food.ToString());

            MD5 md5Hash = MD5.Create();
            double total = (wood*3 + stone/2 + iron*21 + food + 32) * Math.PI;
            string hash = GetMd5Hash(md5Hash, _user.Name + Math.Round(total));

            xmlWriter.WriteStartElement("Token");
            xmlWriter.WriteAttributeString("Value", hash);


            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
        }

        public bool Load()
        {
            // visual reset
            BuildingMenuVisibility.Instance.HideBuildingMenu();
            BuildingDetailsVisibility.Instance.HideBuildingDetails();
            ResetMapElementBorder.Instance.ResetSelectionBorder();

            if(!File.Exists(_user.Name + ".xml"))
                return false;

            Element[,] elements = _gameboard.GetMap();

            int wood = 0, stone = 0, iron = 0, food = 0;
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
                                    if (x >= 0 && x < _gameboard.GetFrameCount && y >= 0 && y < _gameboard.GetFrameCount)
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
                                                IElementType type = ElementType.GetElementFromType(elementType);
                                                if(type != null)
                                                {
                                                    element.SetElementType(type);

                                                    if (type.GetElementType() == ElementType.Type.RESSOURCE_FIELDS)
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

                                case "Ressources":
                                    Ressources ressources = Ressources.Instance;

                                    wood = Convert.ToInt32(reader["Wood"]);
                                    ressources.WoodQty = wood;
                                    stone = Convert.ToInt32(reader["Stone"]);
                                    ressources.StoneQty = stone;
                                    iron = Convert.ToInt32(reader["Iron"]);
                                    ressources.IronQty = iron;
                                    food = Convert.ToInt32(reader["Food"]);
                                    ressources.FoodQty = food;

                                    break;

                                case "Token":
                                    MD5 md5Hash = MD5.Create();
                                    double total = (wood*3 + stone/2 + iron*21 + food + 32) * Math.PI;
                                    string hash = GetMd5Hash(md5Hash, _user.Name + Math.Round(total));

                                    string token = reader["Value"];

                                    if (hash != token)
                                    {
                                        throw new LoadException("Wrong token mate");
                                    }
                                    break;
                            }
		                }
	                }
	            }
                // set the neighbouring ressources count
                RessourcesBuildingCheck.Instance.cheakAllNeighbourRessources();

                // fix for leftover level indicator
                LevelIndicatorVisibility.Instance.HideLevelIndicator();
                LevelIndicatorVisibility.Instance.HandleLevelIndicatorVisibility();

                return true;
            }
            catch(Exception)
            {
                // reset map if there was an exception
                ResetMapElements.Instance.ResetMap();
                ResetMap.Instance.InitialiseNewGame();

                // TODO : Show error
                return false;
            }

        }

        static string GetMd5Hash(HashAlgorithm md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
    }
}
