﻿using System;
using System.Threading.Tasks;
using BlockAppsSDK;
using BlockAppsSDK.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlockAppsSDKTest.Contracts
{
    [TestClass]
    public class BoundContractTest
    {
        public BoundContractManager BoundContractManager { get; set; }

        [TestMethod]
        public async Task CanCreateBoundContract()
        {
            var src =
                "contract SimpleStorage { uint storedData; function set(uint x) { storedData = x; } function get() returns (uint retVal) { return storedData; } }";
            var SimpleStorage = await BoundContractManager.CreateBoundContract(src, "SimpleStorage",
                "219e43441e184f16fb0386afd3aed1e780632042");

            Assert.IsNotNull(SimpleStorage);
            Assert.AreEqual(SimpleStorage.Properties["storedData"], "0");
        }

        [TestMethod]
        public async Task CanGetAllBoundContractsWithName()
        {
            var boundContracts = await BoundContractManager.GetBoundContractsWithName("SimpleStorage");
            var nullContracts = await BoundContractManager.GetBoundContractsWithName("NotADeployedContract");
            Assert.IsNotNull(boundContracts);
            Assert.IsTrue(boundContracts.Count > 0);
            Assert.IsNull(nullContracts);
        }

        [TestMethod]
        public async Task CanGetABoundContractWithAddress()
        {
            var contractAddresses = await BoundContractManager.GetContractAddresses("SimpleStorage");
            Assert.IsTrue(contractAddresses.Length > 0);
            var boundSimpleStorage = await BoundContractManager.GetBoundContract("SimpleStorage",
                contractAddresses[0]);

            Assert.IsNotNull(boundSimpleStorage);
            Assert.IsTrue(boundSimpleStorage.Properties["storedData"].Equals("0") || boundSimpleStorage.Properties["storedData"].Equals("1"));
        }


        [TestInitialize]
        public void SetupManagers()
        {
            BoundContractManager = new BoundContractManager(new Connection("http://40.118.255.235:8000",
                "http://40.118.255.235/eth/v1.2"))
            {
                Username = "charlie",
                Password = "test"
            };

        }
    }
}
