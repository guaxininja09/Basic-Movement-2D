using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

/*
	Esse é o script que a posicao do mouse e mostra o caminho pelo qual o personagem se movera.
	Um clique do mouse inicia o movimento do personagem.
*/
public class Selection : MonoBehaviour
{
	[SerializeField] private World world;
	[SerializeField] private LevelManager levelManager;
	[SerializeField] private Camera mainCamera;
	[SerializeField] private Tile overlayGreen;
	[SerializeField] private GameCharacter playerCharacter;
	private SpriteRenderer mySpriteRenderer;
	/*
		Chamada pela Unity quando o cursor é criado.
	*/
	private void Start ()
	{
		//Pega a refencia ao SpriteRenderer do cursor.
		mySpriteRenderer = GetComponent<SpriteRenderer>();
	}
	/*
		Chama DecisionUpdate ou InactiveUpdate dependo do estado no LevelManager.
		Obs: Update é chamada 1 vez por frame pela Unity.
	*/
	private void Update()
	{
		switch (levelManager.CurrentTurnState)
		{
			case LevelManager.TurnState.decision:
				DecisionUpdate();
			break;
			case LevelManager.TurnState.movement:
				InactiveUpdate();
			break;
		}
	}
	/*
		Chamada quando não há nenhuma ação que o player possa tomar.
		Deixa o cursor invisivel.
	*/
	private void InactiveUpdate()
	{
		if (mySpriteRenderer.enabled) mySpriteRenderer.enabled = false;
	}
	/*
		Chamada quando o player pode tomar uma ação.
	*/
	private void DecisionUpdate()
	{
		//Deixa o cursor visivel.
		if (!mySpriteRenderer.enabled) mySpriteRenderer.enabled = true;

		//Le a posicao do mouse e posiciona o cursor
		Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition); //Posicao nao centralizada na tile.
		Vector2 cursorWorldPos = world.CenterWorldPointOnCell(mouseWorldPos); //Centraliza a posicao na tile.
		this.transform.position = new Vector3(cursorWorldPos.x, cursorWorldPos.y, this.transform.position.z);
		//Seta a posicao no tilemap do cursor (Cell position)
		Vector2Int cursorCellPos = world.WorldToCell(cursorWorldPos);

		//Limpa o mapa de overlays.
		world.ClearOverlayMap();
		//Desenha o caminho até o cursor
		DrawPath(playerCharacter.CellPosition, cursorCellPos);
		
		//Se o botao direito foi apertado nesse frame:
		if (Input.GetMouseButtonDown(0))
		{	
			//Move o personagem. Olhe a classe gamecharacter para detalhes.
			playerCharacter.MoveTo(cursorCellPos);

			world.ClearOverlayMap();
		}
	}
	/*
		Desenha um caminho entre dois pontos usando a tile overlayGreen no mapa de overlays.
	*/
	private void DrawPath(Vector2Int startPos, Vector2Int endPos)
	{
		/*
			Usa o pathfinder para achar o caminho. O resultado é um array de posicoes a seguir, indice 0 é o primeiro passo,
			indice 1 o segundo, etc.
		*/
		Vector2Int[] path = Pathfinder.FindPath(startPos, endPos);
		//Se um caminho não for encontrado path sera null.
		if (path != null){
			for (int i = 0; i < path.Length; i++)
			{
				world.SetOverlayTile(path[i], overlayGreen);
			}
		}
	}
}