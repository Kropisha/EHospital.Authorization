using Microsoft.VisualStudio.TestTools.UnitTesting;
using EHospital.Authorization.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using EHospital.Authorization.Model;

namespace EHospital.Authorization.Tests
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