using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectRayInteraction : MonoBehaviour
{
    public float rayLength;
    public LayerMask targetLayer;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit2D _hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, rayLength, targetLayer);
            if (_hit)
            {
                _hit.collider.transform.parent.gameObject.GetComponent<InteractiveShape>().ShapeClicked();
                //Debug.Log(_hit.collider.gameObject.name);
            }
        }
    }
}
