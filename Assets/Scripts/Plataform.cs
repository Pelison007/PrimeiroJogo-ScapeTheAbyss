using System.Collections.Generic;
using UnityEngine;

public class Plataform : MonoBehaviour
{
    public float MoveSpeed = 2f;
    public bool platform1, platform2; // horizontal ou vertical
    public bool moveRight = true, moveUp = true;
    public float postion_platform1_maior_X, postion_platform1_menor_X;
    public float postion_platform2_maior_Y, postion_platform2_menor_Y;

    void Update()
    {
        if (platform1)
        {
            if (transform.position.x > postion_platform1_maior_X)
            {
                moveRight = false;
            }
            else if (transform.position.x < postion_platform1_menor_X)
            {
                moveRight = true;
            }
            if (moveRight)
            {
                transform.Translate(Vector2.right * MoveSpeed * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector2.right * -MoveSpeed * Time.deltaTime);
            }
        }


        if (platform2)
        {
            if (transform.position.y > postion_platform2_maior_Y)
            {
                moveUp = false;
            }
            else if (transform.position.y < postion_platform2_menor_Y)
            {
                moveUp = true;
            }


            if (moveUp)
            {
                transform.Translate(Vector2.up * MoveSpeed * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector2.up * -MoveSpeed * Time.deltaTime);
            }
        }
    }
}
