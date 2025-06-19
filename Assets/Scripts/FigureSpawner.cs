using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Random=UnityEngine.Random;

public class FigureSpawner : MonoBehaviour
{
    [SerializeField] private List<Color> colorList = default;
    [SerializeField] private List<Sprite> spriteBGList = default;
    [SerializeField] private List<Sprite> spriteAnimalList = default;
    [SerializeField] private List<FigureInstance> figuresList = default;
    [SerializeField] private List<InteractiveShape> spawnedShapesList = default;

    [SerializeField] private GameObject shapePrefab;

    [SerializeField] private List<Transform> spawnPoints;

    public static Action ResetShapesAction = default;

    public static Func<bool> CheckShapeActivations = default;

    private void Awake()
    {
        MakeListOfFigures();
        ResetShapesAction += ResetAllShapes;
        CheckShapeActivations += IsEveryShapeDeactivated;
    }

    public void MakeListOfFigures()
    {
        figuresList.Clear();

        foreach (Color _color in colorList)
        {
            foreach (Sprite _spriteBG in spriteBGList)
            {
                foreach (Sprite _spriteAnimal in spriteAnimalList)
                {
                    figuresList.AddRange(Enumerable.Repeat(new FigureInstance(_color, _spriteBG, _spriteAnimal), 3));
                }
            }
        }

        SpawnFigures();
    }

    public void SpawnFigures()
    {
        foreach (FigureInstance _figure in figuresList)
        {
            InteractiveShape _spawnedShape = Instantiate(shapePrefab, transform.position, Quaternion.identity).GetComponent<InteractiveShape>();
            _spawnedShape.Initialize(_figure.figureColor, _figure.figureBGSprite, _figure.figureAnimalSprite);
            spawnedShapesList.Add(_spawnedShape);
        }
    }

    private void ResetAllShapes()
    {
        StopAllCoroutines();
        DisableShapes();

        StartCoroutine(DropShapes());
    }

    private void DisableShapes()
    {
        spawnedShapesList = shuffleGOList(spawnedShapesList);

        foreach (InteractiveShape _shape in spawnedShapesList)
        {
            _shape.Deactivate();
        }
    }

    private IEnumerator DropShapes()
    {
        int _currentSpawnIndex = 0;

        foreach (InteractiveShape _shape in spawnedShapesList)
        {
            _shape.gameObject.transform.position = spawnPoints[_currentSpawnIndex == 2 ? _currentSpawnIndex = 0 : ++_currentSpawnIndex].position;
            _shape.Activate();
            yield return new WaitForSeconds(.1f);
        }
    }

    private bool IsEveryShapeDeactivated()
    {
        if (spawnedShapesList.FindAll(_shape => _shape.gameObject.activeSelf == false).Count == spawnedShapesList.Count)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private List<InteractiveShape> shuffleGOList(List<InteractiveShape> _inputList)
    {
        int _currIndex = 0;
        int _listCount = _inputList.Count;
        int _rand = 0;
        InteractiveShape _shape = null;

        List<InteractiveShape> _tempList = new List<InteractiveShape>();
        _tempList.AddRange(_inputList);

        while (_currIndex < _listCount)
        {
            _rand = Random.Range(_currIndex, _tempList.Count);
            _shape = _tempList[_currIndex];
            _tempList[_currIndex] = _tempList[_rand];
            _tempList[_rand] = _shape;
            _currIndex++;
        }

        return _tempList;
    }

    private void OnDestroy()
    {
        ResetShapesAction -= ResetAllShapes;
        CheckShapeActivations = IsEveryShapeDeactivated;
    }

}

    [System.Serializable]
    public class FigureInstance
    {
        public Color figureColor;

        public Sprite figureBGSprite;

        public Sprite figureAnimalSprite;


        public FigureInstance(Color external_figureColor, Sprite external_figureBGSprite, Sprite external_figureAnimalSprite)
        {
            this.figureColor = external_figureColor;
            this.figureBGSprite = external_figureBGSprite;
            this.figureAnimalSprite = external_figureAnimalSprite;
        }
    }
