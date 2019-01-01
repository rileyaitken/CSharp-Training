using System;
using System.Collections.Generic;
using System.Text;

namespace EssentialTraining
{
    public class FlowControl
    {
        //public string PrimaryOrSecondary(string colour)
        //{
            
        //}

        public string SecondaryOrPrimary(string colour)
        {
            var result = "";
            switch(colour.ToLower())
            {
                case "red":
                    result = "Primary";
                    break;
                case "blue":
                    result = "Primary";
                    break;
                case "green":
                    result = "Primary";
                    break;
                default:
                    result = "Secondary";
                    break;
            }
            return result;
        }

        public bool IsYourFavouriteColourYellow(string colour)
        {
            return (colour.ToLower() == "yellow");
        }

        public bool IsYourFavouriteColourGreen(string colour)
        {
            return (colour.ToLower() == "green") ? true : false;
        }

        public bool IsYourFavouriteColourRed(string colour)
        {
            if (colour.ToLower() == "red") return true;
            return false;
        }

        // Fully fleshed out if-else blocks
        public bool IsYourFavouriteColourBlue(string colour)
        {
            if (colour.ToLower() == "blue")
            {
                return true;
            } else
            {
                return false;
            }
        }
    }
}
