using SmartChess.Models.Chess;
using SmartChess.Models.Chess.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartChess.Algorithms
{
    public class PathFinder
    {
        private class PathNode : IComparable<PathNode>
        {
            public Position Position { get; }
            public int Cost { get; } // G-cost
            public int Heuristic { get; } // H-cost
            public PathNode? Previous { get; }

            public int TotalCost => Cost + Heuristic; // F-cost

            public PathNode(Position position, int cost, int heuristic, PathNode? previous)
            {
                Position = position;
                Cost = cost;
                Heuristic = heuristic;
                Previous = previous;
            }

            public int CompareTo(PathNode? other)
            {
                if (other == null) return 1;
                return TotalCost.CompareTo(other.TotalCost);
            }
        }

        public async Task<(List<Position> path, int cost)> FindPathAsync(Position start, Position target, Board board)
        {
            // В данном примере алгоритм Дейкстры адаптирован под A* (с эвристикой).
            // Для чистого Дейкстры Heuristic всегда 0, и TotalCost = Cost.
            // Используем эвристику "манхэттена" для A*.
            int Heuristic(Position a, Position b) => Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);

            var priorityQueue = new PriorityQueue<PathNode, int>();
            var visited = new Dictionary<Position, int>();
            var bestPath = new Dictionary<Position, PathNode>();

            var startNode = new PathNode(start, 0, Heuristic(start, target), null);
            priorityQueue.Enqueue(startNode, startNode.TotalCost);
            bestPath[start] = startNode;

            while (priorityQueue.Count > 0)
            {
                var current = priorityQueue.Dequeue();

                if (visited.ContainsKey(current.Position) && visited[current.Position] <= current.Cost)
                    continue; // Уже посетили с меньшей стоимостью

                visited[current.Position] = current.Cost;

                if (current.Position == target)
                {
                    return (ReconstructPath(current), current.Cost);
                }

                // Получаем возможные "ходы" для цели поиска (не обязательно реальные шахматные)
                // Для демонстрации используем все 8 направлений как у короля, но с весами
                int[] dx = { -1, -1, -1, 0, 0, 1, 1, 1 };
                int[] dy = { -1, 0, 1, -1, 1, -1, 0, 1 };

                for (int i = 0; i < 8; i++)
                {
                    var neighborPos = new Position(current.Position.X + dx[i], current.Position.Y + dy[i]);

                    // --- ИСПРАВЛЕНО: Проверяем границы доски напрямую ---
                    if (IsPositionOnBoard(neighborPos)) // Проверяем границы доски
                    {
                        // Вес перехода - можно сделать сложнее (например, учитывать фигуры)
                        int moveCost = 1;
                        int newCost = current.Cost + moveCost;

                        if (!visited.ContainsKey(neighborPos) || newCost < visited[neighborPos])
                        {
                            var neighborNode = new PathNode(neighborPos, newCost, Heuristic(neighborPos, target), current);
                            priorityQueue.Enqueue(neighborNode, neighborNode.TotalCost);
                            bestPath[neighborPos] = neighborNode;
                        }
                    }
                }
            }

            // Путь не найден
            return (new List<Position>(), -1);
        }

        // --- НОВЫЙ МЕТОД: Проверяет, находится ли позиция на доске ---
        private bool IsPositionOnBoard(Position pos)
        {
            return pos.X >= 0 && pos.X < 8 && pos.Y >= 0 && pos.Y < 8;
        }
        // --- КОНЕЦ НОВОГО МЕТОДА ---

        private List<Position> ReconstructPath(PathNode node)
        {
            var path = new List<Position>();
            var current = node;
            while (current != null)
            {
                path.Add(current.Position);
                current = current.Previous;
            }
            path.Reverse();
            return path;
        }
    }
}