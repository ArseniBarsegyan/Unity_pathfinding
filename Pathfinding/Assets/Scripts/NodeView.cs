using UnityEngine;

public class NodeView : MonoBehaviour
{
    private Node _node;
    
    public GameObject tile;
    public GameObject arrow;

    [Range(0, 0.5f)]
    public float borderSize = 0.15f;

    public void Init(Node node)
    {
        if (tile != null)
        {
            gameObject.name = "Node(" + node.xIndex + ", " + node.yIndex + ")";
            gameObject.transform.position = node.position;
            tile.transform.localScale = new Vector3(1f - borderSize, 0.001f, 1f - borderSize);
            _node = node;
            EnableObject(arrow, false);
        }
    }

    void ColorNode(Color color, GameObject go)
    {
        if (go != null)
        {
            Renderer goRenderer = go.GetComponent<Renderer>();
            if (goRenderer != null)
            {
                goRenderer.material.color = color;
            }
        }
    }

    public void ColorNode(Color color)
    {
        ColorNode(color, tile);
    }

    void EnableObject(GameObject go, bool state)
    {
        if (go != null)
        {
            go.SetActive(state);
        }
    }

    public void ShowArrow(Color color)
    {
        if (_node != null && arrow != null && _node.previous != null)
        {
            EnableObject(arrow, true);

            Vector3 dirToPrev = (_node.previous.position - _node.position).normalized;
            arrow.transform.rotation = Quaternion.LookRotation(dirToPrev);

            Renderer arrowRenderer = arrow.GetComponent<Renderer>();
            if (arrowRenderer != null)
            {
                arrowRenderer.material.color = color;
            }
        }
    }
}
