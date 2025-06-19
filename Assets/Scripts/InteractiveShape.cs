using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveShape : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteBG;
    [SerializeField] private SpriteRenderer spriteAnimal;

    [SerializeField] private Rigidbody2D rigidbody;

    public FigureInstance shapeData;

    [SerializeField] private float figureToSlotSpeed;

    public void Initialize(Color _initColor, Sprite _initSpriteBG, Sprite _initSpriteAnimal)
    {
        spriteBG.sprite = _initSpriteBG;
        spriteBG.color = _initColor;
        spriteAnimal.sprite = _initSpriteAnimal;

        spriteBG.gameObject.AddComponent<PolygonCollider2D>();

        shapeData = new FigureInstance(_initColor, _initSpriteBG, _initSpriteAnimal);
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        rigidbody.simulated = true;

        spriteBG.sortingLayerName = "figureBG";
        spriteAnimal.sortingLayerName = "figureBG";
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public IEnumerator MoveShapeToPosition(Vector3 targetPosition)
    {
        Vector3 startPosition = transform.position;
        float timeElapsed = 0;

        while (timeElapsed < figureToSlotSpeed)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, timeElapsed / figureToSlotSpeed);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }

    public void ShapeClicked()
    {
        AudioManager.PlayButtonClickAction.Invoke();
        
        MainGameplayManager.shapeChosenAction.Invoke(this);
        transform.rotation = Quaternion.identity;

        rigidbody.simulated = false;

        spriteBG.sortingLayerName = "UI";
        spriteAnimal.sortingLayerName = "UI";

    }
}
