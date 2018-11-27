using System.Threading.Tasks;

namespace EHospital.Authorization.Library
{
    public class Authorization
    {
        IDataProvider dataProvider;

        public Authorization(IDataProvider data)
        {
            dataProvider = data;
        }

        public Task<string> GetRoleByToken(string token)
        {
           return this.dataProvider.GetRoleByToken(token);
        }

        public string GetToken() => dataProvider.Token;
    }
}
