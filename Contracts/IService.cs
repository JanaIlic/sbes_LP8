using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ServiceModel;
using System.Security;

namespace Contracts
{
    [ServiceContract]
    public interface IService
    {

        [OperationContract]
        [FaultContract(typeof(SecurityException))]
        void AddNewPermissions(string rolename, params string[] permissios);

        [OperationContract]
        [FaultContract(typeof(SecurityException))]
        void RemoveSomePermissions(string rolename, params string[] permissions);

        [OperationContract]
        [FaultContract(typeof(SecurityException))]
        void AddNewRole(string rolename);

        [OperationContract]
        [FaultContract(typeof(SecurityException))]
        void RemoveSomeRole(string rolename);

        [OperationContract]
        [FaultContract(typeof(SecurityException))]
        void CreateFile(string filename, string text);

        [OperationContract]
        void ReadFile(string filename);

        [OperationContract]
        void ShowFolderContent(string foldername);

        [OperationContract]
        [FaultContract(typeof(SecurityException))]
        void CreateFolder(string foldername, string parent);

        [OperationContract]
        [FaultContract(typeof(SecurityException))]
        void Delete(string fileorfolder);

        [OperationContract]
        [FaultContract(typeof(SecurityException))]
        void Rename(string fileorfolder, string newname);

        [OperationContract]
        [FaultContract(typeof(SecurityException))]
        void MoveTo(string fileorfolder, string foldername);

    }
}
