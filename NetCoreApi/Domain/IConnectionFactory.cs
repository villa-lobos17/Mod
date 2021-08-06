using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Domain
{
   public  interface IConnectionFactory
    {
        IDbConnection GetConnection { get; }
        void CloseConnection();
    }
}
