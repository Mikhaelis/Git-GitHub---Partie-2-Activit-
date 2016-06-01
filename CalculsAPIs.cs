using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace OC_Calculus_WCFService
{
    public class CalculsAPIs
    {

        System.Globalization.NumberFormatInfo provider;

        public NumberFormatInfo Provider
        {
            get
            {
                return provider;
            }

            set
            {
                provider = value;
            }
        }

        
        public CalculsAPIs(System.Globalization.NumberFormatInfo prov)
        {
            provider = prov;
        }
        

        // permet de gérer la conversion des types string en réel pour l'ensemble des méthodes
        double convertStringToDouble(string chainDigits)
        {
            return Convert.ToDouble(chainDigits.Equals("") ? "0" : chainDigits, Provider);
        }

        // traitement des chaines d'opération non prioritaire, en commençant par l'addition, puis la soustraction
        public double traiteChaineOperationNP(string chaine, char oper)
        {

            double resultat = 0;
			int count = 0;
            if (chaine.Equals("")) return resultat;  // le cas de la chaine vide est traité d'emblée
            string[] termes = chaine.Split(oper); //laissera le premier terme == "" si oper=='-' (en raison de la possibilité d'avoir un '-' en début de chaine)


            foreach (string terme in termes)
            {
                count++;
				if (termes.Length == 1) // traite le cas de la chaine  == "n" ou "n-m..."  pour oper=='+' , sachant que "n" est un réel de type signé
                {

                    resultat += oper.Equals('+') ? traiteChaineOperationNP(terme, '-') : convertStringToDouble(terme); ;

                }
                else if ((termes.Length == 2 && terme.Equals(""))) //traite le cas de la chaine  == "-n"  ou "n-m..."  pour oper =='-', sachant que "n" est un réel de type signé 
                {

                    resultat -= convertStringToDouble(terme);
                }
                else
                {

                    if (oper.Equals('+'))
                        resultat += traiteChaineOperationNP(terme, '-');
                    else
                    {    //oper.Equals('-')
                        if (count == 0 && termes[0] != "")  // cas du premier élément non négatif  si oper =='-'
                            resultat += convertStringToDouble(terme);
                        else
                            resultat -= convertStringToDouble(terme);

                    }
                }

            }

            return resultat;
        }

		       //traitement des opérations prioritaires 
        public double traiteChaineOperationP(string chaine) //, char operP) introduction possible d'un 2ème paramètre, si on devait traiter le cas de la division 
        {
            double resultat = 0;
            int index = 0;
            //char operP;  '*' ou '/'

            if (!chaine.Contains("*")) // si la chaine ne contient pas ou plus d'opération prioritaire alors on appelle le traitement non prioritaire par défaut
            {
                resultat += traiteChaineOperationNP(chaine, '+');
            }
            else
            {
                //le traitement de la multiplication implique 3 cas, sachant que a, b et c = a*b sont des réels de type signé :
                //1. -a*b  transformation de -a*b en -c dans chaine => appel de traiteChaineOperationNP(chaine,'-')
                //2. -a*-b transformation de -a*-b en +c dans chaine => appel de traiteChaineOperationNP(chaine,'+')
                //3. a*b transformation de a*b en +c dans chaine => appel de traiteChaineOperationNP(chaine,'+')
                index = chaine.IndexOf('*');
                double a, b, c;
                string c_s; //version string de c
                int indexLeft,
                    indexRight;
                a = extractNumberFromOperatorLeft(chaine, index, out indexLeft);
                b = extractNumberFromOperatorRight(chaine, index, out indexRight);
                c = a * b;
                c_s = c.ToString();
                chaine = replaceStrBetweenTwoIndex(chaine, c_s, indexLeft, indexRight); //nouvelle chaine transformée ou non dépendamment de la valeur de tous autres paramètres d'entrée
                resultat = traiteChaineOperationP(chaine);  // on relance récursivement pour transformer "toutes" les opérations prioritaires en non prioritaires

            }

            return resultat;

        }
		
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