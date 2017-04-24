using Microsoft.VisualStudio.TestTools.UnitTesting;
using Archer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archer.Tests
{
    [TestClass()]
    public class HandlerTests
    {
        [TestMethod()]
        public void RemoveInitialFolderNameTest()
        {
            //arrange
            string folderPath = "c:\\archer";
            List<string> expected = new List<string>() {
                "subfolder1\\file1.txt",
                "subfolder1\\file3.txt",
                "subfolder2\\file4.txt"
        };
            AllHandler allHandler = new AllHandler();
            allHandler.fileNamesList = new List<string>() {
                "c:\\archer\\subfolder1\\file1.txt",
                "c:\\archer\\subfolder1\\file3.txt",
                "c:\\archer\\subfolder2\\file4.txt"
        };
            //act
            allHandler.RemoveInitialFolderName(folderPath);

            CollectionAssert.AreEqual(expected, allHandler.fileNamesList);
        }
        
        [TestMethod()]
        public void ReverseStringTest()
        {
            string text = "c:\\archer\\subfolder1\\file1.txt";
            string expected = "txt.1elif\\1redlofbus\\rehcra\\:c";
            AllHandler allHandler = new AllHandler();

            string actual = allHandler.ReverseString(text);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ReversePathTest()
        {
            string text = "\\archer\\subfolder1\\file1.txt";
            string expected = "file1.txt\\subfolder1\\archer\\";
            AllHandler allHandler = new AllHandler();

            string actual = allHandler.ReversePath(text);

            Assert.AreEqual(expected, actual);
        }

    }
}