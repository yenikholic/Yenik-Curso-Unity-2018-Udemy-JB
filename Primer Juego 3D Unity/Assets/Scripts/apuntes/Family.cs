using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Family : MonoBehaviour { 

    public Person father;
    public Person mother;
    public Person son;

    // Start is called before the first frame update
    void Start()
    {
        father = new Person();                  // instanciar un objeto por defecto vacío con el constructor por defecto
                                                // después de instanciar, podemos inicializar las variables
        
        // father.firstName = "Anakin";         // error si la variable es privada solo puede acceder la misma persona
        // father.lastName = "Skywalker";       // para solucionar esto haremos métodos SETTERS y GETTERS

        father.SetFirstName("Anakin");          // método setter de father
        father.SetLastName("Skywalker");        // método setter de father
        father.age = 35;
        father.isMale = true;
   
        mother = new Person("Padme","Amidala",28,false,father); // usando constructor de clase con todos los parámetros

        father.spouse = mother;
 
        son = new Person("Luke","Skywalker");   // usando constructor de clase sólo con nombre y apellido
        son.age = 8;
        son.isMale = true;
        son.spouse = null;

        Debug.Log(father.GetFirstName() + " y " + mother.GetFirstName() + " tienen un hijo que se llama " + son.GetFirstName());

        if (father.IsMarriedWith(mother)) Debug.Log(father.GetFirstName() + " y " + mother.GetFirstName() + " están casados");
        else Debug.Log(father.GetFirstName() + " y " + mother.GetFirstName() + " no están casados");

        // mother.SayHello();                   // mother no puede llamar a un metodo estático, lo tiene que llamar la clase del objeto
        Person.SayHello();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
   
}
