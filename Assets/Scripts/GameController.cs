using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controlador principal de la partida. Genera el código secreto, procesa turnos, 
/// evalúa condiciones de victoria o derrota, y comunica los resultados al BoardManager.
/// </summary>
public class GameController : MonoBehaviour
{
	/// <summary>
	/// Código secreto generado al inicio de la partida.
	/// </summary>
	private int[] secretCodeArray;

	/// <summary>
	/// Referencia al gestor del tablero para obtener apuestas y dibujar respuestas.
	/// </summary>
	[SerializeField] private BoardManager boardManagerInstance;

	/// <summary>
	/// Referencia al controlador de turnos que compara la apuesta con el código secreto.
	/// </summary>
	[SerializeField] private TurnController turnControllerInstance;

	/// <summary>
	/// Índice del turno actual.
	/// </summary>
	public int currentTurnIndex = 0;

	/// <summary>
	/// Número máximo de turnos permitidos en la partida.
	/// </summary>
	[SerializeField] private int maxTurns = 9;

	/// <summary>
	/// Se ejecuta al inicio de la partida. Genera el código secreto.
	/// </summary>
	void Start()
	{
		secretCodeArray = CodeGenerator(4, 1, 6);
	}

	/// <summary>
	/// Genera un array de números aleatorios entre minValue y maxValue (ambos inclusivos).
	/// </summary>
	/// <param name="length">Cantidad de elementos a generar</param>
	/// <param name="minValue">Valor mínimo permitido</param>
	/// <param name="maxValue">Valor máximo permitido</param>
	/// <returns>Array con la secuencia generada</returns>
	public int[] CodeGenerator(int length, int minValue, int maxValue)
	{
		int[] array = new int[length];
		for (int i = 0; i < length; i++)
		{
			array[i] = UnityEngine.Random.Range(minValue, maxValue + 1);
		}
		return array;
	}

	/// <summary>
	/// Devuelve el código secreto para que otros componentes puedan acceder a él.
	/// </summary>
	/// <returns>El array con el código secreto</returns>
	public int[] GetSecretCode()
	{
		return secretCodeArray;
	}

	/// <summary>
	/// Llamado cuando el jugador pulsa el botón de jugar turno.
	/// Procesa la apuesta, evalúa la respuesta y avanza al siguiente turno o finaliza el juego.
	/// </summary>
	public void PlayTurnRequest()
	{
		// Obtener la apuesta actual desde el tablero
		int[] apuesta = boardManagerInstance.GetCurrentBetCode();

		// Evaluar la apuesta contra el código secreto
		(int negros, int blancos) = turnControllerInstance.SubmitTurn(apuesta);

		// Mostrar la respuesta en el tablero
		boardManagerInstance.DrawResponse(negros, blancos);

		// Verificar condición de victoria
		if (negros == 4)
		{
            boardManagerInstance.SecretCodeCoverSwitch();
            EndGameVictory(true);
            return;
		}
        // Avanzar turno en el turncontroller
        turnControllerInstance.EndCurrentTurnAndActivateNext();

	}

    /// <summary>
    /// Finaliza la partida, mostrando el resultado final.
    /// </summary>
    /// <param name="victoria">True si se ganó la partida, False si se perdió</param>
    public void EndGameVictory(bool victory)
    {
        boardManagerInstance.ShowEndGameAnimation(victory);
    }




    // Aquí se puede implementar: desactivar botones, mostrar UI final, cargar escena, etc.
    // Ejemplo:
    // UIController.Instance.ShowEndGameScreen(victoria);
    // SceneManager.LoadScene("PantallaFinal");

}
