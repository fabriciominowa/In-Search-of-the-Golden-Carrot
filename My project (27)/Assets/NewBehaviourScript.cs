using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    void Update()
    {
        transform.Translate(Vector2.left * Config.velocidade * Time.deltaTime);

        if (transform.position.x < Config.limiteX)
        {
            transform.position = new Vector2(Config.retornarX, transform.position.y);
        }

    }

}
