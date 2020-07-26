using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillFireConditionController : MonoBehaviour
{
    public List<SkillFireCondition> conditionList;
    public string skillResPath;

    public bool ShouldFireSkill(SkillFireConditionConfigDetails detailds,int listIndex)
    {
        bool result = false;
        bool isShouldTurnNext = false;
        if (listIndex <= conditionList.Count -1 && listIndex >= 0)
        {
            result = conditionList[listIndex].CalculateConditionResultWithOutIsShouldGoNext(detailds,out isShouldTurnNext);
            if (isShouldTurnNext)
            {
                //需要去下一个说明到目前都是false 结果由下一个判断条件判断
                result = ShouldFireSkill(detailds, listIndex+1);
                return result;
            }
            else
            {
                //不需要去下一个说明 完成,直接返回结果即可,退出递归
                return result;
            }
        }
        else
        {
            //条件判断完了 返回false 退出递归
            result = false;
            return result;
        }
    }
}
