using UnityEngine;

/// <summary>
/// Representa un pin de color en el tablero. Asigna automáticamente su color
/// en base al nombre del GameObject (por ejemplo: "ColorPin_1" será rojo, etc.).
/// </summary>
public class ColorPin : MonoBehaviour
{
    // Color y colorCode asignado a este pin
    private Color color;
    private int colorCode;
    // Propiedad pública para acceder al color desde otros scripts
    public Color PinColor => color;
    public int ColorCode => colorCode;

    /// <summary>
    /// Se llama automáticamente al iniciar el objeto. Asigna el color correspondiente.
    /// </summary>
    private void Awake()
    {
        AsignarColorPorNombre();
    }

    /// <summary>
    /// Asigna un color al pin en función del número contenido en su nombre.
    /// Espera nombres como "ColorPin_1", "ColorPin_2", etc.
    /// </summary>
    private void AsignarColorPorNombre()
    {
        string nombre = gameObject.name;

        // Verifica que el nombre comience con "ColorPin_"
        if (nombre.StartsWith("ColorPin_"))
        {
            // Extrae el número después del guion bajo
            string numeroStr = nombre.Substring("ColorPin_".Length);

            // Intenta convertir el número a entero
            if (int.TryParse(numeroStr, out int index))
            {
                // Asigna el color correspondiente al índice
                color = ObtenerColorPorIndice(index);
                colorCode = index;
                return;
            }
        }

        // Si el nombre no es válido, muestra advertencia y asigna gris
        Debug.LogWarning($"Nombre de objeto no válido para ColorPin: {gameObject.name}");
        color = Constants.grisInactivo;
    }

    /// <summary>
    /// Devuelve el color correspondiente al índice dado.
    /// </summary>
    /// <param name="index">Número del pin extraído del nombre</param>
    /// <returns>Color asociado al índice</returns>
    private Color ObtenerColorPorIndice(int index)
    {
        return index switch
        {
            1 => Constants.rojo,
            2 => Constants.verde,
            3 => Constants.azul,
            4 => Constants.amarillo,
            5 => Constants.morado,
            6 => Constants.naranja,
            _ => Constants.grisInactivo // Color por defecto si el índice no es válido
        };
    }
}
