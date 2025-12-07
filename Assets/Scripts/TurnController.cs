using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnController : MonoBehaviour
{
    [SerializeField] private GameController gameControllerInstance;
    [SerializeField] private BoardManager boardManagerInstance;
    
    

    // Start is called before the first frame update
    void Start()
    {
        if (boardManagerInstance == null)
        {
            boardManagerInstance = UnityEngine.Object.FindFirstObjectByType<BoardManager>();
        }

        if (gameControllerInstance == null)
        {
            gameControllerInstance = UnityEngine.Object.FindFirstObjectByType<GameController>();
        }
    }
    /// <summary>
    /// Compara la apuesta del jugador con el código secreto y devuelve cuántos pines negros y blancos hay.
    /// Negro = color y posición correctos.
    /// Blanco = color correcto, pero posición incorrecta.
    /// </summary>
    public (int blackPins, int whitePins) EvaluarApuesta(int[] apuesta, int[] codigoSecreto)
    {
        int length = apuesta.Length;

        // Contadores del resultado
        int blackPins = 0;
        int whitePins = 0;

        // Arrays auxiliares para marcar qué posiciones ya fueron usadas
        bool[] usadoEnApuesta = new bool[length];
        bool[] usadoEnSecreto = new bool[length];

        // Paso 1: contar coincidencias exactas (negros)
        for (int i = 0; i < length; i++)
        {
            if (apuesta[i] == codigoSecreto[i])
            {
                blackPins++;
                usadoEnApuesta[i] = true;
                usadoEnSecreto[i] = true;
            }
        }

        // Paso 2: buscar coincidencias de color en distinta posición (blancos)
        for (int i = 0; i < length; i++)
        {
            if (usadoEnApuesta[i]) continue; // ya fue contado como negro

            for (int j = 0; j < length; j++)
            {
                if (usadoEnSecreto[j]) continue; // ya fue contado

                if (apuesta[i] == codigoSecreto[j])
                {
                    whitePins++;
                    usadoEnApuesta[i] = true;
                    usadoEnSecreto[j] = true;
                    break; // pasamos al siguiente i
                }
            }
        }

        return (blackPins, whitePins);
    }
    /// <summary>
    /// Procesa la apuesta del jugador y compara con el código secreto.
    /// </summary>
    /// <param name="apuesta">Array de 4 números que representa la apuesta del jugador.</param>
    public (int blackPins, int whitePins) SubmitTurn(int[] apuesta)
    {
        int[] secreto = gameControllerInstance.GetSecretCode();

        return EvaluarApuesta(apuesta, secreto);
    }
	/*public bool IsLastTurnPlayed()
	{
        return currentTurnIndex >= boardManagerInstance.GetMaxTurn();

	}*/

    public void EndCurrentTurnAndActivateNext()
    {
        if (boardManagerInstance.betTurns == null || boardManagerInstance.betTurns.Count == 0) {
            Debug.LogError("boardManagerInstance.betTurns no tiene un valor coherente");
            return;
        }
         // 1. Bloquear turno actual
        boardManagerInstance.SwitchBetTurn(false);
        // 2. Avanzar turno
        gameControllerInstance.currentTurnIndex++;
        // 3. Activar siguiente turno si no hemos llegado al límite
        if (gameControllerInstance.currentTurnIndex < boardManagerInstance.GetMaxTurn())
        {
            boardManagerInstance.SwitchBetTurn(true);
        }
        else
        {
            boardManagerInstance.SecretCodeCoverSwitch();
            gameControllerInstance.EndGameVictory(false);
            

            // Aquí se puede desactivar botón o notificar al GameController
        }
    }


}
