using UnityEngine;

public class MapController : MonoBehaviour
{
    [SerializeField] private MapPart[] parts;

    private int currentAct;

    public void SetActMap(int actIndex)
    {
        for (int i = 0; i < parts.Length; i++)
        {
            MapPart part = parts[i];

            bool isActiveInThisAct = part.ActsActive.Contains(actIndex);

            part._Object.SetActive(isActiveInThisAct);
        }
    }
}
