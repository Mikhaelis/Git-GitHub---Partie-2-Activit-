using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace OC_Calculus_WCFService
{
    [ServiceContract]
    public interface IOperationsArithmetiques
    {
        [OperationContract]
        double Plus(double nb1, double nb2);

        [OperationContract]
        double Moins(double nb1, double nb2);

        [OperationContract]
        double Diviser(double nb1, double nb2);

        [OperationContract]
        double Multiplier(double nb1, double nb2);

        [OperationContract]
        bool EstMultipleDe(int nb1, int nb2);
        
        [OperationContract]
        int ChaineDAdditionsEntieres(string chaine);
        
        [OperationContract]
        double ChaineDAdditionsReelles(string chaine);

        [OperationContract]
        int ChaineDAdditionsEtDeSoustractionsEntieres(string chaine);

        [OperationContract]
        double ChaineDAdditionsEtDeSoustractionsReelles(string chaine);

        [OperationContract]
        int ChaineDAdditionsEtSoustractionsEtMultiplicationsEntieres(string chaine);
        
    }
    /*
        [DataContract]
        public class StringOperation
        {

        }
    */
}
