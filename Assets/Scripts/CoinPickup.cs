using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSFX;
    [SerializeField] int pointsForCoinUp;

    bool wasCollected; // Evita que se recoja la moneda dos veces

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !wasCollected)
        {
            FindObjectOfType<GameSession>().AddToScore(pointsForCoinUp);
            wasCollected = true;

            // Param1: el audio
            // Param2: es un juego2D, el audio debe emitirse desde la cámara posición exacta
            AudioSource.PlayClipAtPoint(coinPickupSFX, Camera.main.transform.position); // tercer parámetro es volumen

            gameObject.SetActive(false); // Evita que la moneda se recoja dos veces (doble capa de seguridad)
            Destroy(gameObject);
        }
    }
}
