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
        Operation RetrieveOperationByOperator(User operatorUser);
        List<Product> RetrieveProductsByOperation(int operationID);

        List<User> RetrieveHelpersByOperation(int operationID);
    }
}
