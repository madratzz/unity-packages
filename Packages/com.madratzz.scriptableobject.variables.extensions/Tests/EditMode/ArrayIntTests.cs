using NUnit.Framework;
using ProjectCore.Variables;
using UnityEngine;

namespace Madratzz.Tests.VariablesExtensions
{
    public class ArrayIntTests
    {
        private ArrayInt _array;

        [SetUp]
        public void SetUp()
        {
            _array = ScriptableObject.CreateInstance<ArrayInt>();
            _array.Clear();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_array);
        }

        [Test]
        public void Add_NewItem_ReturnsTrueAndGrows()
        {
            Assert.IsTrue(_array.Add(5));
            Assert.AreEqual(1, _array.Count);
            Assert.AreEqual(5, _array[0]);
        }

        [Test]
        public void Add_Duplicate_ReturnsFalseAndDoesNotGrow()
        {
            _array.Add(5);

            Assert.IsFalse(_array.Add(5));
            Assert.AreEqual(1, _array.Count);
        }

        [Test]
        public void Remove_ExistingItem_ReturnsTrue()
        {
            _array.Add(7);

            Assert.IsTrue(_array.Remove(7));
            Assert.AreEqual(0, _array.Count);
        }

        [Test]
        public void Remove_MissingItem_ReturnsFalse()
        {
            Assert.IsFalse(_array.Remove(404));
        }

        [Test]
        public void InsertAtIndex_PlacesItemAtPosition()
        {
            _array.Add(1);
            _array.Add(3);

            _array.InsertAtIndex(1, 2);

            Assert.AreEqual(2, _array[1]);
            Assert.AreEqual(3, _array.Count);
        }

        [Test]
        public void Clear_EmptiesList()
        {
            _array.Add(1);
            _array.Add(2);

            _array.Clear();

            Assert.AreEqual(0, _array.Count);
        }
    }
}
