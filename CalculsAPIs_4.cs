using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OC_Calculus_WCFService
{
    public  partial class CalculsAPIs
    {

        //  traitement de replacement d'une nouvelle sous-chaine par l'encadrement de deux indexes  pointant sur l'ancienne chaine
        string replaceStrBetweenTwoIndex(string chaine,
            string newSubStr, //correspond à la nouvelle valeur à insérer dans la chaine
            int indexR,  //index de gauche  
            int indexL  //index de droite
            )
        {

            string leftPart, rightPart, newchaine;

            leftPart = indexR == 0 ? "" : chaine.Substring(0, indexR);   //si indexR == 0, il n' y aura pas de leftPart ou leftPart=""
            if (!newSubStr.StartsWith("-") && leftPart != "")
            {
                newSubStr = "+" + newSubStr;  //si le nouveau nombre à insérer n'est pas négatif et que leftPart est non vide, alors il faudra rajouter un "+"
            }
            rightPart = indexL == (chaine.Length - 1) ? "" : chaine.Substring(indexL, (chaine.Length - indexL)); //si indexL == (chaine.Length-1) alors on est en bout de chaine, et donc il n'y aura pas de rightPart
            newchaine = leftPart + newSubStr + rightPart;
            return newchaine;
        }
    }
}