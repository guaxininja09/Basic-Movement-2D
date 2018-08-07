using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;

/*
	Esse script é responsavel pelos tilemaps de background e overlay, e tambem pela grid.
	Junta funcoes relacionadas a conversoes de posicoes, calculo de distancia (sem colisoes), ou alteracoes nos tilemaps.
*/
public class World : MonoBehaviour
{
	[SerializeField] private Tilemap backgroundMap; //Mapa de fundo, onde fica o chao.
	[SerializeField] private Tilemap overlayMap; //Mapa de overlays, elementos que cobrem o background map temporariamente.
	private Grid worldGrid; //Todo tilemap esta numa grid, que controla o posicionamento das tiles.
	/*
		Chamada pela Unity quando World é criado
	*/
	private void Start()
	{
		//Pega referencia do Grid.
		worldGrid = GetComponent<Grid>();
	}
	/*
		Seta uma tile no mapa de overlay.
		Usada por funcoes como Selection.DrawPath.
	*/
	public void SetOverlayTile(Vector2Int cellPosition, Tile tile)
	{
		overlayMap.SetTile(new Vector3Int(cellPosition.x, cellPosition.y, 0), tile);
	}
	/*
		Limpa o mapa de overlay, normalmente chamada antes de funcoes que como Selection.DrawPath.
	*/
	public void ClearOverlayMap()
	{
		overlayMap.ClearAllTiles();
	}
	/*
		Retorna a posicao world do centro da tile.
	*/
	public Vector2 CellToWorldCentered(Vector2Int cellPosition)
	{
		return CellToWorld(cellPosition) + new Vector2(backgroundMap.tileAnchor.x, backgroundMap.tileAnchor.y);
	}
	/*
		Retorna a posicao world da tile.
	*/
	public Vector2 CellToWorld(Vector2Int cellPosition)
	{
		Vector3 worldPosition = worldGrid.CellToWorld(new Vector3Int(cellPosition.x, cellPosition.y, 0));
		return new Vector2(worldPosition.x, worldPosition.y);
	}
	/*
		Retorna a posicao no tilemap mais proxima de uma world position.
	*/
	public Vector2Int WorldToCell(Vector3 worldPosition)
	{
		Vector3Int cellPosition = worldGrid.WorldToCell(worldPosition);
		return new Vector2Int(cellPosition.x, cellPosition.y);
	}
	/*
		Retorna uma posicao world centralizada na tile mais proxima.
	*/
	public Vector2 CenterWorldPointOnCell(Vector2 worldPosition)
	{
		Vector2Int cellPos = WorldToCell(worldPosition);
		return CellToWorldCentered(cellPos);
	}
	/*
		Calcula a distancia mais curta, ignorando paredes, entre dois pontos.
	*/
	public static int GetDistanceTo(Vector2Int initialPos, Vector2Int destination)
	{
		return Math.Abs(initialPos.x - destination.x) + Math.Abs(initialPos.y - destination.y);
	}
}
