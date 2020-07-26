using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillFireConditionValueType
{
    FloatValue,
    Present,
    CastorHp,
    CastorHpPresent,
    TargetHp,
    TargetHpPresent,
    Timer
}
public enum SkillFireConditionType
{
    Greater,
    GreaterEqual,
    Equal,
    LessEqual,
    Less,
}
public enum SkillFireConditionRelation
{
    None,
    Or,
    And
}

public struct SkillFireConditionConfigDetails
{
    public float castorHealth;
    public float castorHealthPresentValue;
    public float targetHealth;
    public float targetHealthPresentValue;
    public float runTimer;
}

public class SkillFireCondition : MonoBehaviour
{
    public SkillFireConditionValueType firstType;
    float firstValue;
    public SkillFireConditionType condition;
    public SkillFireConditionValueType compareType;
    public float compareValue;
    public SkillFireConditionRelation relationWithNextCondition;

    public bool CalculateConditionResultWithOutIsShouldGoNext(SkillFireConditionConfigDetails _details,out bool _isShouldGoNext)
    {
        bool conditionResult = true;
        //判断条件结果
        conditionResult = CalculateConditionResult(_details);
        //判断是否继续执行
        _isShouldGoNext = CalculateShouldResult(conditionResult);
        return conditionResult;
    }

    private bool CalculateConditionResult(SkillFireConditionConfigDetails _details)
    {
        bool conditionResult = false;
        //拼凑判断条件
        switch (firstType)
        {
            case SkillFireConditionValueType.CastorHpPresent:
                firstValue = _details.castorHealthPresentValue;
                break;
            case SkillFireConditionValueType.TargetHpPresent:
                firstValue = _details.targetHealthPresentValue;
                break;
            case SkillFireConditionValueType.CastorHp:
                firstValue = _details.castorHealth;
                break;
            case SkillFireConditionValueType.TargetHp:
                firstValue = _details.targetHealth;
                break;
            case SkillFireConditionValueType.Timer:
                firstValue = _details.runTimer;
                break;
        }

        switch (compareType)
        {
            case SkillFireConditionValueType.FloatValue:
                break;
            case SkillFireConditionValueType.Present:
                break;
        }

        //判断条件
        switch (condition)
        {
            case SkillFireConditionType.Greater:
                if (firstValue > compareValue)
                {
                    conditionResult = true;
                }
                else
                {
                    conditionResult = false;
                }
                break;
            case SkillFireConditionType.GreaterEqual:
                if (firstValue >= compareValue)
                {
                    conditionResult = true;
                }
                else
                {
                    conditionResult = false;
                }
                break;
            case SkillFireConditionType.Equal:
                if (firstValue == compareValue)
                {
                    conditionResult = true;
                }
                else
                {
                    conditionResult = false;
                }
                break;
            case SkillFireConditionType.LessEqual:
                if (firstValue <= compareValue)
                {
                    conditionResult = true;
                }
                else
                {
                    conditionResult = false;
                }
                break;
            case SkillFireConditionType.Less:
                if (firstValue < compareValue)
                {
                    conditionResult = true;
                }
                else
                {
                    conditionResult = false;
                }
                break;
        }
        return conditionResult;
    }

    private bool CalculateShouldResult(bool _conditionResult)
    {
        bool _isShouldGoNext = false;

        //判断条件与下一个条件的关系
        if (_conditionResult)
        {//条件成立
            switch (relationWithNextCondition)
            {
                case SkillFireConditionRelation.None:
                    _isShouldGoNext = false;
                    break;
                case SkillFireConditionRelation.Or:
                    //如果成立or  不需要执行下一个
                    _isShouldGoNext = false;
                    break;
                case SkillFireConditionRelation.And:
                    //如果成立and  需要执行下一个
                    _isShouldGoNext = true;
                    break;

            }

        }
        else
        {//条件不成立
            switch (relationWithNextCondition)
            {
                case SkillFireConditionRelation.None:
                    _isShouldGoNext = false;
                    break;
                case SkillFireConditionRelation.Or:
                    //如果不成立or 需要执行下一个
                    _isShouldGoNext = true;
                    break;
                case SkillFireConditionRelation.And:
                    //如果不成立and  不需要执行下一个
                    _isShouldGoNext = false;
                    break;

            }
        }
        return _isShouldGoNext;
    }


}
