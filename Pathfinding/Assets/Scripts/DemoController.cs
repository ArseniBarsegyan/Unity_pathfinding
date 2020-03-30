using UnityEngine;

public class DemoController : MonoBehaviour
{
    public MapData mapData;
    public Graph graph;

    public Pathfinder Pathfinder;
    public int startX = 0;
    public int startY = 0;
    public int goalX = 8;
    public int goalY = 1;

    public float timeStep = 0.1f;

    void Start()
    {
        if (mapData != null && graph != null)
        {
            int[,] mapInstance = mapData.MakeMap();
            graph.Init(mapInstance);

            var graphView = graph.gameObject.GetComponent<GraphView>();

            if (graphView != null)
            {
                graphView.Init(graph);
            }

            if (graph.IsWithinBounds(startX, startY) && graph.IsWithinBounds(goalX, goalY)
                                                     && Pathfinder != null)
            {
                var startNode = graph.nodes[startX, startY];
                var goalNode = graph.nodes[goalX, goalY];
                Pathfinder.Init(graph, graphView, startNode, goalNode);
                StartCoroutine(Pathfinder.SearchRoutine(timeStep));
            }
        }
    }
}
