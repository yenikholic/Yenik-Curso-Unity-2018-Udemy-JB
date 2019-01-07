using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// para que funcione la instanciación de Person ( new Person(); ) en la clase Family, 
// Person no es un MonoBehaviour, es un objeto no un comportamiento, 
// hay que quitarle la herencia de MonoBehaviour
public class Person
{
    // si ponemos variables privadas solo podrá cambiar el contenido la propia persona
    // no se podrá cambair desde ningún otro trozo de código
    private string firstName;
    private string lastName;
    public int age;
    public bool isMale;
    public Person spouse;

    /* CONSTRUCTORES Y SOBRECARGAS */

    public Person() // constructor por defecto sin sobrecarga
    {

    }
    
    // sobrecarga del constructor con 2 parámetros 
    public Person(string pFirstName, string pLastName) 
    {
        firstName = pFirstName;
        lastName = pLastName;
    }

    // sobrecarga del constructor con 5 parámetros
    public Person(string pFirstName, string pLastName, int pAge, bool isMale, Person spouse)
    {
        firstName = pFirstName;
        lastName = pLastName;
        age = pAge;
        this.isMale = isMale;   // se puede poner el mismo nombre especificando la variable con this. !! Es más claro usarlo así. !!
        this.spouse = spouse;
    }

    public bool IsMarriedWith(Person otherPerson)
    {
        Debug.Log(firstName);
        if (spouse == null)
        {
            Debug.Log("No está casado");
            return false;       // aquí no está casado
        }
        else
        {
            Debug.Log("Está casado");
            if (otherPerson.firstName == spouse.firstName && 
                otherPerson.lastName == spouse.lastName)
            {
                Debug.Log(" con " + otherPerson.firstName);
                return true;    // aquí está casado con otherPerson
            }
            else
            {
                Debug.Log(" pero no con " + otherPerson.firstName);
                return false;   // aquí está casado pero no con otherPerson
            }
        }
            
    }

    // método estático, que no se llama mediante la instancia de la persona, sino desde la clase directamente
    public static void SayHello()
    {
        Debug.Log("Hola, que tal");
    }

    /* SETTERS y GETTERS */
    public void SetFirstName(string firstName)
    {
        this.firstName = firstName;
    }
    public void SetLastName(string lastName)
    {
        this.lastName = lastName;
    }
    public string GetFirstName()
    {
        return this.firstName;
    }
    public string GetLastName()
    {
        return this.lastName;
    }
}
