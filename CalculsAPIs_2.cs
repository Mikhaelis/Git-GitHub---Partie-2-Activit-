using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OC_Calculus_WCFService
{
    public  partial class CalculsAPIs
    {

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


    }
}