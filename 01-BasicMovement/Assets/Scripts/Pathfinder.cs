using System;
using System.Collections.Generic;
using UnityEngine;

/*
    Pathfinder extremamente simples. Não consegue desviar de obstucalos, paredes ou personagems.
    Apenas segue o caminho mais simples.
    Essa classe não é uma MonoBehaviour, ou seja ele não é um script que você coloca num GameObject,
    mas sim uma "ferramenta" para outros scripts.
*/
public class Pathfinder {
    /*
        Como essa funcao nao depende de nenhuma variavel externa, ela pode ser static.
        Retorna null se nao houver um caminho.
        Retorna um array de posicao caso contrario.
    */
    public static Vector2Int[] FindPath(Vector2Int myPos, Vector2Int targetPos){
        int distance = World.GetDistanceTo(myPos, targetPos);
        if (distance == 0) //Se a distancia mais curta é zero, entao nao há necessidade de se mover.
        {
            return null;
        }
        else
        {
            //Um array que tera as posicoes até o destino. Inclui o destino, mas não o inicio.
            // Representa os "passos" a serem dados.
            Vector2Int[] path = new Vector2Int[distance]; 

            int xDistance = Math.Abs(targetPos.x - myPos.x); //Modulo da distancia em x.
            int yDistance = Math.Abs(targetPos.y - myPos.y); //Modulo da distancia em y.
            
            Vector2Int direction = targetPos - myPos; //O vetor de myPos até targetPos.
            direction.Clamp(new Vector2Int(-1,-1), new Vector2Int(1,1)); //Apenas a direcao (sinal) importa, entao direction é limitado de -1 a 1. 

            int pathIndex = 0;
            /*
                Adiciona a path o movimento em x.
                Começa do x = 1 para eliminar a posicao inicial.
                O <= inclui a ultima posicao em x.
            */
            for (int x = 1; x <= xDistance; x++, pathIndex++)
            {
                path[pathIndex] = myPos + new Vector2Int(x, 0) * direction;
            }
            /*
                Adiciona a path o movimento em y.
                Começa do y = 1 para não marcar duas vezes a posiçao de "curva".
                O <= incluia a ultima posicao em y.
            */
            for (int y = 1; y <= yDistance; y++, pathIndex++)
            {
                path[pathIndex] = myPos + new Vector2Int(xDistance, y) * direction;
            }

            return path;
        }
    }
} 