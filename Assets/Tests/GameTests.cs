using System.Collections;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

namespace Tests
{
    public class GameTests
    {
        [UnityTest]
        public IEnumerator DoesRubbleGiveRawMaterialsTest()
        {
            SceneManager.LoadScene("Main");
            yield return new WaitForSeconds(2);

            Rubble rubble = Object.FindObjectOfType<Rubble>();
            rubble.OnRemove();
            yield return new WaitForSeconds(11);

            Assert.GreaterOrEqual(GameManager.Instance.GetRawMaterials(), 5);
        }

        [UnityTest]
        public IEnumerator DoesHouseIncreaseHumansTest()
        {
            SceneManager.LoadScene("Main");
            yield return new WaitForSeconds(2);

            // get test class with building prefabs
            var prefabs = Object.FindObjectOfType<TestingPrefabs>();

            // find tile with nothing on it
            var tiles = Object.FindObjectsOfType<BaseTileScript>();
            var freeTile = tiles.First(tile => tile.PlacedObjects.Count == 0);

            // place tree on the free tile
            var newTree = Object.Instantiate(prefabs.Tree, freeTile.transform.position, freeTile.transform.rotation);
            var newTreeObject = newTree.GetComponent<BuildObject>();
            newTreeObject.OnBuild();
            freeTile.PlaceObject(newTree);

            // wait for tree to be build
            yield return new WaitForSeconds(6);

            // wait for nature to spread
            yield return new WaitForSeconds(10);
            
            // get a tile that has nature and is free
            var freeNatureTile = tiles.FirstOrDefault(tile => tile.naturePollutedDegree > 0 && tile.PlacedObjects.Count == 0);

            // if all nearby tiles have rubble we need to clear some space
            if (freeNatureTile == null)
            {
                var tileToClear = tiles.First(tile =>
                    tile.naturePollutedDegree > 0 &&
                    tile.PlacedObjects.First(building => building.GetComponent<Rubble>() != null));

                tileToClear.PlacedObjects[0].GetComponent<Rubble>().OnRemove();

                // wait for rubble to get removed
                yield return new WaitForSeconds(11);

                freeNatureTile = tileToClear;
            }

            // place house on the free tile
            var newHouse = Object.Instantiate(prefabs.House, freeNatureTile.transform.position, freeNatureTile.transform.rotation);
            var newHouseObject = newHouse.GetComponent<BuildObject>();
            newHouseObject.OnBuild();
            freeNatureTile.PlaceObject(newHouse);

            // wait for house to be build
            yield return new WaitForSeconds(6);

            Assert.GreaterOrEqual(GameManager.Instance.GetPopulationAmount(), 5);
        }

        [UnityTest]
        public IEnumerator DoesFactoryConvertRawMaterailsToBuildMaterials()
        {
            SceneManager.LoadScene("Main");
            yield return new WaitForSeconds(2);

            // get test class with building prefabs
            var prefabs = Object.FindObjectOfType<TestingPrefabs>();

            // find tile with nothing on it
            var tiles = Object.FindObjectsOfType<BaseTileScript>();
            var freeTile = tiles.First(tile => tile.PlacedObjects.Count == 0);

            GameManager.Instance.AddRawMaterial(10);

            // place house on the free tile
            var newFactory = Object.Instantiate(prefabs.Factory, freeTile.transform.position, freeTile.transform.rotation);
            var newFactoryObject = newFactory.GetComponent<BuildObject>();
            newFactoryObject.OnBuild();
            freeTile.PlaceObject(newFactory);

            int rawMaterial = GameManager.Instance.GetRawMaterials();
            int buildMaterial = GameManager.Instance.GetBuildingMaterials();

            // wait for house to be build
            yield return new WaitForSeconds(6);

            yield return new WaitForSeconds(6);

            Assert.Less(GameManager.Instance.GetRawMaterials(), rawMaterial);
            Assert.Greater(GameManager.Instance.GetBuildingMaterials(), buildMaterial);

        }
    }
}
