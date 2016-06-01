using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace OC_Calculus_WCFService
{
    public partial class CalculsAPIs
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
                        if (resultat == 0 && termes[0] != "")  // cas du premier élément non négatif  si oper =='-'
                            resultat += convertStringToDouble(terme);
                        else
                            resultat -= convertStringToDouble(terme);

                    }
                }

            }

            return resultat;
        }


    }
}