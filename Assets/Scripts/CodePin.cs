using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Representa un pin donde el jugador coloca un color como apuesta.
/// Guarda el color visual y su valor num�rico asociado.
/// </summary>
public class CodePin : MonoBehaviour
{
    private SpriteRenderer sr;
    private int colorCode;
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        
    }
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    /// <summary>
    /// Establece el color visual del pin y guarda el c�digo num�rico.
    /// </summary>
    /// <param name="color">Color a mostrar</param>
    /// <param name="code">C�digo del color (1 a 6)</param>
    public void SetColor(Color color, int code)
    {
        sr.color = color;
        colorCode = code;
    }

    /// <summary>
    /// Devuelve el c�digo del color asignado.
    /// </summary>
    public int GetColorCode()
    {
        return colorCode;
    }
}
