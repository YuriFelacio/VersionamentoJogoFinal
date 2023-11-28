using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
    SpriteRenderer sprite;
    public AudioSource shootFx;
    public GameObject projectile; 
    public Transform gunPoint;
    public static int ShotgunlDamage = 3; //dano causado  quando a "shotgunBullet" acertar os inimigos
    public static float ShotgunfireRate = 0.6f; //variavel acessivel de outros scripts que muda a cadencia de disparo da arma
    public static float ShotgunbulletRange = 0.15f; //variavel acessivel de outros scripts que muda o range da arma
    private float canFire = -1f; //variavel usada no comando de fire rate
    private int bulletCount = 5;
    float spread = 15f;

    
    // Start is called before the first frame update


    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        

    }

    // Update is called once per frame
    void Update()
    {
        Aim(); //função para fazer a arma apontar para o mouse
        tiro(); //função que faz o mouse atirar
    }

    void tiro()
    {   
        if (Input.GetKey(KeyCode.Mouse0)&&(Time.time > canFire)) //usa o botão m1 para atirar e verifica o se o player pode atirar
        {
            canFire = Time.time + ShotgunfireRate; //funciona como millis do arduino
            quaternion newRot = gunPoint.rotation;
            
            for (int i = 0; i < bulletCount; i++)
            {
                float addedOffset = ((bulletCount / 2) - i * spread);
                
                newRot = Quaternion.Euler(gunPoint.eulerAngles.x,gunPoint.eulerAngles.y,gunPoint.eulerAngles.z + addedOffset);
                Instantiate(projectile,gunPoint.position,newRot);
            }
            shootFx.Play(); //toca o som de tiro
        }
    }

    void Aim()
    {
        Vector3 mousePos = Input.mousePosition; //pega a posiçao do maouse
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(transform.position); //pega a posição global da camera
        Vector2 offset = new Vector2 (mousePos.x - screenPoint.x, mousePos.y - screenPoint.y); //calcula a diferença entre o mouse e a camera

        float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg; //transforma o offset em um par de ordenadas
        transform.rotation = Quaternion.Euler(0,0,angle);   //setar o valor de rotação da arma

        sprite.flipY = (mousePos.x < screenPoint.x); //caso a arma esteja a esquerda do personagem ela não fica de cabeça pra baixo
    }
}