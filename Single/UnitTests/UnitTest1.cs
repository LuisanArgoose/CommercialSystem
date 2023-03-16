using Microsoft.VisualStudio.TestTools.UnitTesting;
using Single.Model;
using System;
using WSLib.Core;
using WSLib.Model;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void UserAdd_SuccessfullAdd()
        {
            bool res = MainModel.GetDataBase().AddUser("NewUser");
            Assert.AreEqual(true, res);
        }
        [TestMethod]
        public void UserAdd_UnsuccessfullAdd()
        {
            bool res = MainModel.GetDataBase().AddUser("Admin");
            Assert.AreEqual(false, res);
        }
        [TestMethod]
        public void UserDelete_SuccessfullDelete()
        {
            bool res = MainModel.GetDataBase().DeleteUser("NewUser");
            Assert.AreEqual(true, res);
        }
        [TestMethod]
        public void UserDelete_UnsuccessfullDelete()
        {
            bool res = MainModel.GetDataBase().DeleteUser("Admin");
            Assert.AreEqual(false, res);
        }
        [TestMethod]
        public void RoleAdd_SuccessfullAdd()
        {
            bool res = MainModel.GetDataBase().AddRole("NewRole");
            Assert.AreEqual(true, res);
        }
        [TestMethod]
        public void RoleAdd_UnsuccessfullAdd()
        {
            bool res = MainModel.GetDataBase().AddRole("Admin");
            Assert.AreEqual(false, res);
        }
        [TestMethod]
        public void RoleDelete_SuccessfullDelete()
        {
            bool res = MainModel.GetDataBase().DeleteRole("NewRole");
            Assert.AreEqual(true, res);
        }
        [TestMethod]
        public void RoleDelete_UnsuccessfullDelete()
        {
            bool res = MainModel.GetDataBase().DeleteRole("Admin");
            Assert.AreEqual(false, res);
        }
        [TestMethod]
        public void ClientAdd_SuccessfullAdd()
        {
            bool res = MainModel.GetDataBase().AddClient("NewClient");
            Assert.AreEqual(true, res);
        }
        [TestMethod]
        public void ClientAdd_UnsuccessfullAdd()
        {
            bool res = MainModel.GetDataBase().AddClient("Лузан Роман Денисович");
            Assert.AreEqual(false, res);
        }

    }
}
