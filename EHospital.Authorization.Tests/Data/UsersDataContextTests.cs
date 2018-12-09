using EHospital.Authorization.Data.Data;
using EHospital.Authorization.Model.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EHospital.Authorization.Tests.Data
{
    [TestClass()]
    public class UsersDataContextTests
    {
        private static IDataProvider data;

        //[TestInitialize]
        //public static void TestInitialize(IDataProvider dataProvider)
        //{
        //    data = dataProvider;
        //   // var dataProvide = new Mock<IDataProvider>();
        //  //  dataProvide.Setup(q => q.Logins.Add(It.IsAny<Logins>())).Returns(new List<Logins> {});

        //}

        [TestMethod()]
        public async void AddLogin_CorrectValuesTest()
        {
            //arrange
            Logins expected = new Logins { Id = 7, Login = "test@gmail.com", RoleId = 1 };

            //act
            await data.AddLogin(expected);
            int actual = await data.FindByLogin("test@gmail.com");

            //assert
            Assert.AreEqual(expected.Id, actual);
        }
    }
}