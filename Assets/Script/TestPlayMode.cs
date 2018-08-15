using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using NUnit.Framework;
using NSubstitute;
using System.Collections;

public class TestPlayMode {
    Scene testScene;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        Debug.Log("OneTimeSetUp");
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        Debug.Log("OneTimeTearDown");
    }

    // A UnityTest behaves like a coroutine in PlayMode
    // and allows you to yield null to skip a frame in EditMode
    [UnityTest, Order(0)]
    public IEnumerator CubeIsEffectedByGravityOnce()
    {
        // Arrange
        yield return SetUpScene("Test");
        GameObject cube = GameObject.Find("Cube");
        Vector3 originPosition = cube.transform.position;

        // Apply
        yield return new WaitForSeconds(1.0f);

        // Assert
        Assert.AreNotEqual(cube.transform.position, originPosition);
        yield return TearDownScene("Test");
    }

    [UnityTest, Order(1)]
    public IEnumerator CubeIsEffectedByGravity() {
        // Arrange
        yield return SetUpScene("Test");
        GameObject cube = GameObject.Find("Cube");
        Vector3 originPosition = cube.transform.position;

        // Apply
        yield return new WaitForSeconds(1.0f);

        // Assert
        Assert.AreNotEqual(cube.transform.position, originPosition);
    }

    [UnityTest, Order(2)]
    public IEnumerator SphereIsEffectedByGravity()
    {
        // Arrange
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = sphere.transform.position + new Vector3(0.0f, 4.0f, 0.0f);
        Vector3 originPosition = sphere.transform.position;
        sphere.AddComponent<Rigidbody>();

        // Apply
        yield return new WaitForSeconds(1.0f);

        // Assert
        Assert.AreNotEqual(sphere.transform.position, originPosition);
        yield return TearDownScene("Test");
    }

    IEnumerator SetUpScene(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }

    IEnumerator TearDownScene(string sceneName)
    {
        yield return SceneManager.UnloadSceneAsync(sceneName);
    }
}
