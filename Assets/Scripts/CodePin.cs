using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Representa un pin donde el jugador coloca un color como apuesta.
/// Guarda el color visual y su valor numérico asociado.
/// </summary>
public class CodePin : MonoBehaviour
{
    private SpriteRenderer sr;
    private int colorCode;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    /// <summary>
    /// Establece el color visual del pin y guarda el código numérico.
    /// </summary>
    /// <param name="color">Color a mostrar</param>
    /// <param name="code">Código del color (1 a 6)</param>
    public void SetColor(Color color, int code)
    {
        sr.color = color;
        colorCode = code;
    }

    /// <summary>
    /// Devuelve el código del color asignado.
    /// </summary>
    public int GetColorCode()
    {
        return colorCode;
    }
}
