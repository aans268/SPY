using UnityEngine.EventSystems;
using UnityEngine;
using FYFY;

public class TooltipContent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string text;
    private Tooltip tooltip = null;

    private bool isOver = false;

    private void Start()
    {
        GameObject tooltipGO = GameObject.Find("TooltipUI_Pointer");
        if (!tooltipGO)
        {
            GameObjectManager.unbind(gameObject);
            GameObject.Destroy(this);
        }
        else
            tooltip = tooltipGO.GetComponent<Tooltip>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        string formatedContent = text;
        if (text.Contains("#agentName"))
            formatedContent = text.Replace("#agentName", GetComponent<AgentEdit>().associatedScriptName);

        if (formatedContent.Contains("#equation"))
        {
            Transform doorTransform = transform.parent.Find("Door");
            if (doorTransform != null)
            {
                RonDoorSlot1 doorComponent = doorTransform.GetComponentInChildren<RonDoorSlot1>();
                if (doorComponent != null)
                {
                    //formatedContent = formatedContent.Replace("#equation", doorComponent.equation);
                }
                else
                {
                    Debug.LogWarning("RonDoorSlot1 is missing on this GameObject!");
                }
            }
            else
            {
                Debug.LogWarning("Door GameObject is missing on this GameObject!");
            }
        }

        if (formatedContent.Contains("#operator"))
        {
            Transform doorTransform = transform.parent.Find("RonDoor");
            if (doorTransform != null)
            {
                RonDoorSlot2 doorComponent = doorTransform.GetComponentInChildren<RonDoorSlot2>();
                if (doorComponent != null)
                {
                    //formatedContent = formatedContent.Replace("#operator", doorComponent.operator_sign.ToString());
                }
                else
                {
                    Debug.LogWarning("RonDoorSlot2 is missing on this GameObject!");
                }
            }
            else
            {
                Debug.LogWarning("Door GameObject is missing on this GameObject!");
            }
        }

        if (formatedContent.Contains("#result"))
        {
            Transform doorTransform = transform.parent.Find("Door");
            if (doorTransform != null)
            {
                RonDoorSlot3 doorComponent = doorTransform.GetComponentInChildren<RonDoorSlot3>();
                if (doorComponent != null)
                {
                    //formatedContent = formatedContent.Replace("#result", doorComponent.result.ToString());
                }
                else
                {
                    Debug.LogWarning("RonDoorSlot3 is missing on this GameObject!");
                }
            }
            else
            {
                Debug.LogWarning("Door GameObject is missing on this GameObject!");
            }
        }

        
        tooltip.ShowTooltip(formatedContent);
        isOver = true;


    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltip != null)
        {
            tooltip.HideTooltip();
            isOver = false;
        }
    }

    public void OnDisable()
    {
        if (isOver)
        {
            tooltip.HideTooltip();
            isOver = false;
        }
    }
}
