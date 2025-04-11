using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Elements : MonoBehaviour
{
    public List<ElementData> elements;

    private Dictionary<int, ElementData> layerToElement;
    private Dictionary<ElementType, Coroutine> damageCoroutines = new();
    private Dictionary<ElementType, Coroutine> speedCoroutines = new();
    private HashSet<ElementType> activeElements = new();
    private Dictionary<ElementType, int> elementColliderCounts = new();

    private float originalSpeed;

    private PlayerController playerController;
    private PlayerLife life;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        life = GetComponent<PlayerLife>();

        originalSpeed = playerController.speed;

        layerToElement = new Dictionary<int, ElementData>();
        foreach(var element in elements)
        {
            int layer = Mathf.RoundToInt(Mathf.Log(element.layer.value, 2));
            if (!layerToElement.ContainsKey(layer))
                layerToElement[layer] = element;
        }
    }

    private void OnTriggerEnter(Collider hit)
    {
        GameObject obj = hit.gameObject;
        int layer = hit.gameObject.layer;

        if (!layerToElement.TryGetValue(layer, out ElementData element)) return;

        if (!elementColliderCounts.ContainsKey(element.type))
            elementColliderCounts[element.type] = 0;

        elementColliderCounts[element.type]++;

        if (!activeElements.Contains(element.type))
        {
            activeElements.Add(element.type);

            if (element.initialDamage > 0)
            {
                life.TakeDamage(element.initialDamage);
            }

            if (element.continuousDamage > 0)
            {
                Coroutine co = StartCoroutine(ApplyDamageContinous(element));
                damageCoroutines[element.type] = co;
            }

            if (element.speedModifier < 1f)
            {
                if (!speedCoroutines.ContainsKey(element.type) || life.isInvincible)
                {
                    Coroutine co = StartCoroutine(ApplySpeedEffect(element));
                    speedCoroutines[element.type] = co;
                }
            }

            if (element.type == ElementType.Air && element.force > 0)
            {
                Vector3 forceDir = transform.forward;
                GetComponent<CharacterController>().Move(forceDir * element.force * Time.deltaTime);
            }
        }
    }

    private void OnTriggerExit(Collider obj)
    {
        int layer = obj.gameObject.layer;
        if (layerToElement.TryGetValue(layer, out ElementData element))
        {
            if (elementColliderCounts.ContainsKey(element.type))
            {
                elementColliderCounts[element.type]--;

                if (elementColliderCounts[element.type] <= 0)
                {
                    activeElements.Remove(element.type);
                    elementColliderCounts[element.type] = 0;

                    if (damageCoroutines.TryGetValue(element.type, out Coroutine co) && co != null)
                    {
                        StopCoroutine(co);
                        damageCoroutines[element.type] = null;
                    }
                }
            }
        }
    }

    private IEnumerator ApplyDamageContinous(ElementData element)
    {
        while (activeElements.Contains(element.type))
        {
            yield return new WaitForSeconds(element.delay);
            life.TakeDamage(element.continuousDamage);
        }
    }

    private IEnumerator ApplySpeedEffect(ElementData element)
    {
        float modifiedSpeed = originalSpeed * element.speedModifier;
        playerController.speed = modifiedSpeed;

        while (activeElements.Contains(element.type))
        {
            yield return new WaitForSeconds(0.1f);
        }

        playerController.speed = originalSpeed;
        speedCoroutines.Clear();
    }
}
