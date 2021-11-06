using System.Collections.Generic;
using UnityEngine;

public class GhostMove : MonoBehaviour
{
    public Transform destIndicator;
    public LayerMask nodeWallLayerMask;

    public Transform testDestination;
    public float speed = 2f;

    public float screenXMax, screenXMin, screenYMax, screenYMin;

    float journeyTime = 1f;
    Transform nextNode;

    Vector3 moveVec;

    List<Transform> allNodes = new List<Transform>();

    Transform destination;

    int curNodeNum = 0;

    void Start()
    {
        GameObject[] gameObjs = GameObject.FindGameObjectsWithTag("Node");
        foreach (GameObject gameObj in gameObjs)
        {
            allNodes.Add(gameObj.transform);
        }
        SetRandomDestination();

        // destination = testDestination;
        // destIndicator.position = testDestination.position;

        transform.position = allNodes[0].position;
        nextNode = transform;
    }

    void SetRandomDestination()
    {
        int randomIdx;
        do
        {
            randomIdx = UnityEngine.Random.Range(0, allNodes.Count - 1);
        } while (Vector3.Distance(allNodes[randomIdx].position, transform.position) <= 0.01f);
        destination = allNodes[randomIdx];
        destIndicator.position = destination.position;
    }

    void UpdateMoveDir()
    {
        moveVec = (nextNode.position - transform.position) / journeyTime * speed;
    }

    struct NodeRes
    {
        public bool found;
        public RaycastHit2D node;
    }

    NodeRes GetNodeInDirection(Vector3 dir)
    {
        NodeRes res = new NodeRes();
        RaycastHit2D[] hits = Physics2D.CircleCastAll(nextNode.position, 0.1f, dir, Mathf.Infinity, nodeWallLayerMask);

        if (hits.Length > 1)
        {
            res.found = true;
            res.node = hits[1];
        }
        else
        {
            res.found = false;
        }

        return res;
    }

    int getMinIdx(List<float> list)
    {
        int minIdx = 0;

        for (int i = 1; i < list.Count; i++)
        {
            if (list[i] < list[minIdx])
            {
                minIdx = i;
            }
        }

        return minIdx;
    }

    void SetNextNode()
    {

        NodeRes nodeAboveRes = GetNodeInDirection(Vector3.up);
        NodeRes nodeLeftRes = GetNodeInDirection(Vector3.left);
        NodeRes nodeBelowRes = GetNodeInDirection(Vector3.down);
        NodeRes nodeRightRes = GetNodeInDirection(Vector3.right);

        List<Transform> adjacentTransforms = new List<Transform>();

        if (nodeAboveRes.found && nodeAboveRes.node.transform.tag == "Node" && moveVec.normalized != Vector3.down)
        {
            adjacentTransforms.Add(nodeAboveRes.node.transform);
        }
        if (nodeLeftRes.found && nodeLeftRes.node.transform.tag == "Node" && moveVec.normalized != Vector3.right)
        {
            adjacentTransforms.Add(nodeLeftRes.node.transform);
        }
        if (nodeBelowRes.found && nodeBelowRes.node.transform.tag == "Node" && moveVec.normalized != Vector3.up)
        {
            adjacentTransforms.Add(nodeBelowRes.node.transform);
        }
        if (nodeRightRes.found && nodeRightRes.node.transform.tag == "Node" && moveVec.normalized != Vector3.left)
        {
            adjacentTransforms.Add(nodeRightRes.node.transform);
        }

        List<float> distances = new List<float>();
        foreach (Transform t in adjacentTransforms)
        {
            float dist = Vector3.Distance(t.position, destination.position);
            distances.Add(dist);
        }

        int minIdx = getMinIdx(distances);
        nextNode = adjacentTransforms[minIdx];

    }

    void Update()
    {

        if (transform.position.x < screenXMin || transform.position.x > screenXMax || transform.position.y > screenYMax || transform.position.y < screenYMin)
        {
            Time.timeScale = 0;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale = 1;
        }

        float distanceThres = 0.1f;


        if (Vector3.Distance(transform.position, destination.position) <= distanceThres)
        {
            transform.position = destination.position;
            SetRandomDestination();
            // Debug.Log("new destination is: " + destination.name);
            return;
        }

        float nodeDist = Vector3.Distance(transform.position, nextNode.position);
        if (nodeDist <= distanceThres)
        {
            transform.position = nextNode.position;
            // startTime = Time.time;
            SetNextNode();
            UpdateMoveDir();
            curNodeNum += 1;
            // Debug.Log("reached node, new node is: " + nextNode.name);
        }
        transform.position += moveVec * Time.deltaTime;
    }
}
