using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JedenTrzyPiecSiedem;

namespace Tests1357
{
    [TestClass]
    public class EngineTest
    {
        [TestMethod]
        public void TestCheckPairs()
        {
            Assert.IsTrue(Engine.CheckPairs(new List<int> { 1, 1 }));
            Assert.IsTrue(Engine.CheckPairs(new List<int> { 1, 1, 2, 2 }));
            Assert.IsTrue(Engine.CheckPairs(new List<int> { 1, 1, 2, 2, 41, 41 }));
            Assert.IsFalse(Engine.CheckPairs(new List<int> { 1, 1, 2 }));
            Assert.IsFalse(Engine.CheckPairs(new List<int> { 1, 1, 2, 3 }));
        }
        [TestMethod]
        public void TestRemoveFrom()
        {
            CollectionAssert.AreEquivalent(new List<int>(), Engine.RemoveFrom(7, 0, 7));
            CollectionAssert.AreEquivalent(new List<int> { 1 }, Engine.RemoveFrom(7, 1, 6));
            CollectionAssert.AreEquivalent(new List<int> { 5, 1 }, Engine.RemoveFrom(7, 5, 1));
            CollectionAssert.AreEquivalent(new List<int> { 2 }, Engine.RemoveFrom(7, 2, 5));
            CollectionAssert.AreEquivalent(new List<int> { 6 }, Engine.RemoveFrom(7, 6, 1));
            CollectionAssert.AreEquivalent(new List<int> { 2, 3 }, Engine.RemoveFrom(7, 2, 2));
            CollectionAssert.AreEquivalent(new List<int> { 1, 1 }, Engine.RemoveFrom(7, 1, 5));
            CollectionAssert.AreEquivalent(new List<int> { 1, 5 }, Engine.RemoveFrom(7, 1, 1));
            CollectionAssert.AreEquivalent(new List<int> { 1, 2 }, Engine.RemoveFrom(7, 1, 4));
            CollectionAssert.AreEquivalent(new List<int> { 2, 1 }, Engine.RemoveFrom(7, 2, 4));
        }
        [TestMethod]
        public void TestCheck123WithPairs()
        {
            Assert.IsTrue(Engine.Check123WithPairs(new List<int> { 1, 2, 3 }));
            Assert.IsTrue(Engine.Check123WithPairs(new List<int> { 1, 2, 3, 4, 4 }));
            Assert.IsTrue(Engine.Check123WithPairs(new List<int> { 1, 1, 2, 3, 1 }));
        }

        class InfoLineMock : IInfoLine
        {
            public int Col { get; set; }

            public int Row { get; set; }
        }

        class InfoLinesMock : IDataListProvider<InfoLineMock>
        {
            public List<InfoLineMock> Data
            {
                get
                {
                    return new List<InfoLineMock>();
                }
            }
        }

        [TestMethod]
        public void TestRemoving()
        {
            var poss = EngineTemplate<InfoLineMock, InfoLinesMock>.CalcPossibilities(
                new List<InfoLineMock>
                {
                    new InfoLineMock
                    {
                        Col = 0,
                        Row = 0
                    },
                    new InfoLineMock
                    {
                        Col = 0,
                        Row = 1
                    },
                    new InfoLineMock
                    {
                        Col = 1,
                        Row = 1
                    },
                    new InfoLineMock
                    {
                        Col = 2,
                        Row = 1
                    },
                    new InfoLineMock
                    {
                        Col = 0,
                        Row = 2
                    },
                    new InfoLineMock
                    {
                        Col = 1,
                        Row = 2
                    },
                    new InfoLineMock
                    {
                        Col = 2,
                        Row = 2
                    },
                    new InfoLineMock
                    {
                        Col = 3,
                        Row = 2
                    },
                    new InfoLineMock
                    {
                        Col = 4,
                        Row = 2
                    },
                    new InfoLineMock
                    {
                        Col = 0,
                        Row = 3
                    },
                    //new InfoLineMock
                    //{
                    //    Col = 1,
                    //    Row = 3
                    //},
                    //new InfoLineMock
                    //{
                    //    Col = 2,
                    //    Row = 3
                    //},
                    //new InfoLineMock
                    //{
                    //    Col = 3,
                    //    Row = 3
                    //},
                    //new InfoLineMock
                    //{
                    //    Col = 4,
                    //    Row = 3
                    //},
                    new InfoLineMock
                    {
                        Col = 5,
                        Row = 3
                    },
                    new InfoLineMock
                    {
                        Col = 6,
                        Row = 3
                    }
                });
            var res = EngineTemplate<InfoLineMock, InfoLinesMock>.FindWhatToRemoveToWin(poss);
        }
    }
}
