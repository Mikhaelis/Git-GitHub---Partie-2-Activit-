using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OC_Calculus_WCFService
{
    public  partial class CalculsAPIs
    {

        //extraction du facteur à gauche 
        double extractNumberFromOperatorLeft(string chaine, int indexOper,
            out int indexEnd //récupération de l'index de fin encadrant l'extraction. On notera qu'elle point sur "premier caractère" du facteur (ce qui comprend son signe)
            )
        {
            indexEnd = indexOper; //indexOper sera toujours différent de 0, puisse un '*' ne pourra jamais être en début de ligne
            bool loop = true;
            int testNumber;
            string eachCarac = "";
            string LeftNumber = "";
            Stack<string> numbers = new Stack<string>(); //on aura besoin d'une collection de type LIFO pour remettre la séquence de chiffres à l'endroit, sachant que ceux-ci seront inversés 
                                                         //lors de la recherche vers la gauche

            do
            {
                eachCarac = chaine.Substring(--indexEnd, 1);
                loop = //toutes les conditions donnant un résultat booléen pour que la boucle puisse continuer ou non
                    int.TryParse(eachCarac, out testNumber) || eachCarac == "." || eachCarac == "," || eachCarac == "-" || eachCarac == "+";
                if (!loop) break;
                numbers.Push(eachCarac.Equals('+') ? "" : eachCarac);  //on ne mémorise pas l'éventuel signe '+'
                if (eachCarac == "-" || eachCarac == "+") break;

            } while (indexEnd > 0);

            foreach (string c in numbers) LeftNumber += c; //Dépile pour restaurer l'ordre d'origine


            return convertStringToDouble(LeftNumber);

        }

        //extraction du facteur à droite
        double extractNumberFromOperatorRight(string chaine, int indexOper,
            out int indexEnd //récupération de l'index de fin encadrant l'extraction. On notera qu'au  terme de l'extraction, elle pointe en fin de caractère du facteur de droite si on est parvenu
                             //en bout de chaine, sinon sur le début du prochain terme ou facteur (ce qui comprend son signe)
            )
        {
            indexEnd = indexOper; //indexOper sera toujours différent de 0, puisse un '*' ne pourra jamais être en début de ligne
            bool loop = true;
            int testNumber;
            string rightNumber = "";
            string eachCarac = "";

            do
            {
                eachCarac = chaine.Substring(++indexEnd, 1);
                loop = int.TryParse(eachCarac, out testNumber) || eachCarac == "." || eachCarac == "," || eachCarac == "-"; //toutes les conditions donnant un résultat booléen pour que la boucle puisse continuer ou non

                if (!loop ||
                (indexOper + 1) != indexEnd && eachCarac == "-"  //on prend en compte le première signe '-' éventuellement contigu à droite de l'opérateur '*', comme par exemple "12.1*-121,123.21"
                )
                    break;

                rightNumber += eachCarac;


            } while (indexEnd < (chaine.Length - 1));

            return convertStringToDouble(rightNumber);

        }

    }
}