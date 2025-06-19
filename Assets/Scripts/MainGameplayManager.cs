using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


public class MainGameplayManager : MonoBehaviour
{
    public static Action<InteractiveShape> shapeChosenAction = default;

    [SerializeField] private List<TaskSlot> taskSlots = default;

    public static Action ResetAllTaskSlotsAction = default;

    private void Awake()
    {
        shapeChosenAction += ShapeChosen;
        ResetAllTaskSlotsAction += ResetAllTaskSlots;
    }

    private void ShapeChosen(InteractiveShape _shape)
    {
        if (taskSlots.Any(_slot => _slot.occupied == false))
        {
            TaskSlot _selectedSlot = taskSlots.First(_slot => _slot.occupied == false);
            StartCoroutine(_shape.MoveShapeToPosition(_selectedSlot.takeSlot(_shape).position));
            CheckCompletion(_shape);

            if (FigureSpawner.CheckShapeActivations.Invoke() == true)
            {
                MenuManager.ActivateWinScreenAction.Invoke();
            }
        }
        
        if(taskSlots.FindAll(_slot => _slot.occupied == true).Count == 7)
        {
            MenuManager.ActivateLostScreenAction.Invoke();
        }
    }

    private void CheckCompletion(InteractiveShape _newShape)
    {
        List<TaskSlot> _competedSlots = taskSlots.FindAll(_slot => CompareTwoShapeData(_slot.occupiedShapeData, _newShape.shapeData));
        if (_competedSlots.Count == 3)
        {
            foreach (TaskSlot _slot in _competedSlots)
            {
                _slot.occupiedByShape.Deactivate();
                _slot.ClearSlot();
            }
        }
    }

    private bool CompareTwoShapeData(FigureInstance _firstShapeData, FigureInstance _secondShapeData)
    {
        if(_firstShapeData == null)
            return false;

        if (_firstShapeData.figureColor == _secondShapeData.figureColor &&
            _firstShapeData.figureBGSprite == _secondShapeData.figureBGSprite &&
            _firstShapeData.figureAnimalSprite == _secondShapeData.figureAnimalSprite)
            {
                return true;
            }
            else
            {
                return false;
            }
    }

    private void ResetAllTaskSlots()
    {
        foreach (TaskSlot _slot in taskSlots)
        {
            _slot.ClearSlot();
        }
    }

    private void OnDestroy()
    {
        shapeChosenAction -= ShapeChosen;
        ResetAllTaskSlotsAction -= ResetAllTaskSlots;
    }
}

[System.Serializable]
public class TaskSlot
{
    public Transform taskSlotPosition;

    public bool occupied = false;

    public FigureInstance occupiedShapeData = null;

    public InteractiveShape occupiedByShape = null;

    public Transform takeSlot(InteractiveShape _inputShapeInstance)
    {
        occupiedByShape = _inputShapeInstance;
        occupiedShapeData = _inputShapeInstance.shapeData;
        occupied = true;
        return taskSlotPosition;
    }

    public void ClearSlot()
    {
        occupiedByShape = null;
        occupiedShapeData = null;
        occupied = false;
    }
}