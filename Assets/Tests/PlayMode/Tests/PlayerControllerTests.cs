using System.Collections;
using System.Reflection;
using NUnit.Framework;
using Platformer.Mechanics;
using UnityEngine;
using UnityEngine.TestTools;

// Тестовая сборка PlayModeTests (includePlatforms: Editor не указан) — тесты PlayMode
public class PlayerControllerTests
{
    
  
    public IEnumerator PlayMode_ControlDisabled_HorizontalInputIsZero()
    {
       
        GameObject playerObj = new GameObject("TestPlayer");
        playerObj.SetActive(false);

        playerObj.AddComponent<Rigidbody2D>();
        playerObj.AddComponent<BoxCollider2D>();
        playerObj.AddComponent<SpriteRenderer>();
        playerObj.AddComponent<Animator>();
        playerObj.AddComponent<AudioSource>();
        playerObj.AddComponent<Health>();
        PlayerController player = playerObj.AddComponent<PlayerController>();

        player.controlEnabled = false;
        playerObj.SetActive(true);
        yield return null;

        FieldInfo moveField = typeof(PlayerController).GetField(
            "move", BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.IsNotNull(moveField, "Ошибка: в PlayerController не найдено поле 'move'!");

        
        moveField.SetValue(player, new Vector2(1f, 0f));
        player.velocity = new Vector2(5f, 0f);

        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForFixedUpdate();
        }

        Vector2 move = (Vector2)moveField.GetValue(player);
        Assert.AreEqual(0f, move.x, 0.0001f,
            "Ошибка: при controlEnabled = false горизонтальный ввод не обнулился!");
        Assert.AreEqual(0f, player.velocity.x, 0.0001f,
            "Ошибка: при controlEnabled = false персонаж продолжает двигаться по горизонтали!");

        Object.Destroy(playerObj);
        yield return null;
    }
}