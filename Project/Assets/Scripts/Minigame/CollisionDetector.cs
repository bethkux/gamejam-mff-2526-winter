using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngineInternal;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class CollisionDetector : MonoBehaviour
{
    public SpriteRenderer Up;
    public SpriteRenderer Down;
    public GameObject Other;
    [SerializeField] UnityEvent OnSucces;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Cup")
        {
            if (GetComponent<HandMovement>().MovementState == HandMovement.State.FreelyControllable)
            {
                Up.gameObject.SetActive(false);
                Down.gameObject.SetActive(true);
                Other.gameObject.SetActive(true);
                //other.gameObject.
            }
        }
        else if (other.name == "Other")
        {
            OnSucces.Invoke();
            // Scene transition
            SwapManager.Instance.OnMiniGameSucces();
        }
    }
}
