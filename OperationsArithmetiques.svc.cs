using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace OC_Calculus_WCFService
{
    public class OperationsArithmetiques : IOperationsArithmetiques
    {
        /*
            le cahier des charges ne précise pas comment doivent être traitées les entrées erronnées qui pourraient être faites par l'utilisateur.
            Ex.  OC_Calculus_WCFService.OperationsArithmetiques.Plus("patates", 5);
            La seule ligne de conduite à envisager serait de traiter les incohérences de saisie au niveau de la partie cliente (console, fenêtré ou web) et non au niveau des méthodes de service
             
        */

        

        System.Globalization.NumberFormatInfo provider;  // propriété privée
        CalculsAPIs cAPIs;

        

        OperationsArithmetiques()
        {
            //Gestion du séparateur décimal et des groupes pour la conversion des chaines de caractères
            provider = new System.Globalization.NumberFormatInfo();
            provider.NumberDecimalSeparator = ",";
            provider.NumberGroupSeparator = ".";
            cAPIs = new CalculsAPIs(provider);
            
            
        }

        public double Plus(double nb1, double nb2) //1. Plus
        {
            
            return  nb1 + nb2;   
        }

        public double Moins(double nb1, double nb2) //2. Moins
        {
            return nb1 - nb2;
        }

        public double Diviser(double nb1, double nb2) //3. Diviser
        {
            /*
             le cahier des charges précise que le cas de la division par zéro ne sera pas traitée, cependant j'ai la liberté de le traiter 
             comme donnant 0, au lieu de soulever l'exception dans le cas où l'utilisateur fera une telle division interdite

            */
            return (nb2==0 ? 0 : nb1 / nb2);  
        }

        public double Multiplier(double nb1, double nb2) //4. Multiplier
        {
            return nb1 * nb2;
        }

        public bool EstMultipleDe(int nb1, int nb2) //5. EstMultipleDe
        {
            return nb1 % nb2 == 0 ? true : false;
        }
        
        public int ChaineDAdditionsEntieres(string chaine)
        {
           double result = cAPIs.traiteChaineOperationNP(chaine, '+');
            return Convert.ToInt32(result);

        }
        
        public double ChaineDAdditionsReelles(string chaine)
        {
       

            return cAPIs.traiteChaineOperationNP(chaine, '+');
        }
        public int ChaineDAdditionsEtDeSoustractionsEntieres(string chaine)
        {
            double result = cAPIs.traiteChaineOperationNP(chaine, '+');
            return Convert.ToInt32(result);

        }
        public double ChaineDAdditionsEtDeSoustractionsReelles(string chaine)
        {
            return cAPIs.traiteChaineOperationNP(chaine, '+');
        }
        public int ChaineDAdditionsEtSoustractionsEtMultiplicationsEntieres(string chaine)
        {
            double result = cAPIs.traiteChaineOperationP(chaine);
            return Convert.ToInt32(result);

        }



    }
}
