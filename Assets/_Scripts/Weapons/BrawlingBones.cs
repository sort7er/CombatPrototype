using UnityEngine;

public class BrawlingBones : MonoBehaviour
{
    private enum Side
    {
        Left, 
        Right 
    };

    [SerializeField] private HandBones handBones;
    [SerializeField] private Transform[] bones;
    [SerializeField] private Side side;

    private Transform[] bonesToTrack;

    private Vector3 offset;

    private void Awake()
    {
        bonesToTrack = new Transform[bones.Length];

        if (side == Side.Left)
        {
            for (int i = 0; i < bonesToTrack.Length; i++)
            {
                bonesToTrack[i] = handBones.leftBones[i];
            }

            transform.parent = handBones.leftParent;
            transform.position = Vector3.zero;
            offset = new Vector3(0, 180, 0);
        }
        else
        {
            for (int i = 0; i < bonesToTrack.Length; i++)
            {
                bonesToTrack[i] = handBones.rightBones[i];
            }

            transform.parent = handBones.rightParent;
            transform.position = Vector3.zero;
            offset = Vector3.zero;
        }
    }

    private void Update()
    {
        for(int i = 0; i < bones.Length; i++)
        {
            bones[i].position = bonesToTrack[i].position;
            bones[i].rotation = bonesToTrack[i].rotation;
            bones[i].Rotate(offset, Space.Self);
        }
    }


}
