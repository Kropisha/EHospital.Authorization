using System;
using EHospital.Authorization.BusinessLogic.Credentials;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EHospital.Authorization.Tests.Credentials
{
    [TestClass()]
    public class BusinessLogicTests
    {
        [TestMethod]
        public void ValidatePassword_CorrectValuesTest()
        {
            //arrange
            string password = "PLgh4-7";
            bool expected = true;

            //act
            bool actual = PasswordManager.ValidatePassword(password);

            //assert
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ValidatePassword_InCorrectValuesTest()
        {
            //arrange
            string password = "password";

            //act
            bool actual = PasswordManager.ValidatePassword(password);
        }

        [TestMethod]
        public void HashAndVerifyPassword_CorrectValuesTest()
        {
            //arrange
            string password = "PLgh4-7";
            bool expected = true;

            //act
            string hash = PasswordManager.HashPassword(password);
            bool actual = PasswordManager.VerifyHashedPassword(hash, password);

            //assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SetCredentials_CorrectValues()
        {
            //arrange
            CredentialsViewModel credentials = new CredentialsViewModel();
            string login = "test@mail.ru";
            string password = "PLgh4-7";

            //act
            credentials.UserLogin = login;
            credentials.Password = password;

            //assert
            Assert.AreEqual(login,credentials.UserLogin);
            Assert.AreEqual(password,credentials.Password);
        }       
    }
}