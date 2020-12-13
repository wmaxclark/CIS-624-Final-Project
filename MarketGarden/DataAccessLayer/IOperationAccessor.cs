using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface IOperationAccessor
    {
        int CreateOperation(int userID_operator, int zipCode, string operationName);
        Operation RetrieveOperationByOperator(User operatorUser);
        List<Product> RetrieveProductsByOperation(int operationID);

        List<User> RetrieveHelpersByOperation(int operationID);
        List<UserTask> RetrieveTasksBySender(User operationID);
        List<string> RetrieveRolesByEmail(string email);
    }
}
