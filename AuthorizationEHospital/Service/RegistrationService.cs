using eHospital.Authorization.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eHospital.Authorization.Service
{
    public class RegistrationService
    {
        IDataProvider _dataProvider;

        public RegistrationService(IDataProvider data)
        {
            _dataProvider = data;
        }

        public void Do(UsersData user)
        {
            _dataProvider.AddUserData(user);
        }
    }
}
