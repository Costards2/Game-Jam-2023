using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class paralax : MonoBehaviour
{
    [SerializeField] private float velocidadeMapa;
    [SerializeField] private Material background;

    // Start is called before the first frame update
 
    private void Awake()
    {
        Vector2 estatico = new Vector2(Time.fixedDeltaTime * 0, 0);
        background.mainTextureOffset = estatico;
    }

    // Update is called once per frame
    void Update()
    {
        moviMapa();
    }

    void moviMapa()
    {
        
        Vector2 rotacao = new Vector2(Time.time * velocidadeMapa, Time.time * velocidadeMapa);
        background.mainTextureOffset = rotacao;



    }


}
