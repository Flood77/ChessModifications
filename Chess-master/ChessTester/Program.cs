using System;
using chess;
using Chess;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChessTester
{
    [TestClass]
    public class ChessTest {
        //test to make sure that king castle error is being stopped on left side
        [TestMethod]
        public void test1()
        {
            ChessMatch cm = null;
            bool correct = false;

            do
            {
                ChessMatch c = new ChessMatch(1);

                if (c.testList[6].Position.Column <= 3)
                {
                    correct = true;
                    cm = c;
                }
            } while (!correct);

            Assert.AreEqual(cm.testList[6].PossibleMoviments()[cm.testList[6].Position.Line, cm.testList[6].Position.Column - 2], false);
        }
        //test for king castle error right
        [TestMethod]
        public void test2()
        {
            ChessMatch cm = null;
            bool correct = false;

            do
            {
                ChessMatch c = new ChessMatch(1);

                if(c.testList[6].Position.Column >= 5)
                {
                    correct = true;
                    cm = c;
                } 
            } while (!correct);

            Assert.AreEqual(cm.testList[6].PossibleMoviments()[cm.testList[6].Position.Line, cm.testList[6].Position.Column + 2], false);
        }
        //test to show that the random values are not hard coded
        [TestMethod]
        public void test3()
        {
            ChessMatch cms = new ChessMatch(1);
            ChessMatch cmc = new ChessMatch(1);

            Assert.AreNotEqual(cms.BoardOfMatch, cmc.BoardOfMatch);
        }
        //test to make sure that the modes arent using the same load method
        [TestMethod]
        public void test4()
        {
            ChessMatch cms = new ChessMatch(0);
            ChessMatch cmc = new ChessMatch(1);

            Assert.AreNotEqual(cms.BoardOfMatch, cmc.BoardOfMatch);
        }
    }
}
