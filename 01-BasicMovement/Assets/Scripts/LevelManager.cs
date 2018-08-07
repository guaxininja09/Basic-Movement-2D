using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	O LevelManager guarda o estado do jogo assim os outros scripts sabem o que devem fazer.
*/
public class LevelManager : MonoBehaviour
{
	public enum TurnState
	{
		decision, //O jogo espera um player (ou AI) decidir para onde mover.
		movement  //O jogo espera um personagem se mover.
	}
	public TurnState CurrentTurnState { get; set; } 
	/*
		Chamada pela Unity quando o LevelManager é criado.
	*/
	private void Start()
	{
		CurrentTurnState = TurnState.decision;
	}
}
