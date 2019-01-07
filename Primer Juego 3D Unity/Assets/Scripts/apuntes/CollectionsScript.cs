using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionsScript : MonoBehaviour
{
    public string student1 = "Christian";
    public string student2 = "Kate";
    public string student3 = "Mia";
    public string student4 = "Anastasia";
    
    // Todas las estructuras de datos empiezan en la posición número 0
    // El último elemento de un array es el de su dumensión -1
    
    /*
     *  ARRAY
     *  - Es homogéneo ( solo un tipo de dato )
     *  - Es de tamaño fijo ( una vez creado, no puede contener más elementos)
     *  - Tiene un orden ( se accede por posición )
     */

    public string[] students = new string[] { "Christian", "Kate", "Mia", "Anastasia" };
    //public string[] student = new string[] { student1, student2, student3, student4 }; no se pueden poner variables directamente

    public string[] familyNames = new string[4]; // { , , , }

    int[] numberOfDoorsInMyStreet = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 11 };
    private bool[] hasPassedTheExam = new bool[] { true, false, false, true };


    /*
     *  LIST
     *  - Es homogéneo ( solo un tipo de dato )
     *  - Es de tamaño ajustable ( podemos añadir y eliminar elementos en tiempo real )
     *  - Tiene un orden ( se accede por posición )
     */

    public List<string> studentNames = new List<string>();

    /*
     * ARRAYLIST
     * - Es heterogéneo ( puede contener diferentes tipos de datos )
     * - Es de tamaño ajustable ( podemos añadir y eliminar elementos en tiempo real )
     * - Tiene un orden ( se accede por posición )
    */

    public ArrayList inventory = new ArrayList();

    /*
     *  HASHTABLE o DICCIONARIO
     * - Es heterogéneo ( puede contener diferentes tipos de datos )
     * - Es de tamaño ajustable ( podemos añadir y eliminar elementos en tiempo real )
     * - Se accede por clave, no por posición No tiene un orden
    */

    public Hashtable personalDetails = new Hashtable();

    // Start is called before the first frame update
    void Start()
    {
        // lista.Add añade un elemento a una lista
        studentNames.Add("Christian");
        studentNames.Add("Kate");
        studentNames.Add("Mia");
        studentNames.Add("Anastasia");
        studentNames.Add("Jack");

        // lista.contains comprueba si en la lista hay un elemento que contiene exactamente un contenido
        if (studentNames.Contains("Jack"))
        {
            // lista.remove elimina un elemento de una lista
            studentNames.Remove("Jack");
        }

        // permite insertar contenido en una posición en una lista  (posicion , contenido)
        studentNames.Insert(2,"Pepe");

        // ToArray() convierte una lista en un array
        string[] studentNamesToArray = studentNames.ToArray();

        // Clear() elimina definitivamente todos los elementos de una lista.
        //studentNames.Clear();

        Debug.Log("Elementos de la lista");
        // sacar elementos de la lista
        foreach (string studentName in studentNames)
        {
            Debug.Log(studentName);
        }

        // acceso a la LISTAS directamente por posición y tamaño de las mismas
        // .Count para saber cuantos elementos hay en una lista
        Debug.Log("El tamaño de la lista studentNames es " + studentNamesToArray.Length);
        int pos = 0;

        if(pos >= 0 && pos < studentNames.Count)
        {
            Debug.Log("El primer estudiante de la lista es : " + studentNames[pos]);
        }

        // acceso a ARRAYS directamente por posición y tamaño de los mismos
        // .Length para saber cuantos elementos hay en un array
        Debug.Log("El tamaño del array studentNamesToArrays es " + studentNamesToArray.Length);
        pos = 2;

        if(pos >= 0 && pos < studentNamesToArray.Length)
        {
            Debug.Log("El tercer estudiante del array es : " + studentNamesToArray[pos]);
        }

        Debug.Log("Elementos del array");
        // sacar elementos de un array
        foreach (string studentName in studentNamesToArray)
        {
            Debug.Log(studentName);
        }

        inventory.Add(30); // int
        inventory.Add(3.14159265); // float
        inventory.Add("Borja"); // string
        inventory.Add(false); // booleano
        inventory.Add(GameObject.FindGameObjectsWithTag("Firework")); // array de gameobjects

        // pedimos el tipo de dato que va a salir de la arraylist
        Debug.Log(inventory[2].GetType()); // string
        Debug.Log(inventory[4].GetType()); // array de gameobjects

        personalDetails.Add("firstName", "Borja");
        personalDetails.Add("lastName", "Costa");
        personalDetails.Add("age", 28);
        personalDetails.Add("gender", "male");
        personalDetails.Add("isMarried", false);
        personalDetails.Add("hasChildren", false);

        // normalmente en archivos de json de webservice vienen datos muy variados 
        // y se usa hashtables para guardarlo

        // casting, transformación de una variable en otra
        string name = (string)personalDetails["firstName"];
        int age = (int)personalDetails["age"];

        Debug.Log(personalDetails["firstName"]);


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
