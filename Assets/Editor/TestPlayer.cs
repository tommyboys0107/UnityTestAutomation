using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NSubstitute;
using NUnit.Framework;
using System.Collections;

[TestFixture]
public class TestPlayer{
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

    [SetUp]
    public void SetUp()
    {
        Debug.Log("SetUp");
    }

    [TearDown]
    public void TearDown()
    {
        Debug.Log("TearDown");
    }

    [Test]
    public void PlayerLoseHealthAfterTakingDamage()
    {
        // Arrange
        Player player = new Player();
        player.health = 100;

        // Act
        player.TakeDamage(10);

        // Assert
        Assert.That(player.health, Is.EqualTo(90));
    }

    [Test]
    [TestCase(100, 10, ExpectedResult = 90)]
    [TestCase(100, 110, ExpectedResult = 0)]
    public int PlayerLoseHealthAfterTakingDamageWithTestCase(int health, int damage)
    {
        // Arrange
        Player player = new Player();
        player.health = health;

        // Act
        player.TakeDamage(damage);

        // Assert
        return player.health;
    }

    [Test]
    public void PlayerCanEquipWeaponDummy()
    {
        // Arrange
        Player player = new Player();
        IWeapon weapon = new WeaponDummy();

        // Act
        player.Equip(weapon);

        // Assert
        Assert.IsTrue(player.isWeaponEquipped);
    }

    [Test]
    public void PlayerCanEquipWeaponNSubs()
    {
        // Arrange
        Player player = new Player();
        IWeapon weapon = Substitute.For<IWeapon>();

        // Act
        player.Equip(weapon);

        // Assert
        Assert.IsTrue(player.isWeaponEquipped);
    }

    [Test]
    public void PlayerDamageComputationIsCorrectStub()
    {
        // Arrange
        Player player = new Player();
        IWeapon weapon = new WeaponStub();
        player.Equip(weapon);

        // Act
        float actualResult = player.Damage(2);

        // Assert
        Assert.That(actualResult, Is.EqualTo(4));
    }

    [Test]
	public void PlayerDamageComputationIsCorrectNSubs() {
        // Arrange
        Player player = new Player();
        IWeapon weapon = Substitute.For<IWeapon>();
        weapon.Damage().Returns(2);
        player.Equip(weapon);

        // Act
        float actualResult = player.Damage(2);

        // Assert
        Assert.That(actualResult, Is.EqualTo(4));
    }

    [Test]
    public void PlayerInventoryCanBeAddedSpy()
    {
        // Arrange
        Player player = new Player();
        InventorySpy inventory = new InventorySpy();
        IItem item = Substitute.For<IItem>();
        player.inventory = inventory;

        // Act
        player.AddToInventory(item);
        player.AddToInventory(item);

        // Assert
        Assert.That(inventory.itemCount, Is.EqualTo(2));
    }

    [Test]
    public void PlayerInventoryCanBeAddedNSubs()
    {
        // Arrange
        Player player = new Player();
        IInventory inventory = Substitute.For<IInventory>();
        IItem item = Substitute.For<IItem>();
        int itemCount = 0;
        player.inventory = inventory;
        inventory.Add(Arg.Do<IItem>(x => itemCount++));

        // Act
        player.AddToInventory(item);
        player.AddToInventory(item);

        // Assert
        Assert.That(itemCount, Is.EqualTo(2));
    }

    [Test]
    public void PlayerInventoryCanBeAddedMock()
    {
        // Arrange
        Player player = new Player();
        IInventory inventory = Substitute.For<IInventory>();
        IItem item = Substitute.For<IItem>();
        player.inventory = inventory;

        // Act
        player.AddToInventory(item);
        player.AddToInventory(item);

        // Assert
        inventory.Received(2).Add(item);
    }
}

public class WeaponDummy : IWeapon
{
    public int Damage()
    {
        throw new System.NotImplementedException();
    }
}

public class WeaponStub : IWeapon
{
    public int Damage()
    {
        return 2;
    }
}

public class InventorySpy : IInventory
{
    public int itemCount = 0;

    public void Add(IItem item)
    {
        itemCount++;
    }
}
