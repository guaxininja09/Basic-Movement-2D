using System.Collections.Generic;
using UnityEngine;

/*
	Esse é o script resposavel por um personagem. Mantem o estado do personagem, e tem funcoes para o controlar.
*/
public class GameCharacter : MonoBehaviour
{
	[SerializeField] private World world;
	[SerializeField] private LevelManager levelManager;
	private Vector2Int[] movementPath; //Quando o personagem está se movendo, ele seguira esse caminho.
	private int movementIndex = -1; //Conta os "passos" do personagem, ou seja, o indice atual dele no movementPath;
	/*
		Como um jogo pode ter mais de um personagem, nao todos devem se mover no TurnState.movement
		Essa propriedade analisa movementPath, e movementIndex e diz se o personagem deve se mover.
	*/
	private bool ShouldMove
	{
		get 
		{
			//Quando movementPath é null não há para onde ir, entao o personagem deve ficar parado.
			//Se o movementPath existe, mas movementIndex é igual ao tamanho do movementPath o personagem já terminou de se mexer.
			return movementPath != null && movementIndex != movementPath.Length;
		}
	}
	//Essa propriedade expoe a posicao do transform do personagem.
	public Vector3 WorldPosition
	{
		get
		{
			return this.transform.position;
		}
		set 
		{
			this.transform.position = value;
		}
	}
	//Essa proprieda permite obter a posicao no tilemap de um personagem ou move-lo.
	public Vector2Int CellPosition
	{
		get 
		{
			return world.WorldToCell(WorldPosition);
		}
		set 
		{
			this.transform.position = world.CenterWorldPointOnCell(value);
		}
	}
	/*
		Chamada todo frame pela Unity.
		Se o TurnState for movement, e esse personagem tem um caminho para andar, chama MovementUpdate.
	*/
	private void Update()
	{
		switch (levelManager.CurrentTurnState)
		{
			case LevelManager.TurnState.movement:
				if (ShouldMove) MovementUpdate();
			break;
		}
	}
	/*
		Move o personagem 1 tile.
		Como é chamada todo frame pelo Update, significa que o personagem se move com velocidade dependente do framerate.
		E normalmente se move bem rapido.
	*/
	private void MovementUpdate(){
		//Move o personagem para a tile seguinte no caminho.
		WorldPosition = world.CellToWorldCentered(movementPath[movementIndex]);
		movementIndex++;
		//Se o personagem chegou ao fim do movimento muda o TurnState de volta para decision.
		//Devido a isso só um personagem deve se mover de uma vez.
		if (movementIndex == movementPath.Length)
			levelManager.CurrentTurnState = LevelManager.TurnState.decision;
	}
	/*
		Inicia o movimento do personagem até a posicao destino.
		Apenas um personagem pode se mover em cada TurnState movement.
	*/
    public void MoveTo(Vector2Int targetPos)
	{
		if (targetPos != CellPosition)
		{
			/*
				Usa o pathfinder para achar o caminho. O resultado é um array de posicoes a seguir, indice 0 é o primeiro passo,
				indice 1 o segundo, etc.
			*/
			Vector2Int[] path = Pathfinder.FindPath(CellPosition, targetPos);
			//Se um caminho não for encontrado path sera null.
			if (path != null)
			{
				movementPath = path;
				movementIndex = 0;
				//Imediatamente coloca o jogo no modo de movimento, evitando que mais movimentos sejam iniciados.
				levelManager.CurrentTurnState = LevelManager.TurnState.movement;
			}
		}
	}
}