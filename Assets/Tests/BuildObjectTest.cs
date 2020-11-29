using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

namespace Tests
{
    public class BuildObjectTest
    {
        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator BuildObjectTestWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            SceneManager.LoadScene("Main");
            yield return new WaitForSeconds(2);
            Rubble rubble = GameManager.FindObjectOfType<Rubble>();
            rubble.OnRemove();
            yield return new WaitForSeconds(11);
            Assert.GreaterOrEqual(GameManager.Instance.GetRawMaterials(), 5);

            yield return null;
        }
    }
}
