using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elements : MonoBehaviour
{
    public List<ElementData> elements;

    public Dictionary<int, ElementData> layerToElement;
    public Dictionary<ElementType, Coroutine> damageCoroutine = new();

    private PlayerController playerController;
    private PlayerLife life;

    private bool firstDamage = false;
    private HashSet<GameObject> currentCollision = new();

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        life = GetComponent<PlayerLife>();
        layerToElement = new Dictionary<int, ElementData>();

        foreach(var element in elements)
        {
            int layerNumber = Mathf.RoundToInt(Mathf.Log(element.layer.value, 2));
            if (!layerToElement.ContainsKey(layerNumber))
                layerToElement.Add(layerNumber, element);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit other)
    {
        GameObject obj = other.gameObject;
        int layer = other.gameObject.layer;

        if (layerToElement.TryGetValue(layer, out ElementData element))
        {
            currentCollision.Add(obj);
            ApplyElementEffect(element);
        }
    }

    private void ApplyElementEffect(ElementData element)
    {
        if (life == null) return;

        if (!firstDamage)
        {
            FirstDamage(element.initialDamage);
            firstDamage = true;
        }

        if (element.continuousDamage > 0f && (!damageCoroutine.ContainsKey(element.type) || damageCoroutine[element.type] == null))
        {
            damageCoroutine[element.type] = StartCoroutine(ApplyDamageContinous(element));
        }

        if (element.speedModifier < 1f)
        {
            StartCoroutine(ApplySpeedEffect(element.speedModifier, element.delay));
        }

        if (element.type == ElementType.Air && element.force > 0f)
        {
            Vector3 forceDir = transform.forward;
            GetComponent<CharacterController>().Move(forceDir * element.force * Time.deltaTime);
        }
    }

    private IEnumerator FirstDamage(float damage)
    {
        life.TakeDamage(damage);
        yield return new WaitForSeconds(30f);
    }

    private IEnumerator ApplySpeedEffect(float speedMultiplier, float duration)
    {
        float originalSpeed = playerController.speed;
        playerController.speed *= speedMultiplier;

        yield return new WaitForSeconds(duration);

        playerController.speed = originalSpeed;
    }

    private IEnumerator ApplyDamageContinous(ElementData data)
    {
        while (true)
        {
            yield return new WaitForSeconds(data.delay);
            life.TakeDamage(data.continuousDamage);
        }
    }
}
