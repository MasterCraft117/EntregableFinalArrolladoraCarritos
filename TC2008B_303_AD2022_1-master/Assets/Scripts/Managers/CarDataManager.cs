using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDataManager : MonoBehaviour
{

    [SerializeField]
    private Carro[] _listaDeCarros;

    [SerializeField]

    private CarroSO[] _carritosScriptableObjects;

    //Referencias a objetos luces de semaforos
    [SerializeField]
    private GameObject[] _listaSemaforos;

    

    private GameObject[] _carrosGO;
    private GameObject[] _carrosGOS;
    //private ScriptableObject[] _carrosGOS;

    //Declaracion de Semaforos
    //private GameObject[] _semaforos;

    private Vector3[] _direcciones;


    void Start() 
    {
        _carrosGO = new GameObject[_listaDeCarros.Length];
        _carrosGOS = new GameObject[_carritosScriptableObjects.Length];
        //_carrosGOS = new ScriptableObject[_carritosScriptableObjects.Length];
        //_semaforos = new GameObject[_listaSemaforos.Length];

        // activarlos por primera vez
        for (int i = 0; i < _listaDeCarros.Length; i++)
        {
            //Desactivados porque ya no se utilizan
            //_carrosGO[i] = CarPoolManager.Instance.Activar(Vector3.zero);
            // actualizar scriptable object de carrito
        }

        // Activacion para scriptable
        for (int i = 0; i < _carritosScriptableObjects.Length; i++)
        {
            //_carrosGOS[i] = CarPoolManager.Instance.Activar(Vector3.zero);
            //_carrosGOS[i] = PosicionarCarrosS();
            // actualizar scriptable object de carrito

            _carrosGOS[i] = Instantiate(_carritosScriptableObjects[i].prefabDeModelo, Vector3.zero, Quaternion.identity) as GameObject;

            //_carrosGOS[i] = _carritosScriptableObjects[i];

            //_carrosGOS[i].SetActive(true);
            //_carrosGOS[i].transform.position = Vector3.zero;
        }

        //PosicionarCarros();
        PosicionarCarrosS();

        //ajuste a la escala de los autos
        for (int i = 0; i < _carritosScriptableObjects.Length; i++)
        {
            _carrosGOS[i].transform.localScale = new Vector3(
                _carrosGOS[i].transform.localScale.x * 4,
                _carrosGOS[i].transform.localScale.y * 4,
                _carrosGOS[i].transform.localScale.z * 4
                );
        }
    }

    private void PosicionarCarros() {
        // activar los 10 carritos en las posiciones congruentes
        for(int i = 0; i < _listaDeCarros.Length; i++) 
        {
            _carrosGO[i].transform.position = new Vector3(
                _listaDeCarros[i].x,
                _listaDeCarros[i].y,
                _listaDeCarros[i].z
            );
        }
    }

    private void PosicionarCarrosS() {
        // activar los 10 carritos en las posiciones congruentes
        for (int i = 0; i < _carritosScriptableObjects.Length; i++) 
        {
            /*
            _carrosGOS[i].transform.position = new Vector3(
               Random.Range(0f, 10f),
               Random.Range(0f, 10f),
               Random.Range(0f, 10f)
            );
            */
            

            _carrosGOS[i].transform.position = new Vector3(
                _listaDeCarros[i].x,
                _listaDeCarros[i].y,
                _listaDeCarros[i].z
                
                
            );
            //Debug.Log(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*
        // MUY IMPORTANTE
        // CENTRALIZAR ENTRADA
        if(Input.GetKeyDown(KeyCode.R)){

            // recalcular posiciones
            // En teoria debido a la corrutina esto ya no es necesario
            for(int i = 0; i < _listaDeCarros.Length; i++) 
            {
                _listaDeCarros[i] = new Carro();
                _listaDeCarros[i].id = 0;
                _listaDeCarros[i].x = Random.Range(0f, 10f);
                _listaDeCarros[i].y = Random.Range(0f, 10f);
                _listaDeCarros[i].z = Random.Range(0f, 10f);
            }

            // posicionar carritos en nuevo lugar
            PosicionarCarros();
        }

        //ScriptableObjects
        if (Input.GetKeyDown(KeyCode.T))
        {
            // posicionar carritos en nuevo lugar
            //Debug.Log(_carritosScriptableObjects.Length);
            PosicionarCarrosS();
        }
        */
        if(_direcciones != null)
        {
            for (int i = 0; i < _carrosGOS.Length; i++)
            {
                //Vuelve a su orientacion original para aplicar correctamente el translate
                //_carrosGOS[i].transform.forward = Vector3.forward;

                //desplazar utilizando vectores
                //_carrosGOS[i].transform.Translate(_direcciones[i] * Time.deltaTime);

                //modificar orientaci�n de veh�culo
                //_carrosGOS[i].transform.forward = _direcciones[i].normalized;
            }
        }

        
        for (int i = 0; i < _carrosGOS.Length; i++)
        {
            _carrosGOS[i].transform.rotation = Quaternion.Euler(0, _listaDeCarros[i].dir, 0);
        }
    }

    public void EscucharSinArgumentos() {

        print("EVENTO LANZADO SIN ARGUMENTOS");
    }

    public void EscucharConArgumentos(ListaCarro datos) {

        print("RECIBIDO: " + datos);

        // actualizar _listaDeCarros
        // invocar PosicionarCarros()

        _direcciones = new Vector3[datos.frames[0].cars.Length];
        for(int i = 0; i < _direcciones.Length; i++)
        {
            _direcciones[i] = new Vector3();
        }

        StartCoroutine(ActualizarPosicionesConDatos(datos));
    }

    IEnumerator ActualizarPosicionesConDatos(ListaCarro datos)
    {
        for(int i = 0; i < datos.frames.Length; i++)
        {
            

            //Actualizacion semaforos
            for (int j = 0; j < _listaSemaforos.Length; j++)
            {
                if (datos.frames[i].semaphores[j].state == 0)
                {
                    //_listaSemaforos[0].SetActive(false);
                    _listaSemaforos[j].GetComponent<Light>().color = Color.green;
                }
                else if (datos.frames[i].semaphores[j].state == 1)
                {
                    //_listaSemaforos[0].SetActive(false);
                    _listaSemaforos[j].GetComponent<Light>().color = Color.yellow;
                }
                else if (datos.frames[i].semaphores[j].state == 2)
                {
                    //_listaSemaforos[0].SetActive(true);
                    _listaSemaforos[j].GetComponent<Light>().color = Color.red;
                }
            }
            
            //Actualizar posici�n
            _listaDeCarros = datos.frames[i].cars; //Llenado de lista de carros con la informacion del JSON sobre las caracteristicas de estos

            //_listaSemaforos = datos.frames[i].semaphores; //Llenado de lista de semaforos con la informacion del JSON

            PosicionarCarrosS();

                

            //Recalcular direcci�n

            for (int j = 0; j < _direcciones.Length; j++)
            {

                if (i < datos.frames.Length - 1)
                {
                    _direcciones[j] = new Vector3(
                            datos.frames[i + 1].cars[j].x - datos.frames[i].cars[j].x,
                            datos.frames[i + 1].cars[j].y - datos.frames[i].cars[j].y,
                            datos.frames[i + 1].cars[j].z - datos.frames[i].cars[j].z  
                        );
                    //print(_direcciones[j]); //genera las direcciones correctamente
                }
                else
                {
                    _direcciones[j] = Vector3.zero;
                }
            }


            //Espera un poquito
            yield return new WaitForSeconds(0.1f);

            if (i == datos.frames.Length - 1)
            {
                i = 0;
            }
        }
    }
}
