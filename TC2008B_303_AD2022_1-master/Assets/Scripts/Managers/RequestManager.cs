using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
using System;

// 3 cosas 
// 1. parsing de JSON
// 2. corrutinas 
// 3. eventos de unity (puede que quede mañana)


// si necesitamos parámetros en nuestro evento tenemos
// que declarar nuestro propio tipo
[Serializable]
public class RequestConArg : UnityEvent<ListaCarro>{}

public class RequestManager : MonoBehaviour
{
    //JSON
    [SerializeField]
    public TextAsset jsonData;

    [SerializeField]
    private RequestConArg _requestConArgumento;
    
    [SerializeField]
    private UnityEvent _requestSinArgumento;

    [SerializeField]
    private string _urlRequest = "http://127.0.0.1:5000/";

    private IEnumerator _enumeratorCorrutina;
    private Coroutine _corrutina;

    void Start(){
        /*
        string json = "{\"carros\": [" + 
        "{\"id\": 0, \"x\": 0, \"y\": 0, \"z\": 0}," +
        "{\"id\": 1, \"x\": 1, \"y\": 1, \"z\": 1}," +
        "{\"id\": 2, \"x\": 2, \"y\": 2, \"z\": 2}," +
        "{\"id\": 3, \"x\": 3, \"y\": 3, \"z\": 3}," +
        "{\"id\": 4, \"x\": 4, \"y\": 4, \"z\": 4}" + 
        "]}";

        ListaCarro carros = JsonUtility.FromJson<ListaCarro>(json);
        for(int i = 0; i < carros.carros.Length; i++){
            print(carros.carros[i].x + " , "  +
                    carros.carros[i].y + " , "  +
                    carros.carros[i].z);
        }
        */

        _enumeratorCorrutina = Request(); 

        _corrutina = StartCoroutine(_enumeratorCorrutina);

        // agregar listener programaticamente
        _requestSinArgumento.AddListener(EscuchaDummy);
    }

    void EscuchaDummy() {
        print("METODO AGREGADO A EVENTO PROGRAMATICAMENTE");
    }

    void Update() {

        if(Input.GetKeyDown(KeyCode.A)){
            StopAllCoroutines();
        }

        if(Input.GetKeyDown(KeyCode.B)){
            StopCoroutine(_enumeratorCorrutina);
        }

        if(Input.GetKeyDown(KeyCode.C)){
            StopCoroutine(_corrutina);
        }
    }

    // CORRUTINAS 
    // mecanismo de manejo de pseudo concurrencia en Unity
    // NO es un hilo PERO se comporta como uno
    IEnumerator RequestRecurrente() {

        while(true){

            // empezamos haciendo request
            UnityWebRequest www = UnityWebRequest.Get(_urlRequest);

            // como en cualquier request este asunto es asíncrono
            yield return www.SendWebRequest();

            if(www.result != UnityWebRequest.Result.Success){
                Debug.LogError("PROBLEMA EN REQUEST");
            } else {
                print(www.downloadHandler.text);

                // hacer parsing de JSON
                ListaCarro listaCarro = JsonUtility.FromJson<ListaCarro>(
                    www.downloadHandler.text
                );


                // cuando decidamos avisarle a los observadores
                // utilizamos lo siguiente:
                _requestSinArgumento?.Invoke();

                _requestConArgumento?.Invoke(listaCarro);
            }

            yield return new WaitForSeconds(1);
        }
    }
    IEnumerator RequestSimulado() {
    
        while(true){

            // hacemos solicitud para obtener string
            string json = ServerSimulado.Instance.JSON;

            // hacemos interpretación de json
            ListaCarro listaCarro = JsonUtility.FromJson<ListaCarro>(json);
            //print(listaCarro);

            // LO QUE VA A SEGUIR:
            // AVISAR A LOS DEMÁS QUE ALGO SUCEDIÓ
            // (UnityEvents)

            // esperamos para ejecutar siguiente actualización
            yield return new WaitForSeconds(1);
        }
    }

    //Ejemplo por si sólo se hace 1 request al inicio
    IEnumerator Request()
    {
        ListaCarro listaCarro = new ListaCarro(); ;
        //Request
        // empezamos haciendo request
        UnityWebRequest www = UnityWebRequest.Get(_urlRequest);

        // como en cualquier request este asunto es asíncrono
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("PROBLEMA EN REQUEST");
        }
        else
        {
            print(www.downloadHandler.text);

            // hacer parsing de JSON
            listaCarro = JsonUtility.FromJson<ListaCarro>(
                www.downloadHandler.text
            );


            // cuando decidamos avisarle a los observadores
            // utilizamos lo siguiente:
            //_requestSinArgumento?.Invoke();

            //_requestConArgumento?.Invoke(listaCarro);
        }

        //yield return new WaitForSeconds(1);

        // 1. hacer request
        // 2. parsear datos en string a json
        // 3. invocar evento de unity para avisar que ya estuvo
        // NOTA: NO HACER LOOP

        print("CORRUTINA!");

        yield return new WaitForSeconds(0.5f);

        // OJO
        // AQUÍ LO HARDCODEAMOS PARA LA DEMO EN CLASE
        // PERO ESTO DEBE SER RESULTADO DEL PARSING DEL JSON
        ListaCarro datos = new ListaCarro();

        datos = listaCarro;
        //datos = JsonUtility.FromJson<ListaCarro>(jsonData.text);

        //Datos hardcodeados
        //datos.frames = new Step[50];

        for (int i = 0; i < datos.frames.Length; i++)
        {
            //datos.frames[i] = new Step();

            //datos.frames[i].frame = i;
            //datos.frames[i].cars = new Carro[10];
            //datos.frames[i].semaphores = new Semaforo[2];

            for (int j = 0; j < datos.frames[i].cars.Length; j++)
            {
                //datos.frames[i].cars[j] = new Carro();
                //datos.frames[i].cars[j].id = j;
                //datos.frames[i].cars[j].x = (j * 3f - 6f) + (i*i *0.02f);
                //datos.frames[i].cars[j].x -= 7;
                //datos.frames[i].cars[j].y = 0;
                //datos.frames[i].cars[j].z = (i * 1f);
                //datos.frames[i].cars[j].dir = 0;
            }

            /*for (int j = 0; j < datos.frames[i].semaphores.Length; j++)
            {
                datos.frames[i].semaphores[j] = new Semaforo();
                datos.frames[i].semaphores[j].id = 0;
                datos.frames[i].semaphores[j].state = 0;
            }*/
        }

        print(JsonUtility.ToJson(datos));

        /*
        {"frame":0,"frames":[{"cars":[{"id":0,"x":-6.0,"y":0.0,"z":0.0,"dir":0.0},{"id":1,"x":-3.0,"y":0.0,"z":0.0,"dir":0.0},{"id":2,"x":0.0,"y":0.0,"z":0.0,"dir":0.0},{"id":3,"x":3.0,"y":0.0,"z":0.0,"dir":0.0},{"id":4,"x":6.0,"y":0.0,"z":0.0,"dir":0.0},{"id":5,"x":9.0,"y":0.0,"z":0.0,"dir":0.0},{"id":6,"x":12.0,"y":0.0,"z":0.0,"dir":0.0},{"id":7,"x":15.0,"y":0.0,"z":0.0,"dir":0.0},{"id":8,"x":18.0,"y":0.0,"z":0.0,"dir":0.0},{"id":9,"x":21.0,"y":0.0,"z":0.0,"dir":0.0}],"semaphores":[{"id":0,"state":0},{"id":0,"state":0}]},{"cars":[{"id":0,"x":-5.980000019073486,"y":0.0,"z":1.0,"dir":0.0},{"id":1,"x":-2.9800000190734865,"y":0.0,"z":1.0,"dir":0.0},{"id":2,"x":0.019999999552965165,"y":0.0,"z":1.0,"dir":0.0},{"id":3,"x":3.0199999809265138,"y":0.0,"z":1.0,"dir":0.0},{"id":4,"x":6.019999980926514,"y":0.0,"z":1.0,"dir":0.0},{"id":5,"x":9.020000457763672,"y":0.0,"z":1.0,"dir":0.0},{"id":6,"x":12.020000457763672,"y":0.0,"z":1.0,"dir":0.0},{"id":7,"x":15.020000457763672,"y":0.0,"z":1.0,"dir":0.0},{"id":8,"x":18.020000457763673,"y":0.0,"z":1.0,"dir":0.0},{"id":9,"x":21.020000457763673,"y":0.0,"z":1.0,"dir":0.0}],"semaphores":[{"id":0,"state":0},{"id":0,"state":0}]},{"cars":[{"id":0,"x":-5.920000076293945,"y":0.0,"z":2.0,"dir":0.0},{"id":1,"x":-2.9200000762939455,"y":0.0,"z":2.0,"dir":0.0},{"id":2,"x":0.07999999821186066,"y":0.0,"z":2.0,"dir":0.0},{"id":3,"x":3.0799999237060549,"y":0.0,"z":2.0,"dir":0.0},{"id":4,"x":6.079999923706055,"y":0.0,"z":2.0,"dir":0.0},{"id":5,"x":9.079999923706055,"y":0.0,"z":2.0,"dir":0.0},{"id":6,"x":12.079999923706055,"y":0.0,"z":2.0,"dir":0.0},{"id":7,"x":15.079999923706055,"y":0.0,"z":2.0,"dir":0.0},{"id":8,"x":18.079999923706056,"y":0.0,"z":2.0,"dir":0.0},{"id":9,"x":21.079999923706056,"y":0.0,"z":2.0,"dir":0.0}],"semaphores":[{"id":0,"state":0},{"id":0,"state":0}]},{"cars":[{"id":0,"x":-5.820000171661377,"y":0.0,"z":3.0,"dir":0.0},{"id":1,"x":-2.819999933242798,"y":0.0,"z":3.0,"dir":0.0},{"id":2,"x":0.17999999225139619,"y":0.0,"z":3.0,"dir":0.0},{"id":3,"x":3.180000066757202,"y":0.0,"z":3.0,"dir":0.0},{"id":4,"x":6.179999828338623,"y":0.0,"z":3.0,"dir":0.0},{"id":5,"x":9.180000305175782,"y":0.0,"z":3.0,"dir":0.0},{"id":6,"x":12.180000305175782,"y":0.0,"z":3.0,"dir":0.0},{"id":7,"x":15.180000305175782,"y":0.0,"z":3.0,"dir":0.0},{"id":8,"x":18.18000030517578,"y":0.0,"z":3.0,"dir":0.0},{"id":9,"x":21.18000030517578,"y":0.0,"z":3.0,"dir":0.0}],"semaphores":[{"id":0,"state":0},{"id":0,"state":0}]},{"cars":[{"id":0,"x":-5.679999828338623,"y":0.0,"z":4.0,"dir":0.0},{"id":1,"x":-2.680000066757202,"y":0.0,"z":4.0,"dir":0.0},{"id":2,"x":0.3199999928474426,"y":0.0,"z":4.0,"dir":0.0},{"id":3,"x":3.319999933242798,"y":0.0,"z":4.0,"dir":0.0},{"id":4,"x":6.320000171661377,"y":0.0,"z":4.0,"dir":0.0},{"id":5,"x":9.319999694824219,"y":0.0,"z":4.0,"dir":0.0},{"id":6,"x":12.319999694824219,"y":0.0,"z":4.0,"dir":0.0},{"id":7,"x":15.319999694824219,"y":0.0,"z":4.0,"dir":0.0},{"id":8,"x":18.31999969482422,"y":0.0,"z":4.0,"dir":0.0},{"id":9,"x":21.31999969482422,"y":0.0,"z":4.0,"dir":0.0}],"semaphores":[{"id":0,"state":0},{"id":0,"state":0}]},{"cars":[{"id":0,"x":-5.5,"y":0.0,"z":5.0,"dir":0.0},{"id":1,"x":-2.5,"y":0.0,"z":5.0,"dir":0.0},{"id":2,"x":0.5,"y":0.0,"z":5.0,"dir":0.0},{"id":3,"x":3.5,"y":0.0,"z":5.0,"dir":0.0},{"id":4,"x":6.5,"y":0.0,"z":5.0,"dir":0.0},{"id":5,"x":9.5,"y":0.0,"z":5.0,"dir":0.0},{"id":6,"x":12.5,"y":0.0,"z":5.0,"dir":0.0},{"id":7,"x":15.5,"y":0.0,"z":5.0,"dir":0.0},{"id":8,"x":18.5,"y":0.0,"z":5.0,"dir":0.0},{"id":9,"x":21.5,"y":0.0,"z":5.0,"dir":0.0}],"semaphores":[{"id":0,"state":0},{"id":0,"state":0}]},{"cars":[{"id":0,"x":-5.28000020980835,"y":0.0,"z":6.0,"dir":0.0},{"id":1,"x":-2.2799999713897707,"y":0.0,"z":6.0,"dir":0.0},{"id":2,"x":0.7199999690055847,"y":0.0,"z":6.0,"dir":0.0},{"id":3,"x":3.7200000286102297,"y":0.0,"z":6.0,"dir":0.0},{"id":4,"x":6.71999979019165,"y":0.0,"z":6.0,"dir":0.0},{"id":5,"x":9.720000267028809,"y":0.0,"z":6.0,"dir":0.0},{"id":6,"x":12.720000267028809,"y":0.0,"z":6.0,"dir":0.0},{"id":7,"x":15.720000267028809,"y":0.0,"z":6.0,"dir":0.0},{"id":8,"x":18.719999313354493,"y":0.0,"z":6.0,"dir":0.0},{"id":9,"x":21.719999313354493,"y":0.0,"z":6.0,"dir":0.0}],"semaphores":[{"id":0,"state":0},{"id":0,"state":0}]},{"cars":[{"id":0,"x":-5.019999980926514,"y":0.0,"z":7.0,"dir":0.0},{"id":1,"x":-2.0199999809265138,"y":0.0,"z":7.0,"dir":0.0},{"id":2,"x":0.9799999594688416,"y":0.0,"z":7.0,"dir":0.0},{"id":3,"x":3.9800000190734865,"y":0.0,"z":7.0,"dir":0.0},{"id":4,"x":6.980000019073486,"y":0.0,"z":7.0,"dir":0.0},{"id":5,"x":9.979999542236329,"y":0.0,"z":7.0,"dir":0.0},{"id":6,"x":12.979999542236329,"y":0.0,"z":7.0,"dir":0.0},{"id":7,"x":15.979999542236329,"y":0.0,"z":7.0,"dir":0.0},{"id":8,"x":18.979999542236329,"y":0.0,"z":7.0,"dir":0.0},{"id":9,"x":21.979999542236329,"y":0.0,"z":7.0,"dir":0.0}],"semaphores":[{"id":0,"state":0},{"id":0,"state":0}]},{"cars":[{"id":0,"x":-4.720000267028809,"y":0.0,"z":8.0,"dir":0.0},{"id":1,"x":-1.7200000286102296,"y":0.0,"z":8.0,"dir":0.0},{"id":2,"x":1.2799999713897706,"y":0.0,"z":8.0,"dir":0.0},{"id":3,"x":4.279999732971191,"y":0.0,"z":8.0,"dir":0.0},{"id":4,"x":7.279999732971191,"y":0.0,"z":8.0,"dir":0.0},{"id":5,"x":10.279999732971192,"y":0.0,"z":8.0,"dir":0.0},{"id":6,"x":13.279999732971192,"y":0.0,"z":8.0,"dir":0.0},{"id":7,"x":16.280000686645509,"y":0.0,"z":8.0,"dir":0.0},{"id":8,"x":19.280000686645509,"y":0.0,"z":8.0,"dir":0.0},{"id":9,"x":22.280000686645509,"y":0.0,"z":8.0,"dir":0.0}],"semaphores":[{"id":0,"state":0},{"id":0,"state":0}]},{"cars":[{"id":0,"x":-4.380000114440918,"y":0.0,"z":9.0,"dir":0.0},{"id":1,"x":-1.3799999952316285,"y":0.0,"z":9.0,"dir":0.0},{"id":2,"x":1.6200000047683716,"y":0.0,"z":9.0,"dir":0.0},{"id":3,"x":4.619999885559082,"y":0.0,"z":9.0,"dir":0.0},{"id":4,"x":7.619999885559082,"y":0.0,"z":9.0,"dir":0.0},{"id":5,"x":10.619999885559082,"y":0.0,"z":9.0,"dir":0.0},{"id":6,"x":13.619999885559082,"y":0.0,"z":9.0,"dir":0.0},{"id":7,"x":16.6200008392334,"y":0.0,"z":9.0,"dir":0.0},{"id":8,"x":19.6200008392334,"y":0.0,"z":9.0,"dir":0.0},{"id":9,"x":22.6200008392334,"y":0.0,"z":9.0,"dir":0.0}],"semaphores":[{"id":0,"state":0},{"id":0,"state":0}]},{"cars":[{"id":0,"x":-4.0,"y":0.0,"z":10.0,"dir":0.0},{"id":1,"x":-1.0,"y":0.0,"z":10.0,"dir":0.0},{"id":2,"x":2.0,"y":0.0,"z":10.0,"dir":0.0},{"id":3,"x":5.0,"y":0.0,"z":10.0,"dir":0.0},{"id":4,"x":8.0,"y":0.0,"z":10.0,"dir":0.0},{"id":5,"x":11.0,"y":0.0,"z":10.0,"dir":0.0},{"id":6,"x":14.0,"y":0.0,"z":10.0,"dir":0.0},{"id":7,"x":17.0,"y":0.0,"z":10.0,"dir":0.0},{"id":8,"x":20.0,"y":0.0,"z":10.0,"dir":0.0},{"id":9,"x":23.0,"y":0.0,"z":10.0,"dir":0.0}],"semaphores":[{"id":0,"state":0},{"id":0,"state":0}]},{"cars":[{"id":0,"x":-3.580000162124634,"y":0.0,"z":11.0,"dir":0.0},{"id":1,"x":-0.5800000429153442,"y":0.0,"z":11.0,"dir":0.0},{"id":2,"x":2.419999837875366,"y":0.0,"z":11.0,"dir":0.0},{"id":3,"x":5.420000076293945,"y":0.0,"z":11.0,"dir":0.0},{"id":4,"x":8.420000076293946,"y":0.0,"z":11.0,"dir":0.0},{"id":5,"x":11.420000076293946,"y":0.0,"z":11.0,"dir":0.0},{"id":6,"x":14.420000076293946,"y":0.0,"z":11.0,"dir":0.0},{"id":7,"x":17.420000076293947,"y":0.0,"z":11.0,"dir":0.0},{"id":8,"x":20.420000076293947,"y":0.0,"z":11.0,"dir":0.0},{"id":9,"x":23.420000076293947,"y":0.0,"z":11.0,"dir":0.0}],"semaphores":[{"id":0,"state":0},{"id":0,"state":0}]},{"cars":[{"id":0,"x":-3.120000123977661,"y":0.0,"z":12.0,"dir":0.0},{"id":1,"x":-0.12000006437301636,"y":0.0,"z":12.0,"dir":0.0},{"id":2,"x":2.879999876022339,"y":0.0,"z":12.0,"dir":0.0},{"id":3,"x":5.880000114440918,"y":0.0,"z":12.0,"dir":0.0},{"id":4,"x":8.880000114440918,"y":0.0,"z":12.0,"dir":0.0},{"id":5,"x":11.880000114440918,"y":0.0,"z":12.0,"dir":0.0},{"id":6,"x":14.880000114440918,"y":0.0,"z":12.0,"dir":0.0},{"id":7,"x":17.8799991607666,"y":0.0,"z":12.0,"dir":0.0},{"id":8,"x":20.8799991607666,"y":0.0,"z":12.0,"dir":0.0},{"id":9,"x":23.8799991607666,"y":0.0,"z":12.0,"dir":0.0}],"semaphores":[{"id":0,"state":0},{"id":0,"state":0}]},{"cars":[{"id":0,"x":-2.620000123977661,"y":0.0,"z":13.0,"dir":0.0},{"id":1,"x":0.37999993562698367,"y":0.0,"z":13.0,"dir":0.0},{"id":2,"x":3.379999876022339,"y":0.0,"z":13.0,"dir":0.0},{"id":3,"x":6.380000114440918,"y":0.0,"z":13.0,"dir":0.0},{"id":4,"x":9.380000114440918,"y":0.0,"z":13.0,"dir":0.0},{"id":5,"x":12.380000114440918,"y":0.0,"z":13.0,"dir":0.0},{"id":6,"x":15.380000114440918,"y":0.0,"z":13.0,"dir":0.0},{"id":7,"x":18.3799991607666,"y":0.0,"z":13.0,"dir":0.0},{"id":8,"x":21.3799991607666,"y":0.0,"z":13.0,"dir":0.0},{"id":9,"x":24.3799991607666,"y":0.0,"z":13.0,"dir":0.0}],"semaphores":[{"id":0,"state":0},{"id":0,"state":0}]},{"cars":[{"id":0,"x":-2.080000162124634,"y":0.0,"z":14.0,"dir":0.0},{"id":1,"x":0.919999897480011,"y":0.0,"z":14.0,"dir":0.0},{"id":2,"x":3.919999837875366,"y":0.0,"z":14.0,"dir":0.0},{"id":3,"x":6.920000076293945,"y":0.0,"z":14.0,"dir":0.0},{"id":4,"x":9.920000076293946,"y":0.0,"z":14.0,"dir":0.0},{"id":5,"x":12.920000076293946,"y":0.0,"z":14.0,"dir":0.0},{"id":6,"x":15.920000076293946,"y":0.0,"z":14.0,"dir":0.0},{"id":7,"x":18.920000076293947,"y":0.0,"z":14.0,"dir":0.0},{"id":8,"x":21.920000076293947,"y":0.0,"z":14.0,"dir":0.0},{"id":9,"x":24.920000076293947,"y":0.0,"z":14.0,"dir":0.0}],"semaphores":[{"id":0,"state":0},{"id":0,"state":0}]},{"cars":[{"id":0,"x":-1.5000001192092896,"y":0.0,"z":15.0,"dir":0.0},{"id":1,"x":1.4999998807907105,"y":0.0,"z":15.0,"dir":0.0},{"id":2,"x":4.5,"y":0.0,"z":15.0,"dir":0.0},{"id":3,"x":7.5,"y":0.0,"z":15.0,"dir":0.0},{"id":4,"x":10.5,"y":0.0,"z":15.0,"dir":0.0},{"id":5,"x":13.5,"y":0.0,"z":15.0,"dir":0.0},{"id":6,"x":16.5,"y":0.0,"z":15.0,"dir":0.0},{"id":7,"x":19.5,"y":0.0,"z":15.0,"dir":0.0},{"id":8,"x":22.5,"y":0.0,"z":15.0,"dir":0.0},{"id":9,"x":25.5,"y":0.0,"z":15.0,"dir":0.0}],"semaphores":[{"id":0,"state":0},{"id":0,"state":0}]},{"cars":[{"id":0,"x":-0.880000114440918,"y":0.0,"z":16.0,"dir":0.0},{"id":1,"x":2.119999885559082,"y":0.0,"z":16.0,"dir":0.0},{"id":2,"x":5.119999885559082,"y":0.0,"z":16.0,"dir":0.0},{"id":3,"x":8.119999885559082,"y":0.0,"z":16.0,"dir":0.0},{"id":4,"x":11.119999885559082,"y":0.0,"z":16.0,"dir":0.0},{"id":5,"x":14.119999885559082,"y":0.0,"z":16.0,"dir":0.0},{"id":6,"x":17.119998931884767,"y":0.0,"z":16.0,"dir":0.0},{"id":7,"x":20.119998931884767,"y":0.0,"z":16.0,"dir":0.0},{"id":8,"x":23.119998931884767,"y":0.0,"z":16.0,"dir":0.0},{"id":9,"x":26.119998931884767,"y":0.0,"z":16.0,"dir":0.0}],"semaphores":[{"id":0,"state":0},{"id":0,"state":0}]},{"cars":[{"id":0,"x":-0.22000013291835786,"y":0.0,"z":17.0,"dir":0.0},{"id":1,"x":2.7799999713897707,"y":0.0,"z":17.0,"dir":0.0},{"id":2,"x":5.779999732971191,"y":0.0,"z":17.0,"dir":0.0},{"id":3,"x":8.779999732971192,"y":0.0,"z":17.0,"dir":0.0},{"id":4,"x":11.779999732971192,"y":0.0,"z":17.0,"dir":0.0},{"id":5,"x":14.779999732971192,"y":0.0,"z":17.0,"dir":0.0},{"id":6,"x":17.780000686645509,"y":0.0,"z":17.0,"dir":0.0},{"id":7,"x":20.780000686645509,"y":0.0,"z":17.0,"dir":0.0},{"id":8,"x":23.780000686645509,"y":0.0,"z":17.0,"dir":0.0},{"id":9,"x":26.780000686645509,"y":0.0,"z":17.0,"dir":0.0}],"semaphores":[{"id":0,"state":0},{"id":0,"state":0}]},{"cars":[{"id":0,"x":0.479999840259552,"y":0.0,"z":18.0,"dir":0.0},{"id":1,"x":3.4799997806549074,"y":0.0,"z":18.0,"dir":0.0},{"id":2,"x":6.480000019073486,"y":0.0,"z":18.0,"dir":0.0},{"id":3,"x":9.479999542236329,"y":0.0,"z":18.0,"dir":0.0},{"id":4,"x":12.479999542236329,"y":0.0,"z":18.0,"dir":0.0},{"id":5,"x":15.479999542236329,"y":0.0,"z":18.0,"dir":0.0},{"id":6,"x":18.479999542236329,"y":0.0,"z":18.0,"dir":0.0},{"id":7,"x":21.479999542236329,"y":0.0,"z":18.0,"dir":0.0},{"id":8,"x":24.479999542236329,"y":0.0,"z":18.0,"dir":0.0},{"id":9,"x":27.479999542236329,"y":0.0,"z":18.0,"dir":0.0}],"semaphores":[{"id":0,"state":0},{"id":0,"state":0}]},{"cars":[{"id":0,"x":1.2199997901916505,"y":0.0,"z":19.0,"dir":0.0},{"id":1,"x":4.21999979019165,"y":0.0,"z":19.0,"dir":0.0},{"id":2,"x":7.21999979019165,"y":0.0,"z":19.0,"dir":0.0},{"id":3,"x":10.220000267028809,"y":0.0,"z":19.0,"dir":0.0},{"id":4,"x":13.220000267028809,"y":0.0,"z":19.0,"dir":0.0},{"id":5,"x":16.219999313354493,"y":0.0,"z":19.0,"dir":0.0},{"id":6,"x":19.219999313354493,"y":0.0,"z":19.0,"dir":0.0},{"id":7,"x":22.219999313354493,"y":0.0,"z":19.0,"dir":0.0},{"id":8,"x":25.219999313354493,"y":0.0,"z":19.0,"dir":0.0},{"id":9,"x":28.219999313354493,"y":0.0,"z":19.0,"dir":0.0}],"semaphores":[{"id":0,"state":0},{"id":0,"state":0}]},{"cars":[{"id":0,"x":1.999999761581421,"y":0.0,"z":20.0,"dir":0.0},{"id":1,"x":5.0,"y":0.0,"z":20.0,"dir":0.0},{"id":2,"x":8.0,"y":0.0,"z":20.0,"dir":0.0},{"id":3,"x":11.0,"y":0.0,"z":20.0,"dir":0.0},{"id":4,"x":14.0,"y":0.0,"z":20.0,"dir":0.0},{"id":5,"x":17.0,"y":0.0,"z":20.0,"dir":0.0},{"id":6,"x":20.0,"y":0.0,"z":20.0,"dir":0.0},{"id":7,"x":23.0,"y":0.0,"z":20.0,"dir":0.0},{"id":8,"x":26.0,"y":0.0,"z":20.0,"dir":0.0},{"id":9,"x":29.0,"y":0.0,"z":20.0,"dir":0.0}],"semaphores":[{"id":0,"state":0},{"id":0,"state":0}]},{"cars":[{"id":0,"x":2.8199996948242189,"y":0.0,"z":21.0,"dir":0.0},{"id":1,"x":5.819999694824219,"y":0.0,"z":21.0,"dir":0.0},{"id":2,"x":8.819999694824219,"y":0.0,"z":21.0,"dir":0.0},{"id":3,"x":11.819999694824219,"y":0.0,"z":21.0,"dir":0.0},{"id":4,"x":14.819999694824219,"y":0.0,"z":21.0,"dir":0.0},{"id":5,"x":17.81999969482422,"y":0.0,"z":21.0,"dir":0.0},{"id":6,"x":20.81999969482422,"y":0.0,"z":21.0,"dir":0.0},{"id":7,"x":23.81999969482422,"y":0.0,"z":21.0,"dir":0.0},{"id":8,"x":26.81999969482422,"y":0.0,"z":21.0,"dir":0.0},{"id":9,"x":29.81999969482422,"y":0.0,"z":21.0,"dir":0.0}],"semaphores":[{"id":0,"state":0},{"id":0,"state":0}]},{"cars":[{"id":0,"x":3.679999828338623,"y":0.0,"z":22.0,"dir":0.0},{"id":1,"x":6.679999828338623,"y":0.0,"z":22.0,"dir":0.0},{"id":2,"x":9.679999351501465,"y":0.0,"z":22.0,"dir":0.0},{"id":3,"x":12.679999351501465,"y":0.0,"z":22.0,"dir":0.0},{"id":4,"x":15.679999351501465,"y":0.0,"z":22.0,"dir":0.0},{"id":5,"x":18.68000030517578,"y":0.0,"z":22.0,"dir":0.0},{"id":6,"x":21.68000030517578,"y":0.0,"z":22.0,"dir":0.0},{"id":7,"x":24.68000030517578,"y":0.0,"z":22.0,"dir":0.0},{"id":8,"x":27.68000030517578,"y":0.0,"z":22.0,"dir":0.0},{"id":9,"x":30.68000030517578,"y":0.0,"z":22.0,"dir":0.0}],"semaphores":[{"id":0,"state":0},{"id":0,"state":0}]},{"cars":[{"id":0,"x":4.579999923706055,"y":0.0,"z":23.0,"dir":0.0},{"id":1,"x":7.579999923706055,"y":0.0,"z":23.0,"dir":0.0},{"id":2,"x":10.579999923706055,"y":0.0,"z":23.0,"dir":0.0},{"id":3,"x":13.579999923706055,"y":0.0,"z":23.0,"dir":0.0},{"id":4,"x":16.579999923706056,"y":0.0,"z":23.0,"dir":0.0},{"id":5,"x":19.579999923706056,"y":0.0,"z":23.0,"dir":0.0},{"id":6,"x":22.579999923706056,"y":0.0,"z":23.0,"dir":0.0},{"id":7,"x":25.579999923706056,"y":0.0,"z":23.0,"dir":0.0},{"id":8,"x":28.579999923706056,"y":0.0,"z":23.0,"dir":0.0},{"id":9,"x":31.579999923706056,"y":0.0,"z":23.0,"dir":0.0}],"semaphores":[{"id":0,"state":0},{"id":0,"state":0}]},{"cars":[{"id":0,"x":5.5199995040893559,"y":0.0,"z":24.0,"dir":0.0},{"id":1,"x":8.519999504089356,"y":0.0,"z":24.0,"dir":0.0},{"id":2,"x":11.519999504089356,"y":0.0,"z":24.0,"dir":0.0},{"id":3,"x":14.519999504089356,"y":0.0,"z":24.0,"dir":0.0},{"id":4,"x":17.520000457763673,"y":0.0,"z":24.0,"dir":0.0},{"id":5,"x":20.520000457763673,"y":0.0,"z":24.0,"dir":0.0},{"id":6,"x":23.520000457763673,"y":0.0,"z":24.0,"dir":0.0},{"id":7,"x":26.520000457763673,"y":0.0,"z":24.0,"dir":0.0},{"id":8,"x":29.520000457763673,"y":0.0,"z":24.0,"dir":0.0},{"id":9,"x":32.52000045776367,"y":0.0,"z":24.0,"dir":0.0}],"semaphores":[{"id":0,"state":0},{"id":0,"state":0}]},{"cars":[{"id":0,"x":6.499999523162842,"y":0.0,"z":25.0,"dir":0.0},{"id":1,"x":9.5,"y":0.0,"z":25.0,"dir":0.0},{"id":2,"x":12.5,"y":0.0,"z":25.0,"dir":0.0},{"id":3,"x":15.5,"y":0.0,"z":25.0,"dir":0.0},{"id":4,"x":18.5,"y":0.0,"z":25.0,"dir":0.0},{"id":5,"x":21.5,"y":0.0,"z":25.0,"dir":0.0},{"id":6,"x":24.5,"y":0.0,"z":25.0,"dir":0.0},{"id":7,"x":27.5,"y":0.0,"z":25.0,"dir":0.0},{"id":8,"x":30.5,"y":0.0,"z":25.0,"dir":0.0},{"id":9,"x":33.5,"y":0.0,"z":25.0,"dir":0.0}],"semaphores":[{"id":0,"state":0},{"id":0,"state":0}]},{"cars":[{"id":0,"x":7.5199995040893559,"y":0.0,"z":26.0,"dir":0.0},{"id":1,"x":10.519999504089356,"y":0.0,"z":26.0,"dir":0.0},{"id":2,"x":13.519999504089356,"y":0.0,"z":26.0,"dir":0.0},{"id":3,"x":16.520000457763673,"y":0.0,"z":26.0,"dir":0.0}
        */

        _requestConArgumento?.Invoke(datos);
    }
}